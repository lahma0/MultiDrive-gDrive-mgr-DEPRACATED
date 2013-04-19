<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Dim HeaderStateStyle1 As BrightIdeasSoftware.HeaderStateStyle = New BrightIdeasSoftware.HeaderStateStyle()
        Dim HeaderStateStyle2 As BrightIdeasSoftware.HeaderStateStyle = New BrightIdeasSoftware.HeaderStateStyle()
        Dim HeaderStateStyle3 As BrightIdeasSoftware.HeaderStateStyle = New BrightIdeasSoftware.HeaderStateStyle()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.docIconsSmall = New System.Windows.Forms.ImageList(Me.components)
        Me.objListViewTransfers = New BrightIdeasSoftware.ObjectListView()
        Me.olvColFileName = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColTransferSize = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColProgress = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.barRendTransferProgress = New BrightIdeasSoftware.BarRenderer()
        Me.olvColUser = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColTransferTo = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.contextMenuQ = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.toolStripItemQTransferPause = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripItemQTransferResume = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripSepQTop = New System.Windows.Forms.ToolStripSeparator()
        Me.toolStripItemQTransferCancel = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripItemQRetry = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripSepQBottom = New System.Windows.Forms.ToolStripSeparator()
        Me.toolStripItemQClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripItemQClearFinished = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolStripItemQOpenDir = New System.Windows.Forms.ToolStripMenuItem()
        Me.headerFormatStyleBoldArial = New BrightIdeasSoftware.HeaderFormatStyle()
        Me.tabControlMain = New System.Windows.Forms.TabControl()
        Me.tabPageDrive = New System.Windows.Forms.TabPage()
        Me.splitContainerMain = New System.Windows.Forms.SplitContainer()
        Me.objTreeExplorer = New BrightIdeasSoftware.TreeListView()
        Me.olvColExpTitle = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColExpFileSize = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColExpModifiedDate = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColExpMimeType = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColExpDescription = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColExpQuotaBytesUsed = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColExpExtension = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.contextMenuExplorerFiles = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tStripExpFilesDownload = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesDownloadTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesDownloadAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesNewFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesTrash = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesStar = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.tStripExpFilesEditDescription = New System.Windows.Forms.ToolStripMenuItem()
        Me.hotItemStyleExplorer = New BrightIdeasSoftware.HotItemStyle()
        Me.tStripExplorer = New System.Windows.Forms.ToolStrip()
        Me.tStripBtnExpNewFolder = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnExpDelete = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnExpTrash = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnExpDownload = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tStripMenuItemExpAddDownload = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripMenuItemExpAddDownloadTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripMenuItemExpAddDownloadAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripBtnExpOpen = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnExpStar = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnExpRefresh = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnExpAbout = New System.Windows.Forms.ToolStripButton()
        Me.splitContainerLower = New System.Windows.Forms.SplitContainer()
        Me.objLViewLog = New BrightIdeasSoftware.ObjectListView()
        Me.olvLogTime = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvLogMessage = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.tStripQueue = New System.Windows.Forms.ToolStrip()
        Me.tStripBtnQueueStart = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnQueuePause = New System.Windows.Forms.ToolStripButton()
        Me.tStripBtnQueueCancel = New System.Windows.Forms.ToolStripButton()
        Me.tStripDropBtnQueueClear = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tStripMenuItemQueueClearSelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripMenuItemQueueClearCompleted = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripMenuItemQueueClearInactive = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabPageOptions = New System.Windows.Forms.TabPage()
        Me.splitContainerOptions = New System.Windows.Forms.SplitContainer()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbTrashed = New System.Windows.Forms.ComboBox()
        Me.grpOptionsDownloads = New System.Windows.Forms.GroupBox()
        Me.btnResetToDefaults = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbFileExistsDecision = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDefaultDownloadLocation = New System.Windows.Forms.TextBox()
        Me.btnBrowseDownloadLocation = New System.Windows.Forms.Button()
        Me.grpBoxMD5 = New System.Windows.Forms.GroupBox()
        Me.chkBoxOptionsMD5P2P = New System.Windows.Forms.CheckBox()
        Me.chkBoxOptionsMD5Uploads = New System.Windows.Forms.CheckBox()
        Me.chkBoxOptionsMD5Downloads = New System.Windows.Forms.CheckBox()
        Me.toolStripUsers = New System.Windows.Forms.ToolStrip()
        Me.tStripUsersButtonAddUser = New System.Windows.Forms.ToolStripButton()
        Me.tStripUsersButtonRemoveUser = New System.Windows.Forms.ToolStripButton()
        Me.tStripUsersButtonRemoveAllUsers = New System.Windows.Forms.ToolStripButton()
        Me.grpBoxOptionsTransfers = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.numUpDownSimulPeerToPeer = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.numUpDownSimulUploads = New System.Windows.Forms.NumericUpDown()
        Me.numUpDownSimulDownloads = New System.Windows.Forms.NumericUpDown()
        Me.grpBoxTransferChunkSize = New System.Windows.Forms.GroupBox()
        Me.numUpDownULoadChunkSize = New System.Windows.Forms.NumericUpDown()
        Me.numUpDownP2PChunkSize = New System.Windows.Forms.NumericUpDown()
        Me.numUpDownDLoadChunkSize = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.olvUsers = New BrightIdeasSoftware.ObjectListView()
        Me.olvColUsersEmail = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersPicture = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersName = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersLoggedIn = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersQuotaUsed = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersQuotaUsedGlobally = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersQuotaFree = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersQuotaTotal = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersQuotaPercentUsed = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.olvColUsersQuotaTrash = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.docIconsXL = New System.Windows.Forms.ImageList(Me.components)
        Me.toolTipFileInfo = New System.Windows.Forms.ToolTip(Me.components)
        Me.docIconsMedium = New System.Windows.Forms.ImageList(Me.components)
        Me.btnResetApi = New System.Windows.Forms.Button()
        CType(Me.objListViewTransfers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.contextMenuQ.SuspendLayout()
        Me.tabControlMain.SuspendLayout()
        Me.tabPageDrive.SuspendLayout()
        CType(Me.splitContainerMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainerMain.Panel1.SuspendLayout()
        Me.splitContainerMain.Panel2.SuspendLayout()
        Me.splitContainerMain.SuspendLayout()
        CType(Me.objTreeExplorer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.contextMenuExplorerFiles.SuspendLayout()
        Me.tStripExplorer.SuspendLayout()
        CType(Me.splitContainerLower, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainerLower.Panel1.SuspendLayout()
        Me.splitContainerLower.Panel2.SuspendLayout()
        Me.splitContainerLower.SuspendLayout()
        CType(Me.objLViewLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tStripQueue.SuspendLayout()
        Me.tabPageOptions.SuspendLayout()
        CType(Me.splitContainerOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainerOptions.Panel1.SuspendLayout()
        Me.splitContainerOptions.Panel2.SuspendLayout()
        Me.splitContainerOptions.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpOptionsDownloads.SuspendLayout()
        Me.grpBoxMD5.SuspendLayout()
        Me.toolStripUsers.SuspendLayout()
        Me.grpBoxOptionsTransfers.SuspendLayout()
        CType(Me.numUpDownSimulPeerToPeer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numUpDownSimulUploads, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numUpDownSimulDownloads, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpBoxTransferChunkSize.SuspendLayout()
        CType(Me.numUpDownULoadChunkSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numUpDownP2PChunkSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numUpDownDLoadChunkSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.olvUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 626)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(722, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'docIconsSmall
        '
        Me.docIconsSmall.ImageStream = CType(resources.GetObject("docIconsSmall.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.docIconsSmall.TransparentColor = System.Drawing.Color.Transparent
        Me.docIconsSmall.Images.SetKeyName(0, "application/vnd.google-apps.folder")
        Me.docIconsSmall.Images.SetKeyName(1, "application/vnd.google-apps.unknown")
        Me.docIconsSmall.Images.SetKeyName(2, "application/vnd.google-apps.document")
        Me.docIconsSmall.Images.SetKeyName(3, "application/vnd.google-apps.presentation")
        Me.docIconsSmall.Images.SetKeyName(4, "application/vnd.google-apps.spreadsheet")
        Me.docIconsSmall.Images.SetKeyName(5, "application/vnd.google-apps.drawing")
        Me.docIconsSmall.Images.SetKeyName(6, "download")
        Me.docIconsSmall.Images.SetKeyName(7, "open")
        Me.docIconsSmall.Images.SetKeyName(8, "rename")
        Me.docIconsSmall.Images.SetKeyName(9, "trash")
        Me.docIconsSmall.Images.SetKeyName(10, "deletefile")
        Me.docIconsSmall.Images.SetKeyName(11, "information")
        Me.docIconsSmall.Images.SetKeyName(12, "warning")
        Me.docIconsSmall.Images.SetKeyName(13, "removeuser")
        Me.docIconsSmall.Images.SetKeyName(14, "adduser")
        Me.docIconsSmall.Images.SetKeyName(15, "error")
        Me.docIconsSmall.Images.SetKeyName(16, "star")
        Me.docIconsSmall.Images.SetKeyName(17, "peer2peer")
        Me.docIconsSmall.Images.SetKeyName(18, "upload")
        Me.docIconsSmall.Images.SetKeyName(19, "lock")
        Me.docIconsSmall.Images.SetKeyName(20, "refresh")
        '
        'objListViewTransfers
        '
        Me.objListViewTransfers.AllColumns.Add(Me.olvColFileName)
        Me.objListViewTransfers.AllColumns.Add(Me.olvColTransferSize)
        Me.objListViewTransfers.AllColumns.Add(Me.olvColProgress)
        Me.objListViewTransfers.AllColumns.Add(Me.olvColUser)
        Me.objListViewTransfers.AllColumns.Add(Me.olvColTransferTo)
        Me.objListViewTransfers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.olvColFileName, Me.olvColTransferSize, Me.olvColProgress, Me.olvColUser, Me.olvColTransferTo})
        Me.objListViewTransfers.ContextMenuStrip = Me.contextMenuQ
        Me.objListViewTransfers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.objListViewTransfers.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.objListViewTransfers.FullRowSelect = True
        Me.objListViewTransfers.HeaderFont = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.objListViewTransfers.HeaderFormatStyle = Me.headerFormatStyleBoldArial
        Me.objListViewTransfers.HeaderUsesThemes = False
        Me.objListViewTransfers.Location = New System.Drawing.Point(0, 0)
        Me.objListViewTransfers.Name = "objListViewTransfers"
        Me.objListViewTransfers.OwnerDraw = True
        Me.objListViewTransfers.Size = New System.Drawing.Size(708, 132)
        Me.objListViewTransfers.SmallImageList = Me.docIconsSmall
        Me.objListViewTransfers.TabIndex = 13
        Me.objListViewTransfers.TileSize = New System.Drawing.Size(32, 32)
        Me.objListViewTransfers.UseCellFormatEvents = True
        Me.objListViewTransfers.UseCompatibleStateImageBehavior = False
        Me.objListViewTransfers.View = System.Windows.Forms.View.Details
        '
        'olvColFileName
        '
        Me.olvColFileName.AspectName = "FileName"
        Me.olvColFileName.CellPadding = Nothing
        Me.olvColFileName.Text = "Filename"
        Me.olvColFileName.Width = 166
        '
        'olvColTransferSize
        '
        Me.olvColTransferSize.AspectName = "TransferSize"
        Me.olvColTransferSize.CellPadding = Nothing
        Me.olvColTransferSize.Text = "Size"
        Me.olvColTransferSize.Width = 87
        '
        'olvColProgress
        '
        Me.olvColProgress.AspectName = "Progress"
        Me.olvColProgress.CellPadding = Nothing
        Me.olvColProgress.MaximumWidth = 128
        Me.olvColProgress.MinimumWidth = 128
        Me.olvColProgress.Renderer = Me.barRendTransferProgress
        Me.olvColProgress.Text = "Progress"
        Me.olvColProgress.Width = 128
        '
        'barRendTransferProgress
        '
        Me.barRendTransferProgress.MaximumWidth = 120
        '
        'olvColUser
        '
        Me.olvColUser.AspectName = "User.Email"
        Me.olvColUser.CellPadding = Nothing
        Me.olvColUser.Text = "User"
        Me.olvColUser.Width = 190
        '
        'olvColTransferTo
        '
        Me.olvColTransferTo.AspectName = "TransferLocation"
        Me.olvColTransferTo.CellPadding = Nothing
        Me.olvColTransferTo.FillsFreeSpace = True
        Me.olvColTransferTo.Text = "Transfer To:"
        Me.olvColTransferTo.Width = 213
        '
        'contextMenuQ
        '
        Me.contextMenuQ.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripItemQTransferPause, Me.toolStripItemQTransferResume, Me.tStripSepQTop, Me.toolStripItemQTransferCancel, Me.toolStripItemQRetry, Me.tStripSepQBottom, Me.toolStripItemQClear, Me.toolStripItemQClearFinished, Me.ToolStripSeparator3, Me.toolStripItemQOpenDir})
        Me.contextMenuQ.Name = "contextMenu"
        Me.contextMenuQ.Size = New System.Drawing.Size(164, 176)
        '
        'toolStripItemQTransferPause
        '
        Me.toolStripItemQTransferPause.Name = "toolStripItemQTransferPause"
        Me.toolStripItemQTransferPause.Size = New System.Drawing.Size(163, 22)
        Me.toolStripItemQTransferPause.Text = "Pause"
        '
        'toolStripItemQTransferResume
        '
        Me.toolStripItemQTransferResume.Name = "toolStripItemQTransferResume"
        Me.toolStripItemQTransferResume.Size = New System.Drawing.Size(163, 22)
        Me.toolStripItemQTransferResume.Text = "Resume"
        '
        'tStripSepQTop
        '
        Me.tStripSepQTop.Name = "tStripSepQTop"
        Me.tStripSepQTop.Size = New System.Drawing.Size(160, 6)
        '
        'toolStripItemQTransferCancel
        '
        Me.toolStripItemQTransferCancel.Name = "toolStripItemQTransferCancel"
        Me.toolStripItemQTransferCancel.Size = New System.Drawing.Size(163, 22)
        Me.toolStripItemQTransferCancel.Text = "Cancel"
        '
        'toolStripItemQRetry
        '
        Me.toolStripItemQRetry.Name = "toolStripItemQRetry"
        Me.toolStripItemQRetry.Size = New System.Drawing.Size(163, 22)
        Me.toolStripItemQRetry.Text = "Retry"
        '
        'tStripSepQBottom
        '
        Me.tStripSepQBottom.Name = "tStripSepQBottom"
        Me.tStripSepQBottom.Size = New System.Drawing.Size(160, 6)
        '
        'toolStripItemQClear
        '
        Me.toolStripItemQClear.Name = "toolStripItemQClear"
        Me.toolStripItemQClear.Size = New System.Drawing.Size(163, 22)
        Me.toolStripItemQClear.Text = "Clear"
        '
        'toolStripItemQClearFinished
        '
        Me.toolStripItemQClearFinished.Name = "toolStripItemQClearFinished"
        Me.toolStripItemQClearFinished.Size = New System.Drawing.Size(163, 22)
        Me.toolStripItemQClearFinished.Text = "Clear Completed"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(160, 6)
        '
        'toolStripItemQOpenDir
        '
        Me.toolStripItemQOpenDir.Name = "toolStripItemQOpenDir"
        Me.toolStripItemQOpenDir.Size = New System.Drawing.Size(163, 22)
        Me.toolStripItemQOpenDir.Text = "Open Directory"
        '
        'headerFormatStyleBoldArial
        '
        HeaderStateStyle1.BackColor = System.Drawing.SystemColors.ButtonFace
        HeaderStateStyle1.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        HeaderStateStyle1.ForeColor = System.Drawing.Color.Black
        HeaderStateStyle1.FrameColor = System.Drawing.SystemColors.ScrollBar
        HeaderStateStyle1.FrameWidth = 1.0!
        Me.headerFormatStyleBoldArial.Hot = HeaderStateStyle1
        HeaderStateStyle2.BackColor = System.Drawing.SystemColors.ButtonFace
        HeaderStateStyle2.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        HeaderStateStyle2.ForeColor = System.Drawing.Color.Black
        HeaderStateStyle2.FrameColor = System.Drawing.SystemColors.ButtonFace
        HeaderStateStyle2.FrameWidth = 1.0!
        Me.headerFormatStyleBoldArial.Normal = HeaderStateStyle2
        HeaderStateStyle3.BackColor = System.Drawing.SystemColors.ButtonFace
        HeaderStateStyle3.Font = New System.Drawing.Font("Arial Narrow", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        HeaderStateStyle3.ForeColor = System.Drawing.Color.Black
        HeaderStateStyle3.FrameColor = System.Drawing.SystemColors.ScrollBar
        HeaderStateStyle3.FrameWidth = 1.0!
        Me.headerFormatStyleBoldArial.Pressed = HeaderStateStyle3
        '
        'tabControlMain
        '
        Me.tabControlMain.Controls.Add(Me.tabPageDrive)
        Me.tabControlMain.Controls.Add(Me.tabPageOptions)
        Me.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlMain.Location = New System.Drawing.Point(0, 0)
        Me.tabControlMain.Name = "tabControlMain"
        Me.tabControlMain.SelectedIndex = 0
        Me.tabControlMain.Size = New System.Drawing.Size(722, 626)
        Me.tabControlMain.TabIndex = 15
        '
        'tabPageDrive
        '
        Me.tabPageDrive.Controls.Add(Me.splitContainerMain)
        Me.tabPageDrive.Location = New System.Drawing.Point(4, 22)
        Me.tabPageDrive.Name = "tabPageDrive"
        Me.tabPageDrive.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPageDrive.Size = New System.Drawing.Size(714, 600)
        Me.tabPageDrive.TabIndex = 0
        Me.tabPageDrive.Text = "Drive"
        Me.tabPageDrive.UseVisualStyleBackColor = True
        '
        'splitContainerMain
        '
        Me.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainerMain.Location = New System.Drawing.Point(3, 3)
        Me.splitContainerMain.Name = "splitContainerMain"
        Me.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContainerMain.Panel1
        '
        Me.splitContainerMain.Panel1.Controls.Add(Me.objTreeExplorer)
        Me.splitContainerMain.Panel1.Controls.Add(Me.tStripExplorer)
        '
        'splitContainerMain.Panel2
        '
        Me.splitContainerMain.Panel2.Controls.Add(Me.splitContainerLower)
        Me.splitContainerMain.Panel2.Controls.Add(Me.tStripQueue)
        Me.splitContainerMain.Size = New System.Drawing.Size(708, 594)
        Me.splitContainerMain.SplitterDistance = 311
        Me.splitContainerMain.TabIndex = 17
        '
        'objTreeExplorer
        '
        Me.objTreeExplorer.AllColumns.Add(Me.olvColExpTitle)
        Me.objTreeExplorer.AllColumns.Add(Me.olvColExpFileSize)
        Me.objTreeExplorer.AllColumns.Add(Me.olvColExpModifiedDate)
        Me.objTreeExplorer.AllColumns.Add(Me.olvColExpMimeType)
        Me.objTreeExplorer.AllColumns.Add(Me.olvColExpDescription)
        Me.objTreeExplorer.AllColumns.Add(Me.olvColExpQuotaBytesUsed)
        Me.objTreeExplorer.AllColumns.Add(Me.olvColExpExtension)
        Me.objTreeExplorer.AlternateRowBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.objTreeExplorer.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.F2Only
        Me.objTreeExplorer.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.olvColExpTitle, Me.olvColExpFileSize, Me.olvColExpModifiedDate, Me.olvColExpMimeType, Me.olvColExpDescription})
        Me.objTreeExplorer.ContextMenuStrip = Me.contextMenuExplorerFiles
        Me.objTreeExplorer.Cursor = System.Windows.Forms.Cursors.Default
        Me.objTreeExplorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.objTreeExplorer.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.objTreeExplorer.FullRowSelect = True
        Me.objTreeExplorer.HeaderFont = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.objTreeExplorer.HeaderFormatStyle = Me.headerFormatStyleBoldArial
        Me.objTreeExplorer.HeaderUsesThemes = False
        Me.objTreeExplorer.HighlightBackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.objTreeExplorer.HotItemStyle = Me.hotItemStyleExplorer
        Me.objTreeExplorer.IsSimpleDragSource = True
        Me.objTreeExplorer.IsSimpleDropSink = True
        Me.objTreeExplorer.Location = New System.Drawing.Point(0, 31)
        Me.objTreeExplorer.Name = "objTreeExplorer"
        Me.objTreeExplorer.OwnerDraw = True
        Me.objTreeExplorer.ShowGroups = False
        Me.objTreeExplorer.Size = New System.Drawing.Size(708, 280)
        Me.objTreeExplorer.SmallImageList = Me.docIconsSmall
        Me.objTreeExplorer.TabIndex = 16
        Me.objTreeExplorer.UseAlternatingBackColors = True
        Me.objTreeExplorer.UseCellFormatEvents = True
        Me.objTreeExplorer.UseCompatibleStateImageBehavior = False
        Me.objTreeExplorer.UseFiltering = True
        Me.objTreeExplorer.UseHotItem = True
        Me.objTreeExplorer.View = System.Windows.Forms.View.Details
        Me.objTreeExplorer.VirtualMode = True
        '
        'olvColExpTitle
        '
        Me.olvColExpTitle.AspectName = "Title"
        Me.olvColExpTitle.CellPadding = Nothing
        Me.olvColExpTitle.Text = "Title"
        Me.olvColExpTitle.Width = 167
        '
        'olvColExpFileSize
        '
        Me.olvColExpFileSize.AspectName = "FileSize"
        Me.olvColExpFileSize.CellPadding = Nothing
        Me.olvColExpFileSize.IsEditable = False
        Me.olvColExpFileSize.Text = "File Size"
        Me.olvColExpFileSize.UseFiltering = False
        Me.olvColExpFileSize.Width = 76
        '
        'olvColExpModifiedDate
        '
        Me.olvColExpModifiedDate.AspectName = "ModifiedDate"
        Me.olvColExpModifiedDate.CellPadding = Nothing
        Me.olvColExpModifiedDate.IsEditable = False
        Me.olvColExpModifiedDate.Text = "Last Modified"
        Me.olvColExpModifiedDate.Width = 148
        '
        'olvColExpMimeType
        '
        Me.olvColExpMimeType.AspectName = "MimeType"
        Me.olvColExpMimeType.CellPadding = Nothing
        Me.olvColExpMimeType.IsEditable = False
        Me.olvColExpMimeType.Text = "Mime Type"
        Me.olvColExpMimeType.Width = 200
        '
        'olvColExpDescription
        '
        Me.olvColExpDescription.AspectName = "Description"
        Me.olvColExpDescription.CellPadding = Nothing
        Me.olvColExpDescription.FillsFreeSpace = True
        Me.olvColExpDescription.IsEditable = False
        Me.olvColExpDescription.Text = "Description"
        Me.olvColExpDescription.UseFiltering = False
        Me.olvColExpDescription.Width = 219
        '
        'olvColExpQuotaBytesUsed
        '
        Me.olvColExpQuotaBytesUsed.AspectName = "QuotaBytesUsed"
        Me.olvColExpQuotaBytesUsed.CellPadding = Nothing
        Me.olvColExpQuotaBytesUsed.DisplayIndex = 2
        Me.olvColExpQuotaBytesUsed.IsEditable = False
        Me.olvColExpQuotaBytesUsed.IsVisible = False
        Me.olvColExpQuotaBytesUsed.Text = "Uses Quota Space"
        Me.olvColExpQuotaBytesUsed.UseFiltering = False
        Me.olvColExpQuotaBytesUsed.Width = 136
        '
        'olvColExpExtension
        '
        Me.olvColExpExtension.AspectName = "FileExtension"
        Me.olvColExpExtension.CellPadding = Nothing
        Me.olvColExpExtension.DisplayIndex = 5
        Me.olvColExpExtension.IsEditable = False
        Me.olvColExpExtension.IsVisible = False
        Me.olvColExpExtension.Text = "File Extension"
        Me.olvColExpExtension.Width = 110
        '
        'contextMenuExplorerFiles
        '
        Me.contextMenuExplorerFiles.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripExpFilesDownload, Me.ToolStripSeparator4, Me.tStripExpFilesDownloadTo, Me.ToolStripSeparator5, Me.tStripExpFilesDownloadAs, Me.ToolStripSeparator9, Me.tStripExpFilesOpen, Me.ToolStripSeparator6, Me.tStripExpFilesNewFolder, Me.ToolStripSeparator10, Me.tStripExpFilesRename, Me.ToolStripSeparator7, Me.tStripExpFilesTrash, Me.ToolStripSeparator8, Me.tStripExpFilesDelete, Me.ToolStripSeparator1, Me.tStripExpFilesRefresh, Me.ToolStripSeparator2, Me.tStripExpFilesStar, Me.ToolStripSeparator11, Me.tStripExpFilesEditDescription})
        Me.contextMenuExplorerFiles.Name = "contextMenuExplorerFiles"
        Me.contextMenuExplorerFiles.Size = New System.Drawing.Size(234, 306)
        '
        'tStripExpFilesDownload
        '
        Me.tStripExpFilesDownload.Name = "tStripExpFilesDownload"
        Me.tStripExpFilesDownload.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.tStripExpFilesDownload.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesDownload.Text = "Download"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesDownloadTo
        '
        Me.tStripExpFilesDownloadTo.Name = "tStripExpFilesDownloadTo"
        Me.tStripExpFilesDownloadTo.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.tStripExpFilesDownloadTo.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesDownloadTo.Text = "Download to..."
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesDownloadAs
        '
        Me.tStripExpFilesDownloadAs.Name = "tStripExpFilesDownloadAs"
        Me.tStripExpFilesDownloadAs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.tStripExpFilesDownloadAs.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesDownloadAs.Text = "Download As..."
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesOpen
        '
        Me.tStripExpFilesOpen.Name = "tStripExpFilesOpen"
        Me.tStripExpFilesOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.tStripExpFilesOpen.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesOpen.Text = "Open in Browser"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesNewFolder
        '
        Me.tStripExpFilesNewFolder.Name = "tStripExpFilesNewFolder"
        Me.tStripExpFilesNewFolder.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.tStripExpFilesNewFolder.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesNewFolder.Text = "New Folder"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesRename
        '
        Me.tStripExpFilesRename.Name = "tStripExpFilesRename"
        Me.tStripExpFilesRename.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.tStripExpFilesRename.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesRename.Text = "Rename"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesTrash
        '
        Me.tStripExpFilesTrash.Name = "tStripExpFilesTrash"
        Me.tStripExpFilesTrash.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.tStripExpFilesTrash.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesTrash.Text = "Move to Trash"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesDelete
        '
        Me.tStripExpFilesDelete.Name = "tStripExpFilesDelete"
        Me.tStripExpFilesDelete.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.Delete), System.Windows.Forms.Keys)
        Me.tStripExpFilesDelete.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesDelete.Text = "Delete Permanently"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesRefresh
        '
        Me.tStripExpFilesRefresh.Name = "tStripExpFilesRefresh"
        Me.tStripExpFilesRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.tStripExpFilesRefresh.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesRefresh.Text = "Refresh"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesStar
        '
        Me.tStripExpFilesStar.Name = "tStripExpFilesStar"
        Me.tStripExpFilesStar.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Space), System.Windows.Forms.Keys)
        Me.tStripExpFilesStar.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesStar.Text = "Toggle Star"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(230, 6)
        '
        'tStripExpFilesEditDescription
        '
        Me.tStripExpFilesEditDescription.Name = "tStripExpFilesEditDescription"
        Me.tStripExpFilesEditDescription.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.tStripExpFilesEditDescription.Size = New System.Drawing.Size(233, 22)
        Me.tStripExpFilesEditDescription.Text = "Edit Description"
        Me.tStripExpFilesEditDescription.ToolTipText = "Edit Description of Selected Files"
        '
        'hotItemStyleExplorer
        '
        Me.hotItemStyleExplorer.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.hotItemStyleExplorer.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'tStripExplorer
        '
        Me.tStripExplorer.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.tStripExplorer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripBtnExpNewFolder, Me.tStripBtnExpDelete, Me.tStripBtnExpTrash, Me.tStripBtnExpDownload, Me.tStripBtnExpOpen, Me.tStripBtnExpStar, Me.tStripBtnExpRefresh, Me.tStripBtnExpAbout})
        Me.tStripExplorer.Location = New System.Drawing.Point(0, 0)
        Me.tStripExplorer.Name = "tStripExplorer"
        Me.tStripExplorer.Size = New System.Drawing.Size(708, 31)
        Me.tStripExplorer.TabIndex = 18
        Me.tStripExplorer.Text = "ToolStrip1"
        '
        'tStripBtnExpNewFolder
        '
        Me.tStripBtnExpNewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpNewFolder.Image = CType(resources.GetObject("tStripBtnExpNewFolder.Image"), System.Drawing.Image)
        Me.tStripBtnExpNewFolder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpNewFolder.Name = "tStripBtnExpNewFolder"
        Me.tStripBtnExpNewFolder.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnExpNewFolder.Text = "New Folder"
        Me.tStripBtnExpNewFolder.ToolTipText = "Create New Folder in selected directory"
        '
        'tStripBtnExpDelete
        '
        Me.tStripBtnExpDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpDelete.Image = CType(resources.GetObject("tStripBtnExpDelete.Image"), System.Drawing.Image)
        Me.tStripBtnExpDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpDelete.Name = "tStripBtnExpDelete"
        Me.tStripBtnExpDelete.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnExpDelete.Text = "Delete"
        Me.tStripBtnExpDelete.ToolTipText = "Delete Selected Files/Folders"
        '
        'tStripBtnExpTrash
        '
        Me.tStripBtnExpTrash.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpTrash.Image = CType(resources.GetObject("tStripBtnExpTrash.Image"), System.Drawing.Image)
        Me.tStripBtnExpTrash.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpTrash.Name = "tStripBtnExpTrash"
        Me.tStripBtnExpTrash.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnExpTrash.Text = "Trash"
        Me.tStripBtnExpTrash.ToolTipText = "Trash Selected Files/Folders"
        '
        'tStripBtnExpDownload
        '
        Me.tStripBtnExpDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpDownload.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripMenuItemExpAddDownload, Me.tStripMenuItemExpAddDownloadTo, Me.tStripMenuItemExpAddDownloadAs})
        Me.tStripBtnExpDownload.Image = CType(resources.GetObject("tStripBtnExpDownload.Image"), System.Drawing.Image)
        Me.tStripBtnExpDownload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpDownload.Name = "tStripBtnExpDownload"
        Me.tStripBtnExpDownload.Size = New System.Drawing.Size(37, 28)
        Me.tStripBtnExpDownload.Text = "Download"
        Me.tStripBtnExpDownload.ToolTipText = "Download Selected Files/Folders"
        '
        'tStripMenuItemExpAddDownload
        '
        Me.tStripMenuItemExpAddDownload.Name = "tStripMenuItemExpAddDownload"
        Me.tStripMenuItemExpAddDownload.Size = New System.Drawing.Size(205, 22)
        Me.tStripMenuItemExpAddDownload.Text = "Add to Download Queue"
        Me.tStripMenuItemExpAddDownload.ToolTipText = "Add Selected Files/Folders to Queue"
        '
        'tStripMenuItemExpAddDownloadTo
        '
        Me.tStripMenuItemExpAddDownloadTo.Name = "tStripMenuItemExpAddDownloadTo"
        Me.tStripMenuItemExpAddDownloadTo.Size = New System.Drawing.Size(205, 22)
        Me.tStripMenuItemExpAddDownloadTo.Text = "Download To..."
        Me.tStripMenuItemExpAddDownloadTo.ToolTipText = "Download Selected Files/Folders to Custom Location"
        '
        'tStripMenuItemExpAddDownloadAs
        '
        Me.tStripMenuItemExpAddDownloadAs.Name = "tStripMenuItemExpAddDownloadAs"
        Me.tStripMenuItemExpAddDownloadAs.Size = New System.Drawing.Size(205, 22)
        Me.tStripMenuItemExpAddDownloadAs.Text = "Download As..."
        Me.tStripMenuItemExpAddDownloadAs.ToolTipText = "Download Single File/Folder As Custom Filename"
        '
        'tStripBtnExpOpen
        '
        Me.tStripBtnExpOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpOpen.Image = CType(resources.GetObject("tStripBtnExpOpen.Image"), System.Drawing.Image)
        Me.tStripBtnExpOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpOpen.Name = "tStripBtnExpOpen"
        Me.tStripBtnExpOpen.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnExpOpen.Text = "Open"
        Me.tStripBtnExpOpen.ToolTipText = "Open Selected Files/Folders in Web Browser"
        '
        'tStripBtnExpStar
        '
        Me.tStripBtnExpStar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpStar.Image = CType(resources.GetObject("tStripBtnExpStar.Image"), System.Drawing.Image)
        Me.tStripBtnExpStar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpStar.Name = "tStripBtnExpStar"
        Me.tStripBtnExpStar.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnExpStar.Text = "Star"
        Me.tStripBtnExpStar.ToolTipText = "Toggle Stars on Selected Files/Folders"
        '
        'tStripBtnExpRefresh
        '
        Me.tStripBtnExpRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpRefresh.Image = CType(resources.GetObject("tStripBtnExpRefresh.Image"), System.Drawing.Image)
        Me.tStripBtnExpRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpRefresh.Name = "tStripBtnExpRefresh"
        Me.tStripBtnExpRefresh.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnExpRefresh.Text = "Refresh"
        Me.tStripBtnExpRefresh.ToolTipText = "Refresh All Visible Folders"
        '
        'tStripBtnExpAbout
        '
        Me.tStripBtnExpAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tStripBtnExpAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnExpAbout.Image = CType(resources.GetObject("tStripBtnExpAbout.Image"), System.Drawing.Image)
        Me.tStripBtnExpAbout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnExpAbout.Name = "tStripBtnExpAbout"
        Me.tStripBtnExpAbout.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnExpAbout.Text = "About"
        Me.tStripBtnExpAbout.ToolTipText = "About MultiDrive"
        '
        'splitContainerLower
        '
        Me.splitContainerLower.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainerLower.Location = New System.Drawing.Point(0, 31)
        Me.splitContainerLower.Name = "splitContainerLower"
        Me.splitContainerLower.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContainerLower.Panel1
        '
        Me.splitContainerLower.Panel1.Controls.Add(Me.objListViewTransfers)
        '
        'splitContainerLower.Panel2
        '
        Me.splitContainerLower.Panel2.Controls.Add(Me.objLViewLog)
        Me.splitContainerLower.Size = New System.Drawing.Size(708, 248)
        Me.splitContainerLower.SplitterDistance = 132
        Me.splitContainerLower.TabIndex = 0
        '
        'objLViewLog
        '
        Me.objLViewLog.AllColumns.Add(Me.olvLogTime)
        Me.objLViewLog.AllColumns.Add(Me.olvLogMessage)
        Me.objLViewLog.AlternateRowBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.objLViewLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.olvLogTime, Me.olvLogMessage})
        Me.objLViewLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.objLViewLog.FullRowSelect = True
        Me.objLViewLog.Location = New System.Drawing.Point(0, 0)
        Me.objLViewLog.Name = "objLViewLog"
        Me.objLViewLog.OwnerDraw = True
        Me.objLViewLog.Size = New System.Drawing.Size(708, 112)
        Me.objLViewLog.SmallImageList = Me.docIconsSmall
        Me.objLViewLog.TabIndex = 14
        Me.objLViewLog.UseAlternatingBackColors = True
        Me.objLViewLog.UseCompatibleStateImageBehavior = False
        Me.objLViewLog.View = System.Windows.Forms.View.Details
        '
        'olvLogTime
        '
        Me.olvLogTime.CellPadding = Nothing
        Me.olvLogTime.Groupable = False
        Me.olvLogTime.Text = "Time"
        Me.olvLogTime.Width = 55
        '
        'olvLogMessage
        '
        Me.olvLogMessage.AspectName = "Message"
        Me.olvLogMessage.CellPadding = Nothing
        Me.olvLogMessage.FillsFreeSpace = True
        Me.olvLogMessage.Text = "Log Message:"
        Me.olvLogMessage.Width = 835
        '
        'tStripQueue
        '
        Me.tStripQueue.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.tStripQueue.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripBtnQueueStart, Me.tStripBtnQueuePause, Me.tStripBtnQueueCancel, Me.tStripDropBtnQueueClear})
        Me.tStripQueue.Location = New System.Drawing.Point(0, 0)
        Me.tStripQueue.Name = "tStripQueue"
        Me.tStripQueue.Size = New System.Drawing.Size(708, 31)
        Me.tStripQueue.TabIndex = 14
        Me.tStripQueue.Text = "ToolStrip1"
        '
        'tStripBtnQueueStart
        '
        Me.tStripBtnQueueStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnQueueStart.Image = CType(resources.GetObject("tStripBtnQueueStart.Image"), System.Drawing.Image)
        Me.tStripBtnQueueStart.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnQueueStart.Name = "tStripBtnQueueStart"
        Me.tStripBtnQueueStart.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnQueueStart.Text = "Start/Resume"
        Me.tStripBtnQueueStart.ToolTipText = "Start/Resume Selected Transfers"
        '
        'tStripBtnQueuePause
        '
        Me.tStripBtnQueuePause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnQueuePause.Image = CType(resources.GetObject("tStripBtnQueuePause.Image"), System.Drawing.Image)
        Me.tStripBtnQueuePause.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnQueuePause.Name = "tStripBtnQueuePause"
        Me.tStripBtnQueuePause.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnQueuePause.Text = "Pause Transfer"
        Me.tStripBtnQueuePause.ToolTipText = "Pause Selected Transfers"
        '
        'tStripBtnQueueCancel
        '
        Me.tStripBtnQueueCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripBtnQueueCancel.Image = CType(resources.GetObject("tStripBtnQueueCancel.Image"), System.Drawing.Image)
        Me.tStripBtnQueueCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripBtnQueueCancel.Name = "tStripBtnQueueCancel"
        Me.tStripBtnQueueCancel.Size = New System.Drawing.Size(28, 28)
        Me.tStripBtnQueueCancel.Text = "Cancel Transfer"
        Me.tStripBtnQueueCancel.ToolTipText = "Cancel Selected Transfers"
        '
        'tStripDropBtnQueueClear
        '
        Me.tStripDropBtnQueueClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripDropBtnQueueClear.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripMenuItemQueueClearSelected, Me.tStripMenuItemQueueClearCompleted, Me.tStripMenuItemQueueClearInactive})
        Me.tStripDropBtnQueueClear.Image = CType(resources.GetObject("tStripDropBtnQueueClear.Image"), System.Drawing.Image)
        Me.tStripDropBtnQueueClear.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripDropBtnQueueClear.Name = "tStripDropBtnQueueClear"
        Me.tStripDropBtnQueueClear.Size = New System.Drawing.Size(37, 28)
        Me.tStripDropBtnQueueClear.Text = "Clear Queue Items"
        '
        'tStripMenuItemQueueClearSelected
        '
        Me.tStripMenuItemQueueClearSelected.Name = "tStripMenuItemQueueClearSelected"
        Me.tStripMenuItemQueueClearSelected.Size = New System.Drawing.Size(163, 22)
        Me.tStripMenuItemQueueClearSelected.Text = "Clear Selected"
        Me.tStripMenuItemQueueClearSelected.ToolTipText = "Clear Selected Items"
        '
        'tStripMenuItemQueueClearCompleted
        '
        Me.tStripMenuItemQueueClearCompleted.Name = "tStripMenuItemQueueClearCompleted"
        Me.tStripMenuItemQueueClearCompleted.Size = New System.Drawing.Size(163, 22)
        Me.tStripMenuItemQueueClearCompleted.Text = "Clear Completed"
        Me.tStripMenuItemQueueClearCompleted.ToolTipText = "Clear All Completed Transfers"
        '
        'tStripMenuItemQueueClearInactive
        '
        Me.tStripMenuItemQueueClearInactive.Name = "tStripMenuItemQueueClearInactive"
        Me.tStripMenuItemQueueClearInactive.Size = New System.Drawing.Size(163, 22)
        Me.tStripMenuItemQueueClearInactive.Text = "Clear Inactive"
        Me.tStripMenuItemQueueClearInactive.ToolTipText = "Clear All Inactive Transfers"
        '
        'tabPageOptions
        '
        Me.tabPageOptions.Controls.Add(Me.splitContainerOptions)
        Me.tabPageOptions.Location = New System.Drawing.Point(4, 22)
        Me.tabPageOptions.Name = "tabPageOptions"
        Me.tabPageOptions.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPageOptions.Size = New System.Drawing.Size(714, 600)
        Me.tabPageOptions.TabIndex = 1
        Me.tabPageOptions.Text = "Options"
        Me.tabPageOptions.UseVisualStyleBackColor = True
        '
        'splitContainerOptions
        '
        Me.splitContainerOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainerOptions.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.splitContainerOptions.Location = New System.Drawing.Point(3, 3)
        Me.splitContainerOptions.Name = "splitContainerOptions"
        Me.splitContainerOptions.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContainerOptions.Panel1
        '
        Me.splitContainerOptions.Panel1.Controls.Add(Me.GroupBox1)
        Me.splitContainerOptions.Panel1.Controls.Add(Me.grpOptionsDownloads)
        Me.splitContainerOptions.Panel1.Controls.Add(Me.grpBoxMD5)
        Me.splitContainerOptions.Panel1.Controls.Add(Me.toolStripUsers)
        Me.splitContainerOptions.Panel1.Controls.Add(Me.grpBoxOptionsTransfers)
        Me.splitContainerOptions.Panel1.Controls.Add(Me.grpBoxTransferChunkSize)
        Me.splitContainerOptions.Panel1MinSize = 200
        '
        'splitContainerOptions.Panel2
        '
        Me.splitContainerOptions.Panel2.Controls.Add(Me.olvUsers)
        Me.splitContainerOptions.Size = New System.Drawing.Size(708, 594)
        Me.splitContainerOptions.SplitterDistance = 254
        Me.splitContainerOptions.TabIndex = 15
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbTrashed)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(215, 72)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "File Explorer"
        '
        'cmbTrashed
        '
        Me.cmbTrashed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTrashed.FormattingEnabled = True
        Me.cmbTrashed.Items.AddRange(New Object() {"Highlight trashed files", "Show trashed files", "Hide trashed files"})
        Me.cmbTrashed.Location = New System.Drawing.Point(9, 30)
        Me.cmbTrashed.Name = "cmbTrashed"
        Me.cmbTrashed.Size = New System.Drawing.Size(201, 21)
        Me.cmbTrashed.TabIndex = 1
        '
        'grpOptionsDownloads
        '
        Me.grpOptionsDownloads.Controls.Add(Me.btnResetApi)
        Me.grpOptionsDownloads.Controls.Add(Me.btnResetToDefaults)
        Me.grpOptionsDownloads.Controls.Add(Me.Label11)
        Me.grpOptionsDownloads.Controls.Add(Me.cmbFileExistsDecision)
        Me.grpOptionsDownloads.Controls.Add(Me.Label1)
        Me.grpOptionsDownloads.Controls.Add(Me.txtDefaultDownloadLocation)
        Me.grpOptionsDownloads.Controls.Add(Me.btnBrowseDownloadLocation)
        Me.grpOptionsDownloads.Location = New System.Drawing.Point(5, 81)
        Me.grpOptionsDownloads.Name = "grpOptionsDownloads"
        Me.grpOptionsDownloads.Size = New System.Drawing.Size(475, 118)
        Me.grpOptionsDownloads.TabIndex = 10
        Me.grpOptionsDownloads.TabStop = False
        Me.grpOptionsDownloads.Text = "Downloads"
        '
        'btnResetToDefaults
        '
        Me.btnResetToDefaults.Location = New System.Drawing.Point(303, 84)
        Me.btnResetToDefaults.Name = "btnResetToDefaults"
        Me.btnResetToDefaults.Size = New System.Drawing.Size(157, 23)
        Me.btnResetToDefaults.TabIndex = 11
        Me.btnResetToDefaults.Text = "Reset All Options to Defaults"
        Me.btnResetToDefaults.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 89)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(123, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "If local file already exists:"
        '
        'cmbFileExistsDecision
        '
        Me.cmbFileExistsDecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFileExistsDecision.FormattingEnabled = True
        Me.cmbFileExistsDecision.Items.AddRange(New Object() {"Prompt", "Auto Overwrite", "Auto Rename"})
        Me.cmbFileExistsDecision.Location = New System.Drawing.Point(149, 86)
        Me.cmbFileExistsDecision.Name = "cmbFileExistsDecision"
        Me.cmbFileExistsDecision.Size = New System.Drawing.Size(107, 21)
        Me.cmbFileExistsDecision.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Default Download Location:"
        '
        'txtDefaultDownloadLocation
        '
        Me.txtDefaultDownloadLocation.Location = New System.Drawing.Point(151, 28)
        Me.txtDefaultDownloadLocation.Name = "txtDefaultDownloadLocation"
        Me.txtDefaultDownloadLocation.ReadOnly = True
        Me.txtDefaultDownloadLocation.Size = New System.Drawing.Size(309, 20)
        Me.txtDefaultDownloadLocation.TabIndex = 9
        '
        'btnBrowseDownloadLocation
        '
        Me.btnBrowseDownloadLocation.Location = New System.Drawing.Point(151, 51)
        Me.btnBrowseDownloadLocation.Name = "btnBrowseDownloadLocation"
        Me.btnBrowseDownloadLocation.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseDownloadLocation.TabIndex = 10
        Me.btnBrowseDownloadLocation.Text = "Browse..."
        Me.btnBrowseDownloadLocation.UseVisualStyleBackColor = True
        '
        'grpBoxMD5
        '
        Me.grpBoxMD5.Controls.Add(Me.chkBoxOptionsMD5P2P)
        Me.grpBoxMD5.Controls.Add(Me.chkBoxOptionsMD5Uploads)
        Me.grpBoxMD5.Controls.Add(Me.chkBoxOptionsMD5Downloads)
        Me.grpBoxMD5.Location = New System.Drawing.Point(226, 3)
        Me.grpBoxMD5.Name = "grpBoxMD5"
        Me.grpBoxMD5.Size = New System.Drawing.Size(254, 72)
        Me.grpBoxMD5.TabIndex = 13
        Me.grpBoxMD5.TabStop = False
        Me.grpBoxMD5.Text = "MD5 Checksum Verification"
        '
        'chkBoxOptionsMD5P2P
        '
        Me.chkBoxOptionsMD5P2P.AutoSize = True
        Me.chkBoxOptionsMD5P2P.Location = New System.Drawing.Point(169, 36)
        Me.chkBoxOptionsMD5P2P.Name = "chkBoxOptionsMD5P2P"
        Me.chkBoxOptionsMD5P2P.Size = New System.Drawing.Size(85, 17)
        Me.chkBoxOptionsMD5P2P.TabIndex = 15
        Me.chkBoxOptionsMD5P2P.Text = "Peer-to-Peer"
        Me.chkBoxOptionsMD5P2P.UseVisualStyleBackColor = True
        '
        'chkBoxOptionsMD5Uploads
        '
        Me.chkBoxOptionsMD5Uploads.AutoSize = True
        Me.chkBoxOptionsMD5Uploads.Location = New System.Drawing.Point(98, 36)
        Me.chkBoxOptionsMD5Uploads.Name = "chkBoxOptionsMD5Uploads"
        Me.chkBoxOptionsMD5Uploads.Size = New System.Drawing.Size(65, 17)
        Me.chkBoxOptionsMD5Uploads.TabIndex = 14
        Me.chkBoxOptionsMD5Uploads.Text = "Uploads"
        Me.chkBoxOptionsMD5Uploads.UseVisualStyleBackColor = True
        '
        'chkBoxOptionsMD5Downloads
        '
        Me.chkBoxOptionsMD5Downloads.AutoSize = True
        Me.chkBoxOptionsMD5Downloads.Location = New System.Drawing.Point(13, 36)
        Me.chkBoxOptionsMD5Downloads.Name = "chkBoxOptionsMD5Downloads"
        Me.chkBoxOptionsMD5Downloads.Size = New System.Drawing.Size(79, 17)
        Me.chkBoxOptionsMD5Downloads.TabIndex = 13
        Me.chkBoxOptionsMD5Downloads.Text = "Downloads"
        Me.chkBoxOptionsMD5Downloads.UseVisualStyleBackColor = True
        '
        'toolStripUsers
        '
        Me.toolStripUsers.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.toolStripUsers.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.toolStripUsers.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripUsersButtonAddUser, Me.tStripUsersButtonRemoveUser, Me.tStripUsersButtonRemoveAllUsers})
        Me.toolStripUsers.Location = New System.Drawing.Point(0, 215)
        Me.toolStripUsers.Name = "toolStripUsers"
        Me.toolStripUsers.Size = New System.Drawing.Size(708, 39)
        Me.toolStripUsers.TabIndex = 11
        Me.toolStripUsers.Text = "ToolStrip1"
        '
        'tStripUsersButtonAddUser
        '
        Me.tStripUsersButtonAddUser.Image = CType(resources.GetObject("tStripUsersButtonAddUser.Image"), System.Drawing.Image)
        Me.tStripUsersButtonAddUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripUsersButtonAddUser.Name = "tStripUsersButtonAddUser"
        Me.tStripUsersButtonAddUser.Size = New System.Drawing.Size(91, 36)
        Me.tStripUsersButtonAddUser.Text = "Add User"
        '
        'tStripUsersButtonRemoveUser
        '
        Me.tStripUsersButtonRemoveUser.Image = CType(resources.GetObject("tStripUsersButtonRemoveUser.Image"), System.Drawing.Image)
        Me.tStripUsersButtonRemoveUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripUsersButtonRemoveUser.Name = "tStripUsersButtonRemoveUser"
        Me.tStripUsersButtonRemoveUser.Size = New System.Drawing.Size(112, 36)
        Me.tStripUsersButtonRemoveUser.Text = "Remove User"
        Me.tStripUsersButtonRemoveUser.ToolTipText = "Remove User"
        '
        'tStripUsersButtonRemoveAllUsers
        '
        Me.tStripUsersButtonRemoveAllUsers.Image = CType(resources.GetObject("tStripUsersButtonRemoveAllUsers.Image"), System.Drawing.Image)
        Me.tStripUsersButtonRemoveAllUsers.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripUsersButtonRemoveAllUsers.Name = "tStripUsersButtonRemoveAllUsers"
        Me.tStripUsersButtonRemoveAllUsers.Size = New System.Drawing.Size(134, 36)
        Me.tStripUsersButtonRemoveAllUsers.Text = "Remove All Users"
        Me.tStripUsersButtonRemoveAllUsers.ToolTipText = "Remove User"
        '
        'grpBoxOptionsTransfers
        '
        Me.grpBoxOptionsTransfers.Controls.Add(Me.Label10)
        Me.grpBoxOptionsTransfers.Controls.Add(Me.numUpDownSimulPeerToPeer)
        Me.grpBoxOptionsTransfers.Controls.Add(Me.Label3)
        Me.grpBoxOptionsTransfers.Controls.Add(Me.Label2)
        Me.grpBoxOptionsTransfers.Controls.Add(Me.numUpDownSimulUploads)
        Me.grpBoxOptionsTransfers.Controls.Add(Me.numUpDownSimulDownloads)
        Me.grpBoxOptionsTransfers.Location = New System.Drawing.Point(486, 3)
        Me.grpBoxOptionsTransfers.Name = "grpBoxOptionsTransfers"
        Me.grpBoxOptionsTransfers.Size = New System.Drawing.Size(215, 92)
        Me.grpBoxOptionsTransfers.TabIndex = 4
        Me.grpBoxOptionsTransfers.TabStop = False
        Me.grpBoxOptionsTransfers.Text = "Transfers"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 66)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(143, 13)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "Simultaneous P2P Transfers:"
        '
        'numUpDownSimulPeerToPeer
        '
        Me.numUpDownSimulPeerToPeer.Location = New System.Drawing.Point(158, 64)
        Me.numUpDownSimulPeerToPeer.Name = "numUpDownSimulPeerToPeer"
        Me.numUpDownSimulPeerToPeer.Size = New System.Drawing.Size(52, 20)
        Me.numUpDownSimulPeerToPeer.TabIndex = 11
        Me.numUpDownSimulPeerToPeer.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Simultaneous Uploads:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(129, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Simultaneous Downloads:"
        '
        'numUpDownSimulUploads
        '
        Me.numUpDownSimulUploads.Location = New System.Drawing.Point(158, 41)
        Me.numUpDownSimulUploads.Name = "numUpDownSimulUploads"
        Me.numUpDownSimulUploads.Size = New System.Drawing.Size(52, 20)
        Me.numUpDownSimulUploads.TabIndex = 1
        Me.numUpDownSimulUploads.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'numUpDownSimulDownloads
        '
        Me.numUpDownSimulDownloads.Location = New System.Drawing.Point(158, 18)
        Me.numUpDownSimulDownloads.Name = "numUpDownSimulDownloads"
        Me.numUpDownSimulDownloads.Size = New System.Drawing.Size(52, 20)
        Me.numUpDownSimulDownloads.TabIndex = 0
        Me.numUpDownSimulDownloads.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'grpBoxTransferChunkSize
        '
        Me.grpBoxTransferChunkSize.Controls.Add(Me.numUpDownULoadChunkSize)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.numUpDownP2PChunkSize)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.numUpDownDLoadChunkSize)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.Label9)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.Label8)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.Label7)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.Label6)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.Label5)
        Me.grpBoxTransferChunkSize.Controls.Add(Me.Label4)
        Me.grpBoxTransferChunkSize.Location = New System.Drawing.Point(486, 101)
        Me.grpBoxTransferChunkSize.Name = "grpBoxTransferChunkSize"
        Me.grpBoxTransferChunkSize.Size = New System.Drawing.Size(215, 98)
        Me.grpBoxTransferChunkSize.TabIndex = 9
        Me.grpBoxTransferChunkSize.TabStop = False
        Me.grpBoxTransferChunkSize.Text = "Transfer Chunk-Size"
        '
        'numUpDownULoadChunkSize
        '
        Me.numUpDownULoadChunkSize.Increment = New Decimal(New Integer() {256, 0, 0, 0})
        Me.numUpDownULoadChunkSize.Location = New System.Drawing.Point(80, 42)
        Me.numUpDownULoadChunkSize.Maximum = New Decimal(New Integer() {51200, 0, 0, 0})
        Me.numUpDownULoadChunkSize.Minimum = New Decimal(New Integer() {256, 0, 0, 0})
        Me.numUpDownULoadChunkSize.Name = "numUpDownULoadChunkSize"
        Me.numUpDownULoadChunkSize.Size = New System.Drawing.Size(89, 20)
        Me.numUpDownULoadChunkSize.TabIndex = 15
        Me.numUpDownULoadChunkSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numUpDownULoadChunkSize.Value = New Decimal(New Integer() {2048, 0, 0, 0})
        '
        'numUpDownP2PChunkSize
        '
        Me.numUpDownP2PChunkSize.Increment = New Decimal(New Integer() {256, 0, 0, 0})
        Me.numUpDownP2PChunkSize.Location = New System.Drawing.Point(80, 65)
        Me.numUpDownP2PChunkSize.Maximum = New Decimal(New Integer() {51200, 0, 0, 0})
        Me.numUpDownP2PChunkSize.Minimum = New Decimal(New Integer() {256, 0, 0, 0})
        Me.numUpDownP2PChunkSize.Name = "numUpDownP2PChunkSize"
        Me.numUpDownP2PChunkSize.Size = New System.Drawing.Size(89, 20)
        Me.numUpDownP2PChunkSize.TabIndex = 21
        Me.numUpDownP2PChunkSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numUpDownP2PChunkSize.Value = New Decimal(New Integer() {2048, 0, 0, 0})
        '
        'numUpDownDLoadChunkSize
        '
        Me.numUpDownDLoadChunkSize.Increment = New Decimal(New Integer() {256, 0, 0, 0})
        Me.numUpDownDLoadChunkSize.Location = New System.Drawing.Point(80, 19)
        Me.numUpDownDLoadChunkSize.Maximum = New Decimal(New Integer() {51200, 0, 0, 0})
        Me.numUpDownDLoadChunkSize.Minimum = New Decimal(New Integer() {256, 0, 0, 0})
        Me.numUpDownDLoadChunkSize.Name = "numUpDownDLoadChunkSize"
        Me.numUpDownDLoadChunkSize.Size = New System.Drawing.Size(89, 20)
        Me.numUpDownDLoadChunkSize.TabIndex = 20
        Me.numUpDownDLoadChunkSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numUpDownDLoadChunkSize.Value = New Decimal(New Integer() {2048, 0, 0, 0})
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(177, 67)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(20, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Kb"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(177, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(20, 13)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Kb"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(177, 46)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(20, 13)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Kb"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 67)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Peer to Peer:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Download:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 44)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Upload:"
        '
        'olvUsers
        '
        Me.olvUsers.AllColumns.Add(Me.olvColUsersEmail)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersPicture)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersName)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersLoggedIn)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersQuotaUsed)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersQuotaUsedGlobally)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersQuotaFree)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersQuotaTotal)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersQuotaPercentUsed)
        Me.olvUsers.AllColumns.Add(Me.olvColUsersQuotaTrash)
        Me.olvUsers.AlternateRowBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.olvUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.olvColUsersEmail, Me.olvColUsersPicture, Me.olvColUsersName, Me.olvColUsersLoggedIn, Me.olvColUsersQuotaUsed, Me.olvColUsersQuotaFree, Me.olvColUsersQuotaTrash})
        Me.olvUsers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.olvUsers.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.olvUsers.FullRowSelect = True
        Me.olvUsers.HeaderFont = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.olvUsers.HeaderFormatStyle = Me.headerFormatStyleBoldArial
        Me.olvUsers.HeaderUsesThemes = False
        Me.olvUsers.HeaderWordWrap = True
        Me.olvUsers.HighlightBackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.olvUsers.HotItemStyle = Me.hotItemStyleExplorer
        Me.olvUsers.Location = New System.Drawing.Point(0, 0)
        Me.olvUsers.Name = "olvUsers"
        Me.olvUsers.OwnerDraw = True
        Me.olvUsers.Size = New System.Drawing.Size(708, 336)
        Me.olvUsers.SmallImageList = Me.docIconsXL
        Me.olvUsers.TabIndex = 8
        Me.olvUsers.UseAlternatingBackColors = True
        Me.olvUsers.UseCompatibleStateImageBehavior = False
        Me.olvUsers.View = System.Windows.Forms.View.Details
        '
        'olvColUsersEmail
        '
        Me.olvColUsersEmail.AspectName = "Email"
        Me.olvColUsersEmail.CellPadding = Nothing
        Me.olvColUsersEmail.IsEditable = False
        Me.olvColUsersEmail.Text = "Email"
        Me.olvColUsersEmail.Width = 137
        '
        'olvColUsersPicture
        '
        Me.olvColUsersPicture.AspectName = "Email"
        Me.olvColUsersPicture.CellPadding = Nothing
        Me.olvColUsersPicture.Groupable = False
        Me.olvColUsersPicture.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.olvColUsersPicture.IsEditable = False
        Me.olvColUsersPicture.Searchable = False
        Me.olvColUsersPicture.ShowTextInHeader = False
        Me.olvColUsersPicture.Text = "Pic"
        Me.olvColUsersPicture.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.olvColUsersPicture.Width = 101
        '
        'olvColUsersName
        '
        Me.olvColUsersName.AspectName = "Name"
        Me.olvColUsersName.CellPadding = Nothing
        Me.olvColUsersName.Groupable = False
        Me.olvColUsersName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.olvColUsersName.IsEditable = False
        Me.olvColUsersName.Text = "Account Name"
        Me.olvColUsersName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.olvColUsersName.Width = 167
        '
        'olvColUsersLoggedIn
        '
        Me.olvColUsersLoggedIn.AspectName = "LoggedIn"
        Me.olvColUsersLoggedIn.CellPadding = Nothing
        Me.olvColUsersLoggedIn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.olvColUsersLoggedIn.IsEditable = False
        Me.olvColUsersLoggedIn.Text = "Status"
        Me.olvColUsersLoggedIn.Width = 93
        '
        'olvColUsersQuotaUsed
        '
        Me.olvColUsersQuotaUsed.AspectName = "QuotaUsed"
        Me.olvColUsersQuotaUsed.CellPadding = Nothing
        Me.olvColUsersQuotaUsed.Groupable = False
        Me.olvColUsersQuotaUsed.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.olvColUsersQuotaUsed.IsEditable = False
        Me.olvColUsersQuotaUsed.Text = "Drive Usage"
        Me.olvColUsersQuotaUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.olvColUsersQuotaUsed.Width = 142
        Me.olvColUsersQuotaUsed.WordWrap = True
        '
        'olvColUsersQuotaUsedGlobally
        '
        Me.olvColUsersQuotaUsedGlobally.AspectName = "QuotaUsedGlobally"
        Me.olvColUsersQuotaUsedGlobally.CellPadding = Nothing
        Me.olvColUsersQuotaUsedGlobally.DisplayIndex = 4
        Me.olvColUsersQuotaUsedGlobally.Groupable = False
        Me.olvColUsersQuotaUsedGlobally.IsEditable = False
        Me.olvColUsersQuotaUsedGlobally.IsVisible = False
        Me.olvColUsersQuotaUsedGlobally.Text = "Total Acct Usage"
        Me.olvColUsersQuotaUsedGlobally.Width = 98
        '
        'olvColUsersQuotaFree
        '
        Me.olvColUsersQuotaFree.AspectName = "QuotaFree"
        Me.olvColUsersQuotaFree.CellPadding = Nothing
        Me.olvColUsersQuotaFree.Groupable = False
        Me.olvColUsersQuotaFree.IsEditable = False
        Me.olvColUsersQuotaFree.Text = "Free Space"
        Me.olvColUsersQuotaFree.Width = 82
        '
        'olvColUsersQuotaTotal
        '
        Me.olvColUsersQuotaTotal.AspectName = "QuotaTotal"
        Me.olvColUsersQuotaTotal.CellPadding = Nothing
        Me.olvColUsersQuotaTotal.DisplayIndex = 5
        Me.olvColUsersQuotaTotal.IsEditable = False
        Me.olvColUsersQuotaTotal.IsVisible = False
        Me.olvColUsersQuotaTotal.Text = "Total Storage"
        Me.olvColUsersQuotaTotal.Width = 82
        '
        'olvColUsersQuotaPercentUsed
        '
        Me.olvColUsersQuotaPercentUsed.AspectName = "QuotaPercentUsed"
        Me.olvColUsersQuotaPercentUsed.CellPadding = Nothing
        Me.olvColUsersQuotaPercentUsed.DisplayIndex = 6
        Me.olvColUsersQuotaPercentUsed.Groupable = False
        Me.olvColUsersQuotaPercentUsed.IsEditable = False
        Me.olvColUsersQuotaPercentUsed.IsVisible = False
        Me.olvColUsersQuotaPercentUsed.Text = "% Used"
        Me.olvColUsersQuotaPercentUsed.Width = 62
        '
        'olvColUsersQuotaTrash
        '
        Me.olvColUsersQuotaTrash.AspectName = "QuotaTrash"
        Me.olvColUsersQuotaTrash.CellPadding = Nothing
        Me.olvColUsersQuotaTrash.FillsFreeSpace = True
        Me.olvColUsersQuotaTrash.Groupable = False
        Me.olvColUsersQuotaTrash.IsEditable = False
        Me.olvColUsersQuotaTrash.Text = "Trash Usage"
        Me.olvColUsersQuotaTrash.Width = 100
        '
        'docIconsXL
        '
        Me.docIconsXL.ImageStream = CType(resources.GetObject("docIconsXL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.docIconsXL.TransparentColor = System.Drawing.Color.Transparent
        Me.docIconsXL.Images.SetKeyName(0, "lock")
        Me.docIconsXL.Images.SetKeyName(1, "remove")
        '
        'docIconsMedium
        '
        Me.docIconsMedium.ImageStream = CType(resources.GetObject("docIconsMedium.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.docIconsMedium.TransparentColor = System.Drawing.Color.Transparent
        Me.docIconsMedium.Images.SetKeyName(0, "adduser")
        Me.docIconsMedium.Images.SetKeyName(1, "removeuser")
        Me.docIconsMedium.Images.SetKeyName(2, "trash")
        Me.docIconsMedium.Images.SetKeyName(3, "deletefile")
        Me.docIconsMedium.Images.SetKeyName(4, "newFolder")
        Me.docIconsMedium.Images.SetKeyName(5, "download")
        Me.docIconsMedium.Images.SetKeyName(6, "downloadas")
        Me.docIconsMedium.Images.SetKeyName(7, "downloadto")
        Me.docIconsMedium.Images.SetKeyName(8, "addqueue")
        Me.docIconsMedium.Images.SetKeyName(9, "upload")
        Me.docIconsMedium.Images.SetKeyName(10, "error")
        Me.docIconsMedium.Images.SetKeyName(11, "warning")
        Me.docIconsMedium.Images.SetKeyName(12, "edituser")
        Me.docIconsMedium.Images.SetKeyName(13, "removeallusers")
        Me.docIconsMedium.Images.SetKeyName(14, "help")
        Me.docIconsMedium.Images.SetKeyName(15, "information")
        Me.docIconsMedium.Images.SetKeyName(16, "open")
        Me.docIconsMedium.Images.SetKeyName(17, "deletefinishedqueue")
        Me.docIconsMedium.Images.SetKeyName(18, "deletequeue")
        Me.docIconsMedium.Images.SetKeyName(19, "configuration")
        Me.docIconsMedium.Images.SetKeyName(20, "properties")
        Me.docIconsMedium.Images.SetKeyName(21, "search")
        Me.docIconsMedium.Images.SetKeyName(22, "settings")
        Me.docIconsMedium.Images.SetKeyName(23, "lock")
        Me.docIconsMedium.Images.SetKeyName(24, "remove")
        Me.docIconsMedium.Images.SetKeyName(25, "refresh")
        Me.docIconsMedium.Images.SetKeyName(26, "star")
        Me.docIconsMedium.Images.SetKeyName(27, "check")
        Me.docIconsMedium.Images.SetKeyName(28, "pause")
        Me.docIconsMedium.Images.SetKeyName(29, "start")
        '
        'btnResetApi
        '
        Me.btnResetApi.Location = New System.Drawing.Point(303, 55)
        Me.btnResetApi.Name = "btnResetApi"
        Me.btnResetApi.Size = New System.Drawing.Size(157, 23)
        Me.btnResetApi.TabIndex = 12
        Me.btnResetApi.Text = "Reset API Key Info"
        Me.btnResetApi.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(722, 648)
        Me.Controls.Add(Me.tabControlMain)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(738, 469)
        Me.Name = "MainForm"
        Me.Text = "MultiDrive File Explorer"
        CType(Me.objListViewTransfers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.contextMenuQ.ResumeLayout(False)
        Me.tabControlMain.ResumeLayout(False)
        Me.tabPageDrive.ResumeLayout(False)
        Me.splitContainerMain.Panel1.ResumeLayout(False)
        Me.splitContainerMain.Panel1.PerformLayout()
        Me.splitContainerMain.Panel2.ResumeLayout(False)
        Me.splitContainerMain.Panel2.PerformLayout()
        CType(Me.splitContainerMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainerMain.ResumeLayout(False)
        CType(Me.objTreeExplorer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.contextMenuExplorerFiles.ResumeLayout(False)
        Me.tStripExplorer.ResumeLayout(False)
        Me.tStripExplorer.PerformLayout()
        Me.splitContainerLower.Panel1.ResumeLayout(False)
        Me.splitContainerLower.Panel2.ResumeLayout(False)
        CType(Me.splitContainerLower, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainerLower.ResumeLayout(False)
        CType(Me.objLViewLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tStripQueue.ResumeLayout(False)
        Me.tStripQueue.PerformLayout()
        Me.tabPageOptions.ResumeLayout(False)
        Me.splitContainerOptions.Panel1.ResumeLayout(False)
        Me.splitContainerOptions.Panel1.PerformLayout()
        Me.splitContainerOptions.Panel2.ResumeLayout(False)
        CType(Me.splitContainerOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainerOptions.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.grpOptionsDownloads.ResumeLayout(False)
        Me.grpOptionsDownloads.PerformLayout()
        Me.grpBoxMD5.ResumeLayout(False)
        Me.grpBoxMD5.PerformLayout()
        Me.toolStripUsers.ResumeLayout(False)
        Me.toolStripUsers.PerformLayout()
        Me.grpBoxOptionsTransfers.ResumeLayout(False)
        Me.grpBoxOptionsTransfers.PerformLayout()
        CType(Me.numUpDownSimulPeerToPeer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numUpDownSimulUploads, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numUpDownSimulDownloads, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpBoxTransferChunkSize.ResumeLayout(False)
        Me.grpBoxTransferChunkSize.PerformLayout()
        CType(Me.numUpDownULoadChunkSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numUpDownP2PChunkSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numUpDownDLoadChunkSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.olvUsers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents olvColFileName As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColTransferTo As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColProgress As BrightIdeasSoftware.OLVColumn
    Public WithEvents docIconsSmall As System.Windows.Forms.ImageList
    Friend WithEvents olvColTransferSize As BrightIdeasSoftware.OLVColumn
    Friend WithEvents contextMenuQ As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents toolStripItemQTransferPause As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripItemQTransferResume As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripSepQTop As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents toolStripItemQTransferCancel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripItemQRetry As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripSepQBottom As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents toolStripItemQClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripItemQClearFinished As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabControlMain As System.Windows.Forms.TabControl
    Friend WithEvents tabPageDrive As System.Windows.Forms.TabPage
    Friend WithEvents tabPageOptions As System.Windows.Forms.TabPage
    Friend WithEvents grpBoxOptionsTransfers As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents numUpDownSimulUploads As System.Windows.Forms.NumericUpDown
    Friend WithEvents numUpDownSimulDownloads As System.Windows.Forms.NumericUpDown
    Public WithEvents objTreeExplorer As BrightIdeasSoftware.TreeListView
    Friend WithEvents olvColExpTitle As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColExpModifiedDate As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColExpFileSize As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColExpMimeType As BrightIdeasSoftware.OLVColumn
    Friend WithEvents contextMenuExplorerFiles As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tStripExpFilesDownload As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripExpFilesOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripExpFilesRename As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripExpFilesTrash As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripExpFilesDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents olvColExpExtension As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColExpDescription As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColExpQuotaBytesUsed As BrightIdeasSoftware.OLVColumn
    Public WithEvents objListViewTransfers As BrightIdeasSoftware.ObjectListView
    Friend WithEvents olvColUsersEmail As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColUsersQuotaUsed As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColUsersQuotaFree As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColUsersQuotaUsedGlobally As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColUsersQuotaTotal As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColUsersQuotaPercentUsed As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColUsersQuotaTrash As BrightIdeasSoftware.OLVColumn
    Public WithEvents olvUsers As BrightIdeasSoftware.ObjectListView
    Friend WithEvents olvColUsersLoggedIn As BrightIdeasSoftware.OLVColumn
    Friend WithEvents olvColUsersPicture As BrightIdeasSoftware.OLVColumn
    Public WithEvents docIconsXL As System.Windows.Forms.ImageList
    Friend WithEvents olvColUsersName As BrightIdeasSoftware.OLVColumn
    Public WithEvents barRendTransferProgress As BrightIdeasSoftware.BarRenderer
    Friend WithEvents olvColUser As BrightIdeasSoftware.OLVColumn
    Friend WithEvents btnBrowseDownloadLocation As System.Windows.Forms.Button
    Friend WithEvents txtDefaultDownloadLocation As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tStripExpFilesRefresh As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpBoxTransferChunkSize As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents numUpDownSimulPeerToPeer As System.Windows.Forms.NumericUpDown
    Friend WithEvents toolTipFileInfo As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tStripExpFilesStar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents hotItemStyleExplorer As BrightIdeasSoftware.HotItemStyle
    Friend WithEvents objLViewLog As BrightIdeasSoftware.ObjectListView
    Friend WithEvents olvLogMessage As BrightIdeasSoftware.OLVColumn
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents toolStripItemQOpenDir As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents olvLogTime As BrightIdeasSoftware.OLVColumn
    Friend WithEvents splitContainerMain As System.Windows.Forms.SplitContainer
    Friend WithEvents splitContainerLower As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tStripExpFilesDownloadTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripExpFilesDownloadAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents headerFormatStyleBoldArial As BrightIdeasSoftware.HeaderFormatStyle
    Friend WithEvents grpOptionsDownloads As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents cmbFileExistsDecision As System.Windows.Forms.ComboBox
    Friend WithEvents chkBoxOptionsMD5Downloads As System.Windows.Forms.CheckBox
    Friend WithEvents numUpDownP2PChunkSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents numUpDownDLoadChunkSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents numUpDownULoadChunkSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkBoxOptionsMD5Uploads As System.Windows.Forms.CheckBox
    Friend WithEvents toolStripUsers As System.Windows.Forms.ToolStrip
    Friend WithEvents tStripUsersButtonAddUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripUsersButtonRemoveUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents docIconsMedium As System.Windows.Forms.ImageList
    Friend WithEvents grpBoxMD5 As System.Windows.Forms.GroupBox
    Friend WithEvents chkBoxOptionsMD5P2P As System.Windows.Forms.CheckBox
    Friend WithEvents tStripExpFilesNewFolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tStripExplorer As System.Windows.Forms.ToolStrip
    Friend WithEvents tStripBtnExpNewFolder As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripBtnExpDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripBtnExpTrash As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripBtnExpDownload As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tStripMenuItemExpAddDownload As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripMenuItemExpAddDownloadTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripMenuItemExpAddDownloadAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripUsersButtonRemoveAllUsers As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripBtnExpOpen As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripBtnExpStar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripBtnExpRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripQueue As System.Windows.Forms.ToolStrip
    Friend WithEvents tStripBtnQueuePause As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripBtnQueueStart As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripDropBtnQueueClear As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tStripMenuItemQueueClearCompleted As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripMenuItemQueueClearSelected As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripMenuItemQueueClearInactive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripBtnQueueCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmbTrashed As System.Windows.Forms.ComboBox
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tStripExpFilesEditDescription As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents splitContainerOptions As System.Windows.Forms.SplitContainer
    Friend WithEvents tStripBtnExpAbout As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnResetToDefaults As System.Windows.Forms.Button
    Friend WithEvents btnResetApi As System.Windows.Forms.Button

End Class
