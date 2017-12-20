Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class PublicAccess
    Inherits PageBase
    Implements IViewPublicAccess


#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As PublicAccessPresenter
    Public ReadOnly Property CurrentPresenter() As PublicAccessPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PublicAccessPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewPublicAccess.isSystemOutOfOrder
        Get
            Return Not Me.AccessoSistema
        End Get
    End Property
    Public ReadOnly Property PreloadedIdCommunity As Integer Implements IViewPublicAccess.PreloadedIdCommunity
        Get
            Try
                If IsNumeric(Request.QueryString("IdC")) Then
                    Return CInt(Request.QueryString("IdC"))
                End If
            Catch ex As Exception

            End Try
            Return 0
        End Get
    End Property
    Public ReadOnly Property PreloadedIdUser As Integer Implements IViewPublicAccess.PreloadedIdUser
        Get
            Try
                If IsNumeric(Request.QueryString("IdU")) Then
                    Return CInt(Request.QueryString("IdU"))
                End If
            Catch ex As Exception

            End Try
            Return 0
        End Get
    End Property
#End Region


#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            Me.Master.ShowLanguage = False
            Me.CurrentPresenter.InitView(PreloadedIdUser, PreloadedIdCommunity)
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UserSessionExpired", "Modules", "Common")
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTtitle)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub LogonIntoCommunity(idPerson As Integer, idCommunity As Integer) Implements IViewPublicAccess.LogonIntoCommunity
        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(LinguaCode)
        Me.PageUtility.AccessToCommunity(idPerson, idCommunity, oResourceConfig, True)
    End Sub
    Public Sub LogonUser(user As lm.Comol.Core.DomainModel.Person, idProvider As Long, providerUrl As String) Implements IViewPublicAccess.LogonUser
        Me.PageUtility.LogonUserIntoSystem(user, False, idProvider, PageUtility.BaseUrl & providerUrl)
    End Sub
    Public Sub DisplayCommunityName(name As String) Implements IViewPublicAccess.DisplayCommunityName
        LTtitle.Text = String.Format(Resource.getValue("DisplayCommunityName"), name)
    End Sub

    Public Sub DisplayAccountDisabled() Implements IViewPublicAccess.DisplayAccountDisabled
        LTdisplayInfo.Text = Resource.getValue("DisplayAccountDisabled")
    End Sub
    Public Sub DisplayNotAllowedCommunity() Implements IViewPublicAccess.DisplayNotAllowedCommunity
        LTdisplayInfo.Text = Resource.getValue("DisplayNotAllowedCommunity")
    End Sub
    Public Sub DisplaySystemOutOfOrder() Implements IViewPublicAccess.DisplaySystemOutOfOrder
        LTdisplayInfo.Text = Resource.getValue("DisplaySystemOutOfOrder")
    End Sub
    Public Sub DisplayUnknownCommunity() Implements IViewPublicAccess.DisplayUnknownCommunity
        LTdisplayInfo.Text = Resource.getValue("DisplayUnknownCommunity")
    End Sub
    Public Sub DisplayUnknownUser() Implements IViewPublicAccess.DisplayUnknownUser
        LTdisplayInfo.Text = Resource.getValue("DisplayUnknownUser")
    End Sub
#End Region

   
End Class