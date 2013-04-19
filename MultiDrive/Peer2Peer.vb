Imports System.IO
Imports System.Net
Imports Google.Apis
Imports Google.Apis.Drive.v2
Imports Google.Apis.Authentication.OAuth2
Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth
Imports Google.Apis.Drive.v2.Data
Imports System.Web.Script.Serialization

Public Class Peer2Peer
    Inherits QueueItem
    Private _mSourceFile As Data.File
    Private _mNewFile As Data.File
    Private _mTargetParentFolder As Data.File
    Private ReadOnly _mTargetUser As GoogleUser
    Private ReadOnly _mPeer2PeerBufferSize As Long
    Private _mResumableUri As String

    Public ReadOnly Property TargetUser As GoogleUser
        Get
            Return _mTargetUser
        End Get
    End Property
    Public Property SourceFile As Drive.v2.Data.File
        Get
            Return _mSourceFile
        End Get
        Set(value As Drive.v2.Data.File)
            _mSourceFile = value
        End Set
    End Property
    Public Property TargetParentFolder As Drive.v2.Data.File
        Get
            Return _mTargetParentFolder
        End Get
        Set(value As Drive.v2.Data.File)
            _mTargetParentFolder = value
        End Set
    End Property
    Public Sub CopyStream(targetStream As Stream, sourceStream As Stream, Optional isResuming As Boolean = False)

        Dim buffer As Byte() = New Byte(_mPeer2PeerBufferSize - 1) {}
        Dim len As Integer
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
    Private Function GetRangeHeader(resumeableUri As String) As Long
        Dim myUploadWebRequest As HttpWebRequest = WebRequest.Create(resumeableUri)
        myUploadWebRequest.Method = "PUT"
        myUploadWebRequest.ContentLength = 0
        myUploadWebRequest.Headers.Add("Content-Range", String.Format(" bytes */{0}", _mTransferSize))
        _mTargetUser.Authenticator.ApplyAuthenticationToRequest(myUploadWebRequest)
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
        _mTargetUser.Authenticator.ApplyAuthenticationToRequest(myUploadWebRequest)
        Return myUploadWebRequest
    End Function
    Private Function AuthenticateRequest(Optional isResuming As Boolean = False) As HttpWebRequest
        Dim authenticator As OAuth2Authenticator(Of NativeApplicationClient) = _mUser.Authenticator
        Dim downloadUri As New Uri(_mSourceFile.DownloadUrl)
        Dim myHttpWebRequest As HttpWebRequest = WebRequest.Create(downloadUri)
        If isResuming = True And _mBytesTransferred > 0 Then
            myHttpWebRequest.AddRange(_mBytesTransferred + 1)
        End If
        authenticator.ApplyAuthenticationToRequest(myHttpWebRequest)
        Return myHttpWebRequest
    End Function
    Private Function BuildRequestBody() As String
        Return "{  ""kind"": ""drive#file"", ""title"": """ & _mSourceFile.Title & """, ""description"": """ & _mSourceFile.Description & """, ""createddate"": """ & _mSourceFile.CreatedDate & """, ""parents"": [   {    ""kind"": ""drive#parentReference"",    ""id"": """ & _mTargetParentFolder.Id & """   }  ]}"
    End Function

    Friend Overrides Sub BackgroundTransferStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            UpdateStatus(StatusEnum.Transferring)
            MainForm.ActiveP2P += 1
            Dim isResuming As Boolean = e.Argument
            If Not IsResuming Then
                Dim uploadUri As New Uri("https://www.googleapis.com/upload/drive/v2/files?uploadType=resumable")
                Dim requestBody As String = BuildRequestBody()
                Dim myHttpWebRequest As HttpWebRequest = WebRequest.Create(uploadUri)
                myHttpWebRequest.Method = "POST"
                myHttpWebRequest.ContentLength = requestBody.Length
                myHttpWebRequest.ContentType = "application/json; charset=UTF-8"
                myHttpWebRequest.Headers.Add("X-Upload-Content-Type", _mMimeType)
                myHttpWebRequest.Headers.Add("X-Upload-Content-Length", _mTransferSize.ToString)
                _mTargetUser.Authenticator.ApplyAuthenticationToRequest(myHttpWebRequest)
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
            Dim authenticatedRequest As HttpWebRequest = AuthenticateRequest(e.Argument)
            Using downloadResponse As HttpWebResponse = authenticatedRequest.GetResponse
                If downloadResponse.StatusCode = HttpStatusCode.OK Or downloadResponse.StatusCode = HttpStatusCode.PartialContent Then
                    Using downloadStream As Stream = downloadResponse.GetResponseStream
                        Dim myUploadRequest As HttpWebRequest = RetrieveUploadWebRequest(_mResumableUri, isResuming)
                        Using uploadStream As Stream = myUploadRequest.GetRequestStream
                            uploadStream.WriteTimeout = _mWriteTimeOutMs
                            downloadStream.ReadTimeout = _mReadTimeOutMs
                            CopyStream(uploadStream, downloadStream, isResuming)
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
                End If
            End Using
        Catch exWeb As WebException
            e.Result = GetResultAlternate()
        Catch ex As Exception
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while attempting to transfer " & _mFileName))
        End Try
    End Sub
    Friend Overrides Sub BackgroundTransferFinished(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        MainForm.ActiveP2P -= 1
        RemoveBwHandlers()
        MainForm.objTreeExplorer.RefreshObject(_mTargetParentFolder)
        If Not e.Result Is Nothing Then
            UpdateStatus(GetResult(e.Result))
            'UpdateStatus(e.Result)
            MainForm.objListViewTransfers.RefreshObject(Me)

        Else
            UpdateStatus(GetResultAlternate())
            _mTransferError = e.Error
        End If
        _bwTransfer.Dispose()
        UpdateListViewItem()
        SortListViewItem()
        MainForm.RunPeer2PeerQueue()
        If _markedForDeletion Then
            MainForm.objListViewTransfers.RemoveObject(Me)
        End If
    End Sub
    Private Function GetResult(uploadWebResponse As HttpStatusCode) As StatusEnum
        Select Case uploadWebResponse
            Case 201, 200
                MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " transferred successfully."))
                If MainForm.AppOptions.Md5CheckP2P Then
                    If FileManager.CompareMd5(_mSourceFile, _mNewFile) Then
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
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " failed to transfer."))
                    Return StatusEnum.Failed
                End If
            Case Else
                If _mProgress > 0 Then
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " was interrupted."))
                    Return StatusEnum.Interrupted
                Else
                    MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " failed to transfer."))
                    Return StatusEnum.Failed
                End If
        End Select
    End Function
    Private Function GetResultAlternate() As StatusEnum
        If _mProgress = 100 Then
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Information, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " transferred successfully."))
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
            MainForm.AddLogItem(New LogEntry(LogEntry.LogEntryType.Warning, LogEntry.LogEntryGroup.Queue, DateTime.Now, _mFileName & " failed to transfer."))
            Return StatusEnum.Failed
        End If
    End Function
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
                Dim pFile As Data.File = _mTargetUser.DriveService.Files.Get(p.Id).Fetch()
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
    Public Sub New(sourceFile As Data.File, targetFolder As Data.File, sourceUser As GoogleUser, targetUser As GoogleUser)
        MyBase.New(QueueType.Peer2Peer)
        _mSourceFile = sourceFile
        _mUser = sourceUser
        _mTargetUser = targetUser
        _mTargetParentFolder = targetFolder
        _mTransferLocation = GetUploadLocation(_mTargetParentFolder)
        _mTransferSize = Convert.ToInt64(_mSourceFile.FileSize)
        _mPeer2PeerBufferSize = MainForm.AppOptions.Peer2PeerBufferSize
        _mFileName = _mSourceFile.Title
        _mReadTimeOutMs = MainForm.AppOptions.ReadTimeOutMs
        _mWriteTimeOutMs = MainForm.AppOptions.WriteTimeOutMs
        _mMimeType = FileManager.GetMimeType(_mFileName)
        UpdateStatus(StatusEnum.Queued)
    End Sub
End Class
