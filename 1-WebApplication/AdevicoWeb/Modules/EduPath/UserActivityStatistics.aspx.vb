Public Class UserActivityStatistics
    Inherits PageBaseEduPath

#Region "Implements"

#End Region

#Region "Inherits"
    Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
        Get

        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean

    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

    ' DA SISTEMARE PER CONSENTIRE L'ACCESSO CORRETTO ALLA PAGINA
    Public Sub DisplaySessionTimeout(ByVal idCommunity As Integer, ByVal url As String)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        dto.IdCommunity = idCommunity

        webPost.Redirect(dto)
    End Sub


    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)

    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)

    End Sub
    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return False
        End Get
    End Property
End Class