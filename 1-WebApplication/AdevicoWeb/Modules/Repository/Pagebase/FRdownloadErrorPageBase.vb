Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public MustInherit Class FRdownloadErrorPageBase
    Inherits FRpageBase
    Implements IViewDownloadError


#Region "Context"
    Private _Presenter As DownloadErrorPresenter
    Public ReadOnly Property CurrentPresenter() As DownloadErrorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DownloadErrorPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdItem As Long Implements IViewDownloadError.PreloadIdItem
        Get
            If IsNumeric(Request.QueryString("idItem")) Then
                Return CLng(Request.QueryString("idItem"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdVersion As Long Implements IViewDownloadError.PreloadIdVersion
        Get
            If IsNumeric(Me.Request.QueryString("idVersion")) Then
                Return CLng(Me.Request.QueryString("idVersion"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdModule As Integer Implements IViewDownloadError.PreloadIdModule
        Get
            Return GetIntFromQueryString(lm.Comol.Core.FileRepository.Domain.QueryKeyNames.idModule, 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdLink As Long Implements IViewDownloadError.PreloadIdLink
        Get
            Return GetLongFromQueryString(lm.Comol.Core.FileRepository.Domain.QueryKeyNames.idLink, 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadErrorType As lm.Comol.Core.FileRepository.Domain.DownloadErrorType Implements IViewDownloadError.PreloadErrorType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.FileRepository.Domain.DownloadErrorType).GetByString(Request.QueryString("type"), lm.Comol.Core.FileRepository.Domain.DownloadErrorType.none)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdNews As Guid Implements IViewDownloadError.PreloadIdNews
        Get
            Return GetGuidFromQueryString(lm.Comol.Core.FileRepository.Domain.QueryKeyNames.NewsId, Guid.Empty)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadWorkingSessionId As Guid Implements IViewDownloadError.PreloadWorkingSessionId
        Get
            Return GetGuidFromQueryString(lm.Comol.Core.FileRepository.Domain.QueryKeyNames.wSessionId, Guid.Empty)
        End Get
    End Property
#End Region

#End Region

#Region "Internal"
    Private ReadOnly Property GetLongFromQueryString(key As lm.Comol.Core.FileRepository.Domain.QueryKeyNames, dValue As Long) As Long
        Get
            Dim idItem As Long = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key.ToString)) Then
                Long.TryParse(Request.QueryString(key.ToString), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property GetIntFromQueryString(key As lm.Comol.Core.FileRepository.Domain.QueryKeyNames, dValue As Integer) As Long
        Get
            Dim result As Integer = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key.ToString)) Then
                Integer.TryParse(Request.QueryString(key.ToString), result)
            End If
            Return result
        End Get
    End Property
    Private ReadOnly Property GetGuidFromQueryString(key As lm.Comol.Core.FileRepository.Domain.QueryKeyNames, dValue As Guid) As Guid
        Get
            Dim idItem As Guid = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key.ToString)) Then
                Guid.TryParse(Request.QueryString(key.ToString), idItem)
            End If
            Return idItem
        End Get
    End Property

    Private Function GetPreviousUrl() As String
        Dim url As String = ""
        If Not IsNothing(Request.UrlReferrer) Then
            If Request.UrlReferrer.AbsoluteUri.StartsWith(PageUtility.ApplicationUrlBase(True)) OrElse Request.UrlReferrer.AbsoluteUri.StartsWith(PageUtility.ApplicationUrlBase()) Then
                url = Request.UrlReferrer.AbsoluteUri
            End If
        End If
        Return url
    End Function
#End Region

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Not Page.IsPostBack AndAlso Not Page.IsCallback AndAlso Not IsModalPage() AndAlso Not IsNothing(Request.UrlReferrer) AndAlso Not Request.UrlReferrer.AbsoluteUri.Contains(".download") Then 'AndAlso Not Request.UrlReferrer.AbsoluteUri = Request.absoluteuri Then
            DisplayBackUrl(GetPreviousUrl)
            GetHeader().InitializeHeader(True, lm.Comol.Core.FileRepository.Domain.PresetType.None, New Dictionary(Of PresetType, List(Of ViewOption)), New Dictionary(Of PresetType, List(Of ViewOption)), -2, -2, "FRitemPageBase")
        End If
        CurrentPresenter.InitView(IsExternalPage, PreloadIdItem, PreloadIdVersion, PreloadIdModule, PreloadIdLink, PreloadErrorType)
    End Sub
#End Region

#Region "Implements"
    Private Sub InitializeCommunityView(fullname As String, fileExtension As String, errorType As lm.Comol.Core.FileRepository.Domain.DownloadErrorType, idCommunity As Integer, communityName As String) Implements IViewDownloadError.InitializeCommunityView
        Dim value As String = GetTranslationValue(lm.Comol.Core.FileRepository.Domain.RepositoryType.Community, errorType, Not String.IsNullOrEmpty(communityName))
        If Not String.IsNullOrEmpty(communityName) Then
            DisplayMessage(String.Format(value, GetFilenameRender(fullname, fileExtension), communityName), errorType)
        Else
            DisplayMessage(String.Format(value, GetFilenameRender(fullname, fileExtension)), errorType)
        End If
    End Sub
    Private Sub InitializePortalView(fullname As String, fileExtension As String, errorType As lm.Comol.Core.FileRepository.Domain.DownloadErrorType) Implements IViewDownloadError.InitializePortalView
        DisplayMessage(String.Format(GetTranslationValue(lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal, errorType), GetFilenameRender(fullname, fileExtension)), errorType)
    End Sub

    Private Sub InitializeView(errorType As lm.Comol.Core.FileRepository.Domain.DownloadErrorType) Implements IViewDownloadError.InitializeView
        DisplayMessage(Resource.getValue("IViewDownloadError.UnknownItem.DownloadErrorType." & errorType.ToString), errorType)
    End Sub
    Private Sub InitializeContext(Optional context As Helpers.ExternalPageContext = Nothing) Implements IViewDownloadError.InitializeContext
        InitializeMaster(context)
    End Sub
    Private Function GetTranslationValue(type As lm.Comol.Core.FileRepository.Domain.RepositoryType, errorType As lm.Comol.Core.FileRepository.Domain.DownloadErrorType, Optional withName As Boolean = False)
        Return Resource.getValue("IViewDownloadError.RepositoryType." & type.ToString() & IIf(withName, ".name", "") & ".DownloadErrorType." & errorType.ToString)
    End Function
    Protected Friend Function GetRepositoryCookie(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) As lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository Implements IViewDownloadError.GetRepositoryCookie
        Dim name As String = "comol-RepositoryCookie-" & identifier.ToString("-")
        Dim oCookie As HttpCookie = Request.Cookies(name)
        If IsNothing(oCookie) Then
            Return Nothing
        ElseIf oCookie.Values.Count = 1 Then
            Return lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository.CreateFromString(identifier, "|", oCookie.Value)
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "Internal MustOverride"
    Protected MustOverride Sub DisplayMessage(message As String, errorType As lm.Comol.Core.FileRepository.Domain.DownloadErrorType)
    Protected MustOverride Function GetFilenameRender(fullname As String, fileExtension As String) As String
    Protected MustOverride Function IsModalPage() As Boolean
    Protected MustOverride Function IsExternalPage() As Boolean
    Protected MustOverride Sub DisplayBackUrl(url As String)
    Protected MustOverride Sub InitializeMaster(context As Helpers.ExternalPageContext)
    Protected MustOverride Function GetHeader() As UC_RepositoryHeader
#End Region


   
End Class