Partial Public Class helpUtentiInvitati_mail
    Inherits Base


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_helpUtentiInvitati_mail", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBTitolo)

            Dim testo As String = .getValue("testoUnico")

            Me.LBTesto1.Text = testo
            'String.Format(testo, .getValue("IMGdomandaMultipla0"), .getValue("IMGpaginaAmerica"), .getValue("IMGdomandaMultipla1"), .getValue("IMGpesoDomanda"), .getValue("IMGdifficoltaDomanda"), .getValue("IMGnumeroRisposte"), .getValue("IMGdomandaMultipla2"), .getValue("IMGdomandaMultipla6"))

        End With

    End Sub
End Class