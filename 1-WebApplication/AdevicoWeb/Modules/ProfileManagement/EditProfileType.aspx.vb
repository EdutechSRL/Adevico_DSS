Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class EditProfileType
    Inherits PageBase
    Implements IViewEditProfileType

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
    Private _Presenter As EditProfileTypePresenter
    Private ReadOnly Property CurrentPresenter() As EditProfileTypePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditProfileTypePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property PreloadedIdProfile As Integer Implements IViewEditProfileType.PreloadedIdProfile
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedIdProfileType As Integer Implements IViewEditProfileType.PreloadedIdProfileType
        Get
            If IsNumeric(Request.QueryString("IdProfileType")) Then
                Return CInt(Request.QueryString("IdProfileType"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public Property AllowEdit As Boolean Implements IViewEditProfileType.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            LNBsaveTypeTop.Visible = value
            LNBsaveTypeBottom.Visible = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewEditProfileType.IdProfile
        Get
            ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewEditProfileType.IdProfileType
        Get
            ViewStateOrDefault("IdProfileType", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewEditProfileType.AllowManagement
        Set(value As Boolean)
            HYPbackToManagement.Visible = value
            HYPbackToManagement.NavigateUrl = BaseUrl & RootObject.ManagementProfilesWithFilters
        End Set
    End Property
    Public Property SelectedIdProfileType As Integer Implements IViewEditProfileType.SelectedIdProfileType
        Get
            If Me.DDLnewUserType.Items.Count = 0 Then
                Return UserTypeStandard.Guest
            Else
                Return Me.DDLnewUserType.SelectedValue
            End If
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLnewUserType.Items.FindByValue(value)) Then
                Me.DDLnewUserType.SelectedValue = value
            End If
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

    End Sub

#Region "Implements"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(PreloadedIdProfile)

    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function


    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesManagement", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleEditProfileType")
            Me.Master.ServiceNopermission = .getValue("serviceEditProfileTypeNopermission")
            .setHyperLink(HYPbackToManagement, True, True)

            .setLinkButton(LNBsaveTypeTop, True, True)
            .setLinkButton(LNBsaveTypeBottom, True, True)
            .setLabel(LBnewUserType_t)
            .setLabel(LBcurrentUserType_t)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub LoadProfile(idProfile As Integer, idProfileType As Integer) Implements IViewEditProfileType.LoadProfile
        Me.MLVprofiles.SetActiveView(VIWedit)
        Me.CTRLprofileData.InitializeControlForEditingType(idProfile, idProfileType)
        Me.LNBsaveTypeBottom.Enabled = False
        Me.LNBsaveTypeTop.Enabled = False
    End Sub
    Public Sub LoadProfileName(displayName As String) Implements IViewEditProfileType.LoadProfileName
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleEditNamedProfileType"), displayName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitleEditNamedProfileType"), displayName)
    End Sub
    Public Sub DisplayProfileUnknown() Implements IViewEditProfileType.DisplayProfileUnknown
        Me.MLVprofiles.SetActiveView(VIWdefault)
        Me.LBmessage.Text = Resource.getValue("EditTypeDisplayProfileUnknown")
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewEditProfileType.DisplaySessionTimeout
        'Me.MLVprofiles.SetActiveView(VIWdefault)
        'Me.LBmessage.Text = Resource.getValue("DisplaySessionTimeout")
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditProfileType(IIf(IdProfile > 0, IdProfile, PreloadedIdProfile), IIf(IdProfileType > 0, IdProfileType, PreloadedIdProfileType))
        webPost.Redirect(dto)
    End Sub

    Public Sub GotoManagement() Implements IViewEditProfileType.GotoManagement
        Try
            Dim profile As dtoBaseProfile = Me.CTRLprofileData.CurrentProfile
            If IdProfileType <> profile.IdProfileType Then
                Me.Response.Cookies("AdminProfilesManagement")("Value") = profile.Surname
                Me.Response.Cookies("AdminProfilesManagement")("idProvider") = Me.Request.Cookies("AdminProfilesManagement")("idProvider")
                Me.Response.Cookies("AdminProfilesManagement")("IdOrganization") = Me.Request.Cookies("AdminProfilesManagement")("IdOrganization")
                Me.Response.Cookies("AdminProfilesManagement")("IdProfileType") = profile.IdProfileType
                Me.Response.Cookies("AdminProfilesManagement")("SearchBy") = SearchProfilesBy.Surname.ToString
                Me.Response.Cookies("AdminProfilesManagement")("StartWith") = profile.FirstLetter

                Me.Response.Cookies("AdminProfilesManagement")("Status") = Me.Request.Cookies("AdminProfilesManagement")("Status")
                Me.Response.Cookies("AdminProfilesManagement")("PageIndex") = 0
                Me.Response.Cookies("AdminProfilesManagement")("OrderBy") = OrderProfilesBy.SurnameAndName.ToString
                Me.Response.Cookies("AdminProfilesManagement")("Ascending") = True
                Me.Response.Cookies("AdminProfilesManagement")("PageSize") = Me.Request.Cookies("AdminProfilesManagement")("PageSize")
            End If
           
        Catch ex As Exception

        End Try
      
        PageUtility.RedirectToUrl(RootObject.ManagementProfilesWithFilters())
    End Sub
    Public Function ValidateContent() As Boolean Implements IViewEditProfileType.ValidateContent
        Return Me.CTRLprofileData.ValidateContent
    End Function
    Public Sub DisplayNoPermission() Implements IViewEditProfileType.DisplayNoPermission
        Master.ShowNoPermission = True
    End Sub
    Public Sub DisplayErrorSaving() Implements IViewEditProfileType.DisplayErrorSaving
        LBerrors.Text = Resource.getValue("DisplayErrorSaving")
        LBerrors.Visible = True
    End Sub
#End Region

#Region "Types"
    Public Sub LoadProfileTypes(idCurrent As Integer, exceptItems As List(Of Integer)) Implements IViewEditProfileType.LoadProfileTypes
        Dim types As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList

        Dim cProfile As COL_TipoPersona = (From t As COL_TipoPersona In types Where t.ID = idCurrent Select t).FirstOrDefault
        If IsNothing(cProfile) Then
            Me.LBcurrentUserType.Text = ""
        Else
            Me.LBcurrentUserType.Text = cProfile.Descrizione
        End If
        Me.DDLnewUserType.DataSource = (From t As COL_TipoPersona In types Where Not exceptItems.Contains(t.ID) Select t).ToList
        Me.DDLnewUserType.DataValueField = "ID"
        Me.DDLnewUserType.DataTextField = "Descrizione"
        Me.DDLnewUserType.DataBind()

        Me.SelectedIdProfileType = idCurrent
    End Sub

    Private Sub DDLnewUserType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLnewUserType.SelectedIndexChanged
        Me.CTRLprofileData.ReloadProfileByType(SelectedIdProfileType, True)

        LNBsaveTypeBottom.Enabled = (Me.DDLnewUserType.SelectedValue <> IdProfileType)
        LNBsaveTypeTop.Enabled = (Me.DDLnewUserType.SelectedValue <> IdProfileType)
    End Sub
#End Region

    Private Sub LNBsaveTypeTop_Click(sender As Object, e As System.EventArgs) Handles LNBsaveTypeBottom.Click, LNBsaveTypeTop.Click
        If ValidateContent() Then
            LBerrors.Visible = False
            Me.CurrentPresenter.ProfileChanged(Me.CTRLprofileData.EditProfileType(SelectedIdProfileType))
        End If
    End Sub


   
    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class