Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class UC_ProfileBaseInfo
    Inherits BaseControl
    Implements IViewBaseProfileInfo
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
    Private _Presenter As BaseProfileInfoPresenter
    Private ReadOnly Property CurrentPresenter() As BaseProfileInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New BaseProfileInfoPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewBaseProfileInfo.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewBaseProfileInfo.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewBaseProfileInfo.IdProfileType
        Get
            Return ViewStateOrDefault("IdProfileType", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
    End Property
    Public Property AlsoProfileTypeInfo As Boolean Implements IViewBaseProfileInfo.AlsoProfileTypeInfo
        Get
            Return ViewStateOrDefault("AlsoProfileTypeInfo", False)
        End Get
        Set(value As Boolean)
            ViewState("AlsoProfileTypeInfo") = value
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
            .setLabel(LBname_t)
            .setLabel(LBlingua_t)
            .setLabel(LBmail_t)
            .setLabel(LBsurname_t)
            .setLabel(LBtaxCode_t)
            .setLabel(Me.LBmatricola_t)
            .setLabel(Me.LBimmatricolazione_t)
            .setLabel(Me.LBistitutoSTU_t)
            .setLabel(Me.LBclasseSezione_t)
            .setLabel(Me.LBistituto_t)
            .setLabel(Me.LBmatters_t)
            .setLabel(Me.LBphd_t)
            .setLabel(Me.LBtechnician_t)
            .setLabel(Me.LBexternalUser_t)
            .setLabel(Me.LBcompanyName_t)
            .setLabel(Me.LBcompanyAddress_t)
            .setLabel(Me.LBcompanyTaxCode_t)
            .setLabel(Me.LBprimaryOrganization_t)
            .setLabel(Me.LBotherOrganizations_t)

            .setLabel(LBassociationCategories_t)
            .setLabel(LBcompanyReaNumber_t)
            .setLabel(LBcompanyCity_t)
            .setLabel(LBcompanyRegion_t)
            .setLabel(LBagency_t)
        End With
    End Sub
#End Region
    '    '
#Region "Implements"
    Public Sub InitializeControl(idProfile As Integer, alsoProfileTypeInfo As Boolean) Implements IViewBaseProfileInfo.InitializeControl
        alsoProfileTypeInfo = alsoProfileTypeInfo
        SPNotherOrganizations.Visible = False
        SPNdefaultOrganization.Visible = False
        Me.SPNtaxCode.Visible = Me.SystemSettings.Presenter.DefaultTaxCodeRequired

        MLVinfo.SetActiveView(VIWdata)
        Me.MLVprofileAdvancedInfo.SetActiveView(VIWempty)

        Me.SetInternazionalizzazione()
        Me.IsInitialized = True
        Me.CurrentPresenter.InitView(idProfile)
    End Sub

    Private Sub LoadOrganizations(organizations As List(Of dtoProfileOrganization)) Implements IViewBaseProfileInfo.LoadOrganizations
        If organizations.Where(Function(o) o.isDefault = True).Any Then
            Me.LBprimaryOrganization.Text = (From o In organizations Where o.isDefault Select o.Name).FirstOrDefault()
            SPNdefaultOrganization.Visible = True
        End If

        If organizations.Where(Function(o) Not o.isDefault).Any Then
            SPNotherOrganizations.Visible = True
            LBotherOrganizations.Text = "<ul>" & vbCrLf
            For Each item As dtoProfileOrganization In organizations
                LBotherOrganizations.Text &= "<li>" & item.Name & "</li>"
            Next
            LBotherOrganizations.Text &= "</ul>"
        End If
    End Sub

    Private Sub LoadProfileData(idProfile As Integer, idType As Integer) Implements IViewBaseProfileInfo.LoadProfileData
        IdProfileType = idType
        LoadUserInfo(idProfile, idType)
    End Sub

    Private Sub LoadProfileData(companyUser As dtoCompany) Implements IViewBaseProfileInfo.LoadProfileData
        IdProfileType = CInt(UserTypeStandard.Company)
        LoadUserInfo(companyUser)
    End Sub
    Public Sub LoadProfileData(employeeUser As dtoEmployee) Implements IViewBaseProfileInfo.LoadProfileData
        IdProfileType = CInt(UserTypeStandard.Employee)
        LoadUserInfo(employeeUser)
    End Sub
    Private Sub DisplayEmpty() Implements IViewBaseProfileInfo.DisplayEmpty
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
            If AlsoProfileTypeInfo Then
                Select Case idType
                    Case CInt(UserTypeStandard.ExternalUser)
                        Dim external As New CL_Esterno.COL_Esterno With {.ID = idProfile}

                        external.Estrai(PageUtility.LinguaID)
                        If external.Errore = Errori_Db.None Then
                            Me.LBexternalUser.Text = external.Mansione
                        End If
                End Select
            End If

        End If
    End Sub

    Private Sub LoadUserInfo(companyUser As dtoCompany)
        With companyUser
            Me.LBname.Text = .Name
            Me.LBsurname.Text = .Surname
            Me.LBmail.Text = .Mail
            Me.LBtaxCode.Text = .TaxCode
            Me.LBlingua.Text = GetLanguageName(.IdLanguage)
            If AlsoProfileTypeInfo Then
                Me.LBcompanyAddress.Text = .Info.Address
                Me.LBcompanyName.Text = .Info.Name
                Me.LBcompanyTaxCode.Text = .Info.TaxCode
                Me.LBcompanyCity.Text = .Info.City
                Me.LBcompanyRegion.Text = .Info.Region
                Me.LBassociationCategories.Text = .Info.AssociationCategories
                Me.LBcompanyReaNumber.Text = .Info.ReaNumber
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWcompany)
            End If

        End With
    End Sub
    Private Sub LoadUserInfo(employeeUser As dtoEmployee)
        With employeeUser
            Me.LBname.Text = .Name
            Me.LBsurname.Text = .Surname
            Me.LBmail.Text = .Mail
            Me.LBtaxCode.Text = .TaxCode
            Me.LBlingua.Text = GetLanguageName(.IdLanguage)
            If AlsoProfileTypeInfo Then
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
                Me.MLVprofileAdvancedInfo.SetActiveView(VIWemployee)
            End If

        End With
    End Sub
    Private Sub LoadPersonData(person As COL_Persona)
        With person
            Me.LBname.Text = .Nome
            Me.LBsurname.Text = .Cognome
            Me.LBmail.Text = .Mail
            Me.LBtaxCode.Text = .CodFiscale
            Me.LBlingua.Text = GetLanguageName(.Lingua.ID)
        End With
    End Sub

#End Region


End Class