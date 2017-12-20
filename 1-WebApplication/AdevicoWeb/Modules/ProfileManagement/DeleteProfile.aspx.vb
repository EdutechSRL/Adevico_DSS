Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation


Partial Public Class DeleteProfile
    Inherits PageBase
    Implements IViewDeleteProfile

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
    Private _Presenter As DeleteProfilePresenter
    Private ReadOnly Property CurrentPresenter() As DeleteProfilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DeleteProfilePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Private _CurrentManager As ManagerProfiles
    Public ReadOnly Property CurrentManager() As ManagerProfiles
        Get
            If IsNothing(_CurrentManager) Then
                _CurrentManager = New ManagerProfiles(Me.CurrentContext)
            End If
            Return _CurrentManager
        End Get
    End Property

#Region "Implements"
    Public WriteOnly Property AllowDelete As Boolean Implements IViewDeleteProfile.AllowDelete
        Set(value As Boolean)
            Me.LNBconfirmDelete.Visible = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewDeleteProfile.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedIdProfile As Integer Implements IViewDeleteProfile.PreloadedIdProfile
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedIdProfileType As Integer Implements IViewDeleteProfile.PreloadedIdProfileType
        Get
            If IsNumeric(Request.QueryString("idProfileType")) Then
                Return CInt(Request.QueryString("idProfileType"))
            Else
                Return 0
            End If
        End Get
    End Property
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

    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Me.Resource.getValue("OnDeletingTranslation")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
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
            Me.Master.ServiceTitle = .getValue("serviceTitleDeleteProfile")

            .setHyperLink(HYPbackToManagement, True, True)
            HYPbackToManagement.NavigateUrl = Me.BaseUrl & RootObject.ManagementProfilesWithFilters

            .setLinkButton(Me.LNBconfirmDelete, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Private Sub LNBconfirmDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBconfirmDelete.Click
        'Dim PersonID As Integer = IdProfile
        'If PersonID > 0 AndAlso PersonID <> Me.CurrentContext.UserContext.CurrentUserID Then
        '    Dim oPersona As New COL_Persona With {.ID = PersonID}
        '    oPersona.DeletePerson()
        'End If
        CurrentPresenter.DeleteProfileInfo()
    End Sub


    Public Sub DisplayProfileInfo(displayName As String) Implements IViewDeleteProfile.DisplayProfileInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleDeleteNamedProfile"), displayName)

        Dim user As New COL_Persona With {.ID = IdProfile}
        user.Estrai(Session("LinguaID"))
        Dim totale As Integer = user.GetTotaleComunitaAssociate()
        If totale > 0 Then
            Me.LBelimina_t.Text = String.Format(Me.Resource.getValue("Delete.Multiple"), displayName, user.TipoPersona.Descrizione)
        Else
            Me.LBelimina_t.Text = String.Format(Me.Resource.getValue("Delete.Single"), displayName, user.TipoPersona.Descrizione)
        End If
    End Sub

    Public Sub DisplayProfileUnknown() Implements IViewDeleteProfile.DisplayProfileUnknown
        Me.LBelimina_t.Text = Resource.getValue("DisplayProfileUnknown")
    End Sub

    Public Sub NoPermission() Implements IViewDeleteProfile.NoPermission
        Me.Master.ShowNoPermission = True
    End Sub

    Public Function DeleteProfile(idProfile As Integer) As Boolean Implements IViewDeleteProfile.DeleteProfile
        Dim PersonID As Integer = idProfile
        If PersonID > 0 AndAlso PersonID <> Me.CurrentContext.UserContext.CurrentUserID Then
            Dim oPersona As New COL_Persona With {.ID = PersonID}
            oPersona.DeletePerson()
            Return (oPersona.Errore = Errori_Db.None)
        End If
        Return False
    End Function

    Public Sub GotoManagementPage() Implements IViewDeleteProfile.GotoManagementPage
        PageUtility.RedirectToUrl(RootObject.ManagementProfilesWithFilters())
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewDeleteProfile.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.DeleteProfile(IIf(IdProfile > 0, IdProfile, PreloadedIdProfile), PreloadedIdProfileType)
        webPost.Redirect(dto)
    End Sub
 
    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class