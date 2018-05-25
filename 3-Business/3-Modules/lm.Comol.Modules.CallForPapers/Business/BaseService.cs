using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using iTextSharp.text;
using lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel.Helpers.Export;
using lm.Comol.Core.MailCommons.Domain;
using System.Xml;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public abstract class BaseService : iLinkedService
    {
        protected const int maxItemsForQuery = 80;
        protected lm.Comol.Core.Business.BaseModuleManager Manager { get; set; }
        protected iUserContext UC { set; get; }

        protected lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository RepositoryService { get; set; }

        #region initClass
            public BaseService() { }
            public BaseService(iApplicationContext oContext)
            {
                this.Manager = new lm.Comol.Core.Business.BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
                RepositoryService = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(oContext);
            }
            public BaseService(iDataContext oDC)
            {
                this.Manager = new lm.Comol.Core.Business.BaseModuleManager(oDC);
                RepositoryService = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(oDC);
                this.UC = null;
            }
        #endregion

        protected Dictionary<String, Int32> _IdModules = new Dictionary<string, int>();

        protected int ServiceModuleID(String code)
        {
            if (_IdModules==null)
                _IdModules = new Dictionary<string, int>();
            if (!_IdModules.ContainsKey(code))
                _IdModules[code] = Manager.GetModuleID(code);
            return _IdModules[code];
        }
        protected Int32 GetIdRepositoryModule()
        {
            return ServiceModuleID(lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode);
        }

        #region iLinkedService
            #region Not Implemented

                public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
                {
                    return new List<dtoItemEvaluation<long>>();
                }
                public dtoEvaluation EvaluateModuleLink(ModuleLink link, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
                {
                    return new dtoEvaluation() { isCompleted = false, Completion = 0, isPassed = false, isStarted = false };
                }
                public void SaveActionExecution(ModuleLink link, Boolean isStarted, Boolean isPassed, short Completion, Boolean isCompleted, Int16 mark, Int32 idUser, bool alreadyCompleted, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
                {

                }
                public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
                {

                }
                public void PhisicalDeleteCommunity(Int32 idCommunity, Int32 idUser, String baseFilePath, String baseThumbnailPath)
                {

                }
                public void PhisicalDeleteRepositoryItem(long idFileItem,Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
                {

                }
                public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics( long objectId, Int32 objectTypeId, Dictionary<Int32, string> Translations,Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
                {
                    return new StatTreeNode<StatFileTreeLeaf>();
                }
            #endregion
            public bool AllowActionExecution(ModuleLink link, Int32 idUser, Int32 idCommunity, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                litePerson person = Manager.GetLitePerson(idUser);
                switch (link.SourceItem.ObjectTypeID)
                {
                    case (int)ModuleRequestForMembership.ObjectType.RequestForMembership:
                    case (int)ModuleCallForPaper.ObjectType.CallForPaper:
                        return AllowDownloadFileOfCall(link.SourceItem.ObjectLongID, idUser, person, idCommunity, idRole);
                    case (int)ModuleCallForPaper.ObjectType.AttachmentFile:
                        return AllowAttachmentDownload(link.SourceItem.ObjectLongID, idUser, person, idCommunity, idRole);
                    case (int)ModuleCallForPaper.ObjectType.UserSubmission:
                        return AllowDownloadFileOfSubmission(link.SourceItem.ObjectLongID, idUser, person, idCommunity, idRole);
                    case (int)ModuleCallForPaper.ObjectType.SubmittedFile:
                        return AllowDownloadSubmittedFile(link.SourceItem.ObjectLongID, idUser, person, idCommunity, idRole);
                    case (int)ModuleCallForPaper.ObjectType.Revision:
                        return AllowDownloadSignFile(link.SourceItem.ObjectLongID, idUser, person, idCommunity, idRole);
                    case (int)ModuleCallForPaper.ObjectType.VerbaliCommissione:
                        //return true;
                        return AllowDownloadVerbali(link.SourceItem.ObjectLongID, idUser, person, idCommunity, idRole);
                    case (int)ModuleCallForPaper.ObjectType.Integrazioni:
                        //return true;
                        return AllowDownloadIntegrazioni(link.SourceItem.ObjectLongID, idUser, person, idCommunity, idRole);
                default:
                        return false;
                }
            }
            public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Int32 idCommunity, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                List<StandardActionType> actions = new List<StandardActionType>();
                litePerson person = Manager.GetLitePerson(idUser);
                switch (source.ObjectTypeID)
                {
                    case (int)ModuleRequestForMembership.ObjectType.RequestForMembership:
                    case (int)ModuleCallForPaper.ObjectType.CallForPaper:
                        actions = GetStandardActionForFileOfCall(source.ObjectLongID, destination.ObjectLongID, idUser, person, idCommunity, idRole);
                        break;
                    case (int)ModuleCallForPaper.ObjectType.AttachmentFile:
                        actions = GetStandardActionForAttachment(source.ObjectLongID, destination.ObjectLongID, idUser, person, idCommunity, idRole);
                        break;
                    case (int)ModuleCallForPaper.ObjectType.UserSubmission:
                        actions = GetStandardActionForFileOfSubmission(source.ObjectLongID, destination.ObjectLongID, idUser, person, idCommunity, idRole);
                        break;
                    case (int)ModuleCallForPaper.ObjectType.SubmittedFile:
                        actions = GetStandardActionForSubmittedFile(source.ObjectLongID, destination.ObjectLongID, idUser, person, idCommunity, idRole);
                        break;
                }
                return actions;
            }
            public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
            {
                litePerson person = Manager.GetLitePerson(idUser);
                switch (source.ObjectTypeID)
                {
                    case (int)ModuleRequestForMembership.ObjectType.RequestForMembership:
                    case (int)ModuleCallForPaper.ObjectType.CallForPaper:
                        return AllowCallAction(actionType, source.ObjectLongID, person,source.CommunityID, idRole, destination);
                    case (int)ModuleCallForPaper.ObjectType.AttachmentFile:
                        break;
                    case (int)ModuleCallForPaper.ObjectType.UserSubmission:
                        break;
                    case (int)ModuleCallForPaper.ObjectType.SubmittedFile:
                        break;
                }
                return false;
            }
            #region Inherited"
                protected abstract Boolean AllowCallAction(StandardActionType actionType, long idCall, litePerson person,int idCommunity , int idRole, ModuleObject destination);
                protected abstract Boolean AllowDownloadFileOfCall(long idCall, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract Boolean AllowAttachmentDownload(long idAttachment, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract Boolean AllowDownloadFileOfSubmission(long idSubmission, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract Boolean AllowDownloadSubmittedFile(long idSubmittedFile, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract Boolean AllowDownloadSignFile(long idRevision, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract Boolean AllowDownloadVerbali(long idRevision, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract Boolean AllowDownloadIntegrazioni(long idIntegration, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract List<StandardActionType> GetStandardActionForFileOfCall(long idCall, long idItem, int idUser,litePerson person, int idCommunity, int idRole);
                protected abstract List<StandardActionType> GetStandardActionForAttachment(long idAttachment, long idItem, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract List<StandardActionType> GetStandardActionForFileOfSubmission(long idSubmission, long idItem, int idUser, litePerson person, int idCommunity, int idRole);
                protected abstract List<StandardActionType> GetStandardActionForSubmittedFile(long idSubmittedFile, long idItem, int idUser, litePerson person, int idCommunity, int idRole);
            #endregion
        #endregion

            #region "Call"
            public String GetCallName(long idCall)
            {
                String name = "";
                try
                {
                    name = (from c in Manager.GetIQ<BaseForPaper>() where c.Id == idCall select c.Name).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new UnknownCallForPaper();
                }
                return name;
            }
            
            #region "Call Presentation Methods"
                public List<FilterCallVisibility> GetCallVisibilityFilters(ModuleCallForPaper module,Boolean forPortal, int idCommunity, int idUser, CallStatusForSubmitters status)
                {
                    return GetCallVisibilityFilters((module.Administration || module.ManageCallForPapers), forPortal, idCommunity, idUser, CallForPaperType.CallForBids, status);
                }
                public List<FilterCallVisibility> GetCallVisibilityFilters(ModuleRequestForMembership module, Boolean forPortal, int idCommunity, int idUser, CallStatusForSubmitters status)
                {
                    return GetCallVisibilityFilters((module.Administration || module.ManageBaseForPapers), forPortal, idCommunity, idUser, CallForPaperType.RequestForMembership, status);
                }
                public List<FilterCallVisibility> GetCallVisibilityFilters(Boolean forPortal, int idCommunity, int idUser, CallForPaperType type, CallStatusForSubmitters status)
                {
                    return GetCallVisibilityFilters(HasManagePermission(idCommunity, idUser, type), forPortal, idCommunity, idUser, type, status);
                }
                public List<FilterCallVisibility> GetCallVisibilityFilters(Boolean isManager,Boolean forPortal, int idCommunity, int idUser, CallForPaperType type, CallStatusForSubmitters status)
                {
                    List<FilterCallVisibility> filters = new List<FilterCallVisibility>();
                    try
                    {
                        DateTime currentTime = DateTime.Now;
                        liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                        litePerson person = Manager.GetLitePerson(idUser);
                        var query = (from c in Manager.GetIQ<BaseForPaper>()
                                     where ((c.IsPortal == forPortal && forPortal) || (!forPortal && c.Community == community)) && c.Type == type && (isManager || (c.CreatedBy!=null && c.CreatedBy == person))
                                     select c);

                        switch(status){
                            case CallStatusForSubmitters.Draft:
                                query = query.Where(c=>c.Status== CallForPaperStatus.Draft);
                                break;
                            case CallStatusForSubmitters.SubmissionOpened:
                                query = query.Where(c => !c.SubmissionClosed && (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
                                        && (c.EndDate == null || currentTime <= c.EndDate));
                                break;
                            case CallStatusForSubmitters.SubmissionClosed:
                                query = query.Where(c => c.Status!= CallForPaperStatus.Draft && (c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || c.Status== CallForPaperStatus.SubmissionsLimitReached || (c.EndDate != null && currentTime > c.EndDate)));
                                break;

                            default:
                                return filters;
                        }
                        if (query.Where(c=>c.Deleted == BaseStatusDeleted.None).Any())
                            filters.Add(FilterCallVisibility.OnlyVisible);
                        if (query.Where(c=>c.Deleted != BaseStatusDeleted.None).Any())
                            filters.Add(FilterCallVisibility.OnlyDeleted);
                    }

                    catch (Exception ex) {

                    }
                    return filters;
                }

                public List<CallStatusForSubmitters> GetAvailableViews(CallStandardAction action, Boolean isManager, Boolean fromAllcommunities, Boolean forPortal, int idCommunity, int idUser, CallForPaperType type)
                {
                    List<CallStatusForSubmitters> views = new List<CallStatusForSubmitters>();
                    liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                   litePerson person = Manager.GetLitePerson(idUser);
                    switch (action)
                    {
                        case CallStandardAction.Manage:
                            if (CallCount(isManager, forPortal, community, person, type, CallStatusForSubmitters.Draft, FilterCallVisibility.All) > 0)
                                views.Add(CallStatusForSubmitters.Draft);
                            if (CallCount(isManager, forPortal, community, person, type, CallStatusForSubmitters.SubmissionClosed, FilterCallVisibility.All) > 0)
                                views.Add(CallStatusForSubmitters.SubmissionClosed);
                            if (CallCount(isManager, forPortal, community, person, type, CallStatusForSubmitters.SubmissionOpened, FilterCallVisibility.All) > 0)
                                views.Add(CallStatusForSubmitters.SubmissionOpened);
                            if (RevisionCount(fromAllcommunities, forPortal, community, person, type, RevisionType.Manager) > 0)
                                views.Add(CallStatusForSubmitters.Revisions);
                            break;
                        case CallStandardAction.List:
                            if (CallCountBySubmission(false,  fromAllcommunities, forPortal, community, person, type, CallStatusForSubmitters.Submitted) > 0)
                                views.Add(CallStatusForSubmitters.Submitted);
                            if (CallCountBySubmission(false, false, forPortal, community, person, type, CallStatusForSubmitters.SubmissionClosed) > 0)
                                views.Add(CallStatusForSubmitters.SubmissionClosed);
                            if (RevisionCount(fromAllcommunities, forPortal, community, person, type, RevisionType.UserRequired) > 0)
                                views.Add(CallStatusForSubmitters.Revisions);
                            //if (Service.UserCallForPapersCount(CommunityId, UserContext.CurrentUserID, UserCallForPaperStatus.SubmissionOpened) > 0)
                            views.Add(CallStatusForSubmitters.SubmissionOpened);
                            break;
                    }
                    return views;
                }
                public Boolean HasManagePermission(int idCommunity, int idUser,CallForPaperType type) {
                    switch (type) { 
                        case CallForPaperType.CallForBids:
                            ModuleCallForPaper moduleC = CallForPaperServicePermission(idUser, idCommunity);
                            return (moduleC.Administration || moduleC.ManageCallForPapers);
                        case CallForPaperType.RequestForMembership:
                            ModuleRequestForMembership moduleR = RequestForMembershipServicePermission(idUser, idCommunity);
                            return (moduleR.Administration || moduleR.ManageBaseForPapers);
                        default:
                            return false;
                    }
                }
                public long CallCount(Boolean forPortal, int idCommunity, int idUser, CallStatusForSubmitters status, ModuleCallForPaper module, FilterCallVisibility filter)
                {
                    return CallCount((module.ManageCallForPapers || module.Administration),forPortal, idCommunity, idUser, CallForPaperType.CallForBids, status, filter);
                }
                public long CallCount(Boolean forPortal, int idCommunity, int idUser, CallStatusForSubmitters status, ModuleRequestForMembership module, FilterCallVisibility filter)
                {
                    return CallCount((module.ManageBaseForPapers || module.Administration), forPortal, idCommunity, idUser, CallForPaperType.RequestForMembership, status, filter);
                }
                public long CallCount(Boolean forPortal, int idCommunity, int idUser, CallForPaperType type, CallStatusForSubmitters status, FilterCallVisibility filter)
                {
                    return CallCount(HasManagePermission(idCommunity, idUser, type), forPortal, idCommunity, idUser, type, status, filter);
                }
                public long CallCount(Boolean isManager, Boolean forPortal, int idCommunity, int idUser, CallForPaperType type, CallStatusForSubmitters status, FilterCallVisibility filter) {
                    liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                    litePerson person = Manager.GetLitePerson(idUser);
                    return CallCount(isManager,  forPortal, community, person, type, status, filter);
                }
                public long CallCount(Boolean isManager, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, CallStatusForSubmitters status, FilterCallVisibility filter)
                {
                    long count = 0;
                    
                    DateTime currentTime = DateTime.Now;
                    var query = GetBaseCallQuery(isManager, false, forPortal, community, person, type, filter);
                    //var query = (from c in Manager.GetIQ<BaseForPaper>()
                    //             where (fromAllcommunities || (c.IsPortal == forPortal && forPortal) || (!forPortal && c.Community == community)) && (isManager || c.CreatedBy == person) && c.Type == type 
                    //             && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.Deleted != BaseStatusDeleted.None))
                    //             select c);
                    if (status == CallStatusForSubmitters.SubmissionOpened)
                    {
                        query = query.Where(c => !c.SubmissionClosed && (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
                                        && (c.EndDate == null || currentTime <= c.EndDate));
                    }
                    else if (status == CallStatusForSubmitters.Draft)
                        query = query.Where(c => c.Status == CallForPaperStatus.Draft);
                    else
                    {
                        query = query.Where(c => c.Status != CallForPaperStatus.Draft && (c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionsLimitReached || (c.EndDate != null && currentTime > c.EndDate)));
                    }

                    count = query.Count();
                    return count;
                }
                public long RevisionCount(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, CallForPaperType type, RevisionType revType)
                {
                    long count = 0;
                    if (!(person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser))
                    {
                        List<long> idAssignedCalls = new List<long>();
                        if (revType== RevisionType.UserRequired)
                            idAssignedCalls = GetIdCallsByCommunity(fromAllcommunities, forPortal, community, type, FilterCallVisibility.OnlyVisible);

                        //IEnumerable<BaseForPaper> query = (from c in Manager.GetIQ<BaseForPaper>()
                        //                                   where (fromAllcommunities || (c.IsPortal == forPortal && forPortal) || (!forPortal &&
                        //                                   (c.Community == community || idAssignedCalls.Contains(c.Id)))) && c.Type == type
                        //                                   && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.Deleted != BaseStatusDeleted.None))
                        //                                   select c);

                        var query = (from s in Manager.GetIQ<RevisionRequest>()
                                     where s.Deleted == BaseStatusDeleted.None && s.IsActive == false  &&
                                     (fromAllcommunities 
                                     || (s.Submission.Call.IsPortal == forPortal && forPortal)
                                     || (!forPortal && 
                                            (s.Submission.Community == community
                                            || idAssignedCalls.Contains(s.Submission.Call.Id)
                                            )
                                        )
                                     )
                                     && (s.Submission != null && s.Submission.Call != null && s.Submission.Call.Type == type)
                                     select s);
                        switch (revType)
                        {
                            case RevisionType.Manager:
                                query = query.Where(r => (r.Status != RevisionStatus.Approved && r.Status != RevisionStatus.Cancelled) && ((r.Type == RevisionType.Manager && r.RequiredBy == person) || (r.Type == RevisionType.UserRequired && r.RequiredTo == person)));
                                count = query.Count();
                                break;
                            case RevisionType.UserRequired:
                                query = query.Where(r => (r.Status != RevisionStatus.Approved && r.Status != RevisionStatus.Cancelled) && ((r.Type== RevisionType.Manager && r.RequiredTo== person)  || (r.Type== RevisionType.UserRequired && r.RequiredBy== person)));
                                count = query.Count();
                                break;
                            default:
                                count = 0;
                                break;

                        }
                    }
                    return count;
                }
            #endregion
            #region "Submission Presentation Methods"
                public long CallCountBySubmission(Boolean fromAllcommunities, Boolean forPortal, int idCommunity, int idUser, CallForPaperType type, CallStatusForSubmitters status)
                {
                    try
                    {
                        liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                        litePerson person = Manager.GetLitePerson(idUser);
                        return CallCountBySubmission(false, fromAllcommunities, forPortal, community, person, type, status);
                    }
                    catch (Exception ex) {
                        return 0;
                    }
                }
                public long CallCountBySubmission(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, CallStatusForSubmitters status)
                {
                    long count = 0;
                    Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);

                    var query = (from s in Manager.GetIQ<UserSubmission>()
                                 where s.Deleted == BaseStatusDeleted.None && (!isAnonymous && s.Owner == person) && (fromAllcommunities || (s.Call.IsPortal == forPortal && forPortal) || (!forPortal && s.Community == community)) && (s.Call!=null && s.Call.Type==type)
                                 select s);
                    switch (status)
                    {
                        case CallStatusForSubmitters.Submitted:
                            count = query.Where(s=>  s.Status >= SubmissionStatus.submitted).Select(s=> s.Call.Id).ToList().Distinct().Count();
                            break;
                        case CallStatusForSubmitters.SubmissionClosed:
                            var queryClosedCall = GetBaseForPaperQuery(forAdministrationMode,fromAllcommunities, forPortal, community, person, type, status, FilterCallVisibility.OnlyVisible);
                            if (isAnonymous)
                                count = queryClosedCall.Select(c => c.Id).Count();
                            else{
                                List<long> idSubmittedCalls = query.Where(s => s.Status >= SubmissionStatus.submitted).Select(s => s.Call.Id).ToList().Distinct().ToList();
                                var calls = queryClosedCall.Select(c => new { Id = c.Id, IsPublic = c.IsPublic, ForSubscribedUsers = c.ForSubscribedUsers }).ToList();

                                //List<long> idCalls = queryClosedCall.Select(c => c.Id).ToList().Except(idSubmittedCalls).ToList();

                                count = calls.Where(c => (c.IsPublic || (c.ForSubscribedUsers && person != null && person.TypeID != (Int32)UserTypeStandard.Guest && person.TypeID != (Int32)UserTypeStandard.PublicUser)) && !idSubmittedCalls.Contains(c.Id)).ToList().Count();
                                count += GetIdCallsBySubmissionQuery(calls.Where(c => !c.IsPublic && !idSubmittedCalls.Contains(c.Id)).Select(c => c.Id).ToList(),
                                    fromAllcommunities, forPortal, community, person, type, status, FilterCallVisibility.OnlyVisible).Count();
                            }
                            break;
                        case CallStatusForSubmitters.SubmissionOpened:
                            var queryOpenCall = GetBaseForPaperQuery(forAdministrationMode,fromAllcommunities, forPortal, community, person, type, status, FilterCallVisibility.OnlyVisible);
                            if (isAnonymous)
                                count = queryOpenCall.Select(c => c.Id).Count();
                            else {
                                // Find submitted Calls
                                List<long> idSubmittedCalls = query.Where(s => s.Status >= SubmissionStatus.submitted).Select(s => s.Call.Id).ToList().Distinct().ToList();
                                if (idSubmittedCalls.Count > 0) {
                                    var querySubmission = (from s in Manager.GetIQ<UserSubmission>()
                                                           where s.Deleted == BaseStatusDeleted.None && (!isAnonymous && s.Owner == person) && idSubmittedCalls.Contains(s.Call.Id)
                                                           select new { Id = s.Id, IdSubmitter = s.Type.Id, Status = s.Status }).ToList();
                                    List<long> idSubmitters = querySubmission.Select(sb => sb.IdSubmitter).ToList().Distinct().ToList();
                                    List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                                      where idSubmitters.Contains(s.Id)
                                                                        select  s).ToList();
                                    foreach (SubmitterType submitter in submitters.Where(s => s.AllowMultipleSubmissions).ToList())
                                    {
                                        if (querySubmission.Where(s => s.IdSubmitter == submitter.Id).Count() < submitter.MaxMultipleSubmissions || (querySubmission.Where(s => s.IdSubmitter == submitter.Id).Count() == submitter.MaxMultipleSubmissions && querySubmission.Where(s => s.IdSubmitter == submitter.Id && s.Status< SubmissionStatus.submitted).Any()))
                                            idSubmittedCalls.Remove(submitter.Call.Id);
                                    }
                                }
                                var calls = queryOpenCall.Select(c => new { Id = c.Id, IsPublic = c.IsPublic, ForSubscribedUsers = c.ForSubscribedUsers }).ToList();

                                count = calls.Where(c => (c.IsPublic || (c.ForSubscribedUsers && person !=null && person.TypeID != (Int32) UserTypeStandard.Guest  && person.TypeID != (Int32) UserTypeStandard.PublicUser))  && !idSubmittedCalls.Contains(c.Id)).ToList().Count();


                                    //idSubmittedCalls = query.Where(s => s.Status < SubmissionStatus.submitted && !idSubmittedCalls.Contains(s.Call.Id)).Select(s => s.Call.Id).ToList();
                                count += GetIdCallsBySubmissionQuery(calls.Where(c => !c.IsPublic && ! c.ForSubscribedUsers  && !idSubmittedCalls.Contains(c.Id)).Select(c => c.Id).ToList(),
                                    fromAllcommunities, forPortal, community, person, type, status, FilterCallVisibility.OnlyVisible).Count();
                            }
                            break;
                        default:
                            count = 0;
                            break;

                    }
                    return count;
                }

                private IEnumerable<BaseForPaper> GetBaseCallQuery(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, CallForPaperType type, FilterCallVisibility filter)
                {
                    List<long> idAssignedCalls = new List<long>();
                    if (!forAdministrationMode)
                        idAssignedCalls = GetIdCallsByCommunity(fromAllcommunities, forPortal, community, type, filter);

                    IEnumerable<BaseForPaper> query = (from c in Manager.GetIQ<BaseForPaper>()
                                                       where (fromAllcommunities || (c.IsPortal == forPortal && forPortal) || (!forPortal &&
                                                       (c.Community == community || idAssignedCalls.Contains(c.Id)))) && c.Type == type
                                                       && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.Deleted != BaseStatusDeleted.None))
                                                       select c);
                    //if (!forAdministrationMode)
                    //{
                    //    IEnumerable<BaseForPaper> pQuert = (from c in Manager.GetIQ<BaseForPaper>()
                    //                                        where c.Type == type && (fromAllcommunities || (c.IsPortal == forPortal && forPortal)
                    //                                        || (!forPortal
                    //                                                 && (c.Community == community
                    //                                                     ||
                    //                                                     (c.CommunityAssignments.Any() && c.CommunityAssignments.Where(a => a.AssignedTo == community && a.Deleted == BaseStatusDeleted.None).Any())
                    //                                                     ||
                    //                                                     (c.RoleAssignments.Any() && c.RoleAssignments.Where(a => a.Community == community && a.Deleted == BaseStatusDeleted.None).Any())
                    //                                                     )
                    //                                         ))
                    //                                        && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.Deleted != BaseStatusDeleted.None))
                    //                                        select c);
                    //}
                    return query;
                }
                private List<long> GetIdCallsByCommunity(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, CallForPaperType type, FilterCallVisibility filter)
                {
                    List<long> results = new List<long>();
                    results.AddRange((from c in Manager.GetIQ<BaseForPaper>() where c.Type == type && c.Community == community && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.Deleted != BaseStatusDeleted.None)) select c.Id).ToList());
                    results.AddRange((from c in Manager.GetIQ<BaseForPaperCommunityAssignment>() where c.AssignedTo == community && c.Deleted== BaseStatusDeleted.None
                                      && c.BaseForPaper.Type == type && c.BaseForPaper.Community == community && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.BaseForPaper.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.BaseForPaper.Deleted != BaseStatusDeleted.None))
                                      select c.BaseForPaper.Id).ToList());
                    results.AddRange((from c in Manager.GetIQ<BaseForPaperRoleAssignment>() 
                                      where c.Community == community && c.Deleted== BaseStatusDeleted.None 
                                      && c.BaseForPaper.Type == type && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.BaseForPaper.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.BaseForPaper.Deleted != BaseStatusDeleted.None))
                                      select c.BaseForPaper.Id).ToList());
                    return (results.Any()) ? results.Distinct().ToList() : results;
                    //return (call!=null && community != null) && 
                    //    (
                    //        (call.Community == community) 
                    //        ||
                    //        (
                    //        (call.CommunityAssignments.Where(a=>a.AssignedTo== community && a.Deleted== BaseStatusDeleted.None).Any())
                    //        )
                    //    );
                }
                private IEnumerable<BaseForPaper> GetBaseCallQuery(Boolean isManager, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, FilterCallVisibility filter)
                {
                    IEnumerable<BaseForPaper> query = GetBaseCallQuery(isManager,fromAllcommunities, forPortal, community, type, filter).Where(c => (isManager || c.CreatedBy == person));
                    return query;
                }
                protected IEnumerable<BaseForPaper> GetBaseForPaperQuery(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, CallStatusForSubmitters status, FilterCallVisibility filter)
                {
                    DateTime currentTime = DateTime.Now;
                    DateTime oldTime = currentTime.AddMonths(-4);
                    IEnumerable<BaseForPaper> query = GetBaseCallQuery(forAdministrationMode,fromAllcommunities, forPortal, community, type, filter);
                    Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);

                    switch (status){
                        case CallStatusForSubmitters.Draft:
                            query = query.Where(c=>(c.Status == CallForPaperStatus.Draft));
                            if (isAnonymous)
                                query = query.Where(c=> c.IsPublic);
                            break;
                        case CallStatusForSubmitters.SubmissionClosed:
                            query = query.Where(c=>(c.Status != CallForPaperStatus.Draft && (c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || c.Status== CallForPaperStatus.SubmissionsLimitReached || (c.EndDate != null && currentTime > c.EndDate)))
                                    && (
                                        (forAdministrationMode || c.EndDate == null || (forPortal && c.IsPortal) || (fromAllcommunities && c.IsPortal) || (!c.IsPortal && c.Community == community))
                                        ||
                                            (   !forAdministrationMode 
                                                && (c.EndDate != null && c.EndDate> oldTime )
                                                && ((!forPortal && c.IsPortal ) || (fromAllcommunities && !c.IsPortal) || (!c.IsPortal && c.Community !=community)
                                                    )
                                            ) 
                                        )
                                     );
                            if (isAnonymous)
                                query = query.Where(c=> c.IsPublic);
                            break;
                        case CallStatusForSubmitters.SubmissionOpened:
                            query = query.Where(c=> !c.SubmissionClosed &&
                                         (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
                                         && (c.EndDate == null || currentTime <= c.EndDate));
                            if (isAnonymous)
                                query = query.Where(c=> c.IsPublic);
                            break;
                    }
                    return query;
                }

                //
                protected List<long> GetIdCallsBySubmissionQuery(List<long> idCalls, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, CallStatusForSubmitters status, FilterCallVisibility filter)
                {
                    List<long> results = new List<long>();
                    Int32 pageIndex = 0;

                    List<long> idPagedCalls = idCalls.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedCalls.Any())
                    {
                        results = (from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                   where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo == person && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                   select pa.BaseForPaper.Id).ToList();
                        idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();
                        if (forPortal && idPagedCalls.Any()) {
                            results.AddRange((from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                              where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo == person.TypeID && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                              select pa.BaseForPaper.Id).ToList());
                            idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();
                        }
                        if (!fromAllcommunities && idPagedCalls.Any())
                        {
                            results.AddRange((from pa in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                              where idPagedCalls.Contains(pa.BaseForPaper.Id) && community != null && pa.AssignedTo == community && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                              select pa.BaseForPaper.Id).ToList());
                            idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();

                            if (idPagedCalls.Any()) {
                                Int32 idRole = Manager.GetActiveSubscriptionIdRole(person.Id, community.Id);
                                results.AddRange((from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                  where idPagedCalls.Contains(pa.BaseForPaper.Id) && community != null && pa.Community == community && pa.AssignedTo == idRole && idRole >0 && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                                  select pa.BaseForPaper.Id).ToList());
                            }
                        }
                        else {
                            var callCommunities = (from pa in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                               where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo != null && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                               select new { idCall = pa.BaseForPaper.Id,idCallCommunity = (pa.BaseForPaper.Community==null) ?0 : pa.BaseForPaper.Community.Id,idCommunity = pa.AssignedTo.Id }).ToList();
                            List<int> idCommunities = callCommunities.Select(i => i.idCommunity).ToList();
                            List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
                            // add other assigned communities
                            results.AddRange((from c in callCommunities where subscriptions.Where(s => s.IdCommunity == c.idCommunity && c.idCallCommunity != c.idCommunity).Any() select c.idCall).ToList());
                            idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();

                            if (idPagedCalls.Any())
                            {
                                //add by permissions
                                List<LazySubscription> tSubscriptions = subscriptions.Where(s => callCommunities.Where(c => c.idCommunity == c.idCallCommunity && c.idCommunity == s.IdCommunity).Any()).ToList();
                                idCommunities = tSubscriptions.Select(s=> s.IdCommunity).Distinct().ToList();
                                Int32 idModule = (type== CallForPaperType.CallForBids) ? ServiceModuleID(ModuleCallForPaper.UniqueCode) : ServiceModuleID(ModuleRequestForMembership.UniqueCode);
                                Int32 idRole = 0;
                                long requiredPermission = (type == CallForPaperType.CallForBids) ? (long)ModuleCallForPaper.Base2Permission.ListCalls : (long)ModuleRequestForMembership.Base2Permission.ListRequests;

                                List<CommunityModuleAssociation> modules = (from cModule in Manager.GetIQ<CommunityModuleAssociation>()
                                           where cModule.Enabled && cModule.Service.Available && idCommunities.Contains(cModule.Community.Id) && cModule.Service.Id == idModule
                                           select cModule).ToList();

                                foreach (CommunityModuleAssociation m in modules)
                                {
                                    idRole = tSubscriptions.Where(s => s.IdCommunity == m.Community.Id).Select(s => s.IdRole).FirstOrDefault();
                                    var qPermission = (from crmp in Manager.GetIQ<LazyCommunityModulePermission>()
                                                        where crmp.CommunityID == m.Community.Id && crmp.ModuleID == idModule && crmp.RoleID==idRole 
                                                        select crmp).Skip(0).Take(1).FirstOrDefault();
                                    if (qPermission != null && qPermission.PermissionLong > 0 && PermissionHelper.CheckPermissionSoft(requiredPermission, qPermission.PermissionLong))
                                    {
                                        results.AddRange(callCommunities.Where(c=>c.idCallCommunity== c.idCommunity && c.idCallCommunity== m.Community.Id).Select(c=> c.idCall).ToList());
                                    }
                                }
                                idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();
                            }

                            if (idPagedCalls.Any()) {
                                var callRoles = (from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                 where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo != null && pa.Community != null && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                                 select new { idCall = pa.BaseForPaper.Id, idRole = pa.AssignedTo, idCommunity = pa.Community.Id }).ToList();
                                idCommunities = callRoles.Select(i => i.idCommunity).ToList();
                                subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
                                results.AddRange((from c in callRoles where subscriptions.Where(s => s.IdCommunity == c.idCommunity && s.IdRole== c.idRole).Any() select c.idCall).ToList());
                            }
                        }
                        pageIndex++;
                        idPagedCalls = idCalls.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }




                    ////var query = (from c in Manager.GetIQ<BaseForPaper>() select c);
                    //for (int i = 0; i < idCalls.Count; i += maxItemsForQuery)
                    //{
                    //    var pagedCalls = idCalls.Skip(i).Take(maxItemsForQuery).ToArray();

                          
                    //    if (!fromAllcommunities)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        var communities = (from pa in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                    //                           where pagedCalls.Except(results).ToList().Contains(pa.BaseForPaper.Id) && pa.AssignedTo != null && !pa.Deny && pa.Deleted == BaseStatusDeleted.None 
                    //                           select new { idCall = pa.BaseForPaper.Id, idCommunity = pa.AssignedTo.Id }).ToList();
                    //        List<int> idCommunities = GetComunitiesWithSubmissionPermission(communities.Select(cm => cm.idCommunity).Distinct().ToList(), person, type);
                    //        results.AddRange(communities.Where(c => idCommunities.Contains(c.idCommunity) && pagedCalls.Except(results).ToList().Contains(c.idCall)).Select(c => c.idCall).ToList());

                    //        var roles = (from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
                    //                     where pagedCalls.Except(results).ToList().Contains(pa.BaseForPaper.Id) && pa.AssignedTo != null && !pa.Deny && pa.Deleted == BaseStatusDeleted.None 
                    //                           select new { idCall = pa.BaseForPaper.Id, role = pa.AssignedTo , community= pa.Community }).ToList();
                    //        List<int> idRoleCommunities = GetComunitiesWithSubmissionPermission(roles.Select(cm => cm.community.Id).Distinct().ToList(), person, type);
                    //        foreach (int idRoleCommunity in idRoleCommunities) {
                    //            Role role = Manager.GetRole(Manager.GetActiveSubscriptionRoleId(person, roles.Where(r=>r.community.Id==idRoleCommunity).Select(r=>r.community).FirstOrDefault()));

                    //            results.AddRange(roles.Where(c => c.community.Id == idRoleCommunity && c.role==role && pagedCalls.Except(results).ToList().Contains(c.idCall)).Select(c => c.idCall).ToList());
                    //        }
                    //    }
                    //}
                    return results.Distinct().ToList();
                }

                //private List<int> GetComunitiesWithSubmissionPermission(List<int> communities, litePerson person, CallForPaperType type)
                //{
                //    List<int> results = new List<int>();
                //    switch (type) { 
                //        case CallForPaperType.CallForBids:
                //            foreach (int idCommunity in communities) { 
                //                ModuleCallForPaper moduleC = CallForPaperServicePermission(person,idCommunity);
                //                if (moduleC.ViewCallForPapers)
                //                    results.Add(idCommunity);
                //            }
                //            break;
                //        case CallForPaperType.RequestForMembership:
                //            foreach (int idCommunity in communities) { 
                //                ModuleRequestForMembership moduleR = RequestForMembershipServicePermission(person,idCommunity);
                //                if (moduleR.ViewBaseForPapers)
                //                    results.Add(idCommunity);
                //            }
                //            break;
                //    }
                //    return results;
                //}
                //private List<int> GetRolesWithSubmissionPermission(List<int> communities, litePerson person, CallForPaperType type)
                //{
                //    List<int> results = new List<int>();
                //    switch (type)
                //    {
                //        case CallForPaperType.CallForBids:
                //            foreach (int idCommunity in communities)
                //            {
                //                ModuleCallForPaper moduleC = CallForPaperServicePermission(person, idCommunity);
                //                if (moduleC.ViewCallForPapers)
                //                    results.Add(idCommunity);
                //            }
                //            break;
                //        case CallForPaperType.RequestForMembership:
                //            foreach (int idCommunity in communities)
                //            {
                //                ModuleRequestForMembership moduleR = RequestForMembershipServicePermission(person, idCommunity);
                //                if (moduleR.ViewBaseForPapers)
                //                    results.Add(idCommunity);
                //            }
                //            break;
                //    }
                //    return results;
                //}

                #region ""
                //public List<dtoItemPermission> GetCalls(CallStandardAction action, Boolean fromAllcommunities, Boolean forPortal, Community community, litePerson person, CallForPaperType type, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
                //{
                //    List<dtoItemPermission> items = null;
                //    try
                //    {
                //        DateTime currentTime = DateTime.Now;
                //        Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);
                //        var query = (from s in Manager.GetIQ<UserSubmission>()
                //                     where s.Deleted == BaseStatusDeleted.None && (!isAnonymous && s.Owner == person) && (fromAllcommunities || (s.Call.IsPortal == forPortal && forPortal) || (!forPortal && s.Community == community)) && (s.Call != null && s.Call.Type == type)
                //                     select s);
                //        switch (status)
                //        {
                //            case CallStatusForSubmitters.Submitted:
                //                query = query.Where(s => s.Status >= SubmissionStatus.submitted).OrderBy(s => s.Call.Name).ThenBy(s => s.Call.EndDate);
                //                if (type == CallForPaperType.CallForBids)
                //                    items = query.Select(s =>
                //                         new dtoItemPermission(s.Call.Id, s.Community, status,
                //                         new dtoSubmissionInfo(s.Id) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate },
                //                         new dtoCall()
                //                         {
                //                             Id = s.Call.Id,
                //                             Name = s.Call.Name,
                //                             Edition = s.Call.Edition,
                //                             ShortDescription = s.Call.ShortDescription,
                //                             Description = s.Call.Description,
                //                             StartDate = s.Call.StartDate,
                //                             EndDate = s.Call.EndDate,
                //                             AwardDate = ((CallForPaper)s.Call).AwardDate,
                //                             SubmissionClosed = s.Call.SubmissionClosed,
                //                             Type = s.Call.Type,
                //                             DisplayWinner = ((CallForPaper)s.Call).DisplayWinner,
                //                             Community = s.Community,
                //                             IsPublic = s.Call.IsPublic,
                //                             Status = s.Call.Status,
                //                             EvaluationType = ((CallForPaper)s.Call).EvaluationType,
                //                             AllowSubmissionExtension = s.Call.AllowSubmissionExtension
                //                         }
                //                         )).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                //                else if (type == CallForPaperType.RequestForMembership)
                //                    items = query.Select(s =>
                //                         new dtoItemPermission(s.Call.Id, s.Community, status,
                //                         new dtoSubmissionInfo(s.Id) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate },
                //                             new dtoRequest()
                //                             {
                //                                 Id = s.Call.Id,
                //                                 Name = s.Call.Name,
                //                                 Edition = s.Call.Edition,
                //                                 ShortDescription = s.Call.ShortDescription,
                //                                 Description = s.Call.Description,
                //                                 StartDate = s.Call.StartDate,
                //                                 EndDate = s.Call.EndDate,
                //                                 StartMessage = ((RequestForMembership)s.Call).StartMessage,
                //                                 SubmissionClosed = s.Call.SubmissionClosed,
                //                                 Type = s.Call.Type,
                //                                 EndMessage = ((RequestForMembership)s.Call).EndMessage,
                //                                 Community = s.Community,
                //                                 IsPublic = s.Call.IsPublic,
                //                                 Status = s.Call.Status,
                //                                 AllowSubmissionExtension = s.Call.AllowSubmissionExtension
                //                             }
                //                         )).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                //                break;
                //            default:
                //                if (action== CallStandardAction.List)
                //                    items = GetCalls(query, fromAllcommunities, forPortal, community, person, status, filter, pageIndex, pageSize);
                //                else
                //                    items = GetCalls(fromAllcommunities, forPortal, community, person, status, filter, pageIndex, pageSize);
                //                break;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        items = new List<dtoItemPermission>();
                //    }
                //    return items;
                //}
                #endregion
                public Boolean IsCallAvailableByUser(long idCall, Int32 idUser)
                {
                    litePerson person = null;
                    if (idUser == 0)
                        person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                    else
                        person = Manager.GetLitePerson(idUser);
                    return IsCallAvailableByUser(idCall, person);
                }

                // CAMBIO MODALITA' di ACCESSO
                public Boolean IsCallAvailableByUser(long idCall,litePerson person)
                {
                    Boolean permission = false;
                    if (person != null)
                    {
                        permission = (from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                      where pa.BaseForPaper.Id == idCall 
                                      && pa.AssignedTo.Id == person.Id  
                                      && !pa.Deny 
                                      && pa.Deleted == BaseStatusDeleted.None
                                      select pa.BaseForPaper.Id).Any();
                       
                        if (!permission) {
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);

                            //NOTA:
                            //LASCIO qui questo controllo, perchè altrimenti è NECESSARIO 
                            //che l'utente "anonimo" sia iscritto alla comunità con ruolo "-4", cioè "Utente Non autenticato"

                            if (call != null && call.IsPublic)
                            {
                                return true;
                            }

                            if (call != null && call.IsPortal)
                            {
                                permission = (
                                    call.ForSubscribedUsers 
                                    && (person.TypeID != (int)UserTypeStandard.Guest 
                                        && person.TypeID != (int)UserTypeStandard.PublicUser)) 
                                    ||
                                    (from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                              where pa.BaseForPaper.Id == idCall 
                                              && pa.AssignedTo == person.TypeID 
                                              && !pa.Deny 
                                              && pa.Deleted == BaseStatusDeleted.None
                                              select pa.BaseForPaper.Id).Any();
                            }
                            else if (call!=null && call.Community !=null)
                            {
                                permission = IsCallAvailableByCommunity(idCall, person);
                                if (!permission) {
                                    permission = IsCallAvailableByRole(idCall, person);
                                }
                            }
                        }
                    }
                    return permission;
                }
                private Boolean IsCallAvailableByCommunity(long idCall,litePerson person)
                {
                    Boolean permission = false;
                    if (person != null)
                    {
                        var query = (from pa in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                     where pa.BaseForPaper.Id == idCall 
                                     && !pa.Deny 
                                     && pa.Deleted == BaseStatusDeleted.None 
                                     && pa.AssignedTo != null
                                     select pa);

                        List<Int32> idCommunities = query.Select(a => a.AssignedTo.Id).Distinct().ToList();
                        
                        if (idCommunities.Any())
                        {
                            List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
                            permission = subscriptions.Where(s => idCommunities.Contains(s.IdCommunity)).Any();
                        }
                    }
                    return permission;
                }
                private Boolean IsCallAvailableByRole(long idCall,litePerson person)
                {
                    Boolean permission = false;
                    if (person != null)
                    {
                        var query = (from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                     where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.Community != null
                                     select pa);
                        List<Int32> idCommunities = query.Select(a => a.Community.Id).Distinct().ToList();
                        if (idCommunities.Any()) {
                            List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
                            var result = query.Select(a => new { IdRole = a.AssignedTo, IdCommunity = a.Community.Id }).ToList();
                            permission = result.Where(a => subscriptions.Where(s => s.IdCommunity == a.IdCommunity && s.IdRole == a.IdRole).Any()).Any();
                        }
                    }
                    return permission;
                }
            #endregion
                public Boolean ExistCallForEnroll(lm.Comol.Core.DomainModel.Helpers.SearchRange range,  CallForPaperType type, Int32 idCommunity=-1)
                {
                    Boolean result = false;
                    try
                    {
                        result = GetSubmissions(range, type, idCommunity).Select(s => s.Id).Any();
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }
                private IEnumerable<UserSubmission> GetSubmissions(lm.Comol.Core.DomainModel.Helpers.SearchRange range, CallForPaperType type, Int32 idCommunity = -1)
                {
                    var query = (from s in Manager.GetIQ<UserSubmission>()
                                 where s.isAnonymous && s.Deleted == BaseStatusDeleted.None && s.Call != null && s.Call.Type == type
                                 select s);

                    switch (range)
                    {
                        case SearchRange.Communities:
                            query = query.Where(s => !s.Call.IsPortal);
                            break;
                        case SearchRange.Community:
                            query = query.Where(s => !s.Call.IsPortal && s.Call.Community != null && s.Call.Community.Id == idCommunity);
                            break;
                        case SearchRange.Portal:
                            query = query.Where(s => s.Call.IsPortal);
                            break;
                        default:
                            break;
                    }
                    return query;
                }
        #endregion

               

        #region "Submitters"
            public SubmitterType GetSubmitterType(long typeId)
            {
                return Manager.Get<SubmitterType>(typeId);
            }
        #endregion 

      
        #region "Edit Cfp"
            public CallForPaperType GetCallType(long idCall) { 
                CallForPaperType found = CallForPaperType.None;
                try
                {
                    found = (from c in Manager.GetIQ<BaseForPaper>() where c.Id == idCall select c.Type).FirstOrDefault();
                }
                catch (Exception ex) { }

                return found;
            }
            public BaseForPaper GetCall(long idCall)
            {
                return Manager.Get<BaseForPaper>(idCall);
            }
            public dtoBaseForPaper GetDtoBaseCall(long idCall)
            {
                dtoBaseForPaper call = null;
                try
                {
                    call = (from c in Manager.GetAll<BaseForPaper>(c=>c.Id == idCall)
                     select new dtoBaseForPaper()
                     {
                         AllowSubmissionExtension = c.UseStartCompilationDate,
                         Deleted = c.Deleted,
                         Description = c.Description,
                         Edition = c.Edition,
                         EndDate = c.EndDate,
                         Id = c.Id,
                         SubmissionClosed = c.SubmissionClosed,
                         IsPortal = c.IsPortal,
                         ForSubscribedUsers = c.ForSubscribedUsers,
                         IsPublic = c.IsPublic,
                         Name = c.Name,
                         IdDssMethod= c.IdDssMethod,
                         IdDssRatingSet = c.IdDssRatingSet,
                         IsDssMethodFuzzy= c.IsDssMethodFuzzy,
                         UseManualWeights = c.UseManualWeights,
                         OrderedWeights = c.UseOrderedWeights,
                         FuzzyMeWeights = c.FuzzyMeWeights,
                         RevisionSettings = c.RevisionSettings,
                         AcceptRefusePolicy= c.AcceptRefusePolicy,
                         OverrideHours = c.OverrideHours,
                         OverrideMinutes = c.OverrideMinutes,
                         StartDate = c.StartDate,
                         Status = c.Status,
                         Summary = c.Summary,
                         Community = c.Community,
                         Owner = c.CreatedBy,
                         AllowDraft = c.AllowPrintDraft,
                         AttachSign = c.AttachSign,
                         Type = c.Type,
                         Tags = c.Tags,
                         AdvacedEvaluation = c.AdvacedEvaluation
                     }).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex) { 
                
                }
                return call;
            }

            public RequestForMembership SaveCallSettings(dtoRequest dto, int idCommunity, Boolean validate)
            {
                RequestForMembership call = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (dto != null)
                    {
                        Manager.BeginTransaction();
                        call = Manager.Get<RequestForMembership>(dto.Id);
                        if (call == null)
                        {
                            call = new RequestForMembership();
                            call.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            if (idCommunity == 0)
                            {
                                call.IsPortal = true;
                                call.IsPublic = true;
                            }
                            else
                                call.Community = Manager.GetLiteCommunity(idCommunity);
                            call.Type = CallForPaperType.RequestForMembership;
                            call.Status = CallForPaperStatus.Draft;
                            call.StartMessage = "--";
                            call.EndMessage = "--";
                        }
                        else
                            call.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        call.UseStartCompilationDate = dto.AllowSubmissionExtension;
                        call.Description = dto.Description;
                        call.Summary = dto.Summary;
                        call.Edition = dto.Edition;
                        call.RevisionSettings = dto.RevisionSettings;
                        call.AcceptRefusePolicy = dto.AcceptRefusePolicy;
                        call.Name = dto.Name;
                        call.OverrideHours = dto.OverrideHours;
                        call.OverrideMinutes = dto.OverrideMinutes;
                        call.StartDate = dto.StartDate;

                        call.AdvacedEvaluation = dto.AdvacedEvaluation;

                        if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
                        {
                            call.StartDate = dto.EndDate.Value;
                            call.EndDate = dto.StartDate;
                        }
                        else
                            call.EndDate = dto.EndDate;
                        call.SubmissionClosed = dto.SubmissionClosed;
                        if (!validate)
                            call.Status = dto.Status;
                        Manager.SaveOrUpdate(call);
                        Manager.Commit();
                        if (validate && ValidateStatus(call, dto.Status))
                        {
                            Manager.BeginTransaction();
                            call.Status = dto.Status;
                            Manager.SaveOrUpdate(call);
                            Manager.Commit();
                        }
                        else if (validate)
                            throw new CallForPaperInvalidStatus();
                    }
                }
                catch (CallForPaperInvalidStatus ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return call;
            }

            #region "Clone Call"
                public BaseForPaper CloneCall(long idCall, Int32 idCreator,Int32 idCommunity, String prefix, String filePath,String thumbnailPath)
                {
                    BaseForPaper destinationCall = null;
                    try
                    {
                        Manager.BeginTransaction();
                        BaseForPaper sourceCall = Manager.Get<BaseForPaper>(idCall);
                        litePerson p = Manager.GetLitePerson(idCreator);
                        if (sourceCall != null && p != null && p.TypeID != (int)UserTypeStandard.Guest)
                        {
                            switch (sourceCall.Type)
                            {
                                case CallForPaperType.CallForBids:
                                    destinationCall = CloneCall((CallForPaper)sourceCall, idCommunity, p, prefix);
                                    
                                    break;
                                case CallForPaperType.RequestForMembership:
                                    destinationCall = CloneCall((RequestForMembership)sourceCall, idCommunity, p, prefix);
                                    break;
                            }
                            if (destinationCall != null) {
                                CloneAvailability(destinationCall, idCommunity,  sourceCall, p);
                                Dictionary<long, SubmitterType> sItems = CloneSubmitters(destinationCall, sourceCall, p);
                                if (destinationCall != null && destinationCall.Type == CallForPaperType.CallForBids)
                                    CloneEvaluationCommittees(destinationCall, sourceCall, p, sItems);
                                if (sourceCall.Attachments.Any())
                                    CloneAttachments(destinationCall, idCommunity, sourceCall, p, filePath,thumbnailPath, sItems);
                                CloneMailTemplate(destinationCall, sourceCall, p, sItems);
                                if (sourceCall.Sections.Any())
                                {
                                    Dictionary<long, FieldDefinition> fields = CloneSections(destinationCall, sourceCall, p, sItems);
                                    if (fields != null && fields.Keys.Count > 0)
                                        CloneProfileAttributeAssociation(destinationCall, sourceCall, p, fields);
                                }
                            }
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        destinationCall = null;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return destinationCall;
                }
                private RequestForMembership CloneCall(RequestForMembership sourceCall, Int32 idCommunity, litePerson person, String prefix)
                {
                    RequestForMembership call = new RequestForMembership();
                    call.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    call.Name = prefix + sourceCall.Name;
                    call.UseStartCompilationDate = sourceCall.UseStartCompilationDate;
                    call.Community = (idCommunity > 0) ? Manager.GetLiteCommunity(idCommunity) : null;
                    call.IdDssMethod = sourceCall.IdDssMethod;
                    call.IdDssRatingSet = sourceCall.IdDssRatingSet;
                    call.IsDssMethodFuzzy = sourceCall.IsDssMethodFuzzy;
                    call.UseManualWeights = sourceCall.UseManualWeights;
                    call.FuzzyMeWeights = sourceCall.FuzzyMeWeights;
                    call.UseOrderedWeights = sourceCall.UseOrderedWeights;
                    call.Deleted = BaseStatusDeleted.None;
                    call.Description = sourceCall.Description;
                    call.Edition = sourceCall.Edition;
                    call.IsPortal = (idCommunity>0) ? false : sourceCall.IsPortal;
                    call.IsPublic = sourceCall.IsPublic;
                    call.NotificationEmail = sourceCall.NotificationEmail;
                    call.OverrideHours = sourceCall.OverrideHours;
                    call.OverrideMinutes = sourceCall.OverrideMinutes;
                    call.RevisionSettings = sourceCall.RevisionSettings;
                    call.AcceptRefusePolicy = sourceCall.AcceptRefusePolicy;
                    call.Status = CallForPaperStatus.Draft;
                    call.SubmissionClosed = false;
                    call.Summary = sourceCall.Summary;
                    call.Type = sourceCall.Type;
                    call.StartMessage = sourceCall.StartMessage;
                    call.EndMessage = sourceCall.EndMessage;
                    call.StartDate = DateTime.Now.Date;
                    call.ForSubscribedUsers = sourceCall.ForSubscribedUsers;
                    Manager.SaveOrUpdate(call);
                    return call;
                }
                private CallForPaper CloneCall(CallForPaper sourceCall,Int32 idCommunity, litePerson person, String prefix)
                {
                    CallForPaper call = new CallForPaper();
                    call.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    call.Name = prefix + sourceCall.Name;
                    call.UseStartCompilationDate = sourceCall.UseStartCompilationDate;
                    call.AwardDate = "";
                    call.Community = (idCommunity > 0) ? Manager.GetLiteCommunity(idCommunity) : null;
                    call.Deleted = BaseStatusDeleted.None;
                    call.Description = sourceCall.Description;
                    call.DisplayWinner = sourceCall.DisplayWinner;
                    call.Edition = sourceCall.Edition;
                    call.EvaluationType = sourceCall.EvaluationType;
                    call.IdDssMethod = sourceCall.IdDssMethod;
                    call.IdDssRatingSet = sourceCall.IdDssRatingSet;
                    call.IsDssMethodFuzzy = sourceCall.IsDssMethodFuzzy;
                    call.UseManualWeights = sourceCall.UseManualWeights;
                    call.FuzzyMeWeights = sourceCall.FuzzyMeWeights;
                    call.IsPortal = (idCommunity > 0) ? false : sourceCall.IsPortal;
                    call.IsPublic = sourceCall.IsPublic;
                    call.NotificationEmail = sourceCall.NotificationEmail;
                    call.OneCommitteeMembership = sourceCall.OneCommitteeMembership;
                    call.OverrideHours = sourceCall.OverrideHours;
                    call.OverrideMinutes = sourceCall.OverrideMinutes;
                    call.RevisionSettings = sourceCall.RevisionSettings;
                    call.AcceptRefusePolicy = sourceCall.AcceptRefusePolicy;
                    call.Status = CallForPaperStatus.Draft;
                    call.SubmissionClosed = false;
                    call.Summary = sourceCall.Summary;
                    call.Type = sourceCall.Type;
                    call.StartDate = DateTime.Now.Date;
                    call.ForSubscribedUsers = sourceCall.ForSubscribedUsers;
                    Manager.SaveOrUpdate(call);
                    return call;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="dCall">cloned call</param>
                /// <param name="sourceCall">call To clone</param>
                /// <param name="person">who clone the call</param>
                /// <returns>old id submitter and new submitter</returns>
                private Dictionary<long, SubmitterType> CloneSubmitters(BaseForPaper dCall, BaseForPaper sourceCall, litePerson person)
                {
                    Dictionary<long, SubmitterType> items = new Dictionary<long, SubmitterType>();
                    Int32 displayOrder = 1;
                    dCall.SubmittersType = new List<SubmitterType>();
                    foreach (SubmitterType s in sourceCall.SubmittersType.Where(s => s.Deleted == BaseStatusDeleted.None).OrderBy(s => s.DisplayOrder).ToList())
                    {
                        SubmitterType submitter = new SubmitterType();
                        submitter.Call = dCall;
                        submitter.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        submitter.Name = s.Name;
                        submitter.Description = s.Description;
                        submitter.Deleted = BaseStatusDeleted.None;
                        submitter.AllowMultipleSubmissions = s.AllowMultipleSubmissions;
                        if (!s.AllowMultipleSubmissions || s.MaxMultipleSubmissions < 1)
                            submitter.MaxMultipleSubmissions = 1;
                        else
                            submitter.MaxMultipleSubmissions = s.MaxMultipleSubmissions;

                        submitter.DisplayOrder = displayOrder;
                        Manager.SaveOrUpdate(submitter);
                        displayOrder++;
                        dCall.SubmittersType.Add(submitter);
                        items.Add(s.Id, submitter);
                    }
                    return items;
                }
                /// <summary>
                ///     Clone call availability for user / community / portal
                /// </summary>
                /// <param name="dCall">cloned call</param>
                /// <param name="sourceCall">call To clone</param>
                /// <param name="person">who clone the call</param>
                private void CloneAvailability(BaseForPaper dCall, Int32 destinationIdCommunity, BaseForPaper sourceCall, litePerson person)
                {
                    List<BaseForPaperAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperAssignment>()
                                                                where a.BaseForPaper.Id == sourceCall.Id && a.Deleted == BaseStatusDeleted.None
                                                                select a).ToList();

                    List<BaseForPaperAssignment> toClone = new List<BaseForPaperAssignment>(); 
                    foreach (BaseForPaperAssignment a in assignments.Where(a=>a.GetType()== typeof(BaseForPaperCommunityAssignment)).ToList())
                    {
                        BaseForPaperCommunityAssignment cAssignment = new BaseForPaperCommunityAssignment();
                        cAssignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        cAssignment.AssignedTo = ((BaseForPaperCommunityAssignment)a).AssignedTo;
                        cAssignment.BaseForPaper = dCall;
                        cAssignment.Deny = ((BaseForPaperCommunityAssignment)a).Deny;
                        toClone.Add(cAssignment);
                    }
                    foreach (BaseForPaperAssignment a in assignments.Where(a => a.GetType() == typeof(BaseForPaperPersonTypeAssignment)).ToList())
                    {
                        BaseForPaperPersonTypeAssignment cAssignment = new BaseForPaperPersonTypeAssignment();
                        cAssignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        cAssignment.AssignedTo = ((BaseForPaperPersonTypeAssignment)a).AssignedTo;
                        cAssignment.BaseForPaper = dCall;
                        cAssignment.Deny = ((BaseForPaperPersonTypeAssignment)a).Deny;
                        toClone.Add(cAssignment);
                    }
                    foreach (BaseForPaperAssignment a in assignments.Where(a => a.GetType() == typeof(BaseForPaperPersonAssignment)).ToList())
                    {
                        BaseForPaperPersonAssignment cAssignment = new BaseForPaperPersonAssignment();
                        cAssignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        cAssignment.AssignedTo = ((BaseForPaperPersonAssignment)a).AssignedTo;
                        cAssignment.BaseForPaper = dCall;
                        cAssignment.Deny = ((BaseForPaperPersonAssignment)a).Deny;
                        toClone.Add(cAssignment);
                    }
                    foreach (BaseForPaperAssignment a in assignments.Where(a => a.GetType() == typeof(BaseForPaperRoleAssignment)).ToList())
                    {
                        BaseForPaperRoleAssignment cAssignment = new BaseForPaperRoleAssignment();
                        cAssignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        cAssignment.AssignedTo = ((BaseForPaperRoleAssignment)a).AssignedTo;
                        cAssignment.BaseForPaper = dCall;
                        cAssignment.Deny = ((BaseForPaperRoleAssignment)a).Deny;
                        cAssignment.Community = ((BaseForPaperRoleAssignment)a).Community;
                        toClone.Add(cAssignment);
                    }
                    if (toClone.Any())
                        Manager.SaveOrUpdateList(toClone);
                }

                private lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier CreateRepositoryIdentifier(BaseForPaper dCall, Int32 destinationIdCommunity)
                {
                    lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier rIdentifier = new Core.FileRepository.Domain.RepositoryIdentifier();
                    if (dCall.IsPortal)
                        rIdentifier.Type = Core.FileRepository.Domain.RepositoryType.Portal;
                    else
                    {
                        rIdentifier.Type = Core.FileRepository.Domain.RepositoryType.Community;
                        rIdentifier.IdCommunity = destinationIdCommunity;
                    }
                    return rIdentifier;
                }
                private lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier CreateRepositoryIdentifier(BaseForPaper call)
                {
                    lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier rIdentifier = new Core.FileRepository.Domain.RepositoryIdentifier();
                    if (call.IsPortal)
                        rIdentifier.Type = Core.FileRepository.Domain.RepositoryType.Portal;
                    else
                    {
                        rIdentifier.Type = Core.FileRepository.Domain.RepositoryType.Community;
                        rIdentifier.IdCommunity = (call.Community != null ? call.Community.Id : 0);
                    }
                    return rIdentifier;
                }
                /// <summary>
                ///     Clone File attachments
                /// </summary>
                /// <param name="dCall">destination call</param>
                /// <param name="sCall">source call</param>
                /// <param name="person">user who clone</param>
                /// <param name="sourcePath">source file path</param>
                /// <param name="submittersInfo">dictionary of old submitters id with new submitters</param>
                private void CloneAttachments(BaseForPaper dCall,Int32 destinationIdCommunity, BaseForPaper sCall, litePerson person, String filePath,String thumbnailPath,Dictionary<long, SubmitterType> submittersInfo)
                {
                    Int32 displayOrder = 1;
                    String moduleCode = (dCall.Type== CallForPaperType.CallForBids ? ModuleCallForPaper.UniqueCode : ModuleRequestForMembership.UniqueCode);
                    Int32 idModule = ServiceModuleID(moduleCode);
                    lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier rIdentifier = CreateRepositoryIdentifier(dCall,destinationIdCommunity);
                    foreach (AttachmentFile fa in sCall.Attachments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Item != null ).OrderBy(a => a.DisplayOrder).ToList())
                    {
                        try
                        {
                            AttachmentFile attachment = new AttachmentFile();
                            attachment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            attachment.Call = dCall;
                            attachment.Description = fa.Description;
                            attachment.DisplayOrder = displayOrder;
                            attachment.ForAll = fa.ForAll;
                            if (fa.Item != null) {
                                Manager.SaveOrUpdate(attachment);
                                lm.Comol.Core.FileRepository.Domain.liteRepositoryItem item = (fa.Item.IsInternal ? null : fa.Item);
                                if (item == null && fa.Item!=null )
                                {
                                    lm.Comol.Core.FileRepository.Domain.RepositoryItem cItem = null; 
                                    lm.Comol.Core.FileRepository.Domain.RepositoryItemVersion cVersion = null;
                                    RepositoryService.CloneInternalItem(attachment.Id, idModule, moduleCode, fa.Item.Id,  0, person, filePath, thumbnailPath, rIdentifier, ref cItem, ref cVersion); 
                                    if (cItem!=null){
                                        item = RepositoryService.ItemGet(cItem.Id);
                                    }
                                }
                                if (item != null)
                                {
                                    attachment.Item = item;
                                    ModuleLink link = new ModuleLink(fa.Link.Description, fa.Link.Permission, fa.Link.Action);
                                    link.CreateMetaInfo(Manager.GetPerson(person.Id), UC.IpAddress, UC.ProxyIpAddress);
                                    link.DestinationItem = ModuleObject.CreateLongObject(item.Id, item, fa.Link.DestinationItem.ObjectTypeID, destinationIdCommunity, fa.Link.DestinationItem.ServiceCode, fa.Link.DestinationItem.ServiceID);
                                    link.AutoEvaluable = false;
                                    link.SourceItem = ModuleObject.CreateLongObject(attachment.Id, attachment, fa.Link.SourceItem.ObjectTypeID, destinationIdCommunity, fa.Link.SourceItem.ServiceCode, fa.Link.SourceItem.ServiceID);
                                    Manager.SaveOrUpdate(link);
                                    attachment.Link = Manager.Get<liteModuleLink>(link.Id);
                                    Manager.SaveOrUpdate(attachment);
                                }
                                else
                                    Manager.DeletePhysical(attachment);
                            }
                            if (!fa.ForAll && attachment != null)
                            {
                                foreach (AttachmentAssignment sAssignment in (from ta in Manager.GetIQ<AttachmentAssignment>() where ta.Attachment!=null && ta.Attachment.Id== fa.Id && ta.Deleted == BaseStatusDeleted.None select ta).ToList())
                                {
                                    SubmitterType submitter = null;
                                    if (sAssignment.SubmitterType != null && submittersInfo.ContainsKey(sAssignment.SubmitterType.Id))
                                        submitter = submittersInfo[sAssignment.SubmitterType.Id];

                                    AttachmentAssignment assignment = new AttachmentAssignment();
                                    assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    assignment.Call = sCall;
                                    assignment.SubmitterType = submitter;
                                    assignment.Attachment = attachment;
                                    Manager.SaveOrUpdate(assignment);
                                }
                            }
                        }
                        catch (Exception ex) { 
                    
                        }          
                    displayOrder++;
                    }
                }
                /// <summary>
                ///     Clone mail templates
                /// </summary>
                /// <param name="dCall">cloned call</param>
                /// <param name="sourceCall">call To clone</param>
                /// <param name="person">who clone the call</param>
                /// <param name="submittersInfo">dictionary with old ID and new submitter</param>
                private void CloneMailTemplate(BaseForPaper dCall, BaseForPaper sourceCall, litePerson person,Dictionary<long, SubmitterType> submittersInfo)
                {
                    List<SubmitterTemplateMail> sTemplates = (from a in Manager.GetIQ<SubmitterTemplateMail>()
                                                                where a.Call.Id == sourceCall.Id && a.Deleted == BaseStatusDeleted.None
                                                                select a).ToList();
                    foreach (SubmitterTemplateMail sTemplate in sTemplates)
                    {
                        SubmitterTemplateMail st = new SubmitterTemplateMail();
                        st.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        st.Body = sTemplate.Body;
                        st.Call = dCall;
                        st.IdLanguage = sTemplate.IdLanguage;
                        st.MailSettings = sTemplate.MailSettings;
                        st.Name = sTemplate.Name;
                        st.Subject = sTemplate.Subject;
                        Manager.SaveOrUpdate(st);

                        foreach (TemplateAssignment tAssignment in (from ta in Manager.GetIQ<TemplateAssignment>() where ta.Template != null && ta.Template.Id== sTemplate.Id && ta.Deleted== BaseStatusDeleted.None select ta).ToList())
                        {
                            SubmitterType submitter = null;
                            if (tAssignment.SubmitterType != null && submittersInfo.ContainsKey(tAssignment.SubmitterType.Id))
                                submitter = submittersInfo[tAssignment.SubmitterType.Id];
                        
                            TemplateAssignment assignment = new TemplateAssignment();
                            assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            assignment.Call = dCall;
                            assignment.SubmitterType = submitter;
                            assignment.Template = st;
                            Manager.SaveOrUpdate(assignment);
                        }
                    }

                    List<ManagerTemplateMail> mTemplates = (from a in Manager.GetIQ<ManagerTemplateMail>()
                                                                where a.Call.Id == sourceCall.Id && a.Deleted == BaseStatusDeleted.None
                                                                select a).ToList();
                    foreach (ManagerTemplateMail mTemplate in mTemplates)
                    {
                        ManagerTemplateMail mt = new ManagerTemplateMail();
                        mt.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        mt.Body = mTemplate.Body;
                        mt.Call = dCall;
                        mt.IdLanguage = mTemplate.IdLanguage;
                        mt.MailSettings = mTemplate.MailSettings;
                        mt.Name = mTemplate.Name;
                        mt.Subject = mTemplate.Subject;
                        mt.NotifyTo = mTemplate.NotifyTo;
                        mt.NotifyFields = mt.NotifyFields;
                        Manager.SaveOrUpdate(mt);
                    }
                }

                /// <summary>
                ///     Clone all available section of a call/request
                /// </summary>
                /// <param name="nCall">new call</param>
                /// <param name="sourceCall">old call</param>
                /// <param name="p">person who clone call</param>
                /// <param name="submittersInfo">dictionary of old submitters id with new submitters</param>
                private Dictionary<long, FieldDefinition> CloneSections(BaseForPaper nCall, BaseForPaper sourceCall, litePerson p, Dictionary<long, SubmitterType> submittersInfo)
                {
                    Int32 displayOrder = 1;
                    Dictionary<long, FieldDefinition> results = new Dictionary<long, FieldDefinition>();
                    foreach (FieldsSection source in sourceCall.Sections.Where(s=>s.Deleted== BaseStatusDeleted.None).OrderBy(s=>s.DisplayOrder).ToList())
                    {
                        FieldsSection section = new FieldsSection();
                        section.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        section.Description = source.Description;
                        section.DisplayOrder = displayOrder;
                        section.Name = section.DisplayOrder.ToString() + "-" + source.Name;
                        section.Call = nCall;
                        section.Fields = new List<FieldDefinition>();
                        Manager.SaveOrUpdate(section);
                        nCall.Sections.Add(section);

                        if (source.Fields.Any() && source.Fields.Where(f => f.Deleted == BaseStatusDeleted.None).Any())
                        {
                            Int32 fieldOrder = 1;
                            foreach (FieldDefinition field in source.Fields.Where(f => f.Deleted == BaseStatusDeleted.None).OrderBy(f=>f.DisplayOrder).ToList())
                            {
                                FieldDefinition f = CloneField(section, field, p, submittersInfo, fieldOrder);
                                if (f != null)
                                {
                                    section.Fields.Add(f);
                                    results.Add(field.Id, f);
                                    fieldOrder++;
                                }
                            }
                        }
                        Manager.SaveOrUpdate(nCall);
                        displayOrder++;
                    }
                    return results;
                }

                /// <summary>
                ///     Clone field from old call to new call
                /// </summary>
                /// <param name="person">who clone the call</param>
                /// <param name="submittersInfo">dictionary of old submitters id with new submitters</param>
                /// <param name="displayOrder">new field display order</param>
                /// <param name="sField">source field</param>
                /// <param name="dSection">destination section</param>
                /// <returns></returns>
                private FieldDefinition CloneField(FieldsSection dSection,FieldDefinition sField,litePerson person, Dictionary<long, SubmitterType> submittersInfo, Int32 displayOrder)
                {
                    FieldDefinition field = null;
                    if (sField != null && dSection != null)
                    {
                        field = new FieldDefinition();
                        switch (sField.Type)
                        {
                            case FieldType.CheckboxList:
                            case FieldType.RadioButtonList:
                            case FieldType.DropDownList:
                                field.MaxOption = sField.MaxOption;
                                field.MinOption = sField.MinOption;
                                break;
                            case FieldType.Disclaimer:
                                field.MaxOption = sField.MaxOption;
                                field.MinOption = sField.MinOption;
                                field.DisclaimerType = sField.DisclaimerType;
                                break;
                            case FieldType.TableSimple:
                            case FieldType.TableReport:
                                field.TableMaxRows = sField.TableMaxRows;
                                field.TableCols = sField.TableCols;
                                field.TableMaxTotal = sField.TableMaxTotal;
                                break;
                        }
                        field.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        field.Call = dSection.Call;
                        field.Section = dSection;
                        field.Description = sField.Description;
                        field.DisplayOrder = displayOrder;
                        field.Mandatory = sField.Mandatory;
                        field.Type = sField.Type;
                        field.ToolTip = sField.ToolTip;
                        field.RegularExpression = sField.RegularExpression;
                        field.Name = sField.Name;
                        field.MaxLength = sField.MaxLength;
                        Manager.SaveOrUpdate(field);

                        if (field.Type == FieldType.CheckboxList || field.Type == FieldType.RadioButtonList || field.Type == FieldType.DropDownList ||field.Type == FieldType.Disclaimer)
                            CloneOptions(person, field, sField.Options.Where(o => o.Deleted == BaseStatusDeleted.None).ToList());
                        Manager.SaveOrUpdate(field);

                        foreach (FieldAssignment a in (from a in Manager.GetIQ<FieldAssignment>() where a.Deleted == BaseStatusDeleted.None && a.Field.Id == sField.Id select a).ToList()) {
                            if (a.SubmitterType != null && submittersInfo.ContainsKey(a.SubmitterType.Id))
                            {
                                FieldAssignment assignment = new FieldAssignment();
                                assignment.Field = field;
                                assignment.SubmitterType = submittersInfo[a.SubmitterType.Id];
                                assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(assignment);
                            }
                        }
                     }
                    return field;
                }

                /// <summary>
                ///     Clone Profile AttributeAssociation
                /// </summary>
                /// <param name="dCall">destination call</param>
                /// <param name="sCall">source call</param>
                /// <param name="person">who clone the call</param>
                /// <param name="fieldsInfo">dictionary with source field id and destination field</param>
                private void CloneProfileAttributeAssociation(BaseForPaper dCall, BaseForPaper sCall, litePerson person,Dictionary<long, FieldDefinition> fieldsInfo)
                {
                    List<ProfileAttributeAssociation> dAssociations = new List<ProfileAttributeAssociation>();
                    List<ProfileAttributeAssociation> sAssociations = (from af in Manager.GetIQ<ProfileAttributeAssociation>()
                                                                          where af.Call != null && af.Deleted == BaseStatusDeleted.None && af.Call.Id == sCall.Id
                                                                          select af).ToList();
                    foreach (ProfileAttributeAssociation a in sAssociations)
                    {
                        if (a.Field != null && fieldsInfo.ContainsKey(a.Field.Id))
                        {
                            ProfileAttributeAssociation association = new ProfileAttributeAssociation();
                            association.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            association.Attribute = a.Attribute;
                            association.Call = dCall;
                            association.Field = fieldsInfo[a.Field.Id];
                            dAssociations.Add(association);
                        }
                    }
                    if (dAssociations.Any())
                        Manager.SaveOrUpdateList(dAssociations);
                }

                private void CloneEvaluationCommittees(BaseForPaper dCall, BaseForPaper sCall, litePerson person, Dictionary<long, SubmitterType> submittersInfo) {
                    Int32 displayOrder = 1;
                    foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee sComittee in (from c in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee>() where c.Deleted == BaseStatusDeleted.None && c.Call.Id == sCall.Id orderby c.DisplayOrder select c)) {
                        lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee comittee = new lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee();
                        comittee.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        comittee.Call = dCall;
                        comittee.AssignedTypes= new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommitteeAssignedSubmitterType>();
                        comittee.Criteria= new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.BaseCriterion>();
                        comittee.Description=sComittee.Description;
                        comittee.DisplayOrder=displayOrder;
                        comittee.ForAllSubmittersType= sComittee.ForAllSubmittersType;
                        comittee.Name= sComittee.Name;
                        comittee.MethodSettings = sComittee.MethodSettings;
                        comittee.WeightSettings = sComittee.WeightSettings;
                        comittee.UseDss = sComittee.UseDss;
                        Manager.SaveOrUpdate(comittee);
                        if (!sComittee.ForAllSubmittersType && sComittee.AssignedTypes.Any() && sComittee.AssignedTypes.Where(a=> a.Deleted== BaseStatusDeleted.None).Any()){
                             foreach( lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommitteeAssignedSubmitterType sAssignment in sComittee.AssignedTypes.Where(a=> a.Deleted== BaseStatusDeleted.None)){
                                if (sAssignment.SubmitterType!=null && submittersInfo.ContainsKey(sAssignment.SubmitterType.Id)){
                                    lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommitteeAssignedSubmitterType assignment = new lm.Comol.Modules.CallForPapers.Domain.Evaluation.CommitteeAssignedSubmitterType();
                                    assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    assignment.Committee = comittee;
                                    assignment.SubmitterType = submittersInfo[sAssignment.SubmitterType.Id];
                                    Manager.SaveOrUpdate(assignment);
                                    comittee.AssignedTypes.Add(assignment);
                                }
                             }
                        }
                        List<Domain.Evaluation.BaseCriterion> criteria = CloneCommitteCriteria(comittee,sComittee,person);
                        comittee.Criteria= criteria;
                        Manager.SaveOrUpdate(comittee);
                        displayOrder++;
                    }
                }

                private List<Domain.Evaluation.BaseCriterion> CloneCommitteCriteria(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee dComittee, lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee source, litePerson person) {
                    List<Domain.Evaluation.BaseCriterion> criteria = new List<Domain.Evaluation.BaseCriterion>();
                    Int32 displayOrder = 1;
                    foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.BaseCriterion sCriterion in source.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None))
                    {
                        lm.Comol.Modules.CallForPapers.Domain.Evaluation.BaseCriterion criterion = null;
                        switch(sCriterion.Type){
                            case Domain.Evaluation.CriterionType.DecimalRange:
                            case Domain.Evaluation.CriterionType.IntegerRange:
                                criterion = new Domain.Evaluation.NumericRangeCriterion();
                                ((Domain.Evaluation.NumericRangeCriterion)criterion).DecimalMinValue = ((Domain.Evaluation.NumericRangeCriterion)sCriterion).DecimalMinValue;
                                ((Domain.Evaluation.NumericRangeCriterion)criterion).DecimalMaxValue = ((Domain.Evaluation.NumericRangeCriterion)sCriterion).DecimalMaxValue;
                                break;
                            case Domain.Evaluation.CriterionType.Textual:
                                criterion = new Domain.Evaluation.TextualCriterion();
                                ((Domain.Evaluation.TextualCriterion)criterion).MaxLength = ((Domain.Evaluation.TextualCriterion)sCriterion).MaxLength;
                                break;
                            case Domain.Evaluation.CriterionType.StringRange:
                                criterion = new Domain.Evaluation.StringRangeCriterion();
                                ((Domain.Evaluation.StringRangeCriterion)criterion).MaxOption = ((Domain.Evaluation.StringRangeCriterion)sCriterion).MaxOption;
                                ((Domain.Evaluation.StringRangeCriterion)criterion).MinOption = ((Domain.Evaluation.StringRangeCriterion)sCriterion).MinOption;
                                ((Domain.Evaluation.StringRangeCriterion)criterion).Options = new List<Domain.Evaluation.CriterionOption>();
                                break;
                            case Domain.Evaluation.CriterionType.RatingScale:
                            case Domain.Evaluation.CriterionType.RatingScaleFuzzy:
                                criterion = new Domain.Evaluation.DssCriterion();
                                ((Domain.Evaluation.DssCriterion)criterion).IsFuzzy = ((Domain.Evaluation.DssCriterion)sCriterion).IsFuzzy;
                                ((Domain.Evaluation.DssCriterion)criterion).IdRatingSet = ((Domain.Evaluation.DssCriterion)sCriterion).IdRatingSet;
                                ((Domain.Evaluation.DssCriterion)criterion).Options = new List<Domain.Evaluation.CriterionOption>();
                                break;
                        }
                        if (criterion!=null){
                            criterion.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            criterion.Type= sCriterion.Type;
                            criterion.CommentMaxLength= sCriterion.CommentMaxLength;
                            criterion.CommentType= sCriterion.CommentType;
                            criterion.Committee= dComittee;
                            criterion.Description = sCriterion.Description;
                            criterion.DisplayOrder= displayOrder;
                            criterion.Name= sCriterion.Name;
                            criterion.UseDss = sCriterion.UseDss;
                            criterion.MethodSettings = sCriterion.MethodSettings;
                            criterion.WeightSettings = sCriterion.WeightSettings;
                            Manager.SaveOrUpdate(criterion);
                            switch (criterion.Type)
                            {
                                case Domain.Evaluation.CriterionType.StringRange:
                                    if (((Domain.Evaluation.StringRangeCriterion)sCriterion).Options.Any() && ((Domain.Evaluation.StringRangeCriterion)sCriterion).Options.Where(o => o.Deleted == BaseStatusDeleted.None).Any())
                                    {
                                        Int32 optDisplayOrder = 1;
                                        foreach (Domain.Evaluation.CriterionOption sOption in ((Domain.Evaluation.StringRangeCriterion)sCriterion).Options.Where(o => o.Deleted == BaseStatusDeleted.None))
                                        {
                                            Domain.Evaluation.CriterionOption option = new Domain.Evaluation.CriterionOption();
                                            option.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            option.Criterion = (Domain.Evaluation.StringRangeCriterion)criterion;
                                            option.DisplayOrder = optDisplayOrder;
                                            option.Name = sOption.Name;
                                            option.ShortName = sOption.ShortName;
                                            option.Value = sOption.Value;
                                            optDisplayOrder++;

                                            Manager.SaveOrUpdate(option);
                                            ((Domain.Evaluation.StringRangeCriterion)criterion).Options.Add(option);
                                        }
                                    }
                                    break;
                                case Domain.Evaluation.CriterionType.RatingScale:
                                case Domain.Evaluation.CriterionType.RatingScaleFuzzy:
                                    if (((Domain.Evaluation.DssCriterion)sCriterion).Options.Any() && ((Domain.Evaluation.DssCriterion)sCriterion).Options.Where(o => o.Deleted == BaseStatusDeleted.None).Any())
                                    {
                                        Int32 optDisplayOrder = 1;
                                        foreach (Domain.Evaluation.CriterionOption sOption in ((Domain.Evaluation.DssCriterion)sCriterion).Options.Where(o => o.Deleted == BaseStatusDeleted.None))
                                        {
                                            Domain.Evaluation.CriterionOption option = new Domain.Evaluation.CriterionOption();
                                            option.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            option.Criterion = (Domain.Evaluation.DssCriterion)criterion;
                                            option.DisplayOrder = optDisplayOrder++;
                                            option.Name = sOption.Name;
                                            option.ShortName = sOption.ShortName;
                                            option.IdRatingSet = sOption.IdRatingSet;
                                            option.IdRatingValue = sOption.IdRatingValue;
                                            option.IsFuzzy = sOption.IsFuzzy;
                                            option.UseDss = true;
                                            option.DoubleValue = sOption.DoubleValue;
                                            option.FuzzyValue = sOption.FuzzyValue;

                                            Manager.SaveOrUpdate(option);
                                            ((Domain.Evaluation.DssCriterion)criterion).Options.Add(option);
                                        }
                                    }
                                    break;
                            }
                            criteria.Add(criterion);
                            displayOrder++;
                        }
                    }
                    return criteria;
                }
            #endregion

            #region "Delete Call"
                public Boolean DeleteCall(long idCall, String baseFilePath, String baseThumbnailPath)
                {
                    Int32 idCommunity = 0;
                    List<String> filesToRemove = new List<String>();
                    Boolean deleted = false;
                    try
                    {
                        Manager.BeginTransaction();
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (call != null)
                        {
                            idCommunity = (call.IsPortal || call.Community == null) ? 0 : call.Community.Id;
                            #region "Assignemnts"
                            List<BaseForPaperAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperAssignment>()
                                                                        where a.BaseForPaper.Id == idCall
                                                                select a).ToList();
                            if (assignments.Any())
                                Manager.DeletePhysicalList(assignments);

                            #endregion

                            #region "Templates"
                            List<ManagerTemplateMail> mTemplates = (from a in Manager.GetIQ<ManagerTemplateMail>()
                                                                    where a.Call.Id == idCall
                                                                    select a).ToList();

                            if (mTemplates.Any())
                                Manager.DeletePhysicalList(mTemplates);

                            List<SubmitterTemplateMail> sTemplates = (from a in Manager.GetIQ<SubmitterTemplateMail>()
                                                                      where a.Call.Id == idCall
                                                                      select a).ToList();

                            if (sTemplates.Any())
                                Manager.DeletePhysicalList(sTemplates);
                            #endregion

                            #region "Profile Associations"
                            List<ProfileAttributeAssociation> sAssociations = (from af in Manager.GetIQ<ProfileAttributeAssociation>()
                                                                               where af.Call != null && af.Call.Id == idCall
                                                                               select af).ToList();
                            if (sAssociations.Any())
                                Manager.DeletePhysicalList(sAssociations);
                            #endregion
                            #region "Attachments"

                            filesToRemove = DeleteAttachments(call, baseFilePath, baseThumbnailPath);
                                
                            #endregion

                            switch (call.Type) { 
                                case CallForPaperType.CallForBids:
                                    List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.DssCallEvaluation> dssEvaluations = (from c in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.DssCallEvaluation>()
                                                                                                                                where c.IdCall == idCall select c).ToList();

                                    if (dssEvaluations.Any())
                                        Manager.DeletePhysicalList(dssEvaluations);
                                    if (call.Status == CallForPaperStatus.Draft)
                                    {
                                        List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee> comittees = (from c in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee>()
                                                                                                                                where c.Call.Id == idCall
                                                                                                                                select c).ToList();
                                        if (comittees.Any())
                                        {
                                            Manager.DeletePhysicalList(comittees);
                                            //List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionEvaluated> evaluations = (from c in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionEvaluated>()
                                            //                                                                                         where c.Call.Id == idCall
                                            //                                                                                         select c).ToList();
                                            //if (evaluations.Any())
                                            //    Manager.DeletePhysicalList(evaluations);
                                            //List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.CallEvaluator> evaluators = (from c in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.CallEvaluator>()
                                            //                                                                                   where c.Call.Id == idCall
                                            //                                                                                   select c).ToList();

                                            //if (evaluators.Any())
                                            //    Manager.DeletePhysicalList(evaluators);
                                        }
                                    }
                                    break;
                            }
                            #region "Delete submissions"
                            List<String> sFiles = DeleteSubmissions(call, baseFilePath, baseThumbnailPath);
                            if (sFiles.Any())
                                filesToRemove.AddRange(sFiles);
                            #endregion

                            #region "Profile Associations"
                            //List<FieldDefinition> fields = (from f in Manager.GetIQ<FieldDefinition>()
                            //                                where f.Call != null && f.Call.Id == idCall
                            //                                select f).ToList();
                            //if (fields.Any())
                            //{
                            //    foreach (FieldDefinition f in fields) {
                            //        List<FieldAssignment> fAssignments = (from fa in Manager.GetIQ<FieldAssignment>()
                            //                                              where fa.Field != null && fa.Field.Id == f.Id
                            //                                        select fa).ToList();
                            //        if (fAssignments.Any())
                            //            Manager.DeletePhysicalList(fAssignments);
                            //    }
                            //    //Manager.DeletePhysicalList(fields);
                            //}
                            #endregion
                            Manager.DeletePhysical(call);
                        }
                        Manager.Commit();
                        deleted = true;
                        if (filesToRemove.Any()) {
                            lm.Comol.Core.File.Delete.Files(filesToRemove);
                        }
                    }
                    catch (Exception ex)
                    {
                        deleted = false;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return deleted;
                }


                /// <summary>
                ///     Delete File attachments
                /// </summary>
                /// <param name="call">call</param>
                /// <param name="sourcePath">source file path</param>
                private List<String> DeleteAttachments(BaseForPaper call, String baseFilePath, String baseThumbnailPath)
                {
                    List<String> filesToRemove = new List<String>();
                    List<AttachmentAssignment> assignments = (from aa in Manager.GetIQ<AttachmentAssignment>() where aa.Call.Id == call.Id select aa).ToList();
                    if (assignments.Any())
                        Manager.DeletePhysicalList(assignments);

                    List<AttachmentFile> attachments = (from a in Manager.GetIQ<AttachmentFile>() where a.Call.Id == call.Id select a).ToList();

                    foreach (AttachmentFile attachment in attachments)
                    {
                        try
                        {
                            if (attachment.Item != null && attachment.Item.IsInternal)
                            {
                                filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, attachment.Item));
                                filesToRemove.Add(RepositoryService.GetItemThumbnailFullPath(baseThumbnailPath, attachment.Item));
                                Manager.DeletePhysical(attachment.Link);
                                Manager.DeletePhysical(attachment.Item);
                            }
                            else if (attachment.Item != null)
                                Manager.DeletePhysical(attachment.Link);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    return filesToRemove.Where(f => !String.IsNullOrWhiteSpace(f)).ToList(); ;
                }

                private List<String> DeleteSubmissions(BaseForPaper call, String filePath, String thumbnailPath)
                {
                    List<String> filesToRemove = new List<String>();
                    List<UserSubmission> submissions = (from s in Manager.GetIQ<UserSubmission>() where s.Call.Id == call.Id select s).ToList();
                    foreach(UserSubmission s in submissions){
                        if (s.Files.Any()){
                            foreach (SubmittedFile sFile in s.Files)
                            {
                                try
                                {
                                    if (sFile.File != null)
                                    {
                                        filesToRemove.Add(RepositoryService.GetItemDiskFullPath(filePath, sFile.File));
                                        filesToRemove.Add(RepositoryService.GetItemThumbnailFullPath(filePath, sFile.File));
                                        Manager.DeletePhysical(sFile.Link);
                                        Manager.DeletePhysical(sFile.File);
                                    }
                                    Manager.DeletePhysical(sFile);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        List<SubmissionFieldBaseValue> values = (from v in Manager.GetIQ<SubmissionFieldBaseValue>() where v.Submission.Id== s.Id select v).ToList();
                        if (values.Any())
                        {
                            foreach(SubmissionFieldBaseValue v in values.Where(fv=>fv.Field.Type== FieldType.FileInput)){
                                if (((SubmissionFieldFileValue)v).Item != null)
                                {
                                    filesToRemove.Add(RepositoryService.GetItemDiskFullPath(filePath, ((SubmissionFieldFileValue)v).Item));
                                    filesToRemove.Add(RepositoryService.GetItemThumbnailFullPath(filePath, ((SubmissionFieldFileValue)v).Item));
                                    Manager.DeletePhysical(((SubmissionFieldFileValue)v).Link);
                                    Manager.DeletePhysical(((SubmissionFieldFileValue)v).Item);
                                }
                            }
                            Manager.DeletePhysical(values);
                        }
                    }
                    if (submissions.Any())
                        Manager.DeletePhysical(submissions);
                    return filesToRemove.Where(f=> !String.IsNullOrWhiteSpace(f)).ToList();
                }
            #endregion

            #region "Availability"

                private void UpdatePublicSettings(litePerson p, BaseForPaper call, Boolean isPublic, Boolean forSubscribedUsers)
                {
                    if (call!=null && p!= null && call.IsPublic != isPublic)
                    {
                        call.IsPublic = isPublic;
                        call.ForSubscribedUsers = forSubscribedUsers;
                        call.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(call);
                    }
                }

                #region "Remove Assignments"
                private void RemoveCallAssignments(litePerson p, long idCall)
                {
                    List<BaseForPaperAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperAssignment>()
                                                                        where a.BaseForPaper.Id == idCall && a.Deleted == BaseStatusDeleted.None select a).ToList();
                    foreach(BaseForPaperAssignment assignment in assignments){
                        assignment.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    if (assignments.Any())
                        Manager.SaveOrUpdateList(assignments);

                }
                private void RemoveCallPersonTypeAssignments(litePerson p, long idCall)
                {
                    List<BaseForPaperPersonTypeAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                                                where a.BaseForPaper.Id == idCall && a.Deleted == BaseStatusDeleted.None
                                                                select a).ToList();
                    foreach (BaseForPaperPersonTypeAssignment assignment in assignments)
                    {
                        assignment.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    if (assignments.Any())
                        Manager.SaveOrUpdateList(assignments);

                }
                private void RemoveCallRoleAssignments(litePerson p, long idCall)
                {
                    List<BaseForPaperRoleAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                                          where a.BaseForPaper.Id == idCall && a.Deleted == BaseStatusDeleted.None
                                                                          select a).ToList();
                    foreach (BaseForPaperRoleAssignment assignment in assignments)
                    {
                        assignment.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    if (assignments.Any())
                        Manager.SaveOrUpdateList(assignments);

                }
                #endregion

                #region "Default Assignments"
                    public dtoCallAssignment GetDefaultAssignmentSettings(long idCall, Boolean forPortal, Boolean isPublic)
                    {
                        dtoCallAssignment settings = null;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            if (p != null && call != null)
                            {
                                UpdatePublicSettings(p, call, isPublic, call.IsPortal);
                                RemoveCallAssignments(p, idCall);
                                settings = AddDefaultCallAssignment(p, call, forPortal);
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                            settings = null;
                        }
                        return settings;
                    }
                    private dtoCallAssignment AddDefaultCallAssignment(litePerson p, BaseForPaper call, Boolean forPortal)
                    {
                        if (forPortal)
                            return new dtoCallPortalAssignment() { IsDefault = true };
                        else if (call.Community != null)
                        {
                            BaseForPaperCommunityAssignment assignment = (from a in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                                          where a.BaseForPaper.Id == call.Id && a.AssignedTo != null && a.AssignedTo.Id == call.Community.Id
                                                                          select a).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (assignment == null)
                            {
                                assignment = new BaseForPaperCommunityAssignment();
                                assignment.Deleted = BaseStatusDeleted.None;
                                assignment.Deny = false;
                                assignment.AssignedTo = call.Community;
                                assignment.BaseForPaper = call;
                                assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(assignment);
                            }
                            else if (assignment.Deleted != BaseStatusDeleted.None)
                            {
                                assignment.RecoverMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(assignment);
                            }
                            if (assignment != null)
                                return new dtoCallCommunityAssignment() { Id = assignment.Id, IdCommunity = (assignment.AssignedTo == null) ? -1 : assignment.AssignedTo.Id, IdCall = call.Id, IsDefault = true, DisplayName = (assignment.AssignedTo == null) ? "" : assignment.AssignedTo.Name };
                        }
                        return null;
                    }
                #endregion
               
                #region "Portal Assignments"
                    public dtoCallPortalAssignment GetPortalAssignments(long idCall, Dictionary<Int32, String> translatedProfileTypes)
                    {
                        dtoCallPortalAssignment assignment = null;
                        try
                        {
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            if (call != null)
                            {
                                assignment = new dtoCallPortalAssignment() { IsDefault = true, Id = 0 };
                                assignment.ProfileTypes = (from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                                           where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                                           select pa).ToList().Select(
                                                            pa => new dtoCallPersonTypeAssignment()
                                                            {
                                                                IdPersonType = pa.AssignedTo,
                                                                Deleted = BaseStatusDeleted.None,
                                                                Id = pa.Id,
                                                                Deny = pa.Deny,
                                                                DisplayName = (translatedProfileTypes.ContainsKey(pa.AssignedTo) ? translatedProfileTypes[pa.AssignedTo] : pa.AssignedTo.ToString())
                                                            }).ToList();
                                if (!assignment.ForAllUsers)
                                    assignment.ProfileTypes = assignment.ProfileTypes.OrderBy(p => p.DisplayName).ThenBy(p => p.Id).ToList();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return assignment;
                    }
                    public Boolean SavePortalAvailability(long idCall, Boolean forSubscribedUsers, List<Int32> profileTypes)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {

                                if (call.ForSubscribedUsers != forSubscribedUsers)
                                {
                                    call.ForSubscribedUsers = forSubscribedUsers;
                                    call.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    Manager.SaveOrUpdate(call);
                                }
                                if (forSubscribedUsers)
                                    RemoveCallPersonTypeAssignments(person, idCall);
                                else
                                    SavePortalProfileTypesAvailability(call, person, profileTypes);
                            }
                            Manager.Commit();
                            result = (call != null);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    private void SavePortalProfileTypesAvailability(BaseForPaper call, litePerson person, List<Int32> profileTypes)
                    {
                        List<BaseForPaperPersonTypeAssignment> pAssignments = (from a in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                                                                where a.BaseForPaper.Id == call.Id
                                                                                select a).ToList();
                        // Delete removed profile types
                        if (pAssignments.Where(p => !profileTypes.Contains(p.AssignedTo)).Any())
                        {
                            foreach (BaseForPaperAssignment assignment in pAssignments.Where(p => !profileTypes.Contains(p.AssignedTo)))
                            {
                                assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                        foreach (Int32 pType in profileTypes)
                        {
                            BaseForPaperPersonTypeAssignment pa = pAssignments.Where(p => p.AssignedTo == pType).FirstOrDefault();
                            if (pa == null)
                            {
                                pa = new BaseForPaperPersonTypeAssignment();
                                pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                pa.BaseForPaper = call;
                                pa.AssignedTo = pType;
                                Manager.SaveOrUpdate(pa);
                            }
                            else if (pa.Deleted != BaseStatusDeleted.None)
                            {
                                pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                    }
                #endregion

                public List<dtoCallAssignment> GetCallAssignments(long idCall, Boolean forPortal,Dictionary<Int32, String> translatedProfileTypes, Dictionary<Int32, String> translatedRoles)
                {
                    List<dtoCallAssignment> assignments = new List<dtoCallAssignment>();
                    try
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (call != null)
                        {
                            #region "Portal Assignment"
                            if (forPortal)
                            {
                                dtoCallPortalAssignment assignment = new dtoCallPortalAssignment() { IsDefault = true, Id=0 };
                                assignment.ProfileTypes = (from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                                           where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None select pa).ToList().Select(
                                                           pa=> new dtoCallPersonTypeAssignment()
                                                           {
                                                               IdPersonType = pa.AssignedTo,
                                                               Deleted = BaseStatusDeleted.None,
                                                               Id = pa.Id,
                                                               Deny = pa.Deny,
                                                               DisplayName = (translatedProfileTypes.ContainsKey(pa.AssignedTo) ? translatedProfileTypes[pa.AssignedTo] : pa.AssignedTo.ToString())
                                                           }).ToList();
                                assignments.Add(assignment);
                            }
                            #endregion

                            #region "Community/Role Assignments"
                            // GET role assignments
                            List<dtoCallRoleAssignment> rAssignments = (from ra in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                                        where ra.BaseForPaper.Id == idCall && !ra.Deny && ra.Deleted == BaseStatusDeleted.None && ra.Community != null && ra.AssignedTo != null
                                                                        select ra).ToList().Select(ra =>
                                                                        new dtoCallRoleAssignment()
                                                                        {
                                                                            IdCommunity = ra.Community.Id,
                                                                            IdRole = ra.AssignedTo,
                                                                            Deleted = BaseStatusDeleted.None,
                                                                            Id = ra.Id,
                                                                            Deny = ra.Deny,
                                                                            DisplayName = (translatedRoles.ContainsKey(ra.AssignedTo) ? translatedRoles[ra.AssignedTo] : ra.AssignedTo.ToString())
                                                                        }).ToList();

                            // Add communities
                            List<dtoCallCommunityAssignment> cAssignments = (from ca in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                                                where ca.BaseForPaper.Id == idCall && !ca.Deny && ca.Deleted == BaseStatusDeleted.None && ca.AssignedTo != null
                                                                                select ca).ToList().Select(ca =>
                                                                        new dtoCallCommunityAssignment()
                                                                        {
                                                                            IdCommunity = (ca.AssignedTo == null) ? -1 : ca.AssignedTo.Id,
                                                                            IsUnknown = (ca.AssignedTo == null),
                                                                            DisplayName = (ca.AssignedTo == null) ? "" : ca.AssignedTo.Name,
                                                                            Deleted = BaseStatusDeleted.None,
                                                                            Id = ca.Id,
                                                                            Deny = ca.Deny
                                                                        }).ToList();

                            if (rAssignments.Any())
                            {
                                foreach (var r in rAssignments.GroupBy(ra => ra.IdCommunity).ToList())
                                {
                                    dtoCallCommunityAssignment communityAssignment = cAssignments.Where(c => c.IdCommunity == r.Key).FirstOrDefault();
                                    if (communityAssignment != null)
                                        communityAssignment.Roles = r.OrderBy(ra=>ra.DisplayName).ThenBy(ra=>ra.IdRole).ToList();
                                    else if (Manager.GetCommunity(r.Key) != null)
                                    {
                                        assignments.Add(new dtoCallCommunityAssignment()
                                        {
                                            Id =-1,
                                            IdCommunity = r.Key,
                                            IdCall = idCall,
                                            Roles = r.OrderBy(ra=>ra.DisplayName).ThenBy(ra=>ra.IdRole).ToList(),
                                            IsDefault = (call.Community != null && r.Key == call.Community.Id),
                                            DisplayName = Manager.GetCommunity(r.Key).Name
                                        });
                                    }
                                }
                            }
                            foreach (dtoCallCommunityAssignment cAssignment in cAssignments.OrderBy(c => c.DisplayName).ThenBy(c => c.IdCommunity))
                            {
                                cAssignment.IsDefault = (call.Community != null && cAssignment.IdCommunity == call.Community.Id);
                                cAssignment.ForAllUsers = (cAssignment.Roles == null || !cAssignment.Roles.Any());
                                assignments.Add(cAssignment);
                            }
                            #endregion

                            if (!call.IsPortal && call.Community != null && !assignments.Where(a => a.Type == CallAssignmentType.Community).Select(a => (dtoCallCommunityAssignment)a).Where(a => !a.IsEmpty && a.IsDefault).Any()) { 
                                assignments.Add(new dtoCallCommunityAssignment()
                                                {
                                                    IdCommunity = call.Community.Id,
                                                    IsUnknown = false,
                                                    DisplayName =  call.Community.Name,
                                                    Deleted = BaseStatusDeleted.None,
                                                    Id = -1,
                                                    ForAllUsers=false,
                                                    IsDefault=true,
                                                    Deny = false
                                                });
                            }

                            (from pa in Manager.GetAll<BaseForPaperPersonAssignment>(pa => pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo != null)
                             select new dtoCallPersonAssignment()
                             {
                                 AssignedTo = pa.AssignedTo,
                                 Deleted = BaseStatusDeleted.None,
                                 Id = pa.Id,
                                 Deny = pa.Deny,
                                 DisplayName = ( pa.AssignedTo!=null) ? pa.AssignedTo.SurnameAndName : ""
                             }).ToList().ForEach(pa => assignments.Add(pa));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return assignments;
                }

                

                #region "Community Assignments"
                    public dtoCallCommunityAssignment GetCommunityAssignments(long idCall, Int32 idCommunity, long idAssignment, Dictionary<Int32, String> translatedRoles)
                    {
                        dtoCallCommunityAssignment assignment = null;
                        try
                        {
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            if (call != null)
                            {
                                assignment = new dtoCallCommunityAssignment();
                                BaseForPaperCommunityAssignment cAssignment = Manager.Get<BaseForPaperCommunityAssignment>(idAssignment);


                                if (cAssignment != null)
                                {
                                    assignment.ForAllUsers = true;
                                    assignment.IsDefault = (call.Community != null && cAssignment.AssignedTo != null && call.Community.Id == cAssignment.AssignedTo.Id);
                                }
                                else
                                {
                                    assignment.IsDefault = (call.Community != null && call.Community.Id == idCommunity);
                                    assignment.Roles = (from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                        where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo != null
                                                        && pa.Community != null && pa.Community.Id == idCommunity 
                                                        select pa).ToList().Select(
                                                          pa => new dtoCallRoleAssignment()
                                                          {
                                                              IdRole = pa.AssignedTo,
                                                              Deleted = BaseStatusDeleted.None,
                                                              Id = pa.Id,
                                                              Deny = pa.Deny,
                                                              DisplayName = (translatedRoles.ContainsKey(pa.AssignedTo) ? translatedRoles[pa.AssignedTo] : pa.AssignedTo.ToString())
                                                          }).ToList();
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return assignment;
                    }
                    public Boolean SaveCommunityAvailability(long idCall, Int32 idCommunity, Boolean forSubscribedUsers, List<Int32> roles)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                            if (call != null && person != null && community != null)
                            {
                                List<BaseForPaperCommunityAssignment> cAssignments = (from a in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                                                      where a.BaseForPaper.Id == call.Id && a.AssignedTo != null && a.AssignedTo.Id == idCommunity
                                                                                      select a).ToList();

                                if (forSubscribedUsers)
                                {
                                    if (cAssignments.Where(a => a.Deleted != BaseStatusDeleted.None).Any())
                                        cAssignments.Where(a => a.Deleted != BaseStatusDeleted.None).FirstOrDefault().RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    else if (!cAssignments.Where(a => a.Deleted == BaseStatusDeleted.None).Any())
                                    {
                                        BaseForPaperCommunityAssignment assignment = new BaseForPaperCommunityAssignment();
                                        assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        assignment.BaseForPaper = call;
                                        assignment.AssignedTo = community;
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                    RemoveCallRoleAssignments(person, idCall);
                                }
                                else
                                {
                                    foreach (BaseForPaperCommunityAssignment assignment in cAssignments)
                                    {
                                        assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    SaveCommunityRolesAvailability(call, person, community, roles);
                                }
                            }
                            Manager.Commit();
                            result = (call != null);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    private void SaveCommunityRolesAvailability(BaseForPaper call, litePerson person, liteCommunity community, List<Int32> roles)
                    {
                        List<BaseForPaperRoleAssignment> rAssignments = (from a in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                                         where a.BaseForPaper.Id == call.Id && a.AssignedTo != null && a.Community !=null && a.Community.Id == community.Id 
                                                                         select a).ToList();
                        // Delete removed profile types
                        if (rAssignments.Where(p => !roles.Contains(p.AssignedTo)).Any())
                        {
                            foreach (BaseForPaperRoleAssignment assignment in rAssignments.Where(p => !roles.Contains(p.AssignedTo)))
                            {
                                assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                        foreach (Int32 role in roles)
                        {
                            BaseForPaperRoleAssignment pa = rAssignments.Where(p => p.AssignedTo == role).FirstOrDefault();
                            if (pa == null)
                            {
                                pa = new BaseForPaperRoleAssignment();
                                pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                pa.BaseForPaper = call;
                                pa.AssignedTo = role;
                                pa.Community = community;
                                Manager.SaveOrUpdate(pa);
                            }
                            else if (pa.Deleted != BaseStatusDeleted.None)
                            {
                                pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                    }
                    public List<Int32> GetIdCommunityAssignments(long idCall)
                    {
                        List<Int32> items = new List<Int32>();
                        try
                        {
                            items = (from a in Manager.GetIQ<BaseForPaperCommunityAssignment>() where a.Deleted == BaseStatusDeleted.None && a.BaseForPaper.Id == idCall && a.AssignedTo != null select a.AssignedTo.Id).ToList();
                            items.AddRange((from ra in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                            where ra.BaseForPaper.Id == idCall && !ra.Deny && ra.Deleted == BaseStatusDeleted.None && ra.Community != null && ra.AssignedTo != null
                                            select ra.Community.Id).ToList());
                            items = items.Distinct().ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        return items;
                    }
                    public Boolean AddCommunityAssignment(long idCall, List<Int32> items)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {
                                List<BaseForPaperCommunityAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                                                     where a.BaseForPaper.Id == call.Id && a.Deleted != BaseStatusDeleted.None
                                                                                     select a).ToList();
                                foreach (BaseForPaperCommunityAssignment assignment in assignments.Where(a => a.AssignedTo != null && items.Contains(a.AssignedTo.Id)))
                                {
                                    assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    items.Remove(assignment.AssignedTo.Id);
                                }
                                foreach (Int32 idCommunity in items)
                                {
                                    List<BaseForPaperRoleAssignment> rAssignments = (from a in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                                                     where a.BaseForPaper.Id == call.Id && a.Deleted == BaseStatusDeleted.None && a.Community != null
                                                                                     && a.Community.Id == idCommunity
                                                                                     select a).ToList();
                                    foreach (BaseForPaperRoleAssignment rAssignment in rAssignments)
                                    {
                                        rAssignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    BaseForPaperCommunityAssignment assignment = new BaseForPaperCommunityAssignment();
                                    assignment.AssignedTo = Manager.GetLiteCommunity(idCommunity);
                                    if (assignment.AssignedTo != null)
                                    {
                                        assignment.BaseForPaper = call;
                                        assignment.Deny = false;
                                        assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                }
                            }
                            Manager.Commit();
                            result = (call != null);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    public Boolean DeleteCommunityAssignments(long idCall, Int32 idCommunity, long idAssignment)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {
                                if (idAssignment > 0)
                                {
                                    BaseForPaperCommunityAssignment assignment = Manager.Get<BaseForPaperCommunityAssignment>(idAssignment);
                                    if (assignment != null)
                                        assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                                else
                                {
                                    List<BaseForPaperCommunityAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                                                         where a.BaseForPaper.Id == call.Id && a.Deleted != BaseStatusDeleted.None && a.AssignedTo != null && a.AssignedTo.Id == idCommunity
                                                                                         select a).ToList();
                                    foreach (BaseForPaperCommunityAssignment assignment in assignments)
                                    {
                                        assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                }

                                List<BaseForPaperRoleAssignment> rAssignments = (from a in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                                                 where a.BaseForPaper.Id == call.Id && a.Deleted == BaseStatusDeleted.None && a.Community != null
                                                                                 && a.Community.Id == idCommunity
                                                                                 select a).ToList();
                                foreach (BaseForPaperRoleAssignment rAssignment in rAssignments)
                                {
                                    rAssignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                            }
                            Manager.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                #endregion

                #region "User Assignments From Call"
                    public List<Int32> GetIdUserAssignments(long idCall)
                    {
                        List<Int32> items = new List<Int32>();
                        try
                        {
                            items = (from a in Manager.GetIQ<BaseForPaperPersonAssignment>() 
                                     where a.Deleted == BaseStatusDeleted.None && a.BaseForPaper.Id == idCall && a.AssignedTo != null select a.AssignedTo.Id).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        return items.Distinct().ToList();
                    }
                    public List<dtoCallInfo> GetCallsForPersonAssignments(CallForPaperType type, Boolean fromPortal, List<Int32> fromCommunities,List<Int32> removeUsers, String unknownCommunity, String portalTranslation)
                    {
                        List<dtoCallInfo> list = new List<dtoCallInfo>();
                        try
                        {
                            List<BaseForPaper> calls = (from c in Manager.GetIQ<BaseForPaper>()
                                                        where c.Type == type && c.Deleted == BaseStatusDeleted.None && c.Status != CallForPaperStatus.Draft &&
                                                        (
                                                            (fromPortal && c.IsPortal) || (fromCommunities.Any() && !c.IsPortal && c.Community != null)
                                                        )
                                                        select c).ToList();
                            if (fromCommunities.Any())
                                calls = calls.Where(c => (fromPortal && c.IsPortal) || (!c.IsPortal && fromCommunities.Contains(c.Community.Id))).ToList();

                            list = calls.Select(c =>
                                    new dtoCallInfo()
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        Edition = c.Edition,
                                        StartDate = c.StartDate,
                                        CreatedOn = c.CreatedOn,
                                        EndDate = c.EndDate,
                                        Type = c.Type,
                                        IsPublic = c.IsPublic,
                                        IsPortal = c.IsPortal,
                                        IdCommunity = (c.IsPortal) ? 0 : ((c.Community == null) ? -1 : c.Community.Id),
                                        CommunityName = (c.IsPortal) ? portalTranslation : ((c.Community == null) ? unknownCommunity : c.Community.Name),
                                        Submissions = GetSubmissionsCountForPersonAssignments(c.Id, removeUsers)
                                    }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        return list.Where(c => c.Submissions > 0).OrderBy(c => c.IsPortal).ThenBy(c => c.Name).ThenBy(c => c.CommunityName).ToList();
                    }
                    private long GetSubmissionsCountForPersonAssignments(long idCall,List<Int32> removeUsers)
                    {
                        long result = 0;
                        try
                        {
                            if (removeUsers.Any())
                            {
                                if (removeUsers.Count > maxItemsForQuery)
                                {
                                    List<Int32> idPersons = (from s in Manager.GetIQ<UserSubmission>()
                                                 where s.Call.Id == idCall && !s.isAnonymous && s.Type != null && s.Deleted == BaseStatusDeleted.None && s.Owner != null
                                                 select s.Owner.Id).ToList();
                                    result = idPersons.Where(p => !removeUsers.Contains(p)).Count();
                                }
                                else
                                    result = (from s in Manager.GetIQ<UserSubmission>()
                                     where s.Call.Id == idCall && !s.isAnonymous && s.Type != null && s.Deleted == BaseStatusDeleted.None && s.Owner != null
                                     && !removeUsers.Contains(s.Owner.Id)
                                     select s.Owner.Id).Count();
                            }
                            else
                            {
                                result = (from s in Manager.GetIQ<UserSubmission>()
                                          where s.Call.Id == idCall && !s.isAnonymous && s.Type != null && s.Deleted == BaseStatusDeleted.None && s.Owner != null
                                          select s.Id).Count();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return result;
                    }
                    public Dictionary<long, String> GetCallSubmittersTypeForPersonAssignments(long idCall)
                    {
                        Dictionary<long, string> result = new Dictionary<long, string>();
                        try
                        {
                            List<SubmitterType> types = (from t in Manager.GetIQ<SubmitterType>()
                                                         where t.Deleted == BaseStatusDeleted.None && t.Call.Id == idCall
                                                         select t).ToList();

                            result = types.Where(t => (from s in Manager.GetIQ<UserSubmission>()
                                                       where !s.isAnonymous && s.Deleted == BaseStatusDeleted.None && s.Call.Id == idCall && s.Type.Id == t.Id
                                                       select s).Any()).OrderBy(t => t.Name).ToDictionary(t => t.Id, t => t.Name);
                        }
                        catch (Exception ex)
                        {

                        }
                        return result;
                    }
                    public List<SubmissionFilterStatus> GetAvailableSubmissionStatusForPersonAssignments(long idCall, List<Int32> removeUsers)
                    {
                        List<SubmissionFilterStatus> items = new List<SubmissionFilterStatus>();
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (call != null)
                        {
                            var query = (from s in Manager.GetIQ<UserSubmission>() where !s.isAnonymous && s.Owner != null && s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id select s);
                            if (query.Where(s => s.Status == SubmissionStatus.draft).Select(s => s.Owner.Id).ToList().Where(u => !removeUsers.Any() || !removeUsers.Contains(u)).Any())
                                items.Add(SubmissionFilterStatus.WaitingSubmission);
                            if (query.Where(s => s.Status >= SubmissionStatus.submitted).Select(s => s.Owner.Id).ToList().Where(u => !removeUsers.Any() || !removeUsers.Contains(u)).Any())
                                items.Add(SubmissionFilterStatus.OnlySubmitted);
                            if (query.Where(s => s.Status == SubmissionStatus.rejected).Select(s => s.Owner.Id).ToList().Where(u => !removeUsers.Any() || !removeUsers.Contains(u)).Any())
                                items.Add(SubmissionFilterStatus.Rejected);
                            if (query.Where(s => s.Status == SubmissionStatus.accepted || s.Status == SubmissionStatus.valuated || s.Status == SubmissionStatus.valuating || s.Status == SubmissionStatus.waitingValuation).Select(s => s.Owner.Id).ToList().Where(u => !removeUsers.Any() || !removeUsers.Contains(u)).Any())
                                items.Add(SubmissionFilterStatus.Accepted);
                        }
                        return items.Distinct().ToList();
                    }
                    public List<dtoSubmissionDisplayItem> GetSubmissionsForPersonAssignments(long idCall, List<Int32> removeUsers, dtoFilterSubmissions filter, Int32 pageIndex, Int32 pageSize)
                    {
                        List<dtoSubmissionDisplayItem> items = null;
                        try
                        {
                            List<dtoSubmissionDisplayItem> submissions = null;
                            var query = GetSubmissionsQuery(idCall, removeUsers, filter);

                            SubmissionsOrder orderBy = filter.OrderBy;
                            Boolean ascending = filter.Ascending;
                            if (orderBy == SubmissionsOrder.None)
                            {
                                ascending = false;
                                switch (filter.Status)
                                {
                                    case SubmissionFilterStatus.OnlySubmitted:
                                        orderBy = SubmissionsOrder.BySubmittedOn;
                                        ascending = true;
                                        break;
                                    case SubmissionFilterStatus.VirtualDeletedSubmission:
                                        orderBy = SubmissionsOrder.ByDate;
                                        break;
                                    case SubmissionFilterStatus.WaitingSubmission:
                                        orderBy = SubmissionsOrder.ByDate;
                                        break;
                                    default:
                                        orderBy = SubmissionsOrder.ByDate;
                                        break;
                                }
                            }

                            switch (orderBy)
                            {
                                case SubmissionsOrder.BySubmittedOn:
                                    if (ascending)
                                        query = query.OrderBy(s => s.SubmittedOn);
                                    else
                                        query = query.OrderByDescending(s => s.SubmittedOn);
                                    break;
                                case SubmissionsOrder.ByDate:
                                    if (ascending)
                                        query = query.OrderBy(r => r.ModifiedOn);
                                    else
                                        query = query.OrderByDescending(r => r.ModifiedOn);
                                    break;
                                case SubmissionsOrder.ByStatus:
                                    if (ascending)
                                        query = query.OrderBy(r => filter.TranslationsSubmission[r.Status]);
                                    else
                                        query = query.OrderByDescending(r => filter.TranslationsSubmission[r.Status]);
                                    break;
                                case SubmissionsOrder.ByType:
                                    if (ascending)
                                        query = query.OrderBy(s => s.Type.Name);
                                    else
                                        query = query.OrderByDescending(s => s.Type.Name);
                                    break;
                                case SubmissionsOrder.ByUser:
                                    if (ascending)
                                        query = query.OrderBy(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                                    else
                                        query = query.OrderByDescending(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                                    break;
                            }

                            if (pageSize > 0)
                                query = query.ToList().Where(s => !removeUsers.Contains(s.Owner.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                            else if (removeUsers.Any())
                                query = query.ToList().Where(s => !removeUsers.Contains(s.Owner.Id)).ToList();
                            else
                                query = query.ToList();
                            submissions = (from r in query
                                     select new dtoSubmissionDisplayItem()
                                     {
                                         SubmitterType = r.Type.Name ,
                                         Status = r.Status,
                                         Deleted = r.Deleted,
                                         Id = r.Id,
                                         SubmittedOn = r.SubmittedOn,
                                         ModifiedOn = r.ModifiedOn,
                                         PersonId = (r.Owner != null) ? r.Owner.Id : 0,
                                         SubmitterName = (r.Owner != null) ? r.Owner.SurnameAndName : "",
                                         CreatedOn = r.CreatedOn 
                                     }).ToList();
                            items = new List<dtoSubmissionDisplayItem>();


                            foreach (dtoSubmissionDisplayItem s in submissions)
                            {
                                var eQuery = (from e in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                                             where e.Call.Id == idCall && e.Deleted == BaseStatusDeleted.None && e.Status != Domain.Evaluation.EvaluationStatus.Invalidated && e.Status != Domain.Evaluation.EvaluationStatus.EvaluatorReplacement
                                             && e.Submission.Id == s.Id
                                             select e);
                                s.HasEvaluations = eQuery.Any();
                                eQuery= eQuery.Where(e=> e.SumRating > 0);
                                List<Double> evaluations = eQuery.Select(e => e.SumRating).ToList();
                                s.SumRating = evaluations.Sum();
                                items.Add(s);
                            }
                        }
                        catch (Exception ex)
                        {
                            items = new List<dtoSubmissionDisplayItem>();
                        }
                        return items;
                    }
                    public Int32 GetSubmissionsCountForPersonAssignments(long idCall, List<Int32> removeUsers, dtoFilterSubmissions filter)
                    {
                        Int32 result = 0;
                        try
                        {
                            var query = GetSubmissionsQuery(idCall, removeUsers, filter);
                            if (removeUsers.Any())
                            {
                                List<Int32> idUsers = query.Select(s => s.Owner.Id).ToList();
                                result = idUsers.Where(i => !removeUsers.Contains(i)).Count();
                            }
                            else
                                result = query.Count();
                        }
                        catch (Exception ex)
                        {

                        }
                        return result;
                    }
                    private IEnumerable<UserSubmission> GetSubmissionsQuery(long idCall, List<Int32> removeUsers, dtoFilterSubmissions filter)
                    {
                        var query = GetBaseSubmissionsQuery(idCall, removeUsers, filter);
                        String searchName = "";
                        if (!String.IsNullOrEmpty(filter.SearchForName))
                        {
                            searchName = filter.SearchForName.ToLower();
                            List<long> idSubmissions = GetSubmissionsByUser(GetBaseSubmissionsQuery(idCall, removeUsers, filter), searchName);
                            query = query.Where(r => idSubmissions.Contains(r.Id));
                        }
                        return query;
                    }
                    private IEnumerable<UserSubmission> GetBaseSubmissionsQuery(long idCall, List<Int32> removeUsers, dtoFilterSubmissions filter)
                    {
                        var query = (from s in Manager.GetIQ<UserSubmission>() where s.Call.Id == idCall && !s.isAnonymous && s.Owner != null select s);
                        if (filter.IdSubmitterType>0)
                            query = query.Where(s => s.Type.Id == filter.IdSubmitterType);
                        switch (filter.Status)
                        {
                            //case SubmissionFilterStatus.VirtualDeletedSubmission:
                            //    query = query.Where(s => s.Deleted != BaseStatusDeleted.None);
                            //    break;
                            case SubmissionFilterStatus.OnlySubmitted:
                                query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status == SubmissionStatus.submitted);
                                break;
                            case SubmissionFilterStatus.WaitingSubmission:
                                query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status < SubmissionStatus.submitted);
                                break;
                            case SubmissionFilterStatus.Rejected:
                                query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status == SubmissionStatus.rejected);
                                break;
                            case SubmissionFilterStatus.Accepted:
                                query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status == SubmissionStatus.accepted || s.Status == SubmissionStatus.valuated || s.Status == SubmissionStatus.valuating || s.Status == SubmissionStatus.waitingValuation);
                                break;
                            case SubmissionFilterStatus.All:
                                query = query.Where(s => s.Deleted == BaseStatusDeleted.None);
                                break;
                        }
                        return query;
                    }

                    public List<Int32> GetIdUsersFromSubmissions(long idCall, List<Int32> removeUsers, Boolean allSubmissions, dtoFilterSubmissions filter, List<long> idSubmissions)
                    {
                        List<Int32> items = null;
                        try
                        {
                            if (allSubmissions) {
                                var query = GetSubmissionsQuery(idCall, removeUsers, filter);
                                idSubmissions = query.Select(s => s.Id).ToList();
                            }
                            items = GetIdUsersFromSubmissions(idSubmissions);
                            if (items.Any())
                                items = items.Where(i => !removeUsers.Contains(i)).ToList();
                        }
                        catch (Exception ex)
                        {
                            items = new List<Int32>();
                        }
                        return items;
                    }
                    private List<Int32> GetIdUsersFromSubmissions(List<long> items)
                    {
                        List<Int32> idUsers = new List<Int32>();
                        Int32 pageIndex = 0;

                        List<long> idSubmissions = items.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        while (idSubmissions.Any())
                        {
                            idUsers.AddRange(
                                (from s in Manager.GetIQ<UserSubmission>() where idSubmissions.Contains(s.Id) && s.Owner != null select s.Owner.Id).ToList());
                            pageIndex++;
                            idSubmissions = items.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }

                        return idUsers.Distinct().ToList();
                    }

                    public List<BaseForPaperPersonAssignment> AddPersonAssignments(long idCall, List<Int32> users)
                    {
                        List<BaseForPaperPersonAssignment> assignments = (users.Any() ? new List<BaseForPaperPersonAssignment>() : null);
                        try
                        {
                            Manager.BeginTransaction();
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {
                                List<BaseForPaperPersonAssignment> pAssignments = (from a in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                                                                   where a.BaseForPaper == call
                                                                                   select a).ToList();
                                foreach (Int32 idUser in users)
                                {
                                    BaseForPaperPersonAssignment pa = pAssignments.Where(p => p.AssignedTo.Id == idUser).FirstOrDefault();
                                    if (pa == null)
                                    {
                                        pa = new BaseForPaperPersonAssignment();
                                        pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        pa.BaseForPaper = call;
                                        pa.AssignedTo = Manager.Get<litePerson>(idUser);
                                        if (pa.AssignedTo != null)
                                        {
                                            Manager.SaveOrUpdate(pa);
                                            assignments.Add(pa);
                                        }
                                    }
                                    else if (pa.Deleted != BaseStatusDeleted.None)
                                    {
                                        pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(pa);
                                        assignments.Add(pa);
                                    }
                                }
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex) {
                            assignments = new List<BaseForPaperPersonAssignment>();
                        }
                        return assignments;
                    }

                    public Boolean HasCallUserAssignments(long idCall)
                    {
                        Boolean result = false;
                        try
                        {
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            if (call != null)
                            {
                                result = (from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                          where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
                                          select pa.Id).Any();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return result;
                    }

                    private void SavePersonAssignments(BaseForPaper call, litePerson person, List<Int32> users)
                    {
                        List<BaseForPaperPersonAssignment> pAssignments = (from a in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                                                           where a.BaseForPaper == call
                                                                           select a).ToList();
                        // Delete removed profile types
                        if (pAssignments.Where(p => !users.Contains(p.AssignedTo.Id)).Any())
                        {
                            pAssignments.Where(p => !users.Contains(p.AssignedTo.Id)).ToList().ForEach(a => a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                            Manager.SaveOrUpdateList(pAssignments.Where(p => !users.Contains(p.AssignedTo.Id)).ToList());
                        }
                        foreach (Int32 idUser in users)
                        {
                            BaseForPaperPersonAssignment pa = pAssignments.Where(p => p.AssignedTo.Id == idUser).FirstOrDefault();
                            if (pa == null)
                            {
                                pa = new BaseForPaperPersonAssignment();
                                pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                pa.BaseForPaper = call;
                                pa.AssignedTo = Manager.GetLitePerson(idUser);
                                if (pa.AssignedTo != null)
                                    Manager.SaveOrUpdate(pa);
                            }
                            else if (pa.Deleted != BaseStatusDeleted.None)
                            {
                                pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(pa);
                            }
                        }
                    }
                #endregion

                public Boolean SaveCallAvailability(long idCall, Boolean isPublic, List<Int32> users)
                {
                    Boolean saved = false;
                    try
                    {
                        Manager.BeginTransaction();
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {

                            if (call.IsPublic != isPublic)
                            {
                                call.IsPublic = isPublic;
                                call.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(call);
                            }
                            SavePersonAssignments(call, person, users);
                        }
                        Manager.Commit();
                        saved = (call != null);
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return saved;
                }

                #region "Agency / Profile"
                    public Boolean HasAssignmentWithProfileType(long idCall, int idProfileType)
                    {
                        Boolean found = false;
                        try
                        {
                            found = (from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                     where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo == idProfileType
                                     select pa).Any();

                            if (!found)
                            {
                                found = (from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                         where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo != null
                                         select pa.AssignedTo).ToList().Where(p => p.TypeID == idProfileType).Any();

                                if (!found)
                                {
                                    List<Int32> persons = new List<Int32>();
                                    var qRole = (from ra in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                                 where ra.BaseForPaper.Id == idCall && !ra.Deny && ra.Deleted == BaseStatusDeleted.None && ra.Community != null && ra.AssignedTo != null
                                                 select ra);
                                    foreach (BaseForPaperRoleAssignment a in qRole)
                                    {
                                        found = (from s in Manager.GetIQ<Subscription>()
                                                 where s.Community != null && s.Community.Id == a.Community.Id && s.Role.Id == a.AssignedTo && s.Accepted && s.Enabled
                                                 select s.Person).ToList().Where(p => p.TypeID == idProfileType).Select(p => p.Id).Any();
                                        if (found)
                                            break;

                                    }
                                    if (!found)
                                    {
                                        var qCommunity = (from ca in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                          where ca.BaseForPaper.Id == idCall && !ca.Deny && ca.Deleted == BaseStatusDeleted.None && ca.AssignedTo != null
                                                          select ca);
                                        foreach (BaseForPaperCommunityAssignment a in qCommunity)
                                        {
                                            found = (from s in Manager.GetIQ<Subscription>()
                                                     where s.Community != null && s.Community.Id == a.AssignedTo.Id && s.Accepted && s.Enabled
                                                     select s.Person).ToList()
                                                     .Where(p => p.TypeID == idProfileType).Select(p => p.Id).Any();
                                            if (found)
                                                break;

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return found;
                    }
                    public Dictionary<long, String> GetAgenciesForAssignments(long idCall)
                    {
                        Dictionary<long, String> list = new Dictionary<long, String>();
                        try
                        {
                            List<Agency> agencies = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a).ToList();
                            List<Int32> idUsers = new List<Int32>();
                            if ((from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                 where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo == (int)UserTypeStandard.Employee
                                 select pa).Any())
                            {
                                idUsers.AddRange((from p in Manager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Employee select p.Id).ToList());
                            }
                            else
                            {
                                idUsers.AddRange((from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                                  where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo != null
                                                  select pa.AssignedTo).ToList().Select(p => p.Id).ToList().Distinct().ToList());

                                var qRole = (from ra in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                             where ra.BaseForPaper.Id == idCall && !ra.Deny && ra.Deleted == BaseStatusDeleted.None && ra.Community != null && ra.AssignedTo != null
                                             select ra);
                                foreach (BaseForPaperRoleAssignment a in qRole)
                                {
                                    idUsers.AddRange((from s in Manager.GetIQ<Subscription>()
                                                      where s.Community != null && s.Community.Id == a.Community.Id && s.Role.Id == a.AssignedTo && s.Accepted && s.Enabled
                                                      select s.Person).ToList().Where(p => p.TypeID == (int)UserTypeStandard.Employee && !idUsers.Contains(p.Id)).Select(p => p.Id).ToList());
                                }
                                var qCommunity = (from ca in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                  where ca.BaseForPaper.Id == idCall && !ca.Deny && ca.Deleted == BaseStatusDeleted.None && ca.AssignedTo != null
                                                  select ca);
                                foreach (BaseForPaperCommunityAssignment a in qCommunity)
                                {
                                    idUsers.AddRange((from s in Manager.GetIQ<Subscription>()
                                                      where s.Community != null && s.Community.Id == a.AssignedTo.Id && s.Accepted && s.Enabled
                                                      select s.Person).ToList()
                                                 .Where(p => p.TypeID == (int)UserTypeStandard.Employee && !idUsers.Contains(p.Id)).Select(p => p.Id).ToList());
                                }
                            }
                            List<long> idUserAgencies = new List<long>();
                            if (idUsers.Count() <= maxItemsForQuery)
                                idUserAgencies = (from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && idUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList();
                            else
                            {
                                Int32 index = 0;
                                List<Int32> tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                while (tUsers.Any())
                                {
                                    idUserAgencies.AddRange((from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && tUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList());
                                    index++;
                                    tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                }
                                idUserAgencies = idUserAgencies.Distinct().ToList();
                            }
                            list = agencies.Where(a => idUserAgencies.Contains(a.Id)).OrderBy(a => a.Name).ToDictionary(a => a.Id, a => a.Name);
                        }
                        catch (Exception ex)
                        {

                        }
                        return list;
                    }

                    public List<litePerson> GetUsersForAssignments(long idCall, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService service)
                    {
                        List<litePerson> users = new List<litePerson>();
                        try
                        {
                            (from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                             where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo == (int)UserTypeStandard.Employee
                             select pa).ToList().Select(p => p.AssignedTo).ToList().ForEach(t => users.AddRange(service.GetLitePersonsByType(t, true)));

                            users.AddRange((from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                                where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo != null
                                                select pa.AssignedTo).ToList().Where(p=> !users.Where(u=>u.Id== p.Id).Any()).ToList());

                            var qRole = (from ra in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                            where ra.BaseForPaper.Id == idCall && !ra.Deny && ra.Deleted == BaseStatusDeleted.None && ra.Community != null && ra.AssignedTo != null
                                            select ra);
                            foreach (BaseForPaperRoleAssignment a in qRole)
                            {
                                users.AddRange((from s in Manager.GetIQ<liteSubscription>()
                                                where s.IdCommunity == a.Community.Id && s.IdRole == a.AssignedTo && s.Accepted && s.Enabled
                                                select s).ToList().Select(s => s.Person).Where(p => !users.Where(u => u.Id == p.Id).Any()).ToList());
                            }
                            var qCommunity = (from ca in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                                where ca.BaseForPaper.Id == idCall && !ca.Deny && ca.Deleted == BaseStatusDeleted.None && ca.AssignedTo != null
                                                select ca);
                            foreach (BaseForPaperCommunityAssignment a in qCommunity)
                            {
                                users.AddRange((from s in Manager.GetIQ<liteSubscription>()
                                                where s.IdCommunity == a.AssignedTo.Id && s.Accepted && s.Enabled
                                                    select s).ToList().Select(s=>s.Person).Where(p => !users.Where(u => u.Id == p.Id).Any()).Select(p=>p).ToList());
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return users;
                    }
                #endregion

            #endregion

            #region "Status Available"
                public List<CallForPaperStatus> GetAvailableStatus(long idCall)
                    {
                        List<CallForPaperStatus> items = new List<CallForPaperStatus>();
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (idCall == 0)
                            items.Add(CallForPaperStatus.Draft);
                        else
                        {
                            if (call.Status == CallForPaperStatus.Draft || !CallHasSubmissions(call))
                                items.Add(CallForPaperStatus.Draft);
                            items.Add(CallForPaperStatus.Published);
                            items.Add(CallForPaperStatus.SubmissionOpened);
                            items.Add(CallForPaperStatus.SubmissionClosed);
                            items.Add(CallForPaperStatus.SubmissionsLimitReached);
                        }
                        return items;
                    }
            public CallForPaperStatus GetValidCallStatus(dtoBaseForPaper call)
            {
                CallForPaperStatus status = CallForPaperStatus.Draft;
                if (call != null)
                {
                    DateTime today = DateTime.Today;
                    switch (call.Status)
                    {
                        case CallForPaperStatus.Draft:
                        case CallForPaperStatus.SubmissionClosed:
                        case CallForPaperStatus.SubmissionsLimitReached:
                            status = call.Status;
                            break;
                        case CallForPaperStatus.Published:
                            if ((today >= call.StartDate && (!call.EndDate.HasValue || today <= call.EndDate) && call.Status != CallForPaperStatus.Draft && call.Status != CallForPaperStatus.SubmissionOpened))
                                status = CallForPaperStatus.SubmissionOpened;
                            break;
                        case CallForPaperStatus.SubmissionOpened:
                            if ((call.EndDate.HasValue && today > call.EndDate)
                                && call.Status != CallForPaperStatus.Draft && call.Status != CallForPaperStatus.SubmissionClosed)
                            {
                                status = CallForPaperStatus.SubmissionClosed;
                            }
                            else
                                status = call.Status;
                            break;
                        default:
                            status = call.Status;
                            break;
                    }
                }
                return status;
            }
            #endregion

            #region "Submitters"
                public SubmitterType AddSubmitterType(long idCall)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return AddSubmitterType(call);
                }
                public SubmitterType AddSubmitterType(BaseForPaper call)
                {
                    if (call == null)
                        return null;
                    else {
                        dtoSubmitterType submitter = new dtoSubmitterType() { AllowMultipleSubmissions = false, Name = "--", MaxMultipleSubmissions = 1};
                        return SaveSubmitterType(call, submitter);
                    }
                }
                public SubmitterType SaveSubmitterType(BaseForPaper call, dtoSubmitterType dto)
                {
                    SubmitterType submitter = null;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person!=null)
                        {
                            Manager.BeginTransaction();

                            submitter = Manager.Get<SubmitterType>(dto.Id);
                            if (submitter == null)
                            {
                                submitter = new SubmitterType();
                                submitter.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                submitter.Call = call;
                                Int32 count = (from c in Manager.GetIQ<SubmitterType>() where c.Deleted == BaseStatusDeleted.None && c.Call == call select c.Id).Count();
                                if (count >= dto.DisplayOrder || (from c in Manager.GetIQ<SubmitterType>() where c.Deleted == BaseStatusDeleted.None && c.Call == call && c.DisplayOrder == dto.DisplayOrder select c.Id).Any())
                                {
                                    Int32 displayOrder = 1;
                                    dto.DisplayOrder = count + 1;
                                    List<SubmitterType> items = (from c in Manager.GetIQ<SubmitterType>() where c.Deleted == BaseStatusDeleted.None && c.Call == call orderby c.DisplayOrder, c.Name select c).ToList();
                                    items.ForEach(i => i.DisplayOrder = displayOrder++);
                                    Manager.SaveOrUpdateList(items);
                                }
                            }
                            else
                            {
                                submitter.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                if (submitter.DisplayOrder != dto.DisplayOrder)
                                    UpdateDisplayOrder(dto.DisplayOrder, submitter);
                            }

                            submitter.Name = dto.Name;
                            submitter.Description = dto.Description;
                            submitter.Deleted = BaseStatusDeleted.None;
                            submitter.AllowMultipleSubmissions = dto.AllowMultipleSubmissions;
                            if (!dto.AllowMultipleSubmissions || dto.MaxMultipleSubmissions<1)
                                submitter.MaxMultipleSubmissions = 1;
                            else
                                submitter.MaxMultipleSubmissions = dto.MaxMultipleSubmissions;

                            submitter.DisplayOrder = dto.DisplayOrder;
                            Manager.SaveOrUpdate(submitter);
                            Manager.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return submitter;
                }
                public Boolean SaveSubmittersType(long idCall, List<dtoSubmitterType> items)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return SaveSubmittersType(call, items);
                }
                public Boolean SaveSubmittersType(BaseForPaper call, List<dtoSubmitterType> items)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            List<dtoSubmitterType> toAdd = new List<dtoSubmitterType>();
                            Int32 displayOrder = 1;
                            foreach (dtoSubmitterType dto in items) {
                                SubmitterType submitter = Manager.Get<SubmitterType>(dto.Id);
                                if (submitter != null)
                                {
                                    submitter.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    submitter.Name = dto.Name;
                                    submitter.Description = dto.Description;
                                    submitter.Deleted = BaseStatusDeleted.None;
                                    submitter.AllowMultipleSubmissions = dto.AllowMultipleSubmissions;
                                    if (!dto.AllowMultipleSubmissions || dto.MaxMultipleSubmissions < 1)
                                        submitter.MaxMultipleSubmissions = 1;
                                    else
                                        submitter.MaxMultipleSubmissions = dto.MaxMultipleSubmissions;

                                    submitter.DisplayOrder = displayOrder;
                                    Manager.SaveOrUpdate(submitter);
                                    displayOrder++;
                                }
                                else
                                    toAdd.Add(dto);
                            }
                            foreach (dtoSubmitterType dto in toAdd)
                            {
                                SubmitterType submitter = new SubmitterType();
                                submitter.Call = call;
                              
                                submitter.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                submitter.Name = dto.Name;
                                submitter.Description = dto.Description;
                                submitter.Deleted = BaseStatusDeleted.None;
                                submitter.AllowMultipleSubmissions = dto.AllowMultipleSubmissions;
                                if (!dto.AllowMultipleSubmissions || dto.MaxMultipleSubmissions < 1)
                                    submitter.MaxMultipleSubmissions = 1;
                                else
                                    submitter.MaxMultipleSubmissions = dto.MaxMultipleSubmissions;

                                submitter.DisplayOrder = displayOrder;
                                Manager.SaveOrUpdate(submitter);
                                displayOrder++;
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                public Boolean UpdateSubmittersDisplayOrder(List<long> idSubmitters)
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person!=null && idSubmitters.Count > 0 && AllowReorder(GetCallFromSubmitters(idSubmitters), person))
                    {
                        DateTime CurrentTime = DateTime.Now;
                        SubmitterType submitter;
                        try
                        {
                            Manager.BeginTransaction();
                            int displayOrder = 1;
                            foreach (var idItem in idSubmitters)
                            {
                                submitter = Manager.Get<SubmitterType>(idItem);
                                if (submitter != null)
                                {
                                    submitter.DisplayOrder = displayOrder;
                                    submitter.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    Manager.SaveOrUpdate(submitter);
                                    displayOrder++;
                                }
                            }
                            Manager.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            return false;
                        }
                    }
                    return false;
                }
                private BaseForPaper GetCallFromSubmitters(List<long> idSubmitters)
                {
                    List<long> idItems = (from s in Manager.GetIQ<SubmitterType>() where idSubmitters.Contains(s.Id) && s.Call !=null select s.Call.Id).ToList().Distinct().ToList();
                    if (idItems.Count() !=1)
                        return null;
                    else
                        return Manager.Get<BaseForPaper>(idItems[0]);
                }
                private void UpdateDisplayOrder(long displayOrder, SubmitterType item)
                {
                    List<SubmitterType> list = (from i in Manager.GetIQ<SubmitterType>()
                                              where i.Deleted == BaseStatusDeleted.None && i.Call == item.Call && i.Id != item.Id
                                              orderby i.DisplayOrder
                                              select i).ToList();
                    Int32 newDisplayOrder = 1;
                    foreach (SubmitterType it in list)
                    {
                        if (newDisplayOrder == displayOrder)
                            newDisplayOrder++;
                        it.DisplayOrder = newDisplayOrder++;
                    }
                    if (list.Count > 0)
                        Manager.SaveOrUpdateList(list);
                }
                private Int32 GetNewSubmitterDisplayOrder(BaseForPaper call){
                    return (from s in Manager.GetIQ<SubmitterType>() 
                            where s.Deleted == BaseStatusDeleted.None && s.Call==call select s.Id).Count()+1;
                }
                      
                public Boolean VirtualDeleteSubmitterType(long idSubmitter, Boolean delete)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);
                        if (submitter != null && person!=null )
                        {
                            if ((from s in Manager.GetIQ<UserSubmission>() where s.Call == submitter.Call && s.Type == submitter && s.Deleted == BaseStatusDeleted.None select s.Id).Any())
                                throw new SubmissionLinked();
                            else
                            {
                                Manager.BeginTransaction();
                                submitter.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                submitter.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                Manager.SaveOrUpdate(submitter);
                                Manager.Commit();
                                result = true;
                            }
                        }
                    }
                    catch (SubmissionLinked ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return result;
                }
                public List<dtoSubmitterTypePermission> GetCallSubmittersType(long idCall, ModuleCallForPaper module, Boolean getAll)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    dtoGenericPermission permission = GetPermissionForSubmitters(call, module);
                    return GetCallSubmittersType(call, permission, getAll);
                }
                public List<dtoSubmitterTypePermission> GetCallSubmittersType(long idCall, ModuleRequestForMembership module, Boolean getAll)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    dtoGenericPermission permission = GetPermissionForSubmitters(call, module);
                    return GetCallSubmittersType(call, permission, getAll);
                }
                private List<dtoSubmitterTypePermission> GetCallSubmittersType(BaseForPaper call, dtoGenericPermission permission, Boolean getAll)
                {
                    List<dtoSubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                         where s.Call == call && (getAll || s.Deleted == BaseStatusDeleted.None)
                                                         select new dtoSubmitterType()
                                                         {
                                                             Id = s.Id,
                                                             AllowMultipleSubmissions = s.AllowMultipleSubmissions,
                                                             Deleted = s.Deleted,
                                                             Description = s.Description,
                                                             DisplayOrder = s.DisplayOrder,
                                                             MaxMultipleSubmissions = s.MaxMultipleSubmissions,
                                                             Name = s.Name
                                                         }).ToList();
                    return submitters.OrderBy(s => s.DisplayOrder).ThenBy(s=>s.Name).Select(s => new dtoSubmitterTypePermission(s, permission, GetSubmissionForSubmitter(call,s.Id))).ToList();
                }
                private dtoGenericPermission GetPermissionForSubmitters(BaseForPaper call, ModuleCallForPaper module)
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    dtoGenericPermission permission = new dtoGenericPermission();
                    if (call != null)
                    {

                        permission.AllowDelete = (module.Administration || module.ManageCallForPapers);
                        permission.AllowEdit = (module.Administration || module.ManageCallForPapers || (call.CreatedBy == person && module.EditCallForPaper));
                        permission.AllowUnDelete = (module.Administration || module.ManageCallForPapers || (call.CreatedBy == person && module.EditCallForPaper));
                        permission.AllowVirtualDelete = (module.Administration || module.ManageCallForPapers || (call.CreatedBy == person && module.EditCallForPaper));
                    }
                    return permission;
                }
                private dtoGenericPermission GetPermissionForSubmitters(BaseForPaper call, ModuleRequestForMembership module)
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    dtoGenericPermission permission = new dtoGenericPermission();
                    if (call != null)
                    {

                        permission.AllowDelete = (module.Administration || module.ManageBaseForPapers);
                        permission.AllowEdit = (module.Administration || module.ManageBaseForPapers || (call.CreatedBy == person && module.EditBaseForPaper));
                        permission.AllowUnDelete = (module.Administration || module.ManageBaseForPapers || (call.CreatedBy == person && module.EditBaseForPaper));
                        permission.AllowVirtualDelete = (module.Administration || module.ManageBaseForPapers || (call.CreatedBy == person && module.EditBaseForPaper));
                    }
                    return permission;
                }
                private long GetSubmissionForSubmitter(BaseForPaper call, long idSubmitter)
                {
                    return (from s in Manager.GetIQ<UserSubmission>()
                            where s.Call == call && s.Deleted == BaseStatusDeleted.None && s.Type != null && s.Type.Id == idSubmitter
                            select s.Id).Count();
                }
                public List<dtoSubmitterType> GetCallAvailableSubmittersType(long idCall)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return GetCallAvailableSubmittersType(call);
                }
                public List<dtoSubmitterType> GetCallAvailableSubmittersType(BaseForPaper call)
                {
                    List<dtoSubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                         where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                         select new dtoSubmitterType()
                                                         {
                                                             Id = s.Id,
                                                             AllowMultipleSubmissions = s.AllowMultipleSubmissions,
                                                             Deleted = s.Deleted,
                                                             Description = s.Description,
                                                             DisplayOrder = s.DisplayOrder,
                                                             MaxMultipleSubmissions = s.MaxMultipleSubmissions,
                                                             Name = s.Name
                                                         }).ToList();
                    return submitters.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                }
            #endregion

            #region "Attachments"
                public Boolean UploadAttachments(BaseForPaper call, List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> itemsToAdd, String moduleCode, Int32 idModule, Int32 idCommunity,  Int32 idObjectType)
                {
                    Boolean uploaded = false;
                    Boolean OpenTransaction = !Manager.IsInTransaction();
                    try
                    {
                        if (OpenTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person!=null){
                            Int32 displayOrder = (from c in Manager.GetIQ<AttachmentFile>() where c.Deleted == BaseStatusDeleted.None && c.Call == call select c.DisplayOrder).Max() + 1;
                            Int32 count = (from c in Manager.GetIQ<AttachmentFile>() where c.Deleted == BaseStatusDeleted.None && c.Call == call select c.Id).Count();
                            if (count >= displayOrder)
                            {
                                displayOrder = count + 1;
                                List<AttachmentFile> items = (from c in Manager.GetIQ<AttachmentFile>() where c.Deleted == BaseStatusDeleted.None && c.Call == call orderby c.DisplayOrder select c).ToList();
                                if (items.Count > 0)
                                    items.ForEach(i => i.DisplayOrder = displayOrder++);
                                Manager.SaveOrUpdateList(items);
                            }
                            foreach (lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem item in itemsToAdd)
                            {
                                AttachmentFile attachment = new AttachmentFile();
                                attachment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                attachment.Call = call;
                                attachment.Description = "";
                                attachment.DisplayOrder = displayOrder;
                                attachment.Item = item.ItemAdded;
                                attachment.ForAll = true;
                                
                                Manager.SaveOrUpdate(attachment);
                                ModuleLink link = new ModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);
                                link.CreateMetaInfo(GetPersonFromLite(person), UC.IpAddress, UC.ProxyIpAddress);
                                link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
                                link.AutoEvaluable = false;
                                link.SourceItem = ModuleObject.CreateLongObject(attachment.Id, attachment, idObjectType, idCommunity, moduleCode, idModule);
                                Manager.SaveOrUpdate(link);
                                attachment.Link = Manager.Get<liteModuleLink>(link.Id);
                                Manager.SaveOrUpdate(attachment);

                                if (item.ItemAdded.IsInternal)
                                {
                                    if (item.ItemAdded.Module == null)
                                    {
                                        item.ItemAdded.Module = new lm.Comol.Core.FileRepository.Domain.ItemModuleSettings();
                                    }
                                    item.ItemAdded.Module.IdObject = attachment.Id;
                                    item.ItemAdded.Module.IdObjectType = idObjectType;
                                    Manager.SaveOrUpdate(item.ItemAdded);
                                }

                                displayOrder++;
                            }
                        }
                        if (OpenTransaction)
                            Manager.Commit();
                        uploaded = true;
                    }
                    catch (Exception ex)
                    {
                        uploaded = false;
                        if (OpenTransaction)
                            Manager.RollBack();
                    }
                    if (!uploaded)
                    {

                    }
                    return uploaded;
                }

                public Boolean SaveAttachments(long idCall, List<dtoAttachmentFile> items)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return SaveAttachments(call, items);
                }
                public Boolean SaveAttachments(BaseForPaper call, List<dtoAttachmentFile> items)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            Int32 displayOrder = 1;
                            foreach (dtoAttachmentFile dto in items)
                            {
                                AttachmentFile attFile = Manager.Get<AttachmentFile>(dto.Id);
                                if (attFile != null)
                                {
                                    attFile.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    attFile.Description = dto.Description;


                                    List<AttachmentAssignment> assignments = (from a in Manager.GetIQ<AttachmentAssignment>()
                                                                              where a.Attachment == attFile 
                                                                              select a).ToList();
                                    if (dto.ForAll && !attFile.ForAll) {
                                        assignments.Where(a=> a.Deleted == BaseStatusDeleted.None).ToList().ForEach(a=>a.SetDeleteMetaInfo(person, UC.IpAddress,UC.ProxyIpAddress));
                                        Manager.SaveOrUpdateList(assignments);
                                    }
                                    attFile.ForAll = dto.ForAll;
                                    if (!dto.ForAll) {
                                        foreach (long idSubmitter in dto.SubmitterAssignments){
                                            SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter); 
                                            AttachmentAssignment assignment = (from a in Manager.GetIQ<AttachmentAssignment>()
                                                                               where a.Attachment == attFile && a.SubmitterType == submitter
                                                                                  select a).Skip(0).Take(1).ToList().FirstOrDefault();
                                            if (assignment == null && submitter!=null)
                                            {
                                                assignment = new AttachmentAssignment();
                                                assignment.Call = call;
                                                assignment.Attachment = attFile;
                                                assignment.SubmitterType = submitter;
                                                assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                Manager.SaveOrUpdate(assignment);
                                            }
                                            else if (assignment != null && assignment.Deleted != BaseStatusDeleted.None)
                                            {
                                                assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                Manager.SaveOrUpdate(assignment);
                                            }
                                        }
                                        List<AttachmentAssignment> toRemove = assignments.Where(a => a.Deleted== BaseStatusDeleted.None && dto.SubmitterAssignments.Contains(a.SubmitterType.Id) == false).ToList();
                                        if (toRemove.Count > 0)
                                        {
                                            toRemove.ForEach(a => a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                            Manager.SaveOrUpdateList(toRemove);
                                        }
                                    }
                                    attFile.DisplayOrder = displayOrder;
                                    Manager.SaveOrUpdate(attFile);

                                    displayOrder++;
                                }
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                public Boolean UpdateAttachmentsDisplayOrder(List<long> idAttachments)
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person != null && idAttachments.Count > 0 && AllowReorder(GetCallAttachments(idAttachments), person))
                    {
                        DateTime CurrentTime = DateTime.Now;
                        AttachmentFile attachment;
                        try
                        {
                            Manager.BeginTransaction();
                            int displayOrder = 1;
                            foreach (var idItem in idAttachments)
                            {
                                attachment = Manager.Get<AttachmentFile>(idItem);
                                if (attachment != null)
                                {
                                    attachment.DisplayOrder = displayOrder;
                                    attachment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    Manager.SaveOrUpdate(attachment);
                                    displayOrder++;
                                }
                            }
                            Manager.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            return false;
                        }
                    }
                    return false;
                }
                public List<dtoAttachmentFilePermission> GetCallAttachments(long idCall, ModuleCallForPaper module, Boolean getAll)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    dtoGenericPermission permission = GetPermissionForSubmitters(call, module);
                    return GetCallAttachments(call, permission, getAll);
                }
                public List<dtoAttachmentFilePermission> GetCallAttachments(long idCall, ModuleRequestForMembership module, Boolean getAll)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    dtoGenericPermission permission = GetPermissionForSubmitters(call, module);
                    return GetCallAttachments(call, permission, getAll);
                }
                private List<dtoAttachmentFilePermission> GetCallAttachments(BaseForPaper call, dtoGenericPermission permission, Boolean getAll)
                {
                    List<AttachmentFile> attachments = (from s in Manager.GetAll<AttachmentFile>(s=>s.Call == call && s.Link !=null && (getAll || s.Deleted == BaseStatusDeleted.None))
                                                        select s).ToList();
                    List<dtoAttachmentFile> items = (from a in attachments
                                                     select new dtoAttachmentFile()
                                                      {
                                                          Id = a.Id,
                                                          Deleted = a.Deleted,
                                                          Description = a.Description,
                                                          DisplayOrder = a.DisplayOrder,
                                                          Link = a.Link,
                                                          ForAll = a.ForAll,
                                                          SubmitterAssignments = (from asgn in Manager.GetIQ<AttachmentAssignment>()
                                                                         where asgn.Deleted == BaseStatusDeleted.None && asgn.Attachment == a
                                                                                  select asgn.SubmitterType.Id).ToList()
                                                      }).ToList();
                    long count = GetSubmissionsCount(call);
                    return items.OrderBy(s => s.DisplayOrder).Select(s => new dtoAttachmentFilePermission(s, permission, count)).ToList();
                }
                private BaseForPaper GetCallAttachments(List<long> idAttachments)
                {
                    List<long> idItems = (from s in Manager.GetIQ<AttachmentFile>() where idAttachments.Contains(s.Id) && s.Call != null select s.Call.Id).ToList().Distinct().ToList();
                    if (idItems.Count() != 1)
                        return null;
                    else
                        return Manager.Get<BaseForPaper>(idItems[0]);
                }
                public Boolean AllowSeeAttachment(AttachmentFile attachment,litePerson person)
                {
                    return true;
                    
                    //Una PERSONA puo' usare qualunque "Tipo sottomittore".
                    //Quindi i suoi permessi DIPENDONO DA ALTRO!!!
                    //Se ne occuperà POI l'interfaccia a nascondere i file non di interesse!!!
                    Boolean allow = (attachment != null && attachment.ForAll);


                    try
                    {
                        if (!allow && attachment != null && person != null && attachment.Call != null)
                        { 
                            List<SubmitterType> submitters = (from s in Manager.GetIQ<UserSubmission>()
                                             where s.Call== attachment.Call 
                                             && s.Owner==person 
                                             && s.Deleted== BaseStatusDeleted.None
                                             select s.Type).ToList();

                            if (submitters.Count > 0)
                                allow = (from a in Manager.GetIQ<AttachmentAssignment>()
                                         where a.Attachment == attachment && a.Deleted == BaseStatusDeleted.None
                                         && submitters.Contains(a.SubmitterType)
                                         select a.Id).Any();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    return allow;
                }
                public Boolean VirtualDeleteAttachment(long idAttachment, Boolean delete)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        AttachmentFile attachment = Manager.Get<AttachmentFile>(idAttachment);
                        if (attachment != null && person != null)
                        {
                            Manager.BeginTransaction();
                            attachment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            attachment.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                            Manager.SaveOrUpdate(attachment);
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return result;
                }
            #endregion

            #region "Editor"
              

                #region "Section"
                     public FieldsSection AddSectionToCall(long idCall, List<dtoCallSection<dtoCallField>> sections,String name, String description)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (call != null)
                            SaveSections(call, sections, call.Tags);
                        return AddSection(call, name, description);
                    }
                    public FieldsSection AddSection(long idCall, String name, String description)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return AddSection(call, name, description);
                    }
                    public FieldsSection AddSection(BaseForPaper call, String name, String description) {
                        FieldsSection section = null;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null) {
                                section = new FieldsSection();
                                section.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                section.DisplayOrder = (from s in Manager.GetIQ<FieldsSection>() where s.Call.Id == call.Id select s.DisplayOrder).Max() + 1;
                                section.Name = name + section.DisplayOrder.ToString();
                                section.Description = description;
                                section.Call = call;
                                Manager.SaveOrUpdate(section);
                                call.Sections.Add(section);
                                Manager.SaveOrUpdate(call);
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex) {
                            section = null;
                            Manager.RollBack();
                        }
                        return section;
                    }
                    public FieldsSection CloneSection(long idCall, List<dtoCallSection<dtoCallField>> sections, long idSection)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (call != null)
                            SaveSections(call, sections, call.Tags);
                        return CloneSection(call, idSection);
                    }
                    public FieldsSection CloneSection(BaseForPaper call, long idSection)
                    {
                        FieldsSection section = null;
                        try
                        {
                            Manager.BeginTransaction();
                            FieldsSection source = Manager.Get<FieldsSection>(idSection);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null && source != null)
                            {
                                section = new FieldsSection();
                                section.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                section.Description = source.Description;
                                section.DisplayOrder = source.DisplayOrder;
                                //Bello impostare l'ordinamento degli elementi in modo che TUTTI seguano il giosto "ordinamento",
                                //peccato però sovrascrivere l'ordinamento dell'utente e che la cosa non serva assolutamente a nulla, essendo l'ordinamento INTERNO al bando/sezione!
                                //(from s in Manager.GetIQ<FieldsSection>() where s.Call.Id == call.Id select s.DisplayOrder).Max() + 1;
                                section.Name = section.DisplayOrder.ToString() + "-" + source.Name;
                                section.Call = call;
                                section.Fields = new List<FieldDefinition>();
                                Manager.SaveOrUpdate(section);
                                call.Sections.Add(section);

                                foreach (FieldDefinition field in source.Fields.Where(f => f.Deleted == BaseStatusDeleted.None).ToList()) {
                                   
                                    FieldDefinition f = CloneField(person,field, section,false);
                                    if (f != null)
                                    {
                                        section.Fields.Add(f);
                                    }
                                }
                                Manager.SaveOrUpdate(call);
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            section = null;
                            Manager.RollBack();
                        }
                        return section;
                    }
                    public Boolean SaveSections(
                        long idCall, 
                        List<dtoCallSection<dtoCallField>> sections,
                        string Tags)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return SaveSections(call, sections, Tags);
                    }
                    public Boolean SaveSections(
                        BaseForPaper call, 
                        List<dtoCallSection<dtoCallField>>sections,
                        string tags)
                    {
                        if (!String.IsNullOrWhiteSpace(tags))
                        {
                            call.Tags = tags;
                        }

                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {
                                Manager.BeginTransaction();
                                int displayNumber = 1;
                                foreach (dtoCallSection<dtoCallField> item in sections) {
                                    if (String.IsNullOrEmpty(item.Name))
                                        item.Name = displayNumber.ToString();
                                    FieldsSection section = Manager.Get<FieldsSection>(item.Id);
                                    if (section == null)
                                    {
                                        section = new FieldsSection();
                                        section.Call = call;
                                        section.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        section.DisplayOrder = GetNewSectionDisplayOrder(call);
                                        section.Name = item.Name;
                                        section.Description = item.Description;
                                        section.DisplayOrder = displayNumber;
                                        Manager.SaveOrUpdate(section);
                                    }
                                    else if (section.Name != item.Name || section.Description != item.Description)
                                    {
                                        section.Name = item.Name;
                                        section.Description = item.Description;
                                        section.DisplayOrder = displayNumber;
                                        section.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(section);
                                    }
                                     SaveFields(call, section, item.Fields);
                                    //Manager.SaveOrUpdate(section);
                                    displayNumber++;
                                }
                                Manager.Commit();
                                result = true;
                            }
                        }
                        catch (Exception ex) {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                        }
                        return result;
                    }
                    public Boolean isNewCall(BaseForPaper call)
                    {
                        return !(from s in Manager.GetIQ<FieldsSection>() where s.Call == call select s.Id).Any();
                    }
                    public List<dtoCallSection<dtoCallField>> GetEditorSections(long idCall)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return GetEditorSections(call);
                    }
                    public List<dtoCallSection<dtoCallField>> GetEditorSections(BaseForPaper call) {
                        List<dtoCallSection<dtoCallField>> sections = new List<dtoCallSection<dtoCallField>>();
                        try
                        {
                            sections = (from s in Manager.GetIQ<FieldsSection>()
                                        where s.Deleted == BaseStatusDeleted.None 
                                        && s.Call == call
                                        select new dtoCallSection<dtoCallField>() 
                                        { 
                                            Id = s.Id, 
                                            Name = s.Name, 
                                            Description = s.Description, 
                                            DisplayOrder = s.DisplayOrder 
                                        }).ToList();

                            //List<dtoCallField> fields = (from f in Manager.GetIQ<FieldDefinition>()
                            //                             where f.Call == call
                            //                                 && f.Deleted == BaseStatusDeleted.None
                            //                             select f).ToList()
                            //                             .Select( f => new dtoCallField(f)).ToList();

                            //IList<FieldDefinition> test = Manager.GetAll<FieldDefinition>(fd => !String.IsNullOrEmpty(fd.Tags));
                            //Int64 id = 520;
                            //FieldDefinition test2 = Manager.Get<FieldDefinition>(id);
                            //id = 519;
                            //FieldDefinition test3 = Manager.Get<FieldDefinition>(id);

                            IList<dtoCallField> fields = (from f in Manager.GetAll<FieldDefinition>(
                                fd => fd.Call == call
                                      && fd.Deleted == BaseStatusDeleted.None)
                                select new dtoCallField(f)
                                ).ToList();

                            foreach (dtoCallSection<dtoCallField> section in sections) 
                            { 
                                section.Fields= 
                                    (from f in fields 
                                     where f.IdSection == section.Id orderby f.DisplayOrder, f.Name 
                                     select f)
                                     .ToList();

                                section.Fields.ForEach(
                                    f => f.Submitters= 
                                        (from s in Manager.GetIQ<FieldAssignment>()
                                            where s.Deleted== BaseStatusDeleted.None 
                                            && s.SubmitterType!=null 
                                            && s.Field !=null 
                                            && s.Field.Id== f.Id
                                            select s.SubmitterType.Id).ToList());

                            }
                        }
                        catch (Exception ex) {
                    
                        }
                        return sections.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                    }
                    private int GetNewSectionDisplayOrder(BaseForPaper call)
                    {
                        try
                        {
                            int displayOrder = (from s in Manager.GetIQ<FieldsSection>()
                                                where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                select s.DisplayOrder).Max();
                            displayOrder++;
                            return displayOrder;
                        }
                        catch (Exception)
                        {
                            return 1;

                        }
                    }
                    public Boolean UpdateSectionsDisplayOrder(List<long> idSections)
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (idSections.Count > 0 && AllowReorder(GetCallFromSections(idSections), person))
                        {
                            DateTime CurrentTime = DateTime.Now;
                            FieldsSection section=null;
                            try
                            {
                                Manager.BeginTransaction();
                                int displayOrder = 1;
                                foreach (var idSection in idSections)
                                {
                                    section = Manager.Get<FieldsSection>(idSection);
                                    if (section != null) {
                                        section.DisplayOrder = displayOrder;
                                        section.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate<FieldsSection>(section);
                                        displayOrder++;
                                    }
                                }
                                Manager.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                return false;
                            }
                        }
                        return false;
                    }
                    private BaseForPaper GetCallFromSections(List<long> idSections)
                    {
                        List<long> idCalls = (from s in Manager.GetIQ<FieldsSection>() where idSections.Contains(s.Id) select s.Call.Id).ToList();
                        if (idCalls.Distinct<long>().ToList().Count != 1)
                            return null;
                        else
                            return Manager.Get<BaseForPaper>(idCalls[0]);
                    }
                    private Dictionary<long, BaseForPaper> GetCallsFromSections(List<long> idSections)
                    {
                        return (from s in Manager.GetIQ<FieldsSection>() where idSections.Contains(s.Id) select s).ToDictionary(s => s.Id, s => s.Call);
                    }
                    public Boolean VirtualDeleteCallSection(long idSection, Boolean delete, ref long outputIdSection)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            FieldsSection section = Manager.Get<FieldsSection>(idSection);
                            if (section != null)
                            {
                                outputIdSection = section.Id;
                                if (section.Call != null &&
                                     (section.Call.Type == CallForPaperType.CallForBids && (from s in Manager.GetIQ<UserSubmission>()
                                                                                             where s.Call.Id == section.Call.Id && s.Deleted == BaseStatusDeleted.None
                                                                                             select s.Id).Any())
                                    )
                                    throw new SubmissionLinked();
                                else
                                {
                                    Manager.BeginTransaction();
                                    section.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    section.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                    foreach (FieldDefinition field in section.Fields)
                                    {
                                        field.Deleted = delete ? (field.Deleted | BaseStatusDeleted.Cascade) : (field.Deleted = (BaseStatusDeleted)((int)field.Deleted - (int)BaseStatusDeleted.Cascade));
                                        Manager.SaveOrUpdate(field);
                                    }
                                    if (delete)
                                    {
                                        var query = (from s in Manager.GetIQ<FieldsSection>()
                                                     where s.Call.Id == section.Call.Id && s.Deleted == BaseStatusDeleted.None && s.Id != section.Id
                                                     select s);
                                        outputIdSection = (from s in query where s.DisplayOrder <= section.DisplayOrder orderby s.DisplayOrder descending select s.Id).FirstOrDefault();
                                        if (outputIdSection == 0)
                                            outputIdSection = (from s in query where s.DisplayOrder > section.DisplayOrder orderby s.DisplayOrder select s.Id).FirstOrDefault();

                                    }
                                    Manager.SaveOrUpdate(section);
                                    Manager.Commit();
                                    result = true;
                                }
                            }
                            else
                                result = true;
                        }
                        catch (SubmissionLinked ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                #endregion 

                #region "Field"
                    public FieldDefinition CloneField(litePerson person,FieldDefinition sField, FieldsSection dSection, Boolean rename)
                    {
                        FieldDefinition field = null;
                        if (sField != null && dSection != null)
                        {
                            //VEDI SOPRA: NON SERVE A NULLA SE NON A FAR DANNARE L'UTENTE
                            //TANTO ' INTERNO ALLA SEZIONE!!!
                            //(!dSection.Fields.Any() || dSection.Fields.Count==0) ?  
                            //1 : 
                            //dSection.Fields.Where(f=>f.Deleted== BaseStatusDeleted.None).Select(f=>f.DisplayOrder).Max() + 1;

                            Int32 displayOrder = sField.DisplayOrder;

                            
                            field = new FieldDefinition();
                            switch (sField.Type) { 
                                case FieldType.CheckboxList:
                                case FieldType.RadioButtonList:
                                case FieldType.DropDownList:
                                    field.MaxOption = sField.MaxOption;
                                    field.MinOption = sField.MinOption;
                                    break;
                                case FieldType.Disclaimer:
                                    field.MaxOption = sField.MaxOption;
                                    field.MinOption = sField.MinOption;
                                    field.DisclaimerType = sField.DisclaimerType;
                                    break;
                                case FieldType.TableSimple:
                                    field.TableMaxRows = sField.TableMaxRows;
                                    field.TableCols = sField.TableCols;
                                    field.TableMaxTotal = sField.TableMaxTotal;
                                    break;
                                case FieldType.TableReport:
                                    field.TableMaxRows = sField.TableMaxRows;
                                    field.TableCols = sField.TableCols;
                                    field.TableMaxTotal = sField.TableMaxTotal;
                                    break;
                            }
                            field.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            field.Call = dSection.Call;
                            field.Section = dSection;
                            field.Description = sField.Description;
                            field.DisplayOrder = displayOrder;
                            field.Mandatory = sField.Mandatory;
                            field.Type = sField.Type;
                            field.ToolTip = sField.ToolTip;
                            field.RegularExpression = sField.RegularExpression;
                            field.Name = (rename) ? displayOrder.ToString() + "-" + sField.Name : sField.Name;
                            field.MaxLength = sField.MaxLength;
                            Manager.SaveOrUpdate(field);

                            if (field.Type == FieldType.CheckboxList 
                                || field.Type == FieldType.RadioButtonList 
                                || field.Type == FieldType.DropDownList 
                                || field.Type ==  FieldType.Disclaimer)
                                CloneOptions(person, field, sField.Options.Where(o=>o.Deleted== BaseStatusDeleted.None).ToList());
                            Manager.SaveOrUpdate(field);
                            AddFieldAssignments(person, field, (from a in Manager.GetIQ<FieldAssignment>() where a.Deleted== BaseStatusDeleted.None && a.Field.Id==sField.Id select a).ToList());
                        }
                        return field;
                    }
                    public Boolean CloneField(long idField, ref long outputIdSection, ref long outputIdField)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            FieldDefinition field = Manager.Get<FieldDefinition>(idField);
                            if (field != null && field.Section != null)
                            {
                                outputIdSection = field.Section.Id;
                                outputIdField = field.Id;
                                if (field.Call != null &&
                                     (field.Call.Type == CallForPaperType.CallForBids && (from s in Manager.GetIQ<UserSubmission>()
                                                                                          where s.Call.Id == field.Call.Id && s.Deleted == BaseStatusDeleted.None
                                                                                          select s.Id).Any())
                                    )
                                    throw new SubmissionLinked();
                                else
                                {
                                    Manager.BeginTransaction();
                                    FieldDefinition clone = CloneField(person, field, field.Section,true);
                                    if (clone == null)
                                        outputIdField = field.Id;
                                    else
                                        outputIdField = clone.Id;
                                    Manager.Commit();
                                    result = true;
                                }
                            }
                            else
                                result = true;
                        }
                        catch (SubmissionLinked ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    private List<FieldOption> CloneOptions(litePerson person, FieldDefinition field, List<FieldOption> items)
                    {
                        List<FieldOption> options = new List<FieldOption>();
                        if (field != null && person != null && items != null && items.Any())
                        {
                            int displayNumber = 1;
                            foreach (FieldOption item in items.OrderBy(i => i.DisplayOrder))
                            {
                                FieldOption opt = new FieldOption();
                                opt.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                opt.DisplayOrder = displayNumber; // item.DisplayOrder; Perchè commentato? Boh...
                                opt.Field = field;
                                opt.Name = item.Name;
                                opt.IsDefault = item.IsDefault;
                                opt.IsFreeValue = item.IsFreeValue;
                                if (!String.IsNullOrEmpty(item.Value))
                                    opt.Value = item.Value;
                                else
                                    opt.Value = GetOptionValue(field);
                                Manager.SaveOrUpdate(opt);
                                options.Add(opt);
                                displayNumber++;
                                switch (field.Type) { 
                                    case FieldType.Disclaimer:
                                    case FieldType.RadioButtonList:
                                    case FieldType.CheckboxList:
                                    case FieldType.DropDownList:
                                        field.Options.Add(opt);
                                        break;
                                }
                            }
                        }

                        return options;
                    }
                   
                    public List<FieldDefinition> AddFields(long idCall, List<dtoCallSection<dtoCallField>> sections, long idSection, List<dtoCallField> items)
                    {
                        List<FieldDefinition> fields = new List<FieldDefinition>();
                        SaveSections(idCall, sections, "");
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            FieldsSection section = Manager.Get<FieldsSection>(idSection);
                            if (section != null && person !=null) {
                                List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>() where s.Deleted == BaseStatusDeleted.None && s.Call == section.Call select s).ToList();
                                Int32 displayOrder = (from f in Manager.GetIQ<FieldDefinition>() where f.Deleted== BaseStatusDeleted.None && f.Section.Id == section.Id select f.DisplayOrder).Max() + 1;
                                foreach (dtoCallField dto in items) {
                                    FieldDefinition field =  new FieldDefinition();
                                    if (dto.Type == FieldType.CheckboxList || dto.Type == FieldType.RadioButtonList || dto.Type == FieldType.DropDownList)
                                    {
                                        field.MaxOption = dto.MaxOption;
                                        field.MinOption = dto.MinOption;
                                    }
                                    else if (dto.Type == FieldType.Disclaimer) {
                                        field.MaxOption = dto.MaxOption;
                                        field.MinOption = dto.MinOption;
                                        field.DisclaimerType = dto.DisclaimerType;
                                    }
                                    else
                                        field.MaxLength = dto.MaxLength;
                                    field.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    field.Call = section.Call;
                                    field.Section = section;
                                    field.Description = dto.Description;
                                    field.DisplayOrder = displayOrder;
                                    field.Mandatory = dto.Mandatory;
                                    field.Type = dto.Type;
                                    field.ToolTip = dto.ToolTip;
                                    field.RegularExpression = dto.RegularExpression;
                                    if (String.IsNullOrEmpty(dto.Name))
                                        field.Name = displayOrder.ToString();
                                    else if (dto.Name.Contains("{0}"))
                                        field.Name = String.Format(dto.Name,displayOrder.ToString());
                                    else
                                        field.Name = dto.Name;
                                    Manager.SaveOrUpdate(field);
                                    if (dto.Type == FieldType.CheckboxList || dto.Type == FieldType.RadioButtonList || dto.Type == FieldType.DropDownList ||dto.Type == FieldType.Disclaimer)
                                        SaveOptions(person, field, dto.Options);
                                    Manager.SaveOrUpdate(field);
                                    displayOrder++;
                                    AddFieldAssignments(person, field, submitters);
                                    section.Fields.Add(field);
                                    fields.Add(field);
                                }
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex) {
                            Manager.RollBack();
                            fields.Clear();
                        }
                        return fields;
                    }
                    private List<FieldDefinition> SaveFields(BaseForPaper call, FieldsSection section, List<dtoCallField> items)
                    {
                        List<FieldDefinition> fields = new List<FieldDefinition>();
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && section != null && person != null)
                            {
                                int displayNumber = 1;
                                foreach (dtoCallField item in items)
                                {
                                    if (String.IsNullOrEmpty(item.Name))
                                        item.Name = item.DisplayOrder.ToString();
                                    FieldDefinition field = Manager.Get<FieldDefinition>(item.Id);
                                    if (field == null)
                                    {
                                        field = new FieldDefinition();
                                        field.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    else
                                        field.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    if (item.Type == FieldType.CheckboxList || item.Type == FieldType.RadioButtonList || item.Type == FieldType.DropDownList || item.Type == FieldType.Disclaimer)
                                    {
                                        field.MaxOption = item.MaxOption;
                                        field.MinOption = item.MinOption;
                                        field.DisclaimerType = (item.Type == FieldType.Disclaimer) ? item.DisclaimerType : DisclaimerType.None;
                                    } else if (item.Type == FieldType.TableReport || item.Type == FieldType.TableSimple)
                                    {
                                        field.TableCols = item.TableFieldSetting.Cols;
                                        field.TableMaxRows = item.TableFieldSetting.MaxRows;
                                        field.TableMaxTotal = item.TableFieldSetting.MaxTotal;
                                    }
                                    field.Call = call;
                                    field.Section = section;
                                    field.Name = item.Name;
                                    field.Tags = item.Tags;
                                    field.Description = item.Description;
                                    field.DisplayOrder = item.DisplayOrder;
                                    field.Mandatory = item.Mandatory;
                                    field.MaxLength = item.MaxLength;
                                    field.Type = item.Type;
                                    field.ToolTip = item.ToolTip;
                                    field.RegularExpression = item.RegularExpression;

                                    //field.TableCols = item.TableFieldSetting.Cols;
                                    //field.TableMaxRows = item.TableFieldSetting.MaxRows;

                                    Manager.SaveOrUpdate(field);
                                    if (item.Type == FieldType.CheckboxList || item.Type == FieldType.RadioButtonList || item.Type == FieldType.DropDownList||item.Type == FieldType.Disclaimer)
                                        SaveOptions(person, field, item.Options);

                                    SaveFieldAssignments(person, field, item.Submitters);
                                    //Manager.SaveOrUpdate(field);
                                    displayNumber++;
                                    fields.Add(field);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return fields;
                    }
                    private List<FieldAssignment> SaveFieldAssignments(litePerson person, FieldDefinition field, List<long> idSubmitters)
                    {
                        List<FieldAssignment> assignments = new List<FieldAssignment>();
                        try
                        {
                            if (person != null && field != null)
                            {
                                foreach (long idSubmitter in idSubmitters)
                                {
                                    SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);
                                    FieldAssignment assignment = (from a in Manager.GetIQ<FieldAssignment>()
                                                                  where a.Field.Id == field.Id && a.SubmitterType == submitter
                                                                       select a).Skip(0).Take(1).ToList().FirstOrDefault();
                                    if (assignment == null && submitter != null)
                                    {
                                        assignment = new FieldAssignment();
                                        assignment.Field = field;
                                        assignment.SubmitterType = submitter;
                                        assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                    else if (assignment != null && assignment.Deleted != BaseStatusDeleted.None)
                                    {
                                        assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                    assignments.Add(assignment);
                                }
                                List<FieldAssignment> toRemove = (from a in Manager.GetIQ<FieldAssignment>()
                                                                  where a.Field.Id == field.Id && idSubmitters.Contains(a.SubmitterType.Id) == false
                                                                        && a.Deleted == BaseStatusDeleted.None
                                                                        select a).ToList();
                                if (toRemove.Count > 0)
                                {
                                    toRemove.ForEach(a => a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    Manager.SaveOrUpdateList(toRemove);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return assignments;
                    }
                    private List<FieldAssignment> AddFieldAssignments(litePerson person, FieldDefinition field, List<FieldAssignment> assignments)
                    {
                        return (assignments == null || assignments.Count == 0) ? new List<FieldAssignment>() : AddFieldAssignments(person, field, assignments.Select(a => a.SubmitterType).ToList());
                    }
                    private List<FieldAssignment> AddFieldAssignments(litePerson person, FieldDefinition field, List<SubmitterType> submitters)
                    {
                        List<FieldAssignment> assignments = new List<FieldAssignment>();
                        try
                        {
                            if (person != null && field != null)
                            {
                                foreach (SubmitterType submitter in submitters)
                                {
                                    FieldAssignment assignment = new FieldAssignment();
                                    assignment.Field = field;
                                    assignment.SubmitterType = submitter;
                                    assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    Manager.SaveOrUpdate(assignment);
                                    assignments.Add(assignment);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return assignments;
                    }
                    public Boolean UpdateFieldsDisplayOrder(List<long> idFields, long idSection)
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        FieldsSection section = Manager.Get<FieldsSection>(idSection);
                        if (idFields.Count > 0 && section!=null && section.Call !=null &  AllowReorder(section.Call, person))
                        {
                            try
                            {
                                Manager.BeginTransaction();

                                FieldDefinition field;
                                int displayOrder = 1;
                                foreach (var idField in idFields)
                                {
                                    field = Manager.Get<FieldDefinition>(idField);
                                    if (field != null && field.Call.Id == section.Call.Id)
                                    {
                                        field.DisplayOrder = displayOrder;
                                        if (field.Section.Id != section.Id)
                                            field.Section = section;
                                        field.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(field);
                                        displayOrder++;
                                    }
                                }
                                Manager.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                return false;
                            }
                        }
                        return false;
                    }
                    public Boolean VirtualDeleteCallField(long idField, Boolean delete, ref long outputIdSection, ref long outputIdField)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            FieldDefinition field = Manager.Get<FieldDefinition>(idField);
                            if (field != null && field.Section !=null)
                            {
                                outputIdSection = field.Section.Id;
                                outputIdField = field.Id;
                                if (field.Call != null &&
                                     (field.Call.Type == CallForPaperType.CallForBids && (from s in Manager.GetIQ<UserSubmission>()
                                                                                          where s.Call.Id == field.Call.Id && s.Deleted == BaseStatusDeleted.None
                                                                                            select s.Id).Any())
                                    )
                                    throw new SubmissionLinked();
                                else
                                {
                                    Manager.BeginTransaction();
                                    field.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    field.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                    if (field.Type == FieldType.CheckboxList || field.Type == FieldType.DropDownList || field.Type == FieldType.RadioButtonList || field.Type == FieldType.Disclaimer)
                                    {
                                        foreach (FieldOption option in field.Options)
                                        {
                                            option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                            Manager.SaveOrUpdate(option);
                                        }
                                    }
                                    ProfileAttributeAssociation association = (from a in Manager.GetIQ<ProfileAttributeAssociation>()
                                                                               where a.Field.Id == field.Id
                                                                               select a).Skip(0).Take(1).ToList().FirstOrDefault();
                                    if (association != null)
                                        association.Deleted = delete ? (association.Deleted | BaseStatusDeleted.Cascade) : (association.Deleted = (BaseStatusDeleted)((int)association.Deleted - (int)BaseStatusDeleted.Cascade));

                                    if (delete)
                                    {
                                        var query = (from s in Manager.GetIQ<FieldDefinition>()
                                                     where s.Section !=null && s.Section.Id==field.Section.Id && s.Deleted == BaseStatusDeleted.None && s.Id != field.Id
                                                     select s);
                                        outputIdField = (from s in query where s.DisplayOrder <= field.DisplayOrder orderby s.DisplayOrder descending select s.Id).FirstOrDefault();
                                        if (outputIdField == 0)
                                            outputIdField = (from s in query where s.DisplayOrder > field.DisplayOrder orderby s.DisplayOrder select s.Id).FirstOrDefault();

                                    }
                                    Manager.SaveOrUpdate(field);
                                    Manager.Commit();
                                    result = true;
                                }
                            }
                            else
                                result = true;
                        }
                        catch (SubmissionLinked ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                #endregion

                #region "FieldOptions"
                    public FieldOption AddOptionToField(long idField, List<dtoCallSection<dtoCallField>> sections, String name, Boolean isDefault, Boolean isFreeValue )
                    {
                        FieldDefinition field = Manager.Get<FieldDefinition>(idField);
                        if (field != null && field.Call !=null )
                            SaveSections(field.Call, sections, field.Call.Tags);

                        return AddOptionToField(field, name, isDefault,isFreeValue);
                    }
                    public FieldOption AddOptionToField(FieldDefinition field, String name,Boolean isDefault, Boolean isFreeValue)
                    {
                        FieldOption option = null;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (field != null && person != null)
                            {
                                option = new FieldOption();
                                option.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                option.DisplayOrder = (from s in Manager.GetIQ<FieldOption>() where s.Field.Id == field.Id select s.DisplayOrder).Max() + 1;
                                option.Value = GetOptionValue(field);
                                if (string.IsNullOrEmpty(name))
                                    option.Name = option.Value.ToString();
                                else
                                    option.Name = name;
                                option.Field = field;
                                option.IsDefault = isDefault;
                                option.IsFreeValue = isFreeValue;

                                Manager.SaveOrUpdate(option);
                                switch (field.Type)
                                {
                                    case FieldType.Disclaimer:
                                    case FieldType.RadioButtonList:
                                    case FieldType.CheckboxList:
                                    case FieldType.DropDownList:
                                        field.Options.Add(option);
                                        break;
                                }

                                if (isDefault || isFreeValue){
                                    List<FieldOption> dOptions = (from s in Manager.GetIQ<FieldOption>()
                                                                  where s.Field.Id == field.Id && s.IsDefault && s.Deleted == BaseStatusDeleted.None && s.Id != option.Id
                                                                  select s).ToList();
                                    if (dOptions.Any())
                                        dOptions.ForEach(o => o.IsDefault = false);
                                    List<FieldOption> fOptions = (from s in Manager.GetIQ<FieldOption>()
                                                                  where s.Field.Id == field.Id && s.IsFreeValue && s.Deleted == BaseStatusDeleted.None && s.Id != option.Id
                                                                  select s).ToList();
                                    if (fOptions.Any())
                                        fOptions.ForEach(o => o.IsDefault = false);
                                }
                                Manager.SaveOrUpdate(field);
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex) {
                            option = null;
                            Manager.RollBack();
                        }
                        return option;
                    }
                    //public FieldOption CloneOption(litePerson person,FieldMultipleDefinition field, String name)
                    //{
                    //    FieldOption option = null;
                    //    if (field != null && person != null)
                    //    {
                    //        option = new FieldOption();
                    //        option.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    //        option.DisplayOrder = (from s in Manager.GetIQ<FieldOption>() where s.Field.Id == field.Id select s.DisplayOrder).Max() + 1;
                    //        option.Value = GetOptionValue(field);
                    //        if (string.IsNullOrEmpty(name))
                    //            option.Name = option.Value.ToString();
                    //        else
                    //            option.Name = name;
                    //        option.Field = field;
                    //        Manager.SaveOrUpdate(option);
                    //        field.Options.Add(option);
                    //        Manager.SaveOrUpdate(field);
                    //    }
                    //    return option;
                    //}
                    public Boolean VirtualDeleteFieldOption(long idOption, Boolean delete, ref long outputIdSection, ref long outputIdField, ref long outputIdOption)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            FieldOption option = Manager.Get<FieldOption>(idOption);
                            if (option != null && option.Field != null)
                            {
                                if (option.Field.Section !=null)
                                    outputIdSection = option.Field.Section.Id;
                                outputIdField = option.Field.Id;
                                outputIdOption = option.Id;
                                if (option.Field.Call != null &&
                                     (option.Field.Call.Type == CallForPaperType.CallForBids && (from s in Manager.GetIQ<UserSubmission>()
                                                                                                 where s.Call.Id == option.Field.Call.Id && s.Deleted == BaseStatusDeleted.None
                                                                                          select s.Id).Any())
                                    )
                                    throw new SubmissionLinked();
                                else
                                {
                                    Manager.BeginTransaction();
                                    option.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    option.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                    if (delete)
                                    {
                                        option.IsDefault = false;
                                        if (option.Field != null && option.Field.MaxOption > option.Field.Options.Where(o => o.Deleted == BaseStatusDeleted.None).Count())
                                            option.Field.MaxOption = option.Field.Options.Where(o => o.Deleted == BaseStatusDeleted.None).Count();
                                        var query = (from s in Manager.GetIQ<FieldOption>()
                                                     where s.Field != null && s.Field.Id == option.Field.Id && s.Deleted == BaseStatusDeleted.None && s.Id != option.Id
                                                     select s);
                                        outputIdOption = (from s in query where s.DisplayOrder <= option.DisplayOrder orderby s.DisplayOrder descending select s.Id).FirstOrDefault();
                                        if (outputIdOption == 0)
                                            outputIdOption = (from s in query where s.DisplayOrder > option.DisplayOrder orderby s.DisplayOrder select s.Id).FirstOrDefault();

                                    }
                                    Manager.SaveOrUpdate(option);
                                    Manager.Commit();
                                    result = true;
                                }
                            }
                            else
                                result = true;
                        }
                        catch (SubmissionLinked ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    public Boolean SetAsDefaultOption(long idOption, Boolean isDefault)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            FieldOption option = Manager.Get<FieldOption>(idOption);
                            if (option != null && option.Field != null)
                            {
                                Manager.BeginTransaction();
                                if (isDefault && option.Field.Options.Where(o => o.Deleted == BaseStatusDeleted.None && o.IsDefault && o.Id != idOption).Any())
                                {
                                    foreach (FieldOption opt in option.Field.Options.Where(o => o.Deleted == BaseStatusDeleted.None && o.IsDefault && o.Id != idOption)) {
                                        opt.IsDefault = false;
                                        opt.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(option);
                                    }
                                }
                                option.IsDefault = isDefault;
                                option.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(option);
                                Manager.Commit();
                                result = true;
                            }
                            else
                                result = !isDefault;
                        }
                        catch (SubmissionLinked ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    public Boolean SaveDisclaimerType(long idField, DisclaimerType type)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            FieldDefinition field = Manager.Get<FieldDefinition>(idField);
                            if (field != null && field.Type== FieldType.Disclaimer)
                            {
                                Manager.BeginTransaction();
                                field.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                
                                switch (type) { 
                                    case DisclaimerType.Standard:
                                    case DisclaimerType.CustomDisplayOnly:
                                    case DisclaimerType.None:
                                        foreach (FieldOption option in field.Options.Where(o => o.Deleted == BaseStatusDeleted.None)) {
                                            option.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            option.Deleted = BaseStatusDeleted.Automatic;
                                        }
                                        field.MinOption = 0;
                                        field.MaxOption = 0;
                                        break;
                                    case DisclaimerType.CustomMultiOptions:
                                    case DisclaimerType.CustomSingleOption:
                                        if (field.DisclaimerType != DisclaimerType.CustomSingleOption) {
                                            foreach (FieldOption option in field.Options.Where(o => o.Deleted == BaseStatusDeleted.Automatic))
                                            {
                                                option.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                option.Deleted = BaseStatusDeleted.None;
                                            }
                                        }
                                        field.MinOption = (type== DisclaimerType.CustomSingleOption)? 1 :0;
                                        field.MaxOption = 1;
                                        break;
                                }
                                field.DisclaimerType = type;
                                Manager.SaveOrUpdate(field);
                                Manager.Commit();
                                result = true;
                            }
                            else
                                result = true;
                        }
                        catch (SubmissionLinked ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    private List<FieldOption> SaveOptions(litePerson person, FieldDefinition field, List<dtoFieldOption> items)
                    {
                        List<FieldOption> options = new List<FieldOption>();
                        try
                        {
                            if (field != null && person != null)
                            {
                                Boolean isNew = false;
                                int displayNumber = 1;
                                foreach (dtoFieldOption item in items)
                                {
                                    if (String.IsNullOrEmpty(item.Name))
                                        item.Name = item.DisplayOrder.ToString();
                                    FieldOption opt = Manager.Get<FieldOption>(item.Id);
                                    isNew = (opt == null);
                                    if (isNew)
                                    {
                                        opt = new FieldOption();
                                        opt.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    else
                                        opt.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    opt.DisplayOrder = displayNumber; // item.DisplayOrder;
                                    opt.Field = field;
                                    opt.IsDefault = item.IsDefault;
                                    opt.IsFreeValue = item.IsFreeValue;
                                    if (String.IsNullOrEmpty(item.Name))
                                        opt.Name = item.DisplayOrder.ToString();
                                    else if (item.Name.Contains("{0}"))
                                        opt.Name = String.Format(item.Name,item.DisplayOrder.ToString());
                                    else
                                        opt.Name = item.Name;
                                    if (!String.IsNullOrEmpty(item.Value))
                                        opt.Value = item.Value;
                                    else if (isNew)
                                        opt.Value = GetOptionValue(field);
                                    Manager.SaveOrUpdate(opt);
                                    options.Add(opt);
                                    displayNumber++;
                                }

                                List<FieldOption> dOptions = (from s in Manager.GetIQ<FieldOption>()
                                                                where s.Field.Id == field.Id && s.IsDefault && s.Deleted == BaseStatusDeleted.None 
                                                                select s).ToList();
                                if (dOptions.Any() && dOptions.Count>1)
                                    dOptions.Skip(1).ToList().ForEach(o => o.IsDefault = false);
                                List<FieldOption> fOptions = (from s in Manager.GetIQ<FieldOption>()
                                                              where s.Field.Id == field.Id && s.IsFreeValue && s.Deleted == BaseStatusDeleted.None 
                                                                select s).ToList();
                                if (fOptions.Any())
                                    fOptions.Skip(1).ToList().ForEach(o => o.IsDefault = false);

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return options;
                    }
                    private String GetOptionValue(FieldDefinition field)
                    {
                        String optionValue = "";
                        List<String> optionsValue = (from f in Manager.GetIQ<FieldOption>()
                                                     where f.Deleted == BaseStatusDeleted.None && f.Field.Id == field.Id 
                                                     select f.Value).ToList();
                        try
                        {
                            if (optionsValue.Count == 0)
                                optionValue = "1";
                            else
                            {
                                Int32 maxValue = optionsValue.Select(o => Convert.ToInt32(o)).Max() + 1;
                                optionValue = maxValue.ToString();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return optionValue;
                    }
                    public Boolean UpdateOptionsDisplayOrder(List<long> idOptions)
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        FieldDefinition field = GetFieldFromOptions(idOptions);
                        if (idOptions.Count > 0 && field != null && field.Call != null & AllowReorder(field.Call, person))
                        {
                            try
                            {
                                Manager.BeginTransaction();

                                FieldOption opt;
                                int displayOrder = 1;
                                foreach (var idOption in idOptions)
                                {
                                    opt = Manager.Get<FieldOption>(idOption);
                                    if (opt != null && field.Id == opt.Field.Id)
                                    {
                                        opt.DisplayOrder = displayOrder;
                                        opt.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(opt);
                                        displayOrder++;
                                    }
                                }
                                Manager.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                return false;
                            }
                        }
                        return false;
                    }
                    //private FieldMultipleDefinition GetFieldMultipleFromOptions(List<long> idOptions)
                    //{
                    //    List<long> idFields = (from s in Manager.GetIQ<FieldOption>() where idOptions.Contains(s.Id) select s.Field.Id).ToList();
                    //    if (idFields.Distinct<long>().ToList().Count != 1)
                    //        return null;
                    //    else
                    //        return Manager.Get<FieldMultipleDefinition>(idFields[0]);
                    //}
                    //private FieldDisclaimerDefinition GetFieldDisclaimerFromOptions(List<long> idOptions)
                    //{
                    //    List<long> idFields = (from s in Manager.GetIQ<FieldOption>() where idOptions.Contains(s.Id) select s.Field.Id).ToList();
                    //    if (idFields.Distinct<long>().ToList().Count != 1)
                    //        return null;
                    //    else
                    //        return Manager.Get<FieldDisclaimerDefinition>(idFields[0]);
                    //}
                    private FieldDefinition GetFieldFromOptions(List<long> idOptions)
                    {
                        List<long> idFields = (from s in Manager.GetIQ<FieldOption>() where idOptions.Contains(s.Id) select s.Field.Id).ToList();
                        if (idFields.Distinct<long>().ToList().Count != 1)
                            return null;
                        else
                            return Manager.Get<FieldDefinition>(idFields[0]);
                    }
                #endregion

                    public dtoGenericPermission GetPermissionForEditor(long idCall, ModuleCallForPaper module)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return GetPermissionForEditor(call, module);
                    }
                    public dtoGenericPermission GetPermissionForEditor(BaseForPaper call, ModuleCallForPaper module)
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        dtoGenericPermission permission = new dtoGenericPermission();
                        if (call != null)
                        {

                            permission.AllowDelete = (module.Administration || module.ManageCallForPapers);
                            permission.AllowEdit = (module.Administration || module.ManageCallForPapers || (call.CreatedBy == person && module.EditCallForPaper));
                            permission.AllowUnDelete = (module.Administration || module.ManageCallForPapers || (call.CreatedBy == person && module.EditCallForPaper));
                            permission.AllowVirtualDelete = (module.Administration || module.ManageCallForPapers || (call.CreatedBy == person && module.EditCallForPaper));
                        }
                        return permission;
                    }
                    public dtoGenericPermission GetPermissionForEditor(long idCall, ModuleRequestForMembership module)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return GetPermissionForEditor(call, module);
                    }
                    public dtoGenericPermission GetPermissionForEditor(BaseForPaper call, ModuleRequestForMembership module)
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        dtoGenericPermission permission = new dtoGenericPermission();
                        if (call != null)
                        {

                            permission.AllowDelete = (module.Administration || module.ManageBaseForPapers);
                            permission.AllowEdit = (module.Administration || module.ManageBaseForPapers || (call.CreatedBy == person && module.EditBaseForPaper));
                            permission.AllowUnDelete = (module.Administration || module.ManageBaseForPapers || (call.CreatedBy == person && module.EditBaseForPaper));
                            permission.AllowVirtualDelete = (module.Administration || module.ManageBaseForPapers || (call.CreatedBy == person && module.EditBaseForPaper));
                        }
                        return permission;
                    }
            #endregion

            #region "Templates"
                public Int32 GetSubmittersTemplateCount(long idCall)
                {
                    return (from t in Manager.GetIQ<SubmitterTemplateMail>() where t.Call.Id == idCall && t.Deleted == BaseStatusDeleted.None select t.Id).Count();
                }
                public SubmitterTemplateMail AddSubmitterTemplate(long idCall, dtoSubmitterTemplateMail dto)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return AddSubmitterTemplate(call, dto);
                }
                public SubmitterTemplateMail AddSubmitterTemplate(BaseForPaper call, dtoSubmitterTemplateMail dto)
                {
                    SubmitterTemplateMail result = null;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            SubmitterTemplateMail template = new SubmitterTemplateMail();
                            template.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            template.Body = dto.Body;
                            template.Call = call;
                            template.IdLanguage = dto.IdLanguage;
                            template.MailSettings = dto.MailSettings;
                            template.Name = (String.IsNullOrEmpty(dto.Name)) ? "--" : dto.Name;
                            template.Subject = dto.Subject;
                            Manager.SaveOrUpdate(template);

                            foreach (long idSubmitter in dto.SubmitterAssignments)
                            {
                                SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);
                                if (submitter != null && submitter.Deleted == BaseStatusDeleted.None)
                                {
                                    TemplateAssignment assignment = new TemplateAssignment();
                                    assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    assignment.Call = call;
                                    assignment.SubmitterType = submitter;
                                    assignment.Template = template;
                                    Manager.SaveOrUpdate(assignment);
                                }
                            }
                            Manager.Commit();
                            result = template;
                        }
                    }
                    catch (Exception ex)
                    {
                        result = null;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
                public Boolean SaveSubmittersTemplate(long idCall, List<dtoSubmitterTemplateMail> templates, List<long> inEditing)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return SaveSubmittersTemplate(call, templates, inEditing);
                }
                public Boolean SaveSubmittersTemplate(BaseForPaper call, List<dtoSubmitterTemplateMail> templates, List<long> inEditing)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>() where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id select s).ToList();
                            foreach (dtoSubmitterTemplateMail item in templates)
                            {
                                SubmitterTemplateMail template = Manager.Get<SubmitterTemplateMail>(item.Id);
                                if (template != null)
                                {
                                    template.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);

                                    if (inEditing.Contains(item.Id))
                                    {
                                        template.Body = item.Body;
                                        template.MailSettings = item.MailSettings;
                                        template.Subject = item.Subject;
                                    }

                                    template.IdLanguage = item.IdLanguage;
                                    template.Name = (String.IsNullOrEmpty(item.Name)) ? template.Name : item.Name;
                                    Manager.SaveOrUpdate(template);
                                    SaveTemplateAssignments(person, template, submitters, item.SubmitterAssignments);
                                }
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
                public Boolean SaveSubmitterTemplate(dtoSubmitterTemplateMail item)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        SubmitterTemplateMail template = Manager.Get<SubmitterTemplateMail>(item.Id);
                        if (template != null && person != null)
                        {
                            Manager.BeginTransaction();
                            List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>() where s.Deleted == BaseStatusDeleted.None && s.Call.Id == template.Call.Id select s).ToList();

                            template.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            template.Body = item.Body;
                            template.MailSettings = item.MailSettings;
                            template.Subject = item.Subject;
                            template.IdLanguage = item.IdLanguage;
                            template.Name = (String.IsNullOrEmpty(item.Name)) ? template.Name : item.Name;
                            Manager.SaveOrUpdate(template);

                            SaveTemplateAssignments(person, template, submitters, item.SubmitterAssignments);
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
                private void SaveTemplateAssignments(litePerson person, SubmitterTemplateMail template, List<SubmitterType> submitters, List<long> idSubmitters)
                {
                    foreach (long idSubmitter in idSubmitters)
                    {
                        SubmitterType submitter = submitters.Where(s => s.Id == idSubmitter).FirstOrDefault();
                        if (submitter != null)
                        {
                            TemplateAssignment assignment = (from a in Manager.GetIQ<TemplateAssignment>()
                                                                where a.Template == template && a.SubmitterType == submitter
                                                                select a).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (assignment == null && submitter != null)
                            {
                                assignment = new TemplateAssignment();
                                assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.Call = template.Call;
                                assignment.SubmitterType = submitter;
                                assignment.Template = template;
                                Manager.SaveOrUpdate(assignment);
                            }
                            else if (assignment != null && assignment.Deleted != BaseStatusDeleted.None)
                            {
                                assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(assignment);
                            }
                        }
                    }
                    List<TemplateAssignment> assignments = (from a in Manager.GetIQ<TemplateAssignment>()
                                                            where a.Template == template && idSubmitters.Contains(a.SubmitterType.Id) == false
                                                            && a.Deleted == BaseStatusDeleted.None
                                                            select a).ToList();
                    if (assignments.Count > 0)
                    {
                        assignments.ForEach(a => a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                        Manager.SaveOrUpdateList(assignments);
                    }
                }
                public Boolean VirtualDeleteSubmitterTemplate(long idTemplate, Boolean delete)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        SubmitterTemplateMail template = Manager.Get<SubmitterTemplateMail>(idTemplate);
                        if (template != null)
                        {
                            Manager.BeginTransaction();
                            template.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            template.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                            List<TemplateAssignment> assignments = (from a in Manager.GetIQ<TemplateAssignment>()
                                                                    where a.Template == template
                                                                    select a).ToList();

                            foreach (TemplateAssignment assignment in assignments)
                            {
                                assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.Deleted = delete ? (assignment.Deleted | BaseStatusDeleted.Cascade) : (assignment.Deleted = (BaseStatusDeleted)((int)assignment.Deleted - (int)BaseStatusDeleted.Cascade));
                                Manager.SaveOrUpdate(assignment);
                            }

                            Manager.SaveOrUpdate(template);
                            Manager.Commit();
                            result = true;
                        }
                        else
                            result = true;
                    }
                    catch (SubmissionLinked ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return result;
                }
                public dtoSubmitterTemplateMail GetSubmitterTemplateMail(long idTemplate)
                {
                    dtoSubmitterTemplateMail template = null;
                    try
                    {
                        Manager.BeginTransaction();
                        template = (from t in Manager.GetAll<SubmitterTemplateMail>(t => t.Id == idTemplate)
                                    select new dtoSubmitterTemplateMail()
                                    {
                                        Id = t.Id,
                                        Body = t.Body,
                                        Subject = t.Subject,
                                        IdLanguage = t.IdLanguage,
                                        MailSettings = t.MailSettings,
                                        Name = t.Name
                                    }).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (template != null)
                            template.SubmitterAssignments = (from a in Manager.GetIQ<TemplateAssignment>() where a.Deleted == BaseStatusDeleted.None && a.Template != null && a.Template.Id == template.Id select a.SubmitterType.Id).ToList();
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return template;
                }
                public List<dtoSubmitterTemplateMail> GetSubmittersTemplateMail(long idCall)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                        return GetSubmittersTemplateMail(call);
                    else
                        return new List<dtoSubmitterTemplateMail>();
                }
                public List<dtoSubmitterTemplateMail> GetSubmittersTemplateMail(BaseForPaper call)
                {
                    List<dtoSubmitterTemplateMail> templates = new List<dtoSubmitterTemplateMail>();
                    try
                    {
                        Manager.BeginTransaction();
                        templates = (from t in Manager.GetAll<SubmitterTemplateMail>(t => t.Call == call && t.Deleted == BaseStatusDeleted.None)
                                        select new dtoSubmitterTemplateMail()
                                        {
                                            Id = t.Id,
                                            Body = t.Body,
                                            Subject = t.Subject,
                                            IdLanguage = t.IdLanguage,
                                            MailSettings = t.MailSettings,
                                            Name = t.Name
                                        }).ToList();
                        templates.ForEach(t => t.SubmitterAssignments = (from a in Manager.GetIQ<TemplateAssignment>() where a.Deleted == BaseStatusDeleted.None && a.Template != null && a.Template.Id == t.Id select a.SubmitterType.Id).ToList());
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        templates = new List<dtoSubmitterTemplateMail>();
                    }
                    return templates.OrderBy(t => t.Name).ToList();
                }

                public ManagerTemplateMail SaveManagerTemplate(long idCall, dtoManagerTemplateMail item) {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                        return SaveManagerTemplate(call, item);
                    else
                        return null;
                }
                public ManagerTemplateMail SaveManagerTemplate(BaseForPaper call, dtoManagerTemplateMail item)
                {
                    ManagerTemplateMail template = null;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null) {
                            if (item.Id == 0) { 
                                ManagerTemplateMail callTemplate = (from mt in Manager.GetIQ<ManagerTemplateMail>() where mt.Deleted== BaseStatusDeleted.None && mt.Call.Id==call.Id select mt).Skip(0).Take(1).ToList().FirstOrDefault();
                                if (callTemplate!=null)
                                    item.Id=callTemplate.Id;
                            }
                            if (item.Id > 0)
                                template = Manager.Get<ManagerTemplateMail>(item.Id);
                            else
                            {
                                template = new ManagerTemplateMail();
                                template.Call = call;
                                template.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                item.NotifyFields = NotifyFields.Submitter;
                            }
                            if (template != null)
                            {
                                Manager.BeginTransaction();
                        
                                template.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                template.Body = item.Body;
                                template.MailSettings = item.MailSettings;
                                template.Subject = item.Subject;
                                template.IdLanguage = item.IdLanguage;
                                template.Name = (String.IsNullOrEmpty(item.Name)) ? template.Name : item.Name;
                                template.NotifyTo = item.NotifyTo;
                                template.NotifyFields = item.NotifyFields;

                                Manager.SaveOrUpdate(template);
                                Manager.Commit();
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        template = null;
                    }
                    return template;
                }
                public dtoManagerTemplateMail GetManagerTemplateMail(long idCall)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                        return GetManagerTemplateMail(call);
                    else
                        return null;
                }
                public dtoManagerTemplateMail GetManagerTemplateMail(BaseForPaper call)
                {
                    dtoManagerTemplateMail template = null;
                    try
                    {
                        Manager.BeginTransaction();
                        template = (from t in Manager.GetAll<ManagerTemplateMail>(t => t.Call == call && t.Deleted == BaseStatusDeleted.None)
                                    select new dtoManagerTemplateMail()
                                    {
                                        Id = t.Id,
                                        Body = t.Body,
                                        Subject = t.Subject,
                                        IdLanguage = t.IdLanguage,
                                        MailSettings = t.MailSettings,
                                        Name = t.Name,
                                        NotifyTo = t.NotifyTo,
                                        NotifyFields = t.NotifyFields
                                    }).Skip(0).Take(1).ToList().FirstOrDefault();
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return template;
                }
            #endregion

            #region "Deprecated Required file"
                public RequestedFile AddRequestedFile(long idCall, dtoCallRequestedFile dto)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return AddRequestedFile(call, dto);
                }
                public RequestedFile AddRequestedFile(BaseForPaper call, dtoCallRequestedFile dto)
                {
                    RequestedFile result = null;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();

                            List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>() where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id select s).ToList();

                            RequestedFile rFile = new RequestedFile();
                            rFile.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            rFile.Call = call;
                            rFile.Description = dto.Description;
                            rFile.Mandatory = dto.Mandatory;
                            rFile.Tooltip = dto.Tooltip;

                            Int32 displayOrder = (from c in Manager.GetIQ<RequestedFile>() where c.Deleted == BaseStatusDeleted.None && c.Call == call select c.DisplayOrder).Max() + 1;
                            Int32 count = (from c in Manager.GetIQ<RequestedFile>() where c.Deleted == BaseStatusDeleted.None && c.Call == call select c.Id).Count();
                            if (count >= displayOrder)
                            {
                                displayOrder = count + 1;
                                List<RequestedFile> items = (from c in Manager.GetIQ<RequestedFile>() where c.Deleted == BaseStatusDeleted.None && c.Call == call orderby c.DisplayOrder select c).ToList();
                                if (items.Count > 0)
                                    items.ForEach(i => i.DisplayOrder = displayOrder++);
                                Manager.SaveOrUpdateList(items);
                            }
                            rFile.DisplayOrder = displayOrder;
                            if (String.IsNullOrEmpty(dto.Name))
                                dto.Name = "{0}";
                            if (dto.Name.Contains("{0}"))
                                rFile.Name = String.Format(dto.Name, displayOrder);
                            else
                                rFile.Name = dto.Name;
                            Manager.SaveOrUpdate(rFile);

                            SaveRequestedFileAssignments(person, rFile, submitters, dto.SubmitterAssignments);
                            Manager.Commit();
                            result = rFile;
                        }
                    }
                    catch (Exception ex)
                    {
                        result = null;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
                public Boolean SaveRequestedFiles(long idCall, List<dtoCallRequestedFile> items)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return SaveRequestedFiles(call, items);
                }
                public Boolean SaveRequestedFiles(BaseForPaper call, List<dtoCallRequestedFile> items)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            Int32 displayOrder = 1;

                            List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>() where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id select s).ToList();
                            foreach (dtoCallRequestedFile dto in items)
                            {
                                RequestedFile file = Manager.Get<RequestedFile>(dto.Id);
                                if (file != null)
                                {
                                    file.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    file.Name = dto.Name;
                                    file.Description = dto.Description;
                                    file.Tooltip = dto.Tooltip;
                                    file.DisplayOrder = displayOrder;
                                    SaveRequestedFileAssignments(person, file, submitters, dto.SubmitterAssignments);

                                    Manager.SaveOrUpdate(file);
                                    displayOrder++;
                                }
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                private void SaveRequestedFileAssignments(litePerson person, RequestedFile rFile, List<SubmitterType> submitters, List<long> idSubmitters)
                {
                    foreach (long idSubmitter in idSubmitters)
                    {
                        SubmitterType submitter = submitters.Where(s => s.Id == idSubmitter).FirstOrDefault();
                        if (submitter != null)
                        {
                            RequestedFileAssignment assignment = (from a in Manager.GetIQ<RequestedFileAssignment>()
                                                             where a.RequestedFile == rFile  && a.SubmitterType == submitter
                                                             select a).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (assignment == null && submitter != null)
                            {
                                assignment = new RequestedFileAssignment();
                                assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.RequestedFile = rFile;
                                assignment.SubmitterType = submitter;
                                      Manager.SaveOrUpdate(assignment);
                            }
                            else if (assignment != null && assignment.Deleted != BaseStatusDeleted.None)
                            {
                                assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(assignment);
                            }
                        }
                    }
                    List<RequestedFileAssignment> assignments = (from a in Manager.GetIQ<RequestedFileAssignment>()
                                                                 where a.RequestedFile == rFile && idSubmitters.Contains(a.SubmitterType.Id) == false
                                                            && a.Deleted == BaseStatusDeleted.None
                                                            select a).ToList();
                    if (assignments.Count > 0)
                    {
                        assignments.ForEach(a => a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                        Manager.SaveOrUpdateList(assignments);
                    }
                }
                public Boolean VirtualDeleteCallRequestedFile(long idRequestedFile, Boolean delete)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        RequestedFile rFile = Manager.Get<RequestedFile>(idRequestedFile);
                        if (rFile != null)
                        {
                            Manager.BeginTransaction();
                            rFile.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            rFile.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                            List<RequestedFileAssignment> assignments = (from a in Manager.GetIQ<RequestedFileAssignment>()
                                                                         where a.RequestedFile == rFile
                                                                    select a).ToList();

                            foreach (RequestedFileAssignment assignment in assignments)
                            {
                                assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.Deleted = delete ? (assignment.Deleted | BaseStatusDeleted.Cascade) : (assignment.Deleted = (BaseStatusDeleted)((int)assignment.Deleted - (int)BaseStatusDeleted.Cascade));
                                Manager.SaveOrUpdate(assignment);
                            }

                            Manager.SaveOrUpdate(rFile);
                            Manager.Commit();
                            result = true;
                        }
                        else
                            result = true;
                    }
                    catch (SubmissionLinked ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return result;
                }
                public List<dtoRequestedFilePermission> GetRequestedFiles(long idCall,dtoGenericPermission permission,  Boolean getAll)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return GetRequestedFiles(call, permission, getAll);
                }
                private List<dtoRequestedFilePermission> GetRequestedFiles(BaseForPaper call, dtoGenericPermission permission, Boolean getAll)
                {
                    List<dtoRequestedFilePermission> items = null;
                    try {
                        items = (from f in Manager.GetIQ<RequestedFile>()
                                 where f.Call != null && f.Call.Id == call.Id && (getAll || f.Deleted == BaseStatusDeleted.None)
                                 select new dtoRequestedFilePermission(f.Id, new dtoCallRequestedFile() { Id = f.Id, DisplayOrder= f.DisplayOrder , Name = f.Name, Description = f.Description, Mandatory = f.Mandatory, Tooltip = f.Tooltip })).ToList();

                        items.ForEach(i=>i.File.SubmitterAssignments = (from s in Manager.GetIQ<RequestedFileAssignment>()
                                                                  where s.RequestedFile.Id == i.Id select s.SubmitterType.Id).ToList());
                        items.ForEach(i => i.UpdatePermission(permission,GetSubmittedRequestedFile(i.Id)));
                        //            )
                        //items = (from rf in Manager.GetAll<RequestedFile>(rf => rf.Call == call && (getAll || rf.Deleted == BaseStatusDeleted.None))
                        //        orderby rf.Name
                        //         select new dtoRequestedFilePermission(new dtoCallRequestedFile(rf,
                        //            (from s in Manager.GetAll<RequestedFileAssignment>(s => s.RequestedFile == rf && s.Deleted == BaseStatusDeleted.None) select s.SubmitterType.Id).ToList()
                        //            )
                        //            , permission, GetSubmittedRequestedFile(rf)
                        //            )).ToList();

                    }
                    catch (Exception ex) { 
                        items = new List<dtoRequestedFilePermission>();
                    }
                    return items.OrderBy(rf=> rf.File.DisplayOrder).ThenBy(rf=>rf.File.Name).ToList();
                }

                private long GetSubmittedRequestedFile(long idFile)
                {
                    return (from s in Manager.GetIQ<SubmittedFile>()
                            where s.SubmittedAs != null && s.SubmittedAs.Id == idFile && s.Deleted == BaseStatusDeleted.None
                                && s.Submission.Deleted == BaseStatusDeleted.None
                            select s.Id).Count();
                }
                private long GetSubmittedRequestedFile(RequestedFile requestedFile)
                {
                    return (from s in Manager.GetIQ<SubmittedFile>()
                            where s.SubmittedAs == requestedFile && s.Deleted == BaseStatusDeleted.None
                                && s.Submission.Deleted == BaseStatusDeleted.None
                            select s.Id).Count();
                }

                public Boolean UpdateRequestedFilesDisplayOrder(List<long> idRequiredFiles)
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person != null && idRequiredFiles.Count > 0 && AllowReorder(GetCallRequestedFile(idRequiredFiles), person))
                    {
                        DateTime CurrentTime = DateTime.Now;
                        RequestedFile file;
                        try
                        {
                            Manager.BeginTransaction();
                            int displayOrder = 1;
                            foreach (var idItem in idRequiredFiles)
                            {
                                file = Manager.Get<RequestedFile>(idItem);
                                if (file != null)
                                {
                                    file.DisplayOrder = displayOrder;
                                    file.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    Manager.SaveOrUpdate(file);
                                    displayOrder++;
                                }
                            }
                            Manager.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            return false;
                        }
                    }
                    return false;
                }

                private BaseForPaper GetCallRequestedFile(List<long> idRequiredFiles)
                {
                    List<long> idItems = (from s in Manager.GetIQ<RequestedFile>() where idRequiredFiles.Contains(s.Id) && s.Call != null select s.Call.Id).ToList().Distinct().ToList();
                    if (idItems.Count() != 1)
                        return null;
                    else
                        return Manager.Get<BaseForPaper>(idItems[0]);
                }
            #endregion
                protected Boolean AllowReorder(BaseForPaper call, litePerson user)
                {
                    Boolean iResponse = false;
                    switch (call.Type)
                    {
                        case CallForPaperType.CallForBids:
                            ModuleCallForPaper moduleCall = CallForPaperServicePermission(user.Id, (call.Community == null) ? 0 : call.Community.Id);
                            iResponse = (moduleCall.Administration || moduleCall.ManageCallForPapers || (moduleCall.EditCallForPaper && call.CreatedBy == user));
                            break;

                        case CallForPaperType.RequestForMembership:
                            ModuleRequestForMembership moduleMembership = RequestForMembershipServicePermission(user.Id, (call.Community == null) ? 0 : call.Community.Id);
                            iResponse = (moduleMembership.Administration || moduleMembership.ManageBaseForPapers || (moduleMembership.EditBaseForPaper && call.CreatedBy == user));
                            break;

                        default:
                            iResponse = false;
                            break;
                    }

                    return iResponse;
                }
            #region "Editing Steps"
                public List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> GetAvailableSteps(long idCall, long fromIdCall, CallForPaperType type)
                {
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>>();

                    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                    {
                        DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.first,
                        Id = (fromIdCall != 0 && idCall == 0) ? WizardCallStep.ImportUsersFrom : WizardCallStep.GeneralSettings,
                        Status = Core.Wizard.WizardItemStatus.none,
                        AutoPostBack = true,
                        Active = true
                    });
                    items.AddRange(GetStepsForCall(idCall, items[0].Id, type));
                    return items;
                }
                public List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> GetAvailableSteps(long idCall, WizardCallStep current, CallForPaperType type)
                {
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>>();

                    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                    {
                        DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.first,
                        Id = WizardCallStep.GeneralSettings,
                        Status = (idCall==0) ? Core.Wizard.WizardItemStatus.none : Core.Wizard.WizardItemStatus.valid,
                        AutoPostBack = !(idCall == 0) || (current == WizardCallStep.GeneralSettings),
                        Active = (idCall == 0) || (current== WizardCallStep.GeneralSettings)
                    });
                    items.AddRange(GetStepsForCall(idCall, current, type));
                    return items;
                }
                private List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> GetStepsForCall(long idCall, WizardCallStep current, CallForPaperType type)
                {
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>>();

                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call == null)
                    {
                        #region "NewCall"
                        if (type == CallForPaperType.RequestForMembership)
                            items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                            {
                                Id = WizardCallStep.RequestMessages,
                                Status = Core.Wizard.WizardItemStatus.disabled,
                                AutoPostBack = true,
                                Active = (current== WizardCallStep.RequestMessages)
                            });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.CallAvailability,
                            Status = Core.Wizard.WizardItemStatus.disabled,
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.CallAvailability)
                        });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.SubmittersType,
                            Status = Core.Wizard.WizardItemStatus.disabled,
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.SubmittersType)
                        });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.Attachments,
                            Status = Core.Wizard.WizardItemStatus.disabled,
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.Attachments)
                        });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.SubmissionEditor,
                            Status = Core.Wizard.WizardItemStatus.disabled,
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.SubmissionEditor)
                        });
                        if (type == CallForPaperType.RequestForMembership)
                            items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                            {
                                Id = WizardCallStep.FieldsAssociation,
                                Status = Core.Wizard.WizardItemStatus.disabled,
                                AutoPostBack = true,
                                Active = (current == WizardCallStep.FieldsAssociation)
                            });

                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.NotificationTemplateMail,
                            Status = Core.Wizard.WizardItemStatus.disabled,
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.NotificationTemplateMail)
                        });
                        
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.SubmitterTemplateMail,
                            Status = Core.Wizard.WizardItemStatus.disabled,
                            DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last,
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.SubmitterTemplateMail)
                        });
                        #endregion
                    }
                    else {
                        if (call.Type == CallForPaperType.RequestForMembership)
                            items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                            {
                                Id = WizardCallStep.RequestMessages,
                                Status = GetStepStatus(call,WizardCallStep.RequestMessages),
                                AutoPostBack = true,
                                Active = (current== WizardCallStep.RequestMessages)
                            });

                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.CallAvailability,
                            Status = GetStepStatus(call, WizardCallStep.CallAvailability),
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.CallAvailability)
                        });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.SubmittersType,
                            Status = GetStepStatus(call, WizardCallStep.SubmittersType),
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.SubmittersType)
                        });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.Attachments,
                            Status = GetStepStatus(call, WizardCallStep.Attachments),
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.Attachments)
                        });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.SubmissionEditor,
                            Status = GetStepStatus(call, WizardCallStep.SubmissionEditor),
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.SubmissionEditor)
                        });

                        if ((from r in Manager.GetIQ<RequestedFile>() where r.Call.Id == call.Id select r.Id).Any()) {
                            items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                            {
                                Id = WizardCallStep.FileToSubmit,
                                Status = GetStepStatus(call, WizardCallStep.FileToSubmit),
                                AutoPostBack = true,
                                Active = (current == WizardCallStep.FileToSubmit)
                            });
                        }
                        if (type == CallForPaperType.RequestForMembership || call.IsPublic)
                            items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                            {
                                Id = WizardCallStep.FieldsAssociation,
                                Status = GetStepStatus(call, WizardCallStep.FieldsAssociation),
                                AutoPostBack = true,
                                Active = (current == WizardCallStep.FieldsAssociation)
                            });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {
                            Id = WizardCallStep.NotificationTemplateMail,
                            Status = GetStepStatus(call, WizardCallStep.NotificationTemplateMail),
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.NotificationTemplateMail),
                        });
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>()
                        {

                            Id = WizardCallStep.SubmitterTemplateMail,
                            DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last,
                            Status = GetStepStatus(call, WizardCallStep.SubmitterTemplateMail),
                            AutoPostBack = true,
                            Active = (current == WizardCallStep.SubmitterTemplateMail)
                        });
                    }
                    return items;
                }

                public List<WizardCallStep> GetSkippedSteps(List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> wSteps,long idCall, CallForPaperType type)
                {
                    switch (type) {
                        //|| (ws.Id== WizardCallStep.SubmitterTemplateMail && mFields)
                        case CallForPaperType.RequestForMembership:
                            Boolean mFields = (from f in Manager.GetIQ<FieldDefinition>() where f.Deleted== BaseStatusDeleted.None && f.Type== FieldType.Mail && f.Call.Id== idCall select f.Id).Any();
                            return wSteps.Where(ws => ws.Status == Core.Wizard.WizardItemStatus.error || (ws.Status == Core.Wizard.WizardItemStatus.disabled && (ws.Id != WizardCallStep.SubmitterTemplateMail ))).Select(ws => ws.Id).ToList();
                        default:
                            return wSteps.Where(ws => ws.Status == Core.Wizard.WizardItemStatus.error || (ws.Status == Core.Wizard.WizardItemStatus.disabled && ws.Id != WizardCallStep.SubmitterTemplateMail)).Select(ws => ws.Id).ToList();
                    }
                }

                private Core.Wizard.WizardItemStatus GetStepStatus(BaseForPaper call, WizardCallStep step) { 
                    Core.Wizard.WizardItemStatus status = Core.Wizard.WizardItemStatus.none;
                    switch (step){
                        case WizardCallStep.CallAvailability:
                            status = CallHasAssignment(call) ? Core.Wizard.WizardItemStatus.valid : Core.Wizard.WizardItemStatus.error;
                            break;
                        case WizardCallStep.RequestMessages:
                            if (call.Type == CallForPaperType.RequestForMembership) {
                                RequestForMembership request = Manager.Get<RequestForMembership>(call.Id );
                                if (string.IsNullOrEmpty(request.StartMessage) && string.IsNullOrEmpty(request.EndMessage))
                                    status = Core.Wizard.WizardItemStatus.none;
                                else if (string.IsNullOrEmpty(request.StartMessage) || string.IsNullOrEmpty(request.EndMessage))
                                    status = Core.Wizard.WizardItemStatus.error;
                                else
                                    status = Core.Wizard.WizardItemStatus.valid;
                            }
                            else
                                status = Core.Wizard.WizardItemStatus.none;
                            break;
                        case WizardCallStep.SubmittersType:
                            if (CallHasSubmitters(call))
                                status = Core.Wizard.WizardItemStatus.valid;
                            else
                                status = Core.Wizard.WizardItemStatus.error;
                            break;
                        case WizardCallStep.Attachments:
                            status = GetAttachmentsStatus(call);
                            break;
                        case WizardCallStep.SubmissionEditor:
                            if (CallHasInvalidStructure(call))
                                status = CallHasSubmittersAssignment(call) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.error;
                            else
                                status = Core.Wizard.WizardItemStatus.none;
                            break;
                        case WizardCallStep.FileToSubmit:
                            status = GetRequiredFilesStatus(call);
                        
                            break;
                        case WizardCallStep.FieldsAssociation:
                            status = GetFieldsAssociationStatus(call);

                            break;
                        case WizardCallStep.NotificationTemplateMail:
                        case WizardCallStep.SubmitterTemplateMail:
                            status = GetTemplateStatus(call,step);
                            break;
                    }
                    return status;
                }
                protected Boolean CallHasAssignment(BaseForPaper call)
                {
                    Boolean result = false;
                    if (call.IsPublic || call.ForSubscribedUsers)
                        result = true;
                    else{
                        if (call.IsPortal)
                            result = (from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
                                      where pa.BaseForPaper == call && !pa.Deny && pa.Deleted== BaseStatusDeleted.None 
                                              select pa.Id).Any();
                        if (!result)
                             result = (from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
                                       where pa.BaseForPaper == call && !pa.Deny && pa.Deleted == BaseStatusDeleted.None 
                                              select pa.Id).Any();
                        if (!result)
                            result = (from pa in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                      where pa.BaseForPaper == call && !pa.Deny && pa.Deleted == BaseStatusDeleted.None 
                                              select pa.Id).Any();
                         if (!result)
                            result = (from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
                                      where pa.BaseForPaper == call && !pa.Deny && pa.Deleted == BaseStatusDeleted.None 
                                              select pa.Id).Any();
                    }
                    return result;
                }
                protected Boolean CallHasSubmitters(BaseForPaper call)
                {
                    Boolean result =  (from s in Manager.GetIQ<SubmitterType>()
                                      where s.Call == call &&  s.Deleted== BaseStatusDeleted.None 
                                      select s.Id).Any();
                   
                    return result;
                }
                protected Core.Wizard.WizardItemStatus GetAttachmentsStatus(BaseForPaper call)
                {
                    List<long> attachments = (from a in Manager.GetIQ<AttachmentFile>() where a.Call == call && a.Deleted == BaseStatusDeleted.None select a.Id).ToList();
                    if (attachments.Count == 0)
                        return Core.Wizard.WizardItemStatus.none;
                    else
                    {
                        if ((from a in Manager.GetIQ<AttachmentFile>() where a.Call == call && a.Deleted == BaseStatusDeleted.None && a.ForAll select a.Id).Any())
                            return Core.Wizard.WizardItemStatus.valid;
                        else
                        {
                            List<long> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                     where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                     select s.Id).ToList();
                            if (submitters.Count == 0)
                                return Core.Wizard.WizardItemStatus.none;
                            else {
                                List<long> usedSubmitters = (from att in Manager.GetIQ<AttachmentAssignment>()
                                        where att.Call == call && att.Deleted == BaseStatusDeleted.None && att.Attachment != null && !attachments.Contains(att.Attachment.Id)
                                        && att.SubmitterType != null
                                        select att.SubmitterType.Id).ToList().Distinct().ToList();

                                return (usedSubmitters.Count != submitters.Count()) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.valid;
                            }
                        }
                    }

                }
                protected Boolean CallHasInvalidStructure(BaseForPaper call)
                {
                    if (call.Sections.Where(s=>s.Deleted== BaseStatusDeleted.None).Count() == 0)
                        return true;
                    else
                    {
                        List<long> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                 where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                 select s.Id).ToList();
                        List<long> fields = (from f in Manager.GetIQ<FieldDefinition>() 
                                             where f.Call == call && f.Deleted == BaseStatusDeleted.None select f.Id).ToList();
                        List<long> usedSubmitters = (from fa in Manager.GetIQ<FieldAssignment>()
                                                     where fa.Deleted == BaseStatusDeleted.None && fa.SubmitterType != null && fa.Field != null && fields.Contains(fa.Field.Id)
                                                     select fa.SubmitterType.Id).ToList().Distinct().ToList();

                        return (fields.Count == 0 || submitters.Count == 0 || submitters.Where(s=> !usedSubmitters.Contains(s)).Any()  || (from fa in Manager.GetIQ<FieldAssignment>()
                                where fa.Deleted == BaseStatusDeleted.None && fa.SubmitterType != null && fa.Field != null && fields.Contains(fa.Field.Id) && !submitters.Contains(fa.SubmitterType.Id)
                                select fa.Id).Any());
                    }

                }
                protected Boolean CallHasSubmittersAssignment(BaseForPaper call)
                {
                    List<long> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                             where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                             select s.Id).ToList();
                    List<long> fields = (from f in Manager.GetIQ<FieldDefinition>()
                                         where f.Call == call && f.Deleted == BaseStatusDeleted.None
                                         select f.Id).ToList();

                    return ((from fa in Manager.GetIQ<FieldAssignment>()
                            where fa.Deleted == BaseStatusDeleted.None && fa.SubmitterType != null && fa.Field != null && fields.Contains(fa.Field.Id) && !submitters.Contains(fa.SubmitterType.Id)
                            select fa.Id).Any());

                }
                protected Core.Wizard.WizardItemStatus GetRequiredFilesStatus(BaseForPaper call)
                {
                    Core.Wizard.WizardItemStatus result = Core.Wizard.WizardItemStatus.none;

                    List<long> files = (from f in Manager.GetIQ<RequestedFile>() where f.Call == call && f.Deleted == BaseStatusDeleted.None select f.Id).ToList();
                    if (files.Count>0)
                    {
                        List<long> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                 where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                 select s.Id).ToList();
                        List<long> assigned = (from f in Manager.GetIQ<RequestedFileAssignment>()
                                               where files.Contains(f.RequestedFile.Id) && f.Deleted == BaseStatusDeleted.None && f.SubmitterType != null
                                               select f.SubmitterType.Id).ToList();

                        if (assigned.Count == 0)
                            result = Core.Wizard.WizardItemStatus.error;
                        else if (submitters.Except(assigned).Any()) 
                            result = Core.Wizard.WizardItemStatus.warning;
                        else
                            result = Core.Wizard.WizardItemStatus.valid;
                    }
                    return result;
                }
                public Core.Wizard.WizardItemStatus GetTemplateStatus(BaseForPaper call, WizardCallStep step)
                {
                    Core.Wizard.WizardItemStatus result = Core.Wizard.WizardItemStatus.warning;

                    if (step == WizardCallStep.NotificationTemplateMail)
                    {
                        if (!(from f in Manager.GetIQ<ManagerTemplateMail>()
                              where f.Call == call && f.Deleted == BaseStatusDeleted.None
                              select f.Id).Any())
                            result = Core.Wizard.WizardItemStatus.error;
                        else if ((from f in Manager.GetIQ<ManagerTemplateMail>()
                              where f.Call == call && f.Deleted == BaseStatusDeleted.None
                              select f.Id).Any())
                            result = Core.Wizard.WizardItemStatus.valid;
                    }
                    else {
                        if (!(from s in Manager.GetIQ<SubmitterType>() where s.Call.Id == call.Id && s.Deleted == BaseStatusDeleted.None select s.Id).Any() || (!(from f in Manager.GetIQ<FieldDefinition>() where f.Deleted== BaseStatusDeleted.None && f.Type== FieldType.Mail && f.Call.Id== call.Id select f.Id).Any())) {
                            result = Core.Wizard.WizardItemStatus.disabled;
                        }
                        else if (!(from f in Manager.GetIQ<SubmitterTemplateMail>()
                              where f.Call == call && f.Deleted == BaseStatusDeleted.None
                              select f.Id).Any())
                            result = Core.Wizard.WizardItemStatus.error;
                        else
                        {
                            List<long> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                     where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                     select s.Id).ToList();
                            List<long> assigned = (from f in Manager.GetIQ<TemplateAssignment>()
                                                   where f.Call == call && f.Deleted == BaseStatusDeleted.None && f.SubmitterType != null
                                                   select f.SubmitterType.Id).ToList();
                            result = (submitters.Except(assigned).Any()) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.valid;
                        }
                    }
               

                    return result;
                }
                protected Core.Wizard.WizardItemStatus GetFieldsAssociationStatus(BaseForPaper call)
                {
                    Core.Wizard.WizardItemStatus result = Core.Wizard.WizardItemStatus.warning;
                    if ((from f in Manager.GetIQ<FieldDefinition>()
                         where f.Call.Id == call.Id && f.Deleted== BaseStatusDeleted.None  select f.Id).Any()){

                        if (!(from f in Manager.GetIQ<ProfileAttributeAssociation>()
                              where f.Call == call && f.Deleted == BaseStatusDeleted.None
                              select f.Id).Any())
                            result = (call.Type== CallForPaperType.RequestForMembership) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.none ;
                        else
                        {
                            long association = (from s in Manager.GetIQ<ProfileAttributeAssociation>()
                                                     where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                     select s.Field.Id).Count();
                            long fields = (from s in Manager.GetIQ<FieldDefinition>()
                                                where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                select s.Id).Count();
                            result = (association == fields) ? Core.Wizard.WizardItemStatus.valid : Core.Wizard.WizardItemStatus.none;
                        }
                    }
                    else
                        result = Core.Wizard.WizardItemStatus.disabled;

                    return result;
                }
            #endregion

            #region "Skin"
                public ExternalPageContext GetExternalContext(long idCall){
                    ExternalPageContext content = new ExternalPageContext();
                    try
                    {
                        //Manager.BeginTransaction();
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        content.Skin = GetDefaultSkinSettings(call, person);
                        content.Source = CreateModuleObject(call);
                        content.ShowDocType = true;
                        //Manager.Commit();
                    }
                    catch (Exception ex) {
                        //Manager.RollBack();
                    }
                    return content;
                }
                public ExternalPageContext GetExternalContext(Int32 idCommunity, litePerson person)
                {
                    ExternalPageContext content = new ExternalPageContext();
                    try
                    {
                        //Manager.BeginTransaction();
                        liteCommunity community = Manager.Get<liteCommunity>(idCommunity);
                        content.Skin = GetDefaultSkinSettings(idCommunity, community, person);
                        content.ShowDocType = true;
                       // content.Source = CreateModuleObject(call);
                        //Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Manager.RollBack();
                    }
                    return content;
                }
                public ExternalPageContext GetUserExternalContext(long idCall, Int32 idUser)
                {
                    return GetUserExternalContext(idCall, Manager.GetLitePerson(idUser));
                }
                public ExternalPageContext GetUserExternalContext(long idCall, litePerson person)
                {
                    ExternalPageContext content = new ExternalPageContext();
                    try
                    {
                        //Manager.BeginTransaction();
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        content.Skin = GetDefaultSkinSettings(call, person);
                        content.Source = CreateModuleObject(call);
                        content.ShowDocType = true;
                        //Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Manager.RollBack();
                    }
                    return content;
                }
               
                private dtoItemSkin GetDefaultSkinSettings(BaseForPaper call,litePerson person){
                    dtoItemSkin itemSkin = new dtoItemSkin();
                    if (call!= null && !call.IsPortal && call.Community!=null){
                        itemSkin.IdCommunity = call.Community.Id;
                        itemSkin.IdOrganization = call.Community.IdOrganization;
                    }
                    itemSkin.IsForPortal = call.IsPortal;
                    if (itemSkin.IdOrganization==0 && !itemSkin.IsForPortal && person !=null ){
                        try{
                            OrganizationProfiles profileAfference = (from a in Manager.GetIQ<OrganizationProfiles>()
                                                                     where a.isDefault && a.Profile != null && a.Profile.Id ==person.Id  select a).Skip(0).Take(1).FirstOrDefault();
                            itemSkin.IdOrganization = (profileAfference!=null) ? profileAfference.OrganizationID : 0;
                        }
                        catch(Exception ex){
                        }
                    }
                    return itemSkin;
                }
                private dtoItemSkin GetDefaultSkinSettings(Int32 idCommunity, liteCommunity community, litePerson person)
                {
                    dtoItemSkin itemSkin = new dtoItemSkin();

                    itemSkin.IdCommunity = (community!= null && idCommunity >0 ) ? idCommunity :0;
                    itemSkin.IdOrganization = (community != null && idCommunity > 0) ? community.IdOrganization : 0;
                    itemSkin.IsForPortal = (community==null || idCommunity==0);
                    if (itemSkin.IdOrganization == 0 && !itemSkin.IsForPortal && person != null)
                    {
                        try
                        {
                            OrganizationProfiles profileAfference = (from a in Manager.GetIQ<OrganizationProfiles>()
                                                                     where a.isDefault && a.Profile != null && a.Profile.Id == person.Id 
                                                                     select a).Skip(0).Take(1).FirstOrDefault();
                            itemSkin.IdOrganization = (profileAfference != null) ? profileAfference.OrganizationID : 0;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    return itemSkin;
                }
        // Public Function CanEditSkin(ByVal idQuestionnnaire As Integer) As Boolean
        //    Dim result As Boolean = False

        //    Try
        //        Manager.BeginTransaction()
        //        Dim quest As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnnaire)

        //        If IsNothing(quest) Then
        //            result = True
        //        Else
        //            result = Not quest.isAnonymous OrElse (quest.isAnonymous AndAlso Not (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(idQuestionnnaire) Select r.Id).Any())
        //        End If
        //        Manager.Commit()
        //    Catch ex As Exception
        //        Manager.RollBack()
        //    End Try
        //    Return result
        //End Function
        //Public Function GetModuleObject(ByVal idQuestionnnaire As Integer) As ModuleObject
        //    Dim result As ModuleObject = New ModuleObject With {.ObjectLongID = idQuestionnnaire}

        //    Try
        //        Manager.BeginTransaction()
        //        result = GetItemModuleObject(idQuestionnnaire)
        //        Manager.Commit()
        //    Catch ex As Exception
        //        Manager.RollBack()
        //    End Try
        //    Return result
        //End Function
        //Private Function GetItemModuleObject(ByVal idQuestionnnaire As Integer) As ModuleObject
        //    Dim result As ModuleObject = New ModuleObject With {.ObjectLongID = idQuestionnnaire}
        //    Dim quest As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnnaire)
        //    If Not IsNothing(quest) Then
        //        Dim groupOwner As LazyGroup = Manager.Get(Of LazyGroup)(quest.IdGroup)

        //        If Not IsNothing(groupOwner) Then
        //            result.CommunityID = groupOwner.IdCommunity
        //        End If
        //        result.ObjectTypeID = ModuleQuestionnaire.ObjectType.Questionario
        //        result.FQN = GetType(Questionario).FullName
        //        result.ServiceID = ServiceModuleID()
        //    End If

        //    Return result
        //End Function
      
     


       
       
            #endregion

            public BaseForPaper VirtualDeleteCall(long idCall, int idPerson, Boolean delete)
            {
                try
                {
                    Manager.BeginTransaction();
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    litePerson person = Manager.GetLitePerson(idPerson);
                    if (call != null && person != null)
                    {
                        call.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        call.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(call);
                        Manager.Commit();
                        return call;
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return null; ;
            }

            public dtoCallCommunityContext GetCallCommunityContext(dtoBaseForPaper call, String portalname, Int32 idContainerCommunity = -1) {
                dtoCallCommunityContext c = new dtoCallCommunityContext();
                c.IdCommunity = 0;
                c.CallName= (call != null) ? call.Name : "";
                Boolean forPortal = (call != null) ? call.IsPortal : false;
                Community currentCommunity = Manager.GetCommunity((idContainerCommunity>0) ? idContainerCommunity : UC.CurrentCommunityID);
                Community community = null;
                if (call != null)
                    c.IdCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


                community = Manager.GetCommunity(c.IdCommunity);
                if (community != null)
                    c.CommunityName = community.Name;
                else if (currentCommunity != null && !forPortal)
                {
                    c.IdCommunity = (idContainerCommunity > 0) ? idContainerCommunity : UC.CurrentCommunityID;
                    c.CommunityName = currentCommunity.Name;
                }
                else if (forPortal)
                    c.IdCommunity = 0;
                else
                {
                    c.IdCommunity = 0;
                    c.CommunityName = portalname;
                }
                return c;
            }
            public Int32 GetCallIdCommunityContext(dtoBaseForPaper call, Int32 idContainerCommunity = -1)
            {
                Int32 idCommunity = 0;
                Boolean forPortal = (call != null) ? call.IsPortal : false;
                Community currentCommunity = Manager.GetCommunity((idContainerCommunity > 0) ? idContainerCommunity : UC.CurrentCommunityID);
                Community community = null;
                if (call != null)
                    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


                community = Manager.GetCommunity(idCommunity);
                if (community == null && currentCommunity != null && !forPortal)
                    idCommunity = (idContainerCommunity > 0) ? idContainerCommunity : UC.CurrentCommunityID;
                else if (community==null)
                    idCommunity = 0;
                return idCommunity;
            }
        #endregion

        #region "Submission"
            #region "Common"
                public UserSubmission GetUserSubmission(long submissionID)
                {
                    return Manager.Get<UserSubmission>(submissionID);
                }
                public Revision GetRevision(long idRevision)
                {
                    return Manager.Get<Revision>(idRevision);
                }
                public String GetSubmissionOwnerDisplayName(long submissionID)
                {
                    try
                    {
                        LazyUserSubmission sub = Manager.Get<LazyUserSubmission>(submissionID);
                        if (sub != null)
                            return sub.Owner.SurnameAndName;
                        else
                            return "";
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }
            #endregion
            #region "View"
                public List<dtoAttachmentFile> GetAvailableCallAttachments(long idCall, long idSubmitter)
                {
                    List<dtoAttachmentFile> attachments = null;
                    try
                    {
                        attachments = (from af in Manager.GetAll<AttachmentFile>(a => a.Call != null && a.Call.Id == idCall && a.Deleted == BaseStatusDeleted.None && a.ForAll)
                                        select new dtoAttachmentFile()
                                        {
                                            Id = af.Id,
                                            Deleted = af.Deleted,
                                            Description = af.Description,
                                            DisplayOrder = af.DisplayOrder,
                                            Link = af.Link,
                                            ForAll = true
                                        }).ToList();
                        if (idSubmitter > 0)
                        {
                            List<long> assigned = (from asgn in Manager.GetIQ<AttachmentAssignment>()
                                                    where asgn.Deleted == BaseStatusDeleted.None && asgn.SubmitterType != null && asgn.SubmitterType.Id == idSubmitter
                                                    select asgn.Attachment.Id).ToList();
                            attachments.AddRange(
                                (from a in Manager.GetAll<AttachmentFile>(a => assigned.Contains(a.Id) && a.Deleted == BaseStatusDeleted.None)
                                    select new dtoAttachmentFile()
                                    {
                                        Id = a.Id,
                                        Deleted = a.Deleted,
                                        Description = a.Description,
                                        DisplayOrder = a.DisplayOrder,
                                        Link = a.Link,
                                        ForAll = false
                                    }).ToList()
                                );
                        }
                    }
                    catch (Exception ex)
                    {
                        attachments = new List<dtoAttachmentFile>();
                    }

                    return attachments.OrderBy(s => s.DisplayOrder).ToList();
                }
                public List<dtoCallSection<dtoSubmissionValueField>> GetSubmissionFields(BaseForPaper call, long idSubmitter)
                {
                    SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);
                    if (call != null && submitter != null)
                    {
                        try
                        {
                            List<dtoCallSection<dtoSubmissionValueField>> sections = (from s in Manager.GetIQ<FieldsSection>()
                                                                                      where s.Deleted == BaseStatusDeleted.None && s.Call == call
                                                                                      select new dtoCallSection<dtoSubmissionValueField>()
                                                                                      {
                                                                                          Id = s.Id,
                                                                                          Name = s.Name,
                                                                                          Description = s.Description,
                                                                                          DisplayOrder = s.DisplayOrder
                                                                                      }).ToList();
                            List<long> idFields = (from a in Manager.GetIQ<FieldAssignment>() where a.Deleted == BaseStatusDeleted.None && a.SubmitterType == submitter select a.Field.Id).ToList();
                            List<dtoSubmissionValueField> fields = (from f in Manager.GetIQ<FieldDefinition>()
                                                                    where f.Call == call && idFields.Contains(f.Id)
                                                             && f.Deleted == BaseStatusDeleted.None
                                                                    select f).ToList().Select(f => new dtoSubmissionValueField(f)).ToList();
                            foreach (dtoCallSection<dtoSubmissionValueField> section in sections)
                            {
                                section.Fields = (from f in fields where f.Field.IdSection == section.Id orderby f.Field.DisplayOrder, f.Field.Name select f).ToList();
                            }
                            // REMOVED GET SECTIONS ONLY WITH FIELDS
                            return sections.Where(s => s.Fields.Count > 0).OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                            //return sections.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                        }
                        catch (Exception ex)
                        {
                            return new List<dtoCallSection<dtoSubmissionValueField>>();
                        }
                    }
                    else
                        return new List<dtoCallSection<dtoSubmissionValueField>>();
                }
                public List<dtoCallSubmissionFile> GetRequiredFiles(long idCall, long idSubmitter){
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return GetRequiredFiles(call,idSubmitter);
                }
                public List<dtoCallSubmissionFile> GetRequiredFiles(BaseForPaper call, long idSubmitter)
                {
                    SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);
                    if (call != null && submitter != null)
                    {
                        List<long> idFiles = (from a in Manager.GetIQ<RequestedFileAssignment>() where a.Deleted == BaseStatusDeleted.None && a.SubmitterType == submitter select a.RequestedFile.Id).ToList();

                        return (from s in Manager.GetAll<RequestedFile>(s=>s.Deleted == BaseStatusDeleted.None && idFiles.Contains(s.Id) && s.Call == call)
                                select new dtoCallSubmissionFile(s,false,true)).ToList();
                    }
                    else
                        return new List<dtoCallSubmissionFile>();
                }
               
            #endregion

            #region "Submit"
                #region "1 - LoadItems"
                    public List<dtoCallSection<dtoSubmissionValueField>> GetSubmissionFields(Revision revision) {
                        if (revision != null && revision.Submission != null && revision.Submission.Type != null && revision.Submission.Call != null)
                            return GetSubmissionFields(revision.Submission.Call, revision.Submission.Type.Id, revision.Submission.Id, revision.Id);
                        else
                            return new List<dtoCallSection<dtoSubmissionValueField>>();
                
                    }
                    public List<dtoCallSection<dtoSubmissionValueField>> GetSubmissionFields(
                        BaseForPaper call, 
                        long idSubmitterType, 
                        long idSubmission, 
                        long idRevision)
                    {
                        SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitterType);
                        if (call != null && submitter != null )
                        {
                            try
                            {
                                UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);
                                List<dtoCallSection<dtoSubmissionValueField>> sections = (from s in Manager.GetIQ<FieldsSection>()
                                                                                          where s.Deleted == BaseStatusDeleted.None && s.Call == call
                                                                                          select new dtoCallSection<dtoSubmissionValueField>()
                                                                                          {
                                                                                              Id = s.Id,
                                                                                              Name = s.Name,
                                                                                              Description = s.Description,
                                                                                              DisplayOrder = s.DisplayOrder
                                                                                          }).ToList();

                                List<long> idFields = (from a in Manager.GetIQ<FieldAssignment>() where a.Deleted == BaseStatusDeleted.None 
                                                       && a.SubmitterType != null 
                                                       && a.SubmitterType.Id == submitter.Id
                                                       select a.Field.Id).ToList();

                                List<dtoSubmissionValueField> fields = (from f in Manager.GetIQ<FieldDefinition>()
                                                                        where f.Call != null && f.Call.Id == call.Id && idFields.Contains(f.Id) && f.Deleted == BaseStatusDeleted.None 
                                                                        select f).ToList().Select(f => new dtoSubmissionValueField(f, (from vf in Manager.GetAll<SubmissionFieldBaseValue>(vf => vf.Field.Id == f.Id && vf.Submission.Id == idSubmission && vf.Revision.Id == idRevision && vf.Deleted == BaseStatusDeleted.None) select vf).FirstOrDefault(), GetRevisionCount(submission, idRevision, f))).ToList();

                                foreach (dtoCallSection<dtoSubmissionValueField> section in sections)
                                {
                                    section.Fields = (from f in fields where f.Field.IdSection == section.Id orderby f.Field.DisplayOrder, f.Field.Name select f).ToList();
                                }
                                 //REMOVED GET SECTIONS ONLY WITH FIELDS
                                return sections.Where(s => s.Fields.Count > 0).OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                                //return sections.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                            }
                            catch (Exception ex)
                            {
                                return new List<dtoCallSection<dtoSubmissionValueField>>();
                            }
                        }
                        else
                            return new List<dtoCallSection<dtoSubmissionValueField>>();
                    }
                    public List<dtoCallSubmissionFile> GetRequiredFiles(BaseForPaper call, long idSubmitter, long idSubmission)
                    {
                        List<dtoCallSubmissionFile> files = new List<dtoCallSubmissionFile>();
                        SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);
                        UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);
                        if (call != null && submitter != null)
                        {
                            List<long> idFiles = (from a in Manager.GetIQ<RequestedFileAssignment>() where a.Deleted == BaseStatusDeleted.None && a.RequestedFile != null && a.SubmitterType.Id == submitter.Id select a.RequestedFile.Id).ToList();

                            files = (from s in Manager.GetAll<RequestedFile>(s => s.Deleted == BaseStatusDeleted.None && idFiles.Contains(s.Id) && s.Call == call)
                                     select new dtoCallSubmissionFile(s, false, true)).ToList();
                            if (submission != null)
                            {
                                Boolean allowUpload = (submission.Status == SubmissionStatus.draft || submission.Status == SubmissionStatus.none);
                                files.ForEach(f => f.SetSubmittedFile((from sf in Manager.GetAll<SubmittedFile>(sf => sf.Deleted == BaseStatusDeleted.None && sf.SubmittedAs.Id == f.FileToSubmit.Id && sf.Submission == submission) select sf).Skip(0).Take(1).ToList().FirstOrDefault(), allowUpload, allowUpload));
                            }
                        }
                        return files;
                    }
                    public Boolean CallWithFileToUpload(long idCall, long idSubmitter)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return CallWithFileToUpload(call, idSubmitter);
                    }
                    public Boolean CallWithFileToUpload(BaseForPaper call, long idSubmitter)
                    {
                        Boolean result = false;
                        if (idSubmitter != 0)
                        {
                            SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);

                            result = (from f in Manager.GetIQ<RequestedFileAssignment>()
                                      where f.Deleted == BaseStatusDeleted.None && f.SubmitterType == submitter
                                      select f.Id).Any();
                            if (!result)
                            {
                                List<long> idFields = (from f in Manager.GetIQ<FieldDefinition>()
                                                       where f.Deleted == BaseStatusDeleted.None && f.Call.Id == call.Id && f.Type == FieldType.FileInput
                                                       select f.Id).ToList();
                                result = (from f in Manager.GetIQ<FieldAssignment>()
                                          where f.Deleted == BaseStatusDeleted.None && f.SubmitterType == submitter && idFields.Contains(f.Field.Id)
                                          select f.Id).Any();
                            }
                        }
                        return result;
                    }
                    public Boolean CallWithRequestedFiles(BaseForPaper call, long idSubmitter)
                    {
                        Boolean result = false;
                        if (idSubmitter != 0){ 
                            SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);

                            result = (from f in Manager.GetIQ<RequestedFileAssignment>()
                                               where f.Deleted == BaseStatusDeleted.None && f.SubmitterType == submitter
                                               select f.Id).Any();
                        }
                        return result;
                    }
                    public Boolean SubmissionWithUploadedFile(long idSubmission, long idRevision)
                    {
                        Boolean result = false;
                        if (idSubmission != 0)
                        {
                            try
                            {
                                result = (from v in Manager.GetIQ<SubmissionFieldFileValue>()
                                          where v.Submission.Id == idSubmission && v.Deleted == BaseStatusDeleted.None && v.Revision.Id == idRevision && v.Item != null
                                          select v).Any();
                                if (!result)
                                    result = (from v in Manager.GetIQ<SubmittedFile>()
                                              where v.Submission.Id == idSubmission && v.Deleted == BaseStatusDeleted.None && v.File != null
                                              select v).Any();
                            }
                            catch (Exception ex) { 
                            
                            }
                          }
                        return result;
                    }

                    public dtoSubmissionRevision GetActiveUserSubmission(long idCall, int idPerson, long idSubmission)
                    {
                        dtoSubmissionRevision dto = null;
                        try
                        {
                            UserSubmission sub = (from s in Manager.GetIQ<UserSubmission>()
                                   where (idSubmission == 0 || (idSubmission > 0 && s.Id == idSubmission)) && s.Call.Id == idCall && s.Deleted == BaseStatusDeleted.None && s.Owner != null && s.Owner.Id == idPerson
                                   select s).Skip(0).Take(1).ToList().FirstOrDefault();
                            dto = GetSubmissionWithRevisions(sub, false);
                        }
                        catch (Exception ex)
                        {
                            dto = null;
                        }
                        return dto;
                    }

                    public dtoSubmissionRevision GetActiveUserSubmission(long idCall, int idPerson, long idSubmission, System.Guid uniqueId)
                    {
                        dtoSubmissionRevision dto = null;
                        try
                        {
                            UserSubmission sub = (from s in Manager.GetIQ<UserSubmission>()
                                                  where (idSubmission > 0 && s.Id == idSubmission) && s.UserCode == uniqueId  && s.Call.Id == idCall && s.Deleted == BaseStatusDeleted.None && s.Owner != null && s.Owner.Id == idPerson
                                                  select s).Skip(0).Take(1).ToList().FirstOrDefault();
                            dto = GetSubmissionWithRevisions(sub, false);
                        }
                        catch (Exception ex)
                        {
                            dto = null;
                        }
                        return dto;
                    }
                    public dtoLazySubmission GetSubmission(long idCall, long idSubmission, System.Guid uniqueId)
                    {
                        dtoLazySubmission dto = null;
                        try
                        {
                            dto = (from s in Manager.GetIQ<UserSubmission>()
                                   where (idSubmission > 0 && s.Id == idSubmission) && s.UserCode == uniqueId && s.Call.Id == idCall && s.Deleted == BaseStatusDeleted.None && s.Owner !=null
                                   select new dtoLazySubmission()
                                   {
                                       Deleted = s.Deleted,
                                       Id = s.Id,
                                       Status = s.Status,
                                       IdPerson = s.Owner.Id,
                                       IdCall = idCall,
                                       ExtensionDate = s.ExtensionDate,
                                       Type = new dtoSubmitterType() { Id = s.Type.Id },
                                       IsAnonymous = s.isAnonymous,
                                       UniqueId = s.UserCode
                                   }
                                    ).Skip(0).Take(1).ToList().FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (dto != null)
                        {
                            dto.Owner = Manager.Get<litePerson>(dto.IdPerson);
                            dto.Type = (from s in Manager.GetIQ<SubmitterType>()
                                        where s.Id == dto.Type.Id
                                        select new dtoSubmitterType() { Deleted = s.Deleted, Name = s.Name, Id = s.Id }).Skip(0).Take(1).ToList().FirstOrDefault();

                        }
                        return dto;
                    }
                    public Boolean  ExistSubmission(long idSubmission)
                    {
                        return ExistSubmission(idSubmission, Guid.Empty);
                    }
                    private Boolean ExistSubmission(System.Guid uniqueId)
                    {
                        return ExistSubmission(0, uniqueId);
                    }
                    private Boolean ExistSubmission(long idSubmission, System.Guid uniqueId)
                    {
                        Boolean result = false;
                        try
                        {
                            result = (from s in Manager.GetIQ<UserSubmission>()
                                      where (idSubmission > 0 && s.Id == idSubmission) || (uniqueId != Guid.Empty && s.UserCode == uniqueId)
                                      select s.Id).Any();
                        }
                        catch (Exception ex)
                        {

                        }
                        return result;
                    }
                    public long  GetSubmissionCountBySubmitter(long idCall, int idPerson, long idSubmitter)
                    {
                        return (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Owner.Id == idPerson && s.Type.Id == idSubmitter select s.Id).Count();
                    }


                    public UserSubmission SubmissionGet(Int64 SubmissionId)
                    {
                        return Manager.Get<UserSubmission>(SubmissionId);
                    }
                #endregion
                
                #region "2 - Save items"
                    public UserSubmission SaveSubmission(long idSubmission,ref long idRevision, long idCall, long idSubmitter, int idOwner, List<dtoSubmissionValueField> fields)
                    {
                        litePerson person = null;
                        if (idOwner == 0)
                            person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                        else
                            person = Manager.GetLitePerson(idOwner);
                        return SaveSubmission(person, idSubmission, ref idRevision, idCall, idSubmitter, fields);
                    }
                    public UserSubmission SaveAnonymousSubmission(long idSubmission, ref long idRevision, long idCall, long idSubmitter, List<dtoSubmissionValueField> fields)
                    {
                        litePerson person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                        return SaveSubmission(person, idSubmission, ref idRevision, idCall, idSubmitter, fields);
                    }
                    private UserSubmission SaveSubmission(litePerson submitter, long idSubmission, ref long idRevision, long idCall, long idSubmitter, List<dtoSubmissionValueField> fields)
                    {
                        UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);

                        if (submission == null && idSubmission > 0)
                            throw new SubmissionNotFound(idSubmission.ToString());
                        else
                        {
                            try
                            {
                                Manager.BeginTransaction();
                                BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                                if (call != null && call.Status != CallForPaperStatus.Draft)
                                {
                                    if (submission == null)
                                    {
                                        submission = new UserSubmission();
                                        submission.CreateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);
                                        submission.Call = Manager.Get<BaseForPaper>(idCall);
                                        submission.Community = submission.Call.Community;
                                        submission.Type = Manager.Get<SubmitterType>(idSubmitter);
                                        submission.Owner = submitter;
                                        submission.isAnonymous = (submitter.TypeID == (int) UserTypeStandard.Guest) ||
                                                                 (submitter.TypeID == (int) UserTypeStandard.PublicUser);
                                        submission.UserCode = Guid.NewGuid();
                                        submission.isComplete = false;
                                    }
                                    else
                                    {
                                        submission.UpdateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);
                                        //Nota: SE risulta cancellata in fase di sottomissione, la rimetto a normale!
                                        submission.Deleted = BaseStatusDeleted.None;
                                    }
                                        

                                    List<SubmissionFieldStringValue> values = new List<SubmissionFieldStringValue>();
                                    if (submission.Type != null)
                                    {
                                        Manager.SaveOrUpdate(submission);
                                        Manager.Refresh(submission);

                                        Revision revision = Manager.Get<Revision>(idRevision);
                                        if (revision != null && revision.Submission != null && revision.Submission.Id != revision.Submission.Id)
                                            throw new Exception();
                                        else
                                        {
                                            if (idSubmission == 0 || (revision == null) && !submission.Revisions.Where(r => r.Type == RevisionType.Original).Any())
                                            {
                                                revision = new OriginalRevision();
                                                revision.CreateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);
                                                revision.IsActive = true;
                                                revision.Submission = submission;
                                                revision.Status = RevisionStatus.Approved;
                                                revision.Number = 0;
                                            }
                                            else if (revision == null)
                                            {
                                                if (submission.Revisions.Count == 1)
                                                    revision = submission.Revisions[0];
                                                else
                                                    throw new Exception();
                                            }
                                            else
                                                revision.UpdateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);
                                            Manager.SaveOrUpdate(revision);
                                            idRevision = revision.Id;

                                            if (fields.Any())
                                            {
                                                foreach (dtoSubmissionValueField item in fields)//.Where(f => f.Field.Type != FieldType.FileInput).ToList())
                                                {
                                                    if (item.Field.Type != FieldType.FileInput)
                                                    {
                                                        SubmissionFieldStringValue fieldValue =
                                                            Manager.Get<SubmissionFieldStringValue>(item.IdValueField);
                                                        if (fieldValue == null)
                                                        {
                                                            FieldDefinition definition =
                                                                Manager.Get<FieldDefinition>(item.IdField);
                                                            if (definition.Call == submission.Call)
                                                            {
                                                                fieldValue = new SubmissionFieldStringValue();
                                                                fieldValue.CreateMetaInfo(submitter, UC.IpAddress,
                                                                    UC.ProxyIpAddress);
                                                                fieldValue.Submission = submission;
                                                                fieldValue.Revision = revision;
                                                                fieldValue.Field = definition;
                                                                fieldValue.Value = item.Value.Text;
                                                                fieldValue.UserValue = item.Value.FreeText;
                                                                values.Add(fieldValue);
                                                            }
                                                        }
                                                        else if (fieldValue.Field.Call == submission.Call)
                                                        {
                                                            fieldValue.UpdateMetaInfo(submitter, UC.IpAddress,
                                                                UC.ProxyIpAddress);
                                                            fieldValue.Value = item.Value.Text;
                                                            fieldValue.UserValue = item.Value.FreeText;
                                                            values.Add(fieldValue);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SubmissionFieldStringValue fieldValue =
                                                            Manager.Get<SubmissionFieldStringValue>(item.IdValueField);

                                                        if (fieldValue != null &&
                                                            fieldValue.Field.Call == submission.Call)
                                                        {
                                                            fieldValue.Value = item.Value.Text;
                                                            values.Add(fieldValue);
                                                        }

                                                    }
                                                }
                                                if (values.Any())
                                                {
                                                    Manager.SaveOrUpdateList(values);
                                                    //   submission.FieldsValues = values;
                                                    //   Manager.SaveOrUpdate(submission);
                                                }
                                            }
                                        }
                                        if (revision != null && !submission.Revisions.Where(r => r.Id == revision.Id).Any())
                                            submission.Revisions.Add(revision);

                                    }
                                }
                                else
                                    throw new SubmissionCallUnavailable();
                                Manager.Commit();
                                //var queryUpdate = (from fv in values where !(from FieldValue av in submission.FieldsValues select av.Id).ToList().Contains(fv.Id) select fv);
                                //foreach (FieldValue fv in queryUpdate.ToList())
                                //{
                                //    submission.FieldsValues.Add(fv);
                                //}
                            }
                            catch (SubmissionCallUnavailable ex)
                            {
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                if (idSubmission > 0)
                                    throw new SubmissionNotSaved(ex.Message, ex);
                                else
                                {
                                    idRevision = 0;
                                    throw new SubmissionNotCreated(ex.Message, ex);
                                }
                            }

                        }
                        return submission;
                    }
                    public void SaveSubmissionFiles(UserSubmission submission, Revision revision, List<dtoRequestedFileUpload> requiredFiles, List<dtoSubmissionFileValueField> fileValues)
                    {
                        litePerson person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                        SaveSubmissionFiles(submission,revision, person, requiredFiles, fileValues);
                    }
                    public void SaveSubmissionFiles(UserSubmission submission, Revision revision, int idOwner, List<dtoRequestedFileUpload> requiredFiles, List<dtoSubmissionFileValueField> fileValues)
                    {
                        litePerson person = null;
                        if (idOwner == 0 && submission.isAnonymous)
                            person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                        else
                            person = Manager.GetLitePerson(idOwner);
                        SaveSubmissionFiles(submission, revision, person, requiredFiles, fileValues);
                    }
                    public void SaveSubmissionFiles(UserSubmission submission, Revision revision, litePerson owner, List<dtoRequestedFileUpload> requiredFiles, List<dtoSubmissionFileValueField> fileValues)
                    {
                        try
                        {
                            if (owner != null && submission != null && revision !=null )
                            {

                                Manager.BeginTransaction();
                                foreach (dtoRequestedFileUpload item in requiredFiles)
                                {
                                    SubmittedFile ToSubmit = new SubmittedFile();
                                    ToSubmit.CreateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    ToSubmit.Submission = submission;
                                    ToSubmit.SubmittedAs = Manager.Get<RequestedFile>(item.IdRequired);
                                    ToSubmit.File = (lm.Comol.Core.FileRepository.Domain.liteRepositoryItem)item.ActionLink.ModuleObject.ObjectOwner;
                                    SubmittedFile submitted = Manager.GetAll<SubmittedFile>(s => s.Submission == submission && s.SubmittedAs.Id == item.IdRequired && s.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                                    if (submitted != null)
                                    {
                                        submitted.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                        submitted.File.Deleted= BaseStatusDeleted.Manual;
                                        Manager.SaveOrUpdate(submitted.File);
                                        submitted.Link.Deleted = BaseStatusDeleted.Automatic;
                                        submitted.Deleted = BaseStatusDeleted.Manual;
                                        Manager.SaveOrUpdate(submitted);
                                    }
                                    Manager.SaveOrUpdate(ToSubmit);
                                    liteModuleLink link = new liteModuleLink(item.ActionLink.Description, item.ActionLink.Permission, item.ActionLink.Action);
                                    link.CreateMetaInfo(owner.Id, UC.IpAddress, UC.ProxyIpAddress);
                                    link.DestinationItem = (ModuleObject)item.ActionLink.ModuleObject;
                                    link.SourceItem = CreateModuleObject(submission);
                                    Manager.SaveOrUpdate(link);
                                    ToSubmit.Link = link;
                                    Manager.SaveOrUpdate(ToSubmit);
                                }

                                foreach (dtoSubmissionFileValueField item in fileValues)
                                {
                                    SubmissionFieldFileValue toCreate = new SubmissionFieldFileValue();
                                    toCreate.CreateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    toCreate.Submission = submission;
                                    toCreate.Revision = revision;
                                    toCreate.Field = Manager.Get<FieldDefinition>(item.IdField);
                                    toCreate.Item = (lm.Comol.Core.FileRepository.Domain.liteRepositoryItem)item.ActionLink.ModuleObject.ObjectOwner;
                                    

                                    SubmissionFieldFileValue fileValue = (from f in Manager.GetIQ<SubmissionFieldFileValue>()
                                                                          where f.Submission == submission && f.Deleted == BaseStatusDeleted.None && f.Field.Id == item.IdField
                                                                          && f.Revision.Id == revision.Id
                                                                          select f).Skip(0).Take(1).ToList().FirstOrDefault();
                                    if (fileValue != null)
                                    {
                                        fileValue.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                        fileValue.Item.Deleted = BaseStatusDeleted.Manual;
                                        Manager.SaveOrUpdate(fileValue.Item);
                                        fileValue.Link.Deleted = BaseStatusDeleted.Automatic;
                                        fileValue.Deleted = BaseStatusDeleted.Manual;
                                        fileValue.Revision = revision;
                                        Manager.SaveOrUpdate(fileValue);
                                    }

                                    Manager.SaveOrUpdate(toCreate);
                                    liteModuleLink link = new liteModuleLink(item.ActionLink.Description, item.ActionLink.Permission, item.ActionLink.Action);
                                    link.CreateMetaInfo(owner.Id, UC.IpAddress, UC.ProxyIpAddress);
                                    link.DestinationItem = (ModuleObject)item.ActionLink.ModuleObject;
                                    link.SourceItem = CreateModuleObject(submission);
                                    Manager.SaveOrUpdate(link);
                                    toCreate.Link = link;
                                    Manager.SaveOrUpdate(toCreate);
                                }
                                Manager.Commit();
                            }
                            else if (owner == null)
                                throw new SubmissionInternalFileNotLinked("");
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            throw new SubmissionInternalFileNotLinked(ex.Message, ex);
                        }
                    }

                    public void RemoveSubmittedFile(long idSubmittedFile)
                    {
                        try
                        {
                            Int32 idUser = UC.CurrentUserID;
                            litePerson person = null;
                            if (idUser == 0)
                                person = (from p in Manager.GetIQ<litePerson>()
                                          where p.TypeID == (int)UserTypeStandard.Guest
                                          select p).Skip(0).Take(1).ToList().FirstOrDefault();
                            else
                                person = Manager.GetLitePerson(idUser);
                            SubmittedFile submittedFile = Manager.Get<SubmittedFile>(idSubmittedFile);
                            if (submittedFile != null && person != null && (person.TypeID != (int)UserTypeStandard.Guest || submittedFile.Submission.isAnonymous))
                            {
                                Manager.BeginTransaction();
                                submittedFile.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                submittedFile.Deleted |= BaseStatusDeleted.Manual;
                                Manager.SaveOrUpdate(submittedFile);
                                Manager.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                    }
                    public void RemoveFieldFileValue(long idSubmittedField)
                    {
                        try
                        {
                            Int32 idUser = UC.CurrentUserID;
                            litePerson person = null;
                            if (idUser == 0)
                                person = (from p in Manager.GetIQ<litePerson>()
                                          where p.TypeID == (int)UserTypeStandard.Guest
                                          select p).Skip(0).Take(1).ToList().FirstOrDefault();
                            else
                                person = Manager.GetLitePerson(idUser);
                            SubmissionFieldFileValue fileValue = Manager.Get<SubmissionFieldFileValue>(idSubmittedField);
                            if (fileValue != null && person != null && (person.TypeID != (int)UserTypeStandard.Guest || fileValue.Submission.isAnonymous))
                            {
                                Manager.BeginTransaction();
                                fileValue.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                fileValue.Item.Deleted = BaseStatusDeleted.Manual;
                                Manager.SaveOrUpdate(fileValue.Item);
                                fileValue.Link.Deleted = BaseStatusDeleted.Automatic;
                                fileValue.Deleted = BaseStatusDeleted.Manual;
                                Manager.SaveOrUpdate(fileValue);
                                Manager.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                    }
                    public void VirtualDeleteSubmission(long idSubmission, Boolean delete)
                    {
                        try
                        {
                            Manager.BeginTransaction();
                            UserSubmission sub = Manager.Get<UserSubmission>(idSubmission);
                            Int32 idUser = UC.CurrentUserID;
                            litePerson person = null;
                            if (idUser == 0)
                                person = (from p in Manager.GetIQ<litePerson>()
                                          where p.TypeID == (int)UserTypeStandard.Guest
                                          select p).Skip(0).Take(1).ToList().FirstOrDefault();
                            else
                                person = Manager.GetLitePerson(idUser);
                            if (sub != null && person != null && (person.TypeID != (int)UserTypeStandard.Guest || sub.isAnonymous))
                            {
                                sub.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                sub.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(sub);
                               
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }

                    }
                #endregion

                #region "2b - lock Submission for Signature"
                public void UserLockSubmission(UserSubmission submission, Revision revision, DateTime initTime, int idUser, String BaseFilePath, List<dtoSubmissionValueField> fields, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, String websiteUrl, Dictionary<SubmissionTranslations, string> translations, String translationFileName,
                                DateTime clickDt)
                {
                    litePerson person = Manager.GetLitePerson(idUser);
                    UserLockSubmission(submission, revision, initTime, person, BaseFilePath, fields, smtpConfig, websiteUrl, translations, translationFileName, clickDt);
                }
                #endregion

                #region "3 - Complete items"
                    public void UserCompleteSubmission(
                        UserSubmission submission, 
                        Revision revision, 
                        DateTime initTime, 
                        int idUser, 
                        String BaseFilePath, 
                        List<dtoSubmissionValueField> fields, 
                        lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, 
                        String websiteUrl, 
                        Dictionary<SubmissionTranslations, string> translations,
                        String translationFileName,
                        DateTime clickDt)
                    {
                        litePerson person = Manager.GetLitePerson(idUser);
                        UserCompleteSubmission(submission, revision, initTime, person, BaseFilePath, fields, smtpConfig, websiteUrl, translations, translationFileName, clickDt);
                    }
                    public void UserCompleteAnonymousSubmission(
                        UserSubmission submission, 
                        Revision revision, 
                        DateTime initTime, 
                        String BaseFilePath, 
                        List<dtoSubmissionValueField> fields, 
                        lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, 
                        String websiteUrl, 
                        Dictionary<SubmissionTranslations, string> translations, 
                        String translationFileName,
                        DateTime clickDt)
                    {
                        UserCompleteSubmission(submission, revision, initTime, submission.Owner, BaseFilePath, fields, smtpConfig, websiteUrl, translations, translationFileName, clickDt);
                    }
                    private void UserCompleteSubmission(
                        UserSubmission submission, 
                        Revision revision, 
                        DateTime initTime, 
                        litePerson owner, 
                        String BaseFilePath, 
                        List<dtoSubmissionValueField> fields, 
                        lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, 
                        String websiteUrl, 
                        Dictionary<SubmissionTranslations, string> translations,
                        String translationFileName,
                        DateTime clickDt)
                    {
                        if (!submission.Call.AllowLateSubmission(initTime, submission, clickDt) || !AllowTimeRevision(revision, initTime))
                        {
                            throw new SubmissionTimeExpired();
                        }
                        else
                        {
                            if (AllowSubmissionComplete(submission, revision))
                            {
                                List<string> filesToRemove = new List<string>();
                                try
                                {
                                    Manager.BeginTransaction();
                                    Manager.Refresh(submission);
                                    submission.Status = SubmissionStatus.submitted;
                                    submission.SubmittedOn = DateTime.Now;
                                    submission.SubmittedBy = owner;
                                    submission.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    revision.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    revision.ModifiedOn = submission.SubmittedOn;
                                    filesToRemove = ZipSubmissionFiles(submission,revision, owner, BaseFilePath, translationFileName);

                                    submission.isComplete = true;
                                    Manager.SaveOrUpdate(revision);
                                    Manager.SaveOrUpdate(submission);
                                    Manager.Commit();
                                    SubmissionMailNotification(submission, owner, fields, smtpConfig, websiteUrl,translations);
                                    Delete.Files(filesToRemove);
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                    throw new SubmissionStatusChange(ex.Message, ex);
                                }
                            }
                            else
                                throw new SubmissionItemsEmpty();
                        }
                    }

        //sign

        public bool SignSubmissionIsNotExpired(Int64 submissionId, Int64 revisionId, DateTime initTime, DateTime clickDt)
        {

            UserSubmission submission = Manager.Get<UserSubmission>(submissionId);
            Revision revision = Manager.Get<Revision>(revisionId);

            return SignSubmissionIsNotExpired(submission, revision, initTime, clickDt);
        }

        public bool SignSubmissionIsNotExpired(UserSubmission submission, Revision revision, DateTime initTime, DateTime clickDt)
        {
            if (submission == null || submission.Call == null)
                return false;

            if (!((submission.SubmittedBy != null && submission.SubmittedBy.Id == UC.CurrentUserID) ||
                (submission.CreatedBy != null && submission.CreatedBy.Id == UC.CurrentUserID)))
                return false;

            if (((int)submission.Status >= (int)SubmissionStatus.accepted) && submission.Status != SubmissionStatus.waitforsignature)
                return true;


            bool canSubmit =
                submission.Call.AllowLateSubmission(initTime, submission, clickDt) ||
                    !AllowTimeRevision(revision, initTime);

            return canSubmit;
        }


        public bool UserCompleteSubmissionSign(Int64 submissionId,
            Int64 revisionId,
            DateTime initTime,
            int ownerId,
            //String BaseFilePath,
            List<dtoSubmissionValueField> fields,
            lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig,
            String websiteUrl,
            Dictionary<SubmissionTranslations, string> translations,
            DateTime clickDt
            )
        {
            litePerson owner = Manager.GetLitePerson(ownerId);
            Revision revision = Manager.Get<Revision>(revisionId);
            UserSubmission submission = Manager.Get<UserSubmission>(submissionId);


            if (submission == null
                || submission.Id <= 0
                || revision == null || revision.Id <= 0)
                throw new SubmissionNotFound();

            //if (!submission.Call.AllowLateSubmission(initTime, submission, clickDt) ||
            //    !AllowTimeRevision(revision, initTime))

            //if (!SignSubmissionIsNotExpired(submission, revision, initTime, clickDt))
            //{
            //    return false;
            //}
            //else
            //{
            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                submission.SubmittedOn = clickDt;//DateTime.Now;
                submission.SubmittedBy = owner;
                submission.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);

                if ((int)submission.Status <= (int)SubmissionStatus.waitforsignature)
                    submission.Status = SubmissionStatus.submitted;
                                
                revision.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                revision.ModifiedOn = submission.SubmittedOn;

                Manager.SaveOrUpdate<UserSubmission>(submission);
                Manager.SaveOrUpdate<Revision>(revision);

                Manager.Commit();

                SubmissionMailNotification(submission, owner, fields, smtpConfig, websiteUrl, translations);

            }
            catch (Exception)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
            }


            //}
            return true;
        }



                    private void UserLockSubmission(
                                   UserSubmission submission,
                                   Revision revision,
                                   DateTime initTime, litePerson owner,
                                   String BaseFilePath,
                                   List<dtoSubmissionValueField> fields,
                                   lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig,
                                   String websiteUrl,
                                   Dictionary<SubmissionTranslations, string> translations,
                                   String translationFileName,
                                   DateTime clickDt)
                    {
                        if (!submission.Call.AllowLateSubmission(initTime, submission, clickDt) || !AllowTimeRevision(revision, initTime))
                        {
                            throw new SubmissionTimeExpired();
                        }
                        else
                        {
                            if (AllowSubmissionComplete(submission, revision))
                            {
                                List<string> filesToRemove = new List<string>();
                                try
                                {
                                    Manager.BeginTransaction();
                                    Manager.Refresh(submission);
                                    submission.Status = SubmissionStatus.waitforsignature;
                                    submission.SubmittedOn = clickDt;//DateTime.Now;
                                    submission.SubmittedBy = owner;
                                    submission.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    revision.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    revision.ModifiedOn = submission.SubmittedOn;
                                    filesToRemove = ZipSubmissionFiles(submission, revision, owner, BaseFilePath, translationFileName);

                                    submission.isComplete = true;
                                    Manager.SaveOrUpdate(revision);
                                    Manager.SaveOrUpdate(submission);
                                    Manager.Commit();
                                    //SubmissionMailNotification(submission, owner, fields, smtpConfig, websiteUrl, translations);
                                    Delete.Files(filesToRemove);
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                    throw new SubmissionStatusChange(ex.Message, ex);
                                }
                            }
                            else
                                throw new SubmissionItemsEmpty();
                        }
                    }

        //end sign



                    private Boolean AllowTimeRevision(Revision revision, DateTime initTime) {
                        if (revision != null && (revision.Type == RevisionType.Original || revision.Type == RevisionType.None))
                            return true;
                        else if (revision != null && revision is RevisionRequest)
                            return (revision != null && (((RevisionRequest)revision).AllowSubmission(initTime)) && revision.AllowSave);
                        else
                            return false;
                    }
                    public Boolean AllowSubmissionComplete(UserSubmission submission, Revision revision)
                    {
                        Boolean allow = false;
                        if (submission != null)
                        {
                            allow = (GetSubmissionFieldErrors(submission, revision).Count == 0);
                            if (allow)
                                allow = (GetSubmissionRequiredFileErrors(submission).Count == 0);
                        }
                        return allow;
                    }
                    public Dictionary<long, FieldError> GetSubmissionFieldErrors(long idSubmission, long idRevision)
                    {
                        UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);
                        Revision revision = Manager.Get<Revision>(idRevision);
                        return GetSubmissionFieldErrors(submission, revision);
                    }
                    public Dictionary<long, FieldError> GetSubmissionFieldErrors(UserSubmission submission, long idRevision)
                    {
                        Revision revision = Manager.Get<Revision>(idRevision);
                        return GetSubmissionFieldErrors(submission, revision);
                    }
                    public Dictionary<long, FieldError> GetSubmissionFieldErrors(UserSubmission submission, Revision revision)
                    {
                        Dictionary<long, FieldError> items = new Dictionary<long, FieldError>();
                        if (submission != null && revision !=null )
                        {
                            List<FieldAssignment> assignments = (from rf in Manager.GetIQ<FieldAssignment>()
                                                                 where rf.SubmitterType == submission.Type && rf.Deleted == BaseStatusDeleted.None
                                                                 select rf).ToList();
                            List<FieldDefinition> fields = (from a in assignments where a.Field.Deleted == BaseStatusDeleted.None select a.Field).ToList();
                            List<SubmissionFieldBaseValue> values = (from v in Manager.GetIQ<SubmissionFieldBaseValue>()
                                                                     where v.Submission == submission && v.Revision.Id == revision.Id && v.Deleted == BaseStatusDeleted.None
                                                                     select v).ToList();

                            foreach (FieldDefinition field in fields)
                            {
                                if (field.Mandatory && !values.Where(v => v.Field.Id == field.Id).Any())
                                    items.Add(field.Id, FieldError.Mandatory);
                                else
                                {
                                    switch (field.Type) { 
                                        case FieldType.DropDownList:
                                        case FieldType.RadioButtonList:
                                              SubmissionFieldStringValue fieldValuerb = values.Where(v => v.Field.Id == field.Id).Select(v => (SubmissionFieldStringValue)v).FirstOrDefault();
                                            //Se obbligatorio: verifica che ci sia almeno una selezione.
                                            //HA SENSO per la scelta multipla?!

                                             if (fieldValuerb == null && field.Mandatory)
                                                items.Add(field.Id, FieldError.Mandatory);

                                            //NON HA SENSO e crea errori!

                                            //else if (fieldValuerb != null && !String.IsNullOrEmpty(fieldValuerb.Value))
                                            //{
                                            //    List<String> mValue = fieldValuerb.Value.Split('|').ToList();
                                            //    if (mValue.Count < field.MinOption)
                                            //        items.Add(field.Id, FieldError.LessOptions);

                                            //    else if (mValue.Count > field.MaxOption)
                                            //        items.Add(field.Id, FieldError.MoreOptions);

                                            //    //else if (String.IsNullOrEmpty(fieldValue.UserValue) && field.Options.Where(o => o.Deleted == BaseStatusDeleted.None && o.IsFreeValue && mValue.Contains(o.Id.ToString())).Any())
                                            //    //    items.Add(field.Id, FieldError.UserValueMissing);
                                            //}
                                            //else if (field.MinOption > 0 && field.Type != FieldType.RadioButtonList)
                                            //    items.Add(field.Id, FieldError.LessOptions);
                                            break;

                                        case FieldType.CheckboxList:

                                            SubmissionFieldStringValue fieldValue = values.Where(v => v.Field.Id == field.Id).Select(v => (SubmissionFieldStringValue)v).FirstOrDefault();

                                            
                                            //Ho dei valori
                                            if (fieldValue != null && !String.IsNullOrEmpty(fieldValue.Value))
                                            {
                                                List<String> mValue = fieldValue.Value.Split('|').ToList();
                                                
                                                    if (mValue.Count < field.MinOption)
                                                        items.Add(field.Id, FieldError.LessOptions);

                                                    else if (mValue.Count > field.MaxOption)
                                                        items.Add(field.Id, FieldError.MoreOptions);
                                            }
                                            else if (field.Mandatory)
                                                items.Add(field.Id, FieldError.LessOptions);
                                            break;

                                        case FieldType.Disclaimer:
                                            SubmissionFieldStringValue dValue = values.Where(v => v.Field.Id == field.Id).Select(v => (SubmissionFieldStringValue)v).FirstOrDefault();
                                            switch (field.DisclaimerType)
                                            {
                                                case DisclaimerType.Standard:
                                                    if ((dValue == null || String.IsNullOrEmpty(dValue.Value)) && field.Mandatory)
                                                        items.Add(field.Id, FieldError.Mandatory);
                                                    break;
                                                case DisclaimerType.CustomDisplayOnly:
                                                    break;
                                                case DisclaimerType.CustomMultiOptions:
                                                case DisclaimerType.CustomSingleOption:
                                                    if (dValue == null && field.Mandatory)
                                                        items.Add(field.Id, FieldError.Mandatory);
                                                    else if (dValue != null && !String.IsNullOrEmpty(dValue.Value))
                                                    {
                                                        List<String> mValue = dValue.Value.Split('|').ToList();
                                                        if (mValue.Count < field.MinOption)
                                                            items.Add(field.Id, FieldError.LessOptions);
                                                        else if (mValue.Count > field.MaxOption)
                                                            items.Add(field.Id, FieldError.MoreOptions);
                                                        //else if (String.IsNullOrEmpty(dValue.UserValue) && field.Options.Where(o => o.Deleted == BaseStatusDeleted.None && o.IsFreeValue && mValue.Contains(o.Id.ToString())).Any())
                                                        //    items.Add(field.Id, FieldError.UserValueMissing);
                                                    }
                                                    else if (field.MinOption > 0)
                                                        items.Add(field.Id, FieldError.LessOptions);
                                                    break;
                                            }
                                            break;
                                        case FieldType.FileInput:
                                            SubmissionFieldFileValue fileValue = values.Where(v => v.Field.Id == field.Id).Select(v => (SubmissionFieldFileValue)v).FirstOrDefault();
                                            if ((fileValue == null || fileValue.Link == null) && field.Mandatory)
                                                items.Add(field.Id, FieldError.Mandatory);
                                            break;

                                        case FieldType.TableSimple:
                                            if(field.Mandatory)
                                            { 
                                                SubmissionFieldStringValue tValue = values.Where(v => v.Field.Id == field.Id).Select(v => (SubmissionFieldStringValue)v).FirstOrDefault();

                                                if (tValue == null || String.IsNullOrEmpty(tValue.Value))
                                                    items.Add(field.Id, FieldError.Mandatory);
                                                else
                                                {
                                                    string content = tValue.Value;
                                                    XmlDocument doc = new XmlDocument();
                                                    content =
                                                        String.Format(
                                                            "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>{0}{1}",
                                                            System.Environment.NewLine, content);

                                                    doc.LoadXml(content);

                                                    bool hasvalue = false;
                                                    foreach (XmlNode tdNode in doc.GetElementsByTagName("td"))
                                                    {
                                                        if (!String.IsNullOrEmpty(tdNode.InnerText))
                                                        {
                                                            hasvalue = true;
                                                            break;
                                                        }
                                                    }

                                                    if (!hasvalue)
                                                    {
                                                        items.Add(field.Id, FieldError.Mandatory);
                                                    }
                                                }
                                            }

                                            break;

                                        case FieldType.TableReport:

                                            bool tr_hasvalue = false;

                                            SubmissionFieldStringValue trValue = values.Where(v => v.Field.Id == field.Id).Select(v => (SubmissionFieldStringValue)v).FirstOrDefault();

                                            if (!(trValue == null || String.IsNullOrEmpty(trValue.Value)))
                                            {
                                                string content = trValue.Value;
                                                XmlDocument doc = new XmlDocument();
                                                content =
                                                    String.Format(
                                                        "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>{0}{1}",
                                                        System.Environment.NewLine, content);

                                                doc.LoadXml(content);

                                                
                                                foreach (XmlNode tdNode in doc.GetElementsByTagName("td"))
                                                {
                                                    if (tdNode.Attributes != null &&
                                                        tdNode.Attributes["class"] != null)
                                                    {
                                                        double val = 0;
                                                        try
                                                        {
                                                            double.TryParse(tdNode.InnerText, out val);
                                                        }
                                                        catch (Exception) {}

                                                        if (val > 0)
                                                        {
                                                            tr_hasvalue = true;
                                                            break;
                                                        }

                                                    } else if (!String.IsNullOrEmpty(tdNode.InnerText))
                                                    {
                                                        tr_hasvalue = true;
                                                        break;
                                                    }
                                                }

                                                FieldError tblError = FieldError.None;

                                                if (field.Mandatory)
                                                {
                                                    if (!tr_hasvalue)
                                                    {
                                                        tblError |= FieldError.Mandatory;   //add
                                                        tblError &= ~FieldError.None;       //remove

                                                        //tblError = tblError & FieldError.Mandatory;
                                                    }    
                                                }

                                                if (CallTableHelper.IsOverRange(trValue))
                                                {
                                                    tblError |= FieldError.TableReportOverValue;   //add
                                                    tblError &= ~FieldError.None;       //remove
                                                }

                                                if(tblError != FieldError.None)
                                                    items.Add(field.Id, tblError);


                                            }

                                            break;
                                        case FieldType.Note:
                                            break;

                                        default:
                                            SubmissionFieldStringValue genericValue = values.Where(v => v.Field.Id == field.Id).Select(v => (SubmissionFieldStringValue)v).FirstOrDefault();
                                            if ((genericValue == null || String.IsNullOrEmpty(genericValue.Value)) &&
                                                field.Mandatory)
                                            {
                                                items.Add(field.Id, FieldError.Mandatory);
                                                string type = field.Type.ToString();
                                            }
                                                
                                            break;
                                    }
                                }

                            }
                        }
                        return items;
                    }
                    public Dictionary<long, FieldError> GetSubmissionRequiredFileErrors(long idSubmission) {
                        UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);
                        return GetSubmissionRequiredFileErrors(submission);
                    }
                    public Dictionary<long, FieldError> GetSubmissionRequiredFileErrors(UserSubmission submission)
                    {
                        Dictionary<long, FieldError> items = new Dictionary<long, FieldError>();
                        if (submission != null)
                        {
                            List<long> idFiles = (from f in Manager.GetIQ<SubmittedFile>()
                                                  where f.Submission == submission && f.Deleted == BaseStatusDeleted.None && f.SubmittedAs.Deleted == BaseStatusDeleted.None
                                                  select f.SubmittedAs.Id).ToList();
                            if (idFiles.Count > 0)
                                (from rf in Manager.GetIQ<RequestedFileAssignment>()
                                 where rf.SubmitterType == submission.Type && rf.Deleted == BaseStatusDeleted.None && rf.RequestedFile.Deleted == BaseStatusDeleted.None
                                 && rf.RequestedFile.Mandatory && !idFiles.Contains(rf.RequestedFile.Id)
                                 select rf.RequestedFile.Id).ToList().ForEach(f => items.Add(f, FieldError.Mandatory));
                        }
                        return items;
                    }
                    public Boolean ValidateStatus(BaseForPaper call, CallForPaperStatus status)
                    {
                        Boolean response = true;
                        if (call != null)
                        {
                            DateTime today = DateTime.Today;
                            switch (status)
                            {
                                case CallForPaperStatus.Draft:
                                    response = !(CallHasSubmissions(call));
                                    break;
                                case CallForPaperStatus.SubmissionsLimitReached:
                                    response = CallHasSubmissions(call);
                                    break;
                                case CallForPaperStatus.SubmissionClosed:
                                    response = (!call.EndDate.HasValue || (today > call.EndDate) || call.Status == CallForPaperStatus.SubmissionClosed);
                                    break;
                                case CallForPaperStatus.SubmissionOpened:
                                    response = (!call.EndDate.HasValue || today <= call.EndDate);
                                    break;
                                case CallForPaperStatus.Published:
                                    response = !((today >= call.StartDate) && (call.Status != CallForPaperStatus.Published || call.Status != CallForPaperStatus.Draft));
                                    break;
                            }
                        }
                        return response;
                    }
                    public CallForPaperStatus GetValidCallForPaperStatus(dtoCallForPaper callForPaper)
                    {
                        CallForPaperStatus status = CallForPaperStatus.Draft;
                        if (callForPaper != null)
                        {
                            DateTime today = DateTime.Today;
                            if ((callForPaper.EndDate.HasValue && today > callForPaper.EndDate)
                                && callForPaper.Status != CallForPaperStatus.Draft && callForPaper.Status != CallForPaperStatus.SubmissionClosed)
                            {
                                status = CallForPaperStatus.SubmissionClosed;
                            }
                            else if ((today >= callForPaper.StartDate &&
                                (!callForPaper.EndDate.HasValue || today <= callForPaper.EndDate)
                                && callForPaper.Status != CallForPaperStatus.Draft && callForPaper.Status != CallForPaperStatus.SubmissionOpened))
                                status = CallForPaperStatus.SubmissionOpened;
                            else
                                status = callForPaper.Status;
                        }
                        return status;
                    }
                #endregion
                   
                #region"4 - Zip File"
                     //<summary>
                     //    Zip file and return files to delete
                     //</summary>
                     //<param name="submission"></param>
                     //<param name="user"></param>
                     //<param name="BaseFilePath"></param>
                     //<returns></returns>
                    private List<string> ZipSubmissionFiles(UserSubmission submission, Revision revision, litePerson user, String baseFilePath, String translationFileName)
                    {
                        List<string> filesToRemove = new List<string>();

                        if (revision.LinkZip != null)
                        {
                            Manager.DeletePhysical(revision.LinkZip);
                            revision.LinkZip = null;
                        }
                        if (revision.FileZip != null)
                        {
                            filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, revision.FileZip));
                            Manager.DeletePhysical(revision.FileZip);
                            revision.FileZip = null;
                        }
                        Manager.SaveOrUpdate(revision);
                        Manager.SaveOrUpdate(submission);
                        List<SubmittedFile> requiredFiles = new List<SubmittedFile>();
                        if (submission.Files.Any())
                            requiredFiles = submission.Files.Where(f=>f.Deleted== BaseStatusDeleted.None).ToList();
                        List<SubmissionFieldFileValue> fieldFiles = (from f in Manager.GetIQ<SubmissionFieldFileValue>() where f.Submission.Id == submission.Id && f.Revision.Id ==revision.Id  && f.Deleted == BaseStatusDeleted.None select f).ToList();

                        int count = requiredFiles.Count + fieldFiles.Count;
                        if (count > 0)
                        {
                            lm.Comol.Core.FileRepository.Domain.RepositoryItem internalFile = CreateInternalFile(user, submission, ExportFileType.zip, translationFileName);
                            int index = 1;
                            List<dtoFileToZip> filesToZip = (from f in requiredFiles
                                                             where f.Deleted == BaseStatusDeleted.None
                                                             orderby f.SubmittedAs.Name
                                                             select new dtoFileToZip(RepositoryService.GetItemDiskFullPath(baseFilePath, f.File),
                                                                 f.File.DownloadFullName, ref index, count)).ToList();
                            filesToZip.AddRange(
                                (from f in fieldFiles
                                 where f.Deleted == BaseStatusDeleted.None
                                 orderby f.Field.DisplayOrder
                                 select new dtoFileToZip(RepositoryService.GetItemDiskFullPath(baseFilePath, f.Item),
                                     f.Item.DownloadFullName, ref index, count)).ToList()
                                );
                            String fileZip = RepositoryService.GetItemDiskFullPath(baseFilePath, internalFile);
                            lm.Comol.Core.File.Zip.ZipFilesRename(filesToZip, fileZip);
                            lm.Comol.Core.File.dtoFileSystemInfo info = lm.Comol.Core.File.ContentOf.File_dtoInfo(fileZip);

                            if (info != null)
                            {
                                internalFile.Size = info.Length;
                                Manager.SaveOrUpdate(internalFile);
                                lm.Comol.Core.FileRepository.Domain.RepositoryItemVersion version = internalFile.CreateFirstVersion();
                                Manager.SaveOrUpdate(version);
                                internalFile.IdVersion = version.Id;
                                Manager.SaveOrUpdate(internalFile);
                                lm.Comol.Core.FileRepository.Domain.liteRepositoryItem file = Manager.Get<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>(internalFile.Id);
                                liteModuleLink link = new liteModuleLink("", (Int32)lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.DownloadOrPlay, (int)lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DownloadFile);
                                link.CreateMetaInfo(user.Id, UC.IpAddress, UC.ProxyIpAddress);
                                link.DestinationItem = ModuleObject.CreateLongObject(file.Id, 0, file, (Int32)lm.Comol.Core.FileRepository.Domain.ModuleRepository.GetObjectType(file.Type), (Int32)internalFile.Repository.IdCommunity, lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode, GetIdRepositoryModule());
                                link.SourceItem = CreateModuleObject(submission);
                                Manager.SaveOrUpdate(link);
                                revision.LinkZip = link;
                                revision.FileZip = file;
                                Manager.SaveOrUpdate(revision);
                            }
                        }

                        return filesToRemove;
                    }
                    //public void UpdateSubmissionZipFile(UserSubmission submission, String baseFilePath, String translationFileName)
                    //{
                    //    IList<string> filesToRemove = new List<string>();
                    //    try
                    //    {
                    //        Manager.BeginTransaction();
                    //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    //        if (person != null)
                    //        {
                    //            filesToRemove = ZipSubmissionFiles(submission, person, baseFilePath, translationFileName);
                    //            Manager.SaveOrUpdate(submission);
                    //        }
                    //        Manager.Commit();
                    //        Delete.Files(filesToRemove);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Manager.RollBack();
                    //        throw new SubmissionStatusChange(ex.Message, ex);
                    //    }
                    //}
                    private lm.Comol.Core.FileRepository.Domain.RepositoryItem CreateInternalFile(litePerson person, UserSubmission submission, ExportFileType type, String fileName)
                    {
                        lm.Comol.Core.FileRepository.Domain.RepositoryItem file = new lm.Comol.Core.FileRepository.Domain.RepositoryItem();
                        file.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                        file.Availability = Core.FileRepository.Domain.ItemAvailability.available;
                        file.CloneOf = 0;
                        file.CloneOfUniqueId = Guid.Empty;
                        file.Extension = "." + type.ToString();
                        switch(type){
                            case ExportFileType.zip:
                                file.ContentType = "application/x-zip-compressed";
                                break;
                            case ExportFileType.pdf:
                                file.ContentType = "text/pdf";
                                break;
                            //case ExportFileType.rtf:
                            //    file.ContentType = "text/rtf";
                            //    break;
                            case ExportFileType.xls:
                                file.ContentType = "Application/x-msexcel";
                                break;
                            default:
                                file.ContentType = "application/x-zip-compressed";
                                file.Extension = ".zip";
                                file.Name = "SubmittedFiles";
                                break;
                        }
                        file.Description = "";
                        file.DisplayMode = Core.FileRepository.Domain.DisplayMode.downloadOrPlay;
                        file.Downloaded = 0;
                        file.IdFolder = 0;
                        file.IdOwner = person.Id;
                        file.IdPlayer = 0;
                        file.IdVersion = 0;
                        file.IsDownloadable = true;
                        file.IsFile = true;
                        file.IsInternal = true;
                        file.IsVisible = true;
                        file.Name = fileName;
                        file.Size = 0;
                        file.Status = Core.FileRepository.Domain.ItemStatus.Active;
                        file.Thumbnail = "";
                        file.Type = Core.FileRepository.Domain.ItemType.File;
                        file.UniqueId = Guid.NewGuid();
                        file.UniqueIdVersion = file.UniqueId;
                        file.Url = "";
                        file.Repository = (submission.Call != null ? CreateRepositoryIdentifier(submission.Call) : null);
                        if (file.Repository != null)
                            file.IdCommunity= file.Repository.IdCommunity;
                        file.Module = new Core.FileRepository.Domain.ItemModuleSettings();
                        file.Module.IdObject = submission.Id;
                        switch(submission.Call.Type){
                            case CallForPaperType.CallForBids:
                                file.Module.FullyQualifiedName = submission.GetType().FullName;
                                file.Module.IdModuleAction = 0;
                                file.Module.IdModuleAjaxAction = (Int32)ModuleCallForPaper.ActionType.DownloadSubmittedFile;;
                                file.Module.IdObjectType=(Int32)ModuleCallForPaper.ObjectType.UserSubmission;
                                file.Module.ModuleCode= ModuleCallForPaper.UniqueCode;
                                break;
                            case CallForPaperType.RequestForMembership:
                                file.Module.FullyQualifiedName = submission.GetType().FullName;
                                file.Module.IdModuleAction = 0;
                                file.Module.IdModuleAjaxAction = (Int32)ModuleRequestForMembership.ActionType.DownloadSubmittedFile;
                                file.Module.IdObjectType=(Int32)ModuleRequestForMembership.ObjectType.UserSubmission;
                                file.Module.ModuleCode=ModuleRequestForMembership.UniqueCode;
                                break;
                        }
                        return file;
                    }
                #endregion

                #region "5 - Mail Notification"
                    public void SubmissionMailNotification(
                        UserSubmission submission, 
                        litePerson submitter, 
                        List<dtoSubmissionValueField> fields, 
                        lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, 
                        String websiteUrl, 
                        Dictionary<SubmissionTranslations, string> translations)
                    {
                        bool hasError = false;
                        string error = "";

                        try
                        {
                            if (submission.Call != null)
                            {
                                Int32 idUserLanguage =
                                    (submitter.TypeID == (int) UserTypeStandard.Guest ||
                                     submitter.TypeID == (int) UserTypeStandard.PublicUser)
                                        ? 0
                                        : submitter.LanguageID;

                                Language dLanguage = Manager.GetDefaultLanguage();
                                List<dtoManagerTemplateMail> templates = (from t in Manager.GetIQ<ManagerTemplateMail>()
                                    where t.Deleted == BaseStatusDeleted.None
                                          && t.Call != null
                                          && t.Call.Id == submission.Call.Id
                                    select t).ToList()
                                    .Select(t => new dtoManagerTemplateMail()
                                    {
                                        Id = t.Id,
                                        Body = t.Body,
                                        Deleted = t.Deleted,
                                        IdLanguage = t.IdLanguage,
                                        Subject = t.Subject,
                                        MailSettings = t.MailSettings,
                                        NotifyFields = t.NotifyFields,
                                        NotifyTo = t.NotifyTo
                                    }).ToList();

                                foreach (
                                    dtoManagerTemplateMail template in
                                        templates.Where(t => !String.IsNullOrEmpty(t.NotifyTo)).ToList())
                                {
                                    lm.Comol.Core.Mail.MailService mailService =
                                        new lm.Comol.Core.Mail.MailService(smtpConfig, template.MailSettings);
                                    lm.Comol.Core.Mail.dtoMailMessage message =
                                        new lm.Comol.Core.Mail.dtoMailMessage(
                                            AnalyzeContent(
                                                submission.Call,
                                                template.MailSettings.IsBodyHtml,
                                                template.Subject,
                                                submission,
                                                submitter,
                                                fields,
                                                websiteUrl,
                                                translations),
                                            AnalyzeContent(submission.Call,
                                                template.MailSettings.IsBodyHtml,
                                                template.Body,
                                                submission,
                                                submitter,
                                                fields,
                                                websiteUrl,
                                                translations)
                                            );

                                    message.FromUser = smtpConfig.GetSystemSender();

                                    mailService.SendMail(
                                        idUserLanguage,
                                        dLanguage,
                                        message,
                                        template.NotifyTo,
                                        lm.Comol.Core.MailCommons.Domain.RecipientType.To);
                                }

                                dtoSubmitterTemplateMail submitterTemplate = GetSubmitterTemplateMail(submission,
                                    submitter);
                                if (submitterTemplate != null)
                                {
                                    String mailField = (from dtoSubmissionValueField f in fields
                                        where
                                            f.Field.Type == FieldType.Mail && f.Value != null &&
                                            !String.IsNullOrEmpty(f.Value.Text)
                                        select f.Value.Text).FirstOrDefault();
                                    if (submission.Owner != null &&
                                        (submission.Owner.TypeID != (int) UserTypeStandard.PublicUser &&
                                         submission.Owner.TypeID != (int) UserTypeStandard.Guest))
                                        mailField = submission.Owner.Mail;

                                    if (!string.IsNullOrEmpty(mailField))
                                    {
                                        lm.Comol.Core.Mail.MailService mailService =
                                            new lm.Comol.Core.Mail.MailService(smtpConfig,
                                                submitterTemplate.MailSettings);
                                        lm.Comol.Core.Mail.dtoMailMessage message =
                                            new lm.Comol.Core.Mail.dtoMailMessage(
                                                AnalyzeContent(submission.Call,
                                                    submitterTemplate.MailSettings.IsBodyHtml, submitterTemplate.Subject,
                                                    submission, submitter, fields, websiteUrl, translations),
                                                AnalyzeContent(submission.Call,
                                                    submitterTemplate.MailSettings.IsBodyHtml, submitterTemplate.Body,
                                                    submission, submitter, fields, websiteUrl, translations));
                                        message.FromUser = smtpConfig.GetSystemSender();
                                        mailService.SendMail(idUserLanguage, dLanguage, message, mailField,
                                            lm.Comol.Core.MailCommons.Domain.RecipientType.To);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            hasError = true;
                            error = ex.ToString();
                        }

                        if (hasError)
                        {
                            try
                            {
                                dtoSubmitterTemplateMail submitterTemplate = GetSubmitterTemplateMail(submission,
                                   submitter);

                                 Int32 idUserLanguage =
                                    (submitter.TypeID == (int) UserTypeStandard.Guest ||
                                     submitter.TypeID == (int) UserTypeStandard.PublicUser)
                                        ? 0
                                        : submitter.LanguageID;

                                Language dLanguage = Manager.GetDefaultLanguage();

                                lm.Comol.Core.Mail.dtoMailMessage errmessage =
                                    new lm.Comol.Core.Mail.dtoMailMessage("ERROR", error);

                                lm.Comol.Core.Mail.MailService mailService =
                                            new lm.Comol.Core.Mail.MailService(smtpConfig,
                                                submitterTemplate.MailSettings);

                                mailService.SendMail(idUserLanguage, dLanguage, errmessage, "mborsato@edutech.it",
                                            lm.Comol.Core.MailCommons.Domain.RecipientType.To);
                            }
                            catch (Exception)
                            {
                                
                                //throw;
                            }
                            
                        }
                        
                    }
                    public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(String openTag, String closeTag, Boolean isHtml, string webSiteurl, long idCall, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, litePerson fakeSubmitter, Dictionary<SubmissionTranslations, string> translations)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        litePerson currentUser = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && currentUser != null)
                        {
                            SubmitterType sType = call.SubmittersType.Where(s => s.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                            content = GetTemplateContentPreview(false, openTag, closeTag, isHtml, content, webSiteurl,call, null, fakeSubmitter, sType, currentUser, translations);
                        }
                        return content;
                    }
                    public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(String openTag, String closeTag, Boolean isHtml, string webSiteurl, BaseForPaper call, List<dtoSubmissionValueField> fieldValues, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, litePerson fakeSubmitter, Dictionary<SubmissionTranslations, string> translations)
                    {
                        litePerson currentUser = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && currentUser != null)
                        {
                            SubmitterType sType = call.SubmittersType.Where(s => s.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                            content = GetTemplateContentPreview(true,openTag, closeTag, isHtml, content,webSiteurl, call,fieldValues, fakeSubmitter, sType, currentUser, translations);
                        }
                        return content;
                    }
                    private lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(Boolean isFakeCall, String openTag, String closeTag, Boolean isHtml, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, string webSiteurl, BaseForPaper call, List<dtoSubmissionValueField> fieldValues, litePerson fakeSubmitter, SubmitterType sType, litePerson currentUser, Dictionary<SubmissionTranslations, string> translations)
                    {
                        UserSubmission fakeSubmission = GetFakeSubmission(call, fakeSubmitter, sType);
                        List<dtoCallSection<dtoSubmissionValueField>> sections = null;
                        if (sType != null && !isFakeCall)
                            sections = GetSubmissionFields(call, sType.Id, 0, 0);
                        else if (!isFakeCall)
                            sections = new List<dtoCallSection<dtoSubmissionValueField>>();
                        else
                            sections = GetFakeSections(call, fakeSubmission, fieldValues);
                        if (!String.IsNullOrEmpty(content.Subject) && content.Subject.Contains(openTag) && content.Subject.Contains(closeTag))
                            content.Subject = AnalyzeContent(call,isHtml, content.Subject, fakeSubmission, fakeSubmitter, sections.SelectMany(s => s.Fields).ToList(), webSiteurl, translations);
                        if (!String.IsNullOrEmpty(content.Body) && content.Body.Contains(openTag) && content.Body.Contains(closeTag))
                            content.Body = AnalyzeContent(call, isHtml, content.Body, fakeSubmission, fakeSubmitter, sections.SelectMany(s => s.Fields).ToList(), webSiteurl, translations);
                        if (!String.IsNullOrEmpty(content.ShortText) && content.ShortText.Contains(openTag) && content.ShortText.Contains(closeTag))
                            content.ShortText = AnalyzeContent(call, isHtml, content.ShortText, fakeSubmission, fakeSubmitter, sections.SelectMany(s => s.Fields).ToList(), webSiteurl, translations);

                        return content;
                    }

                    private List<dtoCallSection<dtoSubmissionValueField>> GetFakeSections(BaseForPaper call, UserSubmission fakeSubmission, List<dtoSubmissionValueField> fieldValues)
                    {
                        if (call != null )
                        {
                            try
                            {
                                List<dtoCallSection<dtoSubmissionValueField>> sections = call.Sections.Select(s=>new dtoCallSection<dtoSubmissionValueField>()
                                                                                          {
                                                                                              Id = s.Id,
                                                                                              Name = s.Name,
                                                                                              Description = s.Description,
                                                                                              DisplayOrder = s.DisplayOrder
                                                                                          }).ToList();
                                foreach (dtoCallSection<dtoSubmissionValueField> section in sections)
                                {
                                    section.Fields = (from f in fieldValues where f.Field.IdSection == section.Id orderby f.Field.DisplayOrder, f.Field.Name select f).ToList();
                                }
                                // REMOVED GET SECTIONS ONLY WITH FIELDS
                                //return sections.Where(s => s.Fields.Count > 0).OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                                return sections.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                            }
                            catch (Exception ex)
                            {
                                return new List<dtoCallSection<dtoSubmissionValueField>>();
                            }
                        }
                        else
                            return new List<dtoCallSection<dtoSubmissionValueField>>();
                    }

                    public lm.Comol.Core.Mail.dtoMailMessage GetMailPreview(long idCall, dtoManagerTemplateMail mTemplate, litePerson fakeSubmitter, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, string webSiteurl, Dictionary<SubmissionTranslations, string> translations)
                    {
                        lm.Comol.Core.Mail.dtoMailMessage message = null;
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        litePerson currentUser = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && currentUser != null)
                        {
                            SubmitterType sType = call.SubmittersType.Where(s => s.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                            message = GetMailPreview(mTemplate.MailSettings.IsBodyHtml, mTemplate.Subject, mTemplate.Body, call, fakeSubmitter, sType, currentUser, webSiteurl, translations);
                        }
                        return message;
                    }
                    public lm.Comol.Core.Mail.dtoMailMessage GetMailPreview(long idCall, dtoSubmitterTemplateMail submitterTemplate, litePerson fakeSubmitter, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, string webSiteurl, Dictionary<SubmissionTranslations, string> translations)
                    {
                        lm.Comol.Core.Mail.dtoMailMessage message = null;
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        litePerson currentUser = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && currentUser!=null)
                        {
                            SubmitterType sType = call.SubmittersType.Where(s => s.Deleted == BaseStatusDeleted.None && submitterTemplate.SubmitterAssignments.Contains(s.Id)).FirstOrDefault();
                            if (sType != null && !submitterTemplate.SubmitterAssignments.Any())
                                sType = call.SubmittersType.Where(s => s.Deleted == BaseStatusDeleted.None).FirstOrDefault();


                            message = GetMailPreview(submitterTemplate.MailSettings.IsBodyHtml, submitterTemplate.Subject, submitterTemplate.Body, call, fakeSubmitter, sType, currentUser, webSiteurl, translations);
                        }
                        return message;
                    }


                    public List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> GetMessagesToSend(long idCall, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations, String websiteUrl, Dictionary<String, Dictionary<SubmissionTranslations, string>> translationsValues)
                    {
                        List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages = new List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage>();
                        if (translations.Any())
                        {  
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            if (call!=null){
                             
                                messages = translations.Select(t => new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage()
                                    {
                                        IdLanguage= t.IdLanguage,
                                        CodeLanguage= t.LanguageCode,
                                        Subject = (!String.IsNullOrEmpty(t.Translation.Subject) && t.Translation.Subject.Contains("[") && t.Translation.Subject.Contains("]")) ? AnalyzeContent(call, t.Translation.IsHtml, t.Translation.Subject, null, null, null, websiteUrl, translationsValues[t.LanguageCode]) : t.Translation.Subject,
                                        Body = (!String.IsNullOrEmpty(t.Translation.Body) && t.Translation.Body.Contains("[") && t.Translation.Body.Contains("]")) ? AnalyzeContent(call, t.Translation.IsHtml, t.Translation.Body, null, null, null, websiteUrl, translationsValues[t.LanguageCode]) : t.Translation.Body
                                    }).ToList();
                            }
                        }
                        return messages;
                    }

                    public List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> GetMessagesToSend(long idCall, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations, List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> recipients, String websiteUrl, Dictionary<String, Dictionary<SubmissionTranslations, string>> translationValues)
                    {
                        List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages = new List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage>();
                        if (recipients.Any() ) {
                            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                            if (call != null)
                            {
                                Language l = Manager.GetDefaultLanguage();
                                foreach( lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient recipient in recipients.Where(r => r.IsInternal)){
                                    lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation content = translations.Where(t => t.IdLanguage == recipient.IdLanguage && t.LanguageCode == recipient.CodeLanguage).FirstOrDefault();
                                    if (content == null)
                                        content = translations.Where(t => t.LanguageCode == "multi").FirstOrDefault();
                                    if (content == null)
                                        content = translations.Where(t => t.IdLanguage == l.Id).FirstOrDefault();
                                    //Boolean hasSubmissionLink = (content != null && content.Translation.Body.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmissionUrl)));
                                    List<UserSubmission> submissions = (from s in Manager.GetIQ<UserSubmission>()
                                                                        where s.Deleted == BaseStatusDeleted.None && s.Call != null && s.Call.Id == idCall
                                                                        && s.Owner != null && s.Owner.Id == recipient.IdPerson && (recipient.IdModuleObject==0 || recipient.IdModuleObject==s.Id)
                                                                        select s).ToList();
                                    foreach(UserSubmission s in submissions){
                                        List<dtoCallSection<dtoSubmissionValueField>> sections = GetSubmissionFields(call,s.Type.Id, s.Id, s.GetIdLastActiveRevision());
                                        if (content != null)
                                        {
                                            List<dtoSubmissionValueField> fields = null;
                                            if (sections == null)
                                                fields = new List<dtoSubmissionValueField>();
                                            else
                                                fields = sections.SelectMany(sc => sc.Fields).ToList();
                                            messages.Add(new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage(recipient)
                                            {
                                                IdLanguage = content.IdLanguage,
                                                CodeLanguage = content.LanguageCode,
                                                Subject = (!String.IsNullOrEmpty(content.Translation.Subject) && content.Translation.Subject.Contains("[") && content.Translation.Subject.Contains("]")) ? AnalyzeContent(call, content.Translation.IsHtml, content.Translation.Subject, s, s.Owner, fields, websiteUrl,  translationValues[content.LanguageCode]) : content.Translation.Subject,
                                                Body = (!String.IsNullOrEmpty(content.Translation.Body) && content.Translation.Body.Contains("[") && content.Translation.Body.Contains("]")) ? AnalyzeContent(call, content.Translation.IsHtml, content.Translation.Body, s, s.Owner, fields, websiteUrl,  translationValues[content.LanguageCode]) : content.Translation.Body,
                                            });
                                        }
                                    }
                                }
                                foreach (lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient recipient in recipients.Where(r =>  !r.IsInternal && r.IdUserModule ==0))
                                {
                                    lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation content = translations.Where(t => t.IdLanguage == recipient.IdLanguage && t.LanguageCode == recipient.CodeLanguage).FirstOrDefault();
                                    if (content == null)
                                        content = translations.Where(t => t.LanguageCode == "multi").FirstOrDefault();
                                    if (content == null)
                                        content = translations.Where(t => t.IdLanguage == l.Id).FirstOrDefault();

                                    
                                    messages.Add(new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage(recipient)
                                    {
                                        IdLanguage = content.IdLanguage,
                                        CodeLanguage = content.LanguageCode,
                                        Subject = (!String.IsNullOrEmpty(content.Translation.Subject) && content.Translation.Subject.Contains("[") && content.Translation.Subject.Contains("]")) ? AnalyzeContent(call, content.Translation.IsHtml, content.Translation.Subject, null, null, null, websiteUrl, translationValues[content.LanguageCode]) : content.Translation.Subject,
                                        Body = (!String.IsNullOrEmpty(content.Translation.Body) && content.Translation.Body.Contains("[") && content.Translation.Body.Contains("]")) ? AnalyzeContent(call, content.Translation.IsHtml, content.Translation.Body, null,null, null, websiteUrl, translationValues[content.LanguageCode]) : content.Translation.Body
                                    });
                                }
                                //if (recipients.Where(r => !r.IsInternal).Any()) {
                                //    messages.AddRange(translations.Select(t => new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage()
                                //    {
                                //        IdLanguage = t.IdLanguage,
                                //        CodeLanguage = t.LanguageCode,
                                //        Subject = (!String.IsNullOrEmpty(t.Translation.Subject) && t.Translation.Subject.Contains("[") && t.Translation.Subject.Contains("]")) ? AnalyzeContent(call, t.Translation.IsHtml, t.Translation.Subject, null, null, null, websiteUrl, disclaimerTranslation, yesOrNoTranslation) : t.Translation.Subject,
                                //        Body = (!String.IsNullOrEmpty(t.Translation.Body) && t.Translation.Body.Contains("[") && t.Translation.Body.Contains("]")) ? AnalyzeContent(call, t.Translation.IsHtml, t.Translation.Body, null, null, null, websiteUrl, disclaimerTranslation, yesOrNoTranslation) : t.Translation.Body,
                                //        RemovedRecipients = recipients.Where(r => !r.IsInternal && r.IdLanguage== t.IdLanguage && r.CodeLanguage==t.LanguageCode).ToList()
                                //    }).ToList().Where(r=>r.RemovedRecipients.Any()).ToList());
                                //}
                            }
                        }
                        return messages;
                    }

                    private lm.Comol.Core.Mail.dtoMailMessage GetMailPreview(Boolean isHtml, String subject, String body, BaseForPaper call, litePerson fakeSubmitter, SubmitterType sType, litePerson currentUser, string webSiteurl, Dictionary<SubmissionTranslations, string> translations)
                    {
                        UserSubmission fakeSubmission = GetFakeSubmission(call, fakeSubmitter,sType);
                        List<dtoCallSection<dtoSubmissionValueField>> sections = null;
                        if (sType != null)
                            sections = GetSubmissionFields(call, sType.Id, 0, 0);
                        else
                            sections = new List<dtoCallSection<dtoSubmissionValueField>>();
                        lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(AnalyzeContent(call, isHtml, subject, fakeSubmission, fakeSubmitter, sections.SelectMany(s => s.Fields).ToList(), webSiteurl, translations), AnalyzeContent(call, isHtml, body, fakeSubmission, fakeSubmitter, sections.SelectMany(s => s.Fields).ToList(), webSiteurl, translations));
                        message.FromUser = new System.Net.Mail.MailAddress(currentUser.Mail, currentUser.SurnameAndName);

                        return message;
                    }

                    private UserSubmission GetFakeSubmission(BaseForPaper call, litePerson fakeSubmitter, SubmitterType sType)
                    {
                        UserSubmission fakeSubmission = new UserSubmission();
                        fakeSubmission.Call = call;
                        fakeSubmission.Community = call.Community;
                        fakeSubmission.Owner = fakeSubmitter;
                        fakeSubmission.SubmittedBy = fakeSubmitter;
                        fakeSubmission.SubmittedOn = DateTime.Now;
                        fakeSubmission.Type = sType;
                        fakeSubmission.ModifiedBy = fakeSubmitter;
                        fakeSubmission.ModifiedOn = fakeSubmission.SubmittedOn.Value.AddHours(-1);
                        fakeSubmission.CreatedBy = fakeSubmitter;
                        fakeSubmission.CreatedOn = fakeSubmission.SubmittedOn.Value.AddHours(-2);

                        fakeSubmission.isAnonymous = false;
                        fakeSubmission.isComplete = true;
                        return fakeSubmission;
                    }
                    private dtoSubmitterTemplateMail GetSubmitterTemplateMail(UserSubmission submission, litePerson submitter)
                    {
                        dtoSubmitterTemplateMail result = null;
                        if (submission.Call != null)
                        {
                            List<long> idTemplates = (from a in Manager.GetIQ<TemplateAssignment>()
                                                      where a.Deleted == BaseStatusDeleted.None && a.SubmitterType != null
                                                          && a.SubmitterType.Id == submission.Type.Id
                                                      select a.Template.Id).ToList();
                            List<dtoSubmitterTemplateMail> submitterTemplates = (from t in Manager.GetIQ<SubmitterTemplateMail>()
                                                                                 where t.Deleted == BaseStatusDeleted.None && t.Call != null && t.Call.Id == submission.Call.Id
                                                                                 && idTemplates.Contains(t.Id)
                                                                                 select t).ToList().Select(t => new dtoSubmitterTemplateMail(submission.Type.Id) { Id = t.Id, Body = t.Body, Deleted = t.Deleted, IdLanguage = t.IdLanguage, Subject = t.Subject, MailSettings = t.MailSettings }).ToList();
                            if (submitterTemplates.Count > 0)
                            {
                                if (submitter.TypeID == (int)UserTypeStandard.Guest || submitter.TypeID == (int)UserTypeStandard.PublicUser)
                                {
                                    Language language = Manager.GetDefaultLanguage();
                                    if (language != null)
                                        result = submitterTemplates.Where(st => st.IdLanguage == language.Id).FirstOrDefault();
                                }
                                else
                                    result = submitterTemplates.Where(st => st.IdLanguage == submitter.LanguageID).FirstOrDefault();
                                if (result == null)
                                    result = submitterTemplates.FirstOrDefault();
                            }
                        }
                        return result;
                    }

                    private String AnalyzeContent(
                        BaseForPaper call, 
                        Boolean isHtml, 
                        String content, 
                        UserSubmission submission, 
                        litePerson submitter, 
                        List<dtoSubmissionValueField> fields, 
                        string webSiteurl, 
                        Dictionary<SubmissionTranslations, string> translations)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(content))
                            {
                                DateTime dayComplete = DateTime.Now;
                                if (submitter != null || fields != null)
                                {
                                    content = AnalyzeContent(content, submitter, PlaceHoldersType.SubmitterName,
                                        FieldType.Name, fields);
                                    content = AnalyzeContent(content, submitter, PlaceHoldersType.SubmitterSurname,
                                        FieldType.Surname, fields);
                                    content = AnalyzeContent(content, submitter, PlaceHoldersType.SubmitterMail,
                                        FieldType.Mail, fields);
                                    content = AnalyzeContentForAllFields(isHtml, content, submitter, fields,
                                        translations);
                                }
                                if (submission != null && submission.SubmittedOn.HasValue)
                                    dayComplete = submission.SubmittedOn.Value;
                                if (submission != null)
                                {
                                    content =
                                        content.Replace(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmissionDay),
                                            dayComplete.ToString("dd  MMMM  yyyy"));
                                    content =
                                        content.Replace(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmissionTime),
                                            dayComplete.ToShortTimeString());
                                    if (
                                        content.Contains(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmitterType)) &&
                                        submission.Type != null)
                                        content =
                                            content.Replace(
                                                TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmitterType),
                                                submission.Type.Name);
                                }
                                if (content.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallPublicUrl)))
                                    content =
                                        content.Replace(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallPublicUrl),
                                            webSiteurl +
                                            RootObject.StartNewSubmission(call.Type, call.Id, true, false,
                                                CallStatusForSubmitters.None, -1));

                                // DA CONTROLLARE
                                if (
                                    content.Contains(
                                        TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallInternalUrl)) &&
                                    submission != null)
                                    content =
                                        content.Replace(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallInternalUrl),
                                            webSiteurl +
                                            RootObject.StartNewSubmission(call.Type, call.Id, false, false,
                                                CallStatusForSubmitters.None, -1));

                                if (
                                    content.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmissionUrl)) &&
                                    submission != null)
                                {
                                    String submissionUrl = "";
                                    if (submission.Status >= SubmissionStatus.submitted ||
                                        submission.Status == SubmissionStatus.deleted ||
                                        submission.Deleted != BaseStatusDeleted.None)
                                        submissionUrl = RootObject.ViewSubmission(call.Type, call.Id, submission.Id,
                                            submission.UserCode, (call.IsPublic && submission.isAnonymous), false,
                                            CallStatusForSubmitters.Submitted, 
                                            (call.IsPortal) ? 0 : -1,
                                            0);
                                    else
                                        submissionUrl = RootObject.ContinueSubmission(call.Type, call.Id,
                                            (call.IsPublic && submission.isAnonymous), submission.Id, false,
                                            (call.Status == CallForPaperStatus.SubmissionClosed ||
                                             call.Status == CallForPaperStatus.SubmissionsLimitReached ||
                                             (call.EndDate.HasValue && call.EndDate.Value < DateTime.Now))
                                                ? CallStatusForSubmitters.SubmissionClosed
                                                : CallStatusForSubmitters.SubmissionOpened, (call.IsPortal) ? 0 : -1);
                                    content =
                                        content.Replace(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmissionUrl),
                                            webSiteurl + submissionUrl);
                                }

                                if (content.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallCommunity)))
                                {
                                    liteCommunity community = (submission.Call != null)
                                        ? GetCallCommunity(submission.Call.Id)
                                        : null;
                                    content =
                                        content.Replace(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallCommunity),
                                            (community == null) ? "" : community.Name);
                                }
                                if (
                                    content.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallEdition)) ||
                                    content.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallName)))
                                {
                                    content =
                                        content.Replace(
                                            TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallEdition),
                                            (submission.Call == null) ? "" : submission.Call.Edition);
                                    content =
                                        content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallName),
                                            (submission.Call == null) ? "" : submission.Call.Name);
                                }
                                if (fields != null &&
                                    fields.Select(f => f.IdField)
                                        .Where(
                                            f =>
                                                content.Contains("[Field_" + f.ToString() + "]") ||
                                                content.Contains("[Value_" + f.ToString() + "]"))
                                        .Any())
                                    content = AnalyzeFieldsContent(isHtml, content, fields, translations);
                            }
                        }
                        catch (Exception ex)
                        {
                            content = ex.ToString();
                        }
                        
                        return content;
                    }
                    private String AnalyzeFieldsContent(Boolean isHtml, String content, List<dtoSubmissionValueField> fields, Dictionary<SubmissionTranslations, string> translations)
                    {
                        foreach (dtoSubmissionValueField fieldValue in fields)
                        {
                            switch (fieldValue.Field.Type)
                            {
                                case FieldType.FileInput:
                                    Boolean uploaded = (fieldValue.Value !=null && fieldValue.Value.IdLink>0);
                                    SubmissionTranslations tItem = (uploaded) ? SubmissionTranslations.FileSubmitted : SubmissionTranslations.FileNotSubmitted;
                                    content = AnalyzeTag(content, fieldValue.Field.Id, fieldValue.Field.Name, (translations.ContainsKey((tItem))  ? translations[tItem] : "//"));
                                    break;
                                case FieldType.Disclaimer:
                                    switch (fieldValue.Field.DisclaimerType) { 
                                        case DisclaimerType.None:
                                            content = AnalyzeTag(content, fieldValue.Field.Id, fieldValue.Field.Name,  "");
                                            break;
                                        case DisclaimerType.CustomDisplayOnly:
                                            content = AnalyzeTag(content, fieldValue.Field.Id, fieldValue.Field.Name, translations[SubmissionTranslations.DisclaimerRead]);
                                            break;
                                        case DisclaimerType.Standard:
                                            Boolean response = false;
                                            if (fieldValue.Value != null )
                                                Boolean.TryParse(fieldValue.Value.Text, out response);
                                            content = AnalyzeTag(content, fieldValue.Field.Id, fieldValue.Field.Name, (response) ? translations[SubmissionTranslations.DisclaimerAccept] : translations[SubmissionTranslations.DisclaimerReject]);
                                            break;
                                        case DisclaimerType.CustomSingleOption:
                                        case DisclaimerType.CustomMultiOptions:
                                            String disclaimerValue = "";
                                            if (fieldValue.Value !=null && !String.IsNullOrEmpty(fieldValue.Value.Text))
                                            {
                                                string[] values = fieldValue.Value.Text.Split('|');
                                                if (isHtml)
                                                    disclaimerValue = "<ul>";
                                                FieldDefinition multipleField = Manager.Get<FieldDefinition>(fieldValue.Field.Id);
                                                foreach (string value in values)
                                                {
                                                    long idOption = Convert.ToInt64(value);
                                                    FieldOption opt = multipleField.Options.Where(o => o.Deleted == BaseStatusDeleted.None && o.Id.ToString() == value).FirstOrDefault();
                                                    if (opt != null)
                                                        disclaimerValue = disclaimerValue + ((isHtml) ? "<li>" : "") + ((opt.IsFreeValue) ? String.Format(translations[SubmissionTranslations.OtherOptionValue], opt.Name.ToString(), fieldValue.Value.Text) : opt.Name.ToString()) + ((isHtml) ? "</li>" : " ;<br>");
                                                }
                                                if (isHtml)
                                                    disclaimerValue += "</ul>";
                                                else
                                                    disclaimerValue = disclaimerValue.Remove(disclaimerValue.Length - 5, 5);
                                            }
                                            else
                                                disclaimerValue = "//";
                                            content = AnalyzeTag(content, fieldValue.Field.Id, fieldValue.Field.Name, (fieldValue==null || String.IsNullOrEmpty(fieldValue.Value.Text)) ? "//" : disclaimerValue);
                                            break;
                                        }
                                    break;
                                case FieldType.CheckboxList:
                                case FieldType.DropDownList:
                                case FieldType.RadioButtonList:
                                    String str = "";
                                    if (fieldValue.Value !=null && !String.IsNullOrEmpty(fieldValue.Value.Text))
                                    {
                                        string[] values = fieldValue.Value.Text.Split('|');
                                        if (isHtml)
                                            str = "<ul>";
                                        FieldDefinition multipleField = Manager.Get<FieldDefinition>(fieldValue.Field.Id);
                                        foreach (string value in values)
                                        {
                                            long idOption = Convert.ToInt64(value);
                                            FieldOption opt = multipleField.Options.Where(o => o.Deleted == BaseStatusDeleted.None && o.Id.ToString() == value).FirstOrDefault();
                                            if (opt != null)
                                                str = str + ((isHtml) ? "<li>" : "") + ((opt.IsFreeValue) ? String.Format(translations[SubmissionTranslations.OtherOptionValue], opt.Name.ToString(), fieldValue.Value.Text) : opt.Name.ToString()) + ((isHtml) ? "</li>" : " ;<br>");
                                        }
                                        if (isHtml)
                                            str += "</ul>";
                                        else
                                            str = str.Remove(str.Length - 5, 5);
                                    }
                                    else
                                        str = "//";
                                    content = AnalyzeTag(content, fieldValue.Field.Id, fieldValue.Field.Name, (fieldValue.Value==null) ? "//" : str);
                                    break;
                                default:
                                    content = AnalyzeTag(content, fieldValue.Field.Id, fieldValue.Field.Name, (fieldValue.Value == null || String.IsNullOrEmpty(fieldValue.Value.Text)) ? "//" : fieldValue.Value.Text);
                                    break;
                            }
                        }
                        return content;
                    }
                    private String AnalyzeContent(String content, litePerson submitter, PlaceHoldersType tag, FieldType fieldType, List<dtoSubmissionValueField> fields)
                    {
                        if (content.Contains(TemplatePlaceHolders.GetPlaceHolder(tag)))
                        {
                            String svalue="";
                            dtoValueField value = null;
                            if (this.UC.isAnonymous || submitter == null || submitter.TypeID == (int)UserTypeStandard.Guest || submitter.TypeID == (int)UserTypeStandard.PublicUser)
                            {
                                value = (fields == null) ? null : (from dtoSubmissionValueField field in fields where field.Field.Type == fieldType select field.Value).FirstOrDefault();
                                svalue = (value == null) ? "" : value.Text;
                            }
                            else
                            {
                                switch (fieldType)
                                {
                                    case FieldType.Mail:
                                        svalue = submitter.Mail;
                                        break;
                                    case FieldType.Name:
                                        svalue = submitter.Name;
                                        break;
                                    case FieldType.Surname:
                                        svalue = submitter.Surname;
                                        break;
                                    case FieldType.TaxCode:
                                        svalue = submitter.TaxCode;
                                        break;
                                }
                            }
                            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(tag), svalue);
                        }
                        return content;
                    }
                    private String AnalyzeContentForAllFields(
                        Boolean isHtml, 
                        String content, 
                        litePerson submitter, 
                        List<dtoSubmissionValueField> fields, 
                        Dictionary<SubmissionTranslations, string> translations)
                    {
                        if (
                            fields != null 
                            && content.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.AllFields))
                            )
                        {
                            String allFields = "";
                            String FieldFormat = (isHtml) ? "<b>{0}</b>: {1}" : "{0}: {1}";
                            foreach (dtoSubmissionValueField fieldValue in fields)
                            {
                               
                                
                                String value = "";
                                try
                                {

                                    switch (fieldValue.Field.Type)
                                    {
                                        case FieldType.FileInput:
                                            Boolean uploaded = (fieldValue.Value != null && fieldValue.Value.IdLink > 0);
                                            SubmissionTranslations tItem = (uploaded)
                                                ? SubmissionTranslations.FileSubmitted
                                                : SubmissionTranslations.FileNotSubmitted;
                                            value = String.Format(FieldFormat, fieldValue.Field.Name,
                                                (translations.ContainsKey((tItem)) ? translations[tItem] : "//"));
                                            break;
                                        case FieldType.Disclaimer:
                                            switch (fieldValue.Field.DisclaimerType)
                                            {
                                                case DisclaimerType.None:
                                                    value = String.Format(FieldFormat, fieldValue.Field.Name, "");
                                                    break;
                                                case DisclaimerType.CustomDisplayOnly:
                                                    value = String.Format(FieldFormat, fieldValue.Field.Name,
                                                        translations[SubmissionTranslations.DisclaimerRead]);
                                                    break;
                                                case DisclaimerType.Standard:
                                                    Boolean response = false;
                                                    if (fieldValue.Value != null)
                                                        Boolean.TryParse(fieldValue.Value.Text, out response);
                                                    value = String.Format(FieldFormat, fieldValue.Field.Name,
                                                        (response)
                                                            ? translations[SubmissionTranslations.DisclaimerAccept]
                                                            : translations[SubmissionTranslations.DisclaimerReject]);
                                                    break;
                                                case DisclaimerType.CustomSingleOption:
                                                case DisclaimerType.CustomMultiOptions:
                                                    String disclaimerValue = "";
                                                    if (fieldValue.Value != null &&
                                                        !String.IsNullOrEmpty(fieldValue.Value.Text))
                                                    {
                                                        string[] values = fieldValue.Value.Text.Split('|');
                                                        if (isHtml)
                                                            disclaimerValue = "<ul>";
                                                        FieldDefinition multipleField =
                                                            Manager.Get<FieldDefinition>(fieldValue.Field.Id);
                                                        foreach (String v in values)
                                                        {
                                                            long idOption = Convert.ToInt64(v);
                                                            FieldOption opt =
                                                                multipleField.Options.Where(
                                                                    o =>
                                                                        o.Deleted == BaseStatusDeleted.None &&
                                                                        o.Id.ToString() == v).FirstOrDefault();
                                                            if (opt != null)
                                                                disclaimerValue = disclaimerValue +
                                                                                  ((isHtml) ? "<li>" : "") +
                                                                                  ((opt.IsFreeValue)
                                                                                      ? String.Format(
                                                                                          translations[
                                                                                              SubmissionTranslations
                                                                                                  .OtherOptionValue],
                                                                                          opt.Name.ToString(),
                                                                                          fieldValue.Value.Text)
                                                                                      : opt.Name.ToString()) +
                                                                                  ((isHtml) ? "</li>" : " ;<br>");
                                                        }
                                                        if (isHtml)
                                                            disclaimerValue += "</ul>";
                                                        else
                                                            disclaimerValue =
                                                                disclaimerValue.Remove(disclaimerValue.Length - 5, 5);
                                                    }
                                                    else
                                                        disclaimerValue = "//";
                                                    value = String.Format(FieldFormat, fieldValue.Field.Name,
                                                        (fieldValue.Value == null ||
                                                         String.IsNullOrEmpty(fieldValue.Value.Text))
                                                            ? "//"
                                                            : disclaimerValue);
                                                    break;

                                            }
                                            break;
                                        case FieldType.CheckboxList:
                                        case FieldType.DropDownList:
                                        case FieldType.RadioButtonList:
                                            String str = "";
                                            if (fieldValue.Value != null && !String.IsNullOrEmpty(fieldValue.Value.Text))
                                            {
                                                string[] values = fieldValue.Value.Text.Split('|');
                                                if (isHtml)
                                                    str = "<ul>";
                                                FieldDefinition multipleField =
                                                    Manager.Get<FieldDefinition>(fieldValue.Field.Id);
                                                foreach (string v in values)
                                                {
                                                    long idOption = Convert.ToInt64(v);
                                                    FieldOption opt =
                                                        multipleField.Options.Where(
                                                            o =>
                                                                o.Deleted == BaseStatusDeleted.None &&
                                                                o.Id.ToString() == v).FirstOrDefault();
                                                    if (opt != null)
                                                        str = str + ((isHtml) ? "<li>" : "") +
                                                              ((opt.IsFreeValue)
                                                                  ? String.Format(
                                                                      translations[
                                                                          SubmissionTranslations.OtherOptionValue],
                                                                      opt.Name.ToString(), fieldValue.Value.Text)
                                                                  : opt.Name.ToString()) +
                                                              ((isHtml) ? "</li>" : " ;<br>");
                                                }
                                                if (isHtml)
                                                    str += "</ul>";
                                                else
                                                    str = str.Remove(str.Length - 5, 5);
                                            }
                                            else
                                                str = "//";
                                            value = String.Format(FieldFormat, fieldValue.Field.Name,
                                                (fieldValue.Value == null || String.IsNullOrEmpty(fieldValue.Value.Text))
                                                    ? "//"
                                                    : str);
                                            break;
                                        case FieldType.Note:
                                            value = String.Format(FieldFormat, fieldValue.Field.Name,
                                                fieldValue.Field.Description);
                                            //value = "";
                                            break;
                                        case FieldType.MultiLine:
                                            value = String.Format(
                                                FieldFormat,
                                                fieldValue.Field.Name,
                                                (fieldValue.Value == null || String.IsNullOrEmpty(fieldValue.Value.Text))
                                                    ? "//"
                                                    : String.Format("<br/>{0}",
                                                        fieldValue.Value.Text.Replace("\r\n", "<br/>"))
                                                );
                                            break;
                                        case FieldType.TableSimple:
                                        case FieldType.TableReport:
                                            
                                            String htmlTable = string.Format("<br>{0}", CallTableHelper.TableDecorateHtml(fieldValue, false));

                                            value = string.Format(
                                                FieldFormat,
                                                fieldValue.Field.Name,
                                                htmlTable);
                                            
                                            break;
                                        case FieldType.TableSummary:
                                            value = "";
                                            break;

                                        default:
                                            value = String.Format(FieldFormat, fieldValue.Field.Name,
                                                (fieldValue.Value == null || String.IsNullOrEmpty(fieldValue.Value.Text))
                                                    ? "//"
                                                    : fieldValue.Value.Text);
                                            break;
                                    }
                                }
                                catch
                                {
                                    value = fieldValue.Field.Type.ToString();
                                }

                                if (!string.IsNullOrEmpty(value))
                                    allFields = allFields + ((isHtml) ? "<br>" : "\r\n") + value;
                            }
                            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.AllFields), allFields);
                        }
                        return content;
                    }
                    private String AnalyzeTag(String content, long idField, String fieldName, String fieldValue)
                    {
                        String openFieldTag = String.Format("[Field_{0}]", idField.ToString());
                        String closeFieldTag = String.Format("[/Field_{0}]", idField.ToString());
                        String openValueTag = String.Format("[Value_{0}]", idField.ToString());
                        String closeValueTag = String.Format("[/Value_{0}]", idField.ToString());

                        dtoFieldTag tag = new dtoFieldTag(content, openFieldTag, closeFieldTag);
                        content = tag.ReplaceTag(content, fieldName);
                        tag = new dtoFieldTag(content, openValueTag, closeValueTag);
                        content = tag.ReplaceTag(content, fieldValue);
                        return content;
                    }
                  
                #endregion

                #region "6 Status Management"
                    public Boolean EditSubmissionStatus(long idSubmission, int idUser, SubmissionStatus status, string webSiteurl, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, String body, String subject)
                    {
                        Boolean result = false;
                        UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);
                        litePerson person = Manager.GetLitePerson(idUser);
                        if (submission == null && idSubmission > 0)
                            throw new SubmissionNotFound(idSubmission.ToString());
                        else
                        {
                            try
                            {
                                Manager.BeginTransaction();
                                if (submission != null)
                                {
                                    submission.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    submission.Status = status;
                                    Manager.SaveOrUpdate(submission);
                                }
                                Manager.Commit();
                                result = true;
                                if (submission.Call.AcceptRefusePolicy == NotifyAcceptRefusePolicy.All || (status == SubmissionStatus.accepted && submission.Call.AcceptRefusePolicy == NotifyAcceptRefusePolicy.Accept) || (status == SubmissionStatus.rejected && submission.Call.AcceptRefusePolicy == NotifyAcceptRefusePolicy.Refuse))
                                {
                                    String mailField = "";
                                    if (submission.Owner != null && (submission.Owner.TypeID != (int)UserTypeStandard.PublicUser && submission.Owner.TypeID != (int)UserTypeStandard.Guest))
                                        mailField = submission.Owner.Mail;
                                    if (!string.IsNullOrEmpty(mailField))
                                    {
                                        lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mSettings = new lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings() { CopyToSender = false, IsBodyHtml = true, NotifyToSender = false, PrefixType =  Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration, SenderType =  Core.MailCommons.Domain.SenderUserType.System};
                                        lm.Comol.Core.Mail.MailService mailService = new lm.Comol.Core.Mail.MailService(smtpConfig, mSettings);
                                        body = String.Format(body, submission.ModifiedOn.Value.ToString("dd/MM/yyyy"), submission.ModifiedOn.Value.ToString("HH:mm"));
                                        body = body.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CallName), submission.Call.Name);
                                        body = body.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmitterName), submission.Owner.Name);
                                        body = body.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmitterSurname), submission.Owner.Surname);
                                        body = body.Replace("[LinkUrl]", webSiteurl + RootObject.ViewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, GetIdLastActiveRevision(submission.Id), submission.UserCode, false, false, CallStatusForSubmitters.Submitted, -1, 0));
                                        lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(subject, body);
                                        message.FromUser = smtpConfig.GetSystemSender();
                                        mailService.SendMail((submission.Owner == null) ? 0 : submission.Owner.LanguageID, Manager.GetDefaultLanguage(), message, mailField, lm.Comol.Core.MailCommons.Domain.RecipientType.To);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                throw new SubmissionStatusChange(ex.Message, ex);
                            }
                        }
                        return result;
                    }
                #endregion

                #region "7 - Export"

                    public static string TemplateGetDefaultBody()
                    {
                        string body = "<h1>[cfp.Title]</h1>";
                        body = string.Format("{0}<br>[cfp.Description]-[cfp.Edition]", body);
                        body = string.Format("{0}<br><br><br>[cfp.CallBaseInfo]", body);
                        body = string.Format("{0}<br><br><br><br><i>Stampato da: [cfp.PrintedBy] il [cfp.PrintedOn]</i>", body);
                        body = string.Format("{0}[cfp.NewPage][cfp.CallBody]", body);
                        return body;

                    }

                    public iTextSharp5.text.Document ExportSubmissionToPDF(
                        Boolean webOnlyRender,
                        long idSubmission,
                        long idRevision,
                        String baseFilePath,
                        String clientFileName,
                        Dictionary<SubmissionTranslations, string> translations,
                        System.Web.HttpResponse webResponse,
                        System.Web.HttpCookie cookie,
                        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template,
                        CallPrintSettings callPrintSet,
                        CommonPlaceHolderData phData)
                    {
                        if (template == null)
                            template = new DTO_Template();
                        if (template.Body == null)
                            template.Body = new DTO_ElementText();
                        if (String.IsNullOrEmpty(template.Body.Text))
                        {
                            template.Body.Text = TemplateGetDefaultBody();
                        }



                        List<String> filesToRemove = new List<String>();
                        HelperExportToPDF helper = new HelperExportToPDF(translations, template);
                        Boolean addContentDisposition = true;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

                            if (!webOnlyRender)
                                webOnlyRender = !(person != null && person.Id > 0);
                            Revision revision = Manager.Get<Revision>(idRevision);

                            iTextSharp5.text.Document file = null;
                            if (revision == null || revision.Submission == null || revision.Submission.Id != idSubmission)
                                file = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie);
                            else
                            {
                                lm.Comol.Core.FileRepository.Domain.RepositoryItem internalFile = null;
                                String fileExport = "";
                                if (!webOnlyRender)
                                {
                                    if (revision.Submission.Status >= SubmissionStatus.submitted && revision.FilePDF != null)
                                    {
                                        filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, revision.FilePDF));
                                        Manager.DeletePhysical(revision.LinkPDF);
                                        Manager.DeletePhysical(revision.FilePDF);
                                        revision.LinkPDF = null;
                                        revision.FilePDF = null;
                                    }
                                    foreach (char c in System.IO.Path.GetInvalidFileNameChars().Where(cr => clientFileName.Contains(cr)).ToList())
                                    {
                                        clientFileName = clientFileName.Replace(c, '_');
                                    }
                                    internalFile = CreateInternalFile(person, revision.Submission, ExportFileType.pdf, clientFileName);
                                    fileExport = RepositoryService.GetItemDiskFullPath(baseFilePath, internalFile);
                                }
                                dtoExportSubmission settings = null;
                                if (revision.Submission.Status >= SubmissionStatus.submitted && !webOnlyRender)
                                    settings = GetSettingsFromRevision(revision, clientFileName, fileExport, person);
                                else
                                    settings = GetSettingsFromRevision(revision, clientFileName, person);

                                addContentDisposition = !settings.ForWebDownload;

                                //Add submitted file info (ToDo: rivedere, magari persistere nel campo all'upload!)

                                //phData.ModuleObject = Manager.GetPerson(settings.Submission.SubmittedBy.Id);
                                phData.ModuleObject = Manager.GetPerson(
                                    (settings.Submission.SubmittedBy != null) ?
                                    settings.Submission.SubmittedBy.Id :
                                    settings.Submission.CreatedBy.Id
                                    );

                                //try
                                //{
                                //    //ToDo: refactor!!!
                                //    //ToDo: MEGLIO se il Value.Text viene impostato sul nome del file all'UPLOAD del file nel bando.
                                //    foreach (dtoCallSection<dtoSubmissionValueField> sect in settings.Sections)
                                //    {
                                //        foreach (dtoSubmissionValueField vf in sect.Fields)
                                //        {
                                //            if (vf.Field != null &&
                                //                vf.Field.Type == FieldType.FileInput
                                //                && vf.Value != null
                                //                && vf.Value.Link != null
                                //                && vf.Value.Link.DestinationItem != null)
                                //            {
                                //                try
                                //                {
                                //                    ModuleLongInternalFile miFile = Manager.Get<ModuleLongInternalFile>(vf.Value.Link.DestinationItem.ObjectLongID);

                                //                    if (miFile != null)
                                //                        vf.Value.Text = miFile.DisplayName;

                                //                }
                                //                catch (Exception) { }
                                //            }
                                //        }
                                //    }
                                //}
                                //catch (Exception) { }
                                try
                                {
                                    //ToDo: refactor!!!
                                    //ToDo: MEGLIO se il Value.Text viene impostato sul nome del file all'UPLOAD del file nel bando.
                                    foreach (dtoCallSection<dtoSubmissionValueField> sect in settings.Sections)
                                    {
                                        foreach (dtoSubmissionValueField vf in sect.Fields)
                                        {
                                            if (vf.Field != null &&
                                                vf.Field.Type == FieldType.FileInput
                                                && vf.Value != null
                                                && vf.Value.Link != null
                                                && vf.Value.Link.DestinationItem != null)
                                            {
                                                try
                                                {
                                                    lm.Comol.Core.FileRepository.Domain.liteRepositoryItem intFile = Manager.Get<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>(vf.Value.Link.DestinationItem.ObjectLongID);

                                                    if (intFile != null)
                                                        vf.Value.Text = intFile.DisplayName;
                                                }
                                                catch (Exception) { }
                                            }
                                        }
                                    }
                                }
                                catch (Exception) { }

                                file = helper.Submission(false, settings, webResponse, cookie, callPrintSet, phData);

                                if (!webOnlyRender)
                                {
                                    dtoFileSystemInfo info = lm.Comol.Core.File.ContentOf.File_dtoInfo(fileExport);
                                    if (info != null)
                                    {
                                        internalFile.Size = info.Length;
                                        Manager.SaveOrUpdate(internalFile);
                                        lm.Comol.Core.FileRepository.Domain.RepositoryItemVersion version = internalFile.CreateFirstVersion();
                                        Manager.SaveOrUpdate(version);
                                        internalFile.IdVersion = version.Id;
                                        Manager.SaveOrUpdate(internalFile);
                                        lm.Comol.Core.FileRepository.Domain.liteRepositoryItem liteFile = Manager.Get<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>(internalFile.Id);
                                        liteModuleLink link = new liteModuleLink("", (int)lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.DownloadOrPlay, (int)lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DownloadFile);
                                        link.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                        link.DestinationItem = ModuleObject.CreateLongObject(liteFile.Id, 0, liteFile, (Int32)lm.Comol.Core.FileRepository.Domain.ModuleRepository.GetObjectType(internalFile.Type), internalFile.Repository.IdCommunity, lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode, GetIdRepositoryModule());
                                        link.SourceItem = CreateModuleObject(revision.Submission);
                                        Manager.SaveOrUpdate(link);

                                        revision.LinkPDF = link;
                                        revision.FilePDF = liteFile;

                                        Manager.SaveOrUpdate(revision);
                                    }
                                }
                            }
                            Manager.Commit();
                            Delete.Files(filesToRemove);
                            return file;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            throw new ExportError("", ex) { ErrorPdfDocument = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie) };
                        }

                    }

                //    public iTextSharp5.text.Document ExportSubmissionDraftToPDF(
                //Boolean webOnlyRender,
                //long idSubmission,
                //long idRevision,
                //String baseFilePath,
                //String clientFileName,
                //Dictionary<SubmissionTranslations, string> translations,
                //System.Web.HttpResponse webResponse,
                //System.Web.HttpCookie cookie,
                //lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template,
                //CallPrintSettings callPrintSet,
                //CommonPlaceHolderData phData
                //)
                //    {

                //        if (template == null)
                //            template = new DTO_Template();
                //        if (template.Body == null)
                //            template.Body = new DTO_ElementText();
                //        if (String.IsNullOrEmpty(template.Body.Text))
                //        {
                //            template.Body.Text = TemplateGetDefaultBody();
                //        }


                //        HelperExportToPDF helper = new HelperExportToPDF(translations, template);
                //        Boolean addContentDisposition = true;
                //        try
                //        {
                //            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

                //            if (!webOnlyRender)
                //                webOnlyRender = !(person != null && person.Id > 0);
                //            Revision revision = Manager.Get<Revision>(idRevision);

                //            iTextSharp5.text.Document file = null;
                //            if (revision == null || revision.Submission == null || revision.Submission.Id != idSubmission)
                //                file = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie);
                //            else
                //            {
                //                dtoExportSubmission settings = GetSettingsFromRevision(revision, clientFileName, person);

                //                addContentDisposition = !settings.ForWebDownload;
                //                phData.ModuleObject = Manager.GetPerson(
                //                    (settings.Submission.SubmittedBy != null) ?
                //                    settings.Submission.SubmittedBy.Id :
                //                    settings.Submission.CreatedBy.Id
                //                    );

                //                try
                //                {
                //                    //ToDo: refactor!!!
                //                    //ToDo: MEGLIO se il Value.Text viene impostato sul nome del file all'UPLOAD del file nel bando.
                //                    foreach (dtoCallSection<dtoSubmissionValueField> sect in settings.Sections)
                //                    {
                //                        foreach (dtoSubmissionValueField vf in sect.Fields)
                //                        {
                //                            if (vf.Field != null &&
                //                                vf.Field.Type == FieldType.FileInput
                //                                && vf.Value != null
                //                                && vf.Value.Link != null
                //                                && vf.Value.Link.DestinationItem != null)
                //                            {
                //                                try
                //                                {
                //                                    ModuleLongInternalFile miFile = Manager.Get<ModuleLongInternalFile>(vf.Value.Link.DestinationItem.ObjectLongID);

                //                                    if (miFile != null)
                //                                        vf.Value.Text = miFile.DisplayName;

                //                                }
                //                                catch (Exception) { }
                //                            }
                //                        }
                //                    }
                //                }
                //                catch (Exception) { }


                //                file = helper.Submission(false, settings, webResponse, cookie, callPrintSet, phData, true); //, callPrintSet);

                //            }
                //            return file;
                //        }
                //        catch (Exception ex)
                //        {
                //            throw new ExportError("", ex) { ErrorPdfDocument = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie) };
                //        }

                //    }
                  
                    //public iTextSharp5.text.Document ExportSubmissionToPDF(
                    //                Boolean webOnlyRender,
                    //                long idSubmission,
                    //                long idRevision,
                    //                String baseFilePath,
                    //                String clientFileName,
                    //                Dictionary<SubmissionTranslations, string> translations,
                    //                System.Web.HttpResponse webResponse,
                    //                System.Web.HttpCookie cookie,
                    //                lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template,
                    //                CallPrintSettings callPrintSet,
                    //                CommonPlaceHolderData phData
                    //                )
                    //{

                    //    if (template == null)
                    //        template = new DTO_Template();
                    //    if (template.Body == null)
                    //        template.Body = new DTO_ElementText();
                    //    if (String.IsNullOrEmpty(template.Body.Text))
                    //    {
                    //        template.Body.Text = TemplateGetDefaultBody();
                    //    }


                    //    List<String> filesToRemove = new List<String>();
                    //    HelperExportToPDF helper = new HelperExportToPDF(translations, template);
                    //    Boolean addContentDisposition = true;
                    //    try
                    //    {
                    //        Manager.BeginTransaction();
                    //        string fileName = baseFilePath + "\\{0}\\{1}.stored";
                    //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

                    //        if (!webOnlyRender)
                    //            webOnlyRender = !(person != null && person.Id > 0);
                    //        Revision revision = Manager.Get<Revision>(idRevision);






                    //        iTextSharp5.text.Document file = null;
                    //        if (revision == null || revision.Submission == null || revision.Submission.Id != idSubmission)
                    //            file = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie);
                    //        else
                    //        {
                    //            ModuleLongInternalFile internalFile = null;
                    //            String fileExport = "";
                    //            if (!webOnlyRender)
                    //            {
                    //                if (revision.Submission.Status >= SubmissionStatus.submitted && revision.FilePDF != null)
                    //                {
                    //                    filesToRemove.Add(String.Format(fileName, revision.FilePDF.CommunityOwner == null ? "0" : revision.FilePDF.CommunityOwner.Id.ToString(), revision.FilePDF.UniqueID.ToString()));
                    //                    Manager.DeletePhysical(revision.LinkPDF);
                    //                    Manager.DeletePhysical(revision.FilePDF);
                    //                    revision.LinkPDF = null;
                    //                    revision.FilePDF = null;
                    //                }
                    //                foreach (char c in System.IO.Path.GetInvalidFileNameChars().Where(cr => clientFileName.Contains(cr)).ToList())
                    //                {
                    //                    clientFileName = clientFileName.Replace(c, '_');
                    //                }
                    //                internalFile = CreateInternalFile(person, revision.Submission, ExportFileType.pdf, clientFileName);
                    //                fileExport = String.Format(fileName, revision.Submission.Community == null ? "0" : revision.Submission.Community.Id.ToString(), internalFile.UniqueID.ToString());
                    //            }
                    //            dtoExportSubmission settings = null;
                    //            if (revision.Submission.Status >= SubmissionStatus.submitted && !webOnlyRender)
                    //                settings = GetSettingsFromRevision(revision, clientFileName, fileExport, person);
                    //            else
                    //                settings = GetSettingsFromRevision(revision, clientFileName, person);

                    //            addContentDisposition = !settings.ForWebDownload;


                    //            phData.ModuleObject = Manager.GetPerson(settings.Submission.SubmittedBy.Id);


                    //            try
                    //            {
                    //                //ToDo: refactor!!!
                    //                //ToDo: MEGLIO se il Value.Text viene impostato sul nome del file all'UPLOAD del file nel bando.
                    //                foreach (dtoCallSection<dtoSubmissionValueField> sect in settings.Sections)
                    //                {
                    //                    foreach (dtoSubmissionValueField vf in sect.Fields)
                    //                    {
                    //                        if (vf.Field != null &&
                    //                            vf.Field.Type == FieldType.FileInput
                    //                            && vf.Value != null
                    //                            && vf.Value.Link != null
                    //                            && vf.Value.Link.DestinationItem != null)
                    //                        {
                    //                            try
                    //                            {
                    //                                ModuleLongInternalFile miFile = Manager.Get<ModuleLongInternalFile>(vf.Value.Link.DestinationItem.ObjectLongID);

                    //                                if (miFile != null)
                    //                                    vf.Value.Text = miFile.DisplayName;

                    //                            }
                    //                            catch (Exception) { }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            catch (Exception) { }


                    //            file = helper.Submission(false, settings, webResponse, cookie, callPrintSet, phData); //, callPrintSet);




                    //            if (!webOnlyRender)
                    //            {
                    //                dtoFileSystemInfo info = lm.Comol.Core.File.ContentOf.File_dtoInfo(fileExport);
                    //                if (info != null)
                    //                {
                    //                    internalFile.Size = info.Length;
                    //                    Manager.SaveOrUpdate(internalFile);

                    //                    ModuleLink link = new ModuleLink("", (int)CoreModuleRepository.Base2Permission.DownloadFile, (int)CoreModuleRepository.ActionType.DownloadFile);
                    //                    link.CreateMetaInfo(GetPersonFromLite(person), UC.IpAddress, UC.ProxyIpAddress);
                    //                    link.DestinationItem = ModuleObject.CreateLongObject(internalFile.Id, internalFile, (int)CoreModuleRepository.ObjectType.File, revision.Submission.Call.Community == null ? 0 : revision.Submission.Call.Community.Id, CoreModuleRepository.UniqueID, Manager.GetModuleID(CoreModuleRepository.UniqueID));
                    //                    link.SourceItem = CreateModuleObject(revision.Submission);
                    //                    Manager.SaveOrUpdate(link);

                    //                    revision.LinkPDF = link;
                    //                    revision.FilePDF = internalFile;


                    //                    Manager.SaveOrUpdate(revision);
                    //                }
                    //            }
                    //        }
                    //        Manager.Commit();
                    //        Delete.Files(filesToRemove);
                    //        return file;
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Manager.RollBack();
                    //        throw new ExportError("", ex) { ErrorPdfDocument = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie) };
                    //    }

                    //}
                    public Document ExportSubmissionToRTF(
                        Boolean webOnlyRender,
                        long idSubmission,
                        long idRevision,
                        String baseFilePath,
                        String clientFileName,
                        Dictionary<SubmissionTranslations, string> translations,
                        System.Web.HttpResponse webResponse,
                        System.Web.HttpCookie cookie,
                        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template)
                    {
                        List<String> filesToRemove = new List<String>();
                        HelperExportToRTF helper = new HelperExportToRTF(translations, template);
                        Boolean addContentDisposition = true;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (!webOnlyRender)
                                webOnlyRender = !(person != null && person.Id > 0);

                            Revision revision = Manager.Get<Revision>(idRevision);

                            Document file = null;
                            if (revision == null || revision.Submission== null || revision.Submission.Id != idSubmission)
                                file = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie);
                            else
                            {
                                lm.Comol.Core.FileRepository.Domain.RepositoryItem internalFile = null;
                                String fileExport = "";
                                if (!webOnlyRender)
                                {
                                    if (revision.Submission.Status >= SubmissionStatus.submitted && revision.FileRTF != null)
                                    {
                                        filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, revision.FileRTF));
                                        Manager.DeletePhysical(revision.LinkRTF);
                                        Manager.DeletePhysical(revision.FileRTF);
                                        revision.LinkRTF = null;
                                        revision.FileRTF = null;
                                    }
                                    foreach (char c in System.IO.Path.GetInvalidFileNameChars().Where(cr => clientFileName.Contains(cr)).ToList())
                                    {
                                        clientFileName = clientFileName.Replace(c, '_');
                                    }
                                    //internalFile = CreateInternalFile(person, revision.Submission, ExportFileType.rtf, clientFileName);
                                    fileExport = RepositoryService.GetItemDiskFullPath(baseFilePath, internalFile);
                                }
                                //if (revision.Submission.Status >= SubmissionStatus.submitted && !webOnlyRender)
                                //    file = helper.Submission(false, GetSettingsFromRevision(revision, clientFileName, fileExport, person), webResponse, cookie);
                                //else
                                //    file = helper.Submission(false, GetSettingsFromRevision(revision, clientFileName, person), webResponse, cookie);
                                dtoExportSubmission settings = null;
                                if (revision.Submission.Status >= SubmissionStatus.submitted && !webOnlyRender)
                                    settings = GetSettingsFromRevision(revision, clientFileName, fileExport, person);
                                else
                                    settings = GetSettingsFromRevision(revision, clientFileName, person);

                                addContentDisposition = !settings.ForWebDownload;
                                file = helper.Submission(false, settings, webResponse, cookie);

                                if (!webOnlyRender)
                                {
                                    dtoFileSystemInfo info = lm.Comol.Core.File.ContentOf.File_dtoInfo(fileExport);
                                    if (info != null)
                                    {
                                        internalFile.Size = info.Length;
                                        Manager.SaveOrUpdate(internalFile);
                                        lm.Comol.Core.FileRepository.Domain.RepositoryItemVersion version = internalFile.CreateFirstVersion();
                                        Manager.SaveOrUpdate(version);
                                        internalFile.IdVersion = version.Id;
                                        Manager.SaveOrUpdate(internalFile);
                                        lm.Comol.Core.FileRepository.Domain.liteRepositoryItem liteFile = Manager.Get<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>(internalFile.Id);

                                        liteModuleLink link = new liteModuleLink("", (int)lm.Comol.Core.FileRepository.Domain.ModuleRepository.Base2Permission.DownloadOrPlay, (int)lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.DownloadFile);
                                        link.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                        link.DestinationItem = ModuleObject.CreateLongObject(liteFile.Id, 0, liteFile, (Int32)lm.Comol.Core.FileRepository.Domain.ModuleRepository.GetObjectType(internalFile.Type), internalFile.Repository.IdCommunity, lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode, GetIdRepositoryModule());
                                        link.SourceItem = CreateModuleObject(revision.Submission);
                                        Manager.SaveOrUpdate(link);

                                        revision.LinkRTF = link;
                                        revision.FileRTF = liteFile;

                                        Manager.SaveOrUpdate(revision);
                                    }
                                }
                            }
                            Manager.Commit();
                            Delete.Files(filesToRemove);
                            return file;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            throw new ExportError("", ex) { ErrorDocument = helper.GetErrorDocument(false,addContentDisposition, clientFileName, webResponse, cookie) };
                        }

                    }

                    private dtoExportSubmission GetSettingsFromRevision(Revision revision, String clientFilename, String fileName, litePerson person)
                    {
                        dtoExportSubmission settings = new dtoExportSubmission();
                        settings.ClientFilename = clientFilename;
                        settings.Filename = fileName;
                        settings.Submission = revision.Submission;
                        settings.PrintBy = person;
                        settings.RequiredFiles = GetRequiredFiles(revision.Submission.Call, revision.Submission.Type.Id, revision.Submission.Id);
                        settings.Sections = GetSubmissionFields(revision);
                        return settings;
                    }
                    private dtoExportSubmission GetSettingsFromRevision(Revision revision, String clientFilename, litePerson person)
                    {
                        dtoExportSubmission settings = new dtoExportSubmission();
                        settings.ClientFilename = clientFilename;
                        settings.Submission = revision.Submission;
                        settings.PrintBy = person;
                        settings.RequiredFiles = GetRequiredFiles(revision.Submission.Call, revision.Submission.Type.Id, revision.Submission.Id);
                        settings.Sections = GetSubmissionFields(revision);
                        return settings;
                    }
                    private dtoExportSubmission GetSettingsFromSubmission(UserSubmission submission, String clientFilename, String fileName, litePerson person)
                    {
                        //New for draft print
                        dtoExportSubmission settings = new dtoExportSubmission();
                        settings.ClientFilename = clientFilename;
                        settings.Filename = fileName;
                        settings.Submission = submission;
                        settings.PrintBy = person;
                        settings.RequiredFiles = GetRequiredFiles(submission.Call, submission.Type.Id, submission.Id);
                        settings.Sections = GetSubmissionFields(submission.Call, submission.Type.Id);
                        return settings;
                    }
                    //public Document ExportSubmission(long idCall, long idSubmitter, Dictionary<SubmissionTranslations, string> translations, System.IO.Stream stream, ExportFileType fileType)
                    //{
                    //    try
                    //    {
                    //        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    //        SubmitterType submitter = Manager.Get<SubmitterType>(idSubmitter);
                    //        if (call == null || submitter == null)
                    //            return OLDExportHelpers.GetErrorDocument(translations, stream, fileType);
                    //        else
                    //        {
                    //            litePerson person = Manager.Get<Person>(UC.CurrentUserID);
                    //            return OLDExportHelpers.SubmissionToCompile(call, submitter, GetRequiredFiles(call, idSubmitter, 0), GetSubmissionFields(call, idSubmitter, 0), person, translations, stream, fileType);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw new ExportError("", ex) { ErrorDocument = OLDExportHelpers.GetErrorDocument(translations, stream, fileType) };
                    //    }
                    //}
                #endregion

                #region "8 - Revision"
                    public Boolean RevisionIsReviewed(long idSubmission, long idRevision, List<long> idFields)
                    {
                        Boolean reviewed = (idFields.Count > 0);
                        try
                        {
                            //long idPrevius = (from r in Manager.GetIQ<Revision>()
                            //                  where r.Status == RevisionStatus.Approved && r.Deleted == BaseStatusDeleted.None && r.Submission != null && r.Submission.Id == idSubmission
                            //                  select r.Id).Skip(0).Take(1).ToList().FirstOrDefault();

                            //List<SubmissionFieldBaseValue> Oldvalues = (from v in Manager.GetIQ<SubmissionFieldBaseValue>()
                            //                                            where v.Deleted == BaseStatusDeleted.None && v.Revision.Id == idPrevius
                            //                                         select v).ToList();

                            List<SubmissionFieldBaseValue> values = (from v in Manager.GetIQ<SubmissionFieldBaseValue>()
                                                                     where v.Deleted == BaseStatusDeleted.None && v.Revision.Id == idRevision
                                                                     select v).ToList();
                            reviewed = (from v in values where v.CreatedBy.Id != v.ModifiedBy.Id select v.Id).Any();
                        }
                        catch (Exception ex)
                        {

                        }
                        return reviewed;
                    }
                    public List<dtoRevision> GetAvailableRevisions(long idSubmission, Boolean onlyAccepted)
                    {
                        return GetAvailableRevisions(Manager.Get<UserSubmission>(idSubmission), onlyAccepted);
                    }
                    public List<dtoRevision> GetAvailableRevisions(UserSubmission sub, Boolean onlyAccepted)
                    {
                        List<dtoRevision> revisions = new List<dtoRevision>();
                        try
                        {
                            if (sub != null && sub.Revisions != null)
                            {
                                revisions.AddRange(
                                sub.Revisions.Where(r => !onlyAccepted || (onlyAccepted && (r.IsActive || r.Type == RevisionType.Original
                                    || r.Status == RevisionStatus.Approved))).OrderByDescending(r => r.Id).Select(r => dtoRevision.Initialize(r)).ToList());
                            }
                        }
                        catch (Exception ex)
                        {
                            revisions = new List<dtoRevision>();
                        }
                        return revisions;
                    }
                    public dtoSubmissionRevision GetSubmissionWithRevisions(long idSubmission, Boolean full)
                    {
                        dtoSubmissionRevision dto = null;
                        try
                        {
                            UserSubmission sub = Manager.Get<UserSubmission>(idSubmission);
                            dto = GetSubmissionWithRevisions(sub, full);
                        }
                        catch (Exception ex)
                        {
                            dto = null;
                        }
                        return dto;
                    }
                    private dtoSubmissionRevision GetSubmissionWithRevisions(UserSubmission sub, Boolean full)
                    {
                        dtoSubmissionRevision dto = null;
                        try
                        {
                            dto = GetSubmissionDetails(sub);
                            if (dto != null && sub != null)
                            {
                                foreach (Revision rev in sub.Revisions.Where(r => r.Deleted == BaseStatusDeleted.None).ToList())
                                {
                                    if (rev.Type == RevisionType.Manager || rev.Type == RevisionType.UserRequired)
                                        dto.Revisions.Add(dtoRevisionRequest.Initialize((RevisionRequest)rev, full));
                                    else
                                        dto.Revisions.Add(dtoRevision.Initialize(rev));
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return dto;
                    }
                    private dtoSubmissionRevision GetSubmissionDetails(UserSubmission submission)
                    {
                        dtoSubmissionRevision dto = null;
                        try
                        {
                            if (submission != null)
                            {
                                dto = new dtoSubmissionRevision();
                                dto.Deleted = submission.Deleted;
                                dto.Id = submission.Id;
                                dto.Status = submission.Status;
                                dto.IdPerson = submission.Owner.Id;
                                dto.IdCall = submission.Call.Id;
                                dto.ExtensionDate = submission.ExtensionDate;
                                dto.Type = new dtoSubmitterType() { Id = submission.Type.Id };
                                dto.IsAnonymous = submission.isAnonymous;
                                dto.UniqueId = submission.UserCode;
                                dto.ModifiedOn = submission.ModifiedOn;
                                dto.SubmittedOn = submission.ModifiedOn;
                                dto.IdSubmittedBy = (submission.SubmittedBy == null) ? 0 : submission.SubmittedBy.Id;
                                dto.SubmittedBy = submission.SubmittedBy;
                                dto.Owner = submission.Owner;
                                dto.Type = (from s in Manager.GetIQ<SubmitterType>()
                                            where s.Id == dto.Type.Id
                                            select new dtoSubmitterType() { Deleted = s.Deleted, Name = s.Name, Id = s.Id }).Skip(0).Take(1).ToList().FirstOrDefault();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return dto;
                    }

                    //public dtoSubmissionRevision GetSubmissionDetails(long idSubmission, long idRevision, Boolean full)
                    //{
                    //    dtoSubmissionRevision dto = null;
                    //    try
                    //    {
                    //        UserSubmission sub = Manager.Get<UserSubmission>(idSubmission);
                    //        if (sub != null)
                    //        {
                    //            dto = new dtoSubmissionRevision();
                    //            dto.Deleted = sub.Deleted;
                    //            dto.Id = sub.Id;
                    //            dto.Status = sub.Status;
                    //            dto.IdPerson = sub.Owner.Id;
                    //            dto.IdCall = sub.Call.Id;
                    //            dto.ExtensionDate = sub.ExtensionDate;
                    //            dto.Type = new dtoSubmitterType() { Id = sub.Type.Id };
                    //            dto.IsAnonymous = sub.isAnonymous;
                    //            dto.UniqueId = sub.UserCode;
                    //            dto.ModifiedOn = sub.ModifiedOn;
                    //            dto.SubmittedOn = sub.ModifiedOn;
                    //            dto.IdSubmittedBy = (sub.SubmittedBy == null) ? 0 : sub.SubmittedBy.Id;

                    //            Revision revision = null;
                    //            if (idRevision == 0 || !sub.Revisions.Where(r => r.Id == idRevision).Any())
                    //                revision = sub.Revisions.Where(r => r.IsActive).OrderByDescending(r => r.Id).FirstOrDefault();
                    //            else
                    //                revision = sub.Revisions.Where(r => r.Id == idRevision).FirstOrDefault();
                    //            if (revision != null)
                    //            {
                    //                dto.InitializeRevision(revision, full);
                    //            }

                    //            dto.SubmittedBy = sub.SubmittedBy;
                    //            dto.Owner = sub.Owner;
                    //            dto.Type = (from s in Manager.GetIQ<SubmitterType>()
                    //                        where s.Id == dto.Type.Id
                    //                        select new dtoSubmitterType() { Deleted = s.Deleted, Name = s.Name, Id = s.Id }).Skip(0).Take(1).ToList().FirstOrDefault();
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //    return dto;
                    //}
                    public dtoRevision GetRevision(long idSubmission, long idRevision, Boolean full)
                    {
                        dtoRevision dto = null;
                        try
                        {
                            Revision revision = (from r in Manager.GetIQ<Revision>()
                                                 where r.Id == idRevision && r.Submission != null && r.Submission.Id == idSubmission
                                                 select r).Skip(0).Take(1).ToList().FirstOrDefault();

                            if (revision != null)
                            {
                                if (revision is OriginalRevision)
                                    dto = dtoRevision.Initialize(revision);
                                else if (revision is RevisionRequest)
                                    dto = dtoRevisionRequest.Initialize((RevisionRequest)revision, full);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return dto;
                    }
                    public dtoRevisionRequest GetRevisionRequest(long idRevision, Boolean full)
                    {
                        dtoRevisionRequest dto = null;
                        try
                        {
                            RevisionRequest revision = Manager.Get<RevisionRequest>(idRevision);
                            if (revision != null)
                                dto = dtoRevisionRequest.Initialize((RevisionRequest)revision, full);
                        }
                        catch (Exception ex)
                        {

                        }
                        return dto;
                    }
                    public long GetIdSubmissionFromRevision(long idRevision)
                    {
                        long idSubmission = 0;
                        try
                        {
                            idSubmission = (from r in Manager.GetIQ<RevisionRequest>() where r.Id == idRevision select r.Submission.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        return idSubmission;
                    }
                    public RevisionRequest AskForRevision(long idSubmission, Int32 idUser, string reason, string webSiteurl, string url, dtoRevisionMessage message)
                    {
                        RevisionRequest rev = null;
                        try
                        {
                            Manager.BeginTransaction();
                            UserSubmission sub = Manager.Get<UserSubmission>(idSubmission);
                            litePerson person = Manager.GetLitePerson(idUser);
                            if (sub != null && person != null && !sub.HasWorkingRevision())
                            {
                                rev = new RevisionRequest() { Deleted = BaseStatusDeleted.None, IsActive = false, Reason = reason, Type = RevisionType.UserRequired };
                                rev.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = RevisionStatus.Request;
                                rev.RequiredBy = person;
                                rev.RequiredTo = sub.Call.CreatedBy;
                                rev.Submission = sub;
                                //rev.RequiredTo

                                sub.Revisions.Add(rev);
                                Manager.SaveOrUpdate(sub);
                            }
                            Manager.Commit();
                            if (rev != null && sub != null)
                                RevisionMailNotification(sub, rev, person, sub.Call.CreatedBy, reason, webSiteurl, url, message);
                        }
                        catch (Exception ex)
                        {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                        }
                        return rev;
                    }
                    public RevisionRequest RequireRevision(long idSubmission, Int32 idUser,Int32 containerIdCommunity, string reason, dtoRevisionMessage message, List<dtoRevisionItem> fieldsToReview, DateTime deadline, string webSiteurl, String baseFilePath, String baseThumbnailPath)
                    {
                        RevisionRequest rev = null;
                        try
                        {
                            Manager.BeginTransaction();
                            UserSubmission sub = Manager.Get<UserSubmission>(idSubmission);
                            litePerson person = Manager.GetLitePerson(idUser);
                            String url = "";
                            if (sub != null && person != null && !sub.HasWorkingRevision())
                            {
                                rev = new RevisionRequest() { Deleted = BaseStatusDeleted.None, IsActive = false, Reason = reason, Type = RevisionType.Manager, Submission = sub };
                                rev.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = RevisionStatus.Required;
                                rev.EndDate = deadline;
                                rev.RequiredBy = person;
                                rev.RequiredTo = sub.Owner;
                                if (fieldsToReview != null)
                                {
                                    foreach (dtoRevisionItem item in fieldsToReview)
                                    {
                                        RevisionItem revItem = new RevisionItem();
                                        revItem.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        revItem.Revision = rev;
                                        revItem.Type = item.Type;
                                        revItem.Field = Manager.Get<FieldDefinition>(item.IdField);

                                        if (revItem.Field != null && revItem.Field.Id > 0)
                                        {
                                            Manager.SaveOrUpdate(revItem);
                                            rev.ItemsToReview.Add(revItem);
                                        }
                                    }
                                    if (fieldsToReview.Count > 0)
                                        Manager.SaveOrUpdate(rev);
                                    CreateSubmissionRevision(sub, rev, person, baseFilePath, baseThumbnailPath);
                                }
                                sub.Revisions.Add(rev);
                                Manager.SaveOrUpdate(sub);
                                url = RootObject.UserReviewCall(sub.Call.Type, sub.Call.Id, idSubmission, rev.Id, CallStatusForSubmitters.Submitted, containerIdCommunity);
                            }
                            Manager.Commit();
                            if (rev != null && sub != null && sub.Owner != null && (sub.Owner.TypeID != (Int32)UserTypeStandard.PublicUser && sub.Owner.TypeID != (Int32)UserTypeStandard.Guest))
                                RevisionMailNotification(sub, rev, person, sub.Owner, reason, webSiteurl, url, message);
                        }
                        catch (Exception ex)
                        {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                        }
                        return rev;
                    }
                    public Boolean RemoveSelfRequest(CallForPaperType type, long idSubmission, long idRevision, Int32 idUser,Int32 idContainerCommunity, dtoRevisionMessage selfRemoveMessage, string webSiteurl)
                    {
                        Boolean removed = false;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(idUser);

                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);
                            removed = (rev == null);
                            if (rev != null && person != null && (rev.Status != RevisionStatus.Approved))
                            {
                                rev.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = RevisionStatus.Cancelled;
                                Manager.SaveOrUpdate(rev);
                            }
                            Manager.Commit();
                            removed = (rev != null && person != null && (rev.Status == RevisionStatus.Cancelled));

                            if (removed && rev.Submission != null && rev.Submission.Call != null)
                                RevisionMailNotification(rev.Submission, rev, person, rev.Submission.Call.CreatedBy, "", webSiteurl, RootObject.ViewSubmission(type, rev.Submission.Call.Id, idSubmission, false, CallStatusForSubmitters.Submitted, idContainerCommunity, 0), selfRemoveMessage);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return removed;
                    }
                    public Boolean RemoveUserRequest(CallForPaperType type, long idSubmission, long idRevision, Int32 idUser,Int32 idContainerCommunity, string reason, dtoRevisionMessage managerMessage, String webSiteurl)
                    {
                        Boolean removed = false;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(idUser);

                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);
                            removed = (rev == null);
                            if (rev != null && person != null && (rev.Status != RevisionStatus.Approved))
                            {
                                rev.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = RevisionStatus.Cancelled;
                                rev.Feedback = reason;
                                Manager.SaveOrUpdate(rev);
                            }
                            Manager.Commit();
                            removed = (rev != null && person != null && (rev.Status == RevisionStatus.Cancelled));

                            if (removed && rev.Submission != null && rev.RequiredBy != null && (rev.RequiredBy.TypeID != (Int32)UserTypeStandard.PublicUser && rev.RequiredBy.TypeID != (Int32)UserTypeStandard.Guest))
                                RevisionMailNotification(rev.Submission, rev, person, rev.RequiredBy, reason, webSiteurl, RootObject.ViewSubmission(type, rev.Submission.Call.Id, idSubmission, false, CallStatusForSubmitters.Submitted, idContainerCommunity, 0), managerMessage);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return removed;
                    }
                    public Boolean RemoveManagerRequest(CallForPaperType type, long idSubmission, long idRevision, Int32 idUser, Int32 idContainerCommunity, string reason, dtoRevisionMessage managerMessage, String webSiteurl)
                    {
                        Boolean removed = false;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(idUser);

                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);
                            removed = (rev == null);
                            if (rev != null && person != null && (rev.Status != RevisionStatus.Approved))
                            {
                                rev.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = RevisionStatus.Cancelled;
                                rev.Feedback = reason;
                                Manager.SaveOrUpdate(rev);
                            }
                            Manager.Commit();
                            removed = (rev != null && person != null && (rev.Status == RevisionStatus.Cancelled));

                            if (removed && rev.Submission != null && rev.Submission.Owner != null && (rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.PublicUser && rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.Guest))
                                RevisionMailNotification(rev.Submission, rev, person, rev.Submission.Owner, reason, webSiteurl, RootObject.ViewSubmission(type, rev.Submission.Call.Id, idSubmission, false, CallStatusForSubmitters.Submitted, idContainerCommunity, 0), managerMessage);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return removed;
                    }

                    public Boolean AcceptUserRequest(Int32 idUser, long idSubmission, long idRevision, string reason, dtoRevisionMessage message, List<dtoRevisionItem> fieldsToReview, DateTime deadline, string webSiteurl, String baseFilePath, String baseThumbnailPath)
                    {

                        Boolean accepted = false;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(idUser);

                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);
                            if (rev != null && person != null && (rev.Status != RevisionStatus.Approved))
                            {
                                rev.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = RevisionStatus.RequestAccepted;
                                rev.Feedback = reason;
                                rev.EndDate = deadline;
                                if (fieldsToReview != null)
                                {
                                    foreach (dtoRevisionItem item in fieldsToReview)
                                    {
                                        RevisionItem revItem = new RevisionItem();
                                        revItem.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        revItem.Revision = rev;

                                        revItem.Type = item.Type;
                                        revItem.Field = Manager.Get<FieldDefinition>(item.IdField);
                                        if (revItem.Field != null && revItem.Field.Id > 0)
                                        {
                                            Manager.SaveOrUpdate(revItem);
                                            rev.ItemsToReview.Add(revItem);
                                        }
                                    }
                                }
                                Manager.SaveOrUpdate(rev);
                                CreateSubmissionRevision(rev.Submission, rev, person, baseFilePath, baseThumbnailPath);
                            }
                            Manager.Commit();
                            accepted = (rev != null && person != null && (rev.Status == RevisionStatus.RequestAccepted));

                            if (accepted && rev.Submission != null && rev.Submission.Owner != null && (rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.PublicUser && rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.Guest))
                                RevisionMailNotification(rev.Submission, rev, person, rev.Submission.Owner, reason, webSiteurl, RootObject.UserReviewCall(rev.Submission.Call.Type, rev.Submission.Call.Id, idSubmission, rev.Id, CallStatusForSubmitters.Submitted,-1), message);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return accepted;
                    }
                    public Boolean RefuseUserRequest(CallForPaperType type, Int32 idUser, Int32 idContainerCommunity, long idSubmission, long idRevision, string reason, dtoRevisionMessage message, string webSiteurl)
                    {
                        Boolean removed = false;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(idUser);

                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);
                            removed = (rev == null);
                            if (rev != null && person != null && (rev.Status != RevisionStatus.Approved))
                            {
                                rev.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = RevisionStatus.Refused;
                                rev.Feedback = reason;
                                Manager.SaveOrUpdate(rev);
                            }
                            Manager.Commit();
                            removed = (rev != null && person != null && (rev.Status == RevisionStatus.Refused));

                            if (removed && rev.Submission != null && rev.Submission.Owner != null && (rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.PublicUser && rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.Guest))
                                RevisionMailNotification(rev.Submission, rev, person, rev.Submission.Owner, reason, webSiteurl, RootObject.ViewSubmission(type, rev.Submission.Call.Id, idSubmission, false, CallStatusForSubmitters.Submitted, idContainerCommunity, 0), message);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return removed;
                    }
                    public Boolean ManageUserRevision(Boolean accept, CallForPaperType type, Int32 idUser,Int32 idContainerCommunity, long idSubmission, long idRevision, string reason, dtoRevisionMessage message, string webSiteurl)
                    {
                        Boolean managed = false;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(idUser);

                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);

                            if (rev != null && person != null && (rev.Status != RevisionStatus.Approved))
                            {
                                rev.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                rev.Status = (accept) ? RevisionStatus.Approved : RevisionStatus.Refused;
                                if (accept)
                                    rev.Number = (from r in Manager.GetIQ<Revision>() where r.Deleted == BaseStatusDeleted.None && r.IsActive && r.Submission.Id == idSubmission select r.Number).Max() + 1;
                                rev.Feedback = reason;
                                rev.IsActive = accept;
                                Manager.SaveOrUpdate(rev);
                            }
                            Manager.Commit();
                            managed = (rev != null && person != null && ((accept && rev.Status == RevisionStatus.Approved) || (!accept && rev.Status == RevisionStatus.Refused)));

                            if (managed && rev.Submission != null && rev.Submission.Owner != null && (rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.PublicUser && rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.Guest))
                                RevisionMailNotification(rev.Submission, rev, person, rev.Submission.Owner, reason, webSiteurl, RootObject.ViewSubmission(type, rev.Submission.Call.Id, idSubmission, false, CallStatusForSubmitters.Submitted, idContainerCommunity, 0), message);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return managed;
                    }

                    public lm.Comol.Modules.CallForPapers.Presentation.RevisionErrorView SaveRevisionSettings(CallForPaperType type, long idCall, long idSubmission, long idRevision, Int32 idUser, DateTime deadline, List<dtoRevisionItem> fieldsToReview, dtoRevisionMessage userMessage, String webSiteurl)
                    {
                        lm.Comol.Modules.CallForPapers.Presentation.RevisionErrorView result = Presentation.RevisionErrorView.None;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(idUser);

                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);

                            if (rev != null && person != null && (rev.Status != RevisionStatus.Approved))
                            {
                                List<long> idFields = fieldsToReview.Select(f => f.IdField).ToList();
                                List<long> idCurrentFields = rev.ItemsToReview.Where(i => i.Deleted == BaseStatusDeleted.None).Select(i => i.Field.Id).ToList();
                                List<long> idToValidate = new List<long>();
                                idToValidate.AddRange(idFields);
                                idToValidate.AddRange(idCurrentFields);
                                if (RevisionIsReviewed(idSubmission, rev.Id, idToValidate))
                                    result = Presentation.RevisionErrorView.FieldsNotEditable;
                                else
                                {
                                    rev.ItemsToReview.Where(i => i.Deleted != BaseStatusDeleted.None && idFields.Contains(i.Field.Id)).ToList().ForEach(i => i.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    rev.ItemsToReview.Where(i => i.Deleted == BaseStatusDeleted.None && !idFields.Contains(i.Field.Id)).ToList().ForEach(i => i.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));

                                    List<RevisionItem> test1 = rev.ItemsToReview.Where(i => i.Deleted != BaseStatusDeleted.None && idFields.Contains(i.Field.Id)).ToList();
                                    List<RevisionItem> test2 = rev.ItemsToReview.Where(i => i.Deleted == BaseStatusDeleted.None && !idFields.Contains(i.Field.Id)).ToList();
                                    foreach (dtoRevisionItem item in fieldsToReview.Where(f => !idCurrentFields.Contains(f.IdField)).ToList())
                                    {
                                        RevisionItem revItem = new RevisionItem();
                                        revItem.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        revItem.Revision = rev;

                                        revItem.Type = item.Type;
                                        revItem.Field = Manager.Get<FieldDefinition>(item.IdField);
                                        if (revItem.Field != null && revItem.Field.Id > 0)
                                        {
                                            Manager.SaveOrUpdate(revItem);
                                            rev.ItemsToReview.Add(revItem);
                                        }
                                    }
                                }
                                rev.EndDate = deadline;
                                rev.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(rev);
                            }
                            Manager.Commit();

                            if (rev.Submission != null && rev.Submission.Owner != null && (rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.PublicUser && rev.Submission.Owner.TypeID != (Int32)UserTypeStandard.Guest))
                                RevisionMailNotification(rev.Submission, rev, person, rev.Submission.Owner, rev.ItemsToReview.Where(i => i.Deleted == BaseStatusDeleted.None).ToList(), webSiteurl, RootObject.UserReviewCall(type, idCall, idSubmission, rev.Id, CallStatusForSubmitters.Submitted,-1), userMessage);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            if (result == Presentation.RevisionErrorView.None)
                                result = Presentation.RevisionErrorView.SavingSettings;
                        }
                        return result;
                    }


                    private void RevisionMailNotification(UserSubmission submission, RevisionRequest rev, litePerson sender, litePerson receiver, string reason, string webSiteurl, string url, dtoRevisionMessage revMessage)
                    {
                        if (submission.Call != null)
                        {
                            lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings = new lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings() { CopyToSender = false, IsBodyHtml = true, NotifyToSender = true,  SenderType= SenderUserType.LoggedUser,  PrefixType= SubjectPrefixType.SystemConfiguration };
                            lm.Comol.Core.Mail.MailService mailService = new lm.Comol.Core.Mail.MailService(revMessage.SmtpConfig, settings);

                            String subject = revMessage.Subject;
                            String body = revMessage.Body;

                            if (rev.RequiredBy != null)
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.RequiredBy), rev.RequiredBy.SurnameAndName);

                            if (rev.Status == RevisionStatus.Submitted && sender != null)
                            {
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.SubmitterSurname), sender.Surname);
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.SubmitterName), sender.Name);
                            }
                            else if ((rev.Status == RevisionStatus.Approved || rev.Status == RevisionStatus.Refused) && receiver != null)
                            {
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.SubmitterSurname), receiver.Surname);
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.SubmitterName), receiver.Name);
                            }

                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.CallName), submission.Call.Name);
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.RequestOn), (rev.Status == RevisionStatus.Request || rev.Status == RevisionStatus.Required) ? rev.ModifiedOn.Value.ToString("dd  MMMM  yyyy") : rev.CreatedOn.Value.ToString("dd  MMMM  yyyy"));
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.RequestTime), (rev.Status == RevisionStatus.Request || rev.Status == RevisionStatus.Required) ? rev.ModifiedOn.Value.ToString("dd  MMMM  yyyy") : rev.CreatedOn.Value.ToShortTimeString());
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.LinkUrl), webSiteurl + url);

                            if (rev.EndDate.HasValue)
                            {
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.DeadlineOn), rev.CreatedOn.Value.ToString("dd  MMMM  yyyy"));
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.DeadlineTime), rev.CreatedOn.Value.ToShortTimeString());
                            }
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.Reason), reason);

                            lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(subject, body);
                            if (sender == null)
                                message.FromUser = revMessage.SmtpConfig.GetSystemSender();
                            else
                                message.FromUser = new System.Net.Mail.MailAddress(sender.Mail, sender.SurnameAndName);


                            if (receiver != null && !string.IsNullOrEmpty(receiver.Mail))
                                mailService.SendMail((receiver == null) ? 0 : receiver.LanguageID, Manager.GetDefaultLanguage(), message, receiver.Mail, RecipientType.To);
                        }
                    }
                    private void RevisionMailNotification(UserSubmission submission, RevisionRequest rev, litePerson sender, litePerson receiver, List<RevisionItem> items, string webSiteurl, string url, dtoRevisionMessage revMessage)
                    {
                        if (submission.Call != null)
                        {
                            lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings = new lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings() { CopyToSender = false, IsBodyHtml = true, NotifyToSender = true,  SenderType=  SenderUserType.LoggedUser, PrefixType = SubjectPrefixType.SystemConfiguration   };
                            lm.Comol.Core.Mail.MailService mailService = new lm.Comol.Core.Mail.MailService(revMessage.SmtpConfig, settings);

                            String subject = revMessage.Subject;
                            String body = revMessage.Body;

                            if (rev.RequiredBy != null)
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.RequiredBy), rev.RequiredBy.SurnameAndName);

                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.SubmitterSurname), sender.Surname);
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.SubmitterName), sender.Name);

                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.CallName), submission.Call.Name);
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.RequestOn), (rev.Status == RevisionStatus.Request || rev.Status == RevisionStatus.Required) ? rev.ModifiedOn.Value.ToString("dd  MMMM  yyyy") : rev.CreatedOn.Value.ToString("dd  MMMM  yyyy"));
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.RequestTime), (rev.Status == RevisionStatus.Request || rev.Status == RevisionStatus.Required) ? rev.ModifiedOn.Value.ToString("dd  MMMM  yyyy") : rev.CreatedOn.Value.ToShortTimeString());
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.LinkUrl), webSiteurl + url);

                            if (rev.EndDate.HasValue)
                            {
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.DeadlineOn), rev.CreatedOn.Value.ToString("dd  MMMM  yyyy"));
                                body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.DeadlineTime), rev.CreatedOn.Value.ToShortTimeString());
                            }

                            String fields = "";
                            foreach (RevisionItem item in items)
                            {
                                fields = item.Field.Name + "<br/>";
                            }
                            body = body.Replace(dtoRevisionMessage.GetPlaceHolder(dtoRevisionMessage.RevisionPlaceHoldersType.Fields), fields);

                            lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(subject, body);
                            if (sender == null)
                                message.FromUser = revMessage.SmtpConfig.GetSystemSender();
                            else
                                message.FromUser = new System.Net.Mail.MailAddress(sender.Mail, sender.SurnameAndName);


                            if (receiver != null && !string.IsNullOrEmpty(receiver.Mail))
                                mailService.SendMail((sender == null) ? 0 : sender.LanguageID,Manager.GetDefaultLanguage(), message, receiver.Mail, RecipientType.To);
                        }
                    }
                    public long GetRevisionCount(UserSubmission submission, long idRevision, FieldDefinition field)
                    {
                        if (submission == null || idRevision == 0)
                            return 0;
                        else
                        {
                            List<RevisionRequest> revs = submission.Revisions.Where(r => r.Id <= idRevision && r.Status == RevisionStatus.Approved && r.Type != RevisionType.Original).Select(r => (RevisionRequest)r).ToList();

                            return revs.Where(r => r.ItemsToReview.Where(i => i.Field.Id == field.Id).Any()).Count();
                        }
                    }

                    public Boolean RevisionWithFileToUpload(long idRevision)
                    {
                        Boolean result = false;
                        if (idRevision != 0)
                        {
                            RevisionRequest rev = Manager.Get<RevisionRequest>(idRevision);
                            if (rev != null)
                            {
                                result = rev.ItemsToReview.Where(f => f.Field.Type == FieldType.FileInput).Any();
                            }
                        }
                        return result;
                    }
                    private void CreateSubmissionRevision(UserSubmission sub, Revision rev, litePerson person, String baseFilePath, String baseThumbnailPath)
                    {
                        long idLastRevision = sub.Revisions.Where(r => r.IsActive).OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
                        if (idLastRevision > 0 && rev != null)
                        {
                            List<SubmissionFieldBaseValue> values = (from v in Manager.GetIQ<SubmissionFieldBaseValue>()
                                                                     where v.Deleted == BaseStatusDeleted.None && v.Submission.Id == sub.Id
                                                                     && v.Revision.Id == idLastRevision
                                                                     select v).OrderBy(f => f.Field.Section.DisplayOrder).ThenBy(f => f.Field.DisplayOrder).ToList();

                            List<SubmissionFieldStringValue> stringValues = new List<SubmissionFieldStringValue>();
                            //List<SubmissionFieldFileValue> fileValues = new List<SubmissionFieldFileValue>();

                            String moduleCode = (sub.Call.Type == CallForPaperType.CallForBids ? ModuleCallForPaper.UniqueCode : ModuleRequestForMembership.UniqueCode);
                            Int32 idModule = ServiceModuleID(moduleCode);
                            lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier rIdentifier = CreateRepositoryIdentifier(sub.Call);
                            foreach (SubmissionFieldBaseValue item in values)
                            {
                                if (item is SubmissionFieldStringValue)
                                {
                                    SubmissionFieldStringValue fieldValue = new SubmissionFieldStringValue();
                                    fieldValue.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    fieldValue.Submission = sub;
                                    fieldValue.Revision = rev;
                                    fieldValue.Field = item.Field;
                                    fieldValue.Value = ((SubmissionFieldStringValue)item).Value;
                                    fieldValue.UserValue = ((SubmissionFieldStringValue)item).UserValue;
                                    stringValues.Add(fieldValue);
                                }
                                else if (item is SubmissionFieldFileValue)
                                {
                                    SubmissionFieldFileValue itemFile = (SubmissionFieldFileValue)item;
                                    SubmissionFieldFileValue fileValue = new SubmissionFieldFileValue();
                                    fileValue.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    fileValue.Submission = sub;
                                    fileValue.Revision = rev;
                                    fileValue.Field = item.Field;
                                    if (itemFile.Item != null)
                                    {
                                        lm.Comol.Core.FileRepository.Domain.liteRepositoryItem lItem = null;
                                        lm.Comol.Core.FileRepository.Domain.liteRepositoryItemVersion lVersion = null;

                                        //lm.Comol.Core.FileRepository.Domain.RepositoryItem file = DuplicateFile(itemFile.Item, baseFilePath, baseThumbnailPath);
                                        lm.Comol.Core.FileRepository.Domain.RepositoryItem cItem = null;
                                        lm.Comol.Core.FileRepository.Domain.RepositoryItemVersion cVersion = null;
                                        RepositoryService.CloneInternalItem(sub.Id, idModule, moduleCode, itemFile.Item.Id, 0, person, baseFilePath, baseThumbnailPath, rIdentifier, ref cItem, ref cVersion);
                                        if (cItem != null)
                                        {
                                            lItem = RepositoryService.ItemGet(cItem.Id);
                                            //if (cVersion != null)
                                            //    version = RepositoryService.VersionGet(cVersion.Id);
                                            fileValue.Item = lItem;
                                        }

                                        if (lItem == null)
                                            throw new Exception();

                                        Manager.SaveOrUpdate(fileValue);
                                        if (fileValue.Item != null)
                                        {
                                            liteModuleLink link = new liteModuleLink(itemFile.Link.Description, itemFile.Link.Permission, itemFile.Link.Action);
                                            link.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                                            link.DestinationItem = CreateModuleObject(sub, fileValue.Item);
                                            link.SourceItem = CreateModuleObject(sub);
                                            Manager.SaveOrUpdate(link);
                                            fileValue.Link = link;
                                            Manager.SaveOrUpdate(fileValue);
                                        }
                                    }
                                }
                            }
                            if (stringValues.Count > 0)
                                Manager.SaveOrUpdateList(stringValues);
                        }
                    }

                    //private lm.Comol.Core.FileRepository.Domain.RepositoryItem DuplicateFile(lm.Comol.Core.FileRepository.Domain.RepositoryItem source, String baseFilePath, String baseThumbnailPath)
                    //{
                    //    RepositoryService.CloneInternalItem()
                    //    ModuleLongInternalFile file = new ModuleLongInternalFile();
                    //    file.AllowUpload = source.AllowUpload;
                    //    file.CloneID = source.Id;
                    //    file.CloneUniqueID = source.UniqueID;
                    //    file.CommunityOwner = community;
                    //    file.ContentType = source.ContentType;
                    //    file.CreatedOn = DateTime.Now;
                    //    file.Description = source.Description;
                    //    file.Downloads = 0;
                    //    file.Extension = source.Extension;
                    //    file.FileCategoryID = source.FileCategoryID;
                    //    file.FilePath = source.FilePath;
                    //    file.FolderId = source.FolderId;
                    //    file.isDeleted = false;
                    //    file.IsDownloadable = source.IsDownloadable;
                    //    file.isFile = true;
                    //    file.IsInternal = true;
                    //    file.isPersonal = source.isPersonal;
                    //    file.isSCORM = source.isSCORM;
                    //    file.isVideocast = source.isVideocast;
                    //    file.isVirtual = source.isVirtual;
                    //    file.isVisible = source.isVisible;
                    //    file.ModifiedBy = source.ModifiedBy;
                    //    file.ModifiedOn = source.ModifiedOn;
                    //    file.Name = source.Name;
                    //    file.ObjectOwner = source.ObjectOwner;
                    //    file.ObjectTypeID = source.ObjectTypeID;
                    //    file.Owner = source.Owner;
                    //    file.RepositoryItemType = source.RepositoryItemType;
                    //    file.ServiceActionAjax = source.ServiceActionAjax;
                    //    file.ServiceOwner = source.ServiceOwner;
                    //    file.Size = source.Size;
                    //    file.UniqueID = Guid.NewGuid();

                    //    if (lm.Comol.Core.File.Create.CopyFile_FM(filePath + "\\" + source.UniqueID.ToString() + ".stored", filePath + "\\" + file.UniqueID.ToString() + ".stored") == FileMessage.FileCreated)
                    //        return file;
                    //    else
                    //        return null;
                    //}

                    public long GetIdLastActiveRevision(long idSubmission)
                    {
                        long idRevision = 0;
                        try
                        {
                            idRevision = (from r in Manager.GetIQ<Revision>()
                                          where r.Deleted == BaseStatusDeleted.None && r.Submission.Id == idSubmission && r.IsActive && r.Status == RevisionStatus.Approved
                                          select r.Id).Max();
                        }
                        catch (Exception ex) { 

                        }
                        return idRevision;
                    }


                    public List<dtoRevisionDisplayItemPermission> GetRevisionList(ModuleRequestForMembership module, CallStandardAction action, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, dtoRevisionFilters filters, int pageIndex, int pageSize)
                    {
                        List<dtoRevisionDisplayItemPermission> items = null;
                        try
                        {
                            List<dtoRevisionDisplay> revisions = GetRevisionList(CallForPaperType.RequestForMembership, action, fromAllcommunities, forPortal, community, person, filters, pageIndex, pageSize);

                            items = (from r in revisions select new dtoRevisionDisplayItemPermission() { Id = r.Id, Deleted = r.Deleted, Revision = r, Permission = new dtoRevisionRequestPermission(action,r) }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        return items;
                    }
                    public List<dtoRevisionDisplayItemPermission> GetRevisionList(ModuleCallForPaper module, CallStandardAction action, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, dtoRevisionFilters filters, int pageIndex, int pageSize)
                    {
                        List<dtoRevisionDisplayItemPermission> items = null;
                        try
                        {
                            List<dtoRevisionDisplay> revisions = GetRevisionList(CallForPaperType.CallForBids, action, fromAllcommunities, forPortal, community, person,filters,pageIndex, pageSize);

                            items = (from r in revisions select new dtoRevisionDisplayItemPermission() { Id = r.Id, Deleted = r.Deleted, Revision = r, Permission = new dtoRevisionRequestPermission(action, r) }).ToList();
                        }
                        catch (Exception ex) { 
                        
                        }
                        return items;
                    }
                    private List<dtoRevisionDisplay> GetRevisionList(CallForPaperType type, CallStandardAction action, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, dtoRevisionFilters filters, int pageIndex, int pageSize)
                    {
                        List<dtoRevisionDisplay> items = null;
                        var query = GetRevisionQuery(fromAllcommunities, forPortal, community, person, filters, type, (action == CallStandardAction.Manage) ? RevisionType.Manager : RevisionType.UserRequired);

                       

                        RevisionOrder orderBy = filters.OrderBy;
                        Boolean ascending = filters.Ascending;
                        if (orderBy== RevisionOrder.None){
                            orderBy = (action == CallStandardAction.Manage) ? RevisionOrder.ByDate: RevisionOrder.ByDeadline;
                            ascending= false;
                        }
                        switch( orderBy){
                            case RevisionOrder.ByCall:
                                if (ascending)
                                    query = query.OrderBy(r => r.Submission.Call.Name);
                                else
                                    query = query.OrderByDescending(r => r.Submission.Call.Name);
                                break;
                            case RevisionOrder.ByDate:
                                if (ascending)
                                    query = query.OrderBy(r => r.CreatedOn);
                                else
                                     query = query.OrderByDescending(r => r.CreatedOn);
                                break;
                            case RevisionOrder.ByDeadline:
                                if (ascending)
                                    query = query.OrderBy(r => r.EndDate);
                                else
                                    query = query.OrderByDescending(r => r.EndDate);
                                break;
                            case RevisionOrder.ByStatus:
                                if (ascending)
                                    query = query.OrderBy(r => filters.TranslationsStatus[r.Status]);
                                else
                                    query = query.OrderByDescending(r => filters.TranslationsStatus[r.Status]);
                                break;
                            case RevisionOrder.ByType:
                                if (ascending)
                                    query = query.OrderBy(r => filters.TranslationsType[r.Type]);
                                else
                                    query = query.OrderByDescending(r => filters.TranslationsType[r.Type]);
                                break;
                            case RevisionOrder.ByUser:
                                break;
                        }
                        //if  (action == CallStandardAction.Manage){
                        //    query = query.OrderByDescending(r=> r.EndDate).OrderByDescending(r=> r.EndDate);
                        //}
                        //else
                        //{
                        //    query = query(r=> r.EndDate).OrderByDescending(r=> r.EndDate);
                        //}

                        query = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        items = (from r in query
                                 select new dtoRevisionDisplay()
                                 {
                                     Type = r.Type,
                                     Status = r.Status,
                                     RequiredTo = r.RequiredTo,
                                     RequiredBy = r.RequiredBy,
                                     IdCall = (r.Submission != null && r.Submission.Call != null) ? r.Submission.Call.Id : 0,
                                     IdSubmission = (r.Submission != null) ? r.Submission.Id : 0,
                                     CallName= (r.Submission != null && r.Submission.Call !=null) ? r.Submission.Call.Name : "",
                                     CreatedBy = r.CreatedBy,
                                     CreatedOn = r.CreatedOn,
                                     Deleted = r.Deleted,
                                     EndDate = r.EndDate,
                                     IsActive = r.IsActive,
                                     Id = r.Id,
                                     Number = r.Number
                                 }).ToList();

                        return items;
                    }
                    private IEnumerable<RevisionRequest> GetRevisionQuery(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, dtoRevisionFilters filters, CallForPaperType type, RevisionType revType)
                    {
                        var query = GetBaseRevisionQuery(fromAllcommunities,forPortal,community,person,filters,type,revType);
                        String searchName = "";
                        if (!String.IsNullOrEmpty(filters.SearchForName))
                        {
                            searchName = filters.SearchForName.ToLower();
                            List<long> idRevisions = GetRevisionsByUser(GetBaseRevisionQuery(fromAllcommunities, forPortal, community, person, filters, type, revType), searchName);
                            query = query.Where(r => idRevisions.Contains(r.Id));
                        }
                        return query;
                    }
                    private IEnumerable<RevisionRequest> GetBaseRevisionQuery(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, dtoRevisionFilters filters, CallForPaperType type, RevisionType revType)
                    {
                        List<long> idAssignedCalls = new List<long>();
                        if (revType == RevisionType.UserRequired)
                            idAssignedCalls = GetIdCallsByCommunity(fromAllcommunities, forPortal, community, type, FilterCallVisibility.OnlyVisible);

                        var query = (from s in Manager.GetIQ<RevisionRequest>()
                                     where s.Deleted == BaseStatusDeleted.None && s.IsActive == false && 
                                     (fromAllcommunities 
                                     ||
                                     (s.Submission.Call.IsPortal == forPortal && forPortal) 
                                     ||
                                     (!forPortal && 
                                            (s.Submission.Community == community
                                            || idAssignedCalls.Contains(s.Submission.Call.Id)
                                            )
                                     )
                                     ) 
                                     
                                     && (s.Submission != null && s.Submission.Call != null && s.Submission.Call.Type == type)
                                     select s);
                        switch (revType)
                        {
                            case RevisionType.Manager:
                                query = query.Where(r => ((filters.Status != RevisionStatus.None && r.Status == filters.Status) || (filters.Status == RevisionStatus.None && (r.Status != RevisionStatus.Approved && r.Status != RevisionStatus.Cancelled))) && ((r.Type == RevisionType.Manager && r.RequiredBy == person) || (r.Type == RevisionType.UserRequired && r.RequiredTo == person)));
                                break;
                            case RevisionType.UserRequired:
                                query = query.Where(r => ((filters.Status != RevisionStatus.None && r.Status == filters.Status) || (filters.Status == RevisionStatus.None && (r.Status != RevisionStatus.Approved && r.Status != RevisionStatus.Cancelled))) && ((r.Type == RevisionType.Manager && r.RequiredTo == person) || (r.Type == RevisionType.UserRequired && r.RequiredBy == person)));
                                break;
                            default:
                                query = query.Where(r => r.Id == -1);
                                break;

                        }
                        return query;
                    }

                    public List<long> GetRevisionsByUser(IEnumerable<RevisionRequest> query, String searchName)
                    {
                        List<long> idRevisions = new List<long>();
                        var users = query.Where(r => r.Type == RevisionType.Manager).Select(r => new { IdPerson = r.RequiredTo.Id, idRevision = r.Id }).ToList();
                        users.AddRange(query.Where(r => r.Type == RevisionType.UserRequired).Select(r => new { IdPerson = r.RequiredBy.Id, idRevision = r.Id }).ToList());

                        Int32 pageSize = 50;
                        Int32 pageIndex = 0;
                        List<Int32> idUsers = users.Select(u => u.IdPerson).ToList();
                        List<Int32> idPersons = new List<Int32>();

                        var userQuery = idUsers.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        while (userQuery.Any()) { 
                            idPersons.AddRange(
                                (from p in Manager.GetIQ<Person>() where userQuery.Contains(p.Id) && (p.Name.Contains(searchName) || p.Surname.Contains(searchName)) select p.Id).ToList());
                            pageIndex++;
                            userQuery = idUsers.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        }
                        idRevisions = (from r in users where idPersons.Contains(r.IdPerson) select r.idRevision).Distinct().ToList();

                        return idRevisions;
                    }
                    public List<RevisionStatus> GetAvailableRevisionStatus(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, dtoRevisionFilters filters, RevisionType revType)
                    {
                        List<RevisionStatus> items = new List<RevisionStatus>();
                        if (!(person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser))
                        {
                            var query = GetRevisionQuery(fromAllcommunities, forPortal, community, person, filters, type, revType);
                            items = query.Select(r => r.Status).ToList().Distinct().ToList();
                        }
                        return items;
                    }
                    public long RevisionCount(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, dtoRevisionFilters filters, RevisionType revType)
                    {
                        long count = 0;
                        if (person!= null && !(person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser))
                        {
                            var query = GetRevisionQuery(fromAllcommunities, forPortal, community, person, filters, type, revType);
                            count = query.Count();
                        }
                        return count;
                    }
                #endregion

                #region "8.1 - Save Submission Revision"
                    public UserSubmission SaveSubmissionRevision(long idSubmission, long idRevision, int idOwner, List<dtoSubmissionValueField> fields)
                    {
                        litePerson person = null;
                        if (idOwner == 0)
                            person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                        else
                            person = Manager.GetLitePerson(idOwner);
                        return SaveSubmissionRevision(person, idSubmission, idRevision, fields);
                    }
                    private UserSubmission SaveSubmissionRevision(litePerson submitter, long idSubmission, long idRevision, List<dtoSubmissionValueField> fields)
                    {
                        UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);
                        RevisionRequest revision = Manager.Get<RevisionRequest>(idRevision);
                        if (submission == null || revision == null)
                            throw new SubmissionNotFound(idSubmission.ToString());
                        else
                        {
                            try
                            {
                                Manager.BeginTransaction();

                                List<SubmissionFieldStringValue> values = new List<SubmissionFieldStringValue>();

                                if (fields.Count > 0)
                                {
                                    foreach (dtoSubmissionValueField item in fields.Where(f => f.Field.Type != FieldType.FileInput).ToList())
                                    {
                                        SubmissionFieldStringValue fieldValue = Manager.Get<SubmissionFieldStringValue>(item.IdValueField);
                                        if (fieldValue == null)
                                        {
                                            FieldDefinition definition = Manager.Get<FieldDefinition>(item.IdField);
                                            if (definition.Call == submission.Call)
                                            {
                                                fieldValue = new SubmissionFieldStringValue();
                                                fieldValue.CreateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);
                                                fieldValue.Submission = submission;
                                                fieldValue.Revision = revision;
                                                fieldValue.Field = definition;
                                                fieldValue.Value = (item.Value ==null) ? "" : item.Value.Text ;
                                                fieldValue.UserValue = (item.Value == null) ? "" : item.Value.FreeText;
                                                values.Add(fieldValue);
                                            }
                                        }
                                        else if (fieldValue.Field.Call == submission.Call)
                                        {
                                            fieldValue.UpdateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);
                                            fieldValue.Value = (item.Value == null) ? "" : item.Value.Text;
                                            fieldValue.UserValue = (item.Value == null) ? "" : item.Value.FreeText;
                                            values.Add(fieldValue);
                                        }
                                    }
                                    if (values.Count > 0)
                                        Manager.SaveOrUpdateList(values);
                                }
                                submission.UpdateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);
                                revision.UpdateMetaInfo(submitter, UC.IpAddress, UC.ProxyIpAddress);

                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                throw new SubmissionNotSaved(ex.Message, ex);
                            }
                        }
                        return submission;
                    }
                    public void SaveSubmissionRevisionFiles(UserSubmission submission, long idRevision, int idOwner, List<dtoSubmissionFileValueField> fileValues)
                    {
                        try
                        {
                            RevisionRequest revision = Manager.Get<RevisionRequest>(idRevision);
                            litePerson owner = Manager.GetLitePerson(idOwner);
                            if (owner != null && submission != null && revision != null)
                            {

                                Manager.BeginTransaction();

                                foreach (dtoSubmissionFileValueField item in fileValues)
                                {
                                    SubmissionFieldFileValue toCreate = new SubmissionFieldFileValue();
                                    toCreate.CreateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    toCreate.Submission = submission;
                                    toCreate.Revision = revision;
                                    toCreate.Field = Manager.Get<FieldDefinition>(item.IdField);
                                    toCreate.Item = (lm.Comol.Core.FileRepository.Domain.liteRepositoryItem)item.ActionLink.ModuleObject.ObjectOwner;
                                    SubmissionFieldFileValue fileValue = (from f in Manager.GetIQ<SubmissionFieldFileValue>()
                                                                          where f.Submission == submission && f.Deleted == BaseStatusDeleted.None && f.Field.Id == item.IdField
                                                                          && f.Revision.Id == revision.Id
                                                                          select f).Skip(0).Take(1).ToList().FirstOrDefault();
                                    if (fileValue != null)
                                    {
                                        fileValue.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                        fileValue.Item.Deleted = BaseStatusDeleted.Manual;
                                        Manager.SaveOrUpdate(fileValue.Item);
                                        fileValue.Link.Deleted = BaseStatusDeleted.Automatic;
                                        fileValue.Deleted = BaseStatusDeleted.Manual;
                                        fileValue.Revision = revision;
                                        Manager.SaveOrUpdate(fileValue);
                                    }

                                    Manager.SaveOrUpdate(toCreate);
                                    liteModuleLink link = new liteModuleLink(item.ActionLink.Description, item.ActionLink.Permission, item.ActionLink.Action);
                                    link.CreateMetaInfo(owner.Id, UC.IpAddress, UC.ProxyIpAddress);
                                    link.DestinationItem = (ModuleObject)item.ActionLink.ModuleObject;
                                    link.SourceItem = CreateModuleObject(submission);
                                    Manager.SaveOrUpdate(link);
                                    toCreate.Link = link;
                                    Manager.SaveOrUpdate(toCreate);
                                }
                                Manager.Commit();
                            }
                            else if (owner == null)
                                throw new SubmissionInternalFileNotLinked("");
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            throw new SubmissionInternalFileNotLinked(ex.Message, ex);
                        }
                    }
                    public void CompleteSubmissionRevision(UserSubmission submission, long idRevision, int idOwner, DateTime initTime, String BaseFilePath, List<dtoSubmissionValueField> fields, String translationFileName, string webSiteurl, dtoRevisionMessage message)
                    {
                        litePerson person = null;
                        if (idOwner == 0)
                            person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                        else
                            person = Manager.GetLitePerson(idOwner);
                        CompleteSubmissionRevision(submission, Manager.Get<RevisionRequest>(idRevision), person, initTime, BaseFilePath, fields, translationFileName, webSiteurl, message);
                    }

                    private void CompleteSubmissionRevision(UserSubmission submission, RevisionRequest revision, litePerson owner, DateTime initTime, String BaseFilePath, List<dtoSubmissionValueField> fields, String translationFileName, string webSiteurl, dtoRevisionMessage message)
                    {
                        if (!AllowTimeRevision(revision, initTime))
                            throw new SubmissionTimeExpired();
                        else
                        {
                            if (AllowSubmissionComplete(submission, revision))
                            {
                                List<string> filesToRemove = new List<string>();
                                try
                                {
                                    Manager.BeginTransaction();
                                    Manager.Refresh(submission);
                                    submission.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    revision.UpdateMetaInfo(owner, UC.IpAddress, UC.ProxyIpAddress);
                                    revision.ModifiedOn = DateTime.Now;
                                    revision.Status = RevisionStatus.Submitted;
                                    revision.SubmittedBy = owner;
                                    revision.SubmittedOn = revision.ModifiedOn;
                                    filesToRemove = ZipSubmissionFiles(submission, revision, owner, BaseFilePath, translationFileName);

                                    Manager.SaveOrUpdate(revision);
                                    Manager.SaveOrUpdate(submission);
                                    Manager.Commit();
                                    RevisionMailNotification(submission, revision, owner, (revision.Type == RevisionType.Manager) ? revision.RequiredBy : revision.RequiredTo, "", webSiteurl, RootObject.ManageReviewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, revision.Id, CallStandardAction.Manage, CallStatusForSubmitters.Revisions), message);
                                    Delete.Files(filesToRemove);
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                    throw new SubmissionStatusChange(ex.Message, ex);
                                }
                            }
                            else
                                throw new SubmissionItemsEmpty();
                        }
                    }
                #endregion
            #endregion

            protected IQueryable<UserSubmission> GetBaseUserSubmissionQuery(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community,litePerson person, CallForPaperType type, FilterCallVisibility filter)
            {
                Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);
                IQueryable<UserSubmission> query = (from s in Manager.GetIQ<UserSubmission>()
                                                    where s.Deleted == BaseStatusDeleted.None && (!isAnonymous && s.Owner == person) && (fromAllcommunities || (s.Call.IsPortal == forPortal && forPortal) || (!forPortal && s.Community == community)) && (s.Call != null && s.Call.Type == type)
                                                    select s);
                return query;
            }
            protected List<dtoBaseSubmitterType> GetSubmissionAs(long idCall, List<long> submissions,litePerson person)
            {
                List<dtoBaseSubmitterType> results = new List<dtoBaseSubmitterType>();
                submissions.ForEach(s => results.AddRange(GetSubmissionAs(idCall, s, person)));
                return results;
            }
            protected List<dtoBaseSubmitterType> GetSubmissionAs(long idCall, long idSubmitterType,litePerson person)
            {
                List<dtoBaseSubmitterType> results = new List<dtoBaseSubmitterType>();
                SubmitterType type = Manager.Get<SubmitterType>(idSubmitterType);
                if (type != null && type.AllowMultipleSubmissions)
                {
                    int count = (from s in Manager.GetIQ<UserSubmission>()
                                    where s.Owner == person && s.Deleted == BaseStatusDeleted.None && s.Type == type
                                    select s.Id).Count();
                    if (count < type.MaxMultipleSubmissions)
                    {
                        results = (from sd in Enumerable.Range(0, type.MaxMultipleSubmissions - count).ToList()
                                    select new dtoBaseSubmitterType() { Id = type.Id, Name = type.Name }).ToList();
                    }
                }

                return results;
            }

            public long GetSubmissionsCount(BaseForPaper call)
            {
                return (from s in Manager.GetIQ<UserSubmission>()
                        where s.Call == call && s.Deleted == BaseStatusDeleted.None
                        select s.Id).Count();
            }
            public Boolean CallHasSubmissions(long idCall)
            {
                BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                return CallHasSubmissions(call);
            }
            public Boolean CallHasSubmissions(BaseForPaper call)
            {
                return (from s in Manager.GetIQ<UserSubmission>() where s.Call == call && s.Deleted == BaseStatusDeleted.None select s.Id).Any();
            }
          
            public Boolean ExistCallWithSubmissions(long idCallToRemove, CallForPaperType cType, Int32 idCommunity,Boolean fromPortal)
            {
                Boolean result = false;
                try
                {
                    result = (from s in Manager.GetIQ<UserSubmission>()
                              where s.Call.Id != idCallToRemove && s.Call.Type == cType && (s.Call.IsPortal == fromPortal || (idCommunity > 0 && s.Call != null && s.Call.Community.Id == idCommunity) && s.Deleted == BaseStatusDeleted.None && !s.isAnonymous)
                              select s.Id).Any();
                }
                catch (Exception ex) { 
                
                }
                return result;
            }
            private ModuleObject CreateModuleObject(UserSubmission submission)
            {
                int idObject = 0;
                int idModule = 0;
                int idCommunity = (submission.Call.Community == null) ? 0 : submission.Call.Community.Id;
                String moduleCode = "";
                switch (submission.Call.Type)
                {
                    case CallForPaperType.CallForBids:
                        moduleCode = ModuleCallForPaper.UniqueCode;
                        idObject = (int)ModuleCallForPaper.ObjectType.UserSubmission;
                        break;
                    case CallForPaperType.RequestForMembership:
                        moduleCode = ModuleRequestForMembership.UniqueCode;
                        idObject = (int)ModuleRequestForMembership.ObjectType.UserSubmission;
                        break;
                }
                idModule = ServiceModuleID(moduleCode);
                return ModuleObject.CreateLongObject(submission.Id, submission, idObject, idCommunity, moduleCode, idModule);
            }
            private ModuleObject CreateModuleObject(BaseForPaper call)
            {
                int idObject = 0;
                int idModule = 0;
                int idCommunity = (call.Community == null) ? 0 : call.Community.Id;
                String moduleCode = "";
                switch (call.Type)
                {
                    case CallForPaperType.CallForBids:
                        moduleCode = ModuleCallForPaper.UniqueCode;
                        idObject = (int)ModuleCallForPaper.ObjectType.CallForPaper;
                        break;
                    case CallForPaperType.RequestForMembership:
                        moduleCode = ModuleRequestForMembership.UniqueCode;
                        idObject = (int)ModuleRequestForMembership.ObjectType.RequestForMembership;
                        break;
                }
                idModule = ServiceModuleID(moduleCode);
                return ModuleObject.CreateLongObject(call.Id, call, idObject, idCommunity, moduleCode, idModule);
            }
            private ModuleObject CreateModuleObject(UserSubmission submission, lm.Comol.Core.FileRepository.Domain.liteRepositoryItem item)
            {
                String moduleCode = lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode;
                return ModuleObject.CreateLongObject(item.Id, 0, item, (Int32)lm.Comol.Core.FileRepository.Domain.ModuleRepository.GetObjectType(item.Type), item.Repository.IdCommunity, moduleCode, ServiceModuleID(moduleCode));
            }

            #region "Agency / Profile"
                public Boolean HasSubmissionsWithProfileType(long idCall, int idProfileType)
                {
                    Boolean found = false;
                    try
                    {
                        found = (from s in Manager.GetIQ<UserSubmission>()
                                 where s.Call.Id == idCall && s.Deleted != BaseStatusDeleted.None && !s.isAnonymous
                                 select s.Owner).ToList().Where(p => p.TypeID == idProfileType).Any();
                    }
                    catch (Exception ex)
                    {

                    }
                    return found;
                }
                public Dictionary<long, String> GetAgenciesForSubmitters(long idCall)
                {
                    Dictionary<long, String> list = new Dictionary<long, String>();
                    try
                    {
                        List<Int32> idUsers = (from s in Manager.GetIQ<UserSubmission>()
                                               where s.Call.Id == idCall && s.Deleted != BaseStatusDeleted.None && !s.isAnonymous
                                               select s.Owner).ToList().Select(p => p.Id).ToList().Distinct().ToList();
                        list = GetAgenciesForUsers(idUsers);
                    }
                    catch (Exception ex)
                    {

                    }
                    return list;
                }
                private Dictionary<long, String> GetAgenciesForUsers(List<Int32> idUsers)
                {
                    Dictionary<long, String> list = new Dictionary<long, String>();
                    try
                    {
                        List<Agency> agencies = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a).ToList();
                        List<long> idUserAgencies = new List<long>();
                        if (idUsers.Count() <= maxItemsForQuery)
                            idUserAgencies = (from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && idUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList();
                        else
                        {
                            Int32 index = 0;
                            List<Int32> tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (tUsers.Any())
                            {
                                idUserAgencies.AddRange((from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && tUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList());
                                index++;
                                tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                            idUserAgencies = idUserAgencies.Distinct().ToList();
                        }
                        list = agencies.Where(a => idUserAgencies.Contains(a.Id)).OrderBy(a => a.Name).ToDictionary(a => a.Id, a => a.Name);
                    }
                    catch (Exception ex)
                    {

                    }
                    return list;
                }
            
            #endregion
        #endregion

        #region "Submissions management"
            public List<SubmissionFilterStatus> GetAvailableSubmissionStatus(long idCall)
            {
                List<SubmissionFilterStatus> items = new List<SubmissionFilterStatus>();
                try
                {
                    var query = (from s in Manager.GetIQ<UserSubmission>() where s.Call.Id == idCall select s);

                    if (query.Where(s=> s.Status== SubmissionStatus.draft && s.Deleted== BaseStatusDeleted.None).Select(s=>s.Id).Any())
                        items.Add(SubmissionFilterStatus.WaitingSubmission);
                    if (query.Where(s => s.Deleted != BaseStatusDeleted.None).Select(s => s.Id).Any())
                        items.Add(SubmissionFilterStatus.VirtualDeletedSubmission);
                    if (items.Count> 0 && query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted).Select(s => s.Id).Any())
                        items.Add(SubmissionFilterStatus.OnlySubmitted);
                    else if (items.Count==0)
                        items.Add(SubmissionFilterStatus.OnlySubmitted);
                }
                catch (Exception ex) { 
                
                }
                if (items.Count > 1 || items.Count==0)
                    items.Insert(0, SubmissionFilterStatus.All);
                return items;
            }
            public List<dtoSubmissionDisplayItemPermission> GetSubmissionList(Boolean allowManage, long idCall, dtoSubmissionFilters filters, int pageIndex, int pageSize)
            {
                List<dtoSubmissionDisplayItemPermission> submissions = null;
                try
                {
                    List<dtoSubmissionDisplay> items = GetSubmissionList(idCall,filters,pageIndex,pageSize);
                    
                    submissions = (from s in items
                                   select new dtoSubmissionDisplayItemPermission()
                                   {
                                       Id = s.Id,
                                       Deleted = s.Deleted,
                                       Submission = s,
                                       Permission = new dtoSubmissionDisplayPermission(s, allowManage, filters.CallType)
                                   }
                                   ).ToList();
                }
                catch (Exception ex)
                {

                }
                return submissions;
            }

            

            public List<dtoBaseSubmission> GetSubmissionsForEvaluation(long idCall, dtoSubmissionFilters filters, int pageIndex, int pageSize)
            {
                List<dtoBaseSubmission> items = null;
                var query = GetSubmissionsQuery(idCall, filters);

                SubmissionsOrder orderBy = filters.OrderBy;
                Boolean ascending = filters.Ascending;
                if (orderBy == SubmissionsOrder.None)
                {
                    ascending = false;
                    switch (filters.Status)
                    {
                        case SubmissionFilterStatus.OnlySubmitted:
                            orderBy = SubmissionsOrder.BySubmittedOn;
                            ascending = true;
                            break;
                        case SubmissionFilterStatus.VirtualDeletedSubmission:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                        case SubmissionFilterStatus.WaitingSubmission:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                        default:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                    }
                }

                switch (orderBy)
                {
                    case SubmissionsOrder.BySubmittedOn:
                        if (ascending)
                            query = query.OrderBy(s => s.SubmittedOn);
                        else
                            query = query.OrderByDescending(s => s.SubmittedOn);
                        break;
                    case SubmissionsOrder.ByDate:
                        if (ascending)
                            query = query.OrderBy(r => r.ModifiedOn);
                        else
                            query = query.OrderByDescending(r => r.ModifiedOn);
                        break;
                    case SubmissionsOrder.ByStatus:
                        if (ascending)
                            query = query.OrderBy(r => filters.TranslationsSubmission[r.Status]);
                        else
                            query = query.OrderByDescending(r => filters.TranslationsSubmission[r.Status]);
                        break;
                    case SubmissionsOrder.ByType:
                        if (ascending)
                            query = query.OrderBy(s => s.Type.Name);
                        else
                            query = query.OrderByDescending(s => s.Type.Name);
                        break;
                    case SubmissionsOrder.ByUser:
                        if (ascending)
                            query = query.OrderBy(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                        else
                            query = query.OrderByDescending(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                        break;
                }

                if (pageSize > 0)
                    query = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                else
                    query = query.ToList();
                items = (from r in query
                         select new dtoBaseSubmission()
                         {
                             Type = new dtoSubmitterType() { Name = r.Type.Name, Id=r.Type.Id  },
                             Status = r.Status,
                             Deleted = r.Deleted,
                             Id = r.Id,
                             ExtensionDate = r.ExtensionDate,
                             SubmittedOn = r.SubmittedOn,
                             ModifiedOn = r.ModifiedOn,
                             Owner = r.Owner,
                             IsAnonymous = r.isAnonymous
                         }).ToList();

                return items;
            }

            public List<dtoSubmissionDisplay> GetSubmissionList( long idCall, dtoSubmissionFilters filters, int pageIndex, int pageSize)
            {
                List<dtoSubmissionDisplay> items = null;
                var query = GetSubmissionsQuery(idCall, filters);

                SubmissionsOrder orderBy = filters.OrderBy;
                Boolean ascending = filters.Ascending;
                if (orderBy == SubmissionsOrder.None)
                {
                    ascending = false;
                    switch (filters.Status)
                    {
                        case SubmissionFilterStatus.OnlySubmitted:
                            orderBy = SubmissionsOrder.BySubmittedOn;
                            ascending = true;
                            break;
                        case SubmissionFilterStatus.VirtualDeletedSubmission:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                        case SubmissionFilterStatus.WaitingSubmission:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                        default:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                    }
                }

                switch (orderBy)
                {
                    case SubmissionsOrder.BySubmittedOn:
                        if (ascending)
                            query = query.OrderBy(s => s.SubmittedOn);
                        else
                            query = query.OrderByDescending(s => s.SubmittedOn);
                        break;
                    case SubmissionsOrder.ByDate:
                        if (ascending)
                            query = query.OrderBy(r => r.ModifiedOn);
                        else
                            query = query.OrderByDescending(r => r.ModifiedOn);
                        break;
                    case SubmissionsOrder.ByStatus:
                        if (ascending)
                            query = query.OrderBy(r => filters.TranslationsSubmission[r.Status]);
                        else
                            query = query.OrderByDescending(r => filters.TranslationsSubmission[r.Status]);
                        break;
                    case SubmissionsOrder.ByType:
                        if (ascending)
                            query = query.OrderBy(s => s.Type.Name);
                        else
                            query = query.OrderByDescending(s => s.Type.Name);
                        break;
                    case SubmissionsOrder.ByUser:
                        if (ascending)
                            query = query.OrderBy(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                        else
                            query = query.OrderByDescending(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                        break;
                }

                if (pageSize>0)
                    query = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                else
                    query = query.ToList();

                IDictionary<Int64, ModuleLink> dcSignLink = new Dictionary<long, ModuleLink>();

                try
                {
                    dcSignLink = (from CFPSignLink sign in Manager.GetAll<CFPSignLink>(sl => sl.CallId == idCall)
                                  select new
                                  {
                                      revId = sign.RevisionId,
                                      ml = Manager.Get<ModuleLink>(sign.LinkId)
                                  }).Distinct().ToDictionary(i => i.revId, i => i.ml);
                }
                catch (Exception)
                {

                }
                
                items = (from r in query
                         select new dtoSubmissionDisplay()
                         {
                             Type = new dtoSubmitterType() { Name = r.Type.Name },
                             Status = r.Status,
                             Deleted = r.Deleted,
                             Id = r.Id,
                             ExtensionDate = r.ExtensionDate,
                             SubmittedOn = r.SubmittedOn,
                             ModifiedOn = r.ModifiedOn,
                             Owner = r.Owner,
                             IsAnonymous= r.isAnonymous,
                              UniqueID= r.UserCode 
                         }).ToList();
                List<dtoSubmissionDisplay> results = new List<dtoSubmissionDisplay>();

               
                foreach (dtoSubmissionDisplay item in items) {
                    var queryRevision = (from r in Manager.GetIQ<Revision>()
                                         where r.Deleted == BaseStatusDeleted.None 
                                         && r.Submission.Id == item.Id
                                         select new
                                         {
                                             IdRevision = r.Id, 
                                             Type = r.Type, 
                                             Status = r.Status, 
                                             isActive = r.IsActive
                                         }).ToList().OrderBy(r => r.IdRevision).ToList();
                    
                    if (queryRevision.Count > 0)
                    {
                        item.IdRevision = queryRevision
                            .Where(r => r.isActive)
                            .Select(r => r.IdRevision)
                            .Max();

                        item.SignLink = (dcSignLink.ContainsKey(item.IdRevision)
                            ? dcSignLink[item.IdRevision]
                            : new ModuleLink());

                        if (queryRevision.Count > 1)
                        {
                            List<dtoSubmissionItem> revisions = 
                                (from r in queryRevision 
                                 select new dtoSubmissionItem()
                                 {
                                     Id = r.IdRevision, 
                                     isActive = r.isActive, 
                                     RevisionStatus = r.Status, 
                                     RevisionType = r.Type, 
                                     Submission = item,
                                     SignLink = dcSignLink.ContainsKey(r.IdRevision)
                                                    ? dcSignLink[r.IdRevision]
                                                    : new ModuleLink()
                                 }).ToList();

                            
                                     

                            if (revisions.Count > 0)
                            {
                                Int32 version = -1;
                                Int32 subVersion = 1;

                                foreach (dtoSubmissionItem rev in revisions)
                                {
                                    if (rev.isActive)
                                    {
                                        version++;
                                        rev.DisplayNumber = version.ToString();
                                        subVersion = 1;
                                    }
                                    else
                                    {
                                        rev.DisplayNumber = version.ToString() + "." + subVersion.ToString();
                                        subVersion++;
                                    }
                                }
                                item.Revisions = revisions.OrderByDescending(r => r.Id).ToList();
                                item.Revisions[0].Display = displayAs.first;
                                item.Revisions.Last().Display = displayAs.last;
                            }
                        }
                    }
                    
                    results.Add(item);
                }
                return results;
            }

            private DateTime? LastRevisionModify(Int64 submissionId)
            {
                var query = (from Revision rev in Manager.GetIQ<Revision>()
                             where rev.Submission != null && rev.Submission.Id == submissionId
                             select new
                             {
                                 creation = rev.CreatedOn,
                                 modified = rev.ModifiedOn
                             }).ToList()
                ;

                if (!query.Any())
                    return null;


                DateTime? lastCreation = null;
                DateTime? lastModification = null;

                try
                {
                    lastCreation = query.Max(r => r.creation);
                }
                catch (Exception)
                {

                }

                try
                {
                    lastModification = query.Max(r => r.modified);
                }
                catch (Exception)
                {


                    throw;
                }

                if (lastModification != null
                    && lastCreation != null
                    && lastModification >= lastCreation)
                    return lastModification;

                return lastCreation;

            }

            private List<dtoSubmissionDataDisplay> GetSubmissionDataList(long idCall, dtoSubmissionFilters filters, int pageIndex, int pageSize)
            {
                List<dtoSubmissionDataDisplay> items = null;
                var query = GetSubmissionsQuery(idCall, filters);

                SubmissionsOrder orderBy = filters.OrderBy;
                Boolean ascending = filters.Ascending;
                if (orderBy == SubmissionsOrder.None)
                {
                    ascending = false;
                    switch (filters.Status)
                    {
                        case SubmissionFilterStatus.OnlySubmitted:
                            orderBy = SubmissionsOrder.BySubmittedOn;
                            ascending = true;
                            break;
                        case SubmissionFilterStatus.VirtualDeletedSubmission:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                        case SubmissionFilterStatus.WaitingSubmission:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                        default:
                            orderBy = SubmissionsOrder.ByDate;
                            break;
                    }
                }

                switch (orderBy)
                {
                    case SubmissionsOrder.BySubmittedOn:
                        if (ascending)
                            query = query.OrderBy(s => s.SubmittedOn);
                        else
                            query = query.OrderByDescending(s => s.SubmittedOn);
                        break;
                    case SubmissionsOrder.ByDate:
                        if (ascending)
                            query = query.OrderBy(r => r.ModifiedOn);
                        else
                            query = query.OrderByDescending(r => r.ModifiedOn);
                        break;
                    case SubmissionsOrder.ByStatus:
                        if (ascending)
                            query = query.OrderBy(r => filters.TranslationsSubmission[r.Status]);
                        else
                            query = query.OrderByDescending(r => filters.TranslationsSubmission[r.Status]);
                        break;
                    case SubmissionsOrder.ByType:
                        if (ascending)
                            query = query.OrderBy(s => s.Type.Name);
                        else
                            query = query.OrderByDescending(s => s.Type.Name);
                        break;
                    case SubmissionsOrder.ByUser:
                        if (ascending)
                            query = query.OrderBy(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                        else
                            query = query.OrderByDescending(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                        break;
                }

                if (pageSize > 0)
                    query = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                else
                    query = query.ToList();
                items = (from r in query
                         select new dtoSubmissionDataDisplay()
                         {
                             Type = new dtoSubmitterType() { Name = r.Type.Name },
                             Status = r.Status,
                             Deleted = r.Deleted,
                             Id = r.Id,
                             ExtensionDate = r.ExtensionDate,
                             SubmittedOn = r.SubmittedOn,
                             ModifiedOn = r.ModifiedOn,
                             Owner = r.Owner,
                             IsAnonymous = r.isAnonymous,
                             UniqueID = r.UserCode,
                             Sections= new List<dtoCallSection<dtoSubmissionValueField>>()
                         }).ToList();
                List<dtoSubmissionDataDisplay> results = new List<dtoSubmissionDataDisplay>();


                foreach (dtoSubmissionDataDisplay item in items)
                {
                    var queryRevision = (from r in Manager.GetIQ<Revision>()
                                         where r.Deleted == BaseStatusDeleted.None && r.Submission.Id == item.Id
                                         select new { IdRevision = r.Id, Type = r.Type, Status = r.Status, isActive = r.IsActive }).ToList().OrderBy(r => r.IdRevision).ToList();
                    if (queryRevision.Count > 0)
                    {
                        item.IdRevision = queryRevision.Where(r => r.isActive).Select(r => r.IdRevision).Max();
                        item.Sections = GetSubmissionFields(Manager.Get<Revision>(item.IdRevision));
                        if (queryRevision.Count > 1)
                        {
                            List<dtoSubmissionItem> revisions = (from r in queryRevision select new dtoSubmissionItem() { Id = r.IdRevision, isActive = r.isActive, RevisionStatus = r.Status, RevisionType = r.Type, Submission = item }).ToList();
                            if (revisions.Count > 0)
                            {
                                Int32 version = -1;
                                Int32 subVersion = 1;

                                foreach (dtoSubmissionItem rev in revisions)
                                {
                                    if (rev.isActive)
                                    {
                                        version++;
                                        rev.DisplayNumber = version.ToString();
                                        subVersion = 1;
                                    }
                                    else
                                    {
                                        rev.DisplayNumber = version.ToString() + "." + subVersion.ToString();
                                        subVersion++;
                                    }
                                }
                                item.Revisions = revisions.OrderByDescending(r => r.Id).ToList();
                                item.Revisions[0].Display = displayAs.first;
                                item.Revisions.Last().Display = displayAs.last;
                            }
                        }
                    }
                    results.Add(item);
                }
                return results;
            }

            public String ExportSubmissionList(lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType exportType, Boolean allowManage, long idCall, dtoSubmissionFilters filters, Dictionary<SubmissionsListTranslations, string> translations, Dictionary<SubmissionStatus, String> status, Dictionary<RevisionStatus, string> revisions)
            {
                String content = "";
                try
                {
                    dtoBaseForPaper call = GetDtoBaseCall(idCall);
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

                    if (call != null || person != null)
                    {
                        List<dtoSubmissionDisplay> items = GetSubmissionList(idCall, filters, 0, 0);

                        switch (exportType)
                        {
                            case ExportFileType.xml:
                            case ExportFileType.xls:
                                HelperExportToXml helper = new HelperExportToXml(translations, status, revisions);
                                content = helper.ExportSubmissionsList(call.Type, call.Name, call.Edition, items, person, filters.Status);
                                break;
                            default:
                                HelperExportToCsv helperCsv = new HelperExportToCsv(translations, status, revisions);
                                content = helperCsv.ExportSubmissionsList(call.Type, call.Name, call.Edition, items, person, filters.Status);
                                break;
                        }
                    }
                    else {
                        switch (exportType)
                        {
                            case ExportFileType.xml:
                            case ExportFileType.xls:
                                content = HelperExportToXml.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                                break;
                            default:
                                content = HelperExportToCsv.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (exportType) { 
                        case ExportFileType.xml:
                        case ExportFileType.xls:
                            content = HelperExportToXml.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                            break;
                        default:
                            content = HelperExportToCsv.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                            break;
                    }
                }
                return content;
            }

            public String ExportSubmissionsData(lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType exportType,Boolean allowManage, long idCall, dtoSubmissionFilters filters, Dictionary<SubmissionsListTranslations, string> translations, Dictionary<SubmissionStatus, String> status, Dictionary<RevisionStatus, string> revisions)
            {
                String content = "";
                try
                {
                    dtoBaseForPaper call = GetDtoBaseCall(idCall);
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (call != null || person != null)
                    {
                        List<dtoSubmissionDataDisplay> items = GetSubmissionDataList(idCall, filters, 0, 0);

                        switch (exportType)
                        {
                            case ExportFileType.xml:
                            case ExportFileType.xls:
                                HelperExportToXml helper = new HelperExportToXml(translations, status, revisions);
                                content = helper.ExportSubmissionsData(call.Name, call.Edition, GetEditorSections(idCall), items, person, filters.Status);
                                break;
                            default:
                                HelperExportToCsv helperCsv = new HelperExportToCsv(translations, status, revisions);
                                content = helperCsv.ExportSubmissionsData(call.Name, call.Edition, GetEditorSections(idCall), items, person, filters.Status);
                                break;
                        }
                    }
                    else
                    {
                        switch (exportType)
                        {
                            case ExportFileType.xml:
                            case ExportFileType.xls:
                                content = HelperExportToXml.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                                break;
                            default:
                                content = HelperExportToCsv.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    switch (exportType)
                    {
                        case ExportFileType.xml:
                        case ExportFileType.xls:
                            content = HelperExportToXml.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                            break;
                        default:
                            content = HelperExportToCsv.GetErrorDocument(translations[SubmissionsListTranslations.FileCreationError]);
                            break;
                    }
                }
                return content;
            }
            public long SubmissionsCount(long idCall, dtoSubmissionFilters filters)
            {
                long count = 0;
                try
                {
                    var query = GetSubmissionsQuery(idCall, filters);
                    count = query.Count();
                }
                catch (Exception ex) { }
                return count;
            }
            private IEnumerable<UserSubmission> GetSubmissionsQuery(long idCall, dtoSubmissionFilters filters)
            {
                var query = GetBaseSubmissionsQuery(idCall, filters);
                String searchName = "";
                if (!String.IsNullOrEmpty(filters.SearchForName))
                {
                    searchName = filters.SearchForName.ToLower();
                    List<long> idSubmissions = GetSubmissionsByUser(GetBaseSubmissionsQuery(idCall, filters), searchName);
                    query = query.Where(r => idSubmissions.Contains(r.Id));
                }
                return query;
            }
            private IEnumerable<UserSubmission> GetBaseSubmissionsQuery(long idCall, dtoSubmissionFilters filters)
            {
                var query = (from s in Manager.GetIQ<UserSubmission>() where s.Call.Id == idCall select s);
                switch(filters.Status){
                    case SubmissionFilterStatus.VirtualDeletedSubmission:
                        query = query.Where(s=>s.Deleted!= BaseStatusDeleted.None);
                        break;
                    case SubmissionFilterStatus.OnlySubmitted:
                        query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status>= SubmissionStatus.submitted);
                        break;
                    case SubmissionFilterStatus.WaitingSubmission:
                        query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status < SubmissionStatus.submitted);
                        break;
                    case SubmissionFilterStatus.Accepted:
                        query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.accepted && s.Status != SubmissionStatus.rejected);
                        break;

                }
                return query;
            }
            private List<long> GetSubmissionsByUser(IEnumerable<UserSubmission> query, String searchName)
            {
                List<long> idSubmissions = new List<long>();
                var users = query.Where(s=> s.Owner!=null).Select(r => new { IdPerson = r.Owner.Id, idSubmission = r.Id }).ToList();

                Int32 pageSize = 50;
                Int32 pageIndex = 0;
                List<Int32> idUsers = users.Select(u => u.IdPerson).ToList();
                List<Int32> idPersons = new List<Int32>();

                var userQuery = idUsers.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                while (userQuery.Any())
                {
                    idPersons.AddRange(
                        (from p in Manager.GetIQ<litePerson>() where userQuery.Contains(p.Id) && (p.Name.Contains(searchName) || p.Surname.Contains(searchName)) select p.Id).ToList());
                    pageIndex++;
                    userQuery = idUsers.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                idSubmissions = (from r in users where idPersons.Contains(r.IdPerson) select r.idSubmission).Distinct().ToList();

                return idSubmissions;
            }
            private List<LazySubscription> GetSubscriptionsBySubmission(List<int> idSubmitters, Int32 idCommunity)
            {
                List<LazySubscription> subscriptions = new List<LazySubscription>();
                Int32 pageSize = 50;
                Int32 pageIndex = 0;

                var userQuery = idSubmitters.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                while (userQuery.Any())
                {
                    subscriptions.AddRange(
                        (from s in Manager.Linq<LazySubscription>() 
                         where userQuery.Contains(s.IdPerson) && s.IdCommunity == idCommunity select s).ToList());

                    pageIndex++;
                    userQuery = idSubmitters.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                return subscriptions;
            }

   
            public Boolean PhisicalDeleteSubmission(long idSubmission, String baseFilePath) {
                Boolean deleted = false;
                try
                {
                    List<string> filesToRemove = new List<string>();

                    Manager.BeginTransaction();
                    UserSubmission submission = Manager.Get<UserSubmission>(idSubmission);

                    if (submission != null)
                    {
                        Int32 idCommunity = (submission.Community == null) ? 0 : submission.Community.Id;

                        Manager.DeletePhysicalList((from f in Manager.GetIQ<SubmissionFieldStringValue>()
                                                    where f.Submission == submission
                                                    select f).ToList());

                        List<SubmissionFieldFileValue> fieldFiles = (from f in Manager.GetIQ<SubmissionFieldFileValue>()
                                                                     where f.Submission == submission
                                                                     select f).ToList();
                        if (fieldFiles.Count > 0)
                        {
                            fieldFiles.Where(f => f.Item != null).ToList().ForEach(f => filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, f.Item)));                      
                            Manager.DeletePhysicalList(fieldFiles);
                        }
                        if (submission.Revisions.Count > 0)
                        {
                            submission.Revisions.Where(f => f.FilePDF != null).ToList().ForEach(f => filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, f.FilePDF)));
                            submission.Revisions.Where(f => f.FileRTF != null).ToList().ForEach(f => filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, f.FileRTF)));
                            submission.Revisions.Where(f => f.FileZip != null).ToList().ForEach(f => filesToRemove.Add(RepositoryService.GetItemDiskFullPath(baseFilePath, f.FileZip)));
                        }
                        Manager.DeletePhysical(submission);
                    }

                    Manager.Commit();
                    deleted = true;
                    Delete.Files(filesToRemove);
                }
                catch (Exception ex) {
                    Manager.RollBack();
                    deleted = false;
                }
                return deleted;
            }
        #endregion

            #region "Permission"
            public ModuleCallForPaper CallForPaperServicePermission(int personId, int idCommunity)
            {
                litePerson person = Manager.GetLitePerson(personId);
                return CallForPaperServicePermission(person, idCommunity);
            }
            public ModuleCallForPaper CallForPaperServicePermission(litePerson person, int idCommunity)
            {
                ModuleCallForPaper module = new ModuleCallForPaper();
                if (person == null|| person.Id ==0)
                    person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                if (idCommunity == 0)
                    module = ModuleCallForPaper.CreatePortalmodule((person == null) ? (int)UserTypeStandard.Guest : person.TypeID);
                else
                    module = new ModuleCallForPaper(Manager.GetModulePermission(person.Id, idCommunity, ServiceModuleID(ModuleCallForPaper.UniqueCode)));
                return module;
            }
            public ModuleRequestForMembership RequestForMembershipServicePermission(int personId, int idCommunity)
            {
                litePerson person = Manager.GetLitePerson(personId);
                return RequestForMembershipServicePermission(person, idCommunity);
            }
            public ModuleRequestForMembership RequestForMembershipServicePermission(litePerson person, int idCommunity)
            {
                ModuleRequestForMembership module = new ModuleRequestForMembership();
                if (person == null || person.Id==0)
                    person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();

                if (idCommunity == 0)
                    module = ModuleRequestForMembership.CreatePortalmodule((person == null) ? (int)UserTypeStandard.Guest : person.TypeID);
                else
                    module = new ModuleRequestForMembership(Manager.GetModulePermission(person.Id, idCommunity, ServiceModuleID(ModuleRequestForMembership.UniqueCode)));
                return module;
            }
        #endregion

        #region "Profile"
            #region "1 - Definition columns"
                //public List<dtoCallSection<dtoFieldAssociation>> GetFieldAssociations(long idCall)
                //{
                //    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                //    return GetFieldAssociations(call);
                //}
                public List<dtoCallSection<dtoFieldAssociation>> GetFieldAssociations(long idCall)
                {
                    List<dtoCallSection<dtoFieldAssociation>> sections = new List<dtoCallSection<dtoFieldAssociation>>();
                    try
                    {
                        sections = (from s in Manager.GetIQ<FieldsSection>()
                                    where s.Deleted == BaseStatusDeleted.None && s.Call.Id == idCall
                                    select new dtoCallSection<dtoFieldAssociation>()
                                    {
                                        Id = s.Id,
                                        Name = s.Name,
                                        Description = s.Description,
                                        DisplayOrder = s.DisplayOrder
                                    }).ToList();

                        List<dtoFieldAssociation> fields = (from f in Manager.GetIQ<FieldDefinition>()
                                                            where f.Call.Id == idCall
                                                         && f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.Note && f.Type!= FieldType.FileInput 
                                                            select f).ToList().Select(f => new dtoFieldAssociation() { IdField = f.Id, Name= f.Name, DisplayOrder= f.DisplayOrder, Type=f.Type, IdSection= f.Section.Id}).ToList();
                        List<LazyProfileAttributeAssociation> associations = (from af in Manager.GetIQ<LazyProfileAttributeAssociation>()
                                                                              where af.IdCall== idCall && af.Deleted== BaseStatusDeleted.None
                                                                              select af).ToList();
                        fields.Where(f => associations.Where(a => a.IdField == f.IdField).Any()).ToList().ForEach(f => f.SetAssociationInfo(associations.Where(a => a.IdField == f.IdField).Select(a => a.Id).FirstOrDefault(), associations.Where(a => a.IdField == f.IdField).Select(a => a.Attribute).FirstOrDefault()));
                        fields.Where(f => !associations.Where(a => a.IdField == f.IdField).Any()).ToList().ForEach(f => f.Attribute = GetProfileAttributeType(f.Type));
                           
                        foreach (dtoCallSection<dtoFieldAssociation> section in sections)
                        {
                            section.Fields = (from f in fields where f.IdSection == section.Id orderby f.DisplayOrder, f.Name select f).ToList();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return sections.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                }
                public Boolean SaveSettings(List<dtoFieldAssociation> associations, long idCall) {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (person != null && call !=null)
                        {
                            foreach (dtoFieldAssociation dto in associations) {
                                ProfileAttributeAssociation association = Manager.Get<ProfileAttributeAssociation>(dto.Id);
                                if (association == null) {
                                    association = new ProfileAttributeAssociation();

                                    association.Call = call;
                                    association.Field = Manager.Get<FieldDefinition>(dto.IdField);
                                    association.CreateMetaInfo(person,UC.IpAddress, UC.ProxyIpAddress);
                                }
                                else
                                    association.UpdateMetaInfo(person,UC.IpAddress,UC.ProxyIpAddress);
                                association.Deleted= BaseStatusDeleted.None;
                                association.Attribute = dto.Attribute;
                                if (association.Field!=null)
                                    Manager.SaveOrUpdate(association);
                            }
                            result = true;
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        result = false;
                        Manager.RollBack();
                    }

                    return result;
                }
                private Core.Authentication.ProfileAttributeType GetProfileAttributeType(FieldType fieldType) { 
                    switch (fieldType) {
                        case FieldType.CheckboxList:
                            return Core.Authentication.ProfileAttributeType.skip;
                        case FieldType.CompanyCode:
                            return Core.Authentication.ProfileAttributeType.companyReaNumber;
                        case  FieldType.CompanyTaxCode:
                            return Core.Authentication.ProfileAttributeType.companyTaxCode;
                        case FieldType.Date:
                        case FieldType.DateTime:
                        case FieldType.Disclaimer:
                        case FieldType.DropDownList:
                        case FieldType.FileInput:
                            return Core.Authentication.ProfileAttributeType.skip;
                        case FieldType.Mail:
                            return Core.Authentication.ProfileAttributeType.mail;
                        case FieldType.MultiLine:
                            return Core.Authentication.ProfileAttributeType.skip;
                        case FieldType.Name:
                            return Core.Authentication.ProfileAttributeType.name;
                        case FieldType.Note:
                        case FieldType.RadioButtonList:
                        case FieldType.SingleLine:
                            return Core.Authentication.ProfileAttributeType.skip;
                        case FieldType.Surname:
                            return Core.Authentication.ProfileAttributeType.surname;
                        case FieldType.TaxCode:
                            return Core.Authentication.ProfileAttributeType.taxCode;
                        case FieldType.TelephoneNumber:
                            return Core.Authentication.ProfileAttributeType.telephoneNumber;
                        case FieldType.Time:
                            return Core.Authentication.ProfileAttributeType.skip;
                        case FieldType.VatCode:
                            return Core.Authentication.ProfileAttributeType.companyReaNumber;
                        case FieldType.ZipCode:
                            return Core.Authentication.ProfileAttributeType.zipCode;
                        default:
                            return Core.Authentication.ProfileAttributeType.skip;
                    }
                }
                private InputType GetInputTypeForField(FieldType fieldType)
                {
                    switch (fieldType)
                    {
                        case FieldType.FileInput:
                        case FieldType.Note:
                            return InputType.unknown;
                        case FieldType.Mail:
                            return InputType.mail;
                        case FieldType.TaxCode:
                            return InputType.taxCode;
                        case FieldType.Date:
                        case FieldType.DateTime:
                        case FieldType.Time:
                            return InputType.datetime;
                        default:
                            return InputType.strings;
                    }
                }
            #endregion
            #region "2 - Export functions"
                public List<ExternalColumn> GetAvailableExternalColumns(long idCall, long idSubmitterType) 
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call == null)
                        return new List<ExternalColumn>();
                    else
                        return GetAvailableExternalColumns(call, idSubmitterType);
                }
                public List<ProfileColumnComparer<String>> GetAvailableProfileColumns(long idCall, long idSubmitterType)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call == null)
                        return new List<ProfileColumnComparer<String>>();
                    else
                    {
                        List<dtoCallSection<dtoCallField>> sections = (from s in Manager.GetIQ<FieldsSection>()
                                                                       where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id
                                                                       select new dtoCallSection<dtoCallField>()
                                                                       {
                                                                           Id = s.Id,
                                                                           Name = s.Name,
                                                                           DisplayOrder = s.DisplayOrder
                                                                       }).ToList();

                        List<long> idFields = (from a in Manager.GetIQ<FieldAssignment>() where a.Deleted == BaseStatusDeleted.None && a.SubmitterType.Id == idSubmitterType select a.Field.Id).ToList();
                        List<dtoCallField> fields = (from f in Manager.GetIQ<FieldDefinition>()
                                                     where f.Call == call && idFields.Contains(f.Id)
                                             && f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.Note && f.Type != FieldType.FileInput
                                                     select f).ToList().Select(f => new dtoCallField() { Name = f.Name, Type = f.Type, IdSection = f.Section.Id, DisplayOrder = f.DisplayOrder }).ToList();
                    
                        List<ProfileColumnComparer<String>> columns = new List<ProfileColumnComparer<String>>();
                        int number = 1;
                        List<LazyProfileAttributeAssociation> associations = (from a in Manager.GetIQ<LazyProfileAttributeAssociation>() where a.IdCall == call.Id && a.Deleted == BaseStatusDeleted.None select a).ToList();

                        foreach (dtoCallSection<dtoCallField> section in sections.OrderBy(s => s.DisplayOrder).ToList())
                        {
                            foreach(dtoCallField field in (from f in fields
                                              where f.IdSection == section.Id
                                              orderby f.DisplayOrder, f.Name select f).ToList()){

                                if (associations.Where(a => a.IdField == field.Id).Any())
                                    columns.Add(new ProfileColumnComparer<string>() { SourceColumn = field.Name, DestinationColumn = associations.Where(a => a.IdField == field.Id).Select(a => a.Attribute).FirstOrDefault(), Number = number });
                                else
                                    columns.Add(new ProfileColumnComparer<string>() { SourceColumn = field.Name, DestinationColumn = GetProfileAttributeType(field.Type), Number = number });
                                number++;
                            }
                        }
                        return columns;
                    }
                }
                public List<ExternalColumn> GetAvailableExternalColumns(BaseForPaper call, long idSubmitterType) 
                {
                    List<ExternalColumn> columns = new List<ExternalColumn>();
                    List<FieldDefinition> definitions = new List<FieldDefinition>();


                    List<dtoCallSection<dtoCallField>> sections = (from s in Manager.GetIQ<FieldsSection>()
                                                                                      where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id
                                                                         select new dtoCallSection<dtoCallField>()
                                                                                      {
                                                                                          Id = s.Id,
                                                                                          Name = s.Name,
                                                                                          DisplayOrder = s.DisplayOrder
                                                                                      }).ToList();

                    List<long> idFields = (from a in Manager.GetIQ<FieldAssignment>() where a.Deleted == BaseStatusDeleted.None && a.SubmitterType.Id == idSubmitterType select a.Field.Id).ToList();
                    List<dtoCallField> fields = (from f in Manager.GetIQ<FieldDefinition>()
                                                        where f.Call == call && idFields.Contains(f.Id)
                                                && f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.Note && f.Type!= FieldType.FileInput 
                                                 select f).ToList().Select(f => new dtoCallField() { Name = f.Name, Type = f.Type, IdSection=f.Section.Id, DisplayOrder=f.DisplayOrder   }).ToList();
                    

                    int number = 1;
                    foreach (dtoCallSection<dtoCallField> section in sections.OrderBy(s=> s.DisplayOrder).ToList())
                    {
                        columns.AddRange((from f in fields where f.IdSection == section.Id orderby f.DisplayOrder, f.Name 
                                          select new ExternalColumn(GetInputTypeForField(f.Type)) { Name = f.Name, Number = number }).ToList());
                        number++;
                    }
                    return columns;
                }

                public ProfileExternalResource CreateProfileResource(long idCall, SubmissionStatus status, DateTime? fromData, Int32 itemsToLoad, long submitterId)
                {
                    List<ProfileColumnComparer<String>> columns = GetAvailableProfileColumns(idCall, submitterId);
                    return CreateProfileResource(columns, idCall, status, fromData, itemsToLoad, submitterId);
                }
                public ProfileExternalResource CreateProfileResource(List<ProfileColumnComparer<String>> columns, long idCall, SubmissionStatus status, DateTime? fromData, Int32 itemsToLoad, long idSubmitterType)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call == null)
                        return null;
                    else
                    {
                        ProfileExternalResource sourceValues = new ProfileExternalResource(columns, GetValuesForExternalResource(call, status, fromData, itemsToLoad, idSubmitterType, true));
                        return sourceValues;
                    }
                }
                public ExternalResource CreateExternalResource(long idCall, SubmissionStatus status, DateTime? fromData, Int32 itemsToLoad, long idSubmitterType)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call == null)
                        return null;
                    else
                    {
                        List<ExternalColumn> columns = GetAvailableExternalColumns(call, idSubmitterType);
                        ExternalResource sourceValues = new ExternalResource(columns, GetValuesForExternalResource(call, status, fromData, itemsToLoad, idSubmitterType,true ));
                        return sourceValues;
                    }
                }
                private List<List<string>> GetValuesForExternalResource(BaseForPaper call, SubmissionStatus status, DateTime? fromData, Int32 itemsToLoad, long idSubmitterType, Boolean onlyAnonymous)
                {
                    List<List<string>> values = new List<List<string>>();
                    //var submissions = (from s in Manager.GetIQ<UserSubmission>()
                    //                   where s.Deleted == BaseStatusDeleted.None && s.Call == call && (status == SubmissionStatus.none || s.Status == status) && (fromData.HasValue == false || s.CreatedOn >= fromData.Value) && (s.Type.Id == submitterId)
                    //                   select s);

                    //if (itemsToLoad > 0)
                    //    submissions = submissions.Skip(0).Take(itemsToLoad);

                    var idSubmissions = (from s in Manager.GetIQ<UserSubmission>()
                                         where s.Deleted == BaseStatusDeleted.None && ((onlyAnonymous && s.isAnonymous) || !onlyAnonymous) && s.Call.Id == call.Id && (status == SubmissionStatus.none || s.Status == status) && (fromData.HasValue == false || s.CreatedOn >= fromData.Value) && (s.Type.Id == idSubmitterType)
                                       select s.Id);

                    if (itemsToLoad > 0)
                        idSubmissions = idSubmissions.Skip(0).Take(itemsToLoad);
                    List<dtoCallSection<dtoCallField>> sections = (from s in Manager.GetIQ<FieldsSection>()
                                                                   where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id
                                                                   select new dtoCallSection<dtoCallField>()
                                                                   {
                                                                       Id = s.Id,
                                                                       Name = s.Name,
                                                                       DisplayOrder = s.DisplayOrder
                                                                   }).ToList();

                    List<long> idFields = (from a in Manager.GetIQ<FieldAssignment>() where a.Deleted == BaseStatusDeleted.None && a.SubmitterType.Id == idSubmitterType select a.Field.Id).ToList();
                    List<dtoCallField> fields = (from f in Manager.GetIQ<FieldDefinition>()
                                                 where f.Call == call && idFields.Contains(f.Id)
                                         && f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.Note && f.Type != FieldType.FileInput
                                                 select f).ToList().Select(f => new dtoCallField(f)).ToList();


                    foreach (long idSubmission in idSubmissions) {
                        long idRevision = GetIdLastActiveRevision(idSubmission);
                        var savedValues = (from v in Manager.GetIQ<SubmissionFieldStringValue>()
                                           where v.Deleted == BaseStatusDeleted.None && v.Submission.Id == idSubmission && v.Revision.Id == idRevision
                                           select new { IdField = v.Field.Id, Value = v.Value, UserValue = v.UserValue }).ToList();

                        List<String> rowValues = new List<string>();
                        foreach (dtoCallSection<dtoCallField> section in sections.OrderBy(s => s.DisplayOrder).ToList())
                        {
                            foreach (dtoCallField field in (from f in fields
                                                            where f.IdSection == section.Id
                                                            orderby f.DisplayOrder, f.Name
                                                            select f).ToList())
                            {
                                if (savedValues.Where(s => s.IdField == field.Id).Any()) {
                                    switch (field.Type) { 
                                        case FieldType.RadioButtonList:
                                        case FieldType.DropDownList:
                                        case FieldType.CheckboxList:
                                            String tmpValue = savedValues.Where(s => s.IdField == field.Id).Select(s => s.Value).FirstOrDefault();
                                            List<String> multipleValues = new List<string>();
                                            if (!String.IsNullOrEmpty(tmpValue))
                                            {
                                                if (tmpValue.Contains("|"))
                                                    multipleValues.AddRange(tmpValue.Split('|').ToList());
                                                else
                                                    multipleValues.Add(tmpValue);
                                            }

                                            if (multipleValues.Count == 0)
                                                rowValues.Add("");
                                            else
                                                rowValues.Add(MultipleValueToString((from opt in field.Options where multipleValues.Contains(opt.Value) select opt.Name).ToList()));
                                                break;
                                        case FieldType.Disclaimer:
                                            String dString = savedValues.Where(s => s.IdField == field.Id).Select(s => s.Value).FirstOrDefault();
                                            rowValues.Add((String.IsNullOrEmpty(dString) ?  "False" : dString));
                                            break;
                                        default:
                                            rowValues.Add(savedValues.Where(s => s.IdField == field.Id).Select(s=> s.Value).FirstOrDefault());
                                            break;
                                    }
                                }
                                else
                                    rowValues.Add("");
                            }
                        }
                        values.Add(rowValues);
                    }
                    //var definitions = (from p in Manager.GetAll<FieldAssignment>(p => p.Deleted == BaseStatusDeleted.None && p.Field.Call == call && (p.SubmitterType.Id == submitterId))
                    //                   orderby (p.Field.DisplayOrder)
                    //                   select p.Field).ToList();
                    ////.ToList(); //<RfM_FieldDefinition>

                    //List<FieldOption> options = new List<FieldOption>();

                    //var optionQuery = (from opt in Manager.GetIQ<FieldOption>() where opt.Deleted == BaseStatusDeleted.None && opt.Field != null select opt);
                    //definitions.Where(f => f.Type == FieldType.CheckboxList || f.Type == FieldType.DropDownList).Select(f => f.Id).ToList().ForEach(f => options.AddRange(optionQuery.Where(o => o.Field.Id == f).ToList()));
                    ////definitions.Where(f => f.Type == FieldType.ChexkboxList || f.Type == FieldType.RadioButtonList).ToList().ForEach(f => Manager.Refresh(f));
                    //foreach (var submission in submissions)
                    //{
                    //    List<String> value = new List<string>();

                    //    foreach (var field in definitions)
                    //    {
                    //        if (submission.FieldsValues.Where(f => f.Field != null && f.Field.Id == field.Id).Any())
                    //        {
                    //            if (field.Type == FieldType.CheckboxList || field.Type == FieldType.DropDownList)
                    //            {
                                    
                    //            }
                    //            else
                    //                value.Add(submission.FieldsValues.Where(f => f.Field == field).Select(v => v.Value).FirstOrDefault());
                    //        }
                    //        else
                    //            value.Add("");
                    //    }
                    //    values.Add(value);
                    //}
                    return values;
                }
                private String MultipleValueToString(List<String> values)
                {
                    String value = "";
                    values.ForEach(v => value += lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator + v);
                    if (String.IsNullOrEmpty(value))
                        return "";
                    else
                        return value.Remove(0, lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator.Length);
                }
            #endregion

            #region "3 - Utility functions"
                /// <summary>
                /// Metodo che restituise una lista di Dto
                /// </summary>
                /// <param name="idCommunity"> il ID della Comunità in cui si cercano </param>
                /// <param name="range"> Range intervallo di ricerca</param>
                /// <returns> Returns a List of DtoCalltoFind che rappresentano le richieste  disponibili nella comunità selezionata</returns>
                public List<dtoCallToFind> GetCallsForProfileInsert(CallForPaperType type, SearchRange range, Int32 idCommunity = -1)
                {
                    List<dtoCallToFind> list = new List<dtoCallToFind>();
                    try
                    {
                        var query = (from c in Manager.GetIQ<BaseForPaper>()
                                     where c.Deleted== BaseStatusDeleted.None && c.Type == type
                                     select c);
                        switch (range)
                        {
                            case SearchRange.Communities:
                                query = query.Where(s => !s.IsPortal);
                                break;
                            case SearchRange.Community:
                                query = query.Where(s => !s.IsPortal && s.Community != null && s.Community.Id == idCommunity);
                                break;
                            case SearchRange.Portal:
                                query = query.Where(s => s.IsPortal);
                                break;
                            default:
                                query = query.Where(s => s.IsPortal);
                                break;
                        }

                        list = query.ToList().Select(c=>
                                new dtoCallToFind()
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Edition = c.Edition,
                                    StartDate = c.StartDate,
                                    CreatedOn = c.CreatedOn,
                                    EndDate = c.EndDate,
                                    Type = c.Type,
                                    IsPublic = c.IsPublic,
                                    IsPortal = c.IsPortal,
                                    Community = c.Community 
                                }).ToList();
                        List<long> idCalls = GetSubmissions(range, type, idCommunity).Select(s => s.Call.Id).Distinct().ToList();
                        list = list.Where(i => i.IsPublic || idCalls.Contains(i.Id)).ToList();
                        //List<liteCommunity> communities = Manager.GetLiteCommunities(list.Where(c=> !c.IsPortal))
                        //list.ForEach(d => d.Community = community);
                        list.ForEach(d => d.SubmittedItems = GetPublicSubmissionsCount(d.Id, SubmissionStatus.submitted));
                        list.ForEach(d => d.WaitingItems = GetPublicSubmissionsCount(d.Id, SubmissionStatus.draft));
                        list.ForEach(d => d.AcceptedItems = GetPublicSubmissionsCount(d.Id, SubmissionStatus.accepted));

                    }
                    catch (Exception ex) { 
                    
                    }
                    
                   
                    return list;
                }
                /// <summary>
                /// Metodo che restituise un singolo Dto di Richieste Adesione
                /// </summary>
                /// <param name="idCall">Id</param>
                /// <returns>DtoCallToFind del BaseForpaper di cui è fornito l identificativo</returns>
                public dtoCallToFind GetCallDtoForProfile(long idCall)
                {
                    dtoCallToFind result = null;
                    try
                    {
                        result = (from c in Manager.GetIQ<BaseForPaper>()
                                  where c.Id == idCall
                                  select new dtoCallToFind()
                                  {
                                      Id = c.Id,
                                      Name = c.Name,
                                      Edition = c.Edition,
                                      StartDate = c.StartDate,
                                      CreatedOn = c.CreatedOn,
                                      EndDate = c.EndDate,
                                      Type = c.Type,
                                      IsPublic = c.IsPublic,
                                      IsPortal = c.IsPortal,
                                  }).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (result != null)
                        {
                            result.Community = GetCallCommunity(idCall);
                            result.SubmittedItems = GetPublicSubmissionsCount(idCall, SubmissionStatus.submitted);
                            result.WaitingItems = GetPublicSubmissionsCount(idCall, SubmissionStatus.draft);
                            result.AcceptedItems = GetPublicSubmissionsCount(idCall, SubmissionStatus.accepted);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new dtoCallToFind();
                    }
                    if (result == null)
                        return new dtoCallToFind();
                    else
                        return result;
                }
                private long GetPublicSubmissionsCount(long idCall, SubmissionStatus status)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call == null)
                        return (long)0;
                    else
                    {
                        return (from s in Manager.GetIQ<UserSubmission>() where s.Call == call && s.isAnonymous && s.Type != null && s.Deleted == BaseStatusDeleted.None && s.Status == status select s.Id).Count();
                    }
                }
                public liteCommunity GetCallCommunity(long idCall)
                {
                    liteCommunity result = null;
                    try
                    {
                        result = (from c in Manager.GetIQ<BaseForPaper>()
                                  where c.Id == idCall
                                  select c.Community).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }
                public litePerson GetCallOwner(long idCall)
                {
                    litePerson result = null;
                    try
                    {
                        result = (from c in Manager.GetIQ<BaseForPaper>()
                                  where c.Id == idCall
                                  select c.CreatedBy).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }

                /// <summary>
                /// Metodo che restistuisce la lista di SubmitterType disponibili per un Richiesta Adesione e con cui popolare la DDL di selezione
                /// </summary>
                /// <param name="RFMid"> L'ID della Richiesta Adesione</param>
                /// <returns> List di SubmitterType per cui la Richiesta Adesione è formulata</returns>
                public Dictionary<long, string> GetSubmittersList(long idCall)
                {
                    Dictionary<long, string> result = new Dictionary<long, string>();
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                    {
                        result = (from s in Manager.GetIQ<SubmitterType>() where s.Call == call && s.Deleted == BaseStatusDeleted.None select s).ToList().ToDictionary(s => s.Id, s => s.Name);
                    }
                    return result;
                }

                /// <summary>
                /// Restituisce una lista di SubmissionStatus contenuti nelle UserSubmission dell adesione
                /// </summary>
                /// <param name="idCall"></param>
                /// <returns></returns>
                /// 
                public List<SubmissionStatus> GetAvailablePublicSubmissionStatus(long idCall)
                {
                    return GetAvailableSubmissionStatus(idCall, true, false);
                }

                public List<SubmissionStatus> GetAvailableSubmissionStatus(long idCall, Boolean onlyAnonymous, Boolean alsoAnonymous)
                {
                    List<SubmissionStatus> items = new List<SubmissionStatus>();
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                        items = (from s in Manager.GetIQ<UserSubmission>() where ((onlyAnonymous && s.isAnonymous) || (!onlyAnonymous && (alsoAnonymous || !alsoAnonymous && ! s.isAnonymous)) ) && s.Call.Id == call.Id select s.Status).ToList();
                    return items.Distinct().ToList(); ;
                }
        
            #endregion
      

            //public List<UserSubmission> GetSubmissions(long idCall, SubmissionStatus status)
            //{
            //    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
            //    if (call == null)
            //        return new List<UserSubmission>();
            //    else
            //        return (from s in Manager.GetAll<UserSubmission>(s => s.Call == call && s.Type != null && s.Deleted == BaseStatusDeleted.None && s.Status == status) select s).ToList();
            //}


           
        #endregion

        #region "Messages - User List"
            public List<dtoCallMessageRecipient> GetUsers(lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService,List<dtoCallMessageRecipient> recipients , dtoUsersByMessageFilter filter, Boolean loadAllInfo=true)
            {
                List<dtoCallMessageRecipient> items = new List<dtoCallMessageRecipient>();
                try
                {
                    var query = (from r in recipients where (filter.IdSubmitterType<0 || filter.IdSubmitterType== r.IdSubmitterType || (filter.Status== SubmissionFilterStatus.All && r.IdSubmitterType==0)) select r);
                    
                    if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Value.Trim()) == false)
                    {
                        switch (filter.SearchBy)
                        {
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains:
                                List<String> values = filter.Value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                                if (values.Any() && values.Count == 1)
                                    query = query.Where(r => !String.IsNullOrEmpty(r.DisplayName) && r.DisplayName.ToLower().Contains(filter.Value.ToLower()));
                                else if (values.Any() && values.Count > 1)
                                    query = query.Where(r => (!String.IsNullOrEmpty(r.Name) && values.Any(r.Name.ToLower().Contains)) || (!String.IsNullOrEmpty(r.Surname) && values.Any(r.Surname.ToLower().Contains) )|| values.Any(r.MailAddress.ToLower().Contains) || values.Any(r.DisplayName.ToLower().Contains));
                                break;
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail:
                                query = query.Where(r => r.MailAddress.ToLower().Contains(filter.Value.ToLower()));
                                break;
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Name:
                                query = query.Where(r=> r.Name.ToLower().StartsWith(filter.Value.ToLower()));
                                break;
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname:
                                query = query.Where(r => r.Surname.ToLower().StartsWith(filter.Value.ToLower()));
                                break;
                        }
                    }
                    if ((filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Name || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.All || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains || string.IsNullOrEmpty(filter.Value)) && !string.IsNullOrEmpty(filter.StartWith))
                    {
                        if (filter.StartWith != "#")
                            query = query.Where(r => r.FirstLetter == filter.StartWith.ToLower());
                        else
                            query = query.Where(r => pService.DefaultOtherChars().Contains(r.FirstLetter));
                    }
                    if (filter.IdAgency == -3)
                        query = query.Where(r => r.IdProfileType  != (int)UserTypeStandard.Employee);
                    else
                    {
                        Dictionary<long, List<Int32>> agencyInfos = pService.GetUsersWithAgencies(query.Where(r => r.IsInternal).Select(r => r.IdPerson).ToList().Distinct().ToList());
                        if (filter.IdAgency == -2)
                            query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee);
                        else if (agencyInfos.ContainsKey(filter.IdAgency))
                            query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee && agencyInfos[filter.IdAgency].Contains(r.IdPerson));
                        else if (filter.IdAgency >0)
                            query = query.Where(r => 1 == 2);
                        if (loadAllInfo || filter.OrderBy == UserByMessagesOrder.ByAgency)
                        {
                            Dictionary<long, String> agencyName = pService.GetAgenciesName(agencyInfos.Keys.ToList());
                            foreach (var i in agencyInfos)
                            {
                                query.Where(r => r.IsInternal && i.Value.Contains(r.IdPerson)).ToList().ForEach(r => r.UpdateAgencyInfo(i.Key, (agencyName.ContainsKey(i.Key) ? agencyName[i.Key] : "")));
                            }
                        }
                    }

                    switch (filter.OrderBy) {
                        case UserByMessagesOrder.ByMessageNumber:
                            if (filter.Ascending)
                                query = query.OrderBy(r => r.MessageNumber);
                            else
                                query = query.OrderByDescending(r => r.MessageNumber);
                            break;
                        case UserByMessagesOrder.ByStatus:
                            if (filter.Ascending)
                                query = query.OrderBy(r => filter.StatusTranslations[r.Status]);
                            else
                                query = query.OrderByDescending(r => filter.StatusTranslations[r.Status]);
                            break;
                        case UserByMessagesOrder.ByUser:
                            if (filter.Ascending)
                                query = query.OrderBy(r => r.DisplayName);
                            else
                                query = query.OrderByDescending(r => r.DisplayName);
                            break;
                        case UserByMessagesOrder.ByType:
                            if (filter.Ascending)
                                query = query.OrderBy(r => r.SubmitterType);
                            else
                                query = query.OrderByDescending(r => r.SubmitterType);
                            break;
                        case UserByMessagesOrder.ByAgency:
                            if (filter.Ascending)
                                query = query.OrderBy(r => r.AgencyName);
                            else
                                query = query.OrderByDescending(r => r.AgencyName);
                            break;
                    }
                   
                    items = query.ToList();
                }
                catch (Exception ex)
                {

                }
                return items;
            }
            public List<dtoCallMessageRecipient> GetAllUsers(String unknownUser, String anonymousUser, ModuleObject obj, List<Int32> removeUsers, dtoUsersByMessageFilter filter, Boolean hasUserValues, lm.Comol.Core.Mail.Messages.MailMessagesService service, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService)
                {
                List<dtoCallMessageRecipient> items = new List<dtoCallMessageRecipient>();
                try
                {
                    List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages> recipients = (filter.IdMessages.Any() ? service.GetUsersForMessage(obj, filter.IdMessages) : new List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages>());

                    //items.AddRange(recipients.Select(r => new dtoCallMessageRecipient(r)).ToList());
                    switch (filter.Access) { 
                        case AccessTypeFilter.NoSubmitters:
                            List<litePerson> persons = GetUsersForAssignments(obj.ObjectLongID, pService);
                            items.AddRange(persons.Select(p => new dtoCallMessageRecipient(p, obj.ServiceCode, anonymousUser)).ToList());
                            break;
                        case AccessTypeFilter.Submitters:
                            var query = (from s in Manager.GetIQ<UserSubmission>() where s.Call.Id == obj.ObjectLongID && s.Owner != null select s);
                            switch (filter.Status)
                            {
                                case SubmissionFilterStatus.VirtualDeletedSubmission:
                                    query = query.Where(s => s.Deleted != BaseStatusDeleted.None);
                                    break;
                                case SubmissionFilterStatus.OnlySubmitted:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted);
                                    break;
                                case SubmissionFilterStatus.WaitingSubmission:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status < SubmissionStatus.submitted);
                                    break;
                                case SubmissionFilterStatus.Accepted:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.accepted && s.Status != SubmissionStatus.rejected);
                                    break;
                                case SubmissionFilterStatus.Rejected:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status == SubmissionStatus.rejected);
                                    break;
                            }
                            //if (hasUserValues)
                            items.AddRange(query.ToList().Select(s => new dtoCallMessageRecipient(s, obj.ServiceCode, unknownUser, anonymousUser)).ToList());
                            items.ForEach(i => i.MessageNumber = recipients.Where(r => r.IsInternal && r.IdPerson == i.IdPerson).SelectMany(r => r.Messages.Where(m => m.IdModuleObject == i.IdModuleObject || m.IdModuleObject==0)).Count());
                            items.ForEach(i => i.Messages = recipients.Where(r => r.IsInternal && r.IdPerson == i.IdPerson).SelectMany(r => r.Messages.Where(m => m.IdModuleObject == i.IdModuleObject || m.IdModuleObject == 0)).ToList());
                            break;
                    }
                    items.AddRange((from r in recipients where !r.IsInternal select new dtoCallMessageRecipient(r)).ToList());
                    items.AddRange(recipients.Where(r => r.IsInternal && !items.Where(i => i.IdPerson == r.IdPerson).Any()).Select(r => new dtoCallMessageRecipient(r)).ToList());
                  }
                catch (Exception ex) { 
                
                }
                return items;
            }

            public List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> GetSelectedUsers(ModuleObject obj, List<Int32> removeUsers, dtoUsersByMessageFilter filter, Boolean selectAll, List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> sItems, lm.Comol.Core.Mail.Messages.MailMessagesService service, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService)
            {
                List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> items = new List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient>();
                try
                {
                    List<dtoCallMessageRecipient> recipients = GetAllUsers(obj, removeUsers, filter, service, pService);
                    recipients = ParseMessageRecipients(pService, recipients, filter);
                    if (selectAll)
                        items = recipients.Select(r => (lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient)r).ToList();
                    else
                        items = recipients.Where(r=> sItems.Where(s=> s.IsEqualsTo(r)).Any()).Select(r => (lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient)r).ToList();
                }
                catch (Exception ex) { 
                
                }
                return items;
            }
            private List<dtoCallMessageRecipient> GetAllUsers(ModuleObject obj, List<Int32> removeUsers, dtoUsersByMessageFilter filter, lm.Comol.Core.Mail.Messages.MailMessagesService service, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService)
            {
                List<dtoCallMessageRecipient> items = new List<dtoCallMessageRecipient>();
                try
                {
                    List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages> recipients = (filter.IdMessages.Any() ? service.GetUsersForMessage(obj, filter.IdMessages) : new List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages>());

                    switch (filter.Access)
                    {
                        case AccessTypeFilter.NoSubmitters:
                            List<litePerson> persons = GetUsersForAssignments(obj.ObjectLongID, pService);
                            items.AddRange(persons.Select(p => new dtoCallMessageRecipient(p, obj.ServiceCode)).ToList());
                            break;
                        case AccessTypeFilter.Submitters:
                            var query = (from s in Manager.GetIQ<UserSubmission>() where s.Call.Id == obj.ObjectLongID && s.Owner != null select s);
                            switch (filter.Status)
                            {
                                case SubmissionFilterStatus.VirtualDeletedSubmission:
                                    query = query.Where(s => s.Deleted != BaseStatusDeleted.None);
                                    break;
                                case SubmissionFilterStatus.OnlySubmitted:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted);
                                    break;
                                case SubmissionFilterStatus.WaitingSubmission:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status < SubmissionStatus.submitted);
                                    break;
                                case SubmissionFilterStatus.Accepted:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.accepted && s.Status != SubmissionStatus.rejected);
                                    break;
                                case SubmissionFilterStatus.Rejected:
                                    query = query.Where(s => s.Deleted == BaseStatusDeleted.None && s.Status == SubmissionStatus.rejected);
                                    break;
                            }
                            items.AddRange(query.ToList().Select(s => new dtoCallMessageRecipient(s, obj.ServiceCode,"","")).ToList());
                            break;
                    }
                    items.AddRange((from r in recipients where !r.IsInternal select new dtoCallMessageRecipient(r)).ToList());
                    items.AddRange(recipients.Where(r => r.IsInternal && !items.Where(i => i.IdPerson == r.IdPerson).Any()).Select(r => new dtoCallMessageRecipient(r)).ToList());

                    List<Language> languages = Manager.GetAllLanguages().ToList();
                    items.Where(i => i.IdLanguage > 0 && String.IsNullOrEmpty(i.CodeLanguage)).ToList().ForEach(i => i.CodeLanguage = languages.Where(l => l.Id == i.IdLanguage).Select(l => l.Code).Skip(0).Take(1).ToList().FirstOrDefault());
                }
                catch (Exception ex)
                {

                }
                return items;
            }
            private List<dtoCallMessageRecipient> ParseMessageRecipients(lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, List<dtoCallMessageRecipient> recipients, dtoUsersByMessageFilter filter)
            {
                List<dtoCallMessageRecipient> items = new List<dtoCallMessageRecipient>();
                try
                {
                    var query = (from r in recipients where (filter.IdSubmitterType < 0 || filter.IdSubmitterType == r.IdSubmitterType || (filter.Status == SubmissionFilterStatus.All && r.IdSubmitterType == 0)) select r);

                    if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Value.Trim()) == false)
                    {
                        switch (filter.SearchBy)
                        {
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains:
                                List<String> values = filter.Value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                                if (values.Any() && values.Count == 1)
                                    query = query.Where(r => !String.IsNullOrEmpty(r.DisplayName) && r.DisplayName.ToLower().Contains(filter.Value.ToLower()));
                                else if (values.Any() && values.Count > 1)
                                    query = query.Where(r => (!String.IsNullOrEmpty(r.Name) && values.Any(r.Name.ToLower().Contains)) || (!String.IsNullOrEmpty(r.Surname) && values.Any(r.Surname.ToLower().Contains)) || values.Any(r.MailAddress.ToLower().Contains) || values.Any(r.DisplayName.ToLower().Contains));
                                break;
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail:
                                query = query.Where(r => r.MailAddress.ToLower().Contains(filter.Value.ToLower()));
                                break;
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Name:
                                query = query.Where(r => r.Name.ToLower().StartsWith(filter.Value.ToLower()));
                                break;
                            case Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname:
                                query = query.Where(r => r.Surname.ToLower().StartsWith(filter.Value.ToLower()));
                                break;
                        }
                    }
                    if ((filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Name || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.All || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains || string.IsNullOrEmpty(filter.Value)) && !string.IsNullOrEmpty(filter.StartWith))
                    {
                        if (filter.StartWith != "#")
                            query = query.Where(r => r.FirstLetter == filter.StartWith.ToLower());
                        else
                            query = query.Where(r => pService.DefaultOtherChars().Contains(r.FirstLetter));
                    }
                    if (filter.IdAgency == -3)
                        query = query.Where(r => r.IdProfileType != (int)UserTypeStandard.Employee);
                    else
                    {
                        Dictionary<long, List<Int32>> agencyInfos = pService.GetUsersWithAgencies(query.Where(r => r.IsInternal).Select(r => r.IdPerson).ToList().Distinct().ToList());
                        if (filter.IdAgency == -2)
                            query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee);
                        else if (agencyInfos.ContainsKey(filter.IdAgency))
                            query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee && agencyInfos[filter.IdAgency].Contains(r.IdPerson));
                        else if (filter.IdAgency > 0)
                            query = query.Where(r => 1 == 2);
                    }
                    items = query.ToList();
                }
                catch (Exception ex)
                {

                }
                return items;
            }
        #endregion

            public Person GetPersonFromLite(litePerson person)
            {
                return (person==null)? null : Manager.GetPerson(person.Id);
            }
            public Community GetCommunityFromLite(liteCommunity community)
            {
                return (community == null) ? null : Manager.GetCommunity(community.Id);
            }





            public iTextSharp5.text.Document ExportSubmissionDraftToPDF(
                          Boolean webOnlyRender,
                          long idSubmission,
                          long idRevision,
                          String baseFilePath,
                          String clientFileName,
                          Dictionary<SubmissionTranslations, string> translations,
                          System.Web.HttpResponse webResponse,
                          System.Web.HttpCookie cookie,
                          lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template,
                          CallPrintSettings callPrintSet,
                          CommonPlaceHolderData phData
                          )
            {

                if (template == null)
                    template = new DTO_Template();
                if (template.Body == null)
                    template.Body = new DTO_ElementText();
                if (String.IsNullOrEmpty(template.Body.Text))
                {
                    template.Body.Text = TemplateGetDefaultBody();
                }


                HelperExportToPDF helper = new HelperExportToPDF(translations, template);
                Boolean addContentDisposition = true;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

                    if (!webOnlyRender)
                        webOnlyRender = !(person != null && person.Id > 0);
                    Revision revision = Manager.Get<Revision>(idRevision);

                    iTextSharp5.text.Document file = null;
                    if (revision == null || revision.Submission == null || revision.Submission.Id != idSubmission)
                        file = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie);
                    else
                    {
                        dtoExportSubmission settings = GetSettingsFromRevision(revision, clientFileName, person);

                        addContentDisposition = !settings.ForWebDownload;
                        phData.ModuleObject = Manager.GetPerson(
                            (settings.Submission.SubmittedBy != null) ?
                            settings.Submission.SubmittedBy.Id :
                            settings.Submission.CreatedBy.Id
                            );

                        try
                        {
                            //ToDo: refactor!!!
                            //ToDo: MEGLIO se il Value.Text viene impostato sul nome del file all'UPLOAD del file nel bando.
                            foreach (dtoCallSection<dtoSubmissionValueField> sect in settings.Sections)
                            {
                                foreach (dtoSubmissionValueField vf in sect.Fields)
                                {
                                    if (vf.Field != null &&
                                        vf.Field.Type == FieldType.FileInput
                                        && vf.Value != null
                                        && vf.Value.Link != null
                                        && vf.Value.Link.DestinationItem != null)
                                    {
                                        try
                                        {
                                            lm.Comol.Core.FileRepository.Domain.liteRepositoryItem intFile = Manager.Get<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>(vf.Value.Link.DestinationItem.ObjectLongID);

                                            if (intFile != null)
                                                vf.Value.Text = intFile.DisplayName;


                                            //ModuleLongInternalFile miFile = Manager.Get<ModuleLongInternalFile>(vf.Value.Link.DestinationItem.ObjectLongID);

                                            //if (miFile != null)
                                            //    vf.Value.Text = miFile.DisplayName;

                                        }
                                        catch (Exception) { }
                                    }
                                }
                            }
                        }
                        catch (Exception) { }


                        file = helper.Submission(false, settings, webResponse, cookie, callPrintSet, phData, true); //, callPrintSet);

                    }
                    return file;
                }
                catch (Exception ex)
                {
                    throw new ExportError("", ex) { ErrorPdfDocument = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie) };
                }

            }

            public iTextSharp5.text.Document ExportCallDraftToPDF(
                           long idCall,
                           String clientFileName,
                           Dictionary<SubmissionTranslations, string> translations,
                           System.Web.HttpResponse webResponse,
                           System.Web.HttpCookie cookie,
                           lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template,
                           CallPrintSettings callPrintSet,
                           CommonPlaceHolderData phData,
                           Int64 idSubmitterType
                           )
            {

                if (template == null)
                    template = new DTO_Template();
                if (template.Body == null)
                    template.Body = new DTO_ElementText();
                if (String.IsNullOrEmpty(template.Body.Text))
                {
                    template.Body.Text = TemplateGetDefaultBody();
                }


                List<String> filesToRemove = new List<String>();
                HelperExportToPDF helper = new HelperExportToPDF(translations, template);
                Boolean addContentDisposition = true;
                try
                {
                    //Manager.BeginTransaction();
                    //string fileName = baseFilePath + "\\{0}\\{1}.stored";
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID) ?? Manager.GetLiteUnknownUser();

                    //if (!webOnlyRender)
                    //    webOnlyRender = !(person != null && person.Id > 0);
                    //Revision revision = Manager.Get<Revision>(idRevision);

                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);

                    SubmitterType type = call.SubmittersType.FirstOrDefault(st => st.Id == idSubmitterType);

                    if (type == null)
                        type = call.SubmittersType.OrderBy(st => st.DisplayOrder).FirstOrDefault();

                    //ToDo: manage Error!
                    //Se non ci sono DESTINATARI, non è possibile stampare!!!


                    if (type == null)
                        return null;

                    UserSubmission fakeSubmission = GetFakeSubmission(call, person, type);





                    iTextSharp5.text.Document file = null;
                    //if (revision == null || revision.Submission == null || revision.Submission.Id != idSubmission)
                    //    file = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie);
                    //else
                    //{
                    ModuleLongInternalFile internalFile = null;

                    dtoExportSubmission settings = null;

                    settings = GetSettingsFromSubmission(fakeSubmission, clientFileName, "", person);

                    settings.Filename = "";

                    addContentDisposition = !settings.ForWebDownload;


                    //phData.ModuleObject = Manager.GetPerson(settings.Submission.SubmittedBy.Id);
                    phData.ModuleObject = Manager.GetPerson(
                            (settings.Submission.SubmittedBy != null) ?
                            settings.Submission.SubmittedBy.Id :
                            settings.Submission.CreatedBy.Id
                            );

                    try
                    {
                        //ToDo: refactor!!!
                        //ToDo: MEGLIO se il Value.Text viene impostato sul nome del file all'UPLOAD del file nel bando.
                        foreach (dtoCallSection<dtoSubmissionValueField> sect in settings.Sections)
                        {
                            foreach (dtoSubmissionValueField vf in sect.Fields)
                            {
                                if (vf.Field != null &&
                                    vf.Field.Type == FieldType.FileInput
                                    && vf.Value != null
                                    && vf.Value.Link != null
                                    && vf.Value.Link.DestinationItem != null)
                                {
                                    try
                                    {
                                        ModuleLongInternalFile miFile = Manager.Get<ModuleLongInternalFile>(vf.Value.Link.DestinationItem.ObjectLongID);

                                        if (miFile != null)
                                            vf.Value.Text = miFile.DisplayName;

                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                    catch (Exception) { }

                    //Sono in DRAFT!
                    //string waterMark = ();




                    file = helper.Submission(false, settings, webResponse, cookie, callPrintSet, phData, true); //, callPrintSet);

                    //file = helper.Submission(false, settings, webResponse, cookie, callPrintSet, phData); //, callPrintSet);


                    //Manager.Commit();
                    //Delete.Files(filesToRemove);
                    return file;
                }
                catch (Exception ex)
                {
                    //Manager.RollBack();
                    throw new ExportError("", ex) { ErrorPdfDocument = helper.GetErrorDocument(false, addContentDisposition, clientFileName, webResponse, cookie) };
                }

            }

            public bool NeedSignature(Int64 CallId)
            {
                CallForPaper call = Manager.Get<CallForPaper>(CallId);
                return call != null && call.AttachSign;
            }

        public bool UserHasOtherSubmissionType(long idCall, int idPerson, long idSubmitter)
        {
            bool HasOtherTypeSubmission = (from UserSubmission sub in Manager.GetIQ<UserSubmission>()
                         where sub.Deleted == BaseStatusDeleted.None
                         && sub.Call != null
                         && sub.Call.Id == idCall
                         && (sub.Owner != null && sub.Owner.Id == idPerson)
                         && sub.Type != null && sub.Type.Id != idSubmitter
                         select sub.Id).Any();

            return HasOtherTypeSubmission;
        }
    }
}