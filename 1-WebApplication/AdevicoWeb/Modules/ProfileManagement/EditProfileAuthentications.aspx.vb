Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication


Public Class EditProfileAuthentications
    Inherits PageBase
    Implements IViewEditProfileAuthentications


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
    Private _Presenter As EditProfileAuthenticationsPresenter
    Private ReadOnly Property CurrentPresenter() As EditProfileAuthenticationsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditProfileAuthenticationsPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property PreloadedIdProfile As Integer Implements IViewEditProfileAuthentications.PreloadedIdProfile
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedIdProfileType As Integer Implements IViewEditProfileAuthentications.PreloadedIdProfileType
        Get
            If IsNumeric(Request.QueryString("IdProfileType")) Then
                Return CInt(Request.QueryString("IdProfileType"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public Property IdProfile As Integer Implements IViewEditProfileAuthentications.IdProfile
        Get
            ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewEditProfileAuthentications.IdProfileType
        Get
            ViewStateOrDefault("IdProfileType", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewEditProfileAuthentications.AllowManagement
        Set(value As Boolean)
            HYPbackToManagement.Visible = value
            HYPbackToManagement.NavigateUrl = BaseUrl & RootObject.ManagementProfilesWithFilters
        End Set
    End Property
    Public WriteOnly Property AllowAddprovider As Boolean Implements IViewEditProfileAuthentications.AllowAddprovider
        Set(value As Boolean)
            Me.LNBaddNewProvider.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowEditProfile As Boolean Implements IViewEditProfileAuthentications.AllowEditProfile
        Set(value As Boolean)
            HYPbackToEdit.Visible = value
            HYPbackToEdit.NavigateUrl = RootObject.EditProfile(PreloadedIdProfile, PreloadedIdProfileType)
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
        Me.CurrentPresenter.InitView()
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
            Me.Master.ServiceTitle = .getValue("serviceTitleEditProfileAuthentications")
            Me.Master.ServiceNopermission = .getValue("serviceEditProfileAuthenticationsNopermission")
            .setHyperLink(HYPbackToManagement, True, True)
            .setHyperLink(HYPbackToEdit, True, True)

            .setLinkButton(LNBaddNewProvider, True, True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idProfile As Integer) Implements IViewEditProfileAuthentications.InitializeControl
        Me.CTRLauthentications.InitializeControl(idProfile, False)
        HYPbackToEdit.NavigateUrl = BaseUrl & RootObject.EditProfile(idProfile, IdProfileType)
    End Sub
    Public Sub SetTitle(displayName As String) Implements IViewEditProfileAuthentications.SetTitle
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleEditNamedProfileAuthentications"), displayName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitleEditNamedProfileAuthentications"), displayName)
    End Sub

    Public Sub DisplayAddAuthentication() Implements IViewEditProfileAuthentications.DisplayAddAuthentication
        Me.DVmenu.Visible = True
    End Sub

    Public Sub DisplayNoPermission() Implements IViewEditProfileAuthentications.DisplayNoPermission
        Me.MLVprofiles.SetActiveView(VIWmessage)
        Me.LBmessage.Text = Resource.getValue("EditProfileAuthentications.DisplayNoPermission")
    End Sub

    Public Sub DisplayProfileUnknown() Implements IViewEditProfileAuthentications.DisplayProfileUnknown
        Me.MLVprofiles.SetActiveView(VIWmessage)
        Me.LBmessage.Text = Resource.getValue("EditProfileAuthentications.DisplayProfileUnknown")
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewEditProfileAuthentications.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditProfileAuthentications(IIf(IdProfile > 0, IdProfile, PreloadedIdProfile), IIf(IdProfileType > 0, IdProfileType, PreloadedIdProfileType))
        webPost.Redirect(dto)
    End Sub

#End Region

    Private Sub LNBaddNewProvider_Click(sender As Object, e As System.EventArgs) Handles LNBaddNewProvider.Click
        Me.CTRLauthentications.DisplayAddView()
        Me.DVmenu.Visible = False
    End Sub

    Private Sub CTRLauthentications_DisplayAuthenticationEdit() Handles CTRLauthentications.DisplayAuthenticationEdit
        Me.DVmenu.Visible = False
    End Sub

    Private Sub CTRLauthentications_DisplayAuthenticationsList() Handles CTRLauthentications.DisplayAuthenticationsList
        Me.DVmenu.Visible = True
    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class