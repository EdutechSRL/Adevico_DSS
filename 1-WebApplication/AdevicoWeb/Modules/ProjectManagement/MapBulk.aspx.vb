Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class MapBulk
    Inherits PMbaseProjectMap
    Implements IViewMapBulk

#Region "Context"
    Private _Presenter As MapBulkPresenter
    Private ReadOnly Property CurrentPresenter() As MapBulkPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MapBulkPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Settings"
    Private Property AllowSave As Boolean Implements IViewMapBulk.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveProjectDateInfoBottom.Visible = value
            Me.BTNsaveProjectDateInfoTop.Visible = value
        End Set
    End Property
    Private ReadOnly Property PreviousStartDate As DateTime? Implements IViewMapBulk.PreviousStartDate
        Get
            Return CTRLprojectInfo.ProjectStartDate
        End Get
    End Property
    Private ReadOnly Property PreviousDeadline As DateTime? Implements IViewMapBulk.PreviousDeadline
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
            .setHyperLink(HYPbackToResourceDashboardTop, False, True)
            .setHyperLink(HYPbackToManagerDashboardTop, False, True)
            .setHyperLink(HYPgoToProjectEditBottom, False, True)
            .setHyperLink(HYPbackToResourceDashboardBottom, False, True)
            .setHyperLink(HYPbackToManagerDashboardBottom, False, True)
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
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.MapBulk(IdProject, PreloadIdCommunity, FromPage, PreloadIdContainerCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
#End Region

    Protected Overrides Sub LoadProjectDateInfo(project As dtoProject, allowUpdate As Boolean) Implements IViewBaseProjectMap.LoadProjectDateInfo
        LTprojectName.Text = project.Name
        CTRLprojectInfo.InitializeControl(project, Resource.CultureInfo, Resource.CultureInfo.DateTimeFormat.ShortDatePattern, allowUpdate)

        HYPprojectMap.NavigateUrl = ApplicationUrlBase & RootObject.ProjectMap(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapBulkEdit.Visible = AllowSave
        HYPprojectMapReorder.NavigateUrl = ApplicationUrlBase & RootObject.MapReorder(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapReorder.Visible = AllowSave
        HYPprojectGantt.NavigateUrl = ApplicationUrlBase & RootObject.Gantt(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectGantt.Visible = True

        HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
    End Sub
#End Region

#Region "Implements"

#Region "Display"
    Private Sub DisplayErrorSavingDates() Implements IViewMapBulk.DisplayErrorSavingDates
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewMapBulk.DisplayErrorSavingDates"), Helpers.MessageType.error)
    End Sub

    Private Sub DisplaySavedDates(startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?)) Implements IViewMapBulk.DisplaySavedDates
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewMapBulk.DisplaySavedDates"), Helpers.MessageType.success)
        CTRLprojectInfo.UpdateDate(startDate, endDate, deadLine)
    End Sub
#End Region

#End Region

    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Private Sub BTNsaveProjectDateInfoTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveProjectDateInfoTop.Click, BTNsaveProjectDateInfoBottom.Click
        CurrentPresenter.SaveProjectDate(IdProject, CTRLprojectInfo.InEditStartDate, CTRLprojectInfo.InEditDeadline)
    End Sub

    Protected Overrides Sub SetEditMapUrl(url As String)

    End Sub

    Protected Overrides Sub LoadAttachments(attachments As List(Of dtoAttachmentItem))

    End Sub
End Class