Imports Google.Apis
Imports Google.Apis.Drive.v2
Imports BrightIdeasSoftware
Imports System.ComponentModel
Imports MultiDrive.My.Resources
Imports System.IO
Imports System.Timers
Imports System.Xml.Serialization
Imports File = Google.Apis.Drive.v2.Data.File

Public Class MainForm

    Public MAppOptions As Options
    Public MActiveUsers As New List(Of GoogleUser)
    Delegate Sub MToolLabel(ByVal value As String)
    Delegate Function MGetUserByEmail(email As String)
    Private ReadOnly _mTempPath As String = AppData.SpecificPath
    Private _mDta As DataObject
    Private _mDroppedTmpPath As String
    Private _mTempFileTimer As System.Timers.Timer
    Public MSettingsFilePath As String = _mTempPath & "\settings.xml"
    Private _mActiveDownloads As Integer = 0
    Private _mActiveUploads As Integer = 0
    Private _mActiveP2P As Integer = 0

    Public Property ActiveDownloads As Integer
        Get
            Return _mActiveDownloads
        End Get
        Set(value As Integer)
            _mActiveDownloads = value
        End Set
    End Property
    Public Property ActiveUploads As Integer
        Get
            Return _mActiveUploads
        End Get
        Set(value As Integer)
            _mActiveUploads = value
        End Set
    End Property
    Public Property ActiveP2P As Integer
        Get
            Return _MActiveP2P
        End Get
        Set(value As Integer)
            _MActiveP2P = value
        End Set
    End Property
    Public ReadOnly Property AppOptions As Options
        Get
            If MAppOptions Is Nothing Then
                MAppOptions = New Options()
            End If
            Return MAppOptions
        End Get
    End Property
    Public Property ActiveUsers As List(Of GoogleUser)
        Get
            If Not MActiveUsers Is Nothing Then
                Return MActiveUsers
            Else
                Return Nothing
            End If
        End Get
        Set(value As List(Of GoogleUser))
            MActiveUsers = value
        End Set
    End Property

    Public Sub AddLogItem(entry As LogEntry)
        If Not entry Is Nothing Then

            If objLViewLog.InvokeRequired Then
                objLViewLog.Invoke(New MAddLogObject(AddressOf AddLogObject), entry)
                objLViewLog.Invoke(New MMoveLogObjectsToTop(AddressOf MoveLogObjectsToTop), New ArrayList({entry}))
            Else
                objLViewLog.AddObject(entry)
                objLViewLog.MoveObjects(0, New ArrayList({entry}))
            End If
        End If
    End Sub

    Private Sub PopulateOptions()
        Try
            numUpDownSimulDownloads.Value = AppOptions.SimultaneousDownloads
            numUpDownSimulUploads.Value = AppOptions.SimultaneousUploads
            numUpDownSimulPeerToPeer.Value = AppOptions.SimultaneousP2PTransfers
            txtDefaultDownloadLocation.Text = AppOptions.DownloadLocation
            numUpDownDLoadChunkSize.Value = AppOptions.DownloadBufferSize / 1024
            numUpDownULoadChunkSize.Value = AppOptions.UploadBufferSize / 1024
            numUpDownP2PChunkSize.Value = AppOptions.Peer2PeerBufferSize / 1024
            Select Case AppOptions.IfFileExistsDecision
                Case Options.FileExistsDecision.Prompt
                    cmbFileExistsDecision.SelectedIndex = 0
                Case Options.FileExistsDecision.Overwrite
                    cmbFileExistsDecision.SelectedIndex = 1
                Case Options.FileExistsDecision.Rename
                    cmbFileExistsDecision.SelectedIndex = 2
            End Select
            If AppOptions.Md5CheckDLoad Then chkBoxOptionsMD5Downloads.Checked = True
            If AppOptions.Md5CheckULoad Then chkBoxOptionsMD5Uploads.Checked = True
            If AppOptions.Md5CheckP2P Then chkBoxOptionsMD5P2P.Checked = True
            Select Case AppOptions.ShowTrashed
                Case Options.TrashDecision.Highlight
                    cmbTrashed.SelectedIndex = 0
                Case Options.TrashDecision.Show
                    cmbTrashed.SelectedIndex = 1
                Case Options.TrashDecision.Hide
                    cmbTrashed.SelectedIndex = 2
            End Select
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Settings, DateTime.Now, "Error populating options."))
        End Try
    End Sub
    Private Sub LoadOlvSettings()
        objTreeExplorer.CellToolTip.Font = New Font("Microsoft Sans Serif", 10, FontStyle.Bold)
        objTreeExplorer.CellToolTip.BackColor = Color.WhiteSmoke
        objListViewTransfers.UseCellFormatEvents = True
        olvColFileName.ImageGetter = New ImageGetterDelegate(AddressOf GetDirectionIcon)
        olvColProgress.AspectGetter = New AspectGetterDelegate(AddressOf TransferProgressUpdater)
        olvColTransferSize.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatter)
        olvColProgress.GroupKeyGetter = New GroupKeyGetterDelegate(AddressOf ProgressGroupKeyGetter)
        olvColFileName.GroupKeyGetter = New GroupKeyGetterDelegate(AddressOf QueueDirectionGroupKeyGetter)
        olvColExpFileSize.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatter)
        olvColExpModifiedDate.AspectGetter = New AspectGetterDelegate(AddressOf DateFormatter)
        olvColExpQuotaBytesUsed.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatter)
        olvColUsersQuotaFree.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatterQuotaFree)
        olvColUsersQuotaPercentUsed.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatterQuotaPercentUsed)
        olvColUsersQuotaTotal.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatterQuotaTotal)
        olvColUsersQuotaTrash.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatterTrash)
        olvColUsersQuotaUsed.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatterQuotaUsed)
        olvColUsersQuotaUsedGlobally.AspectGetter = New AspectGetterDelegate(AddressOf FileSizeFormatterQuotaUsedGlobally)
        objTreeExplorer.CanExpandGetter = New TreeListView.CanExpandGetterDelegate(AddressOf CanExplorerItemExpand)
        objTreeExplorer.ChildrenGetter = New TreeListView.ChildrenGetterDelegate(AddressOf ChildrenRetriever)
        objTreeExplorer.AddDecoration(New EditingCellBorderDecoration With {.UseLightbox = True})
        olvColExpTitle.ImageGetter = New ImageGetterDelegate(AddressOf GetFileIcon)
        olvColExpDescription.ImageGetter = New ImageGetterDelegate(AddressOf GetLabelIcons)
        olvColUsersEmail.GroupKeyGetter = New GroupKeyGetterDelegate(AddressOf UsersGroupKeyGetter)
        olvColUsersPicture.AspectGetter = New AspectGetterDelegate(AddressOf UsersPicAspectGetter)
        olvColUsersPicture.ImageGetter = New ImageGetterDelegate(AddressOf UsersPicImageGetter)
        olvColUsersPicture.AspectToStringConverter = New AspectToStringConverterDelegate(AddressOf UsersPicAspectToStringConverter)
        olvColUsersLoggedIn.AspectGetter = New AspectGetterDelegate(AddressOf UsersStatusAspectGetter)
        olvColUsersLoggedIn.ImageGetter = New ImageGetterDelegate(AddressOf UsersStatusImageGetter)
        olvColUsersLoggedIn.AspectToStringConverter = New AspectToStringConverterDelegate(AddressOf UsersStatusAspectToStringConverter)
        olvLogMessage.ImageGetter = New ImageGetterDelegate(AddressOf GetLogTypeIcon)
        olvLogMessage.GroupKeyGetter = New GroupKeyGetterDelegate(AddressOf LogGroupKeyGetter)
        olvLogTime.AspectGetter = New AspectGetterDelegate(AddressOf LogTimeFormatter)
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If AppOptions IsNot Nothing Then
            SaveOptions()
            SaveFormState()
            SaveExplorerState()
            SaveQueueState()
            SaveUsersState()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not IO.Directory.Exists(_mTempPath) Then IO.Directory.CreateDirectory(_mTempPath)
        LoadOptions()
        LoadFormState()
        RestoreExplorerState()
        RestoreQueueState()
        RestoreUsersState()
        PopulateOptions()
        LoadOlvSettings()
        SetupFsm()
        contextMenuExplorerFiles.ImageList = docIconsSmall
        tStripExpFilesDelete.ImageKey = "deletefile"
        tStripExpFilesDownload.ImageKey = ImageKey_download
        tStripExpFilesDownloadTo.ImageKey = "downloadto"
        tStripExpFilesDownloadAs.ImageKey = "downloadas"
        tStripExpFilesOpen.ImageKey = ImageKey_open
        tStripExpFilesRename.ImageKey = ImageKey_rename
        tStripExpFilesTrash.ImageKey = ImageKey_trash
        tStripExpFilesStar.ImageKey = ImageKey_star
        tStripExpFilesRefresh.ImageKey = "refresh"
        tStripExpFilesNewFolder.ImageKey = "newfolder"
        tStripExplorer.ImageList = docIconsMedium
        tStripBtnExpNewFolder.ImageKey = "newfolder"
        tStripBtnExpDelete.ImageKey = "remove"
        tStripBtnExpTrash.ImageKey = ImageKey_trash
        tStripBtnExpDownload.ImageKey = "addqueue"
        tStripMenuItemExpAddDownloadAs.Image = docIconsMedium.Images("downloadas")
        tStripMenuItemExpAddDownloadTo.Image = docIconsMedium.Images("downloadto")
        tStripMenuItemExpAddDownload.Image = docIconsMedium.Images("addqueue")
        tStripBtnExpOpen.ImageKey = ImageKey_open
        tStripBtnExpStar.ImageKey = ImageKey_star
        tStripBtnExpRefresh.ImageKey = "refresh"
        tStripQueue.ImageList = docIconsMedium
        tStripBtnQueueCancel.ImageKey = "remove"
        tStripBtnQueueStart.ImageKey = "start"
        tStripBtnQueuePause.ImageKey = "pause"
        tStripDropBtnQueueClear.ImageKey = "deletequeue"
        tStripMenuItemQueueClearSelected.Image = docIconsMedium.Images("deletequeue")
        tStripMenuItemQueueClearCompleted.Image = docIconsMedium.Images("check")
        tStripMenuItemQueueClearInactive.Image = docIconsMedium.Images("deletefinishedqueue")
        _bwExistingUsersCheck = New BackgroundWorker
        AddHandler _bwExistingUsersCheck.DoWork, AddressOf ExistingUsersCheckStart
        AddHandler _bwExistingUsersCheck.RunWorkerCompleted, AddressOf ExistingUsersCheckFinish
        _bwExistingUsersCheck.RunWorkerAsync()
        tStripUsersButtonAddUser.Image = docIconsMedium.Images("adduser")
        tStripUsersButtonRemoveUser.Image = docIconsMedium.Images("removeuser")
        tStripUsersButtonRemoveAllUsers.Image = docIconsMedium.Images("removeallusers")
        tStripBtnExpAbout.ImageKey = "help"
    End Sub
    Private Sub objListViewTransfers_FormatCell(sender As Object, e As FormatCellEventArgs) Handles objListViewTransfers.FormatCell
        Dim qItem As QueueItem = CType(e.Model, QueueItem)
        If e.ColumnIndex = 2 Then
            e.SubItem.Decoration = New TextDecoration(qItem.Status.ToString, 80, ContentAlignment.MiddleCenter)
        Else
            e.SubItem.Decoration = Nothing
        End If
    End Sub

    Private Sub objListViewTransfers_ItemsAdding(sender As Object, e As ItemsAddingEventArgs) Handles objListViewTransfers.ItemsAdding
        Try
            Dim oList() As Object = e.ObjectsToAdd
            For Each qItem As QueueItem In From qItem1 As QueueItem In oList Where qItem1 IsNot Nothing
                Select Case qItem.TransferType
                    Case QueueItem.QueueType.Download
                        If _mActiveDownloads < AppOptions.SimultaneousDownloads Then
                            qItem.TransferStart()
                        End If
                    Case QueueItem.QueueType.Upload
                        If _mActiveUploads < AppOptions.SimultaneousUploads Then
                            qItem.TransferStart()
                        End If
                    Case QueueItem.QueueType.Peer2Peer
                        If _mActiveP2P < AppOptions.SimultaneousP2PTransfers Then
                            qItem.TransferStart()
                        End If
                End Select
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Function GetUserByEmail(email As String) As GoogleUser
        For Each gUser As GoogleUser In From gUser1 In MActiveUsers Where gUser1.Email = email
            Return gUser
        Next
        Return Nothing
    End Function

    Private Sub tStripExpFilesDownload_Click(sender As Object, e As EventArgs) Handles tStripExpFilesDownload.Click
        AddDownloadsToQueue()
    End Sub
    Private Sub AddDownloadsToQueue()
        For Each file As Drive.v2.Data.File In objTreeExplorer.SelectedObjects
            Dim user As GoogleUser = GetUserByEmail(file.OwnerNames(0))
            Dim dLoad As New Download(user, file, AppOptions)
            Dim q As QueueItem = dLoad
            objListViewTransfers.AddObject(dLoad)
        Next
    End Sub
    Private Sub tStripExpFilesOpen_Click(sender As Object, e As EventArgs) Handles tStripExpFilesOpen.Click
        OpenSelectedFiles()
    End Sub
    Private Sub OpenSelectedFiles()
        For Each file As Drive.v2.Data.File In objTreeExplorer.SelectedObjects
            Try
                Process.Start(file.AlternateLink)
            Catch ex As Win32Exception
                Process.Start("IExplore.exe", file.AlternateLink)
            End Try
        Next
    End Sub
    Private Sub objTreeExplorer_CellEditFinishing(sender As Object, e As CellEditEventArgs) Handles objTreeExplorer.CellEditFinishing
        If Not e.Value = e.NewValue Then
            _bwFilesRename = New BackgroundWorker
            AddHandler _bwFilesRename.DoWork, AddressOf FilesRenameStart
            AddHandler _bwFilesRename.RunWorkerCompleted, AddressOf FilesRenameFinish
            _bwFilesRename.RunWorkerAsync(e)
        End If
    End Sub
    Private Sub tStripExpFilesRename_Click(sender As Object, e As EventArgs) Handles tStripExpFilesRename.Click
        If objTreeExplorer.SelectedObjects.Count > 1 Then
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, "You may only rename 1 file at a time!"))
        Else
            Dim file As Drive.v2.Data.File = objTreeExplorer.SelectedObject
            Dim listRootObjects As ArrayList = objTreeExplorer.Roots
            Dim found As Boolean = False
            For Each f As Data.File In listRootObjects
                If f Is file Then found = True
            Next
            If found = True Then Exit Sub
            objTreeExplorer.StartCellEdit(objTreeExplorer.SelectedItem, 0)
        End If
    End Sub
    Private Sub tStripExpFilesTrash_Click(sender As Object, e As EventArgs) Handles tStripExpFilesTrash.Click
        ExplorerRemoveFiles(False)
    End Sub
    Private Sub tStripExpFilesDelete_Click(sender As Object, e As EventArgs) Handles tStripExpFilesDelete.Click
        ExplorerRemoveFiles(True)
    End Sub
    Private Sub ExplorerTrash()
        Dim deleteFiles As String = "Moved to Trash: "
        Dim failedFiles As String = "Failed to Trash: "
        Dim successful As Boolean = False
        Dim parentList As New List(Of Object)
        Dim fileList As ArrayList = If(objTreeExplorer.InvokeRequired, (objTreeExplorer.Invoke(New MGetExplorerSelectedObjects(AddressOf GetExplorerSelectedObjects))), objTreeExplorer.SelectedObjects)
        For Each f As Google.Apis.Drive.v2.Data.File In fileList
            Dim user As GoogleUser = GetUserByEmail(f.OwnerNames(0))
            successful = FileManager.DeleteFile(user.DriveService, f.Id, False)
            If successful = True Then
                deleteFiles = deleteFiles & f.Title & ", "
                Dim obj As Object
                If objTreeExplorer.InvokeRequired Then
                    obj = objTreeExplorer.Invoke(New MGetExplorerObjectParent(AddressOf GetExplorerObjectParent), f)
                    objTreeExplorer.Invoke(New MRemoveExplorerObject(AddressOf RemoveExplorerObject), f)
                Else
                    obj = objTreeExplorer.GetParent(f)
                    objTreeExplorer.RemoveObject(f)
                End If
                If Not parentList.Contains(obj) Then parentList.Add(obj)
            Else
                failedFiles = failedFiles & f.Title & ", "
            End If
        Next
        For Each o As Object In parentList
            If objTreeExplorer.InvokeRequired Then
                objTreeExplorer.Invoke(New MRefreshExplorerObject(AddressOf RefreshExplorerObject), o)
            Else
                objTreeExplorer.RefreshObject(o)
            End If
        Next
        Dim lblText As String = ""
        If deleteFiles.EndsWith(", ") Then
            lblText = deleteFiles.Substring(0, deleteFiles.Length - 2)
        End If
        If failedFiles.EndsWith(", ") Then
            If Not lblText = "" Then
                lblText = lblText & "; " & failedFiles.Substring(0, failedFiles.Length - 1)
            Else
                lblText = failedFiles.Substring(0, failedFiles.Length - 1)
            End If
        End If
        If Not lblText = "" Then
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, lblText))
        Else
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "No files trashed."))
        End If
    End Sub
    Private Sub ExplorerDelete()
        Dim result As DialogResult = MessageBox.Show(Message_question_perm_delete, Message_question_Delete_file, MessageBoxButtons.YesNo)
        If result = Windows.Forms.DialogResult.Yes Then
            Dim deleteFiles As String = "Successfully Deleted: "
            Dim failedFiles As String = "Failed to Delete: "
            Dim successful As Boolean = False
            Dim parentList As New List(Of Object)
            Dim fileList As ArrayList = If(objTreeExplorer.InvokeRequired, (objTreeExplorer.Invoke(New MGetExplorerSelectedObjects(AddressOf GetExplorerSelectedObjects))), objTreeExplorer.SelectedObjects)
            For Each f As Drive.v2.Data.File In fileList
                Dim user As GoogleUser = GetUserByEmail(f.OwnerNames(0))
                successful = FileManager.DeleteFile(user.DriveService, f.Id, True)
                If successful = True Then
                    deleteFiles = deleteFiles & f.Title & ", "
                    Dim obj As Object
                    If objTreeExplorer.InvokeRequired Then
                        obj = objTreeExplorer.Invoke(New MGetExplorerObjectParent(AddressOf GetExplorerObjectParent), f)
                        objTreeExplorer.Invoke(New MRemoveExplorerObject(AddressOf RemoveExplorerObject), f)
                    Else
                        obj = objTreeExplorer.GetParent(f)
                        objTreeExplorer.RemoveObject(f)
                    End If
                    If Not parentList.Contains(obj) Then parentList.Add(obj)
                Else
                    failedFiles = failedFiles & f.Title & ", "
                End If
            Next
            For Each o As Object In parentList
                If objTreeExplorer.InvokeRequired Then
                    objTreeExplorer.Invoke(New MRefreshExplorerObject(AddressOf RefreshExplorerObject), o)
                Else
                    objTreeExplorer.RefreshObject(o)
                End If
            Next
            Dim lblText As String = ""
            If deleteFiles.EndsWith(", ") Then
                lblText = deleteFiles.Substring(0, deleteFiles.Length - 2)
            End If
            If failedFiles.EndsWith(", ") Then
                If Not lblText = "" Then
                    lblText = lblText & "; " & failedFiles.Substring(0, failedFiles.Length - 1)
                Else
                    lblText = failedFiles.Substring(0, failedFiles.Length - 1)
                End If
            End If
            If Not lblText = "" Then
                AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, lblText))
            Else
                AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "No files deleted."))
            End If
        Else
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Delete cancelled."))
        End If
    End Sub
    Private Sub ExplorerRemoveFiles(Optional skipTrash As Boolean = False)
        _bwFilesDelete = New BackgroundWorker
        AddHandler _bwFilesDelete.DoWork, AddressOf FilesDeleteStart
        AddHandler _bwFilesDelete.RunWorkerCompleted, AddressOf FilesDeleteFinish
        _bwFilesDelete.RunWorkerAsync(skipTrash)
    End Sub

    Private Sub toolStripItemQClearFinished_Click(sender As Object, e As EventArgs) Handles toolStripItemQClearFinished.Click
        ClearCompletedQueueItems()

    End Sub
    Private Sub ClearCompletedQueueItems()
        Try
            Dim qArray As New ArrayList(objListViewTransfers.Objects)
            For Each q As QueueItem In From q1 As QueueItem In qArray Where q1.Status = QueueItem.StatusEnum.Completed
                q.ClearQueueItem()
            Next
            qArray = Nothing
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Error clearing queue items."))
        End Try
    End Sub
    Private Sub numUpDownSimulDownloads_ValueChanged(sender As Object, e As EventArgs) Handles numUpDownSimulDownloads.ValueChanged
        AppOptions.SimultaneousDownloads = numUpDownSimulDownloads.Value
    End Sub

    Private Sub numUpDownSimulUploads_ValueChanged(sender As Object, e As EventArgs) Handles numUpDownSimulUploads.ValueChanged
        AppOptions.SimultaneousUploads = numUpDownSimulUploads.Value
    End Sub


    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowseDownloadLocation.Click
        Try
            AppOptions.SetDownloadLocation()
        Catch exF As FormatException
            MessageBox.Show(exF.Message)
            txtDefaultDownloadLocation.Text = AppOptions.DownloadLocation
        End Try
    End Sub
    Private Sub txtDefaultDownloadLocation_Leave(sender As Object, e As EventArgs) Handles txtDefaultDownloadLocation.Leave
        Try
            AppOptions.DownloadLocation = txtDefaultDownloadLocation.Text
        Catch exF As FormatException
            MessageBox.Show(exF.Message)
            txtDefaultDownloadLocation.Text = AppOptions.DownloadLocation
        End Try
    End Sub
    Private Sub tStripExpFilesRefresh_Click(sender As Object, e As EventArgs) Handles tStripExpFilesRefresh.Click
        Try
            Dim file As Data.File = objTreeExplorer.SelectedObjects(0)
            If FileManager.IsFileFolder(file) Then
                objTreeExplorer.RefreshObject(file)
            Else
                objTreeExplorer.RefreshObject(objTreeExplorer.GetParent(file))
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Error refreshing explorer items."))
        End Try
    End Sub
    Private Sub objTreeExplorer_CellToolTipShowing(sender As Object, e As ToolTipShowingEventArgs) Handles objTreeExplorer.CellToolTipShowing
        Dim file As Data.File = e.Model
        Dim lbls As String = ""
        If file.Labels.Hidden = True Then
            lbls = lbls & "Hidden, "
        End If
        If file.Labels.Restricted Then
            lbls = lbls & "Restricted, "
        End If
        If file.Labels.Starred Then
            lbls = lbls & "Starred, "
        End If
        If file.Labels.Trashed Then
            lbls = lbls & "Trashed, "
        End If
        If file.Labels.Viewed Then
            lbls = lbls & "Viewed, "
        End If
        lbls = lbls.Trim(", ")
        Dim toolTipText As String = "Title: " & vbTab & vbTab & vbTab & file.Title & vbNewLine & "Description: " & vbTab & vbTab & file.Description & vbNewLine & "Labels: " & vbTab & vbTab & vbTab & lbls & vbNewLine & "Created On: " & vbTab & vbTab & file.CreatedDate _
                                    & vbNewLine & "Last Modified By: " & vbTab & file.LastModifyingUserName & vbNewLine & "Last Viewed By Me: " & vbTab & file.LastViewedByMeDate _
                                    & vbNewLine & "Last Modified By Me: " & vbTab & file.ModifiedByMeDate & vbNewLine & "Shared With Me On: " & vbTab & file.SharedWithMeDate _
                                    & vbNewLine & "MD5 Checksum" & vbTab & file.Md5Checksum
        e.Text = toolTipText
    End Sub

    Private Sub tStripExpFilesStar_Click(sender As Object, e As EventArgs) Handles tStripExpFilesStar.Click
        StarSelectedFiles()
    End Sub
    Private Sub StarSelectedFiles()
        Try
            Dim tmpList As New List(Of Data.File)
            For Each f As Data.File In objTreeExplorer.SelectedObjects
                FileManager.ToggleStar(GetUserByEmail(f.OwnerNames(0)).DriveService, f)
                If FileManager.IsFileFolder(f) Then
                    If Not tmpList.Contains(f) Then
                        tmpList.Add(f)
                    End If
                Else
                    Dim file2 As Data.File = objTreeExplorer.GetParent(f)
                    If Not tmpList.Contains(file2) Then
                        tmpList.Add(file2)
                    End If
                End If
            Next
            objTreeExplorer.RefreshObjects(tmpList)
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Error ""Starring"" explorer items."))
        End Try
    End Sub
    Private Sub tStripExpFilesDownloadTo_Click(sender As Object, e As EventArgs) Handles tStripExpFilesDownloadTo.Click
        DownloadSelectedFilesToFolder()
    End Sub
    Private Sub DownloadSelectedFilesToFolder()
        Try
            Dim folderInfo As New FolderBrowserDialog
            folderInfo.Description = Message_Save_selected_files_to
            folderInfo.ShowNewFolderButton = True
            Dim dlgResult As DialogResult = folderInfo.ShowDialog()
            Dim saveToFolder As String = folderInfo.SelectedPath
            If dlgResult = Windows.Forms.DialogResult.OK Then
                For Each file As Drive.v2.Data.File In objTreeExplorer.SelectedObjects
                    Dim user As GoogleUser = GetUserByEmail(file.OwnerNames(0))
                    Dim localFile As New System.IO.FileInfo(saveToFolder & "\" & file.Title)
                    Dim dLoad As New Download(user, file, AppOptions, localFile)
                    objListViewTransfers.AddObject(dLoad)
                Next
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Error download explorer item(s) to selected folder."))
        End Try
    End Sub
    Private Sub DownloadSelectedFileAs()
        Try
            If objTreeExplorer.SelectedObjects.Count > 1 Then
                AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, "To save a file as a custom filename, please choose 1 file at a time."))
            Else
                Dim gFile As Data.File = objTreeExplorer.SelectedObjects(0)
                Dim fileDialog As New SaveFileDialog
                fileDialog.FileName = gFile.Title
                fileDialog.Title = "Save '" & gFile.Title & "' as:"
                fileDialog.OverwritePrompt = True
                Dim dlgResult As DialogResult = fileDialog.ShowDialog()
                If dlgResult = DialogResult.OK Then
                    Dim user As GoogleUser = GetUserByEmail(gFile.OwnerNames(0))
                    Dim localFile As New System.IO.FileInfo(fileDialog.FileName)
                    Dim dLoad As New Download(user, gFile, AppOptions, localFile)
                    dLoad.IgnoreFileExists = True
                    objListViewTransfers.AddObject(dLoad)
                End If
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Error downloading explorer item(s)."))
        End Try
    End Sub

    Private Sub tStripExpFilesDownloadAs_Click(sender As Object, e As EventArgs) Handles tStripExpFilesDownloadAs.Click
        DownloadSelectedFileAs()
    End Sub

    Private Sub cmbFileExistsDecision_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFileExistsDecision.SelectedIndexChanged
        Select Case cmbFileExistsDecision.SelectedIndex
            Case 0 'prompt user for decision
                AppOptions.IfFileExistsDecision = Options.FileExistsDecision.Prompt
            Case 1 'automatically overwrite
                AppOptions.IfFileExistsDecision = Options.FileExistsDecision.Overwrite
            Case 2 'automatically rename (ex: file(1).txt )
                AppOptions.IfFileExistsDecision = Options.FileExistsDecision.Rename
        End Select
    End Sub

    Private Sub numUpDownDLoadChunkSize_ValueChanged(sender As Object, e As EventArgs) Handles numUpDownDLoadChunkSize.ValueChanged
        AppOptions.SetDownloadBufferSizeKB(numUpDownDLoadChunkSize.Value)
    End Sub
    Private Sub numUpDownULoadChunkSize_ValueChanged(sender As Object, e As EventArgs) Handles numUpDownULoadChunkSize.ValueChanged
        AppOptions.SetUploadBufferSizeKB(numUpDownULoadChunkSize.Value)
    End Sub
    Private Sub numUpDownP2PChunkSize_ValueChanged(sender As Object, e As EventArgs) Handles numUpDownP2PChunkSize.ValueChanged
        AppOptions.SetP2PBufferSizeKB(numUpDownP2PChunkSize.Value)
    End Sub

    Private Sub tStripUsersButtonAddUser_Click(sender As Object, e As EventArgs) Handles tStripUsersButtonAddUser.Click
        Try
            If Not ApiFile.CheckApiKeyFileExists() Then
                Dim result As Integer = ChangeApiKeyFile.ShowDialog()
                If Not result = DialogResult.OK Or Not ApiFile.CheckApiKeyFileExists() Then
                    Exit Sub
                End If
            End If
            Dim dResult As DialogResult = MessageBox.Show("The application will now open your primary web browser to validate your Google account. Before continuing, logout of any active Google sessions (i.e. Gmail), unless that is the account you intend to add.", "Important Notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
            If dResult = DialogResult.OK Then
                Dim newUser As GoogleUser = CredentialsManager.CreateNewGUser()
                If newUser IsNot Nothing Then
                    newUser.InitiateLogin()
                End If
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to add a new user."))
        End Try
    End Sub
    Private Sub tStripUsersButtonRemoveUser_Click(sender As Object, e As EventArgs) Handles tStripUsersButtonRemoveUser.Click
        RemoveSelectedUsers()
    End Sub
    Private Sub RemoveSelectedUsers(Optional removeAllUsers As Boolean = False)
        Try
            Dim userList As ArrayList
            If removeAllUsers Then
                userList = olvUsers.Objects()
            Else
                userList = olvUsers.SelectedObjects()
            End If

            Dim emails As String = ""
            For Each u As GoogleUser In userList
                emails = emails & " " & u.Email & ","
            Next
            Dim dResult As DialogResult = MessageBox.Show(Message_Click_OK_to_remove_the_following_users & emails, Message_Remove_users_confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
            If dResult = Windows.Forms.DialogResult.OK Then
                For Each u As GoogleUser In userList
                    CredentialsManager.ClearClientCredentials(u.Email)
                Next
                olvUsers.Refresh()
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to remove the selected users."))
        End Try
    End Sub
    Private Sub chkBoxOptionsMD5Downloads_CheckedChanged(sender As Object, e As EventArgs) Handles chkBoxOptionsMD5Downloads.CheckedChanged
        Select Case chkBoxOptionsMD5Downloads.CheckState
            Case CheckState.Checked
                AppOptions.Md5CheckDLoad = True
            Case CheckState.Unchecked
                AppOptions.Md5CheckDLoad = False
        End Select
    End Sub
    Private Sub chkBoxOptionsMD5Uploads_CheckedChanged(sender As Object, e As EventArgs) Handles chkBoxOptionsMD5Uploads.CheckedChanged
        Select Case chkBoxOptionsMD5Uploads.CheckState
            Case CheckState.Checked
                AppOptions.Md5CheckULoad = True
            Case CheckState.Unchecked
                AppOptions.Md5CheckULoad = False
        End Select
    End Sub
    Private Sub chkBoxOptionsMD5P2P_CheckedChanged(sender As Object, e As EventArgs) Handles chkBoxOptionsMD5P2P.CheckedChanged
        Select Case chkBoxOptionsMD5P2P.CheckState
            Case CheckState.Checked
                AppOptions.Md5CheckP2P = True
            Case CheckState.Unchecked
                AppOptions.Md5CheckP2P = False
        End Select
    End Sub

    Private Sub SetupFsm()
        Try
            Dim tmpPath As String = _mTempPath & "\tmp_dont_delete.GMultiDrive"
            If Not IO.File.Exists(tmpPath) Then
                'IO.File.Create(tmpPath)
                Using wStream As New StreamWriter(tmpPath)
                    wStream.WriteLine(" ")
                End Using
            End If
            SetAttr(tmpPath, FileAttribute.Hidden + FileAttribute.System)

            Dim localDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives
            For i As Integer = 0 To localDrives.Length - 1
                If localDrives(i).DriveType = IO.DriveType.Fixed Then
                    If localDrives(i).IsReady Then
                        Dim fsm As IO.FileSystemWatcher = New IO.FileSystemWatcher(localDrives(i).RootDirectory.FullName, "*.GMultiDrive")

                        AddHandler fsm.Created, AddressOf ExplorerDragDrop
                        fsm.IncludeSubdirectories = True
                        fsm.EnableRaisingEvents = True
                    End If
                End If
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while setting up the file system watcher."))
        End Try
    End Sub
    Private Sub ExplorerDragDrop(sender As Object, e As IO.FileSystemEventArgs)
        Try
            Dim localDropPath As String = IO.Path.GetDirectoryName(e.FullPath)
            Dim selectedObjects As ArrayList = objTreeExplorer.Invoke((New MGetExplorerSelectedObjects(AddressOf GetExplorerSelectedObjects)))
            For Each file As Drive.v2.Data.File In selectedObjects
                Dim user As GoogleUser = GetUserByEmail(file.OwnerNames(0))
                Dim dLoad As New Download(user, file, AppOptions)
                dLoad.LocalFile = Nothing
                dLoad.LocalFile = New IO.FileInfo(localDropPath & "\" & dLoad.OriginalFileName)
                Dim q As QueueItem = dLoad
                objListViewTransfers.AddObject(dLoad)
            Next
            _mTempFileTimer = New System.Timers.Timer(1000)
            AddHandler _mTempFileTimer.Elapsed, AddressOf TempFileTimerElapsed
            _mDroppedTmpPath = localDropPath.Trim("""") & "\tmp_dont_delete.GMultiDrive"
            _mTempFileTimer.Enabled = True
            _mTempFileTimer.Start()
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to setup the dropped download(s)."))
        End Try
    End Sub
    Private Sub TempFileTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        Try
            _mTempFileTimer.Enabled = False
            If System.IO.File.Exists(_mDroppedTmpPath) Then
                My.Computer.FileSystem.DeleteFile(_mDroppedTmpPath)
            End If
        Catch ex As Exception
            _mTempFileTimer.Enabled = True
            _mTempFileTimer.Start()
        End Try
    End Sub

    Private Sub objTreeExplorer_ModelDropped(sender As Object, e As ModelDropEventArgs) Handles objTreeExplorer.ModelDropped
        Try
            If e.DataObject IsNot Nothing Then
                If e.DropTargetItem IsNot Nothing Then
                    ExplorerDropObject(sender, e)
                End If
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to setup the dropped file transfer."))
        End Try
    End Sub
    Private Sub objTreeExplorer_Dropped(sender As Object, e As OlvDropEventArgs) Handles objTreeExplorer.Dropped
        Try
            Try
                If e.DataObject.ModelObjects.Count > 0 Then
                    Exit Sub
                End If
            Catch ex As Exception

            End Try

            If e.DropTargetItem IsNot Nothing Then
                If e.DataObject IsNot Nothing Then
                    ExplorerDropFiles(sender, e)
                End If
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to setup the dropped file transfer."))
        End Try
    End Sub
    Private Sub ExplorerDropObject(sender As Object, e As OlvDropEventArgs)
        If e.DataObject.ModelObjects.Contains(e.DropTargetItem.RowObject) Then
            Exit Sub
        End If
        _bwExplorerObjectsDropped = New BackgroundWorker
        _bwExplorerObjectsDropped.WorkerSupportsCancellation = True
        _bwExplorerObjectsDropped.WorkerReportsProgress = True
        AddHandler _bwExplorerObjectsDropped.DoWork, AddressOf ExplorerObjectsDroppedStart
        AddHandler _bwExplorerObjectsDropped.RunWorkerCompleted, AddressOf ExplorerObjectsDroppedFinish
        _bwExplorerObjectsDropped.RunWorkerAsync(e)
    End Sub
    Private Sub ExplorerDropFiles(sender As Object, e As OlvDropEventArgs)
        _bwExplorerFilesDropped = New BackgroundWorker
        _bwExplorerFilesDropped.WorkerSupportsCancellation = True
        _bwExplorerFilesDropped.WorkerReportsProgress = True
        AddHandler _bwExplorerFilesDropped.DoWork, AddressOf ExplorerFilesDroppedStart
        AddHandler _bwExplorerFilesDropped.RunWorkerCompleted, AddressOf ExplorerFilesDroppedFinish
        _bwExplorerFilesDropped.RunWorkerAsync(e)
    End Sub

    Private Sub objTreeExplorer_CanDrop(sender As Object, e As OlvDropEventArgs) Handles objTreeExplorer.CanDrop
        Try
            If e.DataObject.ModelObjects.Count > 0 Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub objTreeExplorer_ModelCanDrop(sender As Object, e As OlvDropEventArgs) Handles objTreeExplorer.ModelCanDrop
        Try
            If e.DropSink.IsLeftMouseButtonDown Then
                Dim sourceFiles As ArrayList = e.DataObject.ModelObjects
                If sourceFiles.Contains(e.DropTargetItem.RowObject) Then
                    e.Effect = DragDropEffects.None
                Else
                    Dim targetFile As Data.File = e.DropTargetItem.RowObject
                    Dim errorFlag As Boolean = False
                    Dim p2pFlag As Boolean = False
                    Dim folderFlag As Boolean = False
                    For Each sourceFile As Data.File In sourceFiles
                        If objTreeExplorer.IsExpanded(sourceFile) Then
                            Dim sourceChildren As ArrayList = objTreeExplorer.GetChildren(sourceFile)

                            If sourceChildren IsNot Nothing Then
                                If sourceChildren.Contains(targetFile) Or objTreeExplorer.GetParent(sourceFile) Is targetFile Then
                                    errorFlag = True
                                End If
                            End If
                        End If
                        If objTreeExplorer.GetParent(sourceFile) Is targetFile Then errorFlag = True
                        If Not sourceFile.OwnerNames(0) = targetFile.OwnerNames(0) Then p2pFlag = True
                        If sourceFile.MimeType = "application/vnd.google-apps.folder" Then folderFlag = True
                    Next
                    If errorFlag Or (p2pFlag And folderFlag) Then
                        e.Effect = DragDropEffects.None
                    Else
                        e.Effect = DragDropEffects.Move
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub objTreeExplorer_DragEnter(sender As Object, e As DragEventArgs) Handles objTreeExplorer.DragEnter
        Try
            e.Effect = DragDropEffects.Copy
        Catch ex As Exception

        End Try
    End Sub

    Private Sub objTreeExplorer_DragLeave(sender As Object, e As EventArgs) Handles objTreeExplorer.DragLeave
        Try
            Dim rect As New Rectangle(objTreeExplorer.Location, New Size(objTreeExplorer.Width, objTreeExplorer.Height))
            If Not rect.Contains(PointToClient(Control.MousePosition)) Then
                If objTreeExplorer.SelectedObjects.Count > 0 Then
                    If _mDta Is Nothing Then
                        Dim fileas As String() = New [String](0) {}
                        fileas(0) = _mTempPath & "\tmp_dont_delete.GMultiDrive"
                        _mDta = New DataObject(DataFormats.FileDrop, fileas)
                        _mDta.SetData(DataFormats.StringFormat, fileas)
                    End If
                    DoDragDrop(_mDta, DragDropEffects.Copy)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tStripExpFilesNewFolder_Click(sender As Object, e As EventArgs) Handles tStripExpFilesNewFolder.Click
        CreateNewFolder()
    End Sub
    Private Sub CreateNewFolder()
        Try
            Dim parentFolder As Data.File = objTreeExplorer.SelectedObjects(0)
            Dim user As GoogleUser = GetUserByEmail(parentFolder.OwnerNames(0))
            If Not FileManager.IsFileFolder(parentFolder) Then
                parentFolder = objTreeExplorer.GetParent(parentFolder)
            End If
            Dim strFolderName As String = InputBox("Enter new folder name:", "Create New Folder")
            If FileManager.CreateFolder(user.DriveService, strFolderName, parentFolder.Id) Then
                AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Created new folder '" & strFolderName & "' successfully."))
                objTreeExplorer.RefreshObject(parentFolder)
            Else
                AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Failed to create folder '" & strFolderName & "'."))
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to create a new folder."))
        End Try
    End Sub


    Private Sub tStripMenuItemExpAddDownload_Click(sender As Object, e As EventArgs) Handles tStripMenuItemExpAddDownload.Click
        AddDownloadsToQueue()
    End Sub

    Private Sub tStripMenuItemExpAddDownloadTo_Click(sender As Object, e As EventArgs) Handles tStripMenuItemExpAddDownloadTo.Click
        DownloadSelectedFilesToFolder()
    End Sub

    Private Sub tStripMenuItemExpAddDownloadAs_Click(sender As Object, e As EventArgs) Handles tStripMenuItemExpAddDownloadAs.Click
        DownloadSelectedFileAs()
    End Sub

    Private Sub tStripBtnExpNewFolder_Click(sender As Object, e As EventArgs) Handles tStripBtnExpNewFolder.Click
        CreateNewFolder()
    End Sub

    Private Sub tStripBtnExpDelete_Click(sender As Object, e As EventArgs) Handles tStripBtnExpDelete.Click
        ExplorerRemoveFiles(True)
    End Sub

    Private Sub tStripBtnExpTrash_Click(sender As Object, e As EventArgs) Handles tStripBtnExpTrash.Click
        ExplorerRemoveFiles(False)
    End Sub

    Private Sub tStripBtnExpRefresh_Click(sender As Object, e As EventArgs) Handles tStripBtnExpRefresh.Click
        'RefreshExpandedExplorerObjects()
        objTreeExplorer.RefreshObjects(objTreeExplorer.Roots)
    End Sub
    Private Sub RefreshExpandedExplorerObjects()
        Try
            Dim fileList As List(Of File) = objTreeExplorer.ExpandedObjects.Cast(Of File)().ToList()
            objTreeExplorer.RefreshObjects(fileList)
            objTreeExplorer.Refresh()
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to refresh the explorer items."))
        End Try
    End Sub
    Private Sub tStripBtnExpStar_Click(sender As Object, e As EventArgs) Handles tStripBtnExpStar.Click
        StarSelectedFiles()
    End Sub

    Private Sub tStripBtnExpOpen_Click(sender As Object, e As EventArgs) Handles tStripBtnExpOpen.Click
        OpenSelectedFiles()
    End Sub

    Private Sub toolStripItemQOpenDir_Click(sender As Object, e As EventArgs) Handles toolStripItemQOpenDir.Click
        Try
            For Each qItem As QueueItem In objListViewTransfers.SelectedObjects
                If qItem.TransferType = QueueItem.QueueType.Download Then
                    Process.Start("explorer.exe", qItem.TransferLocation)
                End If
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Queue, DateTime.Now, "An error occurred while attempting to open the requested local folder."))
        End Try
    End Sub

    Private Sub tStripBtnQueueStart_Click(sender As Object, e As EventArgs) Handles tStripBtnQueueStart.Click
        StartResumeSelectedQueueItems()
    End Sub

    Private Sub tStripBtnQueuePause_Click(sender As Object, e As EventArgs) Handles tStripBtnQueuePause.Click
        PauseSelectedQueueItems()
    End Sub

    Private Sub tStripMenuItemQueueClearCompleted_Click(sender As Object, e As EventArgs) Handles tStripMenuItemQueueClearCompleted.Click
        ClearCompletedQueueItems()
    End Sub

    Private Sub tStripMenuItemQueueClearSelected_Click(sender As Object, e As EventArgs) Handles tStripMenuItemQueueClearSelected.Click
        ClearSelectedQueueItems()
    End Sub

    Private Sub ClearInactiveQueueItems()
        Try
            Dim qItemsToClear As New List(Of QueueItem)
            For Each qItem As QueueItem In objListViewTransfers.Objects
                If qItem.Status = QueueItem.StatusEnum.Cancelled Or qItem.Status = QueueItem.StatusEnum.Failed Or qItem.Status = QueueItem.StatusEnum.Completed Then
                    qItemsToClear.Add(qItem)
                    'qItem.ClearQueueItem()
                End If
            Next
            For Each qItem As QueueItem In qItemsToClear
                qItem.ClearQueueItem()
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while clearing the inactive queue items."))
        End Try
    End Sub

    Private Sub tStripMenuItemQueueClearInactive_Click(sender As Object, e As EventArgs) Handles tStripMenuItemQueueClearInactive.Click
        ClearInactiveQueueItems()
    End Sub

    Private Sub tStripBtnQueueCancel_Click(sender As Object, e As EventArgs) Handles tStripBtnQueueCancel.Click
        CancelSelecetedQueueItems()
    End Sub

    Private Sub toolStripItemQTransferCancel_Click(sender As Object, e As EventArgs) Handles toolStripItemQTransferCancel.Click
        CancelSelecetedQueueItems()
    End Sub
    Private Sub CancelSelecetedQueueItems()
        Try
            For Each qItem As QueueItem In objListViewTransfers.SelectedObjects
                qItem.TransferCancel()
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while cancelling the selected queue items."))
        End Try
    End Sub

    Private Sub tStripUsersButtonRemoveAllUsers_Click(sender As Object, e As EventArgs) Handles tStripUsersButtonRemoveAllUsers.Click
        RemoveSelectedUsers(True)
    End Sub

    Private Sub cmbTrashed_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTrashed.SelectedIndexChanged
        Try
            Select Case cmbTrashed.SelectedIndex
                Case 0
                    AppOptions.ShowTrashed = Options.TrashDecision.Highlight
                Case 1
                    AppOptions.ShowTrashed = Options.TrashDecision.Show
                Case 2
                    AppOptions.ShowTrashed = Options.TrashDecision.Hide
            End Select
            objTreeExplorer.Refresh()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub objTreeExplorer_FormatRow(sender As Object, e As BrightIdeasSoftware.FormatRowEventArgs) Handles objTreeExplorer.FormatRow
        Try
            If AppOptions.ShowTrashed = Options.TrashDecision.Highlight Then
                Dim file As Data.File = e.Model
                If file.Labels.Trashed Then
                    e.Item.ForeColor = Color.Red
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tStripExpFilesEditDescription_Click(sender As Object, e As EventArgs) Handles tStripExpFilesEditDescription.Click
        Try
            If objTreeExplorer.SelectedObjects.Count > 0 Then
                Dim description As String
                If objTreeExplorer.SelectedObjects.Count = 1 Then
                    description = InputBox("Enter the new file(s) description:", "Edit File Description", CType(objTreeExplorer.SelectedObjects(0), Data.File).Description)
                Else
                    description = InputBox("Enter the new file(s) description:", "Edit File Description")
                End If
                For Each file As Data.File In objTreeExplorer.SelectedObjects
                    Dim user As GoogleUser = GetUserByEmail(file.OwnerNames(0))
                    'If Not description = "" Then
                    FileManager.EditDescription(user.DriveService, file, description)
                    'End If
                    objTreeExplorer.RefreshObject(objTreeExplorer.GetParent(file))
                Next
            End If
        Catch ex As Google.GoogleApiRequestException
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "Error editing description."))
        End Try
    End Sub

    Private Sub objTreeExplorer_FormatCell(sender As Object, e As FormatCellEventArgs) Handles objTreeExplorer.FormatCell
        Try
            If e.ColumnIndex = 0 Then
                Dim file As Data.File = e.Model
                If file.Labels.Trashed Then
                    Dim decoration As New TextDecoration("Trashed!", 255)
                    decoration.Alignment = ContentAlignment.MiddleLeft
                    decoration.Font = New Font(objListViewTransfers.Font, objTreeExplorer.Font.SizeInPoints + 2)
                    decoration.TextColor = Color.Firebrick
                    decoration.Rotation = -20
                    e.SubItem.Decoration = decoration
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadFormState()
        Dim ptLocation As Point = My.Settings.WindowLocation
        If (ptLocation.X = -1) Or (ptLocation.Y = -1) Then Exit Sub
            Dim bLocationVisible As Boolean = False
            For Each s As Screen In Screen.AllScreens
                If s.Bounds.Contains(ptLocation) Then
                    bLocationVisible = True
                    Exit For
                End If
            Next
        If Not bLocationVisible Then Exit Sub
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = ptLocation
        Me.Size = My.Settings.WindowSize
        splitContainerMain.SplitterDistance = My.Settings.TopSplitterDistance
        splitContainerLower.SplitterDistance = My.Settings.BottomSplitterDistance
    End Sub
    Private Sub SaveFormState()
        My.Settings.WindowLocation = Me.Location
        My.Settings.WindowSize = Me.Size
        My.Settings.TopSplitterDistance = splitContainerMain.SplitterDistance
        My.Settings.BottomSplitterDistance = splitContainerLower.SplitterDistance
    End Sub
    Private Sub SaveOptions()
        Try
            If System.IO.File.Exists(MSettingsFilePath) Then
                System.IO.File.Delete(MSettingsFilePath)
            End If
            Using objStreamWriter As New StreamWriter(MSettingsFilePath)
                Dim x As New XmlSerializer(AppOptions.GetType)
                x.Serialize(objStreamWriter, AppOptions)
            End Using
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while saving the current settings."))
        End Try
    End Sub
    Private Sub LoadOptions()
        Try
            If System.IO.File.Exists(MSettingsFilePath) Then
                Using objStreamReader As New StreamReader(MSettingsFilePath)
                    Dim x As New XmlSerializer(GetType(Options))
                    MAppOptions = New Options()
                    MAppOptions = x.Deserialize(objStreamReader)
                End Using
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while loading the previously saved options."))
        End Try
    End Sub
    Private Sub SaveExplorerState()
        Try
            Dim filePath As String = _mTempPath & "\explorer_state.xml"
            If System.IO.File.Exists(filePath) Then
                System.IO.File.Delete(filePath)
            End If
            Using objStreamWriter As Stream = System.IO.File.OpenWrite(filePath)
                Dim b() As Byte = objTreeExplorer.SaveState()
                objStreamWriter.Write(b, 0, b.Length)
            End Using
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while saving the explorer's current state."))
        End Try
    End Sub
    Private Sub RestoreExplorerState()
        Try
            Dim filePath As String = _mTempPath & "\explorer_state.xml"
            If System.IO.File.Exists(filePath) Then
                Dim b() As Byte = System.IO.File.ReadAllBytes(filePath)
                objTreeExplorer.RestoreState(b)
                b = Nothing
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while restoring the explorer's previous state."))
        End Try
    End Sub
    Private Sub SaveQueueState()
        Try
            Dim filePath As String = _mTempPath & "\queue_state.xml"
            If System.IO.File.Exists(filePath) Then
                System.IO.File.Delete(filePath)
            End If
            Using objStreamWriter As Stream = System.IO.File.OpenWrite(filePath)
                Dim b() As Byte = objListViewTransfers.SaveState()
                objStreamWriter.Write(b, 0, b.Length)
            End Using
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while saving the queue's current state."))
        End Try
    End Sub
    Private Sub RestoreQueueState()
        Try
            Dim filePath As String = _mTempPath & "\queue_state.xml"
            If System.IO.File.Exists(filePath) Then
                Dim b() As Byte = System.IO.File.ReadAllBytes(filePath)
                objListViewTransfers.RestoreState(b)
                b = Nothing
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while restoring the queue's previous state."))
        End Try
    End Sub
    Private Sub SaveUsersState()
        Try
            Dim filePath As String = _mTempPath & "\users_state.xml"
            If System.IO.File.Exists(filePath) Then
                System.IO.File.Delete(filePath)
            End If
            Using objStreamWriter As Stream = System.IO.File.OpenWrite(filePath)
                Dim b() As Byte = olvUsers.SaveState()
                objStreamWriter.Write(b, 0, b.Length)
            End Using
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while saving the user list's current state."))
        End Try
    End Sub
    Private Sub RestoreUsersState()
        Try
            Dim filePath As String = _mTempPath & "\users_state.xml"
            If System.IO.File.Exists(filePath) Then
                Dim b() As Byte = System.IO.File.ReadAllBytes(filePath)
                olvUsers.RestoreState(b)
                b = Nothing
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while restoring the user list's previous state."))
        End Try
    End Sub
    Private Sub tStripBtnExpAbout_Click(sender As Object, e As EventArgs) Handles tStripBtnExpAbout.Click
        AboutBox.ShowDialog()
    End Sub

    Private Sub btnResetToDefaults_Click(sender As Object, e As EventArgs) Handles btnResetToDefaults.Click
        AppOptions.ResetToDefaults()
        PopulateOptions()
    End Sub

    Private Sub toolStripItemQTransferPause_Click(sender As Object, e As EventArgs) Handles toolStripItemQTransferPause.Click
        PauseSelectedQueueItems()
    End Sub
    Private Sub PauseSelectedQueueItems()
        Try
            For Each obj As Object In objListViewTransfers.SelectedObjects
                Dim qItem As QueueItem = CType(obj, QueueItem)
                qItem.TransferPause()
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while pausing the selected queue items."))
        End Try
    End Sub
    Private Sub toolStripItemQTransferResume_Click(sender As Object, e As EventArgs) Handles toolStripItemQTransferResume.Click
        StartResumeSelectedQueueItems()

    End Sub
    Private Sub StartResumeSelectedQueueItems()
        Try
            For Each obj As Object In objListViewTransfers.SelectedObjects
                Dim qItem As QueueItem = CType(obj, QueueItem)
                qItem.TransferStart()
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while starting the selected queue items."))
        End Try
    End Sub
    Private Sub toolStripItemQClear_Click(sender As Object, e As EventArgs) Handles toolStripItemQClear.Click
        ClearSelectedQueueItems()
    End Sub
    Private Sub ClearSelectedQueueItems()
        Try
            For Each qItem As QueueItem In objListViewTransfers.SelectedObjects
                qItem.ClearQueueItem()
            Next
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while clearing the selected queue items."))
        End Try
    End Sub

    Private Sub contextMenuExplorerFiles_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs) Handles contextMenuExplorerFiles.Closing
        tStripExpFilesDelete.Enabled = True
        tStripExpFilesRename.Enabled = True
        tStripExpFilesDownload.Enabled = True
        tStripExpFilesDownloadAs.Enabled = True
        tStripExpFilesDownloadTo.Enabled = True
        tStripExpFilesEditDescription.Enabled = True
        tStripExpFilesNewFolder.Enabled = True
        tStripExpFilesOpen.Enabled = True
        tStripExpFilesRefresh.Enabled = True
        tStripExpFilesStar.Enabled = True
        tStripExpFilesTrash.Enabled = True
    End Sub

    Private Sub contextMenuExplorerFiles_Opening(sender As Object, e As CancelEventArgs) Handles contextMenuExplorerFiles.Opening
        Dim selectedObjects As ArrayList = objTreeExplorer.SelectedObjects
        Dim folderFlag As Boolean = False
        For Each f As File In From f1 As File In selectedObjects Where f1.MimeType = "application/vnd.google-apps.folder"
            folderFlag = True
        Next
        If folderFlag Then
            tStripExpFilesDownload.Enabled = False
            tStripExpFilesDownloadTo.Enabled = False
            tStripExpFilesDownloadAs.Enabled = False
        End If
        If selectedObjects.Count > 1 Then
            tStripExpFilesRename.Enabled = False
            tStripExpFilesNewFolder.Enabled = False
            tStripExpFilesRefresh.Enabled = False
        End If
    End Sub

    Private Sub btnResetApi_Click(sender As Object, e As EventArgs) Handles btnResetApi.Click
        Dim result As Integer = ChangeApiKeyFile.ShowDialog()
    End Sub
End Class