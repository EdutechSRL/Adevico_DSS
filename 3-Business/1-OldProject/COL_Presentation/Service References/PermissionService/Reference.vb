﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace PermissionService
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="PermissionService.IServicePermission")>  _
    Public Interface IServicePermission
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/GetAllowedStandardAction", ReplyAction:="http://tempuri.org/IServicePermission/GetAllowedStandardActionResponse")>  _
        Function GetAllowedStandardAction(ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.StandardActionType)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/GetAllowedStandardActionForExternal", ReplyAction:="http://tempuri.org/IServicePermission/GetAllowedStandardActionForExternalResponse"& _ 
            "")>  _
        Function GetAllowedStandardActionForExternal(ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.StandardActionType)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/AllowStandardAction", ReplyAction:="http://tempuri.org/IServicePermission/AllowStandardActionResponse")>  _
        Function AllowStandardAction(ByVal actionType As lm.Comol.Core.DomainModel.StandardActionType, ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/AllowStandardActionForExternal", ReplyAction:="http://tempuri.org/IServicePermission/AllowStandardActionForExternalResponse")>  _
        Function AllowStandardActionForExternal(ByVal actionType As lm.Comol.Core.DomainModel.StandardActionType, ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/AllowActionExecution", ReplyAction:="http://tempuri.org/IServicePermission/AllowActionExecutionResponse")>  _
        Function AllowActionExecution(ByVal idLink As Long, ByVal idAction As Integer, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/AllowActionExecutionForExternal", ReplyAction:="http://tempuri.org/IServicePermission/AllowActionExecutionForExternalResponse")>  _
        Function AllowActionExecutionForExternal(ByVal idLink As Long, ByVal idAction As Integer, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/ModuleLinkPermission", ReplyAction:="http://tempuri.org/IServicePermission/ModuleLinkPermissionResponse")>  _
        Function ModuleLinkPermission(ByVal idLink As Long, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Long
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/ModuleLinkActionPermission", ReplyAction:="http://tempuri.org/IServicePermission/ModuleLinkActionPermissionResponse")>  _
        Function ModuleLinkActionPermission(ByVal idLink As Long, ByVal idAction As Integer, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Long
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/ActionPermission", ReplyAction:="http://tempuri.org/IServicePermission/ActionPermissionResponse")>  _
        Function ActionPermission(ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Long
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/AllowedActionPermission", ReplyAction:="http://tempuri.org/IServicePermission/AllowedActionPermissionResponse")>  _
        Function AllowedActionPermission(ByVal idLink As Long) As Long
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/ExecutedAction", ReplyAction:="http://tempuri.org/IServicePermission/ExecutedActionResponse")>  _
        Sub ExecutedAction(ByVal idLink As Long, ByVal isStarted As Boolean, ByVal isPassed As Boolean, ByVal completion As Short, ByVal isCompleted As Boolean, ByVal mark As Short, ByVal idUser As Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/ExecutedActionForExternal", ReplyAction:="http://tempuri.org/IServicePermission/ExecutedActionForExternalResponse")>  _
        Sub ExecutedActionForExternal(ByVal idLink As Long, ByVal isStarted As Boolean, ByVal isPassed As Boolean, ByVal completion As Short, ByVal isCompleted As Boolean, ByVal mark As Short, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String))
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/ExecutedActions", ReplyAction:="http://tempuri.org/IServicePermission/ExecutedActionsResponse")>  _
        Sub ExecutedActions(ByVal evaluatedLinks As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)), ByVal idUser As Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/EvaluateModuleLink", ReplyAction:="http://tempuri.org/IServicePermission/EvaluateModuleLinkResponse"),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)))>  _
        Function EvaluateModuleLink(ByVal idLink As Long, ByVal idUser As Integer) As lm.Comol.Core.DomainModel.dtoEvaluation
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/EvaluateModuleLinkForExternal", ReplyAction:="http://tempuri.org/IServicePermission/EvaluateModuleLinkForExternalResponse"),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)))>  _
        Function EvaluateModuleLinkForExternal(ByVal idLink As Long, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As lm.Comol.Core.DomainModel.dtoEvaluation
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/EvaluateModuleLinks", ReplyAction:="http://tempuri.org/IServicePermission/EvaluateModuleLinksResponse")>  _
        Function EvaluateModuleLinks(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long))
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/EvaluateModuleLinksForExternal", ReplyAction:="http://tempuri.org/IServicePermission/EvaluateModuleLinksForExternalResponse")>  _
        Function EvaluateModuleLinksForExternal(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long))
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/GetPendingEvaluations", ReplyAction:="http://tempuri.org/IServicePermission/GetPendingEvaluationsResponse")>  _
        Function GetPendingEvaluations(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long))
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/GetPendingEvaluationsForExternal", ReplyAction:="http://tempuri.org/IServicePermission/GetPendingEvaluationsForExternalResponse")>  _
        Function GetPendingEvaluationsForExternal(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long))
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/PhisicalDeleteCommunity", ReplyAction:="http://tempuri.org/IServicePermission/PhisicalDeleteCommunityResponse")>  _
        Sub PhisicalDeleteCommunity(ByVal serviceCode As String, ByVal idCommunity As Integer, ByVal idUser As Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/PhisicalDeleteRepositoryItem", ReplyAction:="http://tempuri.org/IServicePermission/PhisicalDeleteRepositoryItemResponse")>  _
        Sub PhisicalDeleteRepositoryItem(ByVal serviceCode As String, ByVal idFileItem As Long, ByVal idCommunity As Integer, ByVal idUser As Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/PhisicalDeleteRepositoryItemForExternal", ReplyAction:="http://tempuri.org/IServicePermission/PhisicalDeleteRepositoryItemForExternalResp"& _ 
            "onse")>  _
        Sub PhisicalDeleteRepositoryItemForExternal(ByVal serviceCode As String, ByVal idFileItem As Long, ByVal idCommunity As Integer, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String))
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/FederationCommunityCheck", ReplyAction:="http://tempuri.org/IServicePermission/FederationCommunityCheckResponse")>  _
        Function FederationCommunityCheck(ByVal communityId As Integer) As lm.Comol.Core.BaseModules.Federation.Enums.FederationType
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IServicePermission/FederationUserCheck", ReplyAction:="http://tempuri.org/IServicePermission/FederationUserCheckResponse")>  _
        Function FederationUserCheck(ByVal communityId As Integer, ByVal userId As Integer, ByVal externlUrl As String) As lm.Comol.Core.BaseModules.Federation.Enums.FederationResult
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface IServicePermissionChannel
        Inherits PermissionService.IServicePermission, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class ServicePermissionClient
        Inherits System.ServiceModel.ClientBase(Of PermissionService.IServicePermission)
        Implements PermissionService.IServicePermission
        
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
        
        Public Function GetAllowedStandardAction(ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.StandardActionType) Implements PermissionService.IServicePermission.GetAllowedStandardAction
            Return MyBase.Channel.GetAllowedStandardAction(source, destination, idUser)
        End Function
        
        Public Function GetAllowedStandardActionForExternal(ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.StandardActionType) Implements PermissionService.IServicePermission.GetAllowedStandardActionForExternal
            Return MyBase.Channel.GetAllowedStandardActionForExternal(source, destination, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function AllowStandardAction(ByVal actionType As lm.Comol.Core.DomainModel.StandardActionType, ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer) As Boolean Implements PermissionService.IServicePermission.AllowStandardAction
            Return MyBase.Channel.AllowStandardAction(actionType, source, destination, idUser)
        End Function
        
        Public Function AllowStandardActionForExternal(ByVal actionType As lm.Comol.Core.DomainModel.StandardActionType, ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Boolean Implements PermissionService.IServicePermission.AllowStandardActionForExternal
            Return MyBase.Channel.AllowStandardActionForExternal(actionType, source, destination, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function AllowActionExecution(ByVal idLink As Long, ByVal idAction As Integer, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer) As Boolean Implements PermissionService.IServicePermission.AllowActionExecution
            Return MyBase.Channel.AllowActionExecution(idLink, idAction, destination, idUser)
        End Function
        
        Public Function AllowActionExecutionForExternal(ByVal idLink As Long, ByVal idAction As Integer, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Boolean Implements PermissionService.IServicePermission.AllowActionExecutionForExternal
            Return MyBase.Channel.AllowActionExecutionForExternal(idLink, idAction, destination, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function ModuleLinkPermission(ByVal idLink As Long, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Long Implements PermissionService.IServicePermission.ModuleLinkPermission
            Return MyBase.Channel.ModuleLinkPermission(idLink, destination, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function ModuleLinkActionPermission(ByVal idLink As Long, ByVal idAction As Integer, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Long Implements PermissionService.IServicePermission.ModuleLinkActionPermission
            Return MyBase.Channel.ModuleLinkActionPermission(idLink, idAction, destination, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function ActionPermission(ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As Long Implements PermissionService.IServicePermission.ActionPermission
            Return MyBase.Channel.ActionPermission(source, destination, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function AllowedActionPermission(ByVal idLink As Long) As Long Implements PermissionService.IServicePermission.AllowedActionPermission
            Return MyBase.Channel.AllowedActionPermission(idLink)
        End Function
        
        Public Sub ExecutedAction(ByVal idLink As Long, ByVal isStarted As Boolean, ByVal isPassed As Boolean, ByVal completion As Short, ByVal isCompleted As Boolean, ByVal mark As Short, ByVal idUser As Integer) Implements PermissionService.IServicePermission.ExecutedAction
            MyBase.Channel.ExecutedAction(idLink, isStarted, isPassed, completion, isCompleted, mark, idUser)
        End Sub
        
        Public Sub ExecutedActionForExternal(ByVal idLink As Long, ByVal isStarted As Boolean, ByVal isPassed As Boolean, ByVal completion As Short, ByVal isCompleted As Boolean, ByVal mark As Short, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) Implements PermissionService.IServicePermission.ExecutedActionForExternal
            MyBase.Channel.ExecutedActionForExternal(idLink, isStarted, isPassed, completion, isCompleted, mark, idUser, moduleUserLong, moduleUserString)
        End Sub
        
        Public Sub ExecutedActions(ByVal evaluatedLinks As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)), ByVal idUser As Integer) Implements PermissionService.IServicePermission.ExecutedActions
            MyBase.Channel.ExecutedActions(evaluatedLinks, idUser)
        End Sub
        
        Public Function EvaluateModuleLink(ByVal idLink As Long, ByVal idUser As Integer) As lm.Comol.Core.DomainModel.dtoEvaluation Implements PermissionService.IServicePermission.EvaluateModuleLink
            Return MyBase.Channel.EvaluateModuleLink(idLink, idUser)
        End Function
        
        Public Function EvaluateModuleLinkForExternal(ByVal idLink As Long, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As lm.Comol.Core.DomainModel.dtoEvaluation Implements PermissionService.IServicePermission.EvaluateModuleLinkForExternal
            Return MyBase.Channel.EvaluateModuleLinkForExternal(idLink, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function EvaluateModuleLinks(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)) Implements PermissionService.IServicePermission.EvaluateModuleLinks
            Return MyBase.Channel.EvaluateModuleLinks(linksId, idUser)
        End Function
        
        Public Function EvaluateModuleLinksForExternal(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)) Implements PermissionService.IServicePermission.EvaluateModuleLinksForExternal
            Return MyBase.Channel.EvaluateModuleLinksForExternal(linksId, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Function GetPendingEvaluations(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)) Implements PermissionService.IServicePermission.GetPendingEvaluations
            Return MyBase.Channel.GetPendingEvaluations(linksId, idUser)
        End Function
        
        Public Function GetPendingEvaluationsForExternal(ByVal linksId As System.Collections.Generic.List(Of Long), ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoItemEvaluation(Of Long)) Implements PermissionService.IServicePermission.GetPendingEvaluationsForExternal
            Return MyBase.Channel.GetPendingEvaluationsForExternal(linksId, idUser, moduleUserLong, moduleUserString)
        End Function
        
        Public Sub PhisicalDeleteCommunity(ByVal serviceCode As String, ByVal idCommunity As Integer, ByVal idUser As Integer) Implements PermissionService.IServicePermission.PhisicalDeleteCommunity
            MyBase.Channel.PhisicalDeleteCommunity(serviceCode, idCommunity, idUser)
        End Sub
        
        Public Sub PhisicalDeleteRepositoryItem(ByVal serviceCode As String, ByVal idFileItem As Long, ByVal idCommunity As Integer, ByVal idUser As Integer) Implements PermissionService.IServicePermission.PhisicalDeleteRepositoryItem
            MyBase.Channel.PhisicalDeleteRepositoryItem(serviceCode, idFileItem, idCommunity, idUser)
        End Sub
        
        Public Sub PhisicalDeleteRepositoryItemForExternal(ByVal serviceCode As String, ByVal idFileItem As Long, ByVal idCommunity As Integer, ByVal idUser As Integer, ByVal moduleUserLong As System.Collections.Generic.Dictionary(Of String, Long), ByVal moduleUserString As System.Collections.Generic.Dictionary(Of String, String)) Implements PermissionService.IServicePermission.PhisicalDeleteRepositoryItemForExternal
            MyBase.Channel.PhisicalDeleteRepositoryItemForExternal(serviceCode, idFileItem, idCommunity, idUser, moduleUserLong, moduleUserString)
        End Sub
        
        Public Function FederationCommunityCheck(ByVal communityId As Integer) As lm.Comol.Core.BaseModules.Federation.Enums.FederationType Implements PermissionService.IServicePermission.FederationCommunityCheck
            Return MyBase.Channel.FederationCommunityCheck(communityId)
        End Function
        
        Public Function FederationUserCheck(ByVal communityId As Integer, ByVal userId As Integer, ByVal externlUrl As String) As lm.Comol.Core.BaseModules.Federation.Enums.FederationResult Implements PermissionService.IServicePermission.FederationUserCheck
            Return MyBase.Channel.FederationUserCheck(communityId, userId, externlUrl)
        End Function
    End Class
End Namespace
