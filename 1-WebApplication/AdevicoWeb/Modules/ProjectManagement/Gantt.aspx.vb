Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ProjectGantt
    Inherits PMbaseProjectMap
    Implements IViewProjectGantt

#Region "Context"
    Private _Presenter As ProjectGanttPresenter
    Private ReadOnly Property CurrentPresenter() As ProjectGanttPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProjectGanttPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Settings"
    Private Property AllowSave As Boolean Implements IViewProjectGantt.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveProjectDateInfoBottom.Visible = value
            Me.BTNsaveProjectDateInfoTop.Visible = value
        End Set
    End Property
    Private ReadOnly Property PreviousStartDate As DateTime? Implements IViewProjectGantt.PreviousStartDate
        Get
            Return CTRLprojectInfo.ProjectStartDate
        End Get
    End Property
    Private ReadOnly Property PreviousDeadline As DateTime? Implements IViewProjectGantt.PreviousDeadline
        Get
            Return CTRLprojectInfo.ProjectDeadline
        End Get
    End Property
#End Region


#End Region

#Region "Internal"
    Protected ReadOnly Property CurrentDatePickerShortDatePattern As String
        Get
            Dim pattern As String = CTRLprojectInfo.CurrentShortDatePattern.ToLower

            Return pattern.Replace("mm", "m").Replace("yy", "y").Replace("dd", "d")
        End Get
    End Property
    Protected ReadOnly Property InternalDialogTitleTranslation() As String
        Get
            Return Resource.getValue("InternalDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property ExternalDialogTitleTranslation() As String
        Get
            Return Resource.getValue("ExternalDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property RemoveResourceDialogTitleTranslation() As String
        Get
            Return Resource.getValue("RemoveResourceDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property ProjectResourcesDialogTitleTranslation() As String
        Get
            Return Resource.getValue("ProjectResourcesDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property EditActivityDialogTitleTranslation() As String
        Get
            Return Resource.getValue("EditActivityDialogTitleTranslation")
        End Get
    End Property

    Protected ReadOnly Property OnLoadingTranslation() As String
        Get
            Return Resource.getValue("OnLoadingTranslation")
        End Get
    End Property
    Protected ReadOnly Property CurrentShortDatePattern As String
        Get
            Return LoaderCultureInfo.DateTimeFormat.ShortDatePattern
        End Get
    End Property
    Protected Property LoaderCultureInfo As System.Globalization.CultureInfo
        Get
            Return ViewStateOrDefault("LoaderCultureInfo", Resource.CultureInfo)
        End Get
        Set(value As System.Globalization.CultureInfo)
            ViewState("LoaderCultureInfo") = value
        End Set
    End Property
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
            .setHyperLink(HYPprojectMapReorder, True, True)
            .setHyperLink(HYPprojectMap, False, True)
            .setHyperLink(HYPprojectMapBulkEdit, False, True)
            .setHyperLink(HYPprojectGantt, False, True)

            .setButton(BTNsaveProjectDateInfoTop, True)
            BTNsaveProjectDateInfoBottom.Text = BTNsaveProjectDateInfoTop.Text
            BTNsaveProjectDateInfoBottom.ToolTip = BTNsaveProjectDateInfoTop.ToolTip

            Master.ServiceTitle = .getValue("ServiceTitle.ProjectGantt")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ProjectGantt.ToolTip")
            .setLiteral(LTganttGoToStartDateTop)
            .setLiteral(LTganttGoToTodayTop)

            .setHyperLink(HYPbackToProjectsTop, False, True)
            .setHyperLink(HYPgoToProjectEditTop, False, True)
            .setHyperLink(HYPbackToResourceDashboardTop, False, True)
            .setHyperLink(HYPbackToManagerDashboardTop, False, True)
            .setHyperLink(HYPbackToProjectsBottom, False, True)
            .setHyperLink(HYPgoToProjectEditBottom, False, True)
            .setHyperLink(HYPbackToResourceDashboardBottom, False, True)
            .setHyperLink(HYPbackToManagerDashboardBottom, False, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

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
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownProject.ProjectGantt")
    End Sub
    Protected Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(RootObject.Gantt(IdProject, PreloadIdCommunity, PreloadFromPage, PreloadIdContainerCommunity), PreloadIdCommunity)
    End Sub
#End Region

    Protected Overrides Sub LoadProjectDateInfo(project As dtoProject, allowUpdate As Boolean)
        LTprojectName.Text = project.Name
        CTRLprojectInfo.InitializeControl(project, Resource.CultureInfo, Resource.CultureInfo.DateTimeFormat.ShortDatePattern, allowUpdate)
        If (allowUpdate) Then
            HYPprojectMap.NavigateUrl = ApplicationUrlBase & RootObject.ProjectMap(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        Else
            HYPprojectMap.NavigateUrl = ApplicationUrlBase & RootObject.ViewProjectMap(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        End If
        'HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        'HYPprojectMapBulkEdit.Visible = AllowSave
        HYPprojectMapReorder.NavigateUrl = ApplicationUrlBase & RootObject.MapReorder(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectMapReorder.Visible = AllowSave
        HYPprojectGantt.NavigateUrl = ApplicationUrlBase & RootObject.Gantt(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
        HYPprojectGantt.Visible = True

        'HYPprojectMapBulkEdit.NavigateUrl = ApplicationUrlBase & RootObject.MapBulk(IdProject, project.IdCommunity, project.isPortal, project.isPersonal, FromPage, PreloadIdContainerCommunity)
    End Sub
#End Region

#Region "Implements"

#Region "Display"
    Private Sub DisplayErrorSavingDates() Implements IViewProjectGantt.DisplayErrorSavingDates
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewProjectGantt.DisplayErrorSavingDates"), Helpers.MessageType.error)
    End Sub

    Private Sub DisplaySavedDates(startDate As dtoField(Of Date?), endDate As dtoField(Of Date?), deadLine As dtoField(Of Date?)) Implements IViewProjectGantt.DisplaySavedDates
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("IViewProjectGantt.DisplaySavedDates"), Helpers.MessageType.success)
        CTRLprojectInfo.UpdateDate(startDate, endDate, deadLine)
    End Sub
#End Region
    Private Sub LoadGantt(project As dtoProject) Implements IViewProjectGantt.LoadGantt
        CTRLgantt.InitializeControl(project, CurrentShortDatePattern)
        DVganttCommands.Visible = True
    End Sub
    Private Sub DisplayProjectWithNoTasks() Implements IViewProjectGantt.DisplayProjectWithNoTasks
        DVganttCommands.Visible = False
    End Sub
   
#End Region

    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub



    Private Sub BTNsaveProjectDateInfoTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveProjectDateInfoTop.Click, BTNsaveProjectDateInfoBottom.Click
        CurrentPresenter.SaveProjectDate(IdProject, CTRLprojectInfo.InEditStartDate, CTRLprojectInfo.InEditDeadline)
    End Sub

   
End Class