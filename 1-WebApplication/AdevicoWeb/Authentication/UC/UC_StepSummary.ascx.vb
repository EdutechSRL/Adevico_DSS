Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.DomainModel.ProfileManagement
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.BaseModules.ProfileManagement

Public Class UC_AuthenticationStepSummary
    Inherits BaseControl
    Implements IViewUserProfileAdd

#Region "IViewUserProfileAdd"
    Public Property IdProfile As Integer Implements IViewUserProfileAdd.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property ExternalCredentials As dtoExternalCredentials Implements IViewUserProfileAdd.ExternalCredentials
        Get
            Return ViewStateOrDefault("ExternalCredentials", New dtoExternalCredentials())
        End Get
        Set(value As dtoExternalCredentials)
            ViewState("ExternalCredentials") = value
        End Set
    End Property
    Public Property IdProvider As Long Implements IViewUserProfileAdd.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProvider") = value
        End Set
    End Property
#End Region

#Region "Context"
    Private _Presenter As UserProfileAddPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As UserProfileAddPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UserProfileAddPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property isCompleted As Boolean
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
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
    Public Property EditingType As EditingType
        Get
            Return ViewStateOrDefault("EditingType", EditingType.None)
        End Get
        Set(ByVal value As EditingType)
            Me.ViewState("EditingType") = value
        End Set
    End Property
    Public Property ProviderSelectOrganization As Boolean
        Get
            Return ViewStateOrDefault("ProviderSelectOrganization", False)
        End Get
        Set(value As Boolean)
            ViewState("ProviderSelectOrganization") = value
        End Set
    End Property
    Public Property ProviderSelectProfileType As Boolean
        Get
            Return ViewStateOrDefault("ProviderSelectProfileType", False)
        End Get
        Set(value As Boolean)
            ViewState("ProviderSelectProfileType") = value
        End Set
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
            .setLabel(LBlingua_t)
            .setLabel(LBlogin_t)
            .setLabel(LBmail_t)
            .setLabel(LBmailInfo)
            .setLabel(LBmostraMail)
            .setLabel(LBnome_t)
            .setLabel(LBtaxCode_t)
            .setLabel(LBauthenticationType_t)

            .setLabel(Me.LBexternalUser_t)
            .setLabel(Me.LBcompanyName_t)
            .setLabel(Me.LBcompanyAddress_t)
            .setLabel(Me.LBcompanyCity_t)
            .setLabel(Me.LBcompanyRegion_t)
            .setLabel(Me.LBcompanyTaxCode_t)
            .setLabel(Me.LBcompanyReaNumber_t)
            .setLabel(Me.LBassociationCategories_t)


            Me.LBcognome_t.Text = Replace(Me.LBcognome_t.Text, "(*)", "")
            Me.LBlogin_t.Text = Replace(Me.LBlogin_t.Text, "(*)", "")
            Me.LBmail_t.Text = Replace(Me.LBmail_t.Text, "(*)", "")
            Me.LBnome_t.Text = Replace(Me.LBnome_t.Text, "(*)", "")

            Me.LBtaxCode_t.Text = Replace(Me.LBtaxCode_t.Text, "(*)", "")
       
            Me.LBexternalUser_t.Text = Replace(Me.LBexternalUser_t.Text, "(*)", "")
            Me.LBcompanyName_t.Text = Replace(Me.LBcompanyName_t.Text, "(*)", "")
            Me.LBcompanyAddress_t.Text = Replace(Me.LBcompanyAddress_t.Text, "(*)", "")
            Me.LBcompanyCity_t.Text = Replace(Me.LBcompanyCity_t.Text, "(*)", "")
            Me.LBcompanyRegion_t.Text = Replace(Me.LBcompanyRegion_t.Text, "(*)", "")
            Me.LBcompanyTaxCode_t.Text = Replace(Me.LBcompanyTaxCode_t.Text, "(*)", "")
            Me.LBcompanyReaNumber_t.Text = Replace(Me.LBcompanyReaNumber_t.Text, "(*)", "")
            Me.LBassociationCategories_t.Text = Replace(Me.LBassociationCategories_t.Text, "(*)", "")
            .setLabel(LBuserType_t)
            .setLabel(LBorganizationInfo_t)
            .setLabel(LBotherOrganizations_t)
            .setLabel(LBsendNotification_t)
            .setCheckBox(CBXnotify)
            .setLabel(LBagency_t)
        End With
    End Sub
#End Region

#Region "Initialize Control"
    Public Sub InitializeForManagement(ByVal dto As dtoBaseProfile, OrganizationName As String, ByVal OtherOrganizations As List(Of String), ByVal ProfileName As String, ByVal IdProfileType As Integer, ByVal provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, ByVal externalCredential As dtoExternalCredentials)
        Me.ExternalCredentials = externalCredential
        Me.SPNauthentication.Visible = True
        Me.SPNnotification.Visible = True
        Me.SPNotherOrganizations.Visible = (OtherOrganizations.Count > 0)
        Me.SPNtaxCode.Visible = Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        Me.IdProvider = provider.IdProvider

        InitializeUserInfoControl(dto, OrganizationName, ProfileName, IdProfileType, provider.Type)

        Me.EditingType = lm.Comol.Core.BaseModules.ProfileManagement.EditingType.FromManagement
        Me.LBauthenticationType.Text = provider.Translation.Name

        If OtherOrganizations.Count > 0 Then
            Me.LBotherOrganizations.Text = OtherOrganizations(0)
            OtherOrganizations.RemoveAt(0)
            For Each Name As String In OtherOrganizations
                Me.LBotherOrganizations.Text &= ", " & Name
            Next
        End If

        If provider.Type <> AuthenticationProviderType.Internal Then
            Me.MLVuserInfo.SetActiveView(VIWadminInsert)
            Me.SPNexternalLong.Visible = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(provider.IdentifierFields, IdentifierField.longField)
            Me.SPNexternalString.Visible = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(provider.IdentifierFields, IdentifierField.stringField)
            Me.LBexternalLong_t.Text = provider.Translation.FieldLong
            Me.LBexternalString_t.Text = provider.Translation.FieldString
            Me.LBexternalString.Text = externalCredential.IdentifierString
            Me.LBexternalLong.Text = externalCredential.IdentifierLong
        End If
        Me.LBorganizationInfo.Text = OrganizationName
    End Sub
    Public Sub InitializeControlForSubscription(ByVal dto As dtoBaseProfile, OrganizationName As String, ByVal ProfileName As String, ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType, externalID As String, _idProvider As Long)
        Me.ExternalCredentials = New dtoExternalCredentials With {.IdentifierString = externalID}
        InitializeControlForSubscription(dto, OrganizationName, ProfileName, IdProfileType, authentication, _idProvider)
    End Sub
    'Public Sub InitializeControlForSubscription(ByVal dto As dtoBaseProfile, OrganizationName As String, ByVal ProfileName As String, ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType, externalID As Long, _idProvider As Long)
    '    ExternalIdLong = externalID

    '    InitializeControlForSubscription(dto, OrganizationName, ProfileName, IdProfileType, authentication, _idProvider)
    'End Sub
    Public Sub InitializeControlForSubscription(ByVal dto As dtoBaseProfile, OrganizationName As String, ByVal ProfileName As String, ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType, _idProvider As Long)
        Me.SPNauthentication.Visible = False
        Me.SPNnotification.Visible = False
        Me.SPNotherOrganizations.Visible = False
        Me.SPNtaxCode.Visible = Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        Me.IdProvider = _idProvider
        Me.LBorganizationInfo.Text = OrganizationName
        InitializeUserInfoControl(dto, OrganizationName, ProfileName, IdProfileType, authentication)

        Me.EditingType = lm.Comol.Core.BaseModules.ProfileManagement.EditingType.User
    End Sub
    Private Sub InitializeUserInfoControl(ByVal dto As dtoBaseProfile, OrganizationName As String, ByVal ProfileName As String, ByVal IdProfileType As Integer, ByVal authentication As AuthenticationProviderType)
        Me.idProfileType = IdProfileType
        Me.AuthenticationProvider = authentication
        If authentication = AuthenticationProviderType.Internal Then
            Me.MLVuserInfo.SetActiveView(VIWinternalAuthentication)
            Me.LBlogin.Text = dto.Login
        Else
            Me.MLVuserInfo.SetActiveView(VIWemptyLoginInfo)
        End If
        Me.LBorganizationInfo.Text = OrganizationName
        Me.LBnome.Text = dto.Name
        Me.LBcognome.Text = dto.Surname
        Me.LBmostraMailUserSelection.Text = Me.Resource.getValue("ShowMail." & dto.ShowMail)
        Me.LBmail.Text = dto.Mail
        Me.LBlingua.Text = dto.LanguageName
        Me.LBtaxCode.Text = dto.TaxCode
        Me.LBuserType.Text = ProfileName
        InitializeAdvancedControl(dto, IdProfileType)
        Me.isInitialized = True
        Me.isCompleted = False
    End Sub

#Region "Advanced profiles"

    Private Sub InitializeAdvancedControl(ByVal dto As dtoBaseProfile, ByVal IdProfileType As Integer)
        Select Case IdProfileType
            Case lm.Comol.Core.DomainModel.UserTypeStandard.ExternalUser
                InitializeExternalUser(DirectCast(dto, dtoExternal))
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Company
                InitializeCompany(DirectCast(dto, dtoCompany))
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Employee
                InitializeEmployee(DirectCast(dto, dtoEmployee))
            Case Else
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWempty)
        End Select
    End Sub

#Region "Initialize"
    Private Sub InitializeExternalUser(dto As dtoExternal)
        Me.MLVprofileAdvancedInfo.SetActiveView(Me.VIWexternal)
        Me.LBexternalUser.Text = dto.ExternalUserInfo
    End Sub
    Private Sub InitializeCompany(dto As dtoCompany)
        Me.LBcompanyName.Text = dto.Info.Name
        Me.LBcompanyAddress.Text = dto.Info.Address
        Me.LBcompanyCity.Text = dto.Info.City
        Me.LBcompanyRegion.Text = dto.Info.Region
        Me.LBcompanyTaxCode.Text = dto.Info.TaxCode
        Me.LBcompanyReaNumber.Text = dto.Info.ReaNumber
        Me.LBassociationCategories.Text = dto.Info.AssociationCategories
        Me.MLVprofileAdvancedInfo.SetActiveView(VIWcompany)
    End Sub
    Private Sub InitializeEmployee(dto As dtoEmployee)
        Me.LBagency.Text = dto.CurrentAgency.Value
        Me.MLVprofileAdvancedInfo.SetActiveView(VIWemployee)
    End Sub
#End Region

#End Region

    Public Sub ReloadLanguageSettings(language As Lingua)
        Dim showMail As Boolean = True
        If Me.LBmostraMailUserSelection.Text <> Me.Resource.getValue("ShowMail." & showMail) Then
            showMail = False
        End If
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
        Me.LBmostraMailUserSelection.Text = Me.Resource.getValue("ShowMail." & showMail)
    End Sub

#End Region

#Region "AddUser"

    'Public Function CreateProfile1(profile As lm.Comol.Core.Authentication.dtoBaseProfile, idProfileType As Integer, profileName As String, idOrganization As Integer, otherOrganizations As System.Collections.Generic.List(Of Integer), provider As lm.Comol.Core.BaseModules.ProfileManagement.dtoBaseProvider, credentials As lm.Comol.Core.BaseModules.ProfileManagement.dtoExternalCredential) As lm.Comol.Core.BaseModules.ProfileManagement.ProfileSubscriptionMessage Implements lm.Comol.Core.BaseModules.ProfileManagement.Presentation.IViewUserProfileAdd.CreateProfile

    'End Function
    Public Function CreateProfile(profile As dtoBaseProfile, idProfileType As Integer, profileName As String, idOrganization As Integer, otherOrganizations As List(Of Integer), provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, credentials As dtoExternalCredentials) As ProfileSubscriptionMessage Implements IViewUserProfileAdd.CreateProfile
        Return CreateUserProfile(profile, idProfileType, profileName, idOrganization, otherOrganizations, provider, credentials)
    End Function
    Public Function CreateProfile(profile As dtoBaseProfile, idProfileType As Integer, profileName As String, idOrganization As Integer, authentication As AuthenticationProviderType, idProvider As Long, credentials As dtoExternalCredentials) As ProfileSubscriptionMessage Implements IViewUserProfileAdd.CreateProfile
        Return CreateUserProfile(profile, idProfileType, profileName, idOrganization, New List(Of Integer), New lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider() With {.Type = authentication, .IdProvider = idProvider}, credentials)
    End Function
    Public Function CreateProfile(profile As dtoBaseProfile, idProfileType As Integer, ProfileName As String, idOrganization As Integer, authentication As AuthenticationProviderType, idProvider As Long) As ProfileSubscriptionMessage Implements IViewUserProfileAdd.CreateProfile
        Return CreateProfile(profile, idProfileType, ProfileName, idOrganization, New List(Of Integer), authentication, idProvider)
    End Function


    Private Function CreateProfile(profile As dtoBaseProfile, idProfileType As Integer, ProfileName As String, idOrganization As Integer, otherOrganizations As List(Of Integer), authentication As AuthenticationProviderType, idProvider As Long) As ProfileSubscriptionMessage Implements IViewUserProfileAdd.CreateProfile
        Return CreateUserProfile(profile, idProfileType, ProfileName, idOrganization, otherOrganizations, New lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider() With {.Type = authentication, .IdProvider = idProvider}, New dtoExternalCredentials)
    End Function

    Private Function CreateUserProfile(profile As dtoBaseProfile, idProfileType As Integer, profileName As String, idOrganization As Integer, otherOrganizations As List(Of Integer), provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, credentials As dtoExternalCredentials) As ProfileSubscriptionMessage
        Dim result As ProfileSubscriptionMessage = ProfileSubscriptionMessage.UnknownError
        If profile.Password = "" Then
            profile.Password = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, True, True, False)
        End If
        Dim oldProfile As COL_Persona = CreateUser(profile, provider.Type)

        Try
            Me.IdProvider = IdProvider
            Dim idPerson As Integer = 0
            If TypeOf profile Is dtoExternal Then
                idPerson = AddExternalUser(CL_Esterno.COL_Esterno.CreateFromPerson(oldProfile), DirectCast(profile, dtoExternal))
            ElseIf TypeOf profile Is dtoCompany Then
                idPerson = AddCompanyUser(CreateUserInfo(profile, provider.Type), provider, credentials)
            ElseIf TypeOf profile Is dtoEmployee Then
                idPerson = AddEmployee(CreateUserInfo(profile, provider.Type), provider, credentials)
            Else
                oldProfile.Aggiungi()
                idPerson = oldProfile.ID
            End If
            If idPerson > 0 Then
                oldProfile.ID = idPerson
                profile.Id = idPerson
                If profile.IdProfileType <> lm.Comol.Core.DomainModel.UserTypeStandard.Company AndAlso profile.IdProfileType <> lm.Comol.Core.DomainModel.UserTypeStandard.Employee Then
                    If EditingType = lm.Comol.Core.BaseModules.ProfileManagement.EditingType.User Then
                        AddAuthenticationInfo(profile, provider.Type)
                    Else
                        AddAuthenticationInfo(profile, provider, credentials)
                    End If
                End If

                Try
                    Me.AddUserToOrganization(oldProfile, idOrganization, True)
                    ' ADD TO OTHER ORGANIZATION
                    For Each OrganizationID As Integer In otherOrganizations
                        Me.AddUserToOrganization(oldProfile, OrganizationID, False)
                    Next
                    If (Me.EditingType = EditingType.User AndAlso SystemSettings.Login.SendRegistrationMail) OrElse (Me.EditingType = EditingType.FromManagement AndAlso Me.CBXnotify.Checked) Then
                        Me.SendMailToUser(oldProfile, profileName, True, provider.Type, profile)
                    End If

                Catch ex As Exception
                    'errore verifica "già inserito"
                End Try
                IdProfile = idPerson
                result = ProfileSubscriptionMessage.Created

                If Me.EditingType = EditingType.User Then
                    Dim MustBeActivated As Boolean = (From o In Me.SystemSettings.Presenter.DefaultProfileTypesToActivate Where o = idProfileType Select o).Any
                    If oldProfile.Bloccata Then
                        If MustBeActivated Then
                            result = ProfileSubscriptionMessage.CreatedAndWaiting
                        Else
                            result = ProfileSubscriptionMessage.CreatedAndDisabled
                        End If
                    ElseIf MustBeActivated Then
                        result = ProfileSubscriptionMessage.CreatedAndDisabled
                    Else
                        result = ProfileSubscriptionMessage.CreatedWithAutoLogon
                    End If
                End If
                Me.isCompleted = True
            End If
        Catch ex As Exception

        End Try

        Return result
    End Function

    Private Sub AddAuthenticationInfo(profile As dtoBaseProfile, authentication As AuthenticationProviderType)
        Select Case authentication
            Case AuthenticationProviderType.Internal
                CurrentPresenter.AddInternalProfile(profile, profile.Id)
            Case AuthenticationProviderType.Url
                If String.IsNullOrEmpty(Me.ExternalCredentials.IdentifierString) = False OrElse Me.ExternalCredentials.IdentifierLong <> 0 Then
                    CurrentPresenter.AddExternalProfile(profile.Id, IdProvider, Me.ExternalCredentials)
                End If
            Case AuthenticationProviderType.UrlMacProvider
                If String.IsNullOrEmpty(Me.ExternalCredentials.IdentifierString) = False OrElse Me.ExternalCredentials.IdentifierLong <> 0 Then
                    CurrentPresenter.AddExternalProfile(profile.Id, IdProvider, Me.ExternalCredentials)
                End If
        End Select
    End Sub
    Private Sub AddAuthenticationInfo(profile As dtoBaseProfile, provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, externalCredentials As dtoExternalCredentials)
        If provider.Type = AuthenticationProviderType.Internal Then
            If String.IsNullOrEmpty(profile.Password) OrElse String.IsNullOrEmpty(profile.Password.Trim) Then
                profile.Password = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, True, True, False)
            End If
            CurrentPresenter.AddInternalProfile(profile, profile.Id)
        Else
            CurrentPresenter.AddExternalProfile(profile.Id, IdProvider, externalCredentials)
        End If
    End Sub

    Private Function CreateUser(profile As dtoBaseProfile, authentication As AuthenticationProviderType) As COL_Persona
        Dim person As New COL_Persona
        With person
            .AUTN_ID = authentication
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
            'If oAuthenticationTypeID = Main.TipoAutenticazione.IOP Then
            Dim oPasswordGenerator As New PasswordGenerator
            .Pwd = oPasswordGenerator.Generate
            'Else
            '    .Pwd = profile.Password
            'End If

            .Note = Me.EditingType.ToString

            Dim MustBeActivated As Boolean = (From o In Me.SystemSettings.Presenter.DefaultProfileTypesToActivate Where o = idProfileType Select o).Any
            .Bloccata = (Me.EditingType = EditingType.User AndAlso Not SystemSettings.Login.ProfileAutoActivation AndAlso (authentication = AuthenticationProviderType.Internal OrElse MustBeActivated))
            .Cap = ""
            .Cellulare = ""
            .Citta = ""
            .Fax = ""
            .HomePage = ""
            .Indirizzo = ""
            .InfoRicevimento = ""
        End With
        Return person
    End Function

    Private Function CreateUserInfo(profile As dtoBaseProfile, authentication As AuthenticationProviderType) As lm.Comol.Core.DomainModel.Person
        Dim person As lm.Comol.Core.DomainModel.Person

        If TypeOf profile Is dtoCompany Then
            Dim companyUser As New lm.Comol.Core.DomainModel.CompanyUser()
            companyUser.PersonInfo = CreatePersonInfo(profile, authentication)
            companyUser.CompanyInfo = DirectCast(profile, dtoCompany).Info
            person = companyUser
        ElseIf TypeOf profile Is dtoEmployee Then
            Dim employee As New lm.Comol.Core.DomainModel.Employee()
            employee.PersonInfo = CreatePersonInfo(profile, authentication)
            employee.Affiliations.Add(New lm.Comol.Core.DomainModel.AgencyAffiliation() With {.Agency = New lm.Comol.Core.DomainModel.Agency() With {.Id = DirectCast(profile, dtoEmployee).CurrentAgency.Key}})
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
            'If oAuthenticationTypeID = Main.TipoAutenticazione.IOP Then
            Dim oPasswordGenerator As New PasswordGenerator
            If String.IsNullOrEmpty(profile.Password) Then
                .Password = oPasswordGenerator.Generate
            Else
                .Password = profile.Password
            End If

            'Else
            '    .Pwd = profile.Password
            'End If
            .FirstLetter = profile.FirstLetter
            Dim MustBeActivated As Boolean = (From o In Me.SystemSettings.Presenter.DefaultProfileTypesToActivate Where o = idProfileType Select o).Any
            .isDisabled = (Me.EditingType = EditingType.User AndAlso Not SystemSettings.Login.ProfileAutoActivation AndAlso (authentication = AuthenticationProviderType.Internal OrElse MustBeActivated))
        End With
        Return person
    End Function

    Private Function CreatePersonInfo(profile As dtoBaseProfile, authentication As AuthenticationProviderType) As lm.Comol.Core.DomainModel.PersonInfo
        Dim personInfo As New lm.Comol.Core.DomainModel.PersonInfo
        With personInfo
            .RemoteUniqueID = profile.Login
            .BirthDate = DateSerial(2999, 1, 1)
            .SecondaryMail = profile.Mail
            .DefaultShowMailAddress = profile.ShowMail
            .IdProvince = Me.SystemSettings.Presenter.DefaultProvinceID
            .IdNation = Me.SystemSettings.Presenter.DefaultNationID
            .isInternalProfile = (authentication = AuthenticationProviderType.Internal)
            .IdIstitution = Me.PageUtility.IstituzioneID
            .Note = Me.EditingType.ToString
        End With
        Return personInfo
    End Function
    'Public Function AddUser(ByVal UserTypeID As Integer, ByVal UserTypeName As String, ByVal DefaultOrganizationID As Integer, ByVal oOrganizationsList As List(Of Integer), ByVal oAuthenticationTypeID As Integer, ByVal oDetails As dtoBaseSpecializedDetails, ByVal oUser As PersonInfo) As Main.ErroreRegistrazione
    '        Case 0 'nessun parametro indicativo assegnato
    '          
    '            If PersonID > 0 Then
    '                oPerson.ID = PersonID
    '                If Me.EditingType = EditingType.User Then
    '                    If oPerson.Bloccata Then
    '                        If (From o In Me.SystemSettings.Presenter.DefaultProfileTypesToActivate Where o = oPerson.TipoPersona.ID).Count > 0 Then
    '                            iResponse = ErroreRegistrazione.InseritoBloccatoInAttesa
    '                        Else
    '                            iResponse = ErroreRegistrazione.InseritoBloccato
    '                        End If
    '                    Else
    '                        iResponse = ErroreRegistrazione.InserimentoElogon
    '                    End If
    '                End If
    '                Me.isCompleted = True
    '            Else
    '                iResponse = ErroreRegistrazione.ErroreGenerico
    '            End If
    '    End Select

    '    'If Not ProfileCreated(iResponse) Then
    '    '    Me.DVerrors.Visible = True
    '    '    Me.LBerrors.Text = Me.Resource.getValue("ErroreRegistrazione." & iResponse)
    '    'End If
    '    Return iResponse
    'End Function


    'Private Function ProfileCreated(ByVal iResponse As Main.ErroreRegistrazione) As Boolean
    '    Return (iResponse = ErroreRegistrazione.InserimentoAvvenuto OrElse iResponse = ErroreRegistrazione.InserimentoElogon OrElse iResponse = ErroreRegistrazione.InseritoBloccato OrElse iResponse = ErroreRegistrazione.InseritoBloccatoInAttesa)
    'End Function

    Private Sub AddUserToOrganization(ByVal oPerson As COL_Persona, ByVal OrganizationID As Integer, ByVal isDefault As Boolean)
        Try
            ' FIRST 1 ADD TO ORGANIZATION
            oPerson.AssociaOrganizzazione(OrganizationID, IIf(isDefault, 1, 0))

            Dim DefaultCommunityId As Integer = 0

            DefaultCommunityId = COL_BusinessLogic_v2.Comunita.COL_Comunita.AggiungiPersonaOrganizzazione(oPerson.ID, oPerson.TipoPersona.ID, OrganizationID, True, True, False, oPerson.RicezioneSMS)

            Try
                Dim oTreeComunita As New COL_BusinessLogic_v2.Comunita.COL_TreeComunita
                Dim oRuoloComunita As New COL_BusinessLogic_v2.Comunita.COL_RuoloPersonaComunita
                Dim oComunita As New COL_BusinessLogic_v2.Comunita.COL_Comunita
                oComunita.Id = DefaultCommunityId
                oComunita.Estrai()
                oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
                oRuoloComunita.EstraiByLinguaDefault(DefaultCommunityId, oPerson.ID)
                If Me.EditingType = EditingType.User AndAlso Not ProviderSelectOrganization Then
                    If oComunita.IsChiusa Then
                        oRuoloComunita.Abilitato = False
                        oRuoloComunita.Attivato = False
                        oRuoloComunita.Modifica()
                    ElseIf (From o In Me.SystemSettings.Presenter.DefaultProfileTypesToActivate Where o = oPerson.TipoPersona.ID).Count > 0 Then
                        oRuoloComunita.Abilitato = False
                        oRuoloComunita.Attivato = False
                        oRuoloComunita.Modifica()
                    End If
                    If (oRuoloComunita.Attivato = False Or oRuoloComunita.Abilitato = False) Then
                        Try
                            oComunita.MailNotifica(oPerson, Me.PageUtility.ApplicationUrlBase, Session.SessionID, oRuoloComunita.TipoRuolo.Nome, Session("LinguaCode"), Me.PageUtility.LocalizedMail)
                        Catch ex As Exception

                        End Try
                    End If
                End If
                oTreeComunita.Directory = PageUtility.ProfilePath & oPerson.ID & "\"
                oTreeComunita.Nome = oPerson.ID & ".xml"
                oTreeComunita.Insert(oComunita, "." & DefaultCommunityId & ".", oComunita.GetNomeResponsabile_NomeCreatore, oRuoloComunita)
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

#Region "Mail"
    Private Sub SendMailToUser(ByVal oPerson As COL_Persona, ByVal UserTypeName As String, ByVal NotifyByMail As Boolean, ByVal authentication As AuthenticationProviderType, ByVal profile As dtoBaseProfile)
        Try
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))

            If NotifyByMail = True Or oPerson.Bloccata Then
                Dim oUtility As New OLDpageUtility(Me.Context)
                Dim oMail As New COL_E_Mail(oUtility.LocalizedMail)
                Dim BodyAdmin As String

                oMail.Mittente = New MailAddress(oResourceConfig.getValue("systemMail"), oResourceConfig.getValue("systemMailSender"))
                If oPerson.Bloccata Then

                    '' NUOVO SISTEMA DI MESSA IN ATTESA PER VERIFICA MAIL

                    oMail.Oggetto = Me.Resource.getValue("mail.infoSubject_Bloccata")
                    oMail.Body = Me.Resource.getValue("mail.infoBody_Bloccata")

                    Dim waiting As lm.Comol.Core.Authentication.WaitingActivationProfile = CurrentPresenter.AddWaitingActivationProfile(oPerson.ID)
                    If Not IsNothing(waiting) Then
                        oMail.Body = Replace(oMail.Body, "#Link1#", " " & oUtility.ApplicationUrlBase & RootObject.ActivateUserProfile(oPerson.ID, waiting.UrlIdentifier))
                    Else
                        oMail.Body = Replace(oMail.Body, "#Link1#", " ")
                    End If

                    'Dim PwdRandom As String

                    'PwdRandom = COL_Persona.generaPasswordNumerica(8)
                    'oPerson.Aggiungi_InAttesaAttivazione(Session.SessionID, oPerson.ID, PwdRandom)

                    'oMail.Oggetto = Me.Resource.getValue("mail.infoSubject_Bloccata")
                    'oMail.Body = Me.Resource.getValue("mail.infoBody_Bloccata")
                    'oMail.Body = Replace(oMail.Body, "#Link1#", " " & oUtility.ApplicationUrlBase & "activateUser.aspx?action=activate&" & oPerson.CriptaParametriAttivazione(oPerson.ID, Session.SessionID, PwdRandom))
                Else
                    oMail.Oggetto = Me.Resource.getValue("mail.infoSubject")
                    oMail.Body = Me.Resource.getValue("mail.infoBody")
                End If
                oMail.Body = Replace(oMail.Body, "#Link2#", " " & oUtility.ApplicationUrlBase)
                BodyAdmin = oMail.Body

                Me.GetGenericMail(oPerson, UserTypeName, authentication, oMail.Body, BodyAdmin, profile)

                oMail.Body = oMail.Body & vbCrLf & vbCrLf & vbCrLf & oResourceConfig.getValue("systemMailFirma")

                oMail.IndirizziTO.Add(New MailAddress(oPerson.Mail))
                oMail.Body = Replace(oMail.Body, "(*)", "")
                oMail.Body = Replace(oMail.Body, "*", "")
                oMail.Body = Replace(oMail.Body, "&nbsp;", "")
                oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
                oMail.InviaMail()

                If oPerson.Bloccata Then
                    oMail.IndirizziTO.Clear()
                    oMail.IndirizziTO.Add(New MailAddress(oResourceConfig.getValue("systemMail")))
                    oMail.Body = BodyAdmin

                    oMail.Body = oMail.Body & vbCrLf & vbCrLf & vbCrLf & oResourceConfig.getValue("systemMailFirma")
                    oMail.Body = Replace(oMail.Body, "(*)", "")
                    oMail.Body = Replace(oMail.Body, "*", "")
                    oMail.Body = Replace(oMail.Body, "&nbsp;", "")
                    oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
                    oMail.InviaMail()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetGenericMail(ByVal oPerson As COL_Persona, ByVal UserTypeName As String, authentication As AuthenticationProviderType, ByRef BodyUserMail As String, ByRef BodyAdminMail As String, ByVal profile As dtoBaseProfile)
        Try
            BodyUserMail = Replace(BodyUserMail, "#Link2#", " " & Me.PageUtility.ApplicationUrlBase)
            BodyAdminMail = BodyUserMail

            BodyUserMail &= vbCrLf _
               & "+-------------------------------------------" & vbCrLf _
               & "|" & vbCrLf


            If authentication = AuthenticationProviderType.Internal Then
                BodyUserMail &= "| " & Me.LBlogin_t.Text & "                 " & profile.Login & vbCrLf
                BodyUserMail &= "| Password:              " & profile.Password & vbCrLf
                '   Else
                '      BodyUserMail &= "| Password:              " & Me.Resource.getValue("DescriptionAuthentication." & oPerson.AUTN_ID) & vbCrLf
            End If

            BodyUserMail &= "| " & Me.LBnome_t.Text & "                  " & oPerson.Nome & vbCrLf _
                & "| " & Me.LBcognome_t.Text & "               " & oPerson.Cognome & vbCrLf
            If Me.SystemSettings.Presenter.DefaultTaxCodeRequired Then
                BodyUserMail &= "| " & Me.LBtaxCode_t.Text & "        " & oPerson.CodFiscale & vbCrLf
            End If
            BodyUserMail &= "| " & Me.LBmail_t.Text & "                  " & oPerson.Mail & vbCrLf

            If oPerson.Bloccata Then
                BodyAdminMail &= vbCrLf _
                   & "+-------------------------------------------" & vbCrLf _
                   & "|" & vbCrLf _
                   & "| " & Me.LBlogin_t.Text & "                 " & profile.Login & vbCrLf _
                   & "| " & Me.LBnome_t.Text & "                  " & oPerson.Nome & vbCrLf _
                   & "| " & Me.LBcognome_t.Text & "               " & oPerson.Cognome & vbCrLf _
                   & "| " & Me.LBmail_t.Text & "                  " & oPerson.Mail & vbCrLf

                If Me.SystemSettings.Presenter.DefaultTaxCodeRequired Then
                    BodyAdminMail &= "| " & Me.LBtaxCode_t.Text & "        " & oPerson.CodFiscale & vbCrLf
                End If
                BodyAdminMail &= "| " & Me.LBuserType_t.Text & "                  " & UserTypeName & vbCrLf

            End If
            Me.GetSpecializedMail(BodyUserMail, BodyAdminMail, profile)

            BodyUserMail &= "|" & vbCrLf _
                & "+------------------------------------------" & vbCrLf

            If oPerson.Bloccata Then

                BodyAdminMail &= vbCrLf & "|" & vbCrLf
                BodyAdminMail &= "+------------------------------------------" & vbCrLf

                '& "| Tipo Persona:               " & Main.TipoPersonaStandard.Dottorando.ToString & vbCrLf _
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetSpecializedMail(ByRef BodyUserMail As String, ByRef BodyAdminMail As String, ByVal item As dtoBaseProfile)
        If TypeOf item Is dtoExternal Then
            GetEsternoMail(BodyUserMail, BodyAdminMail, DirectCast(item, dtoExternal))
        ElseIf TypeOf item Is dtoCompany Then
            GetCompanyUserMail(BodyUserMail, BodyAdminMail, DirectCast(item, dtoCompany))
        ElseIf TypeOf item Is dtoEmployee Then
            GetEmployeeMail(BodyUserMail, BodyAdminMail, DirectCast(item, dtoEmployee))
        End If
    End Sub

    Private Sub GetEsternoMail(ByRef BodyUserMail As String, ByRef BodyAdminMail As String, ByVal oDto As dtoExternal)
        BodyUserMail &= "| " & Me.LBexternalUser_t.Text & "                 " & oDto.ExternalUserInfo & vbCrLf
        BodyAdminMail &= "| " & Me.LBexternalUser_t.Text & "                 " & oDto.ExternalUserInfo & vbCrLf
    End Sub
    Private Sub GetCompanyUserMail(ByRef BodyUserMail As String, ByRef BodyAdminMail As String, ByVal oDto As dtoCompany)
        BodyUserMail &= "| " & Me.LBcompanyName_t.Text & "                 " & oDto.Info.Name & vbCrLf
        BodyUserMail &= "| " & Me.LBcompanyTaxCode_t.Text & "                 " & oDto.Info.TaxCode & vbCrLf
        BodyUserMail &= "| " & Me.LBcompanyReaNumber_t.Text & "                 " & oDto.Info.ReaNumber & vbCrLf
        BodyUserMail &= "| " & Me.LBcompanyAddress_t.Text & "                 " & oDto.Info.Address & vbCrLf
        BodyUserMail &= "| " & Me.LBcompanyCity_t.Text & "                 " & oDto.Info.City & vbCrLf
        BodyUserMail &= "| " & Me.LBcompanyRegion_t.Text & "                 " & oDto.Info.Region & vbCrLf
        BodyUserMail &= "| " & Me.LBassociationCategories_t.Text & "                 " & oDto.Info.associationCategories & vbCrLf

        BodyAdminMail &= "| " & Me.LBcompanyName_t.Text & "                 " & oDto.Info.Name & vbCrLf
        BodyAdminMail &= "| " & Me.LBcompanyTaxCode_t.Text & "                 " & oDto.Info.TaxCode & vbCrLf
        BodyAdminMail &= "| " & Me.LBcompanyReaNumber_t.Text & "                 " & oDto.Info.ReaNumber & vbCrLf
        BodyAdminMail &= "| " & Me.LBcompanyAddress_t.Text & "                 " & oDto.Info.Address & vbCrLf
        BodyAdminMail &= "| " & Me.LBcompanyCity_t.Text & "                 " & oDto.Info.City & vbCrLf
        BodyAdminMail &= "| " & Me.LBcompanyRegion_t.Text & "                 " & oDto.Info.Region & vbCrLf
        BodyAdminMail &= "| " & Me.LBassociationCategories_t.Text & "                 " & oDto.Info.associationCategories & vbCrLf
    End Sub
    Private Sub GetEmployeeMail(ByRef BodyUserMail As String, ByRef BodyAdminMail As String, ByVal oDto As dtoEmployee)
        BodyUserMail &= "| " & Me.LBagency_t.Text & "                 " & oDto.CurrentAgency.Value & vbCrLf

        BodyAdminMail &= "| " & Me.LBagency_t.Text & "                 " & oDto.CurrentAgency.Value & vbCrLf
    End Sub
    ' If Me.TypeAction <> Azione.Iscrizione Then
    'Dim oLdap As New LDAPgenericAuthentication

    '        If IsNothing(oResource) Then
    '            Me.SetCulture(Session("LinguaCode"))
    '            Me.SetupInternazionalizzazione()
    '        End If
    '        If Me.RBLautenticazione.SelectedValue = Main.TipoAutenticazione.LDAP Then
    '            If Not IsNothing(oLdap.FindUser(Me.TBlogin.Text)) Then
    '                Me.TBRautenticazione0.Visible = True
    '                If oLdap.Errore = ErroreLDAP.connessioneFallita Then
    ''  Me.LBerroreLDAP.Text = "Connessione al server LDAP fallita, impossibile registrare l'utente"
    '                    oResource.setLabel_To_Value(Me.LBerroreLDAP, "ErroreLDAP." & CType(oLdap.Errore, ErroreLDAP))
    '                ElseIf oLdap.Errore = ErroreLDAP.erroreGenerale Then
    ''Me.LBerroreLDAP.Text = "Connessione al server LDAP fallita, impossibile registrare l'utente"
    '                    oResource.setLabel_To_Value(Me.LBerroreLDAP, "ErroreLDAP." & CType(oLdap.Errore, ErroreLDAP))
    '                ElseIf oLdap.Errore = ErroreLDAP.notAutenthicated Then
    ''Me.LBerroreLDAP.Text = "Autenticazione utente fallita, impossibile registrare l'utente"
    '                    oResource.setLabel_To_Value(Me.LBerroreLDAP, "ErroreLDAP." & CType(oLdap.Errore, ErroreLDAP))
    '                ElseIf oLdap.Errore = ErroreLDAP.notFound Then
    ''Me.LBerroreLDAP.Text = "La login specificata non è registrata su LDAP fallita, impossibile registrare l'utente"
    '                    oResource.setLabel_To_Value(Me.LBerroreLDAP, "ErroreLDAP." & CType(oLdap.Errore, ErroreLDAP))
    '                End If
    '                Return Main.ErroreRegistrazione.FillField
    '            Else
    '                Me.TBRautenticazione0.Visible = False
    '            End If
    '        End If
    '    End If
#End Region

#Region "Add Specialized"
    Private Function AddExternalUser(ByVal oExternalUser As CL_Esterno.COL_Esterno, ByVal dto As dtoExternal) As Integer
        oExternalUser.Mansione = dto.ExternalUserInfo
        oExternalUser.Aggiungi()
        Return oExternalUser.ID
    End Function

    Private Function AddCompanyUser(ByVal person As lm.Comol.Core.DomainModel.CompanyUser, provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, externalCredentials As dtoExternalCredentials) As Integer
        Dim result As lm.Comol.Core.DomainModel.CompanyUser = Nothing
        Try
            If provider.Type = AuthenticationProviderType.Internal Then
                result = CurrentPresenter.AddCompanyUser(person)
            Else
                result = CurrentPresenter.AddCompanyUser(person, provider.IdProvider, externalCredentials)
            End If

        Catch ex As Exception

        End Try
        If IsNothing(result) Then
            Return 0
        Else
            Return result.Id
        End If

    End Function
    Private Function AddEmployee(ByVal person As lm.Comol.Core.DomainModel.Employee, provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, externalCredentials As dtoExternalCredentials) As Integer
        Dim result As lm.Comol.Core.DomainModel.Employee = Nothing
        Try
            If provider.Type = AuthenticationProviderType.Internal Then
                result = CurrentPresenter.AddEmployee(person)
            Else
                result = CurrentPresenter.AddEmployee(person, provider.IdProvider, externalCredentials)
            End If

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

End Class