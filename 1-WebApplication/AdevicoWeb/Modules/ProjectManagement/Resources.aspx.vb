Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ProjectResources
    Inherits PMpageBaseEdit
    Implements IViewProjectResources

#Region "Context"
    Private _Presenter As ProjectResourcesPresenter
    Private ReadOnly Property CurrentPresenter() As ProjectResourcesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProjectResourcesPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Settings"
    Private Property AllowAddCommunityUser As Boolean Implements IViewProjectResources.AllowAddCommunityUser
        Get
            Return ViewStateOrDefault("AllowAddCommunityUser", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAddCommunityUser") = value
            Me.LNBaddUsersFromCommunity.Visible = value
        End Set
    End Property
    Private Property AllowAddExternalUser As Boolean Implements IViewProjectResources.AllowAddExternalUser
        Get
            Return ViewStateOrDefault("AllowAddExternalUser", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAddExternalUser") = value
            Me.LNBaddCustomUsers.Visible = value
            Me.LNBaddCustomUser.Visible = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewProjectResources.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveResourcesBottom.Visible = value
            Me.BTNsaveResourcesTop.Visible = value
        End Set
    End Property
#End Region

    Private ReadOnly Property UnknownUser As String Implements IViewProjectResources.UnknownUser
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property
#End Region

#Region "Internal"
   
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
            .setLinkButton(LNBaddCustomUsers, False, True)
            .setLinkButton(LNBaddCustomUser, False, True)
            .setLinkButton(LNBaddUsersFromCommunity, False, True)
            .setButton(BTNsaveResourcesBottom, True)
            .setButton(BTNsaveResourcesTop, True)
            .setLabel(LBresourceListTitle)
            Master.ServiceTitle = .getValue("ServiceTitle.ProjectResources")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ProjectResources.ToolTip")

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
    Private Sub DisplayUnknownProject() Implements IViewProjectResources.DisplayUnknownProject
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnoResources.Text = Resource.getValue("DisplayUnknownProject.Resources")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewProjectResources.DisplaySessionTimeout
        RedirectOnSessionTimeOut(RootObject.ProjectResources(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity), PreloadIdCommunity)
    End Sub
    Private Sub DisplayNoResources() Implements IViewProjectResources.DisplayNoResources
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoResources"), Helpers.MessageType.info)
    End Sub
    Private Sub DisplayUnableToAddExternalResource() Implements IViewProjectResources.DisplayUnableToAddExternalResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToAddExternalResource"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnableToSaveResources() Implements IViewProjectResources.DisplayUnableToSaveResources
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToSaveResources"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnableToAddInternalResource() Implements IViewProjectResources.DisplayUnableToAddInternalResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToAddInternalResource"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnableToAddInternalResources() Implements IViewProjectResources.DisplayUnableToAddInternalResources
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToAddInternalResources"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayErrors(multipleLongName As Integer, multipleShortName As Integer) Implements IViewProjectResources.DisplayErrors
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(GetMessage("DisplayErrors.", multipleLongName, multipleShortName), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayRemovedResource(name As String) Implements IViewProjectResources.DisplayRemovedResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayRemovedResource"), name), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayResourceAdded(added As Integer, Optional multipleLongName As Integer = 0, Optional multipleShortName As Integer = 0) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewProjectResources.DisplayResourceAdded
        CTRLmessages.Visible = True

        If added > 0 Then
            Dim message As String = "DisplayResourceAdded." & IIf(added = 1, "1", "n")
            Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)

            If added > 1 Then
                items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = String.Format(Resource.getValue(message), added), .Type = Helpers.MessageType.success})
            Else
                items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = Resource.getValue(message), .Type = Helpers.MessageType.success})
            End If
            If multipleLongName > 0 OrElse multipleShortName > 0 Then
                items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = GetMessage("DisplayErrors.", multipleLongName, multipleShortName), .Type = Helpers.MessageType.alert})
            End If
            CTRLmessages.InitializeControl(items)
        Else
            DisplayErrors(multipleLongName, multipleShortName)
        End If

    End Sub
    Private Sub DisplayUnableToRemoveResource(name As String) Implements IViewProjectResources.DisplayUnableToRemoveResource
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayUnableToRemoveResource"), name), Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySavedSettings(Optional multipleLongName As Integer = 0, Optional multipleShortName As Integer = 0) Implements IViewProjectResources.DisplaySavedSettings
        Dim items As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        CTRLmessages.Visible = True
        items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = Resource.getValue("DisplaySavedSettings"), .Type = Helpers.MessageType.success})

        If multipleLongName > 0 OrElse multipleShortName > 0 Then
            items.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = GetMessage("DisplayErrors.", multipleLongName, multipleShortName), .Type = Helpers.MessageType.alert})
        End If
        CTRLmessages.InitializeControl(items)
    End Sub
    Private Sub DisplayUnableToRemoveResource(idProject As Long, idResource As Long, name As String, assignedTasks As Long, completedTasks As Long) Implements IViewProjectResources.DisplayUnableToRemoveResource
        Me.CTRLmessages.Visible = False
        Me.DVremoveResources.Visible = True
        CTRLconfirmRemoveResource.InitializeControl(idProject, idResource, name, assignedTasks, completedTasks, Resource.getValue("RemoveResource.Description"))
    End Sub

    Private Function GetMessage(ByVal startMessage As String, multipleLongName As Integer, multipleShortName As Integer) As String
        Dim message As String = startMessage
        Select Case multipleLongName
            Case 0, 1
                message &= multipleLongName.ToString & "."
            Case Else
                If multipleLongName > 1 Then
                    message &= "n."
                Else
                    message &= "0."
                End If
        End Select
        Select Case multipleShortName
            Case 0, 1
                message &= multipleShortName.ToString
            Case Else
                If multipleShortName > 1 Then
                    message &= "n"
                Else
                    message &= "0"
                End If
        End Select
        If multipleLongName > 1 AndAlso multipleShortName > 1 Then
            Return String.Format(Resource.getValue(message), multipleLongName, multipleShortName)
        ElseIf multipleLongName > 1 Then
            Return String.Format(Resource.getValue(message), multipleLongName)
        ElseIf multipleShortName > 1 Then
            Return String.Format(Resource.getValue(message), multipleShortName)
        Else
            Return Resource.getValue(message)
        End If
    End Function

#End Region

    Private Sub LoadWizardSteps(idProject As Long, idCommunity As Integer, personal As Boolean, forPortal As Boolean, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep))) Implements IViewBaseEdit.LoadWizardSteps
        Me.CTRLsteps.InitializeControl(idProject, idCommunity, personal, forPortal, steps, FromPage, PreloadIdContainerCommunity)
    End Sub
    Private Sub LoadResources(resources As List(Of dtoProjectResource)) Implements IViewProjectResources.LoadResources
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.DVaddResource.Visible = AllowAddCommunityUser OrElse AllowAddExternalUser
        '    HYPbackToProjectsBottom.NavigateUrl = ApplicationUrlBase() & RootObject.p
        Me.DVaddResource.Attributes.Add("class", IIf((AllowAddExternalUser), "ddbuttonlist enabled", "ddbuttonlist"))

        Me.CTRLmanageResources.InitializeControl(resources)
    End Sub

    Private Function GetResources() As List(Of dtoProjectResource) Implements IViewProjectResources.GetResources
        Return CTRLmanageResources.GetResources()
    End Function

    Private Sub InitializeControlToAddUsers(idCommunity As Integer, hideUsers As List(Of Integer)) Implements IViewProjectResources.InitializeControlToAddUsers
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunity, hideUsers, Nothing, Resource.getValue("SelectResourcesFromCommunity.Description"))
    End Sub
    Private Sub InitializeControlToAddExternalResource() Implements IViewProjectResources.InitializeControlToAddExternalResource
        Me.CTRLmessages.Visible = False
        Me.DVaddExternalResources.Visible = True
        CTRLaddExternalResources.InitializeControl(Resource.getValue("AddExternalResource.Description"))
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
    Private Sub LNBaddCustomUser_Click(sender As Object, e As System.EventArgs) Handles LNBaddCustomUser.Click
        Me.CurrentPresenter.AddExternalResource(IdProject, GetResources(), UnknownUser)
    End Sub
    Private Sub LNBaddCustomUsers_Click(sender As Object, e As System.EventArgs) Handles LNBaddCustomUsers.Click
        Me.CurrentPresenter.SelectExternalResources(IdProject, GetResources(), UnknownUser)
    End Sub
    Private Sub LNBaddUsersFromCommunity_Click(sender As Object, e As System.EventArgs) Handles LNBaddUsersFromCommunity.Click
        Me.CurrentPresenter.SelectResourcesFromCommunity(IdProject, GetResources(), UnknownUser)
    End Sub

#Region "Add Internal Resources"
    Private Sub CLTRselectUsers_CloseWindow() Handles CLTRselectUsers.CloseWindow
        Me.DVselectUsers.Visible = False
    End Sub
    Private Sub CLTRselectUsers_UsersSelected(idUsers As List(Of Integer)) Handles CLTRselectUsers.UsersSelected
        Me.DVselectUsers.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.AddInternalResources(IdProject, GetResources(), idUsers, UnknownUser)
    End Sub
#End Region

#Region "Add External Resources"
    Private Sub CTRLaddExternalResources_CloseWindow() Handles CTRLaddExternalResources.CloseWindow
        Me.DVaddExternalResources.Visible = False
    End Sub
    Private Sub CTRLaddExternalResources_AddingResources(resources As List(Of dtoExternalResource)) Handles CTRLaddExternalResources.AddingResources
        Me.DVaddExternalResources.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.AddExternalResources(IdProject, GetResources(), resources, UnknownUser)
    End Sub
#End Region

#Region "Confirm Delete"
    Private Sub CTRLconfirmRemoveResource_CloseWindow() Handles CTRLconfirmRemoveResource.CloseWindow
        Me.DVremoveResources.Visible = False
    End Sub
    Private Sub CTRLconfirmRemoveResource_DeletingResource(idResource As Long, resourceName As String, rAction As RemoveAction) Handles CTRLconfirmRemoveResource.DeletingResource
        Me.DVremoveResources.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.ConstrainVirtualDeleteResource(idResource, resourceName, rAction)
    End Sub
#End Region

    Private Sub CTRLmanageResources_VirtualDeleteResource(idResource As Long, name As String) Handles CTRLmanageResources.VirtualDeleteResource
        CurrentPresenter.VirtualDeleteResource(idResource, name)
    End Sub

    Private Sub BTNsaveResourcesTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveResourcesTop.Click, BTNsaveResourcesBottom.Click
        Me.CurrentPresenter.SaveSettings(IdProject, GetResources(), UnknownUser)
    End Sub
#End Region

    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

End Class