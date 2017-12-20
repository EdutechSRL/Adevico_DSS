Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Wiki
Imports lm.ActionDataContract

Partial Public Class Wiki_Comunita
	Inherits PageBase
    Implements COL_Wiki.WikiNew.IViewWiki


    Private _PageTitle As String

    Private MaxNumberRefreshSession As Integer = 6


    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property



#Region "Proprietà interne"

    Private _Utility As OLDpageUtility
    Private _Servizio As New UCServices.Services_Wiki 'Diverrà poi quello pagebase, eprmessi, etc...
    Private oPresenter As COL_Wiki.WikiNew.PresenterWiki
    Private _UltimaLettera As String = "^"

    Private _showauthors As Boolean
    Private ReadOnly Property Utility() As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
    End Property
    ReadOnly Property ViewDeleted() As Boolean Implements COL_Wiki.WikiNew.IViewWiki.ViewDeleted
        Get
            Return Me.CBX_SezVisEliminate.Checked
        End Get
    End Property
    Public Property ExternalTopicID() As System.Guid Implements COL_Wiki.WikiNew.IViewWiki.ExternalTopicID
        Get
            If String.IsNullOrEmpty(ViewState("ExternalTopicID").ToString) Then
                ViewState("ExternalTopicID") = 1
            End If

            Dim tGuid As Guid = New System.Guid(ViewState("ExternalTopicID").ToString)

            Return tGuid
        End Get
        Set(ByVal value As System.Guid)
            ViewState("ExternalTopicID") = value
        End Set
    End Property
    Private Property ExternalComunityID() As Integer Implements COL_Wiki.WikiNew.IViewWiki.ExternalComunityID
        Get
            If String.IsNullOrEmpty(ViewState("ExternalComunityID")) Then
                ViewState("ExternalComunityID") = 1
            End If
            Return ViewState("ExternalComunityID")
        End Get
        Set(ByVal value As Integer)
            ViewState("ExternalComunityID") = value
        End Set
    End Property
    Private Property PageIndex() As Integer Implements COL_Wiki.WikiNew.IViewWiki.pageIndex
        Get
            If String.IsNullOrEmpty(ViewState("Currentpagewikisearch")) Then
                ViewState("Currentpagewikisearch") = 1
            End If
            Return ViewState("Currentpagewikisearch")
        End Get
        Set(ByVal value As Integer)
            ViewState("Currentpagewikisearch") = value
        End Set
    End Property
    Public Property PageSize() As Integer Implements COL_Wiki.WikiNew.IViewWiki.pageSize
        Get
            If String.IsNullOrEmpty(ViewState("Sizepagewikisearch")) Then
                ViewState("Sizepagewikisearch") = 15
            End If
            Return ViewState("Sizepagewikisearch")
        End Get
        Set(ByVal value As Integer)
            ViewState("Sizepagewikisearch") = value
        End Set
    End Property
    Public Property IsLastPage() As Boolean Implements COL_Wiki.WikiNew.IViewWiki.IsLastPage
        Get
            If String.IsNullOrEmpty(ViewState("IsLastPageWiki")) Then
                ViewState("IsLastPageWiki") = False
            End If
            Return ViewState("IsLastPageWiki")
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsLastPageWiki") = value
        End Set
    End Property
    Private Property ActualTopicId() As System.Guid Implements COL_Wiki.WikiNew.IViewWiki.ActualTopicId

        Get
            Dim GuidOut As Guid
            If Not IsNothing(ViewState("Wiki_TopicId")) Then
                Try
                    Dim GuidStr As String = ViewState("Wiki_TopicId")
                    If GuidStr = "" Then
                        Return Guid.Empty
                    End If
                    'Return New System.Guid(GuidStr)
                    GuidOut = New System.Guid(GuidStr)
                    Return GuidOut
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Guid.Empty
            End If
        End Get
        Set(ByVal value As System.Guid)
            ViewState("Wiki_TopicId") = value.ToString
        End Set
    End Property

    Private Property SearchString() As String Implements COL_Wiki.WikiNew.IViewWiki.SearchString
        Get
            Dim temp As String
            temp = Me.TXB_search.Text
            Select Case DDL_ricerca.SelectedIndex
                Case 1
                    temp &= "%"
                Case 2
                    temp = "%" & temp
                Case 3
                    temp &= "%"
                    temp = "%" & temp
                Case Else
                    temp = temp
            End Select
            Return temp
        End Get
        Set(ByVal value As String)
            Me.TXB_search.Text = value
        End Set
    End Property

    Private Property ActualWikiId() As System.Guid Implements COL_Wiki.WikiNew.IViewWiki.ActualWikiId
        Get
            Dim GuidStr As String = ""
            Dim GuidOut As Guid
            Try
                GuidStr = ViewState("Wiki_Id")
                GuidOut = New System.Guid(GuidStr)
                'Return New System.Guid(GuidStr)
                Return GuidOut
            Catch ex As Exception
                GuidStr = Me.oPresenter.GetWikiID
                If Not GuidStr = "" Then
                    ViewState("Wiki_Id") = GuidStr
                    'Return New System.Guid(GuidStr)
                    'Return System.Guid.NewGuid(GuidStr)
                    GuidOut = New System.Guid(GuidStr)
                    Return GuidOut
                Else
                    Return System.Guid.Empty
                End If
            End Try
        End Get
        Set(ByVal value As System.Guid)
            ViewState("Wiki_Id") = value.ToString
        End Set
    End Property
    Public Property ActualSezioneId() As System.Guid Implements COL_Wiki.WikiNew.IViewWiki.ActualSezioneId

        Get
            Dim GuidOut As Guid
            If Not IsNothing(ViewState("Wiki_SezioneId")) Then
                Try
                    Dim GuidStr As String = ViewState("Wiki_SezioneId")
                    If GuidStr = "" Then
                        Return Guid.Empty
                    End If
                    GuidOut = New System.Guid(GuidStr)
                    Return GuidOut
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Guid.Empty
            End If
        End Get
        Set(ByVal value As System.Guid)
            ViewState("Wiki_SezioneId") = value.ToString
        End Set
    End Property
    'Viene utilizzato per un doppio uso
    Public Property OriginalSezioneID() As System.Guid Implements COL_Wiki.WikiNew.IViewWiki.OriginalSezioneID
        Get
            Dim SectionID As System.Guid = System.Guid.Empty

            If TypeOf (ViewState("Wiki_OriginalSezioneId")) Is System.Guid Then
                Try
                    SectionID = ViewState("Wiki_OriginalSezioneId")
                Catch ex As Exception

                End Try
            End If
            Return SectionID
        End Get
        Set(ByVal value As System.Guid)
            ViewState("Wiki_OriginalSezioneId") = value
        End Set
    End Property
    Private Property Servizio() As UCServices.Services_Wiki Implements COL_Wiki.WikiNew.IViewWiki.Servizio
        Get
            Return _Servizio
        End Get
        Set(ByVal value As UCServices.Services_Wiki)
            _Servizio = value
        End Set
    End Property

    Private Enum V_Contenuto As Integer
        V_SezioneNo = 0
        V_SezioneAddMod = 1
        V_TopicNo = 2
        V_TopicElenco = 3
        V_TopicAddMod = 4
        V_TopicCrono = 5
        V_TopicShow = 6
        V_TopicFounded = 7
        'V_ComunityList = 8
        V_ImportTopics = 8 'Attenzione sono due con il numero 7 che utilizzano la stessa data list
    End Enum
#End Region




#Region "Proprietà esterne"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property MyComunitaCorrente() As COL_BusinessLogic_v2.Comunita.COL_Comunita Implements COL_Wiki.WikiNew.IViewWiki.MyComunitaCorrente
        Get
            ''Return MyBase.ComunitaCorrente
            'Dim oComunita As New COL_BusinessLogic_v2.Comunita.COL_Comunita
            'Try
            '    oComunita.Id = Session("IdComunita") 'Session("ComunitaCorrente").id()
            'Catch ex As Exception
            '    oComunita.Id = -1
            'End Try
            Return Utility.ComunitaCorrente
        End Get
    End Property
    Public ReadOnly Property MyPersonaCorrente() As COL_BusinessLogic_v2.CL_persona.COL_Persona Implements COL_Wiki.IViewGenerico.MyPersonaCorrente
        Get
            Return MyBase.UtenteCorrente
        End Get
    End Property
    Public ReadOnly Property MyIscrizioneCorrente() As COL_BusinessLogic_v2.IscrizioneComunita Implements COL_Wiki.IViewGenerico.MyIscrizioneCorrente
        Get

        End Get
    End Property

    Public Property TitoloPagina() As String Implements COL_Wiki.IViewGenerico.TitoloPagina
        Get
            Return _PageTitle '"" 'Me.Master.ServiceTitle
        End Get
        Set(ByVal value As String)
            _PageTitle = value
            Me.Master.ServiceTitle = value
        End Set
    End Property
#End Region




#Region "PageBase"

    Public Overrides Sub BindDati()
        If Not Me.CheckQueryString() Then
            Dim SectionID As String = Request.QueryString("SectionID")
            If String.IsNullOrEmpty(SectionID) = False Then
                Me.ActualSezioneId = New Guid(SectionID)
            End If

            If Me.Page.IsPostBack = False AndAlso Me.ActualSezioneId <> System.Guid.Empty Then
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.ResetToUrlSection)
            Else
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.Reset)
            End If
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()

        Me.Show(COL_Wiki.WikiNew.Visualizzazioni.NoPermessi)


    End Sub
    Public Overrides Function HasPermessi() As Boolean
        If Me.ComunitaCorrenteID = 0 Then
            Return True
        End If
        Dim oTipoPermesso As UCServices.Services_Wiki.PermissionType
        Dim PermessiAssociati As String
        Try
            PermessiAssociati = Permessi(Me._Servizio.Codex, Me.Page)
            If Not (PermessiAssociati = "") Then
                Me._Servizio.PermessiAssociati = PermessiAssociati
            End If
        Catch ex As Exception
            Me._Servizio.PermessiAssociati = "00000000000000000000000000000000"
        End Try
        With Me._Servizio
            If .Admin Or .GestioneCronologia Or .GestioneSezioni Or .GestioneTopics Or .GestioneWiki Or .GrantPermission Or .Lettura Then
                Return True
            Else
                Return False
            End If
        End With
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Wiki_New", "Wiki")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBL_NoPermessi)
            .setLabel(LBL_NoWiki)
            .setLabel(LBL_Nav_NoSezione)
            .setLabel(LBL_Con_SezioneNO)

            '.setLabel(LBL_NoTopic_t)
            '.setLabel(LBL_NoTopicAdd_t)

            .setLabel(Lbl_NASezione_Nome_t)
            .setLabel(Lbl_NASezione_Descrizione_t)

            .setCheckBox(CBX_NASezione_IsPubblica_t)
            .setCheckBox(CBX_NASezione_IsDefault)

            .setLabel(LBL_ModTopic_Titolo_t)

            .setButton(BTN_Addtopic)
            .setButton(BTN_AddSezione)

            .setLabel(LBL_ElencoSezioneDescrizione_t)
            .setLabel(LBL_ElencoSezionePersona_t)

            .setLabel(LBL_NoWikiADD)
            .setLabel(LBL_WikiAdd_Nome_t)
            .setButton(Btn_AnnWiki, True, False, False, True)
            .setButton(BTN_AddWiki, True, False, False, True)

            .setLabel(LBL_Navigatore_t)

            .setButton(BTN_AddSezione, True, False, False, True)
            .setButton(BTN_ModSezione, True, False, False, True)
            .setButton(BTN_RecSezione, True, False, True, True)
            .setButton(BTN_DelSezione, True, False, True, True)
            .setButton(BTNSaveQuit, True, False, False, True)
            .setButton(BTNSaveContinua, True, False, False, True)
            .setButton(BTN_Addtopic, True, False, False, True)
            .setButton(BTN_ModWiki, True, False, False, True)

            'Aggiunti da alessandro
            .setButton(BTN_Home, True, False, False, True)
            .setButton(BTN_Import, True, False, False, True)
            .setButton(BTN_importa, True, False, False, True)
            .setButton(BTN_deselTutti, True, False, False, True)
            .setButton(BTN_selTutti, True, False, False, True)
            .setButton(BTN_search, True, False, False, True)
            .setButton(BTN_EditView, True, False, False, True)

            .setButton(BTN_Torna, True, False, False, True)
            .setLabel(LBL_ricerca)
            .setCheckBox(CBX_SezVisEliminate)
            .setCheckBox(CBX_Topic_IsPubblico)

            .setLabel(LBL_proce1)
            .setLabel(LBL_proce)
            .setLabel(LBL_PassoUno)
            .setLabel(LBL_PassoDue)

            .setButton(BTN_Sezione_Annulla, True, False, False, True)
            .setButton(BTN_Sezione_Salva, True, False, False, True)

            .setLabel(LBL_ElencoSezioneNome_t)
            .setButton(BTNAnnulla, True, False, False, True)
            .setLabel(LBL_WikiAdd_showauthors)
            .setCheckBox(CHB_WikiAdd_showauthors)

            Me.Master.ServiceTitle = .getValue("LBTitolo.text")

            .setButton(BTN_delete, False, False, False, False)
            .setButton(BTN_cancel, False, False, False, False)

            .setButton(BTN_keepedit, False, False, False, False)
            .setButton(BTN_backtolist, False, False, False, False)
            'Me.LBL_deletetopicmsg.Text = .getValue("deleteMessage")
        End With

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

#End Region




#Region "Property singoli elementi"

    Public ReadOnly Property SezioneDescrizione() As String Implements COL_Wiki.WikiNew.IViewWiki.SezioneDescrizione
        Get
            Return Me.CTRLeditorDescription.HTML
        End Get
    End Property
    Public ReadOnly Property SezioneIsDefault() As Boolean Implements COL_Wiki.WikiNew.IViewWiki.SezioneIsDefault
        Get
            Return Me.CBX_NASezione_IsDefault.Checked
        End Get
    End Property
    Public ReadOnly Property SezioneIsPubblica() As Boolean Implements COL_Wiki.WikiNew.IViewWiki.SezioneIsPubblica
        Get
            Return Me.CBX_NASezione_IsPubblica_t.Checked
        End Get
    End Property
    Public ReadOnly Property SezioneNome() As String Implements COL_Wiki.WikiNew.IViewWiki.SezioneNome
        Get
            Return Me.Txb_NASezione_Nome.Text
        End Get
    End Property

    Public ReadOnly Property TopicText() As String Implements COL_Wiki.WikiNew.IViewWiki.TopicText
        Get
            'Return Me.EditorAvanzato.Html
            Return Me.CTRLeditor.HTML '.EditorAvanzato.Html
        End Get
    End Property
    Public ReadOnly Property TopicNome() As String Implements COL_Wiki.WikiNew.IViewWiki.TopicNome
        Get
            Return Me.TXB_TitoloTopic.Text
        End Get
    End Property
    Public ReadOnly Property TopicIsCancellato() As Boolean Implements COL_Wiki.WikiNew.IViewWiki.TopicIsCancellato
        Get
            Return False
        End Get
    End Property
    Public ReadOnly Property TopicIsPubblico() As Boolean Implements COL_Wiki.WikiNew.IViewWiki.TopicIsPubblico
        Get
            Return Me.CBX_Topic_IsPubblico.Checked
        End Get
    End Property

    Public Property NomeWiki() As String Implements COL_Wiki.WikiNew.IViewWiki.NomeWiki
        Set(ByVal value As String)
            Me.TXB_WikiAdd_Nome.Text = value
        End Set
        Get
            Return Me.TXB_WikiAdd_Nome.Text
        End Get
    End Property
#End Region




#Region "Eventi page"
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.oPresenter = New COL_Wiki.WikiNew.PresenterWiki(Me, PageUtility.CurrentContext, COL_Wiki.FactoryWiki.ConnectionType.SQL, True)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.CheckQueryString() Then
            If (Not Me.ComunitaCorrenteID = 0) Then
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicView)
            Else
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicViewForced)
            End If
        End If
        'Me.CBX_SezVisEliminate.Visible = Me.Servizio.Admin
        'Me.AddAction(ActionType.List, Nothing)
        'Me.AddAction(ActionType.EditTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, System.Guid.NewGuid.ToString))
        PNL_DeleteTopic.Visible = False
    End Sub
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(UCServices.Services_Wiki.Codex)
    End Sub
#End Region

    Public Sub AnnullaQueryString(ByVal pagina As Page)

        pagina.RegisterClientScriptBlock("annulla_querystring", "<script language=""javascript"" type=""text/javascript"">document.forms[0].action = (document.forms[0].action.indexOf('?') != -1)?document.forms[0].action.substr(0, document.forms[0].action.indexOf('?')): document.forms[0].action;</script>")
    End Sub


#Region "Gestione eventi bottoni"
    Private Sub BTN_AddWiki_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_AddWiki.Click
        If Not Me.NomeWiki = "" Then
            Me.oPresenter.SaveOrUpdateWiki()

        End If
    End Sub
    Protected Sub BTN_Home_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Home.Click
        'Me.AnnullaQueryString(Me.Page)
        Response.Redirect(Request.Url.AbsolutePath)
        Me.oPresenter.BackToSection() '.BindDati(COL_Wiki.WikiNew.Binding.Reset)
    End Sub
    Private Sub IMG_avanti_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMG_avanti.Click
        If IsLastPage = False Then
            PageIndex = PageIndex + 1
        End If
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.ImportTopics)
    End Sub

    Private Sub IMG_indietro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMG_indietro.Click
        PageIndex = PageIndex - 1
        If PageIndex < 1 Then
            PageIndex = 1
        End If
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.ImportTopics)
    End Sub

    Protected Sub BTN_Torna_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Torna.Click
        If Me.ActualSezioneId = Guid.Empty Then
            Me.ActualSezioneId = Me.OriginalSezioneID
        End If
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
    End Sub
    Protected Sub BTN_Import_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Import.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.ComunityList)
    End Sub

    Protected Sub BTN_selTutti_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_selTutti.Click
        For Each riga As DataListItem In DLS_topicsImport.Items
            Dim tBox As CheckBox = riga.FindControl("CBX_selected")
            tBox.Checked = True
        Next
    End Sub

    Protected Sub BTN_deselTutti_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_deselTutti.Click
        For Each riga As DataListItem In DLS_topicsImport.Items
            Dim tBox As CheckBox = riga.FindControl("CBX_selected")
            tBox.Checked = False
        Next
    End Sub

    Protected Sub BTN_importa_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_importa.Click
        For Each riga As DataListItem In DLS_topicsImport.Items
            Dim tBox As CheckBox = riga.FindControl("CBX_selected")
            If tBox.Checked Then
                Dim tGuid As Guid = DLS_topicsImport.DataKeys(riga.ItemIndex)
                Me.ExternalTopicID = tGuid
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.ImportTopic)
            End If
        Next
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
    End Sub
    Private Sub BTN_ModWiki_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_ModWiki.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.WikiModify)
    End Sub
    Private Sub Btn_AnnWiki_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_AnnWiki.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
    End Sub
    Protected Sub BTN_EditView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_EditView.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicModify)
    End Sub
    Private Sub BTN_AddSezione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_AddSezione.Click
        'Server perchè altrtiemtni quando torno indietro non trova nessuna sezione
        Me.OriginalSezioneID = ActualSezioneId
        Me.ActualSezioneId = Guid.Empty
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneAdd)

    End Sub
    Private Sub BTN_ModSezione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_ModSezione.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneModify)
    End Sub
    Private Sub BTN_Sezione_Annulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Sezione_Annulla.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
    End Sub
    Private Sub BTN_Sezione_Salva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Sezione_Salva.Click
        Dim strTest As String = Me.ActualSezioneId.ToString
        If Me.ActualSezioneId = Nothing Then
            Me.AddAction(ActionType.CreateSection, Me.PageUtility.CreateObjectsList(ObjectType.Section, Me.ActualSezioneId.ToString))
        Else
            Me.AddAction(ActionType.EditSection, Me.PageUtility.CreateObjectsList(ObjectType.Section, Me.ActualSezioneId.ToString))
        End If

        Me.oPresenter.SaveOrUpdateSezione()

    End Sub
    Private Sub BTN_DelSezione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_DelSezione.Click

        Me.oPresenter.EliminaSezione()
        Me.AddAction(ActionType.DeleteSection, Me.PageUtility.CreateObjectsList(ObjectType.Section, Me.ActualSezioneId.ToString))
    End Sub
    Private Sub BTN_RecSezione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_RecSezione.Click
        Me.oPresenter.RecuperaSezione()
        Me.AddAction(ActionType.ResumeSection, Me.PageUtility.CreateObjectsList(ObjectType.Section, Me.ActualSezioneId.ToString))
    End Sub

    Private Sub BTN_Addtopic_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Addtopic.Click
        Me.ActualTopicId = Guid.Empty
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicAdd)
    End Sub
    Private Sub BTNSaveContinua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNSaveContinua.Click

        If Me.ActualTopicId = Guid.Empty Then
            Me.AddAction(ActionType.CreateTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Me.ActualTopicId.ToString))
        Else
            Me.AddAction(ActionType.EditTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Me.ActualTopicId.ToString))
        End If
        Me.oPresenter.SaveOrUpdateTopic(True)
    End Sub
    Private Sub BTNSaveQuit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNSaveQuit.Click
        If String.IsNullOrEmpty(TXB_TitoloTopic.Text) Then
            ''mostra un messaggio

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Inserire titolo topic", "alert(Inserire titolo topic")
        Else
            Me.oPresenter.SaveOrUpdateTopic(False)
        End If
        'Me.AddAction(ActionType.EditTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, System.Guid.NewGuid.ToString))
    End Sub
    Protected Sub BTN_search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_search.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicSearch)
        Me.AddAction(ActionType.SearchTopic, Me.PageUtility.CreateObjectsList(ObjectType.SearchString, Me.SearchString))
    End Sub
    Private Sub BTNAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNAnnulla.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
    End Sub
#End Region




#Region "DataBound, ItemCommand, etc..."
    Private Sub RPT_LinkNavigatore_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPT_LinkNavigatore.ItemCommand
        Me.AddAction(ActionType.List, Me.PageUtility.CreateObjectsList(ObjectType.Section, Me.ActualSezioneId.ToString))
        Select Case e.CommandName
            Case "GotoSection"
                Me.ActualSezioneId = New Guid(e.CommandArgument.ToString)
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
        End Select

    End Sub
    Public Sub BindTopicsFunded(ByVal Topics As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewWiki.BindTopicsFunded

        Me.PNL_search.Visible = True
        Me.DLS_result.DataSource = Topics
        Me.DLS_result.DataBind()

    End Sub
    Public Sub BindTopicsImport(ByVal Topics As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewWiki.BindTopicsImport

        'Me.PNL_search.Visible = True
        Me.DLS_topicsImport.DataSource = Topics
        Me.DLS_topicsImport.DataBind()

    End Sub
    Private Sub RPT_LinkNavigatore_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPT_LinkNavigatore.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim tmpSezione As COL_Wiki.WikiNew.SezioneWiki
            tmpSezione = e.Item.DataItem
            Dim LnkSezione As LinkButton
            Try
                LnkSezione = e.Item.FindControl("LNK_VoceNavi")

                LnkSezione.Text = tmpSezione.NomeSezione 'e.Item.DataItem("NomeSezione")
                LnkSezione.CommandName = "GotoSection"
                LnkSezione.CommandArgument = tmpSezione.ID.ToString 'e.Item.DataItem("ID")


                If tmpSezione.IsDeleted Then



                    If Me.Servizio.GestioneSezioni Or Me.Servizio.Admin Then
                        'LnkSezione.Text = Me.Resource.getValue("LnkSezione.IsDeleted").Replace("#%%#", LnkSezione.Text)

                    Else
                        LnkSezione.Visible = False
                        LnkSezione.Enabled = False
                    End If
                End If
            Catch ex As Exception

            End Try

            If tmpSezione.IsDeleted Then
                If Not CBX_SezVisEliminate.Checked Then
                    e.Item.Visible = False
                End If
            End If

        End If


    End Sub

    Protected Function Deleted(del As Boolean) As String
        If del Then
            Return "deleted"
        Else
            Return ""
        End If
    End Function

    Private Sub DL_TopicCrono_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles DL_TopicCrono.ItemCommand
        Dim oTopicCrono As New COL_Wiki.WikiNew.TopicHistoryWiki
        Dim idstr As String = e.CommandArgument.ToString
        oTopicCrono.ID = New Guid(idstr)
        Me.oPresenter.RecoverTopicCrono(oTopicCrono)
        Me.AddAction(ActionType.BackFromHistory, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Me.ActualTopicId.ToString))
    End Sub
    Private Sub DL_TopicCrono_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles DL_TopicCrono.ItemDataBound
        Dim BTNRipristina As Button
        Dim Lbl_Anagrafica As Label

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try
                BTNRipristina = e.Item.FindControl("BTNRipristina")
            Catch ex As Exception
                Exit Sub
            End Try

            Try
                Lbl_Anagrafica = e.Item.FindControl("Lbl_Crono_Anagrafica")
            Catch ex As Exception

            End Try

            Dim oTopicCrono As COL_Wiki.WikiNew.TopicHistoryWiki
            Try
                oTopicCrono = e.Item.DataItem
            Catch ex As Exception
            End Try


            If Me.Servizio.Admin Or Me.Servizio.GestioneCronologia Then

                Resource.setButton(BTNRipristina, True, False, True, True)

                With BTNRipristina
                    .Visible = True
                    .CommandName = "Ripristina"
                    .CommandArgument = oTopicCrono.ID.ToString
                End With
            Else
                BTNRipristina.Visible = False
            End If
            Lbl_Anagrafica.Text = oTopicCrono.Persona.Anagrafica

        End If

    End Sub

    Private Sub DLS_topics_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles DLS_topics.ItemCommand
        Try
            'Dim strTest As String = e.CommandArgument.ToString
            Me.ActualTopicId = New Guid(e.CommandArgument.ToString)

        Catch ex As Exception
        End Try

        Select Case e.CommandName
            Case "Modifica"
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicModify)

            Case "Elimina"
                Me.oPresenter.EliminaTopic()
                'Fare il "rebind" dei dati DOPO la cancellazione...
                Me.AddAction(ActionType.DeleteTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Me.ActualTopicId.ToString))
            Case "Cronologia"
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicCronologia)
                Me.AddAction(ActionType.ShowHistory, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Me.ActualTopicId.ToString))
            Case "Ripristina"
                Me.oPresenter.ripristinaTopic()
                Me.AddAction(ActionType.ResumeTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Me.ActualTopicId.ToString))

        End Select
    End Sub

    Private Sub DLS_topics_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLS_topics.ItemDataBound

        Dim LBL_intest1 As Label
        If e.Item.ItemType = ListItemType.Header Then
            Try
                LBL_intest1 = e.Item.FindControl("LBL_intest1")
            Catch ex As Exception

            End Try
            If Not IsNothing(LBL_intest1) Then
                LBL_intest1.Text = Resource.getValue("LBL_intest1.text")
            End If
        End If


        Dim tmpTopic As COL_Wiki.WikiNew.TopicWiki
        tmpTopic = e.Item.DataItem


        Dim BtnEdit, BtnDel, BtnCrono, BtnRip As Button
        'Dim PNL_temp As Panel = CType(e.Item.FindControl("PNL_DSL_item"), Panel)
        Dim LBL_temp As Label
        Dim LBL_temp2 As Label = CType(e.Item.FindControl("LBL_Iniziale"), Label)
        Try
            LBL_temp = e.Item.FindControl("LBL_topic")

            Dim SPN_Author As HtmlControl = CType(e.Item.FindControl("SPN_Author"), HtmlControl)

            SPN_Author.Visible = ShowAuthors

            If (Me._UltimaLettera = Char.ToUpper(LBL_temp.Text.Chars(0))) Then
                LBL_temp2.Visible = False
            Else
                LBL_temp2.Text = Char.ToUpper(LBL_temp.Text.Chars(0)) + "<br/>"
                LBL_temp2.Visible = True
                LBL_temp2 = e.Item.FindControl("LBL_separatore")
                LBL_temp2.Visible = True
            End If
            Me._UltimaLettera = Char.ToUpper(LBL_temp.Text.Chars(0))
        Catch ex As Exception

        End Try

        'Dim oTopic As COL_Wiki.WikiNew.TopicWiki
        'Try
        '    oTopic = e.Item.DataItem
        'Catch ex As Exception

        'End Try
        Try
            BtnEdit = e.Item.FindControl("BTNEdit")
            BtnDel = e.Item.FindControl("BTNDel")
            BtnCrono = e.Item.FindControl("BTNCrono")
            BtnRip = e.Item.FindControl("BTNRip")

            If Me.Servizio.Admin Or Me.Servizio.GestioneTopics Then
                With Resource
                    .setButton(BtnEdit, True, False, False, True)
                    .setButton(BtnDel, True, False, True, True)
                    .setButton(BtnCrono, True, False, False, True)
                    .setButton(BtnRip, True, False, False, True)
                    '--Per il pulsante del e ripristina verifichiamo se è eliminata
                    'e.Item.DataItem
                End With
                With BtnEdit
                    .Visible = True

                End With
            Else
                'If BtnRip.Enabled Then
                '    PNL_temp.Visible = False
                'End If
                BtnEdit.Visible = False
                BtnDel.Visible = False
                BtnCrono.Visible = False
                BtnRip.Visible = False
            End If

        Catch ex As Exception
            Exit Sub
        End Try

        If tmpTopic.isCancellato Then
            If Not CBX_SezVisEliminate.Checked Then
                e.Item.Visible = False
            End If
        End If
    End Sub

#End Region



#Region "visualizzazioni"
    Public Sub Show(ByVal Content As COL_Wiki.WikiNew.Visualizzazioni) Implements COL_Wiki.WikiNew.IViewWiki.Show

        Me.PNL_Navigatore.Enabled = True

        Me.PNL_NoPermessi.Visible = False
        Me.PNL_NoWiki.Visible = False
        Me.PNL_NoWiki_Add.Visible = False
        Me.PNL_Wiki.Visible = False
        Me.PNL_InfoImport.Visible = False
        Me.PNL_search.Visible = False
        Me.PNL_NavigatoreNoTopic.Visible = False
        Me.PNL_Navigatore.Visible = False
        Me.PNL_InfoSezione.Visible = False
        Me.PNL_ListaComunita.Visible = False
        Me.PNL_ViewTopic.Visible = False
        Me.PNL_ViewImport.Visible = False

        Me.MLV_Contenuto.Visible = False
        Me.DL_TopicCrono.Visible = False
        Me.Lbl_NoCronologia.Visible = False
        Me.DIV_Lbl_NoCronologia.Visible = False
        Me.BTN_AddSezione.Visible = False
        Me.BTN_Addtopic.Visible = False
        Me.BTN_AddWiki.Visible = False
        Me.BTN_Torna.Visible = False
        Me.BTN_ModWiki.Visible = False
        Me.BTN_Home.Visible = False
        Me.CBX_SezVisEliminate.Visible = False
        Me.BTN_Import.Visible = False
        Me.LBL_PassoUno.Enabled = False
        Me.LBL_PassoDue.Enabled = False

        'Me.BTN_Import.Visible = False
        'Me.BTN_Import.Enabled = False

        Select Case Content
            Case COL_Wiki.WikiNew.Visualizzazioni.NoPermessi
                Me.PNL_NoPermessi.Visible = True
                Me.AddAction(ActionType.NoPermission, Nothing)

            Case COL_Wiki.WikiNew.Visualizzazioni.NoWikiNoPermessi
                Me.PNL_NoWiki.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.NoWiki
                Me.PNL_NoWiki.Visible = True
                Me.PNL_NoWiki_Add.Visible = True
                Me.TXB_WikiAdd_Nome.Text = "Wiki    " & Me.ComunitaCorrente.Nome
                Me.LBL_NoWiki.Visible = True
                Me.DIV_LBL_NoWiki.Visible = True

                Me.LBL_NoWikiADD.Visible = True
                Me.DIV_LBL_NoWikiADD.Visible = True
                Me.BTN_AddWiki.Visible = True
                Me.CBX_SezVisEliminate.Visible = True
                'Me.BTN_Import.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.WikiMod
                Me.PNL_NoWiki.Visible = True
                Me.PNL_NoWiki_Add.Visible = True
                Me.LBL_NoWiki.Visible = False
                Me.DIV_LBL_NoWiki.Visible = False

                Me.LBL_NoWikiADD.Visible = False
                Me.DIV_LBL_NoWikiADD.Visible = False
                Me.BTN_AddWiki.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.NoSezioniNoPermessi
                Me.PNL_Wiki.Visible = True
                Me.PNL_NavigatoreNoTopic.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_SezioneNo
                Me.MLV_Contenuto.Visible = True
                Me.BTN_AddWiki.Visible = True
                Me.PNL_InfoSezione.Visible = False

            Case COL_Wiki.WikiNew.Visualizzazioni.NoSezione
                Me.PNL_Wiki.Visible = True
                Me.PNL_NavigatoreNoTopic.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_SezioneAddMod
                Me.MLV_Contenuto.Visible = True
                Me.BTN_AddSezione.Visible = True
                Me.BTN_AddWiki.Visible = True
                Me.CBX_SezVisEliminate.Visible = True
                'Me.BTN_Import.Visible = True
                Me.PNL_search.Visible = True



            Case COL_Wiki.WikiNew.Visualizzazioni.AddSezione
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_SezioneAddMod
                Me.MLV_Contenuto.Visible = True
                Me.BTN_Torna.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.NoTopicNoPermessi
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicNo
                Me.MLV_Contenuto.Visible = True


            Case COL_Wiki.WikiNew.Visualizzazioni.NoTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicNo
                Me.MLV_Contenuto.Visible = True
                Me.BTN_AddSezione.Visible = True
                Me.BTN_Addtopic.Visible = True
                Me.CBX_SezVisEliminate.Visible = True
                Me.BTN_Import.Visible = True
                Me.PNL_InfoSezione.Visible = True
                Me.PNL_search.Visible = True

                'Me.BTN_Import.Enabled = True
                'Me.BTN_Import.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.ListaTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicElenco
                Me.MLV_Contenuto.Visible = True
                Me.BTN_AddSezione.Visible = True
                Me.BTN_Addtopic.Visible = True
                Me.CBX_SezVisEliminate.Visible = True
                Me.BTN_Import.Visible = True
                Me.BTN_ModWiki.Visible = True
                Me.PNL_InfoSezione.Visible = True
                Me.PNL_search.Visible = True

                'Me.BTN_Import.Enabled = True
                'Me.BTN_Import.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.AddTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicAddMod
                Me.MLV_Contenuto.Visible = True
                Me.BTN_Torna.Visible = True

                Me.BTN_Import.Enabled = True
                Me.BTN_Import.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.ModifyTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicAddMod
                Me.MLV_Contenuto.Visible = True
                Me.BTN_Torna.Visible = True

                Me.BTN_Import.Enabled = True
                Me.BTN_Import.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.ShowTopic
                Me.PNL_Wiki.Visible = False 'Me.PNL_Wiki.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = -1
                Me.PNL_ViewTopic.Visible = True
                Me.MLV_Contenuto.Visible = True
                Me.LBL_TestView.Visible = True
                'Me.BTN_Addtopic.Visible = True
                If Me.CheckQueryString Then
                    Me.BTN_Home.Visible = True
                End If
                If Not Me.ActualSezioneId = Guid.Empty Then
                    Me.BTN_Torna.Visible = True
                End If


            Case COL_Wiki.WikiNew.Visualizzazioni.CronologiaTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicCrono
                Me.MLV_Contenuto.Visible = True
                Me.DL_TopicCrono.Visible = True
                Me.BTN_Torna.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.CronologiaNoTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicCrono
                Me.MLV_Contenuto.Visible = True
                Lbl_NoCronologia.Visible = True
                Me.DIV_Lbl_NoCronologia.Visible = True
                Me.BTN_Torna.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.ListaTopicSearched
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_TopicFounded
                Me.MLV_Contenuto.Visible = True
                Me.BTN_Torna.Visible = True
                Me.PNL_search.Visible = True

            Case COL_Wiki.WikiNew.Visualizzazioni.ListaComunita
                Me.PNL_Wiki.Visible = False 'Me.PNL_Wiki.Visible = True
                'Me.PNL_Navigatore.Visible = True
                'Me.PNL_Navigatore.Enabled = False
                Me.PNL_InfoImport.Visible = True
                Me.MLV_Contenuto.Visible = True
                Me.PNL_ListaComunita.Visible = True
                'Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_ComunityList
                Me.MLV_Contenuto.ActiveViewIndex = -1
                Me.BTN_Torna.Visible = True
                Me.LBL_PassoUno.Enabled = True

            Case COL_Wiki.WikiNew.Visualizzazioni.ListaTopicsImport
                Me.PNL_Wiki.Visible = False 'Me.PNL_Wiki.Visible = True
                'Me.PNL_Navigatore.Visible = True
                'Me.PNL_Navigatore.Enabled = False
                Me.BTN_selTutti.Visible = True
                Me.BTN_deselTutti.Visible = True
                Me.PNL_InfoImport.Visible = True
                Me.MLV_Contenuto.Visible = True
                'Me.MLV_Contenuto.ActiveViewIndex = Me.V_Contenuto.V_ImportTopics
                Me.PNL_ViewImport.Visible = True
                Me.BTN_Torna.Visible = True
                Me.LBL_PassoDue.Enabled = True

                'Case COL_Wiki.WikiNew.Visualizzazioni.ListaTopicSearched




        End Select

        If Me.ComunitaCorrenteID = 0 Then
            CBX_SezVisEliminate.Visible = False
            PNL_InfoSezione.Visible = False
        End If


        If Me.Servizio.Admin Then
            'Non faccio niente perchè l'amministratore deve vedere tutto
        ElseIf Me.Servizio.GestioneWiki Then
            Me.BTN_ModWiki.Visible = False
            Me.BTN_Import.Visible = False
        ElseIf Me.Servizio.GestioneSezioni Then
            Me.BTN_ModWiki.Visible = False
            Me.BTN_Import.Visible = False
        ElseIf Me.Servizio.GestioneTopics Then
            Me.BTN_AddSezione.Visible = False
            Me.BTN_ModSezione.Visible = False
            Me.BTN_DelSezione.Visible = False
            Me.BTN_ModWiki.Visible = False
            Me.BTN_Import.Visible = False
        Else
            Me.CBX_SezVisEliminate.Visible = False
            Me.BTN_AddSezione.Visible = False
            Me.BTN_Addtopic.Visible = False
            Me.BTN_ModSezione.Visible = False
            Me.BTN_DelSezione.Visible = False
            Me.BTN_ModWiki.Visible = False
            Me.BTN_EditView.Visible = False
            Me.BTN_Import.Visible = False
        End If

        'BOTTONI DISATTIVATI PER MOTIVI DI TEST O ALTRO
        'Me.BTN_Import.Visible = False

    End Sub

    Public Property ActualView() As COL_Wiki.WikiNew.Visualizzazioni Implements COL_Wiki.WikiNew.IViewWiki.ActualView
        Get
            Dim Out As COL_Wiki.WikiNew.Visualizzazioni
            Try
                If Not IsNothing(ViewState("WikiView")) Then
                    Out = CInt(ViewState("WikiView"))
                Else
                    Out = COL_Wiki.WikiNew.Visualizzazioni.PageNotRender
                End If
            Catch ex As Exception
                Out = COL_Wiki.WikiNew.Visualizzazioni.PageNotRender
            End Try
            Return Out
        End Get
        Set(ByVal value As COL_Wiki.WikiNew.Visualizzazioni)
            ViewState("WikiView") = value
        End Set
    End Property
#End Region



#Region "Binding"
    'ATTENZIONE!!! SOLO settaggio delle varie cose, la loro visualizzazione è decisa poi dal PRESENTER!
    Public Sub BindNavigatore(ByVal Sezioni As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewWiki.BindNavigatore
        Me.RPT_LinkNavigatore.DataSource = Sezioni

        Me.RPT_LinkNavigatore.DataBind()
    End Sub

    Public Sub BindSezione(ByVal oSezione As COL_Wiki.WikiNew.SezioneWiki) Implements COL_Wiki.WikiNew.IViewWiki.BindSezione
        If Me.Servizio.Admin Or Me.Servizio.GestioneSezioni Then
            With oSezione
                Me.Txb_NASezione_Nome.Text = .NomeSezione
                Me.TXB_NASezione_Descrizione.Text = .PlainDescription
                Me.CBX_NASezione_IsDefault.Checked = .IsDefault
                Me.CBX_NASezione_IsPubblica_t.Checked = .IsPubblica
                Me.ActualSezioneId = .ID
                Me.CTRLeditorDescription.HTML = .Descrizione
            End With
        Else
            With oSezione
                If Not .IsDeleted Then
                    Me.Txb_NASezione_Nome.Text = .NomeSezione
                    Me.TXB_NASezione_Descrizione.Text = .PlainDescription
                    Me.CBX_NASezione_IsDefault.Checked = .IsDefault
                    Me.CBX_NASezione_IsPubblica_t.Checked = .IsPubblica
                    Me.ActualSezioneId = .ID
                    Me.CTRLeditorDescription.HTML = .Descrizione
                Else
                    Me.Show(COL_Wiki.WikiNew.Visualizzazioni.NoSezione)
                End If
            End With
        End If
    End Sub

    Public Sub BindTopicsSezione(ByVal Topics As System.Collections.IList, ByVal Sezione As COL_Wiki.WikiNew.SezioneWiki) Implements COL_Wiki.WikiNew.IViewWiki.BindTopicsSezione

        Me.PNL_InfoSezione.Visible = True
        'Me.DLTopics.Visible = True

        If Me.Servizio.Admin Or Me.Servizio.GestioneSezioni Then
            If Not Sezione.IsDeleted Then
                PNL_InfoSezione.CssClass = "InfoSezione"
                Me.BTN_DelSezione.Visible = True
                Me.BTN_RecSezione.Visible = False
                Me.LBL_SezioneElininata.Visible = False
            Else
                PNL_InfoSezione.CssClass = "InfoSezioneDeleted"
                Me.BTN_DelSezione.Visible = False
                Me.BTN_RecSezione.Visible = True
                Me.LBL_SezioneElininata.Visible = True
            End If
        ElseIf Sezione.IsDeleted Then
            Me.PNL_InfoSezione.Visible = False
            'Me.DLTopics.Visible = False
            Exit Sub
        End If

        'Carichiamo il riquadro sulla modifica della sezione
        Me.LBL_ElencoSezioneNome.Text = Sezione.NomeSezione
        Me.LBL_ElencoSezioneDescrizione.Text = Sezione.Descrizione

        If Sezione.Descrizione = "" Then
            DIVdescription.Visible = False
        End If

        Me.LBL_NomeSezione.Text = Sezione.NomeSezione
        Me.LBL_NomeComunita.Text = Me.ComunitaCorrente.Nome
        Try
            Me.LBL_ElencoSezionePersona.Text = Sezione.Persona.Anagrafica
        Catch ex As Exception
        End Try

        Me.DLS_topics.DataSource = Topics
        Me.DLS_topics.DataBind()
    End Sub

    Public Sub BindComunita() Implements COL_Wiki.WikiNew.IViewWiki.BindComunita
        Dim oService As Services_Wiki = Services_Wiki.Create()
        oService.Admin = True
        oService.Lettura = True
        Dim oServiceBase As New ServiceBase(0, oService.Codex, oService.PermessiAssociati)
        Dim oClause As New GenericClause(Of ServiceClause)
        oClause.OperatorForNextClause = OperatorType.OrCondition
        oClause.Clause = New ServiceClause(oServiceBase, OperatorType.AndCondition)
        Me.CTRLcommunity.ServiceClauses = oClause
        Me.CTRLcommunity.InitializeControl(0)
    End Sub

    Public Sub BindTopic(ByVal oTopic As COL_Wiki.WikiNew.TopicWiki) Implements COL_Wiki.WikiNew.IViewWiki.BindTopic
        Me.TXB_TitoloTopic.Text = oTopic.Nome
        Me.CTRLeditor.HTML = oTopic.Contenuto
        Me.ActualTopicId = oTopic.ID

        Me.CBX_Topic_IsPubblico.Checked = oTopic.IsPubblica
    End Sub
    Public Sub BindTopicTest(ByVal oTopic As COL_Wiki.WikiNew.TopicWiki) Implements COL_Wiki.WikiNew.IViewWiki.BindTopicTest
        Me.ActualTopicId = oTopic.ID
        Me.TXB_TitoloTopic.Text = oTopic.Nome
        Me.LBL_TitoloTopicView.Text = oTopic.Nome
        'Me.LBL_autore.Text = "Inserito da " & oTopic.Persona.Anagrafica.ToString & "  ultima modifica " & oTopic.DataModifica.ToString
        Me.LBL_autore.Text = oTopic.Persona.Anagrafica.ToString & " - "
        Me.LBL_date.Text = oTopic.DataModifica.ToString
        Me.LBL_autore.Visible = ShowAuthors
        Me.LBL_TestView.Text = oTopic.Contenuto
        Me.CTRLeditor.HTML = oTopic.Contenuto
        Me.CBX_Topic_IsPubblico.Checked = oTopic.IsPubblica
    End Sub

    Public Sub Bindcronologia(ByVal Lista As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewWiki.bindcronologia
        If Lista.Count > 0 Then
            Me.DL_TopicCrono.DataSource = Lista
            Me.DL_TopicCrono.DataBind()
        End If
    End Sub

#End Region



#Region "Ajax"

    Private Sub TMR_Session_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMR_Session.Tick
        'Vedo se esiste già altrimenti la creo
        If String.IsNullOrEmpty(Session("MyNumberRefresh")) Then
            Session("MyNumberRefresh") = 1
        Else
            If MaxNumberRefreshSession < Session("MyNumberRefresh") Then
                Session.Abandon()
            Else
                Dim T As Integer = Integer.Parse(Session("MyNumberRefresh"))
                T = T + 1
                Session("MyNumberRefresh") = T
                LBL_Ajax.Text = T
            End If
        End If
    End Sub
#End Region



    Protected Sub CBX_SezVisEliminate_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CBX_SezVisEliminate.CheckedChanged
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
    End Sub

    Public Sub AddAction(ByVal oType As Integer, ByVal oObjectActions As List(Of PresentationLayer.WS_Actions.ObjectAction), Optional ByVal TypeIteration As InteractionType = InteractionType.UserWithUser)
        Me.PageUtility.AddAction(oType, oObjectActions, TypeIteration)
    End Sub

    Private Sub CTRLcommunity_SelectedCommunityChanged(ByVal CommunityID As Integer) Handles CTRLcommunity.SelectedCommunityChanged
        Me.ExternalComunityID = Nothing

        Me.ExternalComunityID = CommunityID
        If Not CommunityID = -1 Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.ImportTopics)
        End If
    End Sub
    Private Function CheckQueryString() As Boolean
        'Verifica se c'è una query string con un guid e se è valido


        If Not (String.IsNullOrEmpty(Request.QueryString("id"))) Then
            Try
                Dim TopicID As String = Request.QueryString("id")
                Me.ActualTopicId = New Guid(TopicID)

                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If

    End Function

    'Private Sub DLS_result_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLS_result.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Header Then
    '        Try
    '            Dim LBL_intVoce As Label
    '            LBL_intVoce = DLS_result.FindControl("LBL_intVoce")
    '            Resource.setLabel(LBL_intVoce)
    '            Dim LBL_intSezione As Label
    '            LBL_intSezione = DLS_result.FindControl("LBL_intSezione")
    '            Resource.setLabel(LBL_intSezione)
    '            Dim LBL_intSezione1 As Label
    '            LBL_intSezione1 = DLS_result.FindControl("LBL_intSezione1")
    '            Resource.setLabel(LBL_intSezione1)
    '            Dim LBL_intVoce1 As Label
    '            LBL_intVoce1 = DLS_result.FindControl("LBL_intVoce1")
    '            Resource.setLabel(LBL_intVoce1)
    '            Dim LBL_intComunita As Label
    '            LBL_intComunita = DLS_result.FindControl("LBL_intComunita")
    '            Resource.setLabel(LBL_intComunita)
    '        Catch ex As Exception

    '        End Try
    '    End If
    'End Sub


    Private Sub DLS_result_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DLS_result.PreRender
        Dim LBL_intVoce As Label
        LBL_intVoce = DLS_result.FindControl("LBL_intVoce")
        Resource.setLabel(LBL_intVoce)
        Dim LBL_intSezione As Label
        LBL_intSezione = DLS_result.FindControl("LBL_intSezione")
        Resource.setLabel(LBL_intSezione)
        Dim LBL_intSezione1 As Label
        LBL_intSezione1 = DLS_result.FindControl("LBL_intSezione1")
        Resource.setLabel(LBL_intSezione1)
        Dim LBL_intVoce1 As Label
        LBL_intVoce1 = DLS_result.FindControl("LBL_intVoce1")
        Resource.setLabel(LBL_intVoce1)
        Dim LBL_intComunita As Label
        LBL_intComunita = DLS_result.FindControl("LBL_intComunita")
        Resource.setLabel(LBL_intComunita)


    End Sub

    Sub NotifyTopicAdd(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal CreatorName As String) Implements COL_Wiki.WikiNew.IViewWiki.NotifyTopicAdd
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifyTopicAdd(CommunityID, TopicID, Title, CreatorName)
    End Sub
    Sub NotifyTopicEdit(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal CreatorName As String) Implements COL_Wiki.WikiNew.IViewWiki.NotifyTopicEdit
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifyTopicEdit(CommunityID, TopicID, Title, CreatorName)
    End Sub
    Sub NotifyTopicDelete(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal UserName As String) Implements COL_Wiki.WikiNew.IViewWiki.NotifyTopicDelete
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifyTopicDelete(CommunityID, TopicID, Title, UserName)
    End Sub

    Sub NotifySectionAdd(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionName As String, ByVal SectionId As System.Guid) Implements COL_Wiki.WikiNew.IViewWiki.NotifySectionAdd
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifySectionAdd(CommunityID, CreatorName, SectionName, SectionId)
    End Sub
    Sub NotifySectionEdit(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionId As System.Guid, ByVal PreviousName As String, ByVal NewName As String) Implements COL_Wiki.WikiNew.IViewWiki.NotifySectionEdit
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifySectionEdit(CommunityID, CreatorName, SectionId, PreviousName, NewName)
    End Sub
    Sub NotifySectionDelete(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionId As System.Guid, ByVal SectionName As String) Implements COL_Wiki.WikiNew.IViewWiki.NotifySectionDelete
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifySectionDelete(CommunityID, CreatorName, SectionId, SectionName)
    End Sub

    Public Sub NotifySectionRipristina(ByVal CommunityID As Integer, ByVal SectionId As System.Guid, ByVal SectionName As String, ByVal DataCreation As Date, ByVal UserName As String) Implements COL_Wiki.WikiNew.IViewWiki.NotifySectionRipristina
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifySectionRipristina(CommunityID, SectionId, SectionName, DataCreation, UserName)
    End Sub

    Public Sub NotifyTopicRipristina(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal DataCreation As Date, ByVal UserName As String) Implements COL_Wiki.WikiNew.IViewWiki.NotifyTopicRipristina
        Dim oService As New WikiNotificationUtility(Me.Utility)
        oService.NotifyTopicRipristina(CommunityID, TopicID, Title, DataCreation, UserName)
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

    Public Sub ConfirmDelete(id As Guid, msg As IList(Of KeyValuePair(Of String, Guid))) Implements COL_Wiki.WikiNew.IViewWiki.ConfirmDelete
        Me.PNL_DeleteTopic.Visible = True
        BTN_delete.CommandArgument = id.ToString
        BTN_cancel.CommandArgument = id.ToString

        Dim links As String = ""
        Dim deleteMessage As String = Resource.getValue("deleteMessage")
        Dim pattern As String = "<a href='{0}' class='wikilink'>{1}</a><br />"

        For Each item As KeyValuePair(Of String, Guid) In msg
            Dim url As String = PageUtility.ApplicationUrlBase + "Wiki/Wiki_Comunita.aspx?id=" + item.Value.ToString

            links += String.Format(pattern, url, item.Key)
        Next

        UC_Message.InitializeControl(deleteMessage, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        UC_Message.Visible = True
        DIV_deletetopic.Visible = True
        DIV_missinglinks.Visible = False

        LBL_deletetopicmsg.Text = links
    End Sub

    Private Sub BTN_delete_Click(sender As Object, e As EventArgs) Handles BTN_delete.Click
        ActualTopicId = New Guid(BTN_delete.CommandArgument)
        oPresenter.ForceEliminaTopic()
        ActualTopicId = Guid.Empty
        PNL_DeleteTopic.Visible = False
        BTN_cancel.CommandArgument = ActualTopicId.ToString
        BTN_delete.CommandArgument = ActualTopicId.ToString
        UC_Message.Visible = False
        DIV_deletetopic.Visible = False
        DIV_missinglinks.Visible = False
    End Sub

    Private Sub BTN_cancel_Click(sender As Object, e As EventArgs) Handles BTN_cancel.Click
        ActualTopicId = Guid.Empty
        PNL_DeleteTopic.Visible = False
        BTN_cancel.CommandArgument = ActualTopicId.ToString
        BTN_delete.CommandArgument = ActualTopicId.ToString
        UC_Message.Visible = False
        DIV_deletetopic.Visible = False
        DIV_missinglinks.Visible = False
    End Sub


    Public Property DisplayAuthors As Boolean Implements COL_Wiki.WikiNew.IViewWiki.DisplayAuthors
        Get
            Return CHB_WikiAdd_showauthors.Checked
        End Get
        Set(value As Boolean)
            CHB_WikiAdd_showauthors.Checked = value
        End Set
    End Property


    Public ReadOnly Property SectionDescriptionText As String Implements COL_Wiki.WikiNew.IViewWiki.SectionDescriptionText
        Get
            Return CTRLeditorDescription.Text
        End Get
    End Property

    Public ReadOnly Property TopicPlainText As String Implements COL_Wiki.WikiNew.IViewWiki.TopicPlainText
        Get
            Return CTRLeditor.Text
        End Get
    End Property

    Public Property ShowAuthors As Boolean Implements COL_Wiki.WikiNew.IViewWiki.ShowAuthors
        Get
            Return _showauthors
        End Get
        Set(value As Boolean)
            _showauthors = value

            SPN_SectionAuthor.Visible = value
            LBL_autore.Visible = value
        End Set
    End Property

    Private Sub BTN_backtolist_Click(sender As Object, e As EventArgs) Handles BTN_backtolist.Click
        Me.oPresenter.SwitchToList()
    End Sub

    Private Sub BTN_keepedit_Click(sender As Object, e As EventArgs) Handles BTN_keepedit.Click
        ActualTopicId = Guid.Empty
        PNL_DeleteTopic.Visible = False
        BTN_cancel.CommandArgument = ActualTopicId.ToString
        BTN_delete.CommandArgument = ActualTopicId.ToString
        UC_Message.Visible = False
        DIV_deletetopic.Visible = False
        DIV_missinglinks.Visible = False
    End Sub

    Public Property ShowErrorMessage As Boolean Implements COL_Wiki.WikiNew.IViewWiki.ShowErrorMessage
        Get
            Return DIV_missinglinks.Visible
        End Get
        Set(value As Boolean)
            Dim missinglinks = Resource.getValue("missinglinks")

            If value Then
                UC_Message.InitializeControl(missinglinks, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                UC_Message.Visible = True
                PNL_DeleteTopic.Visible = True
                DIV_missinglinks.Visible = value
                DIV_deletetopic.Visible = Not value
            End If
        End Set
    End Property
End Class



'<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

'<html xmlns="http://www.w3.org/1999/xhtml">
'<head id="Head1" runat="server">

'    <link href="./../Styles.css" type="text/css" rel="stylesheet"/>
'	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>




'<%--
'        <script type="text/javascript" src="<%=Me.BaseUrl %>Jscript/jquery-1.4.1.min.js"></script>
'    <script type="text/javascript" src="<%=Me.BaseUrl %>Jscript/jquery.validate.min.js"></script>--%>

'</head>
'<body>
'     <form id="aspnetForm" runat="server" defaultbutton="BTN_search" defaultfocus="TXB_search">
'     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
'        <div id="DVcontenitore" align="center">
'            <div id="DVheader" style="width: 900px;" align="center">
'                <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
'		    </div>

'	   	    <div id="DVtitle" style="width: 900px; text-align:left;" class="RigaTitolo" align="center">
'			    <asp:Label ID="LBTitolo" Runat="server">Wiki</asp:Label>
'		    </div>





'		    <div id="DVfooter" align="center" style="clear: both;">
'			    <FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
'		    </div>
'	    </div>

'    </form>
'</body>
'</html>