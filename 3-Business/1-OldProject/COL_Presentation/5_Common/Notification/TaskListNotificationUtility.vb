Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class TaskListNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub


    Private ReadOnly Property BaseServiceUrl() As String
        Get
            Return MyBase._Utility.BaseUrl & "Modules/TaskList/"
        End Get
    End Property
    Private ReadOnly Property BaseServiceScormUrl() As String
        Get
            Return MyBase._Utility.BaseUrl & "Modules/"
        End Get
    End Property
    Private ReadOnly Property BaseServiceRepositoryUrl() As String
        Get
            Return MyBase._Utility.BaseUrl & "Modules/epository"
        End Get
    End Property
    Private ReadOnly Property RepositoryViewPage(ByVal CommunityID As String, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceUrl & "PAGINA TASKLIST MANAGEMENT.aspx?CommunityID=" & CommunityID.ToString & "&NewsID=" & NewsID.ToString
        End Get
    End Property
    Private ReadOnly Property RepositoryManagementPage(ByVal CommunityID As String, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceUrl & "PAGINA TASKLIST MANAGEMENT.aspx?CommunityID=" & CommunityID.ToString & "&NewsID=" & NewsID.ToString
        End Get
    End Property
    Private ReadOnly Property DownloadPage(ByVal FileID As Long, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceRepositoryUrl & "File.repository?FileID=" & FileID.ToString & "&NewsID=" & NewsID.ToString
        End Get
    End Property
    Private ReadOnly Property PlayScorm(ByVal FileID As Long, ByVal UniqueID As System.Guid, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceScormUrl & "Scorm/RedirectToScormPlayer.aspx?Id=" & FileID.ToString & "&GUID=" & UniqueID.ToString & "&NewsID=" & NewsID.ToString
        End Get
    End Property



    '    Private Sub GenericNotify(ByVal ToUsers As NotifyTo, ByVal CommunityID As Integer, ByVal NewsID As System.Guid, ByVal ActionID As Integer, ByVal ObjectTypeID As Integer, ByVal ItemID As Long, ByVal oValues As List(Of String), ByVal oNotificated As dtoNotificatedObject, Optional ByVal oPersons As List(Of Integer) = Nothing)
    '        If ToUsers = NotifyTo.ToOwner Then
    '            _Utility.SendNotificationToPerson(NewsID, oPersons, ActionID, CommunityID, Services_File.Codex, oValues, oNotificated)
    '        Else
    '            Dim Permission As Integer = Me.PermissionOnlySee
    '            If ToUsers = NotifyTo.ToAllSee Then
    '                Permission = Me.PermissionToSee
    '            ElseIf ToUsers = NotifyTo.ToAdmin Then
    '                Permission = Me.PermissionToAdmin
    '            End If

    '            _Utility.SendNotificationToItemLong(NewsID, Permission, ItemID, ObjectTypeID, ActionID, CommunityID, Services_File.Codex, oValues, oNotificated)
    '        End If
    '    End Sub

    '    Private Function GenericGenerateValues(ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal oContext As NotifyContext, ByVal ForManage As Boolean) As List(Of String)
    '        Dim oValues = New List(Of String)
    '        oValues.Add(oContext.ItemName)
    '        If oContext.ForItem = NotifyFor.Folder Then
    '            If ForManage Then : oValues.Add(Me.RepositoryManagementPage(CommunityID, oContext.ItemID, NewsID))
    '            Else : oValues.Add(Me.RepositoryViewPage(CommunityID, oContext.ItemID, NewsID))
    '            End If
    '        ElseIf oContext.ForItem = NotifyFor.File Then
    '            oValues.Add(Me.DownloadPage(oContext.ItemID, NewsID))
    '        ElseIf oContext.ForItem = NotifyFor.FileScorm Then
    '            oValues.Add(Me.PlayScorm(oContext.ItemID, oContext.ScormID, NewsID))
    '        End If
    '        oValues.Add(oContext.FatherName)
    '        If ForManage Then : oValues.Add(Me.RepositoryManagementPage(CommunityID, oContext.ItemID, NewsID))
    '        Else : oValues.Add(Me.RepositoryViewPage(CommunityID, oContext.ItemID, NewsID))
    '        End If
    '        Return oValues
    '    End Function

    '#Region "Object To Notify"
    '    Private Function CreateFolderToNotify(ByVal FolderID As Long) As dtoNotificatedObject
    '        Return CreateObjectToNotify(FolderID, Services_File.ObjectType.Folder)
    '    End Function
    '    Private Function CreateFileScormToNotify(ByVal FileScormID As Long) As dtoNotificatedObject
    '        Return CreateObjectToNotify(FileScormID, Services_File.ObjectType.FileScorm)
    '    End Function
    '    Private Function CreateFileStandardToNotify(ByVal FileID As Long) As dtoNotificatedObject
    '        Return CreateObjectToNotify(FileID, Services_File.ObjectType.File)
    '    End Function
    '    Private Function CreateObjectToNotify(ByVal ObjectID As Long, ByVal oType As Services_File.ObjectType) As lm.Notification.DataContract.Domain.dtoNotificatedObject
    '        Dim obj As New dtoNotificatedObject
    '        obj.ObjectID = ObjectID.ToString
    '        obj.ObjectTypeID = oType
    '        obj.ModuleCode = Services_File.Codex
    '        obj.ModuleID = MyBase._Utility.GetModuleID(Services_File.Codex)
    '        obj.FullyQualiFiedName = GetType(lm.Comol.Core.DomainModel.CommunityFile).FullName
    '        Return obj
    '    End Function
    '#End Region

    '#Region "Permission Utility"
    '    Public Function PermissionOnlySee() As Integer
    '        Return Services_File.Base2Permission.DownloadFile 'AndAlso Not (Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate)
    '    End Function
    '    Public Function PermissionToSee() As Integer
    '        Return Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate Or Services_File.Base2Permission.DownloadFile
    '    End Function
    '    Public Function PermissionToAdmin() As Integer
    '        Return Services_File.Base2Permission.AdminService Or Services_File.Base2Permission.Moderate
    '    End Function
    '#End Region

    '    Class NotifyContext
    '        Public ItemID As Long
    '        Public ItemName As String
    '        Public FatherID As Long
    '        Public FatherName As String
    '        Public ScormID As System.Guid
    '        Public OwnerID As Integer
    '        Public ForItem As NotifyFor
    '        Public ToUsers As NotifyTo
    '    End Class

    Enum NotifyFor
        File
        FileScorm
        Task
        Project
    End Enum
    <Flags()> Enum NotifyTo
        ToOwner = 1
        ToAdmin = 2
        ToAllSee = 4
        ToOnlySee = 8
    End Enum
End Class
