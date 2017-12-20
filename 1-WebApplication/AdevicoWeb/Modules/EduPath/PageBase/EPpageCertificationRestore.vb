Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Presentation

Public MustInherit Class EPpageCertificationRestore
    Inherits EPpageBaseCertification
    Implements IViewCertificationRestore

#Region "Context"
    Private _Presenter As CertificationRestorePresenter
    Public ReadOnly Property CurrentPresenter() As CertificationRestorePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CertificationRestorePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"

#End Region


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

#Region "Internal"
    Private ReadOnly Property TemplateBaseUrl As String
        Get
            Return Me.Request.Url.AbsoluteUri.Replace( _
             Me.Request.Url.PathAndQuery, "") & Me.BaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property
    Private ReadOnly Property GetLongFromQueryString(key As String, dValue As Long) As Long
        Get
            Dim idItem As Long = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Long.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property GetIntFromQueryString(key As String, dValue As Integer) As Long
        Get
            Dim result As Integer = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Integer.TryParse(Request.QueryString(key), result)
            End If
            Return result
        End Get
    End Property
    Private ReadOnly Property GetGuidFromQueryString(key As String, dValue As Guid) As Guid
        Get
            Dim idItem As Guid = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Guid.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property GetBooleanFromQueryString(key As String, dValue As Boolean) As Boolean
        Get
            Dim idItem As Boolean = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Boolean.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property CertificationFilePath As String
        Get
            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.UserCertifications.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.UserCertifications.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.UserCertifications.DrivePath
            End If
            Return baseFilePath
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides Sub BindDati()
        DisplayMessage(Resource.getValue("IViewCertificationRestore.DefaultTitle"), Helpers.MessageType.alert)
        CurrentPresenter.InitView(IsOnModalWindow, PreloadTime, PreloadTimeValidity, PreloadMac, PreloadIdPath, PreloadIdActivity, PreloadIdSubactivity, PreloadIdUser)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Certification", "EduPath")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub AddAutoRedirectoToDownloadPage(url As String) Implements IViewCertificationRestore.AddAutoRedirectoToDownloadPage
        'Response.AppendCookie(GetDownloadCookie())
        If Not IsOnModalWindow Then
            DisplayCloseButton()
        End If
        Response.AddHeader("Refresh", "1;" & PageUtility.ApplicationUrlBase() & url)
    End Sub
#End Region

#Region "Internal"
    Protected Friend MustOverride Sub DisplayCloseButton()
#End Region

   
End Class