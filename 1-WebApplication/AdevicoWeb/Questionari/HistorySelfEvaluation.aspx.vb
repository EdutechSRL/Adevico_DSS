Imports COL_Questionario

Partial Public Class HistorySelfEvaluation
    Inherits PageBaseQuestionario

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim id As Integer = Me.Request.QueryString("idq")
            If id > 0 Then
                Me.QuestionarioCorrente.id = id

                MLVquestionari.SetActiveView(Me.VIWlistaRisposte)
                BindGRVlistaRisposteAutovalutazione()
            Else
                MLVquestionari.SetActiveView(Me.VIWlistaQuestionari)
            End If
            'If Me.GruppoCorrente Is Nothing Then
            '    Me.GruppoCorrente = New QuestionarioGruppo(DALQuestionarioGruppo.GruppoPrincipaleByComunita(Me.ComunitaCorrente.Id, Questionario.TipoQuestionario.Questionario))
            'End If
        End If
    End Sub
    Public Overrides Sub BindDati()
        BindGRVelencoAutovalutazione()
    End Sub
    Private Sub BindGRVelencoAutovalutazione()
        Dim questionariList As New List(Of Questionario)
        questionariList = DALQuestionario.readQuestionariAutovalutazioneByIdPersona(DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrente.Id), Me.UtenteCorrente.ID)
        GRVelenco.PageSize = RootObject.nRighePaginaGridView
        GRVelenco.DataSource = questionariList
        GRVelenco.DataBind()
    End Sub
    Private Sub BindGRVlistaRisposteAutovalutazione()
        Dim risposteList As List(Of RispostaQuestionario)
        risposteList = DALRisposte.readRisposteAutovalutazione(Me.QuestionarioCorrente.id, Me.UtenteCorrente.ID)
        LBTitoloListaRisposte.Text = String.Format(Me.Resource.getValue("LBTitoloListaRisposte.text"), Me.QuestionarioCorrente.nome)
        GRVlistaRisposte.PageSize = RootObject.nRighePaginaGridView
        GRVlistaRisposte.DataSource = risposteList
        GRVlistaRisposte.DataBind()
    End Sub
    Public Overrides Sub BindNoPermessi()
        LBerrore.Text = Me.Resource.getValue("MSGerrorNoPermission")
        MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_HistorySelfEvaluation", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        SetCultureSettings()
        With Me.Resource
            .setLabel(LBtitolo)
            .setHeaderGridView(Me.GRVelenco, 0, "headerNome", True)
            .setHeaderGridView(Me.GRVelenco, 1, "headerQuanteRisposte", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 0, "dataCompilazione", True)
            Master.ServiceTitle = .getValue("ServiceTitle")
        End With
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        Dim oGestioneDomande As New GestioneDomande
        Dim iPag As Integer = 0
        DLPagine.Controls(iPag).FindControl("DLDomande").Controls(e.Item.ItemIndex).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, e.Item.ItemIndex, True))
    End Sub
    Protected Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVelenco.RowCommand
        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim oQuest As New Questionario
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim indiceQuestionario = row.RowIndex + (GRVelenco.PageIndex * RootObject.nRighePaginaGridView)
            oQuest.id = GRVelenco.DataKeys(row.RowIndex).Value
            oQuest.linguePresenti = Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti
            oQuest.tipo = Questionario.TipoQuestionario.Autovalutazione
            oQuest.nome = e.CommandArgument.ToString
            Me.QuestionarioCorrente = oQuest
            Me.LinguaQuestionario = Me.LinguaID
            Select Case e.CommandName
                Case "Seleziona"
                    MLVquestionari.SetActiveView(Me.VIWlistaRisposte)
                    BindGRVlistaRisposteAutovalutazione()
            End Select
        End If
    End Sub
    Protected Sub GRVlistaRisposte_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVlistaRisposte.RowCommand
        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim oRisposta As New RispostaQuestionario
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim indiceRisposta = row.RowIndex + (GRVlistaRisposte.PageIndex * RootObject.nRighePaginaGridView)
            oRisposta.id = GRVlistaRisposte.DataKeys(row.RowIndex).Value
            oRisposta.idQuestionarioRandom = CInt(e.CommandArgument)
            Me.QuestionarioCorrente.rispostaQuest = oRisposta
            Select Case e.CommandName
                Case "visualizza"
                    MLVquestionari.SetActiveView(Me.VIWquestionario)
                    vediQuestionario(oRisposta.id, oRisposta.idQuestionarioRandom)
            End Select
        End If
    End Sub
    Protected Sub vediQuestionario(ByVal idRisposta As Integer, ByRef idQuestionarioRandom As Integer)
        Dim oGestioneRisposte As New GestioneRisposte
        Dim oStat As New Statistica
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, Me.UtenteCorrente.ID, 0, idRisposta, Nothing, idQuestionarioRandom)
        'oStat = oGestioneRisposte.calcoloPunteggioAutovalutazione(Me.QuestionarioCorrente)
        'bindDataList(oStat)
        bindDataList(Me.QuestionarioCorrente.rispostaQuest.oStatistica)
    End Sub
    Protected Sub DLDettagli_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLDettagli.ItemDataBound
        SetInternazionalizzazioneDLDettagli(e)
    End Sub
    Public Sub SetInternazionalizzazioneDLDettagli(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBPunteggioRelativo"))
            .setLabel(e.Item.FindControl("LBnRisposteTotali"))
            .setLabel(e.Item.FindControl("LBnRisposteCorrette"))
            .setLabel(e.Item.FindControl("LBnRisposteErrate"))
            .setLabel(e.Item.FindControl("LBnRisposteNonValutate"))
            .setLabel(e.Item.FindControl("LBPunteggioTotale"))
            .setLabel(e.Item.FindControl("LBnOpzioniTotali"))
            .setLabel(e.Item.FindControl("LBnOpzioniCorrette"))
            .setLabel(e.Item.FindControl("LBnOpzioniErrate"))
            .setLabel(e.Item.FindControl("LBnOpzioniNonValutate"))
        End With
    End Sub
    Protected Sub bindDataList(ByRef oStat As Statistica)
        Dim oGestioneRisposte As New GestioneRisposte
        If Not Me.QuestionarioCorrente Is Nothing Then
            Dim listStat As New List(Of Statistica)
            listStat.Add(oStat)
            DLPagine.DataSource = Me.QuestionarioCorrente.pagine
            DLDettagli.DataSource = listStat
            DLDettagli.DataBind()

            If DLDettagli.Controls.Count > 0 Then
                'DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCOpzioni"), Telerik.WebControls.RadChart).Series(0).Item(0).YValue = listStat.Item(0).nOpzioniCorrette
                'DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCOpzioni"), Telerik.WebControls.RadChart).Series(0).Item(1).YValue = listStat.Item(0).nOpzioniErrate
                'DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCOpzioni"), Telerik.WebControls.RadChart).Series(0).Item(2).YValue = listStat.Item(0).nOpzioniNonValutate
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(0).YValue = listStat.Item(0).nRisposteCorrette
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(1).YValue = listStat.Item(0).nRisposteErrate
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(2).YValue = listStat.Item(0).nRisposteNonValutate

            End If

            DLPagine.DataBind()
            'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario Or Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random Or Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            DLPagine = oGestioneRisposte.setRispostePaginaCorrette(DLPagine, Me.QuestionarioCorrente.domande)
            'Else
            '    DLPagine = oGestioneRisposte.setRispostePagina(DLPagine)
            'End If

        End If
    End Sub

    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound
        Dim iPag As Integer
        iPag = e.Item.ItemIndex

        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(iPag).FindControl("DLDomande")

        dlDomande.DataSource = Me.QuestionarioCorrente.pagine.Item(iPag).domande
        dlDomande.DataBind()

    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class