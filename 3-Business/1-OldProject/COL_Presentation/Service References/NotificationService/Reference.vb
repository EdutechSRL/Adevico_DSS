﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.296
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace NotificationService
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="NotificationService.iNotificationService")>  _
    Public Interface iNotificationService
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/NotifyToCommunity"),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToPerson)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToPermission)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToItemGuid)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToItemLong)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToItemInt)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToRole))>  _
        Sub NotifyToCommunity(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToCommunity)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/NotifyToUsers")>  _
        Sub NotifyToUsers(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToPerson)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/NotifyForPermission"),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToItemGuid)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToItemLong)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Notification.DataContract.Domain.NotificationToItemInt))>  _
        Sub NotifyForPermission(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToPermission)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/NotifyForRoles")>  _
        Sub NotifyForRoles(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToRole)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/NotifyForPermissionItemGuid")>  _
        Sub NotifyForPermissionItemGuid(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToItemGuid)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/NotifyForPermissionItemLong")>  _
        Sub NotifyForPermissionItemLong(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToItemLong)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/NotifyForPermissionItemInt")>  _
        Sub NotifyForPermissionItemInt(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToItemInt)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/RemoveNotification")>  _
        Sub RemoveNotification(ByVal NotificationID As System.Guid)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/RemoveNotificationForUser")>  _
        Sub RemoveNotificationForUser(ByVal NotificationID As System.Guid, ByVal PersonID As Integer)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/RemoveUserNotification")>  _
        Sub RemoveUserNotification(ByVal UserNotificationID As System.Guid, ByVal PersonID As Integer)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/ReadNotification")>  _
        Sub ReadNotification(ByVal NotificationID As System.Guid, ByVal PersonID As Integer)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/ReadUserNotification")>  _
        Sub ReadUserNotification(ByVal UserNotificationID As System.Guid, ByVal PersonID As Integer)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/ReadUserCommunityNotification")>  _
        Sub ReadUserCommunityNotification(ByVal CommunityID As Integer, ByVal PersonID As Integer)
        
        <System.ServiceModel.OperationContractAttribute(IsOneWay:=true, Action:="http://tempuri.org/iNotificationService/RemoveUserCommunityNotification")>  _
        Sub RemoveUserCommunityNotification(ByVal CommunityID As Integer, ByVal PersonID As Integer)
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface iNotificationServiceChannel
        Inherits NotificationService.iNotificationService, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class iNotificationServiceClient
        Inherits System.ServiceModel.ClientBase(Of NotificationService.iNotificationService)
        Implements NotificationService.iNotificationService
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Sub NotifyToCommunity(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToCommunity) Implements NotificationService.iNotificationService.NotifyToCommunity
            MyBase.Channel.NotifyToCommunity(Notification)
        End Sub
        
        Public Sub NotifyToUsers(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToPerson) Implements NotificationService.iNotificationService.NotifyToUsers
            MyBase.Channel.NotifyToUsers(Notification)
        End Sub
        
        Public Sub NotifyForPermission(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToPermission) Implements NotificationService.iNotificationService.NotifyForPermission
            MyBase.Channel.NotifyForPermission(Notification)
        End Sub
        
        Public Sub NotifyForRoles(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToRole) Implements NotificationService.iNotificationService.NotifyForRoles
            MyBase.Channel.NotifyForRoles(Notification)
        End Sub
        
        Public Sub NotifyForPermissionItemGuid(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToItemGuid) Implements NotificationService.iNotificationService.NotifyForPermissionItemGuid
            MyBase.Channel.NotifyForPermissionItemGuid(Notification)
        End Sub
        
        Public Sub NotifyForPermissionItemLong(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToItemLong) Implements NotificationService.iNotificationService.NotifyForPermissionItemLong
            MyBase.Channel.NotifyForPermissionItemLong(Notification)
        End Sub
        
        Public Sub NotifyForPermissionItemInt(ByVal Notification As lm.Notification.DataContract.Domain.NotificationToItemInt) Implements NotificationService.iNotificationService.NotifyForPermissionItemInt
            MyBase.Channel.NotifyForPermissionItemInt(Notification)
        End Sub
        
        Public Sub RemoveNotification(ByVal NotificationID As System.Guid) Implements NotificationService.iNotificationService.RemoveNotification
            MyBase.Channel.RemoveNotification(NotificationID)
        End Sub
        
        Public Sub RemoveNotificationForUser(ByVal NotificationID As System.Guid, ByVal PersonID As Integer) Implements NotificationService.iNotificationService.RemoveNotificationForUser
            MyBase.Channel.RemoveNotificationForUser(NotificationID, PersonID)
        End Sub
        
        Public Sub RemoveUserNotification(ByVal UserNotificationID As System.Guid, ByVal PersonID As Integer) Implements NotificationService.iNotificationService.RemoveUserNotification
            MyBase.Channel.RemoveUserNotification(UserNotificationID, PersonID)
        End Sub
        
        Public Sub ReadNotification(ByVal NotificationID As System.Guid, ByVal PersonID As Integer) Implements NotificationService.iNotificationService.ReadNotification
            MyBase.Channel.ReadNotification(NotificationID, PersonID)
        End Sub
        
        Public Sub ReadUserNotification(ByVal UserNotificationID As System.Guid, ByVal PersonID As Integer) Implements NotificationService.iNotificationService.ReadUserNotification
            MyBase.Channel.ReadUserNotification(UserNotificationID, PersonID)
        End Sub
        
        Public Sub ReadUserCommunityNotification(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements NotificationService.iNotificationService.ReadUserCommunityNotification
            MyBase.Channel.ReadUserCommunityNotification(CommunityID, PersonID)
        End Sub
        
        Public Sub RemoveUserCommunityNotification(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements NotificationService.iNotificationService.RemoveUserCommunityNotification
            MyBase.Channel.RemoveUserCommunityNotification(CommunityID, PersonID)
        End Sub
    End Class
End Namespace