Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class AddNewProject
    Inherits PMpageBaseEdit
    Implements IViewAddProject

#Region "Context"
    Private _Presenter As AddProjectPresenter
    Private ReadOnly Property CurrentPresenter() As AddProjectPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddProjectPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Settings"
    Private Property AllowAdd As Boolean Implements IViewAddProject.AllowAdd
        Get
            Return ViewStateOrDefault("AllowAdd", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNaddProjectBottom.Visible = value
            Me.BTNaddProjectTop.Visible = value
        End Set
    End Property
#End Region

#End Region

#Region "Inherits"
    'Property del PageBase
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
    'Sub e Function PageBase
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
            .setButton(BTNaddProjectBottom, True)
            .setButton(BTNaddProjectTop, True)
            Master.ServiceTitle = .getValue("ServiceTitle.AddProject")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.AddProject.ToolTip")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
 
#Region "Display"
    Private Sub DisplaySessionTimeout() Implements IViewBaseEdit.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.AddProject(ProjectIdCommunity, forPortal, isPersonal, PreloadFromPage, PreloadIdContainerCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub LoadWizardSteps(idProject As Long, idCommunity As Integer, personal As Boolean, forPortal As Boolean, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep))) Implements IViewBaseEdit.LoadWizardSteps
        Me.CTRLsteps.InitializeControl(idProject, idCommunity, personal, forPortal, steps, FromPage, PreloadIdContainerCommunity)
    End Sub

    Private Sub DisplayNotAvailableComunity() Implements IViewAddProject.DisplayNotAvailableComunity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNotAvailableComunity"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayNotAvailableForAddCommunityProject() Implements IViewAddProject.DisplayNotAvailableForAddCommunityProject
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNotAvailableForAddCommunityProject"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayNotAvailableForAddPersonalProject() Implements IViewAddProject.DisplayNotAvailableForAddPersonalProject
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNotAvailableForAddPersonalProject"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayNotAvailableForAddPortalProject() Implements IViewAddProject.DisplayNotAvailableForAddPortalProject
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNotAvailableForAddPortalProject"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayProjectAddError() Implements IViewAddProject.DisplayProjectAddError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectAddError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayProjectAddError(startDate As DateTime?, days As FlagDayOfWeek) Implements IViewAddProject.DisplayProjectAddError
        CTRLmessages.Visible = True
        If Not startDate.HasValue AndAlso days = FlagDayOfWeek.None Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectAddError.StartDate.FlagDayOfWeek"), Helpers.MessageType.alert)
        ElseIf days = FlagDayOfWeek.None Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectAddError.FlagDayOfWeek"), Helpers.MessageType.alert)
        ElseIf Not startDate.HasValue Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectAddError.StartDate"), Helpers.MessageType.alert)
        End If

    End Sub
#End Region

    Private Function GetDefaultProjectName() As String Implements IViewAddProject.GetDefaultProjectName
        Return Resource.getValue("GetDefaultProjectName")
    End Function
    Private Function GetDefaultCalendarName() As String Implements IViewAddProject.GetDefaultCalendarName
        Return Resource.getValue("GetDefaultCalendarName")
    End Function
    Private Function GetDefaultActivityName() As String Implements IViewAddProject.GetDefaultActivityName
        Return Resource.getValue("GetDefaultActivityName")
    End Function
    Private Sub InitializeProject(project As dtoProject, owner As dtoResource, items As List(Of ProjectAvailability), ByVal selected As ProjectAvailability, Optional ByVal activityToAdd As Integer = 0) Implements IViewAddProject.InitializeProject
        CTRLsettings.InitializeControl(project, owner, items, selected, activityToAdd, PreloadIdContainerCommunity)
    End Sub
    Private Function GetProjectToAdd() As dtoProject Implements IViewAddProject.GetProjectToAdd
        Return CTRLsettings.GetProject()
    End Function

    Private Sub RedirectToEditProject(idProject As Long, idCommunity As Integer, forPortal As Boolean, isPersonal As Boolean, Optional ByVal rActions As Long = 0, Optional ByVal cActions As Long = 0) Implements IViewAddProject.RedirectToEdit
        Me.RedirectToUrl(RootObject.EditProject(idProject, idCommunity, forPortal, isPersonal, True, rActions, cActions, PreloadFromPage, PreloadIdContainerCommunity))
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
    End Sub
    Private Sub SetDashboardUrl(url As String, fromPage As PageListType) Implements IViewBaseEdit.SetDashboardUrl
       
    End Sub
#End Region

#Region "Internal"
    Private Sub BTNaddProjectBottom_Click(sender As Object, e As System.EventArgs) Handles BTNaddProjectBottom.Click, BTNaddProjectTop.Click
        Me.CurrentPresenter.AddProject(CTRLsettings.GetProject(), CTRLsettings.ActivitiesToAdd)
    End Sub
#End Region

    Private Sub AddNewProject_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

  
End Class