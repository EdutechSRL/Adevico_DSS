Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class CoverNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub



#Region "Add Notification"
    Public Sub NotifyAdd(ByVal CommunityID As Integer, ByVal CoverID As Integer)
        _Utility.SendNotificationToPermission(PermissionToSee, Services_Cover.ActionType.Create, CommunityID, Services_Cover.Codex, New List(Of String), CreateObjectToNotify(CoverID, Services_Cover.ObjectType.Cover))
    End Sub
    Public Sub NotifyEdit(ByVal CommunityID As Integer, ByVal CoverID As Integer)
        _Utility.SendNotificationToPermission(PermissionToSee, Services_Cover.ActionType.Edit, CommunityID, Services_Cover.Codex, New List(Of String), CreateObjectToNotify(CoverID, Services_Cover.ObjectType.Cover))
    End Sub
    Public Sub NotifyDelete(ByVal CommunityID As Integer, ByVal CoverID As Integer)
        _Utility.SendNotificationToPermission(PermissionToSee, Services_Cover.ActionType.Delete, CommunityID, Services_Cover.Codex, New List(Of String), CreateObjectToNotify(CoverID, Services_Cover.ObjectType.Cover))
    End Sub
#End Region



#Region "Object To Notify"
    Private Function CreateObjectToNotify(ByVal ObjectID As Long, ByVal oType As Services_Cover.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_Cover.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_Cover.Codex)
        obj.FullyQualiFiedName = GetType(COL_BusinessLogic_v2.COL_Cover).FullName

        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_Cover.Base2Permission.AdminService Or Services_Cover.Base2Permission.Management OrElse Services_Cover.Base2Permission.ViewCover
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_Cover.Base2Permission.AdminService Or Services_Cover.Base2Permission.Management
    End Function
#End Region

End Class
