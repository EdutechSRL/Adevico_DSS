using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Business
{
    public partial class ServiceCommunityDiary
    {
        #region empty
            public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return new List<dtoItemEvaluation<long>>();
            }
            public void SaveActionExecution(ModuleLink link, Boolean isStarted, Boolean isPassed, short Completion, Boolean isCompleted, Int16 mark, Int32 idUser, bool alreadyCompleted, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {

            }
            public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
            }
            public dtoEvaluation EvaluateModuleLink(ModuleLink link, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                return null;
            }
            public void PhisicalDeleteRepositoryItem(long idFileItem, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {

            }
        #endregion

        public bool AllowActionExecution(ModuleLink link, Int32 idUser, Int32 idCommunity, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            switch (link.SourceItem.ObjectTypeID)
            {
                case (int)ModuleCommunityDiary.ObjectType.Diary:
                    return AllowViewDiary(idUser, idCommunity, idRole);
                case (int)ModuleCommunityDiary.ObjectType.DiaryItem:
                    return AllowViewDiaryItem(link.SourceItem.ObjectLongID, idUser);
                case (int)ModuleCommunityDiary.ObjectType.DiaryItemFile:
                    return AllowDownloadFileOfItem(link.SourceItem.ObjectLongID, idUser);
                case (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile:
                    return AllowDownloadFileLinkedToItem(link.SourceItem.ObjectLongID, idUser);
                default:
                    return false;
            }
        }
        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Int32 idCommunity, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            switch (source.ObjectTypeID)
            {
                case (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile:
                    actions = GetAllowedStandardActionForFile(idUser, source, destination);
                    break;
            }

            return actions;
        }
        private List<StandardActionType> GetAllowedStandardActionForFile(int idUser, ModuleObject source, ModuleObject destination)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            litePerson person = Manager.GetLitePerson(idUser);
            EventItemFile attachment = Manager.Get<EventItemFile>(source.ObjectLongID);
            if (attachment != null && attachment.Link != null && source.ObjectLongID == attachment.Id)
            {
                ModuleCommunityDiary modulePermission = GetPermissions(idUser, attachment.IdCommunity);
                lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = GetCoreModuleRepository(idUser, attachment.IdCommunity);
                CoreItemPermission itemPermission = GetItemPermission(person, EventItemGet(attachment.IdItemOwner), modulePermission, moduleRepository);

                if (attachment.Item != null)
                {
                    Boolean isMultimedia = attachment.Item.Type == Core.FileRepository.Domain.ItemType.Multimedia;
                    Boolean isScorm = attachment.Item.Type == Core.FileRepository.Domain.ItemType.ScormPackage;
                    Boolean isInternal = attachment.Item.IsInternal;
                    if ((isScorm ||isMultimedia) && (attachment.Item.Availability== Core.FileRepository.Domain.ItemAvailability.available || attachment.Item.Availability== Core.FileRepository.Domain.ItemAvailability.waitingsettings)  && ((isInternal && itemPermission.AllowEdit) ||
                      (!isInternal && (moduleRepository.Administration || moduleRepository.EditOthersFiles || (moduleRepository.EditMyFiles && attachment.Item.IdOwner == person.Id)))))
                        actions.Add(StandardActionType.EditMetadata);
                    if (AllowViewFileFromLink(modulePermission, itemPermission, attachment, person))
                    {
                        actions.Add(StandardActionType.Play);
                        if (isScorm)
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                    }
                    if ((isScorm && attachment.Item.Availability== Core.FileRepository.Domain.ItemAvailability.available) && ((isInternal && itemPermission.AllowEdit) || (!isInternal && (moduleRepository.Administration || moduleRepository.EditOthersFiles || (moduleRepository.EditMyFiles && attachment.Item.IdOwner == person.Id)))))
                        actions.Add(StandardActionType.ViewAdvancedStatistics);
                }
            }
            return actions;
        }
        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            Boolean iResponse = false;
            switch (actionType)
            {
                case StandardActionType.EditMetadata:
                    iResponse = AllowEditMetadata(idUser, idRole, source, destination);
                    break;
                case StandardActionType.EditPermission:
                    break;
                case StandardActionType.ViewAdvancedStatistics:
                    break;
                case StandardActionType.ViewPersonalStatistics:
                    break;
            }
            return iResponse;
        }
        private Boolean AllowEditMetadata(Int32 idUser, Int32 idRole, ModuleObject source, ModuleObject destination)
        {
            Boolean iResponse = false;

            if (source.ObjectTypeID == (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile)
            {
                litePerson person = Manager.GetLitePerson(idUser);
                EventItemFile attachment = Manager.Get<EventItemFile>(source.ObjectLongID);
                if (attachment != null && attachment.Link != null && attachment.Item != null && source.ObjectLongID == attachment.Id)
                {
                    ModuleCommunityDiary modulePermission = GetPermissions(idUser, attachment.IdCommunity);
                    lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = GetCoreModuleRepository(idUser, attachment.IdCommunity);
                    CoreItemPermission itemPermission = GetItemPermission(person, EventItemGet(attachment.IdItemOwner), modulePermission, moduleRepository);
                    iResponse = ((attachment.Item.Type == Core.FileRepository.Domain.ItemType.ScormPackage || attachment.Item.Type == Core.FileRepository.Domain.ItemType.Multimedia) && (attachment.Item.Availability == Core.FileRepository.Domain.ItemAvailability.available || attachment.Item.Availability == Core.FileRepository.Domain.ItemAvailability.waitingsettings) && ((attachment.Item.IsInternal && itemPermission.AllowEdit) ||
                      (!attachment.Item.IsInternal && (moduleRepository.Administration || moduleRepository.EditOthersFiles || (moduleRepository.EditMyFiles && attachment.Item.IdOwner == person.Id)))));
                }
            }
            return iResponse;
        }
        private Boolean AllowViewDiary(Int32 idUser, Int32 idCommunity, Int32 idRole)
        {
            Boolean iResponse = false;
            long permission = (long)ModuleCommunityDiary.Base2Permission.ViewLessons | (long)ModuleCommunityDiary.Base2Permission.AdminService;
            iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
            return iResponse;
        }
        private Boolean AllowViewDiaryItem(long idItem, Int32 idUser)
        {
            Boolean iResponse = false;
            CommunityEventItem item = EventItemGet(idItem);
            litePerson person = Manager.GetLitePerson(idUser);
            if (item != null)
            {
                ModuleCommunityDiary modulePermission = GetPermissions(idUser, item.IdCommunityOwner);
                lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = GetCoreModuleRepository(idUser, item.IdCommunityOwner);
                CoreItemPermission itemPermission = GetItemPermission(person, item, modulePermission, moduleRepository);
                iResponse = (itemPermission.AllowView);
            }
            return iResponse;
        }
        private Boolean AllowDownloadFileOfItem(long idItem,Int32 idUser)
        {
            return AllowViewDiaryItem(idItem, idUser);
        }
        private Boolean AllowDownloadFileLinkedToItem(long idAttachment, Int32 idUser)
        {
            Boolean iResponse = false;
            EventItemFile attachment = Manager.Get<EventItemFile>(idAttachment);
            litePerson person = Manager.GetLitePerson(idUser);
            if (attachment != null && attachment.Item != null && attachment.Link != null)
            {
                CommunityEventItem item = EventItemGet(attachment.IdItemOwner);
                if (item != null)
                {
                    ModuleCommunityDiary modulePermission = GetPermissions(idUser, attachment.IdCommunity);
                    lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = GetCoreModuleRepository(idUser, attachment.IdCommunity);
                    CoreItemPermission itemPermission = GetItemPermission(person, EventItemGet(attachment.IdItemOwner), modulePermission, moduleRepository);
                    iResponse = AllowViewFileFromLink(modulePermission, itemPermission, attachment, person) && (attachment.Item.IsDownloadable || (attachment.Item.IdOwner== idUser && attachment.Item.Availability !=  Core.FileRepository.Domain.ItemAvailability.notavailable));
                }
            }
            return iResponse;
        }

        private Boolean AllowViewFileFromLink(ModuleCommunityDiary modulePermission, CoreItemPermission itemPermission, EventItemFile attachment, litePerson person)
        {
            Boolean iResponse = false;
            iResponse = itemPermission.AllowViewFiles && (attachment.isVisible || attachment.IdItemOwner == person.Id  || modulePermission.Administration);
            return iResponse;
        }

        public void PhisicalDeleteCommunity(Int32 idCommunity, Int32 idUser, String baseFilePath, String baseThumbnailPath)
        {
            PhisicalDeleteCommunityDiary(idCommunity, baseFilePath, baseThumbnailPath);
        }

        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, Int32 objectTypeId, Dictionary<Int32, string> translations, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            StatTreeNode<StatFileTreeLeaf> node = null;
            return node;
        }

        public void PhisicalDeleteCommunity(string baseFileRepositoryPath, int idCommunity, int idUser)
        {
            //throw new NotImplementedException();
        }
    }
}
