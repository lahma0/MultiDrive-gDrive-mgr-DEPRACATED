Imports System
Imports System.IO

    ''' <summary>
    ''' Provides access to the user's "AppData" folder
    ''' </summary>
    Public NotInheritable Class AppData
        Private Sub New()
        End Sub
        ''' <summary>
        ''' Path to the Application specific %AppData% folder.
        ''' </summary>
        Public Shared ReadOnly Property SpecificPath() As String
            Get
                Dim appData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                Return Path.Combine(appData, Util.ApplicationName)
            End Get
        End Property

        ''' <summary>
        ''' Returns the path to the specified AppData file. Ensures that the AppData folder exists.
        ''' </summary>
        Public Shared Function GetFilePath(file As String) As String
            Dim dir As String = SpecificPath
            If Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Return Path.Combine(SpecificPath, file)
        End Function

        ''' <summary>
        ''' Reads the specific file (if it exists), or returns null otherwise.
        ''' </summary>
        ''' <returns>File contents or null.</returns>
    Public Shared Function ReadFile(file1 As String) As Byte()
        Dim path As String = GetFilePath(file1)
        Return If(File.Exists(path), File.ReadAllBytes(path), Nothing)
    End Function

        ''' <summary>
        ''' Returns true if the specified file exists in this AppData folder.
        ''' </summary>
    Public Shared Function Exists(file1 As String) As Boolean
        Return File.Exists(GetFilePath(file1))
    End Function

        ''' <summary>
        ''' Writes the content to the specified file. Will create directories and files as necessary.
        ''' </summary>
    Public Shared Sub WriteFile(file1 As String, contents As Byte())
        Dim path As String = GetFilePath(file1)
        File.WriteAllBytes(path, contents)
    End Sub
    End Class
