Public Partial Class ShowCurrentUser
    Inherits PageBaseQuestionario
    Implements PresentationLayer.IviewQuestionarioPresence

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub


    Public Overrides Function HasPermessi() As Boolean

    End Function

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        SetServiceTitle(Master)
    End Sub

   
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class