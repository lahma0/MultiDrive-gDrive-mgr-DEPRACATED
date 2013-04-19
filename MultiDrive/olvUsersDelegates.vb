Partial Public Class MainForm
    Private Function UsersPicAspectToStringConverter(rowObject As Object) As Object
        Return String.Empty
    End Function
    Private Function UsersPicAspectGetter(rowObject As Object) As Object
        Dim user As GoogleUser = CType(rowObject, GoogleUser)
        Return user.Email
    End Function
    Private Function UsersPicImageGetter(rowObject As Object) As Object
        Dim user As GoogleUser = CType(rowObject, GoogleUser)
        Return user.Email
    End Function
    Private Function UsersStatusAspectToStringConverter(rowObject As Object) As Object
        Return String.Empty
    End Function
    Private Function UsersStatusAspectGetter(rowObject As Object) As Object
        Dim user As GoogleUser = CType(rowObject, GoogleUser)
        Return user.LoggedIn
    End Function
    Private Function UsersStatusImageGetter(rowObject As Object) As Object
        Dim user As GoogleUser = CType(rowObject, GoogleUser)
        If user.LoggedIn Then
            Return "lock"
        Else
            Return "unlock"
        End If
    End Function
End Class
