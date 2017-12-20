Public Partial Class helpQuestionariEdit
    Inherits Base

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_helpQuestionariEdit", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBTitolo)

            Dim testo As String = .getValue("testoUnico")

            Me.LBTesto1.Text = String.Format(testo, .getValue("IMGaggiungiDomanda"), .getValue("IMGmodifica"), .getValue("IMGelimina"), .getValue("IMGspostaSu"), .getValue("IMGspostaGiu"), .getValue("IMGnuovaPagina"), .getValue("IMGmodifica"), .getValue("IMGelimina"))



        End With

    End Sub

End Class