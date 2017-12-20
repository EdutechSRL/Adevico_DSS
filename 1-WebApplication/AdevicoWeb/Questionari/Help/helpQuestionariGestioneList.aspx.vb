Public Partial Class helpQuestionariGestioneList
    Inherits Base

    Private Property questType() As Integer
        Get
            Return Request.QueryString("type")
        End Get
        Set(ByVal value As Integer)
            ViewState("type") = Request.QueryString("type")
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        If Me.questType = COL_Questionario.Questionario.TipoQuestionario.Sondaggio Then
            MyBase.SetCulture("pg_helpSondaggiGestioneList", "Questionari")
        Else
            MyBase.SetCulture("pg_helpQuestionariGestioneList", "Questionari")
        End If
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBTitolo)

            Dim testo As String = .getValue("testoUnico")

            Me.LBTesto1.Text = String.Format(testo, .getValue("IMGanteprima"), .getValue("IMGmodifica"), .getValue("IMGgestioneDomande"), .getValue("IMGelimina"), .getValue("IMGdropdownLingue"), .getValue("IMGchiuso"), .getValue("IMGbloccato"), .getValue("IMGlibero"))



        End With

    End Sub

End Class