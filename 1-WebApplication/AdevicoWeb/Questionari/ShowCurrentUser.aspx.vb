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

    Public WriteOnly Property ListaIscritti() As System.Collections.IList Implements PresentationLayer.IviewQuestionarioPresence.ListaIscritti
        Set(ByVal value As System.Collections.IList)

        End Set
    End Property

    Public Property OrderDir() As Boolean Implements PresentationLayer.IviewQuestionarioPresence.OrderDir
        Get
            If IsNothing(ViewState("OrderDir")) Then
                ViewState("OrderDir") = False
            End If
            Return ViewState("OrderDir")

            'True = Desc
            'False = Asc
        End Get
        Set(ByVal value As Boolean)
            ViewState("OrderDir") = value
        End Set
    End Property
    Public Property OrderValue() As String Implements PresentationLayer.IviewQuestionarioPresence.OrderValue
        Get
            Return ViewState("OrderValue")
        End Get
        Set(ByVal value As String)
            ViewState("OrderValue") = value
        End Set
    End Property
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class