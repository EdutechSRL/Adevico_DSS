Imports COL_Questionario
Imports System.Linq

Partial Public Class ucStatisticheUtenti
	Inherits BaseControlQuestionario

    Public Shared iDom As Integer
    Dim oGestioneDomande As New GestioneDomande
    Dim oPagedDataSource As New PagedDataSource
    Dim oGestioneRisposte As New GestioneRisposte
    Dim oGestioneQuest As New GestioneQuestionario

    Private Enum TabIndex
        Invitato = 0
        Incompleto = 1
    End Enum

#Region "Property"
    Private ReadOnly Property PreloadStatistics() As StatisticsType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticsType).GetByString(Request.QueryString("mode"), StatisticsType.UserAttempts)
        End Get
    End Property
    Private Property CurrentStatistics As StatisticsType
        Get
            Return ViewStateOrDefault("CurrentStatistics", StatisticsType.UsersList)
        End Get
        Set(value As StatisticsType)
            ViewState("CurrentStatistics") = value
            Select Case value
                Case StatisticsType.User
                    LNBexportResultsToSingleColumnCsv.Visible = False
                    LNBexportResultsToSingleColumnXml.Visible = False
                Case StatisticsType.UsersList
                    LNBexportResultsToSingleColumnCsv.Visible = True
                    LNBexportResultsToSingleColumnXml.Visible = True
            End Select
        End Set
    End Property

    Private Property CurrentIdAnswer As Integer
        Get
            Return ViewStateOrDefault("CurrentIdAnswer", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdAnswer") = value
        End Set
    End Property
    Private Property CurrentIdRandomQuestionnaire As Integer
        Get
            Return ViewStateOrDefault("CurrentIdRandomQuestionnaire", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdRandomQuestionnaire") = value
        End Set
    End Property
    Private Property CurrentIdPerson As Integer
        Get
            Return ViewStateOrDefault("CurrentIdPerson", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdPerson") = value
        End Set
    End Property
    Private Property CurrentIdInvite As Integer
        Get
            Return ViewStateOrDefault("CurrentIdInvite", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdInvite") = value
        End Set
    End Property
    Private Property CurrentCompilerName As String
        Get
            Return ViewStateOrDefault("CurrentCompilerName", "")
        End Get
        Set(value As String)
            ViewState("CurrentCompilerName") = value
        End Set
    End Property
    Public Property AllowedStandardActionType() As lm.Comol.Core.DomainModel.StandardActionType
        Get
            Return ViewStateOrDefault("AllowedStandardActionType", lm.Comol.Core.DomainModel.StandardActionType.None)
        End Get
        Set(value As lm.Comol.Core.DomainModel.StandardActionType)
            ViewState("AllowedStandardActionType") = value
        End Set
    End Property
#End Region

    Private ReadOnly Property pageMode()
        Get
            Return Request.QueryString("mode")
        End Get
    End Property
    Private Property UtentiLista() As List(Of UtenteInvitato)
        Get
            Return ViewStateOrDefault("UtentiLista", New List(Of UtenteInvitato))
        End Get
        Set(ByVal value As List(Of UtenteInvitato))
            Dim name As String = "--"
            Dim index As Integer = 1
            If QuestionarioCorrente.risultatiAnonimi Then
                For Each u As UtenteInvitato In value.OrderBy(Function(ui) ui.RispostaID).ToList
                    u.Cognome = "User " & index.ToString
                    u.Nome = ""
                    u.Mail = "--"
                    index += 1
                Next
            End If
            ViewState("UtentiLista") = value
        End Set
    End Property
    Private Property iPag() As Integer
        Get
            Return ViewState("idPag")
        End Get
        Set(ByVal value As Integer)
            ViewState("idPag") = value
        End Set
    End Property
    Protected Sub bindDataList(ByVal indice As Integer)

        If Not Me.QuestionarioCorrente Is Nothing Then
            LBNomeUtente.Visible = True
            DLPagine.DataSource = Me.QuestionarioCorrente.pagine

            DLPagine.DataBind()
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.RandomRepeat OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                DLPagine = oGestioneRisposte.setRispostePaginaCorrette(DLPagine, Me.QuestionarioCorrente.domande)
            Else
                DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, Me.QuestionarioCorrente.domande)
            End If
        Else
            LBNomeUtente.Visible = False
        End If
    End Sub

    Protected Sub vediQuestionario(ByVal idRisposta As Integer, ByVal idUI As Integer, ByVal idP As Integer, ByVal IdRandomQuestionnaire As Integer)
        GRVUtenti.Visible = False
        PNLFiltri.Visible = False
        DLPagine.Visible = True
        LNBIndietro.Visible = String.IsNullOrEmpty(Request.QueryString("NoBack")) OrElse Request.QueryString("NoBack") <> "True"
        TBSQuestionari.Visible = False
        LBNomeUtente.Visible = True

        Dim oRis As New RispostaQuestionario
        Dim oPersona As New UtenteInvitato
        'Dim idU As String = GRVUtenti.SelectedDataKey.Item("ID")
        'Dim idP As String = GRVUtenti.SelectedDataKey.Item("PersonaID")
        CurrentCompilerName = Resource.getValue("AnonymousUser")
        CurrentIdPerson = idP
        CurrentIdRandomQuestionnaire = IdRandomQuestionnaire
        CurrentIdInvite = idUI
        CurrentIdAnswer = idRisposta
        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None OrElse pageMode = TipoStatistiche.Utenti OrElse pageMode = TipoStatistiche.Utenti Then
            If idUI > 0 Then
                oPersona = oRis.findUtenteInvitatoBYID(Me.UtentiLista, idUI)
            Else
                oPersona = oRis.findUtenteInvitatoBYIDPersona(Me.UtentiLista, idP)
            End If
            CurrentCompilerName = oPersona.Cognome & " " & oPersona.Nome

            Me.QuestionarioCorrente = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, idP, idUI, idRisposta, , IdRandomQuestionnaire)
        End If
        'Dim oPersone As New List(Of UtenteInvitato)
        'oPersone.Add(oPersona)
        'oGestioneRisposte.calcoloPunteggio(oPersone)
        PNLDettagli.Visible = True
        PNLExcel.Visible = False
        If Me.QuestionarioCorrente.rispostaQuest.oStatistica.punteggio = Decimal.MinValue Then
            PHStat.Controls.Clear()
        Else
            addUCpnlValutazione()
        End If
        With Me.Resource
            .setLabel(LBNomeUtente)
        End With
        LBNomeUtente.Text += "<b>" + oPersona.Cognome + " " + oPersona.Nome + "</b>"
        If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
            Dim items As List(Of LazyUserResponse) = DALQuestionario.GetQuestionnaireAttempts(PageUtility.CurrentContext, Me.QuestionarioCorrente.id, oPersona.PersonaID, oPersona.ID)
            If Not IsNothing(items) AndAlso items.Count > 0 Then
                LBNomeUtente.Text += " " & String.Format(Me.Resource.getValue("AttemptsInfo"), items.Where(Function(i) i.Id <= idRisposta).Count())
            End If
        End If
        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            LBNomeQuestionario.Text = Me.QuestionarioCorrente.nome
        Else
            DVquestionnaireName.Visible = False
            Me.DIVNomi.Visible = True
        End If
        CurrentStatistics = StatisticsType.User
        HYPprintAll.Visible = False
        HYPadvancedStatistics.Visible = False
        bindDataList(iPag)
    End Sub
    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound

        iPag = e.Item.ItemIndex

        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(iPag).FindControl("DLDomande")

        dlDomande.DataSource = Me.QuestionarioCorrente.pagine.Item(iPag).domande
        dlDomande.DataBind()

    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        iDom = e.Item.ItemIndex
        DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, True))
    End Sub
    Protected Sub LKBEsportaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBEsportaExcel.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("Content-Disposition", "attachment; filename=prova.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Response.Charset = ""
        Response.ContentEncoding = System.Text.Encoding.Default
        Me.EnableViewState = False
        Dim tw As New System.IO.StringWriter
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        GRVUtenti.RenderControl(hw)
        Response.Write(tw.ToString())
        Response.End()
    End Sub
    Public Overrides Sub BindDati()
        Dim oDALStatistiche As New DALStatistiche
        If Not Page.IsPostBack Then
            Me.TBSQuestionari.SelectedTab.Value = 0
            CHKUtentiComunita.Checked = True
            CHKUtentiInvitati.Checked = True

        End If
        CurrentStatistics = StatisticsType.UsersList
        Select Case Me.TBSQuestionari.SelectedTab.Value
            Case 0
                Me.UtentiLista = oDALStatistiche.readUtentiConQuestionarioInviato(Me.QuestionarioCorrente.id, CHKUtentiComunita.Checked, CHKUtentiNonComunita.Checked, CHKUtentiEsterni.Checked, CHKUtentiInvitati.Checked, PageUtility.CurrentContext)
                GRVUtenti.DataSource = Me.UtentiLista
                GRVUtenti.Columns(3).Visible = True
                GRVUtenti.Columns(4).Visible = True
            Case 1
                Me.UtentiLista = oDALStatistiche.readUtentiConQuestionarioIncompleto(Me.QuestionarioCorrente.id, CHKUtentiComunita.Checked, CHKUtentiNonComunita.Checked, CHKUtentiEsterni.Checked, CHKUtentiInvitati.Checked, PageUtility.CurrentContext)
                GRVUtenti.DataSource = Me.UtentiLista
                GRVUtenti.Columns(3).Visible = True
                GRVUtenti.Columns(4).Visible = True
            Case 2
                Me.UtentiLista = oDALStatistiche.readUtentiConQuestionarioNonCompilato(Me.QuestionarioCorrente.id, Me.ComunitaCorrenteID, CHKUtentiComunita.Checked, CHKUtentiNonComunita.Checked, CHKUtentiEsterni.Checked, CHKUtentiInvitati.Checked, PageUtility.CurrentContext)
                GRVUtenti.DataSource = Me.UtentiLista
                GRVUtenti.Columns(3).Visible = False
                GRVUtenti.Columns(4).Visible = False
        End Select
        GRVUtenti.DataBind()
        'oGestioneRisposte.calcoloPunteggio(Me.QuestionarioCorrente.id, DALUtenteInvitato.readUtentiInvitatiConRisposta(Me.QuestionarioCorrente.id))

    End Sub
    Protected Sub GRVUtenti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVUtenti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As UtenteInvitato = DirectCast(e.Row.DataItem, UtenteInvitato)
            Dim oLiteral As Literal = e.Row.FindControl("LTattemptsInfo")
            If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                oLiteral.Text = String.Format(Resource.getValue("AttemptsInfo"), item.AttemptNumber)
                oLiteral.Visible = True
            End If
            Me.Resource.setImageButton(e.Row.FindControl("IMBElimina"), False, True, True, True)
            Me.Resource.setImageButton(e.Row.FindControl("IMBVedi"), False, True, True, False)
        End If
      
    End Sub
    Protected Sub GRVUtenti_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVUtenti.PageIndexChanging
        GRVUtenti.PageIndex = e.NewPageIndex
        BindDati()
    End Sub
    Public Overrides Sub SetCultureSettings()
        If IsNothing(Me.QuestionarioCorrente) Then
            MyBase.SetCulture("pg_ucStatisticheUtentiQuest", "Questionari")
        Else
            Select Case Me.QuestionarioCorrente.tipo
                Case Questionario.TipoQuestionario.Sondaggio
                    MyBase.SetCulture("pg_ucStatisticheUtentiSondaggi", "Questionari")
                Case Questionario.TipoQuestionario.Meeting
                    MyBase.SetCulture("pg_ucStatisticheUtentiMeeting", "Questionari")
                Case Else
                    MyBase.SetCulture("pg_ucStatisticheUtentiQuest", "Questionari")
            End Select
        End If
        
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBTitolo)
            .setCheckBox(Me.CHKUtentiComunita)
            .setCheckBox(Me.CHKUtentiEsterni)
            .setCheckBox(Me.CHKUtentiInvitati)
            .setCheckBox(Me.CHKUtentiNonComunita)
            .setLabel(Me.LBNomeUtente)
            For Each item As Telerik.Web.UI.RadTab In TBSQuestionari.Tabs
                item.Text = .getValue(TBSQuestionari, item.Value)
                item.ToolTip = item.Text
            Next

            .setHeaderGridView(Me.GRVUtenti, 0, "headerNome", True)
            .setHeaderGridView(Me.GRVUtenti, 1, "headerEmail", True)
            .setHeaderGridView(Me.GRVUtenti, 2, "headerDescrizione", True)
            .setHeaderGridView(Me.GRVUtenti, 3, "headerVedi", True)
            .setHeaderGridView(Me.GRVUtenti, 4, "headerElimina", True)

            .setLinkButton(Me.LNBIndietro, True, False, False, False)
            '.setLinkButton(Me.LNBTornaLista, True, False, False, False)
            '.setLinkButton(Me.LNBstampaTutti, True, False, False, False)
            .setLinkButton(LNBexportResultsToCsv, False, True)
            .setLinkButton(LNBattemptsExportToCsv, False, True)
            .setLinkButton(LNBattemptsExportToXml, False, True)
            .setLinkButton(LNBexportResultsToXml, False, True)

            .setLinkButton(LNBexportResultsToSingleColumnCsv, False, True)
            .setLinkButton(LNBexportResultsToSingleColumnXml, False, True)

            .setHyperLink(HYPbackToManagement, True, True)
            .setHyperLink(HYPprintAll, True, True)
            .setHyperLink(HYPadvancedStatistics, True, True)

            Dim idQ As Integer = qs_questId
            Dim idType As String = "0"
            If qs_ownerId = 0 Then
                idQ = Me.QuestionarioCorrente.id
            End If
            If Not String.IsNullOrEmpty(qs_questTypeId) AndAlso IsNumeric(qs_questTypeId) Then
                idType = qs_questTypeId
            ElseIf Me.QuestionarioCorrente.tipo <> 4 Then
                idType = Me.QuestionarioCorrente.tipo.ToString()
            End If
            HYPbackToManagement.NavigateUrl = BaseUrl & RootObject.QuestionariStatList & "&type=" & idType
            HYPprintAll.NavigateUrl = BaseUrl & RootObject.Risposte & "?idq=" & idQ.ToString() & GetOwnerQueryString

            '        <url Name="StatQuiz" Path="Questionari/QuestionarioStat.aspx?type=0&amp;owType={2}&amp;owId={0}&amp;idQ={1}&amp;IdP={3}&amp;mode={4}"></url>
            HYPadvancedStatistics.NavigateUrl = BaseUrl & "Questionari/GeneralStatistics.aspx?type=" & idType & "&owType=" & qs_ownerTypeId.ToString & "&owId=" & qs_ownerId & "&IdP=" & qs_PersonaId & "&idQ=" & qs_questId & "&mode=0&NoBack=true"
            LNBattemptsExportToCsv.Visible = (Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts)
            LNBattemptsExportToXml.Visible = (Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts)
            If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                LNBattemptsExportToXml.CssClass &= " active"
            End If
        End With
    End Sub
    Protected Sub LNBStampaQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBStampaQuestionario.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("Content-Disposition", "attachment; filename=prova.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Response.Charset = ""
        Response.ContentEncoding = System.Text.Encoding.Default
        Me.EnableViewState = False
        Dim tw As New System.IO.StringWriter
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        DLPagine.RenderControl(hw)
        Response.Write(tw.ToString())
        Response.End()
    End Sub
    Protected Sub TBSQuestionari_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSQuestionari.TabClick
        BindDati()
    End Sub
    Protected Sub LNBOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBOk.Click
        BindDati()
    End Sub
    Protected Sub GRVUtenti_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVUtenti.RowCommand

        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim Identifyer As Integer = 0
            If IsNumeric(e.CommandArgument) Then
                Identifyer = CInt(e.CommandArgument)
            End If
            Dim item As UtenteInvitato = UtentiLista.Where(Function(u) u.RispostaID = Identifyer).FirstOrDefault()
            If IsNothing(item) Then
                BindDati()
            Else

                Select Case e.CommandName
                    Case "Vedi"
                        vediQuestionario(item.RispostaID, item.ID, item.PersonaID, item.IdRandomQuestionnaire)
                    Case "Elimina"
                        DALRisposte.cancellaRispostaBYID(item.RispostaID)
                        oGestioneQuest.DeleteOneAnswerActionAdd()
                        BindDati()
                End Select
            End If
            'Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            'Dim idUI As Integer = Me.UtentiLista(GRVUtenti.PageIndex * GRVUtenti.PageSize + row.RowIndex).ID
            'Dim idPersona As Integer = Me.UtentiLista(GRVUtenti.PageIndex * GRVUtenti.PageSize + row.RowIndex).PersonaID
            'Dim idRisposta As Integer = GRVUtenti.DataKeys(row.RowIndex).Values("RispostaID")
            
        End If
    End Sub
    Protected Sub LNBIndietro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBIndietro.Click
        CurrentStatistics = StatisticsType.UsersList
        'If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
        If Me.pageMode = TipoStatistiche.Utenti Then
            If GRVUtenti.Visible Then
                If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                    PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
                End If
            Else
                If qs_ownerId > 0 OrElse qs_ownerTypeId > 0 Then
                    HYPadvancedStatistics.Visible = (AllowedStandardActionType = lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics)
                    HYPprintAll.Visible = (AllowedStandardActionType = lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics)
                End If
                Me.HYPadvancedStatistics.Visible =
                Me.CurrentStatistics = StatisticsType.UsersList
                GRVUtenti.Visible = True
                DLPagine.Visible = False
                LBNomeUtente.Visible = False
                TBSQuestionari.Visible = True
                If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
                    LNBIndietro.Visible = False
                    PNLFiltri.Visible = True
                End If
            End If

        ElseIf Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity Then
            If String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                If qs_PersonaId = 0 OrElse qs_PersonaId = Me.UtenteCorrente.ID Then
                    Me.RedirectToUrl(RootObject.EduPath_CompileInActivity(Me.QuestionarioCorrente.ownerId))
                Else
                    'RedirectToUrl(RootObject.EduPath_PersonalStatInActivity(Me.QuestionarioCorrente.ownerId))
                End If
            Else
                PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
            End If

        ElseIf Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity Then
            If String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                'PageUtility.RedirectToUrl(lm.Comol.Modules.EduPath.BusinessLogic.RootObject.UserActivityStatView(
                '                          )
                ' RedirectToUrl(RootObject.EduPath_UserStatInSubActivity(Me.QuestionarioCorrente.ownerId, ComunitaCorrenteID, qs_PersonaId))
            Else
                PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
            End If
        End If
    End Sub
    'Protected Sub LNBTornaLista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBTornaLista.Click
    '    Response.Redirect("../" + RootObject.QuestionariStatList + "&type=" + Me.QuestionarioCorrente.tipo.ToString())
    'End Sub
    'Protected Sub LNBstampaTutti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBstampaTutti.Click

    '    Response.Redirect("../" + RootObject.Risposte + "?idq=" + Me.QuestionarioCorrente.id.ToString())
    'End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.pageMode = TipoStatistiche.Utente Then
            vediQuestionario(Me.QuestionarioCorrente.rispostaQuest.id, Me.QuestionarioCorrente.rispostaQuest.idUtenteInvitato, Me.QuestionarioCorrente.rispostaQuest.idPersona, Me.QuestionarioCorrente.idFiglio)
            LNBattemptsExportToCsv.Visible = (Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts)
            LNBattemptsExportToXml.Visible = (Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts)
            If Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts Then
                LNBattemptsExportToXml.CssClass &= " active"
            End If
        ElseIf Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio Then
            Me.TBSQuestionari.Tabs(TabIndex.Incompleto).Visible = False
        End If
        If Not Page.IsPostBack Then
            BindDati()
        End If
        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            LNBIndietro.Visible = False
        Else
            LNBIndietro.Visible = String.IsNullOrEmpty(Request.QueryString("NoBack")) OrElse Request.QueryString("NoBack") <> "True"
            HYPadvancedStatistics.Visible = True
            ' HYPprintAll.Visible = False
            HYPbackToManagement.Visible = False
            PNLFiltri.Visible = False
            DIVNomi.Visible = False
        End If
    End Sub
    Private Sub addUCpnlValutazione()
        'PNLmenu.Visible = False
        PHStat.Controls.Clear()
        Dim ctrl As New Control
        ctrl = Page.LoadControl(RootObject.ucPNLValutazione)
        ctrl.ID = "ucPNLvaluta"
        DirectCast(ctrl, UCpnlValutazione).carica()
        PHStat.Controls.Add(ctrl)
    End Sub
    Public Overrides Sub SetControlliByPermessi()

    End Sub

  
    Private Sub LNBattemptsExportToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBattemptsExportToCsv.Click
        ExportAttempts(CurrentStatistics, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBattemptsExportToXml_Click(sender As Object, e As System.EventArgs) Handles LNBattemptsExportToXml.Click
        ExportAttempts(CurrentStatistics, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub ExportAttempts(statSettings As StatisticsType, exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        If statSettings = StatisticsType.User OrElse statSettings = StatisticsType.UserAttempts OrElse pageMode = TipoStatistiche.Utente Then
            ExportAttempts(CurrentIdPerson, CurrentIdInvite, exportType)
        Else
            ExportAttempts(exportType)
        End If
    End Sub
    Private Sub ExportAttempts(exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Dim cookieName As String = "", cookieValue As String = ""
        RaiseEvent GetBlockUIinfos(cookieName, cookieValue)

        Dim cookie As HttpCookie
        If Not String.IsNullOrEmpty(cookieName) Then
            cookie = New HttpCookie(cookieName, cookieValue)
        End If
        Dim clientFileName As String = GetFileName("Attempts", exportType, True)
        Dim status As AnswerStatus = AnswerStatus.All

        Dim translations As New Dictionary(Of QuestionnaireExportTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(QuestionnaireExportTranslations))
            translations.Add([Enum].Parse(GetType(QuestionnaireExportTranslations), name), Me.Resource.getValue("QuestionnaireExportTranslations." & name))
        Next
        GestioneRisposte.ExportQuestionnaireAttempts(PageUtility.CurrentContext, Me.QuestionarioCorrente.id, status, Me.LinguaID, Resource.getValue("AnonymousUser"), translations, exportType, True, clientFileName, Response, cookie)
    End Sub
    Private Sub ExportAttempts(idPerson As Integer, idInvite As Integer, exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, Optional ByVal oneColumnForEachQuestion As Boolean = True)
        Dim cookieName As String = "", cookieValue As String = ""
        RaiseEvent GetBlockUIinfos(cookieName, cookieValue)

        Dim cookie As HttpCookie
        If Not String.IsNullOrEmpty(cookieName) Then
            cookie = New HttpCookie(cookieName, cookieValue)
        End If
        Dim clientFileName As String = GetUserFileName("UserAttempts", exportType, oneColumnForEachQuestion)
        Dim status As AnswerStatus = AnswerStatus.All
        If (Me.TBSQuestionari.SelectedTab.Value = 0) Then
            status = AnswerStatus.Completed
        Else
            status = AnswerStatus.Compiling
        End If
        Dim translations As New Dictionary(Of QuestionnaireExportTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(QuestionnaireExportTranslations))
            translations.Add([Enum].Parse(GetType(QuestionnaireExportTranslations), name), Me.Resource.getValue("QuestionnaireExportTranslations." & name))
        Next
        GestioneRisposte.ExportQuestionnaireAttempts(PageUtility.CurrentContext, idPerson, idInvite, Me.QuestionarioCorrente.id, status, Me.LinguaID, Resource.getValue("AnonymousUser"), translations, exportType, True, clientFileName, Response, cookie)
    End Sub
    Private Sub LNBexportResultsToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportResultsToCsv.Click
        ExportAnswers(CurrentStatistics, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportResultsToXml_Click(sender As Object, e As System.EventArgs) Handles LNBexportResultsToXml.Click
        ExportAnswers(CurrentStatistics, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportResultsToSingleColumnCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportResultsToSingleColumnCsv.Click
        ExportAnswers(CurrentStatistics, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv, False)
    End Sub
    Private Sub LNBexportResultsToSingleColumnXml_Click(sender As Object, e As System.EventArgs) Handles LNBexportResultsToSingleColumnXml.Click
        ExportAnswers(CurrentStatistics, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml, False)
    End Sub
    Private Sub ExportAnswers(statSettings As StatisticsType, exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, Optional ByVal oneColumnForEachQuestion As Boolean = True)
        If statSettings = StatisticsType.User OrElse statSettings = StatisticsType.UserAttempts OrElse pageMode = TipoStatistiche.Utente Then
            ExportAnswers(CurrentIdPerson, CurrentIdInvite, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv, oneColumnForEachQuestion)
        Else
            ExportAnswers(exportType, oneColumnForEachQuestion)
        End If
    End Sub
    'GestioneRisposte.PrintQuestionnaire(PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaID, Resource.getValue("AnonymousUser"), Response)
    Private Sub ExportAnswers(exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, Optional ByVal oneColumnForEachQuestion As Boolean = True)
        Dim cookieName As String = "", cookieValue As String = ""
        RaiseEvent GetBlockUIinfos(cookieName, cookieValue)

        Dim cookie As HttpCookie
        If Not String.IsNullOrEmpty(cookieName) Then
            cookie = New HttpCookie(cookieName, cookieValue)
        End If
        Dim clientFileName As String = GetFileName("QuestionnaireResults", exportType, oneColumnForEachQuestion)
        Dim status As AnswerStatus = AnswerStatus.All

        Dim translations As New Dictionary(Of QuestionnaireExportTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(QuestionnaireExportTranslations))
            translations.Add([Enum].Parse(GetType(QuestionnaireExportTranslations), name), Me.Resource.getValue("QuestionnaireExportTranslations." & name))
        Next
        GestioneRisposte.ExportQuestionnaireAnswers(PageUtility.CurrentContext, Me.QuestionarioCorrente.id, SystemSettings.Presenter.DefaultTaxCodeRequired, status, Me.LinguaID, Resource.getValue("AnonymousUser"), translations, exportType, True, clientFileName, Response, cookie, oneColumnForEachQuestion)
    End Sub
    Private Sub ExportAnswers(idPerson As Integer, idInvite As Integer, exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, Optional ByVal oneColumnForEachQuestion As Boolean = True)
        Dim cookieName As String = "", cookieValue As String = ""
        RaiseEvent GetBlockUIinfos(cookieName, cookieValue)

        Dim cookie As HttpCookie
        If Not String.IsNullOrEmpty(cookieName) Then
            cookie = New HttpCookie(cookieName, cookieValue)
        End If
        Dim clientFileName As String = GetUserFileName("UserQuestionnaireResults", exportType, oneColumnForEachQuestion)
        Dim status As AnswerStatus = AnswerStatus.All
        If (Me.TBSQuestionari.SelectedTab.Value = 0) Then
            status = AnswerStatus.Completed
        Else
            status = AnswerStatus.Compiling
        End If
        Dim translations As New Dictionary(Of QuestionnaireExportTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(QuestionnaireExportTranslations))
            translations.Add([Enum].Parse(GetType(QuestionnaireExportTranslations), name), Me.Resource.getValue("QuestionnaireExportTranslations." & name))
        Next
        GestioneRisposte.ExportUserQuestionnaireAnswers(PageUtility.CurrentContext, SystemSettings.Presenter.DefaultTaxCodeRequired, idPerson, idInvite, QuestionarioCorrente.id, status, Me.LinguaID, Resource.getValue("AnonymousUser"), translations, exportType, True, clientFileName, Response, cookie, oneColumnForEachQuestion)
    End Sub

    Private Function GetFileName(ByVal export As String, ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, ByVal oneColumnForEachItem As Boolean) As String
        Dim filename As String = Resource.getValue("Export.Filename." & export & "." & oneColumnForEachItem.ToString)
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            Select Case export
                Case "Attempts"
                    filename = "QuestionnaireAttempts_{0}_{1}_{2}_{3}"
                Case "QuestionnaireResults." & oneColumnForEachItem.ToString
                    filename = "QuestionnaireResults_{0}_{1}_{2}_{3}"
                Case "QuestionnaireResults." & oneColumnForEachItem.ToString
                    filename = "QuestionnaireResultsOneColumnForQuestion_{0}_{1}_{2}_{3}"
                Case Else
                    filename = "PathStatistics_{0}_{1}_{2}_{3}"
            End Select

        End If
        Return String.Format(filename, "{0}", oDate.Year, oDate.Month, oDate.Day) '& IIf(type <> lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.pdf, "." & type.ToString, "")
    End Function
    Private Function GetUserFileName(ByVal export As String, ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, ByVal oneColumnForEachItem As Boolean) As String
        Dim filename As String = Resource.getValue("Export.Filename." & export & "." & oneColumnForEachItem.ToString)
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            Select Case export
                Case "UserAttempts"
                    filename = "User_{0}_Questionnaire{1}_Attempts_{2}_{3}_{4}"
                Case "UserQuestionnaireResults." & oneColumnForEachItem.ToString
                    filename = "User_{0}_QuestionnaireOneColumnForQuestion_{1}_Results_{2}_{3}_{4}"
                Case "UserQuestionnaireResults." & oneColumnForEachItem.ToString
                    filename = "User_{0}_Questionnaire_{1}_Results_{2}_{3}_{4}"
                Case Else
                    filename = "User_{0}_Questionnaire_{1}_Results_{2}_{3}_{4}"
            End Select

        End If
        Return String.Format(filename, "{0}", "{1}", oDate.Year, oDate.Month, oDate.Day) '& IIf(type <> lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.pdf, "." & type.ToString, "")
    End Function
    Public Event GetBlockUIinfos(ByRef cookieName As String, ByRef cookieValue As String)

  
End Class