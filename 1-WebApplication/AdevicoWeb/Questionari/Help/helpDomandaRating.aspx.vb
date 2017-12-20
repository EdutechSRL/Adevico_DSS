Public Partial Class helpDomandaRating
    Inherits Base

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_helpDomandaRating", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBTitolo)

            Dim testo As String = .getValue("testoUnico")

            Me.LBTesto1.Text = String.Format(testo, .getValue("IMGdomandaRatingN"), .getValue("IMGdomandaRatingT"), .getValue("IMGpaginaRating"), .getValue("IMGtestoDomandaRating"), .getValue("IMGpesoDomanda"), .getValue("IMGdifficoltaDomanda"), .getValue("IMGdomandaRating5"), .getValue("IMGnumeroRisposte"), .getValue("IMGdomandaRatingGriglia"), .getValue("IMGrispostaNulla"), .getValue("IMGtestoDopoDomanda"))

        End With

    End Sub

End Class