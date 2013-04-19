Imports MultiDrive.MainForm

Partial Public Class MainForm
    Public Function GetDirectionIcon(rowObject As Object) As Object
        Dim qItem As QueueItem = CType(rowObject, QueueItem)
        If qItem.TransferType = QueueItem.QueueType.Download Then
            Return "download"
        ElseIf qItem.TransferType = QueueItem.QueueType.Upload Then
            Return "upload"
        Else
            Return "peer2peer"
        End If
    End Function

    Public Function GetAuthenticatedIcon(rowObject As Object) As Object
        Dim user As GoogleUser = CType(rowObject, GoogleUser)
        If user.LoggedIn Then
            Return "lock"
        Else
            Return "remove"
        End If
    End Function
    Public Function TransferProgressUpdater(rowObject As Object) As Object
        Dim qItem As QueueItem = CType(rowObject, QueueItem)
        Dim status As QueueItem.StatusEnum = qItem.Status
        If status = QueueItem.StatusEnum.Transferring Or status = QueueItem.StatusEnum.Interrupted Or status = QueueItem.StatusEnum.Paused Then
            Return qItem.Progress
        Else

            'End If
            Return Nothing
        End If
    End Function
    Private Function SizeFormatter(len As Decimal) As String
        Dim sizeTypes() As String = {"b", "Kb", "Mb", "Gb"}

        Dim sizeType As Integer = 0
        Do While len > 1024
            len = Decimal.Round(len / 1024, 2)
            sizeType += 1
            If sizeType >= sizeTypes.Length - 1 Then Exit Do
        Loop

        Dim Resp As String = len.ToString & " " & sizeTypes(sizeType)
        Return Resp
    End Function
    Private Function DateFormatter(rowObject As Object) As Object
        Dim f As Google.Apis.Drive.v2.Data.File = CType(rowObject, Google.Apis.Drive.v2.Data.File)
        Dim d As DateTime = FileManager.ConvertTimeStampToDateTime(f.ModifiedDate)
        Return d.ToUniversalTime()
    End Function
    Private Function FileSizeFormatter(rowObject As Object) As Object
        Dim t As Type = rowObject.GetType
        Dim len As Decimal
        If t.Equals(GetType(QueueItem)) Or t.Equals(GetType(Download)) Or t.Equals(GetType(Upload)) Or t.Equals(GetType(Peer2Peer)) Then
            Dim rowItem As QueueItem = CType(rowObject, QueueItem)
            len = rowItem.TransferSize
        Else
            Dim rowItem As Google.Apis.Drive.v2.Data.File = CType(rowObject, Google.Apis.Drive.v2.Data.File)
            len = rowItem.FileSize
        End If
        Return SizeFormatter(len)
    End Function
    Private Function FileSizeFormatterTrash(rowObject As Object) As Object
        Dim rowItem As GoogleUser = CType(rowObject, GoogleUser)
        Dim len As Decimal = rowItem.QuotaTrash
        Return SizeFormatter(len)
    End Function
    Private Function FileSizeFormatterQuotaUsed(rowObject As Object) As Object
        Dim rowItem As GoogleUser = CType(rowObject, GoogleUser)
        Dim usedLen As Decimal = rowItem.QuotaUsed
        Dim totalLen As Decimal = rowItem.QuotaTotal
        Dim percentUsed As Integer = rowItem.QuotaPercentUsed
        Return percentUsed.ToString & "% Used" & vbNewLine & SizeFormatter(usedLen) & " / " & SizeFormatter(totalLen)
    End Function
    Private Function FileSizeFormatterQuotaUsedGlobally(rowObject As Object) As Object
        Dim rowItem As GoogleUser = CType(rowObject, GoogleUser)
        Dim len As Decimal = rowItem.QuotaUsedGlobally
        Return SizeFormatter(len)
    End Function
    Private Function FileSizeFormatterQuotaTotal(rowObject As Object) As Object
        Dim rowItem As GoogleUser = CType(rowObject, GoogleUser)
        Dim len As Decimal = rowItem.QuotaTotal
        Return SizeFormatter(len)
    End Function
    Private Function FileSizeFormatterQuotaFree(rowObject As Object) As Object
        Dim rowItem As GoogleUser = CType(rowObject, GoogleUser)
        Dim len As Decimal = rowItem.QuotaFree
        Return SizeFormatter(len)
    End Function
    Private Function FileSizeFormatterQuotaPercentUsed(rowObject As Object) As Object
        Dim rowItem As GoogleUser = CType(rowObject, GoogleUser)
        Dim len As Integer = rowItem.QuotaPercentUsed
        Return len.ToString & "%"
    End Function
    'Private Function ApiKeyAspectGetter(rowObject As Object) As Object
    '    Dim user As GoogleUser = CType(rowObject, GoogleUser)
    '    If CredentialsManager.CheckIfApiKeyIsDefault(user.ApiKey) Then
    '        olvColUsersApiKey.IsVisible = False
    '        Return "Default"
    '    Else
    '        Return user.ApiKey
    '    End If
    'End Function
    'Private Function ClientIdAspectGetter(rowObject As Object) As Object
    '    Dim user As GoogleUser = CType(rowObject, GoogleUser)
    '    If CredentialsManager.CheckIfClientIdIsDefault(user.ClientId) Then
    '        olvColUsersClientId.IsVisible = False
    '        Return "Default"
    '    Else
    '        Return user.ApiKey
    '    End If
    'End Function
    'Private Function ClientSecretAspectGetter(rowObject As Object) As Object
    '    Dim user As GoogleUser = CType(rowObject, GoogleUser)
    '    If CredentialsManager.CheckIfClientSecretIsDefault(user.ClientSecret) Then
    '        olvColUsersClientSecret.IsVisible = False
    '        Return "Default"
    '    Else
    '        Return user.ApiKey
    '    End If
    'End Function
    Private Function ProgressGroupKeyGetter(rowObject As Object) As Object
        Dim qItem As QueueItem = CType(rowObject, QueueItem)
        Select Case qItem.Status
            Case QueueItem.StatusEnum.Queued
                Return "Queued"
            Case QueueItem.StatusEnum.Transferring
                Return "Active"
            Case QueueItem.StatusEnum.Cancelled, QueueItem.StatusEnum.Failed
                Return "Inactive"
            Case QueueItem.StatusEnum.Interrupted, QueueItem.StatusEnum.Paused
                Return "Pending"
        End Select
        Return qItem.Status.ToString
    End Function
    Private Function QueueDirectionGroupKeyGetter(rowObject As Object) As Object
        Dim qItem As QueueItem = CType(rowObject, QueueItem)
        Select Case qItem.TransferType
            Case QueueItem.QueueType.Download
                Return "Downloads"
            Case QueueItem.QueueType.Upload
                Return "Uploads"
            Case QueueItem.QueueType.Peer2Peer
                Return "Peer to Peer"
        End Select
        Return Nothing
    End Function
    Private Function UsersGroupKeyGetter(rowObject As Object) As Object
        Dim user As GoogleUser = CType(rowObject, GoogleUser)
        Return user.Mail.Host
    End Function
End Class
