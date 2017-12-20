Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Public Class UC_TaskListMultipleTree
    Inherits PMtasksListBase
    Implements IViewTasksListMultipleTree

#Region "Context"
    Private _Presenter As TasksListMultipleTreePresenter
    Private ReadOnly Property CurrentPresenter() As TasksListMultipleTreePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TasksListMultipleTreePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event GetCurrentFilter(ByRef filter As dtoItemsFilter)
    Public Event CompletionSaved(ByVal saved As Boolean, ByVal idTask As Long, ByVal taskname As String, updateSummary As Boolean)
    Public Event AllowMyCompletionSave()
    Public Event TasksCompletionSaved(ByVal savedTasks As Integer, ByVal unsavedTasks As Integer, ByVal tasks As Integer, updateSummary As Boolean)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTpageBottom)
            .setLiteral(LTtaskName_t)
            .setLiteral(LTtaskStatus_t)
            .setLiteral(LTtaskDeadline_t)
            .setLiteral(LTtaskCompletion_t)
            .setLiteral(LTtaskMyCompletion_t)

            .setLinkButton(LNBtoggleTaskStatus, False, True)
        End With
    End Sub

    Public Overrides Sub InitializeControl(context As dtoProjectContext, ByVal idContainerCommunity As Integer, filter As dtoItemsFilter, containerPage As PageContainerType, fromPage As PageListType, currentPage As PageListType)
        CurrentPresenter.InitView(context, idContainerCommunity, filter, containerPage, fromPage, currentPage, UnknownUser)
    End Sub


    Protected Overrides Sub SetPager(value As lm.Comol.Core.DomainModel.PagerBase)
        Me.PGgridBottom.Pager = value
        Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
    End Sub
    Protected Overrides Sub DisplayPager(display As Boolean)
        DVpagerBottom.Visible = display
    End Sub
    Protected Overrides Sub LoadedNoTasks()
        LoadTasks(New List(Of dtoCommunityProjectTasksGroup))
    End Sub
    Protected Overrides Function GetMyTasksCompletion() As List(Of dtoMyAssignmentCompletion)
        Dim items As New List(Of dtoMyAssignmentCompletion)

        For Each rCommunity As RepeaterItem In RPTcommunities.Items
            Dim cRepeater As Repeater = rCommunity.FindControl("RPTcontainer")
            For Each rContainer As RepeaterItem In cRepeater.Items
                Dim tRepeater As Repeater = rContainer.FindControl("RPTtasks")
                For Each row As RepeaterItem In tRepeater.Items
                    Dim item As New dtoMyAssignmentCompletion()

                    Dim oControl As UC_InLineInput = row.FindControl("CTRLmyCompleteness")
                    If Not (String.IsNullOrEmpty(oControl.NewValue)) Then
                        item.MyCompletion.Current = oControl.NewValue
                    End If
                    item.MyCompletion.Init = oControl.OldValue
                    item.MyCompletion.InEditMode = (oControl.NewValue <> oControl.OldValue)
                    item.IdTask = CLng(DirectCast(row.FindControl("LTidTask"), Literal).Text)
                    item.IdAssignment = CLng(DirectCast(row.FindControl("LTidAssignment"), Literal).Text)

                    items.Add(item)
                Next
            Next
        Next
        Return items
    End Function
    Public Overrides Sub SaveMyCompletions()
        Me.CurrentPresenter.SaveMyCompletions(GetMyTasksCompletion())
    End Sub
    Protected Overrides Sub DisplayTasksCompletionSaved(items As List(Of dtoMyAssignmentCompletion), savedTasks As Integer, unsavedTasks As Integer, updateSummary As Boolean)
        UpdateTasks(items)
        RaiseEvent TasksCompletionSaved(savedTasks, unsavedTasks, items.Where(Function(i) i.MyCompletion.InEditMode OrElse i.MyCompletion.IsUpdated).Count, updateSummary)
    End Sub
#End Region

#Region "Implements"
    Private Sub LoadTasks(items As List(Of dtoCommunityProjectTasksGroup)) Implements IViewTasksListMultipleTree.LoadTasks
        THmyCompleteness.Visible = (PageType = PageListType.DashboardResource)
        THactions.Visible = (PageType = PageListType.DashboardManager OrElse PageType = PageListType.DashboardAdministrator)

        TDfooter.ColSpan = GetCellsCount(PageType)
        TDfooter.Visible = items.Any()
        SPNcommands.Visible = (items.Count > 0)


        RPTcommunities.DataSource = items
        RPTcommunities.DataBind()

        If items.Count > 0 AndAlso items.Where(Function(t) t.Projects.Where(Function(p) p.ProjectInfo.SetMyCompletion).Any()).Any Then
            RaiseEvent AllowMyCompletionSave()
        End If
    End Sub
    Private Function GetCellsCount(ByVal cPage As PageListType) As Integer Implements IViewTasksListMultipleTree.GetCellsCount
        'If cPage = PageListType.DashboardResource Then
        '    Return 5
        'Else
        Return 5
        'End If
    End Function
    Private Function GetContainerCssClass(ByVal gType As ItemsGroupBy) As String Implements IViewTasksListMultipleTree.GetContainerCssClass
        Select Case gType
            Case ItemsGroupBy.Community
                Return LTitemCssClassCommunity.Text
            Case ItemsGroupBy.EndDate
                Return LTitemCssClassDeadline.Text
            Case Else
                Return ""
        End Select
    End Function
    Private Function GetPortalName() As String Implements IViewTasksListMultipleTree.GetPortalName
        Return Resource.getValue("ProjectContainer.Portal")
    End Function
    Private Function GetUnknownCommunityName() As String Implements IViewTasksListMultipleTree.GetUnknownCommunityName
        Return Resource.getValue("ProjectContainer.UnknownCommunity")
    End Function
    Private Function GetStartRowId(gType As ItemsGroupBy) As String Implements IViewTasksListMultipleTree.GetStartRowId
        Return gType.ToString.ToLower() & "-"
    End Function
    Private Overloads Sub DisplayTasksCompletionSaved(items As List(Of dtoMyAssignmentCompletion), projectCompletions As Dictionary(Of Long, Dictionary(Of ResourceActivityStatus, Long)), savedTasks As Integer, unsavedTasks As Integer, updateSummary As Boolean) Implements IViewTasksListMultipleTree.DisplayTasksCompletionSaved
        UpdateTasks(items, projectCompletions)
        RaiseEvent TasksCompletionSaved(savedTasks, unsavedTasks, items.Where(Function(i) i.MyCompletion.InEditMode OrElse i.MyCompletion.IsUpdated).Count, updateSummary)
    End Sub
#End Region

#Region "Internal"
    Protected Friend Function GetTableContainerCssClass() As String
        Select Case PageType
            Case PageListType.DashboardManager, PageListType.DashboardAdministrator
                Return LTcssClassManager.Text
            Case PageListType.DashboardResource
                Return LTcssClassResource.Text
        End Select
        Return ""
    End Function
    Protected Friend Function GetTableCssClass(ByVal gType As ItemsGroupBy) As String
        Select Case gType
            Case ItemsGroupBy.Community
                Return " " & LTcssClassByCommunity.Text
            Case ItemsGroupBy.EndDate
                Return " " & LTcssClassByDeadline.Text
            Case ItemsGroupBy.Project
                Return " " & LTcssClassByProject.Text
        End Select
        Return ""
    End Function

    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoCommunityProjectTasksGroup = e.Item.DataItem
                Dim oTableRow As HtmlTableRow = e.Item.FindControl("TRpreviousPage")
                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBcontinueFromPreviousRow")
                oTableRow.Visible = (dto.PreviousPageIndex > -1)
                If dto.PreviousPageIndex > -1 Then
                    Resource.setLinkButton(oLinkButton, False, True)
                    oLinkButton.CommandArgument = dto.PreviousPageIndex
                    oTableRow.Attributes("class") = LTcssClassContinueTop.Text & dto.IdRow
                End If

                oTableRow = e.Item.FindControl("TRnextPage")
                oTableRow.Visible = (dto.NextPageIndex > -1)
                oLinkButton = e.Item.FindControl("LNBcontinueToNextRow")
                If dto.NextPageIndex > -1 Then
                    Resource.setLinkButton(oLinkButton, False, True)
                    oLinkButton.CommandArgument = dto.NextPageIndex
                    oTableRow.Attributes("class") = LTcssClassContinueBottom.Text & dto.IdRow
                End If

                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDcommunityName")
                oTableCell.ColSpan = dto.CellsCount

            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTcommunities.Items.Count = 0)
                If (RPTcommunities.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    oTableCell.ColSpan = GetCellsCount(PageType)
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")

                    oLabel.Text = Resource.getValue("NoTasksForSelection")
                End If
        End Select
    End Sub
    Private Sub RPTcommunities_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcommunities.ItemCommand
        Select Case e.CommandName
            Case "gotopage"
                Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageType, ItemsGroupBy.CommunityProject)
                RaiseEvent GetCurrentFilter(filter)

                filter.Ascending = CurrentAscending
                filter.OrderBy = CurrentOrderBy
                filter.PageIndex = CInt(e.CommandArgument)
                filter.PageSize = Me.PGgridBottom.Pager.PageSize
                CurrentPresenter.LoadTasks(UnknownUser, PageContext, IdContainerCommunity, filter, PageContainer, CurrentFromPage, PageType, filter.PageIndex, filter.PageSize)
        End Select
    End Sub

    Protected Sub RPTcontainer_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoTasksGroup = e.Item.DataItem
                Dim oTableRow As HtmlTableRow = e.Item.FindControl("TRpreviousPage")
                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBcontinueFromPreviousRow")
                oTableRow.Visible = (dto.PreviousPageIndex > -1)
                If dto.PreviousPageIndex > -1 Then
                    Resource.setLinkButton(oLinkButton, False, True)
                    oLinkButton.CommandArgument = dto.PreviousPageIndex
                    oTableRow.Attributes("class") = LTcssClassContinueTop.Text & dto.IdRow
                End If

                oTableRow = e.Item.FindControl("TRnextPage")
                oTableRow.Visible = (dto.NextPageIndex > -1)
                oLinkButton = e.Item.FindControl("LNBcontinueToNextRow")
                If dto.NextPageIndex > -1 Then
                    Resource.setLinkButton(oLinkButton, False, True)
                    oLinkButton.CommandArgument = dto.NextPageIndex
                    oTableRow.Attributes("class") = LTcssClassContinueBottom.Text & dto.IdRow
                End If


                Dim oHyperLink As HyperLink = e.Item.FindControl("HYPprojectName")
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectDashboard
                oHyperLink.Text = dto.Name

                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDprojectStatusCompleteness")
                oTableCell.Visible = True
                Dim oLabel As Label = e.Item.FindControl("LBprojectStatusCompleteness")
                oLabel.ToolTip = dto.ProjectInfo.Completeness & "%"
                oLabel.Text = String.Format(LTstatusContent.Text, dto.ProjectInfo.Completeness)
                oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(dto.ProjectInfo.Completeness, dto.ProjectInfo.IsCompleted)


                oTableCell = e.Item.FindControl("TDprojectStatus")
                oTableCell.Visible = True
                oLabel = e.Item.FindControl("LBprojectStatus")
                oLabel.Text = Resource.getValue("Project.List.ProjectItemStatus." & dto.ProjectInfo.Status.ToString)

                oTableCell = e.Item.FindControl("TDprojectDeadline")
                oTableCell.Visible = True
                oLabel = e.Item.FindControl("LBdeadline")
                If dto.ProjectInfo.Deadline.HasValue Then
                    oLabel.Text = dto.ProjectInfo.Deadline.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                ElseIf dto.ProjectInfo.EndDate.HasValue Then
                    oLabel.Text = dto.ProjectInfo.VirtualEndDate.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                Else
                    oLabel.Text = "//"
                End If

                oTableCell = e.Item.FindControl("TDprojectMyCompleteness")
                oTableCell.Visible = dto.ProjectInfo.SetMyCompletion OrElse dto.ProjectInfo.ViewMyCompletion
                If dto.ProjectInfo.SetMyCompletion OrElse dto.ProjectInfo.ViewMyCompletion Then
                    Dim oProgressControl As UC_AdvancedProgressBar = e.Item.FindControl("CTRLmyCompletion")
                    oProgressControl.Visible = True
                    oProgressControl.InitializeControl(GenerateProgressBar(dto.ProjectInfo.UserCompletion))
                ElseIf PageType = PageListType.DashboardResource Then
                    oTableCell = e.Item.FindControl("TDmyCompletenessEmpty")
                    oTableCell.Visible = True
                End If
                oTableCell = e.Item.FindControl("TDprojectActions")
                oTableCell.Visible = (PageType <> PageListType.DashboardResource)
                If (PageType <> PageListType.DashboardResource) Then
                    oHyperLink = e.Item.FindControl("HYPeditProjectMap")
                    Resource.setHyperLink(oHyperLink, False, True)
                    oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectUrls.ProjectMap
                    oHyperLink.Visible = dto.HasPermission(PmActivityPermission.ManageProject)

                    oHyperLink = e.Item.FindControl("HYPviewProjectMap")
                    Resource.setHyperLink(oHyperLink, False, True)
                    oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectUrls.ProjectMap
                    oHyperLink.Visible = dto.HasPermission(PmActivityPermission.ViewProjectMap) AndAlso Not dto.HasPermission(PmActivityPermission.ManageProject)


                    oHyperLink = e.Item.FindControl("HYPeditProject")
                    Resource.setHyperLink(oHyperLink, False, True)
                    oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & dto.ProjectInfo.ProjectUrls.Edit
                    oHyperLink.Visible = dto.HasPermission(PmActivityPermission.ManageProject)
                End If

        End Select
    End Sub
    Protected Sub RPTcontainer_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Select Case e.CommandName
            Case "gotopage"
                Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageType, ItemsGroupBy.CommunityProject)
                RaiseEvent GetCurrentFilter(filter)

                filter.Ascending = CurrentAscending
                filter.OrderBy = CurrentOrderBy
                filter.PageIndex = CInt(e.CommandArgument)
                filter.PageSize = Me.PGgridBottom.Pager.PageSize
                CurrentPresenter.LoadTasks(UnknownUser, PageContext, IdContainerCommunity, filter, PageContainer, CurrentFromPage, PageType, filter.PageIndex, filter.PageSize)
        End Select
    End Sub
    Protected Sub RPTtasks_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim results As List(Of String) = e.CommandArgument.ToString().Split(",").ToList()

        If Boolean.TrueString = results(0) Then
            Dim oDialog As UC_TaskInfo = e.Item.FindControl("CTRLcompletionDialog")
            Dim oCell As HtmlTableCell = e.Item.FindControl("TDmyCompleteness")
            Dim oControl As UC_InLineInput = e.Item.FindControl("CTRLmyCompleteness")
            oControl.AutoInitialize(oDialog.GetMyCompletion)
            oCell.Attributes("class") = LTtdmyCompleteness.Text & " " & FieldStatus.updated.ToString

            Dim oLabel As Label = e.Item.FindControl("LBtaskStatus")
            oLabel.Text = oDialog.GetTaskStatus

            oLabel = e.Item.FindControl("LBtaskStatusCompleteness")
            oLabel.ToolTip = oDialog.GetTaskCompletion & "%"
            oLabel.Text = String.Format(LTstatusContent.Text, oDialog.GetTaskCompletion)
            oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(oDialog.GetTaskCompletion, oDialog.GetTaskIsCompleted)
        End If
        RaiseEvent CompletionSaved(Boolean.TrueString = results(0), CLng(DirectCast(e.Item.FindControl("LTidTask"), Literal).Text), DirectCast(e.Item.FindControl("HYPtaskName"), HyperLink).Text, Boolean.TrueString = results(1))
    End Sub
    Protected Sub RPTtasks_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim task As dtoPlainTask = e.Item.DataItem
                Dim oHyperLink As HyperLink = e.Item.FindControl("HYPtaskName")
                oHyperLink.Text = task.Name

                Dim oDialog As UC_TaskInfo = e.Item.FindControl("CTRLcompletionDialog")
                oDialog.InitializeControl(task, False, PageType)

                Dim oLabel As Label = Nothing

                oLabel = e.Item.FindControl("LBtaskStatusCompleteness")
                oLabel.ToolTip = task.Completeness & "%"
                oLabel.Text = String.Format(LTstatusContent.Text, task.Completeness)
                oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(task.Completeness, task.IsCompleted)

                oLabel = e.Item.FindControl("LBtaskStatus")

                oLabel.Text = Resource.getValue("Activity.ProjectItemStatus." & task.Status.ToString)

                oLabel = e.Item.FindControl("LBdeadline")
                If task.Deadline.HasValue Then
                    oLabel.Text = task.Deadline.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                ElseIf task.EndDate.HasValue Then
                    oLabel.Text = task.VirtualEndDate.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern)
                Else
                    oLabel.Text = "//"
                End If

                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDmyCompleteness")
                oTableCell.Visible = task.ProjectInfo.SetMyCompletion OrElse task.ProjectInfo.ViewMyCompletion

                If task.ProjectInfo.SetMyCompletion OrElse task.ProjectInfo.ViewMyCompletion Then
                    Dim oInput As UC_InLineInput = e.Item.FindControl("CTRLmyCompleteness")
                    oInput.Visible = True
                    If task.ProjectInfo.SetMyCompletion AndAlso task.MyCompleteness.InEditMode Then
                        oInput.AutoInitialize(task.MyCompleteness.Init, task.MyCompleteness.Edit)
                    ElseIf task.ProjectInfo.SetMyCompletion Then
                        oInput.AutoInitialize(task.MyCompleteness.GetValue & IIf(task.MyCompleteness.GetValue.EndsWith("%"), "", "%"))
                    Else
                        oInput.ContainerCssClass &= " disabled"
                        oInput.AutoInitialize(task.MyCompleteness.GetValue & IIf(task.MyCompleteness.GetValue.EndsWith("%"), "", "%"))
                    End If
                End If

                oTableCell = e.Item.FindControl("TDactions")
                oTableCell.Visible = Not (PageType = PageListType.DashboardResource)

        End Select
    End Sub

    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageType, ItemsGroupBy.CommunityProject)
        RaiseEvent GetCurrentFilter(filter)

        filter.Ascending = CurrentAscending
        filter.OrderBy = CurrentOrderBy
        filter.PageIndex = Me.PGgridBottom.Pager.PageIndex
        filter.PageSize = Me.PGgridBottom.Pager.PageSize
        CurrentPresenter.LoadTasks(UnknownUser, PageContext, IdContainerCommunity, filter, PageContainer, CurrentFromPage, PageType, filter.PageIndex, filter.PageSize)
    End Sub
    Private Sub UpdateTasks(tasks As List(Of dtoMyAssignmentCompletion), Optional projectCompletions As Dictionary(Of Long, Dictionary(Of ResourceActivityStatus, Long)) = Nothing)
        For Each rCommunity As RepeaterItem In RPTcommunities.Items
            Dim cRepeater As Repeater = rCommunity.FindControl("RPTcontainer")

            If Not IsNothing(projectCompletions) Then
                Dim oProgressControl As UC_AdvancedProgressBar = cRepeater.FindControl("CTRLmyCompletion")
                Dim idProject As Long = CLng(DirectCast(cRepeater.FindControl("LTid"), Literal).Text)
                If oProgressControl.Visible AndAlso projectCompletions.ContainsKey(idProject) Then
                    oProgressControl.InitializeControl(GenerateProgressBar(projectCompletions(idProject)))
                End If
            End If
            For Each rContainer As RepeaterItem In cRepeater.Items
                Dim tRepeater As Repeater = rContainer.FindControl("RPTtasks")
                For Each row As RepeaterItem In tRepeater.Items
                    Dim task As dtoMyAssignmentCompletion = tasks.Where(Function(t) t.IdAssignment = CLng(DirectCast(row.FindControl("LTidAssignment"), Literal).Text)).FirstOrDefault()

                    Dim oCell As HtmlTableCell = row.FindControl("TDmyCompleteness")
                    Dim oControl As UC_InLineInput = row.FindControl("CTRLmyCompleteness")
                    If task.MyCompletion.IsUpdated OrElse task.MyCompletion.InEditMode = False Then
                        oControl.AutoInitialize(task.MyCompletion.GetValue & IIf(task.MyCompletion.GetValue.EndsWith("%"), "", "%"))
                        Dim oDialog As UC_TaskInfo = row.FindControl("CTRLcompletionDialog")
                        oDialog.UpdateCompletion(task.MyCompletion.GetValue & IIf(task.MyCompletion.GetValue.EndsWith("%"), "", "%"), task.TaskCompletion, task.TaskStatus)
                        If task.MyCompletion.IsUpdated Then
                            oCell.Attributes("class") = LTtdmyCompleteness.Text & " " & task.MyCompletion.Status.ToString
                            Dim oLabel As Label = row.FindControl("LBtaskStatus")
                            oLabel.Text = Resource.getValue("Activity.ProjectItemStatus." & task.TaskStatus.ToString)

                            oLabel = row.FindControl("LBtaskStatusCompleteness")
                            oLabel.ToolTip = task.TaskCompletion & "%"
                            oLabel.Text = String.Format(LTstatusContent.Text, task.TaskCompletion)
                            oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(task.TaskCompletion, task.TaskIsCompleted)
                        Else
                            oCell.Attributes("class") = LTtdmyCompleteness.Text
                        End If
                    Else
                        oCell.Attributes("class") = LTtdmyCompleteness.Text & " " & task.MyCompletion.Status.ToString
                    End If
                Next
            Next
        Next
    End Sub
#End Region

End Class