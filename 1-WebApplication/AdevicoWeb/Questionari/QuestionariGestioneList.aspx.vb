Imports COL_Questionario


Partial Public Class _QuestionariGestioneList
    Inherits PageBaseQuestionario

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Service As COL_Questionario.Business.ServiceQuestionnaire
    Private ReadOnly Property CurrentService() As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_Service) Then
                _Service = New COL_Questionario.Business.ServiceQuestionnaire(Me.PageUtility.CurrentContext)
            End If
            Return _Service
        End Get
    End Property
#End Region

    ' indica la modalità della pagina: 0=amministrazione, 1=statistiche
    Private ReadOnly Property pageMode() As String
        Get
            Return Request.QueryString("mode")
        End Get
    End Property

    ' indica il tipo di lista: 0=questionari, 1=libreriedidomande, 2=sondaggi
    Private ReadOnly Property groupType() As String
        Get
            Select Case Me.qs_questIdType
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    Return QuestionarioGruppo.TipoGruppo.CartellaLibrerie
                Case Else
                    Return QuestionarioGruppo.TipoGruppo.CartellaQuestionari
            End Select
        End Get
    End Property

    Protected ReadOnly Property CookieName As String
        Get
            Return "QuestionariGestioneList_" '& LoaderGuid.ToString
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayStatisticsToken.Message")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayStatisticsToken.Title")
        End Get
    End Property

    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Private Enum TipoLista
        Gestione = 0
        Statistiche = 1
    End Enum
    Dim oQuest As New COL_Questionario.Questionario
    Dim oGestioneQuest As New GestioneQuestionario

   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("isZero") = "true" And Me.ComunitaCorrenteID > 0 Then
            GoToPortale()
        End If
    End Sub
    'Private Sub creaGruppoDefault(ByVal idComunita As Integer, ByVal tipo As Integer)
    '    Me.GruppoCorrente.idComunita = idComunita
    '    Me.GruppoCorrente.nome = COL_Questionario.RootObject.nomeGruppoDefault
    '    Me.GruppoCorrente.idGruppoPadre = 0
    '    Me.GruppoCorrente.id = DALQuestionarioGruppo.InsertGruppo(Me.GruppoCorrente)
    'End Sub
    Private Sub Bind()

        If Not Me.pageMode Is Nothing Then

            Dim oImg As ImageButton

            oImg = Me.IMBHelp

            If Me.qs_questIdType = Questionario.TipoQuestionario.Sondaggio Then
                oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpSondaggiGestioneList, "target", "yes", "yes"))
            Else
                oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpQuestionariGestioneList, "target", "yes", "yes"))
            End If

            BindGRVElenco()

        End If

    End Sub
    Private Sub BindGRVElenco()
        Me.GruppoCorrente = DALQuestionario.readQuestionariByGruppo(Me.GruppoCorrente.id, Me.qs_questIdType)
        GRVElenco.PageSize = RootObject.nRighePaginaGridView
        GRVElenco.DataSource = Me.GruppoCorrente.questionari
        GRVElenco.DataBind()
        If Me.qs_questIdType = Questionario.TipoQuestionario.LibreriaDiDomande OrElse Me.qs_questIdType = Questionario.TipoQuestionario.Modello Then
            GRVElenco.Columns(0).Visible = False
        ElseIf Me.qs_questIdType = Questionario.TipoQuestionario.Meeting Then
            If pageMode = 1 Then
                GRVElenco.Columns(3).Visible = True
            Else
                GRVElenco.Columns(3).Visible = False
            End If
        End If
    End Sub
    Private Sub BindDLGruppi()
        DLGruppi.DataSource = DALQuestionarioGruppo.readSottoGruppi(Me.GruppoCorrente.id, Me.ComunitaCorrenteID)
        DLGruppi.DataBind()

        LBLCartella.Text = Me.GruppoCorrente.nome
    End Sub
    Protected Sub DLGruppiItemCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        Me.GruppoCorrente.id = DLGruppi.DataKeys.Item(e.Item.ItemIndex)

        If e.CommandName = "viewGruppo" Then
            Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&" & qs_questType & CInt(Me.qs_questIdType))
        End If

    End Sub
    Protected Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVElenco.RowCommand

        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim indiceQuestionario = row.RowIndex + (GRVElenco.PageIndex * RootObject.nRighePaginaGridView)
            Dim idQuest As Integer = GRVElenco.DataKeys(row.RowIndex).Value
            Dim idCommunity As Integer = CurrentService.GetQuestionnaireIdCommunity(idQuest)
            oQuest.id = idQuest
            oQuest.linguePresenti = Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti
            oQuest.tipo = Me.GruppoCorrente.questionari(indiceQuestionario).tipo

            Dim ddl As New DropDownList
            ddl = row.FindControl("DDLLingue")

            If e.CommandName <> "Export" AndAlso e.CommandName <> "NotIsBloccato" AndAlso e.CommandName <> "Elimina" Then
                Me.LinguaQuestionario = ddl.SelectedItem.Value
                Me.QuestionarioCorrente = oQuest 'ok
            End If


            Select Case e.CommandName
                Case "Gestione"
                    Dim url As String = "idLanguage={0}&IdQ={1}&" & qs_questType + oQuest.tipo.ToString()
                    url = String.Format(url, ddl.SelectedItem.Value, oQuest.id)
                    If qs_questIdType = Questionario.TipoQuestionario.Meeting Then
                        Me.RedirectToUrl(RootObject.MeetingWiz)
                    ElseIf qs_questIdType = Questionario.TipoQuestionario.Sondaggio Then
                        Me.RedirectToUrl(RootObject.SondaggioAdmin + "?" & url)
                    Else
                        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?" & url) ' & qs_questType + oQuest.tipo.ToString())
                    End If
                Case "Domande"
                    Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, oQuest.id, Me.LinguaQuestionario, False)
                    Me.RedirectToUrl(RootObject.QuestionarioEdit + "?IdQ=" & oQuest.id & "&" & qs_questType + oQuest.tipo.ToString())
                Case "StatisticheGenerali"
                    Me.QuestionarioCorrente.isBloccato = False
                    Me.RedirectToUrl(RootObject.QuestionarioStatisticheGenerali & "&IdQ=" & oQuest.id & "&" & qs_questType + oQuest.tipo.ToString())
                Case "StatisticheUtenti"
                    Me.QuestionarioCorrente.isBloccato = False
                    Me.RedirectToUrl(RootObject.QuestionarioStatisticheUtenti & "&IdQ=" & oQuest.id & "&" & qs_questType + oQuest.tipo.ToString())
                Case "Anteprima"
                    If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, True, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, Me.UtenteCorrente.ID, Me.Invito.ID)
                    End If
                    Me.RedirectToUrl(RootObject.QuestionarioView & "?IdQ=" & oQuest.id & "&mode=0&" & qs_questType & oQuest.tipo.ToString())
                Case "Elimina"
                    If oQuest.linguePresenti.Count = 1 Then
                        DALQuestionario.DeleteQuestionarioBYPadre(oQuest)
                        oGestioneQuest.DeleteActionAdd(idQuest, CurrentService.ServiceModuleID(), idCommunity)
                        Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&" & qs_questType & CInt(Me.qs_questIdType))
                    Else
                        Me.MLVquestionari.SetActiveView(VIWCancellaQuestionario)
                        LBLNomeQuestionario.Text += Me.GruppoCorrente.questionari(indiceQuestionario).nome
                        DLLingue.DataSource = oQuest.linguePresenti
                        DLLingue.DataBind()
                    End If
                Case "NotIsBloccato"
                    DALQuestionario.IsBloccatoByIdQuestionario_Update(Me.GruppoCorrente.questionari(indiceQuestionario).id, Not Me.GruppoCorrente.questionari(indiceQuestionario).isBloccato)
                    Me.QuestionarioCorrente = Me.GruppoCorrente.questionari(indiceQuestionario)
                    If Me.QuestionarioCorrente.isBloccato Then
                        Dim oGestioneQuestionario As New GestioneQuestionario
                        oGestioneQuestionario.notifyCurrentQuestionnaire()
                    End If
                    Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&" & qs_questType & CInt(Me.qs_questIdType))
                Case "Export"
                    oGestioneQuest.ExportActionAdd(idQuest, CurrentService.ServiceModuleID(), idCommunity)
                    ExportAnswers(idQuest, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv)
            End Select
        End If
    End Sub

    Private Sub ExportAnswers(ByVal idQuest As Integer, exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Dim cookie As New HttpCookie(CookieName, HDNdownloadTokenValue.Value)

        Dim clientFileName As String = GetFileName("QuestionnaireResults", exportType)
        Dim status As AnswerStatus = AnswerStatus.All

        Dim translations As New Dictionary(Of QuestionnaireExportTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(QuestionnaireExportTranslations))
            translations.Add([Enum].Parse(GetType(QuestionnaireExportTranslations), name), Me.Resource.getValue("QuestionnaireExportTranslations." & name))
        Next
        GestioneRisposte.ExportQuestionnaireAnswers(PageUtility.CurrentContext, idQuest, SystemSettings.Presenter.DefaultTaxCodeRequired, status, Me.LinguaID, Resource.getValue("AnonymousUser"), translations, exportType, True, clientFileName, Response, cookie)
    End Sub


    Private Function GetFileName(ByVal export As String, ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String
        Dim filename As String = Resource.getValue("Export.Filename." & export)
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            Select Case export
                Case "Attempts"
                    filename = "QuestionnaireAttempts_{0}_{1}_{2}_{3}"
                Case "QuestionnaireResults"
                    filename = "QuestionnaireResults_{0}_{1}_{2}_{3}"
            End Select

        End If
        Return String.Format(filename, "{0}", oDate.Year, oDate.Month, oDate.Day) '& IIf(type <> lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.pdf, "." & type.ToString, "")
    End Function

    Protected Sub GRVElenco_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVElenco.RowDataBound
        If e.Row.RowIndex >= 0 Then
            If Not Me.qs_questIdType = Questionario.TipoQuestionario.LibreriaDiDomande Then
                Dim dataI As DateTime = Convert.ToDateTime(e.Row.Cells(5).Text)
                If dataI = Date.MinValue Or dataI > Date.MaxValue.AddDays(-1) Then
                    e.Row.Cells(5).Text = String.Empty
                End If
                Dim dataF As DateTime = Convert.ToDateTime(e.Row.Cells(6).Text)
                If dataF = Date.MinValue Or dataF > Date.MaxValue.AddDays(-1) Then
                    e.Row.Cells(6).Text = String.Empty
                End If
            End If
            Dim indiceQuestionario = e.Row.RowIndex + (GRVElenco.PageIndex * RootObject.nRighePaginaGridView)
            Dim lingue As New List(Of Lingua)
            If Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti.Count = 0 Then
                lingue = DALQuestionario.readLingueQuestionario(Me.GruppoCorrente.questionari(indiceQuestionario).id)
                Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti = lingue
            End If

            DirectCast(e.Row.FindControl("DDLLingue"), DropDownList).DataSource = Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti
            DirectCast(e.Row.FindControl("DDLLingue"), DropDownList).DataBind()
            DirectCast(e.Row.FindControl("DDLLingue"), DropDownList).SelectedValue = Me.GruppoCorrente.questionari(indiceQuestionario).idLingua
            Me.LinguaDefaultQuestionario = Me.GruppoCorrente.questionari(indiceQuestionario).idLingua

            If Me.GruppoCorrente.questionari(indiceQuestionario).isReadOnly Then
                DirectCast(e.Row.FindControl("IMChiuso"), System.Web.UI.WebControls.Image).Visible = True
            Else
                DirectCast(e.Row.FindControl("IMChiuso"), System.Web.UI.WebControls.Image).Visible = False
            End If

            If Me.GruppoCorrente.questionari(indiceQuestionario).isBloccato And Not Me.GruppoCorrente.questionari(indiceQuestionario).TipoQuestionario.LibreriaDiDomande = Me.GruppoCorrente.questionari(indiceQuestionario).tipo Then
                DirectCast(e.Row.FindControl("IMBbloccato"), System.Web.UI.WebControls.Image).Visible = True
                DirectCast(e.Row.FindControl("IMBsbloccato"), System.Web.UI.WebControls.Image).Visible = False
            Else
                DirectCast(e.Row.FindControl("IMBbloccato"), System.Web.UI.WebControls.Image).Visible = False
                DirectCast(e.Row.FindControl("IMBsbloccato"), System.Web.UI.WebControls.Image).Visible = True
            End If

            GRVElenco.Columns(3).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.VisualizzaStatistiche  ' colonna gestione domande
            GRVElenco.Columns(4).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.VisualizzaStatistiche ' colonna cancella questionario
            DirectCast(e.Row.FindControl("IMBGestione"), System.Web.UI.WebControls.Image).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito
            DirectCast(e.Row.FindControl("LNKNomeQuestionario"), System.Web.UI.WebControls.LinkButton).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito
            DirectCast(e.Row.FindControl("LBLNomeQuestionario"), Label).Visible = Not (MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito)

            If Me.pageMode = TipoLista.Statistiche Then
                GRVElenco.Columns(1).Visible = False ' colonna chiuso/bloccato
                GRVElenco.Columns(7).Visible = False ' colonna lingue
                DirectCast(e.Row.FindControl("LNKNomeQuestionario"), LinkButton).Visible = False
                DirectCast(e.Row.FindControl("LBLNomeQuestionario"), Label).Visible = True

                e.Row.FindControl("IMBAnteprima").Visible = False
                e.Row.FindControl("IMBStatisticheGenerali").Visible = True
                e.Row.FindControl("IMBStatisticheUtenti").Visible = Not Me.GruppoCorrente.questionari(indiceQuestionario).risultatiAnonimi
                e.Row.FindControl("IMBGestione").Visible = False
                e.Row.FindControl("IMBDomande").Visible = False
                e.Row.FindControl("IMBElimina").Visible = False
            End If
            If Me.qs_questIdType = Questionario.TipoQuestionario.LibreriaDiDomande Then
                GRVElenco.Columns(5).Visible = False 'colonna datainizio
                GRVElenco.Columns(6).Visible = False 'colonna datafine
                'ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario And pageMode = 1 Then
            ElseIf Me.GruppoCorrente.questionari(indiceQuestionario).tipo = Questionario.TipoQuestionario.Questionario And pageMode = 1 Then
                e.Row.FindControl("IMBExport").Visible = True
                Me.Resource.setImageButton(DirectCast(e.Row.FindControl("IMBExport"), ImageButton), False, True)
            End If
            setInternazionalizzaioneGRVElencoControls(e)
        End If

    End Sub
    Protected Sub LNBNuovoQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNuovoQuestionario.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Me.qs_questIdType
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Or Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario Or Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random Then
            Me.RedirectToUrl(RootObject.QuestionarioAdd + "?type=-1")
        ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting Then
            Me.RedirectToUrl(RootObject.MeetingWiz)
        ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            Me.RedirectToUrl(RootObject.SondaggioAdmin & "?" & qs_questType & Me.QuestionarioCorrente.tipo)
        Else
            Me.RedirectToUrl(RootObject.QuestionarioAdmin & "?" & qs_questType & Me.QuestionarioCorrente.tipo)
        End If
    End Sub
    Protected Sub LNBNuovaCartella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNuovaCartella.Click
        Me.RedirectToUrl(RootObject.GruppoAdmin)
    End Sub
    'Eseguito SOLO al primo accesso alla pagina
    Public Overrides Sub BindDati()
        'carico il gruppo di default
        Me.PaginaCorrenteID = 0

        If Me.GruppoCorrente Is Nothing Then
            Me.GruppoCorrente = New QuestionarioGruppo
        End If

        Me.GruppoCorrente.id = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrenteID)

        If Me.GruppoCorrente.id = 0 Then
            Dim oGestQuest As New GestioneQuestionario
            oGestQuest.creaGruppoDefault(Me.ComunitaCorrenteID)
        End If

        Bind()

        If GRVElenco.Rows.Count > 0 Then
            Me.MLVquestionari.SetActiveView(Me.VIWdati)
        Else
            LBerrore.Text = Me.Resource.getValue("MSGnoQuestionari")
            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
        End If

        If Page.IsPostBack = False Then
            oGestioneQuest.ViewAdminListActionAdd(Me.qs_questIdType)
        End If

    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        'If Me.pageMode = TipoLista.Gestione Then
        '    Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande)
        'Else
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande Or MyBase.Servizio.VisualizzaStatistiche Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito)
        'End If
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        Select Case Me.qs_questIdType
            Case Questionario.TipoQuestionario.Sondaggio
                MyBase.SetCulture("pg_GestioneSondaggiList", "Questionari")
            Case Questionario.TipoQuestionario.Meeting
                MyBase.SetCulture("pg_GestioneMeetingList", "Questionari")
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                MyBase.SetCulture("pg_GestioneLibrerieList", "Questionari")
            Case Else
                MyBase.SetCulture("pg_GestioneQuestionariList", "Questionari")
        End Select
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle." & pageMode)
            .setLinkButton(LNBNuovoQuestionario, False, False)
            .setLinkButton(LNBNuovaCartella, False, False)
            .setLinkButton(LNBCestino, False, False)
            .setLabel(LBerrore)
            .setLabel(LBLCartella)
            .setLabel(LBTitoloSottoCartelle)
            .setHeaderGridView(Me.GRVElenco, 0, "headerStato", True)
            .setHeaderGridView(Me.GRVElenco, 1, "headerEntra", True)
            .setHeaderGridView(Me.GRVElenco, 2, "headerNome", True)
            If Me.pageMode = TipoLista.Gestione Then
                .setHeaderGridView(Me.GRVElenco, 3, "headerModifica", True)
                .setHeaderGridView(Me.GRVElenco, 4, "headerElimina", True)
            Else
                .setHeaderGridView(Me.GRVElenco, 3, "headerStatGenerali", True)
                .setHeaderGridView(Me.GRVElenco, 4, "headerStatUtenti", True)
            End If
            .setHeaderGridView(Me.GRVElenco, 5, "headerDataInizio", True)
            .setHeaderGridView(Me.GRVElenco, 6, "headerDataFine", True)
            .setHeaderGridView(Me.GRVElenco, 7, "headerLingua", True)
            .setImageButton(IMBHelp, False, False, True, False)
            .setLabel(LBHelp)
            .setLabel(LBLNomeQuestionario)
            .setLabel(LBLTestoDescrizione)
            .setButton(BTNEliminaLingua, False, False, True, False)
        End With
    End Sub
    Protected Sub setInternazionalizzaioneGRVElencoControls(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        With Me.Resource
            .setImage(e.Row.FindControl("IMChiuso"), True)
            .setImageButton(e.Row.FindControl("IMBbloccato"), False, True, True, True)
            .setImageButton(e.Row.FindControl("IMBsbloccato"), False, True, True, True)

            .setImageButton(e.Row.FindControl("IMBAnteprima"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBGestione"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBDomande"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBElimina"), False, True, True, True)
            .setImageButton(e.Row.FindControl("IMBStatisticheGenerali"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBStatisticheUtenti"), False, True, True, False)

        End With
    End Sub
    Protected Sub GRVElenco_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        GRVElenco.DataSource = Me.GruppoCorrente.questionari
        GRVElenco.DataBind()
    End Sub
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property
    Protected Sub LNBCestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCestino.Click
        Me.RedirectToUrl(RootObject.QuestionariCestinoList & "?" & qs_questType & CInt(Me.qs_questIdType))
    End Sub
    Public Overrides Sub SetControlliByPermessi()

        If Me.pageMode = "0" Then
            LNBCestino.Visible = MyBase.Servizio.Admin
            LNBNuovoQuestionario.Visible = MyBase.Servizio.Admin
        Else
            LNBCestino.Visible = False
            LNBNuovoQuestionario.Visible = False
        End If

        ' opzioni al momento disattive
        PNLGruppi.Visible = False
        LNBNuovaCartella.Visible = False
        LBLCartella.Visible = False


    End Sub
    Protected Sub BTNEliminaLingua_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNEliminaLingua.Click
        Dim idCommunity As Integer = CurrentService.GetQuestionnaireIdCommunity(QuestionarioCorrente.id)
        Dim idModule As Integer = CurrentService.ServiceModuleID()
        For Each it As DataListItem In DLLingue.Items
            If DirectCast(it.FindControl("CHKSelezionaLingua"), CheckBox).Checked Then
                Dim idLingua As Integer = DLLingue.DataKeys.Item(it.ItemIndex)
                Dim idQuest As Integer = Me.QuestionarioCorrente.id
                Dim idQuestLanguage As Integer = CurrentService.GetQuestionnaireIdByLanguage(idQuest, idLingua)
                DALQuestionario.DeleteQuestionarioByIDLingua(idQuest, idLingua)
                If idQuestLanguage > 0 Then
                    oGestioneQuest.DeleteActionAdd(idQuest, idLingua, idQuestLanguage, idModule, idCommunity)
                Else
                    oGestioneQuest.DeleteActionAdd(idQuest, idLingua, idModule, idCommunity)
                End If

                Me.MLVquestionari.SetActiveView(Me.VIWdati)
                Bind()
            End If
        Next
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