Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ProjectCalendars
    Inherits PMpageBaseEdit
    Implements IViewCalendars


#Region "Context"
    Private _Presenter As CalendarsPresenter
    Private ReadOnly Property CurrentPresenter() As CalendarsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CalendarsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Settings"
    Private Property AllowAdd As Boolean Implements IViewCalendars.AllowAdd
        Get
            Return ViewStateOrDefault("AllowAdd", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAdd") = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewCalendars.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveCalendarsBottom.Visible = value
            Me.BTNsaveCalendarsTop.Visible = value
        End Set
    End Property
#End Region

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
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

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPbackToProjectsBottom, True, True)
            .setHyperLink(HYPbackToProjectsTop, True, True)
            .setButton(BTNsaveCalendarsBottom, True)
            .setButton(BTNsaveCalendarsTop, True)
            Master.ServiceTitle = .getValue("ServiceTitle.ProjectCalendars")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ProjectCalendars.ToolTip")

            .setHyperLink(HYPgoToProjectMapTop, False, True)
            .setHyperLink(HYPbackToResourceDashboardTop, False, True)
            .setHyperLink(HYPbackToManagerDashboardTop, False, True)
            .setHyperLink(HYPgoToProjectMapBottom, False, True)
            .setHyperLink(HYPbackToResourceDashboardBottom, False, True)
            .setHyperLink(HYPbackToManagerDashboardBottom, False, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idProject As Long, action As ModuleProjectManagement.ActionType) Implements IViewBaseEdit.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Project, idProject.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewCalendars.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleProjectManagement.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayUnknownProject() Implements IViewCalendars.DisplayUnknownProject
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnoResources.Text = Resource.getValue("DisplayUnknownProject.Calendars")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewCalendars.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.ProjectCalendars(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
#End Region

    Private Sub LoadWizardSteps(idProject As Long, idCommunity As Integer, personal As Boolean, forPortal As Boolean, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep))) Implements IViewBaseEdit.LoadWizardSteps
        Me.CTRLsteps.InitializeControl(idProject, idCommunity, personal, forPortal, steps, FromPage, PreloadIdContainerCommunity)
    End Sub
    Public Function GetDefaultCalendarName() As String Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewCalendars.GetDefaultCalendarName

    End Function

    Public Sub LoadCalendars(calendars As System.Collections.Generic.List(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoCalendar)) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewCalendars.LoadCalendars

    End Sub

    Public Sub LoadProjectAvailableDays(days As lm.Comol.Modules.Standard.ProjectManagement.Domain.FlagDayOfWeek) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewCalendars.LoadProjectAvailableDays

    End Sub
    Private Sub SetProjectsUrl(url As String) Implements IViewBaseEdit.SetProjectsUrl
        If Not String.IsNullOrEmpty(url) Then
            HYPbackToProjectsTop.Visible = True
            HYPbackToProjectsBottom.Visible = True
            HYPbackToProjectsTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPbackToProjectsBottom.NavigateUrl = HYPbackToProjectsTop.NavigateUrl
        End If
    End Sub
    Private Sub SetProjectMapUrl(url As String) Implements IViewBaseEdit.SetProjectMapUrl
        If Not String.IsNullOrEmpty(url) Then
            HYPgoToProjectMapTop.Visible = True
            HYPgoToProjectMapBottom.Visible = True
            HYPgoToProjectMapTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPgoToProjectMapBottom.NavigateUrl = HYPgoToProjectMapTop.NavigateUrl
        End If
    End Sub
    Private Sub SetDashboardUrl(url As String, fromPage As PageListType) Implements IViewBaseEdit.SetDashboardUrl
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
    Private Sub BTNsaveCalendarsTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveCalendarsBottom.Click, BTNsaveCalendarsTop.Click

    End Sub
#End Region

    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

   
End Class