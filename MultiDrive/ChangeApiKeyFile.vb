Public Class ChangeApiKeyFile

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If (textBoxApiKey.Text.Length > 0 And textBoxClientId.Text.Length > 0 And textBoxClientSecret.Text.Length > 0) Then
            Dim apiData As ApiFile = New ApiFile(textBoxApiKey.Text, textBoxClientId.Text, CredentialsManager.Protect(textBoxClientSecret.Text))
            If Not apiData Is Nothing Then
                ApiFile.CreateApiFile(apiData)
            End If
            Me.DialogResult = DialogResult.OK
        Else
            MessageBox.Show("You must complete all fields.")
        End If
    End Sub
End Class