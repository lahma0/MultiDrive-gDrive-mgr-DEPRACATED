Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports Google.Apis
Imports Google.Apis.Authentication.OAuth2
Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth

Public Class Download
    Inherits QueueItem
    Private _mDownloadFile As Drive.v2.Data.File
    Private ReadOnly _mDownloadBufferSize As Integer
    Private _mLocalFile As FileInfo
    Private _mIgnoreFileExists As Boolean = False
    Private ReadOnly _mAppOptions As Options

    Public Property IgnoreFileExists As Boolean
        Get
            Return _mIgnoreFileExists
        End Get
        Set(value As Boolean)
            _mIgnoreFileExists = value
        End Set
    End Property
    Public Property LocalFile As FileInfo
        Get
            If _mLocalFile Is Nothing Then
                Return Nothing
            End If
            Return _mLocalFile
        End Get
        Set(value As FileInfo)
            _mLocalFile = value
        End Set
    End Property
    Public Overrides ReadOnly Property FileName As String
        Get
            Return _mLocalFile.Name
        End Get
    End Property
    Public ReadOnly Property OriginalFileName As String
        Get
            Return _mFileName
        End Get
    End Property
    Private Property DownloadFile As Drive.v2.Data.File
        Get
            Return _mDownloadFile
        End Get
        Set(value As Drive.v2.Data.File)
            _mDownloadFile = value
        End Set
    End Property
    Private ReadOnly Property ResumePosition As Long
        Get
            Return _mBytesTransferred - ((8 * 1024) - 1)
        End Get
    End Property
    Public ReadOnly Property FullFilePath As String
        Get
            Return _mLocalFile.FullName
        End Get
    End Property
    Public Overrides ReadOnly Property TransferLocation As String
        Get
            Return _mLocalFile.Directory.ToString
        End Get
    End Property
    Private Sub CopyStream(input As Stream, output As Stream, Optional isResuming As Boolean = False)
            Dim buffer As Byte() = New Byte(_mDownloadBufferSize - 1) {}
            Dim len As Integer
            Dim dblProgress As Double
            Dim i As Integer = 0
            If isResuming = True And _mBytesTransferred > 0 Then
                output.Seek(ResumePosition, SeekOrigin.Begin)
                _mBytesTransferred = ResumePosition
            End If
            While (InputRead(input, buffer, len) > 0)
                output.Write(buffer, 0, len)
                _mBytesTransferred += len
                dblProgress = (_mBytesTransferred / TransferSize) * 100
                _mProgress = Convert.ToInt32(dblProgress)
                If i = 100 Then
                    _bwTransfer.ReportProgress(_mProgress)
                    i = 0
                    If _bwTransfer.CancellationPending Then
                        If _mStatus = StatusEnum.Cancelled Then
                            _mBytesTransferred = 0
                        End If
                        Exit Sub
                    End If

                End If
                i += 1
            End While
            _mProgress = 100
    End Sub
    Private Function InputRead(input As Stream, ByRef buffer As Byte(), ByRef len As Integer) As Integer
        len = input.Read(buffer, 0, buffer.Length)
        Return len
    End Function
    Private Function AuthenticateRequest(Optional isResuming As Boolean = False) As HttpWebRequest
        Dim authenticator As OAuth2Authenticator(Of NativeApplicationClient) = _mUser.Authenticator
        Dim downloadUri As New Uri(_mDownloadFile.DownloadUrl)
        Dim myHttpWebRequest As HttpWebRequest = WebRequest.Create(downloadUri)
        If isResuming = True Then myHttpWebRequest.AddRange(ResumePosition)
        authenticator.ApplyAuthenticationToRequest(myHttpWebRequest)
        Return myHttpWebRequest
    End Function
    Private Function StaShowDialog(dialog As FileDialog) As DialogResult
        Dim state As DialogState = New DialogState()
        state.dialog = dialog
        Dim t As System.Threading.Thread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf state.ThreadProcsShowDialog))
        t.SetApartmentState(System.Threading.ApartmentState.STA)
        t.Start()
        t.Join()
        Return state.result
    End Function

    Private Sub CheckResolveFileExists()
        If System.IO.File.Exists(_mLocalFile.FullName) Then
            If _mIgnoreFileExists Then
                File.Delete(_mLocalFile.FullName)
            Else
                Select Case _mAppOptions.IfFileExistsDecision
                    Case Options.FileExistsDecision.Prompt
                        Dim frm As New SaveFileDialog
                        frm.InitializeLifetimeService()
                        frm.FileName = FileName
                        frm.Title = "'" & FileName & "' already exists. Overwrite or rename:"
                        frm.OverwritePrompt = True
                        Dim dlgResult As DialogResult = StaShowDialog(frm)
                        If dlgResult = DialogResult.OK Then
                            _mLocalFile = Nothing
                            _mLocalFile = New FileInfo(frm.FileName)
                            UpdateLocalInfo(_mLocalFile)
                            If File.Exists(_mLocalFile.FullName) Then File.Delete(_mLocalFile.FullName)
                        Else
                            If Not _bwTransfer Is Nothing Then
                                If Not _bwTransfer.CancellationPending Then
                                    _bwTransfer.CancelAsync()
                                End If
                            End If
                        End If
                    Case Options.FileExistsDecision.Overwrite
                        File.Delete(_mLocalFile.FullName)
                    Case Options.FileExistsDecision.Rename
                        Dim fileExists As Boolean = True
                        Dim i As Integer = 1
                        Dim copyFileName As String
                        Do While fileExists = True

                            Dim fileMinusExt As String = System.IO.Path.GetFileNameWithoutExtension(_mLocalFile.FullName)
                            copyFileName = String.Format("{0}({1}){2}", fileMinusExt, i.ToString, _mLocalFile.Extension)
                            fileExists = File.Exists(copyFileName)
                            i += 1
                        Loop
                        _mLocalFile = Nothing
                        _mLocalFile = New FileInfo(copyFileName)
                End Select
            End If
        End If
    End Sub
    Private Sub UpdateLocalInfo(newFileInfo As FileInfo)
        _mLocalFile = newFileInfo
    End Sub
    Friend Overrides Sub BackgroundTransferStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        UpdateStatus(StatusEnum.Transferring)
        'MainForm.ActiveDownloads += 1
        Dim authenticatedRequest As HttpWebRequest = AuthenticateRequest(e.Argument)
        Using response As HttpWebResponse = authenticatedRequest.GetResponse
            If response.StatusCode = HttpStatusCode.OK Or response.StatusCode = HttpStatusCode.PartialContent Then
                Try
                    Using rStream As Stream = response.GetResponseStream
                        If Not e.Argument Then
                            CheckResolveFileExists()
                            If _bwTransfer.CancellationPending Then
                                e.Result = StatusEnum.Cancelled
                                _mStatus = StatusEnum.Cancelled
                                Exit Sub
                            End If
                        End If
                        Using fStream As Stream = System.IO.File.OpenWrite(_mLocalFile.FullName)
                            'rStream.ReadTimeout = _mReadTimeOutMs
                            CopyStream(rStream, fStream, e.Argument)
                        End Using
                    End Using
                Catch exH As HttpRequestException
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to download " & _mFileName))
                Catch ex As Exception
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to download " & _mFileName))
                Finally
                    If _mProgress = 100 Then
                        e.Result = StatusEnum.Completed
                    ElseIf _mProgress > 0 And _mStatus = StatusEnum.Paused Then
                        e.Result = StatusEnum.Paused
                    ElseIf _mProgress > 0 Then
                        e.Result = StatusEnum.Interrupted
                    Else
                        e.Result = StatusEnum.Failed
                    End If
                End Try
            End If
        End Using
    End Sub
    Friend Overrides Sub BackgroundTransferFinished(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        MainForm.ActiveDownloads -= 1
        RemoveBwHandlers()
        If e.Result Is Nothing Then
            _mTransferError = e.Error
            If _mProgress > 0 Then
                UpdateStatus(StatusEnum.Interrupted)
            Else
                UpdateStatus(StatusEnum.Failed)
            End If
        ElseIf e.Result = StatusEnum.Completed Then
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, FileName & " downloaded successfully."))
            If _mAppOptions.Md5CheckDLoad Then
                If FileManager.CompareMd5(_mDownloadFile, _mLocalFile.FullName) Then
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, FileName & " MD5 check successful."))
                Else
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, FileName & " MD5 check failed."))
                End If
            End If
            UpdateStatus(StatusEnum.Completed)
        ElseIf e.Result = StatusEnum.Paused Then
            If _mStatus = StatusEnum.Cancelled Then
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, FileName & " was cancelled"))
            ElseIf _mStatus = StatusEnum.Paused Then
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, FileName & " paused."))
            End If
        ElseIf e.Result = StatusEnum.Interrupted Then
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, FileName & " was interrupted."))
        ElseIf e.Result = StatusEnum.Failed Then
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, FileName & " failed to download."))
        End If
        _bwTransfer.Dispose()
        UpdateListViewItem()
        SortListViewItem()
        MainForm.RunDownloadQueue()
        If _markedForDeletion Then
            MainForm.objListViewTransfers.RemoveObject(Me)
        End If
    End Sub

    Public Sub New(user As GoogleUser, downloadFile As Drive.v2.Data.File, appOptions As Options, Optional localTargetFile As FileInfo = Nothing)
        MyBase.New(QueueType.Download)
        _mAppOptions = appOptions
        If localTargetFile Is Nothing Then
            _mLocalFile = New FileInfo(MainForm.AppOptions.DownloadLocation & "\" & downloadFile.Title)
        Else
            _mLocalFile = localTargetFile
        End If
        _mReadTimeOutMs = MainForm.AppOptions.ReadTimeOutMs
        _mDownloadBufferSize = MainForm.AppOptions.DownloadBufferSize
        _mDownloadFile = downloadFile
        _mFileName = _mDownloadFile.Title
        _mUser = user
        _mTransferSize = Convert.ToInt64(_mDownloadFile.FileSize)
        UpdateStatus(StatusEnum.Queued)
    End Sub
End Class
Public Class DialogState
    Public result As DialogResult
    Public dialog As FileDialog

    Public Sub ThreadProcsShowDialog()
        result = dialog.ShowDialog()
    End Sub
End Class