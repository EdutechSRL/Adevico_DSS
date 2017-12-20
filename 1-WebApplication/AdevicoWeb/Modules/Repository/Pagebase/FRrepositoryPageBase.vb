Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public MustInherit Class FRrepositoryPageBase
    Inherits FRpageBase
    Implements IViewRepositoryPageBase



#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdItem As Long Implements IViewRepositoryPageBase.PreloadIdItem
        Get
            If IsNumeric(Me.Request.QueryString("idItem")) Then
                Return CLng(Me.Request.QueryString("idItem"))
            Else
                Return -1
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadIdPerson As Integer Implements IViewRepositoryPageBase.PreloadIdPerson
        Get
            If IsNumeric(Me.Request.QueryString("idPerson")) Then
                Return CInt(Me.Request.QueryString("idPerson"))
            Else
                Return -1
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadType As lm.Comol.Core.FileRepository.Domain.RepositoryType Implements IViewRepositoryPageBase.PreloadType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.FileRepository.Domain.RepositoryType).GetByString(Request.QueryString("type"), lm.Comol.Core.FileRepository.Domain.RepositoryType.Community)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdentifierPath As String Implements IViewRepositoryPageBase.PreloadIdentifierPath
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

    Protected Friend Property RepositoryType As lm.Comol.Core.FileRepository.Domain.RepositoryType Implements IViewRepositoryPageBase.RepositoryType
        Get
            Return ViewStateOrDefault("RepositoryType", lm.Comol.Core.FileRepository.Domain.RepositoryType.Community)
        End Get
        Set(ByVal value As lm.Comol.Core.FileRepository.Domain.RepositoryType)
            Me.ViewState("RepositoryType") = value
        End Set
    End Property
#End Region
    Private _RepositoryDiskPath As String
    Protected Friend Function GetRepositoryDiskPath() As String Implements IViewRepositoryPageBase.GetRepositoryDiskPath
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

    Protected Property PageIdentifier As Guid Implements IViewRepositoryPageBase.PageIdentifier
        Get
            Return ViewStateOrDefault("PageIdentifier", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("PageIdentifier") = value
        End Set
    End Property
    Protected Property OperationTicket As Long Implements IViewRepositoryPageBase.OperationTicket
        Get
            Return ViewStateOrDefault("OperationTicket", 0)
        End Get
        Set(value As Long)
            ViewState("OperationTicket") = value
        End Set
    End Property
    Protected ReadOnly Property CurrentOperations As Dictionary(Of Guid, Long) Implements IViewRepositoryPageBase.CurrentOperations
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
    Protected Sub AddCurrentTicket(idPage As Guid, idOperationTicket As Long) Implements IViewRepositoryPageBase.AddCurrentTicket
        If CurrentOperations.ContainsKey(idPage) Then
            CurrentOperations.Item(idPage) = idOperationTicket
        Else
            CurrentOperations.Add(idPage, idOperationTicket)
        End If
    End Sub
    Protected Function GetCurrentOperationTicket(idPage As Guid) As Long Implements IViewRepositoryPageBase.GetCurrentOperationTicket
        Dim operations As Dictionary(Of Guid, Long) = CurrentOperations
        If operations.ContainsKey(idPage) Then
            Return operations(idPage)
        Else
            Return 0
        End If
    End Function

    Protected Function isValidOperation() As Boolean Implements IViewRepositoryPageBase.isValidOperation
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

    Protected Friend Function GetRepositoryCookie(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) As lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository Implements IViewRepositoryPageBase.GetRepositoryCookie
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
    Protected Friend Sub WriteRepositoryCookie(item As lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository) Implements IViewRepositoryPageBase.WriteRepositoryCookie
        Dim oCookie As HttpCookie
        Dim name As String = "comol-RepositoryCookie-" & item.Repository.ToString("-")
        oCookie = Request.Cookies(name)
        If IsNothing(oCookie) Then
            oCookie = New HttpCookie(name)
            oCookie.Value = item.ToString("|")
            oCookie.Expires = DateTime.Now.AddMinutes(60)
            Response.Cookies.Add(oCookie)
        Else
            oCookie.Expires = DateTime.Now.AddMinutes(60)
            oCookie.Value = item.ToString("|")
            Response.Cookies.Set(oCookie)
        End If
        'If (From k In Response.Cookies.AllKeys Where k = "name").Any Then
        '    Response.Cookies.Set(oHttpCookie)
        'Else
        '    Response.Cookies.Add(oHttpCookie)
        'End If
    End Sub
    Public Sub RedirectToUrl(url As String) Implements IViewRepositoryPageBase.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region


    Private Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        SaveRefreshState()
    End Sub

    Private Sub SaveRefreshState()
        Dim ticket As Long = GetCurrentOperationTicket(PageIdentifier) + 1
        OperationTicket = ticket
    End Sub

    Public Function GetItemDownloadOrPlayUrl(item As lm.Comol.Core.FileRepository.Domain.RepositoryItem, setBackUrl As Boolean, Optional ByVal backUrl As String = "") As String
        Dim url As String = ""

        Select Case item.Type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.RepositoryItems(item.Repository.Type, item.Repository.IdCommunity, item.Repository.IdPerson, item.Id, item.Id, lm.Comol.Core.FileRepository.Domain.FolderType.standard)

            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                url = item.Url
                If Not String.IsNullOrWhiteSpace(url) Then
                    If Not (item.Url.ToLower().StartsWith("http://") OrElse Not item.Url.ToLower().StartsWith("https://")) Then
                        url = "http://" & item.Url
                    End If
                End If
            Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia, lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.PlayForRepository(item, setBackUrl, backUrl)
            Case Else
                url = lm.Comol.Core.FileRepository.Domain.RootObject.Download(PageUtility.ApplicationUrlBase(), item.Id, IIf(item.HasVersions, item.IdVersion, 0), item.DisplayName)
        End Select


        Return url
    End Function
    Public Function GetItemDownloadOrPlayUrl(item As lm.Comol.Core.FileRepository.Domain.dtoDisplayRepositoryItem, setBackUrl As Boolean, Optional ByVal backUrl As String = "") As String
        Dim url As String = ""

        Select Case item.Type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.RepositoryItems(item.Repository.Type, item.Repository.IdCommunity, item.Repository.IdPerson, item.Id, item.Id, lm.Comol.Core.FileRepository.Domain.FolderType.standard)

            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                url = item.Url
                Dim itemUrl As String = item.Url
                Dim urlStart As String = ""
                If url.Length > 10 Then
                    urlStart = url.Substring(0, 10)
                Else
                    urlStart = url
                End If
                If Not (urlStart.Contains("://") OrElse urlStart.Contains("mailto:") OrElse urlStart.Contains("news:")) Then
                    Dim queryString As String = ""
                    If (url.Contains("?")) Then
                        url = itemUrl.Split("?")(0)
                        queryString = itemUrl.Split("?")(1)
                    End If

                    'If PageUtility.BaseUrl = "/" Then

                    'End If
                    'If Not PageUtility.BaseUrl = "/" AndAlso url.StartsWith("/") Then
                    url = (BaseUrl & url).Replace("//", "/")
                    '   End If

                    If lm.Comol.Core.File.Exists.File(Server.MapPath(url)) Then
                        If itemUrl.StartsWith("/") Then
                            itemUrl = itemUrl.Remove(0, 1)
                        End If
                        url = PageUtility.ApplicationUrlBase & itemUrl
                    Else
                        url &= queryString
                    End If
                End If
            Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia, lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.PlayForRepository(item, setBackUrl, backUrl)
            Case Else
                url = lm.Comol.Core.FileRepository.Domain.RootObject.Download(PageUtility.ApplicationUrlBase(), item.Id, IIf(item.HasVersions, item.IdVersion, 0), item.DisplayName)
        End Select


        Return url
    End Function

    Public Function GetItemDownloadOrPlayUrl(item As lm.Comol.Core.FileRepository.Domain.liteRepositoryItem, setBackUrl As Boolean, Optional ByVal backUrl As String = "") As String
        Dim url As String = ""

        Select Case item.Type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.RepositoryItems(item.Repository.Type, item.Repository.IdCommunity, item.Repository.IdPerson, item.Id, item.Id, lm.Comol.Core.FileRepository.Domain.FolderType.standard)

            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                url = item.Url
                Dim itemUrl As String = item.Url
                Dim urlStart As String = ""
                If url.Length > 10 Then
                    urlStart = url.Substring(0, 10)
                Else
                    urlStart = url
                End If
                If Not (urlStart.Contains("://") OrElse urlStart.Contains("mailto:") OrElse urlStart.Contains("news:")) Then
                    Dim queryString As String = ""
                    If (url.Contains("?")) Then
                        url = itemUrl.Split("?")(0)
                        queryString = itemUrl.Split("?")(1)
                    End If

                    'If PageUtility.BaseUrl = "/" Then

                    'End If
                    'If Not PageUtility.BaseUrl = "/" AndAlso url.StartsWith("/") Then
                    url = (BaseUrl & url).Replace("//", "/")
                    '   End If

                    If lm.Comol.Core.File.Exists.File(Server.MapPath(url)) Then
                        If itemUrl.StartsWith("/") Then
                            itemUrl = itemUrl.Remove(0, 1)
                        End If
                        url = PageUtility.ApplicationUrlBase & itemUrl
                    Else
                        url &= queryString
                    End If
                End If
            Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia, lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.PlayForRepository(item, setBackUrl, backUrl)
            Case Else
                url = lm.Comol.Core.FileRepository.Domain.RootObject.Download(PageUtility.ApplicationUrlBase(), item.Id, IIf(item.HasVersions, item.IdVersion, 0), item.DisplayName)
        End Select


        Return url
    End Function

End Class