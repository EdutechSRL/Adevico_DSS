Imports lm.Comol.Core.DomainModel
Public Class RepositoryCommunityNewsUtility
    Private _IdModule As Integer
    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property IdModule As Integer
        Get
            Return _IdModule
        End Get
    End Property
    Public ReadOnly Property PageUtility As OLDpageUtility
        Get
            Return _PageUtility
        End Get
    End Property
    Public Sub New(idModuleRepository As Integer, utility As OLDpageUtility)
        _IdModule = idModuleRepository
        _PageUtility = utility
    End Sub
    Protected ReadOnly Property CommunityNewsSender() As SrvCommunityNewsOld.iNotificationServiceClient
        Get
            Dim oSender As SrvCommunityNewsOld.iNotificationServiceClient = Nothing
            Try
                oSender = New SrvCommunityNewsOld.iNotificationServiceClient
            Catch ex As Exception

            End Try
            Return oSender
        End Get
    End Property

#Region "Add Notification"

    Public Sub ItemAdded(idCommunity As Integer, idItem As Long, idVersion As Long, type As lm.Comol.Core.FileRepository.Domain.ItemType, ByVal fileName As String, ByVal folder As String, fileurl As String, folderUrl As String, permissions As Integer, Optional ByVal isVersion As Boolean = False)
        If PageUtility.SystemSettings.NotificationService.Enabled Then
            Dim newsId As System.Guid = System.Guid.NewGuid

            If fileurl.Contains("#") Then
                Dim itemParameters As List(Of String) = fileurl.Split("#").ToList()
                If itemParameters(0).Contains("?") Then
                    itemParameters(0) = itemParameters(0) & "&newsId=" & newsId.ToString
                Else
                    itemParameters(0) = itemParameters(0) & "?newsId=" & newsId.ToString
                End If
                fileurl = (String.Join("#", itemParameters))
            ElseIf (fileurl.Contains("?")) Then
                fileurl &= "&newsId=" & newsId.ToString
            Else
                fileurl &= "?newsId=" & newsId.ToString
            End If

            NotifyUpload(newsId, idCommunity, idItem, idVersion, type, fileName, folder, fileurl, folderUrl, permissions, CreateFileToNotify(idItem, idVersion, type), isVersion)
        End If
    End Sub
   
    Private Sub NotifyUpload(newsId As Guid, idCommunity As Integer, idItem As Long, idVersion As Long, type As lm.Comol.Core.FileRepository.Domain.ItemType, ByVal fileName As String, ByVal folder As String, fileurl As String, folderUrl As String, permissions As Integer, ByVal oDto As SrvCommunityNewsOld.dtoNotificatedObject, Optional ByVal isVersion As Boolean = False)
        Dim items As New List(Of String)
        items.Add(fileurl)
        items.Add(fileName)
        If folderUrl.Contains("#") Then
            Dim folderParameters As List(Of String) = folderUrl.Split("#").ToList()
            folderParameters(0) = folderParameters(0) & "&newsId=" & newsId.ToString
            items.Add(String.Join("#", folderParameters))
        Else
            items.Add(folderUrl & "&newsId=" & newsId.ToString)
        End If

        items.Add(folder)
        Dim action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.AddFile
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.AddLink
            Case Else
                If isVersion Then
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VersionAddedToFile
                End If
        End Select

        SendNotificationToItemLong(newsId, idCommunity, idItem, idVersion, type, permissions, action, items, oDto)
    End Sub
#End Region

#Region "Delete Notification"
    Public Sub ItemDeleted(idCommunity As Integer, idItem As Long, type As lm.Comol.Core.FileRepository.Domain.ItemType, ByVal itemName As String, ByVal folder As String, folderUrl As String)
        If PageUtility.SystemSettings.NotificationService.Enabled Then
            Dim newsId As System.Guid = System.Guid.NewGuid

            If folderUrl.Contains("#") Then
                Dim itemParameters As List(Of String) = folderUrl.Split("#").ToList()
                If itemParameters(0).Contains("?") Then
                    itemParameters(0) = itemParameters(0) & "&newsId=" & newsId.ToString
                Else
                    itemParameters(0) = itemParameters(0) & "?newsId=" & newsId.ToString
                End If
                folderUrl = (String.Join("#", itemParameters))
            ElseIf (folderUrl.Contains("?")) Then
                folderUrl &= "&newsId=" & newsId.ToString
            Else
                folderUrl &= "?newsId=" & newsId.ToString
            End If

            Dim items As New List(Of String)
            items.Add(itemName)
            items.Add(folderUrl)
            items.Add(folder)

            SendNotificationToItemLong(newsId, idCommunity, idItem, 0, type, PermissionToAdmin, GetDeletedIdAction(type, True, True), items, CreateFileToNotify(idItem, 0, type))
        End If
       
    End Sub

    Public Sub ItemVirtualDeleted(isVisible As Boolean, idCommunity As Integer, idItem As Long, type As lm.Comol.Core.FileRepository.Domain.ItemType, ByVal itemName As String, ByVal folder As String, fileurl As String, folderUrl As String, isDeleted As Boolean)
        If PageUtility.SystemSettings.NotificationService.Enabled Then
            Dim newsId As System.Guid = System.Guid.NewGuid

            If folderUrl.Contains("#") Then
                Dim itemParameters As List(Of String) = folderUrl.Split("#").ToList()
                If itemParameters(0).Contains("?") Then
                    itemParameters(0) = itemParameters(0) & "&newsId=" & newsId.ToString
                Else
                    itemParameters(0) = itemParameters(0) & "?newsId=" & newsId.ToString
                End If
                folderUrl = (String.Join("#", itemParameters))
            ElseIf (folderUrl.Contains("?")) Then
                folderUrl &= "&newsId=" & newsId.ToString
            Else
                folderUrl &= "?newsId=" & newsId.ToString
            End If

            Dim items As New List(Of String)
            items.Add(itemName)
            If Not isDeleted Then
                If fileurl.Contains("#") Then
                    Dim itemParameters As List(Of String) = fileurl.Split("#").ToList()
                    If itemParameters(0).Contains("?") Then
                        itemParameters(0) = itemParameters(0) & "&newsId=" & newsId.ToString
                    Else
                        itemParameters(0) = itemParameters(0) & "?newsId=" & newsId.ToString
                    End If
                    fileurl = (String.Join("#", itemParameters))
                ElseIf (fileurl.Contains("?")) Then
                    fileurl &= "&newsId=" & newsId.ToString
                Else
                    fileurl &= "?newsId=" & newsId.ToString
                End If
                items.Add(fileurl)
            End If
            items.Add(folderUrl)
            items.Add(folder)

            SendNotificationToItemLong(newsId, idCommunity, idItem, 0, type, IIf(isVisible, PermissionToSee, PermissionToAdmin), GetDeletedIdAction(type, isDeleted), items, CreateFileToNotify(idItem, 0, type))
        End If

    End Sub

    Private Function GetDeletedIdAction(type As lm.Comol.Core.FileRepository.Domain.ItemType, isDeleted As Boolean, Optional isPhisicalDelete As Boolean = False) As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType
        Dim action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.None
        If isDeleted Then
            If isPhisicalDelete Then
                Select Case type
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DeleteLink
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DeleteFolder
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DeleteScormPackage
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DeleteMultimedia
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DeleteSharedDocument
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.VideoStreaming
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DeleteVideoStreaming
                    Case Else
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DeleteFile
                End Select
            Else
                Select Case type
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualDeleteLink
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualDeleteFolder
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualDeleteScormPackage
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualDeleteMultimedia
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualDeleteSharedDocument
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.VideoStreaming
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualDeleteVideoStreaming
                    Case Else
                        action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualDeleteFile
                End Select
            End If
        ElseIf Not isDeleted Then
            Select Case type
                Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualUndeleteLink
                Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualUndeleteFolder
                Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualUndeleteScormPackage
                Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualUndeleteMultimedia
                Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualUndeleteSharedDocument
                Case lm.Comol.Core.FileRepository.Domain.ItemType.VideoStreaming
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualUndeleteVideoStreaming
                Case Else
                    action = lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.VirtualUndeleteFile
            End Select
        End If
        Return action
    End Function

#End Region

#Region "Visibility Notification"
    Public Sub ItemEditVisibility(isVisible As Boolean, idCommunity As Integer, idItem As Long, idVersion As Long, type As lm.Comol.Core.FileRepository.Domain.ItemType, ByVal itemName As String, ByVal folder As String, itemUrl As String, folderUrl As String)
        Dim newsId As System.Guid = System.Guid.NewGuid

        If itemUrl.Contains("#") Then
            Dim itemParameters As List(Of String) = itemUrl.Split("#").ToList()
            If itemParameters(0).Contains("?") Then
                itemParameters(0) = itemParameters(0) & "&newsId=" & newsId.ToString
            Else
                itemParameters(0) = itemParameters(0) & "?newsId=" & newsId.ToString
            End If
            itemUrl = (String.Join("#", itemParameters))
        ElseIf (itemUrl.Contains("?")) Then
            itemUrl &= "&newsId=" & newsId.ToString
        Else
            itemUrl &= "?newsId=" & newsId.ToString
        End If

        Dim action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType = IIf(isVisible, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.ShowItem, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.HideItem)
        Dim parameters As New List(Of String)
        Dim permissions As Integer = PermissionToSee()
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                action = IIf(isVisible, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.ShowFolder, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.HideFolder)
                parameters.Add(folderUrl)
                parameters.Add(folder)
                If isVisible Then : parameters.Add(itemUrl)
                End If
                parameters.Add(itemName)
            Case Else
                If isVisible Then : parameters.Add(itemUrl)
                Else
                    permissions = PermissionToAdmin()
                End If
                parameters.Add(itemName)
                parameters.Add(folderUrl)
                parameters.Add(folder)
                Select Case type
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                        action = IIf(isVisible, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.ShowLink, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.HideLink)
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                        action = IIf(isVisible, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.ShowMultimedia, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.HideMultimedia)
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                        action = IIf(isVisible, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.ShowScormPackage, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.HideScormPackage)
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.SharedDocument
                        action = IIf(isVisible, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.ShowSharedDocument, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.HideSharedDocument)
                    Case lm.Comol.Core.FileRepository.Domain.ItemType.VideoStreaming
                        action = IIf(isVisible, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.ShowVideoStreaming, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.HideVideoStreaming)
                End Select
        End Select

        SendNotificationToItemLong(newsId, idCommunity, idItem, idVersion, type, permissions, action, parameters, CreateFileToNotify(idItem, idVersion, type))
    End Sub
#End Region

    '#Region "Move Notification"
    '    'Public Sub NotifyFileMoved(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String)
    '    '    Dim NewsID As System.Guid = System.Guid.NewGuid
    '    '    Dim FileUrl As String = Me.DownloadPage(FileID, NewsID)
    '    '    Me.NotifyMoved(NewsID, CommunityID, FolderID, FileID, FileName, FileUrl, FromFolder, ToFolder, CreateFileStandardToNotify(FileID))
    '    'End Sub
    '    Public Sub NotifyItemMoved(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal UniqueID As System.Guid, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal type As RepositoryItemType)
    '        Dim NewsID As System.Guid = System.Guid.NewGuid
    '        Dim FileUrl As String = Me.ViewRepositoryItem(FileID, UniqueID, NewsID, type)
    '        Me.NotifyMoved(NewsID, CommunityID, FolderID, FileID, FileName, FileUrl, FromFolder, ToFolder, CreateFileToNotify(FileID, type))
    '    End Sub
    '    Public Sub NotifyFolderMoved(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal ItemID As Long, ByVal Folder As String, ByVal FromFolder As String, ByVal ToFolder As String)
    '        Dim NewsID As System.Guid = System.Guid.NewGuid
    '        Dim oValues = New List(Of String)
    '        oValues.Add(Me.RepositoryViewPage(CommunityID, ItemID, NewsID))
    '        oValues.Add(Folder)
    '        oValues.Add(FromFolder)

    '        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
    '        oValues.Add(ToFolder)
    '        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, ItemID, Services_File.ObjectType.Folder, Services_File.ActionType.MoveFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(ItemID))
    '    End Sub
    '    Private Sub NotifyMoved(ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal ItemID As Long, ByVal FileName As String, ByVal FileUrl As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal oDto As dtoNotificatedObject)
    '        Dim oValues = New List(Of String)

    '        oValues.Add(FileUrl)
    '        oValues.Add(FileName)
    '        oValues.Add(FromFolder)
    '        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
    '        oValues.Add(ToFolder)
    '        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, ItemID, Services_File.ObjectType.File, Services_File.ActionType.MoveFile, CommunityID, Services_File.Codex, oValues, oDto)
    '    End Sub
    '#End Region

   

    '#Region "Import File"
    '    Public Sub NotifyFolderImport(ByVal CommunityID As Integer, ByVal FolderFatherID As Long, ByVal FolderName As String, ByVal Number As Integer)
    '        Dim NewsID As System.Guid = System.Guid.NewGuid
    '        Dim SeeUrl As String = Me.RepositoryViewPage(CommunityID, FolderFatherID, NewsID)
    '        Dim oValues = New List(Of String)
    '        oValues.Add(Number)
    '        oValues.Add(SeeUrl)
    '        oValues.Add(FolderName)
    '        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, FolderFatherID, Services_File.ObjectType.Folder, Services_File.ActionType.ImportFolders, CommunityID, Services_File.Codex, oValues, New List(Of dtoNotificatedObject))
    '    End Sub
    '    Public Sub NotifyFileImport(ByVal CommunityID As Integer, ByVal FolderFatherID As Long, ByVal FolderName As String, ByVal Number As Integer)
    '        Dim NewsID As System.Guid = System.Guid.NewGuid
    '        Dim oValues = New List(Of String)
    '        Dim SeeUrl As String = Me.RepositoryViewPage(CommunityID, FolderFatherID, NewsID)
    '        oValues.Add(Number)
    '        oValues.Add(SeeUrl)
    '        oValues.Add(FolderName)
    '        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, FolderFatherID, Services_File.ObjectType.Folder, Services_File.ActionType.ImportFiles, CommunityID, Services_File.Codex, oValues, New List(Of dtoNotificatedObject))
    '    End Sub
    '#End Region

    '#Region "Edit Permission"
    '    Public Sub NotifyItemPermissionModifyed(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
    '        Dim ActionID As Integer = Services_File.ActionType.None
    '        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
    '            ActionID = Services_File.ActionType.FolderPermissionModifyed
    '        Else
    '            ActionID = Services_File.ActionType.FilePermissionModifyed
    '        End If
    '        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    '    End Sub
    '    Public Sub NotifyItemPermissionModifyedToCommunity(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
    '        Dim ActionID As Integer = Services_File.ActionType.None
    '        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
    '            ActionID = Services_File.ActionType.FolderPermissionToCommunity
    '        Else
    '            ActionID = Services_File.ActionType.FilePermissionToCommunity
    '        End If
    '        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    '    End Sub
    '    Public Sub NotifyItemPermissionModifyedToSome(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
    '        Dim ActionID As Integer = Services_File.ActionType.None
    '        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
    '            ActionID = Services_File.ActionType.FolderPermissionToSome
    '        Else
    '            ActionID = Services_File.ActionType.FilePermissionToSome
    '        End If
    '        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    '    End Sub

    '    Private Sub NotifyItemPermissionModifyed(ByVal CommunityID As Integer, ByVal oContext As NotifyContext, ByVal ActionID As Integer)
    '        Dim oValues = New List(Of String)
    '        Dim oNotificated As dtoNotificatedObject = CreateObjectToNotify(oContext.ItemID, oContext.RepositoryItemType)
    '        Dim ObjectTypeID As Integer = CInt(oContext.RepositoryItemType)


    '        If (oContext.ToUsers And NotifyTo.ToOwner) > 0 Then
    '            Dim UserNewsID As System.Guid = System.Guid.NewGuid
    '            Dim oPersons As New List(Of Integer)
    '            oPersons.Add(oContext.OwnerID)

    '            oValues = GenericGenerateValues(UserNewsID, CommunityID, oContext, False)
    '            GenericNotify(NotifyTo.ToOwner, CommunityID, UserNewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated, oPersons)
    '        End If

    '        If (oContext.ToUsers And NotifyTo.ToAdmin) > 0 Then
    '            Dim AdminNewsID As System.Guid = System.Guid.NewGuid
    '            oValues = GenericGenerateValues(AdminNewsID, CommunityID, oContext, True)

    '            GenericNotify(NotifyTo.ToAdmin, CommunityID, AdminNewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated)
    '        End If

    '        If (oContext.ToUsers And NotifyTo.ToAllSee) > 0 Then
    '            Dim NewsID As System.Guid = System.Guid.NewGuid
    '            oValues = GenericGenerateValues(NewsID, CommunityID, oContext, False)
    '            GenericNotify(NotifyTo.ToAllSee, CommunityID, NewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated)
    '        End If

    '        If (oContext.ToUsers And NotifyTo.ToOnlySee) > 0 Then
    '            Dim NewsID As System.Guid = System.Guid.NewGuid
    '            oValues = GenericGenerateValues(NewsID, CommunityID, oContext, False)
    '            GenericNotify(NotifyTo.ToOnlySee, CommunityID, NewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated)
    '        End If
    '    End Sub
    '#End Region

    '#Region "Edit Permission"
    '    Public Sub NotifyItemModifyed(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
    '        Dim ActionID As Integer = Services_File.ActionType.None
    '        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
    '            ActionID = Services_File.ActionType.FolderEdited
    '        Else
    '            ActionID = Services_File.ActionType.FileEdited
    '        End If
    '        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    '    End Sub
    '#End Region

    'Private Sub GenericNotify(ByVal ToUsers As NotifyTo, ByVal CommunityID As Integer, ByVal NewsID As System.Guid, ByVal ActionID As Integer, ByVal ObjectTypeID As Integer, ByVal ItemID As Long, ByVal oValues As List(Of String), ByVal oNotificated As dtoNotificatedObject, Optional ByVal oPersons As List(Of Integer) = Nothing)
    '    If ToUsers = NotifyTo.ToOwner Then
    '        _Utility.SendNotificationToPerson(NewsID, oPersons, ActionID, CommunityID, Services_File.Codex, oValues, oNotificated)
    '    Else
    '        Dim Permission As Integer = Me.PermissionOnlySee
    '        If ToUsers = NotifyTo.ToAllSee Then
    '            Permission = Me.PermissionToSee
    '        ElseIf ToUsers = NotifyTo.ToAdmin Then
    '            Permission = Me.PermissionToAdmin
    '        End If

    '        _Utility.SendNotificationToItemLong(NewsID, Permission, ItemID, ObjectTypeID, ActionID, CommunityID, Services_File.Codex, oValues, oNotificated)
    '    End If
    'End Sub

    'Private Function GenericGenerateValues(ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal oContext As NotifyContext, ByVal ForManage As Boolean) As List(Of String)
    '    Dim oValues = New List(Of String)
    '    oValues.Add(oContext.ItemName)
    '    If oContext.RepositoryItemType = RepositoryItemType.Folder Then
    '        If ForManage Then : oValues.Add(Me.RepositoryManagementPage(CommunityID, oContext.ItemID, NewsID))
    '        Else : oValues.Add(Me.RepositoryViewPage(CommunityID, oContext.ItemID, NewsID))
    '        End If

    '        oValues.Add(Me.ViewRepositoryItem(oContext.ItemID, oContext.UniqueId, NewsID, oContext.RepositoryItemType))
    '    End If
    '    oValues.Add(oContext.FatherName)
    '    If ForManage Then : oValues.Add(Me.RepositoryManagementPage(CommunityID, oContext.ItemID, NewsID))
    '    Else : oValues.Add(Me.RepositoryViewPage(CommunityID, oContext.ItemID, NewsID))
    '    End If
    '    Return oValues
    'End Function

#Region "Object To Notify"
    Private Function CreateFolderToNotify(ByVal idFolder As Long) As SrvCommunityNewsOld.dtoNotificatedObject
        Return CreateObjectToNotify(idFolder, lm.Comol.Core.FileRepository.Domain.ItemType.Folder)
    End Function
    Private Function CreateLinkToNotify(ByVal idLink As Long) As SrvCommunityNewsOld.dtoNotificatedObject
        Return CreateObjectToNotify(idLink, lm.Comol.Core.FileRepository.Domain.ItemType.Link)
    End Function
    Private Function CreateFileToNotify(idItem As Long, idVersion As Long, ByVal type As lm.Comol.Core.FileRepository.Domain.ItemType) As SrvCommunityNewsOld.dtoNotificatedObject
        Return CreateObjectToNotify(idItem, type)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As Long, ByVal type As lm.Comol.Core.FileRepository.Domain.ItemType) As SrvCommunityNewsOld.dtoNotificatedObject
        Dim obj As New SrvCommunityNewsOld.dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = type
        obj.ModuleCode = lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode
        obj.ModuleID = IdModule
        obj.FullyQualiFiedName = GetType(lm.Comol.Core.FileRepository.Domain.RepositoryItem).FullName
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionOnlySee() As Integer
        Return CLng(lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.DownloadOrPlay)
    End Function
    Public Function PermissionToSee() As Integer
        Return CLng(lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.Administration Or lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.Manage Or lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.DownloadOrPlay)
    End Function
    Public Function PermissionToAdmin() As Integer
        Return CLng(lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.Administration Or lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.Manage)
    End Function
#End Region

    <Flags()> Enum NotifyTo
        ToOwner = 1
        ToAdmin = 2
        ToAllSee = 4
        ToOnlySee = 8
    End Enum

#Region "Base Functions"
    Private Sub SendNotificationToItemLong(newsId As Guid, idCommunity As Integer, idItem As Long, idVersion As Long, type As lm.Comol.Core.FileRepository.Domain.RepositoryType, permissions As Integer, ByVal idAction As Integer, ByVal parameters As List(Of String), ByVal oDto As SrvCommunityNewsOld.dtoNotificatedObject)
        Dim oList As New List(Of SrvCommunityNewsOld.dtoNotificatedObject)
        If Not IsNothing(oDto) Then
            oList.Add(oDto)
        End If
        Me.SendNotificationToItemLong(newsId, idCommunity, idItem, idVersion, type, permissions, idAction, parameters, oList)
    End Sub
    Private Sub SendNotificationToItemLong(sender As SrvCommunityNewsOld.iNotificationServiceClient, newsId As Guid, idCommunity As Integer, idItem As Long, idVersion As Long, type As lm.Comol.Core.FileRepository.Domain.RepositoryType, permissions As Integer, ByVal idAction As Integer, ByVal parameters As List(Of String), ByVal objects As List(Of SrvCommunityNewsOld.dtoNotificatedObject))
        sender.NotifyForPermissionItemVersionLong(CreateNotificationToItem(newsId, idCommunity, idItem, idVersion, type, permissions, idAction, parameters, objects))
    End Sub
    Private Sub SendNotificationToItemLong(newsId As Guid, idCommunity As Integer, idItem As Long, idVersion As Long, type As lm.Comol.Core.FileRepository.Domain.RepositoryType, permissions As Integer, ByVal idAction As Integer, ByVal parameters As List(Of String), ByVal objects As List(Of SrvCommunityNewsOld.dtoNotificatedObject))
        Dim Sender As SrvCommunityNewsOld.iNotificationServiceClient = CommunityNewsSender
        If IsNothing(Sender) Then
            Exit Sub
        ElseIf PageUtility.SystemSettings.NotificationService.Enabled Then
            Try
                Dim oNotification As SrvCommunityNewsOld.NotificationToItemVersionLong = CreateNotificationToItem(newsId, idCommunity, idItem, idVersion, type, permissions, idAction, parameters, objects)
                Sender.NotifyForPermissionItemVersionLong(oNotification)
            Catch ex As Exception

            End Try
            DisposeCommunityNewsSenderSender(Sender)
        End If
    End Sub
    Private Function CreateNotificationToItem(ByVal newsId As System.Guid, idCommunity As Integer, idItem As Long, idVersion As Long, type As lm.Comol.Core.FileRepository.Domain.RepositoryType, permissions As Integer, ByVal idAction As Integer, ByVal parameters As List(Of String), ByVal objects As List(Of SrvCommunityNewsOld.dtoNotificatedObject)) As SrvCommunityNewsOld.NotificationToItemVersionLong
        Dim oNotification As New SrvCommunityNewsOld.NotificationToItemVersionLong()
        With oNotification
            .UniqueID = newsId
            .IdVersion = idVersion
            .ActionID = idAction
            .CommunityID = idCommunity
            .ModuleCode = lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode
            .ModuleID = IdModule
            .NotificatedObjects = objects
            .SentDate = Now
            .ValueParameters = New List(Of String)
            .ValueParameters.AddRange(parameters)
            .IdItem = idItem
            .ObjectTypeID = type
            .Permission = permissions
        End With
        Return oNotification
    End Function
    Private Sub DisposeCommunityNewsSenderSender(ByVal sender As SrvCommunityNewsOld.iNotificationService)
        If Not IsNothing(sender) Then
            Dim service As System.ServiceModel.ClientBase(Of SrvCommunityNewsOld.iNotificationService) = DirectCast(sender, System.ServiceModel.ClientBase(Of SrvCommunityNewsOld.iNotificationService))
            Try
                If service.State <> System.ServiceModel.CommunicationState.Closed AndAlso service.State <> System.ServiceModel.CommunicationState.Faulted Then
                    service.Close()
                ElseIf service.State <> System.ServiceModel.CommunicationState.Closed Then
                    service.Abort()
                End If
                service = Nothing
            Catch ex As Exception
                service.Abort()
            End Try
        End If
    End Sub
#End Region
End Class