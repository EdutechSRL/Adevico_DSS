Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_DasboardListTopControl
    Inherits BaseControl
    Implements IViewDashBoardListTopControl

#Region "Context"
    Private _Presenter As DasboardListTopControlPresenter
    Private ReadOnly Property CurrentPresenter() As DasboardListTopControlPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DasboardListTopControlPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CookieName As String Implements IViewDashBoardListTopControl.CookieName
        Get
            Return ViewStateOrDefault("CookieName", CookieStartName)
        End Get
        Set(value As String)
            ViewState("CookieName") = value
        End Set
    End Property
    Private ReadOnly Property CookieStartName As String Implements IViewDashBoardListTopControl.CookieStartName
        Get
            Return "temp_" & PageUtility.UniqueGuidSession.ToString & "_"
        End Get
    End Property
    Private ReadOnly Property PortalName As String Implements IViewDashBoardListTopControl.PortalName
        Get
            Return Resource.getValue("PortalName")
        End Get
    End Property
    Private Property CurrendIdProject As Long Implements IViewDashBoardListTopControl.CurrendIdProject
        Get
            Return ViewStateOrDefault("CurrendIdProject", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrendIdProject") = value
        End Set
    End Property
    Private Property CurrentTab As TabListItem Implements IViewDashBoardListTopControl.CurrentTab
        Get
            Return ViewStateOrDefault("CurrentTab", TabListItem.Resource)
        End Get
        Set(ByVal value As TabListItem)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSselector.FindTabByValue(value.ToString)
            If Not IsNothing(oTab) Then
                oTab.Selected = True
                Me.ViewState("CurrentTab") = value
            End If
        End Set
    End Property
    Private Property CurrentFilterBy As ProjectFilterBy Implements IViewDashBoardListTopControl.CurrentFilterBy
        Get
            If DDLfilterBy.SelectedIndex > -1 Then
                Return CInt(DDLfilterBy.SelectedValue)
            Else
                Return ProjectFilterBy.All
            End If
        End Get
        Set(ByVal value As ProjectFilterBy)
            If Not IsNothing(DDLfilterBy.Items.FindByValue(value)) Then
                DDLfilterBy.SelectedValue = CInt(value)
            End If
        End Set
    End Property
    Private Property CurrentStatus As ItemListStatus Implements IViewDashBoardListTopControl.CurrentStatus
        Get
            Return ViewStateOrDefault("CurrentStatus", ItemListStatus.All)
        End Get
        Set(ByVal value As ItemListStatus)
            Me.ViewState("CurrentStatus") = value
            If value = ItemListStatus.Ignore Then
                For Each row As RepeaterItem In RPTfilterStatus.Items
                    Dim oHyperLink As HyperLink = row.FindControl("HYPfilterStatus")
                    If oHyperLink.CssClass.Contains(LTsummaryItemSelected.Text) Then
                        oHyperLink.CssClass = Replace(oHyperLink.CssClass, LTsummaryItemSelected.Text, "")
                    End If
                Next
            End If
        End Set
    End Property
    Private Property CurrentGroupBy As ItemsGroupBy Implements IViewDashBoardListTopControl.CurrentGroupBy
        Get
            Return ViewStateOrDefault("CurrentGroupBy", ItemsGroupBy.Plain)
        End Get
        Set(ByVal value As ItemsGroupBy)
            Me.ViewState("CurrentGroupBy") = value
        End Set
    End Property
    Private Property IdContainerCommunity As Integer Implements IViewDashBoardListTopControl.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdContainerCommunity") = value
        End Set
    End Property
    Private Property PageContainer As PageContainerType Implements IViewDashBoardListTopControl.PageContainer
        Get
            Return ViewStateOrDefault("PageContainer", PageContainerType.Dashboard)
        End Get
        Set(value As PageContainerType)
            Me.ViewState("PageContainer") = value
            Resource.setLabel_To_Value(LBfilterItemsBy_t, "LBfilterItemsBy_t." & value.ToString)
            Resource.setLabel_To_Value(LBfilterByProjectFilterStatus_t, "LBfilterByProjectFilterStatus_t." & value.ToString)
        End Set
    End Property
    Private Property PageType As PageListType Implements IViewDashBoardListTopControl.PageType
        Get
            Return ViewStateOrDefault("PageType", PageListType.ListResource)
        End Get
        Set(value As PageListType)
            Me.ViewState("PageType") = value
        End Set
    End Property
    Private Property CurrentFromPage As PageListType Implements IViewDashBoardListTopControl.CurrentFromPage
        Get
            Return ViewStateOrDefault("CurrentFromPage", PageType)
        End Get
        Set(value As PageListType)
            Me.ViewState("CurrentFromPage") = value
        End Set
    End Property
    Private Property PageContext As dtoProjectContext Implements IViewDashBoardListTopControl.PageContext
        Get
            Return ViewStateOrDefault("PageContext", New dtoProjectContext())
        End Get
        Set(ByVal value As dtoProjectContext)
            Me.ViewState("PageContext") = value
        End Set
    End Property
    Private Property CurrentDisplayMode As SummaryDisplay Implements IViewDashBoardListTopControl.CurrentDisplayMode
        Get
            Return ViewStateOrDefault("CurrentDisplayMode", SummaryDisplay.All)
        End Get
        Set(value As SummaryDisplay)
            ViewState("CurrentDisplayMode") = value
        End Set
    End Property
    Private Property CurrentTimeLine As SummaryTimeLine Implements IViewDashBoardListTopControl.CurrentTimeLine
        Get
            Return ViewStateOrDefault("CurrentTimeLine", SummaryTimeLine.Week)
        End Get
        Set(value As SummaryTimeLine)
            ViewState("CurrentTimeLine") = value
        End Set
    End Property
    Private Property CurrentActivityStatus As UserActivityStatus Implements IViewDashBoardListTopControl.CurrentActivityStatus
        Get
            Return ViewStateOrDefault("CurrentActivityStatus", UserActivityStatus.Ignore)
        End Get
        Set(ByVal value As UserActivityStatus)
            Me.ViewState("CurrentActivityStatus") = value
        End Set
    End Property
    Private Property CurrentActivityTimeLine As SummaryTimeLine Implements IViewDashBoardListTopControl.CurrentActivityTimeLine
        Get
            Return ViewStateOrDefault("CurrentActivityTimeLine", SummaryTimeLine.Week)
        End Get
        Set(ByVal value As SummaryTimeLine)
            Me.ViewState("CurrentActivityTimeLine") = value
        End Set
    End Property
    Private ReadOnly Property GetCurrentFilters As dtoItemsFilter Implements IViewDashBoardListTopControl.GetCurrentFilters
        Get
            Dim dto As New dtoItemsFilter

            With dto
                .Display = CurrentDisplayMode
                .TimeLine = CurrentTimeLine
                Select Case PageContainer
                    Case PageContainerType.ProjectsList
                        .ProjectsStatus = CurrentStatus
                        .ActivitiesStatus = ItemListStatus.All
                    Case Else
                        .ProjectsStatus = ItemListStatus.All
                        .ActivitiesStatus = CurrentStatus
                End Select
                .Container = PageContainer
                .GroupBy = CurrentGroupBy
                .FilterBy = CurrentFilterBy
                .IdProject = CurrendIdProject
                .Display = CurrentDisplayMode
                .TimeLine = CurrentTimeLine
                .UserActivitiesStatus = CurrentActivityStatus
                .UserActivitiesTimeLine = CurrentActivityTimeLine
                RaiseEvent UpdateFilter(dto)
            End With
            Return dto
        End Get
    End Property
    Private ReadOnly Property GetSavedFilters As dtoItemsFilter Implements IViewDashBoardListTopControl.GetSavedFilters
        Get
            Dim dto As New dtoItemsFilter
            Try
                With dto
                    Dim myCookie As HttpCookie = Request.Cookies(CookieName)

                    .IdProject = CLng(myCookie("IdProject"))
                    .TimeLine = CInt(myCookie("TimeLine"))
                    .ProjectsStatus = CInt(myCookie("ProjectsStatus"))
                    .ActivitiesStatus = CInt(myCookie("ActivitiesStatus"))
                    .Container = CInt(myCookie("Container"))
                    .GroupBy = CInt(myCookie("GroupBy"))
                    .FilterBy = CInt(myCookie("FilterBy"))
                    .Display = CInt(myCookie("Display"))
                    .PageIndex = CInt(myCookie("PageIndex"))
                    .PageSize = CInt(myCookie("PageSize"))
                    .OrderBy = CInt(myCookie("OrderBy"))
                    .UserActivitiesStatus = CInt(myCookie("UserActivitiesStatus"))
                    .UserActivitiesTimeLine = CInt(myCookie("UserActivitiesTimeLine"))
                    Boolean.TryParse(myCookie("Ascending"), .Ascending)
                End With
            Catch ex As Exception
                dto = dtoItemsFilter.GenerateForGroup(PageContainer, ItemsGroupBy.Plain)
            End Try

            Return dto
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event UpdateCommand(ByVal filter As dtoItemsFilter)
    Public Event Initialized(ByVal filter As dtoItemsFilter)
    Public Event UpdateFilter(ByRef filter As dtoItemsFilter)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBfilterItemsBy_t)
            .setLabel(LBcurrentCommunityFilter_t)
            .setLabel(LBgroupBy_t)
            .setLabel(LBfilterByProjectFilterStatus_t)

            .setLiteral(LTsummaryTitle)
            .setLabel(LBsummaryActivitiesnumber_t)
            .setLabel(LBsummaryResourceActivities_t)
            .setLabel(LBsummaryManageActivities_t)
            .setLabel(LBdashboardProjectName_t)
            LBattachments.ToolTip = .getValue("LBattachments.ToolTip")
            LTsummaryTitleTop.Text = LTsummaryTitle.Text
            LBcloseSummary.ToolTip = .getValue("LBcloseSummary.ToolTip")
        End With
    End Sub
#End Region

#Region "implements"
    Public Sub InitializeControl(context As dtoProjectContext, idContainerCommunity As Integer, loadFromCookies As Boolean, tab As TabListItem, containerType As PageContainerType, fromPage As PageListType, pageType As PageListType, Optional dGroupBy As ItemsGroupBy = ItemsGroupBy.None, Optional filterBy As ProjectFilterBy = ProjectFilterBy.All, Optional projectsStatus As ItemListStatus = ItemListStatus.All, Optional activitiesStatus As ItemListStatus = ItemListStatus.All, Optional ByVal timeline As SummaryTimeLine = SummaryTimeLine.Week, Optional ByVal display As SummaryDisplay = SummaryDisplay.All, Optional ByVal idProject As Long = 0, Optional preloadActivityStatus As UserActivityStatus = UserActivityStatus.Ignore, Optional preloadActivityTimeline As SummaryTimeLine = SummaryTimeLine.Week) Implements IViewDashBoardListTopControl.InitializeControl
        CurrentPresenter.InitView(context, idContainerCommunity, loadFromCookies, tab, containerType, fromPage, pageType, dGroupBy, filterBy, projectsStatus, activitiesStatus, timeline, display, idProject, preloadActivityStatus, preloadActivityTimeline)
        RaiseEvent Initialized(GetCurrentFilters())
    End Sub
    Private Sub InitializeTabs(tabs As List(Of TabListItem), selected As TabListItem, filter As dtoItemsFilter, context As dtoProjectContext) Implements IViewDashBoardListTopControl.InitializeTabs
        Me.TBSselector.Enabled = (tabs.Count > 0)
        For Each view As TabListItem In tabs
            Dim oTabView As Telerik.Web.UI.RadTab = Me.TBSselector.Tabs.FindTabByValue(view.ToString)
            If Not IsNothing(oTabView) Then
                Select Case oTabView.Value
                    Case TabListItem.Manager.ToString
                        Select Case PageContainer
                            Case PageContainerType.ProjectsList
                                oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.ProjectListManager(context.IdCommunity, context.isForPortal, context.isPersonal)
                            Case PageContainerType.Dashboard
                                If PageType <> PageListType.DashboardResource Then
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.DashboardManager, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, CurrentGroupBy, CurrentStatus)
                                Else
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.DashboardManager, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, ItemsGroupBy.Plain, ItemListStatus.Late)
                                End If
                            Case PageContainerType.ProjectDashboard
                                If PageType <> PageListType.ProjectDashboardManager Then
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, CurrendIdProject, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.ProjectDashboardManager, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, CurrentGroupBy, CurrentStatus)
                                Else
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, CurrendIdProject, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.ProjectDashboardManager, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, ItemsGroupBy.Plain, ItemListStatus.Late)
                                End If
                        End Select
                    Case TabListItem.Administration.ToString
                        If PageContainer = PageContainerType.ProjectsList Then
                            oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.ProjectListAdministrator(context.IdCommunity, context.isForPortal, context.isPersonal)
                        End If
                    Case TabListItem.Resource.ToString
                        Select Case PageContainer
                            Case PageContainerType.ProjectsList
                                oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.ProjectListResource(context.IdCommunity, context.isForPortal, context.isPersonal)
                            Case PageContainerType.Dashboard
                                If PageType = PageListType.DashboardResource Then
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.DashboardResource, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, CurrentGroupBy, CurrentStatus)
                                Else
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.DashboardResource, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, ItemsGroupBy.Plain, ItemListStatus.Late)
                                End If
                            Case PageContainerType.ProjectDashboard
                                If PageType = PageListType.ProjectDashboardResource Then
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, CurrendIdProject, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.ProjectDashboardResource, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, CurrentGroupBy, CurrentStatus)
                                Else
                                    oTabView.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.Dashboard(context, CurrendIdProject, IdContainerCommunity, PageContainer, CurrentFromPage, PageListType.ProjectDashboardResource, CurrentTimeLine, CurrentDisplayMode, CurrentFilterBy, ItemsGroupBy.Plain, ItemListStatus.Late)
                                End If
                        End Select
                End Select
                oTabView.Text = Resource.getValue("TabListItem." & view.ToString)
                oTabView.Visible = True
            End If
        Next
        Me.TBSselector.Visible = (tabs.Count > 1)
        CurrentTab = selected
    End Sub

#Region "Summary"
    Public Sub RefreshSummary(filter As dtoItemsFilter, Optional ByVal idProject As Long = 0) Implements IViewDashBoardListTopControl.RefreshSummary
        CurrentPresenter.RefreshSummary(filter, idProject)
    End Sub
    Private Sub DisplayProjectName(name As String) Implements IViewDashBoardListTopControl.DisplayProjectName
        LTsummarySubtitle.Text = String.Format(LTtemplateSummarySubtitle.Text, name)
        LTsummarySubtitle.Visible = True
        LTsummarySubtitleTop.Text = LTsummarySubtitle.Text
        LTsummarySubtitleTop.Visible = True
        DVprojectInfo.Visible = True
        LBdashboardProjectName.Text = name
    End Sub
    Private Sub DisplayUserName(name As String) Implements IViewDashBoardListTopControl.DisplayUserName
        LTsummarySubtitle.Text = String.Format(LTtemplateSummarySubtitle.Text, name)
        LTsummarySubtitle.Visible = True
        LTsummarySubtitleTop.Text = LTsummarySubtitle.Text
        LTsummarySubtitleTop.Visible = True
    End Sub
    Private Sub LoadTimeLines(items As List(Of dtoItemFilter(Of SummaryTimeLine))) Implements IViewDashBoardListTopControl.LoadTimeLines
        Me.RPTtimeline.DataSource = items
        Me.RPTtimeline.DataBind()

        CurrentTimeLine = items.Where(Function(i) i.Selected).Select(Function(i) i.Value).FirstOrDefault()
    End Sub
    Private Sub LoadDisplayMode(items As List(Of dtoItemFilter(Of SummaryDisplay))) Implements IViewDashBoardListTopControl.LoadDisplayMode
        Me.RPTdisplayItems.DataSource = items
        Me.RPTdisplayItems.DataBind()
        If items.Any() Then
            RPTdisplayItems.Visible = (items.Count > 1)
            CurrentDisplayMode = items.Where(Function(i) i.Selected).Select(Function(i) i.Value).FirstOrDefault()
        Else
            RPTdisplayItems.Visible = False
            CurrentDisplayMode = SummaryDisplay.All
        End If
    End Sub
    Private Sub LoadSummaries(items As List(Of dtoDisplayTimelineSummary)) Implements IViewDashBoardListTopControl.LoadSummaries
        Me.RPTsummaryItems.DataSource = items
        Me.RPTsummaryItems.DataBind()
        If items.Where(Function(i) i.ToPage = PageListType.DashboardResource).Any() Then
            LBsummaryResourceActivities.Text = items.Where(Function(i) i.ToPage = PageListType.DashboardResource).Select(Function(i) i.Quantity).FirstOrDefault()
        End If
        If items.Where(Function(i) i.ToPage = PageListType.DashboardManager).Any() Then
            LBsummaryManageActivities.Text = items.Where(Function(i) i.ToPage = PageListType.DashboardManager).Select(Function(i) i.Quantity).FirstOrDefault()
        End If
    End Sub
#End Region

#Region "Filters"
    Private Sub LoadFilterBy(items As List(Of ProjectFilterBy), selected As ProjectFilterBy, Optional isPortal As Boolean = False, Optional communityName As String = "") Implements IViewDashBoardListTopControl.LoadFilterBy
        DVfilterBy.Visible = items.Count > 1

        Dim translations As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) = (From s In items Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(s), .Translation = Me.Resource.getValue("ProjectFilterBy." & PageContainer.ToString & "." & s.ToString)}).ToList

        Me.DDLfilterBy.DataSource = translations
        Me.DDLfilterBy.DataValueField = "Id"
        Me.DDLfilterBy.DataTextField = "Translation"
        Me.DDLfilterBy.DataBind()

        Me.CurrentFilterBy = selected

        Me.DVcurrentCommunity.Visible = Not isPortal AndAlso (selected = ProjectFilterBy.AllPersonalFromCurrentCommunity OrElse selected = ProjectFilterBy.CurrentCommunity)
        Me.LBcurrentCommunityFilter.Text = communityName
    End Sub
    Private Sub LoadGroupByFilters(items As List(Of dtoItemFilter(Of ItemsGroupBy))) Implements IViewDashBoardListTopControl.LoadGroupByFilters
        Me.RPTgroupBy.DataSource = items
        Me.RPTgroupBy.DataBind()
        Me.CurrentGroupBy = items.Where(Function(i) i.Selected).Select(Function(i) i.Value).FirstOrDefault()
    End Sub
    Private Sub LoadStatusFilters(items As List(Of dtoItemFilter(Of ItemListStatus))) Implements IViewDashBoardListTopControl.LoadStatusFilters
        Me.RPTfilterStatus.DataSource = items
        Me.RPTfilterStatus.DataBind()
        If items.Where(Function(i) i.Selected).Any() Then
            Me.CurrentStatus = items.Where(Function(i) i.Selected).Select(Function(i) i.Value).FirstOrDefault()
        Else
            Me.CurrentStatus = ItemListStatus.Ignore
        End If
    End Sub
    Private Sub SaveCurrentFilters(filter As dtoItemsFilter) Implements IViewDashBoardListTopControl.SaveCurrentFilters
        Try
            Dim myCookie As HttpCookie = New HttpCookie(CookieName)
            myCookie("Display") = CInt(filter.Display)
            myCookie("FilterBy") = CInt(filter.FilterBy)
            myCookie("GroupBy") = CInt(filter.GroupBy)
            myCookie("ActivitiesStatus") = CInt(filter.ActivitiesStatus)
            myCookie("ProjectsStatus") = CInt(filter.ProjectsStatus)
            myCookie("Container") = CInt(filter.Container)
            myCookie("TimeLine") = CInt(filter.TimeLine)
            myCookie("PageSize") = filter.PageSize
            myCookie("PageIndex") = filter.PageIndex
            myCookie("OrderBy") = CInt(filter.OrderBy)
            myCookie("Ascending") = filter.Ascending.ToString
            myCookie("IdProject") = filter.IdProject
            myCookie("UserActivitiesStatus") = CInt(filter.UserActivitiesStatus)
            myCookie("UserActivitiesTimeLine") = CInt(filter.UserActivitiesTimeLine)
            myCookie.Expires = DateTime.Now.AddHours(6)


            

          
            Response.Cookies.Add(myCookie)

        Catch ex As Exception

        End Try
    End Sub
#End Region

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        MLVtopItem.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        MLVtopItem.SetActiveView(VIWempty)
    End Sub
#End Region

#Region "internal"

#Region "Filters"
    Protected Function GetItemCssClass(ByVal item As dtoItemFilter(Of ItemListStatus)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " active"
        End If
        Return cssClass
    End Function
    Protected Function GetItemCssClass(ByVal item As dtoItemFilter(Of ItemsGroupBy)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " active"
        End If
        Return cssClass
    End Function
    Private Sub RPTfilterStatus_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfilterStatus.ItemDataBound
        Dim item As dtoItemFilter(Of ItemListStatus) = DirectCast(e.Item.DataItem, dtoItemFilter(Of ItemListStatus))
        Dim oLiteral As Literal = e.Item.FindControl("LTstatus")
        oLiteral.Text = CInt(item.Value)

        Dim oHyperlink As HyperLink = e.Item.FindControl("HYPfilterStatus")
        oHyperlink.Text = Resource.getValue("ItemListStatus." & PageContainer.ToString() & "." & item.Value.ToString)
        oHyperlink.CssClass = LTbtnswitch.Text & GetItemCssClass(item)
        oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & item.Url
    End Sub
    Private Sub RPTgroupBy_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTgroupBy.ItemDataBound
        Dim item As dtoItemFilter(Of ItemsGroupBy) = DirectCast(e.Item.DataItem, dtoItemFilter(Of ItemsGroupBy))
        Dim oHyperlink As HyperLink = e.Item.FindControl("HYPgroupBy")
        oHyperlink.Text = Resource.getValue("ItemsGroupBy." & item.Value.ToString)
        oHyperlink.CssClass = LTbtnswitch.Text & GetItemCssClass(item)
        oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & item.Url
    End Sub
    Private Sub DDLfilterBy_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLfilterBy.SelectedIndexChanged
        Dim oFilter As dtoItemsFilter = GetCurrentFilters()
        DVcurrentCommunity.Visible = (DDLfilterBy.SelectedValue = CInt(ProjectFilterBy.AllPersonalFromCurrentCommunity) OrElse DDLfilterBy.SelectedValue = CInt(ProjectFilterBy.CurrentCommunity))
        CurrentPresenter.Applyfilters(oFilter, PageContext, PageContainer, PageType, CurrentFromPage, CurrentDisplayMode <> SummaryDisplay.All)
        RaiseEvent UpdateCommand(oFilter)
    End Sub
    Public Sub AddDeletedStatus(ByVal filter As dtoItemsFilter)
        Dim hasDeletedStatus As Boolean = False
        For Each row As RepeaterItem In RPTfilterStatus.Items
            Dim oLiteral As Literal = row.FindControl("LTstatus")
            hasDeletedStatus = (oLiteral.Text = CInt(ItemListStatus.Deleted))
        Next
        If Not hasDeletedStatus Then
            CurrentPresenter.ReloadAvailableStatus(filter, PageContext, PageContainer, PageType, CurrentFromPage)
            '        CurrentPresenter.AddDeletedStatus(filter, CurrentListContext, CurrentItemsToView, CurrentView, False)
        End If
    End Sub
    Public Sub RemoveDeletedStatus(ByVal filter As dtoItemsFilter)
        ' CurrentPresenter.ReloadAvailableStatus(filter, PageContext, PageContainer, PageType)
        CurrentPresenter.RemoveDeletedStatus(filter, PageContext, PageContainer, PageType, CurrentFromPage)
        RaiseEvent UpdateCommand(filter)
    End Sub
#End Region

#Region "Summary"
    Protected Function GetItemCssClass(ByVal item As dtoItemFilter(Of SummaryTimeLine)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " active"
        End If
        Return cssClass
    End Function
    Protected Function GetItemCssClass(ByVal item As dtoItemFilter(Of SummaryDisplay)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " active"
        End If
        Return cssClass
    End Function
    Public Function GetItemCssClass(ByVal item As dtoDisplayTimelineActivity) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Status = CurrentActivityStatus AndAlso item.TimeLine = CurrentActivityTimeLine AndAlso item.ToPage = PageType Then
            cssClass &= " " & LTsummaryItemSelected.Text
            Me.Response.Cookies("div.summary").Value = "true"
        End If
        Return cssClass
    End Function
    Protected Sub RPTitems_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Select Case e.CommandName
            Case "display"
                Me.CurrentDisplayMode = e.CommandArgument
                'RaiseEvent ViewCommand(CurrentTimeLine, e.CommandArgument)
            Case "timeline"
                Me.CurrentTimeLine = e.CommandArgument
                'RaiseEvent ViewCommand(e.CommandArgument, CurrentDisplay)
        End Select
        Dim oFilter As dtoItemsFilter = GetCurrentFilters()
        CurrentPresenter.Applyfilters(oFilter, PageContext, PageContainer, PageType, CurrentFromPage, True)
        RaiseEvent UpdateCommand(oFilter)
    End Sub
    Private Sub RPTdisplayItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTdisplayItems.ItemDataBound
        Dim item As dtoItemFilter(Of SummaryDisplay) = DirectCast(e.Item.DataItem, dtoItemFilter(Of SummaryDisplay))
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBdisplayItems")
        oLinkButton.Text = Resource.getValue("SummaryDisplay." & item.Value.ToString)
        oLinkButton.CssClass = LTbtnswitch.Text & GetItemCssClass(item) & IIf(PageType = PageListType.ListAdministrator AndAlso item.Value = SummaryDisplay.Filtered, " " & LTbtnswitchDisabledCssClass.Text, "")
    End Sub
    Private Sub RPTtimeline_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtimeline.ItemDataBound
        Dim item As dtoItemFilter(Of SummaryTimeLine) = DirectCast(e.Item.DataItem, dtoItemFilter(Of SummaryTimeLine))
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBtimeline")
        oLinkButton.Text = Resource.getValue("SummaryTimeLine." & item.Value.ToString)
        oLinkButton.CssClass = LTbtnswitch.Text & GetItemCssClass(item)
    End Sub
    Private Sub RPTsummaryItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsummaryItems.ItemDataBound
        Dim item As dtoDisplayTimelineSummary = e.Item.DataItem
        Dim oLabel As Label = e.Item.FindControl("LBsummaryInfo_t")
        oLabel.Text = Resource.getValue("LBsummaryInfo_t." & item.ToPage.ToString)

    End Sub
    Protected Sub RPTsummaryActivities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim item As dtoDisplayTimelineActivity = e.Item.DataItem
        Dim oHyperLink As HyperLink = e.Item.FindControl("HYPviewDashBoard")
        Dim oLiteral As Literal = e.Item.FindControl("LTviewDashboard")
        Dim disableHyperlink As Boolean = String.IsNullOrEmpty(item.DestinationUrl) OrElse (PageContainer = PageContainerType.ProjectsList AndAlso CurrentDisplayMode = SummaryDisplay.Filtered)
        oHyperLink.Visible = Not disableHyperlink
        oLiteral.Visible = disableHyperlink
        If disableHyperlink Then
            oLiteral.Text = String.Format(LTdefaultSummaryItem.Text, item.Quantity, Resource.getValue("LNBactivitiesToDo.Status." & item.Status.ToString), Resource.getValue("LNBactivitiesToDo.SummaryTimeLine." & item.TimeLine.ToString))
        Else
            oHyperLink.Text = String.Format(LTdefaultSummaryItem.Text, item.Quantity, Resource.getValue("LNBactivitiesToDo.Status." & item.Status.ToString), Resource.getValue("LNBactivitiesToDo.SummaryTimeLine." & item.TimeLine.ToString))
            oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & item.DestinationUrl
        End If
    End Sub
#End Region

    Public Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
        Dim cssClass As String = ""
        Select Case d
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & d.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function

#End Region

    
End Class