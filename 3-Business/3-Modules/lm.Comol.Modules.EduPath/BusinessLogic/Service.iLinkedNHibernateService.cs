using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain.DTO;
using lm.Comol.Modules.EduPath.Domain;
using System.Diagnostics;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public partial class Service : iLinkedNHibernateService
    {

        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, int idUser, int idRole, int idCommunity, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            if (source.ServiceCode == "SRVEDUP")
            {
                PermissionEP permission = new PermissionEP(RoleEP.None);
                switch (source.ObjectTypeID)
                {
                    case (int)ItemType.Path:
                        permission = GetUserPermission_ByPath(source.ObjectLongID, idUser, idRole);
                        break;
                    case (int)ItemType.Unit:
                        permission = GetUserPermission_ByUnit(source.ObjectLongID, idUser, idRole);
                        break;
                    case (int)ItemType.Activity:
                        permission = GetUserPermission_ByActivity(source.ObjectLongID, idUser, idRole);
                        break;
                    case (int)ItemType.SubActivity:
                        SubActivity sub = GetSubActivity(source.ObjectLongID);
                        if (sub != null)
                            permission = GetUserPermission_ByActivity(sub.ParentActivity.Id, idUser, idRole);
                        break;
                }
                if (permission.ViewOwnStat)
                    actions.Add(StandardActionType.ViewPersonalStatistics);
                if (permission.ViewUserStat)
                    actions.Add(StandardActionType.ViewUserStatistics);
                if (permission.ViewUserStat || permission.Create || permission.Update)
                    actions.Add(StandardActionType.ViewAdvancedStatistics);
                if (permission.Create)
                    actions.Add(StandardActionType.Create);
                if (permission.Create || permission.Update)
                    actions.Add(StandardActionType.Edit);

                switch (destination.ServiceCode)
                {
                    case CoreModuleRepository.UniqueID:
                        actions = GetAllowedStandardActionForRepository(permission, actions, destination.ObjectLongID, idUser, idCommunity);
                        break;
                }
            }
            //                Boolean isMultimedia = (eventItemFile.File.RepositoryItemType != RepositoryItemType.FileStandard && eventItemFile.File.RepositoryItemType != RepositoryItemType.Folder && eventItemFile.File.RepositoryItemType != RepositoryItemType.None);
            //è multimediale e posso actions.Add(StandardActionType.EditMetadata)
            //actions.Add(StandardActionType.EditMetadata)
            // actions.Add(StandardActionType.Play);
            // actions.Add(StandardActionType.ViewPersonalStatistics);
            // actions.Add(StandardActionType.ViewAdvancedStatistics);
            return actions;
        }

        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, int idUser, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            Boolean AllowAction = false;
            PermissionEP oPermissionEP = new PermissionEP(0);
            long idActivity = 0;
            switch (source.ObjectTypeID)
            {
                case (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity:
                    idActivity = GetIdActivityBySubActivity(source.ObjectLongID);
                    break;
                case (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity:
                    if (destination == null)
                        idActivity = source.ObjectLongID;
                    else if ((from a in Manager.GetIQ<liteModuleLink>() where a.SourceItem.ObjectLongID == source.ObjectLongID && a.DestinationItem.ObjectLongID == destination.ObjectLongID && a.SourceItem.ObjectTypeID == (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity && a.DestinationItem.ObjectTypeID == destination.ObjectTypeID && a.SourceItem.ServiceCode == source.ServiceCode && a.DestinationItem.ServiceCode == destination.ServiceCode select a.Id).Count() > 0)
                        idActivity = source.ObjectLongID;
                    break;
                default:
                    break;
            }
            oPermissionEP = GetUserPermission_ByActivity(idActivity, idUser, idRole);


            switch (actionType)
            {
                case StandardActionType.Create:
                    AllowAction = oPermissionEP.Create;
                    break;
                case StandardActionType.Play:
                    AllowAction = oPermissionEP.Read;
                    break;
                case StandardActionType.Edit:
                    AllowAction = oPermissionEP.Update;
                    break;
                case StandardActionType.ViewPersonalStatistics:
                    AllowAction = oPermissionEP.ViewOwnStat;
                    break;
                case StandardActionType.ViewUserStatistics:
                    AllowAction = oPermissionEP.ViewUserStat;
                    break;
                case StandardActionType.ViewAdvancedStatistics:
                    AllowAction = oPermissionEP.ViewUserStat;
                    break;
                default:
                    switch (destination.ServiceCode)
                    {
                        case CoreModuleRepository.UniqueID:
                            AllowAction = GetAllowedActionsForRepository(oPermissionEP, destination.ObjectLongID, idUser, destination.CommunityID).Contains(actionType);
                            break;
                        default:
                            AllowAction = false;
                            break;
                    }

                    break;
            }
            return AllowAction;
        }

        public bool AllowActionExecution(ModuleLink link, int idUser, int idCommunity, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            Boolean allow = false;
            switch (link.SourceItem.ObjectTypeID)
            {
                case (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity:
                    long idActivity = (from s in Manager.GetIQ<liteSubActivity>() where s.ModuleLink != null && s.ModuleLink.Id == link.Id select s.IdActivity).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (idActivity > 0)
                        allow = GetUserRole_ByActivity(idActivity, idUser, idRole) > RoleEP.None;
                    break;
                default:
                    break;
            }
            return allow;
        }

        public void SaveActionExecution(
            ModuleLink link, 
            bool isStarted, 
            bool isPassed, 
            short Completion, 
            bool isCompleted, 
            short mark, 
            int idUser,
            bool alreadyCompleted,
            Dictionary<string, long> moduleUserLong = null, 
            Dictionary<string, string> moduleUserString = null)
        {
            try
            {
                
                if (link.SourceItem.ObjectTypeID == (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity)
                {
                    int croleId = Manager.GetActiveSubscriptionIdRole(idUser, link.SourceItem.CommunityID); //Manager.Get<SubActivity>(Link.SourceItem.ObjectLongID).Community.Id);
                    Manager.BeginTransaction();
                    ServiceStat.InitOrUpdateSubActivityNoTransaction(link.SourceItem.ObjectLongID, idUser, croleId, idUser, "", "", Completion, mark, isStarted, isCompleted, isPassed);
                    Manager.Commit();
                }
                else if (link.SourceItem.ObjectTypeID == (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity)
                {

                }
            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                Debug.Write(ex);

            }
        }

        public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
        }
        public void SaveActionsExecution(List<dtoItemEvaluation<long>> evaluatedLinks, int idUser)
        {
            if (evaluatedLinks != null && evaluatedLinks.Count > 0)
            {
                try
                {
                    
                    IList<long> IdLinks = (from e in evaluatedLinks select e.Item).ToList();
                    IList<ModuleLink> links = (from l in Manager.GetIQ<ModuleLink>()
                                               where IdLinks.Contains(l.Id)
                                               select l).ToList();

                    foreach (dtoItemEvaluation<long> dto in evaluatedLinks)
                    {
                        ModuleLink link = (from l in links where l.Id == dto.Item select l).FirstOrDefault();
                        if (link != null)
                        {
                            int croleId = Manager.GetActiveSubscriptionIdRole(idUser, link.SourceItem.CommunityID); //Manager.Get<SubActivity>(Link.SourceItem.ObjectLongID).Community.Id);
                            Manager.BeginTransaction();
                            ServiceStat.InitOrUpdateSubActivityNoTransaction(link.SourceItem.ObjectLongID, idUser, croleId, idUser, "", "", dto.Completion, dto.Mark, dto.isStarted, dto.isCompleted, dto.isPassed);
                            Manager.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    Debug.Write(ex);
                }
            }
        }
        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return new List<dtoItemEvaluation<long>>();
        }

        public dtoEvaluation EvaluateModuleLink(ModuleLink link, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return new dtoEvaluation() { isCompleted = false, Completion = 0, };
        }

        public void PhisicalDeleteCommunity(Int32 idCommunity, Int32 idUser, String baseFilePath, String baseThumbnailPath)
        {
        }
        public void PhisicalDeleteRepositoryItem(long idFileItem, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
        }
        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, int objectTypeId, Dictionary<int, string> translations, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return new StatTreeNode<StatFileTreeLeaf>();
        }

        private lm.Comol.Core.FileRepository.Domain.ModuleRepository GetCoreModuleRepository(int idPerson, int idCommunity)
        {
            lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier = lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(lm.Comol.Core.FileRepository.Domain.RepositoryType.Community, idCommunity);
            return ServiceRepository.GetPermissions(identifier, idPerson);
        }

        List<StandardActionType> GetAllowedActionsForRepository(PermissionEP permission, long idItem, int idUser, int IdCommunity)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = GetCoreModuleRepository(idUser, IdCommunity);
            lm.Comol.Core.FileRepository.Domain.liteRepositoryItem item = Manager.Get<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>(idItem);

            if (item != null)
            {
                Boolean isPlayFile = (item.Type == Core.FileRepository.Domain.ItemType.ScormPackage || item.Type == Core.FileRepository.Domain.ItemType.Multimedia || item.Type== Core.FileRepository.Domain.ItemType.VideoStreaming);
                if (!isPlayFile)
                {
                    if (typeof(CommunityFile) == item.GetType())
                    {
                        if ((moduleRepository.DownloadOrPlay && item.IsDownloadable) || moduleRepository.Administration || moduleRepository.ManageItems || moduleRepository.EditOthersFiles || (moduleRepository.UploadFile && (item.IdOwner == idUser)))
                            actions.Add(StandardActionType.DownloadItem);
                        if (moduleRepository.Administration || moduleRepository.EditOthersFiles || moduleRepository.ManageItems || (moduleRepository.UploadFile && item.IdOwner == idUser))
                            actions.Add(StandardActionType.EditPermission);
                    }
                    else
                    {
                        if (item.IsDownloadable)
                            actions.Add(StandardActionType.DownloadItem);
                    }
                }
                else
                {
                    if (typeof(CommunityFile) == item.GetType())
                    {
                        if ((moduleRepository.DownloadOrPlay && item.IsDownloadable) || moduleRepository.Administration || moduleRepository.ManageItems || moduleRepository.EditOthersFiles || (moduleRepository.UploadFile && (item.IdOwner == idUser)))
                            actions.Add(StandardActionType.EditMetadata);
                        actions.Add(StandardActionType.DownloadItem);
                        if (permission.ViewUserStat)
                        {
                            actions.Remove(StandardActionType.ViewAdvancedStatistics);
                            actions.Remove(StandardActionType.ViewUserStatistics);
                        }
                        if (permission.ViewOwnStat)
                            actions.Add(StandardActionType.ViewUserStatistics);
                    }
                    else if (moduleRepository.DownloadOrPlay && item.IsDownloadable)
                    {
                        if (permission.Read)
                            actions.Add(StandardActionType.DownloadItem);
                        if (permission.ViewOwnStat)
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                    }
                    else
                    {
                        if (permission.Create || permission.Update)
                            actions.Add(StandardActionType.EditMetadata);
                        if (item.IsDownloadable)
                            actions.Add(StandardActionType.DownloadItem);
                        if (permission.ViewUserStat)
                        {
                            actions.Add(StandardActionType.ViewAdvancedStatistics);
                            actions.Add(StandardActionType.ViewUserStatistics);
                        }
                        if (permission.ViewOwnStat)
                            actions.Remove(StandardActionType.ViewUserStatistics);
                    }
                }
            }
            return actions.Distinct().ToList();
        }
        List<StandardActionType> GetAllowedStandardActionForRepository(PermissionEP permission, List<StandardActionType> actions, long idItem, int idUser, int IdCommunity)
        {
            lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = GetCoreModuleRepository(idUser, IdCommunity);
            lm.Comol.Core.FileRepository.Domain.liteRepositoryItem item = Manager.Get<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>(idItem);

            if (item != null)
            {
                Boolean isPlayFile = (item.Type == Core.FileRepository.Domain.ItemType.ScormPackage || item.Type == Core.FileRepository.Domain.ItemType.Multimedia || item.Type == Core.FileRepository.Domain.ItemType.VideoStreaming);
                if (!item.IsInternal)
                {
                    if (isPlayFile && moduleRepository.Administration || moduleRepository.ManageItems || moduleRepository.EditOthersFiles || (moduleRepository.UploadFile && item.IdOwner == idUser))
                        actions.Add(StandardActionType.EditMetadata);
                    else if (moduleRepository.Administration || moduleRepository.ManageItems || moduleRepository.EditOthersFiles || (moduleRepository.UploadFile && item.IdOwner == idUser))
                    {
                        actions.Remove(StandardActionType.ViewAdvancedStatistics);
                        actions.Remove(StandardActionType.ViewUserStatistics);
                        actions.Remove(StandardActionType.ViewPersonalStatistics);
                    }
                }
                else if (isPlayFile && (permission.Create || permission.Update))
                {
                    actions.Add(StandardActionType.EditMetadata);
                }
            }
            return actions.Distinct().ToList();
        }

        //public void SaveActionsExecution(List<dtoItemEvaluation<long>> evaluatedLinks, int UserID)
        //{
        //    if (evaluatedLinks != null && evaluatedLinks.Count > 0) {
        //        try
        //        {
        //            Manager.BeginTransaction();
        //            IList<long> IdLinks = (from e in evaluatedLinks select e.Item).ToList();
        //            IList<ModuleLink> links = (from l in Manager.GetIQ<ModuleLink>()
        //                                       where IdLinks.Contains(l.Id)
        //                                       select l).ToList();

        //            foreach (dtoItemEvaluation<long> dto in evaluatedLinks)
        //            {
        //                ModuleLink link = (from l in links where l.Id == dto.Item select l).FirstOrDefault();
        //                if (link != null)
        //                {
        //                    int croleId = Manager.GetActiveSubscriptionRoleId(UserID, link.SourceItem.CommunityID); //Manager.Get<SubActivity>(Link.SourceItem.ObjectLongID).Community.Id);

        //                    ServiceStat.InitOrUpdateSubActivityNoTransaction(link.SourceItem.ObjectLongID, UserID, croleId, UserID, "", "", dto.Completion, dto.Mark, dto.isStarted, dto.isCompleted, dto.isPassed);
        //                }
        //            }

        //            Manager.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            Manager.RollBack();
        //            Debug.Write(ex);
        //        }
        //    }
        //}


        public void SaveActionExecution(
            Int32 idUser, 
            long idLink, 
            Boolean isStarted, 
            Boolean isPassed, 
            short completion, 
            Boolean isCompleted, 
            short mark, 
            DateTime? referenceTime=null)
        {
            dtoEvaluation evaluation = new dtoEvaluation() { Completion = completion, isCompleted = isCompleted, isPassed = isPassed, isStarted = isStarted, Mark = mark };
            SaveActionExecution(idUser, idLink, evaluation, referenceTime);
        }

        public void SaveActionExecution(
            Int32 idUser, 
            long idLink, 
            dtoEvaluation evaluation, 
            DateTime? referenceTime = null)
        {
            liteModuleLink link = Manager.Get<liteModuleLink>(idLink);
            if (link != null)
            {
                switch (link.SourceItem.ObjectTypeID)
                {
                    case (int) ModuleEduPath.ObjectType.SubActivity:
                        long idPath = GetPathId_BySubActivityId(link.SourceItem.ObjectLongID);
                        Int32 idCommunity = GetPathIdCommunity(idPath);
                        Int32 idRole = Manager.GetActiveSubscriptionIdRole(idUser, idCommunity);
                        SaveActionExecution(idUser, idRole, idPath, link.SourceItem.ObjectLongID, evaluation, referenceTime);
                        break;
                }
            }
        }

        public void SaveActionExecution(
            Int32 idUser, 
            Int32 idRole, 
            long idLink, 
            dtoEvaluation evaluation, 
            DateTime? referenceTime)
        {
            liteModuleLink link = Manager.Get<liteModuleLink>(idLink);
            if (link != null)
            {
                switch (link.SourceItem.ObjectTypeID)
                {
                    case (int)ModuleEduPath.ObjectType.SubActivity:
                        SaveActionExecution(idUser, idRole, GetPathId_BySubActivityId(link.SourceItem.ObjectLongID), link.SourceItem.ObjectLongID, evaluation, referenceTime);
                        break;
                }
            }
        }

        public void SaveActionExecution(
            Int32 idUser, 
            Int32 idRole, 
            long idPath, 
            long idSubActivity, 
            dtoEvaluation evaluation,
            DateTime? referenceTime)
        {
            try
            {
                Boolean save = true;
                Path path = GetPath(idPath);
                PolicySettings settings = null;
                if (path != null)
                    settings = path.Policy;

                if (settings != null)
                {
                    switch (settings.Statistics)
                    {
                        case CompletionPolicy.UpdateAlways:
                            break;
                        default:
                            List<SubActivityStatistic> items = GetUserStatistics(idSubActivity, idUser, ((referenceTime!=null && referenceTime.HasValue) ? referenceTime.Value : DateTime.Now));
                            SubActivityStatistic last = (items == null ? null : items.FirstOrDefault());
                            switch (settings.Statistics)
                            {
                                case CompletionPolicy.NoUpdateIfCompleted:
                                    if (evaluation.isPassed && evaluation.isCompleted)
                                        save = !items.Any(i => ServiceStat.CheckStatusStatistic(i.Status, StatusStatistic.CompletedPassed));
                                    else
                                        save = !items.Any(i => ServiceStat.CheckStatusStatistic(i.Status, StatusStatistic.Completed) || ServiceStat.CheckStatusStatistic(i.Status, StatusStatistic.CompletedPassed));
                                    break;
                                //case CompletionPolicy.UpdateOnlyIfBetter:
                                //    if (last != null)
                                //        save = (last.Completion < evaluation.Completion || (
                                //                 ((!ServiceStat.CheckStatusStatistic(last.Status, StatusStatistic.Completed) && !ServiceStat.CheckStatusStatistic(last.Status, StatusStatistic.CompletedPassed))
                                //                 && (evaluation.Status == Core.FileRepository.Domain.PackageStatus.completed
                                //                     || evaluation.Status == Core.FileRepository.Domain.PackageStatus.completedpassed))
                                //                     ));

                                //    break;
                                //case CompletionPolicy.UpdateOnlyIfWorst:
                                //    break;
                            }
                            break;
                    }
                }
                if (save)
                {
                    Manager.BeginTransaction();
                    ServiceStat.InitOrUpdateSubActivityNoTransaction(idSubActivity, idUser, idRole, idUser, UC.IpAddress, UC.ProxyIpAddress, evaluation.Completion, (short)evaluation.Completion,  evaluation.isStarted, evaluation.isCompleted, evaluation.isPassed);
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #region "to new"
        private List<SubActivityStatistic> GetUserStatistics(long idSubActivity, Int32 idPerson, DateTime viewStatBefore)
        {
            var query = (from stat in Manager.GetIQ<SubActivityStatistic>()
                         where stat.SubActivity != null && stat.Person != null && stat.SubActivity.Id == idSubActivity && stat.Person.Id == idPerson && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
                         select stat);

            return query.OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).ToList();

            //var subStat = (from stat in Manager.GetIQ<SubActivityStatistic>()
            //               where stat.SubActivity != null && stat.Person !=null && stat.SubActivity.Id == idSubActivity && stat.Person.Id == idPerson && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
            //               select new { idSubActivity = stat.SubActivity.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status = stat.Status, Completion = stat.Completion }).ToList();

            //long idStatistic = (from s in subStat
            //                    group s by s.idSubActivity into g
            //                    let data = g.Max(s => s.CreatedOn)
            //                    select new
            //                    {
            //                        Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault() //TODO: CHECK CREATEDON
            //                    }).FirstOrDefault().Id;


            //return (from s in  Manager.GetIQ<SubActivityStatistic>()
        }
        #endregion
    }
   
}