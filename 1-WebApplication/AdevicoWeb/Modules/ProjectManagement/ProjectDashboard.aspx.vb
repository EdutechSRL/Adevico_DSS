Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel

Public Class ProjectDashboard
    Inherits PMpageProjectDashboard
    Implements IViewProjectDashboard


#Region "Implements"
    Private ReadOnly Property PreloadIdProject As Long Implements IViewProjectDashboard.PreloadIdProject
        Get
            If IsNumeric(Me.Request.QueryString("pId")) Then
                Return CLng(Me.Request.QueryString("pId"))
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(TabListItem.Manager)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

            Master.ServiceTitle = .getValue("ServiceTitle.ProjectDashboard." & PageListType.ProjectDashboardManager.ToString)
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ProjectDashboard." & PageListType.ProjectDashboardManager.ToString & ".ToolTip")
            .setHyperLink(HYPprojectListResourceBottom, False, True)
            .setHyperLink(HYPprojectListResourceTop, False, True)
            .setHyperLink(HYPprojectListManagerBottom, False, True)
            .setHyperLink(HYPprojectListManagerTop, False, True)

            .setHyperLink(HYPdashboardManagerBottom, False, True)
            .setHyperLink(HYPdashboardManagerTop, False, True)
            .setHyperLink(HYPdashboardResourceBottom, False, True)
            .setHyperLink(HYPdashboardResourceTop, False, True)
        End With
    End Sub
    Public Overrides Function GetCurrentContainer() As PageContainerType
        Return PageContainerType.ProjectDashboard
    End Function
#End Region

#Region "Implements"
    Private Sub InitializeProjectTopControl(context As dtoProjectContext, idContainerCommunity As Integer, loadFromCookies As Boolean, tab As TabListItem, pContainer As PageContainerType, idProject As Long, Optional dGroupBy As ItemsGroupBy = ItemsGroupBy.None, Optional projectsStatus As ItemListStatus = ItemListStatus.All, Optional activitiesStatus As ItemListStatus = ItemListStatus.Late, Optional timeline As SummaryTimeLine = SummaryTimeLine.Week) Implements IViewProjectDashboard.InitializeProjectTopControl
        CTRLtopControls.InitializeControl(context, idContainerCommunity, loadFromCookies, tab, pContainer, PreloadFromPage, PageListType.ProjectDashboardManager, dGroupBy, ProjectFilterBy.All, projectsStatus, activitiesStatus, timeline, SummaryDisplay.Project, idProject, PreloadUserActivityStatus, PreloadActivityTimeline)
    End Sub
    Private Sub SetLinkToProjectsAsManager(url As String) Implements IViewProjectDashboard.SetLinkToProjectsAsManager
        HYPprojectListManagerBottom.NavigateUrl = ApplicationUrlBase & url
        HYPprojectListManagerTop.NavigateUrl = HYPprojectListManagerBottom.NavigateUrl
        HYPprojectListManagerBottom.Visible = True
        HYPprojectListManagerTop.Visible = True
    End Sub
    Private Sub SetLinkToProjectsAsResource(url As String) Implements IViewProjectDashboard.SetLinkToProjectsAsResource
        HYPprojectListResourceBottom.NavigateUrl = ApplicationUrlBase & url
        HYPprojectListResourceTop.NavigateUrl = HYPprojectListResourceBottom.NavigateUrl
        HYPprojectListResourceBottom.Visible = True
        HYPprojectListResourceTop.Visible = True
    End Sub
    Private Sub SetLinkToDashBoardAsManager(url As String) Implements IViewProjectDashboard.SetLinkToDashBoardAsManager
        HYPdashboardManagerBottom.NavigateUrl = ApplicationUrlBase & url
        HYPdashboardManagerTop.NavigateUrl = HYPdashboardManagerBottom.NavigateUrl
        HYPdashboardManagerBottom.Visible = True
        HYPdashboardManagerTop.Visible = True
    End Sub
    Private Sub SetLinkToDashBoardAsResource(url As String) Implements IViewProjectDashboard.SetLinkToDashBoardAsResource
        HYPdashboardResourceBottom.NavigateUrl = ApplicationUrlBase & url
        HYPdashboardResourceTop.NavigateUrl = HYPdashboardResourceBottom.NavigateUrl
        HYPdashboardResourceBottom.Visible = True
        HYPdashboardResourceTop.Visible = True
    End Sub
#End Region

#Region "Internal"

    Private Sub CTRLtopControls_Initialized(filter As dtoItemsFilter) Handles CTRLtopControls.Initialized
        LastFilterSettings = filter
        CTRLlistTree.Visible = False
        CTRLlistPlain.Visible = False
        Select Case filter.GroupBy
            Case ItemsGroupBy.Plain
                CTRLlistTree.IsInitialized = False
                CTRLlistPlain.Visible = True
                CTRLlistPlain.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.ProjectDashboard, FromPage, PageListType.ProjectDashboardManager)
            Case ItemsGroupBy.EndDate
                CTRLlistPlain.IsInitialized = False

                CTRLlistTree.Visible = True
                CTRLlistTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.ProjectDashboard, FromPage, PageListType.ProjectDashboardManager)

        End Select
    End Sub
    Private Sub CTRLtopControls_UpdateCommand(filter As dtoItemsFilter) Handles CTRLtopControls.UpdateCommand
        Dim previous As dtoItemsFilter = LastFilterSettings
        CTRLlistTree.Visible = False
        CTRLlistPlain.Visible = False
        Select Case filter.GroupBy
            Case ItemsGroupBy.Plain
                CTRLlistTree.IsInitialized = False
                CTRLlistPlain.Visible = True
                If Not CTRLlistPlain.IsInitialized OrElse (previous.FilterBy <> filter.FilterBy OrElse filter.ActivitiesStatus <> previous.ActivitiesStatus OrElse filter.ProjectsStatus <> previous.ProjectsStatus OrElse filter.GroupBy <> previous.GroupBy) Then
                    CTRLlistPlain.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.ProjectDashboard, FromPage, PageListType.ProjectDashboardManager)
                End If
            Case ItemsGroupBy.EndDate,
                CTRLlistPlain.IsInitialized = False

                CTRLlistTree.Visible = True
                If Not CTRLlistTree.IsInitialized OrElse (previous.FilterBy <> filter.FilterBy OrElse filter.ActivitiesStatus <> previous.ActivitiesStatus OrElse filter.ProjectsStatus <> previous.ProjectsStatus OrElse filter.GroupBy <> previous.GroupBy) Then
                    CTRLlistTree.InitializeControl(DashboardContext, IdContainerCommunity, filter, PageContainerType.ProjectDashboard, FromPage, PageListType.ProjectDashboardManager)
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
        End Select
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

    Private Sub CTRLlistTree_CompletionSaved(ByVal saved As Boolean, ByVal idTask As Long, ByVal taskname As String, updateSummary As Boolean) Handles CTRLlistTree.CompletionSaved
        CompletionSaved(saved, idTask, taskname, updateSummary)
    End Sub
    Private Sub CTRLlistTree_GetCurrentFilter(ByRef filter As dtoItemsFilter) Handles CTRLlistTree.GetCurrentFilter
        filter = LastFilterSettings
    End Sub
    Private Sub CTRLlistTree_TasksCompletionSaved(savedTasks As Integer, unsavedTasks As Integer, tasks As Integer, updateSummary As Boolean) Handles CTRLlistTree.TasksCompletionSaved
        CompletionSaved(savedTasks, unsavedTasks, tasks, updateSummary)
    End Sub

    Private Sub CompletionSaved(savedTasks As Integer, unsavedTasks As Integer, tasks As Integer, updateSummary As Boolean)
        Me.CTRLmessages.Visible = True
        Dim message As String = PageContainerType.ProjectDashboard.ToString & "." & PageListType.ProjectDashboardManager.ToString() & ".CompletionSaved."
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
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(PageContainerType.ProjectDashboard.ToString & "." & PageListType.ProjectDashboardManager.ToString() & ".CompletionSaved." & saved.ToString), taskname), Helpers.MessageType.success)
        Else
            CTRLmessages.InitializeControl(String.Format(Resource.getValue(PageContainerType.ProjectDashboard.ToString & "." & PageListType.ProjectDashboardManager.ToString() & ".CompletionSaved." & saved.ToString), taskname), Helpers.MessageType.error)
        End If
        UpdateContents(updateSummary)
    End Sub
    Private Sub UpdateContents(updateSummary As Boolean)
        If updateSummary Then
            Dim filter As dtoItemsFilter = LastFilterSettings
            CTRLtopControls.RefreshSummary(filter)
        End If
    End Sub

    Private Sub ManagerDashboard_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub
#End Region

End Class