Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Presentation

Public MustInherit Class EPpageBaseCertification
    Inherits PageBase
    Implements IViewCertificationPageBase

#Region "Context"
    Protected _serviceQS As COL_Questionario.Business.ServiceQuestionnaire
    Protected ReadOnly Property ServiceQS As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_serviceQS) Then
                _serviceQS = New COL_Questionario.Business.ServiceQuestionnaire(Me.PageUtility.CurrentContext)
            End If
            Return _serviceQS
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewCertificationPageBase.PreloadIdCommunity
        Get
            Return GetIntFromQueryString("idCommunity", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdSubactivity As Long Implements IViewCertificationPageBase.PreloadIdSubactivity
        Get
            Return GetLongFromQueryString("idSubactivity", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdActivity As Long Implements IViewCertificationPageBase.PreloadIdActivity
        Get
            Return GetLongFromQueryString("idActivity", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdPath As Long Implements IViewCertificationPageBase.PreloadIdPath
        Get
            Return GetLongFromQueryString("idPath", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdUser As Integer Implements IViewCertificationPageBase.PreloadIdUser
        Get
            Return GetIntFromQueryString("idUser", -1)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadReloadOpener As Boolean Implements IViewCertificationPageBase.PreloadReloadOpener
        Get
            Return GetBooleanFromQueryString("reloadopener", False)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadForManager As Boolean Implements IViewCertificationPageBase.PreloadForManager
        Get
            Return GetBooleanFromQueryString("manager", False)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadSaveStatistics As Boolean Implements IViewCertificationPageBase.PreloadSaveStatistics
        Get
            Return GetBooleanFromQueryString("saveStat", False)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadTime As Long Implements IViewCertificationPageBase.PreloadTime
        Get
            Return GetLongFromQueryString("st", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadTimeValidity As Long Implements IViewCertificationPageBase.PreloadTimeValidity
        Get
            Return GetLongFromQueryString("tv", 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadMac As String Implements IViewCertificationPageBase.PreloadMac
        Get
            Return Request.querystring("mac")
        End Get
    End Property
#End Region

    Public Property ReloadOpener As Boolean Implements IViewCertificationPageBase.ReloadOpener
        Get
            Return ViewStateOrDefault("ReloadOpener", False)
        End Get
        Set(value As Boolean)
            ViewState("ReloadOpener") = value
        End Set
    End Property
    Protected Friend MustOverride ReadOnly Property IsOnModalWindow As Boolean Implements IViewCertificationPageBase.IsOnModalWindow
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

    Protected Function GetDownloadCookie() As HttpCookie
        Dim query As String = Request.Url.Query
        If query.StartsWith("?") Then
            query = query.Remove(0, 1)
            Dim mac As String = PreloadMac
            If Not String.IsNullOrWhiteSpace(mac) Then
                query = Replace(query, mac, Server.UrlEncode(mac))
            End If
        End If
        Dim cookie As HttpCookie = New HttpCookie("fileDownload", query)
        cookie.Expires = Now.AddMinutes(5)
        If Request.Cookies.AllKeys.Contains("fileDownload") Then

        End If
        Return cookie
    End Function
    MustOverride ReadOnly Property IsDownloadPage As Boolean

#End Region

#Region "Inherits"
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
    Private Sub DisplayNoPermissions() Implements IViewCertificationPageBase.DisplayNoPermissions
        Response.AppendCookie(GetDownloadCookie())
        DisplayMessage(Resource.getValue("IViewCertificationPageBase.DisplayNoPermissions.Download." & IsDownloadPage.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewCertificationPageBase.DisplaySessionTimeout
        Response.AppendCookie(GetDownloadCookie())
        DisplayMessage(Resource.getValue("IViewCertificationPageBase.DisplaySessionTimeout.Download." & IsDownloadPage.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnavailableAction(name As String) Implements IViewCertificationPageBase.DisplayUnavailableAction
        Response.AppendCookie(GetDownloadCookie())
        DisplayMessage(Replace(Resource.getValue("IViewCertificationPageBase.DisplayUnavailableAction.Download." & IsDownloadPage.ToString), "#actionname#", name), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnknownItem() Implements IViewCertificationPageBase.DisplayUnknownItem
        Response.AppendCookie(GetDownloadCookie())
        DisplayMessage(Resource.getValue("IViewCertificationPageBase.DisplayUnknownItem.Download." & IsDownloadPage.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnselectedTemplate() Implements IViewCertificationPageBase.DisplayUnselectedTemplate
        Response.AppendCookie(GetDownloadCookie())
        DisplayMessage(Resource.getValue("IViewCertificationPageBase.DisplayUnselectedTemplate.Download." & IsDownloadPage.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

#End Region

#Region "Internal"
    MustOverride Sub DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
#End Region
End Class