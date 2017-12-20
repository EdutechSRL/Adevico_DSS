Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
Public Class TokenValidation
    Inherits PageBase
    Implements IViewTokenValidation


#Region "Context"
    Private _Presenter As TokenValidationPresenter

    Public ReadOnly Property CurrentPresenter() As TokenValidationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TokenValidationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property AllowAdminAccess As Boolean Implements IViewTokenValidation.AllowAdminAccess
        Get
            Return (Request.QueryString("AdminAccess") = "true")
        End Get
    End Property
    Private ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewTokenValidation.isSystemOutOfOrder
        Get
            Return Not AccessoSistema
        End Get
    End Property
    Private ReadOnly Property HasUrlValues As Boolean Implements IViewTokenValidation.HasUrlValues
        Get
            Return (From k As String In Request.QueryString.AllKeys Where k <> "AdminAccess" AndAlso k <> "Debug" Select k).Any
        End Get
    End Property
    Private ReadOnly Property PreloadForDebug As Boolean Implements IViewTokenValidation.PreloadForDebug
        Get
            Return (Request.QueryString("Debug") = "true")
        End Get
    End Property
    Private ReadOnly Property PreloadFromUrl As String Implements IViewTokenValidation.PreloadFromUrl
        Get
            If Not IsNothing(Request.UrlReferrer) Then
                Return Request.UrlReferrer.AbsoluteUri
            Else
                Return ""
            End If
        End Get
    End Property
    Private ReadOnly Property IsFromLogoutPage As Boolean Implements IViewTokenValidation.IsFromLogoutPage
        Get
            Dim result As Boolean = False
            If Page.IsPostBack = False AndAlso Not IsNothing(Request.UrlReferrer) Then
                result = Request.UrlReferrer.AbsoluteUri.Contains("Modules/Common/logout.aspx")
                ViewState("IsFromLogoutPage") = result
            Else
                result = ViewStateOrDefault("IsFromLogoutPage", False)
            End If
            Return result
        End Get
    End Property
    Private ReadOnly Property SubscriptionActive As Boolean Implements IViewTokenValidation.SubscriptionActive
        Get
            Return SystemSettings.Login.SubscriptionActive
        End Get
    End Property
    'Public WriteOnly Property AllowExternalWebAuthentication As Boolean Implements IViewTokenValidation.AllowExternalWebAuthentication
    '    Set(value As Boolean)
    '        Me.LTexternalWebLogon.Visible = value
    '    End Set
    'End Property

    Private Property FromLogonUrl As String Implements IViewTokenValidation.FromLogonUrl
        Get
            Return ViewStateOrDefault("FromLogonUrl", "")
        End Get
        Set(value As String)
            ViewState("FromLogonUrl") = value
        End Set
    End Property
    Private Property AllowSubscription As Boolean Implements IViewTokenValidation.AllowSubscription
        Get
            Return ViewStateOrDefault("AllowSubscription", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSubscription") = value
        End Set
    End Property
    'Public Property LoggedUserId As Integer Implements IViewLogin.LoggedUserId
    '    Get
    '        Return ViewStateOrDefault("LoggedUserId", 0)
    '    End Get
    '    Set(value As Integer)
    '        ViewState("LoggedUserId") = value
    '    End Set
    'End Property


    'Public Function GetUrlToken(identifiers As List(Of String)) As dtoUrlToken Implements IViewTokenValidation.GetUrlToken
    '    For Each identifier As String In identifiers
    '        If ((From s As String In Request.QueryString.AllKeys Select s.ToLower).ToList.Contains(identifier.ToLower)) Then
    '            Dim dto As New dtoUrlToken
    '            dto.Identifier = identifier
    '            dto.Value = Request.QueryString(identifier)

    '            Return dto
    '        End If
    '    Next
    '    Return Nothing
    'End Function

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

#Region "Internal"
    Private Property ValidationStartOn As DateTime
        Get
            Return ViewStateOrDefault("ValidationStartOn", DateTime.Now)
        End Get
        Set(value As DateTime)
            ViewState("ValidationStartOn") = value
        End Set
    End Property
    Private Property TokenInEvaluation As Boolean
        Get
            Return ViewStateOrDefault("TokenInEvaluation", False)
        End Get
        Set(value As Boolean)
            ViewState("TokenInEvaluation") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowLanguage = False
        If Page.IsPostBack = False Then


            ValidationStartOn = DateTime.Now
            Me.TMvalidationTimer.Enabled = True
            Me.TMvalidationTimer.Interval = 1000
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ISAuthenticationPage", "Authentication")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTtitleTokenValidation)
            .setLabel(LBvalidationInfo_t)
            .setLabel(LBvalidationInfo)
            .setLiteral(LTinvalidToken)
            .setLiteral(LTtokenSupportInfo)
            .setLabel(LBstartValidationOn_t)
            .setLabel(LBcalculatedMac_t)
            .setLabel(LBtokenResult_t)
            .setLabel(LBendValidationOn_t)
            .setLiteral(LTexternalWebLogon)
            .setLiteral(LTtokenSupportInfo)
        End With
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "implements"
    Private Sub DisplaySystemOutOfOrder() Implements IViewTokenValidation.DisplaySystemOutOfOrder
        PageUtility.RedirectToUrl(RootObject.SystemOutOforder)
    End Sub
    Private Sub DisplayDebugInfo(token As dtoMacUrlToken) Implements IViewTokenValidation.DisplayDebugInfo
        Me.MLVtokenValidation.SetActiveView(VIWdebug)
        LBstartValidationOn.Text = ValidationStartOn

        Me.RPTattributes.DataSource = token.Attributes
        Me.RPTattributes.DataBind()

        Me.LBcalculatedMac.Text = token.Evaluation.Mac
        If token.Evaluation.Result = UrlProviderResult.ValidToken Then
            Me.LBtokenResult.Text = Resource.getValue("ValidToken")
        Else
            Me.LBtokenResult.Text = Resource.getValue("UrlProviderResult." & token.Evaluation.Result.ToString)
        End If
        LBendValidationOn.Text = DateTime.Now
    End Sub
    Private Sub DisplayInvalidMessage(message As UrlProviderResult) Implements IViewTokenValidation.DisplayInvalidMessage
        Me.MLVtokenValidation.SetActiveView(VIWinvalidToken)
        LTinvalidToken.Text = Resource.getValue("UrlProviderResult." & message.ToString())
    End Sub
    Private Sub DisplayInvalidMessage(username As String, message As UrlProviderResult) Implements IViewTokenValidation.DisplayInvalidMessage
        Me.MLVtokenValidation.SetActiveView(VIWinvalidToken)
        LTinvalidToken.Text = String.Format(Resource.getValue("username.UrlProviderResult." & message.ToString()), username)
    End Sub
    Private Sub DisplayTaxCodeAlreadyPresent() Implements IViewTokenValidation.DisplayTaxCodeAlreadyPresent
        LTtaxCodeDuplicate.Text = Resource.getValue("DisplayTaxCodeAlreadyPresent")
    End Sub
    Private Sub DisplayAutoEnrollmentFailed() Implements IViewTokenValidation.DisplayAutoEnrollmentFailed
        Me.MLVtokenValidation.SetActiveView(VIWerror)
        LTmessage.Text = Resource.getValue("DisplayAutoEnrollmentFailed")
    End Sub
    Private Sub DisplayUrlAuthenticationUnavailable() Implements IViewTokenValidation.DisplayUrlAuthenticationUnavailable
        Me.MLVtokenValidation.SetActiveView(VIWerror)
        LTmessage.Text = Resource.getValue("DisplayUrlAuthenticationUnavailable")
        LTmessage.Text = String.Format(LTmessage.Text, FromLogonUrl)
    End Sub
    Private Sub DisplayProviderNotFound() Implements IViewTokenValidation.DisplayProviderNotFound
        Me.MLVtokenValidation.SetActiveView(VIWerror)
        LTmessage.Text = Resource.getValue("DisplayProviderNotFound")
        LTmessage.Text = String.Format(LTmessage.Text, FromLogonUrl)
    End Sub
    Private Sub DisplayNoToken() Implements IViewTokenValidation.DisplayNoToken
        Me.MLVtokenValidation.SetActiveView(VIWerror)
        LTmessage.Text = Resource.getValue("DisplayNoToken")
    End Sub
    Private Sub DisplayAccountDisabled(url As String) Implements IViewTokenValidation.DisplayAccountDisabled
        Me.PageUtility.RedirectToUrl(url)
    End Sub

    Private Sub DisplayPrivacyPolicy(userId As Integer, idProvider As Long, providerUrl As String, internalPage As Boolean) Implements IViewTokenValidation.DisplayPrivacyPolicy
        Me.PageUtility.PreloggedUserId = userId
        Me.PageUtility.PreloggedProviderId = idProvider
        Me.PageUtility.PreloggedProviderUrl = IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.PolicyManagement.RootObject.AcceptLogonPolicy)
    End Sub

    Private Sub SetAutoLogonUrl(url As String) Implements IViewTokenValidation.SetAutoLogonUrl
        Me.LTexternalWebLogon.Visible = Not String.IsNullOrEmpty(url)
        If (LTexternalWebLogon.Text.Contains("{0}")) Then
            LTexternalWebLogon.Text = String.Format(LTexternalWebLogon.Text, url)
        End If
        If Not String.IsNullOrEmpty(url) Then
            '  Me.LTredirect.Text = String.Format("<script language=""javascript"" type=""text/javascript"">window.setTimeout('window.location=""{0}""; ', 5000);</script>", url)

            Me.Resource.setLiteral(Me.LTtokenUrl)
            If LTtokenUrl.Text.Contains("{0}") Then
                Me.LTtokenUrl.Text = String.Format(LTtokenUrl.Text, url)
            End If
        End If
    End Sub
    Private Function GetTokenAttributes(attributes As List(Of dtoMacUrlUserAttribute)) As List(Of dtoMacUrlUserAttribute) Implements IViewTokenValidation.GetTokenAttributes
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
    Private Function GetUrlAttributeValue(queryName As String) As String Implements IViewTokenValidation.GetUrlAttributeValue
        queryName = (From s As String In Request.QueryString.AllKeys Where Not String.IsNullOrEmpty(s) Select s.ToLower).ToList.Where(Function(t) t = queryName).Select(Function(s) s).FirstOrDefault()
        If Not String.IsNullOrEmpty(queryName) Then
            Return Request.QueryString(queryName)
        End If
        Return ""
    End Function

    Private Function GetUrlIdentifier(availableItems As List(Of dtoMacUrlProviderIdentifier)) As dtoMacUrlProviderIdentifier Implements IViewTokenValidation.GetUrlIdentifier
        Dim dto As dtoMacUrlProviderIdentifier = Nothing
        For Each item As dtoMacUrlProviderIdentifier In availableItems
            If Request.QueryString(item.Application.QueryStringName) = item.Application.Value AndAlso Request.QueryString(item.Function.QueryStringName) = item.Function.Value Then
                dto = item
                Exit For
            End If
        Next
        Return dto
    End Function
    Private Sub LoadLanguage(language As lm.Comol.Core.DomainModel.Language) Implements IViewTokenValidation.LoadLanguage
        Dim oLingua As New Lingua(language.Id, language.Code) With {.Icona = language.Icon, .isDefault = language.isDefault}

        Me.OverloadLanguage(oLingua)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
    Private Sub GoToProfile(vToken As dtoMacUrlToken, wizardUrl As String) Implements IViewTokenValidation.GoToProfile
        'Me.PageUtility.RedirectToUrl(wizardUrl, SystemSettings.Login.isSSLloginRequired)

        Dim oRemotePost As New lm.Comol.Modules.Base.DomainModel.RemotePost
        oRemotePost.Url = Me.BaseUrl & wizardUrl

        oRemotePost.Add("Identifier", vToken.UniqueIdentifyer)
        oRemotePost.Add("InternalMac", vToken.InternalMac)

        oRemotePost.Post()
    End Sub

    Private Sub LogonUser(user As lm.Comol.Core.DomainModel.Person, idProvider As Long, providerUrl As String, internalPage As Boolean, idUserDefaultIdOrganization As Int32) Implements IViewTokenValidation.LogonUser
        PageUtility.LogonUser(user, idProvider, IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl, idUserDefaultIdOrganization)
    End Sub
    Private Function CreateUserProfile(profile As dtoBaseProfile, idProfileType As Integer, idOrganization As Integer, provider As MacUrlAuthenticationProvider, credentials As dtoExternalCredentials) As Integer Implements IViewTokenValidation.CreateUserProfile
        Dim idProfile As Integer = 0
        If profile.Password = "" Then
            profile.Password = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, True, True, False)
        End If
        Dim oldProfile As COL_Persona = CreateUser(profile, provider.ProviderType)

        Try
            Dim idPerson As Integer = 0
            If TypeOf profile Is dtoExternal Then
                idPerson = AddExternalUser(CL_Esterno.COL_Esterno.CreateFromPerson(oldProfile), DirectCast(profile, dtoExternal))
            ElseIf TypeOf profile Is dtoCompany Then
                idPerson = AddCompanyUser(CreateUserInfo(profile, provider.ProviderType), provider, credentials)
            ElseIf TypeOf profile Is dtoEmployee Then
                idPerson = AddEmployee(CreateUserInfo(profile, provider.ProviderType), provider, credentials)
            Else
                oldProfile.Aggiungi()
                idPerson = oldProfile.ID
            End If
            If idPerson > 0 Then
                oldProfile.ID = idPerson
                profile.Id = idPerson
                If profile.IdProfileType <> lm.Comol.Core.DomainModel.UserTypeStandard.Company AndAlso profile.IdProfileType <> lm.Comol.Core.DomainModel.UserTypeStandard.Employee Then
                    CurrentPresenter.AddExternalProfile(profile.Id, provider.Id, credentials)
                End If

                Try
                    Me.AddUserToOrganization(oldProfile, idOrganization, True)
                Catch ex As Exception

                End Try
                idProfile = idPerson
            End If
        Catch ex As Exception

        End Try

        Return idProfile
    End Function

    Private Function DeletePreviousProfileType(idProfile As Integer, idOldType As Integer, idNewType As Integer) As Boolean Implements IViewTokenValidation.DeletePreviousProfileType
        Dim toDelete As New List(Of lm.Comol.Core.DomainModel.UserTypeStandard)

        toDelete.Add(UserTypeStandard.HighSchoolStudent)
        toDelete.Add(UserTypeStandard.HighSchoolTeacher)
        toDelete.Add(UserTypeStandard.PHDstudent)
        toDelete.Add(UserTypeStandard.Technician)
        toDelete.Add(UserTypeStandard.ExternalUser)
        toDelete.Add(UserTypeStandard.UniversityTeacher)
        toDelete.Add(UserTypeStandard.Undergraduate)

        If toDelete.Contains(idOldType) Then
            Dim person As New COL_Persona With {.ID = idProfile}
            person.CancellaTipoPrecedente(idOldType)
            Return (person.Errore = Errori_Db.None)
        Else
            Return True
        End If
    End Function

    Private Function EditProfileType(idProfile As Integer, profile As dtoBaseProfile, idOldType As Integer, idNewType As Integer) As Boolean Implements IViewTokenValidation.EditProfileType
        If DeletePreviousProfileType(idProfile, idOldType, idNewType) Then
            profile.IdProfileType = idNewType
            Return SaveProfile(profile)
        Else
            Return False
        End If
    End Function

#Region "Profile management"
    Private Function SaveProfile(profile As dtoBaseProfile) As Boolean
        Dim result As Boolean = False

        If TypeOf profile Is dtoExternal Then
            Dim person As New CL_Esterno.COL_Esterno With {.ID = profile.Id}

            Dim basePerson As New COL_Persona() With {.ID = profile.Id}
            basePerson.EstraiTutto(PageUtility.LinguaID)
            UpdatePersonForEditType(person, basePerson, profile.IdProfileType)

            If person.Errore = Errori_Db.None Then

                UpdatePerson(person, profile, Nothing)
                person.Mansione = DirectCast(profile, dtoExternal).ExternalUserInfo
                person.Modifica()
                result = (person.Errore = Errori_Db.None)
            End If
        Else
            Dim person As New COL_Persona With {.ID = profile.Id}

            person.EstraiTutto(PageUtility.LinguaID)

            If person.Errore = Errori_Db.None Then
                person.TipoPersona.ID = profile.IdProfileType

                UpdatePerson(person, profile, Nothing)
                person.ModificaPersonaNoPwd()
                result = (person.Errore = Errori_Db.None)
            End If
        End If

        Return result
    End Function
    Private Sub UpdatePersonForEditType(destination As COL_Persona, source As COL_Persona, idType As Integer)
        With destination
            .AUTN_ID = source.AUTN_ID
            .AUTN_RemoteUniqueID = source.AUTN_RemoteUniqueID
            .Bloccata = source.Bloccata
            .Cap = source.Cap
            .Cellulare = source.Cellulare
            .Citta = source.Citta
            .CodFiscale = source.CodFiscale
            .Cognome = source.Cognome
            .DataInserimento = source.DataInserimento
            .DataNascita = source.DataNascita
            .Fax = source.Fax
            .FotoPath = source.FotoPath
            .HomePage = source.HomePage
            .ID = source.ID
            .Indirizzo = source.Indirizzo
            .InfoRicevimento = source.InfoRicevimento
            .IsInterno = source.IsInterno
            .Istituzione = source.Istituzione
            .Lingua = source.Lingua
            .Login = source.Login
            .LuogoNascita = source.LuogoNascita
            .Mail = source.Mail
            .MailSecondaria = source.MailSecondaria
            .maxAree = source.maxAree
            .MostraMail = source.MostraMail
            .Nome = source.Nome
            .Note = IIf(String.IsNullOrEmpty(source.Note), "", source.Note)
            .ORGNDefault_id = source.ORGNDefault_id
            .ORGNDefault_ragioneSociale = source.ORGNDefault_ragioneSociale
            .Provincia = source.Provincia
            .Pwd = source.Pwd
            .RicezioneSMS = source.RicezioneSMS
            .Sesso = source.Sesso
            .Stato = source.Stato
            .Telefono1 = source.Telefono1
            .Telefono2 = source.Telefono2
            .TipoPersona.ID = idType
            .UltimoCollegamento = source.UltimoCollegamento
        End With
    End Sub
    Private Sub UpdatePerson(person As COL_Persona, profile As dtoBaseProfile, userInfo As PersonInfo)
        If Not IsNothing(userInfo) Then
            With userInfo
                person.Indirizzo = .Address
                person.DataNascita = .BirthDate
                person.LuogoNascita = .BirthPlace
                person.Citta = .City
                person.MostraMail = .DefaultShowMailAddress
                person.Fax = .Fax
                person.HomePage = .Homepage
                person.Telefono2 = .HomePhone
                person.Stato.ID = .IdNation
                person.Provincia.ID = .IdProvince
                person.Sesso = IIf(.IsMale, 1, 0)
                person.Cellulare = .Mobile
                person.Note = .Note
                person.Telefono1 = .OfficePhone
                person.Cap = .PostCode
            End With
        End If

        With person
            .CodFiscale = profile.TaxCode
            .Nome = profile.Name
            .Cognome = profile.Surname
            .Mail = profile.Mail
            .Lingua.ID = profile.IdLanguage
        End With
    End Sub

    Private Function GetOldTypeProfileData(idProfile As Integer, idOldType As Integer) As dtoBaseProfile Implements IViewTokenValidation.GetOldTypeProfileData
        Dim oInfo As dtoBaseProfile
        Dim oldProfile As New COL_Persona With {.ID = idProfile}

        oldProfile.EstraiTutto(PageUtility.LinguaID)
        If oldProfile.Errore = Errori_Db.None Then
            Select Case idOldType
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
                .Id = idProfile
                .Mail = oldProfile.Mail
                .Name = oldProfile.Nome
                .Surname = oldProfile.Cognome
                .TaxCode = oldProfile.CodFiscale

                .ShowMail = oldProfile.MostraMail
                .IdLanguage = oldProfile.Lingua.ID
                .LanguageName = oldProfile.Lingua.Nome

                If .Surname.Length > 0 Then
                    .FirstLetter = .Surname(0).ToString.ToLower
                End If
                .IdProfileType = idOldType
            End With

            Select Case idOldType
                Case lm.Comol.Core.DomainModel.UserTypeStandard.ExternalUser
                    Dim external As New CL_Esterno.COL_Esterno With {.ID = idProfile}
                    Dim oDto As dtoExternal = oInfo

                    external.Estrai(PageUtility.LinguaID)
                    If external.Errore = Errori_Db.None Then
                        oDto.ExternalUserInfo = external.Mansione
                    End If
                    Return oDto
                Case Else
                    Return oInfo

            End Select
        End If
        Return oInfo
    End Function

#End Region

    Private Sub UpdateLogonXml(idUser As Integer) Implements IViewTokenValidation.UpdateLogonXml
        Dim userProfilePath As String = BaseUrlDrivePath & "profili\" & idUser.ToString & "\"
        Dim oTreeComunita As New COL_BusinessLogic_v2.Comunita.COL_TreeComunita
        Try
            oTreeComunita.Directory = userProfilePath
            oTreeComunita.Nome = idUser.ToString & ".xml"
            oTreeComunita.AggiornaInfo(idUser.ToString, LinguaID, SystemSettings.CodiceDB, True)
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Add Profile"
    Private Function CreateUser(profile As dtoBaseProfile, authentication As AuthenticationProviderType) As COL_Persona
        Dim person As New COL_Persona

        If (Not String.IsNullOrEmpty(profile.Mail)) Then
            profile.Mail = profile.Mail.ToLower()
        End If

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
            .Note = lm.Comol.Core.BaseModules.ProfileManagement.EditingType.FromWebService.ToString
            .Bloccata = False
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

        If (Not String.IsNullOrEmpty(profile.Mail)) Then
            profile.Mail = profile.Mail.ToLower()
        End If

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
            .FirstLetter = profile.FirstLetter
            .isDisabled = False
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
            .Note = lm.Comol.Core.BaseModules.ProfileManagement.EditingType.FromWebService.ToString
        End With
        Return personInfo
    End Function

#Region "AddUser"
    'Private Sub AddAuthenticationInfo(profile As dtoBaseProfile, authentication As AuthenticationProviderType)
    '    Select Case authentication
    '        Case AuthenticationProviderType.Internal
    '            CurrentPresenter.AddInternalProfile(profile, profile.Id)
    '        Case AuthenticationProviderType.Shibboleth
    '            If String.IsNullOrEmpty(Me.ExternalCredentials.IdentifierString) = False Then
    '                CurrentPresenter.AddShibbolethProfile(profile.Id, IdProvider, Me.ExternalCredentials.IdentifierString)
    '            End If
    '        Case AuthenticationProviderType.Url

    '    End Select
    'End Sub
    'Private Sub AddAuthenticationInfo(profile As dtoBaseProfile, provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider, externalCredentials As dtoExternalCredentials)
    '    If provider.Type = AuthenticationProviderType.Internal Then
    '        If String.IsNullOrEmpty(profile.Password) OrElse String.IsNullOrEmpty(profile.Password.Trim) Then
    '            profile.Password = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, True, True, False)
    '        End If
    '        CurrentPresenter.AddInternalProfile(profile, profile.Id)
    '    Else
    '       
    '    End If
    'End Sub

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
                oTreeComunita.Directory = PageUtility.ProfilePath & oPerson.ID & "\"
                oTreeComunita.Nome = oPerson.ID & ".xml"
                oTreeComunita.Insert(oComunita, "." & DefaultCommunityId & ".", oComunita.GetNomeResponsabile_NomeCreatore, oRuoloComunita)
            Catch ex As Exception

            End Try
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
    Private Function AddCompanyUser(ByVal person As lm.Comol.Core.DomainModel.CompanyUser, provider As MacUrlAuthenticationProvider, externalCredentials As dtoExternalCredentials) As Integer
        Dim result As lm.Comol.Core.DomainModel.CompanyUser = Nothing
        Try
            If provider.ProviderType = AuthenticationProviderType.Internal Then
                result = CurrentPresenter.AddCompanyUser(person)
            Else
                result = CurrentPresenter.AddCompanyUser(person, provider.Id, externalCredentials)
            End If

        Catch ex As Exception

        End Try
        If IsNothing(result) Then
            Return 0
        Else
            Return result.Id
        End If

    End Function
    Private Function AddEmployee(ByVal person As lm.Comol.Core.DomainModel.Employee, provider As MacUrlAuthenticationProvider, externalCredentials As dtoExternalCredentials) As Integer
        Dim result As lm.Comol.Core.DomainModel.Employee = Nothing
        Try
            If provider.ProviderType = AuthenticationProviderType.Internal Then
                result = CurrentPresenter.AddEmployee(person)
            Else
                result = CurrentPresenter.AddEmployee(person, provider.Id, externalCredentials)
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

    Private Sub TMvalidationTimer_Tick(sender As Object, e As System.EventArgs) Handles TMvalidationTimer.Tick
        Me.TMvalidationTimer.Enabled = False
        If Not TokenInEvaluation Then
            TokenInEvaluation = True

            CurrentPresenter.InitView()
        End If

    End Sub

    Private Sub TokenValidation_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If IsNothing(Request.UserLanguages) Then
                CurrentPresenter.InitializeLanguageView("")
            Else
                CurrentPresenter.InitializeLanguageView(Request.UserLanguages(0))
            End If
        End If
    End Sub

    Public Sub SendMail(profile As lm.Comol.Core.Authentication.dtoBaseProfile, inserted As Boolean) Implements lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation.IViewTokenValidation.SendMail

    End Sub
End Class