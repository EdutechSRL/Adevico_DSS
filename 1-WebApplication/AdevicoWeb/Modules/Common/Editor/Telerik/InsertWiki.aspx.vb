Imports COL_Wiki.WikiNew

Public Class InsertWiki
    Inherits PageBase
    Implements COL_Wiki.WikiNew.IViewEditorWiki

#Region "Context"
    Private _Servizio As New UCServices.Services_Wiki 'Diverrà poi quello pagebase, eprmessi, etc...
    Private _Presenter As PresenterWikiEditor
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentPresenter As PresenterWikiEditor
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PresenterWikiEditor(Me.PageUtility.CurrentContext, Me)
            End If

            Return _Presenter
        End Get
    End Property

    'Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
    '    Get
    '        If IsNothing(_CurrentContext) Then
    '            _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
    '        End If
    '        Return _CurrentContext
    '    End Get
    'End Property
    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property ServiceSkinNew As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.PageUtility.CurrentContext)
            End If

            Return _SkinService
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property PageIndex() As Integer Implements COL_Wiki.WikiNew.IViewEditorWiki.pageIndex
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
    Public Property PageSize() As Integer Implements COL_Wiki.WikiNew.IViewEditorWiki.pageSize
        Get
            If String.IsNullOrEmpty(ViewState("Sizepagewikisearch")) Then
                ViewState("Sizepagewikisearch") = 5
            End If
            Return ViewState("Sizepagewikisearch")
        End Get
        Set(ByVal value As Integer)
            ViewState("Sizepagewikisearch") = value
        End Set
    End Property
    Public Property IsLastPage() As Boolean Implements COL_Wiki.WikiNew.IViewEditorWiki.IsLastPage
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

    'Public ReadOnly Property MyComunitaCorrente() As COL_BusinessLogic_v2.Comunita.COL_Comunita Implements COL_Wiki.WikiNew.IViewWikiUC.MyComunitaCorrente
    '    Get
    '        'Return MyBase.ComunitaCorrente
    '        Dim oComunita As New COL_BusinessLogic_v2.Comunita.COL_Comunita
    '        Try
    '            oComunita.Id = Session("IdComunita") 'Session("ComunitaCorrente").id()
    '        Catch ex As Exception
    '            oComunita.Id = -1
    '        End Try
    '        Return oComunita
    '    End Get
    'End Property
    Private Property ActualWikiId() As System.Guid Implements COL_Wiki.WikiNew.IViewEditorWiki.ActualWikiId
        Get
            Dim GuidStr As String = ""
            Dim GuidOut As Guid
            Try
                GuidStr = Session("Wiki_Id")
                GuidOut = New System.Guid(GuidStr)
                'Return New System.Guid(GuidStr)
                Return GuidOut
            Catch ex As Exception
                GuidStr = Me.CurrentPresenter.GetWikiID
                If Not GuidStr = "" Then
                    Session("Wiki_Id") = GuidStr
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
            Session("Wiki_Id") = value.ToString
        End Set
    End Property
    Private Property SearchString() As String Implements COL_Wiki.WikiNew.IViewEditorWiki.SearchString
        Get
            Dim temp As String
            temp = Me.TXBsearch.Text
            Select Case DDLsearchBy.SelectedIndex
                Case 1
                    temp &= "%"
                Case 2
                    temp = "%" & temp
                Case 3
                    temp &= "%"
                    temp = "%" & temp
                Case Else
                    If (String.IsNullOrEmpty(temp)) Then
                        temp = "%"
                    End If
                    temp = temp
            End Select
            Return temp
        End Get
        Set(ByVal value As String)
            Me.TXBsearch.Text = value
        End Set
    End Property
    Private Property ActualTopicId() As System.Guid Implements COL_Wiki.WikiNew.IViewEditorWiki.ActualTopicId
        Get
            If Not IsNothing(Session("Wiki_TopicId")) Then
                Try
                    Dim GuidStr As String = Session("Wiki_TopicId")
                    If GuidStr = "" Then
                        Return Guid.Empty
                    End If
                    Return New System.Guid(GuidStr)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Guid.Empty
            End If
        End Get
        Set(ByVal value As System.Guid)
            Session("Wiki_TopicId") = value.ToString
        End Set
    End Property
    Private Property ActualSezioneId() As System.Guid Implements COL_Wiki.WikiNew.IViewEditorWiki.ActualSezioneId
        Get
            If Not IsNothing(Session("Wiki_SezioneId")) Then
                Try
                    Dim GuidStr As String = Session("Wiki_SezioneId")
                    Return New System.Guid(GuidStr)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As System.Guid)
            Session("Wiki_SezioneId") = value.ToString
        End Set
    End Property
    Private Property Servizio() As UCServices.Services_Wiki Implements COL_Wiki.WikiNew.IViewEditorWiki.Servizio
        Get
            Return _Servizio
        End Get
        Set(ByVal value As UCServices.Services_Wiki)
            _Servizio = value
        End Set
    End Property
    Public Property ActualView() As COL_Wiki.WikiNew.VisualizzazioniUC Implements COL_Wiki.WikiNew.IViewEditorWiki.ActualView
        Get
            Dim Out As COL_Wiki.WikiNew.VisualizzazioniUC
            Try
                If Not IsNothing(ViewState("WikiView")) Then
                    Out = CInt(ViewState("WikiView"))
                Else
                    Out = COL_Wiki.WikiNew.VisualizzazioniUC.PageNotRender
                End If
            Catch ex As Exception
                Out = COL_Wiki.WikiNew.VisualizzazioniUC.PageNotRender
            End Try
            Return Out
        End Get
        Set(ByVal value As COL_Wiki.WikiNew.VisualizzazioniUC)
            ViewState("WikiView") = value
        End Set
    End Property
    'Public ReadOnly Property MyPersonaCorrente() As COL_BusinessLogic_v2.CL_persona.COL_Persona Implements COL_Wiki.IViewGenerico.MyPersonaCorrente
    '    Get
    '        Return MyBase.UtenteCorrente
    '    End Get
    'End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
#Region "Control"
    Public ReadOnly Property AppUrl As String
        Get
            Return PageUtility.ApplicationUrlBase(True)
        End Get
    End Property
#End Region

    Private Sub Wiki_links_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me._Presenter = New COL_Wiki.WikiNew.PresenterWikiEditor(PageUtility.CurrentContext, Me, COL_Wiki.FactoryWiki.ConnectionType.SQL, True)

        Me.setSkins()
    End Sub

    Private Sub Wiki_links_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Me.TXBsearch.Text = Me.HDNdefaultSearch.Value
        End If
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If ComunitaCorrenteID > 0 Then
            Me.CBXcurrentCommunity.Checked = True
        Else
            Me.CBXpublicWiki.Checked = True
        End If
        Me.CurrentPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.Reset)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Show(COL_Wiki.WikiNew.VisualizzazioniUC.NoPermessi)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Dim oServizio As New CL_permessi.COL_Servizio
        oServizio.EstraiByCode(UCServices.Services_Wiki.Codex, MyBase.LinguaID)
        Return oServizio.isAttivato
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Wiki_Home", "Wiki")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBsearchBy) '        
            .setLabel(LBsearchInto) '
            .setDropDownList(DDLsearchBy, 0)
            .setDropDownList(DDLsearchBy, 1)
            .setDropDownList(DDLsearchBy, 2)
            .setDropDownList(DDLsearchBy, 3)
            .setCheckBox(CBXcurrentCommunity)
            .setCheckBox(CBXpublicWiki)
            .setLabel(LBnotopicFound)
            .setLabel(LBnoPermission)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub Show(ByVal Content As COL_Wiki.WikiNew.VisualizzazioniUC) Implements COL_Wiki.WikiNew.IViewEditorWiki.Show
        'Verifica se attivare i pulsanti avanti indetro della paginazione
        If PageIndex = 1 Then
            Me.IMG_indietro.Visible = False
        Else
            Me.IMG_indietro.Visible = True
        End If
        If IsLastPage = True Then
            Me.IMG_avanti.Visible = False
        Else
            Me.IMG_avanti.Visible = True
        End If

        Select Case Content
            Case COL_Wiki.WikiNew.VisualizzazioniUC.NoPermessi
                Me.MultiView.ActiveViewIndex = 1

            Case COL_Wiki.WikiNew.VisualizzazioniUC.NoTopic
                Me.MultiView.ActiveViewIndex = 0

            Case COL_Wiki.WikiNew.VisualizzazioniUC.ListaTopicSearched
                Me.MultiView.ActiveViewIndex = 2
        End Select
    End Sub
    Public Sub LoadTopics(ByVal Topics As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewEditorWiki.LoadTopics
        Me.DLSresult.DataSource = Topics
        Me.DLSresult.DataBind()
        Me.PNLsearch.Visible = True
    End Sub
#End Region

    Private Sub BTNsearchWiki_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsearchWiki.Click
        IsLastPage = False
        PageIndex = 1
        ReloadData()
    End Sub
    Protected Sub IMG_indietro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMG_indietro.Click
        PageIndex = PageIndex - 1
        If PageIndex < 1 Then
            PageIndex = 1
        End If
        ReloadData()
    End Sub
    Protected Sub IMG_avanti_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMG_avanti.Click
        If IsLastPage = False Then
            PageIndex = PageIndex + 1
        End If
        ReloadData()
    End Sub

    Private Sub ReloadData()
        If (CBXcurrentCommunity.Checked = False) AndAlso (CBXpublicWiki.Checked) Then
            Me.CurrentPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearch)
        ElseIf (CBXcurrentCommunity.Checked AndAlso CBXpublicWiki.Checked) Then
            Me.CurrentPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComPub)
        ElseIf (CBXcurrentCommunity.Checked = True) AndAlso Not (CBXpublicWiki.Checked) Then
            Me.CurrentPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComunita)
        End If
    End Sub


    Private Sub setSkins()
        Dim HTMLStyleSkin As String
        Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath
        Dim Organization_Id As Integer = PageUtility.GetSkinIdOrganization
        Dim Community_Id As Integer = 0
        Try
            Community_Id = PageUtility.CurrentContext.UserContext.CurrentCommunityID()
        Catch ex As Exception

        End Try


        'Main CSS
        HTMLStyleSkin = ServiceSkinNew.GetCSSHtml( _
            Community_Id, _
            Organization_Id, _
            VirPath, _
            Me.SystemSettings.DefaultLanguage.Codice, _
            lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main, _
            Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf


        If (Request.Browser.Browser.Equals("IE")) Then
            HTMLStyleSkin &= ServiceSkinNew.GetCSSHtml( _
                Community_Id, _
                Organization_Id, _
                VirPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.IE, _
                Me.BaseUrl, SystemSettings.SkinSettings) & vbCrLf
        End If
        'End If

        Me.LTskins.Text = HTMLStyleSkin
    End Sub
    'Private Function GetSkinOrganizationId() As Integer
    '    Dim Organization_Id As Integer = 0
    '    If Not IsNothing(Me.PageUtility.CurrentContext) AndAlso Me.PageUtility.CurrentContext.UserContext.CurrentCommunityID > 0 Then
    '        Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
    '        'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
    '    Else
    '        'Non funziona nessuno dei due...
    '        'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
    '        Organization_Id = PageUtility.UserDefaultIdOrganization
    '    End If
    '    Return Organization_Id
    'End Function

    Private Sub DLSresult_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLSresult.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBwikiItem_t")
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBwikiSection_t")
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBwikiCommunity_t")
            Resource.setLabel(oLabel)
        End If

    End Sub
End Class