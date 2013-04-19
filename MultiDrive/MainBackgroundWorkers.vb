'Imports System.IO
'Imports System.Net
'Imports System.Net.Http
Imports Google.Apis
Imports Google.Apis.Drive.v2
Imports DotNetOpenAuth.OAuth2
Imports Google.Apis.Authentication.OAuth2
Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth
Imports Google.Apis.Drive.v2.Data
Imports Google.Apis.Util
Imports Google.Apis.Drive.v2.FilesResource
'Imports System.Threading
Imports System.ComponentModel
'Imports Microsoft.Win32
'Imports System.Runtime.InteropServices
Imports Mvolo.ShellIcons
Imports BrightIdeasSoftware

Partial Public Class MainForm
    'Private bwAuth As BackgroundWorker
    Private _bwExistingUsersCheck As BackgroundWorker
    Private _bwExplorerObjectsDropped As BackgroundWorker
    Private _bwExplorerFilesDropped As BackgroundWorker
    Private _bwFilesRename As BackgroundWorker
    Private _bwFilesDelete As BackgroundWorker

    Private Delegate Function MGetExplorerRoots() As ArrayList
    Private Delegate Function MGetExplorerSelectedObjects() As ArrayList
    Private Delegate Function MGetExplorerExpandedObjects() As ArrayList
    Private Delegate Function MGetExplorerSelectedObject() As Object
    Private Delegate Function MGetExplorerObjectParent(modelObject As Object) As Object
    Private Delegate Function MGetExplorerObjectChildren(modelObject As Object) As ArrayList
    Public Delegate Sub MRefreshExplorerObjects(objects As ArrayList)
    Public Delegate Sub MRefreshExplorerObject(objects As Object)
    Public Delegate Sub MRefreshExplorer()
    Private Delegate Sub MRemoveExplorerObjects(objects As ArrayList)
    Private Delegate Sub MRemoveExplorerObject(rowObject As Object)

    Private Function GetExplorerExpandedObjects() As ArrayList
        Dim expandedObjects As New ArrayList
        For Each obj As Object In objTreeExplorer.ExpandedObjects
            expandedObjects.Add(obj)
        Next
        Return expandedObjects
    End Function
    Private Function GetExplorerObjectChildren(modelObject As Object) As ArrayList
        Return objTreeExplorer.GetChildren(modelObject)
    End Function
    Private Function GetExplorerObjectParent(modelObject As Object) As Object
        Return objTreeExplorer.GetParent(modelObject)
    End Function
    Private Sub RemoveExplorerObjects(objects As ArrayList)
        objTreeExplorer.RemoveObjects(objects)
    End Sub
    Private Sub RemoveExplorerObject(rowObject As Object)
        objTreeExplorer.RemoveObject(rowObject)
    End Sub
    Public Sub RefreshExplorerObject(rowObject As Object)
        Try
            objTreeExplorer.RefreshObject(rowObject)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub RefreshExplorer()
        objTreeExplorer.Refresh()
    End Sub
    Public Sub RefreshExplorerObjects(objects As ArrayList)
        objTreeExplorer.RefreshObjects(objects)
    End Sub
    Private Function GetExplorerRoots() As ArrayList
        Return objTreeExplorer.Roots()
    End Function
    Private Function GetExplorerSelectedObjects() As ArrayList
        Return objTreeExplorer.SelectedObjects
    End Function
    Private Function GetExplorerSelectedObject() As Object
        Return objTreeExplorer.SelectedObject
    End Function
    Private Sub FilesRenameStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim file As Drive.v2.Data.File = objTreeExplorer.Invoke(New MGetExplorerSelectedObjects(AddressOf GetExplorerSelectedObjects))(0)
            Dim oldTitle As String = DirectCast(e.Argument.Value, String)
            Dim user As GoogleUser = GetUserByEmail(file.OwnerNames(0))
            Dim fileId As String = file.Id
            Dim successful As Boolean = False
            Dim newTitle As String = DirectCast(e.Argument.NewValue, String)
            successful = FileManager.RenameFile(user.DriveService, fileId, newTitle)
            If successful = True Then
                file.Title = newTitle
                objTreeExplorer.Invoke(New MRefreshExplorerObjects(AddressOf RefreshExplorerObjects), New ArrayList({file}))
                AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Successfully renamed " & oldTitle & " to " & newTitle))
            Else
                AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Failed to rename " & oldTitle))
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while renaming the selected files."))
        End Try
    End Sub
    Private Sub FilesRenameFinish(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        RemoveHandler _bwFilesRename.DoWork, AddressOf FilesRenameStart
        RemoveHandler _bwFilesRename.RunWorkerCompleted, AddressOf FilesRenameFinish
        _bwFilesRename = Nothing
    End Sub

    Private Sub FilesDeleteStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim skipTrash As Boolean = e.Argument
            If skipTrash = True Then
                ExplorerDelete()
            Else
                ExplorerTrash()
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while deleting the selected items."))
        End Try
    End Sub
    Private Sub FilesDeleteFinish(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        RemoveHandler _bwFilesDelete.DoWork, AddressOf FilesDeleteStart
        RemoveHandler _bwFilesDelete.RunWorkerCompleted, AddressOf FilesDeleteFinish
        _bwFilesDelete = Nothing
    End Sub

    Private Sub ExplorerObjectsDroppedStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim dropEventArgs As OlvDropEventArgs = e.Argument
            Dim dropObject As Object = dropEventArgs.DataObject
            Dim filesToMove As ArrayList = dropObject.ModelObjects
            Dim refreshUsers As New List(Of GoogleUser)
            For Each droppedFile As Data.File In filesToMove
                Dim user As GoogleUser = GetUserByEmail(droppedFile.OwnerNames(0))
                Dim oldParentFolder As Data.File = objTreeExplorer.GetParent(droppedFile)
                Dim oldParentFolderId As String = oldParentFolder.Id
                'If oldParentFolder.Parents.Count = 0 Then
                '    Dim tmp As Data.File = user.DriveService.Files.Get("root").Fetch()
                '    oldParentFolderId = tmp.Id
                'Else
                '    oldParentFolderId = oldParentFolder.Id
                'End If
                Dim newParentFolder As Data.File = dropEventArgs.DropTargetItem.RowObject
                Dim newFolderId As String
                If Not FileManager.IsFileFolder(newParentFolder) Then
                    newParentFolder = objTreeExplorer.GetParent(newParentFolder)
                    newFolderId = newParentFolder.Id
                Else
                    newFolderId = newParentFolder.Id
                End If
                Dim targetUser As GoogleUser = GetUserByEmail(newParentFolder.OwnerNames(0))
                If user Is targetUser Then
                    If Not refreshUsers.Contains(user) Then refreshUsers.Add(user)
                    Dim successful As Boolean = FileManager.MoveFileToNewFolder(user.DriveService, droppedFile.Id, oldParentFolderId, newFolderId)
                    If successful Then
                        AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Successfully moved " & droppedFile.Title & " to " & newParentFolder.Title & "!"))
                    End If
                Else
                    If Not refreshUsers.Contains(user) Then refreshUsers.Add(user)
                    If Not refreshUsers.Contains(targetUser) Then refreshUsers.Add(targetUser)
                    Dim directT As New Peer2Peer(droppedFile, newParentFolder, user, targetUser)
                    Dim q As QueueItem = directT
                    objListViewTransfers.AddObject(q)
                End If
            Next
            'If objTreeExplorer.InvokeRequired Then
            Dim lst As ArrayList = objTreeExplorer.Invoke(New MGetExplorerRoots(AddressOf GetExplorerRoots))
            For Each f As Data.File In lst
                Dim found As Boolean = False
                For Each usr As GoogleUser In refreshUsers
                    If usr.Email = f.OwnerNames(0) Then found = True
                Next
                If Not found Then lst.Remove(f)
            Next
            objTreeExplorer.Invoke(New MRefreshExplorerObjects(AddressOf RefreshExplorerObjects), lst)
            'Else
            'objTreeExplorer.RefreshObjects(objTreeExplorer.Roots)
            'End If
            'objTreeExplorer.Refresh()
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while moving the selected explorer items."))
        End Try
    End Sub
    Private Sub ExplorerObjectsDroppedFinish(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        RemoveHandler _bwExplorerObjectsDropped.DoWork, AddressOf ExplorerObjectsDroppedStart
        RemoveHandler _bwExplorerObjectsDropped.RunWorkerCompleted, AddressOf ExplorerObjectsDroppedFinish
        _bwExplorerObjectsDropped = Nothing
    End Sub

    Private Sub ExplorerFilesDroppedStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim dropEventArgs As OlvDropEventArgs = e.Argument
            Dim dropFiles() As String = dropEventArgs.DragEventArgs.Data.GetData(DataFormats.FileDrop)
            If dropFiles Is Nothing Then
                Exit Sub
            End If
            Dim file As Data.File = dropEventArgs.DropTargetItem.RowObject
            Dim parentRowObject As Data.File
            Dim user As GoogleUser = GetUserByEmail(file.OwnerNames(0))
            If Not FileManager.IsFileFolder(file) Then
                parentRowObject = objTreeExplorer.GetParent(file)
            Else
                parentRowObject = file
            End If
            For Each path As String In dropFiles
                If Not path.Contains("tmp_dont_delete.GMultiDrive") Then
                    If IO.File.Exists(path) Then
                        Dim upload As New Upload(user, path, parentRowObject)
                        Dim q As QueueItem = upload
                        objListViewTransfers.AddObject(q)
                    End If
                End If
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to setup the file transfer."))
        End Try
    End Sub

    Private Sub ExplorerFilesDroppedFinish(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)

    End Sub

    Private Sub ExistingUsersCheckStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Users, DateTime.Now, "Checking for existing users..."))
            Dim tmpList As List(Of GoogleUser) = CredentialsManager.CheckForExistingUsers
            If tmpList IsNot Nothing Then
                ActiveUsers.AddRange(tmpList)
                olvUsers.AddObjects(tmpList)
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Error checking for existing users!"))
        End Try
    End Sub
    Private Sub ExistingUsersCheckFinish(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Users, DateTime.Now, If(ActiveUsers Is Nothing, "0", ActiveUsers.Count) & " users found."))
        RemoveHandler _bwExistingUsersCheck.DoWork, AddressOf ExistingUsersCheckStart
        RemoveHandler _bwExistingUsersCheck.RunWorkerCompleted, AddressOf ExistingUsersCheckFinish
        For Each u As GoogleUser In ActiveUsers
            u.InitiateLogin()
        Next
    End Sub

End Class
