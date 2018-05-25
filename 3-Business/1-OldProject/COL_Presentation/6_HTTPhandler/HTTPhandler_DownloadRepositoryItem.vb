Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel
Imports System.ServiceModel
Imports lm.Comol.Core
Imports System.Threading

Public Class HTTPhandler_DownloadRepositoryItem
    Implements System.Web.IHttpHandler
    Implements System.Web.SessionState.IRequiresSessionState
    Implements iHTTPhandler_comunita
    Implements IDisposable
    ' Implements IHttpAsyncHandler




    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        Dim oHTTPhandler As New HTTPhandler_Utility(context, ConfigFileType.File)
        oHTTPhandler.ClearHTTPcontext()
        If Me.isQueryStringCorrect(oHTTPhandler) Then
            Me.DefineFileToDownload(oHTTPhandler)
        ElseIf Me.isBaseFileRequest(oHTTPhandler) Then
            Me.DefineInternalFileToDownload(oHTTPhandler)
        Else
            Dim oError As New dtoError
            With oError
                .BaseUrl = oHTTPhandler.BaseUrl
                .CommunityID = oHTTPhandler.CurrentCommunityID
                .ErrorType = ItemRepositoryStatus.NoItemSpecified
                If IsNumeric(oHTTPhandler.HttpContext.Request.QueryString("ForUserID")) Then
                    .ForUserID = oHTTPhandler.HttpContext.Request.QueryString("ForUserID")
                Else
                    .ForUserID = 0
                End If
                .Language = oHTTPhandler.HttpContext.Request.QueryString("Language")
                .FileID = 0
                .FolderID = 0
                .FileName = ""
                .Settings = oHTTPhandler.SystemSettings.NotificationErrorService
                .UserID = oHTTPhandler.CurrentUserID
                .FileSettings = oHTTPhandler.SystemSettings.File
            End With
            oHTTPhandler.SendToErrorPage(oError, oHTTPhandler.HttpContext.Request.Url.Query)
        End If
    End Sub

    Public Sub DefineFileToDownload(ByVal oHandler As HTTPhandler_Utility) Implements iHTTPhandler_comunita.DefineFileToDownload
        Dim oMessage As String = ""
        Dim FileID As Long = 0
        Dim FileExist As Boolean = False
        Try
            FileID = oHandler.HttpContext.Request.QueryString("FileID")
        Catch ex As Exception
        End Try

        Dim NomeFile As String = ""

        Dim oError As New dtoError
        With oError
            .BaseUrl = oHandler.BaseUrl
            .CommunityID = oHandler.CurrentCommunityID
            .ErrorType = ItemRepositoryStatus.NotLoggedIn
            .FileID = FileID
            If IsNumeric(oHandler.HttpContext.Request.QueryString("ForUserID")) Then
                .ForUserID = oHandler.HttpContext.Request.QueryString("ForUserID")
            Else
                .ForUserID = 0
            End If
            .Language = oHandler.HttpContext.Request.QueryString("Language")
            .FolderID = 0
            .FileName = ""
            .Settings = oHandler.SystemSettings.NotificationErrorService
            .IsOnModal = False
            Boolean.TryParse(oHandler.HttpContext.Request.QueryString("onModalPage"), .IsOnModal)
            Dim wSessionId As System.Guid
            Try
                wSessionId = New Guid(oHandler.HttpContext.Request.QueryString("wSessionId"))
            Catch ex As Exception
                wSessionId = Guid.Empty
            End Try
            .UserID = oHandler.CurrentUserID
            If (.UserID = 0 AndAlso (wSessionId = oHandler.CurrentContext.UserContext.WorkSessionID)) Then
                .UserID = oHandler.CurrentManager.GetAnonymousIdUser()
            End If

            .FileSettings = oHandler.SystemSettings.File
        End With
        Dim oFile As lm.Comol.Core.DomainModel.BaseCommunityFile = oHandler.CurrentManager.GetItem(FileID)
        If oFile Is Nothing Then
            oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
            oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
        Else
            If IsNothing(oFile.CommunityOwner) Then
                oError.CommunityID = 0
            Else
                oError.CommunityID = oFile.CommunityOwner.Id
            End If
            oError.FolderID = oFile.FolderId
            oError.FileID = oFile.Id
            oError.FileName = oFile.DisplayName
            oError.Extension = oFile.Extension

            Dim idModule As Integer = 0
            Dim idLink As Long = 0
            Dim ModuleCode As String = ""
            If IsNumeric(oHandler.HttpContext.Request.QueryString("LinkID")) Then
                idLink = CLng(oHandler.HttpContext.Request.QueryString("LinkID"))
            End If
            If idLink > 0 Then
                If IsNumeric(oHandler.HttpContext.Request.QueryString("ModuleID")) Then
                    idModule = CInt(oHandler.HttpContext.Request.QueryString("ModuleID"))
                End If
                ModuleCode = oHandler.CommonManager.GetModuleCode(idModule)
            End If

            If oError.UserID = 0 AndAlso idLink = 0 Then
                oError.ErrorType = ItemRepositoryStatus.NotLoggedIn
                oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
            Else
                If Not HasPermission(oHandler, oFile, oError.CommunityID, oError.UserID) Then
                    If oError.UserID = 0 Then
                        oError.ErrorType = ItemRepositoryStatus.NotLoggedIn
                    Else
                        Me.SendNewsRead(oHandler, oError.UserID)
                        oError.ErrorType = ItemRepositoryStatus.NoPermissionToSeeItem
                    End If

                    oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
                Else
                    Dim CommunityPath As String = ""
                    If oHandler.SystemSettings.File.Materiale.DrivePath = "" Then
                        CommunityPath = oHandler.HttpContext.Server.MapPath(oError.BaseUrl & oHandler.SystemSettings.File.Materiale.VirtualPath) & "\" & oError.CommunityID & "\"
                    Else
                        CommunityPath = oHandler.SystemSettings.File.Materiale.DrivePath & "\" & oError.CommunityID & "\"
                    End If

                    Dim oImpersonate As New lm.Comol.Core.File.Impersonate
                    Dim FileNamePath As String = Replace(CommunityPath, "\\", "\")
                    Dim Link As lm.Comol.Core.DomainModel.ModuleLink = Nothing
                    Dim allowDownload As Boolean = False
                    Try

                        FileNamePath &= oFile.UniqueID.ToString & ".stored"
                        If oImpersonate.ImpersonateValidUser() = ItemRepositoryStatus.ImpersonationFailed Then
                            oError.ErrorType = ItemRepositoryStatus.ImpersonationFailed
                            oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
                        Else
                            If File.Exists.File(FileNamePath) Then
                                If oError.UserID > 0 Then
                                    Me.SendNewsRead(oHandler, oError.UserID)
                                End If
                                Dim LinkID As Long = 0
                                Dim ServiceCode As String = Services_File.Codex
                                If IsNumeric(oHandler.HttpContext.Request.QueryString("LinkID")) Then
                                    LinkID = CLng(oHandler.HttpContext.Request.QueryString("LinkID"))
                                End If

                                If LinkID > 0 Then
                                    Link = oHandler.CommonManager.GetModuleLink(LinkID)
                                    ServiceCode = Link.SourceItem.ServiceCode
                                End If

                                Dim s As String = oHandler.HttpContext.Request.QueryString("notSaveStat")
                                Dim stat As Boolean = True
                                If Not String.IsNullOrEmpty(s) Then
                                    s = s.ToLower
                                    stat = (s <> "true")
                                End If

                                If stat Then
                                    oHandler.CurrentManager.AddDownloadToFile(oFile, oError.UserID, Link, ServiceCode)
                                End If

                                allowDownload = True
                            Else
                                Me.DeleteNews(oHandler)
                                oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
                                oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
                            End If
                        End If
                        oImpersonate.UndoImpersonation()
                    Catch ex As Exception
                        oImpersonate.UndoImpersonation()
                    Finally
                        oImpersonate.UndoImpersonation()
                        oImpersonate = Nothing
                    End Try
                    If allowDownload Then
                        Dim notSaveExecution As Boolean = False
                        Boolean.TryParse(oHandler.HttpContext.Request.QueryString("notSaveStat"), notSaveExecution)
                        oImpersonate = New lm.Comol.Core.File.Impersonate
                        If oImpersonate.ImpersonateValidUser() <> ItemRepositoryStatus.ImpersonationFailed Then
                            oHandler.DownloadRepositoryFile(oImpersonate, Link, Not notSaveExecution, oError.UserID, FileNamePath, oFile.DisplayName, oFile.Extension, oFile.ContentType, oHandler.HttpContext.Request.Url.Query)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function HasPermission(ByVal oHandler As HTTPhandler_Utility, ByVal oFile As lm.Comol.Core.DomainModel.BaseCommunityFile, ByVal CommunityID As Integer, ByVal UserID As Integer) As Boolean
        Dim iResponse As Boolean = False
        Dim ModuleID As Integer = 0
        Dim LinkID As Long = 0
        If IsNumeric(oHandler.HttpContext.Request.QueryString("ModuleID")) Then
            ModuleID = CInt(oHandler.HttpContext.Request.QueryString("ModuleID"))
        End If
        If IsNumeric(oHandler.HttpContext.Request.QueryString("LinkID")) Then
            LinkID = CLng(oHandler.HttpContext.Request.QueryString("LinkID"))
        End If
        Dim ModuleCode As String = oHandler.CommonManager.GetModuleCode(ModuleID)
        Dim ItemPermission As Boolean = False

        If LinkID > 0 Then

            '            Using client As IClientChannel = ChannelFactory
            '            using (IClientChannel client = (IClientChannel)channelFactory.CreateChannel())
            '{
            '    IIdentityService proxy = (IIdentityService)client;
            '}
            Dim oSender As PermissionService.IServicePermission = Nothing
            Try
                oSender = New PermissionService.ServicePermissionClient
                Dim Permission As Integer = 0 'oSender.ModuleLinkActionPermission(LinkID, UCServices.Services_File.ActionType.DownloadFile, UserID, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(oFile.Id, oFile, IIf(oFile.isSCORM, UCServices.Services_File.ObjectType.FileScorm, UCServices.Services_File.ObjectType.File), CommunityID, UCServices.Services_File.Codex))
                If oFile.RepositoryItemType = lm.Comol.Core.DomainModel.Repository.RepositoryItemType.FileStandard Then
                    Permission = oSender.ModuleLinkActionPermission(LinkID, UCServices.Services_File.ActionType.DownloadFile, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(oFile.Id, oFile, IIf(oFile.isSCORM, UCServices.Services_File.ObjectType.FileScorm, UCServices.Services_File.ObjectType.File), CommunityID, UCServices.Services_File.Codex), UserID, GetExternalUsers(oHandler), Nothing)
                Else
                    Permission = oSender.ModuleLinkActionPermission(LinkID, UCServices.Services_File.ActionType.PlayFile, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(oFile.Id, oFile, IIf(oFile.isSCORM, UCServices.Services_File.ObjectType.FileScorm, UCServices.Services_File.ObjectType.File), CommunityID, UCServices.Services_File.Codex), UserID, GetExternalUsers(oHandler), Nothing)

                End If

                iResponse = (UCServices.Services_File.Base2Permission.DownloadFile And Permission)
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

        ElseIf ModuleID = 0 OrElse ModuleCode = Services_File.Codex Then
            Dim Service As ModuleCommunityRepository = oHandler.CommunityRepositoryPermission(CommunityID)
            ItemPermission = oHandler.CurrentManager.HasPermissionToSeeItem(oFile.Id, Service.Administration, Service.Administration, UserID)

            '            Not HasPermission() OrElse (Service.Administration = False AndAlso Service.DownLoad = False) 
            iResponse = ItemPermission AndAlso (Service.Administration OrElse Service.DownLoad)
        Else
            iResponse = True
        End If

        Return iResponse
    End Function

    Private Function GetExternalUsers(ByVal oHandler As HTTPhandler_Utility) As Dictionary(Of String, Long)
        Dim result As New Dictionary(Of String, Long)
        If Not IsNothing(oHandler.HttpContext) AndAlso Not IsNothing(oHandler.HttpContext.Session) Then
            If (From k In oHandler.HttpContext.Session.Keys Where k = "TICKET.CurrentExtUser" Select k).Any() Then
                Dim tUser As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User = Nothing
                Try
                    tUser = DirectCast(oHandler.HttpContext.Session("TICKET.CurrentExtUser"), lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User)
                Catch ex As Exception
                End Try
                If Not IsNothing(tUser) Then
                    result.Add(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode, tUser.UserId)
                End If

            End If
        End If
        Return result
    End Function
    Private Sub SendNewsRead(ByVal oHandler As HTTPhandler_Utility, ByVal UserID As Integer)
        Dim NewsID As System.Guid = System.Guid.Empty
        If (oHandler.HttpContext.Request.QueryString("NewsID") <> "") Then
            Try
                NewsID = New Guid(oHandler.HttpContext.Request.QueryString("NewsID"))
            Catch ex As Exception

            End Try
        End If
        If NewsID <> System.Guid.Empty Then
            oHandler.NotificationSender.ReadNotification(NewsID, UserID)
        End If
    End Sub
    Private Sub DeleteNews(ByVal oHandler As HTTPhandler_Utility)
        Dim NewsID As System.Guid = System.Guid.Empty
        If (oHandler.HttpContext.Request.QueryString("NewsID") <> "") Then
            Try
                NewsID = New Guid(oHandler.HttpContext.Request.QueryString("NewsID"))
            Catch ex As Exception

            End Try
        End If
        If NewsID <> System.Guid.Empty Then
            oHandler.NotificationSender.RemoveNotification(NewsID)
        End If
    End Sub
    Public Function isQueryStringCorrect(ByVal oHandler As HTTPhandler_Utility) As Boolean Implements iHTTPhandler_comunita.isQueryStringCorrect
        Dim FileID As Long = 0
        Try
            FileID = oHandler.HttpContext.Request.QueryString("FileID")
        Catch ex As Exception
        End Try

        If FileID > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function isBaseFileRequest(ByVal oHandler As HTTPhandler_Utility) As Boolean
        If BaseFileID(oHandler) <> System.Guid.Empty Then
            Return True
        Else
            Return False
        End If
    End Function

    Public ReadOnly Property BaseFileID(ByVal oHandler As HTTPhandler_Utility) As System.Guid
        Get
            Dim FileID As System.Guid = Guid.Empty
            If Not String.IsNullOrEmpty(oHandler.HttpContext.Request.QueryString("InternalFileID")) Then
                Try
                    FileID = New System.Guid(oHandler.HttpContext.Request.QueryString("InternalFileID"))
                Catch ex As Exception

                End Try
            End If
            Return FileID
        End Get
    End Property


    Public Sub DefineInternalFileToDownload(ByVal oHandler As HTTPhandler_Utility)
        Dim oMessage As String = ""
        Dim InternalFileID As Guid = BaseFileID(oHandler)
        Dim FileExist As Boolean = False

        Dim NomeFile As String = ""

        Dim oError As New dtoInternalError
        With oError
            .BaseUrl = oHandler.BaseUrl
            .CommunityId = oHandler.CurrentCommunityID
            .ErrorType = ItemRepositoryStatus.NotLoggedIn
            .FileID = InternalFileID
            If IsNumeric(oHandler.HttpContext.Request.QueryString("ForUserID")) Then
                .ForUserID = oHandler.HttpContext.Request.QueryString("ForUserID")
            Else
                .ForUserID = 0
            End If
            .Language = oHandler.HttpContext.Request.QueryString("Language")
            .FileName = ""
            .Settings = oHandler.SystemSettings.NotificationErrorService
            .UserID = oHandler.CurrentUserID
            .FileSettings = oHandler.SystemSettings.File
        End With
        Dim oFile As lm.Comol.Core.DomainModel.BaseFile = oHandler.ManagerInternalFiles.GetBaseFile(InternalFileID)
        If oFile Is Nothing Then
            oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
            oHandler.SendToErrorPage(oError)
        Else
            oError.CommunityId = oHandler.CurrentCommunityID
            oError.FileID = oFile.Id
            oError.FileName = oFile.DisplayName
            oError.Extension = oFile.Extension
            If oError.UserID = 0 Then
                oError.ErrorType = ItemRepositoryStatus.NotLoggedIn
                oHandler.SendToErrorPage(oError)
            Else
                Dim CommunityPath As String = ""
                Dim PageUtility As New OLDpageUtility(oHandler.HttpContext)
                CommunityPath = PageUtility.BaseUserRepositoryPath

                Dim FileNamePath As String = CommunityPath
                FileNamePath &= oFile.Id.ToString & ".stored"
                If File.Exists.File(FileNamePath) Then
                    oHandler.DownloadFile(0, FileNamePath, oFile, oHandler.HttpContext.Request.Url.AbsoluteUri)
                Else
                    oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
                    oHandler.SendToErrorPage(oError)
                End If
            End If
        End If
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region


    'Public Function BeginProcessRequest(context As HttpContext, cb As AsyncCallback, state As Object) As IAsyncResult Implements IHttpAsyncHandler.BeginProcessRequest
    '    Dim asyncresult As AsynchOperation = New AsynchOperation(cb, context, state)
    '    asyncresult.DoAction()
    '    Return asyncresult
    'End Function

    'Public Sub EndProcessRequest(result As IAsyncResult) Implements IHttpAsyncHandler.EndProcessRequest
    '    Dim asyncresult As AsynchOperation = DirectCast(result, AsynchOperation)
    '    If Not IsNothing(asyncresult) Then
    '        'result.Context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
    '        'result.Context.Response.ContentType = "application/octet-stream"
    '        'result.Context.Response.AddHeader("Connection", "keep-alive")

    '        'If (Not IsNothing(result.Data)) Then
    '        '    result.Context.Response.OutputStream.Write(result.Data, 0, result.DataLength);

    '        '    result.Close()
    '        'End If
    '    End If


    'End Sub

    'Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest

    'End Sub
End Class


'Class AsynchOperation
'    Implements IAsyncResult

'    Private _completed As Boolean
'    Private _state As [Object]
'    Private _callback As AsyncCallback
'    Private _context As HttpContext

'    ReadOnly Property IsCompleted() As Boolean _
'            Implements IAsyncResult.IsCompleted
'        Get
'            Return _completed
'        End Get
'    End Property


'    ReadOnly Property AsyncState() As [Object] _
'            Implements IAsyncResult.AsyncState
'        Get
'            Return _state
'        End Get
'    End Property

'    ReadOnly Property CompletedSynchronously() As Boolean _
'            Implements IAsyncResult.CompletedSynchronously
'        Get
'            Return False
'        End Get
'    End Property

'    Public Sub New(ByVal callback As AsyncCallback, _
'            ByVal context As HttpContext, _
'            ByVal state As [Object])
'        _callback = callback
'        _context = context
'        _state = state
'        _completed = False
'    End Sub

'    Public Sub DoAction()
'        Dim oHTTPhandler As New HTTPhandler_Utility(_context, ConfigFileType.File)
'        oHTTPhandler.ClearHTTPcontext()
'        If Me.isQueryStringCorrect(oHTTPhandler) Then
'            Me.DefineFileToDownload(oHTTPhandler)
'        ElseIf Me.isBaseFileRequest(oHTTPhandler) Then
'            Me.DefineInternalFileToDownload(oHTTPhandler)
'        Else
'            Dim oError As New dtoError
'            With oError
'                .BaseUrl = oHTTPhandler.BaseUrl
'                .CommunityID = oHTTPhandler.CurrentCommunityID
'                .ErrorType = ItemRepositoryStatus.NoItemSpecified
'                If IsNumeric(oHTTPhandler.HttpContext.Request.QueryString("ForUserID")) Then
'                    .ForUserID = oHTTPhandler.HttpContext.Request.QueryString("ForUserID")
'                Else
'                    .ForUserID = 0
'                End If
'                .Language = oHTTPhandler.HttpContext.Request.QueryString("Language")
'                .FileID = 0
'                .FolderID = 0
'                .FileName = ""
'                .Settings = oHTTPhandler.SystemSettings.NotificationErrorService
'                .UserID = oHTTPhandler.CurrentUserID
'                .FileSettings = oHTTPhandler.SystemSettings.File
'            End With
'            oHTTPhandler.SendToErrorPage(oError, oHTTPhandler.HttpContext.Request.Url.Query)
'        End If
'    End Sub
'    Public ReadOnly Property AsyncWaitHandle As Threading.WaitHandle Implements IAsyncResult.AsyncWaitHandle
'        Get
'            Return Nothing
'        End Get
'    End Property
'    Public Function isQueryStringCorrect(ByVal oHandler As HTTPhandler_Utility) As Boolean
'        Dim FileID As Long = 0
'        Try
'            FileID = oHandler.HttpContext.Request.QueryString("FileID")
'        Catch ex As Exception
'        End Try

'        If FileID > 0 Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function
'    Public Function isBaseFileRequest(ByVal oHandler As HTTPhandler_Utility) As Boolean
'        If BaseFileID(oHandler) <> System.Guid.Empty Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function

'    Public ReadOnly Property BaseFileID(ByVal oHandler As HTTPhandler_Utility) As System.Guid
'        Get
'            Dim FileID As System.Guid = Guid.Empty
'            If Not String.IsNullOrEmpty(oHandler.HttpContext.Request.QueryString("InternalFileID")) Then
'                Try
'                    FileID = New System.Guid(oHandler.HttpContext.Request.QueryString("InternalFileID"))
'                Catch ex As Exception

'                End Try
'            End If
'            Return FileID
'        End Get
'    End Property


'    Public Sub DefineInternalFileToDownload(ByVal oHandler As HTTPhandler_Utility)
'        Dim oMessage As String = ""
'        Dim InternalFileID As Guid = BaseFileID(oHandler)
'        Dim FileExist As Boolean = False

'        Dim NomeFile As String = ""

'        Dim oError As New dtoInternalError
'        With oError
'            .BaseUrl = oHandler.BaseUrl
'            .CommunityId = oHandler.CurrentCommunityID
'            .ErrorType = ItemRepositoryStatus.NotLoggedIn
'            .FileID = InternalFileID
'            If IsNumeric(oHandler.HttpContext.Request.QueryString("ForUserID")) Then
'                .ForUserID = oHandler.HttpContext.Request.QueryString("ForUserID")
'            Else
'                .ForUserID = 0
'            End If
'            .Language = oHandler.HttpContext.Request.QueryString("Language")
'            .FileName = ""
'            .Settings = oHandler.SystemSettings.NotificationErrorService
'            .UserID = oHandler.CurrentUserID
'            .FileSettings = oHandler.SystemSettings.File
'        End With
'        Dim oFile As lm.Comol.Core.DomainModel.BaseFile = oHandler.ManagerInternalFiles.GetBaseFile(InternalFileID)
'        If oFile Is Nothing Then
'            oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
'            oHandler.SendToErrorPage(oError)
'        Else
'            oError.CommunityId = oHandler.CurrentCommunityID
'            oError.FileID = oFile.Id
'            oError.FileName = oFile.DisplayName
'            oError.Extension = oFile.Extension
'            If oError.UserID = 0 Then
'                oError.ErrorType = ItemRepositoryStatus.NotLoggedIn
'                oHandler.SendToErrorPage(oError)
'            Else
'                Dim CommunityPath As String = ""
'                Dim PageUtility As New OLDpageUtility(oHandler.HttpContext)
'                CommunityPath = PageUtility.BaseUserRepositoryPath

'                Dim FileNamePath As String = CommunityPath
'                FileNamePath &= oFile.Id.ToString & ".stored"
'                If File.Exists.File(FileNamePath) Then
'                    oHandler.DownloadFile(0, FileNamePath, oFile, oHandler.HttpContext.Request.Url.AbsoluteUri)
'                Else
'                    oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
'                    oHandler.SendToErrorPage(oError)
'                End If
'            End If
'        End If
'    End Sub
'    Public Sub DefineFileToDownload(ByVal oHandler As HTTPhandler_Utility)
'        Dim oMessage As String = ""
'        Dim FileID As Long = 0
'        Dim FileExist As Boolean = False
'        Try
'            FileID = oHandler.HttpContext.Request.QueryString("FileID")
'        Catch ex As Exception
'        End Try

'        Dim NomeFile As String = ""

'        Dim oError As New dtoError
'        With oError
'            .BaseUrl = oHandler.BaseUrl
'            .CommunityID = oHandler.CurrentCommunityID
'            .ErrorType = ItemRepositoryStatus.NotLoggedIn
'            .FileID = FileID
'            If IsNumeric(oHandler.HttpContext.Request.QueryString("ForUserID")) Then
'                .ForUserID = oHandler.HttpContext.Request.QueryString("ForUserID")
'            Else
'                .ForUserID = 0
'            End If
'            .Language = oHandler.HttpContext.Request.QueryString("Language")
'            .FolderID = 0
'            .FileName = ""
'            .Settings = oHandler.SystemSettings.NotificationErrorService

'            Dim wSessionId As System.Guid
'            Try
'                wSessionId = New Guid(oHandler.HttpContext.Request.QueryString("wSessionId"))
'            Catch ex As Exception
'                wSessionId = Guid.Empty
'            End Try
'            .UserID = oHandler.CurrentUserID
'            If (.UserID = 0 AndAlso (wSessionId = oHandler.CurrentContext.UserContext.WorkSessionID)) Then
'                .UserID = oHandler.CurrentManager.GetAnonymousIdUser()
'            End If

'            .FileSettings = oHandler.SystemSettings.File
'        End With
'        Dim oFile As lm.Comol.Core.DomainModel.BaseCommunityFile = oHandler.CurrentManager.GetItem(FileID)
'        If oFile Is Nothing Then
'            oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
'            oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
'        Else
'            If IsNothing(oFile.CommunityOwner) Then
'                oError.CommunityID = 0
'            Else
'                oError.CommunityID = oFile.CommunityOwner.Id
'            End If
'            oError.FolderID = oFile.FolderId
'            oError.FileID = oFile.Id
'            oError.FileName = oFile.DisplayName
'            oError.Extension = oFile.Extension

'            Dim idModule As Integer = 0
'            Dim idLink As Long = 0
'            Dim ModuleCode As String = ""
'            If IsNumeric(oHandler.HttpContext.Request.QueryString("LinkID")) Then
'                idLink = CLng(oHandler.HttpContext.Request.QueryString("LinkID"))
'            End If
'            If idLink > 0 Then
'                If IsNumeric(oHandler.HttpContext.Request.QueryString("ModuleID")) Then
'                    idModule = CInt(oHandler.HttpContext.Request.QueryString("ModuleID"))
'                End If
'                ModuleCode = oHandler.CommonManager.GetModuleCode(idModule)
'            End If

'            If oError.UserID = 0 AndAlso idLink = 0 Then
'                oError.ErrorType = ItemRepositoryStatus.NotLoggedIn
'                oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
'            Else
'                If Not HasPermission(oHandler, oFile, oError.CommunityID, oError.UserID) Then
'                    If oError.UserID = 0 Then
'                        oError.ErrorType = ItemRepositoryStatus.NotLoggedIn
'                    Else
'                        Me.SendNewsRead(oHandler, oError.UserID)
'                        oError.ErrorType = ItemRepositoryStatus.NoPermissionToSeeItem
'                    End If

'                    oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
'                Else
'                    Dim CommunityPath As String = ""
'                    If oHandler.SystemSettings.File.Materiale.DrivePath = "" Then
'                        CommunityPath = oHandler.HttpContext.Server.MapPath(oError.BaseUrl & oHandler.SystemSettings.File.Materiale.VirtualPath) & "\" & oError.CommunityID & "\"
'                    Else
'                        CommunityPath = oHandler.SystemSettings.File.Materiale.DrivePath & "\" & oError.CommunityID & "\"
'                    End If

'                    Dim oImpersonate As New lm.Comol.Core.File.Impersonate
'                    Dim FileNamePath As String = Replace(CommunityPath, "\\", "\")
'                    Dim Link As lm.Comol.Core.DomainModel.ModuleLink = Nothing
'                    Dim allowDownload As Boolean = False
'                    Try

'                        FileNamePath &= oFile.UniqueID.ToString & ".stored"
'                        If oImpersonate.ImpersonateValidUser() = ItemRepositoryStatus.ImpersonationFailed Then
'                            oError.ErrorType = ItemRepositoryStatus.ImpersonationFailed
'                            oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
'                        Else
'                            If File.Exists.File(FileNamePath) Then
'                                If oError.UserID > 0 Then
'                                    Me.SendNewsRead(oHandler, oError.UserID)
'                                End If
'                                Dim LinkID As Long = 0
'                                Dim ServiceCode As String = Services_File.Codex
'                                If IsNumeric(oHandler.HttpContext.Request.QueryString("LinkID")) Then
'                                    LinkID = CLng(oHandler.HttpContext.Request.QueryString("LinkID"))
'                                End If

'                                If LinkID > 0 Then
'                                    Link = oHandler.CommonManager.GetModuleLink(LinkID)
'                                    ServiceCode = Link.SourceItem.ServiceCode
'                                End If

'                                Dim s As String = oHandler.HttpContext.Request.QueryString("notSaveStat")
'                                Dim stat As Boolean = True
'                                If Not String.IsNullOrEmpty(s) Then
'                                    s = s.ToLower
'                                    stat = (s <> "true")
'                                End If

'                                If stat Then
'                                    oHandler.CurrentManager.AddDownloadToFile(oFile, oError.UserID, Link, ServiceCode)
'                                End If

'                                allowDownload = True
'                            Else
'                                Me.DeleteNews(oHandler)
'                                oError.ErrorType = ItemRepositoryStatus.FileDoesntExist
'                                oHandler.SendToErrorPage(oError, oHandler.HttpContext.Request.Url.Query)
'                            End If
'                        End If
'                        oImpersonate.UndoImpersonation()
'                    Catch ex As Exception
'                        oImpersonate.UndoImpersonation()
'                    Finally
'                        oImpersonate.UndoImpersonation()
'                        oImpersonate = Nothing
'                    End Try
'                    If allowDownload Then
'                        oImpersonate = New lm.Comol.Core.File.Impersonate
'                        If oImpersonate.ImpersonateValidUser() <> ItemRepositoryStatus.ImpersonationFailed Then
'                            oHandler.DownloadRepositoryFile(oImpersonate, Link, oError.UserID, FileNamePath, oFile.DisplayName, oFile.Extension, oFile.ContentType, oHandler.HttpContext.Request.Url.Query)
'                        End If
'                    End If
'                End If
'            End If
'        End If
'    End Sub
'    Private Function HasPermission(ByVal oHandler As HTTPhandler_Utility, ByVal oFile As lm.Comol.Core.DomainModel.BaseCommunityFile, ByVal CommunityID As Integer, ByVal UserID As Integer) As Boolean
'        Dim iResponse As Boolean = False
'        Dim ModuleID As Integer = 0
'        Dim LinkID As Long = 0
'        If IsNumeric(oHandler.HttpContext.Request.QueryString("ModuleID")) Then
'            ModuleID = CInt(oHandler.HttpContext.Request.QueryString("ModuleID"))
'        End If
'        If IsNumeric(oHandler.HttpContext.Request.QueryString("LinkID")) Then
'            LinkID = CLng(oHandler.HttpContext.Request.QueryString("LinkID"))
'        End If
'        Dim ModuleCode As String = oHandler.CommonManager.GetModuleCode(ModuleID)
'        Dim ItemPermission As Boolean = False

'        If LinkID > 0 Then

'            '            Using client As IClientChannel = ChannelFactory
'            '            using (IClientChannel client = (IClientChannel)channelFactory.CreateChannel())
'            '{
'            '    IIdentityService proxy = (IIdentityService)client;
'            '}
'            Dim oSender As PermissionService.IServicePermission = Nothing
'            Try
'                oSender = New PermissionService.ServicePermissionClient
'                Dim Permission As Integer = 0 'oSender.ModuleLinkActionPermission(LinkID, UCServices.Services_File.ActionType.DownloadFile, UserID, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(oFile.Id, oFile, IIf(oFile.isSCORM, UCServices.Services_File.ObjectType.FileScorm, UCServices.Services_File.ObjectType.File), CommunityID, UCServices.Services_File.Codex))
'                If oFile.RepositoryItemType = lm.Comol.Core.DomainModel.Repository.RepositoryItemType.FileStandard Then
'                    Permission = oSender.ModuleLinkActionPermission(LinkID, UCServices.Services_File.ActionType.DownloadFile, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(oFile.Id, oFile, IIf(oFile.isSCORM, UCServices.Services_File.ObjectType.FileScorm, UCServices.Services_File.ObjectType.File), CommunityID, UCServices.Services_File.Codex), UserID, GetExternalUsers(oHandler), Nothing)
'                Else
'                    Permission = oSender.ModuleLinkActionPermission(LinkID, UCServices.Services_File.ActionType.PlayFile, lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(oFile.Id, oFile, IIf(oFile.isSCORM, UCServices.Services_File.ObjectType.FileScorm, UCServices.Services_File.ObjectType.File), CommunityID, UCServices.Services_File.Codex), UserID, GetExternalUsers(oHandler), Nothing)

'                End If

'                iResponse = (UCServices.Services_File.Base2Permission.DownloadFile And Permission)
'                If Not IsNothing(oSender) Then
'                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
'                    service.Close()
'                    service = Nothing
'                End If
'            Catch ex As Exception
'                If Not IsNothing(oSender) Then
'                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
'                    service.Abort()
'                    service = Nothing
'                End If
'            End Try

'        ElseIf ModuleID = 0 OrElse ModuleCode = Services_File.Codex Then
'            Dim Service As ModuleCommunityRepository = oHandler.CommunityRepositoryPermission(CommunityID)
'            ItemPermission = oHandler.CurrentManager.HasPermissionToSeeItem(oFile.Id, Service.Administration, Service.Administration, UserID)

'            '            Not HasPermission() OrElse (Service.Administration = False AndAlso Service.DownLoad = False) 
'            iResponse = ItemPermission AndAlso (Service.Administration OrElse Service.DownLoad)
'        Else
'            iResponse = True
'        End If

'        Return iResponse
'    End Function

'    Private Function GetExternalUsers(ByVal oHandler As HTTPhandler_Utility) As Dictionary(Of String, Long)
'        Dim result As New Dictionary(Of String, Long)
'        If Not IsNothing(oHandler.HttpContext) AndAlso Not IsNothing(oHandler.HttpContext.Session) Then
'            If (From k In oHandler.HttpContext.Session.Keys Where k = "TICKET.CurrentExtUser" Select k).Any() Then
'                Dim tUser As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User = Nothing
'                Try
'                    tUser = DirectCast(oHandler.HttpContext.Session("TICKET.CurrentExtUser"), lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User)
'                Catch ex As Exception
'                End Try
'                If Not IsNothing(tUser) Then
'                    result.Add(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode, tUser.UserId)
'                End If

'            End If
'        End If
'        Return result
'    End Function
'    Private Sub SendNewsRead(ByVal oHandler As HTTPhandler_Utility, ByVal UserID As Integer)
'        Dim NewsID As System.Guid = System.Guid.Empty
'        If (oHandler.HttpContext.Request.QueryString("NewsID") <> "") Then
'            Try
'                NewsID = New Guid(oHandler.HttpContext.Request.QueryString("NewsID"))
'            Catch ex As Exception

'            End Try
'        End If
'        If NewsID <> System.Guid.Empty Then
'            oHandler.NotificationSender.ReadNotification(NewsID, UserID)
'        End If
'    End Sub
'    Private Sub DeleteNews(ByVal oHandler As HTTPhandler_Utility)
'        Dim NewsID As System.Guid = System.Guid.Empty
'        If (oHandler.HttpContext.Request.QueryString("NewsID") <> "") Then
'            Try
'                NewsID = New Guid(oHandler.HttpContext.Request.QueryString("NewsID"))
'            Catch ex As Exception

'            End Try
'        End If
'        If NewsID <> System.Guid.Empty Then
'            oHandler.NotificationSender.RemoveNotification(NewsID)
'        End If
'    End Sub
'End Class 'AsynchOperation