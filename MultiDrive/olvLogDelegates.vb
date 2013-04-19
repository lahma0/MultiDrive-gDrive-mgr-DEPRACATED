Partial Public Class MainForm
    Delegate Sub MMoveLogObjectsToTop(rowObjects As ArrayList)
    Delegate Sub MAddLogObject(rowObject As Object)
    Public Function GetLogTypeIcon(rowObject As Object) As Object
        Dim logItem As LogEntry = CType(rowObject, LogEntry)
        If logItem.Type = LogEntry.LogEntryType.Information Then
            Return "information"
        ElseIf logItem.Type = LogEntry.LogEntryType.Err Then
            Return "error"
        ElseIf logItem.Type = LogEntry.LogEntryType.Warning Then
            Return "warning"
        Else
            Return Nothing
        End If
    End Function
    Private Function LogGroupKeyGetter(rowObject As Object) As Object
        Dim logItem As LogEntry = CType(rowObject, LogEntry)
        Return logItem.Group.ToString
    End Function
    Private Function LogTimeFormatter(rowObject As Object) As Object
        Dim logItem As LogEntry = CType(rowObject, LogEntry)
        Dim dt As DateTime = logItem.TimeStamp
        Return dt.ToShortTimeString()
    End Function
    Public Sub MoveLogObjectsToTop(rowObjects As ArrayList)
        objLViewLog.MoveObjects(0, rowObjects)
    End Sub
    Public Sub AddLogObject(rowObject As Object)
        objLViewLog.AddObject(rowObject)
    End Sub
End Class
