Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class DBbaseEditSettings
    Inherits PageBase
    Implements IViewBaseEditSettings

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseEditSettings.PreloadIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return -2
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadDashboardType As DashboardType Implements IViewBaseEditSettings.PreloadDashboardType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DashboardType).GetByString(Request.QueryString("type"), DashboardType.Portal)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdDashboard As Long Implements IViewBaseEditSettings.PreloadIdDashboard
        Get
            If IsNumeric(Request.QueryString("idDashboard")) Then
                Return CLng(Request.QueryString("idDashboard"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadFromAdd As Boolean Implements IViewBaseEditSettings.PreloadFromAdd
        Get
            Return Not String.IsNullOrEmpty(Request.QueryString("fromAdd")) AndAlso Request.QueryString("fromAdd").ToLower() = "true"
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property IdContainerCommunity As Integer Implements IViewBaseEditSettings.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdContainerCommunity") = value
        End Set
    End Property
    Protected Friend Property IdDashboard As Long Implements IViewBaseEditSettings.IdDashboard
        Get
            Return ViewStateOrDefault("IdDashboard", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdDashboard") = value
        End Set
    End Property
    Protected WriteOnly Property AllowSave As Boolean Implements IViewBaseEditSettings.AllowSave
        Set(ByVal value As Boolean)
            Dim oButton As Button = GetSaveButton()
            If Not IsNothing(oButton) Then
                oButton.Visible = True

                oButton = GetSaveButton(False)
                oButton.Visible = True
            End If
        End Set
    End Property
    Protected Friend Property DashboardType As DashboardType Implements IViewBaseEditSettings.DashboardType
        Get
            Return ViewStateOrDefault("DashboardType", lm.Comol.Core.Dashboard.Domain.DashboardType.Portal)
        End Get
        Set(ByVal value As DashboardType)
            Me.ViewState("DashboardType") = value
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

#Region "Inherits"
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Dashboard", "Modules", "Dashboard")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewBaseEditSettings.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idDashboard As Long, action As ModuleDashboard.ActionType) Implements IViewBaseEditSettings.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Dashboard, idDashboard.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBaseEditSettings.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleDashboard.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayDeletedDashboard() Implements IViewBaseEditSettings.DisplayDeletedDashboard
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            oControl.Visible = True
            oControl.InitializeControl(Resource.getValue("DisplayDeletedDashboard"), Helpers.MessageType.alert)
            GetSaveButton().Visible = False
            GetSaveButton(False).Visible = False
        End If
    End Sub
    Private Sub DisplayMessage(dbError As DashboardErrorType) Implements IViewBaseEditSettings.DisplayMessage
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Select Case dbError
                Case DashboardErrorType.MultipleAssignmentForPerson, DashboardErrorType.MultipleAssignmentForProfileType, DashboardErrorType.MultipleAssignmentForRole, DashboardErrorType.DefaultSettingsUndeletable, DashboardErrorType.DefaultSettingsUnavailable, DashboardErrorType.NoAssignmentsForEnable, DashboardErrorType.NoAssignmentsForActiveSettings, DashboardErrorType.NotActivableSettings, DashboardErrorType.NoAssignmentsForUndelete
                    mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            End Select
            oControl.Visible = True
            oControl.InitializeControl(Resource.getValue("DisplayMessage.ModuleDashboard.DashboardErrorType." & dbError.ToString), mType)
        End If
    End Sub
    Private Sub DisplayMessage(action As ModuleDashboard.ActionType) Implements IViewBaseEditSettings.DisplayMessage
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Select Case action
                Case ModuleDashboard.ActionType.DashboardSettingsAdded, ModuleDashboard.ActionType.DashboardSettingsSaved, ModuleDashboard.ActionType.DashboardSettingsViewsSaved, ModuleDashboard.ActionType.TileAutoGenerateForCommunityTypes, ModuleDashboard.ActionType.DashboardSettingsTilesOrderSaved
                    mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            End Select
            oControl.Visible = True
            oControl.InitializeControl(Resource.getValue("DisplayMessage.ModuleDashboard.ActionType." & action.ToString), mType)
        End If
    End Sub
#End Region
    Protected Friend MustOverride Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub
    Private Sub SetBackUrl(url As String) Implements IViewBaseEditSettings.SetBackUrl
        Dim oHyperlink As HyperLink = GetBackUrlItem()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True

            oHyperlink = GetBackUrlItem(False)
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
    End Sub
    Private Sub SetPreviewUrl(url As String) Implements IViewBaseEditSettings.SetPreviewUrl
        Dim oHyperlink As HyperLink = GetPreviewUrlItem()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True

            oHyperlink = GetPreviewUrlItem(False)
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
    End Sub
    Private Sub DisplayUnknownDashboard() Implements IViewBaseEditSettings.DisplayUnknownDashboard
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            oControl.Visible = True
            oControl.InitializeControl(Resource.getValue("DisplayUnknownDashboard"), Helpers.MessageType.alert)
        End If
    End Sub

    Private Sub LoadWizardSteps(steps As List(Of lm.Comol.Core.Wizard.dtoNavigableWizardItem(Of dtoDashboardStep))) Implements IViewBaseEditSettings.LoadWizardSteps
        Dim toLoad As New List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of Integer))

        For Each item As lm.Comol.Core.Wizard.dtoNavigableWizardItem(Of dtoDashboardStep) In steps
            Dim nStep As New lm.Comol.Core.Wizard.NavigableWizardItem(Of Integer)
            nStep.Id = CInt(item.Id.Type)
            nStep.Name = Resource.getValue("WizardDashboardStep." & item.Id.Type.ToString)
            nStep.Tooltip = Resource.getValue("WizardDashboardStep." & item.Id.Type.ToString & ".Tooltip")
            nStep.Url = item.Url
            nStep.Status = item.Status
            nStep.AutoPostBack = item.AutoPostBack
            nStep.Active = item.Active
            If item.Id.Errors.Any() Then
                nStep.Message = Resource.getValue("WizardDashboardStep.EditingErrors." & item.Id.Errors(0).ToString)
            Else
                Select Case item.Id.Type
                    Case WizardDashboardStep.Settings
                        Dim oItem As dtoSettingsStep = DirectCast(item.Id, dtoSettingsStep)

                        nStep.Message = Resource.getValue("WizardDashboardStep." & item.Id.Type.ToString & ".AvailableStatus." & oItem.DashboardStatus.ToString)
                    Case WizardDashboardStep.HomepageSettings
                        Dim dtoHomeStep As dtoHomeSettingsStep = DirectCast(item.Id, dtoHomeSettingsStep)
                        If dtoHomeStep.Pages > 1 Then
                            nStep.Message = String.Format(Resource.getValue("WizardDashboardStep." & item.Id.Type.ToString & ".Message.n"), dtoHomeStep.Pages)
                        Else
                            nStep.Message = Resource.getValue("WizardDashboardStep." & item.Id.Type.ToString & ".Message" & GetTranslationString(dtoHomeStep.Pages))
                        End If
                    Case WizardDashboardStep.Tiles, WizardDashboardStep.CommunityTypes, WizardDashboardStep.Modules
                        Dim dtoTileStep As dtoTileStep = DirectCast(item.Id, dtoTileStep)
                        Dim messages As New List(Of String)
                        If dtoTileStep.AvailableTiles > 1 Then
                            messages.Add(String.Format(Resource.getValue("WizardDashboardStep.Tiles.Message.AvailableTiles" & GetTranslationString(dtoTileStep.AvailableTiles)), dtoTileStep.AvailableTiles))
                        Else
                            messages.Add(Resource.getValue("WizardDashboardStep.Tiles.Message.AvailableTiles." & GetTranslationString(dtoTileStep.AvailableTiles)))
                        End If

                        If dtoTileStep.InUseTiles > 1 Then
                            messages.Add(String.Format(Resource.getValue("WizardDashboardStep.Tiles.Message.InUseTiles" & GetTranslationString(dtoTileStep.InUseTiles)), dtoTileStep.InUseTiles))
                        Else
                            messages.Add(Resource.getValue("WizardDashboardStep.Tiles.Message.InUseTiles." & GetTranslationString(dtoTileStep.InUseTiles)))
                        End If

                        If dtoTileStep.UserTile > 0 Then
                            If dtoTileStep.UserTile > 1 Then
                                messages.Add(String.Format(Resource.getValue("WizardDashboardStep.Tiles.Message.UserTile" & GetTranslationString(dtoTileStep.UserTile)), dtoTileStep.UserTile))
                            Else
                                messages.Add(Resource.getValue("WizardDashboardStep.Tiles.Message.UserTile." & GetTranslationString(dtoTileStep.UserTile)))
                            End If
                        End If

                        nStep.Message = String.Join(", ", messages.Where(Function(m) Not String.IsNullOrEmpty(m)).ToArray)
                End Select
            End If
            toLoad.Add(nStep)
        Next
        LoadWizardSteps(toLoad)
    End Sub

    Private Sub LoadWizardSteps(steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of Integer))) Implements IViewBaseEditSettings.LoadWizardSteps
        Dim oControl As UC_GenericWizardSteps = GetStepsControl()
        If Not IsNothing(oControl) Then
            oControl.Visible = True
            oControl.InitializeControl(steps)
        End If
    End Sub
  
    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetBackUrlItem(Optional ByVal top As Boolean = True) As HyperLink
    Protected Friend MustOverride Function GetPreviewUrlItem(Optional ByVal top As Boolean = True) As HyperLink
    Protected Friend MustOverride Function GetSaveButton(Optional ByVal top As Boolean = True) As Button
    Protected Friend MustOverride Function GetMessageControl() As UC_ActionMessages
    Protected Friend MustOverride Function GetStepsControl() As UC_GenericWizardSteps
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
    Private Function GetTranslationString(ByVal count As Integer) As String
        Select Case count
            Case 0, 1
                Return "." & count.ToString
            Case Else
                Return ".n"
        End Select
    End Function
#End Region



   
End Class