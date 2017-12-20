Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class UC_ProfileData
    Inherits BaseControl
    Implements IViewProfileData



    Private Enum EditingType
        UserProfile = 0
        AdministrationEdit = 1
        EditingType = 2
    End Enum
    Public Event EditMailAddress()


    Private Property Editing As EditingType
        Get
            Return ViewStateOrDefault("Editing", EditingType.UserProfile)
        End Get
        Set(value As EditingType)
            ViewState("Editing") = value
        End Set
    End Property
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
    Private _Presenter As ProfileDataPresenter
    Private ReadOnly Property CurrentPresenter() As ProfileDataPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfileDataPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdProfile As Integer Implements IViewProfileData.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewProfileData.IdProfileType
        Get
            Return ViewStateOrDefault("IdProfileType", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
    End Property
    Public Property IdOldProfileType As Integer Implements IViewProfileData.IdOldProfileType
        Get
            Return ViewStateOrDefault("IdOldProfileType", 0)
        End Get
        Set(value As Integer)
            ViewState("IdOldProfileType") = value
        End Set
    End Property
    Public Property idDefaultProvider As Long Implements IViewProfileData.idDefaultProvider
        Get
            Return ViewStateOrDefault("idDefaultProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("idDefaultProvider") = value
        End Set
    End Property
    Public Property IsInitialized As Boolean Implements IViewProfileData.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property ContainerMailEdit As Boolean Implements IViewProfileData.ContainerMailEdit
        Get
            Return ViewStateOrDefault("ContainerMailEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("ContainerMailEdit") = value
        End Set
    End Property
    Public Property AllowAdvancedFieldsEdit As Boolean Implements IViewProfileData.AllowAdvancedFieldsEdit
        Get
            Return ViewStateOrDefault("AllowAdvancedFieldsEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowAdvancedFieldsEdit") = value
        End Set
    End Property
    Public Property PreviousIdProfileType As Integer
        Get
            Return ViewStateOrDefault("PreviousIdProfileType", 0)
        End Get
        Set(value As Integer)
            ViewState("PreviousIdProfileType") = value
        End Set
    End Property
    Public ReadOnly Property CurrentProfile As lm.Comol.Core.Authentication.dtoBaseProfile Implements IViewProfileData.CurrentProfile
        Get
            Dim oInfo As dtoBaseProfile

            Select Case IdProfileType
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
                .Id = IdProfile
                .Mail = Me.TXBmail.Text

                .Name = Me.TXBname.Text
                .Surname = Me.TXBsurname.Text

                .Job = Me.TXBjob.Text
                .Sector = Me.TXBsector.Text

                If Me.SystemSettings.Presenter.DefaultTaxCodeRequired = False AndAlso .TaxCode = "" Then
                    Dim oGenerator As New TaxCodeGenerator
                    .TaxCode = oGenerator.Generate
                    Me.TXBtaxCode.Text = .TaxCode
                Else
                    .TaxCode = Me.TXBtaxCode.Text
                End If

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
                .IdProfileType = IdProfileType
            End With

            Select Case IdProfileType
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
                    If Not Me.CTRLagency.ReadOnly AndAlso Me.CTRLagency.SelectedLongItem.Key > 0 Then
                        oDto.CurrentAgency = Me.CTRLagency.SelectedLongItem
                    Else
                        oDto.CurrentAgency = New KeyValuePair(Of Long, String)(-1, "")
                    End If
                    Return oDto
                Case Else
                    Return oInfo
            End Select
            Return oInfo
        End Get
    End Property

    Public ReadOnly Property ProfilePersonalData As lm.Comol.Core.DomainModel.PersonInfo Implements IViewProfileData.ProfilePersonalData
        Get
            Dim info As New lm.Comol.Core.DomainModel.PersonInfo
            info.Address = Me.TXBaddress.Text
            If Not String.IsNullOrEmpty(Me.TXBbirthDate.Text) AndAlso IsDate(Me.TXBbirthDate.Text) Then
                info.BirthDate = CDate(Me.TXBbirthDate.Text)
            Else
                info.BirthDate = New DateTime(2999, 1, 1)
            End If
            info.BirthPlace = Me.TXBbirthPlace.Text
            info.City = Me.TXBcity.Text
            info.DefaultShowMailAddress = Me.CBXshowMail.Checked
            info.Fax = Me.TXBfax.Text
            info.Homepage = Me.TXBhomepage.Text
            info.HomePhone = Me.TXBhomePhone.Text
            info.IdProvince = Me.DDLprovince.SelectedValue
            info.IdNation = Me.DDLnations.SelectedValue
            info.IsMale = (Me.RBLgender.SelectedIndex = 0)
            info.Mobile = Me.TXBmobile.Text
            info.Note = Me.TXBnote.Text
            info.OfficePhone = Me.TXBofficePhone.Text
            info.PostCode = Me.TXBpostCode.Text

            Return (info)
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
            .setLabel(LBmail_t)
            .setLabel(LBmailInfo)
            .setLabel(LBmostraMail)
            .setLabel(LBnome_t)
            .setLabel(LBtaxCode_t)

            .setLabel(Me.LBexternalUser_t)
            .setLabel(Me.LBcompanyName_t)
            .setLabel(Me.LBcompanyAddress_t)
            .setLabel(Me.LBcompanyCity_t)
            .setLabel(Me.LBcompanyRegion_t)
            .setLabel(Me.LBcompanyTaxCode_t)
            .setLabel(LBassociationCategories_t)
            .setLabel(LBcompanyReaNumber_t)

            .setLabel(LBmailDuplicate)
            .setLabel(LBtaxCodeDuplicate)

            .setLabel(LBnote_t)
            .setLabel(LBprovince_t)
            .setLabel(LBnations_t)
            .setLabel(LBbirthDate_t)
            .setLabel(LBbirthPlace_t)
            .setLabel(LBgender_t)
            .setRadioButtonList(RBLgender, "True")
            .setRadioButtonList(RBLgender, "False")
            .setLabel(LBpostCode_t)
            .setLabel(LBcity_t)
            .setLabel(LBofficePhone_t)
            .setLabel(LBhomePhone_t)
            .setLabel(LBmobile_t)
            .setLabel(LBfax_t)
            .setLabel(LBhomePage_t)
            .setLabel(LBaddress_t)
            .setButton(BTNmailEdit, True, False, False, True)
            .setLabel(LBagency_t)

            .setButton(BTNundoSaveAgency, True, False, False, True)
            .setButton(BTNsaveAgency, True, False, False, True)
            .setButton(BTNeditAgency, True, False, False, True)

            .setLabel(LBsector_t)
            .setLabel(LBjob_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControlForUserEdit(idProfile As Integer, idProfileType As Integer, allowSubscription As Boolean) Implements IViewProfileData.InitializeControlForUserEdit
        Editing = EditingType.UserProfile
        IsInitialized = True
        AllowAdvancedFieldsEdit = allowSubscription
        'Me.InitializeAdvancedControl(idProfileType, allowSubscription)
        InternalInitializeControl(idProfile, idProfileType)
        Me.TXBmail.Enabled = False
        Me.TXBtaxCode.Enabled = False
        Me.SPNname.Visible = False
        Me.SPNsurname.Visible = False
        BTNmailEdit.Visible = Me.ContainerMailEdit
    End Sub
    Public Sub InitializeControlForEditingType(idProfile As Integer, idProfileType As Integer) Implements IViewProfileData.InitializeControlForEditingType
        Editing = EditingType.EditingType
        AllowAdvancedFieldsEdit = True
        PreviousIdProfileType = idProfileType
        IdOldProfileType = idProfileType
        InternalInitializeControl(idProfile, idProfileType)
        Me.MLVinfoData.SetActiveView(VIWnoFields)

    End Sub
    Public Sub InitializeControl(idProfile As Integer, idProfileType As Integer) Implements IViewProfileData.InitializeControl
        Editing = EditingType.AdministrationEdit
        AllowAdvancedFieldsEdit = True
        InternalInitializeControl(idProfile, idProfileType)

    End Sub
    Public Sub InternalInitializeControl(idProfile As Integer, idProfileType As Integer)
        Me.InitializeControl(idProfileType)
        Me.CurrentPresenter.InitView(idProfile)
        Me.SetInternazionalizzazione()
        Me.MLVinfoData.SetActiveView(VIWinfoFields)

    End Sub

    Private Sub InitializeControl(idProfileType As Integer)
        Me.LBmailDuplicate.Visible = False
        Me.LBtaxCodeDuplicate.Visible = False
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
        Catch ex As Exception

        End Try
        LoadNations()
        LoadProvince()
        Me.SPNtaxCode.Visible = Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        Me.InitializeAdvancedControl(idProfileType)
    End Sub
    Private Sub LoadProfileData(companyUser As dtoCompany, userData As PersonInfo) Implements IViewProfileData.LoadProfileData
        IdProfileType = CInt(UserTypeStandard.Company)
        LoadProfileData(companyUser)
        LoadUserData(userData)
    End Sub
    Private Sub LoadProfileData(employee As dtoEmployee, userData As PersonInfo) Implements IViewProfileData.LoadProfileData
        IdProfileType = CInt(UserTypeStandard.Employee)
        LoadProfileData(employee)
        LoadUserData(userData)
    End Sub
    Private Sub LoadUserData(userData As PersonInfo)
        Me.TXBaddress.Text = userData.Address
        Me.TXBbirthDate.Text = userData.BirthDate

        Me.TXBbirthPlace.Text = userData.BirthPlace
        Me.TXBcity.Text = userData.City
        Me.CBXshowMail.Checked = userData.DefaultShowMailAddress
        Me.TXBfax.Text = userData.Fax
        Me.TXBhomepage.Text = userData.Homepage
        Me.TXBhomePhone.Text = userData.HomePhone
        Me.DDLprovince.SelectedValue = userData.IdProvince
        Me.DDLnations.SelectedValue = userData.IdNation
        Me.RBLgender.SelectedIndex = IIf(userData.IsMale, 0, 1)
        Me.TXBmobile.Text = userData.Mobile
        Me.TXBnote.Text = userData.Note
        Me.TXBofficePhone.Text = userData.OfficePhone
        Me.TXBpostCode.Text = userData.PostCode
    End Sub
    Private Sub LoadStandardProfileData(idProfile As Integer, idType As Integer) Implements IViewProfileData.LoadProfileData
        IdProfileType = idType
        LoadProfileData(idProfile, idType)
    End Sub

    Private Sub DisplayProfileUnknown() Implements IViewProfileData.DisplayProfileUnknown

    End Sub
    Public Function ValidateContent() As Boolean Implements IViewProfileData.ValidateContent
        If Page.IsValid Then
            Return CurrentPresenter.ValidateInput(Me.SystemSettings.Presenter.DefaultTaxCodeRequired)
        Else
            Return False
        End If
    End Function
#End Region

#Region "Advanced profiles"
    Private Sub InitializeAdvancedControl(ByVal IdProfileType As Integer)
        Select Case IdProfileType
            Case lm.Comol.Core.DomainModel.UserTypeStandard.ExternalUser
                InitializeExternalUser(AllowAdvancedFieldsEdit)
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Company
                InitializeCompany(AllowAdvancedFieldsEdit)
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Employee
                InitializeEmployee(AllowAdvancedFieldsEdit)
            Case Else
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWempty)
        End Select
    End Sub

#Region "common"
    Private Sub LoadNations()
        Me.DDLnations.DataSource = COL_Stato.List
        Me.DDLnations.DataTextField() = "Descrizione"
        Me.DDLnations.DataValueField = "ID"
        Me.DDLnations.DataBind()
        Try
            Me.DDLnations.SelectedValue = Me.SystemSettings.Presenter.DefaultNationID
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LoadProvince()
        DDLprovince.Enabled = True
        DDLprovince.DataSource = ManagerProvincia.List
        DDLprovince.DataTextField() = "Nome"
        DDLprovince.DataValueField = "ID"
        DDLprovince.DataBind()
        Try
            Me.DDLprovince.SelectedValue = Me.SystemSettings.Presenter.DefaultProvinceID
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DDLnations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLnations.SelectedIndexChanged
        If Me.DDLnations.SelectedItem.Value <> "193" Then 'italia
            DDLprovince.SelectedValue = "0" 'estera provincia
            DDLprovince.Enabled = False
        Else
            DDLprovince.Enabled = True
        End If
    End Sub
#End Region
#Region "ExternalUser"
    Private Sub InitializeExternalUser(ByVal allowEdit As Boolean)
        Me.MLVprofileAdvancedInfo.SetActiveView(Me.VIWexternal)
        Me.TXBexternalUser.Text = ""
        Me.TXBexternalUser.ReadOnly = Not allowEdit
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
    Private Sub InitializeCompany(ByVal allowEdit As Boolean)
        Me.TXBcompanyName.Text = ""
        Me.TXBcompanyAddress.Text = ""
        Me.TXBcompanyCity.Text = ""
        Me.TXBcompanyRegion.Text = ""
        Me.TXBcompanytaxCode.Text = ""
        Me.TXBassociationCategories.Text = ""
        Me.TXBreaNumber.Text = ""

        Me.TXBcompanyName.ReadOnly = Not allowEdit
        Me.TXBcompanytaxCode.ReadOnly = Not allowEdit
        Me.TXBassociationCategories.ReadOnly = Not allowEdit
        Me.TXBreaNumber.ReadOnly = Not allowEdit
        TXBcompanytaxCode.ReadOnly = Not allowEdit
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
    Private Sub InitializeEmployee(ByVal allowEdit As Boolean)
        If IdProfile > 0 Then
            CurrentPresenter.GetPersonAffiliations(IdProfile)
        End If
        If Editing = EditingType.EditingType AndAlso allowEdit Then
            DVagency.Visible = True
            Me.BTNeditAgency.Visible = False
            CTRLagency.ReadOnly = False
            Me.CTRLagency.InitalizeControl(New lm.Comol.Core.DomainModel.Helpers.StringItem(Of String) With {.Id = Me.IdProfile, .Name = "idUser"})
        End If
        Me.MLVprofileAdvancedInfo.SetActiveView(VIWemployee)
    End Sub
#End Region

#End Region


#Region "Load Old Profiles"
    Private Sub LoadProfileData(idProfile As Integer, idType As Integer)
        Dim oldProfile As New COL_Persona With {.ID = idProfile}

        oldProfile.EstraiTutto(PageUtility.LinguaID)
        If oldProfile.Errore = Errori_Db.None Then
            LoadPersonData(oldProfile)
            Select Case idType
                Case CInt(UserTypeStandard.ExternalUser)
                    Dim external As New CL_Esterno.COL_Esterno With {.ID = idProfile}

                    external.Estrai(PageUtility.LinguaID)
                    If external.Errore = Errori_Db.None Then
                        Me.ExternalUserInfo = external.Mansione
                    End If
            End Select
        End If
        Me.TXBname.Text = oldProfile.Nome
        Me.TXBsurname.Text = oldProfile.Cognome
        Me.RBLgender.SelectedIndex = IIf((oldProfile.Sesso = 1), 0, 1)
        Me.TXBmail.Text = oldProfile.Mail
        Me.TXBtaxCode.Text = oldProfile.CodFiscale

        Me.DDLlanguage.SelectedValue = oldProfile.Lingua.ID
    End Sub

    Private Sub LoadProfileData(companyUser As dtoCompany)
        With companyUser
            Me.TXBname.Text = .Name
            Me.TXBsurname.Text = .Surname
            Me.TXBmail.Text = .Mail
            Me.TXBtaxCode.Text = .TaxCode

            Me.TXBjob.Text = .Job
            Me.TXBsector.Text = .Sector

            Me.DDLlanguage.SelectedValue = .IdLanguage
            Me.CompanyAddress = .Info.Address
            Me.CompanyCity = .Info.City
            Me.CompanyRegion = .Info.Region
            Me.CompanyName = .Info.Name
            Me.CompanyTaxCode = .Info.TaxCode
            Me.CompanyReaNumber = .Info.ReaNumber
            Me.CompanyAssociationCategories = .Info.AssociationCategories
        End With
    End Sub
    Private Sub LoadProfileData(employee As dtoEmployee)
        With employee
            Me.TXBname.Text = .Name
            Me.TXBsurname.Text = .Surname
            Me.TXBmail.Text = .Mail
            Me.TXBtaxCode.Text = .TaxCode

            Me.TXBjob.Text = .Job
            Me.TXBsector.Text = .Sector

            Me.DDLlanguage.SelectedValue = .IdLanguage

            Me.LoadAffiliations(employee.OrderedAffiliations)

            If Me.Editing = EditingType.UserProfile OrElse (PreviousIdProfileType = UserTypeStandard.Employee AndAlso Editing = EditingType.EditingType) Then
                DVagency.Visible = False
            ElseIf Editing = EditingType.EditingType Then
                DVagency.Visible = True
                CTRLagency.ReadOnly = False
            Else
                DVagency.Visible = True
                Me.BTNundoSaveAgency.Visible = False
                Me.BTNsaveAgency.Visible = False
                Me.BTNeditAgency.Visible = True
            End If
        End With
    End Sub
    Public Sub LoadAffiliations(items As List(Of dtoAgencyAffiliation)) Implements IViewProfileData.LoadAffiliations
        Me.RPTaffiliations.DataSource = items
        Me.RPTaffiliations.DataBind()
        Me.RPTaffiliations.Visible = (items.Count > 0)
    End Sub
    Private Sub RPTaffiliations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTaffiliations.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim affiliation As dtoAgencyAffiliation = DirectCast(e.Item.DataItem, dtoAgencyAffiliation)

            Dim oLabel As Label = e.Item.FindControl("LBagency")
            If affiliation.IsEnabled Then
                oLabel.Text &= String.Format(Resource.getValue("affiliation.IsEnabled"), affiliation.Agency.Value)
            ElseIf affiliation.ToDate.HasValue Then
                oLabel.Text &= String.Format(Resource.getValue("affiliation.EndDate"), affiliation.Agency.Value, affiliation.ToDate.Value.ToString("dd/MM/yyyy"))
            Else
                oLabel.Text &= String.Format(Resource.getValue("affiliation.ClosedNoDate"), affiliation.Agency.Value)
            End If
        End If
    End Sub
    Private Sub LoadPersonData(person As COL_Persona)
        With person
            Me.TXBname.Text = .Nome
            Me.TXBsurname.Text = .Cognome
            Me.TXBaddress.Text = .Indirizzo
            Me.TXBbirthDate.Text = .DataNascita
            Me.TXBbirthPlace.Text = .LuogoNascita

            Me.TXBcity.Text = .Citta
            Me.TXBfax.Text = .Fax
            Me.TXBhomepage.Text = .HomePage
            Me.TXBhomePhone.Text = .Telefono2
            Me.TXBmail.Text = .Mail
            Me.TXBmobile.Text = .Cellulare
            Me.TXBnote.Text = .Note
            Me.TXBofficePhone.Text = .Telefono1
            Me.TXBpostCode.Text = .Cap
            Me.TXBtaxCode.Text = .CodFiscale

            Me.CBXshowMail.Checked = .MostraMail
            If Not IsNothing(Me.DDLprovince.Items.FindByValue(.Provincia.ID)) Then
                Me.DDLprovince.SelectedValue = .Provincia.ID
            End If
            If Not IsNothing(Me.DDLnations.Items.FindByValue(.Stato.ID)) Then
                Me.DDLnations.SelectedValue = .Stato.ID
            End If
        End With
    End Sub
#End Region

#Region "Save Profile"
    Public Function SaveData() As Boolean Implements IViewProfileData.SaveData
        Return Me.CurrentPresenter.SaveData()
    End Function

    Private Function SaveProfile(profile As dtoBaseProfile, userInfo As PersonInfo) As Boolean Implements IViewProfileData.SaveProfile
        Return SaveProfile(profile, userInfo, False)
    End Function
    Private Function SaveProfile(profile As dtoBaseProfile, userInfo As PersonInfo, forTypeEdit As Boolean) As Boolean
        Dim result As Boolean = False

        If TypeOf profile Is dtoExternal Then
            Dim person As New CL_Esterno.COL_Esterno With {.ID = profile.Id}

            If forTypeEdit Then
                Dim basePerson As New COL_Persona() With {.ID = profile.Id}
                basePerson.EstraiTutto(PageUtility.LinguaID)
                UpdatePersonForEditType(person, basePerson, profile.IdProfileType)
            Else
                person.EstraiTutto(PageUtility.LinguaID)
            End If
            If person.Errore = Errori_Db.None Then

                UpdatePerson(person, profile, userInfo)
                person.Mansione = DirectCast(profile, dtoExternal).ExternalUserInfo
                person.Modifica()
                result = (person.Errore = Errori_Db.None)
            End If
        Else
            Dim person As New COL_Persona With {.ID = profile.Id}

            person.EstraiTutto(PageUtility.LinguaID)

            If person.Errore = Errori_Db.None Then
                If forTypeEdit Then
                    person.TipoPersona.ID = profile.IdProfileType
                End If
                UpdatePerson(person, profile, userInfo)
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
#End Region

#Region "Edit Profile type"

    Public Sub ReloadProfileByType(IdType As Integer, forProfileChange As Boolean)
        Select Case IdType
            Case lm.Comol.Core.DomainModel.UserTypeStandard.ExternalUser
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWexternal)
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Company
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWcompany)
            Case lm.Comol.Core.DomainModel.UserTypeStandard.Employee
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWemployee)
            Case Else
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWempty)
        End Select
        Me.IdProfileType = IdType
        If PreviousIdProfileType > 0 AndAlso PreviousIdProfileType <> IdType Then
            Me.InitializeAdvancedControl(IdType)
        End If
    End Sub
    Public Function EditProfileType(IdType As Integer) As Boolean Implements IViewProfileData.EditProfileType
        Return Me.CurrentPresenter.EditProfileType(IdType)
    End Function

    Public Function EditProfileType(profile As dtoBaseProfile, oldIdType As Integer, newIdType As Integer) As Boolean Implements IViewProfileData.EditProfileType
        If DeletePreviousProfileType(IdProfile, oldIdType, newIdType) Then
            profile.IdProfileType = newIdType
            Return SaveProfile(profile, Nothing, True)
        Else
            Return False
        End If
    End Function
    Public Function DeletePreviousProfileType(idProfile As Integer, oldIdType As Integer, newIdType As Integer) As Boolean Implements IViewProfileData.DeletePreviousProfileType
        Dim toDelete As New List(Of lm.Comol.Core.DomainModel.UserTypeStandard)

        toDelete.Add(UserTypeStandard.HighSchoolStudent)
        toDelete.Add(UserTypeStandard.HighSchoolTeacher)
        toDelete.Add(UserTypeStandard.PHDstudent)
        toDelete.Add(UserTypeStandard.Technician)
        toDelete.Add(UserTypeStandard.ExternalUser)
        toDelete.Add(UserTypeStandard.UniversityTeacher)
        toDelete.Add(UserTypeStandard.Undergraduate)

        If toDelete.Contains(oldIdType) Then
            Dim person As New COL_Persona With {.ID = idProfile}
            person.CancellaTipoPrecedente(oldIdType)
            Return (person.Errore = Errori_Db.None)
        Else
            Return True
        End If
    End Function
#End Region

    Private Sub LoadErrors(errors As List(Of ProfilerError)) Implements IViewProfileData.LoadErrors
        Me.LBmailDuplicate.Visible = errors.Contains(ProfilerError.mailDuplicate)
        Me.LBtaxCodeDuplicate.Visible = errors.Contains(ProfilerError.taxCodeDuplicate) AndAlso Me.SystemSettings.Presenter.DefaultTaxCodeRequired
    End Sub

    Private Sub BTNmailEdit_Click(sender As Object, e As System.EventArgs) Handles BTNmailEdit.Click
        RaiseEvent EditMailAddress()
    End Sub

    Public Sub MailUpdated(mail As String)
        Me.TXBmail.Text = mail
    End Sub

    Private Sub BTNeditAgency_Click(sender As Object, e As System.EventArgs) Handles BTNeditAgency.Click
        Me.CTRLagency.ReadOnly = False
        Me.CTRLagency.InitalizeControl(New lm.Comol.Core.DomainModel.Helpers.StringItem(Of String) With {.Id = Me.IdProfile, .Name = "idUser"})
        Me.BTNundoSaveAgency.Visible = True
        Me.BTNsaveAgency.Visible = True
        Me.BTNeditAgency.Visible = False
    End Sub

    Private Sub BTNsaveAgency_Click(sender As Object, e As System.EventArgs) Handles BTNsaveAgency.Click
        Dim item As KeyValuePair(Of Long, String) = Me.CTRLagency.SelectedLongItem

        If Me.CurrentPresenter.SaveAgencyAffiliation(item.Key, IdProfile) Then
            Me.CTRLagency.ReadOnly = True
            Me.CTRLagency.HideContent()
            Me.BTNundoSaveAgency.Visible = False
            Me.BTNsaveAgency.Visible = False
            Me.BTNeditAgency.Visible = True

        End If
      
    End Sub

    Private Sub BTNundoSaveAgency_Click(sender As Object, e As System.EventArgs) Handles BTNundoSaveAgency.Click
        Me.CTRLagency.ReadOnly = True
        Me.CTRLagency.HideContent()
        Me.BTNundoSaveAgency.Visible = False
        Me.BTNsaveAgency.Visible = False
        Me.BTNeditAgency.Visible = True
    End Sub
End Class