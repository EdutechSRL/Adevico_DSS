Imports COL_Questionario

Partial Public Class ucPNLValutazione
    Inherits BaseControlQuestionario
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    Public Sub CaricaByRisposta(ByRef oStat As Statistica)
        Dim listStat As New List(Of Statistica)
        listStat.Add(oStat)
        'listStat.Add(oPersona.statistica)
        DLDettagli.DataSource = listStat
        DLDettagli.DataBind()
        bindDataList(True)
    End Sub
    Public Sub carica()
        Dim listStat As New List(Of Statistica)
        listStat.Add(Me.QuestionarioCorrente.rispostaQuest.oStatistica)
        'listStat.Add(oPersona.statistica)
        DLDettagli.DataSource = listStat
        DLDettagli.DataBind()
        bindDataList()
    End Sub
    Protected Sub bindDataList(Optional ByRef forceBind As Boolean = False)
        If Not Me.QuestionarioCorrente Is Nothing Or forceBind Then
            Dim listStat As New List(Of Statistica)
            listStat = DLDettagli.DataSource
            DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(0).YValue = listStat.Item(0).nRisposteCorrette
            DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(1).YValue = listStat.Item(0).nRisposteErrate
            DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(2).YValue = listStat.Item(0).nRisposteNonValutate
            DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(3).YValue = listStat.Item(0).nRisposteParzialmenteCorrette
            DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(4).YValue = listStat.Item(0).nRisposteSaltate
        End If
    End Sub
    Protected Sub DLDettagli_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLDettagli.ItemDataBound
        SetInternazionalizzazioneDLDettagli(e)
    End Sub
    Public Sub SetInternazionalizzazioneDLDettagli(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            '.setLabel(e.Item.FindControl("LBUtente"))
            .setLabel(e.Item.FindControl("LBPunteggioRelativo"))
            .setLabel(e.Item.FindControl("LBnRisposteTotali"))
            .setLabel(e.Item.FindControl("LBnRisposteCorrette"))
            .setLabel(e.Item.FindControl("LBnRisposteErrate"))
            .setLabel(e.Item.FindControl("LBnRisposteNonValutate"))
            .setLabel(e.Item.FindControl("LBPunteggioTotale"))
            .setLabel(e.Item.FindControl("LBnRisposteParzialmenteCorrette"))
            .setLabel(e.Item.FindControl("LBnRisposteSaltate"))

            '.setLabel(e.Item.FindControl("LBnOpzioniTotali"))
            '.setLabel(e.Item.FindControl("LBnOpzioniCorrette"))
            '.setLabel(e.Item.FindControl("LBnOpzioniErrate"))
            '.setLabel(e.Item.FindControl("LBnOpzioniNonValutate"))
        End With
    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucPNLValutazione", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub
End Class