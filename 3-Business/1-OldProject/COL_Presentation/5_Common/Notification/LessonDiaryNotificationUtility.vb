Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class LessonDiaryNotificationUtility
    Inherits BaseNotificationUtility

    Private ReadOnly Property BaseServiceUrl() As String
        Get
            Return MyBase._Utility.BaseUrl & "Modules/CommunityDiary/"
        End Get
    End Property

    Public ReadOnly Property BaseListUrl(ByVal NewsID As System.Guid, ByVal CommunityID As Integer) As String
        Get
            Return BaseServiceUrl & "CommunityDiary.aspx?CommunityID=" & CommunityID.ToString & "&NewsID=" & NewsID.ToString
        End Get
    End Property
    Private ReadOnly Property RepositoryManagementPage(ByVal CommunityID As String, ByVal ItemID As Long, ByVal NewsID As System.Guid) As String
        Get
            Return BaseServiceUrl & "ManagementFile.aspx?CommunityID=" & CommunityID.ToString & "&ItemID=" & ItemID.ToString & "&NewsID=" & NewsID.ToString
        End Get
    End Property

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

    Public Sub NotifyAddItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        Dim Permission As Integer = Me.PermissionToSee
        If isvisible Then
            oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        Else
            Permission = Me.PermissionToAdmin
            oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        End If

        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.GeneralDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(EndDate, DateFormat.ShortTime))

        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.CreateItem, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyAddItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        Dim Permission As Integer = Me.PermissionToSee
        If isvisible Then
            oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        Else
            Permission = Me.PermissionToAdmin
            oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        End If

        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.CreateItemNoDate, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyRemoveItem(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal isvisible As Boolean)
        Dim oValues = New List(Of String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.GeneralDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(EndDate, DateFormat.ShortTime))

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If

        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.DeleteItem, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyEditItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        Dim Permission As Integer = Me.PermissionToSee
        If isvisible Then
            oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        Else
            Permission = Me.PermissionToAdmin
            oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        End If

        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.EditItemNoDate, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyEditItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal OldStart As DateTime, ByVal OldEnd As DateTime, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(FormatDateTime(OldStart.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(OldStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(OldEnd, DateFormat.ShortTime))

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.ChangeItem, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub

    Public Sub NotifyMoveItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal OldStart As DateTime, ByVal OldEnd As DateTime, ByVal NewStart As DateTime, ByVal NewEnd As DateTime, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(FormatDateTime(OldStart.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(OldStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(OldEnd, DateFormat.ShortTime))

        oValues.Add(FormatDateTime(NewStart.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(NewStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(NewEnd, DateFormat.ShortTime))

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.MoveItem, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub

    Public Sub NotifyAddFilesToItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal Title As String, ByVal FileNumber As Integer, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(Title)
        oValues.Add(FileNumber)

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.AddMultipleFilesNoDate, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyAddFilesToItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal Enddate As DateTime, ByVal FileNumber As Integer, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(Enddate, DateFormat.ShortTime))
        oValues.Add(FileNumber)

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.AddFiles, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyRemoveFilesToItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal Enddate As DateTime, ByVal FileNumber As Integer, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(Enddate, DateFormat.ShortTime))
        oValues.Add(FileNumber)

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.RemoveFiles, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub


    Public Sub NotifyAddFileToItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal Enddate As DateTime, ByVal FileName As String, ByVal FileUrl As String, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(Enddate, DateFormat.ShortTime))

        oValues.Add(FileUrl)
        oValues.Add(FileName)

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.AddFile, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyAddFileToItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal Title As String, ByVal FileName As String, ByVal FileUrl As String, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(Title)
        oValues.Add(FileUrl)
        oValues.Add(FileName)

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.AddFileToItemNoDate, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub
    Public Sub NotifyRemoveFileToItem(ByVal CommunityID As Integer, ByVal ItemID As Integer, ByVal StartDate As DateTime, ByVal Enddate As DateTime, ByVal FileName As String, ByVal isvisible As Boolean)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseListUrl(NewsID, CommunityID) & "#" & ItemID.ToString))
        oValues.Add(FormatDateTime(StartDate.Date, DateFormat.ShortDate))
        oValues.Add(FormatDateTime(StartDate, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(Enddate, DateFormat.ShortTime))
        oValues.Add(FileName)

        Dim Permission As Integer = Me.PermissionToSee
        If Not isvisible Then
            Permission = Me.PermissionToAdmin
        End If
        _Utility.SendNotificationToPermission(NewsID, Permission, Services_DiarioLezioni.ActionType.RemoveFile, CommunityID, Services_DiarioLezioni.Codex, oValues, CreateDiaryItem(ItemID))
    End Sub

    Public Sub NotifyDeleteDiary(ByVal CommunityID As Integer)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)

        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToSee, Services_DiarioLezioni.ActionType.DeleteDiary, CommunityID, Services_DiarioLezioni.Codex, oValues)
    End Sub


#Region "Object To Notify"
    Private Function CreateDiaryItem(ByVal ItemID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(ItemID, Services_DiarioLezioni.ObjectType.DiaryItem)
    End Function
    Private Function CreateFile(ByVal FileScormID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(FileScormID, Services_DiarioLezioni.ObjectType.FileScorm)
    End Function
    Private Function CreateFileStandardToNotify(ByVal FileID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(FileID, Services_DiarioLezioni.ObjectType.File)
    End Function

    Private Function CreateObjectToNotify(ByVal ObjectID As Integer, ByVal oType As Services_DiarioLezioni.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_DiarioLezioni.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_DiarioLezioni.Codex)
        Select Case oType
            Case Services_DiarioLezioni.ObjectType.DiaryItem
                obj.FullyQualiFiedName = GetType(lm.Comol.Core.DomainModel.CommunityEventItem).FullName
            Case Services_DiarioLezioni.ObjectType.File, Services_DiarioLezioni.ObjectType.FileScorm
                obj.FullyQualiFiedName = GetType(lm.Comol.Core.DomainModel.BaseCommunityFile).FullName
        End Select

        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_DiarioLezioni.Base2Permission.AdminService Or Services_DiarioLezioni.Base2Permission.ViewLessons
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_DiarioLezioni.Base2Permission.AdminService
    End Function
#End Region

End Class