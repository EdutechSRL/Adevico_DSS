Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class MapReorder
    Inherits PMbaseProjectMap
    Implements IViewMapReorder

#Region "Context"
    Private _Presenter As MapReorderPresenter
    Private ReadOnly Property CurrentPresenter() As MapReorderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MapReorderPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Settings"
    Private Property AllowSave As Boolean Implements IViewMapReorder.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            BTNsaveProjectReorderBottom.Visible = value
            BTNsaveProjectReorderTop.Visible = value
        End Set
    End Property
    Private ReadOnly Property PreviousStartDate As DateTime? Implements IViewMapReorder.PreviousStartDate
        Get
            Return CTRLprojectInfo.ProjectStartDate
        End Get
    End Property
    Private ReadOnly Property PreviousDeadline As DateTime? Implements IViewMapReorder.PreviousDeadline
        Get
            Return CTRLprojectInfo.ProjectDeadline
        End Get
    End Property
#End Region
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPprojectMap, False, True)
            .setHyperLink(HYPprojectMapBulkEdit, False, True)
            .setHyperLink(HYPprojectGantt, False, True)
            .setHyperLink(HYPprojectMapReorder, False, True)
            .setButton(BTNsaveProjectDateInfoTop, True)
            BTNsaveProjectDateInfoBottom.Text = BTNsaveProjectDateInfoTop.Text
            BTNsaveProjectDateInfoBottom.ToolTip = BTNsaveProjectDateInfoTop.ToolTip

            Master.ServiceTitle = .getValue("ServiceTitle.MapReorder")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.MapReorder.ToolTip")
            .setHyperLink(HYPgoToProjectEditTop, False, True)
            .setHyperLink(HYPbackToProjectsTop, False, True)
            .setHyperLink(HYPbackToResourceDashboardTop, False, True)
            .setHyperLink(HYPbackToManagerDashboardTop, False, True)
            .setHyperLink(HYPgoToProjectEditBottom, False, True)
            .setHyperLink(HYPbackToResourceDashboardBottom, False, True)
            .setHyperLink(HYPbackToManagerDashboardBottom, False, True)

            HYPbackToProjectsBottom.Text = HYPbackToProjectsTop.Text
            HYPbackToProjectsBottom.ToolTip = HYPbackToProjectsTop.ToolTip

            .setLiteral(LTreorderMessageDescription)
            .setLiteral(LTmapReorderIstructions)
            .setLabel(LBthStartDate)
            .setLabel(LBthEndDate)
            .setLabel(LBthName)
            .setLabel(LBthPredecessors)
            .setLabel(LBthDuration)
            .setButton(BTNsaveProjectReorderBottom)
            .setButton(BTNsaveProjectReorderTop)

            .setButton(BTNconfirmReorderAction)
            .setLinkButton(LNBcloseConfirmReorderAction, False, True)
            DVconfirmReorder.Attributes("Title") = Resource.getValue("DVconfirmReorder.Title")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
    Protected Overrides Sub SetProjectsUrl(url As String) Implements IViewBaseProjectMap.SetProjectsUrl
        If Not String.IsNullOrEmpty(url) Then
            HYPbackToProjectsTop.Visible = True
            HYPbackToProjectsBottom.Visible = True
            HYPbackToProjectsTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPbackToProjectsBottom.NavigateUrl = HYPbackToProjectsTop.NavigateUrl
        End If
    End Sub
    Protected Overrides Sub SetEditProjectUrl(url As String) Implements IViewBaseProjectMap.SetEditProjectUrl
        If Not String.IsNullOrEmpty(url) Then
            HYPgoToProjectEditTop.Visible = True
            HYPgoToProjectEditBottom.Visible = True
            HYPgoToProjectEditTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPgoToProjectEditBottom.NavigateUrl = HYPgoToProjectEditTop.NavigateUrl
        End If
    End Sub
    Protected Overrides Sub SetDashboardUrl(url As String, fromPage As PageListType)
        If Not String.IsNullOrEmpty(url) Then
            Select Case fromPage
                Case PageListType.DashboardAdministrator, PageListType.DashboardManager, PageListType.ProjectDashboardManager
                    HYPbackToManagerDashboardTop.Visible = True
                    HYPbackToManagerDashboardBottom.Visible = True
                    HYPbackToManagerDashboardTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
                    HYPbackToManagerDashboardBottom.NavigateUrl = HYPbackToManagerDashboardTop.NavigateUrl
                Case Else
                    HYPbackToResourceDashboardTop.Visible = True
                    HYPbackToResourceDashboardBottom.Visible = True
                    HYPbackToResourceDashboardTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
                    HYPbackToResourceDashboardBottom.NavigateUrl = HYPbackToResourceDashboardTop.NavigateUrl
            End Select
        End If
    End Sub

#Region "Display"
    Protected Overrides Sub DisplayUnknownProject()
        Me.MLVprojectMap.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownProject.ProjectMap")
    End Sub
    Protected Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(RootObject.MapReorder(IdProject, PreloadIdCommunity, FromPage, PreloadIdContainerCommunity), PreloadIdCommunity)
    End Sub
#End Region
    Protected Overrides Sub LoadProjectDateInfo(project As dtoProject, allowUpdate As Boolean)
        LTprojectName.Text = project.Name
        CTRLprojectInfo.InitializeControl(project, Resource.CultureInfo, Resource.CultureInfo.DateTimeFormat.ShortDatePattern, allowUpdate)

        HYPprojectMap.NavigateUrl = ApplicationUrlBase & RootObject.ProjectMap(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        ' HYPprojectMapBulkEdit.Visible = AllowSave
        HYPprojectMapReorder.NavigateUrl = ApplicationUrlBase & RootObject.MapReorder(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapReorder.Visible = True 'AllowSave
        HYPprojectGantt.NavigateUrl = ApplicationUrlBase & RootObject.Gantt(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectGantt.Visible = True

        HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)

        Me.BTNsaveProjectDateInfoBottom.Visible = allowUpdate
        Me.BTNsaveProjectDateInfoTop.Visible = allowUpdate
    End Sub
    Protected Overrides Sub LoadAttachments(attachments As List(Of dtoAttachmentItem))
        LBattachments.Visible = Not IsNothing(attachments) AndAlso attachments.Any()
        If Not IsNothing(attachments) AndAlso attachments.Any() Then
            Select Case attachments.Count
                Case 1, 0
                    LBattachments.ToolTip = Resource.getValue("LBattachments.ToolTip." & Items.Count.ToString)
                Case Else
                    LBattachments.ToolTip = String.Format(Resource.getValue("LBattachments.ToolTip.n"), Items.Count)
            End Select
            CTRLattachment.Visible = True
            CTRLattachment.InitializeControl(attachments)
        Else
            CTRLattachment.Visible = False
        End If
    End Sub
#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewMapReorder.DisplayNoPermission
        DVconfirmReorder.Visible = False
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleProjectManagement.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplayErrorSavingDates() Implements IViewMapReorder.DisplayErrorSavingDates
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewMapReorder.DisplayErrorSavingDates"), Helpers.MessageType.error)
    End Sub

    Private Sub DisplaySavedDates(startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?)) Implements IViewMapReorder.DisplaySavedDates
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewMapReorder.DisplaySavedDates"), Helpers.MessageType.success)
        CTRLprojectInfo.UpdateDate(startDate, endDate, deadLine)
    End Sub

    Private Sub DisplayConfirmActions(errorFound As ReorderError, Optional actions As List(Of dtoReorderAction) = Nothing) Implements IViewMapReorder.DisplayConfirmActions
        DVconfirmReorder.Visible = True
        If Not IsNothing(actions) Then
            RPTconfirmOptions.DataSource = actions
            RPTconfirmOptions.DataBind()
            RPTconfirmOptions.Visible = True
            BTNconfirmReorderAction.Visible = True
        Else
            RPTconfirmOptions.Visible = False
            BTNconfirmReorderAction.Visible = (errorFound = ReorderError.None)
        End If
        LTconfirmReorderAction.Text = Resource.getValue("LTconfirmReorderAction.ReorderError." & errorFound.ToString)
    End Sub

    Private Sub DisplayReorderCompleted() Implements IViewMapReorder.DisplayReorderCompleted
        DVconfirmReorder.Visible = False
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewMapReorder.DisplayReorderCompleted"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayNoActivitiesToReorder() Implements IViewMapReorder.DisplayNoActivitiesToReorder
        DVconfirmReorder.Visible = False
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewMapReorder.DisplayNoActivitiesToReorder"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnableToReorder() Implements IViewMapReorder.DisplayUnableToReorder
        DVconfirmReorder.Visible = False
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewMapReorder.DisplayUnableToReorder"), Helpers.MessageType.error)
    End Sub
#End Region
    Private Function GetDefaultAction() As ReorderAction Implements IViewMapReorder.GetDefaultAction
        If RBLreorderOptions.SelectedIndex > -1 Then
            Return CInt(RBLreorderOptions.SelectedValue)
        Else
            Return ReorderAction.AskAlways
        End If
    End Function
    Private Sub LoadAvailableActions(actions As List(Of ReorderAction), dAction As ReorderAction, allowReorder As Boolean) Implements IViewMapReorder.LoadAvailableActions
        LTreorderScript.Visible = allowReorder
        If actions.Any() AndAlso dAction <> ReorderAction.Ignore Then
            DVreorderMessage.Visible = True
            RBLreorderOptions.DataSource = (From e In actions Select New TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("ReorderAction." & e.ToString)}).ToList()
            RBLreorderOptions.DataTextField = "Translation"
            RBLreorderOptions.DataValueField = "Id"
            RBLreorderOptions.DataBind()
            If actions.Contains(dAction) Then
                RBLreorderOptions.SelectedValue = dAction
            Else
                RBLreorderOptions.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub LoadActivities(tasks As List(Of dtoActivityTreeItem)) Implements IViewMapReorder.LoadActivities
        DVsortableTree.Visible = True
        DVsortableTreeHeader.Visible = True
        If IsNothing(tasks) Then
            tasks = New List(Of dtoActivityTreeItem)
        End If
        Me.RBLreorderOptions.Enabled = Not IsNothing(tasks)

        RPTprojectTree.DataSource = tasks
        RPTprojectTree.DataBind()
        If tasks.Any() Then
            DVcommandsBottom.Visible = (tasks.Last.GetMaxDisplayOrder() > 15)
            BTNsaveProjectReorderBottom.Enabled = True
            BTNsaveProjectReorderTop.Enabled = True
        Else
            DVcommandsBottom.Visible = False
            BTNsaveProjectReorderBottom.Enabled = False
            BTNsaveProjectReorderTop.Enabled = False
        End If
    End Sub
#End Region

#Region "Internal"
    Private Sub RPTprojectTree_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTprojectTree.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim oControl As UC_ProjectTreeItem = e.Item.FindControl("CTRLchild")
                oControl.InitializeControl(e.Item.DataItem, CurrentShortDatePattern)
        End Select
    End Sub

    Private Sub RBLreorderOptions_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLreorderOptions.SelectedIndexChanged
        LTreorderScript.Visible = Not (GetDefaultAction() = ReorderAction.DisableReorder)
    End Sub
    Private Sub BTNsaveProjectDateInfoTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveProjectDateInfoTop.Click, BTNsaveProjectDateInfoBottom.Click
        CurrentPresenter.SaveProjectDate(IdProject, CTRLprojectInfo.InEditStartDate, CTRLprojectInfo.InEditDeadline)
    End Sub

    Private Sub BTNsaveProjectReorderBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveProjectReorderBottom.Click, BTNsaveProjectReorderTop.Click
        DVconfirmReorder.Visible = False
        CTRLmessages.Visible = False
        Dim action As ReorderAction = ReorderAction.Ignore

        CurrentPresenter.ApplyReorder(GetActivitiesReordered(), GetDefaultAction)
    End Sub
    Private Function GetActivitiesReordered() As List(Of dtoReorderGraphActivity)
        Dim items As New List(Of dtoReorderGraphActivity)
        Dim serializedItems As List(Of String) = HDMserializeTasks.Value.Split(";").Where(Function(v) Not String.IsNullOrEmpty(v)).ToList()

        Dim dOrder As Integer = 1
        For Each serialized As String In serializedItems
            Dim values As String() = serialized.Split(":")
            Dim activity As New dtoReorderGraphActivity
            activity.IdActivity = System.Convert.ToInt64(values(0).Replace("srt-", ""))
            If values(1) = "0" Then
                activity.IdParent = 0
            Else
                activity.IdParent = System.Convert.ToInt64(values(1).Replace("srt-", ""))
            End If
            activity.DisplayOrder = dOrder
            dOrder += 1
            items.Add(activity)
        Next
        Return items
    End Function

    Private Sub RPTconfirmOptions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTconfirmOptions.ItemDataBound
        Dim dto As dtoReorderAction = e.Item.DataItem
        Dim oLiteral As Literal = e.Item.FindControl("LTconfirmOption")
        Dim oLabel As Label = e.Item.FindControl("LBconfirmOptionDescription")

        oLiteral.Text = Resource.getValue("LTconfirmOption.ReorderAction." & dto.Action.ToString)
        oLabel.Text = Resource.getValue("LBconfirmOptionDescription.ReorderAction." & dto.Action.ToString)

    End Sub
    Protected Function IsSelected(item As dtoReorderAction) As String
        Return IIf(item.Selected, "checked", "")
    End Function
    Private Sub BTNconfirmReorderAction_Click(sender As Object, e As System.EventArgs) Handles BTNconfirmReorderAction.Click

        CTRLmessages.Visible = False
        If RPTconfirmOptions.Visible Then
            Dim action As ReorderAction = ReorderAction.Ignore
            Try
                action = CInt(Request("RDconfirmAction"))
            Catch ex As Exception

            End Try
            CurrentPresenter.ConfirmReorder(GetActivitiesReordered(), action)
        Else
            CurrentPresenter.ConfirmReorder(GetActivitiesReordered(), ReorderAction.Ignore)
        End If
        DVconfirmReorder.Visible = False
    End Sub
    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub
#End Region

    Protected Overrides Sub SetEditMapUrl(url As String)

    End Sub

    
End Class