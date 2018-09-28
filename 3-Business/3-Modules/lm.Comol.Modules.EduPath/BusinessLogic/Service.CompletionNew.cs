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
    public partial class Service
    {
        public void SaveActionExecution(lm.Comol.Core.FileRepository.Domain.ScormPackageUserEvaluation evaluation)
        {
            try
            {
                Boolean save = true;
                long idSubActivity = evaluation.IdObject;
                long idPath = GetPathId_BySubActivityId(idSubActivity);
                Int32 idCommunity = GetPathIdCommunity(idPath);
                Int32 idRole = Manager.GetActiveSubscriptionIdRole(evaluation.IdPerson, idCommunity);
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
                            List<SubActivityStatistic> items = GetUserStatistics(idSubActivity, evaluation.IdPerson, (evaluation.EndPlayOn.HasValue ? evaluation.EndPlayOn.Value : DateTime.Now));
                            SubActivityStatistic last = (items == null ? null : items.FirstOrDefault());
                            switch (settings.Statistics)
                            {
                                case CompletionPolicy.NoUpdateIfCompleted:
                                    if (evaluation.IsPassed && evaluation.IsCompleted)
                                        save = !items.Any(i => ServiceStat.CheckStatusStatistic(i.Status, StatusStatistic.CompletedPassed));
                                    else
                                        save = !items.Any(i => ServiceStat.CheckStatusStatistic(i.Status, StatusStatistic.Completed) || ServiceStat.CheckStatusStatistic(i.Status, StatusStatistic.CompletedPassed));
                                    break;
                                case CompletionPolicy.UpdateOnlyIfBetter:
                                    if (last != null)
                                        save = (last.Completion < evaluation.Completion || (
                                                 ((!ServiceStat.CheckStatusStatistic(last.Status, StatusStatistic.Completed) && !ServiceStat.CheckStatusStatistic(last.Status, StatusStatistic.CompletedPassed))
                                                 && (evaluation.Status == Core.FileRepository.Domain.PackageStatus.completed
                                                     || evaluation.Status == Core.FileRepository.Domain.PackageStatus.completedpassed))
                                                     ));

                                    break;
                                case CompletionPolicy.UpdateOnlyIfWorst:
                                    break;
                            }
                            break;
                    }
                }
                if (save)
                {
                    Manager.BeginTransaction();
                    ServiceStat.InitOrUpdateSubActivityNoTransaction(idSubActivity, evaluation.IdPerson, idRole, evaluation.IdPerson, UC.IpAddress, UC.ProxyIpAddress, evaluation.Completion, (short)evaluation.Completion, (evaluation.Status != lm.Comol.Core.FileRepository.Domain.PackageStatus.notstarted), evaluation.IsCompleted, evaluation.IsPassed);
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {

            }
        }

        
    }
}