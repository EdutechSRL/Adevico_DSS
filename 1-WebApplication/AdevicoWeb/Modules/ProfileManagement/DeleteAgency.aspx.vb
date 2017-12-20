
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation


Partial Public Class DeleteAgency
    Inherits PageBase
    Implements IViewDeleteAgency

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
    Private _Presenter As DeleteAgencyPresenter
    Private ReadOnly Property CurrentPresenter() As DeleteAgencyPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DeleteAgencyPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private WriteOnly Property AllowDelete As Boolean Implements IViewDeleteAgency.AllowDelete
        Set(value As Boolean)
            Me.LNBconfirmDelete.Visible = value
            UDPdeleteAgency.Update()
        End Set
    End Property
    Private Property IdAgency As Long Implements IViewDeleteAgency.IdAgency
        Get
            Return ViewStateOrDefault("IdAgency", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdAgency") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdAgency As Long Implements IViewDeleteAgency.PreloadedIdAgency
        Get
            If IsNumeric(Request.QueryString("IdAgency")) Then
                Return CLng(Request.QueryString("IdAgency"))
            Else
                Return CLng(0)
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
        MyBase.SetCulture("pg_AgencyManagement", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleDeleteAgency")

            .setHyperLink(HYPbackToManagement, True, True)
            HYPbackToManagement.NavigateUrl = Me.BaseUrl & RootObject.ManagementAgenciesWithFilters

            .setLinkButton(Me.LNBconfirmDelete, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Private Sub LNBconfirmDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBconfirmDelete.Click
        CurrentPresenter.DeleteAgency()
    End Sub


    Public Sub DisplayProfileInfo(item As dtoAgency) Implements IViewDeleteAgency.DisplayAgencyInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceTitleDeleteNamedAgency"), item.Name)


        Me.LBelimina_t.Text = String.Format(Me.Resource.getValue("Delete.Single"), item.Name, item.EmployeeNumber)
    End Sub

    Public Sub DisplayAgencyUnknown() Implements IViewDeleteAgency.DisplayAgencyUnknown
        Me.LBelimina_t.Text = Resource.getValue("DisplayAgencyUnknown")
    End Sub

    Public Sub NoPermission() Implements IViewDeleteAgency.NoPermission
        Me.Master.ShowNoPermission = True
    End Sub

    Public Sub GotoManagementPage() Implements IViewDeleteAgency.GotoManagementPage
        PageUtility.RedirectToUrl(RootObject.ManagementAgenciesWithFilters)
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewDeleteAgency.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.DeleteAgency(IIf(IdAgency > 0, IdAgency, PreloadedIdAgency))
        webPost.Redirect(dto)
    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class