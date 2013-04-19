
Partial Public Class MainForm
    'Private Sub QueueAddDownload(dLoad As Download)
    '    objListViewTransfers.AddObject(dLoad)
    '    If _mActiveDownloads < AppOptions.SimultaneousDownloads Then
    '        RunDownloadQueue()
    '    End If
    'End Sub
    'Private Sub QueueAddDownload(dLoadList As List(Of Download))
    '    objListViewTransfers.AddObjects(dLoadList)
    '    If _mActiveDownloads < AppOptions.SimultaneousDownloads Then
    '        RunDownloadQueue()
    '    End If
    'End Sub
    Public Sub RunDownloadQueue()
        Try
            Dim dLoadList As List(Of Download) = GetQueuedDownloads()
            Dim numOfQueuedDLoads As Integer = dLoadList.Count
            If dLoadList IsNot Nothing Then
                Dim i As Integer = 0
                Do While (_mActiveDownloads < AppOptions.SimultaneousDownloads And numOfQueuedDLoads > 0)
                    dLoadList(i).TransferStart()
                    i += 1
                    numOfQueuedDLoads -= 1
                Loop
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Queue, DateTime.Now, "An error occurred while running the download queue."))
        End Try
    End Sub
    Public Function GetQueuedDownloads() As List(Of Download)
        Try
            Dim qItemArray As ArrayList = objListViewTransfers.Objects
            Return (From qItem As QueueItem In qItemArray Where qItem.TransferType = QueueItem.QueueType.Download And qItem.Status = QueueItem.StatusEnum.Queued).Cast(Of Download)().ToList()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetFreeDownloadSlots() As Integer
        Try
            Dim qItemArray As ArrayList = objListViewTransfers.Objects
            Dim transferringCount As Integer = qItemArray.Cast(Of QueueItem)().Count(Function(qItem) qItem.TransferType = QueueItem.QueueType.Download And qItem.Status = QueueItem.StatusEnum.Transferring)
            Return AppOptions.SimultaneousDownloads - transferringCount
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Sub RunUploadQueue()
        Try
            Dim uLoadList As List(Of Upload) = GetQueuedUploads()
            Dim numOfQueuedULoads As Integer = uLoadList.Count
            If uLoadList IsNot Nothing Then
                Dim i As Integer = 0
                Do While (_mActiveUploads < AppOptions.SimultaneousUploads And numOfQueuedULoads > 0)
                    uLoadList(i).TransferStart()
                    i += 1
                    numOfQueuedULoads -= 1
                Loop
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Queue, DateTime.Now, "An error occurred while running the upload queue."))
        End Try
        'Try
        '    Dim uLoadList As List(Of Upload) = GetQueuedUploads()

        '    Dim queuedULoads As Integer = uLoadList.Count
        '    Dim slots As Integer = GetFreeUploadSlots()
        '    Do While (slots > 0 And queuedULoads > 0)
        '        uLoadList(queuedULoads - 1).TransferStart()
        '        slots -= 1
        '        queuedULoads -= 1
        '    Loop
        'Catch ex As Exception
        '    AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while running the upload queue."))
        'End Try
    End Sub
    Public Function GetQueuedUploads() As List(Of Upload)
        Try
            Dim qItemArray As ArrayList = objListViewTransfers.Objects
            Return (From qItem As QueueItem In qItemArray Where qItem.TransferType = QueueItem.QueueType.Upload And qItem.Status = QueueItem.StatusEnum.Queued).Cast(Of Upload)().ToList()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetFreeUploadSlots() As Integer
        Try
            Dim qItemArray As ArrayList = objListViewTransfers.Objects
            Dim transferringCount As Integer = qItemArray.Cast(Of QueueItem)().Count(Function(qItem) qItem.TransferType = QueueItem.QueueType.Upload And qItem.Status = QueueItem.StatusEnum.Transferring)
            Return AppOptions.SimultaneousUploads - transferringCount
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Sub RunPeer2PeerQueue()
        Try
            Dim p2pList As List(Of Peer2Peer) = GetQueuedPeer2Peer()
            Dim numOfP2P As Integer = p2pList.Count
            If p2pList IsNot Nothing Then
                Dim i As Integer = 0
                Do While (_mActiveP2P < AppOptions.SimultaneousP2PTransfers And numOfP2P > 0)
                    p2pList(i).TransferStart()
                    i += 1
                    numOfP2P -= 1
                Loop
            End If
        Catch ex As Exception
            AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Queue, DateTime.Now, "An error occurred while running the P2P queue."))
        End Try
        'Try
        '    Dim p2PLoadList As List(Of Peer2Peer) = GetQueuedPeer2Peer()

        '    Dim queuedP2PCount As Integer = p2PLoadList.Count
        '    Dim slots As Integer = GetFreeP2PSlots()
        '    Do While (slots > 0 And queuedP2PCount > 0)
        '        p2PLoadList(queuedP2PCount - 1).TransferStart()
        '        slots -= 1
        '        queuedP2PCount -= 1
        '    Loop
        'Catch ex As Exception
        '    AddLogItem(New LogEntry(LogEntry.LogEntryType.Err, LogEntry.LogEntryGroup.Explorer, DateTime.Now, "An error occurred while running the p2p queue."))
        'End Try
    End Sub
    Public Function GetQueuedPeer2Peer() As List(Of Peer2Peer)
        Try
            Dim qItemArray As ArrayList = objListViewTransfers.Objects
            Return (From qItem As QueueItem In qItemArray Where qItem.TransferType = QueueItem.QueueType.Peer2Peer And qItem.Status = QueueItem.StatusEnum.Queued).Cast(Of Peer2Peer)().ToList()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetFreeP2PSlots() As Integer
        Try
            Dim qItemArray As ArrayList = objListViewTransfers.Objects
            Dim transferringCount As Integer = qItemArray.Cast(Of QueueItem)().Count(Function(qItem) qItem.TransferType = QueueItem.QueueType.Peer2Peer And qItem.Status = QueueItem.StatusEnum.Transferring)
            Return AppOptions.SimultaneousP2PTransfers - transferringCount
        Catch ex As Exception
            Return 0
        End Try
    End Function
End Class
