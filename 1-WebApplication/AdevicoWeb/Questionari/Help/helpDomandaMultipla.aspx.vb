Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_Questionario

Partial Public Class helpDomandaMultipla
    Inherits Base

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_helpDomandaMultipla", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBTitolo)

            Dim testo As String = .getValue("testoUnico")

            Me.LBTesto1.Text = String.Format(testo, .getValue("IMGdomandaMultipla"), .getValue("IMGpaginaAmerica"), .getValue("IMGtestoDomanda"), .getValue("IMGpesoDomanda"), .getValue("IMGdifficoltaDomanda"), .getValue("IMGnumeroRisposte"), .getValue("IMGnumeroMaxRisposte"), .getValue("IMGdomandaMultiplaOpzioni"), .getValue("IMGtestoDopoDomanda"), .getValue("IMGdomandaMultipla"))

        End With
       
    End Sub
End Class