Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports System.Web.Services
Imports Telerik.Web.UI
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Public Class UC_AuthenticationStepProfileInfo
    Inherits BaseControl
    Implements IViewStepProfileInfo

#Region "Context"
    Private _Presenter As StepProfileInfoPresenter
    Private ReadOnly Property CurrentPresenter() As StepProfileInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New StepProfileInfoPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Public Event AddJqueryScript(ByVal script As String)

    Public Property SelectedIdOrganization As Integer Implements IViewStepProfileInfo.SelectedIdOrganization
        Get
            Return ViewStateOrDefault("SelectedIdOrganization", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("SelectedIdOrganization") = value
        End Set
    End Property

    Private Const ItemsPerRequest As Integer = 10

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public Event UpdateUserLanguage(ByVal language As Lingua)
    Property CurrentUserInfo() As dtoBaseProfile
        Get
            Dim oInfo As dtoBaseProfile

            Select Case idProfileType
                Case lm.Comol.Core.DomainModel.UserTypeStandard.ExternalUser
                    oInfo = New dtoExternal
                Case lm.Comol.Core.DomainModel.UserTypeStandard.Company
                    oInfo = New dtoCompany
                Case lm.Comol.Core.DomainModel.UserTypeStandard.Employee
                    oInfo = New dtoEmployee
                Case Else
                    oInfo = New dtoBaseProfile
            End Select

            With oInfo
                .Login = TXBlogin.Text
                If Not String.IsNullOrEmpty(Me.TXBmail.Text) Then
                    Me.TXBmail.Text = Me.TXBmail.Text.ToLower
                End If
                .Mail = Me.TXBmail.Text
                If String.IsNullOrEmpty(Me.TXBpassword.Text) Then
                    .Password = Me.GetSavedUserPassword
                Else
                    .Password = Me.TXBpassword.Text
                End If

                .Name = Me.TXBname.Text
                .Surname = Me.TXBsurname.Text
                .TaxCode = Me.TXBtaxCode.Text
                .ShowMail = Me.CBXshowMail.Checked
                If Me.DDLlanguage.Items.Count > 0 Then
                    .IdLanguage = Me.DDLlanguage.SelectedValue
                    .LanguageName = Me.DDLlanguage.SelectedItem.Text
                Else
                    .IdLanguage = Me.PageUtility.LinguaID
                    .LanguageName = ""
                End If
                If .Surname.Length > 0 Then
                    .FirstLetter = .Surname(0).ToString.ToLower
                End If

                .AuthenticationProvider = AuthenticationProvider
                .IdProfileType = idProfileType

                .Job = Me.TXBJob.Text
                .Sector = Me.TXBSector.Text
            End With

            Select Case idProfileType
                Case lm.Comol.Core.DomainModel.UserTypeStandard.ExternalUser
                    Dim oDto As dtoExternal = oInfo
                    oDto.ExternalUserInfo = ExternalUserInfo
                    Return oDto
                Case lm.Comol.Core.DomainModel.UserTypeStandard.Company
                    Dim oDto As dtoCompany = oInfo
                    oDto.Info.Address = CompanyAddress
                    oDto.Info.City = CompanyCity
                    oDto.Info.Name = CompanyName
                    oDto.Info.Region = CompanyRegion
                    oDto.Info.TaxCode = CompanyTaxCode
                    oDto.Info.ReaNumber = CompanyReaNumber
                    oDto.Info.AssociationCategories = CompanyAssociationCategories
                    Return oDto
                Case lm.Comol.Core.DomainModel.UserTypeStandard.Employee
                    Dim oDto As dtoEmployee = oInfo
                    oDto.CurrentAgency = CurrentAgency
                    Return oDto
                Case Else
                    Return oInfo
            End Select
            Return oInfo
        End Get
        Set(ByVal value As dtoBaseProfile)
            With value
                Me.TXBlogin.Text = .Login
                Me.TXBmail.Text = .Mail
                Me.TXBconfirmPwd.Text = ""
                Me.TXBpassword.Text = .Password
                Me.TXBname.Text = StrConv(.Name, VbStrConv.ProperCase)
                Me.TXBsurname.Text = StrConv(.Surname, VbStrConv.ProperCase)
                Me.TXBtaxCode.Text = .TaxCode
                Me.CBXshowMail.Checked = .ShowMail
                Me.DDLlanguage.SelectedValue = .IdLanguage


                Me.TXBJob.Text = .Job
                Me.TXBSector.Text = .Sector

            End With
        End Set
    End Property
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Private Property AuthenticationProvider As AuthenticationProviderType
        Get
            Return ViewStateOrDefault("AuthenticationProvider", AuthenticationProviderType.Internal)
        End Get
        Set(value As AuthenticationProviderType)
            ViewState("AuthenticationProvider") = value
        End Set
    End Property
    Private Property idProfileType As Integer
        Get
            Return ViewStateOrDefault("idProfileType", 0)
        End Get
        Set(value As Integer)
            ViewState("idProfileType") = value
        End Set
    End Property
    Public ReadOnly Property GetExternalCredentials As dtoExternalCredentials
        Get
            Dim dto As New dtoExternalCredentials
            If AuthenticationProvider <> AuthenticationProviderType.Internal Then
                If Not String.IsNullOrEmpty(TXBexternalLong.Text) AndAlso IsNumeric(TXBexternalLong.Text) Then
                    dto.IdentifierLong = CLng(TXBexternalLong.Text)
                End If
                dto.IdentifierString = TXBexternalString.Text
            End If
            Return dto
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBcognome_t)
            .setLabel(LBconfirmpwd)
            .setLabel(LBlingua_t)
            .setLabel(LBlogin_t)
            .setLabel(LBmail_t)
            .setLabel(LBmailInfo)
            .setLabel(LBmostraMail)
            .setLabel(LBnome_t)
            .setLabel(LBpwd)
            .setLabel(LBtaxCode_t)


            .setLabel(Me.LBexternalUser_t)
            .setLabel(Me.LBcompanyName_t)
            .setLabel(Me.LBcompanyAddress_t)
            .setLabel(Me.LBcompanyCity_t)
            .setLabel(Me.LBcompanyRegion_t)
            .setLabel(Me.LBcompanyTaxCode_t)
            .setLabel(Me.LBcompanyReaNumber_t)
            .setLabel(Me.LBassociationCategories_t)

            .setLabel(LBmailDuplicate)
            .setLabel(LBtaxCodeDuplicate)
            .setLabel(LBloginDuplicate)
            .setLabel(LBexternalDuplicateLong)
            .setLabel(LBexternalDuplicateString)
            .setLabel(LBagency_t)
        End With
    End Sub
#End Region

#Region "Initialize Control"
    Public Sub InitializeControlForManagement(ByVal dto As dtoBaseProfile, ByVal IdProfileType As Integer, ByVal dtoProvider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, ByVal idOrganization As Integer)
        Dim allowLong As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(dtoProvider.IdentifierFields, IdentifierField.longField)
        Dim allowString As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(dtoProvider.IdentifierFields, IdentifierField.stringField)
        Me.SPNexternalLong.Visible = allowLong
        Me.SPNexternalString.Visible = allowString

        Me.RFVexternalLong.Visible = allowLong
        Me.RNVexternalLong.Visible = allowLong
        Me.RFVexternalString.Visible = allowString

        Me.LBexternalLong_t.Text = dtoProvider.Translation.FieldLong
        Me.LBexternalString_t.Text = dtoProvider.Translation.FieldString
        Me.TXBexternalString.Text = ""
        Me.TXBexternalLong.Text = ""
        SelectedIdOrganization = idOrganization
        InitializeControl(dto, IdProfileType, dtoProvider.Type, True, dtoProvider.AllowAdminProfileInsert)
    End Sub
    Public Sub InitializeControl(ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType, ByVal idOrganization As Integer)
        InitializeControl(New dtoBaseProfile(), IdProfileType, authentication, idOrganization)
    End Sub
    Public Sub InitializeControl(ByVal dto As dtoBaseProfile, ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType, ByVal idOrganization As Integer)
        SelectedIdOrganization = idOrganization
        InitializeControl(dto, IdProfileType, authentication, False, False)
    End Sub


    Private Sub InitializeControl(ByVal dto As dtoBaseProfile, ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType, ByVal forManagement As Boolean, ByVal allowAdminInsert As Boolean)
        Me.AuthenticationProvider = authentication
        Me.idProfileType = IdProfileType
        Initialize(dto, IdProfileType, authentication, forManagement, allowAdminInsert)

        Me.isInitialized = True
        Me.LBloginDuplicate.Visible = False
        Me.LBmailDuplicate.Visible = False
        Me.LBtaxCodeDuplicate.Visible = False
        Me.DDLlanguage.AutoPostBack = Not forManagement
    End Sub
    Private Sub Initialize(ByVal dto As dtoBaseProfile, ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType, ByVal forManagement As Boolean, ByVal allowAdminInsert As Boolean)
        Dim LanguageID As Integer = Session("LinguaID")
        Try
            If Me.DDLlanguage.SelectedIndex > -1 Then
                LanguageID = Me.DDLlanguage.SelectedValue
            End If
            Me.DDLlanguage.DataSource = ManagerLingua.List
            Me.DDLlanguage.DataTextField = "Nome"
            Me.DDLlanguage.DataValueField = "ID"
            Me.DDLlanguage.DataBind()

            Me.DDLlanguage.SelectedValue = LanguageID
            dto.IdLanguage = LanguageID
        Catch ex As Exception

        End Try
        Me.CurrentUserInfo = dto
        Me.MLVuserInfo.SetActiveView(IIf(authentication = AuthenticationProviderType.Internal, VIWinternalAuthentication, IIf(allowAdminInsert, VIWadminInsert, VIWexternalAuthentication)))



        Me.TXBlogin.Enabled = (authentication = AuthenticationProviderType.Internal)
        'Me.TXBname.Enabled = (authentication <> AuthenticationProviderType.Ldap AndAlso authentication <> AuthenticationProviderType.Shibboleth) AndAlso Not String.IsNullOrEmpty(dto.Name)
        'Me.TXBsurname.Enabled = (authentication <> AuthenticationProviderType.Ldap AndAlso authentication <> AuthenticationProviderType.Shibboleth) AndAlso Not String.IsNullOrEmpty(dto.Surname)
        Me.SPNtaxCode.Visible = Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        If Me.SystemSettings.Presenter.DefaultTaxCodeRequired = False AndAlso dto.TaxCode = "" Then
            Dim oGenerator As New TaxCodeGenerator
            Me.TXBtaxCode.Text = oGenerator.Generate
        End If
        Me.SPNconfirmPassword.Visible = Not forManagement
        Me.CMVpassword.Visible = Not forManagement
        Me.RFVlogin.Visible = Not forManagement
        ' Me.SPNpassword.Visible = Not forManagement
        InitializeAdvancedControl(IdProfileType, dto)

    End Sub

#Region "Advanced profiles"
    Private Sub InitializeAdvancedControl(ByVal IdProfileType As Integer, ByVal dto As dtoBaseProfile)
        Select Case IdProfileType
            Case lm.Comol.Core.DomainModel.UserTypeStandard.ExternalUser
                InitializeExternalUser()
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Company
                InitializeCompany()
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Employee
                If TypeOf (dto) Is dtoEmployee Then
                    InitializeEmployee(DirectCast(dto, dtoEmployee))
                Else
                    InitializeEmployee(Nothing)
                End If
            Case Else
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWempty)
        End Select
    End Sub

#Region "ExternalUser"
    Private Sub InitializeExternalUser()
        Me.MLVprofileAdvancedInfo.SetActiveView(Me.VIWexternal)
        Me.TXBexternalUser.Text = ""
    End Sub
    Private Property ExternalUserInfo() As String
        Get
            Return Me.TXBexternalUser.Text
        End Get
        Set(ByVal value As String)
            Me.TXBexternalUser.Text = value
        End Set
    End Property
#End Region

#Region "Company"
    Private Sub InitializeCompany()
        Me.TXBcompanyName.Text = ""
        Me.TXBcompanyAddress.Text = ""
        Me.TXBcompanyCity.Text = ""
        Me.TXBcompanyRegion.Text = ""
        Me.TXBcompanytaxCode.Text = ""
        Me.MLVprofileAdvancedInfo.SetActiveView(VIWcompany)
    End Sub
    Private Property CompanyName() As String
        Get
            Return Me.TXBcompanyName.Text
        End Get
        Set(ByVal value As String)
            Me.TXBcompanyName.Text = value
        End Set
    End Property
    Private Property CompanyAddress() As String
        Get
            Return Me.TXBcompanyAddress.Text
        End Get
        Set(ByVal value As String)
            Me.TXBcompanyAddress.Text = value
        End Set
    End Property
    Private Property CompanyCity() As String
        Get
            Return Me.TXBcompanyCity.Text
        End Get
        Set(ByVal value As String)
            Me.TXBcompanyCity.Text = value
        End Set
    End Property
    Private Property CompanyRegion() As String
        Get
            Return Me.TXBcompanyRegion.Text
        End Get
        Set(ByVal value As String)
            Me.TXBcompanyRegion.Text = value
        End Set
    End Property
    Private Property CompanyTaxCode() As String
        Get
            Return Me.TXBcompanytaxCode.Text
        End Get
        Set(ByVal value As String)
            Me.TXBcompanytaxCode.Text = value
        End Set
    End Property
    Private Property CompanyReaNumber() As String
        Get
            Return Me.TXBreaNumber.Text
        End Get
        Set(ByVal value As String)
            Me.TXBreaNumber.Text = value
        End Set
    End Property
    Private Property CompanyAssociationCategories() As String
        Get
            Return Me.TXBassociationCategories.Text
        End Get
        Set(ByVal value As String)
            Me.TXBassociationCategories.Text = value
        End Set
    End Property
#End Region

#Region "Employee"
    Private Sub InitializeEmployee(ByVal dto As dtoEmployee)
        Me.CTRLagency.InitalizeControl(New lm.Comol.Core.DomainModel.Helpers.StringItem(Of String) With {.Id = Me.SelectedIdOrganization, .Name = "idOrganization"})
        If Not IsNothing(dto) AndAlso dto.CurrentAgency.Key > 0 Then
            Me.CTRLagency.SelectedLongItem = dto.CurrentAgency
        End If

        Me.MLVprofileAdvancedInfo.SetActiveView(VIWemployee)
    End Sub
    Private ReadOnly Property CurrentAgency() As KeyValuePair(Of Long, String)
        Get
            Dim result As KeyValuePair(Of Long, String) = Me.CTRLagency.SelectedLongItem
            Dim agency As lm.Comol.Core.DomainModel.Agency
            If String.IsNullOrEmpty(result.Value) Then
                result = CurrentPresenter.GetEmptyAgency(SelectedIdOrganization)
            ElseIf Not CurrentPresenter.ExistAgency(CLng(result.Key)) Then
                result = CurrentPresenter.GetEmptyAgency(SelectedIdOrganization)
            End If
            Return result
        End Get
    End Property
#End Region

#End Region

    Public Sub ReloadLanguageSettings(language As Lingua)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
    Public Sub DisableInput(items As List(Of ProfileAttributeType)) Implements IViewStepProfileInfo.DisableInput
        TXBname.ReadOnly = items.Contains(ProfileAttributeType.name) AndAlso Not String.IsNullOrEmpty(TXBname.Text)
        TXBsurname.ReadOnly = items.Contains(ProfileAttributeType.surname) AndAlso Not String.IsNullOrEmpty(TXBsurname.Text)
        If SPNtaxCode.Visible Then
            TXBtaxCode.ReadOnly = items.Contains(ProfileAttributeType.taxCode) AndAlso Not String.IsNullOrEmpty(TXBtaxCode.Text)
        End If
        TXBmail.ReadOnly = items.Contains(ProfileAttributeType.mail) AndAlso Not String.IsNullOrEmpty(TXBmail.Text)
        TXBexternalUser.ReadOnly = items.Contains(ProfileAttributeType.externalUserInfo) AndAlso Not String.IsNullOrEmpty(TXBexternalUser.Text)
        TXBcompanyName.ReadOnly = items.Contains(ProfileAttributeType.companyName) AndAlso Not String.IsNullOrEmpty(TXBcompanyName.Text)
        TXBcompanytaxCode.ReadOnly = items.Contains(ProfileAttributeType.companyTaxCode) AndAlso Not String.IsNullOrEmpty(TXBcompanytaxCode.Text)
        TXBreaNumber.ReadOnly = items.Contains(ProfileAttributeType.companyReaNumber) AndAlso Not String.IsNullOrEmpty(TXBreaNumber.Text)
        TXBcompanyAddress.ReadOnly = items.Contains(ProfileAttributeType.companyAddress) AndAlso Not String.IsNullOrEmpty(TXBcompanyAddress.Text)
        TXBcompanyCity.ReadOnly = items.Contains(ProfileAttributeType.companyCity) AndAlso Not String.IsNullOrEmpty(TXBcompanyCity.Text)
        TXBcompanyRegion.ReadOnly = items.Contains(ProfileAttributeType.companyRegion) AndAlso Not String.IsNullOrEmpty(TXBcompanyRegion.Text)
        TXBassociationCategories.ReadOnly = items.Contains(ProfileAttributeType.companyAssociations) AndAlso Not String.IsNullOrEmpty(TXBassociationCategories.Text)
        CTRLagency.ReadOnly = (items.Contains(ProfileAttributeType.agencyExternalCode) OrElse items.Contains(ProfileAttributeType.agencyNationalCode) OrElse items.Contains(ProfileAttributeType.agencyTaxCode)) AndAlso CTRLagency.SelectedLongItem.Key > 0
    End Sub
#End Region

#Region "Standard user"
    Private Sub DDLlanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLlanguage.SelectedIndexChanged
        Dim oLingua As New Lingua
        oLingua = ManagerLingua.GetByID(Me.DDLlanguage.SelectedValue)
        If Not IsNothing(oLingua) Then
            RaiseEvent UpdateUserLanguage(oLingua)
        End If
    End Sub
    Public Sub SaveUserPassword()
        Me.ViewState("SavedUserPassword") = Me.TXBpassword.Text
    End Sub
    Public Sub SaveUserPassword(ByVal pPassword As String)
        Me.ViewState("SavedUserPassword") = pPassword
    End Sub
    Public Function GetSavedUserPassword() As String
        Return Me.ViewState("SavedUserPassword")
    End Function
#End Region

    Public Sub LoadProfileInfoError(errors As List(Of ProfilerError))
        Me.LBloginDuplicate.Visible = errors.Contains(ProfilerError.loginduplicate)
        Me.LBmailDuplicate.Visible = errors.Contains(ProfilerError.mailDuplicate)
        Me.LBtaxCodeDuplicate.Visible = errors.Contains(ProfilerError.taxCodeDuplicate) AndAlso Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        Me.LBexternalDuplicateLong.Visible = errors.Contains(ProfilerError.externalUniqueIDduplicate)
        Me.LBexternalDuplicateString.Visible = errors.Contains(ProfilerError.externalUniqueIDduplicate)
    End Sub
    Public Sub UnloadProfileInfoError()
        Me.LBloginDuplicate.Visible = False
        Me.LBmailDuplicate.Visible = False
        Me.LBtaxCodeDuplicate.Visible = False
    End Sub


   
End Class