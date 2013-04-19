Imports System.ComponentModel

Public MustInherit Class QueueItem
    Enum QueueType
        Download = 1
        Upload = 2
        Peer2Peer = 3
    End Enum
    Enum StatusEnum
        Queued = 1
        Transferring = 2
        Completed = 4
        Paused = 5
        Cancelled = 6
        Interrupted = 7
        Failed = 8
    End Enum
    Private ReadOnly _mTransferType As QueueType

    Friend _mStatus As StatusEnum
    Friend _mProgress As Integer
    Friend _bwTransfer As BackgroundWorker
    Friend _mBytesTransferred As Long
    Friend _markedForDeletion As Boolean = False
    Friend _mUser As GoogleUser
    Friend _mTransferSize As Long
    Friend _mTransferError As Exception
    Friend _mFileName As String
    Friend _mReadTimeOutMs As Integer
    Friend _mWriteTimeOutMs As Integer
    Friend _mTransferLocation As String
    Friend _mMimeType As String


    Public Overridable ReadOnly Property Progress As Integer
        Get
            Return _mProgress
        End Get
    End Property

    Public Overridable ReadOnly Property FileName As String
        Get
            Return _mFileName
        End Get
    End Property
    Public Overridable ReadOnly Property User As GoogleUser
        Get
            Return _mUser
        End Get
    End Property
    Public Overridable ReadOnly Property TransferSize As Long
        Get
            Return _mTransferSize
        End Get
    End Property

    Public Overridable ReadOnly Property Status As StatusEnum
        Get
            Return _mStatus
        End Get
    End Property

    Public Overridable Property BytesTransferred As Long
        Get
            Return _mBytesTransferred
        End Get
        Set(value As Long)
            _mBytesTransferred = value
        End Set
    End Property

    Public Overridable ReadOnly Property TransferError As Exception
        Get
            Return _mTransferError
        End Get
    End Property

    Public Overridable ReadOnly Property TransferLocation As String
        Get
            Return _mTransferLocation
        End Get
    End Property

    Public ReadOnly Property TransferType As QueueType
        Get
            Return _mTransferType
        End Get
    End Property

    Friend MustOverride Sub BackgroundTransferStart(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
    Friend MustOverride Sub BackgroundTransferFinished(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)

    Friend Overridable Sub TransferProgress(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs)
        MainForm.objListViewTransfers.RefreshObject(Me)
    End Sub

    Friend Overridable Sub TransferGo(isResuming As Boolean)
        _bwTransfer = New BackgroundWorker
        _bwTransfer.WorkerSupportsCancellation = True
        _bwTransfer.WorkerReportsProgress = True
        AddHandler _bwTransfer.DoWork, AddressOf BackgroundTransferStart
        AddHandler _bwTransfer.RunWorkerCompleted, AddressOf BackgroundTransferFinished
        AddHandler _bwTransfer.ProgressChanged, AddressOf TransferProgress
        MainForm.objListViewTransfers.RefreshObject(Me)
        MainForm.objListViewTransfers.Sort()
        Select Case _mTransferType
            Case QueueType.Download
                MainForm.ActiveDownloads += 1
            Case QueueType.Upload
                MainForm.ActiveUploads += 1
            Case QueueType.Peer2Peer
                MainForm.ActiveP2P += 1
        End Select
        _bwTransfer.RunWorkerAsync(isResuming)

    End Sub
    Public Overridable Sub TransferStart()
        Dim doResume As Boolean = False
        If (_mStatus = StatusEnum.Paused Or _mStatus = StatusEnum.Interrupted) And _mProgress > 0 Then
            doResume = True
        ElseIf _mStatus = StatusEnum.Completed Then
            Exit Sub
        Else
            doResume = False
        End If
        If MainForm.ActiveDownloads < MainForm.AppOptions.SimultaneousDownloads Then
            TransferGo(doResume)
        End If
    End Sub
    Public Overridable Sub TransferPause()
        If _mStatus = StatusEnum.Transferring Then
            If Not _bwTransfer Is Nothing Then
                UpdateStatus(StatusEnum.Paused)
                _bwTransfer.CancelAsync()
            End If
        End If
    End Sub
    Public Overridable Sub TransferCancel()
        If _mStatus = StatusEnum.Transferring Then
            If Not _bwTransfer.CancellationPending Then
                UpdateStatus(StatusEnum.Cancelled)
                _bwTransfer.CancelAsync()
            End If
        ElseIf Not (_mStatus = StatusEnum.Failed Or _mStatus = StatusEnum.Completed) Then
            UpdateStatus(StatusEnum.Cancelled)
            _mBytesTransferred = 0
        End If
    End Sub
    Public Overridable Sub TransferRetry()
        If _mStatus = StatusEnum.Cancelled Or _mStatus = StatusEnum.Failed Or _mStatus = StatusEnum.Interrupted Then
            UpdateStatus(StatusEnum.Queued)
            _mBytesTransferred = 0
            MainForm.RunUploadQueue()
        End If
    End Sub
    Public Overridable Sub ClearQueueItem()
        _markedForDeletion = True
        If _mStatus = StatusEnum.Transferring Then
            TransferCancel()
        Else
            MainForm.objListViewTransfers.RemoveObject(Me)
            MainForm.objListViewTransfers.Refresh()
        End If
    End Sub
    Friend Overridable Sub RemoveBwHandlers()
        If Not _bwTransfer Is Nothing Then
            Try
                RemoveHandler _bwTransfer.DoWork, AddressOf BackgroundTransferStart
                RemoveHandler _bwTransfer.RunWorkerCompleted, AddressOf BackgroundTransferFinished
                RemoveHandler _bwTransfer.ProgressChanged, AddressOf TransferProgress
            Catch ex As Exception

            End Try
        End If
    End Sub
    Friend Overridable Sub UpdateStatus(dStatus As StatusEnum)
        _mStatus = dStatus
        UpdateListViewItem()
    End Sub
    Friend Overridable Sub UpdateListViewItem()
        MainForm.objListViewTransfers.RefreshObject(Me)
    End Sub
    Friend Overridable Sub SortListViewItem()
        MainForm.objListViewTransfers.Sort()
    End Sub
    Public Sub New(downloadType As QueueType)
        _mTransferType = downloadType
    End Sub
End Class
