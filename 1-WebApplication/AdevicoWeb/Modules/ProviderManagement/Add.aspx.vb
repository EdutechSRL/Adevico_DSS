Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class AddAuthenticationProvider
    Inherits PageBase
    Implements IViewAddProvider

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
    Private _Presenter As AddProviderPresenter
    Private ReadOnly Property CurrentPresenter() As AddProviderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddProviderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowAdd As Boolean Implements IViewAddProvider.AllowAdd
        Get
            Return Me.ViewStateOrDefault("AllowAdd", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowAdd") = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewAddProvider.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagementTop.Visible = value
            Me.HYPbackToManagementTop.NavigateUrl = BaseUrl & RootObject.Management()
        End Set
    End Property
    Public Property AvailableSteps As List(Of ProviderWizardStep) Implements IViewAddProvider.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of ProviderWizardStep))
        End Get
        Set(value As List(Of ProviderWizardStep))
            Me.ViewState("AvailableSteps") = value
        End Set
    End Property
    Public ReadOnly Property CurrentStep As ProviderWizardStep Implements IViewAddProvider.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", ProviderWizardStep.None)
        End Get
    End Property
    Public Property IdProvider As Long Implements IViewAddProvider.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdProvider") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProviderManagement", "Modules", "ProviderManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceAddProviderTitle")
            Me.Master.ServiceNopermission = .getValue("serviceAddProviderNopermission")

            .setLabel(LBselectProviderType_t)
            .setHyperLink(Me.HYPbackToManagementTop, True, True)

            .setButton(BTNbackBottom, True, , , True)
            .setButton(BTNbackTop, True, , , True)
            .setButton(BTNnextBottom, True, , , True)
            .setButton(BTNnextTop, True, , , True)
            .setButton(BTNcompleteTop, True, , , True)
            .setButton(BTNcompleteBottom, True, , , True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Wizard"
    Public Sub GotoStep(pStep As ProviderWizardStep) Implements IViewAddProvider.GotoStep
        Select Case pStep
            Case ProviderWizardStep.SelectType
                Me.MLVwizard.SetActiveView(VIWselectProviderType)
            Case ProviderWizardStep.ProviderData
                Me.MLVwizard.SetActiveView(VIWproviderData)
            Case ProviderWizardStep.DefaultTranslation
                Me.MLVwizard.SetActiveView(VIWtranslation)
            Case ProviderWizardStep.Summary
                Me.MLVwizard.SetActiveView(VIWsummary)
        End Select

        Me.BTNbackBottom.Visible = (pStep <> ProviderWizardStep.SelectType)
        Me.BTNbackTop.Visible = (pStep <> ProviderWizardStep.SelectType)
        Me.BTNcompleteBottom.Visible = (pStep = ProviderWizardStep.Summary)
        Me.BTNcompleteTop.Visible = (pStep = ProviderWizardStep.Summary)
        Me.BTNnextBottom.Visible = (pStep <> ProviderWizardStep.Summary)
        Me.BTNnextTop.Visible = (pStep <> ProviderWizardStep.Summary)

        Me.ViewState("CurrentStep") = pStep
        If Me.CurrentStep = ProviderWizardStep.Summary Then
            Me.LBstepTitle.Text = String.Format(Resource.getValue("ProviderWizardStep." & Me.CurrentStep.ToString & ".Title"))
        Else
            Me.LBstepTitle.Text = String.Format(Resource.getValue("ProviderWizardStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
        End If

        Me.LBstepDescription.Text = Resource.getValue("ProviderWizardStep." & pStep.ToString & ".Description")
    End Sub
    Private Function GetStepNumber(pStep As ProviderWizardStep) As Integer
        Dim Number As Integer = 1
        Dim list As List(Of ProviderWizardStep) = Me.AvailableSteps
        For Each item As ProviderWizardStep In list
            If item = pStep Then
                Return Number
            Else
                Number += 1
            End If
        Next
    End Function
    Private Sub BTNbackBottom_Click(sender As Object, e As System.EventArgs) Handles BTNbackBottom.Click, BTNbackTop.Click
        Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
    End Sub

    Private Sub BTNcompleteBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteBottom.Click, BTNcompleteTop.Click
        If IdProvider = 0 Then
            Me.CurrentPresenter.AddProvider(ProviderInfo, CTRLtranslation.Translation)
        Else
            Me.GotoManagement(IdProvider)
        End If
    End Sub

    Private Sub BTNnextBottom_Click(sender As Object, e As System.EventArgs) Handles BTNnextBottom.Click, BTNnextTop.Click
        Me.CurrentPresenter.MoveToNextStep(CurrentStep)
    End Sub

#End Region

#End Region
#Region "Step 1 Provider Types"

    Public Sub LoadAvailableTypes(types As List(Of AuthenticationProviderType)) Implements IViewAddProvider.LoadAvailableTypes
        Dim oList As List(Of TranslatedItem(Of String)) = (From s In types Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("AuthenticationProviderType." & s.ToString)}).ToList

        Me.RBLproviderTypes.DataSource = oList.OrderBy(Function(t) t.Translation).ToList
        Me.RBLproviderTypes.DataValueField = "Id"
        Me.RBLproviderTypes.DataTextField = "Translation"
        Me.RBLproviderTypes.DataBind()
        If Me.RBLproviderTypes.Items.Count > 0 Then
            Me.RBLproviderTypes.SelectedIndex = 0
        End If
    End Sub
    Public Property SelectedType As AuthenticationProviderType Implements IViewAddProvider.SelectedType
        Get
            If Me.RBLproviderTypes.SelectedIndex < 0 Then
                Return AuthenticationProviderType.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of AuthenticationProviderType).GetByString(RBLproviderTypes.SelectedValue, AuthenticationProviderType.None)
            End If
        End Get
        Set(value As AuthenticationProviderType)
            If Me.RBLproviderTypes.Items.Count > 0 Then
                Me.RBLproviderTypes.SelectedValue = value.ToString
            End If
        End Set
    End Property
#End Region

#Region "Step 2 Provider Data"
    Public ReadOnly Property IdentifierFields As IdentifierField Implements IViewAddProvider.IdentifierFields
        Get
            Return Me.CTRLauthenticationProvider.IdentifierFields
        End Get
    End Property
    Public ReadOnly Property ProviderInfo As dtoProvider Implements IViewAddProvider.ProviderInfo
        Get
            Return Me.CTRLauthenticationProvider.CurrentProvider
        End Get
    End Property
    Public ReadOnly Property ProviderInfoType As AuthenticationProviderType Implements IViewAddProvider.ProviderInfoType
        Get
            Return Me.CTRLauthenticationProvider.ProviderType
        End Get
    End Property
    Public Sub LoadProviderInfo(provider As dtoProvider, allowAdvancedSettings As Boolean) Implements IViewAddProvider.LoadProviderInfo
        Me.CTRLauthenticationProvider.InitializeControl(provider, allowAdvancedSettings)
    End Sub
    Public Function ValidateProviderInfo() As Boolean Implements IViewAddProvider.ValidateProviderInfo
        Return Me.CTRLauthenticationProvider.ValidateContent
    End Function
#End Region

#Region "Step 3 Translation"
    Public ReadOnly Property isTranslationInitialized As Boolean Implements IViewAddProvider.isTranslationInitialized
        Get
            Return Me.CTRLtranslation.isInitialized()
        End Get
    End Property
    Public Sub InitializeTranslation(idProvider As Long, idLanguage As Integer, fields As IdentifierField) Implements IViewAddProvider.InitializeTranslation
        Me.CTRLtranslation.InitializeControl(idProvider, idLanguage, fields)
    End Sub
    Public Sub UpdateTranslationView(fields As IdentifierField) Implements IViewAddProvider.UpdateTranslationView
        Me.CTRLtranslation.UpdateTranslationView(fields)
    End Sub
    Public Function ValidateDefaultTranslation() As Boolean Implements IViewAddProvider.ValidateDefaultTranslation
        Return Me.CTRLtranslation.ValidateContent
    End Function
#End Region

#Region "Step 4 Summary"
    Public Sub LoadSummaryInfo(provider As dtoProvider, type As AuthenticationProviderType) Implements IViewAddProvider.LoadSummaryInfo
        Me.LBsummary.Text = String.Format(Me.Resource.getValue("Summary"), provider.Name, provider.UniqueCode, Resource.getValue("AuthenticationProviderType." & type.ToString))
        If (type <> AuthenticationProviderType.Internal AndAlso type <> AuthenticationProviderType.None) Then
            Me.LBsummary.Text &= "<br/><br/>" & String.Format(Me.Resource.getValue("SummarySettings"), Resource.getValue("AuthenticationProviderType." & type.ToString))
        End If
    End Sub
#End Region

    Public Sub DisplaySessionTimeout() Implements IViewAddProvider.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.AddProvider
        webPost.Redirect(dto)
    End Sub

    Public Sub GotoManagement(idProvider As Long) Implements IViewAddProvider.GotoManagement
        PageUtility.RedirectToUrl(RootObject.Management(idProvider))
    End Sub
    Public Sub GotoSettings(idProvider As Long, type As AuthenticationProviderType) Implements IViewAddProvider.GotoSettings
        PageUtility.RedirectToUrl(RootObject.EditProviderSettings(IdProvider, type))
    End Sub
    Public Sub DisplayNoPermission() Implements IViewAddProvider.DisplayNoPermission
        Me.BindNoPermessi()
    End Sub
    Public Sub DisplayErrorSaving() Implements IViewAddProvider.DisplayErrorSaving
        Me.ViewState("CurrentStep") = ProviderWizardStep.ErrorMessages
        Me.MLVwizard.SetActiveView(VIWproviderError)
        Me.LTerrors.Text = Resource.getValue("ErrorAddingProvider")

    End Sub

    
End Class