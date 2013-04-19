Imports System.Runtime.InteropServices
Imports MultiDrive.My.Resources

Partial Public Class MainForm
    <DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Public Shared Function GetScrollPos(hWnd As Integer, nBar As Integer) As Integer
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function SetScrollPos(hWnd As IntPtr, nBar As Integer, nPos As Integer, bRedraw As Boolean) As Integer
    End Function

    Private Const SbHorz As Integer = &H0
    Private Const SbVert As Integer = &H1

    Private Function GetTreeViewScrollPos(treeView As TreeView) As Point
        Return New Point(GetScrollPos(CInt(treeView.Handle), SbHorz), GetScrollPos(CInt(treeView.Handle), SbVert))
    End Function
    Private Sub SetTreeViewScrollPos(treeView As TreeView, scrollPosition As Point)
        SetScrollPos(treeView.Handle, SbHorz, scrollPosition.X, True)
        SetScrollPos(treeView.Handle, SbVert, scrollPosition.Y, True)
    End Sub
    Function GetImage(ByVal url As String) As System.Drawing.Image
        Dim request As System.Net.HttpWebRequest
        Dim response As System.Net.HttpWebResponse
        Try
            request = System.Net.WebRequest.Create(url)
            response = request.GetResponse
            If request.HaveResponse Then 'Did we really get a response?
                If response.StatusCode = Net.HttpStatusCode.OK Then 'Is the status code 200? (You can check for more)
                    'GetImage = System.Drawing.Image.FromStream(response.GetResponseStream)
                    Return System.Drawing.Image.FromStream(response.GetResponseStream)
                End If
            End If
        Catch e As System.Net.WebException
            MessageBox.Show("A web exception has occured [" & url & "]." & vbCrLf & " System returned: " & e.Message, Message_Error_Exclamation, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Try
        Catch e As System.Net.ProtocolViolationException
            MessageBox.Show("A protocol violation has occured [" & url & "]." & vbCrLf & "  System returned: " & e.Message, Message_Error_Exclamation, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Try
        Catch e As System.Net.Sockets.SocketException
            MessageBox.Show("Socket error [" & url & "]." & vbCrLf & "  System returned: " & e.Message, Message_Error_Exclamation, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Try
        Catch e As System.IO.EndOfStreamException
            MessageBox.Show("An IO stream exception has occured. System returned: " & e.Message, Message_Error_Exclamation, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Try
        Finally
        End Try
        Return Nothing
    End Function
End Class
