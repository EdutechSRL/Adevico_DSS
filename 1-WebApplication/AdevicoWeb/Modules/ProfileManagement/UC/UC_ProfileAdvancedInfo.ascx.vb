Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class UC_ProfileAdvancedInfo
    Inherits BaseControl
    Implements IViewAdvancedProfileInfo

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
    Private _Presenter As AdvancedProfileInfoPresenter
    Private ReadOnly Property CurrentPresenter() As AdvancedProfileInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AdvancedProfileInfoPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewAdvancedProfileInfo.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewAdvancedProfileInfo.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewAdvancedProfileInfo.IdProfileType
        Get
            Return ViewStateOrDefault("IdProfileType", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
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
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBexternalUser_t)
            .setLabel(Me.LBcompanyName_t)
            .setLabel(Me.LBcompanyAddress_t)
            .setLabel(Me.LBcompanyCity_t)
            .setLabel(Me.LBcompanyRegion_t)
            .setLabel(Me.LBcompanyTaxCode_t)


            .setLabel(LBnote_t)
            .setLabel(LBprovince_t)
            .setLabel(LBnations_t)
            .setLabel(LBbirthDate_t)
            .setLabel(LBbirthPlace_t)
            .setLabel(LBpostCode_t)
            .setLabel(LBcity_t)
            .setLabel(LBofficePhone_t)
            .setLabel(LBhomePhone_t)
            .setLabel(LBmobile_t)
            .setLabel(LBfax_t)
            .setLabel(LBhomePage_t)
            .setLabel(LBaddress_t)
            .setLabel(LBagency_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idProfile As Integer) Implements IViewAdvancedProfileInfo.InitializeControl
        MLVprofileAdvancedInfo.SetActiveView(VIWempty)
        Me.SPNtaxCode.Visible = Me.SystemSettings.Presenter.DefaultTaxCodeRequired

        MLVinfo.SetActiveView(VIWdata)

        Me.SetInternazionalizzazione()
        Me.IsInitialized = True
        Me.CurrentPresenter.InitView(idProfile)
    End Sub
    Private Sub LoadProfileData(idProfile As Integer, idType As Integer) Implements IViewAdvancedProfileInfo.LoadProfileData
        IdProfileType = idType
        LoadUserInfo(idProfile, idType)
    End Sub

    Private Sub LoadProfileData(companyUser As dtoCompany, userData As PersonInfo) Implements IViewAdvancedProfileInfo.LoadProfileData
        IdProfileType = CInt(UserTypeStandard.Company)
        LoadUserInfo(companyUser, userData)
    End Sub
    Public Sub LoadProfileData1(employeeUser As dtoEmployee, userData As PersonInfo) Implements IViewAdvancedProfileInfo.LoadProfileData
        IdProfileType = CInt(UserTypeStandard.Employee)
        LoadUserInfo(employeeUser, userData)
    End Sub
    Private Sub DisplayEmpty() Implements IViewAdvancedProfileInfo.DisplayEmpty
        MLVinfo.SetActiveView(VIWdefault)
    End Sub
#End Region

#Region "Load Old Profiles"
    Private Function GetLanguageName(idLanguage As Integer) As String
        Dim languages As List(Of Lingua) = ManagerLingua.List

        Return (From l In languages Where l.ID = idLanguage Select l.Nome).FirstOrDefault
    End Function
    Private Sub LoadUserInfo(idProfile As Integer, idType As Integer)
        Dim oldProfile As New COL_Persona With {.ID = idProfile}

        oldProfile.EstraiTutto(PageUtility.LinguaID)
        If oldProfile.Errore = Errori_Db.None Then
            LoadPersonData(oldProfile)
            Select Case idType
                Case CInt(UserTypeStandard.ExternalUser)
                    Dim external As New CL_Esterno.COL_Esterno With {.ID = idProfile}

                    external.Estrai(PageUtility.LinguaID)
                    If external.Errore = Errori_Db.None Then
                        Me.LBexternalUser.Text = external.Mansione
                        MLVprofileAdvancedInfo.SetActiveView(VIWexternal)
                    End If

            
            End Select
        End If
        LoadPersonData(oldProfile)

    End Sub

    Private Sub LoadUserInfo(companyUser As dtoCompany, userData As PersonInfo)
        With companyUser
            Me.LBtaxCode.Text = .TaxCode
            Me.LBcompanyAddress.Text = .Info.Address
            Me.LBcompanyName.Text = .Info.Name
            Me.LBcompanyTaxCode.Text = .Info.TaxCode
            Me.LBcompanyCity.Text = .Info.City
            Me.LBcompanyRegion.Text = .Info.Region
            MLVprofileAdvancedInfo.SetActiveView(VIWcompany)
        End With

        Me.LBaddress.Text = userData.Address
        Me.LBbirthDate.Text = userData.BirthDate
        Me.LBbirthPlace.Text = userData.BirthPlace
        Me.LBcity.Text = userData.City
        Me.LBfax.Text = userData.Fax
        Me.LBhomePage.Text = userData.Homepage
        Me.LBhomePhone.Text = userData.HomePhone
        Me.LBmobile.Text = userData.Mobile
        Me.LBnote.Text = userData.Note
        Me.LBofficePhone.Text = userData.OfficePhone
        Me.LBpostCode.Text = userData.PostCode

        Me.LBprovince.Text = (From s In ManagerProvincia.List Where s.ID = userData.IdProvince Select s.Nome).FirstOrDefault
        Me.LBnations.Text = (From s In COL_Stato.List Where s.ID = userData.IdNation Select s.Descrizione).FirstOrDefault
    End Sub
    Private Sub LoadUserInfo(employeeUser As dtoEmployee, userData As PersonInfo)
        With employeeUser
            Me.LBtaxCode.Text = .TaxCode
            If employeeUser.Affiliations.Count = 0 Then
                Me.LBagency.Text = Resource.getValue("NoAffiliations")
            Else
                Me.LBagency.Text = "<ul class=""agencylist"">"
                For Each affiliation As dtoAgencyAffiliation In employeeUser.OrderedAffiliations
                    If affiliation.IsEnabled Then
                        Me.LBagency.Text &= "<li>" & String.Format(Resource.getValue("affiliation.IsEnabled"), affiliation.Agency.Value) & "</li>"
                    ElseIf affiliation.ToDate.HasValue Then
                        Me.LBagency.Text &= "<li>" & String.Format(Resource.getValue("affiliation.EndDate"), affiliation.Agency.Value, affiliation.ToDate.Value.ToString("dd/MM/yyyy)")) & "</li>"
                    Else
                        Me.LBagency.Text &= "<li>" & String.Format(Resource.getValue("affiliation.ClosedNoDate"), affiliation.Agency.Value) & "</li>"
                    End If
                Next
                Me.LBagency.Text &= "</ul>"
            End If
            MLVprofileAdvancedInfo.SetActiveView(VIWemployee)
        End With

        Me.LBaddress.Text = userData.Address
        Me.LBbirthDate.Text = userData.BirthDate
        Me.LBbirthPlace.Text = userData.BirthPlace
        Me.LBcity.Text = userData.City
        Me.LBfax.Text = userData.Fax
        Me.LBhomePage.Text = userData.Homepage
        Me.LBhomePhone.Text = userData.HomePhone
        Me.LBmobile.Text = userData.Mobile
        Me.LBnote.Text = userData.Note
        Me.LBofficePhone.Text = userData.OfficePhone
        Me.LBpostCode.Text = userData.PostCode

        Me.LBprovince.Text = (From s In ManagerProvincia.List Where s.ID = userData.IdProvince Select s.Nome).FirstOrDefault
        Me.LBnations.Text = (From s In COL_Stato.List Where s.ID = userData.IdNation Select s.Descrizione).FirstOrDefault
    End Sub
    Private Sub LoadPersonData(person As COL_Persona)
        With person
            Me.LBtaxCode.Text = .CodFiscale
            Me.LBaddress.Text = .Indirizzo
            Me.LBbirthDate.Text = .DataNascita
            Me.LBbirthPlace.Text = .LuogoNascita
            Me.LBcity.Text = .Citta
            Me.LBfax.Text = .Fax
            Me.LBhomePage.Text = .HomePage
            Me.LBhomePhone.Text = .Telefono2

            Me.LBmobile.Text = .Cellulare
            Me.LBnote.Text = .Note
            Me.LBofficePhone.Text = .Telefono1
            Me.LBpostCode.Text = .Cap

            Me.LBprovince.Text = (From s In ManagerProvincia.List Where s.ID = person.Stato.ID Select s.Nome).FirstOrDefault
            Me.LBnations.Text = (From s In COL_Stato.List Where s.ID = person.Stato.ID Select s.Descrizione).FirstOrDefault
        End With
    End Sub

#End Region

    
End Class