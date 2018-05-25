Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class NoticeboardNotificationUtility
    Inherits BaseNotificationUtility

    Private ReadOnly Property BaseServiceUrl() As String
        Get
            Return MyBase._Utility.BaseUrl & "Generici/"
        End Get
    End Property

    Public ReadOnly Property BaseListUrl(ByVal NewsID As System.Guid, ByVal CommunityID As Integer, ByVal MessageID As Long) As String
        Get
            Return BaseServiceUrl & "CommunityNoticeboard.aspx?CommunityID=" & CommunityID.ToString & "&NewsID=" & NewsID.ToString & "&MessageID=" & MessageID.ToString
        End Get
    End Property

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

#Region "Notification"
    Public Sub NotifyAction(ByVal CommunityID As Integer, ByVal NoticeBoardID As Long, ByVal oActionType As Services_Bacheca.ActionType, ByVal Permission As Integer, ByVal values As List(Of String))
        If values Is Nothing Then
            values = New List(Of String)
        End If
        Me._Utility.SendNotificationToPermission(Permission, oActionType, CommunityID, Services_Bacheca.Codex, values, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub
    Public Sub NotifyCleanNoticeboard(ByVal CommunityID As Integer, ByVal NoticeBoardID As Long)

        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        Me._Utility.SendNotificationToPermission( _
            NewsID, _
            Me.PermissionToSee, _
            Services_Bacheca.ActionType.Clean, _
            CommunityID, _
            Services_Bacheca.Codex, _
            oValues, _
            CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub

    Public Sub NotifyAddMessage(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal ModifiedOn As DateTime?, ByVal ModifiedBy As String, ByVal UrlBase As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        oValues.Add(UrlBase & "&NewsID=" & NewsID.ToString)
        If ModifiedOn.HasValue Then
            oValues.Add(ModifiedOn.Value.ToString)
        Else
            oValues.Add("")
        End If
        oValues.Add(ModifiedBy)
        Me._Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_Bacheca.ActionType.Create, CommunityID, Services_Bacheca.Codex, oValues, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub

    Public Sub NotifyDeleteMessage(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal ModifiedOn As DateTime?, ByVal ModifiedBy As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        If ModifiedOn.HasValue Then
            oValues.Add(FormatDateTime(ModifiedOn.Value, DateFormat.LongDate))
        Else
            oValues.Add("")
        End If
        oValues.Add(ModifiedBy)
        Me._Utility.SendNotificationToPermission(NewsID, Me.PermissionToAdmin, Services_Bacheca.ActionType.Delete, CommunityID, Services_Bacheca.Codex, oValues, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub
    Public Sub NotifyUnDeleteMessage(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal ModifiedOn As DateTime?, ByVal ModifiedBy As String, ByVal UrlBase As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        oValues.Add(UrlBase & "&NewsID=" & NewsID.ToString)
        If ModifiedOn.HasValue Then
            oValues.Add(ModifiedOn.Value.ToString)
        Else
            oValues.Add("")
        End If
        oValues.Add(ModifiedBy)
        Me._Utility.SendNotificationToPermission(NewsID, Me.PermissionToAdmin, Services_Bacheca.ActionType.Undelete, CommunityID, Services_Bacheca.Codex, oValues, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub
    Public Sub NotifyVirtualDeleteMessage(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal ModifiedOn As DateTime?, ByVal ModifiedBy As String, ByVal UrlBase As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        oValues.Add(UrlBase & "&NewsID=" & NewsID.ToString)
        If ModifiedOn.HasValue Then
            oValues.Add(ModifiedOn.Value.ToString)
        Else
            oValues.Add("")
        End If
        oValues.Add(ModifiedBy)
        Me._Utility.SendNotificationToPermission(NewsID, Me.PermissionToAdmin, Services_Bacheca.ActionType.VirtualDelete, CommunityID, Services_Bacheca.Codex, oValues, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub
    Public Sub NotifyUndeleteAndActivate(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal ModifiedOn As DateTime?, ByVal ModifiedBy As String, ByVal UrlBase As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        oValues.Add(UrlBase & "&NewsID=" & NewsID.ToString)
        If ModifiedOn.HasValue Then
            oValues.Add(ModifiedOn.Value.ToString)
        Else
            oValues.Add("")
        End If
        oValues.Add(ModifiedBy)
        Me._Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_Bacheca.ActionType.UndeleteAndActivate, CommunityID, Services_Bacheca.Codex, oValues, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub

    Public Sub NotifySetDefault(ByVal NoticeBoardID As Long, ByVal CommunityID As Integer, ByVal ModifiedOn As DateTime?, ByVal ModifiedBy As String, ByVal UrlBase As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        oValues.Add(UrlBase & "&NewsID=" & NewsID.ToString)
        If ModifiedOn.HasValue Then
            oValues.Add(ModifiedOn.Value.ToString)
        Else
            oValues.Add("")
        End If
        oValues.Add(ModifiedBy)
        Me._Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_Bacheca.ActionType.SetDefault, CommunityID, Services_Bacheca.Codex, oValues, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub
    Public Sub NotifyEditMessage( _
                                ByVal NoticeBoardID As Long, _
                                ByVal CommunityID As Integer, _
                                ByVal ModifiedOn As DateTime?, _
                                ByVal ModifiedBy As String, _
                                ByVal UrlBase As String)

        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        oValues.Add(UrlBase & "&NewsID=" & NewsID.ToString)
        If ModifiedOn.HasValue Then
            oValues.Add(ModifiedOn.Value.ToString)
        Else
            oValues.Add("")
        End If
        oValues.Add(ModifiedBy)
        Me._Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_Bacheca.ActionType.Edit, CommunityID, Services_Bacheca.Codex, oValues, CreateObjectToNotify(NoticeBoardID, Services_Bacheca.ObjectType.WhiteBoard))
    End Sub


#End Region

#Region "Object To Notify"
    Private Function CreateObjectToNotify(ByVal ObjectID As Long, ByVal oType As Services_Cover.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_Bacheca.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_Bacheca.Codex)
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