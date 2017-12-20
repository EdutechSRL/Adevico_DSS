Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation


Public Class WizardInternalProfile
    Inherits PageBase
    Implements IViewUserProfileWizardInternal

#Region "Context"
    Private _Presenter As UserProfileWizardInternalPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As UserProfileWizardInternalPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UserProfileWizardInternalPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property CurrentStep As ProfileWizardStep Implements IViewUserProfileWizard.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", ProfileWizardStep.None)
        End Get
    End Property
    Public Property AvailableSteps As List(Of ProfileWizardStep) Implements IViewUserProfileWizard.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of ProfileWizardStep))
        End Get
        Set(value As List(Of ProfileWizardStep))
            ViewState("AvailableSteps") = value
        End Set
    End Property
    Public Property SkipSteps As List(Of ProfileWizardStep) Implements IViewUserProfileWizard.SkipSteps
        Get
            Return ViewStateOrDefault("SkipSteps", New List(Of ProfileWizardStep))
        End Get
        Set(value As List(Of ProfileWizardStep))
            ViewState("SkipSteps") = value
        End Set
    End Property
    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewUserProfileWizard.isSystemOutOfOrder
        Get
            Return Not Me.AccessoSistema
        End Get
    End Property
    Public ReadOnly Property SubscriptionActive As Boolean Implements IViewUserProfileWizard.SubscriptionActive
        Get
            Return SystemSettings.Login.SubscriptionActive
        End Get
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
            Me.Master.ShowLanguage = False
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
            .setLiteral(LTtitleWizardInternal)
            .setHyperLink(HYPinternalPage, True, True)
            Dim url As String = BaseUrl
            If SystemSettings.Login.isSSLloginRequired AndAlso Me.Request.Url.AbsoluteUri.StartsWith("http://") Then
                url = PageUtility.SecureApplicationUrlBase
            End If
            HYPinternalPage.NavigateUrl = url & lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False)
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
    Public Sub GotoStep(pStep As ProfileWizardStep) Implements IViewUserProfileWizard.GotoStep
        GotoStep(pStep, False)
    End Sub
    Public Sub GotoStep(pStep As ProfileWizardStep, initialize As Boolean) Implements IViewUserProfileWizard.GotoStep
        Select Case pStep
            Case ProfileWizardStep.StandardDisclaimer
                MLVwizard.SetActiveView(VIWdisclaimer)
                If initialize Then : CTRLdisclaimer.InitializeControl()
                End If
            Case ProfileWizardStep.OrganizationSelector
                MLVwizard.SetActiveView(VIWorganization)
                If initialize Then : Me.CTRLorganizations.InitializeControl(False)
                End If
            Case ProfileWizardStep.ProfileTypeSelector
                MLVwizard.SetActiveView(VIWprofileTypes)
                If initialize Then : Me.CTRLprofileTypes.InitializeControl(False, Me.CTRLorganizations.SelectedOrganizationId, SelectedProfileTypeId)
                End If
            Case ProfileWizardStep.ProfileUserData
                MLVwizard.SetActiveView(VIWuserInfo)
                If initialize Then : Me.CTRLprofileInfo.InitializeControl(ProfileInfo, SelectedProfileTypeId, AuthenticationProviderType.Internal, Me.CTRLorganizations.SelectedOrganizationId)
                End If
            Case ProfileWizardStep.Privacy
                MLVwizard.SetActiveView(VIWprivacy)
                If initialize Then : Me.CTRLprivacy.InitializeControl()
                End If
            Case ProfileWizardStep.Summary
                MLVwizard.SetActiveView(VIWcomplete)
                Me.CTRLsummary.InitializeControlForSubscription(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.SelectedProfileName, SelectedProfileTypeId, AuthenticationProviderType.Internal, 0)
        End Select

        Me.BTNbackBottom.Visible = (pStep <> ProfileWizardStep.StandardDisclaimer)
        Me.BTNbackTop.Visible = (pStep <> ProfileWizardStep.StandardDisclaimer)
        Me.BTNcompleteBottom.Visible = (pStep = ProfileWizardStep.Summary)
        Me.BTNcompleteTop.Visible = (pStep = ProfileWizardStep.Summary)
        Me.BTNnextBottom.Visible = (pStep <> ProfileWizardStep.Summary)
        Me.BTNnextTop.Visible = (pStep <> ProfileWizardStep.Summary)

        Me.ViewState("CurrentStep") = pStep
        If Me.CurrentStep = ProfileWizardStep.ProfileUserData Then
            Me.LTtitleWizardInternal.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep), Me.CTRLprofileTypes.SelectedProfileName)
        Else
            Me.LTtitleWizardInternal.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
        End If
        If pStep = ProfileWizardStep.InternalCredentials Then
            Me.BTNnextBottom.OnClientClick = "$('#DVupdate').show();" & Me.BTNnextBottom.OnClientClick
            Me.BTNbackTop.OnClientClick = "$('#DVupdate').show();" & Me.BTNbackTop.OnClientClick
            LTwaitingLogon.Text = Resource.getValue("WaitingLogon")
        ElseIf Me.BTNnextBottom.OnClientClick.Contains("$('#DVupdate').show();") Then
            Me.BTNnextBottom.OnClientClick = Replace(Me.BTNnextBottom.OnClientClick, "$('#DVupdate').show();", "")
            Me.BTNbackTop.OnClientClick = Replace(Me.BTNbackTop.OnClientClick, "$('#DVupdate').show();", "")
            LTwaitingLogon.Text = Resource.getValue("WaitingProfile")
        End If
        Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & pStep.ToString & ".Description")
        If pStep <> ProfileWizardStep.Summary Then
            Me.Page.Form.DefaultButton = Me.BTNnextBottom.UniqueID
            Me.Master.Page.Form.DefaultButton = Me.BTNnextBottom.UniqueID
        Else
            Me.Page.Form.DefaultButton = Me.BTNcompleteTop.UniqueID
            Me.Master.Page.Form.DefaultButton = Me.BTNcompleteTop.UniqueID
        End If

    End Sub

    Public Sub InitializeStep(pStep As ProfileWizardStep) Implements IViewUserProfileWizard.InitializeStep
        Select Case pStep
            Case ProfileWizardStep.StandardDisclaimer
                CTRLdisclaimer.InitializeControl()
            Case ProfileWizardStep.OrganizationSelector
                Me.CTRLorganizations.InitializeControl(False)
            Case ProfileWizardStep.ProfileTypeSelector
                CTRLprofileTypes.InitializeControl(False, SelectedOrganizationId, SelectedProfileTypeId)
            Case ProfileWizardStep.ProfileUserData
                CTRLprofileInfo.InitializeControl(ProfileInfo, SelectedProfileTypeId, AuthenticationProviderType.Internal, Me.CTRLorganizations.SelectedOrganizationId)
            Case ProfileWizardStep.Privacy
                CTRLprivacy.InitializeControl()
            Case ProfileWizardStep.Summary
                Me.CTRLsummary.InitializeControlForSubscription(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.SelectedProfileName, SelectedProfileTypeId, AuthenticationProviderType.Internal, 0)
        End Select
    End Sub

    Public Function IsInitialized(pStep As ProfileWizardStep) As Boolean Implements IViewUserProfileWizard.IsInitialized
        Select Case pStep
            Case ProfileWizardStep.StandardDisclaimer
                Return CTRLdisclaimer.isInitialized
            Case ProfileWizardStep.OrganizationSelector
                Return CTRLorganizations.isInitialized
            Case ProfileWizardStep.ProfileTypeSelector
                Return CTRLprofileTypes.isInitialized
            Case ProfileWizardStep.ProfileUserData
                Return CTRLprofileInfo.isInitialized
            Case ProfileWizardStep.Privacy
                Return CTRLprivacy.isInitialized
            Case ProfileWizardStep.Summary
                Return CTRLsummary.isInitialized
            Case Else
                Return False
        End Select
    End Function

    Private Sub UpdateLanguagers(ByVal oLingua As Lingua)
        MyBase.OverloadLanguage(oLingua)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()

        If Me.CurrentStep = ProfileWizardStep.ProfileUserData Then
            Me.LTtitleWizardInternal.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep), Me.CTRLprofileTypes.SelectedProfileName)
        Else
            Me.LTtitleWizardInternal.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
        End If

        Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Description")

        Me.CTRLdisclaimer.ReloadLanguageSettings(oLingua)
        Me.CTRLorganizations.ReloadLanguageSettings(oLingua)
        Me.CTRLprofileTypes.ReloadLanguageSettings(oLingua)

        Me.CTRLprivacy.ReloadLanguageSettings(oLingua)
        Me.CTRLprofileInfo.ReloadLanguageSettings(oLingua)
        Me.CTRLsummary.ReloadLanguageSettings(oLingua)
    End Sub

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
            Me.LTtitleWizardInternal.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
            Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Description")
        Else
            Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
        End If
    End Sub

    Private Sub BTNcompleteBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteBottom.Click, BTNcompleteTop.Click
        If Me.CTRLsummary.isCompleted Then
            Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
        ElseIf Not SubscriptionActive Then
            Me.PageUtility.RedirectToUrl(RootObject.EndProfileWizard(ProfileSubscriptionMessage.SubscriptionNotActive))
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

    Public Sub DisplaySystemOutOfOrder() Implements IViewUserProfileWizard.DisplaySystemOutOfOrder
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
    End Sub


#Region "Step 1 Disclaimer"
    Public Property SelectedStartLanguageID As Integer Implements IViewUserProfileWizard.SelectedStartLanguageID
        Get
            Return Me.CTRLdisclaimer.SelectedLanguageId
        End Get
        Set(value As Integer)
            Me.CTRLdisclaimer.SelectedLanguageId = value
        End Set
    End Property

    Private Sub CTRLdisclaimer_UpdateUserLanguage(oLingua As Comol.Entity.Lingua) Handles CTRLdisclaimer.UpdateUserLanguage
        UpdateLanguagers(oLingua)
    End Sub
#End Region

#Region "Step 2 Organization"
    Public ReadOnly Property AvailableOrganizationsId As List(Of Integer) Implements IViewUserProfileWizard.AvailableOrganizationsId
        Get
            Return Me.CTRLorganizations.AvailableOrganizationsId
        End Get
    End Property
    Public Property SelectedOrganizationId As Integer Implements IViewUserProfileWizard.SelectedOrganizationId
        Get
            Return Me.CTRLorganizations.SelectedOrganizationId
        End Get
        Set(value As Integer)
            Me.CTRLorganizations.SelectedOrganizationId = value
        End Set
    End Property
#End Region

#Region "Step 3 ProfileTypeSelector"
    'Public Sub InitializeProfileTypeSelector(IdOrganization As Integer, IdProfileType As Integer) Implements lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation.IViewUserProfileWizard.InitializeProfileTypeSelector

    'End Sub
    Public Property SelectedProfileTypeId As Integer Implements IViewUserProfileWizard.SelectedProfileTypeId
        Get
            Return Me.CTRLprofileTypes.SelectedProfileTypeId
        End Get
        Set(value As Integer)
            Me.CTRLprofileTypes.SelectedProfileTypeId = value
        End Set
    End Property
    Public ReadOnly Property AvailableProfileTypes As List(Of Integer) Implements IViewUserProfileWizard.AvailableProfileTypes
        Get
            Return Me.CTRLprofileTypes.AvailableProfileTypes
        End Get
    End Property
    Public ReadOnly Property SelectedProfileName As String Implements IViewUserProfileWizard.SelectedProfileName
        Get
            Return Me.CTRLprofileTypes.SelectedProfileName
        End Get
    End Property
#End Region

#Region "Step 4 Profile Info"
    Public Property ProfileInfo As dtoBaseProfile Implements IViewUserProfileWizard.ProfileInfo
        Get
            Return Me.CTRLprofileInfo.CurrentUserInfo
        End Get
        Set(value As dtoBaseProfile)
            Me.CTRLprofileInfo.CurrentUserInfo = value
        End Set
    End Property

    Private Sub CTRLprofileInfo_UpdateUserLanguage(language As Comol.Entity.Lingua) Handles CTRLprofileInfo.UpdateUserLanguage
        Me.UpdateLanguagers(language)
    End Sub
    Public Sub LoadProfileInfoError(errors As List(Of ProfilerError)) Implements IViewUserProfileWizard.LoadProfileInfoError
        Me.CTRLprofileInfo.LoadProfileInfoError(errors)
    End Sub
    Public Sub UnloadProfileInfoError() Implements IViewUserProfileWizard.UnloadProfileInfoError
        Me.CTRLprofileInfo.UnloadProfileInfoError()
    End Sub

    Public Sub SaveUserPassword() Implements IViewUserProfileWizard.SaveUserPassword
        Me.CTRLprofileInfo.SaveUserPassword()
    End Sub
#End Region

#Region "Step 5 Privacy"
    Public Property GetPolicyInfo As List(Of lm.Comol.Core.BaseModules.PolicyManagement.dtoUserPolicyInfo) Implements IViewUserProfileWizard.GetPolicyInfo
        Get
            Return Me.CTRLprivacy.GetPolicyInfo
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.PolicyManagement.dtoUserPolicyInfo))
            Me.CTRLprivacy.GetPolicyInfo = value
        End Set
    End Property
    Public ReadOnly Property AcceptedMandatoryPolicy As Boolean Implements IViewUserProfileWizard.AcceptedMandatoryPolicy
        Get
            Return CTRLprivacy.AcceptedMandatoryPolicy
        End Get
    End Property
#End Region

#Region "Step 6 Summary"
    Public Function CreateProfile(profile As dtoBaseProfile, idProfileType As Integer, profileName As String, idOrganization As Integer, authentication As AuthenticationProviderType, idProvider As Long) As ProfileSubscriptionMessage Implements IViewUserProfileWizard.CreateProfile
        Return Me.CTRLsummary.CreateProfile(profile, idProfileType, profileName, idOrganization, authentication, idProvider)
    End Function
    Public Property IdProfile As Integer Implements IViewUserProfileWizard.IdProfile
        Get
            Return CTRLsummary.IdProfile
        End Get
        Set(value As Integer)
            CTRLsummary.IdProfile = value
        End Set
    End Property
    Public Sub LoadRegistrationMessage(message As ProfileSubscriptionMessage) Implements IViewUserProfileWizard.LoadRegistrationMessage
        If Me.CTRLsummary.isCompleted Then
            Me.PageUtility.RedirectToUrl(RootObject.EndProfileWizard(message))
        Else
            MLVwizard.SetActiveView(VIWprofileError)
            Me.BTNcompleteBottom.Enabled = False
            Me.BTNcompleteTop.Enabled = False
            LTerrors.Text = Resource.getValue("ProfileSubscriptionMessage." & message.ToString)
            Me.LTtitleWizardInternal.Text = Resource.getValue("ProfileWizardStep." & ProfileWizardStep.SubscriptionError & ".Title")
            Me.LBstepDescription.Text = ""
        End If
    End Sub
#End Region

    Public Sub LogonUser(user As lm.Comol.Core.DomainModel.Person, idProvider As Long, providerUrl As String, internalPage As Boolean, idUserDefaultIdOrganization As Int32) Implements IViewUserProfileWizard.LogonUser
        Me.PageUtility.LogonUser(user, idProvider, IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl, idUserDefaultIdOrganization)
    End Sub

    Public Sub LoadDefaultLogonPage() Implements IViewUserProfileWizardInternal.LoadDefaultLogonPage
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
    End Sub
End Class