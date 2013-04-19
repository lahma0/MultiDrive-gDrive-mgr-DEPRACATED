Imports BrightIdeasSoftware
Imports MultiDrive.MainForm
Imports Google.Apis.Drive.v2.Data
Imports Mvolo.ShellIcons
Imports System.ComponentModel

Partial Public Class MainForm
    Private Function CanExplorerItemExpand(lViewObject As Object) As Object
        Dim f As File = CType(lViewObject, File)
        If FileManager.IsFileFolder(f) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function ChildrenRetriever(lViewObject As Object) As Object
        Try
            Dim f As File = CType(lViewObject, File)
            Dim user As GoogleUser = GetUserByEmail(f.OwnerNames(0))
            Dim fList As FileList = FileManager.GetFilesAndFoldersInFolder(user.DriveService, f.Id, AppOptions.ShowTrashed <> Options.TrashDecision.Hide)
            Dim fArray(fList.Items.Count - 1) As File
            fList.Items.CopyTo(fArray, 0)
            For Each file As File In fArray
                file.OwnerNames(0) = user.Email
            Next
            Return fArray
        Catch ex As Google.GoogleApiRequestException
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, ex.Message))
            Return Nothing
        End Try
    End Function
    Public Function GetLabelIcons(rowObject As Object) As Object
        Dim file As File = CType(rowObject, File)
        If FileManager.IsFileStarred(file) Then
            Return "star"
        Else
            Return Nothing
        End If
    End Function
    Public Function GetFileIcon(rowObject As Object) As Object
        Try
            Dim file As File = CType(rowObject, File)
            Dim strMimeType As String = file.MimeType
            Dim strCurrentImageKey As String = strMimeType
            If file.FileExtension Is Nothing Then
                Dim b As Bitmap
                If Not docIconsSmall.Images.ContainsKey(strMimeType) Then
                    b = GetImage(file.IconLink)
                    docIconsSmall.Images.Add(strCurrentImageKey, b)
                End If
            Else
                strCurrentImageKey = file.FileExtension
                Using fileIcon As IconContainer = ShellIcons.GetIconForFile("." & strCurrentImageKey, True, False)
                    If Not docIconsSmall.Images.ContainsKey(strCurrentImageKey) Then
                        Dim b As Bitmap = fileIcon.Icon.ToBitmap
                        If Not b Is Nothing Then
                            docIconsSmall.Images.Add(strCurrentImageKey, b)
                        Else
                            docIconsSmall.Images.Add(strCurrentImageKey, GetImage(file.IconLink))
                        End If
                    End If
                End Using
            End If
            Return strCurrentImageKey
        Catch ex As Exception
            Return ""
        End Try
    End Function
    End Class
