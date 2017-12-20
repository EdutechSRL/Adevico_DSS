Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class WorkBookNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

    Public ReadOnly Property BaseServiceUrl(ByVal WorkBookID As System.Guid, ByVal oView As String) As String
        Get
            Return "Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView
        End Get
    End Property

    Public ReadOnly Property BaseServiceItemUrl(ByVal WorkBookID As System.Guid, ByVal oView As String, ByVal WorkBookItemID As System.Guid) As String
        Get
            'Return "Generici/WorkBook.aspx#" & WorkBookItemID.ToString & "?WorkBookID=" & WorkBookID.ToString & "&View=" & IIf(isPersonal, 0, 2)
            Return "Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView & "#" & WorkBookItemID.ToString
        End Get
    End Property
#Region "Creazione Workbook"
    Public Sub NotifyPersonalWorkBookCreated(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        oValues.Add(Name)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.CreateWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub
    Public Sub NotifyCommunityWorkBookCreated(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        oValues.Add(Name)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.CreateWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.CreateWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub
    Public Sub NotifyPersonalWorkBookEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        oValues.Add(Name)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.EditWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub
    Public Sub NotifyCommunityWorkBookEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        oValues.Add(Name)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.EditWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.EditWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub

    Public Sub NotifyPersonalWorkBookVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.VirtualDeleteWorkBook, CommunityID, Services_WorkBook.Codex, GetValuesToDelete(CommunityID, Name, CreatorName), CreateWorkBookToNotify(WorkBookID))
    End Sub
    Public Sub NotifyCommunityWorkBookVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Dim oValues As List(Of String) = GetValuesToDelete(CommunityID, Name, CreatorName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.VirtualDeleteWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.VirtualDeleteWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub
    Public Sub NotifyPersonalWorkBookVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToDelete(CommunityID, Name, CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.UndeleteWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub
    Public Sub NotifyCommunityWorkBookVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToDelete(CommunityID, Name, CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.UndeleteWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.UndeleteWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub


    Public Sub NotifyPersonalWorkBookDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.DeleteWorkBook, CommunityID, Services_WorkBook.Codex, GetValuesToDelete(CommunityID, Name, CreatorName), CreateWorkBookToNotify(WorkBookID))
    End Sub
    Public Sub NotifyCommunityWorkBookDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Dim oValues As List(Of String) = GetValuesToDelete(CommunityID, Name, CreatorName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.DeleteWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.DeleteWorkBook, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookToNotify(WorkBookID))
    End Sub


    Private Function GetValuesToDelete(ByVal CommunityID As Integer, ByVal Name As String, ByVal CreatorName As String) As List(Of String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        '   oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, False)))
        oValues.Add(Name)
        Return oValues
    End Function
#End Region

#Region "WorkbookItem"
    Public Sub NotifyItemAdd(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItem(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, WorkBookItemID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.CreateItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.CreateItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub
    Public Sub NotifyItemEdit(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItem(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, WorkBookItemID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.EditItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.EditItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub
    Public Sub NotifyItemVirtualDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItemDeleted(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.VirtualDeleteItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.VirtualDeleteItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub
    Public Sub NotifyItemVirtualUnDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItem(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, WorkBookItemID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.UndeleteItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.UndeleteItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub
    Public Sub NotifyItemDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItemDeleted(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.DeleteItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.DeleteItem, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub

    Public Sub NotifyItemVisibilityOn(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid, ByVal CreatorName As String)
        Dim oValues = New List(Of String)
        'oValues.Add(Name)
        'oValues.Add(ActivationUrl(CommunityID, PersonID))
        'oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl))

    End Sub
    Public Sub NotifyItemVisibilityOff(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid, ByVal CreatorName As String)
        Dim oValues = New List(Of String)
        'oValues.Add(Name)
        'oValues.Add(ActivationUrl(CommunityID, PersonID))
        'oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl))

    End Sub


    Public Sub NotifyItemAddCommunityFileLink(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItem(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, WorkBookItemID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.UploadCommunityFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.UploadCommunityFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub
    Public Sub NotifyItemAddInternalFileLink(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItem(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, WorkBookItemID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.UploadInternalFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.UploadInternalFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub
    Public Sub NotifyItemUnlinkCommunityFileLink(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItem(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, WorkBookItemID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.UnlinkCommunityFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.UnlinkCommunityFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub
    Public Sub NotifyItemRemoveInternalFileLink(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer), ByVal ViewName As String)
        Dim oValues As List(Of String) = GetValuesToItem(CommunityID, WorkBookName, ItemName, ItemData, CreatorName, WorkBookID, WorkBookItemID, ViewName)

        _Utility.SendNotificationToPerson(Authors, Services_WorkBook.ActionType.DeleteInternalFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        If Not isPersonal Then
            _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_WorkBook.ActionType.DeleteInternalFile, CommunityID, Services_WorkBook.Codex, oValues, CreateWorkBookItemToNotify(WorkBookItemID))
        End If
    End Sub

    Private Function GetValuesToItem(ByVal CommunityID As Integer, ByVal WorkBookName As String, ByVal ItemName As String, ByVal CreatorName As String, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid, ByVal ViewName As String) As List(Of String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceItemUrl(WorkBookID, ViewName, WorkBookItemID)))
        oValues.Add(ItemName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        oValues.Add(WorkBookName)

        Return oValues
    End Function
    Private Function GetValuesToItem(ByVal CommunityID As Integer, ByVal WorkBookName As String, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid, ByVal ViewName As String) As List(Of String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceItemUrl(WorkBookID, ViewName, WorkBookItemID)))
        oValues.Add(ItemName)
        oValues.Add(FormatDateTime(ItemData, DateFormat.ShortDate))
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        oValues.Add(WorkBookName)

        Return oValues
    End Function
    Private Function GetValuesToItemDeleted(ByVal CommunityID As Integer, ByVal WorkBookName As String, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal WorkBookID As System.Guid, ByVal ViewName As String) As List(Of String)
        Dim oValues = New List(Of String)
        oValues.Add(CreatorName)
        oValues.Add(ItemName)
        oValues.Add(FormatDateTime(ItemData, DateFormat.ShortDate))
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(WorkBookID, ViewName)))
        oValues.Add(WorkBookName)

        Return oValues
    End Function
#End Region

#Region "Object To Notify"
    Private Function CreateWorkBookToNotify(ByVal WorkBookID As System.Guid) As dtoNotificatedObject
        Return CreateObjectToNotify(WorkBookID, Services_WorkBook.ObjectType.WorkBook)
    End Function
    Private Function CreateWorkBookItemToNotify(ByVal ItemID As System.Guid) As dtoNotificatedObject
        Return CreateObjectToNotify(ItemID, Services_WorkBook.ObjectType.WorkBookItem)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As System.Guid, ByVal oType As Services_WorkBook.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_WorkBook.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_WorkBook.Codex)
        If oType = Services_WorkBook.ObjectType.WorkBook Then
            obj.FullyQualiFiedName = GetType(lm.Comol.Modules.Base.DomainModel.WorkBook).FullName
        ElseIf oType = Services_WorkBook.ObjectType.WorkBookItem Then
            obj.FullyQualiFiedName = GetType(lm.Comol.Modules.Base.DomainModel.WorkBookItem).FullName
        End If
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_WorkBook.Base2Permission.AdminService ' Or Services_WorkBook.Base2Permission.
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_WorkBook.Base2Permission.AdminService Or Services_WorkBook.Base2Permission.ChangeOtherWorkbook
    End Function
#End Region

End Class