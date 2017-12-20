Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_Questionario

Partial Public Class helpDomandaNumerica
    Inherits Base

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_helpDomandaNumerica", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBTitolo)

            Dim testo As String = .getValue("testoUnico")

            Me.LBTesto1.Text = String.Format(testo, .getValue("IMGdomandaNumerica"), .getValue("IMGpaginaGeometria"), .getValue("IMGdomandaNumerica2"), .getValue("IMGpesoDomanda"), .getValue("IMGdifficoltaDomanda"), .getValue("IMGnumeroRisposte"), .getValue("IMGdomandaNumericaOpzioni"), .getValue("IMGtestoDopoDomanda"), .getValue("IMGdomandaMultipla"))

        End With

    End Sub
End Class