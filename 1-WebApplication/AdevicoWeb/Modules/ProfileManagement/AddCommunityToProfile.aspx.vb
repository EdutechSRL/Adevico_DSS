Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class AddCommunityToProfile
    Inherits PageBase
    Implements IViewAddCommunityToProfile





#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As AddCommunityToProfilePresenter
    Private ReadOnly Property CurrentPresenter() As AddCommunityToProfilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddCommunityToProfilePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property PreloadedIdProfile As Integer Implements IViewAddCommunityToProfile.PreloadedIdProfile
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public Property SkipSteps As List(Of CommunitySubscriptionWizardStep) Implements IViewAddCommunityToProfile.SkipSteps
        Get
            Return ViewStateOrDefault("SkipSteps", New List(Of CommunitySubscriptionWizardStep))
        End Get
        Set(value As List(Of CommunitySubscriptionWizardStep))
            ViewState("SkipSteps") = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewAddCommunityToProfile.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagement.Visible = value
            Me.HYPbackToManagement.NavigateUrl = BaseUrl & lm.Comol.Core.BaseModules.ProfileManagement.RootObject.ManagementProfilesWithFilters()
        End Set
    End Property

    Public WriteOnly Property AllowSubscribeToCommunity As Boolean Implements IViewAddCommunityToProfile.AllowSubscribeToCommunity
        Set(value As Boolean)

        End Set
    End Property
    Public Property AvailableSteps As List(Of CommunitySubscriptionWizardStep) Implements IViewAddCommunityToProfile.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of CommunitySubscriptionWizardStep))
        End Get
        Set(value As List(Of CommunitySubscriptionWizardStep))
            ViewState("AvailableSteps") = value
        End Set
    End Property


    Public Property CurrentStep As CommunitySubscriptionWizardStep Implements IViewAddCommunityToProfile.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", CommunitySubscriptionWizardStep.SelectCommunities)
        End Get
        Set(value As CommunitySubscriptionWizardStep)
            ViewState("CurrentStep") = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewAddCommunityToProfile.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
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
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(PreloadedIdProfile)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AddCommunityToProfile", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleAddCommunityToProfile")
            Me.Master.ServiceNopermission = .getValue("serviceTitleAddCommunityToProfileNopermission")
            .setHyperLink(HYPbackToManagement, True, True)

            .setButton(BTNbackBottom, True, , , True)
            .setButton(BTNbackTop, True, , , True)
            .setButton(BTNnextBottom, True, , , True)
            .setButton(BTNnextTop, True, , , True)
            .setButton(BTNcompleteTop, True, , , True)
            .setButton(BTNcompleteBottom, True, , , True)
            .setLiteral(LTprogress)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub LoadProfileName(displayName As String) Implements IViewAddCommunityToProfile.LoadProfileName
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleAddCommunityToNamedProfile"), displayName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitleAddCommunityToNamedProfile"), displayName)
    End Sub

#Region "Move Wizard"

    Public Sub GotoStep(pStep As CommunitySubscriptionWizardStep) Implements IViewAddCommunityToProfile.GotoStep
        GotoStep(pStep, False)
    End Sub

    Public Sub GotoStep(pStep As CommunitySubscriptionWizardStep, initialize As Boolean) Implements IViewAddCommunityToProfile.GotoStep
        Select Case pStep
            Case CommunitySubscriptionWizardStep.SelectCommunities
                MLVwizard.SetActiveView(VIWcommunities)
                If initialize Then : InitializeStep(pStep)
                End If
            Case CommunitySubscriptionWizardStep.SubscriptionsSettings
                MLVwizard.SetActiveView(VIWsubscriptionsSettings)
                If initialize Then : InitializeStep(pStep)
                End If
            Case CommunitySubscriptionWizardStep.RemoveSubscriptions
                MLVwizard.SetActiveView(VIWremoveSubscription)
                If initialize Then : InitializeStep(pStep)
                End If
            Case CommunitySubscriptionWizardStep.Summary
                MLVwizard.SetActiveView(VIWcomplete)
                '   Me.CTRLsummary.InitializeForManagement(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.CTRLorganizations.OtherOrganizationsToSubscribe, Me.SelectedProfileName, SelectedProfileTypeId, Me.CurrentPresenter.GetAuthenticationProvider(SelectedProviderId), Me.GetExternalCredentials)
        End Select

        Me.CurrentStep = pStep
        Me.BTNbackBottom.Visible = (pStep <> CommunitySubscriptionWizardStep.SelectCommunities)
        Me.BTNbackTop.Visible = (pStep <> CommunitySubscriptionWizardStep.SelectCommunities)
        Me.BTNcompleteBottom.Visible = (pStep = CommunitySubscriptionWizardStep.Summary)
        Me.BTNcompleteTop.Visible = (pStep = CommunitySubscriptionWizardStep.Summary)
        Me.BTNnextBottom.Visible = (pStep <> CommunitySubscriptionWizardStep.Summary)
        Me.BTNnextTop.Visible = (pStep <> CommunitySubscriptionWizardStep.Summary)

        Me.LBstepTitle.Text = String.Format(Resource.getValue("CommunitySubscriptionWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
        Me.LBstepDescription.Text = Resource.getValue("CommunitySubscriptionWizardStep." & pStep.ToString & ".Description")
    End Sub

    Public Sub InitializeStep(pStep As CommunitySubscriptionWizardStep) Implements IViewAddCommunityToProfile.InitializeStep
        Select Case pStep
            Case CommunitySubscriptionWizardStep.SelectCommunities
                Me.CTRLcommunities.InitializeControl(IdProfile, PreloadedAvailability)
            Case CommunitySubscriptionWizardStep.SubscriptionsSettings
                Me.CTRLsubscriptions.InitializeControl(IdProfile, CTRLcommunities.SelectedCommunities)
            Case CommunitySubscriptionWizardStep.RemoveSubscriptions

            Case CommunitySubscriptionWizardStep.Summary

        End Select
    End Sub

    Private Function GetStepNumber(pStep As CommunitySubscriptionWizardStep) As Integer
        Dim Number As Integer = 1
        Dim list As List(Of CommunitySubscriptionWizardStep) = Me.AvailableSteps
        Dim toSkip As List(Of CommunitySubscriptionWizardStep) = Me.SkipSteps
        For Each item As CommunitySubscriptionWizardStep In (From i In list Where toSkip.Contains(i) = False Select i).ToList
            If item = pStep Then
                Return Number
            Else
                Number += 1
            End If
        Next

    End Function

    Private Sub BTNbackBottom_Click(sender As Object, e As System.EventArgs) Handles BTNbackBottom.Click, BTNbackTop.Click
        'If MLVwizard.GetActiveView Is VIWprofileError Then
        '    Me.MLVwizard.SetActiveView(VIWcomplete)
        '    Me.BTNcompleteBottom.Enabled = True
        '    Me.BTNcompleteTop.Enabled = True
        '    Me.LBstepTitle.Text = String.Format(Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
        '    Me.LBstepDescription.Text = Resource.getValue("ProfileWizardStep." & Me.CurrentStep.ToString & ".Description")
        'Else
        If Me.MLVwizard.GetActiveView Is VIWerror Then
            MLVwizard.SetActiveView(VIWcomplete)

            Me.BTNcompleteBottom.Visible = True
            Me.BTNcompleteTop.Visible = True
            Me.CurrentStep = CommunitySubscriptionWizardStep.Summary
            Me.LBstepTitle.Text = String.Format(Resource.getValue("CommunitySubscriptionWizardStep." & CommunitySubscriptionWizardStep.Summary.ToString & ".Title"), GetStepNumber(CommunitySubscriptionWizardStep.Summary))
            Me.LBstepDescription.Text = Resource.getValue("CommunitySubscriptionWizardStep." & CommunitySubscriptionWizardStep.Summary.ToString & ".Description")
        Else
            Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
        End If

        'End If
    End Sub

    Private Sub BTNcompleteBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteBottom.Click, BTNcompleteTop.Click
        'If Me.CTRLsummary.isCompleted Then
        '    Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
        'Else
        '    Me.CurrentPresenter.CreateProfile(ProfileInfo, SelectedProfileTypeId, SelectedProfileName, SelectedOrganizationId)
        'End If
        If isCompleted Then
            Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.ProfileManagement.RootObject.ManagementProfilesWithFilters)
        Else
            Me.CurrentPresenter.SaveChanges(Me.CTRLsubscriptions.SelectedSubscriptions, Me.CTRLunsubscriptions.SelectedUnsubscriptions)
        End If

    End Sub

    Private Sub BTNnextBottom_Click(sender As Object, e As System.EventArgs) Handles BTNnextBottom.Click, BTNnextTop.Click
        Me.CurrentPresenter.MoveToNextStep(CurrentStep)
    End Sub
#End Region


#Region "Step 1 - Community selection"
    Public Property CurrentAvailability As CommunityAvailability Implements IViewAddCommunityToProfile.CurrentAvailability
        Get
            Return Me.CTRLcommunities.CurrentAvailability
        End Get
        Set(value As CommunityAvailability)
            Me.CTRLcommunities.CurrentAvailability = value
        End Set
    End Property

    Public ReadOnly Property PreloadedAvailability As CommunityAvailability Implements IViewAddCommunityToProfile.PreloadedAvailability
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CommunityAvailability).GetByString(Me.Request.QueryString("Availability"), CommunityAvailability.None)
        End Get
    End Property
    Public ReadOnly Property CurrentStatus As lm.Comol.Core.Communities.CommunityStatus Implements IViewAddCommunityToProfile.CurrentStatus
        Get
            Return Me.CTRLcommunities.CurrentStatus
        End Get
    End Property
    Public Function SelectedCommunities() As List(Of dtoBaseCommunityNode) Implements IViewAddCommunityToProfile.SelectedCommunities
        Return Me.CTRLcommunities.SelectedCommunities
    End Function
    Public ReadOnly Property CommunityFilters As dtoCommunitiesFilters Implements IViewAddCommunityToProfile.CommunityFilters
        Get
            Return Me.CTRLcommunities.CommunityFilters
        End Get
    End Property
    Public ReadOnly Property HasAvailableCommunities As Boolean Implements IViewAddCommunityToProfile.HasAvailableCommunities
        Get
            Return CTRLcommunities.HasAvailableCommunities
        End Get
    End Property

    Public Function GetNodesById(idCommunities As List(Of Integer)) As List(Of dtoBaseCommunityNode) Implements IViewAddCommunityToProfile.GetNodesById
        Return Me.CTRLcommunities.GetNodesById(idCommunities)
    End Function


    Public Function SelectedIdCommunities() As List(Of Integer) Implements IViewAddCommunityToProfile.SelectedIdCommunities
        Return Me.CTRLcommunities.SelectedIdCommunities
    End Function

#End Region

#Region "STEP 2 - Subscriptions"
    Public ReadOnly Property HasAvailableSubscriptions As Boolean Implements IViewAddCommunityToProfile.HasAvailableSubscriptions
        Get
            Return CTRLsubscriptions.HasAvailableSubscriptions
        End Get
    End Property

    Public Function SelectedSubscriptions() As List(Of dtoUserSubscription) Implements IViewAddCommunityToProfile.SelectedSubscriptions
        Return CTRLsubscriptions.SelectedSubscriptions
    End Function
#End Region

#Region "STEP 3 - Unsubscriptions"
    Public ReadOnly Property HasUnsubscriptions As Boolean Implements IViewAddCommunityToProfile.HasUnsubscriptions
        Get
            Return Me.CTRLunsubscriptions.HasUnsubscriptions
        End Get
    End Property
    Public Sub InitializeUnsubscriptionControl(idProfile As Integer, unsubscriptions As List(Of dtoBaseUserSubscription)) Implements IViewAddCommunityToProfile.InitializeUnsubscriptionControl
        Me.CTRLunsubscriptions.InitializeControl(idProfile, unsubscriptions)
    End Sub
    Public Function SelectedUnsubscriptions() As List(Of dtoBaseUserSubscription) Implements IViewAddCommunityToProfile.SelectedUnsubscriptions
        Return Me.CTRLunsubscriptions.SelectedUnsubscriptions
    End Function
#End Region

#Region "STEP 4 - Conferma"
    Public Property isCompleted As Boolean Implements IViewAddCommunityToProfile.isCompleted
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
        End Set
    End Property
    Public Sub DisplayError() Implements IViewAddCommunityToProfile.DisplayError
        Me.MLVwizard.SetActiveView(VIWerror)
        Me.BTNcompleteTop.Visible = False
        Me.BTNcompleteBottom.Visible = False
        Me.Resource.setLiteral(Me.LTerrors)
        Me.CurrentStep = CommunitySubscriptionWizardStep.Errors
    End Sub

    Public Sub UpdateUserSubscriptions(idProfile As Integer, profileName As String, currentUser As String, subscribed As List(Of dtoUserSubscription), unsubscribed As List(Of Integer)) Implements IViewAddCommunityToProfile.UpdateUserSubscriptions
        If unsubscribed.Count > 0 Then
            Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
            For Each idCommunity As Integer In unsubscribed
                oServiceUtility.NotifyDeleteSubscription(idCommunity, idProfile, profileName, currentUser)
            Next
        End If
        If subscribed.Count > 0 Then
            Dim rolesName As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
            Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
            For Each subscription As dtoUserSubscription In subscribed
                oServiceUtility.NotifyAddSubscription(subscription.IdCommunity, idProfile, subscription.IdRole, profileName, rolesName.Where(Function(r) r.ID = subscription.IdRole).Select(Function(r) r.Name).FirstOrDefault(), currentUser)
            Next

        End If
        Dim oTreeComunita As New COL_BusinessLogic_v2.Comunita.COL_TreeComunita

        Try
            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & idProfile & "\"
            oTreeComunita.Nome = idProfile & ".xml"
            oTreeComunita.AggiornaInfo(idProfile.ToString, LinguaID, SystemSettings.CodiceDB, True)
        Catch ex As Exception

        End Try
        isCompleted = True
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.ProfileManagement.RootObject.ManagementProfilesWithFilters)
    End Sub
    Public Sub DisplaySummaryInfo(toSubscribe As List(Of String), toEdit As List(Of String), toUnsubscribe As List(Of String)) Implements IViewAddCommunityToProfile.DisplaySummaryInfo
        Me.LBcommunityToSubscribe.Visible = (toSubscribe.Count > 0)
        Me.LBsubscriptionsEdited.Visible = (toEdit.Count > 0)
        Me.LBsubscriptionsToDelete.Visible = (toUnsubscribe.Count > 0)

        Dim results As String = ""
        If toSubscribe.Count > 0 Then
            Resource.setLabel(Me.LBcommunityToSubscribe)
            results = Me.LBcommunityToSubscribe.Text & "<ul>"
            For Each name As String In toSubscribe
                results = results & "<li>" & name & "</li> "
            Next
            results &= "</ul>"
            Me.LBcommunityToSubscribe.Text = results
        End If

        If toEdit.Count > 0 Then
            Resource.setLabel(Me.LBsubscriptionsEdited)
            results = Me.LBsubscriptionsEdited.Text & "<ul>"
            For Each name As String In toEdit
                results = results & "<li>" & name & "</li> "
            Next
            results &= "</ul>"
            Me.LBsubscriptionsEdited.Text = results
        End If

        If toUnsubscribe.Count > 0 Then
            Resource.setLabel(Me.LBsubscriptionsToDelete)
            results = Me.LBsubscriptionsToDelete.Text & "<ul>"
            For Each name As String In toUnsubscribe
                results = results & "<li>" & name & "</li> "
            Next
            results &= "</ul>"
            Me.LBsubscriptionsToDelete.Text = results
        End If
    End Sub
#End Region

    Public Sub DisplayNoPermission() Implements IViewAddCommunityToProfile.DisplayNoPermission
        Master.ShowNoPermission = True
    End Sub

    Public Sub DisplayNoPermissionForProfile() Implements IViewAddCommunityToProfile.DisplayNoPermissionForProfile
        MLVprofiles.SetActiveView(VIWdefault)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayNoPermissionForProfile")
    End Sub

    Public Sub DisplayProfileUnknown() Implements IViewAddCommunityToProfile.DisplayProfileUnknown
        MLVprofiles.SetActiveView(VIWdefault)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayProfileUnknown")
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewAddCommunityToProfile.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = lm.Comol.Core.BaseModules.ProfileManagement.RootObject.AddCommunitiesToProfile(IIf(IdProfile > 0, IdProfile, PreloadedIdProfile))
        webPost.Redirect(dto)
    End Sub
#End Region

    Public Function TreeViewClientID() As String
        Return Me.CTRLcommunities.TreeViewClientID
    End Function

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class