Public Class CurrentVersion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LITbaseUrl.Text = Me.ApplicationUrlBase
    End Sub




#Region "Copia da OLDpageUtility"
    Private _SecureBaseUrl As String
    Private _BaseUrl As String

    Public ReadOnly Property ApplicationUrlBase(Optional ByVal WithoutSSLfromConfig As Boolean = False, Optional ByVal forLoginPage As Boolean = False) As String
        Get
            Dim Redirect As String = "http"
            Try
                Dim _Request As HttpRequest = HttpContext.Current.Request

                If (RequireSSL AndAlso Not WithoutSSLfromConfig) OrElse (forLoginPage AndAlso SystemSettings.Login.isSSLloginRequired) Then
                    Redirect &= "s://" & _Request.Url.Host & Me.BaseUrl
                Else
                    Redirect &= "://" & _Request.Url.Host & Me.BaseUrl
                End If
            Catch ex As Exception

            End Try

            Return Redirect
        End Get
    End Property

    Public ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property
    Public ReadOnly Property RequireSSL() As Boolean
        Get
            Dim RichiediSSL As Boolean = False
            Try
                RequireSSL = SystemSettings.Login.isSSLrequired
            Catch ex As Exception
                RequireSSL = False
            End Try
        End Get
    End Property

    Public ReadOnly Property BaseUrl() As String
        Get
            Return BaseUrl(False)
        End Get
    End Property
    Public ReadOnly Property SecureBaseUrl() As String
        Get
            Return BaseUrl(True)
        End Get
    End Property
    Public ReadOnly Property BaseUrl(ByVal secure As Boolean) As String
        Get
            If secure Then
                If _SecureBaseUrl = "" Then
                    Dim url As String = Me.SecureApplicationUrlBase
                    If url.EndsWith("/") Then
                        _SecureBaseUrl = url
                    Else
                        _SecureBaseUrl = url + "/"
                    End If
                End If

                Return _SecureBaseUrl
            Else
                If _BaseUrl = "" Then
                    Dim _Request As HttpRequest = HttpContext.Current.Request
                    Dim url As String = _Request.ApplicationPath
                    If url.EndsWith("/") Then
                        _BaseUrl = url
                    Else
                        _BaseUrl = url + "/"
                    End If
                End If

                Return _BaseUrl
            End If
        End Get
    End Property

    Public ReadOnly Property SecureApplicationUrlBase() As String
        Get
            Dim _Request As HttpRequest = HttpContext.Current.Request
            Return "https://" & _Request.Url.Host & Me.BaseUrl
        End Get
    End Property
#End Region
End Class