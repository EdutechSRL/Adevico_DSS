Imports lm.Modules.NotificationSystem.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract

Partial Public Class CommunitiesLastUpdates
    Inherits PageBase
    Implements IViewCommunityNews

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As CommunityNewsPresenter
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
    Public ReadOnly Property ModulePermission() As ModuleCommunityNews Implements IViewCommunityNews.ModulePermission
        Get
            Return New ModuleCommunityNews(Me.CurrentService)
        End Get
    End Property

    Public ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleCommunityNews)) Implements IViewCommunityNews.CommunitiesPermission
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
    Public ReadOnly Property PortalPermission() As lm.Modules.NotificationSystem.Domain.ModuleCommunityNews Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.PortalPermission
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

    Public ReadOnly Property PreLoadedDaySpecifyed() As Boolean Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.PreLoadedDaySpecifyed
        Get
            Return Not String.IsNullOrEmpty(Request.QueryString("Day"))
        End Get
    End Property
    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements IViewCommunityNews.Pager
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
    Public ReadOnly Property PreLoadedCommunityName() As String Implements IViewCommunityNews.PreLoadedCommunityName
        Get
            Return Request.QueryString("FindByName")
        End Get
    End Property
    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IViewCommunityNews.PreLoadedPageSize
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
    Public ReadOnly Property PreLoadedView() As ViewModeType Implements IViewCommunityNews.PreLoadedView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.Portal)
        End Get
    End Property
    Public ReadOnly Property PreLoadedDay() As Date Implements IViewCommunityNews.PreLoadedDay
        Get
            Dim DayToDisplay As Date = Now.Date
            Try
                DayToDisplay = Request.QueryString("Day")
            Catch ex As Exception

            End Try
            Return DayToDisplay
        End Get
    End Property
    Public ReadOnly Property PreLoadedDayMode() As DayModeType Implements IViewCommunityNews.PreLoadedDayMode
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DayModeType).GetByString(Request.QueryString("DayMode"), DayModeType.TodayYesterday)
        End Get
    End Property
    Public ReadOnly Property PreLoadedPreviousView() As ViewModeType Implements IViewCommunityNews.PreLoadedPreviousView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeType).GetByString(Request.QueryString("View"), ViewModeType.None)
        End Get
    End Property
    Public Property NewsContext() As CommunityNewsContext Implements IViewCommunityNews.NewsContext
        Get
            Dim oContext As New CommunityNewsContext
            If TypeOf Me.ViewState("NewsContext") Is CommunityNewsContext Then
                oContext = Me.ViewState("NewsContext")
            Else
                oContext.CommunityName = PreLoadedCommunityName
                oContext.CurrentDay = PreLoadedDay
                oContext.CurrentView = ViewModeType.Portal
                oContext.DayView = PreLoadedDayMode
                oContext.PageIndex = 0
                oContext.FromView = Me.PreLoadedPreviousView
                If IsNumeric(Me.Request.QueryString("UserID")) Then
                    Try
                        oContext.UserID = Me.Request.QueryString("UserID")
                    Catch ex As Exception

                    End Try
                End If
                oContext.CommunityID = -1
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("CommunityID")
                    Catch ex As Exception

                    End Try
                End If
                Me.ViewState("NewsContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As CommunityNewsContext)
            Me.ViewState("NewsContext") = value
        End Set
    End Property
    Public WriteOnly Property SetPreviousURL() As String Implements IViewCommunityNews.SetPreviousURL
        Set(ByVal value As String)
            If value = "" Then
                Me.HYPbackHistory.Visible = False
            Else
                Me.HYPbackHistory.Visible = True
                Me.HYPbackHistory.NavigateUrl = value
            End If
        End Set
    End Property
    Public Property CommunityNameField() As String Implements IViewCommunityNews.CommunityNameField
        Get
            If String.IsNullOrEmpty(Me.TXBsearch.Text) Then
                Return ""
            Else
                Return Replace(Trim(Me.TXBsearch.Text), "&", " ")
            End If
        End Get
        Set(ByVal value As String)
            Me.TXBsearch.Text = value
        End Set
    End Property

    Public ReadOnly Property CurrentPageIndex() As Integer Implements IViewCommunityNews.CurrentPageIndex
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
    Public Property CurrentPageSize() As Integer Implements IViewCommunityNews.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property
    Public ReadOnly Property CurrentView() As lm.Modules.NotificationSystem.Domain.ViewModeType Implements IViewCommunityNews.CurrentView
        Get
            Return ViewModeType.Portal
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
    Public ReadOnly Property CurrentPresenter() As CommunityNewsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunityNewsPresenter(Me.CurrentContext, Me)
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
            If String.IsNullOrEmpty(Me.Request.Browser.Type) OrElse Me.Request.Browser.Type.ToLower = "ie" Then
                Me.DVnoIE.Visible = False
            Else
                Me.DVnoIE.Visible = True
            End If
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

            .setRadioButtonList(Me.RBLview, 1)
            .setRadioButtonList(Me.RBLview, 2)
            .setHyperLink(HYPbackHistory, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Function GetNavigationUrl(ByVal oContext As CommunityNewsContext) As String Implements IViewCommunityNews.GetNavigationUrl
        Return GetBaseNavigationUrl(oContext)
    End Function

    Public Sub NavigationUrl(ByVal oContext As CommunityNewsContext) Implements IViewCommunityNews.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseNewsUrl(oContext) & "&Page={0}"
        If Me.NewsContext.DayView = DayModeType.AllNews Then
            Try
                Me.PGgrid.BaseNavigateUrl &= "&ModuleID=" & Me.CurrentModuleID & "&ModeFiler=" & Me.CurrentViewFilter
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Function GetBaseNavigationUrl(ByVal oContext As CommunityNewsContext, Optional ByVal WithBaseUrl As Boolean = True) As String
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
    Private Function GetBaseNewsUrl(ByVal oContext As CommunityNewsContext, Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = "?"
        If oContext.UserID > 0 Then
            url &= "&UserID=" & oContext.UserID.ToString
        End If
        If oContext.CommunityID > 0 Then
            url &= "&CommunityID=" & oContext.CommunityID
        End If
        If oContext.FromView <> ViewModeType.None Then
            url &= "&FromView=" & oContext.FromView.ToString
        End If
        url &= "&FilterValue=" & oContext.CommunityName
        url &= "&PageSize=" & Me.CurrentPageSize.ToString
        If oContext.CurrentDay.Equals(New Date) = False Then
            url &= "&Day=" & oContext.CurrentDay.ToShortDateString
        End If
        If oContext.DayView <> DayModeType.None Then
            url &= "&DayMode=" & oContext.DayView.ToString
        End If
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If

        Select Case oContext.CurrentView
            Case ViewModeType.CurrentCommunity
                url = "Notification/CommunityLastUpdates.aspx" & url
            Case ViewModeType.Portal
                url = "Notification/CommunitiesLastUpdates.aspx" & url
            Case ViewModeType.FromDashBoard
                url = SystemSettings.Presenter.DefaultLogonPage & url
            Case ViewModeType.None
                url = Me.Request.Url.AbsolutePath & url
        End Select
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function

    Public Sub NoPermissionToAccess() Implements IViewCommunityNews.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub

    Public Sub LoadDays(ByVal oTabsList As List(Of dtoTab)) Implements IViewCommunityNews.LoadDays
        Dim Tabs As String = ""
        Dim Quote As String = """"
        If oTabsList.Count > 0 Then
            If oTabsList(0).TypeTab = DayModeType.LastMonth Then
                Dim MonthDays As List(Of Integer) = (From o In oTabsList Where o.isType Select o.Month).ToList
                If MonthDays.Count = 2 Then
                    Tabs = Me.CreateTabDays((From o In oTabsList Where o.Month = MonthDays(0) Select o).ToList)
                    Tabs &= "<br class=" & Quote & "clear" & Quote & ">"
                    Tabs &= Me.CreateTabDays((From o In oTabsList Where o.Month = MonthDays(1) Select o).ToList)
                Else
                    Tabs = Me.CreateTabDays(oTabsList)
                End If
            Else
                Tabs = Me.CreateTabDays(oTabsList)
            End If
        End If
        Me.LTdays.Text = Tabs
    End Sub


    Private Function CreateTabDays(ByVal oTabsList As List(Of dtoTab)) As String
        Dim Tabs As String = ""
        Dim Quote As String = """"
        If oTabsList.Count > 0 Then
            Select Case oTabsList(0).TypeTab
                Case DayModeType.TodayYesterday
                    Tabs = "<ul class=" & Quote & "date day" & Quote & ">"
                Case DayModeType.LastMonth
                    Tabs = "<ul class=" & Quote & "date month" & Quote & ">"
                Case DayModeType.LastWeek
                    Tabs = "<ul class=" & Quote & "date week" & Quote & ">"
            End Select

            For Each oTab As dtoTab In oTabsList
                If oTab.isType Then
                    Tabs &= vbCrLf & "<li class=" & Quote & "name" & Quote & ">"
                ElseIf oTab.Selected Then
                    Tabs &= vbCrLf & "<li class=" & Quote & "current" & Quote & ">"
                Else
                    Tabs &= vbCrLf & "<li>"
                End If
                If oTab.Url <> "" AndAlso oTab.Enabled Then
                    Tabs &= "<a href=" & Quote & oTab.Url & Quote & ">" & oTab.Name & "</a>"
                Else
                    Tabs &= oTab.Name
                End If
                Tabs &= "</li>"
            Next
            Tabs &= vbCrLf & "</ul>"
        End If
        Return Tabs
    End Function

    Public Sub LoadTabs(ByVal oTabList As System.Collections.Generic.List(Of lm.Modules.NotificationSystem.Presentation.dtoTab)) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.LoadTabs
        Me.TBSlastUpdates.Enabled = (oTabList.Count > 0)
        Dim SelectedIndex As Integer = 0
        If oTabList.Count > 0 Then
            For Each oTab As dtoTab In oTabList
                Dim oTabView As Telerik.Web.UI.RadTab = Me.TBSlastUpdates.Tabs.FindTabByValue(oTab.TypeTab)
                If Not IsNothing(oTabView) Then
                    If oTab.Selected Then
                        SelectedIndex = oTabView.Index
                    End If
                    oTabView.NavigateUrl = oTab.Url
                    oTabView.Text = Resource.getValue("DayModeType." & oTab.TypeTab.ToString)
                End If
            Next
        End If
        Me.TBSlastUpdates.SelectedIndex = SelectedIndex
    End Sub

    Public ReadOnly Property ToDayTranslated() As String Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.ToDayTranslated
        Get
            If MyBase.Resource Is Nothing Then
                Me.SetCultureSettings()
            End If
            Return Resource.getValue("ToDayTranslated")
        End Get
    End Property

    Public ReadOnly Property YesterdayTranslated() As String Implements IViewCommunityNews.YesterdayTranslated
        Get
            If MyBase.Resource Is Nothing Then
                Me.SetCultureSettings()
            End If
            Return Resource.getValue("YesterdayTranslated")
        End Get
    End Property

    Protected Function GetTabUrl(ByVal CommunityID As Integer, ByVal CurrentView As ViewModeType, ByVal FromView As ViewModeType, ByVal CurrentDay As Date, ByVal DayView As DayModeType, Optional ByVal WithBaseUrl As Boolean = True) As String Implements IViewCommunityNews.GetTabUrl
        Dim url As String = "?"
        If CommunityID > 0 Then
            url &= "&CommunityID=" & CommunityID.ToString
        End If
        If FromView <> ViewModeType.None Then
            url &= "&FromView=" & FromView.ToString
        End If
        '  url &= "&FilterValue=" & oContext.CommunityName
        url &= "&PageSize=" & Me.CurrentPageSize.ToString
        If CurrentDay.Equals(New Date) = False Then
            url &= "&Day=" & CurrentDay.ToShortDateString
        End If
        If DayView <> DayModeType.None Then
            url &= "&DayMode=" & DayView.ToString
        End If
        url &= "&Page=0"
        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If

        Select Case CurrentView
            Case ViewModeType.CurrentCommunity
                url = "Notification/CommunityLastUpdates.aspx" & url
            Case ViewModeType.Portal
                url = "Notification/CommunitiesLastUpdates.aspx" & url
            Case ViewModeType.None
                url = Me.Request.Url.AbsolutePath & url
        End Select
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function

    Private Sub RPTnewsData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTnewsByCommunity.ItemCommand
        If e.CommandName = "enter" Then
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
            Me.PageUtility.AccessToCommunity(Me.CurrentContext.UserContext.CurrentUserID, e.CommandArgument, oResourceConfig, True)
        End If
    End Sub

    Private Sub RPTnewsData_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnewsByCommunity.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim oLink As LinkButton = e.Item.FindControl("LNBentra")
            Dim oName As Literal = e.Item.FindControl("LTcommunityName")
            Dim oDto As dtoCommunitySummaryNews = DirectCast(e.Item.DataItem, dtoCommunitySummaryNews)
            If Not IsNothing(oDto) AndAlso Not IsNothing(oLink) Then
                oLink.Visible = (oDto.ID > 0)
                Try
                    oLink.ToolTip = String.Format(Me.Resource.getValue("CommunityAccess"), oDto.Name)
                Catch ex As Exception

                End Try
            End If
            oName.Visible = Not oLink.Visible
            Dim oHyperLink As HyperLink = e.Item.FindControl("HYPdetails")

            Me.Resource.setHyperLink(oHyperLink, True, True)
            oHyperLink.NavigateUrl = oDto.DetailView
        End If
    End Sub

    Public Sub LoadNotificationSummary(ByVal DayToShow As String, ByVal oList As System.Collections.Generic.List(Of lm.Modules.NotificationSystem.Presentation.dtoCommunitySummaryNews)) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.LoadNotificationSummary
        If oList.Count = 0 Then
            Resource.setLiteral(Me.LTnonews)
            Me.LTnonews.Visible = True
            Me.PGgrid.Visible = False
            Me.RPTnewsByCommunity.Visible = False
        Else
            If oList(0).ID = 0 Then
                oList(0).Name = Me.Resource.getValue("Portale")
            End If
            Me.RPTnewsByCommunity.Visible = True
            Me.LTnonews.Visible = False
            Me.RPTnewsByCommunity.DataSource = oList
            Me.RPTnewsByCommunity.DataBind()
        End If
    End Sub

    Public Sub LoadNotifications(ByVal DayToShow As String, ByVal oList As List(Of dtoModuleNews)) Implements IViewCommunityNews.LoadNotifications

    End Sub

    Public Function GetUrlDetails(ByVal oContext As CommunityNewsContext, Optional ByVal WithBaseUrl As Boolean = True) As Object Implements IViewCommunityNews.GetUrlDetails
        Dim url As String = "?"
        If oContext.UserID > 0 Then
            url &= "&UserID=" & oContext.UserID.ToString
        End If
        url &= "&CommunityID={0}"

        url &= "&PR_CommunityID=" & oContext.CommunityID
        If oContext.CurrentDay.Equals(New Date) = False Then
            url &= "&Day=" & oContext.CurrentDay.ToShortDateString
        End If
        url &= "&PR_PageSize=" & Me.CurrentPageSize.ToString

        If oContext.CommunityName <> "" Then
            url &= "&PR_FindByName=" & oContext.CommunityName
        End If
        If oContext.CurrentDay.Equals(New Date) = False Then
            url &= "&PR_Day=" & oContext.CurrentDay.ToShortDateString
        End If
        If oContext.DayView <> DayModeType.None Then
            url &= "&PR_DayMode=" & oContext.DayView.ToString
        End If

        If oContext.FromView <> ViewModeType.None Then
            url &= "&PR_FromView=" & oContext.FromView.ToString
        End If
        If oContext.PageIndex > 0 Then
            url &= "&PR_Page=" & oContext.PageIndex.ToString
        End If
        url &= "&PR_PageSize=" & Me.CurrentPageSize.ToString
        If oContext.CurrentView <> ViewModeType.None Then
            url &= "&PR_View=" & oContext.CurrentView.ToString
        End If

        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If
        url = "Notification/CommunityNews.aspx" & url
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function

    Public Property CurrentModuleID() As Integer Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.CurrentModuleID
        Get
            If Me.DDLservices.Items.Count > 0 Then
                Return Me.DDLservices.SelectedValue
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Try
                Me.DDLservices.SelectedValue = value
            Catch ex As Exception
                If Me.DDLservices.Items.Count > 0 Then
                    Me.DDLservices.SelectedIndex = 0
                End If
            End Try
        End Set
    End Property

    Public Property CurrentViewFilter() As lm.Modules.NotificationSystem.Domain.ViewModeFiler Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.CurrentViewFilter
        Get
            If Me.RBLview.Visible AndAlso Me.RBLview.SelectedIndex > -1 Then
                Return Me.RBLview.SelectedValue
            Else
                Return ViewModeFiler.None
            End If
        End Get
        Set(ByVal value As lm.Modules.NotificationSystem.Domain.ViewModeFiler)
            Try
                Me.RBLview.SelectedValue = value
            Catch ex As Exception
                Me.RBLview.SelectedValue = ViewModeFiler.ByCommunity
            End Try
        End Set
    End Property

    Public Sub LoadServices(ByVal oList As System.Collections.Generic.List(Of lm.Modules.NotificationSystem.Presentation.dtoRemoteModule)) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.LoadServices
        If IsNothing(oList) Then
            Me.DVfilters.Visible = False
        Else
            Me.DVfilters.Visible = True

            If oList.Count > 1 Then
                oList.Insert(0, New dtoRemoteModule(-1, Resource.getValue("All")))
            End If
            Me.DDLservices.DataSource = oList
            Me.DDLservices.DataTextField = "ModuleName"
            Me.DDLservices.DataValueField = "ModuleID"
            Me.DDLservices.DataBind()
        End If

    End Sub

    Public Sub LoadAllNews(ByVal oList As System.Collections.Generic.List(Of lm.Modules.NotificationSystem.Presentation.dtoMultipleNews)) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.LoadAllNews
        If oList.Count = 0 Then
            Me.LTnonews.Visible = True
            Me.PGgrid.Visible = False
            Me.RPTallNews.Visible = False
            Me.LTnonews.Text = Resource.getValue("NoAllNews")
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

    Private Sub RPTallNews_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTallNews.ItemCommand
        If e.CommandName = "enter" Then
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
            Me.PageUtility.AccessToCommunity(Me.CurrentContext.UserContext.CurrentUserID, e.CommandArgument, oResourceConfig, True)
        End If
    End Sub

    Private Sub RPTallNews_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTallNews.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim oLink As LinkButton = e.Item.FindControl("LNBentra")
            Dim oName As Literal = e.Item.FindControl("LTname")
            Dim oDto As dtoMultipleNews = DirectCast(e.Item.DataItem, dtoMultipleNews)
            If Not IsNothing(oDto) AndAlso Not IsNothing(oLink) Then
                oLink.Visible = (oDto.ID > 0)
                Try
                    oLink.ToolTip = String.Format(Me.Resource.getValue("CommunityAccess"), oDto.Name)
                Catch ex As Exception

                End Try

                Dim oHyperLink As HyperLink = e.Item.FindControl("HYPdetails")
                If oDto.DefaultUrl = "" Then
                    oHyperLink.Visible = False
                Else
                    Me.Resource.setHyperLink(oHyperLink, True, True)
                    oHyperLink.NavigateUrl = oDto.DefaultUrl
                    oHyperLink.Visible = True
                End If


                If oDto.ID = -9 Then
                    Dim oRepeater As Repeater = e.Item.FindControl("RPTmultiples")
                    AddHandler oRepeater.ItemCommand, AddressOf RPTmultiples_ItemCommand
                    AddHandler oRepeater.ItemDataBound, AddressOf RPTmultiples_ItemDataBound
                End If
            End If
            oName.Visible = Not oLink.Visible
        End If
    End Sub

    Protected Friend Sub RPTmultiples_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        If e.CommandName = "enter" Then
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))
            Me.PageUtility.AccessToCommunity(Me.CurrentContext.UserContext.CurrentUserID, e.CommandArgument, oResourceConfig, True)
        End If
    End Sub

    Protected Friend Sub RPTmultiples_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim oLink As LinkButton = e.Item.FindControl("LNBentra")
            Dim oName As Literal = e.Item.FindControl("LTname")
            Dim oDto As dtoNewsItem = DirectCast(e.Item.DataItem, dtoNewsItem)
            If Not IsNothing(oDto) AndAlso Not IsNothing(oLink) Then
                oLink.Visible = (oDto.ID > 0)
                Try
                    oLink.ToolTip = String.Format(Me.Resource.getValue("CommunityAccess"), oDto.Name)
                Catch ex As Exception

                End Try

                Dim oHyperLink As HyperLink = e.Item.FindControl("HYPdetails")
                If oDto.DefaultUrl = "" Then
                    oHyperLink.Visible = False
                Else
                    Me.Resource.setHyperLink(oHyperLink, True, True)
                    oHyperLink.NavigateUrl = oDto.DefaultUrl
                    oHyperLink.Visible = True
                End If
            End If
            oName.Visible = Not oLink.Visible
        End If
    End Sub

    Private Sub DDLservices_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLservices.SelectedIndexChanged
        Me.CurrentPresenter.UpdateAllNews(Not Me.CurrentContext.UserContext.CurrentCommunityID = 0)
    End Sub

    Private Sub RBLview_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLview.SelectedIndexChanged
        Me.CurrentPresenter.UpdateAllNews(Not Me.CurrentContext.UserContext.CurrentCommunityID = 0)
    End Sub

    Public ReadOnly Property PreModuleID() As Integer Implements IViewCommunityNews.PreLoadedModuleID
        Get
            If IsNumeric(Request.QueryString("ModuleID")) Then
                Return Request.QueryString("ModuleID")
            Else
                Return -1
            End If
        End Get
    End Property

    Public ReadOnly Property PreViewFilter() As ViewModeFiler Implements IViewCommunityNews.PreLoadedViewFilter
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ViewModeFiler).GetByString(Request.QueryString("ModeFiler"), ViewModeFiler.ByDate)
        End Get
    End Property

    Public Sub ShowNoNewsFrom(ByVal oDate As DateTime, ByVal CommunityName As String) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.ShowNoNewsFrom
        Me.LTnonews.Visible = True
        Me.LTnonews.Text = String.Format(Resource.getValue("NoNewsFrom"), FormatDateTime(oDate, DateFormat.ShortDate), FormatDateTime(oDate, DateFormat.ShortTime), CommunityName)
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_CommunityNews.Codex)
    End Sub

    Public Sub AddAction(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oActionType As Services_CommunityNews.ActionType, ByVal oDay As Date) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.AddAction
        PageUtility.AddAction(CommunityID, oActionType, CreateObjectToAction(System.Guid.Empty, oDay), InteractionType.SystemToUser)
    End Sub

    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oType As lm.Modules.NotificationSystem.Domain.DayModeType, ByVal oDay As Date) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.AddActionNoPermission
        PageUtility.AddAction(CommunityID, Services_CommunityNews.ActionType.NoPermission, CreateObjectToAction(System.Guid.Empty, oDay), InteractionType.SystemToUser)
    End Sub

    Public Sub AddActionViewNews(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal NewsID As System.Guid) Implements lm.Modules.NotificationSystem.Presentation.IViewCommunityNews.AddActionViewNews

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

   
End Class