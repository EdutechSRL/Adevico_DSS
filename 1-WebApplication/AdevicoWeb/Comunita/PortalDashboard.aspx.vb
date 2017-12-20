Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.ActionDataContract

Partial Public Class PortalDashboard
    Inherits PageBase
    Implements IViewDashBoard


    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.BRheaderActive = False
        Me.UCdaySurvey.CurrentPresenter.Init(Me.PageUtility.CurrentContext)


    End Sub


#Region "Inherited"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.Master.ShowHeaderLanguageChanger = True
            Session("AdminForChange") = False
            Session("idComunita_forAdmin") = ""
            Session("CMNT_path_forAdmin") = ""
            Session("CMNT_path_forNews") = ""
            Session("CMNT_ID_forNews") = ""

            Session("IdCmntPadre") = 0
            Session("limbo") = True
            Session("ArrComunita") = Nothing
            Session("IdComunita") = 0
            Session("IdRuolo") = Nothing
            Session("ORGN_id") = Nothing
            Session("RLPC_id") = Nothing
            Session("TPCM_ID") = -1

            Me.CurrentPresenter.InitView(True)
            HYPmoreCommunities.NavigateUrl = "~/Comunita/EntrataComunita.aspx"

            Me.PageUtility.AddAction(Services_ElencaComunita.ActionType.ViewDashBoard, Nothing, InteractionType.Generic)

            If Not IsNothing(Me.UtenteCorrente) Then
                Dim oResult As dtoLogoutAccess = Me.ReadLogoutAccessCookie(Me.UtenteCorrente.ID, Me.UtenteCorrente.Login)
                If Not IsNothing(oResult) AndAlso oResult.PageUrl <> "" AndAlso oResult.isDownloading Then
                    Dim Script As String = "window.open('" & oResult.PageUrl & "');"
                    ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, Script, True)
                    Me.ClearLogoutAccessCookie()
                End If
            End If
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_ElencaComunita.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_DashBoard", "Comunita")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If
        With MyBase.Resource
            Master.ServiceTitle = .getValue("titolo")

            .setHyperLink(HYPmoreCommunities, True, True)
            .setHyperLink(HYPmoreNews, True, True)
            .setLiteral(LTlastTenCommunities)
            .setLiteral(LTlastNews)
            .setLiteral(LTlastNewsDescription)



        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As DashBoardPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _NoticeBoardService As Services_Bacheca
    Private _CommunitiesNoticeBoardPermission As IList(Of ModuleCommunityPermission(Of ModuleNoticeBoard))
    Private _BaseUrl As String
    Private _PagingUrl As String
#End Region

#Region "Base Context"
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As DashBoardPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DashBoardPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Private ReadOnly Property CurrentService() As Services_Bacheca
        Get
            If IsNothing(_NoticeBoardService) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _NoticeBoardService = Services_Bacheca.Create
                    With _NoticeBoardService
                        .Admin = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
                        .Read = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                        .GrantPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .Write = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)

                    End With
                ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _NoticeBoardService = New Services_Bacheca(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_Bacheca.Codex))
                Else
                    _NoticeBoardService = Me.PageUtility.GetCurrentServices.Find(Services_Bacheca.Codex)
                    If IsNothing(_NoticeBoardService) Then
                        _NoticeBoardService = Services_Bacheca.Create
                    End If
                End If
            End If
            Return _NoticeBoardService
        End Get
    End Property
    Public ReadOnly Property ModulePermission() As ModuleNoticeBoard Implements IViewDashBoard.NoticeBoardPermission
        Get
            Return TranslateComolPermissionToModulePermission(Me.CurrentService)
        End Get
    End Property
    Public ReadOnly Property Latex() As LatexSettings
        Get
            Return Me.SystemSettings.Latex
        End Get
    End Property
    Public ReadOnly Property TemplateCssLatexPath() As String
        Get
            If Me.Latex.CssStylePath = "" Then
                Return ""
            Else
                Return Me.BaseUrl & Me.Latex.CssStylePath
            End If
        End Get
    End Property
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            Return ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
        End Get
    End Property
    Public ReadOnly Property TemplateJsLatexPath() As String
        Get
            If Me.Latex.JavascriptPath = "" Then
                Return ""
            Else
                Return Me.BaseUrl & Me.Latex.JavascriptPath
            End If
        End Get
    End Property
    Public ReadOnly Property MessageTitoloBacheca() As String
        Get
            Return Me.Resource.getValue("TitoloBacheca")
        End Get
    End Property

    Private Function TranslateComolPermissionToModulePermission(ByVal oService As Services_Bacheca) As ModuleNoticeBoard
        Dim oModulePermission As New ModuleNoticeBoard
        With oService
            oModulePermission.DeleteMessage = .Admin OrElse .Write
            oModulePermission.EditMessage = .Admin OrElse .Write
            oModulePermission.ManagementPermission = .GrantPermission
            oModulePermission.PrintMessage = .Read OrElse .Write OrElse .Admin
            oModulePermission.RetrieveOldMessage = .Write OrElse .Admin
            oModulePermission.ServiceAdministration = .Admin OrElse .Write
            oModulePermission.ViewCurrentMessage = .Read OrElse .Write OrElse .Admin
            oModulePermission.ViewOldMessage = .Read OrElse .Write OrElse .Admin
        End With
        Return oModulePermission
    End Function
#End Region

#Region "Dashboard"
    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements IViewDashBoard.AddActionNoPermission

    End Sub

    Public Sub AddShowMessageAction(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer) Implements IViewDashBoard.AddShowMessageAction

    End Sub

    Public ReadOnly Property CommunitiesNoticeBoardPermission() As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.ModuleCommunityPermission(Of ModuleNoticeBoard)) Implements IViewDashBoard.CommunitiesNoticeBoardPermission
        Get
            If IsNothing(_CommunitiesNoticeBoardPermission) Then
                Dim oList As New List(Of ModuleCommunityPermission(Of ModuleNoticeBoard))
                Dim PermissionsList As IList(Of ServiceBase) = ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_Bacheca.Codex)

                For Each oPermission As ServiceBase In PermissionsList
                    oList.Add(New ModuleCommunityPermission(Of ModuleNoticeBoard)() With {.ID = oPermission.CommunityID, .Permissions = TranslateComolPermissionToModulePermission(New Services_Bacheca(oPermission.PermissionString))})
                Next
                _CommunitiesNoticeBoardPermission = oList
            End If
            Return _CommunitiesNoticeBoardPermission
        End Get
    End Property

    Public Property CurrentMessageID() As Long Implements IViewDashBoard.CurrentMessageID
        Get
            Dim MessageID As Long = 0
            Try
                MessageID = Me.ViewState("CurrentMessageID")
            Catch ex As Exception

            End Try
            Return MessageID
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentMessageID") = value
        End Set
    End Property
    Public Property MessageCommunityID() As Integer Implements IViewDashBoard.MessageCommunityID
        Get
            Dim CommunityID As Integer = 0
            Try
                MessageCommunityID = Me.ViewState("MessageCommunityID")
            Catch ex As Exception

            End Try
            Return CommunityID
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("MessageCommunityID") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedCommunityID() As Integer Implements IViewDashBoard.PreloadedCommunityID
        Get
            Dim CommunityID As Integer = -1
            Try
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    CommunityID = CInt(Me.Request.QueryString("CommunityID"))
                End If
            Catch ex As Exception

            End Try
            Return CommunityID
        End Get
    End Property
    Public Function GetMessageNavigationUrl(ByVal MessageID As Long, ByVal oContext As NoticeBoardContext, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType) As String Implements IViewDashBoard.GetMessageNavigationUrl

    End Function

    Public Sub NavigationUrl(ByVal oContext As NoticeBoardContext, ByVal oView As NoticeBoardContext.ViewModeType, ByVal oSmallView As NoticeBoardContext.SmallViewType) Implements IViewDashBoard.NavigationUrl

    End Sub

    Public Sub NoPermissionToAccess() Implements IViewDashBoard.NoPermissionToAccess

    End Sub

    Public Sub setHeaderTitle(ByVal CommunityName As String) Implements IViewDashBoard.setHeaderTitle

    End Sub

    Public Sub ViewMessage() Implements IViewDashBoard.ViewMessage
        CTRLnoticeboard.InitalizeControl(lm.Comol.Core.Dashboard.Domain.PlainLayout.box7box5, 0)
    End Sub

#End Region

    'Private Sub CTRLmessage_EditFromDashboard() Handles CTRLmessage.EditFromDashboard
    '    Me.RedirectToUrl("Generici/CommunityNoticeBoard.aspx?View=Message&SmallView=LastFourMessage&MessageID=" & Me.CurrentMessageID.ToString)
    'End Sub

    'Private Sub CTRLmessage_MoreMessages() Handles CTRLmessage.MoreMessages
    '    Me.RedirectToUrl("Generici/CommunityNoticeBoard.aspx?View=CurrentMessage&SmallView=LastFourMessage")
    'End Sub

#Region "Last Communities"
    Public Sub LoadLastSubscription(ByVal oList As List(Of dtoSubscription)) Implements IViewDashBoard.LoadLastSubscription
        Me.RPTfirstTenCommunities.Visible = oList.Count > 0
        If Me.RPTfirstTenCommunities.Visible Then
            Me.RPTfirstTenCommunities.DataSource = oList
            Me.RPTfirstTenCommunities.DataBind()
        End If
    End Sub

    Private Sub RPTfirstTenCommunities_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTfirstTenCommunities.ItemCommand
        If IsNumeric(e.CommandArgument) Then
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
            'GenericCacheManager.PurgeCacheItems(CachePolicy.PermessiServizioUtente())
            Me.PageUtility.AccessToCommunity(Me.CurrentContext.UserContext.CurrentUserID, e.CommandArgument, oResourceConfig, True)
        End If
    End Sub
    Private Sub RPTfirstTenCommunities_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfirstTenCommunities.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oName As Literal = e.Item.FindControl("LTname")
            If Not IsNothing(oName) Then
                Me.Resource.setLiteral(oName)
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oLink As HyperLink = e.Item.FindControl("HYPallCommunities")
            Me.Resource.setHyperLink(oLink, True, True)
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim oSubscription As dtoSubscription = e.Item.DataItem
            Dim oLink As LinkButton = e.Item.FindControl("LNBlogin")
            Dim oName As Literal = e.Item.FindControl("LTcommunityName")
            oName.Visible = Not oSubscription.Enabled
            oLink.Visible = oSubscription.Enabled

            If oLink.Visible Then
                oLink.CommandArgument = oSubscription.CommunityID
                oLink.Text = oSubscription.CommunityName

                'If oSubscription.Enabled Then
                '    If e.Item.ItemType = ListItemType.AlternatingItem Then
                '        oLink.CssClass = "ROW_ItemLink_Small"
                '    Else
                '        oLink.CssClass = "ROW_ItemLink_Small"
                '    End If
                'Else
                '    oLink.CssClass = "ROW_ItemLinkDisattivate_Small"
                'End If
            End If
            'Dim oLogo As Literal = e.Item.FindControl("LTlogo")
            'oLogo.Text = "<img src='" & Me.BaseUrl & oSubscription.CommunityLogo & "' alt='' border=0>"

            Dim oNews As Literal = e.Item.FindControl("LThasnews")
            oNews.Visible = oSubscription.HasNews
            If oSubscription.HasNews Then
                If oSubscription.NewsInfo.Count = 0 Then
                    oNews.Text = Me.Resource.getValue("LThasnews.0")
                Else
                    oNews.Text = String.Format(Me.Resource.getValue("LThasnews.n"), oSubscription.NewsInfo.Count.ToString)
                End If
                oLink.ToolTip = oSubscription.CommunityName & " " & oNews.Text

                Dim Url As String = "<a href=""{0}"" Title=""{1}"" Alt=""{1}"" class=""ROW_ItemLink_Small"">- <b>NEWS</b></a>"
                Dim FromDay As DateTime = DateTime.MinValue
                If oSubscription.LastAccessOn.HasValue Then
                    FromDay = oSubscription.LastAccessOn.Value
                Else
                    FromDay = Now.Date.AddDays(-30)
                End If

                oNews.Text = String.Format(Url, Me.BaseUrl & "Notification/CommunityNews.aspx?FromDay=" & Me.PageUtility.GetUrlEncoded(FromDay.ToString) & "&PageSize=25&Page=0&CommunityID=" & oSubscription.CommunityID & "&PR_View=" & lm.Modules.NotificationSystem.Domain.ViewModeType.FromDashBoard.ToString, oLink.ToolTip)
            End If
        End If
    End Sub
#End Region

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_ElencaComunita.Codex)
    End Sub
End Class