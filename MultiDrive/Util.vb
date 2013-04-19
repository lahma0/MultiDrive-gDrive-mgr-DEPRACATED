Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''' <summary>
''' General Utility class for samples.
''' </summary>
Public Class Util
    ''' <summary>
    ''' Returns the name of the application currently being run.
    ''' </summary>
    Public Shared ReadOnly Property ApplicationName() As String
        Get
            Return Assembly.GetEntryAssembly().GetName().Name
        End Get
    End Property

    ''' <summary>
    ''' Tries to retrieve and return the content of the clipboard. Will trim the content to the specified length.
    ''' Removes all new line characters from the input.
    ''' </summary>
    ''' <remarks>Requires the STAThread attribute on the Main method.</remarks>
    ''' <returns>Trimmed content of the clipboard, or null if unable to retrieve.</returns>
    Public Shared Function GetSingleLineClipboardContent(maxLen As Integer) As String
        Try
            Dim text As String = Clipboard.GetText().Replace(vbCr, "").Replace(vbLf, "")
            If text.Length > maxLen Then
                Return text.Substring(0, maxLen)
            End If
            Return text
        Catch generatedExceptionName As ExternalException
            ' Something is preventing us from getting the clipboard content -> return.
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Changes the clipboard content to the specified value.
    ''' </summary>
    ''' <remarks>Requires the STAThread attribute on the Main method.</remarks>
    ''' <param name="text"></param>
    Public Shared Sub SetClipboard(text As String)
        Try
            Clipboard.SetText(text)
        Catch generatedExceptionName As ExternalException
        End Try
    End Sub
End Class
