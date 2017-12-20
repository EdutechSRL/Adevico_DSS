Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class EventNotificationUtility
    Inherits BaseNotificationUtility


    Public ReadOnly Property BaseServiceUrl(ByVal CommunityID As Integer) As String
        Get
            Return "Eventi/LoadEvent.aspx?CommunityID=" & CommunityID.ToString & "&EventID="
        End Get
    End Property

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

#Region "ADD"
    Public Sub NotifyAddToCommunity(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
        Me.NotifyAddTo(CommunityID, ItemID, StartDate, EndDate, CreateCommunityEvent(ItemID))
    End Sub
    'Public Sub NotifyAddToPersonal(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
    '    Me.NotifyAddTo(CommunityID, ItemID, StartDate, EndDate, CreatePersonalEvent(ItemID))
    'End Sub
    Public Sub NotifyAddTo(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal oDto As dtoNotificatedObject)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(CommunityID) & ItemID.ToString))
        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.GeneralDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(EndDate, DateFormat.ShortTime))

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_Eventi.ActionType.AddEvent, CommunityID, Services_Eventi.Codex, oValues, oDto)
    End Sub
#End Region

#Region "MOVE"
    Public Sub NotifyMoveFromCommunity(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal OldStart As DateTime, ByVal OldEnd As DateTime, ByVal NewStart As DateTime, ByVal NewEnd As DateTime)
        Me.NotifyMoveFrom(CommunityID, ItemID, OldStart, OldEnd, NewStart, NewEnd, CreateCommunityEvent(ItemID))
    End Sub
    'Public Sub NotifyMoveFromPersonal(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal OldStart As DateTime, ByVal OldEnd As DateTime, ByVal NewStart As DateTime, ByVal NewEnd As DateTime)
    '    Me.NotifyMoveFrom(CommunityID, ItemID, OldStart, OldEnd, NewStart, NewEnd, CreatePersonalEvent(ItemID))
    'End Sub
    Public Sub NotifyMoveFrom(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal OldStart As DateTime, ByVal OldEnd As DateTime, ByVal NewStart As DateTime, ByVal NewEnd As DateTime, ByVal oDto As dtoNotificatedObject)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(CommunityID) & "#" & ItemID.ToString))
        oValues.Add(FormatDateTime(OldStart.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(OldStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(OldEnd, DateFormat.ShortTime))

        oValues.Add(FormatDateTime(NewStart.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(NewStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(NewEnd, DateFormat.ShortTime))
        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_Eventi.ActionType.MoveItem, CommunityID, Services_Eventi.Codex, oValues, oDto)
    End Sub
#End Region

    Public Sub NotifyRemoveItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
        Dim oValues = New List(Of String)
        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.GeneralDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(EndDate, DateFormat.ShortTime))

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_Eventi.ActionType.DeleteEvent, CommunityID, Services_Eventi.Codex, oValues, CreateCommunityEvent(ItemID))
    End Sub
    Public Sub NotifyEditItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal OldStart As DateTime, ByVal OldEnd As DateTime)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(CommunityID) & ItemID.ToString))
        oValues.Add(FormatDateTime(OldStart.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(OldStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(OldEnd, DateFormat.ShortTime))

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_Eventi.ActionType.EditEvent, CommunityID, Services_Eventi.Codex, oValues, CreateCommunityEvent(ItemID))
    End Sub


#Region "Object To Notify"
    Private Function CreateCommunityEvent(ByVal ItemID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(ItemID, Services_Eventi.ObjectType.CommunityEvent)
    End Function
    Private Function CreatePersonalEvent(ByVal ItemID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(ItemID, Services_Eventi.ObjectType.ReminderEvent)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As Integer, ByVal oType As Services_Eventi.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_Eventi.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_Eventi.Codex)
        Select Case oType
            Case Services_Eventi.ObjectType.CommunityEvent
                obj.FullyQualiFiedName = GetType(COL_BusinessLogic_v2.Eventi.COL_Orario).FullName
            Case Services_Eventi.ObjectType.ReminderEvent
                obj.FullyQualiFiedName = GetType(COL_BusinessLogic_v2.Eventi.COL_Reminder).FullName
        End Select

        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_Eventi.Base2Permission.AdminService Or Services_Eventi.Base2Permission.ViewEvents
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_Eventi.Base2Permission.AdminService
    End Function
    Public Function PermissionToChange() As Integer
        Return Services_Eventi.Base2Permission.AdminService Or Services_Eventi.Base2Permission.AddEvents Or Services_Eventi.Base2Permission.DeleteEvent Or Services_Eventi.Base2Permission.EditEvent
    End Function
#End Region

End Class