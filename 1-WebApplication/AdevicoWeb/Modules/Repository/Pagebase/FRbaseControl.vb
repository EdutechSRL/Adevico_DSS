Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.FileRepository.Domain
Public MustInherit Class FRbaseControl
    Inherits BaseControl

#Region "Implements"
    Private _RepositoryDiskPath As String
    Protected Friend Function GetRepositoryDiskPath() As String
        If String.IsNullOrEmpty(_RepositoryDiskPath) Then
            If Not String.IsNullOrEmpty(SystemSettings.File.Materiale.DrivePath) Then
                _RepositoryDiskPath = SystemSettings.File.Materiale.DrivePath
            Else
                _RepositoryDiskPath = Server.MapPath(PageUtility.BaseUrl & SystemSettings.File.Materiale.VirtualPath)
            End If
            If Not String.IsNullOrWhiteSpace(_RepositoryDiskPath) Then
                If _RepositoryDiskPath.StartsWith("\\") Then
                    _RepositoryDiskPath = Replace(_RepositoryDiskPath, "\/", "\")
                End If
                _RepositoryDiskPath = Replace(_RepositoryDiskPath, "\\", "\")
            End If
        End If
        Return _RepositoryDiskPath
    End Function
    Private _RepositoryThumbnailDiskPath As String
    Protected Friend Function GetRepositoryThumbnailDiskPath() As String
        If String.IsNullOrEmpty(_RepositoryThumbnailDiskPath) Then
            If Not String.IsNullOrEmpty(SystemSettings.File.FileThumbnail.DrivePath) Then
                _RepositoryThumbnailDiskPath = SystemSettings.File.FileThumbnail.DrivePath
            Else
                _RepositoryThumbnailDiskPath = Server.MapPath(PageUtility.BaseUrl & SystemSettings.File.FileThumbnail.VirtualPath)
            End If
            If Not String.IsNullOrWhiteSpace(_RepositoryThumbnailDiskPath) Then
                If _RepositoryThumbnailDiskPath.StartsWith("\\") Then
                    _RepositoryThumbnailDiskPath = Replace(_RepositoryThumbnailDiskPath, "\/", "\")
                End If
                _RepositoryThumbnailVirtualPath = Replace(_RepositoryThumbnailVirtualPath, "//", "\")
                _RepositoryThumbnailDiskPath = Replace(_RepositoryThumbnailDiskPath, "\\", "\")
            End If
        End If
        Return _RepositoryThumbnailDiskPath
    End Function
    Private _RepositoryThumbnailVirtualPath As String
    Private _RepositoryThumbnailFullVirtualPath As String
    Protected Friend Function GetRepositoryThumbnailVirtualPath() As String
        If String.IsNullOrEmpty(_RepositoryThumbnailVirtualPath) Then
            _RepositoryThumbnailVirtualPath = SystemSettings.File.FileThumbnail.VirtualPath
            _RepositoryThumbnailVirtualPath &= BaseRepositoryContext
            If Not String.IsNullOrWhiteSpace(_RepositoryThumbnailVirtualPath) Then
                _RepositoryThumbnailVirtualPath = Replace(_RepositoryThumbnailVirtualPath, "\", "/")
                _RepositoryThumbnailVirtualPath = Replace(_RepositoryThumbnailVirtualPath, "\", "/")
                _RepositoryThumbnailVirtualPath = Replace(_RepositoryThumbnailVirtualPath, "//", "/")
                _RepositoryThumbnailVirtualPath = Replace(_RepositoryThumbnailVirtualPath, "\\", "/")
            End If
        End If
        Return _RepositoryThumbnailVirtualPath
    End Function
    Protected Friend Function GetFinalPath() As String
        Dim path As String = GetRepositoryDiskPath()
        If Not String.IsNullOrEmpty(path) Then

            path &= BaseRepositoryContext
            If path.StartsWith("\\") Then
                path = Replace(path, "\/", "\")
            End If
            path = Replace(path, "\\", "\")
        End If
        Return path
    End Function
    Protected Friend Function GetFinalThumbnailPath() As String
        Dim path As String = GetRepositoryThumbnailDiskPath()

        If Not String.IsNullOrEmpty(path) Then

            path &= BaseRepositoryContext
            If path.StartsWith("\\") Then
                path = Replace(path, "\/", "\")
            End If
            path = Replace(path, "\\", "\")
        End If
        Return path
    End Function
    Protected Friend Function GetFinalThumbnailVirtualPath() As String
        If String.IsNullOrWhiteSpace(_RepositoryThumbnailFullVirtualPath) Then
            Dim path As String = GetRepositoryThumbnailVirtualPath()
            If path.StartsWith("/") Then
                path = path.Substring(1)
            End If
            _RepositoryThumbnailFullVirtualPath = PageUtility.ApplicationUrlBase() & path
        End If
        Return _RepositoryThumbnailFullVirtualPath
    End Function
    Private _UploadDiskPath As String
    Protected Friend Function GetUploadDiskPath() As String
        If String.IsNullOrEmpty(_UploadDiskPath) Then
            _UploadDiskPath = System.Configuration.ConfigurationManager.AppSettings("Telerik.RadUpload.TempFolder")
        End If
        If Not String.IsNullOrWhiteSpace(_UploadDiskPath) AndAlso lm.Comol.Core.File.Exists.Directory(_UploadDiskPath) Then
            Return _UploadDiskPath
        Else
            Return ""
        End If
    End Function

    Protected Friend Function GetContextPath(ByVal type As lm.Comol.Core.FileRepository.Domain.RepositoryType, ByVal idCommunity As Integer) As String
        Select Case type
            Case RepositoryType.Community
                Return "\" & idCommunity.ToString() & "\"
            Case Else
                Return "\0\"
        End Select
    End Function
    Protected Friend Sub InitializeRepositoryPath(ByVal type As lm.Comol.Core.FileRepository.Domain.RepositoryType, ByVal idCommunity As Integer)
        BaseRepositoryContext = GetContextPath(type, idCommunity)
    End Sub
    Protected Friend Sub InitializeRepositoryPath(ByVal item As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
        BaseRepositoryContext = GetContextPath(item.Type, item.IdCommunity)
    End Sub

    Private Property BaseRepositoryContext As String
        Get
            Return ViewStateOrDefault("BaseRepositoryContext", "")
        End Get
        Set(value As String)
            ViewState("BaseRepositoryContext") = value
        End Set
    End Property
#End Region


#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private ReadOnly Property CookieBaseName As String
        Get
            Return "comol_FileRepository"
        End Get
    End Property
    'Protected Friend Function GetCurrentCookie() As UserCurrentSettings 'Implements IViewBaseDashboardLoader.GetCurrentCookie
    '    Dim myCookie As HttpCookie = Request.Cookies(CookieName)
    '    If IsNothing(myCookie) Then
    '        Return Nothing
    '    Else
    '        Dim result As New UserCurrentSettings
    '        result.IdSelectedTile = CLng(myCookie("IdSelectedTile"))
    '        result.IdSelectedTag = CLng(myCookie("IdSelectedTag"))
    '        result.AfterUserLogon = CInt(myCookie("AfterUserLogon"))
    '        result.GroupBy = CInt(myCookie("GroupBy"))
    '        result.DefaultNoticeboard = CInt(myCookie("Noticeboard"))
    '        result.TileNoticeboard = CInt(myCookie("TileNoticeboard"))
    '        result.CombinedNoticeboard = CInt(myCookie("CombineNoticeboard"))
    '        result.ListNoticeboard = CInt(myCookie("ListNoticeboard"))
    '        result.OrderBy = CInt(myCookie("OrderBy"))
    '        result.View = CInt(myCookie("View"))
    '        Return result
    '    End If
    'End Function
    'Protected Friend Sub SaveCurrentCookie(settings As UserCurrentSettings) 'Implements IViewBaseDashboardLoader.SaveCurrentCookie
    '    Dim myCookie As HttpCookie = New HttpCookie(CookieName)
    '    myCookie("IdSelectedTile") = settings.IdSelectedTile
    '    myCookie("IdSelectedTag") = settings.IdSelectedTag
    '    myCookie("AfterUserLogon") = CInt(settings.AfterUserLogon)
    '    myCookie("GroupBy") = CInt(settings.GroupBy)
    '    myCookie("Noticeboard") = CInt(settings.DefaultNoticeboard)
    '    myCookie("TileNoticeboard") = CInt(settings.TileNoticeboard)
    '    myCookie("CombineNoticeboard") = CInt(settings.CombinedNoticeboard)
    '    myCookie("ListNoticeboard") = CInt(settings.ListNoticeboard)
    '    myCookie("OrderBy") = CInt(settings.OrderBy)
    '    myCookie("View") = CInt(settings.View)

    '    myCookie.Expires = DateTime.Now.AddHours(24)


    '    If Request.Cookies.AllKeys.Contains(CookieName) Then
    '        Response.Cookies.Set(myCookie)
    '    Else
    '        Response.Cookies.Add(myCookie)
    '    End If
    'End Sub

    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function

    Public Function GetItemUrl(item As lm.Comol.Core.FileRepository.Domain.dtoDisplayRepositoryItem, action As lm.Comol.Core.FileRepository.Domain.ItemAction, ByVal type As lm.Comol.Core.FileRepository.Domain.RepositoryType, ByVal idCommunity As Integer, order As OrderBy, ByVal ascending As Boolean) As String
        Dim url As String = ""
        Select Case action
            Case lm.Comol.Core.FileRepository.Domain.ItemAction.download
                Select Case item.Type
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                        Select Case item.FolderType
                            Case lm.Comol.Core.FileRepository.Domain.FolderType.standard
                                url = PageUtility.ApplicationUrlBase & RootObject.RepositoryItems(type, idCommunity, item.Id, item.Id, item.FolderType, order, ascending)
                            Case Else
                                url = PageUtility.ApplicationUrlBase & RootObject.FolderUrlTemplate(item.Id, item.FolderType, item.IdentifierPath, type, idCommunity)
                                url = Replace(Replace(url, "#OrderBy#", order.ToString), "#Boolean#", ascending.ToString().ToLower)
                        End Select
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

                    Case Else
                        url = RootObject.Download(PageUtility.ApplicationUrlBase(), item.Id, IIf(item.HasVersions, item.IdVersion, 0), item.DisplayName)
                End Select

            Case lm.Comol.Core.FileRepository.Domain.ItemAction.edit, lm.Comol.Core.FileRepository.Domain.ItemAction.editPermission, _
                lm.Comol.Core.FileRepository.Domain.ItemAction.manageVersions
                Dim identifierPath As String = ""
                If Not IsNothing(item.Father) Then
                    identifierPath = item.Father.IdentifierPath
                End If
                url = PageUtility.ApplicationUrlBase & RootObject.EditItem(item.Id, item.IdFolder, identifierPath, action, True)
            Case ItemAction.editSettings
                Dim identifierPath As String = ""
                If Not IsNothing(item.Father) Then
                    identifierPath = item.Father.IdentifierPath
                End If
                Select Case item.Type
                    Case ItemType.Multimedia
                        url = PageUtility.ApplicationUrlBase & RootObject.EditMultimediaSettings(item.Id, item.IdVersion, item.IdFolder, identifierPath, True)
                    Case ItemType.ScormPackage
                        url = PageUtility.ApplicationUrlBase & RootObject.EditScormSettings(item.Id, item.IdVersion, item.IdFolder, identifierPath, True)
                End Select


            Case lm.Comol.Core.FileRepository.Domain.ItemAction.play
                url = PageUtility.ApplicationUrlBase & RootObject.PlayForRepository(item)
            Case lm.Comol.Core.FileRepository.Domain.ItemAction.preview
                Select Case item.Type
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
                    Case Else
                        url = RootObject.Download(PageUtility.ApplicationUrlBase, item.Id, IIf(item.HasVersions, item.IdVersion, 0), item.DisplayName)
                End Select
            Case lm.Comol.Core.FileRepository.Domain.ItemAction.viewMyStatistics
                url = PageUtility.ApplicationUrlBase & RootObject.MyStatistics(item.IdFolder, item.Id)
            Case lm.Comol.Core.FileRepository.Domain.ItemAction.viewOtherStatistics
                url = PageUtility.ApplicationUrlBase & RootObject.Statistics(item.IdFolder, item.Id)
            Case lm.Comol.Core.FileRepository.Domain.ItemAction.viewPermission, lm.Comol.Core.FileRepository.Domain.ItemAction.details
                Dim identifierPath As String = ""
                If Not IsNothing(item.Father) Then
                    identifierPath = item.Father.IdentifierPath
                End If
                url = PageUtility.ApplicationUrlBase & RootObject.Details(item.Id, item.IdFolder, identifierPath, action, True)
        End Select
        Return url
    End Function


    Public Function GetItemUrl(version As lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain.dtoDisplayVersionItem, action As lm.Comol.Core.FileRepository.Domain.ItemAction) As String
        Dim url As String = ""
        Select Case action
            Case lm.Comol.Core.FileRepository.Domain.ItemAction.download
                Select Case version.Type
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                        url = version.Url
                        If Not String.IsNullOrWhiteSpace(version.Url) Then
                            If Not (version.Url.ToLower().StartsWith("http://") OrElse version.Url.ToLower().StartsWith("https://")) Then
                                url = "http://" & version.Url
                            End If
                        End If
                      
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument

                    Case Else
                        url = RootObject.Download(PageUtility.ApplicationUrlBase, version.IdItem, version.Id, version.DisplayName)
                End Select

            Case lm.Comol.Core.FileRepository.Domain.ItemAction.play
                url = PageUtility.ApplicationUrlBase & RootObject.PlayForRepository(version.IdItem, version.Id, version.Type, version.DisplayMode)

        End Select
        Return url
    End Function
    Public Function GetItemDownloadOrPlayUrl(item As lm.Comol.Core.FileRepository.Domain.RepositoryItem, setBackUrl As Boolean, Optional ByVal backUrl As String = "") As String
        Dim url As String = ""

        Select Case item.Type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.RepositoryItems(item.Repository.Type, item.Repository.IdCommunity, item.Repository.IdPerson, item.Id, item.Id, lm.Comol.Core.FileRepository.Domain.FolderType.standard)

            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                url = item.Url
                If Not String.IsNullOrWhiteSpace(item.Url) Then
                    If Not (item.Url.ToLower().StartsWith("http://") OrElse item.Url.ToLower().StartsWith("https://")) Then
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
    Public Function GetItemDownloadOrPlayUrl(item As lm.Comol.Core.FileRepository.Domain.liteRepositoryItem, setBackUrl As Boolean, Optional ByVal backUrl As String = "") As String
        Dim url As String = ""

        Select Case item.Type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                url = PageUtility.ApplicationUrlBase & lm.Comol.Core.FileRepository.Domain.RootObject.RepositoryItems(item.Repository.Type, item.Repository.IdCommunity, item.Repository.IdPerson, item.Id, item.Id, lm.Comol.Core.FileRepository.Domain.FolderType.standard)

            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                 url = item.Url
                If Not String.IsNullOrWhiteSpace(item.Url) Then
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
#Region "DateTimeFormat"
    Protected Friend Function GetDateTimeString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Return GetDateToString(datetime, defaultString) & " " & GetTimeToString(datetime, defaultString)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetDateToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Dim pattern As String = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
            If (pattern.Contains("yyyy")) Then
                pattern = pattern.Replace("yyyy", "yy")
            End If
            Return datetime.Value.ToString(pattern)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetTimeToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Return datetime.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
        Else
            Return defaultString
        End If
    End Function
#End Region
    Protected Sub SetCheckBoxListItemsCssClass(ByVal obj As CheckBoxList)
        For Each item As ListItem In obj.Items
            item.Attributes.Add("class", "item")
        Next
    End Sub
    Protected Sub SetRadioButtonListItemsCssClass(ByVal oRadioButtonList As RadioButtonList)
        For Each item As ListItem In oRadioButtonList.Items
            item.Attributes.Add("class", "item")
        Next
    End Sub
    Protected Function SanitizeLinkUrl(sourceUrl As String) As String
        Dim sUrl As String = sourceUrl


        If Not String.IsNullOrWhiteSpace(sourceUrl) Then
            If Not (sourceUrl.ToLower().StartsWith("http://") OrElse sourceUrl.ToLower().StartsWith("https://")) Then
                sUrl = "http://" & sourceUrl
            End If
        End If
        Return sUrl
    End Function
#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_FileRepository", "Modules", "FileRepository")
    End Sub
#End Region

End Class