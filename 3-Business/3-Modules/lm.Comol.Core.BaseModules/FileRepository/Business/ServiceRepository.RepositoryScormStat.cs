using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Business
{
    public class ServiceRepositoryScormStat
    {
        public void UpdateScormEvalutation(int communityId, int UserId = -1)
        {

        }


        public List<dtoItemEvaluation<long>> EvaluateRepositoryItem(
        Int64 ItemId,
        Guid ItemUniqueId,
        Guid ItemUniqueIdVersion,
        int idUser
        )
        {

            List<dtoItemEvaluation<long>> results = new List<dtoItemEvaluation<long>>();


            //dtoEvaluation result = new dtoEvaluation();
            DateTime date = DateTime.Now;

            dtoPackageEvaluation eval = ScormService.GetPackageEvaluation_NewTMP(
                                idUser,
                                ItemId,
                                ItemUniqueId,
                                ItemUniqueIdVersion,
                                Modules.ScormStat.Domain.UserStatisticsFrom.Last,
                                Modules.ScormStat.Domain.PlayCompletionStatus.Ignore,
                                date);


            Boolean isStarted = (eval != null && eval.Status != PackageStatus.notstarted);
            Boolean isCompleted = (eval != null && eval.IsCompleted);
            Boolean isPassed = (eval != null && eval.IsPassed);
            int completion = (eval == null ? 0 : eval.Completion);
            double mark = (eval == null ? 0 : eval.UserScore);
            bool alreadyCompleted = (eval != null && eval.AlreadyCompleted);


            dtoItemEvaluation<long> temEval = new dtoItemEvaluation<long>()
            {
                Mark = (short)mark,
                Completion = (short)completion,
                isCompleted = isCompleted,
                isPassed = isPassed,
                isStarted = isStarted,
                Item = ItemId,
                AlreadyCompleted = alreadyCompleted
            }






            results.AddRange(
                (from i in items
                 where i.IdItem == item.Id
                 select new dtoItemEvaluation<long>()
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

            return result;
        }
    }
}
