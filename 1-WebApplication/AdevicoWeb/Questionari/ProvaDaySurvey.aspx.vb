Public Partial Class ProvaDaySurvey
    Inherits PageBaseQuestionario

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.UCDaySurvey1.CurrentPresenter.Init()
        'Me.UCDaySurvey2.CurrentPresenter.Init()
        Dim lista As New List(Of COL_Questionario.Domanda)
        Dim l1 As New COL_Questionario.Domanda
        l1.ID = 1
        l1.testo = "DOMANDA 1"
        Dim l2 As New COL_Questionario.Domanda
        l2.id = 1
        l2.testo = "DOMANDA 2"

        lista.Add(l1)
        lista.Add(l2)

        RadChart1.DataSource = lista
        RadChart1.DataBind()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean

    End Function

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = True
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