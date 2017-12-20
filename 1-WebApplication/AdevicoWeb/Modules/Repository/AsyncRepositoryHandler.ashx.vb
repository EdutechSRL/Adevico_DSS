Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Threading
Imports System.Threading.Tasks
Imports lm.Comol.Core.FileRepository.Domain


Public Class AsyncRepositoryHandler
    Implements System.Web.IHttpAsyncHandler, IReadOnlySessionState

    Public Function BeginProcessRequest(context As HttpContext, cb As AsyncCallback, extraData As Object) As IAsyncResult Implements IHttpAsyncHandler.BeginProcessRequest
        Dim asynch As New AsynchOperation(cb, context, extraData)
        asynch.StartAsyncWork()
        Return asynch
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

#Region "Not Used"
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

    End Sub
    Public Sub EndProcessRequest(result As IAsyncResult) Implements IHttpAsyncHandler.EndProcessRequest

    End Sub
#End Region


End Class

Public Class AsynchOperation
    Implements IAsyncResult

#Region "Property"
    Private _service As lm.Comol.Core.FileRepository.Business.ServiceFileRepository
    Private _AppContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _CurrentImpersonation As lm.Comol.Core.File.Impersonate
    Private _completed As Boolean
    Private _state As Object
    Private _callback As AsyncCallback
    Private _context As HttpContext
    Public ReadOnly Property AsyncState As Object Implements IAsyncResult.AsyncState
        Get
            Return _state
        End Get
    End Property
    Public ReadOnly Property AsyncWaitHandle As WaitHandle Implements IAsyncResult.AsyncWaitHandle
        Get
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property CompletedSynchronously As Boolean Implements IAsyncResult.CompletedSynchronously
        Get
            Return False
        End Get
    End Property
    Public ReadOnly Property IsCompleted As Boolean Implements IAsyncResult.IsCompleted
        Get
            Return _completed
        End Get
    End Property
    Private ReadOnly Property Service As lm.Comol.Core.FileRepository.Business.ServiceFileRepository
        Get
            If IsNothing(_service) AndAlso Not IsNothing(_AppContext) Then
                _service = New lm.Comol.Core.FileRepository.Business.ServiceFileRepository(_AppContext)
            End If
            Return _service
        End Get
    End Property
    Private Property CurrentImpersonation As lm.Comol.Core.File.Impersonate
        Get
            If IsNothing(_CurrentImpersonation) Then
                _CurrentImpersonation = New lm.Comol.Core.File.Impersonate
            End If
            Return _CurrentImpersonation
        End Get
        Set(value As lm.Comol.Core.File.Impersonate)
            _CurrentImpersonation = value
        End Set
    End Property
#End Region
   
    Public Sub New(ByVal callback As AsyncCallback, ByVal context As HttpContext, ByVal state As [Object])
        _callback = callback
        _context = context
        _state = state
        _completed = False
        _AppContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
        DownloadItem()
    End Sub
    Public Sub StartAsyncWork()
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf StartAsyncTask), Nothing)
    End Sub
    Private Sub StartAsyncTask(workItemState As Object)
        '_context.Response.Write("<p>Completion IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");
        '_context.Response.Write("Hello World from Async Handler!");
        _completed = True
        _callback(Me)
    End Sub


#Region "Manage Download"
    Private Sub DownloadItem()
        Dim found As Boolean = False
        Dim foundGuidFile As Boolean = False
        Dim idFile As Long = 0, idVersion As Long = 0
        Dim idGuidFile As Guid = Guid.Empty
        _context.Response.BufferOutput = False
        _context.Response.ClearHeaders()
        _context.Response.ClearContent()

        Dim segments As List(Of String) = _context.Request.Url.PathAndQuery.Split("/").Reverse().ToList()
        If segments.Count > 3 Then
            If Not IsNumeric(segments(2)) Then
                Guid.TryParse(segments(2), idGuidFile)
            Else
                Long.TryParse(segments(2), idFile)
            End If
            Long.TryParse(segments(1), idVersion)
            If idFile > 0 Then
                found = True
            ElseIf idGuidFile <> Guid.Empty OrElse IsGuidInternalFile(_context) Then
                found = True
                foundGuidFile = True
                If idGuidFile = Guid.Empty Then
                    idGuidFile = GetGuidIdFile(_context)
                End If
            End If
        End If
        If found Then
            If foundGuidFile Then
            Else
                DownloadRepositoryItem(idFile, idVersion)
            End If
        Else
            Dim oError As New dtoDownloadError
            With oError
                .IdCurrentCommunity = _AppContext.UserContext.CurrentCommunityID
                .IdItem = idFile
                .IdVersion = idVersion
                .IdItemCommunity = 0
                .IdLink = GetLongFromQueryString(_context, QueryKeyNames.idLink, 0)
                .IdModule = GetLongFromQueryString(_context, QueryKeyNames.idModule, 0)
                .IdNews = GetGuidFromQueryString(_context, QueryKeyNames.NewsId, Guid.Empty)
                .InModalWindow = GetBooleanFromQueryString(_context, QueryKeyNames.onModalPage, False)
                .Type = DownloadErrorType.notExist
                .WorkingSession = GetGuidFromQueryString(_context, QueryKeyNames.wSessionId, Guid.Empty)
                .IdCurrentUser = _AppContext.UserContext.CurrentUserID
            End With
            SendToErrorPage(oError, _context.Request.Url.Query)
        End If
    End Sub

    Private Sub DownloadRepositoryItem(idItem As Long, idVersion As Long)
        Dim saveStatistics As Boolean = Not GetBooleanFromQueryString(_context, QueryKeyNames.notSaveStat, False)
        Dim errorItem As New dtoDownloadError With {.IdItem = idItem, .IdVersion = idVersion}
        Dim idUser As Integer = _AppContext.UserContext.CurrentUserID
        Dim idModule As Integer = GetLongFromQueryString(_context, QueryKeyNames.idModule, 0)
        Dim idLink As Long = GetLongFromQueryString(_context, QueryKeyNames.idLink, 0)
        With errorItem
            .IdCurrentCommunity = _AppContext.UserContext.CurrentCommunityID
            .IdItem = idItem
            .IdVersion = idVersion
            .IdItemCommunity = 0
            .IdLink = idLink
            .IdModule = idModule
            .IdNews = GetGuidFromQueryString(_context, QueryKeyNames.NewsId, Guid.Empty)
            .InModalWindow = GetBooleanFromQueryString(_context, QueryKeyNames.onModalPage, False)
            .Type = DownloadErrorType.notExist
            .WorkingSession = GetGuidFromQueryString(_context, QueryKeyNames.wSessionId, Guid.Empty)
            .IdCurrentUser = _AppContext.UserContext.CurrentUserID
        End With
        Dim item As liteRepositoryItem = Service.ItemGet(idItem)
        Dim itemVersion As liteRepositoryItemVersion = Service.ItemGetVersion(idItem, idVersion)
        Dim wSessionId As System.Guid = GetGuidFromQueryString(_context, QueryKeyNames.wSessionId, Guid.Empty)
        If (idUser = 0 AndAlso (wSessionId = _AppContext.UserContext.WorkSessionID)) Then
            idUser = Service.GetIdAnonymousUser()
        End If
        If IsNothing(item) OrElse IsNothing(itemVersion) Then
            errorItem.Type = DownloadErrorType.notExist
            If Not IsNothing(item) Then
                errorItem.IdItemCommunity = item.IdCommunity
            End If
            SendToErrorPage(errorItem, _context.Request.Url.Query)
        Else
            errorItem.IdItemCommunity = item.IdCommunity
            errorItem.Name = item.Name
            errorItem.Extension = item.Extension


            Dim moduleCode As String = ""
            If idLink > 0 Then
                moduleCode = Service.GetModuleCode(idModule)
            ElseIf idItem > 0 Then
                moduleCode = lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode
            End If

            If idUser = 0 AndAlso idLink = 0 Then
                errorItem.Type = DownloadErrorType.notLoggedIn
                SendToErrorPage(errorItem, _context.Request.Url.Query)
            Else
                If Not HasPermission(idUser, item, itemVersion, idLink, idModule, moduleCode) Then
                    If idUser = 0 Then
                        errorItem.Type = DownloadErrorType.notLoggedIn
                    Else
                        SetNewsRead(idUser)
                        errorItem.Type = DownloadErrorType.noPermissions
                    End If
                    SendToErrorPage(errorItem, _context.Request.Url.Query)
                Else
                    Dim filePath As String = GetRepositoryDiskPath()
                    If Not filePath.EndsWith("\") Then
                        filePath &= "\"
                    End If
                    If filePath.Contains("\\\\") Then
                        filePath = Replace(filePath, "\\\\", "\\")
                    End If
                    filePath &= item.IdCommunity.ToString() & "\"
                    Dim fileName As String = filePath & itemVersion.UniqueIdVersion.ToString() & item.Extension
                    Dim allowDownload As Boolean = False
                    Try

                        If CurrentImpersonation.ImpersonateValidUser() = lm.Comol.Core.File.FileMessage.ImpersonationFailed Then
                            errorItem.Type = DownloadErrorType.impersonationFailed
                            SendToErrorPage(errorItem, _context.Request.Url.Query)
                        Else
                            If lm.Comol.Core.File.Exists.File(fileName) Then
                                If idUser > 0 Then
                                    SetNewsRead(idUser)
                                End If

                                If saveStatistics Then
                                    Service.StatisticsAddDownload(idUser, item.Repository, idItem, item.UniqueId, itemVersion.Id, itemVersion.UniqueIdVersion, item.Type, errorItem.IdCurrentCommunity)
                                End If
                                allowDownload = True
                            Else
                                DeleteNews()
                                errorItem.Type = DownloadErrorType.notExist
                                SendToErrorPage(errorItem, _context.Request.Url.Query)
                            End If
                        End If
                        '  CurrentImpersonation.UndoImpersonation()
                    Catch ex As Exception
                        CurrentImpersonation.UndoImpersonation()
                    Finally
                        CurrentImpersonation.UndoImpersonation()
                        CurrentImpersonation = Nothing
                    End Try
                    If allowDownload Then
                        CurrentImpersonation = New lm.Comol.Core.File.Impersonate
                        If CurrentImpersonation.ImpersonateValidUser() <> lm.Comol.Core.File.FileMessage.ImpersonationFailed Then
                            DownloadRepositoryItem(idUser, fileName, item, idLink, _context.Request.Url.Query, saveStatistics)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

#Region "Get identifiers"
    Private Function IsGuidInternalFile(ByVal context As HttpContext) As Boolean
        If GetGuidIdFile(context) <> System.Guid.Empty Then
            Return True
        Else
            Return False
        End If
    End Function
    Private ReadOnly Property GetGuidIdFile(ByVal context As HttpContext) As System.Guid
        Get
            Dim idFile As System.Guid = Guid.Empty
            If Not String.IsNullOrEmpty(context.Request.QueryString("InternalFileID")) Then
                Try
                    idFile = New System.Guid(context.Request.QueryString("InternalFileID"))
                Catch ex As Exception

                End Try
            End If
            Return idFile
        End Get
    End Property
    Private ReadOnly Property GetLongFromQueryString(ByVal context As HttpContext, key As QueryKeyNames, dValue As Long) As Long
        Get
            Dim idItem As Long = dValue
            If Not String.IsNullOrEmpty(context.Request.QueryString(key.ToString)) Then
                Long.TryParse(context.Request.QueryString(key.ToString), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property GetGuidFromQueryString(ByVal context As HttpContext, key As QueryKeyNames, dValue As Guid) As Guid
        Get
            Dim idItem As Guid = dValue
            If Not String.IsNullOrEmpty(context.Request.QueryString(key.ToString)) Then
                Guid.TryParse(context.Request.QueryString(key.ToString), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property GetBooleanFromQueryString(ByVal context As HttpContext, key As QueryKeyNames, dValue As Boolean) As Boolean
        Get
            Dim result As Boolean = dValue
            If Not String.IsNullOrEmpty(context.Request.QueryString(key.ToString)) Then
                Boolean.TryParse(context.Request.QueryString(key.ToString), result)
            End If
            Return result
        End Get
    End Property
    Private Function GetExternalUsers() As Dictionary(Of String, Long)
        Dim result As New Dictionary(Of String, Long)
        If Not IsNothing(_context) AndAlso Not IsNothing(_context.Session) Then
            If (From k In _context.Session.Keys Where k = "TICKET.CurrentExtUser" Select k).Any() Then
                Dim tUser As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User = Nothing
                Try
                    tUser = DirectCast(_context.Session("TICKET.CurrentExtUser"), lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User)
                Catch ex As Exception
                End Try
                If Not IsNothing(tUser) Then
                    result.Add(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode, tUser.UserId)
                End If
            End If
        End If
        Return result
    End Function
#End Region

#Region "File Download"
    Private _RepositoryDiskPath As String
    Private Function GetRepositoryDiskPath() As String
        If String.IsNullOrEmpty(_RepositoryDiskPath) Then
            Dim pageUtility As New OLDpageUtility(_context)
            If Not String.IsNullOrEmpty(pageUtility.SystemSettings.File.Materiale.DrivePath) Then
                _RepositoryDiskPath = pageUtility.SystemSettings.File.Materiale.DrivePath
            Else
                _RepositoryDiskPath = _context.Server.MapPath(pageUtility.BaseUrl & pageUtility.SystemSettings.File.Materiale.VirtualPath)
            End If
        End If
        Return _RepositoryDiskPath
    End Function

    ''' <summary>
    ''' Fill HttpResponse with valid
    ''' </summary>
    ''' <param name="idUser"></param>
    ''' <param name="fileName"></param>
    ''' <param name="item"></param>
    ''' <param name="idLink"></param>
    ''' <param name="query"></param>
    ''' <remarks></remarks>
    Private Sub DownloadRepositoryItem(idUser As Integer, fileName As String, item As liteRepositoryItem, idLink As Long, query As String, saveStatistics As Boolean)
        If Not String.IsNullOrWhiteSpace(fileName) Then
            Dim Quote As String = """"
            Dim fInfo As System.IO.FileInfo = lm.Comol.Core.File.ContentOf.File_Info(fileName)
            Dim directWrite As Boolean = False
            Dim downloadInline As Boolean = False
            If Not String.IsNullOrEmpty(query) Then
                If query.StartsWith("?") Then
                    query = query.Remove(0, 1)
                End If
                Dim cookie = New HttpCookie("fileDownload", query)
                cookie.Expires = Now.AddMinutes(5)
                _context.Response.AppendCookie(cookie)
            End If
            Dim ext As String = item.Extension
            Dim contentType As String = item.ContentType
            If Not String.IsNullOrEmpty(ext) Then
                ext = ext.Trim().ToLower
            End If

            '   Case ".jpg"
            '_HttpContext.Response.ContentType = String.Format("image/jpeg;name={0}{1}{0}", Quote, ItemName & ItemExtension)
            '        Case ".gif", ".png", ".bmp"
            '_HttpContext.Response.ContentType = String.Format("image/{0};name={1}{2}{1}", ItemExtension.TrimStart("."), Quote, ItemName & ItemExtension)
            '        Case ".tif"
            '_HttpContext.Response.ContentType = String.Format("image/tiff;name={0}{1}{0}", Quote, ItemName & ItemExtension)


            If String.IsNullOrEmpty(contentType) Then
                If Not String.IsNullOrEmpty(ext) Then
                    Dim oldPageUtility As New OLDpageUtility(_context)
                    contentType = oldPageUtility.SystemSettings.Extension.FindMimeType(ext)
                Else
                    contentType = "application/octet-stream"
                End If
            End If

            Try
                Dim pageUtility As New OLDpageUtility(_context)
                _context.Response.AddHeader("Content-Disposition", "attachment; filename=" + """" + RecodeFileName(item.DownloadFullName) + """")

                Dim cookie As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl = pageUtility.ReadLogoutAccessCookie()
                If Not IsNothing(cookie) Then
                    _context.Response.AddHeader("Refresh", "1;URL=" & pageUtility.BaseUrl & lm.Comol.Core.Dashboard.Domain.RootObject.AutoLogonToCommunity(cookie.IdCommunity))
                End If
                _context.Response.AddHeader("Content-Length", fInfo.Length.ToString())
                _context.Response.ContentType = contentType

                If idLink > 0 AndAlso Service.ModuleLinkIsAutoEvaluable(idLink) AndAlso saveStatistics Then
                    Dim oSender As PermissionService.IServicePermission = Nothing
                    Try
                        oSender = New PermissionService.ServicePermissionClient
                        If Not IsNothing(oSender) Then
                            oSender.ExecutedActionForExternal(idLink, True, True, 100, True, 100, idUser, GetExternalUsers(), Nothing)
                            Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                            service.Close()
                            service = Nothing
                        End If
                    Catch ex As Exception
                        If Not IsNothing(oSender) Then
                            Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                            service.Abort()
                            service = Nothing
                        End If
                    End Try
                End If

                _context.Response.TransmitFile(fInfo.FullName)
                _context.Response.Flush()
            Catch ex As Exception
            Finally
                CurrentImpersonation.UndoImpersonation()
                CurrentImpersonation = Nothing
            End Try

            _context.ApplicationInstance.CompleteRequest()
        End If
    End Sub
    Private Function RecodeFileName(ByVal name As String) As String
        Dim result As String = ""
        Dim oEncodeChar As New EncodeChar

        For i As Integer = 0 To name.Length - 1
            If oEncodeChar.Contains(name.Chars(i)) Then
                result &= oEncodeChar.Recode(name.Chars(i))
            Else
                result &= name.Chars(i).ToString
            End If
        Next
        Return result
    End Function
#End Region

#Region "Permissions"
    Private Function HasPermission(ByVal idUser As Integer, item As liteRepositoryItem, itemVersion As liteRepositoryItemVersion, idLink As Long, idModule As Integer, moduleCode As String) As Boolean
        Dim iResponse As Boolean = False
        Dim ItemPermission As Boolean = False

        If idLink > 0 Then
            Dim oSender As PermissionService.IServicePermission = Nothing
            Try
                oSender = New PermissionService.ServicePermissionClient
                Dim permission As Integer = 0 'oSender.ModuleLinkActionPermission(LinkID, UCServices.Services_File.ActionType.DownloadFile, UserID, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(oFile.Id, oFile, IIf(oFile.isSCORM, UCServices.Services_File.ObjectType.FileScorm, UCServices.Services_File.ObjectType.File), CommunityID, UCServices.Services_File.Codex))
                If item.Type = ItemType.File Then
                    permission = oSender.ModuleLinkActionPermission(idLink, ModuleRepository.ActionType.DownloadFile, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(item.Id, itemVersion.Id, item, IIf(item.Type = ItemType.ScormPackage, ModuleRepository.ObjectType.ScormPackage, ModuleRepository.ObjectType.File), item.IdCommunity, ModuleRepository.UniqueCode), idUser, GetExternalUsers(), Nothing)
                Else
                    permission = oSender.ModuleLinkActionPermission(idLink, ModuleRepository.ActionType.PlayFile, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(item.Id, itemVersion.Id, item, IIf(item.Type = ItemType.ScormPackage, ModuleRepository.ObjectType.ScormPackage, ModuleRepository.ObjectType.File), item.IdCommunity, ModuleRepository.UniqueCode), idUser, GetExternalUsers(), Nothing)
                End If

                iResponse = (ModuleRepository.Base2Permission.DownloadOrPlay And permission)
                If Not IsNothing(oSender) Then
                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                    service.Close()
                    service = Nothing
                End If
            Catch ex As Exception
                If Not IsNothing(oSender) Then
                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                    service.Abort()
                    service = Nothing
                End If
            End Try
        ElseIf idModule = 0 OrElse moduleCode = lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode Then
            iResponse = Service.HasPermissionToSeeItem(idUser, item, itemVersion, ModuleRepository.ActionType.DownloadFile)
        Else
            iResponse = True
        End If

        Return iResponse
    End Function
#End Region

#Region "Manage News"
    Private Sub SetNewsRead(ByVal idUser As Integer)
        Dim idNews As System.Guid = GetGuidFromQueryString(_context, QueryKeyNames.NewsId, Guid.Empty)
        If idNews <> System.Guid.Empty Then
            Dim oSender As NotificationService.iNotificationServiceClient = Nothing
            Try
                oSender = New NotificationService.iNotificationServiceClient
                If Not IsNothing(oSender) Then
                    oSender.ReadNotification(idNews, idUser)
                    oSender.Close()
                    oSender = Nothing
                End If
            Catch ex As Exception
                If Not IsNothing(oSender) Then
                    oSender.Abort()
                    oSender = Nothing
                End If
            End Try
        End If
    End Sub
    Private Sub DeleteNews()
        Dim idNews As System.Guid = GetGuidFromQueryString(_context, QueryKeyNames.NewsId, Guid.Empty)
        If idNews <> System.Guid.Empty Then
            Dim oSender As NotificationService.iNotificationServiceClient = Nothing
            Try
                oSender = New NotificationService.iNotificationServiceClient
                If Not IsNothing(oSender) Then
                    oSender.RemoveNotification(idNews)
                    oSender.Close()
                    oSender = Nothing
                End If
            Catch ex As Exception
                If Not IsNothing(oSender) Then
                    oSender.Abort()
                    oSender = Nothing
                End If
            End Try
        End If
    End Sub
#End Region

#Region "Manage Errors"
    Private Sub SendToErrorPage(ByVal errItem As dtoDownloadError, ByVal query As String)
        Dim pageUtility As New OLDpageUtility(_context)
        Dim settings As ComolSettings = pageUtility.SystemSettings
        If settings.NotificationErrorService.isSendingEnabled(ErrorsNotificationService.ErrorType.FileError) Then
            Dim oService As New ErrorsNotificationService.iErrorsNotificationServiceClient
            Dim oErrorMessage As New ErrorsNotificationService.FileError

            With oErrorMessage
                .ComolUniqueID = settings.NotificationErrorService.ComolUniqueID
                .UniqueID = Guid.NewGuid
                .Type = ErrorsNotificationService.ErrorType.FileError
                .Persist = settings.NotificationErrorService.FindPersistTo(ErrorsNotificationService.ErrorType.FileError)
                .SentDate = Now
                .Day = .SentDate.Date
                .Message = errItem.Type.ToString & " " & errItem.FullName
                .CommunityID = errItem.IdCurrentCommunity
                .UserID = errItem.IdCurrentUser
            End With
            oService.sendFileError(oErrorMessage)
        End If

        If errItem.Type = DownloadErrorType.notLoggedIn Then
            Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(pageUtility.GetDefaultLogoutPage)
            Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow
            dto.DestinationUrl = RootObject.Download(pageUtility.BaseUrl, errItem.IdItem, errItem.IdVersion, errItem.FullName, errItem.WorkingSession, errItem.IdNews, errItem.IdModule, errItem.IdLink)
            dto.Preserve = True
            dto.IsForDownload = True
            dto.PreviousUrl = ""
            If errItem.IdItemCommunity > 0 Then
                dto.IdCommunity = errItem.IdItemCommunity
            ElseIf errItem.IdCurrentCommunity > 0 Then
                dto.IdCommunity = errItem.IdCurrentCommunity
            End If
            If Not IsNothing(_context.Request.UrlReferrer) Then
                If _context.Request.UrlReferrer.AbsoluteUri.StartsWith(pageUtility.ApplicationUrlBase(True)) OrElse _context.Request.UrlReferrer.AbsoluteUri.StartsWith(pageUtility.ApplicationUrlBase()) Then
                    dto.PreviousUrl = Replace(_context.Request.UrlReferrer.AbsoluteUri, pageUtility.ApplicationUrlBase(True), "")
                    dto.PreviousUrl = Replace(dto.PreviousUrl, pageUtility.ApplicationUrlBase(False), "")
                    Dim qKeys As List(Of String) = _context.Request.UrlReferrer.Query.Split("&").ToList()
                    If qKeys.Any(Function(f) f.Contains("idCommunity=")) Then
                        Dim key As String = qKeys.Where(Function(f) f.Contains("idCommunity=")).FirstOrDefault()
                        Dim value As String = Replace(key, "idCommunity=", "")
                        If IsNumeric(value) Then
                            dto.IdCommunity = CInt(value)
                        End If
                    End If
                End If
            End If
           
            webPost.Redirect(dto)
        Else
            pageUtility.RedirectToUrl(RootObject.DownloadError(errItem.Type, True, errItem.InModalWindow, errItem.IdItem, errItem.IdVersion, errItem.FullName, errItem.WorkingSession, errItem.IdNews, errItem.IdModule, errItem.IdLink))
        End If
        'Dim PreserveUrl As Boolean = (errItem.Type = DownloadErrorType.notLoggedIn)
        'Dim oRemotePost As New RemotePost
        'oRemotePost.Url = oError.BaseUrl & "Modules/Repository/CommunityRepositoryItemError.aspx?PageSender=" & RepositoryPage.DownloadingPage.ToString & "&PreserveDownloadUrl=" & PreserveUrl.ToString

        'oRemotePost.Add("FILE_C_Name_0", oError.FileName)
        'oRemotePost.Add("FILE_C_isFile_0", "True")
        'oRemotePost.Add("FILE_C_Status_0", oError.ErrorType.ToString)
        'oRemotePost.Add("FILE_C_FolderId_0", oError.FolderID)
        'oRemotePost.Add("FILE_C_SavedFilePath_0", "")
        'oRemotePost.Add("FILE_C_SavedName_0", "")
        'oRemotePost.Add("FILE_C_SavedExtension_0", oError.Extension)

        'oRemotePost.Add("ForUserID", oError.ForUserID.ToString)
        'oRemotePost.Add("PreloadedItemID", oError.FileID)
        'oRemotePost.Add("FolderId", oError.FolderID)
        'oRemotePost.Add("Language", oError.Language)
        'oRemotePost.Add("CommunityID", oError.CommunityID)
        'oRemotePost.Post(query)
    End Sub
    'Public Sub SendToErrorPage(ByVal oError As dtoInternalError)
    '    If oError.Settings.isSendingEnabled(ErrorsNotificationService.ErrorType.FileError) Then
    '        Dim oService As New ErrorsNotificationService.iErrorsNotificationServiceClient
    '        Dim oErrorMessage As New ErrorsNotificationService.FileError

    '        With oErrorMessage
    '            .ComolUniqueID = oError.Settings.ComolUniqueID
    '            .UniqueID = Guid.NewGuid
    '            .Type = ErrorsNotificationService.ErrorType.FileError
    '            .Persist = oError.Settings.FindPersistTo(ErrorsNotificationService.ErrorType.FileError)
    '            .SentDate = Now
    '            .Day = .SentDate.Date
    '            .Message = oError.ToString & " " & oError.FileName
    '            .CommunityID = oError.CommunityId
    '            .UserID = oError.UserID
    '        End With
    '        oService.sendFileError(oErrorMessage)
    '    End If

    '    oError.ErrorType = ItemRepositoryStatus.NotLoggedIn

    '    Dim oRemotePost As New RemotePost
    '    oRemotePost.Url = oError.BaseUrl & lm.Comol.Core.BaseModules.Errors.Domain.RootObject.Default(oError.ErrorType <> ItemRepositoryStatus.NotLoggedIn, False, False, False, True)
    '    oRemotePost.Add("ForUserID", oError.ForUserID.ToString)
    '    oRemotePost.Add("Language", oError.Language)
    '    oRemotePost.Add("CommunityID", oError.CommunityId)
    '    oRemotePost.Post()
    'End Sub
#End Region

#End Region

    'Public Sub DefineInternalFileToDownload(ByVal oHandler As HTTPhandler_Utility)
    '    Dim oMessage As String = ""
    '    Dim InternalFileID As Guid = BaseFileID(oHandler)
    '    Dim FileExist As Boolean = False

    '    Dim NomeFile As String = ""

    '    Dim oError As New dtoInternalError
    '    With oError
    '        .BaseUrl = oHandler.BaseUrl
    '        .CommunityId = oHandler.CurrentCommunityID
    '        .ErrorType = ItemRepositoryStatus.NotLoggedIn
    '        .FileID = InternalFileID
    '        If IsNumeric(oHandler.HttpContext.Request.QueryString("ForUserID")) Then
    '            .ForUserID = oHandler.HttpContext.Request.QueryString("ForUserID")
    '        Else
    '            .ForUserID = 0
    '        End If
    '        .Language = oHandler.HttpContext.Request.QueryString("Language")
    '        .FileName = ""
    '        .Settings = oHandler.SystemSettings.NotificationErrorService
    '        .UserID = oHandler.CurrentUserID
    '        .FileSettings = oHandler.SystemSettings.File
    '    End With
    '    Dim oFile As lm.Comol.Core.DomainModel.BaseFile = oHandler.ManagerInternalFiles.GetBaseFile(InternalFileID)
    '    If oFile Is Nothing Then
    '        oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
    '        oHandler.SendToErrorPage(oError)
    '    Else
    '        oError.CommunityId = oHandler.CurrentCommunityID
    '        oError.FileID = oFile.Id
    '        oError.FileName = oFile.DisplayName
    '        oError.Extension = oFile.Extension
    '        If oError.UserID = 0 Then
    '            oError.ErrorType = ItemRepositoryStatus.NotLoggedIn
    '            oHandler.SendToErrorPage(oError)
    '        Else
    '            Dim CommunityPath As String = ""
    '            Dim PageUtility As New OLDpageUtility(oHandler.HttpContext)
    '            CommunityPath = PageUtility.BaseUserRepositoryPath

    '            Dim FileNamePath As String = CommunityPath
    '            FileNamePath &= oFile.Id.ToString & ".stored"
    '            If File.Exists.File(FileNamePath) Then
    '                oHandler.DownloadFile(0, FileNamePath, oFile, oHandler.HttpContext.Request.Url.AbsoluteUri)
    '            Else
    '                oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
    '                oHandler.SendToErrorPage(oError)
    '            End If
    '        End If
    '    End If
    'End Sub

End Class

Public Class EncodeChar
    Private _Hash As New Hashtable

    Public Function Contains(ByVal Carattere As String) As Boolean
        Return _Hash.ContainsKey(Carattere)
    End Function

    Public Function Recode(ByVal Carattere As String) As String
        Return _Hash.Item(Carattere).ToString
    End Function

    Sub New()
        _Hash.Add("–", "-")
        _Hash.Add("—", "-")
        _Hash.Add("«", "-")
        _Hash.Add("»", "-")
        _Hash.Add("£", "L")
        _Hash.Add("©", "c")
        _Hash.Add("®", "r")
        _Hash.Add("°", "_")
        _Hash.Add("µ", "u")
        _Hash.Add("·", "_")
        _Hash.Add("†", "_")
        _Hash.Add("•", "_")
        _Hash.Add("€", "E")
        _Hash.Add("ª", "a")
        _Hash.Add("á", "a")
        _Hash.Add("Á", "a")
        _Hash.Add("à", "a")
        _Hash.Add("À", "a")
        _Hash.Add("â", "a")
        _Hash.Add("Â", "a")
        _Hash.Add("ä", "a")
        _Hash.Add("Ä", "a")
        _Hash.Add("ă", "a")
        _Hash.Add("Ă", "a")
        _Hash.Add("ā", "a")
        _Hash.Add("Ā", "a")
        _Hash.Add("ã", "a")
        _Hash.Add("Ã", "a")
        _Hash.Add("å", "a")
        _Hash.Add("Å", "a")
        _Hash.Add("ą", "a")
        _Hash.Add("Ą", "a")
        _Hash.Add("æ", "ae")
        _Hash.Add("Æ", "AE")
        _Hash.Add("ć", "c")
        _Hash.Add("Ć", "C")
        _Hash.Add("ĉ", "c")
        _Hash.Add("Ĉ", "C")
        _Hash.Add("č", "c")
        _Hash.Add("Č", "C")
        _Hash.Add("ç", "c")
        _Hash.Add("Ç", "C")
        _Hash.Add("ď", "d")
        _Hash.Add("Ď", "D")
        _Hash.Add("đ", "d")
        _Hash.Add("Đ", "D")
        _Hash.Add("ð", "a")
        _Hash.Add("Ð", "D")
        _Hash.Add("é", "e")
        _Hash.Add("É", "e")
        _Hash.Add("è", "e")
        _Hash.Add("È", "E")
        _Hash.Add("ê", "e")
        _Hash.Add("Ê", "E")
        _Hash.Add("ë", "e")
        _Hash.Add("Ë", "E")
        _Hash.Add("ě", "e")
        _Hash.Add("Ě", "E")
        _Hash.Add("ē", "e")
        _Hash.Add("Ē", "E")
        _Hash.Add("ę", "e")
        _Hash.Add("Ę", "E")
        _Hash.Add("ĝ", "g")
        _Hash.Add("Ĝ", "G")
        _Hash.Add("ğ", "g")
        _Hash.Add("Ğ", "G")
        _Hash.Add("ģ", "g")
        _Hash.Add("Ģ", "G")
        _Hash.Add("ĥ", "h")
        _Hash.Add("Ĥ", "H")
        _Hash.Add("ı", "i")
        _Hash.Add("í", "i")
        _Hash.Add("Í", "I")
        _Hash.Add("ì", "i")
        _Hash.Add("Ì", "I")
        _Hash.Add("İ", "I")
        _Hash.Add("î", "i")
        _Hash.Add("Î", "I")
        _Hash.Add("ï", "i")
        _Hash.Add("Ï", "I")
        _Hash.Add("ī", "i")
        _Hash.Add("Ī", "I")
        _Hash.Add("ĵ", "j")
        _Hash.Add("Ĵ", "J")
        _Hash.Add("ķ", "k")
        _Hash.Add("Ķ", "K")
        _Hash.Add("ĺ", "i")
        _Hash.Add("Ĺ", "L")
        _Hash.Add("ľ", "l")
        _Hash.Add("Ľ", "L")
        _Hash.Add("ļ", "l")
        _Hash.Add("Ļ", "L")
        _Hash.Add("ł", "l")
        _Hash.Add("Ł", "L")
        _Hash.Add("ń", "n")
        _Hash.Add("Ń", "N")
        _Hash.Add("ň", "n")
        _Hash.Add("Ň", "N")
        _Hash.Add("ņ", "n")
        _Hash.Add("Ņ", "N")
        _Hash.Add("№", "n")
        _Hash.Add("º", "o")
        _Hash.Add("ó", "o")
        _Hash.Add("Ó", "O")
        _Hash.Add("ò", "o")
        _Hash.Add("Ò", "O")
        _Hash.Add("ô", "o")
        _Hash.Add("Ô", "O")
        _Hash.Add("ö", "o")
        _Hash.Add("Ö", "O")
        _Hash.Add("õ", "o")
        _Hash.Add("Õ", "O")
        _Hash.Add("ő", "o")
        _Hash.Add("Ő", "O")
        _Hash.Add("ø", "o")
        _Hash.Add("Ø", "O")
        _Hash.Add("œ", "ce")
        _Hash.Add("Œ", "CE")
        _Hash.Add("ŕ", "r")
        _Hash.Add("Ŕ", "R")
        _Hash.Add("ř", "r")
        _Hash.Add("Ř", "R")
        _Hash.Add("ŗ", "r")
        _Hash.Add("Ŗ", "R")
        _Hash.Add("ś", "s")
        _Hash.Add("Ś", "S")
        _Hash.Add("ŝ", "s")
        _Hash.Add("Ŝ", "S")
        _Hash.Add("š", "s")
        _Hash.Add("Š", "S")
        _Hash.Add("ş", "s")
        _Hash.Add("Ş", "S")
        _Hash.Add("ß", "b")
        _Hash.Add("ť", "t")
        _Hash.Add("Ť", "T")
        _Hash.Add("ţ", "t")
        _Hash.Add("Ţ", "T")
        _Hash.Add("þ", "_")
        _Hash.Add("Þ", "_")
        _Hash.Add("ú", "u")
        _Hash.Add("Ú", "U")
        _Hash.Add("ù", "u")
        _Hash.Add("Ù", "U")
        _Hash.Add("û", "u")
        _Hash.Add("Û", "U")
        _Hash.Add("ü", "u")
        _Hash.Add("Ü", "U")
        _Hash.Add("ŭ", "u")
        _Hash.Add("Ŭ", "U")
        _Hash.Add("ū", "u")
        _Hash.Add("Ū", "U")
        _Hash.Add("ů", "u")
        _Hash.Add("Ů", "U")
        _Hash.Add("ű", "u")
        _Hash.Add("Ű", "U")
        _Hash.Add("ý", "y")
        _Hash.Add("Ý", "Y")
        _Hash.Add("ÿ", "y")
        _Hash.Add("Ÿ", "Y")
        _Hash.Add("ź", "z")
        _Hash.Add("Ź", "Z")
        _Hash.Add("ż", "z")
        _Hash.Add("Ż", "Z")
        _Hash.Add("ž", "z")
        _Hash.Add("Ž", "Z")
    End Sub
End Class