Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel

Public Class UC_TaskListPlain
    Inherits PMtasksListBase
    Implements IViewTasksListPlain

#Region "Context"
    Private _Presenter As TasksListPlainPresenter
    Private ReadOnly Property CurrentPresenter() As TasksListPlainPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TasksListPlainPresenter(Me.PageUtility.CurrentContext, Me)
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
            .setLiteral(LTtaskProjectName_t)
            .setLiteral(LTtaskStatus_t)
            .setLiteral(LTtaskDeadline_t)
            .setLiteral(LTtaskCompletion_t)
            .setLiteral(LTtaskMyCompletion_t)

            .setLinkButton(LNBtoggleTaskStatus, False, True)
            .setLinkButton(LNBtoggleProjectCommunity, False, True)

            .setLinkButton(LNBorderByTaskNameUp, False, True)
            .setLinkButton(LNBorderByTaskNameDown, False, True)
            .setLinkButton(LNBorderByProjectNameUp, False, True)
            .setLinkButton(LNBorderByProjectNameDown, False, True)
            .setLinkButton(LNBorderByTaskCompletionUp, False, True)
            .setLinkButton(LNBorderByTaskCompletionDown, False, True)
            .setLinkButton(LNBorderByTaskDeadlineUp, False, True)
            .setLinkButton(LNBorderByTaskDeadlineDown, False, True)
        End With
    End Sub

    Public Overrides Sub InitializeControl(context As dtoProjectContext, ByVal idContainerCommunity As Integer, filter As dtoItemsFilter, containerPage As PageContainerType, fromPage As PageListType, currentPage As PageListType)
        LNBtoggleProjectCommunity.Visible = (containerPage <> PageContainerType.ProjectDashboard)
        CurrentPresenter.InitView(context, idContainerCommunity, filter, containerPage, fromPage, currentPage, UnknownUser)
    End Sub
    Protected Overrides Function GetMyTasksCompletion() As List(Of dtoMyAssignmentCompletion)
        Dim items As New List(Of dtoMyAssignmentCompletion)

        For Each row As RepeaterItem In RPTtasks.Items
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
        Return items
    End Function

    Protected Overrides Sub SetPager(value As lm.Comol.Core.DomainModel.PagerBase)
        Me.PGgridBottom.Pager = value
        Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
    End Sub
    Protected Overrides Sub DisplayPager(display As Boolean)
        DVpagerBottom.Visible = display
    End Sub
    Protected Overrides Sub LoadedNoTasks()
        LoadTasks(New List(Of dtoPlainTask))
    End Sub
    Public Overrides Sub SaveMyCompletions()
        Me.CurrentPresenter.SaveMyCompletions(GetMyTasksCompletion())
    End Sub
    Protected Overrides Sub DisplayTasksCompletionSaved(items As List(Of dtoMyAssignmentCompletion), savedTasks As Integer, unsavedTasks As Integer, updateSummary As Boolean)
        UpdateTasks(items)
        RaiseEvent TasksCompletionSaved(savedTasks, unsavedTasks, items.Where(Function(i) i.MyCompletion.InEditMode OrElse i.MyCompletion.IsUpdated).Count, updateSummary)
    End Sub
#End Region

#Region "Implements"
    Private Sub LoadTasks(tasks As List(Of dtoPlainTask)) Implements IViewTasksListPlain.LoadTasks
        THmyCompleteness.Visible = False
        THproject.Visible = False
        Select Case PageType
            Case PageListType.DashboardResource
                TDfooter.ColSpan = 6
                THproject.Visible = True
                THmyCompleteness.Visible = True
            Case PageListType.ProjectDashboardResource
                TDfooter.ColSpan = 5
                THmyCompleteness.Visible = True
            Case PageListType.ProjectDashboardManager
                TDfooter.ColSpan = 4

            Case Else
                THproject.Visible = True
                TDfooter.ColSpan = 5
        End Select
        RPTtasks.DataSource = tasks
        RPTtasks.DataBind()
        TDfooter.Visible = tasks.Any()
        SPNcommands.Visible = (tasks.Count > 0)

        LNBorderByTaskNameUp.Visible = (tasks.Count > 1)
        LNBorderByTaskNameDown.Visible = (tasks.Count > 1)
        LNBorderByProjectNameUp.Visible = (tasks.Count > 1)
        LNBorderByProjectNameDown.Visible = (tasks.Count > 1)
        LNBorderByTaskCompletionUp.Visible = (tasks.Count > 1)
        LNBorderByTaskCompletionDown.Visible = (tasks.Count > 1)
        LNBorderByTaskDeadlineUp.Visible = (tasks.Count > 1)
        LNBorderByTaskDeadlineDown.Visible = (tasks.Count > 1)
        If tasks.Count > 0 AndAlso tasks.Where(Function(t) t.ProjectInfo.SetMyCompletion).Any() Then
            RaiseEvent AllowMyCompletionSave()
        End If

    End Sub
#End Region

#Region "Internal"
    Protected Friend Function GetTableCssClass() As String
        Select Case PageType
            Case PageListType.DashboardManager, PageListType.DashboardAdministrator, PageListType.ProjectDashboardManager
                Return LTcssClassManager.Text
            Case PageListType.DashboardResource, PageListType.ProjectDashboardResource
                Return LTcssClassResource.Text
        End Select
        Return ""
    End Function

    Private Sub RPTtasks_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTtasks.ItemCommand
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

    Private Sub RPTtasks_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtasks.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim task As dtoPlainTask = e.Item.DataItem
                Dim oHyperLink As HyperLink = e.Item.FindControl("HYPtaskName")

                oHyperLink.Text = task.Name

                Dim oDialog As UC_TaskInfo = e.Item.FindControl("CTRLcompletionDialog")
                oDialog.InitializeControl(task, False, PageType)

                oHyperLink = e.Item.FindControl("HYPprojectName")
                oHyperLink.NavigateUrl = PageUtility.ApplicationUrlBase & task.ProjectDashboard
                oHyperLink.Text = task.ProjectInfo.Name
                Dim oLabel As Label = e.Item.FindControl("LBcommunityName")
                Select Case task.ProjectInfo.IdCommunity
                    Case 0
                        oLabel.Text = Resource.getValue("ProjectContainer.Portal")
                    Case -1
                        oLabel.Text = Resource.getValue("ProjectContainer.UnknownCommunity")
                    Case Else
                        oLabel.Text = task.ProjectInfo.CommunityName
                End Select
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

                Select Case PageType
                    Case PageListType.ProjectDashboardManager, PageListType.ProjectDashboardResource
                        oTableCell = e.Item.FindControl("TDproject")
                        oTableCell.Visible = False
                End Select
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

                ElseIf PageType = PageListType.DashboardResource OrElse PageType = PageListType.ProjectDashboardResource Then
                    oTableCell = e.Item.FindControl("TDmyCompletenessEmpty")
                    oTableCell.Visible = True
                End If


            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTtasks.Items.Count = 0)
                If (RPTtasks.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    Select PageType
                        Case PageListType.DashboardResource
                            oTableCell.ColSpan = 6
                        Case PageListType.ProjectDashboardResource
                            oTableCell.ColSpan = 5
                        Case PageListType.ProjectDashboardManager
                            oTableCell.ColSpan = 4
                        Case Else
                            oTableCell.ColSpan = 5
                    End Select
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")

                    oLabel.Text = Resource.getValue("NoTasksForSelection")
                End If
        End Select
    End Sub

    Protected Friend Function RowCssClass(task As dtoPlainTask) As String
        If task.Deadline.HasValue Then
            Return LThasdeadline.Text
        Else
            Return LTnodeadline.Text
        End If
    End Function

    Protected Sub LNBorderBy_Click(sender As Object, e As System.EventArgs)
        CurrentAscending = CBool(DirectCast(sender, LinkButton).CommandArgument)
        Select Case DirectCast(sender, LinkButton).CommandName
            Case "taskname"
                CurrentOrderBy = ProjectOrderBy.TaskName
            Case "deadline"
                CurrentOrderBy = ProjectOrderBy.Deadline
            Case "completion"
                CurrentOrderBy = ProjectOrderBy.Completion
            Case "projectname"
                CurrentOrderBy = ProjectOrderBy.Name
        End Select
        Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageContainer, ItemsGroupBy.Plain)
        RaiseEvent GetCurrentFilter(filter)

        filter.Ascending = CurrentAscending
        filter.OrderBy = CurrentOrderBy
        filter.PageIndex = Me.PGgridBottom.Pager.PageIndex
        filter.PageSize = Me.PGgridBottom.Pager.PageSize
        CurrentPresenter.LoadTasks(PageContext, filter, PageContainer, CurrentFromPage, PageType, filter.PageIndex, filter.PageSize, UnknownUser)
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Dim filter As dtoItemsFilter = dtoItemsFilter.GenerateForGroup(PageContainerType.ProjectsList, ItemsGroupBy.Plain)
        RaiseEvent GetCurrentFilter(filter)

        filter.Ascending = CurrentAscending
        filter.OrderBy = CurrentOrderBy
        filter.PageIndex = Me.PGgridBottom.Pager.PageIndex
        filter.PageSize = Me.PGgridBottom.Pager.PageSize
        CurrentPresenter.LoadTasks(PageContext, filter, PageContainer, CurrentFromPage, PageType, filter.PageIndex, filter.PageSize, UnknownUser)
    End Sub

    Private Sub UpdateTasks(tasks As List(Of dtoMyAssignmentCompletion))
        For Each row As RepeaterItem In RPTtasks.Items
            Dim task As dtoMyAssignmentCompletion = tasks.Where(Function(t) t.IdAssignment = CLng(DirectCast(row.FindControl("LTidAssignment"), Literal).Text)).FirstOrDefault()

            Dim oCell As HtmlTableCell = row.FindControl("TDmyCompleteness")
            If Not IsNothing(task) Then
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
            Else
                oCell.Attributes("class") = LTtdmyCompleteness.Text
            End If
        Next
    End Sub
#End Region


   
End Class