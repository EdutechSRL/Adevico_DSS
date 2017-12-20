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
        #endregion
    }
}