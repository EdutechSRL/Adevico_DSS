Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ListAdministrator
    Inherits PMpageBaseList
    Implements IViewProjectsList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(TabListItem.Administration)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

            Master.ServiceTitle = .getValue("ServiceTitle.ListAdministrator")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ListAdministrator.ToolTip")

        End With
    End Sub
    Public Overrides Function GetCurrentContainer() As PageContainerType
        Return PageContainerType.ProjectsList
    End Function
#End Region

#Region "Implements"
    Private Sub InitializeTopControls(context As dtoProjectContext, idContainerComunity As Integer, loadFromCookies As Boolean, tab As TabListItem, container As PageContainerType, Optional dGroupBy As ItemsGroupBy = ItemsGroupBy.None, Optional filterBy As ProjectFilterBy = ProjectFilterBy.All, Optional projectsStatus As ItemListStatus = ItemListStatus.All, Optional activitiesStatus As ItemListStatus = ItemListStatus.All, Optional ByVal timeline As SummaryTimeLine = SummaryTimeLine.Week, Optional ByVal display As SummaryDisplay = SummaryDisplay.All) Implements IViewProjectsList.InitializeTopControls
        CTRLtopControls.InitializeControl(context, idContainerComunity, loadFromCookies, tab, container, PageListType.ListAdministrator, PageListType.ListAdministrator, dGroupBy, filterBy, projectsStatus, activitiesStatus, timeline, display)
    End Sub
    Private Sub AllowAddProject(personalUrl As String, Optional publicUrl As String = "", Optional personalPortalUrl As String = "") Implements IViewProjectsList.AllowAddProject
        Dim allowAdd As Boolean = Not (String.IsNullOrEmpty(personalUrl) AndAlso String.IsNullOrEmpty(publicUrl))
        Dim allowAll As Boolean = (Not String.IsNullOrEmpty(personalUrl) AndAlso Not String.IsNullOrEmpty(publicUrl))
        Me.DVnewProjectBottom.Visible = allowAdd
        Me.DVnewProjectTop.Visible = allowAdd

        Me.DVnewProjectBottom.Attributes.Add("class", IIf(allowAll, "ddbuttonlist enabled", "ddbuttonlist"))
        Me.DVnewProjectTop.Attributes.Add("class", IIf(allowAll, "ddbuttonlist enabled", "ddbuttonlist"))
        If Not String.IsNullOrEmpty(personalUrl) Then
            HYPnewPersonalProjectBottom.Visible = True
            HYPnewPersonalProjectTop.Visible = True
            HYPnewPersonalProjectBottom.NavigateUrl = ApplicationUrlBase & personalUrl
            HYPnewPersonalProjectTop.NavigateUrl = HYPnewPersonalProjectBottom.NavigateUrl

            If forPortal Then
                Resource.setHyperLink(HYPnewPersonalProjectTop, "forPortal", False, True)
            Else
                Resource.setHyperLink(HYPnewPersonalProjectTop, "forCommunity", False, True)
            End If
            HYPnewPersonalProjectBottom.Text = HYPnewPersonalProjectTop.Text
            HYPnewPersonalProjectBottom.ToolTip = HYPnewPersonalProjectTop.ToolTip
        Else
            HYPnewPersonalProjectTop.Visible = False
            HYPnewPersonalProjectBottom.Visible = False
        End If
        If Not String.IsNullOrEmpty(publicUrl) Then
            HYPnewProjectBottom.Visible = True
            HYPnewProjectTop.Visible = True
            HYPnewProjectBottom.NavigateUrl = ApplicationUrlBase & publicUrl
            HYPnewProjectTop.NavigateUrl = HYPnewProjectBottom.NavigateUrl

            If forPortal Then
                Resource.setHyperLink(HYPnewProjectTop, "forPortal", False, True)
            Else
                Resource.setHyperLink(HYPnewProjectTop, "forCommunity", False, True)
            End If
            HYPnewProjectBottom.Text = HYPnewProjectTop.Text
            HYPnewProjectBottom.ToolTip = HYPnewProjectTop.ToolTip
        Else
            HYPnewProjectBottom.Visible = False
            HYPnewProjectTop.Visible = False
        End If
    End Sub

#End Region

#Region "Internal"
    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Private Sub CTRLtopControls_Initialized(filter As dtoItemsFilter) Handles CTRLtopControls.Initialized
        LastFilterSettings = filter
        CTRLlistTree.Visible = False
        CTRLlistPlain.Visible = False
        Select Case filter.GroupBy
            Case ItemsGroupBy.Plain
                CTRLlistTree.IsInitialized = False
                CTRLlistPlain.Visible = True
                CTRLlistPlain.InitializeControl(CurrentListContext, filter, PageListType.ListAdministrator)
            Case ItemsGroupBy.Community
                CTRLlistPlain.IsInitialized = False
                CTRLlistTree.Visible = True
                CTRLlistTree.InitializeControl(CurrentListContext, filter, PageListType.ListAdministrator)
            Case ItemsGroupBy.EndDate
                CTRLlistPlain.IsInitialized = False
                CTRLlistTree.Visible = True
                CTRLlistTree.InitializeControl(CurrentListContext, filter, PageListType.ListAdministrator)
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
                    CTRLlistPlain.InitializeControl(CurrentListContext, filter, PageListType.ListAdministrator)
                End If
            Case ItemsGroupBy.Community
                CTRLlistPlain.IsInitialized = False
                CTRLlistTree.Visible = True
                If Not CTRLlistTree.IsInitialized OrElse (previous.FilterBy <> filter.FilterBy OrElse filter.ActivitiesStatus <> previous.ActivitiesStatus OrElse filter.ProjectsStatus <> previous.ProjectsStatus OrElse filter.GroupBy <> previous.GroupBy) Then
                    CTRLlistTree.InitializeControl(CurrentListContext, filter, PageListType.ListAdministrator)
                End If
            Case ItemsGroupBy.EndDate
                CTRLlistPlain.IsInitialized = False
                CTRLlistTree.Visible = True
                If Not CTRLlistTree.IsInitialized OrElse (previous.FilterBy <> filter.FilterBy OrElse filter.ActivitiesStatus <> previous.ActivitiesStatus OrElse filter.ProjectsStatus <> previous.ProjectsStatus OrElse filter.GroupBy <> previous.GroupBy) Then
                    CTRLlistTree.InitializeControl(CurrentListContext, filter, PageListType.ListAdministrator)
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
        End Select
    End Sub

    Private Sub CTRLlistPlain_AddDeletedStatus() Handles CTRLlistPlain.AddDeletedStatus
        CTRLtopControls.AddDeletedStatus(LastFilterSettings)
    End Sub

    Private Sub CTRLlistPlain_RemoveDeletedStatus() Handles CTRLlistPlain.RemoveDeletedStatus
        CTRLtopControls.RemoveDeletedStatus(LastFilterSettings.Clone)
    End Sub
    Private Sub CTRLlistPlain_GetCurrentFilter(ByRef filter As dtoItemsFilter) Handles CTRLlistPlain.GetCurrentFilter
        filter = LastFilterSettings
    End Sub
    Private Sub CTRLlistTree_AddDeletedStatus() Handles CTRLlistTree.AddDeletedStatus
        CTRLtopControls.AddDeletedStatus(LastFilterSettings)
    End Sub

    Private Sub CTRLlistTree_RemoveDeletedStatus() Handles CTRLlistTree.RemoveDeletedStatus
        CTRLtopControls.RemoveDeletedStatus(LastFilterSettings.Clone)
    End Sub
    Private Sub CTRLlistTree_GetCurrentFilter(ByRef filter As dtoItemsFilter) Handles CTRLlistTree.GetCurrentFilter
        filter = LastFilterSettings
    End Sub
#End Region

  
End Class