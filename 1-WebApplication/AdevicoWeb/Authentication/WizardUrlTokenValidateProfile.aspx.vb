Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation


Public Class WizardUrlTokenValidateProfile
    Inherits PageBase
    Implements IViewUserProfileWizardMacUrl

#Region "Context"
    Private _Presenter As MacUrlWizardPresenter
    Private ReadOnly Property CurrentPresenter() As MacUrlWizardPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MacUrlWizardPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property CurrentStep As ProfileWizardStep Implements IViewUserProfileWizard.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", ProfileWizardStep.None)
        End Get
    End Property
    Private Property AvailableSteps As List(Of ProfileWizardStep) Implements IViewUserProfileWizard.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of ProfileWizardStep))
        End Get
        Set(value As List(Of ProfileWizardStep))
            ViewState("AvailableSteps") = value
        End Set
    End Property
    Private Property SkipSteps As List(Of ProfileWizardStep) Implements IViewUserProfileWizard.SkipSteps
        Get
            Return ViewStateOrDefault("SkipSteps", New List(Of ProfileWizardStep))
        End Get
        Set(value As List(Of ProfileWizardStep))
            ViewState("SkipSteps") = value
        End Set
    End Property
    Private ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewUserProfileWizard.isSystemOutOfOrder
        Get
            Return Not Me.AccessoSistema
        End Get
    End Property
    Private Property idProvider As Long Implements IViewUserProfileWizardExternal.idProvider
        Get
            Return ViewStateOrDefault("idProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("idProvider") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedIdProvider As Long Implements IViewUserProfileWizardExternal.PreloadedIdProvider
        Get
            If IsNumeric(Request.QueryString("IdProvider")) Then
                Return CLng(Request.QueryString("IdProvider"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Public Property ExternalCredentials As dtoExternalCredentials Implements IViewUserProfileWizardMacUrl.ExternalCredentials
        Get
            Return Me.CTRLsummary.ExternalCredentials
        End Get
        Set(value As dtoExternalCredentials)
            Me.CTRLsummary.ExternalCredentials = value
        End Set
    End Property
    Public ReadOnly Property SubscriptionActive As Boolean Implements IViewUserProfileWizardExternal.SubscriptionActive
        Get
            Return SystemSettings.Login.SubscriptionActive
        End Get
    End Property
    Public Property ExternalUserInfo As String Implements IViewUserProfileWizardExternal.ExternalUserInfo
        Get
            Return Me.LBuserInfo.Text
        End Get
        Set(value As String)
            Me.LBuserInfo.Text = value
        End Set
    End Property
    Private Property TokenAttributes As List(Of dtoMacUrlUserAttribute) Implements IViewUserProfileWizardMacUrl.TokenAttributes
        Get
            Return ViewStateOrDefault("TokenAttributes", New List(Of dtoMacUrlUserAttribute))
        End Get
        Set(value As List(Of dtoMacUrlUserAttribute))
            ViewState("TokenAttributes") = value
        End Set
    End Property
    Private Property PostUserIdentifier As String Implements IViewUserProfileWizardMacUrl.PostUserIdentifier
        Get
            Return ViewStateOrDefault("PostUserIdentifier", "")
        End Get
        Set(value As String)
            ViewState("PostUserIdentifier") = value
        End Set
    End Property
    Private Property PostInternalMac As String Implements IViewUserProfileWizardMacUrl.PostInternalMac
        Get
            Return ViewStateOrDefault("PostInternalMac", "")
        End Get
        Set(value As String)
            ViewState("PostInternalMac") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView(Me.Request.Form("InternalMac"), Me.Request.Form("Identifier"), PreloadedIdProvider)
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
            '.setHyperLink(HYPinternalPage, True, True)
            'HYPinternalPage.NavigateUrl = BaseUrl & lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False)
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
    Private Function GetTokenAttributes(attributes As List(Of dtoMacUrlUserAttribute)) As List(Of dtoMacUrlUserAttribute) Implements IViewUserProfileWizardMacUrl.GetTokenAttributes
        Dim queryAttributes As New List(Of dtoMacUrlUserAttribute)
        Dim queryName As String = ""
        For Each att As dtoMacUrlUserAttribute In attributes
            queryName = att.QueryName.ToLower
            queryName = (From s As String In Request.QueryString.AllKeys Where Not String.IsNullOrEmpty(s) Select s.ToLower).ToList.Where(Function(t) t = queryName).Select(Function(s) s).FirstOrDefault()
            If Not String.IsNullOrEmpty(queryName) Then
                att.QueryValue = Request.QueryString(queryName)
            End If
            queryAttributes.Add(att)
        Next
        Return queryAttributes
    End Function

    Private Function GetUrlIdentifier(availableItems As List(Of dtoMacUrlProviderIdentifier)) As dtoMacUrlProviderIdentifier Implements IViewUserProfileWizardMacUrl.GetUrlIdentifier
        Dim dto As dtoMacUrlProviderIdentifier = Nothing
        For Each item As dtoMacUrlProviderIdentifier In availableItems
            If Request.QueryString(item.Application.QueryStringName) = item.Application.Value AndAlso Request.QueryString(item.Function.QueryStringName) = item.Function.Value Then
                dto = item
                Exit For
            End If
        Next
        Return dto
    End Function

#Region "Move Wizard"
    Public Sub GotoStep(pStep As ProfileWizardStep) Implements IViewUserProfileWizard.GotoStep
        GotoStep(pStep, False)
    End Sub
    Public Sub GotoStep(pStep As ProfileWizardStep, initialize As Boolean) Implements IViewUserProfileWizard.GotoStep
        Select Case pStep
            Case ProfileWizardStep.UnknownProfileDisclaimer
                MLVwizard.SetActiveView(VIWunknownProfile)
            Case ProfileWizardStep.InternalCredentials
                MLVwizard.SetActiveView(VIWinternalCredentials)
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
                If initialize Then : Me.CTRLprofileInfo.InitializeControl(ProfileInfo, SelectedProfileTypeId, AuthenticationProviderType.UrlMacProvider, Me.CTRLorganizations.SelectedOrganizationId)
                End If
            Case ProfileWizardStep.Privacy
                MLVwizard.SetActiveView(VIWprivacy)
                If initialize Then : Me.CTRLprivacy.InitializeControl()
                End If
            Case ProfileWizardStep.Summary
                MLVwizard.SetActiveView(VIWcomplete)
                Me.CTRLsummary.InitializeControlForSubscription(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.SelectedProfileName, SelectedProfileTypeId, AuthenticationProviderType.UrlMacProvider, Me.PostUserIdentifier, idProvider)
        End Select
        Me.LBuserInfo.Visible = (pStep = ProfileWizardStep.UnknownProfileDisclaimer AndAlso Not String.IsNullOrEmpty(LBuserInfo.Text))
        Me.BTNbackBottom.Visible = (pStep <> ProfileWizardStep.UnknownProfileDisclaimer)
        Me.BTNbackTop.Visible = (pStep <> ProfileWizardStep.UnknownProfileDisclaimer)
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

        Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & pStep.ToString & ".Description")
        If pStep <> ProfileWizardStep.Summary Then
            Me.Page.Form.DefaultButton = Me.BTNnextBottom.UniqueID
            Me.Master.Page.Form.DefaultButton = Me.BTNnextBottom.UniqueID
        Else
            Me.Page.Form.DefaultButton = Me.BTNcompleteTop.UniqueID
            Me.Master.Page.Form.DefaultButton = Me.BTNcompleteTop.UniqueID
        End If
    End Sub
    Public Sub InitializeUnknownProfileStep(dProvider As AuthenticationProviderType, providers As List(Of AuthenticationProviderType)) Implements IViewUserProfileWizardExternal.InitializeUnknownProfileStep
        MLVwizard.SetActiveView(VIWunknownProfile)
        CTRLunknownProfile.InitializeControl(dProvider, providers)

    End Sub
    Public Sub InitializeStep(pStep As ProfileWizardStep) Implements IViewUserProfileWizard.InitializeStep
        Select Case pStep
            'Case ProfileWizardStep.UnknownProfileDisclaimer
            '    CTRLunknownProfile.InitializeControl()
            Case ProfileWizardStep.InternalCredentials
                CTRLinternalCredentials.InitializeControl()
            Case ProfileWizardStep.OrganizationSelector
                Me.CTRLorganizations.InitializeControl(False)
            Case ProfileWizardStep.ProfileTypeSelector
                CTRLprofileTypes.InitializeControl(False, SelectedOrganizationId, SelectedProfileTypeId)
            Case ProfileWizardStep.ProfileUserData
                CTRLprofileInfo.InitializeControl(ProfileInfo, SelectedProfileTypeId, AuthenticationProviderType.UrlMacProvider, Me.CTRLorganizations.SelectedOrganizationId)
            Case ProfileWizardStep.Privacy
                CTRLprivacy.InitializeControl()
            Case ProfileWizardStep.Summary
                Me.CTRLsummary.InitializeControlForSubscription(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.SelectedProfileName, SelectedProfileTypeId, AuthenticationProviderType.UrlMacProvider, PostUserIdentifier, idProvider)
        End Select
    End Sub

    Public Function IsInitialized(pStep As ProfileWizardStep) As Boolean Implements IViewUserProfileWizard.IsInitialized
        Select Case pStep
            Case ProfileWizardStep.UnknownProfileDisclaimer
                Return CTRLunknownProfile.isInitialized
            Case ProfileWizardStep.InternalCredentials
                Return CTRLunknownProfile.isInitialized
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

        Me.CTRLunknownProfile.ReloadLanguageSettings(oLingua)
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
            Me.BTNcompleteBottom.Visible = True
            Me.BTNcompleteTop.Visible = True
            Me.LTtitleWizardInternal.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
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
    Private Sub DisplayPrivacyPolicy(userId As Integer, idProvider As Long, providerUrl As String, internalPage As Boolean) Implements IViewUserProfileWizardExternal.DisplayPrivacyPolicy
        Me.PageUtility.PreloggedUserId = userId
        Me.PageUtility.PreloggedProviderId = idProvider
        Me.PageUtility.PreloggedProviderUrl = IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.PolicyManagement.RootObject.AcceptLogonPolicy)
    End Sub
    Public Sub DisplaySystemOutOfOrder() Implements IViewUserProfileWizard.DisplaySystemOutOfOrder
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage, SystemSettings.Login.isSSLloginRequired)
    End Sub


#Region "Step 1 UnknownProfile"
    Public ReadOnly Property SelectedProvider As AuthenticationProviderType Implements IViewUserProfileWizardMacUrl.SelectedProvider
        Get
            Return CTRLunknownProfile.SelectedProvider
        End Get
    End Property
    Public Property SelectedStartLanguageID As Integer Implements IViewUserProfileWizard.SelectedStartLanguageID
        Get
            Return Me.CTRLunknownProfile.SelectedLanguageId
        End Get
        Set(value As Integer)
            Me.CTRLunknownProfile.SelectedLanguageId = value
        End Set
    End Property

    Private Sub CTRLdisclaimer_UpdateUserLanguage(oLingua As Comol.Entity.Lingua) Handles CTRLunknownProfile.UpdateUserLanguage
        UpdateLanguagers(oLingua)
    End Sub
#End Region

#Region "Step 2 Internal"
    Public Sub DisplayInvalidCredentials() Implements IViewUserProfileWizardMacUrl.DisplayInvalidCredentials
        CTRLinternalCredentials.DisplayInvalidCredentials()
    End Sub

    Public ReadOnly Property GetInternalCredentials As dtoInternalCredentials Implements IViewUserProfileWizardMacUrl.GetInternalCredentials
        Get
            Return New dtoInternalCredentials() With {.Login = CTRLinternalCredentials.Login, .Password = CTRLinternalCredentials.Password}
        End Get
    End Property
    Public Sub DisplayInternalCredentialsMessage(message As ProfileSubscriptionMessage) Implements IViewUserProfileWizardExternal.DisplayInternalCredentialsMessage
        CTRLinternalCredentials.DisplayInternalCredentialsMessage(message)
    End Sub
#End Region

#Region "Step 3 Organization"
    Private Property TokenIdOrganization As Integer Implements IViewUserProfileWizardMacUrl.TokenIdOrganization
        Get
            Return ViewStateOrDefault("TokenIdOrganization", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("TokenIdOrganization") = value
            Me.CTRLsummary.ProviderSelectOrganization = (value > 0)
        End Set
    End Property
    Private ReadOnly Property AvailableOrganizationsId As List(Of Integer) Implements IViewUserProfileWizard.AvailableOrganizationsId
        Get
            Return Me.CTRLorganizations.AvailableOrganizationsId
        End Get
    End Property
    Private Property SelectedOrganizationId As Integer Implements IViewUserProfileWizard.SelectedOrganizationId
        Get
            If TokenIdOrganization > 0 Then
                Return TokenIdOrganization
            Else
                Return Me.CTRLorganizations.SelectedOrganizationId
            End If
        End Get
        Set(value As Integer)
            Me.CTRLorganizations.SelectedOrganizationId = value
        End Set
    End Property
    Private Sub InitializeProfileTypeSelector(OtherRolesId As List(Of Integer), IdDefaultRole As Integer) Implements IViewUserProfileWizardExternal.InitializeProfileTypeSelector
        Me.CTRLprofileTypes.InitializeControl(Me.SelectedOrganizationId, SelectedProfileTypeId, OtherRolesId, IdDefaultRole)
    End Sub
#End Region

#Region "Step 4 ProfileTypeSelector"
    Private Property TokenIdProfileType As Integer Implements IViewUserProfileWizardMacUrl.TokenIdProfileType
        Get
            Return ViewStateOrDefault("TokenIdProfileType", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("TokenIdProfileType") = value
            Me.CTRLsummary.ProviderSelectProfileType = (value > 0)
        End Set
    End Property
    Private Property SelectedProfileTypeId As Integer Implements IViewUserProfileWizard.SelectedProfileTypeId
        Get
            If TokenIdProfileType > 0 Then
                Return TokenIdProfileType
            Else
                Return Me.CTRLprofileTypes.SelectedProfileTypeId
            End If
        End Get
        Set(value As Integer)
            Me.CTRLprofileTypes.SelectedProfileTypeId = value
        End Set
    End Property
    Private ReadOnly Property AvailableProfileTypes As List(Of Integer) Implements IViewUserProfileWizard.AvailableProfileTypes
        Get
            Return Me.CTRLprofileTypes.AvailableProfileTypes
        End Get
    End Property
    Private ReadOnly Property SelectedProfileName As String Implements IViewUserProfileWizard.SelectedProfileName
        Get
            If TokenIdProfileType > 0 Then
                Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList

                Return oUserTypes.Where(Function(t) t.ID = TokenIdProfileType).Select(Function(t) t.Descrizione).FirstOrDefault()
            Else
                Return Me.CTRLprofileTypes.SelectedProfileName
            End If
        End Get
    End Property
#End Region

#Region "Step 5 Profile Info"
    Private Sub GotoStepProfileInfo(profile As dtoBaseProfile) Implements IViewUserProfileWizardExternal.GotoStepProfileInfo
        Dim dto As dtoBaseProfile = profile
        If CTRLprofileInfo.isInitialized Then
            dto = ProfileInfo
        Else
            dto = profile
        End If
        If String.IsNullOrEmpty(dto.Mail) Then
            dto.Mail = profile.Mail
        End If
        If String.IsNullOrEmpty(dto.Name) Then
            dto.Name = profile.Name
        End If
        If String.IsNullOrEmpty(dto.Surname) Then
            dto.Surname = profile.Surname
        End If
        Me.CTRLprofileInfo.InitializeControl(dto, SelectedProfileTypeId, AuthenticationProviderType.UrlMacProvider, SelectedOrganizationId)
        Me.MLVwizard.SetActiveView(VIWuserInfo)
        Me.ViewState("CurrentStep") = ProfileWizardStep.ProfileUserData
        Me.LTtitleWizardInternal.Text = String.Format(Resource.getValue("ProfileWizardStep." & ProfileWizardStep.ProfileUserData.ToString & ".Title"), GetStepNumber(Me.CurrentStep), Me.SelectedProfileName)
        Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & ProfileWizardStep.ProfileUserData.ToString & ".Description")

        Me.BTNbackBottom.Visible = SkipSteps.Contains(ProfileWizardStep.InternalCredentials)
        Me.BTNbackTop.Visible = SkipSteps.Contains(ProfileWizardStep.InternalCredentials)
    End Sub
    Private Property ProfileInfo As dtoBaseProfile Implements IViewUserProfileWizard.ProfileInfo
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
    Private Sub LoadProfileInfoError(errors As List(Of ProfilerError)) Implements IViewUserProfileWizard.LoadProfileInfoError
        Me.CTRLprofileInfo.LoadProfileInfoError(errors)
    End Sub
    Private Sub UnloadProfileInfoError() Implements IViewUserProfileWizard.UnloadProfileInfoError
        Me.CTRLprofileInfo.UnloadProfileInfoError()
    End Sub
    Private Sub SaveUserPassword() Implements IViewUserProfileWizard.SaveUserPassword
        Me.CTRLprofileInfo.SaveUserPassword()
    End Sub
    Private Sub DisableInput(items As List(Of ProfileAttributeType)) Implements IViewUserProfileWizardMacUrl.DisableInput
        Me.CTRLprofileInfo.DisableInput(items)
    End Sub
#End Region

#Region "Step 6 Privacy"
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

#Region "Step 7 Summary"
    Public Function CreateProfile(profile As dtoBaseProfile, idProfileType As Integer, profileName As String, idOrganization As Integer, authentication As AuthenticationProviderType, idProvider As Long) As ProfileSubscriptionMessage Implements IViewUserProfileWizard.CreateProfile
        Dim dto As dtoBaseProfile = ProfileInfo
        If String.IsNullOrEmpty(dto.Mail) Then
            dto.Mail = profile.Mail
        End If
        If String.IsNullOrEmpty(dto.Name) Then
            dto.Name = profile.Name
        End If
        If String.IsNullOrEmpty(dto.Surname) Then
            dto.Surname = profile.Surname
        End If
        If String.IsNullOrEmpty(dto.TaxCode) AndAlso String.IsNullOrEmpty(profile.TaxCode) Then
            dto.TaxCode = profile.TaxCode
        End If
        Return Me.CTRLsummary.CreateProfile(dto, idProfileType, profileName, idOrganization, authentication, idProvider, ExternalCredentials)
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
            Me.BTNcompleteBottom.Visible = False
            Me.BTNcompleteTop.Visible = False
            LTerrors.Text = Resource.getValue("ProfileSubscriptionMessage." & message.ToString)
            Me.LTtitleWizardInternal.Text = Resource.getValue("ProfileWizardStep." & ProfileWizardStep.SubscriptionError & ".Title")
            Me.LBstepDescription.Text = ""
        End If
    End Sub
#End Region

    Public Sub LogonUser(user As lm.Comol.Core.DomainModel.Person, idProvider As Long, providerUrl As String, internalPage As Boolean, idUserDefaultIdOrganization As Int32) Implements IViewUserProfileWizard.LogonUser
        Me.PageUtility.LogonUser(user, idProvider, IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl, idUserDefaultIdOrganization)
    End Sub

    Public Sub GotoDefaultPage() Implements IViewUserProfileWizardMacUrl.GotoDefaultPage
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
    End Sub

    Public Sub GotoRemoteLogonPage(url As String) Implements IViewUserProfileWizardMacUrl.GotoRemoteLogonPage
        If String.IsNullOrEmpty(url) Then
            Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
        Else
            Response.Redirect(url)
        End If
    End Sub

End Class