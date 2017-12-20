Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class SubscriptionNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

    Public ReadOnly Property BaseServiceUrl() As String
        Get
            Return "Comunita/GestioneIscritti.aspx"
        End Get
    End Property
    Public ReadOnly Property ActivationUrl(ByVal CommunityID As Integer, ByVal PersonID As Integer) As String
        Get
            Return "../Comunita/UserActivation.aspx?CommunityID=" & CommunityID.ToString & "&PersonID=" & PersonID.ToString
        End Get
    End Property

    Public Sub NotifyAddWaitingSubscription(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal Name As String)
        Dim oValues = New List(Of String)
        oValues.Add(Name)
        oValues.Add(ActivationUrl(CommunityID, PersonID))
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl))

        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_GestioneIscritti.ActionType.SelfWaitingSubscription, CommunityID, Services_GestioneIscritti.Codex, oValues, CreatePersonToNotify(PersonID))
    End Sub
    Public Sub NotifyAddSelfSubscription(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal Name As String)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl))
        oValues.Add(Name)

        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_GestioneIscritti.ActionType.SelfSubscription, CommunityID, Services_GestioneIscritti.Codex, oValues, CreatePersonToNotify(PersonID))
    End Sub
    Public Sub NotifyRoleChanges(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal RoleID As Integer, ByVal PersonName As String, ByVal PreviousRole As String, ByVal NewRole As String, ByVal ByUser As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        oValues.Add(ByUser)
        oValues.Add(PersonName)
        oValues.Add(PreviousRole)
        oValues.Add(NewRole)

        Dim oDto As New List(Of dtoNotificatedObject)
        oDto.Add(CreatePersonToNotify(PersonID))
        oDto.Add(CreateRoleToNotify(RoleID))

        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToAdmin, Services_GestioneIscritti.ActionType.EditSubscription, CommunityID, Services_GestioneIscritti.Codex, oValues, oDto)
    End Sub
    Public Sub NotifyAddSubscription(ByVal idCommunity As Integer, ByVal idPerson As Integer, ByVal idRole As Integer, ByVal PersonName As String, ByVal NewRole As String, ByVal ByUser As String)
        Dim NewsID As System.Guid = System.Guid.NewGuid
        Dim oValues = New List(Of String)
        'oValues.Add(MyBase.ServiceLoaderPage(idCommunity, BaseServiceUrl))

        oValues.Add(ByUser)
        oValues.Add(PersonName)
        oValues.Add(NewRole)

        Dim oDto As New List(Of dtoNotificatedObject)
        oDto.Add(CreatePersonToNotify(idPerson))
        oDto.Add(CreateRoleToNotify(idRole))

        _Utility.SendNotificationToPermission(NewsID, Me.PermissionToAdmin, Services_GestioneIscritti.ActionType.AddPerson, idCommunity, Services_GestioneIscritti.Codex, oValues, oDto)
    End Sub
    Public Sub NotifyDeleteSubscription(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonName As String, ByVal ByUser As String)
        Dim oValues = New List(Of String)
        oValues.Add(ByUser)
        oValues.Add(PersonName)

        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_GestioneIscritti.ActionType.DeleteSubscription, CommunityID, Services_GestioneIscritti.Codex, oValues, CreatePersonToNotify(PersonID))
    End Sub
    Public Sub NotifySelfUnSubscription(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonName As String)
        Dim oValues = New List(Of String)
        oValues.Add(PersonName)

        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_GestioneIscritti.ActionType.SelfUnSubscription, CommunityID, Services_GestioneIscritti.Codex, oValues, CreatePersonToNotify(PersonID))
    End Sub
    Public Sub NotifyAcceptedSubscription(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonName As String, ByVal ByUser As String)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl))

        oValues.Add(ByUser)
        oValues.Add(PersonName)

        _Utility.SendNotificationToPermission(Me.PermissionToAdmin, Services_GestioneIscritti.ActionType.AcceptUser, CommunityID, Services_GestioneIscritti.Codex, oValues, CreatePersonToNotify(PersonID))

        Dim oList As New List(Of Integer)
        oList.Add(PersonID)
        _Utility.SendNotificationToPerson(oList, Services_GestioneIscritti.ActionType.AcceptUser, CommunityID, Services_GestioneIscritti.Codex, oValues, CreatePersonToNotify(PersonID))
    End Sub

#Region "Object To Notify"
    Private Function CreatePersonToNotify(ByVal PersonID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(PersonID, Services_GestioneIscritti.ObjectType.Person)
    End Function
    Private Function CreateRoleToNotify(ByVal RoleID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(RoleID, Services_GestioneIscritti.ObjectType.Role)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As Integer, ByVal oType As Services_GestioneIscritti.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_IscrizioneComunita.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_IscrizioneComunita.Codex)
        If oType = Services_GestioneIscritti.ObjectType.Person Then
            obj.FullyQualiFiedName = GetType(lm.Comol.Core.DomainModel.Person).FullName
        ElseIf oType = Services_GestioneIscritti.ObjectType.Role Then
            obj.FullyQualiFiedName = GetType(lm.Comol.Core.DomainModel.Role).FullName
        End If
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToAdmin() As Integer
        Return Services_GestioneIscritti.Base2Permission.AdminService Or Services_GestioneIscritti.Base2Permission.Management
    End Function
#End Region

End Class