using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository
    {
        #region "Save play evaluation"
        
            public ScormPackageUserEvaluation ScormSaveEvaluation(dtoPackageEvaluation dto, Int32 idPerson,DateTime referenceTime,Boolean isCalculated = false,Boolean isCreatedByPlayer = false)
            {
                liteModuleLink link = null;
                if (dto.IdLink>0){
                    link = Manager.Get<liteModuleLink>(dto.IdLink);
                    if (link != null && link.Id == 0)
                        link = null;    
                }
                return ScormSaveEvaluation(link, dto,idPerson,referenceTime,isCalculated,isCreatedByPlayer);
            }

            public ScormPackageUserEvaluation ScormSaveEvaluation(liteModuleLink link, dtoPackageEvaluation dto, Int32 idPerson, DateTime referenceTime, Boolean isCalculated = false, Boolean isCreatedByPlayer = false)
            {
                ScormPackageUserEvaluation evaluation = null;
                if (idPerson > 0)
                {
                    try
                    {
                        if (dto.Id > 0)
                            evaluation = Manager.Get<ScormPackageUserEvaluation>(dto.Id);
                        else
                            evaluation = ScormQuery(dto, idPerson, (link == null) ? 0 : link.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                        if ((evaluation == null || evaluation.Id == 0) && dto.IdItem>0 && dto.UniqueIdItem != Guid.Empty)
                        {
                            Manager.BeginTransaction();
                            evaluation = ScormPackageUserEvaluation.CreateBy(link, dto, idPerson, referenceTime, isCalculated, isCreatedByPlayer);
                            Manager.SaveOrUpdate(evaluation);
                            Manager.Commit();
                        }
                        else
                        {
                            Manager.BeginTransaction();
                            evaluation.UpdateStatisticsBy(dto, referenceTime, isCalculated);
                            Manager.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        evaluation = null;
                    }
                }
              
                return evaluation;
            }

            public ScormPackageUserEvaluation ScormUpdateEvaluation(dtoPackageEvaluation dto, DateTime referenceTime, Boolean isCalculated)
            {
                ScormPackageUserEvaluation evaluation = null;
                try
                {
                    if (dto.Id > 0)
                    {
                        evaluation = Manager.Get<ScormPackageUserEvaluation>(dto.Id);
                        if (evaluation != null && evaluation.Id == 0){
                            Manager.BeginTransaction();
                            evaluation.UpdateStatisticsBy(dto, referenceTime, isCalculated);
                            Manager.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    evaluation = null;
                }
                return evaluation;
            }
            
            public IQueryable<ScormPackageUserEvaluation> ScormQuery(dtoPackageEvaluation dto, Int32 idPerson, long idLink=0)
            {
                var query = ScormQuery(e=> e.Deleted== BaseStatusDeleted.None && e.IdPerson == idPerson && e.IdItem == dto.IdItem && e.PlaySession==dto.PlaySession);
                if (dto.IdVersion != 0)
                    query = query.Where(e => e.UniqueIdVersion == dto.UniqueIdVersion);
                if (idLink>0)
                    query = query.Where(e => e.IsCreatedByModule && e.IdLink==idLink);
                else
                    query = query.Where(e => !e.IsCreatedByModule);
                return query;
            }
            public IQueryable<ScormPackageUserEvaluation> ScormQuery(Expression<Func<ScormPackageUserEvaluation, bool>> filters)
            {
                return (from q in Manager.GetIQ<ScormPackageUserEvaluation>() select q).Where(filters);
            }
        #endregion
        #region "Get evaluations"
            public Int32 ScormGetPlayIdUser(String playSession, Guid file, Guid version)
            {
                List<ScormPackageUserEvaluation> items = ScormQuery(e => e.PlaySession == playSession && e.Deleted == BaseStatusDeleted.None && e.UniqueIdItem==file && e.UniqueIdVersion== version).ToList();
                if (items != null && (items.Count == 1 || items.Select(i=> i.IdPerson).Distinct().Count()==1))
                    return items.FirstOrDefault().IdPerson;
                else
                    return 0;
            }
            public Int32 ScormGetPlayIdUser(String playSession, long idFile, long idVersion)
            {
                List<ScormPackageUserEvaluation> items = ScormQuery(e => e.PlaySession == playSession && e.Deleted == BaseStatusDeleted.None && e.IdItem == idFile && e.IdVersion == idVersion).ToList();
                if (items != null && (items.Count == 1 || items.Select(i => i.IdPerson).Distinct().Count() == 1))
                    return items.FirstOrDefault().IdPerson;
                else
                    return 0;
            }
            public List<ScormPackageUserEvaluation> ScormGetCompletedEvaluations(Int32 idPerson)
            {
                return ScormQuery(e => e.IdPerson == idPerson && e.Deleted == BaseStatusDeleted.None && e.IsCompleted).ToList();
            }

        public List<ScormPackageUserEvaluation> ScormGetPendingPlayEvaluations(
            Int32 idPerson,
            TimeSpan delay)
        {
            return ScormQuery(e => 
                e.IdPerson == idPerson 
                && e.Deleted == BaseStatusDeleted.None 
                && !e.IsCalculated
                && e.IsCreatedByPlay
                && e.LastUpdate < (DateTime.Now - delay))
                .ToList();
        }

        public IList<ScormPackageWithVersionToEvaluate> GetItemScormToEvaluate(
            Int32 idPerson,
            TimeSpan delay)
        {
            return Manager.GetAll<ScormPackageWithVersionToEvaluate>(
                evItm =>
                !evItm.IsPlaying &&
                evItm.ToUpdate &&
                evItm.ModifiedOn < (DateTime.Now - delay)
                );
        }
        #endregion

        #region Pending evaluations
        public ScormPackageWithVersionToEvaluate ScormAddPendingEvaluation(liteRepositoryItem item, liteRepositoryItemVersion version, Int32 idPerson, long idLink = 0)
        {
            ScormPackageWithVersionToEvaluate pItem = null;
            try
            {
                pItem = (from ScormPackageWithVersionToEvaluate i in Manager.GetIQ<ScormPackageWithVersionToEvaluate>()
                            where i.IdPerson == idPerson && i.IdItem == item.Id && i.IdVersion == version.Id && i.IdLink == idLink
                        select i).Skip(0).Take(1).ToList().FirstOrDefault();
                Manager.BeginTransaction();
                if (pItem == null)
                {
                    pItem = new ScormPackageWithVersionToEvaluate();
                    pItem.IdItem = item.Id;
                    pItem.IdLink = idLink;
                    pItem.IdPerson = idPerson;
                    pItem.IdVersion = version.Id;
                    pItem.UniqueIdItem = item.UniqueId;
                    pItem.UniqueIdVersion = version.UniqueIdVersion;
                }
                pItem.IsPlaying = true;
                pItem.Deleted = BaseStatusDeleted.None;
                pItem.ToUpdate = true;
                pItem.ModifiedOn = DateTime.Now;
                Manager.SaveOrUpdate(pItem);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                pItem = null;
            }
            return pItem;
        }

        public List<lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate> GetPendingEvaluationsForExternal(List<long> idLinks, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            List<dtoItemEvaluation<long>> results = new List<dtoItemEvaluation<long>>();
            List<lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate> pendingLinks = new List<lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate>();
            
            pendingLinks = (from l in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate>()
                            where l.ToUpdate && idLinks.Contains(l.IdLink) && l.IdPerson == idUser
                            select l).ToList();

            return pendingLinks;

            //if (pendingLinks.Count > 0)
            //{
            //    try
            //    {
            //        results = EvaluateModuleLinks((from i in pendingLinks select i.IdLink).Distinct().ToList(), idUser, moduleUserLong, moduleUserString);
            //        Dictionary<long, DateTime> evaluated = (from pl in pendingLinks select new { Id = pl.Id, ModifiedOn = pl.ModifiedOn }).ToDictionary(x => x.Id, x => x.ModifiedOn);
            //        session.BeginTransaction();

            //        foreach (lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate item in pendingLinks)
            //        {
            //            session.Refresh(item);
            //            if (evaluated[item.Id] == item.ModifiedOn)
            //                item.ToUpdate = false;
            //        }
            //        session.Transaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        if (session.Transaction.IsActive)
            //            session.Transaction.Rollback();
            //    }
            //}
           
            //return results;
        }

        //private List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<long> idLinks, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        //{
        //    List<dtoItemEvaluation<long>> evaluations = new List<dtoItemEvaluation<long>>();
        //    DataContext dc = new DataContext(session);
        //    List<ModuleLink> links = (from l in session.Linq<ModuleLink>() where idLinks.Contains(l.Id) select l).ToList();

        //    var query = from l in links
        //                group l by l.DestinationItem.ServiceCode into linksGroup
        //                orderby linksGroup.Key
        //                select linksGroup;

        //    using (ISession icodeon = GetIcodeonSession())
        //    {
        //        DataContext ic = new DataContext(icodeon);
        //        foreach (var groupOfLinks in query)
        //        {
        //            evaluations.AddRange(EvaluateModuleLinks(dc, ic, groupOfLinks.Key, groupOfLinks.ToList(), idUser, moduleUserLong, moduleUserString));
        //        }
        //    }


        //    return evaluations;
        //}
        #endregion
    }
}