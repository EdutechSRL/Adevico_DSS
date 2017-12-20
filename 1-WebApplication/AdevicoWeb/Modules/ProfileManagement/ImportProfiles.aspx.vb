Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class ImportProfiles
    Inherits PageBase
    Implements IViewProfilesImport


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
    Private _Presenter As ProfilesImportPresenter
    Private ReadOnly Property CurrentPresenter() As ProfilesImportPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfilesImportPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
   
#End Region
#Region "Implements"
    Public Property SkipSteps As List(Of ProfileImportStep) Implements IViewProfilesImport.SkipSteps
        Get
            Return ViewStateOrDefault("SkipSteps", New List(Of ProfileImportStep))
        End Get
        Set(value As List(Of ProfileImportStep))
            ViewState("SkipSteps") = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewProfilesImport.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagement.Visible = value
            Me.HYPbackToManagement.NavigateUrl = BaseUrl & lm.Comol.Core.BaseModules.ProfileManagement.RootObject.ManagementProfilesWithFilters()
        End Set
    End Property
    Public Property AvailableSteps As List(Of ProfileImportStep) Implements IViewProfilesImport.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of ProfileImportStep))
        End Get
        Set(value As List(Of ProfileImportStep))
            ViewState("AvailableSteps") = value
        End Set
    End Property
    Public Property CurrentStep As ProfileImportStep Implements IViewProfilesImport.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", ProfileImportStep.SelectSource)
        End Get
        Set(value As ProfileImportStep)
            ViewState("CurrentStep") = value
        End Set
    End Property

    Public Property ImportIdentifier As System.Guid Implements IViewProfilesImport.ImportIdentifier
        Get
            Return ViewStateOrDefault("ImportIdentifier", System.Guid.NewGuid)
        End Get
        Set(value As System.Guid)
            ViewState("ImportIdentifier") = value
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
        If Not Page.IsPostBack Then
            RPAimportProfiles.ProgressIndicators = RPAimportProfiles.ProgressIndicators And Not Telerik.Web.UI.Upload.ProgressIndicators.SelectedFilesCount
        End If
        RPAimportProfiles.Localization.Total = Resource.getValue("Localization.Total")
        RPAimportProfiles.Localization.TotalFiles = Resource.getValue("Localization.TotalFiles")
        RPAimportProfiles.Localization.ElapsedTime = Resource.getValue("Localization.ElapsedTime")
        RPAimportProfiles.Localization.EstimatedTime = Resource.getValue("Localization.EstimatedTime")
        RPAimportProfiles.Localization.TransferSpeed = Resource.getValue("Localization.TransferSpeed")


        RPAimportProfiles.Localization.Uploaded = Resource.getValue("Localization.Uploaded")
        RPAimportProfiles.Localization.UploadedFiles = Resource.getValue("Localization.UploadedFiles")
        RPAimportProfiles.Localization.CurrentFileName = Resource.getValue("Localization.CurrentFileName")
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.Master.AddToolTip()
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesImport", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleImportProfile")
            Me.Master.ServiceNopermission = .getValue("serviceImportProfilesNopermission")
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

#Region "Move Wizard"

    Public Sub GotoStep(pStep As ProfileImportStep) Implements IViewProfilesImport.GotoStep
        GotoStep(pStep, False)
    End Sub

    Public Sub GotoStep(pStep As ProfileImportStep, initialize As Boolean) Implements IViewProfilesImport.GotoStep
        Select Case pStep
            Case ProfileImportStep.SelectSource
                If initialize Then : InitializeStep(pStep)
                End If
                MLVwizard.SetActiveView(VIWselectSource)

            Case ProfileImportStep.SourceCSV
                If initialize Then : InitializeStep(pStep)
                End If
                Me.CTRLfieldsMatcher.isInitialized = False
                Me.CTRLitemsSelector.isInitialized = False
                MLVwizard.SetActiveView(VIWsourceCSV)
            Case ProfileImportStep.SourceRequestForMembership
                If initialize OrElse Me.CTRLsourceUserSubmission.ItemType <> lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.RequestForMembership Then : InitializeStep(pStep)
                End If
                Me.CTRLfieldsMatcher.isInitialized = False
                Me.CTRLitemsSelector.isInitialized = False
                MLVwizard.SetActiveView(VIWsourceRFM)
            Case ProfileImportStep.SourceCallForPapers
                If initialize OrElse Me.CTRLsourceUserSubmission.ItemType <> lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids Then : InitializeStep(pStep)
                End If
                Me.CTRLfieldsMatcher.isInitialized = False
                Me.CTRLitemsSelector.isInitialized = False
                MLVwizard.SetActiveView(VIWsourceRFM)
            Case ProfileImportStep.FieldsMatcher
                If initialize AndAlso Not IsInitialized(pStep) Then
                Else
                    MLVwizard.SetActiveView(VIWfieldsMatcher)
                End If
            Case ProfileImportStep.ItemsSelctor
                If initialize Then 'AndAlso Not IsInitialized(pStep) Then
                    Dim source As New lm.Comol.Core.DomainModel.Helpers.ProfileExternalResource

                    If Me.CurrentSource = SourceType.FileCSV Then
                        source = Me.CTRLsourceCSV.GetFileContent(Me.CTRLfieldsMatcher.Fields)
                    ElseIf Me.CurrentSource = SourceType.RequestForMembership OrElse Me.CurrentSource = SourceType.CallForPapers Then
                        source = Me.CTRLsourceUserSubmission.GetItemSubmissions(Me.CTRLfieldsMatcher.Fields)
                    End If
                    Me.CTRLitemsSelector.InitializeControl(source, Me.CTRLfieldsMatcher.ImportSettings)
                End If
                MLVwizard.SetActiveView(VIWitemsSelector)
            Case ProfileImportStep.SelectOrganizations
                If initialize Then : InitializeStep(pStep)
                End If
                MLVwizard.SetActiveView(VIWorganizations)
            Case ProfileImportStep.SelectCommunities
                If initialize Then : InitializeStep(pStep)
                End If
                MLVwizard.SetActiveView(VIWselectCommunities)
            Case ProfileImportStep.SubscriptionsSettings
                If initialize Then : InitializeStep(pStep)
                End If
                MLVwizard.SetActiveView(VIWsubscriptionsSettings)
            Case ProfileImportStep.MailTemplate
                If initialize Then : InitializeStep(pStep)
                End If
                MLVwizard.SetActiveView(VIWmailTemplate)

                '        MLVwizard.SetActiveView(VIWsubscriptionsSettings)
                '        If initialize Then : InitializeStep(pStep)
                '        End If
                '    Case CommunitySubscriptionWizardStep.RemoveSubscriptions
                '        MLVwizard.SetActiveView(VIWremoveSubscription)
                '        If initialize Then : InitializeStep(pStep)
                '        End If
            Case ProfileImportStep.Summary
                MLVwizard.SetActiveView(VIWsummary)
                If initialize Then : InitializeStep(pStep)
                End If

                '        '   Me.CTRLsummary.InitializeForManagement(ProfileInfo, Me.CTRLorganizations.SelectedOrganizationName, Me.CTRLorganizations.OtherOrganizationsToSubscribe, Me.SelectedProfileName, SelectedProfileTypeId, Me.CurrentPresenter.GetAuthenticationProvider(SelectedProviderId), Me.GetExternalCredentials)
        End Select

        Me.CurrentStep = pStep
        Me.BTNbackBottom.Visible = (pStep <> ProfileImportStep.SelectSource)
        Me.BTNbackTop.Visible = (pStep <> ProfileImportStep.SelectSource)
        Me.BTNcompleteBottom.Visible = (pStep = ProfileImportStep.Summary)
        Me.BTNcompleteTop.Visible = (pStep = ProfileImportStep.Summary)
        Me.BTNnextBottom.Visible = (pStep <> ProfileImportStep.Summary AndAlso pStep <> ProfileImportStep.Errors AndAlso pStep <> ProfileImportStep.ImportCompleted AndAlso pStep <> ProfileImportStep.ImportWithErrors)
        Me.BTNnextTop.Visible = (pStep <> ProfileImportStep.Summary AndAlso pStep <> ProfileImportStep.Errors AndAlso pStep <> ProfileImportStep.ImportCompleted AndAlso pStep <> ProfileImportStep.ImportWithErrors)

        If pStep <> ProfileImportStep.Errors AndAlso pStep <> ProfileImportStep.ImportCompleted AndAlso pStep <> ProfileImportStep.ImportWithErrors Then
            Me.LBstepTitle.Text = String.Format(Resource.getValue("ProfileImportStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
            Me.LBstepDescription.Text = Resource.getValue("ProfileImportStep." & pStep.ToString & ".Description")
        End If

    End Sub

    Public Sub InitializeStep(pStep As ProfileImportStep) Implements IViewProfilesImport.InitializeStep
        Select Case pStep
            Case ProfileImportStep.SelectSource
                Me.CTRLsourceSelector.InitializeControl(SourceType.FileCSV)
            Case ProfileImportStep.SourceCSV
                Me.CTRLsourceCSV.InitializeControl()
            Case ProfileImportStep.SourceRequestForMembership
                Me.CTRLsourceUserSubmission.ItemType = lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.RequestForMembership
                Me.CTRLsourceUserSubmission.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.RequestForMembership, CTRLsourceSelector.CurrentRange, CTRLsourceSelector.IdCommunityRange)
            Case ProfileImportStep.SourceCallForPapers
                Me.CTRLsourceUserSubmission.ItemType = lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids
                Me.CTRLsourceUserSubmission.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids, CTRLsourceSelector.CurrentRange, CTRLsourceSelector.IdCommunityRange)
            Case ProfileImportStep.FieldsMatcher
                If CurrentStep = ProfileImportStep.SourceCSV Then
                    Me.CTRLfieldsMatcher.InitializeControl(AvailableColumns(SourceType.FileCSV))
                ElseIf CurrentStep = ProfileImportStep.SourceRequestForMembership Then
                    Me.CTRLfieldsMatcher.InitializeControl(AvailableColumns(SourceType.RequestForMembership))
                End If
            Case ProfileImportStep.SelectOrganizations
                If Not IsInitialized(pStep) Then
                    Me.CTRLorganizations.InitializeControl(True)
                End If
            Case ProfileImportStep.SelectCommunities
                Dim unloadIdCommunity As List(Of Integer) = CurrentPresenter.GetCommunityIdFromOrganization(CTRLorganizations.AllSelectedOrganizationId)
                Dim unloaded As List(Of Integer) = Me.CTRLselectCommunities.NotLoadIdCommunities

                If Not IsInitialized(pStep) OrElse (unloadIdCommunity.Count <> unloaded.Count) OrElse (unloaded.Where(Function(i) Not unloadIdCommunity.Contains(i)).Any()) OrElse (unloadIdCommunity.Where(Function(i) Not unloaded.Contains(i)).Any()) Then
                    Me.CTRLselectCommunities.InitializeControl(Resource.getValue("CTRLselectCommunities.DisplayInfo"), Nothing, unloadIdCommunity, CTRLorganizations.AllSelectedOrganizationId)
                End If
            Case ProfileImportStep.SubscriptionsSettings
                If Not IsInitialized(pStep) Then
                    Dim items As New List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem)
                    items.AddRange(OrganizationsNodes())
                    items.AddRange(CTRLselectCommunities.GetSelectedItems)
                    Me.CTRLsubscriptions.InitializeControl(items)
                End If
            Case ProfileImportStep.MailTemplate
                If Not IsInitialized(pStep) Then
                    Me.CTRLmailTemplate.InitializeControl(Me.CTRLfieldsMatcher.ImportSettings)
                End If
            Case ProfileImportStep.Summary
                If Not IsInitialized(pStep) Then
                    Me.CTRLsummary.InitializeControl(Me.CTRLfieldsMatcher.SelectedAuthenticationTypeName, Me.CTRLfieldsMatcher.SelectedProfileTypeName, Me.CTRLmailTemplate.SendMailToUsers, PrimaryOrganizationName, OtherOrganizationsToSubscribe, Me.CTRLsubscriptions.SelectedSubscriptions, Me.CTRLitemsSelector.SelectedItemsCount)
                End If
        End Select
    End Sub
    Public Function IsInitialized(pStep As ProfileImportStep) As Boolean Implements IViewProfilesImport.IsInitialized
        Select Case pStep
            Case ProfileImportStep.SelectSource
                Return Me.CTRLsourceSelector.isInitialized
            Case ProfileImportStep.SourceCSV
                Return Me.CTRLsourceCSV.isInitialized
            Case ProfileImportStep.SourceRequestForMembership
                Return Me.CTRLsourceUserSubmission.isInitialized
            Case ProfileImportStep.SourceCallForPapers
                Return Me.CTRLsourceUserSubmission.isInitialized
            Case ProfileImportStep.FieldsMatcher
                Return Me.CTRLfieldsMatcher.isInitialized
                'Case CommunitySubscriptionWizardStep.RemoveSubscriptions
            Case ProfileImportStep.ItemsSelctor
                Return Me.CTRLitemsSelector.isInitialized
            Case ProfileImportStep.SelectOrganizations
                Return Me.CTRLorganizations.isInitialized
            Case ProfileImportStep.SelectCommunities
                Return Me.CTRLselectCommunities.isInitialized
            Case ProfileImportStep.SubscriptionsSettings
                Return Me.CTRLsubscriptions.isInitialized '(0, CTRLcommunities.SelectedCommunities)
            Case ProfileImportStep.MailTemplate
                Return Me.CTRLmailTemplate.isInitialized
            Case ProfileImportStep.Summary
                Return Me.CTRLsummary.isInitialized
            Case Else
                Return True
        End Select
    End Function
    Private Function GetStepNumber(pStep As ProfileImportStep) As Integer
        Dim Number As Integer = 1
        Dim list As List(Of ProfileImportStep) = Me.AvailableSteps
        Dim toSkip As List(Of ProfileImportStep) = Me.SkipSteps
        For Each item As ProfileImportStep In (From i In list Where toSkip.Contains(i) = False Select i).ToList
            If item = pStep Then
                Return Number
            Else
                Number += 1
            End If
        Next

    End Function
    Private Sub BTNbackBottom_Click(sender As Object, e As System.EventArgs) Handles BTNbackBottom.Click, BTNbackTop.Click
        If Me.MLVwizard.GetActiveView Is VIWerror Then
            MLVwizard.SetActiveView(VIWcomplete)

            Me.BTNcompleteBottom.Visible = True
            Me.BTNcompleteTop.Visible = True
            Me.CurrentStep = ProfileImportStep.Summary
            Me.LBstepTitle.Text = String.Format(Resource.getValue("ProfileImportStep." & ProfileImportStep.Summary.ToString & ".Title"), GetStepNumber(ProfileImportStep.Summary))
            Me.LBstepDescription.Text = Resource.getValue("ProfileImportStep." & ProfileImportStep.Summary.ToString & ".Description")
        Else
            Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
        End If
    End Sub

    Private Sub BTNcompleteBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteBottom.Click, BTNcompleteTop.Click
        If Me.isCompleted Then
            Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.ProfileManagement.RootObject.ManagementProfilesWithFilters)
        Else
            Me.CurrentPresenter.ImportProfiles(CTRLfieldsMatcher.ImportSettings, PrimaryOrganizationId, AllOrganizationsId, CTRLitemsSelector.SelectedItems, CTRLsubscriptions.SelectedSubscriptions, CTRLmailTemplate.SendMailToUsers, CTRLmailTemplate.MailContent)
        End If
    End Sub


    Private Sub BTNnextBottom_Click(sender As Object, e As System.EventArgs) Handles BTNnextBottom.Click, BTNnextTop.Click
        Me.CurrentPresenter.MoveToNextStep(CurrentStep)
    End Sub
#End Region

#Region "STEP 1 - Select Source"
    Public ReadOnly Property CurrentSource As SourceType Implements IViewProfilesImport.CurrentSource
        Get
            Return Me.CTRLsourceSelector.CurrentSource
        End Get
    End Property
#End Region

#Region "STEP 2 - common"
    Public Function AvailableColumns(type As lm.Comol.Core.BaseModules.ProfileManagement.SourceType) As List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String)) Implements IViewProfilesImport.AvailableColumns
        Select Case type
            Case SourceType.FileCSV
                If Me.CTRLsourceCSV.isValid Then
                    Return Me.CTRLsourceCSV.AvailableColumns
                Else
                    Return Me.CTRLsourceCSV.GetAvailableColumns
                End If
            Case SourceType.CallForPapers, SourceType.RequestForMembership
                'If Me.CTRLsourceUserSubmission.isValid Then
                '    Return Me.CTRLsourceUserSubmission.AvailableColumns
                'Else
                Return Me.CTRLsourceUserSubmission.GetAvailableColumns
                'End If

            Case Else
                Return New List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String))
        End Select
    End Function
#End Region
#Region "STEP 2 - CSV"
    Public Function GetFileContent(columns As List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String))) As lm.Comol.Core.DomainModel.Helpers.ProfileExternalResource Implements IViewProfilesImport.GetFileContent
        Select Case CurrentSource
            Case SourceType.FileCSV
                Return Me.CTRLsourceCSV.GetFileContent(columns)
            Case SourceType.CallForPapers
            Case SourceType.RequestForMembership
                Return Me.CTRLsourceUserSubmission.GetItemSubmissions(columns)
            Case Else
                Return New lm.Comol.Core.DomainModel.Helpers.ProfileExternalResource(columns, New List(Of List(Of String)))
        End Select
    End Function
    Public Function RetrieveFile() As lm.Comol.Core.DomainModel.Helpers.CsvFile Implements IViewProfilesImport.RetrieveFile
        Return Me.CTRLsourceCSV.RetrieveFile
    End Function
#End Region

#Region "Step 3- Fields Matcher"
    Public ReadOnly Property Fields As List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String)) Implements IViewProfilesImport.Fields
        Get
            Return Me.CTRLfieldsMatcher.Fields
        End Get
    End Property
    Public ReadOnly Property ValidDestinationFields As Boolean Implements lm.Comol.Core.BaseModules.ProfileManagement.Presentation.IViewProfilesImport.ValidDestinationFields
        Get
            Return CTRLfieldsMatcher.isValid
        End Get
    End Property
    Public Sub InitializeFieldsMatcher(sourceColumns As List(Of lm.Comol.Core.DomainModel.Helpers.ProfileColumnComparer(Of String))) Implements IViewProfilesImport.InitializeFieldsMatcher
        Me.CTRLfieldsMatcher.InitializeControl(sourceColumns)
    End Sub
    Public ReadOnly Property ImportSettings As dtoImportSettings Implements IViewProfilesImport.ImportSettings
        Get
            Return Me.CTRLfieldsMatcher.ImportSettings()
        End Get
    End Property
#End Region

#Region "Step 4- Items selection "
    Private ReadOnly Property HasSelectedItems As Boolean Implements IViewProfilesImport.HasSelectedItems
        Get
            Return Me.CTRLitemsSelector.HasSelectedItems
        End Get
    End Property
    Private ReadOnly Property ItemsToSelect As Integer Implements IViewProfilesImport.ItemsToSelect
        Get
            Return Me.CTRLitemsSelector.ItemsToSelect
        End Get
    End Property
    Private ReadOnly Property SelectedItems As lm.Comol.Core.DomainModel.Helpers.ProfileExternalResource Implements IViewProfilesImport.SelectedItems
        Get
            Return Me.CTRLitemsSelector.SelectedItems
        End Get
    End Property
#End Region

#Region "Step 5 - Organizations "
    Public ReadOnly Property AvailableOrganizationsId As List(Of Integer) Implements IViewProfilesImport.AvailableOrganizationsId
        Get
            Return Me.CTRLorganizations.AvailableOrganizationsId
        End Get
    End Property
    Public ReadOnly Property OtherOrganizationsToSubscribe As Dictionary(Of Integer, String) Implements IViewProfilesImport.OtherOrganizationsToSubscribe
        Get
            Return Me.CTRLorganizations.OtherOrganizations
        End Get
    End Property
    Public ReadOnly Property PrimaryOrganizationId As Integer Implements IViewProfilesImport.PrimaryOrganizationId
        Get
            Return Me.CTRLorganizations.SelectedOrganizationId
        End Get
    End Property
    Public ReadOnly Property PrimaryOrganizationName As String Implements IViewProfilesImport.PrimaryOrganizationName
        Get
            Return Me.CTRLorganizations.SelectedOrganizationName
        End Get
    End Property
    Public ReadOnly Property AllOrganizationsId As List(Of Integer) Implements IViewProfilesImport.AllOrganizationsId
        Get
            Return CTRLorganizations.AllSelectedOrganizationId
        End Get
    End Property
    Public Property OrganizationsNodes As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem) Implements IViewProfilesImport.OrganizationsNodes
        Get
            Return ViewStateOrDefault("OrganizationsNodes", New List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem))
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem))
            ViewState("OrganizationsNodes") = value
        End Set
    End Property

#End Region

#Region " STEP 6 - Communities"
    'Public ReadOnly Property HasAvailableCommunities As Boolean Implements IViewProfilesImport.HasAvailableCommunities
    '    Get
    '        Return Me.CTRLcommunities.HasAvailableCommunities
    '    End Get
    'End Property
    Public Function SelectedIdCommunities() As List(Of Integer) Implements IViewProfilesImport.SelectedIdCommunities
        Return Me.CTRLselectCommunities.SelectedIdCommunities
    End Function
    Public Function SelectedCommunities() As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem) Implements IViewProfilesImport.SelectedCommunities
        Return Me.CTRLselectCommunities.GetSelectedItems()
    End Function
    Public Sub UpdateSelectedCommunities(idCommunities As List(Of Integer)) Implements IViewProfilesImport.UpdateSelectedCommunities
        Me.CTRLselectCommunities.UpdateSelectedCommunities(idCommunities)
    End Sub
    Private Sub CTRLselectCommunities_LoadDefaultFiltersToHeader(filters As List(Of Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLselectCommunities.LoadDefaultFiltersToHeader
        CTRLselectCommunitiesHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
    Private Sub CTRLselectCommunities_SetModalIdentifierAndVariables(identifier As String, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLselectCommunities.SetModalIdentifierAndVariables
        CTRLselectCommunitiesHeader.SetInitializeVariables(requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
        Master.SetOpenDialogOnPostbackByCssClass(identifier)
    End Sub
    Private Sub CTRLselectCommunities_SetModalIdentifier(identifier As String) Handles CTRLselectCommunities.SetModalIdentifier
        Master.SetOpenDialogOnPostbackByCssClass(identifier)
    End Sub

    Private Sub CTRLselectCommunities_SetModalTitle(title As String) Handles CTRLselectCommunities.SetModalTitle
        CTRLselectCommunitiesHeader.SetModalTitle(title)
    End Sub
#End Region
#Region "STEP 7 - Subscriptions"
    Public ReadOnly Property HasAvailableSubscriptions As Boolean Implements IViewProfilesImport.HasAvailableSubscriptions
        Get
            Return CTRLsubscriptions.HasAvailableSubscriptions
        End Get
    End Property

    Public Function SelectedSubscriptions() As List(Of dtoNewProfileSubscription) Implements IViewProfilesImport.SelectedSubscriptions
        Return CTRLsubscriptions.SelectedSubscriptions
    End Function
#End Region
#Region "STEP 8 - Template Mail"
    Public ReadOnly Property MailContent As lm.Comol.Core.Mail.dtoMailContent Implements IViewProfilesImport.MailContent
        Get
            Return Me.CTRLmailTemplate.MailContent
        End Get
    End Property

    Public ReadOnly Property SendMailToUsers As Boolean Implements IViewProfilesImport.SendMailToUsers
        Get
            Return Me.CTRLmailTemplate.SendMailToUsers
        End Get
    End Property
#End Region
#Region "STEP 9 - Conferma"
    Public Property isCompleted As Boolean Implements IViewProfilesImport.isCompleted
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
        End Set
    End Property

    Public Sub UpdateSourceItems(type As SourceType) Implements IViewProfilesImport.UpdateSourceItems
        Dim source As New lm.Comol.Core.DomainModel.Helpers.ProfileExternalResource

        Select Case type
            Case SourceType.FileCSV
                source = Me.CTRLsourceCSV.GetFileContent(Me.CTRLfieldsMatcher.Fields)
            Case SourceType.CallForPapers, SourceType.RequestForMembership
                source = Me.CTRLsourceUserSubmission.GetItemSubmissions(Me.CTRLfieldsMatcher.Fields)

        End Select

        Me.CTRLitemsSelector.InitializeControlAfterImport(source, Me.CTRLfieldsMatcher.ImportSettings)
    End Sub
    Public Sub DisplayError(currentStep As lm.Comol.Core.BaseModules.ProfileManagement.ProfileImportStep) Implements IViewProfilesImport.DisplayError

    End Sub

    Public Sub DisplayImportedProfiles(importedItems As Integer, itemsToImport As Integer) Implements IViewProfilesImport.DisplayImportedProfiles
        DisplayImportCommon(importedItems, ProfileImportStep.ImportCompleted)

        Me.LBcompleteInfo.Text = IIf(importedItems = 1, Resource.getValue("Import.Completed.1"), String.Format(Resource.getValue("Import.Completed"), importedItems))
        Select Case itemsToImport
            Case 0
                Exit Select
            Case 1
                Me.LBcompleteInfo.Text &= vbCrLf & Resource.getValue("Import.ImportOther.1")
            Case Else
                Me.LBcompleteInfo.Text &= vbCrLf & String.Format(Resource.getValue("Import.ImportOther"), itemsToImport)
        End Select
    End Sub

    Public Sub DisplayImportError(importedItems As Integer, notCreatedProfiles As List(Of dtoBaseProfile), notaddedToOrganizations As Dictionary(Of Integer, String), notSubscribedCommunities As Dictionary(Of Integer, String), notSentMail As List(Of dtoImportedProfile)) Implements IViewProfilesImport.DisplayImportError
        DisplayImportCommon(importedItems, ProfileImportStep.ImportWithErrors)

        Me.LBcompleteInfo.Text = IIf(importedItems = 1, Resource.getValue("ImportError.Completed.1"), String.Format(Resource.getValue("ImportError.Completed"), importedItems))

        SPNprofiles.Visible = (notCreatedProfiles.Count > 0)
        SPNorganizations.Visible = (notaddedToOrganizations.Count > 0)
        SPNcommunities.Visible = (notSubscribedCommunities.Count > 0)
        SPNmail.Visible = (notSentMail.Count > 0)
        GetDisplayErrors(notCreatedProfiles.Select(Function(p) p.DisplayName & "</li>").ToList, LBprofiles)
        GetDisplayErrors(notaddedToOrganizations.Select(Function(o) o.Value & "</li>").ToList, LBorganizations)
        GetDisplayErrors(notSubscribedCommunities.Select(Function(c) c.Value & "</li>").ToList, LBcommunities)
        GetDisplayErrors(notSentMail.Select(Function(p) p.Profile.DisplayName & "</li>").ToList, LBmail)
    End Sub

    Private Function GetDisplayErrors(ByVal items As List(Of String), ByVal olabel As Label) As String
        Dim errors As String = ""
        errors = String.Join("<li>", items.ToArray)
        If Not String.IsNullOrEmpty(errors) Then
            olabel.Text = "<ul>" & errors & "</ul>"
        Else
            olabel.Text = ""
        End If
    End Function

    Private Sub DisplayImportCommon(importedItems As Integer, ByVal eStep As ProfileImportStep)
        Me.BTNbackBottom.Visible = (PreviousStep <> ProfileImportStep.None)
        Me.BTNbackTop.Visible = (PreviousStep <> ProfileImportStep.None)
        Me.BTNcompleteBottom.Visible = False
        Me.BTNcompleteTop.Visible = False
        Me.BTNnextBottom.Visible = False
        Me.BTNnextTop.Visible = False

        Me.LBstepTitle.Text = Resource.getValue("ProfileImportStep." & eStep.ToString & ".Title")
        Me.LBstepDescription.Text = Resource.getValue("ProfileImportStep." & eStep.ToString & ".Description")
        Me.CurrentStep = eStep
        Me.MLVwizard.SetActiveView(VIWcomplete)

        SPNprofiles.Visible = False
        SPNorganizations.Visible = False
        SPNcommunities.Visible = False
        SPNmail.Visible = False
    End Sub
    Public Property PreviousStep As ProfileImportStep Implements IViewProfilesImport.PreviousStep
        Get
            Return ViewStateOrDefault("PreviousStep", ProfileImportStep.None)
        End Get
        Set(value As ProfileImportStep)
            ViewState("PreviousStep") = value
        End Set
    End Property
#End Region

    Public Sub DisplayNoPermission() Implements IViewProfilesImport.DisplayNoPermission
        Master.ShowNoPermission = True
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewProfilesImport.DisplaySessionTimeout
        'MLVimport.SetActiveView(VIWdefault)
        'Me.LBmessage.Text = Me.Resource.getValue("DisplaySessionTimeout")
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = lm.Comol.Core.BaseModules.ProfileManagement.RootObject.ImportProfiles()
        webPost.Redirect(dto)
    End Sub
#End Region

#Region "Import Profiles"
    Public Sub SetupProgressInfo(actionCount As Integer, userCount As Integer) Implements IViewProfilesImport.SetupProgressInfo
        RPAimportProfiles.Visible = True

        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        progress.CurrentOperationText = ""
        progress.Speed = "" ' "N/A"
        progress.PrimaryTotal = actionCount
        progress.PrimaryValue = 0
        progress.PrimaryPercent = 0

        progress.SecondaryTotal = userCount
        progress.SecondaryValue = 0
        progress.SecondaryPercent = 0
    End Sub

#Region "Action 1 - Create Profile"
    Public Sub UpdateProfileCreation(actionIndex As Integer, userIndex As Integer, created As Boolean, displayName As String) Implements IViewProfilesImport.UpdateProfileCreation
        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        progress.PrimaryValue = actionIndex
        progress.CurrentOperationText = String.Format(Resource.getValue("ProfileImportAction." & ProfileImportAction.Create.ToString & "." & created.ToString), displayName)
        progress.SecondaryValue = userIndex
        progress.SecondaryPercent = Int((userIndex / progress.SecondaryTotal) * 100)
        progress.TimeEstimated = (progress.SecondaryTotal - userIndex) * 100
    End Sub
    Public Function AddUserProfile(profile As dtoBaseProfile, info As PersonInfo, idDefaultOrganization As Integer, provider As AuthenticationProvider) As Integer Implements IViewProfilesImport.AddUserProfile
        Dim idPerson As Integer = 0
        If profile.Password = "" Then
            profile.Password = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, True, True, False)
        End If
        Dim oldProfile As COL_Persona = CreateUser(profile, provider, info)

        Try
            If TypeOf profile Is dtoExternal Then
                'idPerson = 0
                idPerson = AddExternalUser(CL_Esterno.COL_Esterno.CreateFromPerson(oldProfile), DirectCast(profile, dtoExternal))
            ElseIf TypeOf profile Is dtoCompany Then
                'idPerson = 0
                idPerson = AddCompanyUser(CreateUserInfo(profile, provider.ProviderType, info), provider)
            ElseIf TypeOf profile Is dtoEmployee Then
                'idPerson = 0
                idPerson = AddEmployee(CreateUserInfo(profile, provider.ProviderType, info), provider)
            Else
                'idPerson = 0
                oldProfile.Aggiungi()
                idPerson = oldProfile.ID
            End If
            If idPerson > 0 Then
                oldProfile.ID = idPerson
                profile.Id = idPerson
                Me.AddUserToOrganization(oldProfile, idDefaultOrganization, True)

            End If
        Catch ex As Exception

        End Try

        Return idPerson
    End Function

#Region "AddUser"

    Private Function CreateUser(profile As dtoBaseProfile, provider As AuthenticationProvider, info As PersonInfo) As COL_Persona
        Dim person As New COL_Persona
        With person
            .AUTN_ID = CInt(provider.ProviderType)
            .AUTN_RemoteUniqueID = profile.Login


            .Nome = profile.Name
            .Cognome = profile.Surname
            .CodFiscale = profile.TaxCode
            .DataNascita = DateSerial(2999, 1, 1)
            .LuogoNascita = ""
            .Login = profile.Login
            .Mail = profile.Mail
            .MailSecondaria = profile.Mail
            .MostraMail = profile.ShowMail
            .TipoPersona.ID = profile.IdProfileType
            .Provincia.ID = Me.SystemSettings.Presenter.DefaultProvinceID
            .Stato.ID = Me.SystemSettings.Presenter.DefaultNationID
            .Sesso = False
            .IsInterno = 1
            .Lingua = New Lingua() With {.ID = profile.IdLanguage}
            .Istituzione = New COL_BusinessLogic_v2.Comunita.COL_Istituzione() With {.Id = Me.PageUtility.IstituzioneID}
            .RicezioneSMS = 0
            .Pwd = profile.Password
            If String.IsNullOrEmpty(profile.Password) Then
                .Pwd = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, True, True, False)
            End If


            .Bloccata = False
            .Cap = info.PostCode
            .Cellulare = info.Mobile
            .Telefono1 = info.OfficePhone
            .Telefono2 = info.HomePhone
            .Citta = info.City
            .Fax = info.Fax
            .HomePage = ""
            .Indirizzo = info.Address
            .InfoRicevimento = ""
            .Note = ""



            .DataNascita = profile.PersonInfo.BirthDate
            .LuogoNascita = profile.PersonInfo.BirthPlace

        End With
        Return person
    End Function

    Private Function CreateUserInfo(profile As dtoBaseProfile, authentication As AuthenticationProviderType, personInfo As PersonInfo) As lm.Comol.Core.DomainModel.Person
        Dim person As lm.Comol.Core.DomainModel.Person

        If TypeOf profile Is dtoCompany Then
            Dim companyUser As New lm.Comol.Core.DomainModel.CompanyUser()
            companyUser.PersonInfo = UpdatePersonInfo(personInfo, profile, authentication)
            companyUser.CompanyInfo = DirectCast(profile, dtoCompany).Info
            person = companyUser
        ElseIf TypeOf profile Is dtoEmployee Then
            Dim employee As New lm.Comol.Core.DomainModel.Employee()
            Dim agency = New Agency() With {.Id = DirectCast(profile, dtoEmployee).CurrentAgency.Key}
            employee.PersonInfo = UpdatePersonInfo(personInfo, profile, authentication)
            employee.Affiliations.Add(New AgencyAffiliation With {.Agency = agency, .IsEnabled = True})
            person = employee
        Else
            person = New lm.Comol.Core.DomainModel.Person()
        End If

        With person
            .AuthenticationTypeID = authentication
            .Name = profile.Name
            .Surname = profile.Surname
            .TaxCode = profile.TaxCode
            .Login = profile.Login
            .Mail = profile.Mail
            .TypeID = profile.IdProfileType
            .LanguageID = profile.IdLanguage
            .Password = profile.Password
            .FirstLetter = profile.FirstLetter
            .isDisabled = False

            .Job = profile.Job
            .Sector = profile.Sector

        End With
        Return person
    End Function

    Private Function CreatePersonInfo(profile As dtoBaseProfile, authentication As AuthenticationProviderType) As lm.Comol.Core.DomainModel.PersonInfo
        Dim personInfo As New lm.Comol.Core.DomainModel.PersonInfo
        With personInfo
            .RemoteUniqueID = profile.Login

            .BirthDate = profile.PersonInfo.BirthDate 'DateSerial(2999, 1, 1)
            .BirthPlace = profile.PersonInfo.BirthPlace
            
            .SecondaryMail = profile.Mail
            .DefaultShowMailAddress = profile.ShowMail
            .IdProvince = Me.SystemSettings.Presenter.DefaultProvinceID
            .IdNation = Me.SystemSettings.Presenter.DefaultNationID
            .isInternalProfile = (authentication = AuthenticationProviderType.Internal)
            .IdIstitution = Me.PageUtility.IstituzioneID
        End With
        Return personInfo
    End Function

    Private Function UpdatePersonInfo(personInfo As PersonInfo, profile As dtoBaseProfile, authentication As AuthenticationProviderType) As lm.Comol.Core.DomainModel.PersonInfo

        If IsNothing(personInfo) Then
            personInfo = New PersonInfo()
            personInfo.BirthDate = DateSerial(2999, 1, 1)
        Else
            profile.PersonInfo = personInfo
        End If

        With PersonInfo
            .RemoteUniqueID = profile.Login
            .SecondaryMail = profile.Mail
            .DefaultShowMailAddress = profile.ShowMail
            .IdProvince = Me.SystemSettings.Presenter.DefaultProvinceID
            .IdNation = Me.SystemSettings.Presenter.DefaultNationID
            .isInternalProfile = (authentication = AuthenticationProviderType.Internal)
            .IdIstitution = Me.PageUtility.IstituzioneID
        End With
        Return PersonInfo
    End Function

    Private Sub AddUserToOrganization(ByVal oPerson As COL_Persona, ByVal idOrganization As Integer, ByVal isDefault As Boolean)
        Try
            ' FIRST 1 ADD TO ORGANIZATION
            oPerson.AssociaOrganizzazione(idOrganization, IIf(isDefault, 1, 0))

            Dim DefaultCommunityId As Integer = COL_BusinessLogic_v2.Comunita.COL_Comunita.AggiungiPersonaOrganizzazione(oPerson.ID, oPerson.TipoPersona.ID, idOrganization, True, True, False, oPerson.RicezioneSMS)

        Catch ex As Exception

        End Try
    End Sub

#End Region
#Region "Add Specialized"
    Private Function AddExternalUser(ByVal oExternalUser As CL_Esterno.COL_Esterno, ByVal dto As dtoExternal) As Integer
        oExternalUser.Mansione = dto.ExternalUserInfo
        oExternalUser.Aggiungi()
        Return oExternalUser.ID
    End Function
    Private Function AddCompanyUser(ByVal person As lm.Comol.Core.DomainModel.CompanyUser, provider As AuthenticationProvider) As Integer
        Dim result As lm.Comol.Core.DomainModel.CompanyUser = Nothing
        Try
            result = CurrentPresenter.AddCompanyUser(person, provider)
        Catch ex As Exception

        End Try
        If IsNothing(result) Then
            Return 0
        Else
            Return result.Id
        End If

    End Function
    Private Function AddEmployee(ByVal person As lm.Comol.Core.DomainModel.Employee, provider As AuthenticationProvider) As Integer
        Dim result As lm.Comol.Core.DomainModel.Employee = Nothing
        Try
            result = CurrentPresenter.AddEmployee(person, provider)
        Catch ex As Exception

        End Try
        If IsNothing(result) Then
            Return 0
        Else
            Return result.Id
        End If

    End Function
#End Region

#End Region

#Region "Action 2 - Other Organizations"
    Public Sub UpdateAddProfilesToOrganizations(actionIndex As Integer, orgnCount As Integer, orgIndex As Integer, created As Boolean, name As String) Implements IViewProfilesImport.UpdateAddProfilesToOrganizations
        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current

        progress.CurrentOperationText = String.Format(Resource.getValue("ProfileImportAction." & ProfileImportAction.AddToOtherOrganizations.ToString & "." & created.ToString), name)

        progress.PrimaryValue = actionIndex
        progress.PrimaryPercent = Int((actionIndex / progress.PrimaryTotal) * 100)

        progress.SecondaryTotal = orgnCount
        progress.SecondaryValue = orgIndex
        progress.SecondaryPercent = Int((orgIndex / orgnCount) * 100)
        progress.TimeEstimated = (orgnCount - orgIndex) * 100
    End Sub
#End Region

#Region "Action 3- Add Communities"
    Public Sub UpdateAddProfilesToCommunities(actionIndex As Integer, commCount As Integer, commIndex As Integer, created As Boolean, name As String) Implements IViewProfilesImport.UpdateAddProfilesToCommunities
        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current

        progress.CurrentOperationText = String.Format(Resource.getValue("ProfileImportAction." & ProfileImportAction.AddToCommunities.ToString & "." & created.ToString), name)

        progress.PrimaryValue = actionIndex
        progress.PrimaryPercent = Int((actionIndex / progress.PrimaryTotal) * 100)

        progress.SecondaryTotal = commCount
        progress.SecondaryValue = commIndex
        progress.SecondaryPercent = Int((commIndex / commCount) * 100)
        progress.TimeEstimated = (commCount - commIndex) * 100
    End Sub
#End Region

#Region "Action 4- Add Mail"
    Public ReadOnly Property CurrentSmtpConfig As lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig Implements IViewProfilesImport.CurrentSmtpConfig
        Get
            Return PageUtility.CurrentSmtpConfig
        End Get
    End Property
    Public Sub UpdateSendMailToProfile(actionIndex As Integer, mailCount As Integer, mailIndex As Integer, created As Boolean, name As String) Implements IViewProfilesImport.UpdateSendMailToProfile
        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current

        progress.CurrentOperationText = String.Format(Resource.getValue("ProfileImportAction." & ProfileImportAction.SendMail.ToString & "." & created.ToString), name)

        progress.PrimaryValue = actionIndex
        progress.PrimaryPercent = Int((actionIndex / progress.PrimaryTotal) * 100)


        progress.SecondaryTotal = mailCount
        progress.SecondaryValue = mailIndex
        progress.SecondaryPercent = Int((mailIndex / mailCount) * 100)
        progress.TimeEstimated = (mailCount - mailIndex) * 100
    End Sub
#End Region

#End Region

    Private Sub UpdateProgressContext()
        Const total As Integer = 100

        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current

        For j As Integer = 0 To 3
            progress.PrimaryTotal = 1
            progress.PrimaryValue = 1
            For i As Integer = 0 To total - 1

                progress.PrimaryPercent = (4 - j) * 100
                progress.SecondaryTotal = total
                progress.SecondaryValue = i
                progress.SecondaryPercent = i

                progress.CurrentOperationText = "Step " & i.ToString()

                If Not Response.IsClientConnected Then
                    'Cancel button was clicked or the browser was closed, so stop processing
                    Exit For
                End If

                progress.TimeEstimated = (total - i) * 100
                'Stall the current thread for 0.1 seconds
                System.Threading.Thread.Sleep(100)
            Next
        Next

    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub

   
   
End Class