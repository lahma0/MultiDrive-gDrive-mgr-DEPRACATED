Imports System.Security.Cryptography
Imports Google.Apis.Drive.v2
Imports DotNetOpenAuth.OAuth2
Imports Google.Apis.Authentication.OAuth2
Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth
Imports Google.Apis.Drive.v2.Data
Imports Google.Apis.Util
Imports System.Net.Mail
Imports System.ComponentModel
Imports Google.Apis.Oauth2.v2
Imports Google.Apis.Oauth2.v2.Data
Imports System.Text


Public Class GoogleUser
    Private _mEmail As String
    Private _mMail As MailAddress
    Private _mLoggedIn As Boolean
    Private _mApiKey As String
    Private _mClientId As String
    Private _mClientSecret As String
    Private ReadOnly _scope() As String = {DriveService.Scopes.Drive.GetStringValue, "https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/userinfo.profile"}
    Private _mProvider As NativeApplicationClient
    Private _mAuthenticator As OAuth2Authenticator(Of NativeApplicationClient)
    Private _mAuthorizationState As IAuthorizationState
    Private _mDriveService As DriveService
    Private _bwAuth As BackgroundWorker
    Private _mQuotaTotal As Long
    Private _mQuotaUsed As Long
    Private _mQuotaUsedGlobally As Long
    Private _mQuotaTrash As Long
    Private _mName As String
    Private _mDefaultLocale As String
    Private _mPictureLink As String
    Private _mProfileLink As String
    Private _mUserInfoService As Oauth2Service
    Private _mUserInformation As Userinfo
    Private _mTokenFileName As String

    Public Property TokenFileName As String
        Get
            Return _mTokenFileName
        End Get
        Set(value As String)
            _mTokenFileName = value
        End Set
    End Property


    Public ReadOnly Property UserInformation As Userinfo
        Get
            Return _mUserInformation
        End Get
    End Property

    Public ReadOnly Property UserInfoService As Oauth2Service
        Get
            Return _mUserInfoService
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            Return _mName
        End Get
    End Property
    Public ReadOnly Property DefaultLocale As String
        Get
            Return _mDefaultLocale
        End Get
    End Property
    Public ReadOnly Property PictureLink As String
        Get
            Return _mPictureLink
        End Get
    End Property
    Public ReadOnly Property ProfileLink As String
        Get
            Return _mProfileLink
        End Get
    End Property

    Public ReadOnly Property QuotaTrash As Long
        Get
            Return _mQuotaTrash
        End Get
    End Property

    Public ReadOnly Property QuotaUsedGlobally As Long
        Get
            Return _mQuotaUsedGlobally
        End Get
    End Property

    Public ReadOnly Property QuotaFree As Long
        Get
            Return _mQuotaTotal - _mQuotaUsed
        End Get
    End Property
    Public ReadOnly Property QuotaPercentUsed As Integer
        Get
            If _mQuotaTotal = 0 Then
                Return 0
            Else
                Return Convert.ToInt32((_mQuotaUsed / _mQuotaTotal) * 100)
            End If

        End Get
    End Property

    Public ReadOnly Property QuotaUsed As Long
        Get
            Return _mQuotaUsed
        End Get
    End Property

    Public ReadOnly Property QuotaTotal As Long
        Get
            Return _mQuotaTotal
        End Get
    End Property
    Public ReadOnly Property Provider() As NativeApplicationClient
        Get
            Return _mProvider
        End Get
    End Property
    Public ReadOnly Property Authenticator() As OAuth2Authenticator(Of NativeApplicationClient)
        Get
            If Not _mAuthenticator Is Nothing Then
                Return _mAuthenticator
            Else
                Throw New Exception("Cannot get OAuth2 Authenticator because " & Me.Email & " is not logged in.")
            End If
        End Get
    End Property
    Public ReadOnly Property AuthorizationState() As IAuthorizationState
        Get
            Return _mAuthorizationState
        End Get
    End Property
    Public ReadOnly Property DriveService() As DriveService
        Get
            Return _mDriveService
        End Get
    End Property

    Public Property Email() As String
        Get
            Return _mEmail
        End Get
        Set(value As String)
            _mEmail = value
            _mMail = New MailAddress(value)
        End Set
    End Property
    Public ReadOnly Property Mail() As MailAddress
        Get
            Return _mMail
        End Get
    End Property
    Public Property ApiKey() As String
        Get
            Return _mApiKey
        End Get
        Set(value As String)
            _mApiKey = value
        End Set
    End Property
    Public Property ClientId() As String
        Get
            Return _mClientId
        End Get
        Set(value As String)
            _mClientId = value
        End Set
    End Property
    Public Property ClientSecret() As String
        Get
            Return CredentialsManager.Unprotect(_mClientSecret)
        End Get
        Set(value As String)
            _mClientSecret = value
        End Set
    End Property
    Public Property LoggedIn() As Boolean
        Get
            Return _mLoggedIn
        End Get
        Set(value As Boolean)
            _mLoggedIn = value
        End Set
    End Property

    Public Sub New()
        Dim apiData As ApiFile = ApiFile.EnsureApiFile()
        If Not apiData Is Nothing Then
            _mApiKey = apiData.ApiKey
            _mClientId = apiData.ClientId
            _mClientSecret = apiData.EncodedClientSecret
        End If
    End Sub

    Private Function GetAuthorization(client As NativeApplicationClient) As IAuthorizationState
        ' Check if there is a cached refresh token available.
        Dim state As IAuthorizationState
        If _mTokenFileName IsNot Nothing Then
            state = AuthorizationMgr.GetCachedRefreshToken(_mTokenFileName, ClientId, ClientSecret)
            If state IsNot Nothing Then
                Try
                    client.RefreshToken(state)
                    ' Yes - we are done.
                    Return state
                Catch ex As DotNetOpenAuth.Messaging.ProtocolException
                    MessageBox.Show("Using existing refresh token failed: " + ex.Message)
                End Try
            End If
        End If
        ' Retrieve the authorization from the user.
        'If we want to avoid all the complicated junk in the authorization flow, you can just get a state the old way here... use "InitiateLogin"
        state = AuthorizationMgr.RequestNativeAuthorization(client, _scope)
        _mTokenFileName = AuthorizationMgr.SetCachedRefreshToken(ClientId, ClientSecret, state)
        Return state
    End Function
    Public Sub InitiateLogin()
        If Not _bwAuth Is Nothing Then
            If _bwAuth.IsBusy Then
                Exit Sub
            End If
        End If
        If Me.LoggedIn = False Then
            _bwAuth = New BackgroundWorker
            AddHandler _bwAuth.DoWork, AddressOf authEventStart
            AddHandler _bwAuth.RunWorkerCompleted, AddressOf authEventFinished
            _bwAuth.RunWorkerAsync()
        End If
    End Sub
    Public Sub GetUserInfo()
        Try
            _mUserInfoService = New Google.Apis.Oauth2.v2.Oauth2Service(_mAuthenticator)
            _mUserInformation = _mUserInfoService.Userinfo.Get.Fetch()
            Email = _mUserInformation.Email
            _mDefaultLocale = _mUserInformation.Locale
            _mName = _mUserInformation.Name
            _mPictureLink = _mUserInformation.Picture
            _mProfileLink = _mUserInformation.Link
            Dim b As Bitmap
            If Not MainForm.docIconsXL.Images.ContainsKey(_mEmail) Then
                If Not _mPictureLink Is Nothing Then
                    b = MainForm.GetImage(_mPictureLink)
                End If
                If b IsNot Nothing Then
                    MainForm.docIconsXL.Images.Add(_mEmail, b)
                End If
            End If
            If Not MainForm.docIconsXL.Images.ContainsKey(_mEmail) Then
                If b Is Nothing Then
                    If Not _mPictureLink Is Nothing Then
                        b = MainForm.GetImage(_mPictureLink)
                    End If
                End If
                If Not b Is Nothing Then
                    MainForm.docIconsXL.Images.Add(_mEmail, b)
                End If
            End If
        Catch ex As Exception
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting the retrieve user info."))
        Exit Sub
        End Try
    End Sub
    Private Sub AuthEventStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            _mProvider = New NativeApplicationClient(GoogleAuthenticationServer.Description, ClientId, ClientSecret)
            _mAuthorizationState = GetAuthorization(_mProvider)
            _mAuthenticator = New OAuth2Authenticator(Of NativeApplicationClient)(Provider, Function(p) _mAuthorizationState)
            _mDriveService = New DriveService(_mAuthenticator)
        Catch exB As Google.GoogleApiRequestException
            _mLoggedIn = False
            MessageBox.Show(Email + " failed to login! Error: " + exB.Message)
        Catch ex As Google.GoogleApiException
            _mLoggedIn = False
            MessageBox.Show(Email + " failed to login! Error: " + ex.Message)
        Finally
            If Not _mDriveService Is Nothing Then
                e.Result = True
            Else
                e.Result = False
            End If
        End Try
    End Sub
    Private Sub AuthEventFinished(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Try
            If e.Result = True Then
                _mLoggedIn = True
                GetUserInfo()
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Users, DateTime.Now, Me.Email.ToString & " successfully logged in!"))
                UpdateFreeSpace()
                PopulateExplorerRoot()

                If Not CredentialsManager.CheckCredentialsFileExists(_mEmail) Then
                    CredentialsManager.CreateFullClientCredentials(Me)
                End If
                'create credentials file if it doesn't exist
                If Not MainForm.ActiveUsers.Contains(Me) Then
                    MainForm.ActiveUsers.Add(Me)
                End If
                If MainForm.olvUsers.Items.Count > 0 Then
                    Dim users As ArrayList = MainForm.olvUsers.Objects
                    If Not users.Contains(Me) Then
                        MainForm.olvUsers.AddObject(Me)
                    End If
                Else
                    MainForm.olvUsers.AddObject((Me))
                End If
                MainForm.olvUsers.Refresh()
                MainForm.olvUsers.RefreshObject(Me)
            Else
                _mLoggedIn = False
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Users, DateTime.Now, Me.Email.ToString & " failed to login!"))
            End If
        Catch ex As Exception
            Dim tmp As String = ex.Message
        End Try

    End Sub
    Private Sub PopulateExplorerRoot()
        Try
            Dim rootF As File = _mDriveService.Files.Get("root").Fetch()
            rootF.OwnerNames(0) = _mEmail
            rootF.Title = _mEmail.ToString
            MainForm.objTreeExplorer.AddObject(rootF)
            MainForm.objTreeExplorer.RefreshObject(rootF)
            MainForm.objTreeExplorer.RefreshObjects(MainForm.objTreeExplorer.Roots)
        Catch ex As Exception
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Users, DateTime.Now, "An error occurred while attempting to populate the file explorer."))
        End Try
    End Sub
    Public Sub UpdateFreeSpace()
        Try
            Dim bout As About = _mDriveService.About.Get.Fetch()
            _mQuotaUsed = Convert.ToInt64(bout.QuotaBytesUsed)
            _mQuotaTotal = Convert.ToInt64(bout.QuotaBytesTotal)
            _mQuotaUsedGlobally = Convert.ToInt64(bout.QuotaBytesUsedAggregate)
            _mQuotaTrash = Convert.ToInt64(bout.QuotaBytesUsedInTrash)
        Catch ex As Exception
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Users, DateTime.Now, "An error occurred while attempting to retrieve available space."))
        End Try
    End Sub
End Class