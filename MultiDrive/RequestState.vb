Imports System
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic

Public Class RequestState
    Public bufferRead() As Byte
    Public request As HttpWebRequest
    Public response As HttpWebResponse
    Public streamResponse As Stream
    Public requestData As StringBuilder

    Public Sub New(contentLength As Integer)
        bufferRead = New Byte(contentLength) {}
        request = Nothing
        streamResponse = Nothing
        requestData = New StringBuilder("")
    End Sub
End Class
