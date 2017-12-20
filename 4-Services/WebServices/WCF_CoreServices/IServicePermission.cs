using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace WCF_CoreServices
{
    [ServiceContract]
    public interface IServicePermission
    {
        /// <summary>
        /// Get available actions
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [OperationContract]
        List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser);
        /// <summary>
        /// Get available actions
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="idUser"></param>
        /// <param name="moduleUserLong"></param>
        /// <param name="moduleUserString"></param>
        /// <returns></returns>
        [OperationContract]
        List<StandardActionType> GetAllowedStandardActionForExternal(ModuleObject source, ModuleObject destination, Int32 idUser, Dictionary<String, long> moduleUserLong , Dictionary<String, String> moduleUserString);

        /// <summary>
        /// Allow specific action
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser);
        /// <summary>
        /// Allow specific action
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="idUser"></param>
        /// <param name="moduleUserLong"></param>
        /// <param name="moduleUserString"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean AllowStandardActionForExternal(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);
        /// <summary>
        /// Verify if user can execute action requested
        /// </summary>
        /// <param name="idLink"></param>
        /// <param name="idAction"></param>
        /// <param name="destination"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean AllowActionExecution(long idLink, Int32 idAction, ModuleObject destination, Int32 idUser);
        /// <summary>
        /// Verify if user can execute action requested
        /// </summary>
        /// <param name="idLink"></param>
        /// <param name="idAction"></param>
        /// <param name="destination"></param>
        /// <param name="idUser"></param>
        /// <param name="moduleUserLong"></param>
        /// <param name="moduleUserString"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean AllowActionExecutionForExternal(long idLink, Int32 idAction, ModuleObject destination, Int32 idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);

        [OperationContract]
        long ModuleLinkPermission(long idLink, ModuleObject destination, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);
        
        [OperationContract]
        long ModuleLinkActionPermission(long idLink, int idAction, ModuleObject destination, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);

        [OperationContract]
        long ActionPermission(ModuleObject source, ModuleObject destination, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);

        [OperationContract]
        long AllowedActionPermission(long idLink);

        [OperationContract]
        void ExecutedAction(long idLink, Boolean isStarted, Boolean isPassed, Int16 completion, Boolean isCompleted, Int16 mark, int idUser);

        [OperationContract]
        void ExecutedActionForExternal(long idLink, Boolean isStarted, Boolean isPassed, Int16 completion, Boolean isCompleted, Int16 mark, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);


        [OperationContract]
        void ExecutedActions(List<dtoItemEvaluation<long>> evaluatedLinks, int idUser);
        void ExecutedActionsForExternal(List<dtoItemEvaluation<long>> evaluatedLinks, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);

        [OperationContract]
        dtoEvaluation EvaluateModuleLink(long idLink, int idUser);

        [OperationContract]
        dtoEvaluation EvaluateModuleLinkForExternal(long idLink, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);

        [OperationContract]
        List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<long> linksId, int idUser);
        [OperationContract]
        List<dtoItemEvaluation<long>> EvaluateModuleLinksForExternal(List<long> linksId, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);

        [OperationContract]
        List<dtoItemEvaluation<long>> GetPendingEvaluations(List<long> linksId, int idUser);
        [OperationContract]
        List<dtoItemEvaluation<long>> GetPendingEvaluationsForExternal(List<long> linksId, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);

        //[OperationContract]
        //bool IsFederationPerson(int idCommunity, int idUser);
        [OperationContract]
        void PhisicalDeleteCommunity(String serviceCode,int idCommunity, int idUser);

        [OperationContract]
        void PhisicalDeleteRepositoryItem(String serviceCode,long idFileItem,int idCommunity, int idUser );
        [OperationContract]
        void PhisicalDeleteRepositoryItemForExternal(String serviceCode, long idFileItem, int idCommunity, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString);
        //[OperationContract]
        //Boolean RemoveInternalObject();
        //[OperationContract]
        //dtoEvaluation EvaluateAction(ModuleObject source, ModuleObject destination, int UserID);


        /// <summary>
        /// Check Community Federation
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [OperationContract]
        lm.Comol.Core.BaseModules.Federation.Enums.FederationType FederationCommunityCheck(
            int communityId);

        /// <summary>
        /// Check if User is Federated
        /// </summary>
        /// <param name="communityId"></param>
        /// <param name="userId"></param>
        /// <param name="externlUrl"></param>
        /// <returns></returns>
        [OperationContract]
        lm.Comol.Core.BaseModules.Federation.Enums.FederationResult FederationUserCheck(
            int communityId, 
            int userId,
            string externlUrl);
    }
}