Imports lm.Modules.NotificationSystem.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract


Partial Public Class CommunityNews
    Inherits PageBase
    Implements IViewCommunityDayNews


#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As CommunityDayNewsPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_CommunityNews
    Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleCommunityNews))


    Private _BaseUrl As String
#End Region

#Region "View"
    Private ReadOnly Property CurrentService() As Services_CommunityNews
        Get
            If IsNothing(_Servizio) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _Servizio = Services_CommunityNews.Create
                    _Servizio.ViewOtherNews = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
                    _Servizio.DeleteOtherNews = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                    _Servizio.ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                    _Servizio.ViewMyNews = True
                    _Servizio.DeleteMyNews = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_CommunityNews(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_CommunityNews.Codex))
                Else
                    _Servizio = Me.PageUtility.GetCurrentServices.Find(Services_OnLineUsers.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = Services_CommunityNews.Create
                    End If
                End If
            End If
            Return _Servizio
        End Get
    End Property
    Public ReadOnly Property ModulePermission() As ModuleCommunityNews Implements IViewCommunityDayNews.ModulePermission
        Get
            Return New ModuleCommunityNews(Me.CurrentService)
        End Get
    End Property

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleCommunityNews)) Implements IViewCommunityDayNews.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                Dim oList As New List(Of ModuleCommunityPermission(Of ModuleCommunityNews))
                Dim PermissionsList As IList(Of ServiceBase) = ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_CommunityNews.Codex)

                For Each oPermission As ServiceBase In PermissionsList
                    oList.Add(New ModuleCommunityPermission(Of ModuleCommunityNews)() With {.ID = oPermission.CommunityID, .Permissions = New ModuleCommunityNews(New Services_CommunityNews(oPermission.PermissionString))})
                Next
                _CommunitiesPermission = oList
            End If
            Return _CommunitiesPermission
        End Get
    End Property

    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewCommunityDayNews.Pager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DIVpageSize.Style.Add("display", IIf(Me.PGgrid.Visible, "block", "none"))
        End Set
    End Property
    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IViewCommunityDayNews.PreLoadedPageSize
        Get
            Dim PageSize As Integer = Me.DDLpage.Items(0).Value
            Try
                PageSize = Request.QueryString("PageSize")
            Catch ex As Exception

            End Try
            If IsNothing(Request.QueryString("PageSize")) Then
                Return DDLpage.SelectedValue
            Else
                If IsNothing(DDLpage.Items.FindByValue(PageSize)) Then
                    Return DDLpage.SelectedValue
                Else
                    Return PageSize
                End If
            End If
        End Get
    End Property
    'Public ReadOnly Property PreLoadedView() As ViewModeType Implements IViewCommunityDayNews.PreLoadedView
    '    Get
    '        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.CurrentCommunity)
    '    End Get
    'End Property

    'Public ReadOnly Property PreLoadedDayMode() As DayModeType Implements IViewCommunityDayNews.PreLoadedDayMode
    '    Get
    '        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DayModeType).GetByString(Request.QueryString("DayMode"), DayModeType.TodayYesterday)
    '    End Get
    'End Property
    Public ReadOnly Property PreLoadedFromDay() As Date Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedFromDay
        Get
            Dim DayToDisplay As Date = DateTime.MinValue
            Try
                DayToDisplay = Request.QueryString("FromDay")
            Catch ex As Exception

            End Try
            Return DayToDisplay
        End Get
    End Property
    Public ReadOnly Property PreLoadedDay() As Date Implements IViewCommunityDayNews.PreLoadedDay
        Get
            Dim DayToDisplay As Date = Now.Date
            Try
                DayToDisplay = Request.QueryString("Day")
            Catch ex As Exception

            End Try
            Return DayToDisplay
        End Get
    End Property

    Public ReadOnly Property PreLoadedDaySpecifyed() As Boolean Implements IViewCommunityDayNews.PreLoadedDaySpecifyed
        Get
            Return Not String.IsNullOrEmpty(Request.QueryString("Day"))
        End Get
    End Property
    Public Property NewsContext() As DayNewsContext Implements IViewCommunityDayNews.NewsContext
        Get
            Dim oContext As New DayNewsContext
            If TypeOf Me.ViewState("NewsContext") Is DayNewsContext Then
                oContext = Me.ViewState("NewsContext")
            Else
                oContext.CommunityID = Me.PreLoadedCommunityID
                oContext.CurrentDay = Me.PreLoadedDay
                oContext.FromDay = Me.PreLoadedFromDay
                oContext.PreviousCommunityName = Me.PreLoadedPreviousCommunityName
                oContext.PreviousDay = Me.PreLoadedPreviousDay
                oContext.PreviousDayView = Me.PreLoadedPreviousDayView
                oContext.PreviousFromView = Me.PreLoadedPreviousFromView
                oContext.PreviousView = Me.PreLoadedPreviousView
                oContext.PreviousPageIndex = Me.PreLoadedPreviousPageIndex
                oContext.PreviousPageSize = Me.PreLoadedPreviousPageSize
                oContext.PreviousUserID = Me.PreLoadedPreviousUserID
                oContext.PreviousCommunityID = PreviousCommunityID
            End If
            Return oContext
        End Get
        Set(ByVal value As DayNewsContext)
            Me.ViewState("NewsContext") = value
        End Set
    End Property
    Public WriteOnly Property SetPreviousURL() As String Implements IViewCommunityDayNews.SetPreviousURL
        Set(ByVal value As String)
            If value = "" Then
                Me.HYPbackHistory.Visible = False
            Else
                Me.HYPbackHistory.Visible = True
                Me.HYPbackHistory.NavigateUrl = value
            End If
        End Set
    End Property
    Public ReadOnly Property CurrentPageIndex() As Integer Implements IViewCommunityDayNews.CurrentPageIndex
        Get
            If Me.Request.QueryString("Page") Is Nothing Then
                Return 0
            Else
                Try
                    Return CInt(Me.Request.QueryString("Page"))
                Catch ex As Exception
                    Return 0
                End Try
            End If
        End Get
    End Property
    Public Property CurrentPageSize() As Integer Implements IViewCommunityDayNews.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property
    Public ReadOnly Property PreLoadedCommunityID() As Integer Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedCommunityID
        Get
            Dim CommunityID As Integer = 0
            Try
                CommunityID = Me.Request.QueryString("CommunityID")
            Catch ex As Exception

            End Try

            Return CommunityID
        End Get
    End Property

    Public ReadOnly Property PreviousCommunityID() As Integer Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreviousCommunityID
        Get
            Dim CommunityID As Integer = 0
            Try
                CommunityID = Me.Request.QueryString("PR_CommunityID")
            Catch ex As Exception

            End Try

            Return CommunityID
        End Get
    End Property

    Public ReadOnly Property PreLoadedPreviousUserID() As Integer Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedPreviousUserID
        Get
            Dim UserID As Integer
            Try
                UserID = Request.QueryString("PR_UserID")
            Catch ex As Exception

            End Try
            Return UserID
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousDay() As Date Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedPreviousDay
        Get
            Dim DayToDisplay As Date = Now.Date
            Try
                DayToDisplay = Request.QueryString("PR_Day")
            Catch ex As Exception

            End Try
            Return DayToDisplay
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousView() As ViewModeType Implements IViewCommunityDayNews.PreLoadedPreviousView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("PR_View"), ViewModeType.None)
        End Get
    End Property

    Public ReadOnly Property PreLoadedPreviousFromView() As lm.Modules.NotificationSystem.Domain.ViewModeType Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedPreviousFromView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("PR_FromView"), ViewModeType.None)
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousCommunityName() As String Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedPreviousCommunityName
        Get
            Return Request.QueryString("PR_FindByName")
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousDayView() As lm.Modules.NotificationSystem.Domain.DayModeType Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedPreviousDayView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DayModeType).GetByString(Request.QueryString("PR_DayMode"), DayModeType.TodayYesterday)
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousPageIndex() As Integer Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedPreviousPageIndex
        Get
            If Me.Request.QueryString("PR_Page") Is Nothing Then
                Return 0
            Else
                Try
                    Return CInt(Me.Request.QueryString("PR_Page"))
                Catch ex As Exception
                    Return 0
                End Try
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousPageSize() As Integer Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.PreLoadedPreviousPageSize
        Get
            Dim PageSize As Integer = Me.DDLpage.Items(0).Value
            Try
                PageSize = Request.QueryString("PR_PageSize")
            Catch ex As Exception

            End Try
            If IsNothing(Request.QueryString("PR_PageSize")) Then
                Return DDLpage.SelectedValue
            Else
                If IsNothing(DDLpage.Items.FindByValue(PageSize)) Then
                    Return DDLpage.SelectedValue
                Else
                    Return PageSize
                End If
            End If
        End Get
    End Property
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
    Public ReadOnly Property CurrentPresenter() As CommunityDayNewsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunityDayNewsPresenter(Me.CurrentContext, Me)
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
#End Region

#Region "Defaults"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
    End Sub

#Region "Default sub"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityLastUpdates", "Notification")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("titolo.portale")
            .setLiteral(Me.LTnonews)
            .setHyperLink(HYPbackAllNewsRead, True, True)
            .setHyperLink(HYPbackHistory, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Function GetNavigationUrl(ByVal oContext As DayNewsContext, ByVal WithBaseUrl As Boolean) As String Implements IViewCommunityDayNews.GetNavigationUrl
        Return GetBackNavigationUrl(oContext, WithBaseUrl)
    End Function

    Public Sub NavigationUrl(ByVal oContext As DayNewsContext) Implements IViewCommunityDayNews.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseNewsUrl(oContext) & "&Page={0}"
    End Sub

    Private Function GetBaseNavigationUrl(ByVal oContext As DayNewsContext, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = GetBaseNewsUrl(oContext, WithBaseUrl)
        If url.Contains("?") Then
            If oContext.PageIndex = -1 Then
                url &= "&Page={0}"
            ElseIf oContext.PageIndex >= 0 Then
                url &= "&Page=" & oContext.PageIndex.ToString
            End If
        End If
        Return url
    End Function
    Private Function GetBaseNewsUrl(ByVal oContext As DayNewsContext, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = "?"
        If oContext.UserID > 0 Then
            url &= "&UserID=" & oContext.UserID.ToString
        End If
        If oContext.CommunityID > 0 Then
            url &= "&CommunityID=" & oContext.CommunityID
        End If
        If oContext.CurrentDay.Equals(New Date) = False Then
            url &= "&Day=" & oContext.CurrentDay.ToShortDateString
        End If
        If oContext.FromDay.Equals(New Date) = False AndAlso Not oContext.FromDay.Equals(DateTime.MinValue) Then
            url &= "&FromDay=" & PageUtility.GetUrlEncoded(oContext.FromDay.ToString)
        End If
        url &= "&PageSize=" & Me.CurrentPageSize.ToString

        If oContext.PreviousCommunityName <> "" Then
            url &= "&PR_FindByName=" & oContext.PreviousCommunityName
        End If
        If oContext.PreviousDay.Equals(New Date) = False Then
            url &= "&PR_Day=" & oContext.PreviousDay.ToShortDateString
        End If

        If oContext.PreviousDayView <> DayModeType.None Then
            url &= "&PR_DayMode=" & oContext.PreviousDayView.ToString
        End If

        If oContext.PreviousFromView <> ViewModeType.None Then
            url &= "&PR_FromView=" & oContext.PreviousFromView.ToString
        End If
        If oContext.PreviousPageIndex > 0 Then
            url &= "&PR_Page=" & oContext.PreviousPageIndex.ToString
        End If
        If oContext.PreviousPageSize > 0 Then
            url &= "&PR_PageSize=" & oContext.PreviousPageSize.ToString
        End If
        If oContext.PreviousView <> ViewModeType.None Then
            url &= "&PR_View=" & oContext.PreviousView.ToString
        End If
        If oContext.PreviousCommunityID > 0 Then
            url &= "&PR_CommunityID=" & oContext.PreviousCommunityID.ToString
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If

        Select Case oContext.PreviousView
            Case ViewModeType.CurrentCommunity
                url = "Notification/CommunityLastUpdates.aspx" & url
            Case ViewModeType.Portal
                url = "Notification/CommunitiesLastUpdates.aspx" & url
            Case ViewModeType.SelectedCommunity
                WithBaseUrl = False
                url = Me.Request.Url.AbsolutePath & url

            Case ViewModeType.FromMyCommunityList
                url = "Comunita/EntrataComunita.aspx"
            Case ViewModeType.FromTreeView
                url = "Comunita/NavigazioneTreeView.aspx"
            Case ViewModeType.FromDashBoard
                url = SystemSettings.Presenter.DefaultLogonPage
            Case ViewModeType.FromFindCommunity
                url = "Comunita/FindCommunity.aspx?re_set=true"
            Case ViewModeType.FromCommunitiesAccess
                url = "Comunita/EntrataComunita.aspx?re_set=true"
            Case ViewModeType.FromManagementCommunities
                url = SystemSettings.Presenter.DefaultManagement & "?re_set=true"
            Case ViewModeType.FromCommunityTree
                url = "Comunita/NavigazioneTreeView.aspx?re_set=true"

            Case ViewModeType.None
                WithBaseUrl = False
                url = Me.Request.Url.AbsolutePath & url
        End Select
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function

    Private Function GetBackNavigationUrl(ByVal oContext As DayNewsContext, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = "?"

        If oContext.PreviousUserID > 0 Then
            url &= "&UserID=" & oContext.PreviousUserID.ToString
        End If
        If oContext.PreviousCommunityID > 0 Then
            url &= "&CommunityID=" & oContext.PreviousCommunityID
        End If
        If oContext.PreviousFromView <> ViewModeType.None Then
            url &= "&FromView=" & oContext.PreviousFromView.ToString
        End If
        url &= "&FilterValue=" & oContext.PreviousCommunityName
        url &= "&PageSize=" & oContext.PreviousPageSize.ToString
        url &= "&Page=" & oContext.PreviousPageIndex.ToString
        If oContext.PreviousDay.Equals(New Date) = False Then
            url &= "&Day=" & oContext.PreviousDay.ToShortDateString
        End If
        If oContext.FromDay.Equals(New Date) = False AndAlso Not oContext.FromDay.Equals(DateTime.MinValue) Then
            url &= "&FromDay=" & PageUtility.GetUrlEncoded(oContext.FromDay.ToString)
        End If
        If oContext.PreviousDayView <> DayModeType.None Then
            url &= "&DayMode=" & oContext.PreviousDayView.ToString
        End If
        If oContext.PreviousView <> ViewModeType.None Then
            url &= "&View=" & oContext.PreviousView.ToString
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If

        Select Case oContext.PreviousView
            Case ViewModeType.CurrentCommunity
                url = "Notification/CommunityLastUpdates.aspx" & url
            Case ViewModeType.Portal
                url = "Notification/CommunitiesLastUpdates.aspx" & url
            Case ViewModeType.SelectedCommunity
                WithBaseUrl = False
                url = Me.Request.Url.AbsolutePath & url

            Case ViewModeType.FromMyCommunityList
                url = "Comunita/EntrataComunita.aspx"
            Case ViewModeType.FromTreeView
                url = "Comunita/NavigazioneTreeView.aspx"
            Case ViewModeType.FromDashBoard
                url = SystemSettings.Presenter.DefaultLogonPage
            Case ViewModeType.FromFindCommunity
                url = "Comunita/FindCommunity.aspx?re_set=true"
            Case ViewModeType.FromManagementCommunities
                url = SystemSettings.Presenter.DefaultManagement & "?re_set=true"
            Case ViewModeType.FromCommunitiesAccess
                url = "Comunita/EntrataComunita.aspx?re_set=true"
            Case ViewModeType.FromCommunityTree
                url = "Comunita/NavigazioneTreeView.aspx?re_set=true"
            Case ViewModeType.FromDashboardList
                Dim settings As lm.Comol.Core.Dashboard.Domain.UserCurrentSettings = GetCurrentCookie()
                If IsNothing(settings) Then
                    settings = New lm.Comol.Core.Dashboard.Domain.UserCurrentSettings
                    settings.GroupBy = lm.Comol.Core.Dashboard.Domain.GroupItemsBy.None
                    settings.OrderBy = lm.Comol.Core.Dashboard.Domain.OrderItemsBy.LastAccess
                    settings.Ascending = False
                    settings.View = lm.Comol.Core.Dashboard.Domain.DashboardViewType.List
                End If
                url = lm.Comol.Core.Dashboard.Domain.RootObject.LoadPortalView(PageUtility.CurrentContext.UserContext.CurrentUserID, settings)
            Case ViewModeType.FromDashboardCombined
                Dim settings As lm.Comol.Core.Dashboard.Domain.UserCurrentSettings = GetCurrentCookie()
                If IsNothing(settings) Then
                    settings = New lm.Comol.Core.Dashboard.Domain.UserCurrentSettings
                    settings.GroupBy = lm.Comol.Core.Dashboard.Domain.GroupItemsBy.Tile
                    settings.OrderBy = lm.Comol.Core.Dashboard.Domain.OrderItemsBy.LastAccess
                    settings.Ascending = False
                    settings.View = lm.Comol.Core.Dashboard.Domain.DashboardViewType.Combined
                End If
                url = lm.Comol.Core.Dashboard.Domain.RootObject.LoadPortalView(PageUtility.CurrentContext.UserContext.CurrentUserID, settings)
            Case ViewModeType.FromDashboardTree
                url = lm.Comol.Core.Dashboard.Domain.RootObject.ViewTree(False, "", 0)
            Case ViewModeType.FromDashboardTreeAdvanced
                url = lm.Comol.Core.Dashboard.Domain.RootObject.ViewTree(True, "", 0)
            Case ViewModeType.None
                WithBaseUrl = False
                url = Me.Request.Url.AbsolutePath & url
        End Select
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function
    Private ReadOnly Property CookieName As String
        Get
            Return "DashboardSettings"
        End Get
    End Property
    Private Function GetCurrentCookie() As lm.Comol.Core.Dashboard.Domain.UserCurrentSettings 'Implements IViewBaseDashboardLoader.GetCurrentCookie
        Dim myCookie As HttpCookie = Request.Cookies(CookieName)
        If IsNothing(myCookie) Then
            Return Nothing
        Else
            Dim result As New lm.Comol.Core.Dashboard.Domain.UserCurrentSettings
            result.IdSelectedTile = CLng(myCookie("IdSelectedTile"))
            result.IdSelectedTag = CLng(myCookie("IdSelectedTag"))
            result.AfterUserLogon = CInt(myCookie("AfterUserLogon"))
            result.GroupBy = CInt(myCookie("GroupBy"))
            result.DefaultNoticeboard = CInt(myCookie("Noticeboard"))
            result.TileNoticeboard = CInt(myCookie("TileNoticeboard"))
            result.CombinedNoticeboard = CInt(myCookie("CombineNoticeboard"))
            result.ListNoticeboard = CInt(myCookie("ListNoticeboard"))
            result.OrderBy = CInt(myCookie("OrderBy"))
            result.View = CInt(myCookie("View"))
            Return result
        End If
    End Function

    Public Sub NoPermissionToAccess() Implements IViewCommunityDayNews.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub

    Private Sub RPTnewsData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTnewsData.ItemCommand
        If e.CommandName = "enter" Then
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
            Me.PageUtility.AccessToCommunity(Me.CurrentContext.UserContext.CurrentUserID, e.CommandArgument, oResourceConfig, True)
        End If
    End Sub

    Private Sub RPTnewsData_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnewsData.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim oLink As LinkButton = e.Item.FindControl("LNBentra")
            Dim oName As Literal = e.Item.FindControl("LTmoduleName")
            Dim oDto As dtoModuleNews = DirectCast(e.Item.DataItem, dtoModuleNews)

            oLink.Visible = Not String.IsNullOrEmpty(oDto.ModuleDefaultUrl)
            oName.Visible = String.IsNullOrEmpty(oDto.ModuleDefaultUrl)
        End If
    End Sub
    Public Sub LoadNotifications(ByVal DayToShow As String, ByVal oList As System.Collections.Generic.List(Of dtoModuleNews)) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.LoadNotifications
        Me.LTday.Text = Me.NewsContext.CurrentDay.ToString("D", Resource.CultureInfo)

        If oList.Count = 0 Then
            Resource.setLiteral(Me.LTnonews)
            Me.LTnonews.Visible = True
            Me.PGgrid.Visible = False
            Me.RPTnewsData.Visible = False
        Else
            Me.RPTnewsData.Visible = True
            Me.LTnonews.Visible = False
            Me.RPTnewsData.DataSource = oList
            Me.RPTnewsData.DataBind()
        End If
    End Sub

    Public WriteOnly Property CommunityName() As String Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.CommunityName
        Set(ByVal value As String)
            If value = "" Then
                Me.LTcommunityName.Text = Me.Resource.getValue("Portale")
            Else
                Me.LTcommunityName.Text = value
            End If
        End Set
    End Property

    Public Sub LoadAllNews(ByVal oList As System.Collections.Generic.List(Of lm.Modules.NotificationSystem.Presentation.dtoMultipleNews)) Implements IViewCommunityDayNews.LoadAllNews
        If oList.Count = 0 Then
            Me.LTnonews.Text = Resource.getValue("NoAllNews")
            Me.LTnonews.Visible = True
            Me.PGgrid.Visible = False
            Me.RPTallNews.Visible = False
        Else
            'If oList(0).ID = 0 Then
            '    oList(0).Name = Me.Resource.getValue("Portale")
            'End If
            Me.RPTallNews.Visible = True
            Me.LTnonews.Visible = False
            Me.RPTallNews.DataSource = oList
            Me.RPTallNews.DataBind()
        End If
    End Sub
    Public Sub ShowNoNewsFrom(ByVal oDate As DateTime, ByVal CommunityName As String) Implements IViewCommunityDayNews.ShowNoNewsFrom
        Me.LTnonews.Visible = True
        Me.LTnonews.Text = String.Format(Resource.getValue("NoNewsFrom"), FormatDateTime(oDate, DateFormat.ShortDate), FormatDateTime(oDate, DateFormat.ShortTime), CommunityName)
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_CommunityNews.Codex)
    End Sub

    Public Sub AddAction(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oActionType As COL_BusinessLogic_v2.UCServices.Services_CommunityNews.ActionType, ByVal oDay As Date) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.AddAction
        PageUtility.AddAction(CommunityID, oActionType, CreateObjectToAction(System.Guid.Empty, oDay), InteractionType.SystemToUser)
    End Sub

    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oType As lm.Modules.NotificationSystem.Domain.DayModeType, ByVal oDay As Date) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.AddActionNoPermission
        PageUtility.AddAction(CommunityID, Services_CommunityNews.ActionType.NoPermission, CreateObjectToAction(System.Guid.Empty, oDay), InteractionType.SystemToUser)
    End Sub

    Public Sub AddActionViewNews(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal NewsID As System.Guid) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.AddActionViewNews

    End Sub

    Private Function CreateObjectToAction(ByVal NewsID As System.Guid, ByVal oData As Date) As List(Of PresentationLayer.WS_Actions.ObjectAction)
        Dim oList As New List(Of PresentationLayer.WS_Actions.ObjectAction)
        If NewsID <> System.Guid.Empty Then
            oList.Add(New PresentationLayer.WS_Actions.ObjectAction() With {.ModuleID = Me.CurrentModuleID, .ObjectTypeId = Services_CommunityNews.ObjectType.News, .ValueID = NewsID.ToString})
        End If
        If oData.Equals(New Date) = False Then
            oList.Add(New PresentationLayer.WS_Actions.ObjectAction() With {.ModuleID = Me.CurrentModuleID, .ObjectTypeId = Services_CommunityNews.ObjectType.Day, .ValueID = oData.ToString})
        End If
        Return oList
    End Function

    Public ReadOnly Property PortalPermission() As lm.Modules.NotificationSystem.Domain.ModuleCommunityNews Implements IViewCommunityDayNews.PortalPermission
        Get
            Dim PersonTypeID As Integer = Me.TipoPersonaID
            Dim oService As Services_CommunityNews = Services_CommunityNews.Create
            oService.ViewOtherNews = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
            oService.DeleteOtherNews = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
            oService.ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
            oService.ViewMyNews = True
            oService.DeleteMyNews = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
            Return New ModuleCommunityNews(oService)
        End Get
    End Property

    Sub SetAllNewsRead(ByVal CommunityID As Integer, ByVal url As String) Implements IViewCommunityDayNews.SetAllNewsRead
        If url = "" Then
            Me.HYPbackAllNewsRead.Visible = False
        Else
            Me.HYPbackAllNewsRead.Visible = True
            Me.HYPbackAllNewsRead.NavigateUrl = ServiceLoaderPage(CommunityID, url)
        End If
    End Sub

    Public Function ServiceLoaderPage(ByVal CommunityID As Integer, ByVal DestinationUrl As String) As String
        Return Me.PageUtility.EncryptedUrl("Notification/CommunityNewsRead.aspx", "CommunityID=" & CommunityID.ToString & "&DestinationUrl=" & PageUtility.GetUrlEncoded(DestinationUrl), 5)
    End Function

    Public Sub SetNoNewsForComunity(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal UpdateDate As DateTime) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityDayNews.SetNoNewsForComunity
        Me.PageUtility.SendNotificationUpdateCommunityAccess(PersonID, CommunityID, UpdateDate)
    End Sub
End Class