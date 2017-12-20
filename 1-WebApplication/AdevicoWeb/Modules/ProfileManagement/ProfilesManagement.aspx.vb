Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Partial Public Class AuthenticationProfilesManagement
    Inherits PageBase
    Implements IViewProfilesManagement

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
    Private _Presenter As ProfilesManagementPresenter
    Private ReadOnly Property CurrentPresenter() As ProfilesManagementPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfilesManagementPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property AllowAutoupdate As Boolean Implements IViewProfilesManagement.AllowAutoupdate
        Get
            Return Me.CBXautoUpdate.Checked
        End Get
    End Property
    Private ReadOnly Property AllowSearchByTaxCode As Boolean Implements IViewProfilesManagement.AllowSearchByTaxCode
        Get
            Return Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        End Get
    End Property
    Private Property AllowDisplayLoginInfo As Boolean Implements IViewProfilesManagement.AllowDisplayLoginInfo
        Get
            Return ViewStateOrDefault("AllowDisplayLoginInfo", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowDisplayLoginInfo") = value
        End Set
    End Property

    Private Property CurrentDisplayLoginInfo As Boolean Implements IViewProfilesManagement.CurrentDisplayLoginInfo
        Get
            Return Me.CBXdisplayLoginInfo.Checked
        End Get
        Set(value As Boolean)
            Me.CBXdisplayLoginInfo.Checked = value
        End Set
    End Property
    Private ReadOnly Property GetCurrentFilters As dtoFilters Implements IViewProfilesManagement.GetCurrentFilters
        Get
            Dim dto As New dtoFilters

            With dto
                .IdAvailableOrganization = AvailableOrganizations
                .Ascending = OrderAscending
                .idProvider = SelectedIdProvider
                .OrderBy = OrderBy
                .IdOrganization = SelectedIdOrganization
                .PageIndex = Me.Pager.PageIndex
                .PageSize = Me.DDLpage.SelectedValue
                .IdProfileType = SelectedIdProfileType
                If SelectedIdProfileType = UserTypeStandard.Employee Then
                    .IdAgency = SelectedIdAgency
                Else
                    .IdAgency = -1
                End If
                .SearchBy = SelectedSearchBy
                .StartWith = CurrentStartWith
                .Status = SelectedStatusProfile
                .Value = CurrentValue
                .DisplayLoginInfo = CurrentDisplayLoginInfo
            End With
            Return dto
        End Get
    End Property
    Private ReadOnly Property PreLoadedPageSize As Integer Implements IViewProfilesManagement.PreLoadedPageSize
        Get
            If IsNumeric(Request.QueryString("PageSize")) Then
                Return CInt(Request.QueryString("PageSize"))
            Else
                Return Me.DDLpage.Items(0).Value
            End If
        End Get
    End Property
    Private Property SelectedIdOrganization As Integer Implements IViewProfilesManagement.SelectedIdOrganization
        Get
            If (Me.DDLorganizations.SelectedIndex > -1) Then
                Return Me.DDLorganizations.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLorganizations.Items.FindByValue(value)) Then
                Me.DDLorganizations.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedIdProfileType As Integer Implements IViewProfilesManagement.SelectedIdProfileType
        Get
            If (Me.DDLprofileType.SelectedIndex > -1) Then
                Return Me.DDLprofileType.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLprofileType.Items.FindByValue(value)) Then
                Me.DDLprofileType.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedIdProvider As Long Implements IViewProfilesManagement.SelectedIdProvider
        Get
            If (Me.DDLauthenticationType.SelectedIndex > -1) Then
                Return Me.DDLauthenticationType.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Long)
            If Not IsNothing(Me.DDLauthenticationType.Items.FindByValue(value)) Then
                Me.DDLauthenticationType.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedIdAgency As Long Implements IViewProfilesManagement.SelectedIdAgency
        Get
            If (Me.DDLagencies.SelectedIndex > -1) Then
                Return Me.DDLagencies.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Long)
            If Not IsNothing(Me.DDLagencies.Items.FindByValue(value)) Then
                Me.DDLagencies.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedSearchBy As SearchProfilesBy Implements IViewProfilesManagement.SelectedSearchBy
        Get
            If (Me.DDLsearchBy.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchProfilesBy).GetByString(Me.DDLsearchBy.SelectedValue, SearchProfilesBy.All)
            Else
                Return SearchProfilesBy.All
            End If
        End Get
        Set(value As SearchProfilesBy)
            If Not IsNothing(Me.DDLsearchBy.Items.FindByValue(value.ToString)) Then
                Me.DDLsearchBy.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property SelectedStatusProfile As StatusProfile Implements IViewProfilesManagement.SelectedStatusProfile
        Get
            If (Me.DDLstatus.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatusProfile).GetByString(Me.DDLstatus.SelectedValue, StatusProfile.AllStatus)
            Else
                Return StatusProfile.AllStatus
            End If
        End Get
        Set(value As StatusProfile)
            If Not IsNothing(Me.DDLstatus.Items.FindByValue(value.ToString)) Then
                Me.DDLstatus.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentStartWith As String Implements IViewProfilesManagement.CurrentStartWith
        Get
            Return CTRLalphabetSelector.SelectedItem
        End Get
        Set(value As String)
            CTRLalphabetSelector.SelectedItem = value
        End Set
    End Property
    Public Property CurrentValue As String Implements IViewProfilesManagement.CurrentValue
        Get
            If String.IsNullOrEmpty(TXBvalue.Text) Then
                Return ""
            Else
                Return TXBvalue.Text.Trim
            End If
        End Get
        Set(value As String)
            TXBvalue.Text = value
        End Set
    End Property
    Public ReadOnly Property GetSavedFilters As dtoFilters Implements IViewProfilesManagement.GetSavedFilters
        Get
            Dim dto As New dtoFilters
            Try
                With dto
                    Dim value As String = Me.Request.Cookies("AdminProfilesManagement")("Ascending")
                    If String.IsNullOrEmpty(value) Then
                        .Ascending = False
                    Else
                        .Ascending = CBool(value)
                    End If
                    .idProvider = Me.Request.Cookies("AdminProfilesManagement")("idProvider")
                    .OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderProfilesBy).GetByString(Me.Request.Cookies("AdminProfilesManagement")("OrderBy"), OrderProfilesBy.SurnameAndName)
                    .IdOrganization = Me.Request.Cookies("AdminProfilesManagement")("IdOrganization")
                    .IdAgency = Me.Request.Cookies("AdminProfilesManagement")("IdAgency")
                    .PageIndex = Me.Request.Cookies("AdminProfilesManagement")("PageIndex")
                    .PageSize = Me.Request.Cookies("AdminProfilesManagement")("PageSize")
                    .IdProfileType = Me.Request.Cookies("AdminProfilesManagement")("IdProfileType")
                    .SearchBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchProfilesBy).GetByString(Me.Request.Cookies("AdminProfilesManagement")("SearchBy"), SearchProfilesBy.All)
                    .StartWith = Me.Request.Cookies("AdminProfilesManagement")("StartWith")
                    .Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatusProfile).GetByString(Me.Request.Cookies("AdminProfilesManagement")("Status"), StatusProfile.AllStatus)
                    .Value = Me.Request.Cookies("AdminProfilesManagement")("Value")
                    value = Me.Request.Cookies("AdminProfilesManagement")("CurrentDisplayLoginInfo")
                    If String.IsNullOrEmpty(value) Then
                        .DisplayLoginInfo = False
                    Else
                        .DisplayLoginInfo = CBool(value)
                    End If
                End With
            Catch ex As Exception
                With dto
                    .Ascending = True
                    .idProvider = -1
                    .OrderBy = OrderProfilesBy.SurnameAndName
                    .IdOrganization = -1
                    .IdAgency = -1
                    .PageIndex = 0
                    .PageSize = CurrentPageSize
                    .IdProfileType = -1
                    .SearchBy = SearchProfilesBy.Contains
                    .StartWith = ""
                    .Status = StatusProfile.AllStatus
                    .Value = ""
                    .DisplayLoginInfo = False
                End With
            End Try

            Return dto

        End Get
    End Property
    Public ReadOnly Property PreloadedReloadFilters As Boolean Implements IViewProfilesManagement.PreloadedReloadFilters
        Get
            Return (Request.QueryString("ReloadFilters") = "true")
        End Get
    End Property
    Public Property SearchFilters As dtoFilters Implements IViewProfilesManagement.SearchFilters
        Get
            Return ViewStateOrDefault("SearchFilters", GetCurrentFilters)
        End Get
        Set(value As dtoFilters)
            ViewState("SearchFilters") = value
            SaveCurrentFilters(value)
        End Set
    End Property
    Public Property OrderAscending As Boolean Implements IViewProfilesManagement.OrderAscending
        Get
            Return ViewStateOrDefault("OrderAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("OrderAscending") = value
        End Set
    End Property
    Public Property OrderBy As OrderProfilesBy Implements IViewProfilesManagement.OrderBy
        Get
            Return ViewStateOrDefault("OrderBy", OrderProfilesBy.SurnameAndName)
        End Get
        Set(value As OrderProfilesBy)
            ViewState("OrderBy") = value
        End Set
    End Property
    Public Property CurrentPageSize As Integer Implements IViewProfilesManagement.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Public Property Pager As PagerBase Implements IViewProfilesManagement.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = CurrentPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    'Public ReadOnly Property GetTranslatedProfileStatus As List(Of TranslatedItem(Of StatusProfile)) Implements IViewProfilesManagement.GetTranslatedProfileStatus
    '    Get
    '        Dim list As New List(Of StatusProfile)
    '        list.Add(StatusProfile.Active)
    '        list.Add(StatusProfile.Disabled)
    '        list.Add(StatusProfile.Waiting)
    '        Return (From s In list Select New TranslatedItem(Of StatusProfile) With {.Id = s, .Translation = Me.Resource.getValue("StatusProfile." & s.ToString)}).ToList

    '    End Get
    'End Property
    Public ReadOnly Property GetTranslatedProfileTypes As List(Of TranslatedItem(Of Integer)) Implements IViewProfilesManagement.GetTranslatedProfileTypes
        Get
            Return (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID)
                    Select New TranslatedItem(Of Integer) With {.Id = o.ID, .Translation = o.Descrizione}).ToList
        End Get
    End Property
    Public Property AvailableColumns As List(Of ProfileColumn) Implements IViewProfilesManagement.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of ProfileColumn))
        End Get
        Set(value As List(Of ProfileColumn))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Private Property AvailableLogins As Dictionary(Of Integer, String) Implements IViewProfilesManagement.AvailableLogins
        Get
            Return ViewStateOrDefault("AvailableLogins", New Dictionary(Of Integer, String))
        End Get
        Set(value As Dictionary(Of Integer, String))
            ViewState("AvailableLogins") = value
        End Set
    End Property
    Private Property AvailableOrganizations As List(Of Integer) Implements IViewBaseProfilesManagement.AvailableOrganizations
        Get
            Return ViewStateOrDefault("AvailableOrganizations", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("AvailableOrganizations") = value
        End Set
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property TranslateModalView(viewName As String) As String
        Get
            Return Resource.getValue("TranslateModalView.AuthenticationProviders")
        End Get
    End Property
    Public ReadOnly Property isColumnVisible(ByVal column As ProfileColumn) As Boolean
        Get
            Return AvailableColumns.Contains(column)
        End Get
    End Property
    Public ReadOnly Property BackGroundItem(ByVal status As StatusProfile) As String
        Get
            If status = StatusProfile.Active Then
                Return "ROW_Normal_Small"
            ElseIf status = StatusProfile.Disabled Then
                Return "ROW_Disabilitate_Small"
            Else
                Return "ROW_Alternate_Small"
            End If
        End Get
    End Property
    Public ReadOnly Property DefaultPageSize() As Integer
        Get
            Return 25
        End Get
    End Property
    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Me.Resource.getValue("OnLoadingTranslation")
        End Get
    End Property
    'Public Property TemporaryProfileFilters() As dtoProfileFilters
    '    Get
    '        If TypeOf Me.ViewState("TemporaryProfileFilters") Is dtoProfileFilters Then
    '            Return Me.ViewState("TemporaryProfileFilters")
    '        Else
    '            Return Nothing
    '        End If
    '    End Get
    '    Set(ByVal value As dtoProfileFilters)
    '        Me.ViewState("TemporaryProfileFilters") = value
    '    End Set
    'End Property
    'Public Property TemporaryStartWith() As String
    '    Get
    '        If String.IsNullOrEmpty(Me.ViewState("TemporaryStartWith")) Then
    '            Return ""
    '        Else
    '            Return Me.ViewState("TemporaryStartWith")
    '        End If
    '    End Get
    '    Set(ByVal value As String)
    '        Me.ViewState("TemporaryStartWith") = value
    '    End Set
    'End Property
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
        Me.PGgrid.Pager = Pager
        Me.Page.Form.DefaultButton = Me.BTNcerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBvalue.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNcerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBvalue.UniqueID
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False

        Me.SetFocus(Me.TXBvalue)
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
            Me.CBXautoUpdate.Checked = True

            'Me.UDPprofiles.Update()
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
        MyBase.SetCulture("pg_ProfilesManagement", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("serviceNopermission")

            .setLabel(LBorganizzazione_t)
            .setLabel(LBprofileType_t)
            .setLabel(LBstatus_t)

            .setLabel(LBauthenticationType_t)
            .setLabel(LBtipoRicerca_t)

            .setLabel(LBvalore_t)
            .setButton(BTNcerca, True)
            .setHyperLink(Me.HYPaddProfile, True, True)
            .setHyperLink(Me.HYPimportProfile, True, True)

            Me.HYPaddProfile.NavigateUrl = Me.BaseUrl & RootObject.AddPortalProfile()
            Me.HYPimportProfile.NavigateUrl = Me.BaseUrl & RootObject.ImportProfiles
            .setCheckBox(Me.CBXautoUpdate)
            .setLabel(LBpagesize)

            .setLabel(LBagencyFilter_t)
            .setLabel(LBdisplayLoginInfo)
            .setCheckBox(Me.CBXdisplayLoginInfo)

            DDLagencies.Attributes.Add("onchange", "onUpdating();")
            DDLauthenticationType.Attributes.Add("onchange", "onUpdating();")
            DDLorganizations.Attributes.Add("onchange", "onUpdating();")
            DDLpage.Attributes.Add("onchange", "onUpdating();")
            DDLprofileType.Attributes.Add("onchange", "onUpdating();")
            DDLstatus.Attributes.Add("onchange", "onUpdating();")
            CBXautoUpdate.Attributes.Add("onchange", "onUpdating();")
            CBXdisplayLoginInfo.Attributes.Add("onchange", "onUpdating();")
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"

    Private Sub DisplaySessionTimeout() Implements IViewProfilesManagement.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.ManagementProfilesWithFilters()
        webPost.Redirect(dto)
    End Sub

#Region "Load Data"
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))) Implements IViewProfilesManagement.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))) Implements IViewProfilesManagement.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoExternal))) Implements IViewProfilesManagement.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub
  
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))) Implements IViewProfilesManagement.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub
#End Region

#Region "DisplayMessage"
    Private Sub NoPermission() Implements IViewProfilesManagement.NoPermission
        Master.ShowNoPermission = True
    End Sub
    Private Sub NoPermissionToAdmin() Implements IViewBaseProfilesManagement.NoPermissionToAdmin
        Master.ShowNoPermission = True
        Master.ServiceNopermission = Resource.getValue("NoPermissionToAdmin")
    End Sub
    Private Sub DisplayPasswordChanged(name As String) Implements IViewProfilesManagement.DisplayPasswordChanged
        DisplayMessage(String.Format(Resource.getValue("ProfilesManager.DisplayPasswordChanged"), name), Helpers.MessageType.success)
    End Sub

    Private Sub DisplayProfileActivated(disabled As Boolean, name As String) Implements IViewProfilesManagement.DisplayProfileActivated
        DisplayMessage(String.Format(Resource.getValue("ProfilesManager.DisplayProfileActivated." & disabled.ToString.ToLower), name), IIf(disabled, Helpers.MessageType.success, Helpers.MessageType.error))
    End Sub

    Private Sub DisplayProfileDisabled(disabled As Boolean, name As String) Implements IViewProfilesManagement.DisplayProfileDisabled
        DisplayMessage(String.Format(Resource.getValue("ProfilesManager.DisplayProfileDisabled." & disabled.ToString.ToLower), name), IIf(disabled, Helpers.MessageType.success, Helpers.MessageType.error))
    End Sub

    Private Sub DisplayProfileEnabled(disabled As Boolean, name As String) Implements IViewProfilesManagement.DisplayProfileEnabled
        DisplayMessage(String.Format(Resource.getValue("ProfilesManager.DisplayProfileEnabled." & disabled.ToString.ToLower), name), IIf(disabled, Helpers.MessageType.success, Helpers.MessageType.error))
    End Sub

    Private Sub DisplayUnableToChangePassword(name As String) Implements IViewProfilesManagement.DisplayUnableToChangePassword
        DisplayMessage(String.Format(Resource.getValue("ProfilesManager.DisplayUnableToChangePassword"), name), Helpers.MessageType.error)
    End Sub

    Private Sub DisplayUnableToSendPassword(name As String) Implements IViewProfilesManagement.DisplayUnableToSendPassword
        DisplayMessage(String.Format(Resource.getValue("ProfilesManager.DisplayUnableToChangePassword"), name), Helpers.MessageType.alert)
    End Sub

    Private Sub DisplayMessage(message As String, t As Helpers.MessageType)
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(message, t)
    End Sub
#End Region

    Private Function SendMail(user As lm.Comol.Core.Authentication.InternalLoginInfo, password As String) As Boolean Implements IViewProfilesManagement.SendMail
        Dim sent As Boolean = False

        Try
            Dim mailTranslated As MailLocalized = PageUtility.LocalizedMail(user.Person.LanguageID)
            Dim mail As New COL_E_Mail(mailTranslated)

            mail.Mittente = mailTranslated.SystemSender
            mail.IndirizziTO.Add(New MailAddress(user.Person.Mail))

            mail.Oggetto = Me.Resource.getValue("newPasswordmailSubject")
            mail.Body = String.Format(Me.Resource.getValue("newPasswordmailAdministratorBody"), user.Person.SurnameAndName, user.Login, password, Me.PageUtility.ApplicationUrlBase)

            mail.Body = mail.Body & vbCrLf & mailTranslated.SystemFirmaNotifica
            mail.Body = Replace(mail.Body, "<br>", vbCrLf)

            mail.InviaMail()
            If mail.Errore = Errori_Db.None Then
                sent = True
            End If
        Catch ex As Exception
            sent = False
        End Try
        Return sent
    End Function

#End Region

#Region "Filters"

#Region "Loaders"
    Private Sub LoadAvailableOrganizations(items As List(Of Organization), idDefaultOrganization As Integer) Implements IViewProfilesManagement.LoadAvailableOrganizations
        If items.Any Then
            AvailableOrganizations = items.Select(Function(i) i.Id).ToList()
        End If
        Me.DDLorganizations.DataSource = items
        Me.DDLorganizations.DataValueField = "Id"
        Me.DDLorganizations.DataTextField = "Name"
        Me.DDLorganizations.DataBind()
        If items.Count > 1 Then
            Me.DDLorganizations.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLorganizations." & -1), -1))
        End If
    End Sub
    Private Sub LoadProfileTypes(idProfileTypes As List(Of Integer), IdDefaultProfileType As Integer) Implements IViewProfilesManagement.LoadProfileTypes
        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest AndAlso (idProfileTypes.Contains(o.ID)) Select o).ToList
        Me.DDLprofileType.DataSource = oUserTypes
        Me.DDLprofileType.DataValueField = "ID"
        Me.DDLprofileType.DataTextField = "Descrizione"
        Me.DDLprofileType.DataBind()
        If oUserTypes.Count > 1 Then
            Me.DDLprofileType.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLprofileType." & -1), -1))
        End If
        Me.SelectedIdProfileType = IdDefaultProfileType
    End Sub
    Private Sub LoadAvailableStatus(list As List(Of StatusProfile), defaultStatus As StatusProfile) Implements IViewProfilesManagement.LoadAvailableStatus
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("StatusProfile." & s.ToString)}).ToList

        Me.DDLstatus.DataSource = translations
        Me.DDLstatus.DataValueField = "Id"
        Me.DDLstatus.DataTextField = "Translation"
        Me.DDLstatus.DataBind()
        Me.SelectedStatusProfile = defaultStatus

    End Sub
    Private Sub LoadAuthenticationProviders(providers As List(Of lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider), IdDefaultProvider As Long) Implements IViewProfilesManagement.LoadAuthenticationProviders
        Dim translations As List(Of TranslatedItem(Of String)) = (From p In providers Select New TranslatedItem(Of String) With {.Id = p.IdProvider, .Translation = p.Translation.Name}).ToList

        Me.DDLauthenticationType.Items.Clear()
        Me.DDLauthenticationType.DataSource = translations
        Me.DDLauthenticationType.DataValueField = "Id"
        Me.DDLauthenticationType.DataTextField = "Translation"
        Me.DDLauthenticationType.DataBind()

        If translations.Count > 1 Then
            Me.DDLauthenticationType.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLauthenticationType." & -1), -1))
        End If
        Me.SelectedIdProvider = IdDefaultProvider
    End Sub
    Private Sub LoadAgencies(items As Dictionary(Of Long, String), idDefaultAgency As Long) Implements IViewProfilesManagement.LoadAgencies
        Me.DDLagencies.Items.Clear()
        Me.DDLagencies.DataSource = items
        Me.DDLagencies.DataValueField = "Key"
        Me.DDLagencies.DataTextField = "Value"
        Me.DDLagencies.DataBind()

        If items.Count > 1 OrElse items.Count = 0 Then
            Me.DDLagencies.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLagencies." & -1), -1))
        End If
        Me.DVagencyFilter.Visible = True
        Me.SelectedIdAgency = idDefaultAgency
    End Sub
    Private Sub UnLoadAgencies() Implements IViewProfilesManagement.UnLoadAgencies
        If Me.DDLagencies.SelectedIndex > -1 Then
            Me.DDLagencies.SelectedIndex = 0
        End If
        Me.DVagencyFilter.Visible = False
    End Sub

    Private Sub LoadSearchProfilesBy(list As List(Of SearchProfilesBy), defaultSearch As SearchProfilesBy) Implements IViewProfilesManagement.LoadSearchProfilesBy
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SearchProfilesBy." & s.ToString)}).ToList

        Me.DDLsearchBy.DataSource = translations
        Me.DDLsearchBy.DataValueField = "Id"
        Me.DDLsearchBy.DataTextField = "Translation"
        Me.DDLsearchBy.DataBind()
        Me.SelectedSearchBy = defaultSearch
    End Sub

    Public Sub SaveCurrentFilters(filters As dtoFilters) Implements IViewProfilesManagement.SaveCurrentFilters
        Try
            Me.Response.Cookies("AdminProfilesManagement")("Value") = filters.Value
            Me.Response.Cookies("AdminProfilesManagement")("idProvider") = filters.idProvider
            Me.Response.Cookies("AdminProfilesManagement")("IdOrganization") = filters.IdOrganization
            Me.Response.Cookies("AdminProfilesManagement")("IdProfileType") = filters.IdProfileType
            Me.Response.Cookies("AdminProfilesManagement")("IdAgency") = filters.IdAgency.ToString
            Me.Response.Cookies("AdminProfilesManagement")("SearchBy") = filters.SearchBy.ToString
            Me.Response.Cookies("AdminProfilesManagement")("StartWith") = filters.StartWith

            Me.Response.Cookies("AdminProfilesManagement")("Status") = filters.Status.ToString
            Me.Response.Cookies("AdminProfilesManagement")("PageIndex") = filters.PageIndex
            Me.Response.Cookies("AdminProfilesManagement")("PageSize") = filters.PageSize
            Me.Response.Cookies("AdminProfilesManagement")("OrderBy") = filters.OrderBy
            Me.Response.Cookies("AdminProfilesManagement")("Ascending") = filters.Ascending
            Me.Response.Cookies("AdminProfilesManagement")("DisplayLoginInfo") = filters.DisplayLoginInfo
        Catch ex As Exception

        End Try
    End Sub
    Private Sub InitializeWordSelector(words As List(Of String)) Implements IViewProfilesManagement.InitializeWordSelector
        Me.CTRLalphabetSelector.InitializeControl(words)
    End Sub
    Private Sub InitializeWordSelector(words As List(Of String), activeWord As String) Implements IViewProfilesManagement.InitializeWordSelector
        Me.CTRLalphabetSelector.InitializeControl(words, activeWord)
    End Sub
#End Region

#Region "Filters use"
    Private Sub DDLorganizations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizations.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        'Me.UDPprofiles.Update()
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.ChangeOrganization(SelectedIdOrganization, SelectedIdProfileType, PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub DDLprofileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLprofileType.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        'Me.UDPprofiles.Update()
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.ChangeProfileType(SelectedIdOrganization, SelectedIdProfileType, PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub DDLstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLstatus.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        'Me.UDPprofiles.Update()
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub DDLauthenticationType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLauthenticationType.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub DDLagencies_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLagencies.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.SearchFilters = GetCurrentFilters
        Me.DDLstatus.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLauthenticationType.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLagencies.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.CBXdisplayLoginInfo.AutoPostBack = Me.CBXautoUpdate.Checked
        If Me.CBXautoUpdate.Checked Then
            DDLagencies.Attributes.Add("onchange", "onUpdating();")
            DDLauthenticationType.Attributes.Add("onchange", "onUpdating();")
            DDLorganizations.Attributes.Add("onchange", "onUpdating();")
            DDLpage.Attributes.Add("onchange", "onUpdating();")
            DDLprofileType.Attributes.Add("onchange", "onUpdating();")
            DDLstatus.Attributes.Add("onchange", "onUpdating();")
            CBXautoUpdate.Attributes.Add("onchange", "onUpdating();")
            CBXdisplayLoginInfo.Attributes.Add("onchange", "onUpdating();")
        Else
            CBXdisplayLoginInfo.Attributes.Add("onchange", "")
            DDLauthenticationType.Attributes.Add("onchange", "")
            DDLstatus.Attributes.Add("onchange", "")
            DDLauthenticationType.Attributes.Add("onchange", "")
        End If
        'Me.UDPprofiles.Update()
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub CBXdisplayLoginInfo_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXdisplayLoginInfo.CheckedChanged
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub

    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem
        Dim dto As dtoFilters = GetCurrentFilters
        dto.PageIndex = 0
        dto.PageSize = Me.CurrentPageSize
        dto.StartWith = letter
        Me.SearchFilters = dto
        Me.CurrentStartWith = letter
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(0, Me.CurrentPageSize)
    End Sub
#End Region

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        Dim dto As dtoFilters = GetCurrentFilters

        dto.PageIndex = 0
        dto.PageSize = Me.CurrentPageSize
        Me.SearchFilters = dto
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(0, Me.CurrentPageSize)
    End Sub
#End Region

#Region "Grid use"
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub RPTprofiles_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTprofiles.ItemCommand
        Dim idProfile As Integer = 0
        If IsNumeric(e.CommandArgument) Then
            idProfile = CInt(e.CommandArgument)
        End If
        CTRLmessages.Visible = False
        Select Case e.CommandName
            Case "renewPassword"
                CurrentPresenter.RenewPassword(idProfile)
            Case "disable"
                CurrentPresenter.DisableProfile(idProfile)
            Case "enable"
                CurrentPresenter.EnableProfile(idProfile)
            Case "activate"
                CurrentPresenter.ActivateProfile(idProfile)
            Case "login"
                Dim currentUser As COL_Persona = Me.PageUtility.CurrentUser

                Dim oPersona As New COL_Persona With {.ID = idProfile}
                oPersona.LogonAsUser(e.CommandArgument, Me.Request.UserHostAddress, Session.SessionID, Me.Request.Browser.Browser, Me.PageUtility.SystemSettings.CodiceDB)
                If oPersona.Errore = Errori_Db.None Then
                    currentUser.Logout()
                    currentUser.CancellaConnessione(currentUser.ID, Session.SessionID)
                    Session("LogonAs") = True
                    Me.PageUtility.LogonAsUser(oPersona)
                Else
                    Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, CurrentPageSize)
                End If


            Case "addProvider"
                Me.CTRLprofileProviders.InitializeControlForAdd(idProfile, True)
                Me.LTscriptOpen.Visible = True
            Case "manageProvider"
                Me.CTRLprofileProviders.InitializeControl(idProfile, True)
                Me.LTscriptOpen.Visible = True
        End Select
    End Sub

    Private Sub RPTprofiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTprofiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBnameSurname_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBlogin_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtipo_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBstatusInfo_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBstatus_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcompanyName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBagencyName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBauthentication_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBactions")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim profile As lm.Comol.Core.Authentication.dtoBaseProfile
            Dim permission As dtoProfilePermission = Nothing
            Dim hyperlink As HyperLink = e.Item.FindControl("HYPinfo")
            Dim status As StatusProfile = StatusProfile.None
            Dim providersCount As Long = 0
            Dim oLabel As Label = e.Item.FindControl("LBcompanyName")
            Dim oLiteral As Literal
            Dim authenticationType As lm.Comol.Core.Authentication.AuthenticationProviderType = lm.Comol.Core.Authentication.AuthenticationProviderType.None
            If TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))
                permission = item.Permission
                profile = item.Profile
                status = item.Status
                providersCount = item.ProvidersCount
                authenticationType = item.AuthenticationType
            ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))
                permission = item.Permission
                profile = item.Profile
                status = item.Status
                providersCount = item.ProvidersCount
                oLabel.Text = DirectCast(profile, lm.Comol.Core.Authentication.dtoCompany).Info.Name
                authenticationType = item.AuthenticationType
            ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))
                permission = item.Permission
                profile = item.Profile
                status = item.Status
                providersCount = item.ProvidersCount
                oLabel = e.Item.FindControl("LBagencyName")
                If IsNothing(item.Profile.CurrentAgency) Then
                    oLabel.Text = ""
                Else
                    oLabel.Text = item.Profile.CurrentAgency.Value
                End If
                authenticationType = item.AuthenticationType
            End If

            Resource.setHyperLink(hyperlink, True, True)
            ' 
            If permission.Info OrElse permission.AdvancedInfo Then
                hyperlink.NavigateUrl = Me.BaseUrl & RootObject.ProfileInfo(profile.Id, profile.IdProfileType)
                'hyperlink.Attributes.Add("onClick", "OpenWin('" & Me.BaseUrl & RootObject.ProfileInfo(profile.Id, profile.IdProfileType) & "','480','560','no','yes');return false;")
            End If


            hyperlink.Visible = permission.Info OrElse permission.AdvancedInfo

            hyperlink = e.Item.FindControl("HYPedit")
            hyperlink.NavigateUrl = Me.BaseUrl & RootObject.EditProfile(profile.Id, profile.IdProfileType)
            Resource.setHyperLink(hyperlink, True, True)
            hyperlink.Visible = permission.Edit

            hyperlink = e.Item.FindControl("HYPdelete")
            Resource.setHyperLink(hyperlink, True, True)
            hyperlink.NavigateUrl = Me.BaseUrl & RootObject.DeleteProfile(profile.Id, profile.IdProfileType)
            hyperlink.Visible = permission.Delete

            Dim div As HtmlControl = e.Item.FindControl("DVeditType")
            div.Visible = permission.ChangeProfileType

            hyperlink = e.Item.FindControl("HYPeditType")
            hyperlink.NavigateUrl = Me.BaseUrl & RootObject.EditProfileType(profile.Id, profile.IdProfileType)
            hyperlink.Visible = permission.ChangeProfileType
            Resource.setHyperLink(hyperlink, True, True)


            Dim oImage As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGstatus")

            oLabel = e.Item.FindControl("LBstatus")
            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBstatus")
            oLabel.Visible = Not permission.EditStatus
            If Not permission.EditStatus Then
                oLabel.Text = Me.Resource.getValue("StatusProfile." & status.ToString)
            End If
            oLinkButton.Visible = permission.EditStatus
            oLinkButton.CommandArgument = profile.Id
            Try
                Select Case status
                    Case StatusProfile.Active
                        oImage.ImageUrl = Me.BaseUrl & "Images/Grid/attivo.jpg"
                        oLinkButton.CommandName = "disable"
                        oLinkButton.Text = Me.Resource.getValue("staus.disable")
                        oLinkButton.ToolTip = Me.Resource.getValue("staus.disable.ToolTip")
                    Case StatusProfile.Disabled
                        oImage.ImageUrl = Me.BaseUrl & "Images/Grid/bloccato.jpg"
                        oLinkButton.CommandName = "enable"
                        oLinkButton.Text = Me.Resource.getValue("staus.enable")
                        oLinkButton.ToolTip = Me.Resource.getValue("staus.enable.ToolTip")
                    Case StatusProfile.Waiting
                        oImage.ImageUrl = Me.BaseUrl & "Images/Grid/inattesa.jpg"
                        oLinkButton.CommandName = "activate"
                        oLinkButton.Text = Me.Resource.getValue("staus.activate")
                        oLinkButton.ToolTip = Me.Resource.getValue("staus.activate.ToolTip")
                End Select
                oImage.ToolTip = Resource.getValue("StatusProfile." & status.ToString)
            Catch ex As Exception

            End Try

            div = e.Item.FindControl("DVlogin")
            div.Visible = permission.LogonAs AndAlso SystemSettings.Presenter.EnabledLogonAs
            If permission.LogonAs AndAlso SystemSettings.Presenter.EnabledLogonAs Then
                oLinkButton = e.Item.FindControl("LNBlogin")
                oLinkButton.CommandArgument = profile.Id
                Me.Resource.setLinkButton(oLinkButton, True, True)
            End If

            If permission.RenewPassword AndAlso authenticationType = lm.Comol.Core.Authentication.AuthenticationProviderType.Internal Then
                oLinkButton = e.Item.FindControl("LNBpassword")
                Me.Resource.setLinkButton(oLinkButton, True, True)
                oLinkButton.Visible = True
            End If

            If permission.ManageAuthentication Then
                If providersCount = 0 Then
                    oLinkButton = e.Item.FindControl("LNBloginInfoAdd")
                    Me.Resource.setLinkButton(oLinkButton, True, True)
                    oLinkButton.Visible = True
                Else
                    oLinkButton = e.Item.FindControl("LNBloginInfoManage")
                    Me.Resource.setLinkButton(oLinkButton, True, True)
                    oLinkButton.Visible = True
                End If
            End If

            div = e.Item.FindControl("DVcommunities")
            div.Visible = permission.Edit

            hyperlink = e.Item.FindControl("HYPcommunities")
            hyperlink.NavigateUrl = Me.BaseUrl & RootObject.AddCommunitiesToProfile(profile.Id)
            Resource.setHyperLink(hyperlink, True, True)

            If Not isColumnVisible(2) Then
                If AvailableLogins.ContainsKey(profile.Id) AndAlso CurrentDisplayLoginInfo Then
                    oLabel = e.Item.FindControl("LBlogin")
                    oLabel.Text = Me.Resource.getValue("LBloginDisplay") & " " & AvailableLogins(profile.Id)
                    oLabel.Visible = True
                    If providersCount > 1 Then
                        oLabel.ToolTip = Resource.getValue("loginWIthOtherAuthentication")
                        If Not String.IsNullOrEmpty(oLabel.ToolTip) Then
                            oLabel.ToolTip = String.Format(oLabel.ToolTip, providersCount.ToString)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

#End Region


    Private Sub CTRLprofileProviders_CloseContainer() Handles CTRLprofileProviders.CloseContainer
        Me.LTscriptOpen.Visible = False
        ' Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, Me.CurrentPageSize)
        PageUtility.RedirectToUrl(RootObject.ManagementProfilesWithFilters())
    End Sub

    Private Sub CTRLprofileProviders_CloseContainerAndReload() Handles CTRLprofileProviders.CloseContainerAndReload
        'Me.CloseDialog("AuthenticationProviders")
        'Me.UDPautenticationProviders.Update()
        'Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, Me.CurrentPageSize)
        PageUtility.RedirectToUrl(RootObject.ManagementProfilesWithFilters())
    End Sub

    'Private Sub CTRLprofileProviders_UpdateContainer() Handles CTRLprofileProviders.UpdateContainer
    '    'Me.UDPautenticationProviders.Update()
    'End Sub

    Private Sub DDLpage_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub


   
End Class