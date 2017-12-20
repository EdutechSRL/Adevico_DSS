using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
    [CLSCompliant(true)]
    public interface iLinkedService
    {
        List<StandardActionType> GetAllowedStandardAction(
            ModuleObject source, 
            ModuleObject destination, 
            Int32 idUser, 
            Int32 idRole, 
            Int32 idCommunity, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        bool AllowStandardAction(
            StandardActionType actionType, 
            ModuleObject source, 
            ModuleObject destination, 
            Int32 idUser, 
            Int32 idRole, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        bool AllowActionExecution(
            ModuleLink link, 
            Int32 idUser, 
            Int32 idCommunity, 
            Int32 idRole, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        void SaveActionExecution(
            ModuleLink link, 
            Boolean isStarted, 
            Boolean isPassed, 
            short Completion, 
            Boolean isCompleted, 
            Int16 mark, 
            Int32 idUser,
            bool alreadyCompleted,
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        void SaveActionsExecution(
            List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, 
            Int32 idUser, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        List<dtoItemEvaluation<long>> EvaluateModuleLinks(
            List<ModuleLink> links, 
            Int32 idUser, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        dtoEvaluation EvaluateModuleLink(
            ModuleLink link, 
            Int32 idUser, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        void PhisicalDeleteCommunity(
            Int32 idCommunity, 
            Int32 idUser, 
            String baseFilePath, 
            String baseThumbnailPath);

        void PhisicalDeleteRepositoryItem(
            long idFileItem, 
            Int32 idCommunity, 
            Int32 idUser, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);

        StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(
            long objectId, 
            Int32 objectTypeId, 
            Dictionary<Int32, string> translations, 
            Int32 idCommunity, 
            Int32 idUser, 
            Dictionary<String, long> moduleUserLong = null, 
            Dictionary<String, String> moduleUserString = null);


    }
}