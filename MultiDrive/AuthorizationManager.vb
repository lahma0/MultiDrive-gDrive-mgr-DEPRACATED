Imports System
Imports System.Linq
Imports System.Reflection
Imports System.Security.Authentication
Imports System.Security.Cryptography
Imports System.Text
Imports DotNetOpenAuth.OAuth2
Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth
Imports System.IO

Public NotInheritable Class AuthorizationMgr
    Private Sub New()
    End Sub
    Private Shared ReadOnly NativeFlows As INativeAuthorizationFlow() = New INativeAuthorizationFlow() {New LoopbackServerAuthorizationFlow(), New WindowTitleNativeAuthorizationFlow()}

    ''' <summary>
    ''' Requests authorization on a native client by using a predefined set of authorization flows.
    ''' </summary>
    ''' <param name="client">The client used for authentication.</param>
    ''' <param name="authState">The requested authorization state.</param>
    ''' <returns>The authorization code, or null if cancelled by the user.</returns>
    ''' <exception cref="NotSupportedException">Thrown if no supported flow was found.</exception>
    Public Shared Function RequestNativeAuthorization(client As NativeApplicationClient, authState As IAuthorizationState) As String
        ' Try each available flow until we get an authorization / error.
        For Each flow As INativeAuthorizationFlow In NativeFlows
            Try
                Return flow.RetrieveAuthorization(client, authState)
                ' Flow unsupported on this environment 
            Catch generatedExceptionName As NotSupportedException
            End Try
        Next

        Throw New NotSupportedException("Found no supported native authorization flow.")
    End Function

    ''' <summary>
    ''' Requests authorization on a native client by using a predefined set of authorization flows.
    ''' </summary>
    ''' <param name="client">The client used for authorization.</param>
    ''' <param name="scopes">The requested set of scopes.</param>
    ''' <returns>The authorized state.</returns>
    ''' <exception cref="AuthenticationException">Thrown if the request was cancelled by the user.</exception>
    Public Shared Function RequestNativeAuthorization(client As NativeApplicationClient, ParamArray scopes As String()) As IAuthorizationState
        Dim state As IAuthorizationState = New AuthorizationState(scopes)
        'THIS CODE BELOW SIMPLY GETS THE AUTH CODE FROM THE USER, you can replace this with:
        'Dim authUri As Uri = arg.RequestUserAuthorization(state)
        'Process.Start(authUri.ToString())
        'Dim authCode As String = InputBox("Please enter your authentication code provided by your browser: ", "Authorization Code:", "")
        Dim authCode As String = RequestNativeAuthorization(client, state)

        If String.IsNullOrEmpty(authCode) Then
            Throw New AuthenticationException("The authentication request was cancelled by the user.")
        End If

        Return client.ProcessUserAuthorization(authCode, state)
    End Function
    ''' <summary>
    ''' Returns a cached refresh token for this application, or null if unavailable.
    ''' </summary>
    ''' <param name="storageName">The file name (without extension) used for storage.</param>
    ''' <param name="key">The key to decrypt the data with.</param>
    ''' <returns>The authorization state containing a Refresh Token, or null if unavailable</returns>
    Public Shared Function GetCachedRefreshToken(tokenFileName As String, storageName As String, key As String) As AuthorizationState
        Dim contents As Byte() = AppData.ReadFile(tokenFileName)

        If contents Is Nothing Then
            ' No cached token available.
            Return Nothing
        End If

        Dim salt As Byte() = Encoding.Unicode.GetBytes(Assembly.GetEntryAssembly().FullName + key)
        Dim decrypted As Byte() = ProtectedData.Unprotect(contents, salt, DataProtectionScope.CurrentUser)
        Dim content As String() = Encoding.Unicode.GetString(decrypted).Split(New String() {vbCr & vbLf}, StringSplitOptions.None)

        ' Create the authorization state.
        Dim scopes As String() = content(0).Split(New String() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim refreshToken As String = content(1)
        Return New AuthorizationState(scopes) With { _
            .RefreshToken = refreshToken _
        }
    End Function
    ''' <summary>
    ''' Saves a refresh token to the specified storage name, and encrypts it using the specified key.
    ''' Returns cache token data-file's filename
    ''' </summary>
    Public Shared Function SetCachedRefreshToken(storageName As String, key As String, state As IAuthorizationState) As String
        ' Create the file content.
        Dim scopes As String = state.Scope.Aggregate("", Function(left, append) left + " " + append)
        Dim content As String = scopes + vbCr & vbLf + state.RefreshToken

        ' Encrypt it.
        Dim salt As Byte() = Encoding.Unicode.GetBytes(Assembly.GetEntryAssembly().FullName + key)
        Dim encrypted As Byte() = ProtectedData.Protect(Encoding.Unicode.GetBytes(content), salt, DataProtectionScope.CurrentUser)

        ' Save the data to the auth file.
        Dim directoryInfo = New DirectoryInfo(AppData.SpecificPath)
        If directoryInfo.Exists = False Then
            System.IO.Directory.CreateDirectory(AppData.SpecificPath)
        End If
        Dim fileExists As Boolean = True
        Dim i As Integer = 0
        Dim fileName As String = ""
        Do While (fileExists = True)
            fileName = "cacheToken(" & i.ToString & ")_" & storageName & ".auth"
            If Not AppData.Exists(fileName) Then fileExists = False
            i += 1
        Loop
        AppData.WriteFile(fileName, encrypted)
        Return fileName

    End Function
End Class