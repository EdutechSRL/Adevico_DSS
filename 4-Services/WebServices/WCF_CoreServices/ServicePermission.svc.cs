using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath;
using COL_BusinessLogic_v2.UCServices;
using lm.Comol.Core.BaseModules.Federation;
using NHibernate;
using NHibernate.Linq;
using lm.Comol.Core.Data;
using NHibernate.Loader.Entity;

namespace WCF_CoreServices
{
    public class ServicePermission : IServicePermission
    {
        private Int32 GetIdRole(ISession session, Int32 idUser, Int32 idCommunity)
        {
            int idRole = (from s in session.Linq<liteSubscriptionInfo>()
                          where s.IdCommunity == idCommunity && s.IdPerson == idUser && s.Accepted && s.Enabled
                          select s.IdRole).Skip(0).Take(1).ToList().FirstOrDefault<int>();
            if (idRole == null || idRole == 0)
                idRole = (int)COL_BusinessLogic_v2.Main.TipoRuoloStandard.Guest;
            return idRole;
        }
        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser)
        {
            return GetAllowedStandardActionForExternal(source, destination, idUser, null, null);
        }
        public List<StandardActionType> GetAllowedStandardActionForExternal(ModuleObject source, ModuleObject destination, Int32 idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                int idRole = GetIdRole(session, idUser, source.CommunityID);
                //iLinkedService service = null;
                //switch (source.ServiceCode)
                //{
                //    case Services_EduPath.Codex:
                //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                //        service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                //        break;
                //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                //        break;
                //    default:
                //        break;
                //}
                iLinkedService service = ServiceGet(source.ServiceCode, dc);
                if (service != null)
                    actions = service.GetAllowedStandardAction(source, destination, idUser, idRole, source.CommunityID, moduleUserLong, moduleUserString);
            }
            return actions;
        }
        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser)
        {
            return AllowStandardActionForExternal(actionType, source, destination, idUser, null, null);
        }
        public bool AllowStandardActionForExternal(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            Boolean allow = false;
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                int idRole = GetIdRole(session, idUser, source.CommunityID);
                //iLinkedService service = null;
                //switch (source.ServiceCode)
                //{
                //    case Services_EduPath.Codex:
                //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                //        service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                //        break;
                //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                //        break;
                //    case COL_Questionario.ModuleQuestionnaire.UniqueID:
                //        service = new COL_Questionario.Business.ServiceQuestionnaire(dc);
                //        break;
                //    default:
                //        break;
                //}
                iLinkedService service = ServiceGet(source.ServiceCode, dc);
                if (service != null)
                {
                    allow = service.AllowStandardAction(actionType, source, destination, idUser, idRole, moduleUserLong, moduleUserString);
                }
            }
            return allow;
        }
        
        public Boolean AllowActionExecution(long idLink, Int32 idAction, ModuleObject destination, Int32 idUser)
        {
            return AllowActionExecutionForExternal(idLink, idAction, destination, idUser, null, null);
        }
        public Boolean AllowActionExecutionForExternal(long idLink, Int32 idAction, ModuleObject destination, Int32 idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            Boolean allow = false;
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                ModuleLink link = dc.GetById<ModuleLink>(idLink);
                //ApplicationContext oContext = new ApplicationContext();
                //oContext.DataContext = dc;
                if (link != null && link.Action == idAction && link.DestinationItem.Equals(destination))
                {
                    int idSourceCommunity = link.SourceItem.CommunityID;
                    int idRole = GetIdRole(session, idUser, idSourceCommunity);
                    //iLinkedService service = null;
                    //switch (link.SourceItem.ServiceCode)
                    //{
                    //    case Services_EduPath.Codex:
                    //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                    //        break;
                    //    case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                    //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                    //        break;
                    //    case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                    //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                    //        break;
                    //    case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                    //        service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                    //        break;
                    //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                    //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                    //        break;
                    //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                    //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                    //        break;
                    //    default:
                    //        break;
                    //}
                    //iLinkedService service = ServiceGet(serviceCode, dc);
                    iLinkedService service = ServiceGet(link.SourceItem.ServiceCode, dc);
                    if (service != null)
                        allow = service.AllowActionExecution(link, idUser, idSourceCommunity, idRole, moduleUserLong, moduleUserString);
                }
            }
            return allow;
        }

        #region "Save/Evaluate link"
        public void ExecutedAction(long idLink, Boolean isStarted, Boolean isPassed, Int16 completion, Boolean isCompleted, Int16 mark, int idUser)
        {
            ExecutedActionForExternal(idLink, isStarted, isPassed, completion, isCompleted, mark, idUser, null, null);
        }
        public void ExecutedActionForExternal(long idLink, Boolean isStarted, Boolean isPassed, Int16 completion, Boolean isCompleted, Int16 mark, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                ModuleLink link = dc.GetById<ModuleLink>(idLink);
                if (link != null)
                {
                    //iLinkedService service = null;
                    //switch (link.SourceItem.ServiceCode)
                    //{
                    //    case Services_EduPath.Codex:
                    //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                    //        break;
                    //    case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                    //        service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                    //        break;
                    //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                    //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                    //        break;
                    //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                    //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                    //        break;
                    //    default:
                    //        break;
                    //}
                    //

                    //ToDo: alreadyuCompleted!!!
                    bool alreadycompleted = false;

                    iLinkedService service = ServiceGet(link.SourceItem.ServiceCode, dc);
                    if (service != null)
                    {
                        service.SaveActionExecution(link, isStarted, isPassed, completion, isCompleted, mark, idUser, alreadycompleted, moduleUserLong, moduleUserString);
                    }
                }
            }
        }

        /// <summary>
        /// Get evaluation of link
        /// </summary>
        /// <param name="idLink"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public dtoEvaluation EvaluateModuleLink(long idLink, int idUser)
        {
            return EvaluateModuleLinkForExternal(idLink, idUser, null, null);
        }
        /// <summary>
        /// Get evaluation of link
        /// </summary>
        /// <param name="idLink"></param>
        /// <param name="idUser"></param>
        /// <param name="moduleUserLong"></param>
        /// <param name="moduleUserString"></param>
        /// <returns></returns>
        public dtoEvaluation EvaluateModuleLinkForExternal(long idLink, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            dtoEvaluation evaluation = null;
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                ModuleLink link = dc.GetById<ModuleLink>(idLink);
                using (ISession icodeon = NHSessionHelper.GetIcodeonSession())
                {
                    DataContext ic = new DataContext(icodeon);
                    evaluation = EvaluateModule(dc, ic, link, idUser, moduleUserLong, moduleUserString);
                }
            }
            return evaluation;
        }
        private dtoEvaluation EvaluateModule(DataContext dc, DataContext ic, ModuleLink link, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            dtoEvaluation evaluation = null;

            if (link != null)
            {
                //iLinkedService service = null;
                //switch (link.DestinationItem.ServiceCode)
                //{
                //    case Services_EduPath.Codex:
                //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                //        break;
                //    case lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepositoryScorm(dc, ic);
                //        break;
                //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                //        break;
                //    case COL_Questionario.Business.ServiceQuestionnaire.UniqueID:
                //        service = new COL_Questionario.Business.ServiceQuestionnaire(dc);
                //        break;
                //    default:
                //        break;
                //}
                iLinkedService service = ServiceGet(link.DestinationItem.ServiceCode, dc);
                if (service != null)
                {
                    evaluation = service.EvaluateModuleLink(link, idUser, moduleUserLong, moduleUserString);
                }
            }
            return evaluation;
        }

        public void ExecutedActions(List<dtoItemEvaluation<long>> evaluatedLinks, int idUser)
        {
            ExecutedActionsForExternal(evaluatedLinks, idUser, null, null);
        }
        public void ExecutedActionsForExternal(List<dtoItemEvaluation<long>> evaluatedLinks, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);

                List<dtoItemEvaluation<ModuleLink>> links = (from ev in evaluatedLinks
                                                             select new dtoItemEvaluation<ModuleLink>()
                                                             {
                                                                 Item = dc.GetById<ModuleLink>(ev.Item),
                                                                 isCompleted = ev.isCompleted,
                                                                 Completion = ev.Completion,
                                                                 isPassed = ev.isPassed,
                                                                 isStarted = ev.isStarted
                                                             }
                                                                 ).ToList();
                // DA VERIFICARE ASSOLUTAMENTE !!!!
                if (links != null && links.Count > 0)
                {
                    var query = from l in links
                                group l by l.Item.SourceItem.ServiceCode into linksGroup
                                orderby linksGroup.Key
                                select linksGroup;
                    foreach (var groupOfLinks in query)
                    {
                        ExecutedActions(dc, groupOfLinks.Key, groupOfLinks.ToList(), idUser, moduleUserLong, moduleUserString);
                    }
                }
            }
        }

        private void ExecutedActions(DataContext dc, String ServiceCode, List<dtoItemEvaluation<ModuleLink>> links, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            if (links != null && links.Count > 0)
            {
                //iLinkedService service = null;
                //switch (ServiceCode)
                //{
                //    case Services_EduPath.Codex:
                //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                //        service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                //        break;
                //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                //        break;
                //    default:
                //        break;
                //}
                iLinkedService service = ServiceGet(ServiceCode, dc);
                if (service != null)
                {
                    service.SaveActionsExecution(links, idUser, moduleUserLong, moduleUserString);
                    service = null;
                }
            }
        }

        /// <summary>
        /// Get evaluations
        /// </summary>
        /// <param name="idLinks"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<long> idLinks, int idUser)
        {
            return EvaluateModuleLinksForExternal(idLinks, idUser, null, null);
        }
        public List<dtoItemEvaluation<long>> EvaluateModuleLinksForExternal(List<long> idLinks, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            // DA VERIFICARE ASSOLUTAMENTE !!!!
            List<dtoItemEvaluation<long>> evaluations = null;
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                evaluations = EvaluateModuleLinks(session, idLinks, idUser, moduleUserLong, moduleUserString);
            }
            return evaluations;
        }
        private List<dtoItemEvaluation<long>> EvaluateModuleLinks(ISession session, List<long> idLinks, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            List<dtoItemEvaluation<long>> evaluations = new List<dtoItemEvaluation<long>>();
            DataContext dc = new DataContext(session);
            List<ModuleLink> links = (from l in session.Linq<ModuleLink>() where idLinks.Contains(l.Id) select l).ToList();

            var query = from l in links
                        group l by l.DestinationItem.ServiceCode into linksGroup
                        orderby linksGroup.Key
                        select linksGroup;

            using (ISession icodeon = NHSessionHelper.GetIcodeonSession())
            {
                DataContext ic = new DataContext(icodeon);
                foreach (var groupOfLinks in query)
                {
                    evaluations.AddRange(EvaluateModuleLinks(dc, ic, groupOfLinks.Key, groupOfLinks.ToList(), idUser, moduleUserLong, moduleUserString));
                }
            }


            return evaluations;
        }
        private List<dtoItemEvaluation<long>> EvaluateModuleLinks(DataContext dc, DataContext ic, String ServiceCode, List<ModuleLink> links, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            List<dtoItemEvaluation<long>> evaluations = new List<dtoItemEvaluation<long>>();

            if (links != null && links.Count > 0)
            {
                //iLinkedService service = null;
                //switch (ServiceCode)
                //{
                //    case Services_EduPath.Codex:
                //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                //        break;
                //    case lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepositoryScorm(dc, ic);
                //        break;
                //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                //        break;
                //    case COL_Questionario.Business.ServiceQuestionnaire.UniqueID:
                //        service = new COL_Questionario.Business.ServiceQuestionnaire(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                //        break;
                //    default:
                //        break;
                //}
                iLinkedService service = ServiceGet(ServiceCode, dc, ic);
                if (service != null)
                    evaluations = service.EvaluateModuleLinks(links, idUser, moduleUserLong, moduleUserString);
            }
            return evaluations;
        }

        public List<dtoItemEvaluation<long>> GetPendingEvaluations(List<long> idLinks, int idUser)
        {
            return GetPendingEvaluationsForExternal(idLinks, idUser, null, null);
        }
        public List<dtoItemEvaluation<long>> GetPendingEvaluationsForExternal(List<long> idLinks, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            List<dtoItemEvaluation<long>> results = new List<dtoItemEvaluation<long>>();
            List<lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate> pendingLinks = new List<lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate>();
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                pendingLinks = (from l in session.Linq<lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate>()
                                where l.ToUpdate && idLinks.Contains(l.IdLink) && l.IdPerson == idUser
                                select l).ToList();
                if (pendingLinks.Count > 0)
                {
                    try
                    {
                        results = EvaluateModuleLinks(session, (from i in pendingLinks select i.IdLink).Distinct().ToList(), idUser, moduleUserLong, moduleUserString);
                        Dictionary<long, DateTime> evaluated = (from pl in pendingLinks select new { Id = pl.Id, ModifiedOn = pl.ModifiedOn }).ToDictionary(x => x.Id, x => x.ModifiedOn);
                        session.BeginTransaction();

                        foreach (lm.Comol.Core.FileRepository.Domain.ScormPackageWithVersionToEvaluate item in pendingLinks)
                        {
                            session.Refresh(item);
                            if (evaluated[item.Id] == item.ModifiedOn)
                                item.ToUpdate = false;
                        }
                        session.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (session.Transaction.IsActive)
                            session.Transaction.Rollback();
                    }
                }
            }
            return results;
        }
        #endregion

        #region "Permission"
        public long AllowedActionPermission(long idLink)
        {
            long permissions = 0;
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                liteModuleLink link = dc.GetById<liteModuleLink>(idLink);
                if (link != null)
                    permissions = link.Permission;
            }
            return permissions;
        }

        public long ModuleLinkPermission(long idLink, ModuleObject destination, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            long permissions = 0;
            liteModuleLink link = null;
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                link = dc.GetById<liteModuleLink>(idLink);
                if (link != null && !link.DestinationItem.Equals(destination))
                    link = null;
            }

            if (link != null && AllowActionExecutionForExternal(link.Id, link.Action, destination, idUser, moduleUserLong, moduleUserString))
            {
                permissions = link.Permission;
            }
            return permissions;
        }

        public long ModuleLinkActionPermission(long idLink, int idAction, ModuleObject destination, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            long permissions = 0;
            if (AllowActionExecutionForExternal(idLink, idAction, destination, idUser, moduleUserLong, moduleUserString))
            {
                using (ISession session = NHSessionHelper.GetComolSession())
                {
                    DataContext dc = new DataContext(session);
                    liteModuleLink link = dc.GetById<liteModuleLink>(idLink);
                    if (link != null)
                        permissions = link.Permission;
                }

            }
            return permissions;
        }

        public long ActionPermission(ModuleObject source, ModuleObject destination, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            long permissions = 0;
            liteModuleLink link = null;
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                link = (from l in session.Linq<liteModuleLink>() where l.DestinationItem.Equals(destination) && l.SourceItem.Equals(source) select l).FirstOrDefault<liteModuleLink>();
            }

            if (link != null && AllowActionExecutionForExternal(link.Id, link.Action, destination, idUser, moduleUserLong, moduleUserString))
                permissions = link.Permission;
            return permissions;
        }
        #endregion

        /// <summary>
        /// Delete community and allo its object
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="idCommunity"></param>
        /// <param name="idUser"></param>
        public void PhisicalDeleteCommunity(String serviceCode, int idCommunity, int idUser)
        {
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                int idRole = GetIdRole(session, idUser, idCommunity);

                //iLinkedService service = null;
                //switch (serviceCode)
                //{
                //    case Services_EduPath.Codex:
                //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                //        service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                //        break;
                //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                //        break;
                //    default:
                //        break;
                //}
                iLinkedService service = ServiceGet(serviceCode, dc);

                if (service != null)
                    service.PhisicalDeleteCommunity(idCommunity, idUser, System.Configuration.ConfigurationManager.AppSettings["DefaultBaseFileRepository"], System.Configuration.ConfigurationManager.AppSettings["DefaultThumbnailPath"]);
            }
        }
        /// <summary>
        /// Delete repository item of module
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="idFileItem"></param>
        /// <param name="idCommunity"></param>
        /// <param name="idUser"></param>
        public void PhisicalDeleteRepositoryItem(String serviceCode, long idFileItem, int idCommunity, int idUser)
        {
            PhisicalDeleteRepositoryItemForExternal(serviceCode, idFileItem, idCommunity, idUser, null, null);
        }
        public void PhisicalDeleteRepositoryItemForExternal(String serviceCode, long idFileItem, int idCommunity, int idUser, Dictionary<String, long> moduleUserLong, Dictionary<String, String> moduleUserString)
        {
            using (ISession session = NHSessionHelper.GetComolSession())
            {
                DataContext dc = new DataContext(session);
                // int idRole = GetIdRole(session,idUser,idCommunity);

                //iLinkedService service = null;
                //switch (serviceCode)
                //{
                //    case Services_EduPath.Codex:
                //        service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                //        service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                //        break;
                //    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                //        service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                //        break;
                //    case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                //        service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                //        break;
                //    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                //        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                //        break;
                //    default:
                //        break;
                //}
                iLinkedService service = ServiceGet(serviceCode, dc);

                if (service != null)
                    service.PhisicalDeleteRepositoryItem(idFileItem, idCommunity, idUser, moduleUserLong, moduleUserString);
            }
        }


        /// <summary>
        /// Recupera il service dato il service Code,
        /// evitando 50 switch in tutte le funzioni a cui poi tocca "correre dietro" in caso di aggiunta di un servizio!!!
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        private iLinkedService ServiceGet(string serviceCode, iDataContext dc, iDataContext ic = null)
        {
            iLinkedService service = null;
            switch (serviceCode)
            {
                //Percorso formativo
                case Services_EduPath.Codex:
                    service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                    break;

                //Community Diary
                case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                    service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                    break;

                //Project Management
                case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                    service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                    break;

                //Bandi
                case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                    service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                    break;

                //Richieste adesione
                case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                    service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                    break;

                //Ticket
                case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                    service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                    break;

                ////Webinar (WebEx)
                //case "Webinar":     //ToDo: usare UniqueCode
                //    iApplicationContext ac = new ApplicationContext();
                //    ac.DataContext = dc;
                //    service = new WebExService(ac);
                //    break;

                //Questionari
                case COL_Questionario.ModuleQuestionnaire.UniqueID:
                    service = new COL_Questionario.Business.ServiceQuestionnaire(dc);
                    break;

                //Repository - SCORM
                case lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode:
                    service = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepositoryScorm(dc, ic);
                    break;


                default:
                    break;


            }

            return service;
        }

        #region "federation"
        private Enums.FederationResult FederationInternalUserCheck(
           int userId,
           string settings)
        {
            try
            {

                using (ISession session = NHSessionHelper.GetComolSession())
                {
                    DataContext dc = new DataContext(session);

                    lm.Comol.Core.BaseModules.Federation.Business.FederationService service =
                    new lm.Comol.Core.BaseModules.Federation.Business.FederationService(dc);

                    return FederationCheck(service.UserGet(userId), settings);
                }
            } catch
            {

            }

            return Enums.FederationResult.Unknow;
        }



            /// <summary>
            /// Metodo creato per Trentino Sviluppo che chiama i loro servizi per controllare se è un utente Federato
            /// </summary>
            /// <param name="communityId"></param>
            /// <param name="userId"></param>
            /// <param name="externlUrl">ToDo: diventerà OBJECT con i parametri necesari.</param>
            /// <returns></returns>
            /// <remarks>
            /// externlUrl: l'oggetto che lo sostiuirà avrà una classe base con un set minimo di informazioni (es: URL) ed eventuali sottoclassi che lo estendono con ulteriori informazioni specifiche.
            /// </remarks>
            public Enums.FederationResult FederationUserCheck(
            int communityId, 
            int userId, 
            string settings)
        {
            if (String.IsNullOrEmpty(settings))
                return Enums.FederationResult.CommunityNotFederated;


            if (communityId <= 0)
                return FederationInternalUserCheck(userId, settings);


            //lm.Comol.Core.BaseModules.Federation.Enums.FederationType communityFederation = Enums.FederationType.None;

            litePerson person = null;
            liteCommunity community = null;

            Enums.FederationType fedType = Enums.FederationType.None;

            try
            {

                using (ISession session = NHSessionHelper.GetComolSession())
                {
                    DataContext dc = new DataContext(session);

                    lm.Comol.Core.BaseModules.Federation.Business.FederationService service = 
                    new lm.Comol.Core.BaseModules.Federation.Business.FederationService(dc);

                    fedType = service.CommunityFederation(communityId);

                    switch (fedType)
                    {
                        case Enums.FederationType.TrentinoSviluppo:
                            person = service.UserGet(userId);
                            break;
                        case Enums.FederationType.None:
                            return Enums.FederationResult.CommunityNotFederated;
                    }
                    
                }

                switch (fedType)
                {
                    case Enums.FederationType.TrentinoSviluppo:
                        if (person == null || person.Id <= 0)
                        {
                            return Enums.FederationResult.UserNotFound; //o throw new Exception("Persona non trovata");
                        }
                        return FederationCheck(person, settings);

                    caseelse:
                        break;
                }

            }
            catch (Exception)
            {
            }

            return Enums.FederationResult.Unknow;
        }

        public Enums.FederationType FederationCommunityCheck(
            int communityId)
        {
            Enums.FederationType fedType = Enums.FederationType.None;

            try
            {
                using (ISession session = NHSessionHelper.GetComolSession())
                {
                    DataContext dc = new DataContext(session);

                    lm.Comol.Core.BaseModules.Federation.Business.FederationService service =
                    new lm.Comol.Core.BaseModules.Federation.Business.FederationService(dc);

                    fedType = service.CommunityFederation(communityId);
                }
            }
            catch (Exception)
            {
                
            }
            

            return fedType;
        }

        /// <summary>
        /// Controllo esterno
        /// </summary>
        /// <param name="person"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private Enums.FederationResult FederationCheck(litePerson person, string settings)
        {
            //ToDo: External call!

            return Enums.FederationResult.Federated;
        }

    #endregion
    }
}