Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain
Imports lm.Comol.Core.DomainModel.Repository
Public Class RepositoryNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub


    Private ReadOnly Property BaseServiceUrl() As String
        Get
            Return MyBase._Utility.BaseUrl & "Modules/Repository/"
        End Get
    End Property
    Private ReadOnly Property BaseServiceScormUrl() As String
        Get
            Return MyBase._Utility.BaseUrl & "Modules/"
        End Get
    End Property
    Private ReadOnly Property RepositoryViewPage(ByVal CommunityID As String, ByVal FolderID As Long, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceUrl & "CommunityRepository.aspx?CommunityID=" & CommunityID.ToString & "&FolderID=" & FolderID.ToString & "&Page=0&View=FolderList" & "&NewsID=" & NewsID.ToString
        End Get
    End Property
    Private ReadOnly Property RepositoryManagementPage(ByVal CommunityID As String, ByVal FolderID As Long, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceUrl & "CommunityRepositoryManagement.aspx?CommunityID=" & CommunityID.ToString & "&FolderID=" & FolderID.ToString & "&Page=0&View=FileList" & "&NewsID=" & NewsID.ToString
        End Get
    End Property
    Private ReadOnly Property DownloadPage(ByVal FileID As Long, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceUrl & "File.repository?FileID=" & FileID.ToString & "&NewsID=" & NewsID.ToString
        End Get
    End Property
    Private ReadOnly Property ViewRepositoryItem(ByVal FileID As Long, ByVal UniqueID As System.Guid, ByVal NewsID As System.Guid, ByVal type As RepositoryItemType) As String
        Get
            Select Case type
                Case RepositoryItemType.FileStandard
                    Return BaseServiceUrl & "File.repository?FileID=" & FileID.ToString & "&NewsID=" & NewsID.ToString
                Case RepositoryItemType.Multimedia
                    Return BaseServiceUrl & "MultimediaFileLoader.aspx?FileID=" & FileID.ToString & "&GUID=" & UniqueID.ToString & "&NewsID=" & NewsID.ToString
                Case RepositoryItemType.ScormPackage
                    Return BaseServiceScormUrl & "Scorm/ScormPlayerLoader.aspx?FileID=" & FileID.ToString & "&GUID=" & UniqueID.ToString & "&NewsID=" & NewsID.ToString
                Case RepositoryItemType.VideoStreaming
                    Return ""
                Case Else
                    Return ""
            End Select
            Return ""
        End Get
    End Property

#Region "Add Notification"
    'Public Sub NotifyFileUploaded(ByVal Permission As Integer, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal Folder As String)
    '    Dim NewsID As System.Guid = System.Guid.NewGuid
    '    Dim FileUrl As String = Me.DownloadPage(FileID, NewsID)
    '    Me.NotifyUpload(Permission, NewsID, CommunityID, FolderID, FileID, FileName, Folder, FileUrl, CreateFileToNotify(FileID, RepositoryItemType.FileStandard))
    'End Sub
    Public Sub NotifyFileUploaded(ByVal Permission As Integer, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal UniqueID As System.Guid, ByVal FileName As String, ByVal Folder As String, type As RepositoryItemType)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim FileUrl As String = Me.ViewRepositoryItem(FileID, UniqueID, NewsID, type)
        Me.NotifyUpload(Permission, NewsID, CommunityID, FolderID, FileID, FileName, Folder, FileUrl, CreateFileToNotify(FileID, type))
    End Sub
    Public Sub NotifyFolderCreated(ByVal CommunityID As Integer, ByVal FatherID As Long, ByVal FolderID As Long, ByVal Folder As String, ByVal ParentFolder As String)
        Dim oValues = New List(Of String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
        oValues.Add(Folder)
        oValues.Add(ParentFolder)
        oValues.Add(Me.RepositoryViewPage(CommunityID, FatherID, NewsID))
        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionOnlySee, FolderID, Services_File.ObjectType.Folder, Services_File.ActionType.CreateFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FolderID))

        Dim NewsAdminID As System.Guid = System.Guid.NewGuid
        Dim AdminUrl As String = Me.RepositoryManagementPage(CommunityID, FolderID, NewsAdminID)
        oValues(0) = AdminUrl
        _Utility.SendNotificationToItemLong(NewsAdminID, Me.PermissionToAdmin, FolderID, Services_File.ObjectType.Folder, Services_File.ActionType.CreateFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FolderID))
    End Sub
    Public Sub NotifyFolderCreated(ByVal Permission As Integer, ByVal CommunityID As Integer, ByVal FatherID As Long, ByVal FolderID As Long, ByVal Folder As String, ByVal ParentFolder As String)
        Dim oValues = New List(Of String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        If Permission = Me.PermissionOnlySee Then
            oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
        ElseIf Permission = Me.PermissionToAdmin Then
            oValues.Add(Me.RepositoryManagementPage(CommunityID, FolderID, NewsID))
        Else
            oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
        End If

        oValues.Add(Folder)
        oValues.Add(ParentFolder)
        oValues.Add(Me.RepositoryViewPage(CommunityID, FatherID, NewsID))
        _Utility.SendNotificationToItemLong(NewsID, Permission, FolderID, Services_File.ObjectType.Folder, Services_File.ActionType.CreateFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FolderID))
    End Sub

    Private Sub NotifyUpload(ByVal Permission As Integer, ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal ItemId As Long, ByVal FileName As String, ByVal Folder As String, ByVal FileUrl As String, ByVal oDto As dtoNotificatedObject)
        Dim oValues = New List(Of String)
        oValues.Add(FileUrl)
        oValues.Add(FileName)
        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
        oValues.Add(Folder)
        _Utility.SendNotificationToItemLong(NewsID, Permission, ItemId, Services_File.ObjectType.File, Services_File.ActionType.UploadFile, CommunityID, Services_File.Codex, oValues, oDto)
    End Sub
#End Region

#Region "Delete Notification"
    Public Sub NotifyFileDeleted(ByVal CommunityID As Integer, ByVal FolderFatherID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal Folder As String, ByVal type As RepositoryItemType)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        Dim SeeUrl As String = Me.RepositoryViewPage(CommunityID, FolderFatherID, NewsID)

        oValues.Add(FileName)
        oValues.Add(SeeUrl)
        oValues.Add(Folder)
        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, FileID, type, Services_File.ActionType.DeleteFile, CommunityID, Services_File.Codex, oValues, CreateFileToNotify(FileID, type))
    End Sub
    Public Sub NotifyFolderDeleted(ByVal CommunityID As Integer, ByVal FolderFatherID As Long, ByVal FolderID As Long, ByVal Folder As String, ByVal ParentFolder As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        Dim SeeUrl As String = Me.RepositoryViewPage(CommunityID, FolderFatherID, NewsID)

        oValues.Add(Folder)
        oValues.Add(SeeUrl)
        oValues.Add(ParentFolder)
        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, FolderID, Services_File.ObjectType.Folder, Services_File.ActionType.DeleteFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FolderID))
    End Sub
#End Region

#Region "Move Notification"
    'Public Sub NotifyFileMoved(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String)
    '    Dim NewsID As System.Guid = System.Guid.NewGuid
    '    Dim FileUrl As String = Me.DownloadPage(FileID, NewsID)
    '    Me.NotifyMoved(NewsID, CommunityID, FolderID, FileID, FileName, FileUrl, FromFolder, ToFolder, CreateFileStandardToNotify(FileID))
    'End Sub
    Public Sub NotifyItemMoved(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal UniqueID As System.Guid, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal type As RepositoryItemType)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim FileUrl As String = Me.ViewRepositoryItem(FileID, UniqueID, NewsID, type)
        Me.NotifyMoved(NewsID, CommunityID, FolderID, FileID, FileName, FileUrl, FromFolder, ToFolder, CreateFileToNotify(FileID, type))
    End Sub
    Public Sub NotifyFolderMoved(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal ItemID As Long, ByVal Folder As String, ByVal FromFolder As String, ByVal ToFolder As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(Me.RepositoryViewPage(CommunityID, ItemID, NewsID))
        oValues.Add(Folder)
        oValues.Add(FromFolder)

        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
        oValues.Add(ToFolder)
        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, ItemID, Services_File.ObjectType.Folder, Services_File.ActionType.MoveFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(ItemID))
    End Sub
    Private Sub NotifyMoved(ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal ItemID As Long, ByVal FileName As String, ByVal FileUrl As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal oDto As dtoNotificatedObject)
        Dim oValues = New List(Of String)

        oValues.Add(FileUrl)
        oValues.Add(FileName)
        oValues.Add(FromFolder)
        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
        oValues.Add(ToFolder)
        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, ItemID, Services_File.ObjectType.File, Services_File.ActionType.MoveFile, CommunityID, Services_File.Codex, oValues, oDto)
    End Sub
#End Region

#Region "Visibility Notification"
    'Public Sub NotifyFileVisibility(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal Folder As String, ByVal isVisible As Boolean)
    '    Dim NewsID As System.Guid = System.Guid.NewGuid
    '    Dim FileUrl As String = ""
    '    If isVisible Then : FileUrl = Me.DownloadPage(FileID, NewsID)

    '    End If
    '    Me.NotifyVisibility(NewsID, CommunityID, FolderID, FileID, FileUrl, FileName, Folder, CreateFileStandardToNotify(FileID), isVisible)
    'End Sub
    Public Sub NotifyItemVisibility(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal UniqueID As System.Guid, ByVal FileName As String, ByVal Folder As String, ByVal isVisible As Boolean, ByVal type As RepositoryItemType)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim FileUrl As String = ""
        If isVisible Then : FileUrl = Me.ViewRepositoryItem(FileID, UniqueID, NewsID, type)

        End If
        Me.NotifyVisibility(NewsID, CommunityID, FolderID, FileID, FileUrl, FileName, Folder, CreateFileToNotify(FileID, type), isVisible)
    End Sub
    Public Sub NotifyFolderVisibility(ByVal CommunityID As Integer, ByVal FolderFatherID As Long, ByVal ItemID As Long, ByVal Folder As String, ByVal ParentFolder As String, ByVal isVisible As Boolean)
        Dim oValues = New List(Of String)
        Dim NewsID As System.Guid = System.Guid.NewGuid

        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderFatherID, NewsID))
        oValues.Add(ParentFolder)

        If isVisible Then : oValues.Add(Me.RepositoryViewPage(CommunityID, ItemID, NewsID))
        End If
        oValues.Add(Folder)


        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, ItemID, Services_File.ObjectType.Folder, IIf(isVisible, Services_File.ActionType.ShowFolder, Services_File.ActionType.HideFolder), CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(ItemID))
    End Sub
    Private Sub NotifyVisibility(ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal FileUrl As String, ByVal FileName As String, ByVal Folder As String, ByVal oDto As dtoNotificatedObject, ByVal isVisible As Boolean)
        Dim oValues = New List(Of String)
        If isVisible Then : oValues.Add(FileUrl)
        End If

        oValues.Add(FileName)
        oValues.Add(Me.RepositoryViewPage(CommunityID, FolderID, NewsID))
        oValues.Add(Folder)
        If isVisible Then
            _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, FileID, Services_File.ObjectType.File, IIf(isVisible, Services_File.ActionType.ShowFile, Services_File.ActionType.HideFile), CommunityID, Services_File.Codex, oValues, oDto)
        Else
            _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToAdmin, FileID, Services_File.ObjectType.File, IIf(isVisible, Services_File.ActionType.ShowFile, Services_File.ActionType.HideFile), CommunityID, Services_File.Codex, oValues, oDto)
        End If
    End Sub
#End Region

#Region "Import File"
    Public Sub NotifyFolderImport(ByVal CommunityID As Integer, ByVal FolderFatherID As Long, ByVal FolderName As String, ByVal Number As Integer)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim SeeUrl As String = Me.RepositoryViewPage(CommunityID, FolderFatherID, NewsID)
        Dim oValues = New List(Of String)
        oValues.Add(Number)
        oValues.Add(SeeUrl)
        oValues.Add(FolderName)
        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, FolderFatherID, Services_File.ObjectType.Folder, Services_File.ActionType.ImportFolders, CommunityID, Services_File.Codex, oValues, New List(Of dtoNotificatedObject))
    End Sub
    Public Sub NotifyFileImport(ByVal CommunityID As Integer, ByVal FolderFatherID As Long, ByVal FolderName As String, ByVal Number As Integer)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        Dim SeeUrl As String = Me.RepositoryViewPage(CommunityID, FolderFatherID, NewsID)
        oValues.Add(Number)
        oValues.Add(SeeUrl)
        oValues.Add(FolderName)
        _Utility.SendNotificationToItemLong(NewsID, Me.PermissionToSee, FolderFatherID, Services_File.ObjectType.Folder, Services_File.ActionType.ImportFiles, CommunityID, Services_File.Codex, oValues, New List(Of dtoNotificatedObject))
    End Sub
#End Region

#Region "Edit Permission"
    Public Sub NotifyItemPermissionModifyed(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
        Dim ActionID As Integer = Services_File.ActionType.None
        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
            ActionID = Services_File.ActionType.FolderPermissionModifyed
        Else
            ActionID = Services_File.ActionType.FilePermissionModifyed
        End If
        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    End Sub
    Public Sub NotifyItemPermissionModifyedToCommunity(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
        Dim ActionID As Integer = Services_File.ActionType.None
        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
            ActionID = Services_File.ActionType.FolderPermissionToCommunity
        Else
            ActionID = Services_File.ActionType.FilePermissionToCommunity
        End If
        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    End Sub
    Public Sub NotifyItemPermissionModifyedToSome(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
        Dim ActionID As Integer = Services_File.ActionType.None
        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
            ActionID = Services_File.ActionType.FolderPermissionToSome
        Else
            ActionID = Services_File.ActionType.FilePermissionToSome
        End If
        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    End Sub

    Private Sub NotifyItemPermissionModifyed(ByVal CommunityID As Integer, ByVal oContext As NotifyContext, ByVal ActionID As Integer)
        Dim oValues = New List(Of String)
        Dim oNotificated As dtoNotificatedObject = CreateObjectToNotify(oContext.ItemID, oContext.RepositoryItemType)
        Dim ObjectTypeID As Integer = CInt(oContext.RepositoryItemType)

        'If oContext.RepositoryItemType = RepositoryItemType.Folder Then
        '    oNotificated = CreateFolderToNotify(oContext.ItemID)
        'Else
        '    If oContext.ForItem = NotifyFor.File Then : oNotificated = CreateFileStandardToNotify()
        '    Else : oNotificated = CreateFileScormToNotify(oContext.ItemID)
        '    End If
        'End If

        If (oContext.ToUsers And NotifyTo.ToOwner) > 0 Then
            Dim UserNewsID As System.Guid = System.Guid.NewGuid
            Dim oPersons As New List(Of Integer)
            oPersons.Add(oContext.OwnerID)

            oValues = GenericGenerateValues(UserNewsID, CommunityID, oContext, False)
            GenericNotify(NotifyTo.ToOwner, CommunityID, UserNewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated, oPersons)
        End If

        If (oContext.ToUsers And NotifyTo.ToAdmin) > 0 Then
            Dim AdminNewsID As System.Guid = System.Guid.NewGuid
            oValues = GenericGenerateValues(AdminNewsID, CommunityID, oContext, True)

            GenericNotify(NotifyTo.ToAdmin, CommunityID, AdminNewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated)
        End If

        If (oContext.ToUsers And NotifyTo.ToAllSee) > 0 Then
            Dim NewsID As System.Guid = System.Guid.NewGuid
            oValues = GenericGenerateValues(NewsID, CommunityID, oContext, False)
            GenericNotify(NotifyTo.ToAllSee, CommunityID, NewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated)
        End If

        If (oContext.ToUsers And NotifyTo.ToOnlySee) > 0 Then
            Dim NewsID As System.Guid = System.Guid.NewGuid
            oValues = GenericGenerateValues(NewsID, CommunityID, oContext, False)
            GenericNotify(NotifyTo.ToOnlySee, CommunityID, NewsID, ActionID, ObjectTypeID, oContext.ItemID, oValues, oNotificated)
        End If
    End Sub
#End Region

#Region "Edit Permission"
    Public Sub NotifyItemModifyed(ByVal CommunityID As Integer, ByVal oContext As NotifyContext)
        Dim ActionID As Integer = Services_File.ActionType.None
        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
            ActionID = Services_File.ActionType.FolderEdited
        Else
            ActionID = Services_File.ActionType.FileEdited
        End If
        NotifyItemPermissionModifyed(CommunityID, oContext, ActionID)
    End Sub
#End Region

    Private Sub GenericNotify(ByVal ToUsers As NotifyTo, ByVal CommunityID As Integer, ByVal NewsID As System.Guid, ByVal ActionID As Integer, ByVal ObjectTypeID As Integer, ByVal ItemID As Long, ByVal oValues As List(Of String), ByVal oNotificated As dtoNotificatedObject, Optional ByVal oPersons As List(Of Integer) = Nothing)
        If ToUsers = NotifyTo.ToOwner Then
            _Utility.SendNotificationToPerson(NewsID, oPersons, ActionID, CommunityID, Services_File.Codex, oValues, oNotificated)
        Else
            Dim Permission As Integer = Me.PermissionOnlySee
            If ToUsers = NotifyTo.ToAllSee Then
                Permission = Me.PermissionToSee
            ElseIf ToUsers = NotifyTo.ToAdmin Then
                Permission = Me.PermissionToAdmin
            End If

            _Utility.SendNotificationToItemLong(NewsID, Permission, ItemID, ObjectTypeID, ActionID, CommunityID, Services_File.Codex, oValues, oNotificated)
        End If
    End Sub

    Private Function GenericGenerateValues(ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal oContext As NotifyContext, ByVal ForManage As Boolean) As List(Of String)
        Dim oValues = New List(Of String)
        oValues.Add(oContext.ItemName)
        If oContext.RepositoryItemType = RepositoryItemType.Folder Then
            If ForManage Then : oValues.Add(Me.RepositoryManagementPage(CommunityID, oContext.ItemID, NewsID))
            Else : oValues.Add(Me.RepositoryViewPage(CommunityID, oContext.ItemID, NewsID))
            End If
            'ElseIf oContext.ForItem = NotifyFor.File Then
            '    oValues.Add(Me.DownloadPage(oContext.ItemID, NewsID))
            'ElseIf oContext.ForItem = NotifyFor.FileScorm Then
            '    oValues.Add(Me.PlayScorm(oContext.ItemID, oContext.ScormID, NewsID))
            oValues.Add(Me.ViewRepositoryItem(oContext.ItemID, oContext.UniqueId, NewsID, oContext.RepositoryItemType))
        End If
        oValues.Add(oContext.FatherName)
        If ForManage Then : oValues.Add(Me.RepositoryManagementPage(CommunityID, oContext.ItemID, NewsID))
        Else : oValues.Add(Me.RepositoryViewPage(CommunityID, oContext.ItemID, NewsID))
        End If
        Return oValues
    End Function

#Region "Object To Notify"
    Private Function CreateFolderToNotify(ByVal FolderID As Long) As dtoNotificatedObject
        Return CreateObjectToNotify(FolderID, Services_File.ObjectType.Folder)
    End Function
    Private Function CreateFileToNotify(ByVal FileID As Long, ByVal type As RepositoryItemType) As dtoNotificatedObject
        Return CreateObjectToNotify(FileID, type)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As Long, ByVal type As RepositoryItemType) As lm.Notification.DataContract.Domain.dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = type
        obj.ModuleCode = Services_File.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_File.Codex)
        obj.FullyQualiFiedName = GetType(lm.Comol.Core.DomainModel.CommunityFile).FullName
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionOnlySee() As Integer
        Return Services_File.Base2Permission.DownloadFile 'AndAlso Not (Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate)
    End Function
    Public Function PermissionToSee() As Integer
        Return Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate Or Services_File.Base2Permission.DownloadFile
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate
    End Function
#End Region

    Class NotifyContext
        Public ItemID As Long
        Public ItemName As String
        Public FatherID As Long
        Public FatherName As String
        Public UniqueId As System.Guid
        Public OwnerID As Integer
        Public RepositoryItemType As RepositoryItemType
        Public ToUsers As NotifyTo
    End Class

    'Enum NotifyFor
    '    File
    '    FileScorm
    '    Folder
    'End Enum
    <Flags()> Enum NotifyTo
        ToOwner = 1
        ToAdmin = 2
        ToAllSee = 4
        ToOnlySee = 8
    End Enum
End Class
