Public Class LogoutMessage
    Inherits PageBase

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Private"
    Private ReadOnly Property PreloadLogoutMode As lm.Comol.Core.Authentication.LogoutMode
        Get
            Dim mode As lm.Comol.Core.Authentication.LogoutMode = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.LogoutMode).GetByString(Request.QueryString("mode"), lm.Comol.Core.Authentication.LogoutMode.logoutMessage)
            If (mode = lm.Comol.Core.Authentication.LogoutMode.logoutMessageAndUrl AndAlso String.IsNullOrEmpty(PreloadDestinationUrl)) Then
                mode = lm.Comol.Core.Authentication.LogoutMode.logoutMessage
            End If
            Return mode
        End Get
    End Property
    Private ReadOnly Property PreloadAuthenticationProviderType As lm.Comol.Core.Authentication.AuthenticationProviderType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.AuthenticationProviderType).GetByString(Request.QueryString("type"), lm.Comol.Core.Authentication.AuthenticationProviderType.Url)
        End Get
    End Property
    Private ReadOnly Property PreloadDestinationUrl As String
        Get
            Return Request.QueryString("url")
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.ShowLanguage = False
        Me.Master.ShowHelp = False
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        SetInternazionalizzazione()
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
            .setLabel(LBlogoutMessage)
            .setLiteral(LTlogoutMessageTitle)

            Dim s As String = .getValue("LBlogoutMessage.LogoutMode." & PreloadLogoutMode.ToString)
            If Not String.IsNullOrEmpty(s) Then
                If (PreloadLogoutMode = lm.Comol.Core.Authentication.LogoutMode.logoutMessageAndUrl) Then
                    LBlogoutMessage.Text = String.Format(s, PreloadDestinationUrl)
                Else
                    LBlogoutMessage.Text = s
                End If
            End If
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

End Class