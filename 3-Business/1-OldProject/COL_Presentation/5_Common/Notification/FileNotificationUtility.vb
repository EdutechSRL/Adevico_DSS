Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class FileNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub


#Region "Add Notification"
    Public Sub NotifyFileUploaded(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileName As String, ByVal Folder As String, ByVal FileUrl As String)
        Me.NotifyUpload(CommunityID, FileID, FileName, Folder, FileUrl, CreateFileStandardToNotify(FileID))
    End Sub
    Public Sub NotifyScormUploaded(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileName As String, ByVal Folder As String, ByVal FileUrl As String)
        Me.NotifyUpload(CommunityID, FileID, FileName, Folder, FileUrl, CreateFileScormToNotify(FileID))
    End Sub
    Public Sub NotifyFolderCreated(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal Folder As String, ByVal ParentFolder As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(Folder)
        oValues.Add(ParentFolder)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_File.ActionType.CreateFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FileID))
    End Sub
    Private Sub NotifyUpload(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileName As String, ByVal Folder As String, ByVal FileUrl As String, ByVal oDto As dtoNotificatedObject)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(FileUrl)
        oValues.Add(FileName)
        oValues.Add(Folder)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_File.ActionType.UploadFile, CommunityID, Services_File.Codex, oValues, oDto)
    End Sub
#End Region

#Region "Delete Notification"
    Public Sub NotifyFileDeleted(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileName As String, ByVal Folder As String, ByVal isScormFile As Boolean)
        Dim oValues = New List(Of String)
        oValues.Add(FileName)
        oValues.Add(Folder)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_File.ActionType.DeleteFile, CommunityID, Services_File.Codex, oValues, IIf(isScormFile, CreateFileScormToNotify(FileID), CreateFileStandardToNotify(FileID)))
    End Sub
    Public Sub NotifyFolderDeleted(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal Folder As String, ByVal ParentFolder As String)
        Dim oValues = New List(Of String)
        oValues.Add(Folder)
        oValues.Add(ParentFolder)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_File.ActionType.DeleteFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FileID))
    End Sub
#End Region

#Region "Move Notification"
    Public Sub NotifyFileMoved(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal FileUrl As String)
        Me.NotifyMoved(CommunityID, FileID, FileName, FromFolder, ToFolder, FileUrl, CreateFileStandardToNotify(FileID))
    End Sub
    Public Sub NotifyScormFileMoved(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal FileUrl As String)
        Me.NotifyMoved(CommunityID, FileID, FileName, FromFolder, ToFolder, FileUrl, CreateFileScormToNotify(FileID))
    End Sub
    Public Sub NotifyFolderMoved(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal Folder As String, ByVal FromFolder As String, ByVal ToFolder As String)
        Dim oValues = New List(Of String)
        oValues.Add(Folder)
        oValues.Add(FromFolder)
        oValues.Add(ToFolder)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_File.ActionType.MoveFolder, CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FileID))
    End Sub
    Private Sub NotifyMoved(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal FileUrl As String, ByVal oDto As dtoNotificatedObject)
        Dim oValues = New List(Of String)
        oValues.Add(FileUrl)
        oValues.Add(FileName)
        oValues.Add(FromFolder)
        oValues.Add(ToFolder)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_File.ActionType.MoveFile, CommunityID, Services_File.Codex, oValues, oDto)
    End Sub
#End Region

#Region "Visibility Notification"
    Public Sub NotifyFileVisibility(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileUrl As String, ByVal FileName As String, ByVal Folder As String, ByVal isVisible As Boolean)
        Me.NotifyVisibility(CommunityID, FileID, FileUrl, FileName, Folder, CreateFileStandardToNotify(FileID), isVisible)
    End Sub
    Public Sub NotifyScormFileVisibility(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileUrl As String, ByVal FileName As String, ByVal Folder As String, ByVal isVisible As Boolean)
        Me.NotifyVisibility(CommunityID, FileID, FileUrl, FileName, Folder, CreateFileScormToNotify(FileID), isVisible)
    End Sub
    Public Sub NotifyFolderVisibility(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal Folder As String, ByVal ParentFolder As String, ByVal isVisible As Boolean)
        Dim oValues = New List(Of String)
        oValues.Add(Folder)
        oValues.Add(ParentFolder)

        _Utility.SendNotificationToPermission(Me.PermissionToSee, IIf(isVisible, Services_File.ActionType.ShowFolder, Services_File.ActionType.HideFolder), CommunityID, Services_File.Codex, oValues, CreateFolderToNotify(FileID))
    End Sub
    Private Sub NotifyVisibility(ByVal CommunityID As Integer, ByVal FileID As Integer, ByVal FileUrl As String, ByVal FileName As String, ByVal Folder As String, ByVal oDto As dtoNotificatedObject, ByVal isVisible As Boolean)
        Dim oValues = New List(Of String)
        If isVisible Then : oValues.Add(FileUrl)
        End If

        oValues.Add(FileName)
        oValues.Add(Folder)
        _Utility.SendNotificationToPermission(Me.PermissionToSee, IIf(isVisible, Services_File.ActionType.ShowFile, Services_File.ActionType.HideFile), CommunityID, Services_File.Codex, oValues, oDto)
    End Sub
#End Region

#Region "Import File"
    Public Sub NotifyFolderImport(ByVal CommunityID As Integer, ByVal Number As Integer)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(Number)
        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_File.ActionType.ImportFolders, CommunityID, Services_File.Codex, oValues, New List(Of dtoNotificatedObject))
    End Sub
    Public Sub NotifyFileImport(ByVal CommunityID As Integer, ByVal Number As Integer)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(Number)

        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_File.ActionType.ImportFiles, CommunityID, Services_File.Codex, oValues, New List(Of dtoNotificatedObject))
    End Sub
#End Region

#Region "Object To Notify"
    Private Function CreateFolderToNotify(ByVal FolderID As Long) As dtoNotificatedObject
        Return CreateObjectToNotify(FolderID, Services_File.ObjectType.Folder)
    End Function
    Private Function CreateFileScormToNotify(ByVal FileScormID As Long) As dtoNotificatedObject
        Return CreateObjectToNotify(FileScormID, Services_File.ObjectType.FileScorm)
    End Function
    Private Function CreateFileStandardToNotify(ByVal FileID As Long) As dtoNotificatedObject
        Return CreateObjectToNotify(FileID, Services_File.ObjectType.File)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As Long, ByVal oType As Services_File.ObjectType) As lm.Notification.DataContract.Domain.dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_File.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_File.Codex)
        obj.FullyQualiFiedName = GetType(lm.Comol.Core.DomainModel.CommunityFile).FullName
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate Or Services_File.Base2Permission.DownloadFile
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate
    End Function
#End Region

End Class
