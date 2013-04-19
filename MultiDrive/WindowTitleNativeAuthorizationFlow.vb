Imports System
Imports System.Windows.Forms
Imports DotNetOpenAuth.OAuth2
'Imports Google.Apis.Samples.Helper.Forms


''' <summary>
''' Describes a flow which captures the authorization code out of the window title of the browser.
''' </summary>
''' <remarks>Works on Windows, but not on Unix. Will failback to copy/paste mode if unsupported.</remarks>
Friend Class WindowTitleNativeAuthorizationFlow
    Implements MultiDrive.INativeAuthorizationFlow

    Private Const OutOfBandCallback As String = "urn:ietf:wg:oauth:2.0:oob"

    Public Function RetrieveAuthorization(client As UserAgentClient, authorizationState As IAuthorizationState) As String Implements INativeAuthorizationFlow.RetrieveAuthorization
        ' Create the Url.
        authorizationState.Callback = New Uri(OutOfBandCallback)
        Dim url As Uri = client.RequestUserAuthorization(authorizationState)

        ' Show the dialog.
        If Not Application.RenderWithVisualStyles Then
            Application.EnableVisualStyles()
        End If

        Application.DoEvents()
        'Dim authCode As String = OAuth2AuthorizationDialog.ShowDialog(url)
        Dim authCode As String = InputBox("Enter auth code (check code...):")
        Application.DoEvents()

        If String.IsNullOrEmpty(authCode) Then
            ' User cancelled the request.
            Return Nothing
        End If

        Return authCode
    End Function

    'Public Function RetrieveAuthorization1(client As UserAgentClient, authorizationState As IAuthorizationState) As String Implements INativeAuthorizationFlow.RetrieveAuthorization

    'End Function
End Class