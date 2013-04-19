Imports System.IO
Imports System.Net
Imports Google.Apis.Drive.v2
Imports Google.Apis.Drive.v2.Data
Imports System.Web.Script.Serialization

Public Class Upload
    Inherits QueueItem
    Private ReadOnly _mUploadFile As Data.File
    Private ReadOnly _mUploadFilePath As String
    Private ReadOnly _mParentRowObject As Data.File
    Private ReadOnly _mUploadBufferSize As Integer
    Private _mResumableUri As String
    Private _mNewFile As Data.File

    Public ReadOnly Property ParentRowObject As Data.File
        Get
            Return _mParentRowObject
        End Get
    End Property
    Public ReadOnly Property UploadFilePath As String
        Get
            Return _mUploadFilePath
        End Get
    End Property
    Public ReadOnly Property UploadFile As Data.File
        Get
            Return _mUploadFile
        End Get
    End Property

    Public Sub CopyStream(targetStream As Stream, sourceStream As Stream, Optional isResuming As Boolean = False)
        Dim buffer As Byte() = New Byte(_mUploadBufferSize - 1) {}
        Dim len As Integer
        If isResuming = True And _mBytesTransferred > 0 Then
            sourceStream.Seek(_mBytesTransferred + 1, SeekOrigin.Begin)
        End If
        While (InputRead(sourceStream, buffer, len) > 0)
            targetStream.Write(buffer, 0, len)
            _mBytesTransferred += len
            _mProgress = Convert.ToInt32((_mBytesTransferred / TransferSize) * 100)
            _bwTransfer.ReportProgress(_mProgress)
            If _bwTransfer.CancellationPending Then
                If _mStatus = StatusEnum.Cancelled Then
                    _mBytesTransferred = 0
                End If
                Exit Sub
            End If
        End While
        _mProgress = 100
    End Sub
    Private Function InputRead(input As Stream, ByRef buffer As Byte(), ByRef len As Integer) As Integer
        len = input.Read(buffer, 0, buffer.Length)
        Return len
    End Function
    Private Function BuildRequestBody(title As String, parentId As String) As String
        Return "{  ""kind"": ""drive#file"", ""title"": """ & title & """, ""parents"": [   {    ""kind"": ""drive#parentReference"",    ""id"": """ & parentId & """   }  ]}"
    End Function
    Private Function GetRangeHeader(resumeableUri As String) As Long
        Dim myUploadWebRequest As HttpWebRequest = WebRequest.Create(resumeableUri)
        myUploadWebRequest.Method = "PUT"
        myUploadWebRequest.ContentLength = 0
        myUploadWebRequest.Headers.Add("Content-Range", String.Format(" bytes */{0}", _mTransferSize))
        _mUser.Authenticator.ApplyAuthenticationToRequest(myUploadWebRequest)
        Try
            Using uploadResponse As HttpWebResponse = myUploadWebRequest.GetResponse
                Dim tmp As String = uploadResponse.GetResponseHeader("Range")
                If uploadResponse.StatusCode = 308 Then
                    Dim range As String = uploadResponse.Headers.GetKey("Range")
                End If
                Return 0
            End Using
        Catch ex As WebException
            Dim uploadResponse As HttpWebResponse = CType(ex.Response, HttpWebResponse)
            If uploadResponse.StatusCode = 308 Then
                Dim rHeader As String = uploadResponse.GetResponseHeader("Range")
                If rHeader.Contains("-") Then
                    Dim rHeaderFinal As String = rHeader.Split("-")(1)
                    Return Convert.ToInt64(rHeaderFinal)
                End If
            End If
            Return 0
        End Try
    End Function
    Private Function RetrieveUploadWebRequest(resumableUri As String, Optional isResuming As Boolean = False) As HttpWebRequest
        Dim myUploadWebRequest As HttpWebRequest = WebRequest.Create(resumableUri)
        myUploadWebRequest.Method = "PUT"
        If IsResuming And _mBytesTransferred > 0 Then
            myUploadWebRequest.ContentLength = _mTransferSize - (_mBytesTransferred + 1)
        Else
            myUploadWebRequest.ContentLength = _mTransferSize
        End If

        myUploadWebRequest.ContentType = _mMimeType
        If IsResuming Then
            myUploadWebRequest.Headers.Add("Content-Range", String.Format(" bytes {0}-{1}/{2}", _mBytesTransferred + 1, _mTransferSize - 1, _mTransferSize))
        End If
        _mUser.Authenticator.ApplyAuthenticationToRequest(myUploadWebRequest)
        Return myUploadWebRequest
    End Function
    Friend Overrides Sub BackgroundTransferStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            UpdateStatus(StatusEnum.Transferring)
            MainForm.ActiveUploads += 1
            Dim isResuming As Boolean = e.Argument
            If Not IsResuming Then
                Dim uploadUri As New Uri("https://www.googleapis.com/upload/drive/v2/files?uploadType=resumable")
                Dim requestBody As String = BuildRequestBody(_mFileName, _mParentRowObject.Id)
                Dim myHttpWebRequest As HttpWebRequest = WebRequest.Create(uploadUri)
                myHttpWebRequest.Method = "POST"
                myHttpWebRequest.ContentLength = requestBody.Length
                myHttpWebRequest.ContentType = "application/json; charset=UTF-8"
                myHttpWebRequest.Headers.Add("X-Upload-Content-Type", _mMimeType)
                myHttpWebRequest.Headers.Add("X-Upload-Content-Length", _mTransferSize.ToString)
                _mUser.Authenticator.ApplyAuthenticationToRequest(myHttpWebRequest)
                Using sw As New StreamWriter(myHttpWebRequest.GetRequestStream)
                    sw.Write(requestBody)
                End Using
                Using myHttpWebResponse As HttpWebResponse = myHttpWebRequest.GetResponse()
                    If myHttpWebResponse.StatusCode = HttpStatusCode.OK Then
                        _mResumableUri = myHttpWebResponse.GetResponseHeader("Location")
                    End If
                End Using
            End If
            If IsResuming Then
                _mBytesTransferred = GetRangeHeader(_mResumableUri)
            End If
            Using fStream As FileStream = System.IO.File.OpenRead(_mUploadFilePath)
                Dim myUploadRequest As HttpWebRequest = RetrieveUploadWebRequest(_mResumableUri, IsResuming)
                Using uploadStream As Stream = myUploadRequest.GetRequestStream
                    uploadStream.WriteTimeout = _mWriteTimeOutMs
                    CopyStream(uploadStream, fStream, isResuming)
                End Using
                Using myHttpWebResponse As HttpWebResponse = myUploadRequest.GetResponse()
                    Using respStream As New StreamReader(myHttpWebResponse.GetResponseStream())
                        Dim js As New JavaScriptSerializer
                        Dim objText = respStream.ReadToEnd()
                        _mNewFile = js.Deserialize(objText, GetType(Data.File))
                    End Using
                    e.Result = myHttpWebResponse.StatusCode
                End Using
            End Using
        Catch exWeb As WebException
            e.Result = GetUploadResultAlternate()
        Catch ex As Exception
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to upload " & _mFileName))
        End Try
    End Sub
    Private Function GetUploadResult(uploadWebResponse As HttpStatusCode) As StatusEnum
        Select Case uploadWebResponse
            Case 201, 200
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " uploaded successfully."))
                If MainForm.AppOptions.Md5CheckULoad Then
                    If FileManager.CompareMd5(_mNewFile, _mUploadFilePath) Then
                        MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " MD5 check successful."))
                    Else
                        MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " MD5 check failed."))
                    End If
                End If
                Return StatusEnum.Completed
            Case 308
                If _mStatus = StatusEnum.Paused And _mProgress > 0 Then
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " paused."))
                    Return StatusEnum.Paused
                ElseIf _mStatus = StatusEnum.Cancelled Then
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " was cancelled"))
                    Return StatusEnum.Cancelled
                ElseIf _mProgress > 0 Then
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " was interrupted."))
                    Return StatusEnum.Interrupted
                Else
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " failed to upload."))
                    Return StatusEnum.Failed
                End If
            Case Else
                If _mProgress > 0 Then
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " was interrupted."))
                    Return StatusEnum.Interrupted
                Else
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " failed to upload."))
                    Return StatusEnum.Failed
                End If
        End Select
    End Function
    Private Function GetUploadResultAlternate() As StatusEnum
        If _mProgress = 100 Then
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " uploaded successfully."))
            Return StatusEnum.Completed
        ElseIf _mProgress > 0 Then
            If _mStatus = StatusEnum.Paused Then
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " paused."))
                Return StatusEnum.Paused
            ElseIf _mStatus = StatusEnum.Cancelled Then
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " was cancelled"))
                Return StatusEnum.Cancelled
            Else
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " was interrupted."))
                Return StatusEnum.Interrupted
            End If
        Else
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " failed to upload."))
            Return StatusEnum.Failed
        End If
    End Function
    Friend Overrides Sub BackgroundTransferFinished(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        MainForm.ActiveUploads -= 1
        RemoveBwHandlers()
        MainForm.objTreeExplorer.RefreshObject(_mParentRowObject)
        If Not e.Result Is Nothing Then
            UpdateStatus(GetUploadResult(e.Result))
            MainForm.objListViewTransfers.RefreshObject(Me)
        Else
            UpdateStatus(GetUploadResultAlternate())
            _mTransferError = e.Error
        End If
        _bwTransfer.Dispose()
        UpdateListViewItem()
        SortListViewItem()
        MainForm.RunUploadQueue()
        If _markedForDeletion Then
            MainForm.objListViewTransfers.RemoveObject(Me)
        End If
    End Sub
    Private Function GetUploadLocation(targetFolder As Data.File) As String
        Try
            Dim uLocation As String = "\" & targetFolder.OwnerNames(0) & "\"
            Dim lstParents As New List(Of String)
            If targetFolder.Parents.Count = 0 Then
                Return uLocation
            End If
            Dim p As ParentReference = targetFolder.Parents(0)
            lstParents.Add(targetFolder.Title)
            Do While (p.IsRoot = False)
                Dim pFile As Data.File = _mUser.DriveService.Files.Get(p.Id).Fetch()
                lstParents.Add(pFile.Title)
                p = pFile.Parents(0)
            Loop
            lstParents.Reverse()
            Return lstParents.Aggregate(uLocation, Function(current, strTmp) current & strTmp & "\")
        Catch ex As Exception
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while getting the transfer location."))
            Return ""
        End Try
    End Function
    Public Sub New(gUser As GoogleUser, uploadFileFullPath As String, parentTreeRowObject As Data.File)
        MyBase.New(QueueType.Upload)
        _mUser = gUser
        _mParentRowObject = parentTreeRowObject
        _mTransferLocation = GetUploadLocation(_mParentRowObject)
        _mWriteTimeOutMs = MainForm.AppOptions.WriteTimeOutMs
        _mUploadBufferSize = MainForm.AppOptions.UploadBufferSize
        _mUploadFilePath = uploadFileFullPath
        _mMimeType = FileManager.GetMimeType(_mUploadFilePath)
        Dim fileInfo As New FileInfo(uploadFileFullPath)
        _mFileName = fileInfo.Name
        _mTransferSize = fileInfo.Length
        _mUploadFile = New Data.File
        _mUploadFile.Title = fileInfo.Name
        _mUploadFile.MimeType = _mMimeType
        Dim pReference As New List(Of ParentReference)
        pReference.Add(New ParentReference With {.Id = _mParentRowObject.Id})
        _mUploadFile.Parents = pReference
        UpdateStatus(StatusEnum.Queued)
    End Sub
End Class
