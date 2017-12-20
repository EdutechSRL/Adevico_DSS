Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class ActivateUserMail
    Inherits PageBase
    Implements IViewActivateUserMail

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
    Private _Presenter As ActivateUserMailPresenter
    Private ReadOnly Property CurrentPresenter() As ActivateUserMailPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ActivateUserMailPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property Identifier As System.Guid Implements IViewActivateUserMail.Identifier
        Get
            Return ViewStateOrDefault("Identifier", System.Guid.Empty)
        End Get
        Set(value As System.Guid)
            ViewState("Identifier") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedIdentifier As System.Guid Implements IViewActivateUserMail.PreloadedIdentifier
        Get
            If String.IsNullOrEmpty(Request.QueryString("Identifier")) Then
                Return System.Guid.Empty
            Else
                Try
                    Return New System.Guid(Request.QueryString("Identifier"))
                Catch ex As Exception
                    Return System.Guid.Empty
                End Try
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
            Me.CurrentPresenter.InitView(Me.PreloadedIdentifier)
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
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleActivateUserMail")
            Me.Master.ServiceNopermission = .getValue("serviceTitleActivateUserMailNopermission")
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub DisplayActivationComplete() Implements IViewActivateUserMail.DisplayActivationComplete
        Me.LBactivationMessage.Text = Resource.getValue("DisplayActivationComplete")
    End Sub
    Public Sub DisplayError(errorMessage As ErrorMessages) Implements IViewActivateUserMail.DisplayError
        Me.LBactivationMessage.Text = Resource.getValue("ErrorMessages." & errorMessage.ToString)
    End Sub
    Public Sub DisplayProfileUnknown() Implements IViewActivateUserMail.DisplayProfileUnknown
        Me.LBactivationMessage.Text = Resource.getValue("DisplayProfileUnknown")
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewActivateUserMail.DisplaySessionTimeout
        Me.LBactivationMessage.Text = Resource.getValue("DisplaySessionTimeout")
    End Sub
#End Region

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class