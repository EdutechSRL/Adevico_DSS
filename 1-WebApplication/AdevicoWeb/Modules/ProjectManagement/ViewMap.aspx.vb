Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ViewMap
    Inherits PMbaseProjectMap
    Implements IViewViewProjectMap

#Region "Context"
    Private _Presenter As ViewProjectMapPresenter
    Private ReadOnly Property CurrentPresenter() As ViewProjectMapPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewProjectMapPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Settings"
    'Private Property ProjectResources As List(Of dtoResource) Implements IViewViewProjectMap.ProjectResources
    '    Get
    '        Return ViewStateOrDefault("ProjectResources", New List(Of dtoResource))
    '    End Get
    '    Set(value As List(Of dtoResource))
    '        ViewState("ProjectResources") = value
    '    End Set
    'End Property
    Private ReadOnly Property PRendDate As DateTime? ' Implements IViewViewProjectMap.PRendDate
        Get
            Return CTRLprojectInfo.ProjectEndDate
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
            .setHyperLink(HYPbackToProjectsBottom, True, True)
            .setHyperLink(HYPbackToProjectsTop, True, True)

            Master.ServiceTitle = .getValue("ServiceTitle.ProjectMap")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ProjectMap.ToolTip")

            .setLinkButton(LNBshowResourcesColumn, False, True)
            .setLinkButton(LNBhideResourcesColumn, False, True)
            .setLinkButton(LNBshowDateColumns, False, True)
            .setLinkButton(LNBhideDateColumns, False, True)
            .setLinkButton(LNBshowPredecessorsColumn, False, True)
            .setLinkButton(LNBhidePredecessorsColumn, False, True)


            .setHyperLink(HYPprojectMap, False, True)
            .setLabel(LBthDisplayOrder)
            .setLabel(LBthName)
            .setLabel(LBthStatus)
            .setLabel(LBthDuration)
            .setLabel(LBthPredecessors)
            .setLabel(LBthPredecessorsLegend)
            .setLabel(LBthStartDate)
            .setLabel(LBthEndDate)
            .setLabel(LBthResources)

            .setLinkButton(LNBexpandNodes, False, True)
            .setLinkButton(LNBcompressNodes, False, True)
            .setLinkButton(LNBwideview, False, True)
            .setLinkButton(LNBnarrowview, False, True)
            .setLinkButton(LNBtoggleresources, False, True)
            .setLinkButton(LNBrestoreview, False, True)

            .setHyperLink(HYPprojectGantt, False, True)

            .setHyperLink(HYPgoToProjectEditTop, False, True)
            .setHyperLink(HYPbackToResourceDashboardTop, False, True)
            .setHyperLink(HYPbackToManagerDashboardTop, False, True)
            .setHyperLink(HYPgoToProjectEditBottom, False, True)
            .setHyperLink(HYPbackToResourceDashboardBottom, False, True)
            .setHyperLink(HYPbackToManagerDashboardBottom, False, True)

            .setHyperLink(HYPgoToProjectMapTop, False, True)
            .setHyperLink(HYPgoToProjectMapBottom, False, True)

        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#Region "Display"
    Protected Overrides Sub DisplayUnknownProject()
        Me.MLVprojectMap.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownProject.ProjectMap")
    End Sub
    Protected Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(RootObject.ViewProjectMap(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity), PreloadIdCommunity)
    End Sub
#End Region

    Protected Overrides Sub LoadProjectDateInfo(project As dtoProject, allowEdit As Boolean)
        LoaderCultureInfo = Resource.CultureInfo
        LTprojectName.Text = project.Name

        CTRLprojectInfo.InitializeControl(project, Resource.CultureInfo, Resource.CultureInfo.DateTimeFormat.ShortDatePattern, True)

        DVpredecessorsCommand.Visible = project.DateCalculationByCpm
        CTRLpredecessorsHelper.Visible = project.DateCalculationByCpm
        LBthStatus.ToolTip = project.Completeness & "%"
        LBthStatus.Text = String.Format(LTstatusContent.Text, project.Completeness & "%")
        LBthStatus.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(project.Completeness)
        HYPprojectMap.NavigateUrl = ApplicationUrlBase & RootObject.ViewProjectMap(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectGantt.NavigateUrl = ApplicationUrlBase & RootObject.Gantt(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectGantt.Visible = True

        THpredecessors.Visible = project.DateCalculationByCpm
        TDfooterToolBar.ColSpan = IIf(project.DateCalculationByCpm, CInt(LTfullColSpan.Text), CInt(LTnocpmColSpan.Text))
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
    Private Sub DisplayNoActivities() Implements IViewViewProjectMap.DisplayNoActivities
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewViewProjectMap.DisplayNoActivities"), Helpers.MessageType.alert)
    End Sub
#End Region

#Region "Project Activities"
    Private Sub LoadActivities(activities As List(Of dtoMapActivity)) Implements IViewViewProjectMap.LoadActivities
        RPTactivities.DataSource = activities
        RPTactivities.DataBind()
        CTRLlegend.Visible = True


        DVtableCommands.Visible = activities.Any()
        DVcommandsBottom.Visible = activities.Any() AndAlso activities.Count > 15
        LNBexpandNodes.Visible = activities.Where(Function(a) a.IsSummary).Any()
        LNBcompressNodes.Visible = activities.Where(Function(a) a.IsSummary).Any()
    End Sub

#End Region
    Protected Overrides Sub SetProjectsUrl(url As String)
        If Not String.IsNullOrEmpty(url) Then
            HYPbackToProjectsTop.Visible = True
            HYPbackToProjectsBottom.Visible = True
            HYPbackToProjectsTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPbackToProjectsBottom.NavigateUrl = HYPbackToProjectsTop.NavigateUrl
        End If
    End Sub
    Protected Overrides Sub SetEditProjectUrl(url As String)
        If Not String.IsNullOrEmpty(url) Then
            HYPgoToProjectEditTop.Visible = True
            HYPgoToProjectEditBottom.Visible = True
            HYPgoToProjectEditTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPgoToProjectEditBottom.NavigateUrl = HYPgoToProjectEditTop.NavigateUrl
        End If
    End Sub
    Protected Overrides Sub SetEditMapUrl(url As String)
        If Not String.IsNullOrEmpty(url) Then
            HYPgoToProjectMapTop.Visible = True
            HYPgoToProjectMapBottom.Visible = True
            HYPgoToProjectMapTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPgoToProjectMapBottom.NavigateUrl = HYPgoToProjectEditTop.NavigateUrl
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
#End Region

#Region "Internal"
    Private Sub RPTactivities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactivities.ItemDataBound
        Dim dto As dtoMapActivity = DirectCast(e.Item.DataItem, dtoMapActivity)

        Dim oControl As UC_ActivityInLineMenu = e.Item.FindControl("CTRLinlineMenu")
        oControl.InitializeControl(dto.IdActivity, dto.IsSummary, dto.Permission)
        oControl.Visible = False
        Dim oLabel As Label = e.Item.FindControl("LBerror")
        oLabel.Visible = dto.Status.HasFlag(FieldStatus.error)
        If dto.Status.HasFlag(FieldStatus.error) Then

        End If
        Dim oControlInput As UC_InLineInput = e.Item.FindControl("CTRLnameInput")
        oControlInput.AutoInitialize(dto.Name.GetValue)

        oLabel = e.Item.FindControl("LBactivityStatus")
        oLabel.ToolTip = dto.Completeness & "%"
        oLabel.Text = String.Format(LTstatusContent.Text, dto.Completeness & "%")
        oLabel.CssClass = LTstatuslight.Text & " " & GetCssStatuslight(dto.Completeness, dto.IsCompleted)
        oControlInput = e.Item.FindControl("CTRLdurationInput")
        '   oControlInput.ContainerCssClass &= " disabled"

        oControlInput.AutoInitialize(dto.Duration.GetValue.Value & IIf(dto.Duration.GetValue.IsEstimated, "?", ""))


        Dim oTDpredecessors As HtmlTableCell = e.Item.FindControl("TDpredecessors")
        oTDpredecessors.Visible = THpredecessors.Visible
        If THpredecessors.Visible Then
            oTDpredecessors.Attributes("class") = LTpredecessorsCssClass.Text & IIf(dto.Predecessors.Status = FieldStatus.error, " linkserror", GetCellCssClass(dto.Predecessors.Status))
            oControlInput = e.Item.FindControl("CTRLpredecessorsInput")
            '  oControlInput.ContainerCssClass &= " disabled"
            oControlInput.AutoInitialize(dto.Predecessors.GetValue)
        End If

        oControlInput = e.Item.FindControl("CTRLstartDateInput")
        oControlInput.ReadOnlyInput = THpredecessors.Visible
        '   oControlInput.ContainerCssClass &= " disabled"

        If (dto.EarlyStartDate.GetValue.HasValue) Then
            oControlInput.AutoInitialize(dto.EarlyStartDate.GetValue.Value.ToString(CurrentShortDatePattern))
        Else
            oControlInput.AutoInitialize("")
        End If

        oLabel = e.Item.FindControl("LBactivityEndDate")
        If dto.EarlyFinishDate.GetValue.HasValue Then
            oLabel.Text = dto.EarlyFinishDate.GetValue.Value.ToString(CurrentShortDatePattern)
        Else
            oLabel.Text = "&nbsp;"
            oLabel.CssClass &= " empty"
        End If
        oLabel = e.Item.FindControl("LBactivityDeadLine")
        If dto.Deadline.GetValue.HasValue Then
            oLabel.ToolTip = dto.Deadline.GetValue.Value.ToString(CurrentShortDatePattern)
            oLabel.Visible = True
        Else
            oLabel.Visible = False
        End If
        Dim oControlResources As UC_InLineResources = e.Item.FindControl("CTRLinLineResources")
        oControlResources.InitializeControl(dto.Resources.GetValue, Not dto.IsSummary)
    End Sub
#End Region

    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

   
End Class