using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.BaseModules.FileRepository.Domain;

namespace lm.Comol.Core.BaseModules.FileRepository.Business
{
    public partial class ServiceRepositoryScorm : ServiceRepository, iLinkedNHibernateService
    {
        private lm.Comol.Modules.ScormStat.Business.ScormService _ScormService;
        private iDataContext _IcodeonContext;
        private iDataContext _ComolContext;

        private lm.Comol.Modules.ScormStat.Business.ScormService ScormService
        {
            get
            {
                return (_ScormService != null) ? _ScormService : ((_IcodeonContext != null) ? new Modules.ScormStat.Business.ScormService(_ComolContext, _IcodeonContext) : new Modules.ScormStat.Business.ScormService(_ComolContext, null));
            }
        }
          #region initClass
            public ServiceRepositoryScorm(iDataContext comolContext,iDataContext icodeonContext )
                : base(comolContext)
            {
                _IcodeonContext = icodeonContext;
                _ComolContext = comolContext;
                _ScormService = new Modules.ScormStat.Business.ScormService(comolContext, icodeonContext);
            }

        #endregion


        #region Ignore
            public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, int idUser, int idRole, int idCommunity, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
                List<StandardActionType> actions = new List<StandardActionType>();
                return actions;
            }
            public void PhisicalDeleteCommunity(int idCommunity, int idUser, string baseFilePath, string baseThumbnailPath)
            {
            }

            public void PhisicalDeleteRepositoryItem(long idFileItem, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
            }

            public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, int objectTypeId, Dictionary<int, string> translations, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
                return new StatTreeNode<StatFileTreeLeaf>();
            }
            public void SaveActionExecution(ModuleLink link, bool isStarted, bool isPassed, short Completion, bool isCompleted, short mark, int idUser, bool alreadyCompleted, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
       
            }

            public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {

            }
            public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, int idUser, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
                return true;
            }

            public bool AllowActionExecution(ModuleLink link, int idUser, int idCommunity, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
                return true;
            }
        #endregion
           

        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(
            List<ModuleLink> links, 
            int idUser, 
            Dictionary<string, long> moduleUserLong = null, 
            Dictionary<string, string> moduleUserString = null)
        {

            List<dtoItemEvaluation<long>> results = new List<dtoItemEvaluation<long>>();

            links = links.Where(l=> l.DestinationItem.ServiceCode== ModuleRepository.UniqueCode).ToList();

            if (links.Any()){
                
                List<dtoItemToEvaluate> items = 
                    links.Where(l => l.Action == (Int32)ModuleRepository.ActionType.DownloadFile)
                    .Select(l => new dtoItemToEvaluate(l.Id, l.DestinationItem.ObjectLongID, l.DestinationItem.ObjectIdVersion))
                    .ToList();

                List<long> idItems = items
                    .Where(i => i.AlwaysLastVersion)
                    .Select(i => i.IdItem)
                    .Distinct()
                    .ToList();

                List<long> idVersions = items
                    .Where(i => !i.AlwaysLastVersion)
                    .Select(i => i.IdVersion)
                    .Distinct()
                    .ToList();

                if (idItems.Any())
                {
                    idItems = DownloadedItems(idItems, idUser);
                    results.AddRange(
                        (from i in items 
                         where idItems.Contains(i.IdItem) 
                         select new dtoItemEvaluation<long>()
                         {
                             Completion = 100, 
                             isCompleted = true, 
                             isPassed = true, 
                             isStarted = true, 
                             Item = i.Idlink, 
                             IdLink = i.Idlink
                         }).ToList()
                        );
                }

                if (idVersions.Any())
                {
                    idVersions = DownloadedVersions(idVersions, idUser);
                    results.AddRange(
                        (from i in items 
                         where idVersions.Contains(i.IdVersion) 
                         select new dtoItemEvaluation<long>()
                         {
                             Completion = 100, 
                             isCompleted = true, 
                             isPassed = true, 
                             isStarted = true, 
                             Item = i.Idlink, 
                             IdLink = i.Idlink
                         }).ToList()
                        );
                }


                items = links
                    .Where(l => l.Action == (Int32)ModuleRepository.ActionType.PlayFile)
                    .Select(l => new dtoItemToEvaluate(
                        l.Id, 
                        l.DestinationItem.ObjectLongID, 
                        l.DestinationItem.ObjectIdVersion))
                    .ToList();

                List<long> idItemsPlay = items
                    .Where(i => i.AlwaysLastVersion)
                    .Select(i => i.IdItem)
                    .Distinct()
                    .ToList();

                List<long> idVersionPlay = items
                    .Where(i => !i.AlwaysLastVersion)
                    .Select(i => i.IdVersion)
                    .Distinct()
                    .ToList();

                if (idItemsPlay.Any())
                {
                    List<liteRepositoryItem> rItems = GetQuery<liteRepositoryItem>()
                        .Where(i => idItemsPlay.Contains(i.Id))
                        .ToList();

                    if (rItems.Any(i => i.Type == ItemType.Multimedia))
                    {
                        idItems = rItems
                            .Select(i => i.Id)
                            .Distinct()
                            .ToList();

                        idItems = (from p in GetQuery<PlayStatistics>() 
                                   where idItems.Contains(p.IdItem) && p.IdPerson == idUser select p.IdItem)
                                   .Distinct().ToList();

                        results.AddRange(
                            (from i in items 
                             where idItems.Contains(i.IdItem) 
                             select new dtoItemEvaluation<long>()
                             {
                                 Completion = 100, 
                                 isCompleted = true, 
                                 isPassed = true, 
                                 isStarted = true, 
                                 Item = i.Idlink, 
                                 IdLink = i.Idlink
                             }
                            ).ToList());
                    }
                    if (rItems.Any(i => i.Type == ItemType.ScormPackage))
                    {
                        DateTime date = DateTime.Now;

                        foreach(liteRepositoryItem item in rItems)
                        {
                            dtoPackageEvaluation eval = ScormService.GetPackageEvaluation_NewTMP(
                                idUser, 
                                item.Id, 
                                item.UniqueId, 
                                item.UniqueIdVersion, 
                                Modules.ScormStat.Domain.UserStatisticsFrom.Last, 
                                Modules.ScormStat.Domain.PlayCompletionStatus.Ignore, 
                                date);

                            Boolean isStarted = (eval!=null &&  eval.Status!= PackageStatus.notstarted);
                            Boolean isCompleted = (eval!=null && eval.IsCompleted);
                            Boolean isPassed = (eval!=null && eval.IsPassed);
                            int completion = (eval == null ? 0 : eval.Completion);
                            double mark = (eval == null ? 0 : eval.UserScore);
                            bool alreadyCompleted = (eval != null && eval.AlreadyCompleted);


                            results.AddRange(
                                (from i in items 
                                 where i.IdItem == item.Id select new dtoItemEvaluation<long>()
                                 {
                                     Mark = (short)mark, 
                                     Completion = (short)completion, 
                                     isCompleted = isCompleted, 
                                     isPassed = isPassed, 
                                     isStarted = isStarted, 
                                     Item = i.Idlink, 
                                     IdLink = i.Idlink,
                                     AlreadyCompleted = alreadyCompleted
                                 }
                                ).ToList());
                        }
                        
                    }
                }
                if (idVersionPlay.Any())
                {
                    List<liteRepositoryItemVersion> vItems = 
                        GetQuery<liteRepositoryItemVersion>()
                        .Where(i => idVersionPlay.Contains(i.Id))
                        .ToList();

                    if (vItems.Any(i => i.Type == ItemType.Multimedia))
                    {
                        idVersions = vItems.Select(i => i.Id)
                            .Distinct().ToList();

                        idVersions = (
                            from p in GetQuery<PlayStatistics>() 
                            where idVersions.Contains(p.IdVersion) && p.IdPerson == idUser 
                            select p.IdVersion
                            ).Distinct().ToList();

                        results.AddRange(
                            (from i in items where idVersions.Contains(i.IdVersion) select new dtoItemEvaluation<long>()
                            {
                                Completion = 100, 
                                isCompleted = true, 
                                isPassed = true, 
                                isStarted = true, 
                                Item = i.Idlink, 
                                IdLink = i.Idlink
                            }).ToList());
                    }
                    if (vItems.Any(i => i.Type == ItemType.ScormPackage))
                    {
                      
                    }
                }
            }
            return results;
        }

        public dtoEvaluation EvaluateModuleLink(ModuleLink link, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            dtoEvaluation result = new dtoEvaluation();
            
            return result;
        }

        public void PhisicalDeleteCommunity(string baseFileRepositoryPath, int idCommunity, int idUser)
        {
            throw new NotImplementedException();
        }
    }
}