Imports Google.Apis
Imports Google.Apis.Drive.v2
Imports Google.Apis.Drive.v2.Data
Imports Microsoft.Win32
Imports System.IO

Public Class FileManager
    Public Shared Function ConvertTimeStampToDateTime(rfcTimeStamp As String) As DateTime
        Return Xml.XmlConvert.ToDateTime(rfcTimeStamp)
    End Function
    Public Shared Function GetMimeType(ByVal fileName As String) As String
        Dim mimeType As String = "application/unknown"
        Dim ext As String = Path.GetExtension(fileName).ToLower
        Dim regKey As RegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext)
        If ((Not (regKey) Is Nothing) _
                    AndAlso (Not (regKey.GetValue("Content Type")) Is Nothing)) Then
            mimeType = regKey.GetValue("Content Type").ToString
        End If
        Return mimeType
    End Function
    Public Shared Function CompareMd5(inputFile As Data.File, localFilePath As String) As Boolean
        If GetFileMd5(inputFile) = GetFileMd5(localFilePath) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function CompareMd5(inputFile As Data.File, inputFile2 As Data.File)
        If GetFileMd5(inputFile) = GetFileMd5(inputFile2) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetFileMd5(inputFile As Data.File) As String
        Return inputFile.Md5Checksum
    End Function
    Public Shared Function GetFileMd5(inputFilePath As String) As String
        Using rStream As New System.IO.FileStream(inputFilePath, IO.FileMode.Open, IO.FileAccess.Read)
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
                Dim hash As Byte() = md5.ComputeHash(rStream)
                Return ByteArrayToString(hash)
            End Using
        End Using

    End Function
    Private Shared Function ByteArrayToString(ByVal arrInput() As Byte) As String

        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)

        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next

        Return sb.ToString().ToLower

    End Function
    Public Shared Function GetFoldersInFolder(service As DriveService, ByVal folderId As String) As ChildList
        Try
            Dim request As Google.Apis.Drive.v2.ChildrenResource.ListRequest = service.Children.List(folderId)
            request.Q = "mimeType='application/vnd.google-apps.folder'"
            Return request.Fetch()
        Catch ex As Google.GoogleApiRequestException
            Throw
        End Try
    End Function
    Public Shared Function GetAllFolders(service As DriveService) As FileList
        Try
            Dim request As Google.Apis.Drive.v2.FilesResource.ListRequest = service.Files.List
            Dim request2 As Google.Apis.Drive.v2.FilesResource.ListRequest = service.Files.List
            request.Q = "mimeType='application/vnd.google-apps.folder'"
            Return request.Fetch()
        Catch ex As Google.GoogleApiRequestException
            Throw
        End Try
    End Function
    Public Shared Function GetFileListFromIds(service As DriveService, files As List(Of String)) As FileList
        Try
            Dim fileList As New FileList
            For Each fileId As String In files
                fileList.Items.Add(service.Files.Get(fileId).Fetch())
            Next
            Return fileList
        Catch ex As Google.GoogleApiRequestException
            Throw
        End Try

    End Function
    Public Shared Function GetFilesAndFoldersInFolder(ByVal service As DriveService, ByVal folderId As String, Optional getTrashed As Boolean = True) As FileList
        Dim i As Integer = 0
        Dim filesRequest As FilesResource.ListRequest = service.Files.List
        If getTrashed Then
            filesRequest.Q = "'" & folderId & "' in parents"
        Else
            filesRequest.Q = "'" & folderId & "' in parents and trashed = False"
        End If
FetchRequest:
        Try
            Return filesRequest.Fetch
        Catch ex As ArgumentOutOfRangeException
            If i < 3 Then
                i += 1
                GoTo FetchRequest
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function GetFilesInFolder(ByVal service As DriveService, ByVal folderId As String) As FileList
        Try
            Dim filesRequest As FilesResource.ListRequest = service.Files.List
            filesRequest.Q = "'" & folderId & "' in parents and not mimeType='application/vnd.google-apps.folder'"
            Return filesRequest.Fetch
        Catch ex As Google.GoogleApiRequestException
            Throw
        End Try
    End Function
    Public Shared Function MoveFileToNewFolder(service As DriveService, fileIdToMove As String, oldParentId As String, newParentId As String) As Boolean
        Try
            Dim successful As Boolean
            successful = RemoveFileFromFolder(service, oldParentId, fileIdToMove)
            If successful Then
                successful = InsertFileIntoFolder(service, newParentId, fileIdToMove)
                If successful Then
                    Return True
                Else
                    Throw New Exception("Failed to move file to new folder.")
                End If
            Else
                Throw New Exception("Unable to remove original parent folder reference.")
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function InsertFileIntoFolder(service As DriveService, newFolder As String, fileId As String) As Boolean
        Dim newParent As New ParentReference
        Try
            newParent.Id = newFolder
            service.Parents.Insert(newParent, fileId).Fetch()
            Return True
        Catch ex As Google.GoogleApiRequestException
            Return False
        End Try
    End Function
    Public Shared Function RemoveFileFromFolder(service As DriveService, oldParentId As String, fileIdToRemove As String) As Boolean
        Try
            service.Parents.Delete(fileIdToRemove, oldParentId).Fetch()
            Return True
        Catch ex As Google.GoogleApiRequestException
            Return False
        End Try


    End Function
    Public Shared Function IsFileFolder(file As Data.File) As Boolean
        If file.MimeType = "application/vnd.google-apps.folder" Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function IsFileStarred(file As Data.File) As Boolean
        If file.Labels.Starred = True Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Sub ToggleStar(service As DriveService, file As Data.File)
        Dim newFile As New Data.File
        newFile.Labels = file.Labels
        If IsFileStarred(file) Then
            newFile.Labels.Starred = False
        Else
            newFile.Labels.Starred = True
        End If
        Dim request As FilesResource.PatchRequest = service.Files.Patch(newFile, file.Id)
        request.FieldsMask = "labels"
        request.Fetch()
    End Sub
    Public Shared Sub EditDescription(service As DriveService, file As Data.File, description As String)
        Try
            Dim newFile As New Data.File
            newFile.Description = description
            Dim request As FilesResource.PatchRequest = service.Files.Patch(newFile, file.Id)
            request.FieldsMask = "description"
            request.Fetch()
        Catch ex As Google.GoogleApiRequestException
            Throw
        End Try
    End Sub
    'Public Shared Function CheckForChildren(ByVal service As DriveService, ByVal folderId 
    Public Shared Function GetFileById(ByVal service As DriveService, ByVal strFileId As String) As Drive.v2.Data.File
        Try
            Return service.Files.Get(strFileId).Fetch()
        Catch ex As Google.GoogleApiRequestException
            Throw
        End Try
    End Function
    Public Shared Function GetFileExtension(strMimeType As String) As String
        If strMimeType Is Nothing Or strMimeType = "" Or Not strMimeType.Contains("/") Then
            Throw New ArgumentNullException(strMimeType)
        End If

        Dim strExtension As String = ""
        Return If(GetExtensionMappings.TryGetValue(strMimeType, strExtension), strExtension, "")
    End Function

    Public Shared Function RenameFile(service As DriveService, fileId As String, newName As String) As Boolean
        Try
            Dim oldFile As New Drive.v2.Data.File()
            oldFile.Title = newName
            Dim request As FilesResource.PatchRequest = service.Files.Patch(oldFile, fileId)
            request.FieldsMask = "title"
            Dim updatedFile As Drive.v2.Data.File = request.Fetch()
            If updatedFile.Title = newName Then
                Return True
            Else
                Return False
            End If
            Return True
        Catch ex As Google.GoogleApiRequestException
            Return False
        End Try
    End Function
    Public Shared Function DeleteFile(service As DriveService, fileId As String, Optional skipTrash As Boolean = False) As Boolean
        Try
            If skipTrash = True Then
                service.Files.Delete(fileId).Fetch()
                Dim deletedF As New Drive.v2.Data.File
                deletedF.Id = fileId
                If service.Files.List.Fetch.Items.Contains(deletedF) Then
                    Return False
                Else
                    Return True
                End If
            Else
                service.Files.Trash(fileId).Fetch()
                Dim tmpF As Drive.v2.Data.File = service.Files.Get(fileId).Fetch()
                If tmpF.Labels.Trashed = True Then
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Google.GoogleApiRequestException
            Return False
        End Try

    End Function
    Public Shared Function CreateFolder(service As DriveService, folderName As String, parentFolderId As String) As Boolean
        Try
            Dim newParent As New ParentReference
            newParent.Id = parentFolderId
            Dim newFolder As New Data.File
            newFolder.MimeType = "application/vnd.google-apps.folder"
            newFolder.Title = folderName
            newFolder.Parents = {newParent}
            service.Files.Insert(newFolder).Fetch()
            Return True
        Catch ex As Google.GoogleApiRequestException
            Return False
        End Try
    End Function

    Private Shared ReadOnly GetExtensionMappings As IDictionary(Of String, String) = New Dictionary(Of String, String)(StringComparer.InvariantCultureIgnoreCase) From { _
{"application/andrew-inset", ".ez"}, _
{"application/applixware", ".aw"}, _
{"application/atom+xml", ".atom"}, _
{"application/atomcat+xml", ".atomcat"}, _
{"application/atomsvc+xml", ".atomsvc"}, _
{"application/ccxml+xml", ".ccxml"}, _
{"application/cdmi-capability", ".cdmia"}, _
{"application/cdmi-container", ".cdmic"}, _
{"application/cdmi-domain", ".cdmid"}, _
{"application/cdmi-object", ".cdmio"}, _
{"application/cdmi-queue", ".cdmiq"}, _
{"application/cu-seeme", ".cu"}, _
{"application/davmount+xml", ".davmount"}, _
{"application/docbook+xml", ".dbk"}, _
{"application/dssc+der", ".dssc"}, _
{"application/dssc+xml", ".xdssc"}, _
{"application/ecmascript", ".ecma"}, _
{"application/emma+xml", ".emma"}, _
{"application/epub+zip", ".epub"}, _
{"application/exi", ".exi"}, _
{"application/font-tdpfr", ".pfr"}, _
{"application/gml+xml", ".gml"}, _
{"application/gpx+xml", ".gpx"}, _
{"application/gxf", ".gxf"}, _
{"application/hyperstudio", ".stk"}, _
{"application/inkml+xml", ".ink"}, _
{"application/ipfix", ".ipfix"}, _
{"application/java-archive", ".jar"}, _
{"application/java-serialized-object", ".ser"}, _
{"application/java-vm", ".class"}, _
{"application/javascript", ".js"}, _
{"application/json", ".json"}, _
{"application/jsonml+json", ".jsonml"}, _
{"application/lost+xml", ".lostxml"}, _
{"application/mac-binhex40", ".hqx"}, _
{"application/mac-compactpro", ".cpt"}, _
{"application/mads+xml", ".mads"}, _
{"application/marc", ".mrc"}, _
{"application/marcxml+xml", ".mrcx"}, _
{"application/mathematica", ".ma"}, _
{"application/mathml+xml", ".mathml"}, _
{"application/mbox", ".mbox"}, _
{"application/mediaservercontrol+xml", ".mscml"}, _
{"application/metalink+xml", ".metalink"}, _
{"application/metalink4+xml", ".meta4"}, _
{"application/mets+xml", ".mets"}, _
{"application/mods+xml", ".mods"}, _
{"application/mp21", ".m21"}, _
{"application/mp4", ".mp4s"}, _
{"application/msword", ".doc"}, _
{"application/mxf", ".mxf"}, _
{"application/octet-stream", ".bin"}, _
{"application/oda", ".oda"}, _
{"application/oebps-package+xml", ".opf"}, _
{"application/ogg", ".ogx"}, _
{"application/omdoc+xml", ".omdoc"}, _
{"application/onenote", ".onetoc"}, _
{"application/oxps", ".oxps"}, _
{"application/patch-ops-error+xml", ".xer"}, _
{"application/pdf", ".pdf"}, _
{"application/pgp-encrypted", ".pgp"}, _
{"application/pgp-signature", ".asc"}, _
{"application/pics-rules", ".prf"}, _
{"application/pkcs10", ".p10"}, _
{"application/pkcs7-mime", ".p7m"}, _
{"application/pkcs7-signature", ".p7s"}, _
{"application/pkcs8", ".p8"}, _
{"application/pkix-attr-cert", ".ac"}, _
{"application/pkix-cert", ".cer"}, _
{"application/pkix-crl", ".crl"}, _
{"application/pkix-pkipath", ".pkipath"}, _
{"application/pkixcmp", ".pki"}, _
{"application/pls+xml", ".pls"}, _
{"application/postscript", ".ai"}, _
{"application/prs.cww", ".cww"}, _
{"application/pskc+xml", ".pskcxml"}, _
{"application/rdf+xml", ".rdf"}, _
{"application/reginfo+xml", ".rif"}, _
{"application/relax-ng-compact-syntax", ".rnc"}, _
{"application/resource-lists+xml", ".rl"}, _
{"application/resource-lists-diff+xml", ".rld"}, _
{"application/rls-services+xml", ".rs"}, _
{"application/rpki-ghostbusters", ".gbr"}, _
{"application/rpki-manifest", ".mft"}, _
{"application/rpki-roa", ".roa"}, _
{"application/rsd+xml", ".rsd"}, _
{"application/rss+xml", ".rss"}, _
{"application/rtf", ".rtf"}, _
{"application/sbml+xml", ".sbml"}, _
{"application/scvp-cv-request", ".scq"}, _
{"application/scvp-cv-response", ".scs"}, _
{"application/scvp-vp-request", ".spq"}, _
{"application/scvp-vp-response", ".spp"}, _
{"application/sdp", ".sdp"}, _
{"application/set-payment-initiation", ".setpay"}, _
{"application/set-registration-initiation", ".setreg"}, _
{"application/shf+xml", ".shf"}, _
{"application/smil+xml", ".smi"}, _
{"application/sparql-query", ".rq"}, _
{"application/sparql-results+xml", ".srx"}, _
{"application/srgs", ".gram"}, _
{"application/srgs+xml", ".grxml"}, _
{"application/sru+xml", ".sru"}, _
{"application/ssdl+xml", ".ssdl"}, _
{"application/ssml+xml", ".ssml"}, _
{"application/tei+xml", ".tei"}, _
{"application/thraud+xml", ".tfi"}, _
{"application/timestamped-data", ".tsd"}, _
{"application/vnd.3gpp.pic-bw-large", ".plb"}, _
{"application/vnd.3gpp.pic-bw-small", ".psb"}, _
{"application/vnd.3gpp.pic-bw-var", ".pvb"}, _
{"application/vnd.3gpp2.tcap", ".tcap"}, _
{"application/vnd.3m.post-it-notes", ".pwn"}, _
{"application/vnd.accpac.simply.aso", ".aso"}, _
{"application/vnd.accpac.simply.imp", ".imp"}, _
{"application/vnd.acucobol", ".acu"}, _
{"application/vnd.acucorp", ".atc"}, _
{"application/vnd.adobe.air-application-installer-package+zip", ".air"}, _
{"application/vnd.adobe.formscentral.fcdt", ".fcdt"}, _
{"application/vnd.adobe.fxp", ".fxp"}, _
{"application/vnd.adobe.xdp+xml", ".xdp"}, _
{"application/vnd.adobe.xfdf", ".xfdf"}, _
{"application/vnd.ahead.space", ".ahead"}, _
{"application/vnd.airzip.filesecure.azf", ".azf"}, _
{"application/vnd.airzip.filesecure.azs", ".azs"}, _
{"application/vnd.amazon.ebook", ".azw"}, _
{"application/vnd.americandynamics.acc", ".acc"}, _
{"application/vnd.amiga.ami", ".ami"}, _
{"application/vnd.android.package-archive", ".apk"}, _
{"application/vnd.anser-web-certificate-issue-initiation", ".cii"}, _
{"application/vnd.anser-web-funds-transfer-initiation", ".fti"}, _
{"application/vnd.antix.game-component", ".atx"}, _
{"application/vnd.apple.installer+xml", ".mpkg"}, _
{"application/vnd.apple.mpegurl", ".m3u8"}, _
{"application/vnd.aristanetworks.swi", ".swi"}, _
{"application/vnd.astraea-software.iota", ".iota"}, _
{"application/vnd.audiograph", ".aep"}, _
{"application/vnd.blueice.multipass", ".mpm"}, _
{"application/vnd.bmi", ".bmi"}, _
{"application/vnd.businessobjects", ".rep"}, _
{"application/vnd.chemdraw+xml", ".cdxml"}, _
{"application/vnd.chipnuts.karaoke-mmd", ".mmd"}, _
{"application/vnd.cinderella", ".cdy"}, _
{"application/vnd.claymore", ".cla"}, _
{"application/vnd.cloanto.rp9", ".rp9"}, _
{"application/vnd.clonk.c4group", ".c4g"}, _
{"application/vnd.cluetrust.cartomobile-config", ".c11amc"}, _
{"application/vnd.cluetrust.cartomobile-config-pkg", ".c11amz"}, _
{"application/vnd.commonspace", ".csp"}, _
{"application/vnd.contact.cmsg", ".cdbcmsg"}, _
{"application/vnd.cosmocaller", ".cmc"}, _
{"application/vnd.crick.clicker", ".clkx"}, _
{"application/vnd.crick.clicker.keyboard", ".clkk"}, _
{"application/vnd.crick.clicker.palette", ".clkp"}, _
{"application/vnd.crick.clicker.template", ".clkt"}, _
{"application/vnd.crick.clicker.wordbank", ".clkw"}, _
{"application/vnd.criticaltools.wbs+xml", ".wbs"}, _
{"application/vnd.ctc-posml", ".pml"}, _
{"application/vnd.cups-ppd", ".ppd"}, _
{"application/vnd.curl.car", ".car"}, _
{"application/vnd.curl.pcurl", ".pcurl"}, _
{"application/vnd.dart", ".dart"}, _
{"application/vnd.data-vision.rdz", ".rdz"}, _
{"application/vnd.dece.data", ".uvf"}, _
{"application/vnd.dece.ttml+xml", ".uvt"}, _
{"application/vnd.dece.unspecified", ".uvx"}, _
{"application/vnd.dece.zip", ".uvz"}, _
{"application/vnd.denovo.fcselayout-link", ".fe_launch"}, _
{"application/vnd.dna", ".dna"}, _
{"application/vnd.dolby.mlp", ".mlp"}, _
{"application/vnd.dpgraph", ".dpg"}, _
{"application/vnd.dreamfactory", ".dfac"}, _
{"application/vnd.ds-keypoint", ".kpxx"}, _
{"application/vnd.dvb.ait", ".ait"}, _
{"application/vnd.dvb.service", ".svc"}, _
{"application/vnd.dynageo", ".geo"}, _
{"application/vnd.ecowin.chart", ".mag"}, _
{"application/vnd.enliven", ".nml"}, _
{"application/vnd.epson.esf", ".esf"}, _
{"application/vnd.epson.msf", ".msf"}, _
{"application/vnd.epson.quickanime", ".qam"}, _
{"application/vnd.epson.salt", ".slt"}, _
{"application/vnd.epson.ssf", ".ssf"}, _
{"application/vnd.eszigno3+xml", ".es3"}, _
{"application/vnd.ezpix-album", ".ez2"}, _
{"application/vnd.ezpix-package", ".ez3"}, _
{"application/vnd.fdf", ".fdf"}, _
{"application/vnd.fdsn.mseed", ".mseed"}, _
{"application/vnd.fdsn.seed", ".seed"}, _
{"application/vnd.flographit", ".gph"}, _
{"application/vnd.fluxtime.clip", ".ftc"}, _
{"application/vnd.framemaker", ".fm"}, _
{"application/vnd.frogans.fnc", ".fnc"}, _
{"application/vnd.frogans.ltf", ".ltf"}, _
{"application/vnd.fsc.weblaunch", ".fsc"}, _
{"application/vnd.fujitsu.oasys", ".oas"}, _
{"application/vnd.fujitsu.oasys2", ".oa2"}, _
{"application/vnd.fujitsu.oasys3", ".oa3"}, _
{"application/vnd.fujitsu.oasysgp", ".fg5"}, _
{"application/vnd.fujitsu.oasysprs", ".bh2"}, _
{"application/vnd.fujixerox.ddd", ".ddd"}, _
{"application/vnd.fujixerox.docuworks", ".xdw"}, _
{"application/vnd.fujixerox.docuworks.binder", ".xbd"}, _
{"application/vnd.fuzzysheet", ".fzs"}, _
{"application/vnd.genomatix.tuxedo", ".txd"}, _
{"application/vnd.geogebra.file", ".ggb"}, _
{"application/vnd.geogebra.tool", ".ggt"}, _
{"application/vnd.geometry-explorer", ".gex"}, _
{"application/vnd.geonext", ".gxt"}, _
{"application/vnd.geoplan", ".g2w"}, _
{"application/vnd.geospace", ".g3w"}, _
{"application/vnd.gmx", ".gmx"}, _
{"application/vnd.google-earth.kml+xml", ".kml"}, _
{"application/vnd.google-earth.kmz", ".kmz"}, _
{"application/vnd.grafeq", ".gqf"}, _
{"application/vnd.groove-account", ".gac"}, _
{"application/vnd.groove-help", ".ghf"}, _
{"application/vnd.groove-identity-message", ".gim"}, _
{"application/vnd.groove-injector", ".grv"}, _
{"application/vnd.groove-tool-message", ".gtm"}, _
{"application/vnd.groove-tool-template", ".tpl"}, _
{"application/vnd.groove-vcard", ".vcg"}, _
{"application/vnd.hal+xml", ".hal"}, _
{"application/vnd.handheld-entertainment+xml", ".zmm"}, _
{"application/vnd.hbci", ".hbci"}, _
{"application/vnd.hhe.lesson-player", ".les"}, _
{"application/vnd.hp-hpgl", ".hpgl"}, _
{"application/vnd.hp-hpid", ".hpid"}, _
{"application/vnd.hp-hps", ".hps"}, _
{"application/vnd.hp-jlyt", ".jlt"}, _
{"application/vnd.hp-pcl", ".pcl"}, _
{"application/vnd.hp-pclxl", ".pclxl"}, _
{"application/vnd.hydrostatix.sof-data", ".sfd-hdstx"}, _
{"application/vnd.ibm.minipay", ".mpy"}, _
{"application/vnd.ibm.modcap", ".afp"}, _
{"application/vnd.ibm.rights-management", ".irm"}, _
{"application/vnd.ibm.secure-container", ".sc"}, _
{"application/vnd.iccprofile", ".icc"}, _
{"application/vnd.igloader", ".igl"}, _
{"application/vnd.immervision-ivp", ".ivp"}, _
{"application/vnd.immervision-ivu", ".ivu"}, _
{"application/vnd.insors.igm", ".igm"}, _
{"application/vnd.intercon.formnet", ".xpw"}, _
{"application/vnd.intergeo", ".i2g"}, _
{"application/vnd.intu.qbo", ".qbo"}, _
{"application/vnd.intu.qfx", ".qfx"}, _
{"application/vnd.ipunplugged.rcprofile", ".rcprofile"}, _
{"application/vnd.irepository.package+xml", ".irp"}, _
{"application/vnd.is-xpr", ".xpr"}, _
{"application/vnd.isac.fcs", ".fcs"}, _
{"application/vnd.jam", ".jam"}, _
{"application/vnd.jcp.javame.midlet-rms", ".rms"}, _
{"application/vnd.jisp", ".jisp"}, _
{"application/vnd.joost.joda-archive", ".joda"}, _
{"application/vnd.kahootz", ".ktz"}, _
{"application/vnd.kde.karbon", ".karbon"}, _
{"application/vnd.kde.kchart", ".chrt"}, _
{"application/vnd.kde.kformula", ".kfo"}, _
{"application/vnd.kde.kivio", ".flw"}, _
{"application/vnd.kde.kontour", ".kon"}, _
{"application/vnd.kde.kpresenter", ".kpr"}, _
{"application/vnd.kde.kspread", ".ksp"}, _
{"application/vnd.kde.kword", ".kwd"}, _
{"application/vnd.kenameaapp", ".htke"}, _
{"application/vnd.kidspiration", ".kia"}, _
{"application/vnd.kinar", ".kne"}, _
{"application/vnd.koan", ".skp"}, _
{"application/vnd.kodak-descriptor", ".sse"}, _
{"application/vnd.las.las+xml", ".lasxml"}, _
{"application/vnd.llamagraphics.life-balance.desktop", ".lbd"}, _
{"application/vnd.llamagraphics.life-balance.exchange+xml", ".lbe"}, _
{"application/vnd.lotus-1-2-3", ".123"}, _
{"application/vnd.lotus-approach", ".apr"}, _
{"application/vnd.lotus-freelance", ".pre"}, _
{"application/vnd.lotus-notes", ".nsf"}, _
{"application/vnd.lotus-organizer", ".org"}, _
{"application/vnd.lotus-screencam", ".scm"}, _
{"application/vnd.lotus-wordpro", ".lwp"}, _
{"application/vnd.macports.portpkg", ".portpkg"}, _
{"application/vnd.mcd", ".mcd"}, _
{"application/vnd.medcalcdata", ".mc1"}, _
{"application/vnd.mediastation.cdkey", ".cdkey"}, _
{"application/vnd.mfer", ".mwf"}, _
{"application/vnd.mfmp", ".mfm"}, _
{"application/vnd.micrografx.flo", ".flo"}, _
{"application/vnd.micrografx.igx", ".igx"}, _
{"application/vnd.mif", ".mif"}, _
{"application/vnd.mobius.daf", ".daf"}, _
{"application/vnd.mobius.dis", ".dis"}, _
{"application/vnd.mobius.mbk", ".mbk"}, _
{"application/vnd.mobius.mqy", ".mqy"}, _
{"application/vnd.mobius.msl", ".msl"}, _
{"application/vnd.mobius.plc", ".plc"}, _
{"application/vnd.mobius.txf", ".txf"}, _
{"application/vnd.mophun.application", ".mpn"}, _
{"application/vnd.mophun.certificate", ".mpc"}, _
{"application/vnd.mozilla.xul+xml", ".xul"}, _
{"application/vnd.ms-artgalry", ".cil"}, _
{"application/vnd.ms-cab-compressed", ".cab"}, _
{"application/vnd.ms-excel", ".xls"}, _
{"application/vnd.ms-excel.addin.macroenabled.12", ".xlam"}, _
{"application/vnd.ms-excel.sheet.binary.macroenabled.12", ".xlsb"}, _
{"application/vnd.ms-excel.sheet.macroenabled.12", ".xlsm"}, _
{"application/vnd.ms-excel.template.macroenabled.12", ".xltm"}, _
{"application/vnd.ms-fontobject", ".eot"}, _
{"application/vnd.ms-htmlhelp", ".chm"}, _
{"application/vnd.ms-ims", ".ims"}, _
{"application/vnd.ms-lrm", ".lrm"}, _
{"application/vnd.ms-officetheme", ".thmx"}, _
{"application/vnd.ms-pki.seccat", ".cat"}, _
{"application/vnd.ms-pki.stl", ".stl"}, _
{"application/vnd.ms-powerpoint", ".ppt"}, _
{"application/vnd.ms-powerpoint.addin.macroenabled.12", ".ppam"}, _
{"application/vnd.ms-powerpoint.presentation.macroenabled.12", ".pptm"}, _
{"application/vnd.ms-powerpoint.slide.macroenabled.12", ".sldm"}, _
{"application/vnd.ms-powerpoint.slideshow.macroenabled.12", ".ppsm"}, _
{"application/vnd.ms-powerpoint.template.macroenabled.12", ".potm"}, _
{"application/vnd.ms-project", ".mpp"}, _
{"application/vnd.ms-word.document.macroenabled.12", ".docm"}, _
{"application/vnd.ms-word.template.macroenabled.12", ".dotm"}, _
{"application/vnd.ms-works", ".wps"}, _
{"application/vnd.ms-wpl", ".wpl"}, _
{"application/vnd.ms-xpsdocument", ".xps"}, _
{"application/vnd.mseq", ".mseq"}, _
{"application/vnd.musician", ".mus"}, _
{"application/vnd.muvee.style", ".msty"}, _
{"application/vnd.mynfc", ".taglet"}, _
{"application/vnd.neurolanguage.nlu", ".nlu"}, _
{"application/vnd.nitf", ".ntf"}, _
{"application/vnd.noblenet-directory", ".nnd"}, _
{"application/vnd.noblenet-sealer", ".nns"}, _
{"application/vnd.noblenet-web", ".nnw"}, _
{"application/vnd.nokia.n-gage.data", ".ngdat"}, _
{"application/vnd.nokia.n-gage.symbian.install", ".n-gage"}, _
{"application/vnd.nokia.radio-preset", ".rpst"}, _
{"application/vnd.nokia.radio-presets", ".rpss"}, _
{"application/vnd.novadigm.edm", ".edm"}, _
{"application/vnd.novadigm.edx", ".edx"}, _
{"application/vnd.novadigm.ext", ".ext"}, _
{"application/vnd.oasis.opendocument.chart", ".odc"}, _
{"application/vnd.oasis.opendocument.chart-template", ".otc"}, _
{"application/vnd.oasis.opendocument.database", ".odb"}, _
{"application/vnd.oasis.opendocument.formula", ".odf"}, _
{"application/vnd.oasis.opendocument.formula-template", ".odft"}, _
{"application/vnd.oasis.opendocument.graphics", ".odg"}, _
{"application/vnd.oasis.opendocument.graphics-template", ".otg"}, _
{"application/vnd.oasis.opendocument.image", ".odi"}, _
{"application/vnd.oasis.opendocument.image-template", ".oti"}, _
{"application/vnd.oasis.opendocument.presentation", ".odp"}, _
{"application/vnd.oasis.opendocument.presentation-template", ".otp"}, _
{"application/vnd.oasis.opendocument.spreadsheet", ".ods"}, _
{"application/vnd.oasis.opendocument.spreadsheet-template", ".ots"}, _
{"application/vnd.oasis.opendocument.text", ".odt"}, _
{"application/vnd.oasis.opendocument.text-master", ".odm"}, _
{"application/vnd.oasis.opendocument.text-template", ".ott"}, _
{"application/vnd.oasis.opendocument.text-web", ".oth"}, _
{"application/vnd.olpc-sugar", ".xo"}, _
{"application/vnd.oma.dd2+xml", ".dd2"}, _
{"application/vnd.openofficeorg.extension", ".oxt"}, _
{"application/vnd.openxmlformats-officedocument.presentationml.presentation", ".pptx"}, _
{"application/vnd.openxmlformats-officedocument.presentationml.slide", ".sldx"}, _
{"application/vnd.openxmlformats-officedocument.presentationml.slideshow", ".ppsx"}, _
{"application/vnd.openxmlformats-officedocument.presentationml.template", ".potx"}, _
{"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx"}, _
{"application/vnd.openxmlformats-officedocument.spreadsheetml.template", ".xltx"}, _
{"application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx"}, _
{"application/vnd.openxmlformats-officedocument.wordprocessingml.template", ".dotx"}, _
{"application/vnd.osgeo.mapguide.package", ".mgp"}, _
{"application/vnd.osgi.dp", ".dp"}, _
{"application/vnd.osgi.subsystem", ".esa"}, _
{"application/vnd.palm", ".pdb"}, _
{"application/vnd.pawaafile", ".paw"}, _
{"application/vnd.pg.format", ".str"}, _
{"application/vnd.pg.osasli", ".ei6"}, _
{"application/vnd.picsel", ".efif"}, _
{"application/vnd.pmi.widget", ".wg"}, _
{"application/vnd.pocketlearn", ".plf"}, _
{"application/vnd.powerbuilder6", ".pbd"}, _
{"application/vnd.previewsystems.box", ".box"}, _
{"application/vnd.proteus.magazine", ".mgz"}, _
{"application/vnd.publishare-delta-tree", ".qps"}, _
{"application/vnd.pvi.ptid1", ".ptid"}, _
{"application/vnd.quark.quarkxpress", ".qxd"}, _
{"application/vnd.realvnc.bed", ".bed"}, _
{"application/vnd.recordare.musicxml", ".mxl"}, _
{"application/vnd.recordare.musicxml+xml", ".musicxml"}, _
{"application/vnd.rig.cryptonote", ".cryptonote"}, _
{"application/vnd.rim.cod", ".cod"}, _
{"application/vnd.rn-realmedia", ".rm"}, _
{"application/vnd.rn-realmedia-vbr", ".rmvb"}, _
{"application/vnd.route66.link66+xml", ".link66"}, _
{"application/vnd.sailingtracker.track", ".st"}, _
{"application/vnd.seemail", ".see"}, _
{"application/vnd.sema", ".sema"}, _
{"application/vnd.semd", ".semd"}, _
{"application/vnd.semf", ".semf"}, _
{"application/vnd.shana.informed.formdata", ".ifm"}, _
{"application/vnd.shana.informed.formtemplate", ".itp"}, _
{"application/vnd.shana.informed.interchange", ".iif"}, _
{"application/vnd.shana.informed.package", ".ipk"}, _
{"application/vnd.simtech-mindmapper", ".twd"}, _
{"application/vnd.smaf", ".mmf"}, _
{"application/vnd.smart.teacher", ".teacher"}, _
{"application/vnd.solent.sdkm+xml", ".sdkm"}, _
{"application/vnd.spotfire.dxp", ".dxp"}, _
{"application/vnd.spotfire.sfs", ".sfs"}, _
{"application/vnd.stardivision.draw", ".sda"}, _
{"application/vnd.stardivision.impress", ".sdd"}, _
{"application/vnd.stardivision.math", ".smf"}, _
{"application/vnd.stardivision.writer", ".sdw"}, _
{"application/vnd.stardivision.writer-global", ".sgl"}, _
{"application/vnd.stepmania.package", ".smzip"}, _
{"application/vnd.stepmania.stepchart", ".sm"}, _
{"application/vnd.sun.xml.calc", ".sxc"}, _
{"application/vnd.sun.xml.calc.template", ".stc"}, _
{"application/vnd.sun.xml.draw", ".sxd"}, _
{"application/vnd.sun.xml.draw.template", ".std"}, _
{"application/vnd.sun.xml.impress", ".sxi"}, _
{"application/vnd.sun.xml.impress.template", ".sti"}, _
{"application/vnd.sun.xml.math", ".sxm"}, _
{"application/vnd.sun.xml.writer", ".sxw"}, _
{"application/vnd.sun.xml.writer.global", ".sxg"}, _
{"application/vnd.sun.xml.writer.template", ".stw"}, _
{"application/vnd.sus-calendar", ".sus"}, _
{"application/vnd.svd", ".svd"}, _
{"application/vnd.symbian.install", ".sis"}, _
{"application/vnd.syncml+xml", ".xsm"}, _
{"application/vnd.syncml.dm+wbxml", ".bdm"}, _
{"application/vnd.syncml.dm+xml", ".xdm"}, _
{"application/vnd.tao.intent-module-archive", ".tao"}, _
{"application/vnd.tcpdump.pcap", ".pcap"}, _
{"application/vnd.tmobile-livetv", ".tmo"}, _
{"application/vnd.trid.tpt", ".tpt"}, _
{"application/vnd.triscape.mxs", ".mxs"}, _
{"application/vnd.trueapp", ".tra"}, _
{"application/vnd.ufdl", ".ufd"}, _
{"application/vnd.uiq.theme", ".utz"}, _
{"application/vnd.umajin", ".umj"}, _
{"application/vnd.unity", ".unityweb"}, _
{"application/vnd.uoml+xml", ".uoml"}, _
{"application/vnd.vcx", ".vcx"}, _
{"application/vnd.visio", ".vsd"}, _
{"application/vnd.visionary", ".vis"}, _
{"application/vnd.vsf", ".vsf"}, _
{"application/vnd.wap.wbxml", ".wbxml"}, _
{"application/vnd.wap.wmlc", ".wmlc"}, _
{"application/vnd.wap.wmlscriptc", ".wmlsc"}, _
{"application/vnd.webturbo", ".wtb"}, _
{"application/vnd.wolfram.player", ".nbp"}, _
{"application/vnd.wordperfect", ".wpd"}, _
{"application/vnd.wqd", ".wqd"}, _
{"application/vnd.wt.stf", ".stf"}, _
{"application/vnd.xara", ".xar"}, _
{"application/vnd.xfdl", ".xfdl"}, _
{"application/vnd.yamaha.hv-dic", ".hvd"}, _
{"application/vnd.yamaha.hv-script", ".hvs"}, _
{"application/vnd.yamaha.hv-voice", ".hvp"}, _
{"application/vnd.yamaha.openscoreformat", ".osf"}, _
{"application/vnd.yamaha.openscoreformat.osfpvg+xml", ".osfpvg"}, _
{"application/vnd.yamaha.smaf-audio", ".saf"}, _
{"application/vnd.yamaha.smaf-phrase", ".spf"}, _
{"application/vnd.yellowriver-custom-menu", ".cmp"}, _
{"application/vnd.zul", ".zir"}, _
{"application/vnd.zzazz.deck+xml", ".zaz"}, _
{"application/voicexml+xml", ".vxml"}, _
{"application/widget", ".wgt"}, _
{"application/winhlp", ".hlp"}, _
{"application/wsdl+xml", ".wsdl"}, _
{"application/wspolicy+xml", ".wspolicy"}, _
{"application/x-7z-compressed", ".7z"}, _
{"application/x-abiword", ".abw"}, _
{"application/x-ace-compressed", ".ace"}, _
{"application/x-apple-diskimage", ".dmg"}, _
{"application/x-authorware-bin", ".aab"}, _
{"application/x-authorware-map", ".aam"}, _
{"application/x-authorware-seg", ".aas"}, _
{"application/x-bcpio", ".bcpio"}, _
{"application/x-bittorrent", ".torrent"}, _
{"application/x-blorb", ".blb"}, _
{"application/x-bzip", ".bz"}, _
{"application/x-bzip2", ".bz2"}, _
{"application/x-cbr", ".cbr"}, _
{"application/x-cdlink", ".vcd"}, _
{"application/x-cfs-compressed", ".cfs"}, _
{"application/x-chat", ".chat"}, _
{"application/x-chess-pgn", ".pgn"}, _
{"application/x-conference", ".nsc"}, _
{"application/x-cpio", ".cpio"}, _
{"application/x-csh", ".csh"}, _
{"application/x-debian-package", ".deb"}, _
{"application/x-dgc-compressed", ".dgc"}, _
{"application/x-director", ".dir"}, _
{"application/x-doom", ".wad"}, _
{"application/x-dtbncx+xml", ".ncx"}, _
{"application/x-dtbook+xml", ".dtb"}, _
{"application/x-dtbresource+xml", ".res"}, _
{"application/x-dvi", ".dvi"}, _
{"application/x-envoy", ".evy"}, _
{"application/x-eva", ".eva"}, _
{"application/x-font-bdf", ".bdf"}, _
{"application/x-font-ghostscript", ".gsf"}, _
{"application/x-font-linux-psf", ".psf"}, _
{"application/x-font-otf", ".otf"}, _
{"application/x-font-pcf", ".pcf"}, _
{"application/x-font-snf", ".snf"}, _
{"application/x-font-ttf", ".ttf"}, _
{"application/x-font-type1", ".pfa"}, _
{"application/x-font-woff", ".woff"}, _
{"application/x-freearc", ".arc"}, _
{"application/x-futuresplash", ".spl"}, _
{"application/x-gca-compressed", ".gca"}, _
{"application/x-glulx", ".ulx"}, _
{"application/x-gnumeric", ".gnumeric"}, _
{"application/x-gramps-xml", ".gramps"}, _
{"application/x-gtar", ".gtar"}, _
{"application/x-hdf", ".hdf"}, _
{"application/x-install-instructions", ".install"}, _
{"application/x-iso9660-image", ".iso"}, _
{"application/x-java-jnlp-file", ".jnlp"}, _
{"application/x-latex", ".latex"}, _
{"application/x-lzh-compressed", ".lzh"}, _
{"application/x-mie", ".mie"}, _
{"application/x-mobipocket-ebook", ".prc"}, _
{"application/x-ms-application", ".application"}, _
{"application/x-ms-shortcut", ".lnk"}, _
{"application/x-ms-wmd", ".wmd"}, _
{"application/x-ms-wmz", ".wmz"}, _
{"application/x-ms-xbap", ".xbap"}, _
{"application/x-msaccess", ".mdb"}, _
{"application/x-msbinder", ".obd"}, _
{"application/x-mscardfile", ".crd"}, _
{"application/x-msclip", ".clp"}, _
{"application/x-msdownload", ".exe"}, _
{"application/x-msmediaview", ".mvb"}, _
{"application/x-msmetafile", ".wmf"}, _
{"application/x-msmoney", ".mny"}, _
{"application/x-mspublisher", ".pub"}, _
{"application/x-msschedule", ".scd"}, _
{"application/x-msterminal", ".trm"}, _
{"application/x-mswrite", ".wri"}, _
{"application/x-netcdf", ".nc"}, _
{"application/x-nzb", ".nzb"}, _
{"application/x-pkcs12", ".p12"}, _
{"application/x-pkcs7-certificates", ".p7b"}, _
{"application/x-pkcs7-certreqresp", ".p7r"}, _
{"application/x-rar-compressed", ".rar"}, _
{"application/x-research-info-systems", ".ris"}, _
{"application/x-sh", ".sh"}, _
{"application/x-shar", ".shar"}, _
{"application/x-shockwave-flash", ".swf"}, _
{"application/x-silverlight-app", ".xap"}, _
{"application/x-sql", ".sql"}, _
{"application/x-stuffit", ".sit"}, _
{"application/x-stuffitx", ".sitx"}, _
{"application/x-subrip", ".srt"}, _
{"application/x-sv4cpio", ".sv4cpio"}, _
{"application/x-sv4crc", ".sv4crc"}, _
{"application/x-t3vm-image", ".t3"}, _
{"application/x-tads", ".gam"}, _
{"application/x-tar", ".tar"}, _
{"application/x-tcl", ".tcl"}, _
{"application/x-tex", ".tex"}, _
{"application/x-tex-tfm", ".tfm"}, _
{"application/x-texinfo", ".texinfo"}, _
{"application/x-tgif", ".obj"}, _
{"application/x-ustar", ".ustar"}, _
{"application/x-wais-source", ".src"}, _
{"application/x-x509-ca-cert", ".der"}, _
{"application/x-xfig", ".fig"}, _
{"application/x-xliff+xml", ".xlf"}, _
{"application/x-xpinstall", ".xpi"}, _
{"application/x-xz", ".xz"}, _
{"application/x-zmachine", ".z1"}, _
{"application/xaml+xml", ".xaml"}, _
{"application/xcap-diff+xml", ".xdf"}, _
{"application/xenc+xml", ".xenc"}, _
{"application/xhtml+xml", ".xhtml"}, _
{"application/xml", ".xml"}, _
{"application/xml-dtd", ".dtd"}, _
{"application/xop+xml", ".xop"}, _
{"application/xproc+xml", ".xpl"}, _
{"application/xslt+xml", ".xslt"}, _
{"application/xspf+xml", ".xspf"}, _
{"application/xv+xml", ".mxml"}, _
{"application/yang", ".yang"}, _
{"application/yin+xml", ".yin"}, _
{"application/zip", ".zip"}, _
{"audio/adpcm", ".adp"}, _
{"audio/basic", ".au"}, _
{"audio/midi", ".mid"}, _
{"audio/mp4", ".mp4a"}, _
{"audio/mpeg", ".mpga"}, _
{"audio/ogg", ".oga"}, _
{"audio/s3m", ".s3m"}, _
{"audio/silk", ".sil"}, _
{"audio/vnd.dece.audio", ".uva"}, _
{"audio/vnd.digital-winds", ".eol"}, _
{"audio/vnd.dra", ".dra"}, _
{"audio/vnd.dts", ".dts"}, _
{"audio/vnd.dts.hd", ".dtshd"}, _
{"audio/vnd.lucent.voice", ".lvp"}, _
{"audio/vnd.ms-playready.media.pya", ".pya"}, _
{"audio/vnd.nuera.ecelp4800", ".ecelp4800"}, _
{"audio/vnd.nuera.ecelp7470", ".ecelp7470"}, _
{"audio/vnd.nuera.ecelp9600", ".ecelp9600"}, _
{"audio/vnd.rip", ".rip"}, _
{"audio/webm", ".weba"}, _
{"audio/x-aac", ".aac"}, _
{"audio/x-aiff", ".aif"}, _
{"audio/x-caf", ".caf"}, _
{"audio/x-flac", ".flac"}, _
{"audio/x-matroska", ".mka"}, _
{"audio/x-mpegurl", ".m3u"}, _
{"audio/x-ms-wax", ".wax"}, _
{"audio/x-ms-wma", ".wma"}, _
{"audio/x-pn-realaudio", ".ram"}, _
{"audio/x-pn-realaudio-plugin", ".rmp"}, _
{"audio/x-wav", ".wav"}, _
{"audio/xm", ".xm"}, _
{"chemical/x-cdx", ".cdx"}, _
{"chemical/x-cif", ".cif"}, _
{"chemical/x-cmdf", ".cmdf"}, _
{"chemical/x-cml", ".cml"}, _
{"chemical/x-csml", ".csml"}, _
{"chemical/x-xyz", ".xyz"}, _
{"image/bmp", ".bmp"}, _
{"image/cgm", ".cgm"}, _
{"image/g3fax", ".g3"}, _
{"image/gif", ".gif"}, _
{"image/ief", ".ief"}, _
{"image/jpeg", ".jpeg"}, _
{"image/ktx", ".ktx"}, _
{"image/png", ".png"}, _
{"image/prs.btif", ".btif"}, _
{"image/sgi", ".sgi"}, _
{"image/svg+xml", ".svg"}, _
{"image/tiff", ".tiff"}, _
{"image/vnd.adobe.photoshop", ".psd"}, _
{"image/vnd.dece.graphic", ".uvi"}, _
{"image/vnd.dvb.subtitle", ".sub"}, _
{"image/vnd.djvu", ".djvu"}, _
{"image/vnd.dwg", ".dwg"}, _
{"image/vnd.dxf", ".dxf"}, _
{"image/vnd.fastbidsheet", ".fbs"}, _
{"image/vnd.fpx", ".fpx"}, _
{"image/vnd.fst", ".fst"}, _
{"image/vnd.fujixerox.edmics-mmr", ".mmr"}, _
{"image/vnd.fujixerox.edmics-rlc", ".rlc"}, _
{"image/vnd.ms-modi", ".mdi"}, _
{"image/vnd.ms-photo", ".wdp"}, _
{"image/vnd.net-fpx", ".npx"}, _
{"image/vnd.wap.wbmp", ".wbmp"}, _
{"image/vnd.xiff", ".xif"}, _
{"image/webp", ".webp"}, _
{"image/x-3ds", ".3ds"}, _
{"image/x-cmu-raster", ".ras"}, _
{"image/x-cmx", ".cmx"}, _
{"image/x-freehand", ".fh"}, _
{"image/x-icon", ".ico"}, _
{"image/x-mrsid-image", ".sid"}, _
{"image/x-pcx", ".pcx"}, _
{"image/x-pict", ".pic"}, _
{"image/x-portable-anymap", ".pnm"}, _
{"image/x-portable-bitmap", ".pbm"}, _
{"image/x-portable-graymap", ".pgm"}, _
{"image/x-portable-pixmap", ".ppm"}, _
{"image/x-rgb", ".rgb"}, _
{"image/x-tga", ".tga"}, _
{"image/x-xbitmap", ".xbm"}, _
{"image/x-xpixmap", ".xpm"}, _
{"image/x-xwindowdump", ".xwd"}, _
{"message/rfc822", ".eml"}, _
{"model/iges", ".igs"}, _
{"model/mesh", ".msh"}, _
{"model/vnd.collada+xml", ".dae"}, _
{"model/vnd.dwf", ".dwf"}, _
{"model/vnd.gdl", ".gdl"}, _
{"model/vnd.gtw", ".gtw"}, _
{"model/vnd.mts", ".mts"}, _
{"model/vnd.vtu", ".vtu"}, _
{"model/vrml", ".wrl"}, _
{"model/x3d+binary", ".x3db"}, _
{"model/x3d+vrml", ".x3dv"}, _
{"model/x3d+xml", ".x3d"}, _
{"text/cache-manifest", ".appcache"}, _
{"text/calendar", ".ics"}, _
{"text/css", ".css"}, _
{"text/csv", ".csv"}, _
{"text/html", ".html"}, _
{"text/n3", ".n3"}, _
{"text/plain", ".txt"}, _
{"text/prs.lines.tag", ".dsc"}, _
{"text/richtext", ".rtx"}, _
{"text/sgml", ".sgml"}, _
{"text/tab-separated-values", ".tsv"}, _
{"text/troff", ".t"}, _
{"text/turtle", ".ttl"}, _
{"text/uri-list", ".uri"}, _
{"text/vcard", ".vcard"}, _
{"text/vnd.curl", ".curl"}, _
{"text/vnd.curl.dcurl", ".dcurl"}, _
{"text/vnd.curl.scurl", ".scurl"}, _
{"text/vnd.curl.mcurl", ".mcurl"}, _
{"text/vnd.dvb.subtitle", ".sub"}, _
{"text/vnd.fly", ".fly"}, _
{"text/vnd.fmi.flexstor", ".flx"}, _
{"text/vnd.graphviz", ".gv"}, _
{"text/vnd.in3d.3dml", ".3dml"}, _
{"text/vnd.in3d.spot", ".spot"}, _
{"text/vnd.sun.j2me.app-descriptor", ".jad"}, _
{"text/vnd.wap.wml", ".wml"}, _
{"text/vnd.wap.wmlscript", ".wmls"}, _
{"text/x-asm", ".s"}, _
{"text/x-c", ".c"}, _
{"text/x-fortran", ".f"}, _
{"text/x-java-source", ".java"}, _
{"text/x-opml", ".opml"}, _
{"text/x-pascal", ".p"}, _
{"text/x-nfo", ".nfo"}, _
{"text/x-setext", ".etx"}, _
{"text/x-sfv", ".sfv"}, _
{"text/x-uuencode", ".uu"}, _
{"text/x-vcalendar", ".vcs"}, _
{"text/x-vcard", ".vcf"}, _
{"video/3gpp", ".3gp"}, _
{"video/3gpp2", ".3g2"}, _
{"video/h261", ".h261"}, _
{"video/h263", ".h263"}, _
{"video/h264", ".h264"}, _
{"video/jpeg", ".jpgv"}, _
{"video/jpm", ".jpm"}, _
{"video/mj2", ".mj2"}, _
{"video/mp4", ".mp4"}, _
{"video/mpeg", ".mpeg"}, _
{"video/ogg", ".ogv"}, _
{"video/quicktime", ".qt"}, _
{"video/vnd.dece.hd", ".uvh"}, _
{"video/vnd.dece.mobile", ".uvm"}, _
{"video/vnd.dece.pd", ".uvp"}, _
{"video/vnd.dece.sd", ".uvs"}, _
{"video/vnd.dece.video", ".uvv"}, _
{"video/vnd.dvb.file", ".dvb"}, _
{"video/vnd.fvt", ".fvt"}, _
{"video/vnd.mpegurl", ".mxu"}, _
{"video/vnd.ms-playready.media.pyv", ".pyv"}, _
{"video/vnd.uvvu.mp4", ".uvu"}, _
{"video/vnd.vivo", ".viv"}, _
{"video/webm", ".webm"}, _
{"video/x-f4v", ".f4v"}, _
{"video/x-fli", ".fli"}, _
{"video/x-flv", ".flv"}, _
{"video/x-m4v", ".m4v"}, _
{"video/x-matroska", ".mkv"}, _
{"video/x-mng", ".mng"}, _
{"video/x-ms-asf", ".asf"}, _
{"video/x-ms-vob", ".vob"}, _
{"video/x-ms-wm", ".wm"}, _
{"video/x-ms-wmv", ".wmv"}, _
{"video/x-ms-wmx", ".wmx"}, _
{"video/x-ms-wvx", ".wvx"}, _
{"video/x-msvideo", ".avi"}, _
{"video/x-sgi-movie", ".movie"}, _
{"video/x-smv", ".smv"}, _
{"x-conference/x-cooltalk", ".ice"} _
}

End Class
