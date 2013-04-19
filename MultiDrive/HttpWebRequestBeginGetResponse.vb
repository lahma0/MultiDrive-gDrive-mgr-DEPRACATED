Imports System
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic
'Imports Google.Apis
'Imports Google.Apis.Drive.v2
'Imports DotNetOpenAuth.OAuth2
'Imports Google.Apis.Authentication.OAuth2
'Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth
'Imports Google.Apis.Drive.v2.Data
'Imports Google.Apis.Util
'Imports Google.Apis.Drive.v2.FilesResource

Class HttpWebRequestBeginGetResponse

    Public Shared AllDone As New ManualResetEvent(False)
    Private _bufferSize As Integer
    Private _defaultTimeout As Integer = 2 * 60 * 1000
    Public Sub New(ByVal buffersize As Integer)
        _bufferSize = buffersize
    End Sub
    ' 2 minutes timeout 
    ' Abort the request if the timer fires. 
    Private Shared Sub TimeoutCallback(state As Object, timedOut As Boolean)
        If timedOut Then
            Dim request As HttpWebRequest = state

            If Not (request Is Nothing) Then
                request.Abort()
            End If
        End If
    End Sub
    Private Shared Sub ReadCallBack(asyncResult As IAsyncResult)
        Try

            Dim myRequestState As RequestState = CType(asyncResult.AsyncState, RequestState)
            Dim responseStream As Stream = myRequestState.streamResponse
            Dim read As Integer = responseStream.EndRead(asyncResult)
            ' Read the HTML page and then print it to the console. 
            If read > 0 Then
                myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.bufferRead, 0, read))
                Dim asynchronousResult As IAsyncResult = responseStream.BeginRead(myRequestState.bufferRead, 0, 1024, New AsyncCallback(AddressOf ReadCallBack), myRequestState)
                Return
            Else
                MessageBox.Show(ControlChars.Lf + "The contents of the Html page are : ")
                If myRequestState.requestData.Length > 1 Then
                    Dim stringContent As String
                    stringContent = myRequestState.requestData.ToString()
                    MessageBox.Show(stringContent)
                End If
                responseStream.Close()
            End If

        Catch e As WebException
            Debug.Print(ControlChars.Lf + "ReadCallBack Exception raised!")
            Debug.Print(ControlChars.Lf + "Message:{0}", e.Message)
            Debug.Print(ControlChars.Lf + "Status:{0}", e.Status)
        End Try
        allDone.Set()
    End Sub 'ReadCallBack

    Private Shared Sub RespCallback(asynchronousResult As IAsyncResult)
        Try
            ' State of request is asynchronous. 
            Dim myRequestState As RequestState = CType(asynchronousResult.AsyncState, RequestState)
            Dim myHttpWebRequest As HttpWebRequest = myRequestState.request
            myRequestState.response = CType(myHttpWebRequest.EndGetResponse(asynchronousResult), HttpWebResponse)

            ' Read the response into a Stream object. 
            Dim responseStream As Stream = myRequestState.response.GetResponseStream()
            myRequestState.streamResponse = responseStream

            ' Begin the Reading of the contents of the HTML page and print it to the console. 
            Dim asynchronousInputRead As IAsyncResult = responseStream.BeginRead(myRequestState.bufferRead, 0, 1024, New AsyncCallback(AddressOf ReadCallBack), myRequestState)
            Return
        Catch e As WebException
            Debug.Print(ControlChars.Lf + "RespCallback Exception raised!")
            Debug.Print(ControlChars.Lf + "Message:{0}", e.Message)
            Debug.Print(ControlChars.Lf + "Status:{0}", e.Status)
        End Try
        allDone.Set()
    End Sub 'RespCallback

End Class 'HttpWebRequest_BeginGetResponse