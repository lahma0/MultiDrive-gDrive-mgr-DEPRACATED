Public Class LogEntry
    Enum LogEntryType
        Information = 1
        Err = 2
        Warning = 3
    End Enum
    Enum LogEntryGroup
        General = 1
        Settings = 2
        Queue = 3
        Users = 4
        Explorer = 5
    End Enum
    Private ReadOnly _mType As LogEntryType
    Private ReadOnly _mGroup As LogEntryGroup
    Private ReadOnly _mMessage As String
    Private ReadOnly _mTimeStamp As DateTime

    Public ReadOnly Property TimeStamp As DateTime
        Get
            Return _mTimeStamp
        End Get
    End Property
    Public ReadOnly Property Type As LogEntryType
        Get
            Return _mType
        End Get
    End Property
    Public ReadOnly Property Group As LogEntryGroup
        Get
            Return _mGroup
        End Get
    End Property
    Public ReadOnly Property Message As String
        Get
            Return _mMessage
        End Get
    End Property

    Public Sub New(type As LogEntryType, group As LogEntryGroup, timeStamp As DateTime, message As String)
        _mType = type
        _mGroup = group
        _mTimeStamp = timeStamp
        _mMessage = message
    End Sub
End Class
