Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Net.Mail

Public Class CredentialsManager
    Private Const ApplicationFolderName As String = "MultiDrive"
    Private Const ClientCredentialsFileName As String = "_googleusers.dat"
    Private Const FileKeyEmail As String = "EmailKey"
    Private Const FileKeyTokenFile As String = "TokenFileKey"
    Public Const FileKeyApiKey As String = "ProtectedApiKey"
    Public Const FileKeyClientId As String = "ProtectedClientId"
    Public Const FileKeyClientSecret As String = "ProtectedClientSecret"
    Private Const DefaultApiName As String = "MultiDrive - Google Drive Multi-User Manager"

    ' Random data used to make this encryption key different from other information encyrpted with ProtectedData
    ' This does not make it hard to decrypt just adds another small step.  
    Private Shared ReadOnly Entropy As Byte() = New Byte() {150, 116, 112, 35, 241, 210, _
        144, 9, 188, 122, 134, 253, _
        124, 111, 87, 51, 84, 178, _
        43, 176, 239, 123, 198, 249, _
        116, 190, 61, 129, 238, 23, _
        250, 163, 59, 26, 136}

    Public Shared Sub CreateFullClientCredentials(fullCredentials As GoogleUser)
        'Function to invoke forum to retrieve user info
        Using fStream As FileStream = CredentialsFile(fullCredentials.Email).OpenWrite()
            Using tw As TextWriter = New StreamWriter(fStream)
                tw.WriteLine("{0}={1}", FileKeyEmail, fullCredentials.Email)
                tw.WriteLine("{0}={1}", FileKeyTokenFile, fullCredentials.TokenFileName)

            End Using
        End Using
    End Sub

    Public Shared Function CheckForExistingUsers() As List(Of GoogleUser)
        Try
            Dim directoryInfo = New DirectoryInfo(AppData.SpecificPath)
            Dim lstUser As List(Of GoogleUser)
            If directoryInfo.Exists = False Then
                Return Nothing
            Else

                For Each tmpFile As FileInfo In directoryInfo.GetFiles
                    If tmpFile.Name.EndsWith("_googleusers.dat") Then
                        Dim iStart As Integer = tmpFile.Name.LastIndexOf("\") + 1
                        Dim iLength As Integer = tmpFile.Name.LastIndexOf("_") - iStart
                        Dim emailNoSpecialChars As String = tmpFile.Name.Substring(iStart, iLength)
                        If Not emailNoSpecialChars.Contains("_AT_") Then
                            GoTo ContinueLoop
                        End If
                        Dim email As String = emailNoSpecialChars.Replace("_AT_", "@")
                        If lstUser Is Nothing Then
                            lstUser = New List(Of GoogleUser)
                        End If
                        Dim user As GoogleUser = EnsureClientCredentials(email)
                        If user IsNot Nothing Then
                            lstUser.Add(user)
                        End If
                    End If
ContinueLoop:
                Next
            End If
            Return lstUser
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function EnsureClientCredentials(ByVal email As String) As GoogleUser

        If CredentialsFile(email).Exists = False Then
            Return Nothing
        End If

        Dim values As IDictionary(Of String, String) = ParseFile(email)
        If values.ContainsKey(FileKeyEmail) = False Or values.ContainsKey(FileKeyTokenFile) = False Then
            Return Nothing
        Else
            Return New GoogleUser() With { _
                .Email = email, _
                .TokenFileName = values(FileKeyTokenFile) _
            }
        End If

    End Function
    Public Shared Function CreateNewGUser() As GoogleUser
        Return New GoogleUser()
    End Function
    Public Shared Function CheckCredentialsFileExists(email As String) As Boolean
        If CredentialsFile(email).Exists Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Shared ReadOnly Property CredentialsFile(ByVal email As String) As FileInfo
        Get
            Dim mail As New MailAddress(email)
            Dim emailNoSpecialChars As String = mail.User.ToString + "_AT_" + mail.Host
            Dim directoryInfo = New DirectoryInfo(AppData.SpecificPath)
            If directoryInfo.Exists = False Then
                directoryInfo.Create()
            End If
            Return New FileInfo(Path.Combine(AppData.SpecificPath, emailNoSpecialChars & ClientCredentialsFileName))
        End Get
    End Property

    ''' <summary>
    '''     Returns a IDictionary of keys and values from the CredentialsFile which is expected to be of the form
    '''     <example>
    '''       key=value
    '''       key2=value2
    '''     </example>
    ''' </summary>
    Private Shared Function ParseFile(ByVal email As String) As IDictionary(Of String, String)
        Dim parsedValues = New Dictionary(Of String, String)(5)
        Using sr As StreamReader = CredentialsFile(email).OpenText()
            Dim currentLine As String = sr.ReadLine()
            While currentLine IsNot Nothing
                Dim firstEquals As Integer = currentLine.IndexOf("="c)
                If firstEquals > 0 AndAlso firstEquals + 1 < currentLine.Length Then
                    Dim key As String = currentLine.Substring(0, firstEquals)
                    Dim value As String = currentLine.Substring(firstEquals + 1)
                    parsedValues.Add(key, value)
                End If
                currentLine = sr.ReadLine()
            End While
        End Using

        Return parsedValues
    End Function
    Public Shared Sub ClearClientCredentials(ByVal email As String)
        Dim clientCredentials As FileInfo = CredentialsFile(email)
        If clientCredentials.Exists Then
            clientCredentials.Delete()
        End If
    End Sub
    Public Shared Function Protect(clearText As String) As String 'change back to private
        Dim encryptedData As Byte() = ProtectedData.Protect(Encoding.ASCII.GetBytes(clearText), Entropy, DataProtectionScope.CurrentUser)
        Dim tmp As Byte() = ProtectedData.Unprotect(encryptedData, Entropy, DataProtectionScope.CurrentUser)
        Dim tmp2 As String = Encoding.ASCII.GetString(tmp)
        Return Convert.ToBase64String(encryptedData)
    End Function


    Public Shared Function Unprotect(encrypted As String) As String 'change back to private
        Dim encryptedData As Byte() = Convert.FromBase64String(encrypted)
        Dim clearText As Byte() = ProtectedData.Unprotect(encryptedData, Entropy, DataProtectionScope.CurrentUser)
        Dim tmp As String = Encoding.ASCII.GetString(clearText)
        Return tmp
    End Function

End Class

Public Class ApiFile
    Private ReadOnly _mApiKey As String
    Private ReadOnly _mClientId As String
    Private ReadOnly _mEncodedClientSecret As String

    Public Sub New(ByVal apiKey As String, ByVal clientId As String, ByVal encodedClientSecret As String)
        _mApiKey = apiKey
        _mClientId = clientId
        _mEncodedClientSecret = encodedClientSecret
    End Sub
    Public ReadOnly Property ApiKey As String
        Get
            Return _mApiKey
        End Get
    End Property
    Public ReadOnly Property ClientId As String
        Get
            Return _mClientId
        End Get
    End Property
    Public ReadOnly Property EncodedClientSecret As String
        Get
            Return _mEncodedClientSecret
        End Get
    End Property

    Public Shared Function CheckApiKeyFileExists() As Boolean
        If ApiKeyFile().Exists Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Shared ReadOnly Property ApiKeyFile() As FileInfo
        Get
            Dim directoryInfo = New DirectoryInfo(AppData.SpecificPath)
            If directoryInfo.Exists = False Then
                directoryInfo.Create()
            End If
            Return New FileInfo(Path.Combine(AppData.SpecificPath, "APIKey.dat"))
        End Get
    End Property
    Public Shared Sub CreateApiFile(apiData As ApiFile)
        If ApiKeyFile.Exists Then
            ApiKeyFile.Delete()
        End If
        Using fStream As FileStream = ApiKeyFile().OpenWrite()
            Using tw As TextWriter = New StreamWriter(fStream)
                tw.WriteLine("{0}={1}", CredentialsManager.FileKeyApiKey, apiData.ApiKey)
                tw.WriteLine("{0}={1}", CredentialsManager.FileKeyClientId, apiData.ClientId)
                tw.WriteLine("{0}={1}", CredentialsManager.FileKeyClientSecret, apiData.EncodedClientSecret)
            End Using
        End Using
    End Sub
    Private Shared Function ParseApiFile() As IDictionary(Of String, String)
        If (CheckApiKeyFileExists()) Then
            Dim parsedValues = New Dictionary(Of String, String)(3)
            Using sr As StreamReader = ApiKeyFile.OpenText()
                Dim currentLine As String = sr.ReadLine()
                While currentLine IsNot Nothing
                    Dim firstEquals As Integer = currentLine.IndexOf("="c)
                    If firstEquals > 0 AndAlso firstEquals + 1 < currentLine.Length Then
                        Dim key As String = currentLine.Substring(0, firstEquals)
                        Dim value As String = currentLine.Substring(firstEquals + 1)
                        parsedValues.Add(key, value)
                    End If
                    currentLine = sr.ReadLine()
                End While
            End Using
            Return parsedValues
        End If
        Return Nothing
    End Function
    Public Shared Function EnsureApiFile() As ApiFile

        If ApiKeyFile().Exists = False Then
            Return Nothing
        End If

        Dim values As IDictionary(Of String, String) = ParseApiFile()
        If values.ContainsKey(CredentialsManager.FileKeyApiKey) = False Or values.ContainsKey(CredentialsManager.FileKeyClientId) = False Or values.ContainsKey(CredentialsManager.FileKeyClientSecret) = False Then
            Return Nothing
        End If
        Return New ApiFile(values(CredentialsManager.FileKeyApiKey), values(CredentialsManager.FileKeyClientId), values(CredentialsManager.FileKeyClientSecret))

    End Function
End Class
