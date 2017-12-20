Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel

Public Class ResourceDashboard
    Inherits PMpageDashboard
    Implements IViewDashboard

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim a As HttpCookieCollection = Request.Cookies
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(TabListItem.Resource)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

            Master.ServiceTitle = .getValue("ServiceTitle.Dashboard.Resource")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.Dashboard.Resource.ToolTip")
            .setHyperLink(HYPprojectListResourceBottom, False, True)
            .setHyperLink(HYPprojectListResourceTop, False, True)
            .setHyperLink(HYPprojectListManagerBottom, False, True)
            .setHyperLink(HYPprojectListManagerTop, False, True)
            .setButton(BTNsaveMyCompletenessTop, True)
            .setButton(BTNsaveMyCompletenessBottom, True)
        End With
    End Sub
    Public Overrides Function GetCurrentContainer() As PageContainerType
        Return PageContainerType.Dashboard
    End Function
#End Region

#Region "Implements"
    Private Sub InitializeTopControls(context As dtoProjectContext, idContainerComunity As Integer, loadFromCookies As Boolean, tab As TabListItem, pContainer As PageContainerType, Optional dGroupBy As ItemsGroupBy = ItemsGroupBy.None, Optional filterBy As ProjectFilterBy = ProjectFilterBy.All, Optional projectsStatus As ItemListStatus = ItemListStatus.All, Optional activitiesStatus As ItemListStatus = ItemListStatus.All, Optional ByVal timeline As SummaryTimeLine = SummaryTimeLine.Week, Optional ByVal display As SummaryDisplay = SummaryDisplay.All) Implements IViewDashboard.InitializeTopControls
        CTRLtopControls.InitializeControl(context, idContainerComunity, loadFromCookies, tab, pContainer, PreloadFromPage, PageListType.DashboardResource, dGroupBy, filterBy, projectsStatus, activitiesStatus, timeline, display, 0, PreloadUserActivityStatus, PreloadActivityTimeline)
    End Sub
   
    Private Sub SetLinkToProjectsAsManager(url As String) Implements IViewDashboard.SetLinkToProjectsAsManager
        HYPprojectListManagerBottom.NavigateUrl = ApplicationUrlBase & url
        HYPprojectListManagerTop.NavigateUrl = HYPprojectListManagerBottom.NavigateUrl
        HYPprojectListManagerBottom.Visible = True
        HYPprojectListManagerTop.Visible = True
    End Sub
    Private Sub SetLinkToProjectsAsResource(url As String) Implements IViewDashboard.SetLinkToProjectsAsResource
        HYPprojectListResourceBottom.NavigateUrl = ApplicationUrlBase & url
        HYPprojectListResourceTop.NavigateUrl = HYPprojectListResourceBottom.NavigateUrl
        HYPprojectListResourceBottom.Visible = True
        HYPprojectListResourceTop.Visible = True
    End Sub
   
#End Region

#Region "Internal"

    Private Sub CTRLtopControls_Initialized(filter As dtoItemsFilter) Handles CTRLtopControls.Initialized
        LastFilterSettings = filter
        CTRLlistTree.Visible = False
        CTRLlistPlain.Visible = False
        CTRLlistMultipleTree.Visible = False
        Select Case filter.GroupBy
            Case ItemsGroupBy.Plain
                CTRLlistTree.IsInitialized = False
                CTRLlistMultipleTree.IsInitialized = False
                CTRLlistPlain.Visible = True
                CTRLlistPlain.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
            Case ItemsGroupBy.Community, ItemsGroupBy.EndDate, ItemsGroupBy.Project
                CTRLlistPlain.IsInitialized = False
                CTRLlistMultipleTree.IsInitialized = False
                CTRLlistTree.Visible = True
                CTRLlistTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
            Case ItemsGroupBy.CommunityProject
                CTRLlistTree.IsInitialized = False
                CTRLlistPlain.IsInitialized = False
                CTRLlistMultipleTree.Visible = True
                CTRLlistMultipleTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
        End Select
    End Sub
    Private Sub CTRLtopControls_UpdateCommand(filter As dtoItemsFilter) Handles CTRLtopControls.UpdateCommand
        Dim previous As dtoItemsFilter = LastFilterSettings
        CTRLlistTree.Visible = False
        CTRLlistMultipleTree.Visible = False
        CTRLlistPlain.Visible = False
        Select Case filter.GroupBy
            Case ItemsGroupBy.Plain
                CTRLlistMultipleTree.IsInitialized = False
                CTRLlistTree.IsInitialized = False
                CTRLlistPlain.Visible = True
                If Not CTRLlistPlain.IsInitialized OrElse (previous.FilterBy <> filter.FilterBy OrElse filter.ActivitiesStatus <> previous.ActivitiesStatus OrElse filter.ProjectsStatus <> previous.ProjectsStatus OrElse filter.GroupBy <> previous.GroupBy) Then
                    CTRLlistPlain.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
                End If
            Case ItemsGroupBy.Community, ItemsGroupBy.EndDate,
                CTRLlistPlain.IsInitialized = False
                CTRLlistMultipleTree.IsInitialized = False
                CTRLlistTree.Visible = True
                If Not CTRLlistTree.IsInitialized OrElse (previous.FilterBy <> filter.FilterBy OrElse filter.ActivitiesStatus <> previous.ActivitiesStatus OrElse filter.ProjectsStatus <> previous.ProjectsStatus OrElse filter.GroupBy <> previous.GroupBy) Then
                    CTRLlistTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
                End If
            Case ItemsGroupBy.CommunityProject
                CTRLlistPlain.IsInitialized = False
                CTRLlistTree.IsInitialized = False
                CTRLlistMultipleTree.Visible = True
                If Not CTRLlistTree.IsInitialized OrElse (previous.FilterBy <> filter.FilterBy OrElse filter.ActivitiesStatus <> previous.ActivitiesStatus OrElse filter.ProjectsStatus <> previous.ProjectsStatus OrElse filter.GroupBy <> previous.GroupBy) Then
                    CTRLlistMultipleTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
                End If
        End Select
        LastFilterSettings = filter
    End Sub
    Private Sub CTRLtopControls_UpdateFilter(ByRef filter As dtoItemsFilter) Handles CTRLtopControls.UpdateFilter
        Select Case filter.GroupBy
            Case ItemsGroupBy.Plain
                If CTRLlistPlain.IsInitialized Then
                    filter.OrderBy = CTRLlistPlain.CurrentOrderBy
                    filter.PageIndex = CTRLlistPlain.CurrentPageIndex
                    filter.Ascending = CTRLlistPlain.CurrentAscending
                    filter.PageSize = CTRLlistPlain.CurrentPageSize
                Else
                    filter.OrderBy = ProjectOrderBy.Deadline
                    filter.PageIndex = 0
                    filter.PageSize = CTRLlistPlain.CurrentPageSize
                    filter.Ascending = True
                End If
            Case ItemsGroupBy.EndDate
                If CTRLlistTree.IsInitialized AndAlso CTRLlistTree.CurrentGroupBy = ItemsGroupBy.EndDate Then
                    filter.OrderBy = CTRLlistTree.CurrentOrderBy
                    filter.PageIndex = CTRLlistTree.CurrentPageIndex
                    filter.Ascending = CTRLlistTree.CurrentAscending
                    filter.PageSize = CTRLlistTree.CurrentPageSize
                Else
                    filter.OrderBy = ProjectOrderBy.EndDate
                    filter.PageIndex = 0
                    filter.PageSize = CTRLlistTree.CurrentPageSize
                    filter.Ascending = True
                End If
            Case ItemsGroupBy.Community
                If CTRLlistTree.IsInitialized AndAlso CTRLlistTree.CurrentGroupBy = ItemsGroupBy.Community Then
                    filter.OrderBy = CTRLlistTree.CurrentOrderBy
                    filter.PageIndex = CTRLlistTree.CurrentPageIndex
                    filter.Ascending = CTRLlistTree.CurrentAscending
                    filter.PageSize = CTRLlistTree.CurrentPageSize
                Else
                    filter.OrderBy = ProjectOrderBy.CommunityName
                    filter.PageIndex = 0
                    filter.PageSize = CTRLlistTree.CurrentPageSize
                    filter.Ascending = True
                End If
            Case ItemsGroupBy.Project
                If CTRLlistTree.IsInitialized AndAlso CTRLlistTree.CurrentGroupBy = ItemsGroupBy.Project Then
                    filter.OrderBy = CTRLlistTree.CurrentOrderBy
                    filter.PageIndex = CTRLlistTree.CurrentPageIndex
                    filter.Ascending = CTRLlistTree.CurrentAscending
                    filter.PageSize = CTRLlistTree.CurrentPageSize
                Else
                    filter.OrderBy = ProjectOrderBy.Name
                    filter.PageIndex = 0
                    filter.PageSize = CTRLlistTree.CurrentPageSize
                    filter.Ascending = True
                End If
            Case ItemsGroupBy.CommunityProject
                If CTRLlistMultipleTree.IsInitialized Then
                    filter.OrderBy = CTRLlistMultipleTree.CurrentOrderBy
                    filter.PageIndex = CTRLlistMultipleTree.CurrentPageIndex
                    filter.Ascending = CTRLlistMultipleTree.CurrentAscending
                    filter.PageSize = CTRLlistMultipleTree.CurrentPageSize
                Else
                    filter.OrderBy = ProjectOrderBy.CommunityName
                    filter.PageIndex = 0
                    filter.PageSize = CTRLlistMultipleTree.CurrentPageSize
                    filter.Ascending = True
                End If
        End Select
    End Sub

    Private Sub CTRLlistPlain_AllowMyCompletionSave() Handles CTRLlistPlain.AllowMyCompletionSave
        BTNsaveMyCompletenessBottom.Visible = True
        BTNsaveMyCompletenessTop.Visible = True
    End Sub
    Private Sub CTRLlistPlain_GetCurrentFilter(ByRef filter As dtoItemsFilter) Handles CTRLlistPlain.GetCurrentFilter
        filter = LastFilterSettings
    End Sub
    Private Sub CTRLlistPlain_CompletionSaved(ByVal saved As Boolean, ByVal idTask As Long, ByVal taskname As String, updateSummary As Boolean) Handles CTRLlistPlain.CompletionSaved
        CompletionSaved(saved, idTask, taskname, updateSummary)
    End Sub
    Private Sub CTRLlistPlain_TasksCompletionSaved(savedTasks As Integer, unsavedTasks As Integer, tasks As Integer, updateSummary As Boolean) Handles CTRLlistPlain.TasksCompletionSaved
        CompletionSaved(savedTasks, unsavedTasks, tasks, updateSummary)
    End Sub

    Private Sub CTRLlistTree_AllowMyCompletionSave() Handles CTRLlistTree.AllowMyCompletionSave
        BTNsaveMyCompletenessBottom.Visible = True
        BTNsaveMyCompletenessTop.Visible = True
    End Sub
    Private Sub CTRLlistTree_CompletionSaved(ByVal saved As Boolean, ByVal idTask As Long, ByVal taskname As String, updateSummary As Boolean) Handles CTRLlistTree.CompletionSaved
        CompletionSaved(saved, idTask, taskname, updateSummary)
    End Sub
    Private Sub CTRLlistTree_GetCurrentFilter(ByRef filter As dtoItemsFilter) Handles CTRLlistTree.GetCurrentFilter
        filter = LastFilterSettings
    End Sub
    Private Sub CTRLlistTree_TasksCompletionSaved(savedTasks As Integer, unsavedTasks As Integer, tasks As Integer, updateSummary As Boolean) Handles CTRLlistTree.TasksCompletionSaved
        CompletionSaved(savedTasks, unsavedTasks, tasks, updateSummary)
    End Sub

    Private Sub CTRLlistMultipleTree_AllowMyCompletionSave() Handles CTRLlistMultipleTree.AllowMyCompletionSave
        BTNsaveMyCompletenessBottom.Visible = True
        BTNsaveMyCompletenessTop.Visible = True
    End Sub
    Private Sub CTRLlistMultipleTree_GetCurrentFilter(ByRef filter As dtoItemsFilter) Handles CTRLlistMultipleTree.GetCurrentFilter
        filter = LastFilterSettings
    End Sub
    Private Sub CTRLlistMultipleTree_CompletionSaved(ByVal saved As Boolean, ByVal idTask As Long, ByVal taskname As String, updateSummary As Boolean) Handles CTRLlistMultipleTree.CompletionSaved
        CompletionSaved(saved, idTask, taskname, updateSummary)
    End Sub
    Private Sub CTRLlistMultipleTree_TasksCompletionSaved(savedTasks As Integer, unsavedTasks As Integer, tasks As Integer, updateSummary As Boolean) Handles CTRLlistMultipleTree.TasksCompletionSaved
        CompletionSaved(savedTasks, unsavedTasks, tasks, updateSummary)
    End Sub

    Private Sub CompletionSaved(savedTasks As Integer, unsavedTasks As Integer, tasks As Integer, updateSummary As Boolean)
        Me.CTRLmessages.Visible = True
        Dim message As String = PageContainerType.Dashboard.ToString & "." & PageListType.DashboardResource.ToString() & ".CompletionSaved."
        If savedTasks < 0 Then
            savedTasks = 0
        End If
        If savedTasks < 2 Then
            message &= savedTasks.ToString & "."
        Else
            message &= "n."
        End If
        If unsavedTasks < 0 Then
            unsavedTasks = 0
        End If
        If unsavedTasks < 2 Then
            message &= unsavedTasks.ToString
        Else
            message &= "n"
        End If
     
        CTRLmessages.Visible = True
        If savedTasks > 1 AndAlso unsavedTasks > 1 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), savedTasks, unsavedTasks + savedTasks), Me.GetMessageType(savedTasks, unsavedTasks))
        ElseIf savedTasks > 1 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), savedTasks), Me.GetMessageType(savedTasks, unsavedTasks))
        ElseIf unsavedTasks > 1 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(message), unsavedTasks + savedTasks), Me.GetMessageType(savedTasks, unsavedTasks))
        Else
            CTRLmessages.InitializeControl(Resource.getValue(message), Me.GetMessageType(savedTasks, unsavedTasks))
        End If

        UpdateContents(updateSummary)
    End Sub
    
    Private Sub CompletionSaved(ByVal saved As Boolean, ByVal idTask As Long, ByVal taskname As String, updateSummary As Boolean)
        Me.CTRLmessages.Visible = True
        If (saved) Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(PageContainerType.Dashboard.ToString & "." & PageListType.DashboardResource.ToString() & ".CompletionSaved." & saved.ToString), taskname), Helpers.MessageType.success)
        Else
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(PageContainerType.Dashboard.ToString & "." & PageListType.DashboardResource.ToString() & ".CompletionSaved." & saved.ToString), taskname), Helpers.MessageType.error)
        End If
        UpdateContents(updateSummary)
    End Sub
    Private Sub UpdateContents(updateSummary As Boolean)
        If updateSummary Then
            Dim filter As dtoItemsFilter = LastFilterSettings
            CTRLtopControls.RefreshSummary(filter)
        End If

        'If CTRLlistPlain.Visible Then
        '    CTRLlistPlain.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
        'ElseIf CTRLlistMultipleTree.Visible Then
        '    CTRLlistMultipleTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
        'ElseIf CTRLlistTree.Visible Then
        '    CTRLlistTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.Dashboard, FromPage, PageListType.DashboardResource)
        'End If
    End Sub

    Private Sub ManagerDashboard_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub
    Private Sub BTNsaveMyCompletenessBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveMyCompletenessBottom.Click, BTNsaveMyCompletenessTop.Click
        If CTRLlistPlain.Visible Then
            CTRLlistPlain.SaveMyCompletions()
        ElseIf CTRLlistMultipleTree.Visible Then
            CTRLlistMultipleTree.SaveMyCompletions()
        ElseIf CTRLlistTree.Visible Then
            CTRLlistTree.SaveMyCompletions()
        End If
    End Sub
#End Region

End Class