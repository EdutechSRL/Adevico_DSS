Public Partial Class helpDomandaDropDown
    Inherits Base

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_helpDomandaDropDown", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBTitolo)

            Dim testo As String = .getValue("testoUnico")

            Me.LBTesto1.Text = String.Format(testo, .getValue("IMGdomandaDropDown"), .getValue("IMGpaginaUsa"), .getValue("IMGtestoDomanda"), .getValue("IMGpesoDomanda"), .getValue("IMGdifficoltaDomanda"), .getValue("IMGnumeroRisposte"), .getValue("IMGetichettaDropDown"), .getValue("IMGdomandaDropDownOpzioni"), .getValue("IMGtestoDopoDomanda"), .getValue("IMGdomandaDropDown"))

        End With

    End Sub
End Class