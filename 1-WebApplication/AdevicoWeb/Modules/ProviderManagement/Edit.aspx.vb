Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class EditAuthenticationProvider
    Inherits PageBase
    Implements IViewEditProvider

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
    Private _Presenter As EditProviderPresenter
    Private ReadOnly Property CurrentPresenter() As EditProviderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditProviderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdProvider As Long Implements IViewEditProvider.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProvider") = value
        End Set
    End Property
    Public Property IdentifierFields As IdentifierField Implements IViewEditProvider.IdentifierFields
        Get
            Return Me.CTRLauthenticationProvider.IdentifierFields
        End Get
        Set(value As lm.Comol.Core.Authentication.IdentifierField)
            Me.CTRLauthenticationProvider.IdentifierFields = value
        End Set
    End Property
    Public ReadOnly Property PreloadedIdProvider As Long Implements IViewEditProvider.PreloadedIdProvider
        Get
            If IsNumeric(Request.QueryString("IdProvider")) Then
                Return CLng(Request.QueryString("IdProvider"))
            Else : Return CLng(0)
            End If
        End Get
    End Property
    Public Property AllowEdit As Boolean Implements IViewEditProvider.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowEdit") = value
            Me.LNBsaveTop.Visible = value
        End Set
    End Property

    Public WriteOnly Property AllowManagement As Boolean Implements IViewEditProvider.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagementTop.Visible = value
            Me.HYPbackToManagementTop.NavigateUrl = BaseUrl & RootObject.Management(IdProvider)
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
            Me.Master.ServiceTitle = .getValue("serviceEditProviderTitle")
            Me.Master.ServiceNopermission = .getValue("serviceEditProviderNopermission")

            .setLinkButton(Me.LNBsaveTop, True, True)
            .setHyperLink(Me.HYPbackToManagementTop, True, True)
            .setHyperLink(Me.HYPadvancedSettings, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region


#Region "Display"
    Public Sub DisplayErrorSaving() Implements IViewEditProvider.DisplayErrorSaving
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayErrorSaving")
    End Sub

    Public Sub DisplayNoPermission() Implements IViewEditProvider.DisplayNoPermission
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayNoPermission")
    End Sub

    Public Sub DisplayProviderUnknown() Implements IViewEditProvider.DisplayProviderUnknown
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayProviderUnknown")
    End Sub
    Public Sub DisplayDeletedProvider(name As String, type As AuthenticationProviderType) Implements IViewEditProvider.DisplayDeletedProvider
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = String.Format(Me.Resource.getValue("DisplayDeletedProvider"), name, Me.Resource.getValue("AuthenticationProviderType." & type.ToString))
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewEditProvider.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditProvider(IIf(IdProvider > 0, IdProvider, PreloadedIdProvider))
        webPost.Redirect(dto)
    End Sub
#End Region


    Public Sub LoadProviderInfo(provider As dtoProvider, name As String, type As AuthenticationProviderType, allowAdvancedSettings As Boolean) Implements IViewEditProvider.LoadProviderInfo
        Me.MLVdata.SetActiveView(VIWproviderData)
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceEditNameProviderTitle"), name, Me.Resource.getValue("AuthenticationProviderType." & type.ToString))
        HYPadvancedSettings.Visible = allowAdvancedSettings
        HYPadvancedSettings.NavigateUrl = BaseUrl & RootObject.EditProviderSettings(provider.IdProvider, provider.Type)
        Me.CTRLauthenticationProvider.InitializeControl(provider, False)
    End Sub
    Private Sub CTRLauthenticationProvider_UpdateIdentifierFields(fields As IdentifierField) Handles CTRLauthenticationProvider.UpdateIdentifierFields
        Me.CTRLtranslation.SaveData()
        Me.CTRLtranslation.InitializeControl(IdProvider, CInt(TBSlanguages.SelectedTab.Value), fields)
    End Sub
    Public Sub LoadTranslations(fields As IdentifierField, languages As List(Of lm.Comol.Core.DomainModel.Language)) Implements IViewEditProvider.LoadTranslations
        TBSlanguages.Tabs.Clear()
        Me.TBSlanguages.Visible = (languages.Count > 0)

        If languages.Count > 0 Then
            For Each Language As lm.Comol.Core.DomainModel.Language In languages
                Me.TBSlanguages.Tabs.Add(New Telerik.Web.UI.RadTab() With {.Text = Language.Name, .Value = Language.Id})
            Next

            Dim dLanguage As lm.Comol.Core.DomainModel.Language = languages.Where(Function(l) l.isDefault).FirstOrDefault
            If Not IsNothing(dLanguage) Then
                Me.TBSlanguages.Tabs.FindTabByValue(dLanguage.Id).Selected = True
            Else
                Me.TBSlanguages.Tabs(0).Selected = True
            End If
            Me.CTRLauthenticationProvider.UpdateContainer = True
            Me.CTRLtranslation.InitializeControl(IdProvider, CInt(TBSlanguages.SelectedTab.Value), fields)
        End If
      
    End Sub
    Private Sub TBSlanguages_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSlanguages.TabClick
        Me.CTRLtranslation.SaveData()
        Me.CTRLtranslation.InitializeControl(IdProvider, CInt(e.Tab.Value), Me.CTRLauthenticationProvider.IdentifierFields)
    End Sub

    Public Sub GotoManagement() Implements IViewEditProvider.GotoManagement
        PageUtility.RedirectToUrl(RootObject.Management(IdProvider))
    End Sub

    Private Sub LNBsaveTop_Click(sender As Object, e As System.EventArgs) Handles LNBsaveTop.Click
        If Me.CTRLauthenticationProvider.SaveData Then
            Me.CTRLtranslation.SaveData()
            Me.GotoManagement()
        End If
    End Sub

End Class