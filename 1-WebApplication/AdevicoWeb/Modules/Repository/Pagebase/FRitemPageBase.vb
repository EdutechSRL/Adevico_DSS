Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public MustInherit Class FRitemPageBase
    Inherits PageBase
    Implements IViewItemPageBase

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdItem As Long Implements IViewItemPageBase.PreloadIdItem
        Get
            If IsNumeric(Request.QueryString("idItem")) Then
                Return CLng(Request.QueryString("idItem"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdVersion As Long Implements IViewItemPageBase.PreloadIdVersion
        Get
            If IsNumeric(Me.Request.QueryString("idVersion")) Then
                Return CLng(Me.Request.QueryString("idVersion"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdLink As Long Implements IViewItemPageBase.PreloadIdLink
        Get
            Return GetLongFromQueryString(lm.Comol.Core.FileRepository.Domain.QueryKeyNames.idLink, 0)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadItemType As lm.Comol.Core.FileRepository.Domain.ItemType Implements IViewItemPageBase.PreloadItemType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.FileRepository.Domain.ItemType).GetByString(Request.QueryString("type"), lm.Comol.Core.FileRepository.Domain.ItemType.None)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadSetBackUrl As Boolean Implements IViewItemPageBase.PreloadSetBackUrl
        Get
            Dim result As Boolean = False
            Boolean.TryParse(Request.QueryString("setbackUrl"), result)
            Return result
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadBackUrl As String Implements IViewItemPageBase.PreloadBackUrl
        Get
            If Not String.IsNullOrWhiteSpace(Request.QueryString("BackUrl")) Then
                Return Server.HtmlDecode(Request.QueryString("BackUrl"))
                'ElseIf Not IsNothing(Request.UrlReferrer) AndAlso Request.Url <> Request.UrlReferrer AndAlso Request.UrlReferrer.IsLoopback Then
                '    Dim url As String = Request.UrlReferrer.LocalPath

                '    If lm.Comol.Core.File.Exists.File(Server.MapPath(url)) Then
                '        If BaseUrl = "/" Then
                '            Return url.Substring(1, url.Length - 1)
                '        Else
                '            Return Replace(url, PageUtility.BaseUrl, "")
                '        End If
                '    End If
            End If
            Return ""
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property IdItem As Long Implements IViewItemPageBase.IdItem
        Get
            Return ViewStateOrDefault("IdItem", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdItem") = value
        End Set
    End Property
    Protected Friend Property IdLink As Long Implements IViewItemPageBase.IdLink
        Get
            Return ViewStateOrDefault("IdLink", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdLink") = value
        End Set
    End Property
    Protected Friend Property IdVersion As Long Implements IViewItemPageBase.IdVersion
        Get
            Return ViewStateOrDefault("IdVersion", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdVersion") = value
        End Set
    End Property
    Protected Friend Property ItemType As lm.Comol.Core.FileRepository.Domain.ItemType Implements IViewItemPageBase.ItemType
        Get
            Return ViewStateOrDefault("ItemType", lm.Comol.Core.FileRepository.Domain.ItemType.None)
        End Get
        Set(ByVal value As lm.Comol.Core.FileRepository.Domain.ItemType)
            Me.ViewState("ItemType") = value
        End Set
    End Property

    Protected Friend Property BackUrl As String Implements IViewItemPageBase.BackUrl
        Get
            Return ViewStateOrDefault("BackUrl", "")
        End Get
        Set(ByVal value As String)
            Me.ViewState("BackUrl") = value
        End Set
    End Property
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

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            Dim url As String = PreloadBackUrl
            If PreloadSetBackUrl AndAlso Not String.IsNullOrEmpty(url) Then
                BackUrl = url
                SetPageBackUrl(url)
            End If
        End If
    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
      
    End Sub

#Region "Inherits"
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_FileRepository", "Modules", "FileRepository")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub SetPageBackUrl(url As String) Implements IViewItemPageBase.SetPageBackUrl
        Dim oHyperlink As HyperLink = GetBackUrlItem()
        If Not IsNothing(oHyperlink) Then
            SetPageBackUrl(oHyperlink, url)
            SetPageBackUrl(GetBackUrlItem(False), url)
        End If
    End Sub

    Protected Friend Sub DisplaySessionTimeout(url As String, idCommunity As Integer) Implements IViewItemPageBase.DisplaySessionTimeout
        RedirectOnSessionTimeOut(url, idCommunity)
    End Sub

#Region "Actions"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType) Implements IViewItemPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType, idItem As Long, objType As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType) Implements IViewItemPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objType, idItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType, objects As Dictionary(Of lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType, Long)) Implements IViewItemPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objects.ToDictionary(Of Integer, String)(Function(o) CInt(o.Key), Function(o) o.Value.ToString())), InteractionType.UserWithLearningObject)
    End Sub
#End Region

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewItemPageBase.DisplayNoPermission
        PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        BindNoPermessi()
    End Sub
    Protected Friend MustOverride Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer, Optional ByVal display As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub
    Protected Sub RedirectOnSessionTimeOut(ByVal previousUrl As String, ByVal playerUrl As String, ByVal idCommunity As Integer)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow
        dto.DestinationUrl = playerUrl
        dto.Preserve = True
        dto.IsForDownload = False
        dto.IdCommunity = idCommunity
        If previousUrl.StartsWith(PageUtility.ApplicationUrlBase(True)) OrElse previousUrl.StartsWith(PageUtility.ApplicationUrlBase()) Then
            dto.PreviousUrl = Replace(previousUrl, PageUtility.ApplicationUrlBase(True), "")
            dto.PreviousUrl = Replace(dto.PreviousUrl, PageUtility.ApplicationUrlBase(False), "")
            Dim qKeys As List(Of String) = Request.UrlReferrer.Query.Split("&").ToList()
            If qKeys.Any(Function(f) f.Contains("idCommunity=")) Then
                Dim key As String = qKeys.Where(Function(f) f.Contains("idCommunity=")).FirstOrDefault()
                Dim value As String = Replace(key, "idCommunity=", "")
                If IsNumeric(value) Then
                    dto.IdCommunity = CInt(value)
                End If
            End If
        Else
            dto.PreviousUrl = ""
        End If

        webPost.Redirect(dto)
    End Sub
#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetBackUrlItem(Optional ByVal top As Boolean = True) As HyperLink
    Private Sub SetPageBackUrl(oHyperlink As HyperLink, url As String)
        If Not IsNothing(oHyperlink) Then
            If String.IsNullOrWhiteSpace(url) Then
                oHyperlink.Visible = False
            Else
                If (url.StartsWith("http")) OrElse url.StartsWith("~") Then
                    oHyperlink.NavigateUrl = url
                Else
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & url
                End If
                If Not IsNothing(oHyperlink.Parent) Then
                    oHyperlink.Parent.Visible = True
                End If
                oHyperlink.Visible = True
            End If
        End If
    End Sub

    Protected Friend ReadOnly Property GetLongFromQueryString(key As lm.Comol.Core.FileRepository.Domain.QueryKeyNames, dValue As Long) As Long
        Get
            Dim idItem As Long = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key.ToString)) Then
                Long.TryParse(Request.QueryString(key.ToString), idItem)
            End If
            Return idItem
        End Get
    End Property
    Protected Friend ReadOnly Property GetBooleanFromQueryString(key As lm.Comol.Core.FileRepository.Domain.QueryKeyNames, dValue As Boolean) As Boolean
        Get
            Dim result As Boolean = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key.ToString)) Then
                Boolean.TryParse(Request.QueryString(key.ToString), result)
            End If
            Return result
        End Get
    End Property
    Protected Friend ReadOnly Property GetGuidFromQueryString(key As lm.Comol.Core.FileRepository.Domain.QueryKeyNames, dValue As Guid) As Guid
        Get
            Dim item As Guid = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key.ToString)) Then
                Guid.TryParse(Request.QueryString(key.ToString), item)
            End If
            Return item
        End Get
    End Property
#End Region

End Class