Imports COL_Questionario
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Questionario

Partial Public Class QuestionariList
    Inherits PageBaseQuestionario
    Dim _percorsoImgBandiera As String
    Dim _tooltipBandiera As String
    Dim _percorsoRedirectBandiera As String
    Dim oGestioneQuest As New GestioneQuestionario

    Private Enum TabIndex
        Comunita = 0
        Pubblici = 1
        Invito = 2
        Compilati = 3
        CompilatiTutti = 4
    End Enum
    Private Sub bindGrid()
        If Me.qs_questIdType = Questionario.TipoQuestionario.Sondaggio Then
            GRVElenco.Columns(5).Visible = False
        End If
        GRVElenco.PageSize = RootObject.nRighePaginaGridView
        GRVElenco.DataSource = Me.GruppoCorrente.questionari
        GRVElenco.DataBind()
    End Sub
    Public Overrides Sub BindDati()
        If Me.qs_questIdType = Questionario.TipoQuestionario.Autovalutazione Then
            'Me.RedirectToUrl(RootObject.HistoryAutovalutazione)
            Me.TBSQuestionari.Tabs(3).Visible = False
            Me.GRVElenco.Columns(2).Visible = True
        End If
        Me.MLVquestionari.SetActiveView(Me.VIWdati)

        Dim oImg As ImageButton

        oImg = Me.IMBHelp

        Select Case Me.qs_questIdType
            Case Questionario.TipoQuestionario.Sondaggio
                oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpSondaggiList, "target", "yes", "yes"))
            Case Else
                oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpQuestionariList, "target", "yes", "yes"))
        End Select

        If Not Me.ComunitaCorrenteID > 0 Then
            Me.TBSQuestionari.Tabs(TabIndex.Comunita).Visible = False
            If Not Page.IsPostBack Then
                Me.TBSQuestionari.Tabs(TabIndex.Pubblici).Selected = True
            End If
        End If


        Select Case Me.TBSQuestionari.SelectedTab.Value
            Case 0 ' questionari comunità
                Me.GruppoCorrente = New QuestionarioGruppo
                Me.GruppoCorrente.questionari = DALQuestionario.readQuestionariPersonaByComunita(Me.UtenteCorrente.ID, Me.ComunitaCorrenteID, Me.LinguaID, Me.qs_questIdType)

            Case 2 ' questionari compilati
                Me.GruppoCorrente = New QuestionarioGruppo
                Me.GruppoCorrente.questionari = DALQuestionario.readQuestionariCompilatiByPersona(Me.UtenteCorrente.ID, Me.ComunitaCorrenteID, Me.LinguaID, Me.qs_questIdType)

            Case 3 ' questionari pubblici
                Me.GruppoCorrente = New QuestionarioGruppo
                Me.GruppoCorrente.questionari = DALQuestionario.readQuestionariPubbliciByPersona(Me.UtenteCorrente.ID, Me.LinguaID, Me.qs_questIdType)

            Case 4 ' questionari su invito
                Me.GruppoCorrente = New QuestionarioGruppo
                Me.GruppoCorrente.questionari = DALQuestionario.readQuestionariInvitoByPersona(Me.UtenteCorrente.ID, Me.LinguaID, Me.qs_questIdType)
            Case 5 ' questionari su invito
                Me.GruppoCorrente = New QuestionarioGruppo
                Me.GruppoCorrente.questionari = DALQuestionario.readQuestionariCompilatiTuttiByPersona(Me.UtenteCorrente.ID, Me.LinguaID, Me.qs_questIdType)
        End Select

        bindGrid()

        If Page.IsPostBack = False Then
            oGestioneQuest.ViewListActionAdd(Me.qs_questIdType)
        End If

    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        Select Case Me.qs_questIdType
            Case Questionario.TipoQuestionario.Sondaggio
                MyBase.SetCulture("pg_SondaggiList", "Questionari")
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                MyBase.SetCulture("pg_LibrerieList", "Questionari")
            Case Questionario.TipoQuestionario.Autovalutazione
                MyBase.SetCulture("pg_AutovalutazioneList", "Questionari")
            Case Questionario.TipoQuestionario.Meeting
                MyBase.SetCulture("pg_MeetingList", "Questionari")
            Case Else
                MyBase.SetCulture("pg_QuestionariList", "Questionari")
        End Select
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle")
            .setLabel(LBTitolo)
            .setLabel(LBerrore)
            .setHeaderGridView(Me.GRVElenco, 0, "headerStato", True)
            .setHeaderGridView(Me.GRVElenco, 1, "headerNome", True)
            .setHeaderGridView(Me.GRVElenco, 2, "headerNumeroRisposte", True)
            .setHeaderGridView(Me.GRVElenco, 3, "headerDataInizio", True)
            .setHeaderGridView(Me.GRVElenco, 4, "headerDataFine", True)
            .setHeaderGridView(Me.GRVElenco, 5, "headerAutore", True)
            .setHeaderGridView(Me.GRVElenco, 6, "headerLingue", True)

            .setHeaderGridView(Me.GRVlistaRisposte, 0, "headerDataInizio", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 1, "headerDifficolta", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 2, "headerPunteggio", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 3, "headerPunteggioRelativo", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 4, "headerNRisposteTotali", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 5, "headerNRisposteCorrette", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 6, "headerNRisposteParzialmenteCorrette", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 7, "headerNRisposteErrate", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 8, "headerNRisposteNonValutate", True)
            .setHeaderGridView(Me.GRVlistaRisposte, 9, "headerNRisposteSaltate", True)

            .setLabel(LBHelp)
            .setImageButton(IMBHelp, False, False, True, False)

            For Each item As Telerik.Web.UI.RadTab In TBSQuestionari.Tabs
                item.Text = .getValue(TBSQuestionari, item.Value)
                item.ToolTip = item.Text
            Next
            .setLinkButton(LNBautoevaluationList, False, True)
            .setHyperLink(HYPautoevaluationStatistics, False, True)

        End With
    End Sub
    Public Sub SetInternazionalizzazioneGRVElenco(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        With Me.Resource
            .setImage(e.Row.FindControl("IMBloccato"), True)
        End With
    End Sub
    Protected Sub GRVElenco_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        GRVElenco.DataSource = Me.GruppoCorrente.questionari
        GRVElenco.DataBind()
    End Sub
    Private Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVElenco.RowCommand
        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim oQuest As New Questionario
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim indiceQuestionario = row.RowIndex + (GRVElenco.PageIndex * RootObject.nRighePaginaGridView)
            oQuest.id = GRVElenco.DataKeys(row.RowIndex).Value
            oQuest.linguePresenti = Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti
            oQuest.tipo = Questionario.TipoQuestionario.Autovalutazione
            oQuest.nome = DirectCast(row.Cells(1).FindControl("LTname"), Literal).Text

            Me.QuestionarioCorrente = oQuest
            Me.LinguaQuestionario = Me.LinguaID
            Select Case e.CommandName
                Case "vediRisposte"
                    MLVquestionari.SetActiveView(Me.VIWlistaRisposte)
                    BindGRVlistaRisposteAutovalutazione()
            End Select
        End If
    End Sub
    Private Sub BindGRVlistaRisposteAutovalutazione(Optional ByVal idQuest As Integer = 0)
        If idQuest = 0 Then
            idQuest = Me.QuestionarioCorrente.id
        End If
        LNBautoevaluationList.CommandArgument = idQuest

        Me.QuestionarioCorrente.risposteQuestionario = DALRisposte.readRisposteAutovalutazione(idQuest, Me.UtenteCorrente.ID)
        LBTitoloListaRisposte.Text = String.Format(Me.Resource.getValue("LBTitoloListaRisposte.text"), Me.QuestionarioCorrente.nome, Me.EncryptedUrl(RootObject.compileUrl, "idq=" & idQuest, SecretKeyUtil.EncType.Questionario))
        GRVlistaRisposte.PageSize = RootObject.nRighePaginaGridView
        GRVlistaRisposte.DataSource = Me.QuestionarioCorrente.risposteQuestionario
        GRVlistaRisposte.DataBind()
    End Sub
    Private Sub GRVElenco_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVElenco.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim q As COL_Questionario.Questionario = DirectCast(e.Row.DataItem, COL_Questionario.Questionario)
            'svuota le colonne data inizio e fine
            Dim dataI As DateTime = Convert.ToDateTime(e.Row.Cells(3).Text)
            If dataI = Date.MinValue Or dataI > Date.MaxValue.AddDays(-1) Then
                e.Row.Cells(3).Text = String.Empty
            End If
            Dim dataF As DateTime = Convert.ToDateTime(e.Row.Cells(4).Text)
            If dataF = Date.MinValue Or dataF > Date.MaxValue.AddDays(-1) Then
                e.Row.Cells(4).Text = String.Empty
            End If
            Dim oQuest As New Questionario
            Dim indiceRiga As Integer
            indiceRiga = e.Row.RowIndex + (RootObject.nRighePaginaGridView * GRVElenco.PageIndex)
            oQuest = Me.GruppoCorrente.questionari(indiceRiga)

            ' se è un sondaggio o un meeting compilato setto l'url per visualizzare i risultati delle statistiche
            'i meeting no perche' possono essere modificabili.
            Dim maxDate As String = DateTime.MaxValue.ToString
            Dim isAnswered As Boolean = (q.rispostaQuest.id > 0 AndAlso Not String.IsNullOrWhiteSpace(q.rispostaQuest.dataFine) AndAlso Not q.rispostaQuest.dataFine = maxDate)
            Dim viewToCompile As Boolean = Not q.isReadOnly AndAlso (q.editaRisposta OrElse (Not q.editaRisposta AndAlso Not isAnswered))
            If viewToCompile AndAlso q.durata > 0 AndAlso Not String.IsNullOrWhiteSpace(q.rispostaQuest.dataInizio) AndAlso Not q.rispostaQuest.dataInizio = DateTime.MaxValue.ToString AndAlso Not q.rispostaQuest.dataInizio = DateTime.MinValue.ToString Then
                viewToCompile = Not ((q.durata * 60) - DateDiff("s", q.rispostaQuest.dataInizio, DateTime.Now) < 1)
            End If
            Dim viewAnswer As Boolean = q.visualizzaRisposta AndAlso q.rispostaQuest.id > 0

            Dim oHyperlink As HyperLink = e.Row.Cells(1).FindControl("HYPquestionnaire")
            Dim oLiteral As Literal = e.Row.Cells(1).FindControl("LTname")
            oLiteral.Text = q.nome
            oHyperlink.Text = q.nome

            If (Me.TBSQuestionari.Tabs(TabIndex.Compilati).Selected OrElse Me.TBSQuestionari.Tabs(TabIndex.CompilatiTutti).Selected) AndAlso (Me.qs_questIdType = Questionario.TipoQuestionario.Sondaggio) Then 'OrElse Me.questType = Questionario.TipoQuestionario.Meeting) Then
                Me.QuestionarioCorrente = New Questionario
                Me.QuestionarioCorrente.tipo = Me.qs_questIdType
                oHyperlink.NavigateUrl = RootObject.QuestionarioStatisticheGeneraliShort & "&idq=" & oQuest.id & "&comp=1"
                ' e.Row.Cells(1).Text = "<a href='" + oQuest.url + "'>" + oQuest.nome + "</a>"
                'ElseIf (Me.TBSQuestionari.Tabs(TabIndex.Compilati).Selected OrElse Me.TBSQuestionari.Tabs(TabIndex.CompilatiTutti).Selected) AndAlso viewAnswer Then
                '    oHyperlink.NavigateUrl = RootObject.QuestionarioView(( & "&idq=" & oQuest.id & "&comp=1"
            ElseIf viewToCompile OrElse viewAnswer Then
                oHyperlink.NavigateUrl = Me.EncryptedUrl(RootObject.compileUrl, "idq=" & oQuest.id, SecretKeyUtil.EncType.Questionario)
                'e.Row.Cells(1).Text = "<a href='" + oQuest.url + "'>" + oQuest.nome + "</a>"
                DirectCast(e.Row.Cells(2).Controls(1), LinkButton).Text &= Me.Resource.getValue("MSGvisualizza") '"<a href='" & RootObject.HistoryAutovalutazioneShort & "?idq=" & oQuest.id & "'>" & oQuest.nRisposte & Me.Resource.getValue("MSGvisualizza") + "</a>"
                oLiteral.Visible = False
                oHyperlink.Visible = True

                visualizzaBandiera(oQuest.id, oQuest.idLingua)
            Else
                oLiteral.Visible = True
                oHyperlink.Visible = False
            End If
            'Dim compileOrViewAllowed As Boolean = q. Not q.isReadOnly
            'oLiteral.Visible = q.editaRisposta



            e.Row.Cells(6).Text = percorsoImgBandiera
            e.Row.Cells(6).ToolTip = tooltipBandiera
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            If viewToCompile Then
                For Each oLingua As Lingua In oQuest.linguePresenti
                    If Not oLingua.ID = oQuest.idLingua Then
                        visualizzaBandiera(oQuest.id, oLingua.ID)
                        e.Row.Cells(6).Text += "&nbsp;&nbsp;" + percorsoImgBandiera
                        e.Row.Cells(6).ToolTip = tooltipBandiera
                    End If
                Next
            End If


            'If Me.TBSQuestionari.SelectedTab.Value = 2 Then ' questionari su invito
            '    ' nascondo bandiere e url per non consentire una nuova compilazione
            '    GRVElenco.Columns(5).Visible = False
            '    e.Row.Cells(1).Text = oQuest.nome
            'End If

        End If

        SetInternazionalizzazioneGRVElenco(e)

    End Sub
    Private Sub visualizzaBandiera(ByVal idQuest As Integer, ByVal idLingua As Integer)

        Dim url As String = Me.EncryptedUrl(RootObject.compileUrl, "idq=" & idQuest.ToString() & "&idl=" & idLingua.ToString(), SecretKeyUtil.EncType.Questionario)

        Select Case idLingua
            Case 1
                tooltipBandiera = "Italiano"
                percorsoImgBandiera = "<a href='" + url + "'><img src='" + RootObject.imgBandieraItaliano + "' alt='" + tooltipBandiera + "' title='" + tooltipBandiera + "'></a>"
            Case 2
                tooltipBandiera = "English"
                percorsoImgBandiera = "<a href='" + url + "' alt='" + tooltipBandiera + "' title='" + tooltipBandiera + "'><img src='" + RootObject.imgBandieraInglese + "'></a>"
            Case 3
                tooltipBandiera = "Deutsch"
                percorsoImgBandiera = "<a href='" + url + "' alt='" + tooltipBandiera + "' title='" + tooltipBandiera + "'><img src='" + RootObject.imgBandieraTedesco + "'></a>"
            Case 4
                tooltipBandiera = "Français"
                percorsoImgBandiera = "<a href='" + url + "' alt='" + tooltipBandiera + "' title='" + tooltipBandiera + "'><img src='" + RootObject.imgBandieraFrancese + "'></a>"
            Case 5
                tooltipBandiera = "Espanõl"
                percorsoImgBandiera = "<a href='" + url + "' alt='" + tooltipBandiera + "' title='" + tooltipBandiera + "'><img src='" + RootObject.imgBandieraSpagnolo + "'></a>"
        End Select

    End Sub
    Private Property percorsoImgBandiera() As String
        Get
            Return _percorsoImgBandiera
        End Get
        Set(ByVal value As String)
            _percorsoImgBandiera = value
        End Set
    End Property
    Private Property tooltipBandiera() As String
        Get
            Return _tooltipBandiera
        End Get
        Set(ByVal value As String)
            _tooltipBandiera = value
        End Set
    End Property
    Protected Sub TBSQuestionari_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSQuestionari.TabClick
        BindDati()
    End Sub
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Protected Sub vediQuestionario(ByVal idRisposta As Integer, ByRef idQuestionarioRandom As Integer)
        Dim oGestioneRisposte As New GestioneRisposte
        Dim oStat As New Statistica
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioByPersona(PageUtility.CurrentContext, False, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, Me.UtenteCorrente.ID, 0, idRisposta, Nothing, idQuestionarioRandom)
        'oStat = oGestioneRisposte.calcoloPunteggioAutovalutazione(Me.QuestionarioCorrente)
        'bindDataList(oStat)
        bindDataList(Me.QuestionarioCorrente.rispostaQuest.oStatistica)
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
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(0).YValue = listStat.Item(0).nRisposteCorrette
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(1).YValue = listStat.Item(0).nRisposteErrate
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(2).YValue = listStat.Item(0).nRisposteNonValutate
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(3).YValue = listStat.Item(0).nRisposteParzialmenteCorrette
                DirectCast(GestioneDomande.FindControlRecursive(DLDettagli, "RCRisposte"), Telerik.WebControls.RadChart).Series(0).Item(4).YValue = listStat.Item(0).nRisposteSaltate
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
            .setLabel(e.Item.FindControl("LBnRisposteParzialmenteCorrette"))
            .setLabel(e.Item.FindControl("LBnRisposteSaltate"))
        End With
    End Sub
    Private Sub GRVlistaRisposte_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVlistaRisposte.PageIndexChanging
        GRVlistaRisposte.PageIndex = e.NewPageIndex
        GRVlistaRisposte.DataSource = Me.QuestionarioCorrente.risposteQuestionario
        GRVlistaRisposte.DataBind()
    End Sub
    Protected Sub GRVlistaRisposte_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVlistaRisposte.RowCommand
        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim oRisposta As New RispostaQuestionario
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim indiceRisposta = row.RowIndex + (GRVlistaRisposte.PageIndex * RootObject.nRighePaginaGridView)
            oRisposta.id = GRVlistaRisposte.DataKeys(row.RowIndex).Value
            oRisposta.idQuestionarioRandom = CInt(e.CommandArgument)
            HYPautoevaluationStatistics.NavigateUrl = BaseUrl & RootObject.QuestionariList & "?type=5&idQ=" & Me.QuestionarioCorrente.id.ToString & "#attempt_" & oRisposta.id.ToString
            Me.QuestionarioCorrente.rispostaQuest = oRisposta
            Select Case e.CommandName
                Case "visualizza"
                    MLVquestionari.SetActiveView(Me.VIWquestionario)
                    vediQuestionario(oRisposta.id, oRisposta.idQuestionarioRandom)
            End Select
        End If
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        Dim oGestioneDomande As New GestioneDomande
        Dim iPag As Integer = 0
        DLPagine.Controls(iPag).FindControl("DLDomande").Controls(e.Item.ItemIndex).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, e.Item.ItemIndex, True))
    End Sub
    Private Sub GRVlistaRisposte_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVlistaRisposte.RowDataBound
        If e.Row.RowIndex >= 0 Then
            For i As Int16 = 1 To 8
                Dim dataI As Decimal = Convert.ToDecimal(DirectCast(e.Row.Cells(i).Controls(1), Label).Text)
                If dataI = Integer.MinValue Or dataI = Decimal.MinValue Then
                    DirectCast(e.Row.Cells(i).Controls(1), Label).Text = " -- "
                End If
            Next
        End If
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub

    Private Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idQuest As Integer = Request.QueryString("idq")
        If idQuest > 0 Then
            MLVquestionari.SetActiveView(Me.VIWlistaRisposte)
            BindGRVlistaRisposteAutovalutazione(idQuest)
        End If
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property

    Private Sub LNBautoevaluationList_Click(sender As Object, e As System.EventArgs) Handles LNBautoevaluationList.Click
        PageUtility.RedirectToUrl(BaseUrl & RootObject.QuestionariList & "?type=5#quest_" & LNBautoevaluationList.CommandArgument)
    End Sub
End Class
