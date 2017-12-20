using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Scorm.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;


namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class ScormStatisticsUpdatePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewScormStatisticsUpdate View
        {
            get { return (IViewScormStatisticsUpdate)base.View; }
        }

         public ScormStatisticsUpdatePresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
         public ScormStatisticsUpdatePresenter(iApplicationContext oContext, IViewScormStatisticsUpdate view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

         public void InitView(long IdFile)
         {
             View.FileId = IdFile;
         }

         public void UpdateEvaluation(){
             int IdUser = UserContext.CurrentUserID;
             long IdFile = View.FileId;
             if (!UserContext.isAnonymous)
             {
                 dtoEvaluation evaluation = View.GetScormEvaluation(IdUser, IdFile);
                 List<dtoEvaluationLink> links = GetRelatedLinks(IdUser,IdFile);
                 if (evaluation != null && links.Count > 0)
                 {
                     View.EvaluateLinks(IdUser,  (from l in links select l.LinkId ).ToList(), evaluation);
                     RemoveUpdateActions(IdUser, links);
                 }
             }
         }
         private List<dtoEvaluationLink> GetRelatedLinks(int IdUser, long IdFile)
         {
             List<dtoEvaluationLink> results = null;
             try
             {
                 if ((from e in CurrentManager.GetIQ<ScormPackageToEvaluate>() where e.IdFile == IdFile && e.IdPerson == IdUser && e.ToUpdate select e.Id).Any()) {
                     IList<long> IdLinks = (from l in CurrentManager.GetIQ<ModuleLink>()
                                where l.NotifyExecution && l.DestinationItem.ServiceCode == CoreModuleRepository.UniqueID && l.DestinationItem.ObjectLongID == IdFile
                                select l.Id).ToList();
                     results = (from e in CurrentManager.GetIQ<ScormPackageToEvaluate>()
                                where IdLinks.Contains(e.IdLink) && e.IdPerson == IdUser && e.ToUpdate
                                select new dtoEvaluationLink() { Id = e.Id, LinkId = e.IdLink, ModifiedOn = e.ModifiedOn }).ToList();

                 }
                 else
                     results = new List<dtoEvaluationLink>();
             }
             catch (Exception ex) {
                 results = new List<dtoEvaluationLink>();
             }
             return results;
         }
         private void RemoveUpdateActions(int IdUser, List<dtoEvaluationLink> links)
         {
             List<long> IdItems = (from l in links select l.Id).ToList();
             List<ScormPackageToEvaluate> pendingLinks = (from l in CurrentManager.Linq<ScormPackageToEvaluate>()
                                                   where l.ToUpdate && IdItems.Contains(l.Id) && l.IdPerson == IdUser
                                                   select l).ToList();
             try {
                 CurrentManager.BeginTransaction();
                
                 if (pendingLinks.Count > 0) {
                     foreach (ScormPackageToEvaluate item in pendingLinks)
                         {
                             CurrentManager.Refresh(item);
                             if ((from l in links where l.Id==item.Id select l.ModifiedOn).FirstOrDefault() == item.ModifiedOn)
                                 item.ToUpdate = false;
                         }    
                 }

                 CurrentManager.Commit();
             }
             catch (Exception ex) {
                 CurrentManager.RollBack();
             }
         
         }
             
             //List<dtoItemEvaluation<long>> results = new List<dtoItemEvaluation<long>>();
             //List<ScormToEvaluate> pendingLinks = new List<ScormToEvaluate>();
             //using (ISession session = NHSessionHelper.GetComolSession())
             //{
             //    pendingLinks = (from l in session.Linq<ScormToEvaluate>()
             //                    where l.ToUpdate && linksId.Contains(l.IdLink) && l.IdPerson == UserID
             //                    select l).ToList();
             //    if (pendingLinks.Count > 0)
             //    {
             //        try
             //        {
             //            results = EvaluateModuleLinks(session, (from i in pendingLinks select i.IdLink).Distinct().ToList(), UserID);
             //            Dictionary<long, DateTime> evaluated = (from pl in pendingLinks select new { Id = pl.Id, ModifiedOn = pl.ModifiedOn }).ToDictionary(x => x.Id, x => x.ModifiedOn);
             //            session.BeginTransaction();

             //            foreach (ScormToEvaluate item in pendingLinks)
             //            {
             //                session.Refresh(item);
             //                if (evaluated[item.Id] == item.ModifiedOn)
             //                    item.ToUpdate = false;
             //            }
             //            session.Transaction.Commit();
             //        }
             //        catch (Exception ex)
             //        {
             //            session.Transaction.Rollback();
             //        }
             //    }
             //}
             //return results;
    }
}
