Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.FileRepository.Domain

Public MustInherit Class FReditViewDetailsPageBase
    Inherits FRitemPageBase
    Implements IViewEditViewDetailsPageBase

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdFolder As Long Implements IViewEditViewDetailsPageBase.PreloadIdFolder
        Get
            If IsNumeric(Me.Request.QueryString("idFolder")) Then
                Return CLng(Me.Request.QueryString("idFolder"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdentifierPath As String Implements IViewEditViewDetailsPageBase.PreloadIdentifierPath
        Get
            Dim path As String = Request.QueryString("Path")
            If String.IsNullOrWhiteSpace(path) Then
                path = ""
            End If
            Return path
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property RepositoryIdCommunity As Integer Implements IViewEditViewDetailsPageBase.RepositoryIdCommunity
        Get
            Return ViewStateOrDefault("RepositoryIdCommunity", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryIdCommunity") = value
        End Set
    End Property
 
    Protected Friend Property RepositoryType As lm.Comol.Core.FileRepository.Domain.RepositoryType Implements IViewEditViewDetailsPageBase.RepositoryType
        Get
            Return ViewStateOrDefault("RepositoryType", lm.Comol.Core.FileRepository.Domain.RepositoryType.Community)
        End Get
        Set(ByVal value As lm.Comol.Core.FileRepository.Domain.RepositoryType)
            Me.ViewState("RepositoryType") = value
        End Set
    End Property
    Protected Friend Property DefaultLogoutUrl As String Implements IViewEditViewDetailsPageBase.DefaultLogoutUrl
        Get
            Return ViewStateOrDefault("DefaultLogoutUrl", "")
        End Get
        Set(ByVal value As String)
            Me.ViewState("DefaultLogoutUrl") = value
        End Set
    End Property
#End Region

    Private _RepositoryDiskPath As String
    Protected Friend Function GetRepositoryDiskPath() As String Implements IViewEditViewDetailsPageBase.GetRepositoryDiskPath
        If String.IsNullOrEmpty(_RepositoryDiskPath) Then
            If Not String.IsNullOrEmpty(SystemSettings.File.Materiale.DrivePath) Then
                _RepositoryDiskPath = SystemSettings.File.Materiale.DrivePath
            Else
                _RepositoryDiskPath = Server.MapPath(PageUtility.BaseUrl & SystemSettings.File.Materiale.VirtualPath)
            End If
            'If Not String.IsNullOrWhiteSpace(_RepositoryDiskPath) Then
            '    _RepositoryDiskPath &= BaseRepositoryContext
            '    If _RepositoryDiskPath.StartsWith("\\") Then
            '        _RepositoryDiskPath = Replace(_RepositoryDiskPath, "\/", "\")
            '    End If
            '    _RepositoryDiskPath = Replace(_RepositoryDiskPath, "\\", "\")
            'End If
        End If
        Return _RepositoryDiskPath
    End Function
    Protected Property PageIdentifier As Guid Implements IViewEditViewDetailsPageBase.PageIdentifier
        Get
            Return ViewStateOrDefault("PageIdentifier", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("PageIdentifier") = value
        End Set
    End Property
    Protected Property OperationTicket As Long Implements IViewEditViewDetailsPageBase.OperationTicket
        Get
            Return ViewStateOrDefault("OperationTicket", 0)
        End Get
        Set(value As Long)
            ViewState("OperationTicket") = value
        End Set
    End Property
    Protected ReadOnly Property CurrentOperations As Dictionary(Of Guid, Long) Implements IViewEditViewDetailsPageBase.CurrentOperations
        Get
            Dim operations As New Dictionary(Of Guid, Long)
            If IsNothing(Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations)) OrElse Not TypeOf (Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations)) Is Dictionary(Of Guid, Long) Then
                Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations) = operations
            Else
                operations = Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations)
            End If
            Return operations
        End Get
    End Property
    Protected Sub AddCurrentTicket(idPage As Guid, idOperationTicket As Long) Implements IViewEditViewDetailsPageBase.AddCurrentTicket
        If CurrentOperations.ContainsKey(idPage) Then
            CurrentOperations.Item(idPage) = idOperationTicket
        Else
            CurrentOperations.Add(idPage, idOperationTicket)
        End If
    End Sub
    Protected Function GetCurrentOperationTicket(idPage As Guid) As Long Implements IViewEditViewDetailsPageBase.GetCurrentOperationTicket
        Dim operations As Dictionary(Of Guid, Long) = CurrentOperations
        If operations.ContainsKey(idPage) Then
            Return operations(idPage)
        Else
            Return 0
        End If
    End Function
    Protected Function isValidOperation() As Boolean Implements IViewEditViewDetailsPageBase.isValidOperation
        Dim isValid As Boolean = False
        Dim lastTicket As Long = GetCurrentOperationTicket(PageIdentifier)
        Dim thisTicket As Long = OperationTicket
        If thisTicket > lastTicket OrElse (lastTicket = thisTicket AndAlso thisTicket = 0) Then
            AddCurrentTicket(PageIdentifier, thisTicket)
            Return True
        Else
            Return False
        End If
    End Function
    Protected Sub TrackRefreshState()
        Dim lastTicket As Long = GetCurrentOperationTicket(PageIdentifier) + 1
        AddCurrentTicket(PageIdentifier, lastTicket)
    End Sub
   
#Region "Context"
    Private Property IdCurrentFolder As Long Implements IViewEditViewDetailsPageBase.IdCurrentFolder
        Get
            Return ViewStateOrDefault("IdCurrentFolder", 0)
        End Get
        Set(value As Long)
            ViewState("IdCurrentFolder") = value
        End Set
    End Property

    Private Property CurrentFolderIdentifierPath As String Implements IViewEditViewDetailsPageBase.CurrentFolderIdentifierPath
        Get
            Return ViewStateOrDefault("CurrentFolderIdentifierPath", "")
        End Get
        Set(value As String)
            ViewState("CurrentFolderIdentifierPath") = value
        End Set
    End Property
    Private Property CurrentFolderType As FolderType Implements IViewEditViewDetailsPageBase.CurrentFolderType
        Get
            Return ViewStateOrDefault("CurrentFolderType", FolderType.standard)
        End Get
        Set(value As FolderType)
            ViewState("CurrentFolderType") = value
        End Set
    End Property

    Private Property IsInitialized As Boolean Implements IViewEditViewDetailsPageBase.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Private _RepositoryIdentifier As String
    Public Property RepositoryIdentifier As String Implements IViewEditViewDetailsPageBase.RepositoryIdentifier
        Get
            If Not String.IsNullOrWhiteSpace(_RepositoryIdentifier) Then
                Return _RepositoryIdentifier
            Else
                Return ViewStateOrDefault("RepositoryIdentifier", "")
            End If
        End Get
        Set(value As String)
            _RepositoryIdentifier = value
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
#End Region
#End Region


    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        SaveRefreshState()
    End Sub


#Region "Implements"
    Private Function GetRootFolderFullname() As String Implements IViewEditViewDetailsPageBase.GetRootFolderFullname
        Return Resource.getValue("RootFolder")
    End Function
    Private Function GetRootFolderName() As String Implements IViewEditViewDetailsPageBase.GetRootFolderName
        Return Resource.getValue("RootFolderName")
    End Function
    Protected Function GetCurrentUrl() As String Implements IViewEditViewDetailsPageBase.GetCurrentUrl
        Dim url As String = Replace(Request.Url.AbsolutePath, PageUtility.ApplicationUrlBase(), "")
        url = Replace(url, PageUtility.ApplicationUrlBase(True), "")
        Return url
    End Function
    Private Sub GoToUrl(url As String) Implements IViewEditViewDetailsPageBase.GoToUrl
        If String.IsNullOrEmpty(url) Then
            PageUtility.RedirectToUrl(GetCurrentUrl)
        Else
            PageUtility.RedirectToUrl(url)
        End If
    End Sub
    Private Function GetFolderTypeTranslation() As Dictionary(Of FolderType, String) Implements IViewEditViewDetailsPageBase.GetFolderTypeTranslation
        Return (From e As FolderType In [Enum].GetValues(GetType(FolderType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.FolderType." & e.ToString))
    End Function
    Private Function GetTypesTranslations() As Dictionary(Of ItemType, String) Implements IViewEditViewDetailsPageBase.GetTypesTranslations
        Return (From e As ItemType In [Enum].GetValues(GetType(ItemType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.ItemType." & e.ToString))
    End Function
    Private Function GetUnknownUserName() As String Implements IViewEditViewDetailsPageBase.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Function GetPreviousUrl() As String Implements IViewEditViewDetailsPageBase.GetPreviousUrl
        Dim url As String = ""
        If Not IsNothing(Request.UrlReferrer) Then
            If Request.UrlReferrer.AbsoluteUri.StartsWith(PageUtility.ApplicationUrlBase(True)) OrElse Request.UrlReferrer.AbsoluteUri.StartsWith(PageUtility.ApplicationUrlBase()) Then
                url = Request.UrlReferrer.AbsoluteUri
            End If
        End If
        Return url
    End Function
    Private Function GetPreviousRelativeUrl() As String Implements IViewEditViewDetailsPageBase.GetPreviousRelativeUrl
        Dim url As String = GetPreviousUrl
        If Not String.IsNullOrWhiteSpace(url) Then
            If url.Contains(PageUtility.ApplicationUrlBase()) OrElse url.Contains(PageUtility.ApplicationUrlBase(True)) Then
                url = Replace(url, PageUtility.ApplicationUrlBase(True), "")
                url = Replace(url, PageUtility.ApplicationUrlBase(), "")
            Else
                url = ""
            End If
        End If
        Return url
    End Function

    Private Sub DisplayBackUrl(url As String) Implements IViewEditViewDetailsPageBase.DisplayBackUrl
        Dim obj As HyperLink = GetBackUrlItem()
        If String.IsNullOrWhiteSpace(url) Then
            obj.Visible = False
        Else
            obj.Visible = True
            obj.NavigateUrl = ApplicationUrlBase & url
        End If
    End Sub
    Protected Friend Function GetRepositoryCookie(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) As lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository Implements IViewEditViewDetailsPageBase.GetRepositoryCookie
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

#Region "Internal"

#End Region
    Private Sub SaveRefreshState()
        Dim ticket As Long = GetCurrentOperationTicket(PageIdentifier) + 1
        OperationTicket = ticket
    End Sub


End Class