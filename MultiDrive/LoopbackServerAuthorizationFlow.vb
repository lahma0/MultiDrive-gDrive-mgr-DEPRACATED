Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports DotNetOpenAuth.OAuth2
'Imports Google.Apis.Samples.Helper.Properties
'Imports System.Reflection

''' <summary>
''' A native authorization flow which uses a listening local loopback socket to fetch the authorization code.
''' </summary>
''' <remarks>Might not work if blocked by the system firewall.</remarks>
Public Class LoopbackServerAuthorizationFlow
    Implements INativeAuthorizationFlow

    Private Const LoopbackCallback As String = "http://localhost:{0}/{1}/authorize/"
    ''' <summary>
    ''' Returns a random, unused port.
    ''' </summary>
    Private Shared Function GetRandomUnusedPort() As Integer
        Dim listener = New TcpListener(IPAddress.Loopback, 0)
        Try
            listener.Start()
            Return DirectCast(listener.LocalEndpoint, IPEndPoint).Port
        Finally
            listener.[Stop]()
        End Try
    End Function

    ''' <summary>
    ''' Handles an incoming WebRequest.
    ''' </summary>
    ''' <param name="context">The request to handle.</param>
    ''' <returns>The authorization code, or null if the process was cancelled.</returns>
    Private Function HandleRequest(context As HttpListenerContext) As String
        Try
            ' Check whether we got a successful response:
            Dim code As String = context.Request.QueryString("code")
            If Not String.IsNullOrEmpty(code) Then
                Return code
            End If

            ' Check whether we got an error response:
            Dim [error] As String = context.Request.QueryString("error")
            If Not String.IsNullOrEmpty([error]) Then
                ' Request cancelled by user.
                Return Nothing
            End If

            ' The response is unknown to us. Choose a different authentication flow.
            Throw New NotSupportedException("Received an unknown response: " + Environment.NewLine + context.Request.RawUrl)
        Finally
            ' Write a response.
            Using writer = New StreamWriter(context.Response.OutputStream)
                Dim response As String = MultiDrive.My.Resources.LoopbackServerHtmlResponse.Replace("{APP}", Util.ApplicationName)
                writer.WriteLine(response)
                writer.Flush()
            End Using
            context.Response.OutputStream.Close()
            context.Response.Close()
        End Try
    End Function

    Public Function RetrieveAuthorization(client As UserAgentClient, authorizationState As IAuthorizationState) As String Implements INativeAuthorizationFlow.RetrieveAuthorization
        If Not HttpListener.IsSupported Then
            Throw New NotSupportedException("HttpListener is not supported by this platform.")
        End If

        ' Create a HttpListener for the specified url.
        Dim url As String = String.Format(LoopbackCallback, GetRandomUnusedPort(), Util.ApplicationName)
        authorizationState.Callback = New Uri(url)
        Dim webserver = New HttpListener()
        webserver.Prefixes.Add(url)

        ' Retrieve the authorization url.
        Dim authUrl As Uri = client.RequestUserAuthorization(authorizationState)

        Try
            ' Start the webserver.
            webserver.Start()

            ' Open the browser.
            Process.Start(authUrl.ToString())

            ' Wait for the incoming connection, then handle the request.
            Return HandleRequest(webserver.GetContext())
        Catch ex As HttpListenerException
            Throw New NotSupportedException("The HttpListener threw an exception.", ex)
        Finally
            ' Stop the server after handling the one request.
            webserver.[Stop]()
        End Try
    End Function
End Class
