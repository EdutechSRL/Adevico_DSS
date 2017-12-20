Public Class RedirectToDefaultModule
    Inherits PageBase
    'Inherits System.Web.UI.Page


    Private ReadOnly Property IdCommunity As Integer
        Get
            If IsNumeric(Request.QueryString("IdCommunity")) Then

                Return CInt(Request.QueryString("IdCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property ToHomePortal As Boolean
        Get
            Return (Request.QueryString("To") = "HomePortal")
        End Get
    End Property
    Private ReadOnly Property ToSubscribedCommunities As Boolean
        Get
            Return (Request.QueryString("To") = "Subscriptions")
        End Get
    End Property
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
        If Not isUtenteAnonimo AndAlso ToSubscribedCommunities Then
            Dim url As String = Me.SystemSettings.Presenter.DefaultSubscriptionsLink
            If String.IsNullOrEmpty(url) Then
                url = "Comunita/EntrataComunita.aspx"
            End If
            Me.GoToPortale(url)
        ElseIf isUtenteAnonimo OrElse IdCommunity = 0 OrElse ToHomePortal Then
            Me.GoToPortale()
        Else
            Me.RedirectToUrl(PlainRedirectToDefaultPage(IdCommunity, UtenteCorrente.ID))
        End If
    End Sub

   
#Region "Inherits"
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

End Class