Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class EditProject
    Inherits PMpageBaseEdit
    Implements IViewEditProject

#Region "Context"
    Private _Presenter As EditProjectPresenter
    Private ReadOnly Property CurrentPresenter() As EditProjectPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditProjectPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Private ReadOnly Property PreloadRequiredActions As Long Implements IViewEditProject.PreloadRequiredActions
        Get
            If IsNumeric(Me.Request.QueryString("rActions")) Then
                Return CLng(Me.Request.QueryString("rActions"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadAddedActions As Long Implements IViewEditProject.PreloadAddedActions
        Get
            If IsNumeric(Me.Request.QueryString("aActions")) Then
                Return CLng(Me.Request.QueryString("aActions"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadAddedProject As Boolean Implements IViewEditProject.PreloadAddedProject
        Get
            Try
                Return System.Convert.ToBoolean(Me.Request.QueryString("added"))
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property
#End Region

#Region "Settings"
    Private Property AllowAdd As Boolean Implements IViewEditProject.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveProjectSettingsTop.Visible = value
            Me.BTNsaveProjectSettingsBottom.Visible = value
            CTRLsettings.InEditingMode = value
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

#Region "Internal"
    Protected Function ConfirmSettingsDialogTitleTranslation() As String
        Return Resource.getValue("ConfirmSettingsDialogTitleTranslation")
    End Function
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
            .setHyperLink(HYPbackToProjectsBottom, False, True)
            .setHyperLink(HYPbackToProjectsTop, False, True)
            .setButton(BTNsaveProjectSettingsBottom, True)
            .setButton(BTNsaveProjectSettingsTop, True)
            Master.ServiceTitle = .getValue("ServiceTitle.EditProject")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.EditProject.ToolTip")

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

#Region "Display"
    Private Sub DisplaySessionTimeout() Implements IViewEditProject.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.EditProject(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub DisplayUnknownProject() Implements IViewEditProject.DisplayUnknownProject
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnknownProject.Settings"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayOwnerChanged(name As String) Implements IViewEditProject.DisplayOwnerChanged
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayOwnerChanged"), name), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayUnableToChangeOwner() Implements IViewEditProject.DisplayUnableToChangeOwner
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToChangeOwner"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayAddedInfo(rActions As Long, cActions As Long) Implements IViewEditProject.DisplayAddedInfo
        CTRLmessages.Visible = True
        If rActions = 0 Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayAddedInfo"), Helpers.MessageType.success)
        ElseIf rActions = cActions AndAlso cActions = 1 Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayAddedInfo.1"), Helpers.MessageType.success)
        ElseIf rActions = cActions AndAlso cActions > 1 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayAddedInfo.n"), rActions), Helpers.MessageType.success)
        ElseIf cActions <= 0 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayAddedInfo.error." & cActions.ToString), rActions), Helpers.MessageType.alert)
        ElseIf cActions > 1 Then
            CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayAddedInfo.error.n"), rActions, cActions), Helpers.MessageType.alert)
        End If
    End Sub
    Private Sub DisplayProjectSavingError() Implements IViewEditProject.DisplayProjectSavingError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectSavingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayProjectSavingError(startDate As Date?, days As FlagDayOfWeek) Implements IViewEditProject.DisplayProjectSavingError
        CTRLmessages.Visible = True
        If Not startDate.HasValue AndAlso days = FlagDayOfWeek.None Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectSavingError.StartDate.FlagDayOfWeek"), Helpers.MessageType.alert)
        ElseIf days = FlagDayOfWeek.None Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectSavingError.FlagDayOfWeek"), Helpers.MessageType.alert)
        ElseIf Not startDate.HasValue Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectSavingError.StartDate"), Helpers.MessageType.alert)
        End If
    End Sub
    Private Sub DisplayProjectSaved() Implements IViewEditProject.DisplayProjectSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayProjectSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayConfirmActions(dto As dtoProject, cStatistics As dtoProjectStatistics) Implements IViewEditProject.DisplayConfirmActions
        DVconfirmSettings.Visible = True
        CTRLconfirmSettings.InitializeControl(IdProject, dto, cStatistics)
    End Sub
    Public Sub UpdateSettings(actions As dtoProjectSettingsSelectedActions, startDate As DateTime?, endDate As DateTime?) Implements IViewEditProject.UpdateSettings
        CTRLsettings.UpdateSettings(actions, startDate, endDate)
    End Sub
    Public Sub UpdateSettings(startDate As DateTime?, endDate As DateTime?) Implements IViewEditProject.UpdateSettings
        CTRLsettings.UpdateSettings(startDate, endDate)
    End Sub
    Private Sub LoadWizardSteps(idProject As Long, idCommunity As Integer, personal As Boolean, forPortal As Boolean, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep))) Implements IViewBaseEdit.LoadWizardSteps
        Me.CTRLsteps.InitializeControl(idProject, idCommunity, personal, forPortal, steps, FromPage, PreloadIdContainerCommunity)
    End Sub
#End Region

    Private Function GetDefaultCalendarName() As String Implements IViewEditProject.GetDefaultCalendarName
        Return Resource.getValue("GetDefaultCalendarName")
    End Function

    Private Sub InitializeProject(project As dtoProject, hasActivities As Boolean, owner As dtoResource, items As List(Of ProjectAvailability), ByVal selected As ProjectAvailability, allowChangeOwner As Boolean, allowChangeOwnerFromResource As Boolean, allowChangeOwnerFromCommunity As Boolean) Implements IViewEditProject.InitializeProject
        CTRLsettings.DisplayCompletion = hasActivities
        CTRLsettings.DisplayDuration = hasActivities
        CTRLsettings.DisplayOwnerChanger = allowChangeOwner
        CTRLsettings.AllowSelectOwnerFromCommunity = allowChangeOwnerFromCommunity
        CTRLsettings.AllowSelectOwnerFromResources = allowChangeOwnerFromResource

        CTRLsettings.DisplayStatus = hasActivities
        CTRLsettings.InitializeControl(project, owner, items, selected, FromPage, PreloadIdContainerCommunity)
    End Sub
    Private Function GetProject() As dtoProject Implements IViewEditProject.GetProject
        Return CTRLsettings.GetProject()
    End Function

    Private Sub InitializeControlToSelectOwner(idCommunity As Integer, hideUsers As List(Of Integer)) Implements IViewEditProject.InitializeControlToSelectOwner
        CTRLmessages.Visible = False
        CTRLsettings.InitializeControlToSelectOwner(idCommunity, hideUsers)
    End Sub
    Private Sub InitializeControlToSelectOwnerFromProject(resources As List(Of dtoProjectResource)) Implements IViewEditProject.InitializeControlToSelectOwnerFromProject
        CTRLmessages.Visible = False
        CTRLsettings.InitializeControlToSelectOwnerFromProject(resources)
    End Sub
    Private Sub UpdateDefaultResourceSelector(resources As List(Of dtoResource)) Implements IViewEditProject.UpdateDefaultResourceSelector
        CTRLsettings.UpdateDefaultResourceSelector(resources)
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
    Private Sub CTRLsettings_RequireNewOwnerFromCommunity() Handles CTRLsettings.RequireNewOwnerFromCommunity
        CurrentPresenter.RequireNewOwnerFromCommunity(IdProject)
    End Sub
    Private Sub CTRLsettings_SelectedNewOwner(idResource As Long) Handles CTRLsettings.SelectedNewOwner
        CurrentPresenter.SelectNewOwner(IdProject, idResource, Resource.getValue("UnknownUser"))
    End Sub
    Private Sub CTRLsettings_RequireNewOwnerFromResources() Handles CTRLsettings.RequireNewOwnerFromResources
        CurrentPresenter.RequireNewOwnerFromResources(IdProject, Resource.getValue("UnknownUser"))
    End Sub
    Private Sub CTRLsettings_AddNewOwner(idPerson As Integer) Handles CTRLsettings.AddNewOwner
        CurrentPresenter.AddNewOwner(IdProject, idPerson, Resource.getValue("UnknownUser"))
    End Sub
    Private Sub CTRLconfirmSettings_ApplyActions(actions As dtoProjectSettingsSelectedActions) Handles CTRLconfirmSettings.ApplyActions
        Me.DVconfirmSettings.Visible = False
        Me.CurrentPresenter.SaveSettings(IdProject, CTRLsettings.GetProject(), actions)
    End Sub
    Private Sub CTRLconfirmSettings_CloseWindow() Handles CTRLconfirmSettings.CloseWindow
        Me.DVconfirmSettings.Visible = False
    End Sub
    Private Sub BTNsaveProjectSettingsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveProjectSettingsBottom.Click, BTNsaveProjectSettingsTop.Click
        Me.CurrentPresenter.SaveSettings(IdProject, CTRLsettings.GetProject())
    End Sub
#End Region

    Private Sub AddNewProject_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

  
End Class