Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication


Public Class EditProfile
    Inherits PageBase
    Implements IViewEditProfile

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
    Private _Presenter As EditProfilePresenter
    Private ReadOnly Property CurrentPresenter() As EditProfilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditProfilePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property PreloadedIdProfile As Integer Implements IViewEditProfile.PreloadedIdProfile
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedIdProfileType As Integer Implements IViewEditProfile.PreloadedIdProfileType
        Get
            If IsNumeric(Request.QueryString("IdProfileType")) Then
                Return CInt(Request.QueryString("IdProfileType"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public Property AllowEdit As Boolean Implements IViewEditProfile.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            LNBsaveBottom.Visible = value
            LNBsaveTop.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowManageAuthentications As Boolean Implements IViewEditProfile.AllowManageAuthentications
        Set(value As Boolean)
            HYPtoProfileAuthentications.Visible = value
            HYPtoProfileAuthentications.NavigateUrl = BaseUrl & RootObject.EditProfileAuthentications(PreloadedIdProfile, PreloadedIdProfileType)
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewEditProfile.IdProfile
        Get
            ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewEditProfile.IdProfileType
        Get
            ViewStateOrDefault("IdProfileType", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewEditProfile.AllowManagement
        Set(value As Boolean)
            HYPbackToManagement.Visible = value
            HYPbackToManagement.NavigateUrl = BaseUrl & RootObject.ManagementProfilesWithFilters
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

#Region "Inherits"
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
            Me.Master.ServiceTitle = .getValue("serviceTitleEditProfile")
            Me.Master.ServiceNopermission = .getValue("serviceEditProfileNopermission")
            .setHyperLink(HYPbackToManagement, True, True)
            .setHyperLink(Me.HYPtoProfileAuthentications, True, True)

            .setLinkButton(LNBsaveBottom, True, True)
            .setLinkButton(LNBsaveTop, True, True)

        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub LoadProfile(idProfile As Integer, idProfileType As Integer) Implements IViewEditProfile.LoadProfile
        Me.MLVprofiles.SetActiveView(VIWedit)
        Me.CTRLprofileData.InitializeControl(idProfile, idProfileType)
    End Sub
    Public Sub LoadProfileName(displayName As String) Implements IViewEditProfile.LoadProfileName
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleEditNamedProfile"), displayName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitleEditNamedProfile"), displayName)
    End Sub
    Public Sub DisplayProfileUnknown() Implements IViewEditProfile.DisplayProfileUnknown
        Me.MLVprofiles.SetActiveView(VIWdefault)
        Me.LBmessage.Text = Resource.getValue("EditDisplayProfileUnknown")
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewEditProfile.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditProfile(IIf(IdProfile > 0, IdProfile, PreloadedIdProfile), IIf(IdProfileType > 0, IdProfileType, PreloadedIdProfileType))
        webPost.Redirect(dto)
    End Sub

    Public Sub GotoManagement() Implements IViewEditProfile.GotoManagement
        PageUtility.RedirectToUrl(RootObject.ManagementProfilesWithFilters())
    End Sub
    Public Function ValidateContent() As Boolean Implements IViewEditProfile.ValidateContent
        Return Me.CTRLprofileData.ValidateContent
    End Function
    Public Sub DisplayNoPermission() Implements IViewEditProfile.DisplayNoPermission
        Master.ShowNoPermission = True
    End Sub
    Public Sub DisplayErrorSaving() Implements IViewEditProfile.DisplayErrorSaving
        LBerrors.Text = Resource.getValue("DisplayErrorSaving")
        LBerrors.Visible = True
    End Sub
#End Region

    Private Sub LNBsaveBottom_Click(sender As Object, e As System.EventArgs) Handles LNBsaveBottom.Click, LNBsaveTop.Click
        If ValidateContent() Then
            LBerrors.Visible = False
            Me.CurrentPresenter.ProfileSaved(Me.CTRLprofileData.SaveData())
        End If
    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class