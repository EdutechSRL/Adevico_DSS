Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Public Class WizardAddProfile
    Inherits PageBase
    Implements IViewAddProfileToPortalWizard

#Region "Context"
    Private _Presenter As AddProfileToPortalWizardPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As AddProfileToPortalWizardPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddProfileToPortalWizardPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property CurrentStep As ProfileWizardStep Implements IViewProfileWizard.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", ProfileWizardStep.None)
        End Get
    End Property
    Public Property AvailableSteps As List(Of ProfileWizardStep) Implements IViewProfileWizard.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of ProfileWizardStep))
        End Get
        Set(value As List(Of ProfileWizardStep))
            ViewState("AvailableSteps") = value
        End Set
    End Property
    Public Property SkipSteps As List(Of ProfileWizardStep) Implements IViewProfileWizard.SkipSteps
        Get
            Return ViewStateOrDefault("SkipSteps", New List(Of ProfileWizardStep))
        End Get
        Set(value As List(Of ProfileWizardStep))
            ViewState("SkipSteps") = value
        End Set
    End Property
    Public WriteOnly Property AllowBackTomanagement As Boolean Implements IViewAddProfileToPortalWizard.AllowBackTomanagement
        Set(value As Boolean)
            Me.HYPmanage.Visible = value
        End Set
    End Property
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

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            If Request.UrlReferrer Is Nothing Then
                Me.HYPmanage.NavigateUrl = BaseUrl & RootObject.ManagementProfiles
            ElseIf Request.UrlReferrer.AbsoluteUri.Contains(RootObject.ManagementProfiles) Then
                Me.HYPmanage.NavigateUrl = BaseUrl & RootObject.ManagementProfilesWithFilters
            Else
                Me.HYPmanage.NavigateUrl = BaseUrl & RootObject.ManagementProfiles
            End If
            CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setButton(BTNbackBottom, True, , , True)
            .setButton(BTNbackTop, True, , , True)
            .setButton(BTNnextBottom, True, , , True)
            .setButton(BTNnextTop, True, , , True)
            .setButton(BTNcompleteTop, True, , , True)
            .setButton(BTNcompleteBottom, True, , , True)

            .setHyperLink(HYPmanage, True, True)

            Master.ServiceNopermission = Resource.getValue("nopermissionAddProfileToPortal")
            Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleAddProfileToPortal"), "")
        End With
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"

#Region "Move Wizard"
    Public Sub GotoStep(pStep As ProfileWizardStep) Implements IViewProfileWizard.GotoStep
        GotoStep(pStep, False)
    End Sub
    Public Sub GotoStep(pStep As ProfileWizardStep, initialize As Boolean) Implements IViewProfileWizard.GotoStep
        Select Case pStep
            Case ProfileWizardStep.ProfileTypeSelector
                MLVwizard.SetActiveView(VIWprofileTypes)
                If initialize Then : CTRLprofileTypes.InitializeControlForManagement(SelectedProfileTypeId)
                End If
            Case ProfileWizardStep.OrganizationSelector
                MLVwizard.SetActiveView(VIWorganization)
                If initialize Then : Me.CTRLorganizations.InitializeControl(True)
                End If
            Case ProfileWizardStep.AuthenticationTypeSelector
                MLVwizard.SetActiveView(VIWauthenticationTypes)

            Case ProfileWizardStep.ProfileUserData
                MLVwizard.SetActiveView(VIWuserInfo)
                If initialize Then : Me.CTRLprofileInfo.InitializeControlForManagement(ProfileInfo, SelectedProfileTypeId, Me.CurrentPresenter.GetAuthenticationProvider(SelectedProviderId), Me.CTRLorganizations.SelectedOrganizationId)
                End If
            Case ProfileWizardStep.Summary
                MLVwizard.SetActiveView(VIWcomplete)
                Me.CTRLsummary.InitializeForManagement(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.CTRLorganizations.OtherOrganizationsToSubscribe, Me.SelectedProfileName, SelectedProfileTypeId, Me.CurrentPresenter.GetAuthenticationProvider(SelectedProviderId), Me.GetExternalCredentials)
        End Select

        Me.BTNbackBottom.Visible = DisplayBackButton(pStep)
        Me.BTNbackTop.Visible = DisplayBackButton(pStep)
        Me.BTNcompleteBottom.Visible = (pStep = ProfileWizardStep.Summary)
        Me.BTNcompleteTop.Visible = (pStep = ProfileWizardStep.Summary)
        Me.BTNnextBottom.Visible = (pStep <> ProfileWizardStep.Summary)
        Me.BTNnextTop.Visible = (pStep <> ProfileWizardStep.Summary)

        Me.ViewState("CurrentStep") = pStep
        If Me.CurrentStep = ProfileWizardStep.ProfileUserData Then
            Me.LBstepTitle.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep), Me.CTRLprofileTypes.SelectedProfileName)
        Else
            Me.LBstepTitle.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
        End If

        Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & pStep.ToString & ".Description")
        If Me.SelectedProfileTypeId = PreloadedIdProfileType Then
            Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleAddProfileToPortal"), SelectedProfileName)
        ElseIf pStep <> ProfileWizardStep.ProfileTypeSelector Then
            Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleAddProfileToPortal"), SelectedProfileName)
        Else
            Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleAddProfileToPortal"), "")
        End If

    End Sub

    Private Function DisplayBackButton(current As ProfileWizardStep) As Boolean
        If Not SkipSteps.Contains(ProfileWizardStep.ProfileTypeSelector) Then
            Return current <> ProfileWizardStep.ProfileTypeSelector
        ElseIf Not SkipSteps.Contains(ProfileWizardStep.OrganizationSelector) Then
            Return current <> ProfileWizardStep.OrganizationSelector
        ElseIf Not SkipSteps.Contains(ProfileWizardStep.AuthenticationTypeSelector) Then
            Return current <> ProfileWizardStep.AuthenticationTypeSelector
        Else
            Return True
        End If

    End Function
    Public Sub InitializeStep(pStep As ProfileWizardStep) Implements IViewProfileWizard.InitializeStep
        Select Case pStep
            Case ProfileWizardStep.OrganizationSelector
                Me.CTRLorganizations.InitializeControl(True)
            Case ProfileWizardStep.ProfileTypeSelector
                CTRLprofileTypes.InitializeControlForManagement(SelectedProfileTypeId)
            Case ProfileWizardStep.ProfileUserData
                Me.CTRLprofileInfo.InitializeControlForManagement(ProfileInfo, SelectedProfileTypeId, Me.CurrentPresenter.GetAuthenticationProvider(SelectedProviderId), Me.CTRLorganizations.SelectedOrganizationId)
            Case ProfileWizardStep.Summary
                Me.CTRLsummary.InitializeForManagement(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.CTRLorganizations.OtherOrganizationsToSubscribe, Me.SelectedProfileName, SelectedProfileTypeId, Me.CurrentPresenter.GetAuthenticationProvider(SelectedProviderId), Me.GetExternalCredentials)
        End Select
    End Sub

    Public Function IsInitialized(pStep As ProfileWizardStep) As Boolean Implements IViewProfileWizard.IsInitialized
        Select Case pStep
            Case ProfileWizardStep.AuthenticationTypeSelector
                Return CTRLauthentication.isInitialized
            Case ProfileWizardStep.OrganizationSelector
                Return CTRLorganizations.isInitialized
            Case ProfileWizardStep.ProfileTypeSelector
                Return CTRLprofileTypes.isInitialized
            Case ProfileWizardStep.ProfileUserData
                Return CTRLprofileInfo.isInitialized
            Case ProfileWizardStep.Summary
                Return CTRLsummary.isInitialized
            Case Else
                Return False
        End Select
    End Function

    Private Function GetStepNumber(pStep As ProfileWizardStep) As Integer
        Dim Number As Integer = 1
        Dim list As List(Of ProfileWizardStep) = Me.AvailableSteps
        Dim toSkip As List(Of ProfileWizardStep) = Me.SkipSteps
        For Each item As ProfileWizardStep In (From i In list Where toSkip.Contains(i) = False Select i).ToList
            If item = pStep Then
                Return Number
            Else
                Number += 1
            End If
        Next

    End Function

    Private Sub BTNbackBottom_Click(sender As Object, e As System.EventArgs) Handles BTNbackBottom.Click, BTNbackTop.Click
        If MLVwizard.GetActiveView Is VIWprofileError Then
            Me.MLVwizard.SetActiveView(VIWcomplete)
            Me.BTNcompleteBottom.Enabled = True
            Me.BTNcompleteTop.Enabled = True
            Me.LBstepTitle.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
            Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Description")
        Else
            Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
        End If
    End Sub

    Private Sub BTNcompleteBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteBottom.Click, BTNcompleteTop.Click
        If Me.CTRLsummary.isCompleted Then
            Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
        Else
            Me.CurrentPresenter.CreateProfile(ProfileInfo, SelectedProfileTypeId, SelectedProfileName, SelectedOrganizationId)
        End If
    End Sub

    Private Sub BTNnextBottom_Click(sender As Object, e As System.EventArgs) Handles BTNnextBottom.Click, BTNnextTop.Click
        If Page.IsValid Then
            Me.CurrentPresenter.MoveToNextStep(CurrentStep)
        End If
    End Sub
#End Region


#End Region

#Region "Step 1 ProfileTypeSelector"
    Public Sub InitializeProfileTypeSelector(IdDefaultRole As Integer) Implements IViewAddProfileToPortalWizard.InitializeProfileTypeSelector
        Me.CTRLprofileTypes.InitializeControlForManagement(IdDefaultRole)
    End Sub
    Public Property SelectedProfileTypeId As Integer Implements IViewProfileWizard.SelectedProfileTypeId
        Get
            Return Me.CTRLprofileTypes.SelectedProfileTypeId
        End Get
        Set(value As Integer)
            Me.CTRLprofileTypes.SelectedProfileTypeId = value
        End Set
    End Property
    Public ReadOnly Property AvailableProfileTypes As List(Of Integer) Implements IViewProfileWizard.AvailableProfileTypes
        Get
            Return Me.CTRLprofileTypes.AvailableProfileTypes
        End Get
    End Property
    Public ReadOnly Property SelectedProfileName As String Implements IViewProfileWizard.SelectedProfileName
        Get
            Return Me.CTRLprofileTypes.SelectedProfileName
        End Get
    End Property
    Public ReadOnly Property PreloadedIdProfileType As Integer Implements IViewAddProfileToPortalWizard.PreloadedIdProfileType
        Get
            If IsNumeric(Request.QueryString("IdProfileType")) Then
                Return CInt(Request.QueryString("IdProfileType"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property SelectedProviderAllowAdminProfileInsert As Boolean Implements IViewAddProfileToPortalWizard.SelectedProviderAllowAdminProfileInsert
        Get
            Return CTRLauthentication.SelectedProviderAllowAdminProfileInsert
        End Get
    End Property
#End Region

#Region "Step 2 Organization"
    Public ReadOnly Property AvailableOrganizationsId As List(Of Integer) Implements IViewProfileWizard.AvailableOrganizationsId
        Get
            Return Me.CTRLorganizations.AvailableOrganizationsId
        End Get
    End Property
    Public Property SelectedOrganizationId As Integer Implements IViewProfileWizard.SelectedOrganizationId
        Get
            Return Me.CTRLorganizations.SelectedOrganizationId
        End Get
        Set(value As Integer)
            Me.CTRLorganizations.SelectedOrganizationId = value
        End Set
    End Property
    Public ReadOnly Property OtherSelectedOrganizationId As List(Of Integer) Implements IViewAddProfileToPortalWizard.OtherSelectedOrganizationId
        Get
            Return Me.CTRLorganizations.OtherOrganizationsToSubscribeID
        End Get
    End Property
#End Region

#Region "Step 3 Providers"
    Public Sub InitializeAuthenticationTypeSelectorStep(providers As List(Of lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider), idProvider As Long) Implements IViewAddProfileToPortalWizard.InitializeAuthenticationTypeSelectorStep
        Me.CTRLauthentication.InitializeControl(providers, idProvider)
    End Sub

    Public ReadOnly Property AvailableProvidersId As List(Of Long) Implements IViewAddProfileToPortalWizard.AvailableProvidersId
        Get
            Return Me.CTRLauthentication.AvailableProvidersId
        End Get
    End Property
    Public Property SelectedProviderId As Long Implements IViewAddProfileToPortalWizard.SelectedProviderId
        Get
            Return Me.CTRLauthentication.SelectedProviderId
        End Get
        Set(value As Long)
            Me.CTRLauthentication.SelectedProviderId = value
        End Set
    End Property
    Public ReadOnly Property SelectedProviderType As lm.Comol.Core.Authentication.AuthenticationProviderType Implements IViewAddProfileToPortalWizard.SelectedProviderType
        Get
            Return Me.CTRLauthentication.SelectedProviderType
        End Get
    End Property
#End Region

#Region "Step 4 Profile Info"
    Public Property ProfileInfo As dtoBaseProfile Implements IViewProfileWizard.ProfileInfo
        Get
            Return Me.CTRLprofileInfo.CurrentUserInfo
        End Get
        Set(value As dtoBaseProfile)
            Me.CTRLprofileInfo.CurrentUserInfo = value
        End Set
    End Property
    Public Sub LoadProfileInfoError(errors As List(Of ProfilerError)) Implements IViewProfileWizard.LoadProfileInfoError
        Me.CTRLprofileInfo.LoadProfileInfoError(errors)
    End Sub
    Public Sub UnloadProfileInfoError() Implements IViewProfileWizard.UnloadProfileInfoError
        Me.CTRLprofileInfo.UnloadProfileInfoError()
    End Sub

    Public Sub SaveUserPassword() Implements IViewProfileWizard.SaveUserPassword
        Me.CTRLprofileInfo.SaveUserPassword()
    End Sub
    Public ReadOnly Property GetExternalCredentials As dtoExternalCredentials Implements IViewAddProfileToPortalWizard.GetExternalCredentials
        Get
            Return Me.CTRLprofileInfo.GetExternalCredentials()
        End Get
    End Property
#End Region

#Region "Step 6 Summary"
    Public Function CreateProfile(profile As lm.Comol.Core.Authentication.dtoBaseProfile, idProfileType As Integer, profileName As String, idOrganization As Integer, otherOrganizations As List(Of Integer), provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, credentials As dtoExternalCredentials) As lm.Comol.Core.BaseModules.ProfileManagement.ProfileSubscriptionMessage Implements lm.Comol.Core.BaseModules.ProfileManagement.Presentation.IViewAddProfileToPortalWizard.CreateProfile
        Return Me.CTRLsummary.CreateProfile(profile, idProfileType, profileName, idOrganization, otherOrganizations, provider, credentials)
    End Function
    Public Property IdProfile As Integer Implements IViewProfileWizard.IdProfile
        Get
            Return CTRLsummary.IdProfile
        End Get
        Set(value As Integer)
            CTRLsummary.IdProfile = value
        End Set
    End Property
    Public Sub LoadRegistrationMessage(message As ProfileSubscriptionMessage) Implements IViewProfileWizard.LoadRegistrationMessage
        If Me.CTRLsummary.isCompleted Then
            Me.PageUtility.RedirectToUrl(RootObject.EndProfileWizard(message))
        Else
            MLVwizard.SetActiveView(VIWprofileError)
            Me.BTNcompleteBottom.Enabled = False
            Me.BTNcompleteTop.Enabled = False
            LTerrors.Text = Resource.getValue("ProfileSubscriptionMessage." & message.ToString)
            Me.LBstepTitle.Text = Resource.getValue("ProfileWizardStep." & ProfileWizardStep.SubscriptionError & ".Title")
            Me.LBstepDescription.Text = ""
        End If
    End Sub
#End Region


    Public Sub DisplayNoPermission() Implements IViewAddProfileToPortalWizard.DisplayNoPermission
        Me.Master.ShowNoPermission = True
    End Sub

    Public Sub LoadProfiles(ByVal user As lm.Comol.Core.DomainModel.Person) Implements IViewAddProfileToPortalWizard.LoadProfiles
        Try
            Me.Response.Cookies("AdminProfilesManagement")("Value") = user.Surname
            Me.Response.Cookies("AdminProfilesManagement")("idProvider") = user.IdDefaultProvider
            Me.Response.Cookies("AdminProfilesManagement")("IdOrganization") = SelectedOrganizationId
            Me.Response.Cookies("AdminProfilesManagement")("IdProfileType") = SelectedProfileTypeId
            Me.Response.Cookies("AdminProfilesManagement")("SearchBy") = SearchProfilesBy.Surname.ToString
            Me.Response.Cookies("AdminProfilesManagement")("StartWith") = user.FirstLetter

            Me.Response.Cookies("AdminProfilesManagement")("Status") = StatusProfile.Active.ToString
            Me.Response.Cookies("AdminProfilesManagement")("PageIndex") = 0
            Me.Response.Cookies("AdminProfilesManagement")("OrderBy") = OrderProfilesBy.SurnameAndName.ToString
            Me.Response.Cookies("AdminProfilesManagement")("Ascending") = True
            Me.Response.Cookies("AdminProfilesManagement")("PageSize") = 25

            'If (user.GetType() is TypeOf(Employee)) then

            'End If
            'Me.Response.Cookies("AdminProfilesManagement")("Value") = filters.Value
            'Me.Response.Cookies("AdminProfilesManagement")("idProvider") = filters.idProvider
            'Me.Response.Cookies("AdminProfilesManagement")("IdOrganization") = filters.IdOrganization
            'Me.Response.Cookies("AdminProfilesManagement")("IdProfileType") = filters.IdProfileType
            'Me.Response.Cookies("AdminProfilesManagement")("IdAgency") = filters.IdAgency.ToString
            'Me.Response.Cookies("AdminProfilesManagement")("SearchBy") = filters.SearchBy.ToString
            'Me.Response.Cookies("AdminProfilesManagement")("StartWith") = filters.StartWith

            'Me.Response.Cookies("AdminProfilesManagement")("Status") = filters.Status.ToString
            'Me.Response.Cookies("AdminProfilesManagement")("PageIndex") = filters.PageIndex
            'Me.Response.Cookies("AdminProfilesManagement")("PageSize") = filters.PageSize
            'Me.Response.Cookies("AdminProfilesManagement")("OrderBy") = filters.OrderBy
            'Me.Response.Cookies("AdminProfilesManagement")("Ascending") = filters.Ascending
            'Me.Response.Cookies("AdminProfilesManagement")("DisplayLoginInfo") = filters.DisplayLoginInfo
        Catch ex As Exception

        End Try
        PageUtility.RedirectToUrl(RootObject.ManagementProfilesWithFilters)
    End Sub


End Class