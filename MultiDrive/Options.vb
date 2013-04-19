Public Class Options
    Enum FileExistsDecision
        Overwrite = 1
        Rename = 2
        Prompt = 3
    End Enum
    Enum TrashDecision
        Highlight = 1
        Show = 2
        Hide = 3
    End Enum
    Private _mSimultaneousDownloads As Integer
    Private _mSimultaneousUploads As Integer
    Private _mSimultaneousP2PTransfers As Integer
    Private _mReadTimeOutMs As Integer
    Private _mWriteTimeOutMs As Integer
    Private _mDownloadLocation As String
    Private _mUploadBufferSize As Integer
    Private _mDownloadBufferSize As Integer
    Private _mPeer2PeerBufferSize As Integer
    Private _mIfFileExistsDecision As FileExistsDecision
    Private _mMd5CheckDLoad As Boolean
    Private _mMd5CheckULoad As Boolean
    Private _mMd5CheckP2P As Boolean
    Private _mShowTrashed As TrashDecision

    Private Const DefaultUploadBufferSize As Integer = ((1024 * 2) * 1024)
    Private Const DefaultDownloadBufferSize As Integer = ((1024 * 2) * 1024)
    Private Const DefaultPeer2PeerBufferSize As Integer = ((1024 * 2) * 1024)
    Private Const DefaultSimultaneousP2PTransfers As Integer = 1
    Private Const DefaultSimultaneousDownloads As Integer = 1
    Private Const DefaultSimultaneousUploads As Integer = 1
    Private Const DefaultReadTimeOutMs As Integer = 20 * 1000
    '20 seconds
    Private Const DefaultWriteTimeOutMs As Integer = 20 * 1000
    '20 seconds
    Private ReadOnly _dLocationDefault As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

    Public Property ShowTrashed As TrashDecision
        Get
            Return _mShowTrashed
        End Get
        Set(value As TrashDecision)
            _mShowTrashed = value
        End Set
    End Property
    Public Property Md5CheckDLoad As Boolean
        Get
            Return _mMd5CheckDLoad
        End Get
        Set(value As Boolean)
            _mMd5CheckDLoad = value
        End Set
    End Property
    Public Property Md5CheckULoad As Boolean
        Get
            Return _mMd5CheckULoad
        End Get
        Set(value As Boolean)
            _mMd5CheckULoad = value
        End Set
    End Property
    Public Property Md5CheckP2P As Boolean
        Get
            Return _mMd5CheckP2P
        End Get
        Set(value As Boolean)
            _mMd5CheckP2P = value
        End Set
    End Property
    Public Property IfFileExistsDecision As FileExistsDecision
        Get
            Return _mIfFileExistsDecision
        End Get
        Set(value As FileExistsDecision)
            _mIfFileExistsDecision = value
        End Set
    End Property
    Public Property SimultaneousP2PTransfers As Integer
        Get
            Return _mSimultaneousP2PTransfers
        End Get
        Set(value As Integer)
            If value > 0 Then
                _mSimultaneousP2PTransfers = value
            Else
                Throw New FormatException("""Simultaneous 'User to User' Transfers"" setting must be a positive integer.")
            End If
        End Set
    End Property
    Public Property DownloadLocation As String
        Get
            Return _mDownloadLocation
        End Get
        Set(value As String)
            If _mDownloadLocation = value Then
                Exit Property
            End If
            If IO.Directory.Exists(value) Then
                _mDownloadLocation = value
            Else
                Throw New FormatException("Invalid folder!")
            End If
        End Set
    End Property
    Public Sub SetDownloadLocation()
        Dim bDialog As New FolderBrowserDialog
        bDialog.Description = "Default Download Location"
        bDialog.ShowNewFolderButton = True
        bDialog.RootFolder = Environment.SpecialFolder.MyDocuments
        bDialog.ShowDialog()
        If Not bDialog.SelectedPath Is Nothing Then
            _mDownloadLocation = bDialog.SelectedPath
        Else
            Throw New FormatException("Invalid download location!")
        End If
    End Sub
    Public Sub ResetDownloadLocation()
        _mDownloadLocation = _dLocationDefault
    End Sub
    Public Property ReadTimeOutMs As Integer
        Get
            Return _mReadTimeOutMs
        End Get
        Set(value As Integer)
            If value >= 1000 Then
                _mReadTimeOutMs = value
            Else
                Throw New FormatException("'Read TimeOut' setting must be at least 1 second (1000ms).")
            End If
        End Set
    End Property
    Public Property UploadBufferSize As Integer
        Get
            Return _mUploadBufferSize
        End Get
        Set(value As Integer)
            If value >= (256 * 1024) Then
                _mUploadBufferSize = value
            Else
                Throw New FormatException("'Upload Buffer Size' setting must be at least 256kb.")
            End If
        End Set
    End Property
    Public Property DownloadBufferSize As Integer
        Get
            Return _mDownloadBufferSize
        End Get
        Set(value As Integer)
            If value >= (256 * 1024) Then
                _mDownloadBufferSize = value
            Else
                Throw New FormatException("'Download Buffer Size' setting must be at least 256kb.")
            End If
        End Set
    End Property
    Public Sub SetP2PBufferSizeKb(kilobytes As Integer)
        _mPeer2PeerBufferSize = Convert.ToInt64(kilobytes * 1024)
    End Sub
    Public Sub SetUploadBufferSizeKb(kilobytes As Integer)
        _mUploadBufferSize = Convert.ToInt64(kilobytes * 1024)
    End Sub
    Public Sub SetDownloadBufferSizeKb(kilobytes As Integer)
        _mDownloadBufferSize = Convert.ToInt64(kilobytes * 1024)
    End Sub
    Public Property Peer2PeerBufferSize As Integer
        Get
            Return _mPeer2PeerBufferSize
        End Get
        Set(value As Integer)
            If value >= (256 * 1024) Then
                _mPeer2PeerBufferSize = value
            Else
                Throw New FormatException("""'User to User' Buffer Size"" setting must be at least 256kb.")
            End If
        End Set
    End Property

    Public Property WriteTimeOutMs As Integer
        Get
            Return _mWriteTimeOutMs
        End Get
        Set(value As Integer)
            If value >= 1000 Then
                _mWriteTimeOutMs = value
            Else
                Throw New FormatException("'Write TimeOut' setting must be at least 1 second (1000ms).")
            End If
        End Set
    End Property
    Public Property SimultaneousDownloads As Integer
        Get
            Return _mSimultaneousDownloads
        End Get
        Set(value As Integer)
            If value > 0 Then
                _mSimultaneousDownloads = value
            Else
                Throw New FormatException("'Simultaneous Downloads' setting must be a positive integer.")
            End If
        End Set
    End Property
    Public Property SimultaneousUploads As Integer
        Get
            Return _mSimultaneousUploads
        End Get
        Set(value As Integer)
            If value > 0 Then
                _mSimultaneousUploads = value
            Else
                Throw New FormatException("'Simultaneous Uploads' setting must be a positive integer.")
            End If
        End Set
    End Property

    Public Sub New()
        ResetToDefaults()

    End Sub

    Public Sub ResetToDefaults()
        _mSimultaneousDownloads = DefaultSimultaneousDownloads
        _mSimultaneousUploads = DefaultSimultaneousUploads
        _mSimultaneousP2PTransfers = DefaultSimultaneousP2PTransfers
        _mReadTimeOutMs = DefaultReadTimeOutMs
        _mWriteTimeOutMs = DefaultWriteTimeOutMs
        _mDownloadLocation = _dLocationDefault
        _mUploadBufferSize = DefaultUploadBufferSize
        _mDownloadBufferSize = DefaultDownloadBufferSize
        _mPeer2PeerBufferSize = DefaultPeer2PeerBufferSize
        _mIfFileExistsDecision = FileExistsDecision.Prompt
        _mMd5CheckDLoad = True
        _mMd5CheckULoad = True
        _mMd5CheckP2P = True
        _mShowTrashed = TrashDecision.Highlight
    End Sub
End Class
