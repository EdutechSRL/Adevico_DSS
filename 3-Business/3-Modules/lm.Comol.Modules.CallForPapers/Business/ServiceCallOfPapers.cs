using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Modules.CallForPapers.Domain;
//using ICSharpCode.SharpZipLib;
//using ICSharpCode.SharpZipLib.Zip;
//using System.IO;
using iTextSharp.text;
using lm.Comol.Core.File;
namespace lm.Comol.Modules.CallForPapers.Business
{
    public partial class ServiceCallOfPapers : BaseService
    {
        #region initClass
            public ServiceCallOfPapers() { }

            public ServiceCallOfPapers(iApplicationContext oContext)
                : base(oContext)
            {
                _DtService = new lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService(oContext);
            }
            public ServiceCallOfPapers(iDataContext oDC)
                : base(oDC)
            {
                _DtService = new lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService(oDC);
            }

            private lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService _DtService { get; set; }

        #endregion

            public int ServiceModuleID()
            {
                return ServiceModuleID(ModuleCallForPaper.UniqueCode);
            }

        #region iLinkedService
            protected override Boolean AllowCallAction(StandardActionType actionType, long idCall, litePerson person, int idCommunity,int idRole, ModuleObject destination)
            {
                Boolean iResponse = false;
                iResponse = (from s in Manager.GetIQ<CallForPaper>() where s.Id == idCall select s.Id).Any();
                if (iResponse)
                {
                    switch (actionType) { 
                        case StandardActionType.Edit:
                            ModuleCallForPaper m = ModuleCallForPaper.CreatePortalmodule(person.TypeID);
                            long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls;
                            Boolean isOwner = (from s in Manager.GetIQ<CallForPaper>() where s.Id == idCall && s.CreatedBy == person select s.Id).Any();
                            iResponse = isOwner ||(idCommunity==0 && (m.Administration || m.EditCallForPaper || m.ManageCallForPapers)) || Manager.HasModulePermission(person.Id, idRole, idCommunity, this.ServiceModuleID(), permission);
                            break;
                    }
                }
                return iResponse;
            }
            protected override List<StandardActionType> GetStandardActionForFileOfCall(long idCall, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                List<StandardActionType> actions = new List<StandardActionType>();
                lm.Comol.Core.FileRepository.Domain.ItemType type = (from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() where f.Id == idItem select f.Type).Skip(0).Take(1).ToList().FirstOrDefault();
                long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.ListCalls;
                Boolean isOwner = (from s in Manager.GetIQ<CallForPaper>() where s.Id == idCall && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();
                Boolean hasPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                Boolean hasEditPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls);

                if (isOwner || hasPermission || isEvaluatorOfCallForPaper(idCall, idUser))
                {
                    actions.Add(StandardActionType.Play);
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                            break;
                    }
                }
                if (isOwner || hasEditPermission)
                {
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewAdvancedStatistics);
                            actions.Add(StandardActionType.EditMetadata);
                            break;
                    }
                }
                return actions;
            }
            protected override List<StandardActionType> GetStandardActionForAttachment(long idAttachment, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                List<StandardActionType> actions = new List<StandardActionType>();
                lm.Comol.Core.FileRepository.Domain.ItemType type = (from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() where f.Id == idItem select f.Type).Skip(0).Take(1).ToList().FirstOrDefault();
                long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.ListCalls;
                AttachmentFile attachment = Manager.Get<AttachmentFile>(idAttachment);
                if (attachment != null && attachment.Call !=null)
                {
                    Boolean isOwner = (from s in Manager.GetIQ<CallForPaper>() where s.Id == attachment.Call.Id && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();
                    Boolean hasPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                    Boolean hasEditPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls);

                    if (isOwner || hasEditPermission || isEvaluatorOfCallForPaper(attachment.Call.Id, idUser) || (hasPermission && (attachment.ForAll || AllowSeeAttachment(attachment,person) )))
                    {
                        actions.Add(StandardActionType.Play);
                        switch (type)
                        {
                            case Core.FileRepository.Domain.ItemType.Multimedia:
                            case Core.FileRepository.Domain.ItemType.ScormPackage:
                                actions.Add(StandardActionType.ViewPersonalStatistics);
                                break;
                        }
                    }
                    if (isOwner || hasEditPermission)
                    {
                        switch (type)
                        {
                            case Core.FileRepository.Domain.ItemType.Multimedia:
                            case Core.FileRepository.Domain.ItemType.ScormPackage:
                                actions.Add(StandardActionType.ViewAdvancedStatistics);
                                actions.Add(StandardActionType.EditMetadata);
                                break;
                        }
                    }
                }
                return actions;
            }
            protected override List<StandardActionType> GetStandardActionForFileOfSubmission(long idSubmission, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                List<StandardActionType> actions = new List<StandardActionType>();
                lm.Comol.Core.FileRepository.Domain.ItemType type = (from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() where f.Id == idItem select f.Type).Skip(0).Take(1).ToList().FirstOrDefault();
                long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls;
                Boolean isOwner = (from s in Manager.GetIQ<UserSubmission>() where s.Id == idSubmission && (s.CreatedBy == person || s.Owner == person) select s.Id).Any();
                Boolean hasPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                Boolean isEvaluator = isEvaluatorOfSubmission(idSubmission, idUser);

                if (isOwner || hasPermission || isEvaluator)
                {
                    actions.Add(StandardActionType.Play);
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                            break;
                    }
                }
                switch (type)
                {
                    case Core.FileRepository.Domain.ItemType.Multimedia:
                    case Core.FileRepository.Domain.ItemType.ScormPackage:
                        if (isOwner)
                            actions.Add(StandardActionType.EditMetadata);
                        if (isOwner || hasPermission)
                            actions.Add(StandardActionType.ViewAdvancedStatistics);
                        break;
                }
                return actions;
            }
            protected override List<StandardActionType> GetStandardActionForSubmittedFile(long idSubmittedFile, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                List<StandardActionType> actions = new List<StandardActionType>();
                lm.Comol.Core.FileRepository.Domain.ItemType type = (from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() where f.Id == idItem select f.Type).Skip(0).Take(1).ToList().FirstOrDefault();
                long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls;

                Boolean isOwner = (from sf in Manager.GetAll<SubmittedFile>(sf => sf.Id == idSubmittedFile &&
                             (sf.CreatedBy == person || sf.Submission.CreatedBy == person || sf.Submission.Owner == person))
                                   select sf.Id).Any();

                Boolean hasPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                Boolean isEvaluator = isEvaluatorOfSubmission((from sf in Manager.GetIQ<SubmittedFile>() where sf.Id == idSubmittedFile select sf.Submission.Id).Skip(0).Take(1).ToList().FirstOrDefault(), idUser);

                if (isOwner || hasPermission || isEvaluator)
                {
                    actions.Add(StandardActionType.Play);
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                            break;
                    }
                }
                switch (type)
                {
                    case Core.FileRepository.Domain.ItemType.Multimedia:
                    case Core.FileRepository.Domain.ItemType.ScormPackage:
                        if (isOwner)
                            actions.Add(StandardActionType.EditMetadata);
                        if (isOwner || hasPermission)
                            actions.Add(StandardActionType.ViewAdvancedStatistics);
                        break;
                }
                return actions;
            }

            protected override Boolean AllowDownloadFileOfCall(long idCall, int idUser, litePerson person, int idCommunity, int idRole)
            {
                Boolean iResponse = false;
                iResponse = (from s in Manager.GetIQ<CallForPaper>() where s.Id == idCall && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();
                if (!iResponse)
                {
                    long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.ListCalls;

                    iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                    if (!iResponse)
                    {
                        iResponse = IsCallAvailableByUser(idCall, person);
                        if (!iResponse)
                            iResponse = isEvaluatorOfCallForPaper(idCall, idUser);
                    }
                }
                return iResponse;
            }
            protected override Boolean AllowAttachmentDownload(long idAttachment, int idUser, litePerson person, int idCommunity, int idRole)
            {
                Boolean iResponse = false;
                AttachmentFile attachment = Manager.Get<AttachmentFile>(idAttachment);
                if (attachment != null && attachment.Call != null)
                {
                    iResponse = (from s in Manager.GetIQ<CallForPaper>() where s.Id == attachment.Call.Id && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();
                    if (!iResponse)
                    {
                        iResponse = IsCallAvailableByUser(attachment.Call.Id, person);
                        //long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.ListCalls;

                        //iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                        if (iResponse)
                            iResponse = (attachment.ForAll || AllowSeeAttachment(attachment, person));
                        if (!iResponse)
                            iResponse = isEvaluatorOfCallForPaper(attachment.Call.Id, idUser);
                    }
                }
                return iResponse;
            }

            protected override bool AllowDownloadSignFile(long idRevision, int idUser, litePerson person, int idCommunity, int idRole)
            {
                Boolean iResponse = false;
                Revision revision = Manager.Get<Revision>(idRevision);
                if (revision != null && revision.Submission != null && revision.Submission.Call != null)
                {
                    //Sottomittore
                    iResponse = revision.Submission.SubmittedBy.Id == idUser
                        //Amministratori del bando
                        || Manager.HasModulePermission(
                                                idUser,
                                                idRole,
                                                idCommunity,
                                                this.ServiceModuleID(),
                                                (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls)
                        //Valutatori
                        || isEvaluatorOfCallForPaper(revision.Submission.Call.Id, idUser);
                    //Creatore del bando
                    //|| revision.Submission.Call.CreatedBy.Id == idUser
                }
                return iResponse;
            }

            protected override Boolean AllowDownloadFileOfSubmission(long idSubmission, int idUser, litePerson person, int idCommunity, int idRole)
            {
                Boolean iResponse = false;
                iResponse = (from s in Manager.GetIQ<UserSubmission>() where s.Id == idSubmission && (s.CreatedBy == person || s.Owner == person) select s.Id).Any();
                if (!iResponse)
                {
                    long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls;

                    iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                    if (!iResponse)
                        iResponse = isEvaluatorOfSubmission(idSubmission, idUser);
                }
                return iResponse;
            }
            protected override Boolean AllowDownloadSubmittedFile(long idSubmittedFile, int idUser, litePerson person, int idCommunity, int idRole)
            {
                Boolean iResponse = false;
                iResponse = (from sf in Manager.GetAll<SubmittedFile>(sf => sf.Id == idSubmittedFile &&
                             (sf.CreatedBy == person || sf.Submission.CreatedBy == person || sf.Submission.Owner == person))
                             select sf.Id).Any();
                if (!iResponse)
                {
                    long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls;

                    iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                    if (!iResponse)
                        iResponse = isEvaluatorOfSubmission((from sf in Manager.GetIQ<SubmittedFile>() where sf.Id == idSubmittedFile select sf.Submission.Id).Skip(0).Take(1).ToList().FirstOrDefault(), idUser);

                }
                return iResponse;
            }
            
        #endregion
        public CallForPaper GetCallForPaper(long callForPaperID)
        {
            return Manager.Get<CallForPaper>(callForPaperID);
        }
        public EvaluationType GetEvaluationType(long idCall)
        {
            EvaluationType result = EvaluationType.Sum;
            try
            {
                result = (from c in Manager.GetIQ<CallForPaper>() where c.Id== idCall select c.EvaluationType).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public Boolean CallUseDss(long idCall)
        {
            Boolean result = false;
            try
            {
                result = (from c in Manager.GetIQ<CallForPaper>()
                          where c.Id == idCall && c.EvaluationType == EvaluationType.Dss
                          select c.Id).Any();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public dtoCallForPaper GetDtoCallForPaper(long callForPaperID)
        {
            return (from c in Manager.GetAll<CallForPaper>(cfp => cfp.Id == callForPaperID)
                    select new dtoCallForPaper()
                    {
                        AwardDate = c.AwardDate,
                        Description = c.Description,
                        Deleted = c.Deleted,
                        DisplayWinner = c.DisplayWinner,
                        Edition = c.Edition,
                        EndDate = c.EndDate,
                        Id = c.Id,
                        IsPublic = c.IsPublic,
                        StartDate = c.StartDate,
                        Name = c.Name,
                        Status = c.Status,
                        SubmissionClosed = c.SubmissionClosed,
                        Type = c.Type,
                        OverrideHours = c.OverrideHours,
                        OverrideMinutes = c.OverrideMinutes,
                        Community = c.Community,
                        Owner = c.CreatedBy,
                        NotificationEmail = c.NotificationEmail,
                        AllowSubmissionExtension = c.UseStartCompilationDate,
                        EndEvaluationOn = c.EndEvaluationOn,
                        EvaluationType = c.EvaluationType
                    }).FirstOrDefault();
        }

        #region "Call Presentation Methods"
            public List<CallStatusForSubmitters> GetAvailableViews(CallStandardAction action, ModuleCallForPaper module, Boolean fromAllcommunities, int idCommunity, int idUser)
            {
                List<CallStatusForSubmitters> views = GetAvailableViews(action, (module.Administration || module.ManageCallForPapers), fromAllcommunities, (idCommunity == 0), idCommunity, idUser, CallForPaperType.CallForBids);

                liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                litePerson person = Manager.GetLitePerson(idUser);

                if (CallToEvaluateCount(fromAllcommunities, (idCommunity==0), community, person) > 0)
                    views.Add(CallStatusForSubmitters.ToEvaluate);
                if (CallEvaluatedCount(fromAllcommunities, (idCommunity == 0), community, person) > 0)
                    views.Add(CallStatusForSubmitters.Evaluated);
                return views;
            }

            public long CallToEvaluateCount(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person)
            {
                long count = 0;
                count = ((from s in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                          where s.Deleted == BaseStatusDeleted.None && s.Status != Domain.Evaluation.EvaluationStatus.EvaluatorReplacement && s.Status != Domain.Evaluation.EvaluationStatus.Invalidated && (fromAllcommunities || (forPortal && s.Call.IsPortal) || (!forPortal && s.Community == community)) && !s.Evaluated && s.Evaluator.Person == person
                          select s.Call.Id).Distinct().ToList()).Count();
                return count;
            }
            public long CallEvaluatedCount(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person)
            {
                long count = 0;
                List<long> notEvaluated = GetCallsForEvaluationId(fromAllcommunities, forPortal, community, person, DisplayEvaluationStatus.ToEvaluate);
                List<long> evaluated = GetCallsForEvaluationId(fromAllcommunities, forPortal, community, person, DisplayEvaluationStatus.Evaluated);
                if (evaluated.Count > 0)
                    count = (from idCall in evaluated where !notEvaluated.Contains(idCall) select idCall).Distinct().Count();
                return count;
            }
            public long CallCountBySubmission(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, int idCommunity, int idUser, CallStatusForSubmitters status)
            {
                try
                {
                    liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                    litePerson person = Manager.GetLitePerson(idUser);
                    return CallCountBySubmission(forAdministrationMode,fromAllcommunities, forPortal, community, person, status);
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            public long CallCountBySubmission(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, CallStatusForSubmitters status)
            {
                long count = 0;
                switch (status){
                    case CallStatusForSubmitters.ToEvaluate:
                        count = CallToEvaluateCount(fromAllcommunities, forPortal, community, person);
                        break;
                    case CallStatusForSubmitters.Evaluated:
                        count = CallEvaluatedCount(fromAllcommunities, forPortal, community, person);
                        break;
                    default:
                        count = CallCountBySubmission(forAdministrationMode,fromAllcommunities, forPortal, community, person, CallForPaperType.CallForBids, status);
                        break;
                }
                return count;
            }

            private List<long> GetCallsForEvaluationId(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, DisplayEvaluationStatus status)
            {
                return (from s in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                        where s.Deleted == BaseStatusDeleted.None 
                        && s.Status != Domain.Evaluation.EvaluationStatus.EvaluatorReplacement 
                        && s.Status!= Domain.Evaluation.EvaluationStatus.Invalidated 
                        && s.Call != null
                        && (fromAllcommunities 
                            || (forPortal && s.Call.IsPortal) 
                            || (!forPortal && s.Call.Community != null && s.Call.Community.Id == community.Id)) 
                        &&  (s.Evaluator != null && s.Evaluator.Person != null && s.Evaluator.Person.Id == person.Id)
                        && (status == DisplayEvaluationStatus.Any 
                            || (status == DisplayEvaluationStatus.Evaluated && s.Evaluated == true) 
                            || (status == DisplayEvaluationStatus.ToEvaluate && s.Evaluated == false))
                        select s.Call.Id).Distinct().ToList();
            }

            private List<long> GetCallsForAdvanceEvaluationId(int communityId, int personId)
            {
                return (from Advanced.Domain.AdvCommission cm  in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                    where cm.Call != null
                    && cm.Call.Community != null && cm.Call.Community.Id == communityId
                    && cm.Deleted == BaseStatusDeleted.None
                    && 
                        (cm.President != null && cm.President.Id == personId)
                        || (cm.Secretary != null && cm.Secretary.Id == personId)
                        || (cm.Members != null && cm.Members.Any(m => m.Member != null && m.Member.Id == personId && m.Deleted == BaseStatusDeleted.None))
                        select cm.Call.Id
                        ).Distinct().ToList();

                //return (from s in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                //        where s.Deleted == BaseStatusDeleted.None
                //        && s.Status != Domain.Evaluation.EvaluationStatus.EvaluatorReplacement
                //        && s.Status != Domain.Evaluation.EvaluationStatus.Invalidated
                //        && s.Call != null
                //        && s.Call.Community != null && s.Call.Community.Id == communityId
                //        && (s.AdvEvaluator != null && s.AdvEvaluator.Member != null && s.AdvEvaluator.Member.Id == personId)
                //        select s.Call.Id).Distinct().ToList();
            }

        public List<dtoCallItemPermission> GetCallForPapers(ModuleCallForPaper module, CallStandardAction action,Boolean fromAllcommunities, Boolean forPortal, int idCommunity,int idPerson, CallStatusForSubmitters status,FilterCallVisibility filter, int pageIndex, int pageSize) {
                liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                litePerson person = Manager.GetLitePerson(idPerson);
                return GetCallForPapers(module, action, fromAllcommunities, forPortal, community, person, status, filter, pageIndex, pageSize);
            }
            public List<dtoCallItemPermission> GetCallForPapers(ModuleCallForPaper module, CallStandardAction action, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
            {
                List<dtoCallItemPermission> items = null;
                if (action == CallStandardAction.Manage)
                    items = GetCalls(module, forPortal, community, person, status, filter, pageIndex, pageSize);
                else
                    items = GetCalls(fromAllcommunities, forPortal, community, person, status, filter, pageIndex, pageSize);
                return items;
            }

            protected List<dtoCallItemPermission> GetCalls(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
            {
                List<dtoCallItemPermission> items = null;
                try
                {
                    DateTime currentTime = DateTime.Now;
                    Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);

                    List<long> idCallsToEvaluate = new List<long>();
                    var query = GetBaseUserSubmissionQuery(fromAllcommunities, forPortal, community, person, CallForPaperType.CallForBids, filter);

                    List<long> idCallsToEvaluateAdv = GetCallsForAdvanceEvaluationId(community.Id, person.Id);

                    switch (status) { 
                        case CallStatusForSubmitters.Submitted:
                            var callSubmitted = query.Where(s => s.Status >= SubmissionStatus.submitted).Select(s => new { id = s.Call.Id, Name = s.Call.Name, EndDate = s.Call.EndDate }).ToList().Distinct().ToList();
                            List<long> idSubCalls = callSubmitted.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name).Skip(pageIndex * pageSize).Take(pageSize).Select(c => c.id).ToList();

                            items = (from c in Manager.GetIQ<CallForPaper>() where idSubCalls.Contains(c.Id) select c).OrderByDescending(c => c.EndDate).ThenBy(c => c.Name).ToList().ToList().Select(s =>
                                    new dtoCallItemPermission(s.Id, s.Community, status,
                                    new dtoCall()
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                            IdDssMethod = s.IdDssMethod,
                                            IdDssRatingSet = s.IdDssRatingSet,
                                            IsDssMethodFuzzy = s.IsDssMethodFuzzy,
                                            UseManualWeights = s.UseManualWeights,
                                            OrderedWeights = s.UseOrderedWeights,
                                            IsValidFuzzyMeWeights = s.IsValidFuzzyMeWeights,
                                            FuzzyMeWeights = s.FuzzyMeWeights,
                                            Edition = s.Edition,
                                            Summary = s.Summary,
                                            Description = s.Description,
                                            StartDate = s.StartDate,
                                            EndDate = s.EndDate,
                                            AwardDate = s.AwardDate,
                                            SubmissionClosed = s.SubmissionClosed,
                                            Type = s.Type,
                                            DisplayWinner = s.DisplayWinner,
                                            Community = s.Community,
                                            IsPublic = s.IsPublic,
                                            IsPortal = s.IsPortal,
                                            ForSubscribedUsers = s.ForSubscribedUsers,
                                            Status = s.Status,
                                            EvaluationType = s.EvaluationType,
                                            AllowSubmissionExtension = s.UseStartCompilationDate,
                                              AdvacedEvaluation = s.AdvacedEvaluation
                                    }
                                    )).ToList();
                            break;
                        case CallStatusForSubmitters.SubmissionClosed:
                        case CallStatusForSubmitters.SubmissionOpened:
                            //idCallsToEvaluate = GetCallsForEvaluationId(fromAllcommunities, forPortal, community, person, DisplayEvaluationStatus.Any);

                            //idCallsToEvaluate = idCallsToEvaluate.Union(idCallsToEvaluateAdv).ToList();
                    
                        var queryCalls = GetBaseForPaperQuery(false,fromAllcommunities, forPortal, community, person,  CallForPaperType.CallForBids, status, FilterCallVisibility.OnlyVisible);
                            
                            switch (status)
                            {
                                case CallStatusForSubmitters.SubmissionClosed:
                                    queryCalls = queryCalls.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                case CallStatusForSubmitters.SubmissionOpened:
                                    queryCalls = queryCalls.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                default:
                                    queryCalls = queryCalls.OrderByDescending(c => c.CreatedOn).ThenBy(c => c.Name);
                                    break;
                            }
                            List<long> idSubmittedCalls = query.Where(s => s.Status >= SubmissionStatus.submitted).Select(s => s.Call.Id).ToList();
                            //if (idSubmittedCalls.Count > 0)
                            //    idSubmittedCalls = query.Where(s => s.Status < SubmissionStatus.submitted && !idSubmittedCalls.Contains(s.Call.Id)).Select(s => s.Call.Id).ToList();

                            if (idSubmittedCalls.Count > 0)
                            {
                                var querySubmission = (from s in Manager.GetIQ<UserSubmission>()
                                                       where s.Deleted == BaseStatusDeleted.None && (!isAnonymous && s.Owner == person) && idSubmittedCalls.Contains(s.Call.Id)
                                                       select new { Id = s.Id, IdSubmitter = s.Type.Id, Status = s.Status  }).ToList();
                                List<long> idSubmitters = querySubmission.Select(sb => sb.IdSubmitter).ToList().Distinct().ToList();
                                List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                                  where idSubmitters.Contains(s.Id)
                                                                  select s).ToList();
                                foreach (SubmitterType submitter in submitters.Where(s => s.AllowMultipleSubmissions).ToList())
                                {
                                    if (querySubmission.Where(s => s.IdSubmitter == submitter.Id).Count() < submitter.MaxMultipleSubmissions || (querySubmission.Where(s => s.IdSubmitter == submitter.Id).Count() == submitter.MaxMultipleSubmissions && querySubmission.Where(s => s.IdSubmitter == submitter.Id && s.Status < SubmissionStatus.submitted).Any()))
                                        idSubmittedCalls = idSubmittedCalls.Where(i=> i != submitter.Call.Id).ToList();
                                }
                            }

                            var calls = queryCalls.Select(c => new { Id = c.Id, IsPublic = c.IsPublic, ForSubscribedUsers = c.ForSubscribedUsers }).ToList();

                            List<long> idCalls = GetIdCallsBySubmissionQuery(calls.Where(c => !c.IsPublic && !c.ForSubscribedUsers && !idSubmittedCalls.Contains(c.Id)).Select(c => c.Id).ToList(),
                                fromAllcommunities,forPortal,community,person, CallForPaperType.CallForBids, status,filter);

                        idCalls = idCalls.Union(idCallsToEvaluateAdv).ToList();

                        items = queryCalls.Where(c => ((c.IsPublic || (c.ForSubscribedUsers && person != null && person.TypeID != (Int32)UserTypeStandard.Guest && person.TypeID != (Int32)UserTypeStandard.PublicUser)) && !idSubmittedCalls.Contains(c.Id)) || idCalls.Contains(c.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList().ToProxySafeList().Select(c =>
                                 new dtoCallItemPermission(c.Id, c.Community, status ,
                                 new dtoCall()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     IdDssMethod = c.IdDssMethod,
                                     IdDssRatingSet = c.IdDssRatingSet,
                                     IsDssMethodFuzzy = c.IsDssMethodFuzzy,
                                     UseManualWeights = c.UseManualWeights,
                                     OrderedWeights = c.UseOrderedWeights,
                                     IsValidFuzzyMeWeights = c.IsValidFuzzyMeWeights,
                                     FuzzyMeWeights = c.FuzzyMeWeights,
                                     Edition = c.Edition,
                                     Summary = c.Summary,
                                     Description = c.Description,
                                     StartDate = c.StartDate,
                                     EndDate = c.EndDate,
                                     AwardDate = ((CallForPaper)c).AwardDate,
                                     SubmissionClosed = c.SubmissionClosed,
                                     Type = c.Type,
                                     DisplayWinner = ((CallForPaper)c).DisplayWinner,
                                     Community = c.Community,
                                     IsPublic = c.IsPublic,
                                     ForSubscribedUsers = c.ForSubscribedUsers,
                                     IsPortal = c.IsPortal,
                                     Status = c.Status,
                                     EvaluationType = ((CallForPaper)c).EvaluationType,
                                     AllowSubmissionExtension = c.UseStartCompilationDate,
                                     Deleted = c.Deleted,
                                     AdvacedEvaluation = ((CallForPaper)c).AdvacedEvaluation
                                 }
                                 )).ToList();
                           
                            break;

                        case CallStatusForSubmitters.Evaluated:
                        case CallStatusForSubmitters.ToEvaluate:
                            idCallsToEvaluate = GetCallsForEvaluationId(fromAllcommunities, forPortal, community, person, DisplayEvaluationStatus.ToEvaluate);
                            

                            if (status == CallStatusForSubmitters.Evaluated) {
                                List<long> idEvaluatedCalls = GetCallsForEvaluationId(false, forPortal, community, person, DisplayEvaluationStatus.Evaluated);
                                idCallsToEvaluate = idEvaluatedCalls.Except(idCallsToEvaluate).ToList();
                            }

                            IList<long> takeCall = idCallsToEvaluate.Union(idCallsToEvaluateAdv).Distinct().ToList();

                            items =(from c in Manager.GetIQ<CallForPaper>()
                                where c.Deleted == BaseStatusDeleted.None && takeCall.Contains(c.Id)
                                orderby c.EndEvaluationOn, c.EndDate descending, c.Name
                                    select c).Skip(pageIndex * pageSize).Take(pageSize).ToList().Select(c =>
                                new dtoCallItemPermission(c.Id, c.Community, status,
                                        new dtoCall()
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            IdDssMethod = c.IdDssMethod,
                                            IdDssRatingSet = c.IdDssRatingSet,
                                            IsDssMethodFuzzy = c.IsDssMethodFuzzy,
                                            UseManualWeights = c.UseManualWeights,
                                            OrderedWeights = c.UseOrderedWeights,
                                            IsValidFuzzyMeWeights = c.IsValidFuzzyMeWeights,
                                            FuzzyMeWeights = c.FuzzyMeWeights,
                                            Edition = c.Edition,
                                            Summary = c.Summary,
                                            Description = c.Description,
                                            StartDate = c.StartDate,
                                            EndDate = c.EndDate,
                                            AwardDate = ((CallForPaper)c).AwardDate,
                                            SubmissionClosed = c.SubmissionClosed,
                                            Type = c.Type,
                                            DisplayWinner = ((CallForPaper)c).DisplayWinner,
                                            Community = c.Community,
                                            IsPublic = c.IsPublic,
                                            Status = c.Status,
                                            EvaluationType = ((CallForPaper)c).EvaluationType,
                                            AllowSubmissionExtension = c.UseStartCompilationDate,
                                            Owner = c.CreatedBy,
                                            Deleted = c.Deleted,
                                            AdvacedEvaluation = ((CallForPaper)c).AdvacedEvaluation
                                        }
                                        )).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                             break;
                    }
                    if (items.Count > 0)
                        items.ForEach(c => c.SubmissionsInfo = (from s in Manager.GetIQ<UserSubmission>()
                                                                where s.Type != null && s.Call != null && s.Call.Id == c.Id && s.Owner == person && s.Deleted == BaseStatusDeleted.None
                                                                select s).ToList().Select(
                                                                s => new dtoSubmissionDisplayInfo(s.Id, s.Revisions.Where(r=> r.Deleted== BaseStatusDeleted.None).ToList(),false) { IdSubmitterType = s.Type.Id, Status = s.Status, Owner = person, ModifiedOn = s.ModifiedOn, SubmittedBy = s.SubmittedBy, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate, UniqueId = s.UserCode }).ToList());
                    items.Where(i => idCallsToEvaluate.Contains(i.Id)).ToList().ForEach(i => i.RefreshEvaluations(GetItemsToEvaluate(i.Id, person), GetItemsEvaluated(i.Id, person)));

                    if (fromAllcommunities && items.Count>0){
                        List<int> idCommunities = items.Where(c => c.Community != null).Select(c => c.Community.Id).Distinct().ToList();
                        if (items.Where(c => c.Call.IsPortal).Any())
                        {
                            idCommunities.Insert(0, 0);
                        }
                        Dictionary<int, ModuleCallForPaper> permissions = GetCallCommunitiesPermission(idCommunities, person);
                        items.ForEach(i => i.RefreshUserPermission(permissions, person));   
                    }
                    else if (items.Count>0){
                        items.ForEach(i => i.RefreshUserPermission(CallForPaperServicePermission(person, (community == null) ? 0 : community.Id ), person));   
                    }
                    items.Where(i => i.SubmissionsInfo != null & i.SubmissionsInfo.Count > 0).ToList().ForEach(i => i.AllowSubmissionAs = GetSubmissionAs(i.Id, i.SubmissionsInfo.Select(si=>si.IdSubmitterType).Distinct().ToList(),person));
                }
                catch (Exception ex)
                {
                    items = new List<dtoCallItemPermission>();
                }
                return items;
            }
            protected List<dtoCallItemPermission> GetCalls(ModuleCallForPaper module, Boolean forPortal, liteCommunity community, litePerson person, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
            {
                List<dtoCallItemPermission> items = null;
                try
                {
                    Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);

                    List<long> idCallsToEvaluate = new List<long>();
                    
                    if (status != CallStatusForSubmitters.Draft)
                        idCallsToEvaluate = GetCallsForEvaluationId(false, forPortal, community, person, DisplayEvaluationStatus.ToEvaluate);


                    List<long> idCallsToEvaluateAdv = GetCallsForAdvanceEvaluationId(community.Id, person.Id);


                    switch (status) { 
                        case CallStatusForSubmitters.Draft:
                        case CallStatusForSubmitters.SubmissionClosed:
                        case CallStatusForSubmitters.SubmissionOpened:
                            var queryCall = GetBaseForPaperQuery(true,false, forPortal, community, person, CallForPaperType.CallForBids, status, filter);
                            switch (status)
                            {
                                case CallStatusForSubmitters.Draft:
                                    queryCall = queryCall.OrderByDescending(c => c.CreatedOn).ThenBy(c => c.Name);
                                    break;
                                case CallStatusForSubmitters.SubmissionClosed:
                                    queryCall = queryCall.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                case CallStatusForSubmitters.SubmissionOpened:
                                    queryCall = queryCall.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                default:
                                    queryCall = queryCall.OrderByDescending(c => c.CreatedOn).ThenBy(c => c.Name);
                                    break;
                            }
                            items = queryCall.Select(c =>
                                     new dtoCallItemPermission(c.Id, c.Community, status,
                                     new dtoCall()
                                          {
                                             Id = c.Id,
                                             Name = c.Name,
                                             Edition = c.Edition,
                                             Summary = c.Summary,
                                             Description = c.Description,
                                             StartDate = c.StartDate,
                                             EndDate = c.EndDate,
                                             AwardDate = ((CallForPaper)c).AwardDate,
                                             SubmissionClosed = c.SubmissionClosed,
                                             Type = c.Type,
                                             DisplayWinner = ((CallForPaper)c).DisplayWinner,
                                             Community = c.Community,
                                             IsPublic = c.IsPublic,
                                             Status = c.Status,
                                             EvaluationType = ((CallForPaper)c).EvaluationType,
                                             AllowSubmissionExtension = c.UseStartCompilationDate,
                                             Owner = c.CreatedBy,
                                             Deleted = c.Deleted,
                                             AdvacedEvaluation = ((CallForPaper)c).AdvacedEvaluation
                                     }
                                     )).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                            break;
                        case CallStatusForSubmitters.Evaluated:
                        case CallStatusForSubmitters.ToEvaluate:
                            if (status == CallStatusForSubmitters.Evaluated) {
                                List<long> idCalls = GetCallsForEvaluationId(false, forPortal, community, person, DisplayEvaluationStatus.Evaluated);
                                idCallsToEvaluate = idCalls.Except(idCallsToEvaluate).ToList();
                            }

                            IList<long> takeCallId = idCallsToEvaluate.Union(idCallsToEvaluateAdv).Distinct().ToList();


                            //&& idCallsToEvaluate.Contains(c.Id)

                            items =(from c in Manager.GetIQ<CallForPaper>()
                                where c.Deleted == BaseStatusDeleted.None && takeCallId.Contains(c.Id)
                                orderby c.EndEvaluationOn, c.EndDate descending , c.Name
                                select c).Skip(pageIndex * pageSize).Take(pageSize).ToList().Select(c=> new dtoCallItemPermission(c.Id, c.Community, status,
                                        new dtoCall()
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            IdDssMethod = c.IdDssMethod,
                                            IdDssRatingSet = c.IdDssRatingSet,
                                            IsDssMethodFuzzy = c.IsDssMethodFuzzy,
                                            UseManualWeights = c.UseManualWeights,
                                            OrderedWeights = c.UseOrderedWeights,
                                            IsValidFuzzyMeWeights = c.IsValidFuzzyMeWeights,
                                            FuzzyMeWeights = c.FuzzyMeWeights,
                                            Edition = c.Edition,
                                            Summary = c.Summary,
                                            Description = c.Description,
                                            StartDate = c.StartDate,
                                            EndDate = c.EndDate,
                                            AwardDate = ((CallForPaper)c).AwardDate,
                                            SubmissionClosed = c.SubmissionClosed,
                                            Type = c.Type,
                                            DisplayWinner = ((CallForPaper)c).DisplayWinner,
                                            Community = c.Community,
                                            IsPublic = c.IsPublic,
                                            Status = c.Status,
                                            EvaluationType = ((CallForPaper)c).EvaluationType,
                                            AllowSubmissionExtension = c.UseStartCompilationDate,
                                            Owner = c.CreatedBy,
                                            Deleted = c.Deleted,
                                            AdvacedEvaluation = c.AdvacedEvaluation
                                        }
                                        )).ToList();

                            break;
                    }
                    items
                        .Where(i => idCallsToEvaluate.Contains(i.Id))
                        .ToList()
                        .ForEach(i => 
                                i.RefreshEvaluations(GetItemsToEvaluate(i.Id, person), 
                                GetItemsEvaluated(i.Id, person)));

                    items
                        .ForEach(i => i.RefreshPermission(
                                    module, 
                                    person, 
                                    (from s in Manager.GetIQ<UserSubmission>()
                                     where s.Deleted == BaseStatusDeleted.None && s.Call.Id == i.Id
                                     select s.Id).Count()
                                     )
                                );
                }
                catch (Exception ex)
                {
                    items = new List<dtoCallItemPermission>();
                }
                return items;
            }

            private Dictionary<int, ModuleCallForPaper> GetCallCommunitiesPermission(List<int> idCommunities, litePerson person) { 
                Dictionary<int, ModuleCallForPaper> permissions = new Dictionary<int, ModuleCallForPaper>();
                if (idCommunities.Contains(0))
                    permissions[0] = CallForPaperServicePermission(person, 0);

                idCommunities.Where(id => id > 0).Distinct().ToList().ForEach(i => permissions.Add(i, CallForPaperServicePermission(person, i)));

                return permissions;
            }

            public long PublicCallCount(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, int idCommunity, CallStatusForSubmitters status)
            {
                try
                {
                    liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                    litePerson person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                    return CallCountBySubmission(forAdministrationMode,fromAllcommunities, forPortal, community, person, CallForPaperType.CallForBids, status);
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        #endregion
        #region "Evaluation Management"
            private long GetItemsToEvaluate(long idCall, litePerson person)
            {
                return (from s in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                        where s.Call.Id == idCall && s.Deleted == BaseStatusDeleted.None && s.Status != Domain.Evaluation.EvaluationStatus.EvaluatorReplacement && s.Status != Domain.Evaluation.EvaluationStatus.Invalidated && s.Evaluator.Person == person && s.Submission != null
                        select s.Submission.Id).Distinct().Count();
            }
            private long GetItemsEvaluated(long idCall, litePerson person)
            {
                return (from s in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                        where s.Call.Id == idCall && s.Deleted == BaseStatusDeleted.None && s.Status != Domain.Evaluation.EvaluationStatus.EvaluatorReplacement && s.Status != Domain.Evaluation.EvaluationStatus.Invalidated && s.Evaluator.Person == person && s.Submission != null
                        && s.Evaluated == true
                        select s.Submission.Id).Distinct().Count();
            }
        #endregion
        
        #region "Call"
        public dtoCall GetDtoCall(long idCall)
        {
            dtoCall call = null;
            try
            {
                CallForPaper c = Manager.Get<CallForPaper>(idCall);

                call = new dtoCall()
                {
                    AllowSubmissionExtension = c.UseStartCompilationDate,
                    Deleted = c.Deleted,
                    Description = c.Description,
                    Edition = c.Edition,
                    EndDate = c.EndDate,
                    Tags = c.Tags,
                    Id = c.Id,
                    IdDssMethod = c.IdDssMethod,
                    IdDssRatingSet = c.IdDssRatingSet,
                    IsDssMethodFuzzy = c.IsDssMethodFuzzy,
                    UseManualWeights = c.UseManualWeights,
                    OrderedWeights = c.UseOrderedWeights,
                    FuzzyMeWeights = c.FuzzyMeWeights,
                    IsValidFuzzyMeWeights = c.IsValidFuzzyMeWeights,
                    SubmissionClosed = c.SubmissionClosed,
                    IsPortal = c.IsPortal,
                    ForSubscribedUsers = c.ForSubscribedUsers,
                    IsPublic = c.IsPublic,
                    Name = c.Name,
                    RevisionSettings = c.RevisionSettings,
                    AcceptRefusePolicy = c.AcceptRefusePolicy,
                    OverrideHours = c.OverrideHours,
                    OverrideMinutes = c.OverrideMinutes,
                    StartDate = c.StartDate,
                    Status = c.Status,
                    Summary = c.Summary,
                    AwardDate = c.AwardDate,
                    DisplayWinner = c.DisplayWinner,
                    EndEvaluationOn = c.EndEvaluationOn,
                    EvaluationType = c.EvaluationType,
                    Type = c.Type,
                    Community = c.Community,
                    Owner = c.CreatedBy,
                    AdvacedEvaluation = c.AdvacedEvaluation
                };
                
                //call = (from c in Manager.GetAll<CallForPaper>(c => c.Id == idCall)
                //select new dtoCall()
                //{
                //    AllowSubmissionExtension = c.UseStartCompilationDate,
                //    Deleted = c.Deleted,
                //    Description = c.Description,
                //    Edition = c.Edition,
                //    EndDate = c.EndDate,
                //    Tags = c.Tags,
                //    Id = c.Id,
                //    IdDssMethod = c.IdDssMethod,
                //    IdDssRatingSet = c.IdDssRatingSet,
                //    IsDssMethodFuzzy = c.IsDssMethodFuzzy,
                //    UseManualWeights = c.UseManualWeights,
                //    OrderedWeights = c.UseOrderedWeights,
                //    FuzzyMeWeights = c.FuzzyMeWeights,
                //    IsValidFuzzyMeWeights = c.IsValidFuzzyMeWeights,
                //    SubmissionClosed = c.SubmissionClosed,
                //    IsPortal = c.IsPortal,
                //    ForSubscribedUsers = c.ForSubscribedUsers,
                //    IsPublic = c.IsPublic,
                //    Name = c.Name,
                //    RevisionSettings = c.RevisionSettings,
                //    AcceptRefusePolicy = c.AcceptRefusePolicy,
                //    OverrideHours = c.OverrideHours,
                //    OverrideMinutes = c.OverrideMinutes,
                //    StartDate = c.StartDate,
                //    Status = c.Status,
                //    Summary = c.Summary,
                //    AwardDate = c.AwardDate,
                //    DisplayWinner = c.DisplayWinner,
                //    EndEvaluationOn = c.EndEvaluationOn,
                //    EvaluationType = c.EvaluationType,
                //    Type = c.Type,
                //    Community = c.Community,
                //    Owner = c.CreatedBy,
                //    AdvacedEvaluation = c.AdvacedEvaluation
                //        }).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return call;
        }
        public dtoEvaluationSettings GetEvaluationSettings(long idCall, Boolean allowUseOfDss)
        {
            dtoEvaluationSettings settings = null;
            try 
            {
                if (idCall > 0)
                {
                    settings = (from c in Manager.GetIQ<CallForPaper>()
                                where c.Id == idCall
                                select new dtoEvaluationSettings()
                                {
                                    AwardDate = c.AwardDate,
                                    EndEvaluationOn = c.EndEvaluationOn,
                                    EvaluationType = c.EvaluationType,
                                }).FirstOrDefault();
                }
                else
                    settings = new dtoEvaluationSettings() { AwardDate = "", DisplayWinner = false, EndEvaluationOn = null, EvaluationType = EvaluationType.Sum };
            }
            catch(Exception ex){
                settings = new dtoEvaluationSettings() { AwardDate = "", DisplayWinner = false, EndEvaluationOn = null, EvaluationType = EvaluationType.Sum }; 
            }
            Boolean hasCommitees = CallHasCommitees(idCall);
            if (!allowUseOfDss){
                if (settings.EvaluationType== EvaluationType.Dss && hasCommitees){
                    settings.AllowedChangeTo.Remove((EvaluationType.Average));
                    settings.AllowedChangeTo.Remove((EvaluationType.Sum));
                }
                else
                    settings.AllowedChangeTo.Remove((EvaluationType.Dss));
                settings.AllowedTypes.Remove(EvaluationType.Dss);
            }
            else{
                if (settings.EvaluationType== EvaluationType.Dss && hasCommitees){
                    settings.AllowedChangeTo.Remove((EvaluationType.Average));
                    settings.AllowedChangeTo.Remove((EvaluationType.Sum));
                }
                else if (hasCommitees)
                    settings.AllowedChangeTo.Remove((EvaluationType.Dss));
            }
            return settings;
        }
        public Boolean CallHasCommitees(long idCall)
        {
            return (from s in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationCommittee>()
                    where s.Call != null && s.Call.Id == idCall && s.Deleted == BaseStatusDeleted.None
                    select s.Id).Any();
        }

        public CallForPaper SaveCallSettings(dtoCall dto, int idCommunity, Boolean validate)
        {
            CallForPaper callForPaper = null;
            try
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                if (dto != null)
                {
                    Manager.BeginTransaction();
                    callForPaper = Manager.Get<CallForPaper>(dto.Id);
                    if (callForPaper == null)
                    {
                        callForPaper = new CallForPaper();
                        callForPaper.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);

                        callForPaper.Type = CallForPaperType.CallForBids;
                        callForPaper.Status = CallForPaperStatus.Draft;
                        if (idCommunity == 0)
                        {
                            callForPaper.IsPortal = true;
                            callForPaper.IsPublic = true;
                        }
                        else
                            callForPaper.Community = Manager.GetLiteCommunity(idCommunity);
                    }
                    else
                        callForPaper.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    callForPaper.UseStartCompilationDate = dto.AllowSubmissionExtension;
                    callForPaper.AwardDate = dto.AwardDate;
                    callForPaper.EvaluationType = dto.EvaluationType;
                    callForPaper.Description = dto.Description;
                    callForPaper.Summary = dto.Summary;
                    callForPaper.DisplayWinner = dto.DisplayWinner;
                    callForPaper.Edition = dto.Edition;
                    callForPaper.Name = dto.Name;
                    callForPaper.RevisionSettings = dto.RevisionSettings;
                    callForPaper.AcceptRefusePolicy = dto.AcceptRefusePolicy;
                    callForPaper.OverrideHours = dto.OverrideHours;
                    callForPaper.OverrideMinutes = dto.OverrideMinutes;
                    callForPaper.StartDate = dto.StartDate;

                    callForPaper.AttachSign = dto.AttachSign;
                    callForPaper.AllowPrintDraft = dto.AllowDraft;

                    if(this.CommissionAdvanceCanSet(callForPaper))
                    {
                        callForPaper.AdvacedEvaluation = dto.AdvacedEvaluation;
                    }


                    if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
                    {
                        callForPaper.StartDate = dto.EndDate.Value;
                        callForPaper.EndDate = dto.StartDate;
                    }
                    else
                        callForPaper.EndDate = dto.EndDate;
                    if (dto.EndEvaluationOn.HasValue)
                        callForPaper.EndEvaluationOn = dto.EndEvaluationOn.Value;
                    else
                        callForPaper.EndEvaluationOn = null;
                    callForPaper.SubmissionClosed = dto.SubmissionClosed;
                    if (!validate)
                        callForPaper.Status = dto.Status;

                    Manager.SaveOrUpdate(callForPaper);
                    Manager.Commit();
                    //if (validate && ValidateStatus(callForPaper, dto.Status))

                    if (validate && ValidateStatus(callForPaper, dto.Status))
                    {
                        Manager.BeginTransaction();
                        callForPaper.Status = dto.Status;
                        Manager.SaveOrUpdate(callForPaper);

                        if (dto.Id == 0 && callForPaper.Community != null)
                        {
                            BaseForPaperCommunityAssignment assignment = new BaseForPaperCommunityAssignment();
                            assignment.Deleted = BaseStatusDeleted.None;
                            assignment.Deny = false;
                            assignment.AssignedTo = callForPaper.Community;
                            assignment.BaseForPaper = callForPaper;
                            assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            Manager.SaveOrUpdate(assignment);
                        }
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
            catch (SkipRequiredSteps ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return null;
            }
            return callForPaper;
        }
        #endregion

        #region "Evaluations"
            public Boolean isEvaluatorOfCallForPaper(long idCall, int idPerson)
            {
                Boolean iResponse = false;
                try
                {
                    iResponse = (from e in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                                 where e.Call.Id == idCall 
                                    && (
                                        (e.Evaluator != null 
                                            && 
                                            (e.Evaluator.Deleted == BaseStatusDeleted.None 
                                            && e.Evaluator.Person !=null
                                            && e.Evaluator.Person.Id == idPerson))
                                        || (e.AdvEvaluator != null
                                            &&
                                            (e.AdvEvaluator.Deleted == BaseStatusDeleted.None
                                            && e.AdvEvaluator.Member != null
                                            && e.AdvEvaluator.Member.Id == idPerson)
                                        )
                                    )
                                    select e.Id).Any();
                }
                catch (Exception ex) { }

                return iResponse;
            }
            public Boolean isEvaluatorOfSubmission(long idSubmission, int idPerson)
            {
                Boolean iResponse = false;
                try
                {
                    iResponse = (from e in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                                 where e.Submission.Id == idSubmission 
                                 && e.Evaluator != null 
                                 && e.Evaluator.Deleted == BaseStatusDeleted.None && e.Evaluator.Person != null
                                 && e.Evaluator.Person.Id == idPerson
                                 select e.Id).Any();



                    if(!iResponse)
                    {
                        iResponse = (from e in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation>()
                                 where e.Submission.Id == idSubmission
                                 && e.AdvEvaluator != null
                                 && e.AdvEvaluator.Deleted == BaseStatusDeleted.None
                                 && (
                                 (e.AdvEvaluator.Member != null 
                                    && e.AdvEvaluator.Member.Id == idPerson)                                 
                                 ||
                                 (e.AdvCommission != null
                                    && ((e.AdvCommission.President != null 
                                        && e.AdvCommission.President.Id == idPerson)
                                        ||
                                        (e.AdvCommission.Secretary != null
                                        && e.AdvCommission.Secretary.Id == idPerson)
                                        )
                                    )
                                 )
                                 select e.Id).Any();

                    }

                }
                catch (Exception ex) { }

                return iResponse;
            }
        #endregion
        //#############################################################################################
            //#############################################################################################
            //#############################################################################################
            //#############################################################################################

            //#region "Permission"
        //    public ModuleCallForPaper ServicePermission(int personId, int communityId)
        //    {
        //        ModuleCallForPaper module = new ModuleCallForPaper();
        //        litePerson person = Manager.GetLitePerson(personId);
        //        if (communityId == 0)
        //            module = ModuleCallForPaper.CreatePortalmodule(person.TypeID);
        //        else
        //        {
        //            module = new ModuleCallForPaper(Manager.GetModulePermission(personId, communityId, ServiceModuleID()));
        //        }
        //        return module;
        //    }
        //#endregion

        #region "Find CallForPapers for user"
        //public long UserSubmittedCallForPapersCount(int communityId, int userId)
        //{
        //    long count = 0;
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);

        //    count = (from s in Manager.GetIQ<UserSubmission>()
        //             where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person
        //             select s.Id).Count();
        //    return count;
        //}
        //public IList<dtoCallForPaperAvailable> UserSubmittedCallForPapers(int communityId, int userId)
        //{
        //    IList<dtoCallForPaperAvailable> calls = new List<dtoCallForPaperAvailable>();
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);

        //    calls = (from s in Manager.GetIQ<UserSubmission>()
        //             where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person
        //             select new dtoCallForPaperAvailable(s.Call.Id, CallStatusForSubmitters.Submitted,
        //                          new dtoCallForPaper(s.Call.Id, s.Call.Name, s.Call.Edition, s.Call.Description, s.Call.StartDate,
        //                          s.Call.EndDate, ((CallForPaper)s.Call).AwardDate, s.Call.SubmissionClosed, s.Call.Type, ((CallForPaper)s.Call).DisplayWinner,
        //                          s.Call.Community, s.Call.IsPublic, s.Call.Status, ((CallForPaper)s.Call).EvaluationType, s.Call.AllowSubmissionExtension, s.Call.NotificationEmail)
        //                          , new dtoSubmission(s.Id) { Owner = s.Owner, Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate })
        //          ).ToList();

        //    return calls;
        //}
        //// && 
        ////                  (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || s.CallForPaper.IsPublic )
        //public long UserCallForPapersCount(int communityId, int userId, CallStatusForSubmitters status)
        //{
        //    long count = 0;
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);
        //    DateTime currentTime = DateTime.Now;

        //    switch (status)
        //    {
        //        case CallStatusForSubmitters.Submitted:
        //            count = (from s in Manager.GetIQ<UserSubmission>()
        //                     where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person
        //                     select s.Id).Count();
        //            break;
        //        case CallStatusForSubmitters.SubmissionClosed:
        //            List<long> submissionsId = (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Community == community && s.Owner == person select s.Call.Id).ToList();
        //            count = (from c in Manager.GetIQ<CallForPaper>()
        //                     where c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        //                     && !submissionsId.Contains(c.Id) && (c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || (c.EndDate != null && currentTime > c.EndDate))
        //                     select c.Id).Count();
        //            break;
        //        case CallStatusForSubmitters.SubmissionOpened:
        //            List<long> submittedId = (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person select s.Call.Id).ToList();
        //            count = (from c in Manager.GetIQ<CallForPaper>()
        //                     where c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        //                     && !submittedId.Contains(c.Id) && (
        //                        !c.SubmissionClosed &&
        //                        (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
        //                        && (c.EndDate == null || currentTime <= c.EndDate))
        //                     select c.Id).Count();
        //            break;
        //        case CallStatusForSubmitters.ToEvaluate:
        //            count = CallForPaperToEvaluateCount(communityId, userId);
        //            break;
        //        case CallStatusForSubmitters.Evaluated:
        //            count = CallForPaperEvaluatedCount(communityId, userId);
        //            break;
        //        default:
        //            count = 0;
        //            break;

        //    }
        //    //count = (from cfp in Manager.GetIQ<CallForPaper>() 
        //    //        where cfp.Deleted== BaseStatusDeleted.None && cfp.Community == community && cfp.Status != CallForPaperStatus.Draft 
        //    //        &&
        //    //            (
        //    //                (status== UserCallForPaperStatus.SubmissionClosed && (cfp.SubmissionClosed || cfp.Status== CallForPaperStatus.SubmissionClosed || (cfp.EndDate.HasValue && currentTime <= cfp.EndDate.Value)))
        //    //                ||
        //    //                (status== UserCallForPaperStatus.SubmissionOpened && (!cfp.SubmissionClosed || (cfp.Status!= CallForPaperStatus.SubmissionClosed
        //    //            )
        //    //            select cfp.Id).Count();
        //    return count;
        //}
        ////public IList<dtoCallForPaperAvailable> UserCallForPapers(int communityId, int userId, UserCallForPaperStatus status)
        ////{
        ////    IList<dtoCallForPaperAvailable> calls = new List<dtoCallForPaperAvailable>();
        ////    Community community = Manager.GetCommunity(communityId);
        ////    litePerson person = Manager.GetLitePerson(userId);
        ////    DateTime currentTime = DateTime.Now;

        ////    switch (status)
        ////    {
        ////        case UserCallForPaperStatus.Submitted:
        ////            calls = (from s in Manager.GetIQ<UserSubmission>()
        ////                     where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person
        ////                     orderby s.CallForPaper.Name, s.CallForPaper.EndDate
        ////                     select new dtoCallForPaperAvailable(s.CallForPaper.Id, status,
        ////                         new dtoCallForPaper(s.CallForPaper.Id, s.CallForPaper.Name, s.CallForPaper.Edition, s.CallForPaper.Description, s.CallForPaper.StartDate,
        ////                         s.CallForPaper.EndDate, s.CallForPaper.AwardDate, s.CallForPaper.SubmissionClosed, s.CallForPaper.Type, s.CallForPaper.DisplayWinner,
        ////                         s.CallForPaper.Community, s.CallForPaper.IsPublic, s.CallForPaper.Status, s.CallForPaper.EvaluationType, s.CallForPaper.AllowSubmissionExtension, s.CallForPaper.NotificationEmail)
        ////                         , new dtoSubmission(s.Id, s.Owner) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate })
        ////          ).ToList();
        ////            break;
        ////        case UserCallForPaperStatus.SubmissionClosed:
        ////            List<long> submissionsId = (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Community == community && s.Owner == person select s.CallForPaper.Id).ToList();
        ////            calls = (from c in Manager.GetIQ<CallForPaper>()
        ////                     where c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        ////                     && !submissionsId.Contains(c.Id) && (c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || (c.EndDate != null && currentTime > c.EndDate))
        ////                     orderby c.Name, c.EndDate
        ////                     select new dtoCallForPaperAvailable(c.Id, status,
        ////                         new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        ////                         c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        ////                         c.Community, c.IsPublic, c.Status, c.EvaluationType, c.AllowSubmissionExtension, c.NotificationEmail)
        ////                         , new dtoSubmission())
        ////                    ).ToList();
        ////            break;
        ////        case UserCallForPaperStatus.SubmissionOpened:
        ////            List<long> callForPapersId = (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person select s.CallForPaper.Id).ToList();
        ////            calls = (from c in Manager.GetIQ<CallForPaper>()
        ////                     where c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        ////                     && !callForPapersId.Contains(c.Id) && (
        ////                        !c.SubmissionClosed &&
        ////                        (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
        ////                        && (c.EndDate == null || currentTime <= c.EndDate.Value))
        ////                     orderby c.Name, c.EndDate
        ////                     select new dtoCallForPaperAvailable(c.Id, status,
        ////                          new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        ////                          c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        ////                          c.Community, c.IsPublic, c.Status, c.EvaluationType, c.AllowSubmissionExtension, c.NotificationEmail)
        ////                          , (from s in Manager.GetIQ<UserSubmission>()
        ////                             where s.CallForPaper == c && s.Owner == person && s.Deleted != BaseStatusDeleted.None
        ////                             select new dtoSubmission(s.Id, s.Owner) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate }).FirstOrDefault())
        ////                    ).ToList();
        ////            break;
        ////        default:
        ////            break;
        ////    }
        ////    return calls;
        ////}
        //public IList<dtoCallForPaperAvailable> UserCallForPapers(int communityId, int userId, CallStatusForSubmitters status, int currentPageIndex, int currentPageSize)
        //{
        //    IList<dtoCallForPaperAvailable> calls = new List<dtoCallForPaperAvailable>();
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);
        //    DateTime currentTime = DateTime.Now;

        //    switch (status)
        //    {
        //        case CallStatusForSubmitters.Submitted:
        //            calls = (from s in Manager.GetIQ<UserSubmission>()
        //                     where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person
        //                     orderby s.Call.Name, s.Call.EndDate
        //                     select new dtoCallForPaperAvailable(s.Call.Id, status,
        //                         new dtoCallForPaper(s.Call.Id, s.Call.Name, s.Call.Edition, s.Call.Description, s.Call.StartDate,
        //                         s.Call.EndDate, ((CallForPaper)s.Call).AwardDate, s.Call.SubmissionClosed, s.Call.Type, ((CallForPaper)s.Call).DisplayWinner,
        //                         community, s.Call.IsPublic, s.Call.Status, ((CallForPaper)s.Call).EvaluationType, s.Call.AllowSubmissionExtension, s.Call.NotificationEmail)
        //                         , new dtoSubmission(s.Id) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate })
        //          ).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
        //            foreach (dtoCallForPaperAvailable dto in calls.Where(c => c.Submission != null).ToList())
        //            {
        //                dto.Submission.Owner = person;
        //            }
        //            break;
        //        case CallStatusForSubmitters.SubmissionClosed:
        //            List<long> submissionsId = (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Community == community && s.Owner == person select s.Call.Id).ToList();
        //            calls = (from c in Manager.GetIQ<CallForPaper>()
        //                     where c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        //                     && !submissionsId.Contains(c.Id) && (c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || (c.EndDate != null && currentTime > c.EndDate))
        //                     orderby c.Name, c.EndDate
        //                     select new dtoCallForPaperAvailable(c.Id, status, new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        //                         c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        //                         community, c.IsPublic, c.Status, c.EvaluationType, c.AllowSubmissionExtension, c.NotificationEmail))
        //                    ).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
        //            break;
        //        case CallStatusForSubmitters.SubmissionOpened:
        //            List<long> callForPapersId = (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Status >= SubmissionStatus.submitted && s.Community == community && s.Owner == person select s.Call.Id).ToList();
        //            calls = (from c in Manager.GetAll<CallForPaper>(c => c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        //                         && !callForPapersId.Contains(c.Id) && (
        //                            !c.SubmissionClosed &&
        //                            (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
        //                            && (c.EndDate == null || currentTime <= c.EndDate))
        //                            )
        //                     orderby c.Name, c.EndDate
        //                     select new dtoCallForPaperAvailable(c.Id, status,
        //                          new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        //                          c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        //                          c.Community, c.IsPublic, c.Status, c.EvaluationType, c.AllowSubmissionExtension, c.NotificationEmail)
        //                          , (from s in Manager.GetIQ<UserSubmission>()
        //                             where s.Call == c && s.Owner == person && s.Deleted == BaseStatusDeleted.None
        //                             select new dtoSubmission(s.Id) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate }).Skip(0).Take(1).ToList().FirstOrDefault())
        //                    ).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
        //            foreach (dtoCallForPaperAvailable dto in calls.Where(c => c.Submission != null).ToList())
        //            {
        //                dto.Submission.Owner = person;
        //            }
        //            //calls = (from c in Manager.GetAll<CallForPaper>(c => c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        //            //        && !callForPapersId.Contains(c.Id) && (
        //            //            !c.SubmissionClosed &&
        //            //            (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
        //            //            && (c.EndDate == null || currentTime <= c.EndDate))
        //            //            )
        //            //        orderby c.Name, c.EndDate
        //            //        select new dtoCallForPaperAvailable(c.Id, status,
        //            //            new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        //            //            c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        //            //            c.Community, c.IsPublic, c.Status, c.EvaluationType, c.AllowSubmissionExtension, c.NotificationEmail)
        //            //            , new dtoSubmission())
        //            //        ).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
        //            //foreach (dtoCallForPaperAvailable dto in calls) { 
        //            //    dto.Submission=(from s in Manager.GetAll<UserSubmission>()
        //            //                 where s.CallForPaper == c && s.Owner == person && s.Deleted == BaseStatusDeleted.None
        //            //                 select new dtoSubmission(s.Id, person) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate }).Skip(0).Take(1).ToList().FirstOrDefault()
        //            //}
        //            ////calls.fo
        //            //calls = (from c in Manager.GetAll<CallForPaper>(c => c.Deleted == BaseStatusDeleted.None && (person.TypeID != (int)lm.Comol.Core.DomainModel.UserTypeStandard.Guest || c.IsPublic) && c.Community == community
        //            //             && !callForPapersId.Contains(c.Id) && (
        //            //                !c.SubmissionClosed &&
        //            //                (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
        //            //                && (c.EndDate == null || currentTime <= c.EndDate))
        //            //                )
        //            //         orderby c.Name, c.EndDate
        //            //         select new dtoCallForPaperAvailable(c.Id, status,
        //            //              new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        //            //              c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        //            //              c.Community, c.IsPublic, c.Status, c.EvaluationType, c.AllowSubmissionExtension, c.NotificationEmail)
        //            //              , (from s in Manager.GetIQ<UserSubmission>()
        //            //                 where s.CallForPaper == c && s.Owner == person && s.Deleted == BaseStatusDeleted.None
        //            //                 select new dtoSubmission(s.Id, person) { Status = s.Status, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate }).Skip(0).Take(1).ToList().FirstOrDefault())
        //            //        ).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
        //            /**/
        //            break;
        //        default:
        //            break;
        //    }
        //    return calls;
        //}


        #endregion

        #region "Manage Cfp"
        //public Boolean CallForPaperHasDeletedSubmission(long callForPaperId)
        //{
        //    Boolean found = false;
        //    try
        //    {
        //        CallForPaper call = Manager.Get<CallForPaper>(callForPaperId);
        //        if (call != null)
        //        {
        //            found = (from s in Manager.GetIQ<UserSubmission>() where s.Call == call && s.Deleted != BaseStatusDeleted.None select s.Id).Any();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return found;
        //}
        //public long CallForPapersCount(int communityId, int userId, CallStatusForSubmitters status, ModuleCallForPaper module, FilterCallVisibility filter)
        //{
        //    long count = 0;
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);
        //    DateTime currentTime = DateTime.Now;
        //    var query = (from c in Manager.GetIQ<CallForPaper>()
        //                 where c.Community == community && (module.Administration || module.ManageCallForPapers || c.CreatedBy == person)
        //                 && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.Deleted != BaseStatusDeleted.None))
        //                 select c);
        //    if (status == CallStatusForSubmitters.SubmissionOpened)
        //    {
        //        query = query.Where(c => !c.SubmissionClosed && (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published)
        //                        && (c.EndDate == null || currentTime <= c.EndDate));
        //    }
        //    else if (status == CallStatusForSubmitters.Draft)
        //        query = query.Where(c => c.Status == CallForPaperStatus.Draft);
        //    else
        //    {
        //        query = query.Where(c => c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || (c.EndDate != null && currentTime > c.EndDate));
        //    }
        //    count = query.Count();
        //    return count;
        //}
        //public IList<dtoCallForPaperPermission> ManagedCallForPapers(int communityId, int userId, CallStatusForSubmitters status, ModuleCallForPaper module, FilterCallVisibility filter, int currentPageIndex, int currentPageSize)
        //{
        //    IList<dtoCallForPaperPermission> calls = new List<dtoCallForPaperPermission>();
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);
        //    DateTime currentTime = DateTime.Now;
        //    Boolean HasPermission = (module.Administration || module.ManageCallForPapers);
        //    Boolean isOwner = (module.EditCallForPaper || module.CreateCallForPaper);
        //    calls = (from c in Manager.GetIQ<CallForPaper>()
        //             where c.Community == community && (HasPermission || (isOwner && c.CreatedBy == person))
        //             && (filter == FilterCallVisibility.All || (filter == FilterCallVisibility.OnlyVisible && c.Deleted == BaseStatusDeleted.None) || (filter == FilterCallVisibility.OnlyDeleted && c.Deleted != BaseStatusDeleted.None))

        //             && (
        //                (
        //                status == CallStatusForSubmitters.Draft && c.Status == CallForPaperStatus.Draft
        //                ||
        //                status == CallStatusForSubmitters.SubmissionClosed && (c.SubmissionClosed || c.Status == CallForPaperStatus.SubmissionClosed || (c.EndDate != null && currentTime > c.EndDate)))
        //                ||

        //                (status == CallStatusForSubmitters.SubmissionOpened && (!c.SubmissionClosed &&
        //                    (c.Status == CallForPaperStatus.SubmissionOpened || c.Status == CallForPaperStatus.Published || c.Status == CallForPaperStatus.Draft)
        //                    && (c.EndDate == null || currentTime <= c.EndDate)))
        //                )
        //             orderby c.Name, c.EndDate
        //             select new dtoCallForPaperPermission(c.Id, communityId, status,
        //                     new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        //                     c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        //                     c.IsPublic, c.Status, c.EvaluationType, c.Deleted, c.AllowSubmissionExtension, c.NotificationEmail))).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();

        //    calls = (from c in calls select new dtoCallForPaperPermission(c, module, person, (from s in Manager.GetIQ<UserSubmission>() where s.Call.Id == c.Id && s.Deleted == BaseStatusDeleted.None select s.Id).Count())).ToList();
        //    return calls;
        //}

        //public void VirtualDeleteCallForPaper(long itemId, int personId, string IpAddress, string ProxyIpAddress, Boolean delete)
        //{
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        CallForPaper call = Manager.Get<CallForPaper>(itemId);
        //        litePerson person = Manager.GetLitePerson(personId);
        //        if (call != null)
        //        {
        //            call.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
        //            call.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //            Manager.SaveOrUpdate(call);
        //            Manager.Commit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //    }

        //}

        //public long CallForPaperSubmissionsCount(long callForPaperID, int personId, ModuleCallForPaper module, FilterSubmission filter)
        //{
        //    long count = 0;
        //    litePerson person = Manager.GetLitePerson(personId);

        //    try
        //    {
        //        count = (from s in Manager.GetIQ<UserSubmission>()
        //                 where s.Call.Id == callForPaperID &&
        //                 (filter == FilterSubmission.All
        //                 || (filter == FilterSubmission.OnlySubmitted && s.Status >= SubmissionStatus.submitted && s.Deleted == BaseStatusDeleted.None)
        //                 || (filter == FilterSubmission.VirtualDeletedSubmission && s.Deleted == BaseStatusDeleted.Manual)
        //                 || (filter == FilterSubmission.WaitingSubmission && s.Status < SubmissionStatus.submitted && s.Deleted == BaseStatusDeleted.None)
        //                 )
        //                 select s.Id).Count();
        //    }
        //    catch (Exception ex)
        //    {
        //        count = -100;
        //    }
        //    return count;
        //}

        //private IEnumerable<UserSubmission> CallForPaperSubmissionsQuery(long callForPaperID, int personId, ModuleCallForPaper module, FilterSubmission filter, OrderSubmission order, Boolean ascending)
        //{
        //    litePerson person = Manager.GetLitePerson(personId);
        //    Boolean HasPermission = (module.Administration || module.ManageCallForPapers);
        //    Boolean isOwner = (module.EditCallForPaper || module.CreateCallForPaper);
        //    var query = (from sub in Manager.GetAll<UserSubmission>()
        //                 where sub.Call.Id == callForPaperID && (HasPermission || (isOwner && sub.Call.CreatedBy == person))
        //                 && (filter == FilterSubmission.All
        //                     || (filter == FilterSubmission.OnlySubmitted && sub.Status >= SubmissionStatus.submitted && sub.Deleted == BaseStatusDeleted.None)
        //                     || (filter == FilterSubmission.VirtualDeletedSubmission && sub.Deleted == BaseStatusDeleted.Manual)
        //                     || (filter == FilterSubmission.WaitingSubmission && sub.Status < SubmissionStatus.submitted && sub.Deleted == BaseStatusDeleted.None)
        //                     )
        //                 select sub);
        //    switch (order)
        //    {
        //        case OrderSubmission.SubmitterName:
        //            if (ascending)
        //                query = query.OrderBy(s => s.CreatedBy.Surname).ThenBy(s => s.CreatedBy.Name);
        //            else
        //                query = query.OrderByDescending(s => s.CreatedBy.Surname).ThenBy(s => s.CreatedBy.Name);
        //            break;
        //        case OrderSubmission.SubmitterType:
        //            if (ascending)
        //                query = query.OrderBy(s => s.Type.Name);
        //            else
        //                query = query.OrderByDescending(s => s.Type.Name);
        //            break;
        //        case OrderSubmission.SubmissionDate:
        //            if (ascending)
        //                query = query.OrderBy(s => s.SubmittedOn);
        //            else
        //                query = query.OrderByDescending(s => s.SubmittedOn);
        //            break;
        //        case OrderSubmission.ModifiedOn:
        //            if (ascending)
        //                query = query.OrderBy(s => s.ModifiedBy);
        //            else
        //                query = query.OrderByDescending(s => s.ModifiedBy);
        //            break;
        //        case OrderSubmission.CreatedOn:
        //            if (ascending)
        //                query = query.OrderBy(s => s.CreatedOn);
        //            else
        //                query = query.OrderByDescending(s => s.CreatedOn);
        //            break;
        //    }
        //    return query;
        //}

        //public IList<dtoSubmissionPermission> CallForPaperSubmissions(long callForPaperID, int personId, ModuleCallForPaper module, FilterSubmission filter, OrderSubmission order, Boolean ascending, int currentPageIndex, int currentPageSize)
        //{
        //    IList<dtoSubmissionPermission> submissions = new List<dtoSubmissionPermission>();
        //    litePerson person = Manager.GetLitePerson(personId);
        //    Boolean HasPermission = (module.Administration || module.ManageCallForPapers);
        //    Boolean isOwner = (module.EditCallForPaper || module.CreateCallForPaper);
        //    var query = (from sub in Manager.GetAll<UserSubmission>()
        //                 where sub.Call.Id == callForPaperID && (HasPermission || (isOwner && sub.Call.CreatedBy == person))
        //                 && (filter == FilterSubmission.All
        //                     || (filter == FilterSubmission.OnlySubmitted && sub.Status >= SubmissionStatus.submitted && sub.Deleted == BaseStatusDeleted.None)
        //                     || (filter == FilterSubmission.VirtualDeletedSubmission && sub.Deleted == BaseStatusDeleted.Manual)
        //                     || (filter == FilterSubmission.WaitingSubmission && sub.Status < SubmissionStatus.submitted && sub.Deleted == BaseStatusDeleted.None)
        //                     )
        //                 select sub);
        //    switch (order)
        //    {
        //        case OrderSubmission.SubmitterName:
        //            if (ascending)
        //                query = query.OrderBy(s => s.CreatedBy.Surname).ThenBy(s => s.CreatedBy.Name);
        //            else
        //                query = query.OrderByDescending(s => s.CreatedBy.Surname).ThenBy(s => s.CreatedBy.Name);
        //            break;
        //        case OrderSubmission.SubmitterType:
        //            if (ascending)
        //                query = query.OrderBy(s => s.Type.Name);
        //            else
        //                query = query.OrderByDescending(s => s.Type.Name);
        //            break;
        //        case OrderSubmission.SubmissionDate:
        //            if (ascending)
        //                query = query.OrderBy(s => s.SubmittedOn);
        //            else
        //                query = query.OrderByDescending(s => s.SubmittedOn);
        //            break;
        //        case OrderSubmission.ModifiedOn:
        //            if (ascending)
        //                query = query.OrderBy(s => s.ModifiedBy);
        //            else
        //                query = query.OrderByDescending(s => s.ModifiedBy);
        //            break;
        //        case OrderSubmission.CreatedOn:
        //            if (ascending)
        //                query = query.OrderBy(s => s.CreatedOn);
        //            else
        //                query = query.OrderByDescending(s => s.CreatedOn);
        //            break;
        //    }

        //    //
        //    submissions = query.Select(sub => new dtoSubmissionPermission(sub.Id, callForPaperID,
        //                   new dtoSubmission()
        //                   {
        //                       Id = sub.Id,
        //                       Deleted = sub.Deleted,
        //                       CallForPaperId = callForPaperID,
        //                       CreatedOn = sub.CreatedOn,
        //                       ModifiedOn = sub.ModifiedOn,
        //                       ModifiedBy = sub.ModifiedBy,
        //                       Owner = sub.Owner,
        //                       PersonId = sub.Owner.Id,
        //                       Status = sub.Status,
        //                       SubmittedOn = sub.SubmittedOn,
        //                       SubmittedBy = sub.SubmittedBy,
        //                       ExtensionDate = sub.ExtensionDate,
        //                       Type = new dtoSubmitterType(sub.Type),
        //                       //LinkZip = sub.LinkZip
        //                   })).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
        //    //submissions = (from sub in query
        //    //         select new dtoSubmissionPermission(sub.Id,
        //    //             new dtoSubmission()
        //    //             {
        //    //                 Id = sub.Id,
        //    //                 Deleted = sub.Deleted,
        //    //                 CallForPaperId = callForPaperID,
        //    //                 CreatedOn = sub.CreatedOn,
        //    //                 ModifiedOn = sub.ModifiedOn,
        //    //                 Person = sub.Owner,
        //    //                 PersonId = sub.Owner.Id,
        //    //                 Status = sub.Status,
        //    //                 SubmittedOn = sub.SubmittedOn,
        //    //                 Type = new dtoSubmitterType(sub.Type)
        //    //             })).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
        //    submissions = (from c in submissions select new dtoSubmissionPermission(c, module, person)).ToList();
        //    return submissions;
        //}


        ////public String ExportSubmissionsList(Dictionary<SubmissionListTranslations, string> translations, int IdCommunity, long IdCallForPaper, int personId, ModuleCallForPaper module, FilterSubmission filter, OrderSubmission order, Boolean ascending)
        ////{

        ////    try
        ////    {
        ////        var query = CallForPaperSubmissionsQuery(IdCallForPaper, personId, module, filter, order, ascending);

        ////        IList<dtoSubmission> submissions = query.Select(sub => new dtoSubmission()
        ////        {
        ////            Id = sub.Id,
        ////            Deleted = sub.Deleted,
        ////            CallForPaperId = IdCallForPaper,
        ////            CreatedOn = sub.CreatedOn,
        ////            ModifiedOn = sub.ModifiedOn,
        ////            ModifiedBy = sub.ModifiedBy,
        ////            Owner = sub.Owner,
        ////            PersonId = sub.Owner.Id,
        ////            Status = sub.Status,
        ////            SubmittedOn = sub.SubmittedOn,
        ////            SubmittedBy = sub.SubmittedBy,
        ////            ExtensionDate = sub.ExtensionDate,
        ////            Type = new dtoSubmitterType(sub.Type),
        ////        }).ToList();
        ////        List<int> usersId = (from s in submissions select s.PersonId).ToList();
        ////        IList<LazySubscription> subscriptionInfo = (from s in Manager.Linq<LazySubscription>() where usersId.Contains(s.IdPerson) && s.IdCommunity == IdCommunity select s).ToList();

        ////        litePerson person = Manager.Get<Person>(UC.CurrentUserID);

        ////        var queryPaper = (from c in Manager.GetIQ<CallForPaper>() where c.Id == IdCallForPaper select new { Name = c.Name, Edition = c.Edition }).Skip(0).Take(1).ToList().FirstOrDefault();
        ////        if (queryPaper == null)
        ////            return OLDExportHelpers.CreateErrorExcelDocument();
        ////        else
        ////            return OLDExportHelpers.ExportSubmissionsList(queryPaper.Name, queryPaper.Edition, submissions, subscriptionInfo, person, filter, translations);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new ExportError("", ex) { ExcelDocument = OLDExportHelpers.CreateErrorExcelDocument() };
        ////    }

        ////    // return ""; // (advancedStatistics ? ExportAdvancedEvaluations(translations, IdCallForPaper, IdsubmissionType, order, ascending)
        ////    //    ExportStandardEvaluations(translations, IdCallForPaper, IdsubmissionType, order, ascending));

        ////}

        #endregion

        public Boolean CallIsPublic(long idCall)
        {
            CallForPaper call = Manager.Get<CallForPaper>(idCall);
            return (call != null) && call.IsPublic;
        }

        #region "Evaluation"

        #region "Criteria"
        ////public dtoCriterion GetDtoCriterion(long IdCriterion)
        ////{
        ////    dtoCriterion criterion = null;
        ////    CallForPaperCriterion callCriterion = Manager.Get<CallForPaperCriterion>(IdCriterion);
        ////    if (callCriterion == null)
        ////        throw new UnknownItem();
        ////    else
        ////    {
        ////        criterion = new dtoCriterion()
        ////        {
        ////            Id = callCriterion.Id,
        ////            Name = callCriterion.Name,
        ////            Deleted = callCriterion.Deleted,
        ////            Description = callCriterion.Description,
        ////            MaxValue = callCriterion.MaxValue,
        ////            MinValue = callCriterion.MinValue,
        ////            SubmissionCount = GetSubmissionCountForCriterion(callCriterion)
        ////        };
        ////    }
        ////    return criterion;
        ////}
        ////public CallForPaperCriterion SaveCriterion(dtoCriterion dtoCriterion, long IdCallForPaper)
        ////{
        ////    CallForPaperCriterion criterion = null;
        ////    try
        ////    {
        ////        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        ////        CallForPaper call = GetCallForPaper(IdCallForPaper);
        ////        if (call == null)
        ////            throw new UnknownCallForPaper();
        ////        else
        ////        {
        ////            Manager.BeginTransaction();
        ////            if (dtoCriterion.Id == 0)
        ////            {
        ////                criterion = new CallForPaperCriterion();
        ////                criterion.Call = call;
        ////                criterion.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        ////            }
        ////            else
        ////                criterion = Manager.Get<CallForPaperCriterion>(dtoCriterion.Id);
        ////            if (dtoCriterion.Id > 0 && criterion == null)
        ////                throw new UnknownItem();
        ////            else
        ////            {
        ////                if (dtoCriterion.Id != 0)
        ////                    criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        ////                criterion.Name = dtoCriterion.Name;
        ////                criterion.Description = dtoCriterion.Description;
        ////                criterion.MaxValue = dtoCriterion.MaxValue;
        ////                criterion.MinValue = dtoCriterion.MinValue;
        ////                Manager.SaveOrUpdate(criterion);
        ////            }
        ////            Manager.Commit();
        ////        }
        ////    }
        ////    catch (UnknownCallForPaper ex)
        ////    {
        ////        Manager.RollBack();
        ////        throw ex;
        ////    }
        ////    catch (UnknownItem ex)
        ////    {
        ////        Manager.RollBack();
        ////        throw ex;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Manager.RollBack();
        ////        throw new CallForPaperItemSave(ex.Message, ex) { Name = dtoCriterion.Name };
        ////    }
        ////    return criterion;
        ////}
        ////public void VirtualDeleteCriterion(long IdCriterion, Boolean delete)
        ////{
        ////    CallForPaperCriterion criterion = Manager.Get<CallForPaperCriterion>(IdCriterion);
        ////    try
        ////    {
        ////        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        ////        if (criterion != null && criterion.Id > 0)
        ////        {
        ////            long count = GetSubmissionCountForCriterion(criterion);
        ////            if (count > 0)
        ////                throw new ItemDeleteWithSubmission() { Name = criterion.Name, SubmissionCount = count };
        ////            else
        ////            {
        ////                Manager.BeginTransaction();
        ////                criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        ////                criterion.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
        ////                Manager.SaveOrUpdate(criterion);
        ////                Manager.Commit();
        ////            }
        ////        }
        ////        else
        ////            throw new UnknownItem();
        ////    }
        ////    catch (ItemDeleteWithSubmission ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    catch (UnknownItem ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Manager.RollBack();
        ////        throw new CallForPaperItemUndelete(ex.Message, ex) { Name = (criterion == null) ? "" : criterion.Name };
        ////    }
        ////}
        ////public void PhisicalDeleteCriterion(long IdCriterion)
        ////{
        ////    CallForPaperCriterion criterion = Manager.Get<CallForPaperCriterion>(IdCriterion);
        ////    try
        ////    {
        ////        if (criterion != null && criterion.Id > 0)
        ////        {
        ////            long count = GetSubmissionCountForCriterion(criterion);
        ////            if (count > 0)
        ////                throw new ItemDeleteWithSubmission() { Name = criterion.Name, SubmissionCount = count };
        ////            else
        ////            {
        ////                Manager.BeginTransaction();
        ////                Manager.DeletePhysical(criterion);
        ////                Manager.Commit();
        ////            }
        ////        }
        ////        else
        ////            throw new UnknownItem();
        ////    }
        ////    catch (ItemDeleteWithSubmission ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Manager.RollBack();
        ////        throw new CallForPaperItemDelete(ex.Message, ex) { Name = (criterion == null) ? "" : criterion.Name };
        ////    }
        ////}
        ////public Boolean AllowManageCriteria(CallForPaper call)
        ////{
        ////    return !(from s in Manager.GetIQ<SubmissionEvaluation>() where s.Evaluated == true && s.Call == call select s.Id).Any();
        ////}
        ////public List<dtoCriterionPermission> GetCriteria(long IdCallForPaper, Boolean AllCriteria, Boolean allowEdit)
        ////{
        ////    CallForPaper call = GetCallForPaper(IdCallForPaper);
        ////    if (call == null)
        ////        throw new UnknownCallForPaper();
        ////    else
        ////        return GetCriteria(call, AllCriteria, allowEdit);
        ////}
        ////public List<dtoCriterionPermission> GetCriteria(CallForPaper call, Boolean AllCriteria, Boolean allowEdit)
        ////{
        ////    List<dtoCriterionPermission> list = new List<dtoCriterionPermission>();
        ////    try
        ////    {
        ////        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        ////        if (call == null || person == null)
        ////            throw new UnknownCallForPaper();
        ////        else
        ////        {
        ////            list = (from c in Manager.GetAll<CallForPaperCriterion>(c => c.Call == call && (AllCriteria || (c.Deleted == BaseStatusDeleted.None || c.CreatedBy == person)))
        ////                    orderby c.Name
        ////                    select CreateCriterionPermission(c)).ToList();

        ////        }
        ////    }
        ////    catch (UnknownCallForPaper ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    return list;
        ////}
        //public List<dtoCriterion> GetCriteriaForStatistics(long IdCallForPaper, Boolean AllCriteria)
        //{
        //    CallForPaper call = GetCallForPaper(IdCallForPaper);
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //        return GetCriteriaForStatistics(call, AllCriteria);
        //}
        //public List<dtoCriterion> GetCriteriaForStatistics(CallForPaper call, Boolean AllCriteria)
        //{
        //    List<dtoCriterion> list = new List<dtoCriterion>();
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        //        if (call == null || person == null)
        //            throw new UnknownCallForPaper();
        //        else
        //        {
        //            list = (from c in Manager.GetIQ<CallForPaperCriterion>()
        //                    where c.Call == call && (AllCriteria || c.Deleted == BaseStatusDeleted.None)
        //                    orderby c.Name
        //                    select new dtoCriterion() { Id = c.Id, Name = c.Name, Description = c.Description }).ToList();

        //        }
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return list;
        //}
        ////private dtoCriterionPermission CreateCriterionPermission(CallForPaperCriterion criterion)
        ////{
        ////    dtoCriterionPermission dto = new dtoCriterionPermission();
        ////    dto.Item = new dtoCriterion()
        ////    {
        ////        Id = criterion.Id,
        ////        Name = criterion.Name,
        ////        Deleted = criterion.Deleted,
        ////        Description = criterion.Description,
        ////        MaxValue = criterion.MaxValue,
        ////        MinValue = criterion.MinValue,
        ////        SubmissionCount = GetSubmissionCountForCriterion(criterion)
        ////    };
        ////    Boolean HasSubmission = (dto.Item.SubmissionCount > 0);
        ////    dto.Permission = new CoreItemPermission();
        ////    dto.Permission.AllowDelete = !HasSubmission && criterion.Deleted != BaseStatusDeleted.None;
        ////    dto.Permission.AllowEdit = !HasSubmission && criterion.Deleted == BaseStatusDeleted.None;
        ////    dto.Permission.AllowUnDelete = criterion.Deleted != BaseStatusDeleted.None;
        ////    dto.Permission.AllowVirtualDelete = !HasSubmission && criterion.Deleted == BaseStatusDeleted.None;
        ////    return dto;
        ////}
        ////private long GetSubmissionCountForCriterion(CallForPaperCriterion criterion)
        ////{
        ////    return (from s in Manager.GetIQ<EvaluatedCriterion>()
        ////            where s.Call == criterion.Call && s.Deleted == BaseStatusDeleted.None && s.Criterion == criterion && s.Submission != null
        ////            select s.Submission.Id).Distinct().Count();
        ////}
        #endregion

        #region "Evaluators"
        //public dtoEvaluator GetDtoEvaluator(long IdEvaluator)
        //{
        //    dtoEvaluator evaluator = null;
        //    CallForPaperEvaluator callEvaluator = Manager.Get<CallForPaperEvaluator>(IdEvaluator);
        //    if (callEvaluator == null)
        //        throw new UnknownItem();
        //    else
        //    {
        //        evaluator = new dtoEvaluator()
        //        {
        //            Id = callEvaluator.Id,
        //            Deleted = callEvaluator.Deleted,
        //            EvaluatedCount = GetEvaluatedSubmissionForEvaluator(callEvaluator),
        //            SubmissionCount = GetAssignedSubmissionForEvaluator(callEvaluator)
        //        };
        //        if (callEvaluator.Evaluator == null)
        //        {
        //            evaluator.Deleted = BaseStatusDeleted.Manual;
        //            evaluator.DisplayName = "--";
        //            evaluator.Name = "-";
        //            evaluator.Surname = "-";
        //        }
        //        else
        //        {
        //            evaluator.DisplayName = callEvaluator.Evaluator.SurnameAndName;
        //            evaluator.Name = callEvaluator.Evaluator.Name;
        //            evaluator.Surname = callEvaluator.Evaluator.Surname;
        //        }
        //    }
        //    return evaluator;
        //}
        //public CallForPaperEvaluator AddEvaluator(int IdPerson, long IdCallForPaper)
        //{
        //    List<int> IdPersons = new List<int>();
        //    return AddEvaluators(IdPersons, IdCallForPaper).FirstOrDefault();

        //}
        //public List<CallForPaperEvaluator> AddEvaluators(List<int> IdPersons, long IdCallForPaper)
        //{
        //    List<CallForPaperEvaluator> evaluators = new List<CallForPaperEvaluator>();
        //    try
        //    {
        //        CallForPaper call = GetCallForPaper(IdCallForPaper);
        //        Person creator = Manager.GetLitePerson(UC.CurrentUserID);
        //        if (call == null)
        //            throw new UnknownCallForPaper();
        //        else if (creator != null)
        //        {
        //            Manager.BeginTransaction();
        //            List<int> evaluatorsAdded = (from e in Manager.GetIQ<CallForPaperEvaluator>()
        //                                         where e.Call == call && e.Evaluator != null && IdPersons.Contains(e.Evaluator.Id)
        //                                         select e.Evaluator.Id).ToList();

        //            foreach (int IdPerson in IdPersons.Except(evaluatorsAdded))
        //            {
        //                Person evaluator = Manager.GetLitePerson(IdPerson);
        //                CallForPaperEvaluator callEvaluator = new CallForPaperEvaluator();
        //                callEvaluator.Call = call;
        //                callEvaluator.CreateMetaInfo(creator, UC.IpAddress, UC.ProxyIpAddress);
        //                callEvaluator.Evaluator = evaluator;
        //                if (evaluator != null)
        //                    evaluators.Add(callEvaluator);
        //            }

        //            Manager.SaveOrUpdateList(evaluators);
        //            Manager.Commit();
        //        }
        //        else
        //            throw new CallForPaperItemSave();
        //    }
        //    catch (CallForPaperItemSave ex)
        //    {
        //        throw ex;
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        Manager.RollBack();
        //        evaluators.Clear();
        //        throw ex;
        //    }
        //    catch (UnknownItem ex)
        //    {
        //        Manager.RollBack();
        //        evaluators.Clear();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        evaluators.Clear();
        //        throw new CallForPaperItemSave(ex.Message, ex);
        //    }
        //    return evaluators;
        //}
        //public void VirtualDeleteEvaluator(long IdEvaluator, Boolean delete)
        //{
        //    CallForPaperEvaluator callEvaluator = Manager.Get<CallForPaperEvaluator>(IdEvaluator);
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        //        if (callEvaluator != null && callEvaluator.Id > 0)
        //        {
        //            long evaluatedCount = GetEvaluatedSubmissionForEvaluator(callEvaluator);
        //            long count = GetAssignedSubmissionForEvaluator(callEvaluator);
        //            if (count > 0 && evaluatedCount > 0 && delete)
        //                throw new ItemDeleteWithSubmission() { Name = callEvaluator.Evaluator.SurnameAndName, SubmissionCount = count };
        //            else
        //            {
        //                Manager.BeginTransaction();
        //                callEvaluator.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //                callEvaluator.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
        //                Manager.SaveOrUpdate(callEvaluator);
        //                Manager.Commit();
        //            }
        //        }
        //        else
        //            throw new UnknownItem();
        //    }
        //    catch (ItemDeleteWithSubmission ex)
        //    {
        //        throw ex;
        //    }
        //    catch (UnknownItem ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        throw new CallForPaperItemUndelete(ex.Message, ex) { Name = (callEvaluator == null) ? "" : (callEvaluator.Evaluator == null) ? "" : callEvaluator.Evaluator.SurnameAndName };
        //    }
        //}
        //public void PhisicalDeleteEvaluator(long IdEvaluator)
        //{
        //    CallForPaperEvaluator callEvaluator = Manager.Get<CallForPaperEvaluator>(IdEvaluator);
        //    try
        //    {
        //        if (callEvaluator != null && callEvaluator.Id > 0)
        //        {
        //            long evaluatedCount = GetEvaluatedSubmissionForEvaluator(callEvaluator);
        //            long count = GetAssignedSubmissionForEvaluator(callEvaluator);
        //            if (count > 0 && evaluatedCount > 0)
        //                throw new ItemDeleteWithSubmission() { Name = callEvaluator.Evaluator.SurnameAndName, SubmissionCount = count };
        //            else
        //            {
        //                Manager.BeginTransaction();
        //                Manager.DeletePhysical(callEvaluator);
        //                Manager.Commit();
        //            }
        //        }
        //        else
        //            throw new UnknownItem();
        //    }
        //    catch (UnknownItem ex)
        //    {
        //        throw ex;
        //    }
        //    catch (ItemDeleteWithSubmission ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        throw new CallForPaperItemUndelete(ex.Message, ex) { Name = (callEvaluator == null) ? "" : (callEvaluator.Evaluator == null) ? "" : callEvaluator.Evaluator.SurnameAndName };
        //    }
        //}

        //public CallForPaperEvaluator GetActiveEvaluator(CallForPaper call, litePerson person)
        //{
        //    List<CallForPaperEvaluator> list = (from c in Manager.GetAll<CallForPaperEvaluator>(c => c.Call == call && c.Deleted == BaseStatusDeleted.None && c.Evaluator == person) select c).ToList();
        //    return list.FirstOrDefault();
        //}
        //public List<dtoEvaluator> GetEvaluatorsForStatistics(long IdCallForPaper)
        //{
        //    CallForPaper call = GetCallForPaper(IdCallForPaper);
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //        return GetEvaluatorsForStatistics(call);
        //}
        //public List<dtoEvaluator> GetEvaluatorsForStatistics(CallForPaper call)
        //{
        //    List<dtoEvaluator> list = new List<dtoEvaluator>();
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        //        if (call == null || person == null)
        //            throw new UnknownCallForPaper();
        //        else
        //        {
        //            list = (from c in Manager.GetAll<CallForPaperEvaluator>(c => c.Call == call && c.Deleted == BaseStatusDeleted.None)
        //                    select new dtoEvaluator() { Id = c.Id, DisplayName = c.Evaluator.SurnameAndName }).OrderBy(c => c.DisplayName).ToList();
        //        }
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        Manager.RollBack();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //    }
        //    return list;
        //}
        //public List<dtoEvaluatorPermission> GetEvaluators(long IdCallForPaper, Boolean AllEvaluators, Boolean allowEdit)
        //{
        //    CallForPaper call = GetCallForPaper(IdCallForPaper);
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //        return GetEvaluators(call, AllEvaluators, allowEdit);
        //}
        //public List<dtoEvaluatorPermission> GetEvaluators(CallForPaper call, Boolean AllEvaluators, Boolean allowEdit)
        //{
        //    List<dtoEvaluatorPermission> list = new List<dtoEvaluatorPermission>();
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        //        if (call == null || person == null)
        //            throw new UnknownCallForPaper();
        //        else
        //        {
        //            list = (from c in Manager.GetAll<CallForPaperEvaluator>(c => c.Call == call && (AllEvaluators || (c.Deleted == BaseStatusDeleted.None || c.CreatedBy == person)))
        //                    select CreateEvaluatorPermission(c)).ToList();

        //        }
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        Manager.RollBack();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //    }
        //    return list;
        //}
        //public List<int> GetIdEvaluators(CallForPaper call, Boolean AllEvaluators)
        //{
        //    List<int> list = new List<int>();
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        //        list = (from c in Manager.GetIQ<CallForPaperEvaluator>()
        //                where c.Call == call && (AllEvaluators || (c.Deleted == BaseStatusDeleted.None || c.CreatedBy == person))
        //                && c.Evaluator != null
        //                select c.Evaluator.Id).ToList();
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        Manager.RollBack();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //    }
        //    return list;
        //}
        //private dtoEvaluatorPermission CreateEvaluatorPermission(CallForPaperEvaluator evaluator)
        //{
        //    dtoEvaluatorPermission dto = new dtoEvaluatorPermission();
        //    dto.Item = new dtoEvaluator()
        //    {
        //        Id = evaluator.Id,
        //        Deleted = (evaluator.Evaluator == null) ? BaseStatusDeleted.Manual : evaluator.Deleted,
        //        EvaluatedCount = GetEvaluatedSubmissionForEvaluator(evaluator),
        //        SubmissionCount = GetAssignedSubmissionForEvaluator(evaluator),
        //        Name = (evaluator.Evaluator == null) ? "-" : evaluator.Evaluator.Name,
        //        DisplayName = (evaluator.Evaluator == null) ? "-" : evaluator.Evaluator.SurnameAndName,
        //        Surname = (evaluator.Evaluator == null) ? "-" : evaluator.Evaluator.Surname
        //    };
        //    Boolean HasEvaluation = (dto.Item.EvaluatedCount > 0);
        //    dto.Permission = new CoreItemPermission();
        //    dto.Permission.AllowDelete = !HasEvaluation && evaluator.Deleted != BaseStatusDeleted.None;
        //    dto.Permission.AllowEdit = true; //!HasEvaluation && evaluator.Deleted == BaseStatusDeleted.None;
        //    dto.Permission.AllowUnDelete = !HasEvaluation && evaluator.Deleted != BaseStatusDeleted.None;
        //    dto.Permission.AllowVirtualDelete = !HasEvaluation && evaluator.Deleted == BaseStatusDeleted.None;
        //    return dto;
        //}

        //public void AssignSubmissionsToEvaluator(List<long> assignedID, CallForPaper call, CallForPaperEvaluator evaluator)
        //{
        //    List<SubmissionEvaluation> evaluations = (from se in Manager.GetIQ<SubmissionEvaluation>()
        //                                              where se.Call == call && se.Evaluator == evaluator && assignedID.Contains(se.Submission.Id)
        //                                              select se).ToList();
        //    if (call != null && evaluator != null)
        //    {
        //        try
        //        {
        //            Manager.BeginTransaction();
        //            litePerson user = Manager.Get<Person>(UC.CurrentUserID);
        //            foreach (SubmissionEvaluation deleted in (from e in evaluations where e.Deleted != BaseStatusDeleted.None select e).ToList())
        //            {
        //                deleted.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
        //                deleted.Deleted = BaseStatusDeleted.None;
        //                Manager.SaveOrUpdate(deleted);
        //            }
        //            List<SubmissionEvaluation> toDelete = (from se in Manager.GetIQ<SubmissionEvaluation>()
        //                                                   where se.Call == call && se.Evaluator == evaluator && !assignedID.Contains(se.Submission.Id)
        //                                                   && se.Deleted == BaseStatusDeleted.None
        //                                                   select se).ToList();
        //            foreach (SubmissionEvaluation delete in toDelete)
        //            {
        //                delete.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
        //                delete.Deleted = BaseStatusDeleted.Manual;
        //                Manager.SaveOrUpdate(delete);
        //            }
        //            assignedID = assignedID.Except((from e in evaluations select e.Submission.Id).ToList()).ToList();
        //            foreach (long IdSubmission in assignedID)
        //            {
        //                SubmissionEvaluation evaluation = new SubmissionEvaluation();
        //                evaluation.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
        //                evaluation.Call = call;
        //                evaluation.Community = call.Community;
        //                evaluation.Evaluator = evaluator;
        //                evaluation.Submission = Manager.Get<UserSubmission>(IdSubmission);
        //                Manager.SaveOrUpdate(evaluation);
        //            }
        //            Manager.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            Manager.RollBack();
        //        }
        //    }
        //}




        //public List<dtoSubmissionToAssign> GetAvailableSubmissionToEvaluate(long IdCallForPaper, long submissionTypeId)
        //{
        //    if (IdCallForPaper == 0)
        //        throw new UnknownCallForPaper();
        //    else
        //    {
        //        return GetAvailableSubmissionToEvaluate(Manager.Get<CallForPaper>(IdCallForPaper), submissionTypeId);
        //    }
        //}
        //public List<dtoSubmissionToAssign> GetAvailableSubmissionToEvaluate(CallForPaper call, long submissionTypeId)
        //{
        //    List<dtoSubmissionToAssign> list = new List<dtoSubmissionToAssign>();
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //    {
        //        return (from s in Manager.GetAll<LazyUserSubmission>(s => s.Call == call && s.Deleted == BaseStatusDeleted.None && s.Owner != null &&
        //                !(s.Status < SubmissionStatus.accepted
        //                || s.Status == SubmissionStatus.rejected) && (submissionTypeId < 1 || s.Type.Id == submissionTypeId))
        //                select new dtoSubmissionToAssign() { Deleted = s.Deleted, Id = s.Id, Owner = s.Owner.SurnameAndName, SubmittedOn = s.SubmittedOn, Type = s.Type, SubmissionId = s.Id }).ToList();
        //    }
        //}
        //public List<long> GetAvailableSubmissionIdToEvaluate(CallForPaper call, long submissionTypeId)
        //{
        //    List<long> list = new List<long>();
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //    {
        //        return (from s in Manager.GetAll<LazyUserSubmission>(s => s.Call == call && s.Deleted == BaseStatusDeleted.None && s.Owner != null &&
        //                !(s.Status < SubmissionStatus.accepted
        //                || s.Status == SubmissionStatus.rejected) && (submissionTypeId < 1 || s.Type.Id == submissionTypeId))
        //                select s.Id).ToList();
        //    }
        //}
        //public List<dtoSubmissionToEvaluatePermission> GetSubmissionToEvaluatePermission(List<dtoSubmissionToAssign> submissions, CallForPaperEvaluator evaluator)
        //{
        //    List<long> uneditableId = (from s in Manager.GetIQ<SubmissionEvaluation>()
        //                               where s.Call == evaluator.Call && s.Deleted == BaseStatusDeleted.None && s.Evaluator == evaluator && s.Submission != null
        //                               && (s.Evaluated == true || s.EvaluationStartedOn != null)
        //                               select s.Submission.Id).ToList();

        //    List<dtoSubmissionToEvaluatePermission> list = (from s in submissions
        //                                                    orderby s.Owner
        //                                                    select new dtoSubmissionToEvaluatePermission()
        //                                                    {
        //                                                        Item = s,
        //                                                        Permission = new CoreItemPermission()
        //                                                        {
        //                                                            AllowEdit = !uneditableId.Contains(s.SubmissionId),
        //                                                        }
        //                                                    }
        //                                                    ).ToList();
        //    return list;

        //}
        //public List<long> GetAssignedSubmissionIdToEvaluate(CallForPaperEvaluator evaluator)
        //{
        //    if (evaluator == null)
        //        throw new UnknownItem();
        //    else
        //    {
        //        return (from s in Manager.GetIQ<SubmissionEvaluation>()
        //                where s.Call == evaluator.Call && s.Deleted == BaseStatusDeleted.None && s.Evaluator == evaluator && s.Submission != null
        //                select s.Submission.Id).ToList();
        //    }
        //}
        //public long GetAssignedSubmissionForEvaluator(CallForPaperEvaluator evaluator)
        //{
        //    return (from s in Manager.GetIQ<SubmissionEvaluation>()
        //            where s.Call == evaluator.Call && s.Deleted == BaseStatusDeleted.None && s.Evaluator == evaluator && s.Submission != null
        //            select s.Submission.Id).Distinct().Count();
        //}
        //private long GetEvaluatedSubmissionForEvaluator(CallForPaperEvaluator evaluator)
        //{
        //    return (from s in Manager.GetIQ<SubmissionEvaluation>()
        //            where s.Call == evaluator.Call && s.Deleted == BaseStatusDeleted.None && s.Evaluator == evaluator && s.Submission != null
        //            && s.Evaluated == true
        //            select s.Submission.Id).Distinct().Count();
        //}
        //private long GetAssignedSubmissionForUser(long IdCallForPaper, litePerson person)
        //{
        //    return (from s in Manager.GetIQ<SubmissionEvaluation>()
        //            where s.Call.Id == IdCallForPaper && s.Deleted == BaseStatusDeleted.None && s.Evaluator.Evaluator == person && s.Submission != null
        //            select s.Submission.Id).Distinct().Count();
        //}
        //private long GetEvaluatedSubmissionForUser(long IdCallForPaper, litePerson person)
        //{
        //    return (from s in Manager.GetIQ<SubmissionEvaluation>()
        //            where s.Call.Id == IdCallForPaper && s.Deleted == BaseStatusDeleted.None && s.Evaluator.Evaluator == person && s.Submission != null
        //            && s.Evaluated == true
        //            select s.Submission.Id).Distinct().Count();
        //}
        //#endregion

        //#region "evaluations"
        //public List<long> GetAssignedEvaluatorsId(long IdSubmission)
        //{
        //    if (IdSubmission == 0)
        //        throw new UnknownItem();
        //    else
        //    {
        //        return (from s in Manager.GetIQ<SubmissionEvaluation>()
        //                where s.Submission.Id == IdSubmission && s.Deleted == BaseStatusDeleted.None && s.Evaluator != null
        //                select s.Evaluator.Id).ToList();
        //    }
        //}
        //public List<dtoEvaluatorPermission> GetSubmissionAvailableEvaluators(long IdCallForPaper, long IdSubmission)
        //{
        //    List<dtoEvaluatorPermission> list = new List<dtoEvaluatorPermission>();
        //    try
        //    {
        //        List<long> AssignedEvaluators = (from s in Manager.GetIQ<SubmissionEvaluation>()
        //                                         where s.Submission.Id == IdSubmission && s.Deleted == BaseStatusDeleted.None && s.Evaluator != null
        //                                         && (s.Evaluated || s.EvaluationStartedOn != null)
        //                                         select s.Evaluator.Id).ToList();
        //        list = (from c in Manager.GetAll<CallForPaperEvaluator>(c => c.Call.Id == IdCallForPaper && c.Deleted == BaseStatusDeleted.None)
        //                select CreateEvaluatorPermission(c, AssignedEvaluators)).ToList();
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        Manager.RollBack();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //    }
        //    return list;
        //}
        //private dtoEvaluatorPermission CreateEvaluatorPermission(CallForPaperEvaluator evaluator, List<long> AssignedEvaluators)
        //{
        //    dtoEvaluatorPermission dto = new dtoEvaluatorPermission();
        //    dto.Item = new dtoEvaluator()
        //    {
        //        Id = evaluator.Id,
        //        Deleted = (evaluator.Evaluator == null) ? BaseStatusDeleted.Manual : evaluator.Deleted,
        //        // EvaluatedCount = GetEvaluatedSubmissionForEvaluator(evaluator),
        //        // SubmissionCount = GetAssignedSubmissionForEvaluator(evaluator),
        //        Name = (evaluator.Evaluator == null) ? "-" : evaluator.Evaluator.Name,
        //        DisplayName = (evaluator.Evaluator == null) ? "-" : evaluator.Evaluator.SurnameAndName,
        //        Surname = (evaluator.Evaluator == null) ? "-" : evaluator.Evaluator.Surname
        //    };
        //    Boolean HasEvaluation = (AssignedEvaluators.Contains(evaluator.Id)); // (dto.Item.EvaluatedCount > 0);
        //    dto.Permission = new CoreItemPermission();
        //    dto.Permission.AllowDelete = !HasEvaluation && evaluator.Deleted != BaseStatusDeleted.None;
        //    dto.Permission.AllowEdit = !HasEvaluation && evaluator.Deleted == BaseStatusDeleted.None;
        //    dto.Permission.AllowUnDelete = !HasEvaluation && evaluator.Deleted != BaseStatusDeleted.None;
        //    dto.Permission.AllowVirtualDelete = !HasEvaluation && evaluator.Deleted == BaseStatusDeleted.None;
        //    return dto;
        //}
        //public void AssignEvaluatorsToSubmission(List<long> assignedID, CallForPaper call, long IdSubmission)
        //{
        //    List<SubmissionEvaluation> evaluations = (from se in Manager.GetIQ<SubmissionEvaluation>()
        //                                              where se.Call == call && se.Submission.Id == IdSubmission && assignedID.Contains(se.Evaluator.Id)
        //                                              select se).ToList();
        //    LazyUserSubmission submission = Manager.Get<LazyUserSubmission>(IdSubmission);
        //    if (call != null && submission != null)
        //    {
        //        try
        //        {
        //            Manager.BeginTransaction();
        //            litePerson user = Manager.Get<Person>(UC.CurrentUserID);
        //            foreach (SubmissionEvaluation deleted in (from e in evaluations where e.Deleted != BaseStatusDeleted.None select e).ToList())
        //            {
        //                deleted.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
        //                deleted.Deleted = BaseStatusDeleted.None;
        //                Manager.SaveOrUpdate(deleted);
        //            }
        //            List<SubmissionEvaluation> toDelete = (from se in Manager.GetIQ<SubmissionEvaluation>()
        //                                                   where se.Call == call && se.Submission.Id == IdSubmission && !assignedID.Contains(se.Evaluator.Id)
        //                                                   && se.Deleted == BaseStatusDeleted.None
        //                                                   select se).ToList();
        //            foreach (SubmissionEvaluation delete in toDelete)
        //            {
        //                delete.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
        //                delete.Deleted = BaseStatusDeleted.Manual;
        //                Manager.SaveOrUpdate(delete);
        //            }
        //            assignedID = assignedID.Except((from e in evaluations select e.Evaluator.Id).ToList()).ToList();
        //            foreach (long IdEvaluator in assignedID)
        //            {
        //                SubmissionEvaluation evaluation = new SubmissionEvaluation();
        //                evaluation.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
        //                evaluation.Call = call;
        //                evaluation.Community = call.Community;
        //                evaluation.Evaluator = Manager.Get<CallForPaperEvaluator>(IdEvaluator);
        //                evaluation.Submission = Manager.Get<UserSubmission>(IdSubmission);
        //                Manager.SaveOrUpdate(evaluation);
        //            }
        //            Manager.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            Manager.RollBack();
        //        }
        //    }
        //}

        //#region "Statistics"
        //public List<dtoEvaluationStatistic> GetEvaluationStatistics(long IdCallForPaper, long IdsubmissionType, EvaluationOrder order, Boolean ascending, Boolean createPivot)
        //{
        //    CallForPaper call = GetCallForPaper(IdCallForPaper);
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //        return GetEvaluationStatistics(call, IdsubmissionType, order, ascending, createPivot);
        //}
        //public List<dtoEvaluationStatistic> GetEvaluationStatistics(CallForPaper call, long IdsubmissionType, EvaluationOrder order, Boolean ascending, Boolean createPivot)
        //{
        //    List<dtoEvaluationStatistic> list = new List<dtoEvaluationStatistic>();
        //    try
        //    {
        //        if (call == null)
        //            throw new UnknownCallForPaper();
        //        else
        //        {
        //            List<LazyUserSubmission> submissions = (from s in Manager.GetIQ<LazyUserSubmission>()
        //                                                    where s.Deleted == BaseStatusDeleted.None
        //                                                        && s.Call == call && s.Owner != null &&
        //                                                        !(s.Status < SubmissionStatus.accepted
        //                                                        || s.Status == SubmissionStatus.rejected) && (IdsubmissionType < 1 || s.Type.Id == IdsubmissionType)
        //                                                    select s).ToList();

        //            List<SubmissionEvaluation> evaluations = (from c in Manager.GetAll<SubmissionEvaluation>(c => c.Call == call && c.Deleted == BaseStatusDeleted.None)
        //                                                      select c).ToList();
        //            long position = 1;
        //            list = (from s in submissions select CreateEvaluationStatistic(s, (from e in evaluations where e.Submission.Id == s.Id select e).ToList(), createPivot)).ToList();

        //            var query = (from s in list orderby s.SumRating descending select UpdateEvaluationStatistic(s, ref position));

        //            switch (order)
        //            {
        //                case EvaluationOrder.SubmitterName:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.Surname).ThenBy(s => s.Name);
        //                    else
        //                        query = query.OrderByDescending(s => s.Surname).ThenBy(s => s.Name);
        //                    break;
        //                case EvaluationOrder.SubmissionType:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.SubmissionType);
        //                    else
        //                        query = query.OrderByDescending(s => s.SubmissionType);
        //                    break;
        //                case EvaluationOrder.Average:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.AverageRating);
        //                    else
        //                        query = query.OrderByDescending(s => s.AverageRating);
        //                    break;
        //                case EvaluationOrder.Sum:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.SumRating);
        //                    else
        //                        query = query.OrderByDescending(s => s.SumRating);
        //                    break;
        //                default:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.Position);
        //                    else
        //                        query = query.OrderByDescending(s => s.Position);
        //                    break;
        //            }
        //            list = query.ToList();
        //        }
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return list;
        //}
        //private dtoEvaluationStatistic CreateEvaluationStatistic(LazyUserSubmission submission, List<SubmissionEvaluation> evaluations, Boolean createPivot)
        //{
        //    dtoEvaluationStatistic dto = new dtoEvaluationStatistic();
        //    dto.Name = submission.Owner.Name;
        //    dto.Surname = submission.Owner.Surname;
        //    dto.DisplayName = submission.Owner.SurnameAndName;
        //    dto.Id = submission.Id;
        //    dto.IdSubmissionType = submission.Type.Id;
        //    dto.SubmissionType = submission.Type.Name;
        //    //if ((from e in evaluations where e.Evaluated select e).Any())
        //    if ((from e in evaluations where e.Evaluated select e).Any())
        //        dto.AverageRating = Math.Round((from e in evaluations where e.Evaluated select e.AverageRating).Average(), 2);
        //    dto.SumRating = (from e in evaluations select e.SumRating).Sum();
        //    dto.EvaluationsCount = evaluations.Count;
        //    dto.EvaluationComplete = (dto.EvaluationsCount == (from e in evaluations where e.Evaluated select e).Count());

        //    return dto;
        //}
        //private dtoEvaluationStatistic UpdateEvaluationStatistic(dtoEvaluationStatistic statistic, ref long position)
        //{
        //    statistic.Position = position;
        //    position += 1;
        //    return statistic;
        //}
        //#endregion

        //#region "Advanced Statistics"
        //public List<dtoEvaluationAdvancedStatistic> GetEvaluationAdvancedStatistics(long IdCallForPaper, long IdsubmissionType, EvaluationOrder order, Boolean ascending, Boolean createPivot)
        //{
        //    CallForPaper call = GetCallForPaper(IdCallForPaper);
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //        return GetEvaluationAdvancedStatistics(call, IdsubmissionType, order, ascending, createPivot);
        //}
        //public List<dtoEvaluationAdvancedStatistic> GetEvaluationAdvancedStatistics(CallForPaper call, long IdsubmissionType, EvaluationOrder order, Boolean ascending, Boolean createPivot)
        //{
        //    List<dtoEvaluationAdvancedStatistic> list = new List<dtoEvaluationAdvancedStatistic>();
        //    try
        //    {
        //        if (call == null)
        //            throw new UnknownCallForPaper();
        //        else
        //        {
        //            List<LazyUserSubmission> submissions = (from s in Manager.GetIQ<LazyUserSubmission>()
        //                                                    where s.Deleted == BaseStatusDeleted.None
        //                                                        && s.Call == call && s.Owner != null &&
        //                                                        !(s.Status < SubmissionStatus.accepted
        //                                                        || s.Status == SubmissionStatus.rejected) && (IdsubmissionType < 1 || s.Type.Id == IdsubmissionType)
        //                                                    select s).ToList();

        //            List<SubmissionEvaluation> evaluations = (from c in Manager.GetAll<SubmissionEvaluation>(c => c.Call == call && c.Deleted == BaseStatusDeleted.None)
        //                                                      select c).ToList();
        //            List<dtoEvaluator> evaluators = GetEvaluatorsForStatistics(call);
        //            long position = 1;
        //            list = (from s in submissions select CreateEvaluationAdvancedStatistic(s, (from e in evaluations where e.Submission.Id == s.Id select e).ToList(), evaluators, createPivot)).ToList();

        //            var query = (from s in list orderby s.SumRating descending select UpdateEvaluationAdvancedStatistic(s, ref position));

        //            switch (order)
        //            {
        //                case EvaluationOrder.SubmitterName:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.Surname).ThenBy(s => s.Name);
        //                    else
        //                        query = query.OrderByDescending(s => s.Surname).ThenBy(s => s.Name);
        //                    break;
        //                case EvaluationOrder.SubmissionType:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.SubmissionType);
        //                    else
        //                        query = query.OrderByDescending(s => s.SubmissionType);
        //                    break;
        //                case EvaluationOrder.Average:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.AverageRating);
        //                    else
        //                        query = query.OrderByDescending(s => s.AverageRating);
        //                    break;
        //                case EvaluationOrder.Sum:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.SumRating);
        //                    else
        //                        query = query.OrderByDescending(s => s.SumRating);
        //                    break;
        //                default:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.Position);
        //                    else
        //                        query = query.OrderByDescending(s => s.Position);
        //                    break;
        //            }
        //            list = query.ToList();
        //        }
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return list;
        //}
        //private dtoEvaluationAdvancedStatistic CreateEvaluationAdvancedStatistic(LazyUserSubmission submission, List<SubmissionEvaluation> evaluations, List<dtoEvaluator> evaluators, Boolean createPivot)
        //{
        //    dtoEvaluationAdvancedStatistic dto = new dtoEvaluationAdvancedStatistic();
        //    dto.Name = submission.Owner.Name;
        //    dto.Surname = submission.Owner.Surname;
        //    dto.DisplayName = submission.Owner.SurnameAndName;
        //    dto.Id = submission.Id;
        //    dto.IdSubmissionType = submission.Type.Id;
        //    dto.SubmissionType = submission.Type.Name;
        //    //if ((from e in evaluations where e.Evaluated select e).Any())
        //    if ((from e in evaluations where e.Evaluated select e).Any())
        //        dto.AverageRating = Math.Round((from e in evaluations where e.Evaluated select e.AverageRating).Average(), 2);
        //    dto.SumRating = (from e in evaluations select e.SumRating).Sum();
        //    dto.EvaluationsCount = evaluations.Count;
        //    dto.EvaluationComplete = (dto.EvaluationsCount == (from e in evaluations where e.Evaluated select e).Count());
        //    dto.Evaluations = (from e in evaluators
        //                       orderby e.DisplayName
        //                       select new dtoEvaluationResult()
        //                       {
        //                           IdEvaluator = e.Id,
        //                           AverageRating = Math.Round((from ev in evaluations where ev.Evaluator.Id == e.Id select ev.AverageRating).FirstOrDefault(), 2),
        //                           SumRating = (from ev in evaluations where ev.Evaluator.Id == e.Id select ev.SumRating).FirstOrDefault(),
        //                           EvaluationStarted = (from ev in evaluations where ev.Evaluator.Id == e.Id && (ev.Evaluated || ev.EvaluationStartedOn.HasValue) select ev.Evaluated).Any(),
        //                           EvaluationComplete = (from ev in evaluations where ev.Evaluator.Id == e.Id select ev.Evaluated).FirstOrDefault(),
        //                           Assigned = (from ev in evaluations where ev.Evaluator.Id == e.Id select ev.Id).Any()
        //                       }).ToList();

        //    if (createPivot)
        //    {
        //        dto.PivotResults = CreatePivotResults(evaluations);
        //    }
        //    return dto;
        //}
        //private dtoEvaluationAdvancedStatistic UpdateEvaluationAdvancedStatistic(dtoEvaluationAdvancedStatistic statistic, ref long position)
        //{
        //    statistic.Position = position;
        //    position += 1;
        //    return statistic;
        //}
        //#endregion

        //#region "Pivot Statistics"
        //private List<dtoEvaluationPivot> CreatePivotResults(List<SubmissionEvaluation> evaluations)
        //{
        //    List<dtoEvaluationPivot> results = new List<dtoEvaluationPivot>();
        //    foreach (SubmissionEvaluation e in evaluations.Where(e => e.EvaluatedCriteria != null).ToList())
        //    {
        //        results.AddRange((from c in e.EvaluatedCriteria
        //                          where c.Criterion != null && c.Deleted == BaseStatusDeleted.None && c.Criterion.Deleted == BaseStatusDeleted.None
        //                          orderby c.Criterion.Name
        //                          select new dtoEvaluationPivot() { EvaluationComplete = e.Evaluated, CriterionName = c.Criterion.Name, Evaluated = true, IdCriterion = c.Criterion.Id, Value = c.Value, IdEvaluator = e.Evaluator.Id }).ToList());
        //    }
        //    return results;
        //}

        //#endregion
        //public List<dtoEvaluationDetails> GetSubmissionEvaluationsDetail(long IdCallForPaper, long IdSubmission, String TranslatedTotalItem, EvaluationOrder order, Boolean ascending, List<dtoCriterion> criteria)
        //{
        //    //List<dtoEvaluationDetails> details;
        //    List<SubmissionEvaluation> evaluations = (from c in Manager.GetAll<SubmissionEvaluation>(c => c.Call.Id == IdCallForPaper && c.Submission.Id == IdSubmission && c.Deleted == BaseStatusDeleted.None)
        //                                              select c).ToList();
        //    //details = (from c in Manager.GetAll<CallForPaperEvaluator>(c=>c.Call.Id== IdCallForPaper && c.Deleted == BaseStatusDeleted.None)
        //    //           orderby c.Evaluator.SurnameAndName
        //    //           select new dtoEvaluationDetails()
        //    //           {
        //    //                Deleted=c.Deleted, DisplayName= c.Evaluator.SurnameAndName, Id= c.Id, Name=c.Evaluator.Name, Surname = c.Evaluator.Surname}).ToList();

        //    //   var query = (from d in details select UpdateEvaluationDetails(d, (from e in evaluations where e.Evaluator.Id == d.Id select e).FirstOrDefault(), criteria));
        //    var query = (from e in evaluations select UpdateEvaluationDetails(e, criteria));
        //    switch (order)
        //    {
        //        case EvaluationOrder.Sum:
        //            if (ascending)
        //                query = query.OrderBy(s => s.SumRating);
        //            else
        //                query = query.OrderByDescending(s => s.SumRating);
        //            break;
        //        case EvaluationOrder.Average:
        //            if (ascending)
        //                query = query.OrderBy(s => s.AverageRating);
        //            else
        //                query = query.OrderByDescending(s => s.AverageRating);
        //            break;
        //        default:
        //            if (ascending)
        //                query = query.OrderBy(s => s.Surname).ThenBy(s => s.Name);
        //            else
        //                query = query.OrderByDescending(s => s.Surname).ThenBy(s => s.Name);
        //            break;
        //    }
        //    return query.ToList();
        //}

        //private dtoEvaluationDetails UpdateEvaluationDetails(SubmissionEvaluation evaluation, List<dtoCriterion> criteria)
        //{
        //    dtoEvaluationDetails detail = new dtoEvaluationDetails();
        //    detail.Id = evaluation.Id;
        //    detail.DisplayName = evaluation.Evaluator.Evaluator.SurnameAndName;

        //    //if (evaluation != null)
        //    //{
        //    detail.HasComment = !String.IsNullOrEmpty(evaluation.Comment);
        //    detail.Comment = evaluation.Comment;
        //    detail.EvaluationComplete = evaluation.Evaluated;
        //    detail.Criteria = (from c in criteria
        //                       select new dtoCriterionDetails()
        //                       {
        //                           Value = (from ec in evaluation.EvaluatedCriteria
        //                                    where ec.Criterion.Id == c.Id && ec.Deleted == BaseStatusDeleted.None
        //                                    select ec.Value).FirstOrDefault(),
        //                           Evaluated = (from ec in evaluation.EvaluatedCriteria
        //                                        where ec.Criterion.Id == c.Id && ec.Deleted == BaseStatusDeleted.None
        //                                        select ec.Value).Any(),
        //                           Assigned = true,
        //                           Sum = (from ec in evaluation.EvaluatedCriteria
        //                                  where ec.Criterion.Id == c.Id && ec.Deleted == BaseStatusDeleted.None
        //                                  select ec.Value).FirstOrDefault(),
        //                           Id = c.Id
        //                       }).ToList();
        //    detail.EvaluationStarted = (from c in detail.Criteria where c.Evaluated select c).Any();
        //    detail.AverageRating = evaluation.AverageRating;
        //    detail.SumRating = evaluation.SumRating;
        //    detail.Assigned = true;
        //    //}
        //    //else
        //    //{
        //    //    detail.Assigned = false;
        //    //    detail.Criteria = (from c in criteria select new dtoCriterionDetails() { Value = 0, Evaluated = false, Id = c.Id, Assigned=false  }).ToList();
        //    //}
        //    return detail;
        //}


        //public List<dtoSubmissionEvaluation> GetSubmissionsToEvaluate(long IdCallForPaper, long IdsubmissionType, long IdCallForPaperEvaluator, EvaluationOrder order, Boolean ascending)
        //{
        //    CallForPaper call = GetCallForPaper(IdCallForPaper);
        //    CallForPaperEvaluator evaluator = Manager.Get<CallForPaperEvaluator>(IdCallForPaperEvaluator);
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //        return GetSubmissionsToEvaluate(call, IdsubmissionType, evaluator, order, ascending);
        //}

        //public List<dtoSubmissionEvaluation> GetSubmissionsToEvaluate(CallForPaper call, long IdsubmissionType, CallForPaperEvaluator evaluator, EvaluationOrder order, Boolean ascending)
        //{
        //    List<dtoSubmissionEvaluation> list = new List<dtoSubmissionEvaluation>();
        //    try
        //    {
        //        if (call == null)
        //            throw new UnknownCallForPaper();
        //        else
        //        {
        //            List<SubmissionEvaluation> evaluations = (from c in Manager.GetAll<SubmissionEvaluation>(c => c.Call == call && c.Deleted == BaseStatusDeleted.None && c.Evaluator == evaluator && (IdsubmissionType < 1 || c.Submission.Type.Id == IdsubmissionType))
        //                                                      select c).ToList();
        //            List<long> IdSubmissions = (from e in evaluations select e.Submission.Id).ToList();
        //            List<LazyUserSubmission> submissions = (from s in Manager.GetIQ<LazyUserSubmission>()
        //                                                    where IdSubmissions.Contains(s.Id) && (IdsubmissionType < 1 || s.Type.Id == IdsubmissionType)
        //                                                    select s).ToList();


        //            long position = 1;
        //            list = (from e in evaluations select CreateSubmissionToEvaluate(call.Id, evaluator.Id, e, (from s in submissions where s.Id == e.Submission.Id select s).FirstOrDefault())).ToList();

        //            var query = (from s in list orderby s.AverageRating descending select UpdateSubmissionToEvaluate(s, ref position));

        //            switch (order)
        //            {
        //                case EvaluationOrder.SubmitterName:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.Surname).ThenBy(s => s.Name);
        //                    else
        //                        query = query.OrderByDescending(s => s.Surname).ThenBy(s => s.Name);
        //                    break;
        //                case EvaluationOrder.SubmissionType:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.SubmissionType);
        //                    else
        //                        query = query.OrderByDescending(s => s.SubmissionType);
        //                    break;
        //                case EvaluationOrder.Average:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.AverageRating);
        //                    else
        //                        query = query.OrderByDescending(s => s.AverageRating);
        //                    break;
        //                default:
        //                    if (ascending)
        //                        query = query.OrderBy(s => s.Position);
        //                    else
        //                        query = query.OrderByDescending(s => s.Position);
        //                    break;
        //            }
        //            list = query.ToList();
        //        }
        //    }
        //    catch (UnknownCallForPaper ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return list;
        //}
        //private dtoSubmissionEvaluation CreateSubmissionToEvaluate(long IdCallForPaper, long IdEvaluator, SubmissionEvaluation evaluation, LazyUserSubmission submission)
        //{
        //    dtoSubmissionEvaluation dto = new dtoSubmissionEvaluation();
        //    if (submission != null)
        //    {
        //        dto.Name = submission.Owner.Name;
        //        dto.Surname = submission.Owner.Surname;
        //        dto.DisplayName = submission.Owner.SurnameAndName;
        //        dto.IdSubmission = submission.Id;
        //        dto.IdSubmissionType = submission.Type.Id;
        //        dto.SubmissionType = submission.Type.Name;
        //        dto.SubmissionLinkZip = submission.LinkZip;
        //    }
        //    dto.Id = evaluation.Id;
        //    dto.IdCallForPaper = IdCallForPaper;
        //    dto.IdEvaluator = IdEvaluator;
        //    dto.AverageRating = Math.Round(evaluation.AverageRating, 2);
        //    dto.SumRating = evaluation.SumRating;
        //    dto.EvaluationComplete = evaluation.Evaluated;
        //    dto.EvaluationStarted = evaluation.EvaluationStartedOn.HasValue;
        //    dto.Status = (dto.EvaluationComplete ? SubmissionStatus.valuated : dto.EvaluationStarted ? SubmissionStatus.valuating : SubmissionStatus.waitingValuation);

        //    return dto;
        //}
        //private dtoSubmissionEvaluation UpdateSubmissionToEvaluate(dtoSubmissionEvaluation statistic, ref long position)
        //{
        //    statistic.Position = position;
        //    position += 1;
        //    return statistic;
        //}
        //private dtoPrintSubmissionToEvaluate UpdateSubmissionToEvaluate(dtoPrintSubmissionToEvaluate statistic, ref long position)
        //{
        //    statistic.Position = position;
        //    position += 1;
        //    return statistic;
        //}

        //private List<dtoPrintSubmissionToEvaluate> GetSubmissionsToEvaluateForPrint(long IdCallForPaper, long IdsubmissionType, long IdCallForPaperEvaluator, EvaluationOrder order, Boolean ascending)
        //{
        //    CallForPaper call = GetCallForPaper(IdCallForPaper);
        //    CallForPaperEvaluator evaluator = Manager.Get<CallForPaperEvaluator>(IdCallForPaperEvaluator);
        //    if (call == null)
        //        throw new UnknownCallForPaper();
        //    else
        //    {
        //        List<dtoPrintSubmissionToEvaluate> evaluations = (from e in Manager.GetAll<SubmissionEvaluation>(c => c.Call == call && c.Deleted == BaseStatusDeleted.None && c.Evaluator == evaluator && (IdsubmissionType < 1 || (c.Submission != null && c.Submission.Type.Id == IdsubmissionType)))
        //                                                          select new dtoPrintSubmissionToEvaluate()
        //                                                          {
        //                                                              AverageRating = Math.Round(e.AverageRating, 2),
        //                                                              SumRating = e.SumRating,
        //                                                              EvaluationComplete = e.Evaluated,
        //                                                              EvaluationStarted = e.EvaluationStartedOn.HasValue,
        //                                                              Name = e.Submission.Owner.Name,
        //                                                              Surname = e.Submission.Owner.Surname,
        //                                                              Submission = e.Submission,
        //                                                              Status = (e.Evaluated ? SubmissionStatus.valuated : e.EvaluationStartedOn.HasValue ? SubmissionStatus.valuating : SubmissionStatus.waitingValuation)
        //                                                          }).ToList();

        //        long position = 1;
        //        var query = (from s in evaluations orderby s.AverageRating descending select UpdateSubmissionToEvaluate(s, ref position));

        //        switch (order)
        //        {
        //            case EvaluationOrder.SubmitterName:
        //                if (ascending)
        //                    query = query.OrderBy(s => s.Surname).ThenBy(s => s.Name);
        //                else
        //                    query = query.OrderByDescending(s => s.Surname).ThenBy(s => s.Name);
        //                break;
        //            case EvaluationOrder.SubmissionType:
        //                if (ascending)
        //                    query = query.OrderBy(s => s.Submission.Type);
        //                else
        //                    query = query.OrderByDescending(s => s.Submission.Type);
        //                break;
        //            case EvaluationOrder.Average:
        //                if (ascending)
        //                    query = query.OrderBy(s => s.AverageRating);
        //                else
        //                    query = query.OrderByDescending(s => s.AverageRating);
        //                break;
        //            default:
        //                if (ascending)
        //                    query = query.OrderBy(s => s.Position);
        //                else
        //                    query = query.OrderByDescending(s => s.Position);
        //                break;
        //        }
        //        return query.ToList();
        //    }
        //}

        //#region "Print Statistics"
        //public Document PrintSubmissionsToEvaluate(Dictionary<SubmissionTranslations, string> translations, System.IO.Stream stream, Boolean isPdf, long IdCallForPaper, long IdEvaluator, long IdsubmissionType, EvaluationOrder order, Boolean ascending)
        //{
        //    try
        //    {
        //        List<dtoPrintSubmissionToEvaluate> list = GetSubmissionsToEvaluateForPrint(IdCallForPaper, IdsubmissionType, IdEvaluator, order, ascending);
        //        litePerson person = Manager.Get<Person>(UC.CurrentUserID);
        //        if (list == null || list.Count == 0 || person == null)
        //            throw new Exception();
        //        else
        //            //(from s in list select s.Submission).ToList()
        //            return OLDExportHelpers.SubmissionsCompiled(list, list.ToDictionary(s => s.Submission.Id, s => GetSubmissionFiles(s.Submission.Id)), list.ToDictionary(s => s.Submission.Id, s => GetSubmissionSections(s.Submission)), person, translations, stream, isPdf);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ExportError("", ex) { ErrorDocument = OLDExportHelpers.CreateErrorDocument(translations, stream, isPdf) };
        //    }
        //}
        //public String ExportEvaluations(Dictionary<EvaluationTranslations, string> translations, long IdCallForPaper, long IdsubmissionType, EvaluationOrder order, Boolean ascending, Boolean advancedStatistics)
        //{
        //    return (advancedStatistics ? ExportAdvancedEvaluations(translations, IdCallForPaper, IdsubmissionType, order, ascending)
        //        : ExportStandardEvaluations(translations, IdCallForPaper, IdsubmissionType, order, ascending));

        //}
        //private String ExportStandardEvaluations(Dictionary<EvaluationTranslations, string> translations, long IdCallForPaper, long IdsubmissionType, EvaluationOrder order, Boolean ascending)
        //{
        //    try
        //    {
        //        List<dtoEvaluationStatistic> list = GetEvaluationStatistics(IdCallForPaper, IdsubmissionType, order, ascending, true);
        //        litePerson person = Manager.Get<Person>(UC.CurrentUserID);
        //        if (list == null || list.Count == 0 || person == null)
        //            throw new Exception();
        //        else
        //        {
        //            var query = (from c in Manager.GetIQ<CallForPaper>() where c.Id == IdCallForPaper select new { Name = c.Name, Edition = c.Edition, EvalType = c.EvaluationType }).Skip(0).Take(1).ToList().FirstOrDefault();
        //            if (query == null)
        //                return OLDExportHelpers.CreateErrorExcelDocument(translations);
        //            else
        //                return OLDExportHelpers.ExportEvaluations(query.Name, query.Edition, query.EvalType, list, person, translations);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ExportError("", ex) { ExcelDocument = OLDExportHelpers.CreateErrorExcelDocument(translations) };
        //    }
        //}
        //private String ExportAdvancedEvaluations(Dictionary<EvaluationTranslations, string> translations, long IdCallForPaper, long IdsubmissionType, EvaluationOrder order, Boolean ascending)
        //{
        //    try
        //    {
        //        List<dtoEvaluationAdvancedStatistic> list = GetEvaluationAdvancedStatistics(IdCallForPaper, IdsubmissionType, order, ascending, true);
        //        litePerson person = Manager.Get<Person>(UC.CurrentUserID);
        //        if (list == null || list.Count == 0 || person == null)
        //            throw new Exception();
        //        else
        //        {
        //            var query = (from c in Manager.GetIQ<CallForPaper>() where c.Id == IdCallForPaper select new { Name = c.Name, Edition = c.Edition, EvalType = c.EvaluationType }).Skip(0).Take(1).ToList().FirstOrDefault();
        //            if (query == null)
        //                return OLDExportHelpers.CreateErrorExcelDocument(translations);
        //            else
        //                return OLDExportHelpers.ExportAdvancedEvaluations(query.Name, query.Edition, query.EvalType, list, GetEvaluatorsForStatistics(IdCallForPaper), person, translations);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ExportError("", ex) { ExcelDocument = OLDExportHelpers.CreateErrorExcelDocument(translations) };
        //    }
        //}
        //#endregion


        //#endregion

        //#region "Call For Paper To Evaluate"
        //private List<long> CallForPaperForEvaluationID(Community community, litePerson person, Boolean evaluated)
        //{
        //    return (from s in Manager.GetIQ<SubmissionEvaluation>()
        //            where s.Deleted == BaseStatusDeleted.None && s.Community == community && s.Evaluated == evaluated && s.Evaluator.Evaluator == person
        //            select s.Call.Id).Distinct().ToList();
        //}
        //public long CallForPaperToEvaluateCount(int communityId, int userId)
        //{
        //    long count = 0;
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);
        //    //Modules/CallForPapers/List.aspx?CommunityID=2179&View=ToEvaluate
        //    count = ((from s in Manager.GetIQ<SubmissionEvaluation>()
        //              where s.Deleted == BaseStatusDeleted.None && s.Community == community && !s.Evaluated && s.Evaluator.Evaluator == person
        //              select s.Call.Id).Distinct().ToList()).Count();
        //    return count;
        //}
        //public long CallForPaperEvaluatedCount(int communityId, int userId)
        //{
        //    long count = 0;
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);
        //    List<long> notEvaluated = CallForPaperForEvaluationID(community, person, false);
        //    List<long> evaluated = CallForPaperForEvaluationID(community, person, true);
        //    if (evaluated.Count > 0)
        //        count = (from IdCall in evaluated where !notEvaluated.Contains(IdCall) select IdCall).Distinct().Count();
        //    return count;
        //}

        //public IList<dtoCallForPaperForEvaluation> CallForPaperForEvaluation(int communityId, int userId, CallStatusForSubmitters status, int currentPageIndex, int currentPageSize)
        //{
        //    IList<dtoCallForPaperForEvaluation> calls = new List<dtoCallForPaperForEvaluation>();
        //    Community community = Manager.GetCommunity(communityId);
        //    litePerson person = Manager.GetLitePerson(userId);
        //    List<long> IdCallToRetrieve = CallForPaperForEvaluationID(community, person, false);
        //    if (status == CallStatusForSubmitters.Evaluated)
        //    {
        //        List<long> evaluated = CallForPaperForEvaluationID(community, person, true);
        //        IdCallToRetrieve = (from IdCall in evaluated where !IdCallToRetrieve.Contains(IdCall) select IdCall).Distinct().ToList();
        //    }
        //    calls = (from c in Manager.GetIQ<CallForPaper>()
        //             where c.Deleted == BaseStatusDeleted.None && c.Community == community
        //             && IdCallToRetrieve.Contains(c.Id)
        //             orderby c.Name, c.EndDate
        //             select new dtoCallForPaperForEvaluation(c.Id, status, new dtoCallForPaper(c.Id, c.Name, c.Edition, c.Description, c.StartDate,
        //                 c.EndDate, c.AwardDate, c.SubmissionClosed, c.Type, c.DisplayWinner,
        //                 community, c.IsPublic, c.Status, c.EvaluationType, c.AllowSubmissionExtension, c.NotificationEmail))
        //                    ).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();

        //    calls = (from c in calls select c.Update(GetAssignedSubmissionForUser(c.Id, person), GetEvaluatedSubmissionForUser(c.Id, person))).ToList();
        //    return calls;
        //}

        //#endregion

        //#region "evaluations"
        //public Boolean isEvaluatorOfCallForPaper(long IdCallForPaper, int IdPeson)
        //{
        //    Boolean iResponse = false;
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(IdPeson);
        //        iResponse = (from e in Manager.GetIQ<SubmissionEvaluation>()
        //                     where e.Call.Id == IdCallForPaper && e.Evaluator.Deleted == BaseStatusDeleted.None
        //                     && e.Evaluator.Evaluator == person
        //                     select e.Id).Any();
        //    }
        //    catch (Exception ex) { }

        //    return iResponse;
        //}
        //public Boolean isEvaluatorOfSubmission(long IdSubmission, int IdPeson)
        //{
        //    Boolean iResponse = false;
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(IdPeson);
        //        iResponse = (from e in Manager.GetIQ<SubmissionEvaluation>()
        //                     where e.Submission.Id == IdSubmission && e.Evaluator.Deleted == BaseStatusDeleted.None
        //                     && e.Evaluator.Evaluator == person
        //                     select e.Id).Any();
        //    }
        //    catch (Exception ex) { }

        //    return iResponse;
        //}
        //public Boolean AllowViewEvaluation(long IdEvaluation, long IdEvaluator)
        //{
        //    Boolean iResponse = false;
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        //        iResponse = (from e in Manager.GetIQ<SubmissionEvaluation>()
        //                     where e.Id == IdEvaluation && e.Evaluator.Id == IdEvaluator && e.Evaluator.Deleted == BaseStatusDeleted.None
        //                     && e.Evaluator.Evaluator == person
        //                     select e).Any();
        //    }
        //    catch (Exception ex) { }

        //    return iResponse;
        //}
        //public dtoCallForPaper GetDtoCallForPaperFromEvaluation(long IdEvaluation)
        //{
        //    try
        //    {
        //        long IdCallForPaper = (from e in Manager.GetIQ<SubmissionEvaluation>()
        //                               where e.Id == IdEvaluation
        //                               select e.Call.Id).Skip(0).Take(1).ToList().FirstOrDefault();
        //        return GetDtoCallForPaper(IdCallForPaper);

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        //public dtoSubmission GetDtoSubmissionFromEvaluation(long IdEvaluation)
        //{
        //    try
        //    {
        //        long IdSubmission = (from e in Manager.GetIQ<SubmissionEvaluation>()
        //                             where e.Id == IdEvaluation
        //                             select e.Submission.Id).Skip(0).Take(1).ToList().FirstOrDefault();
        //        return GetDtoSubmission(IdSubmission);

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        //public dtoEvaluationData GetEvaluation(long IdEvaluation)
        //{
        //    try
        //    {
        //        var eval = (from e in Manager.GetIQ<SubmissionEvaluation>()
        //                    where e.Id == IdEvaluation
        //                    select new
        //                    {
        //                        AverageRating = e.AverageRating,
        //                        SumRating = e.SumRating,
        //                        Comment = e.Comment,
        //                        Id = e.Id,
        //                        Evaluated = e.Evaluated,
        //                        EvaluatedOn = e.EvaluatedOn,
        //                        EvaluationStartedOn = e.EvaluationStartedOn,
        //                        Deleted = e.Deleted,
        //                        IdCallForPaper = e.Call.Id
        //                    }).Skip(0).Take(1).ToList().FirstOrDefault();

        //        List<EvaluatedCriterion> evaluatedCriteria = (from ec in Manager.GetIQ<EvaluatedCriterion>() where ec.EvaluationOf.Id == IdEvaluation select ec).ToList();
        //        List<dtoCriterion> criteria = (from c in Manager.GetIQ<CallForPaperCriterion>()
        //                                       where c.Deleted == BaseStatusDeleted.None && c.Call.Id == eval.IdCallForPaper
        //                                       orderby c.Name
        //                                       select new dtoCriterion() { Id = c.Id, Name = c.Name, Description = c.Description, MinValue = c.MinValue, MaxValue = c.MaxValue }
        //                                       ).ToList();
        //        dtoEvaluationData data = new dtoEvaluationData() { Deleted = eval.Deleted, Id = eval.Id, AverageRating = eval.AverageRating, SumRating = eval.SumRating, Comment = eval.Comment, EvaluatedOn = eval.EvaluatedOn, EvaluationStartedOn = eval.EvaluationStartedOn };
        //        data.EvaluationComplete = eval.EvaluatedOn.HasValue;
        //        data.EvaluationStarted = eval.EvaluationStartedOn.HasValue;
        //        data.Status = (data.EvaluationComplete ? SubmissionStatus.valuated : (data.EvaluationStarted ? SubmissionStatus.valuating : SubmissionStatus.waitingValuation));
        //        data.Criteria = (from c in criteria
        //                         select new dtoEvaluationCriterion(c, (from ec in evaluatedCriteria where ec.Criterion.Id == c.Id select ec).FirstOrDefault())).ToList();

        //        return data;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        //public void SaveEvaluation(long IdEvaluation, List<dtoEvaluationCriterion> criteria, String comment)
        //{
        //    SaveEvaluation(IdEvaluation, criteria, comment, false);
        //}
        //public void CompleteEvaluation(long IdEvaluation, List<dtoEvaluationCriterion> criteria, String comment)
        //{
        //    SaveEvaluation(IdEvaluation, criteria, comment, true);
        //}
        //public void SaveEvaluation(long IdEvaluation, List<dtoEvaluationCriterion> criteria, String comment, Boolean complete)
        //{
        //    try
        //    {
        //        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
        //        Manager.BeginTransaction();
        //        SubmissionEvaluation evaluation = Manager.Get<SubmissionEvaluation>(IdEvaluation);
        //        evaluation.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //        DateTime saveTime = DateTime.Now;
        //        if (!evaluation.EvaluationStartedOn.HasValue)
        //            evaluation.EvaluationStartedOn = saveTime;
        //        if (complete && (from c in criteria where c.Evaluated select c).Count() != criteria.Count)
        //            throw new EvaluationCompleteError() { CriteriaToEvaluate = (from c in criteria where !c.Evaluated select c).Count() };
        //        else if (complete)
        //            evaluation.EvaluatedOn = saveTime;
        //        evaluation.Comment = comment;
        //        evaluation.Evaluated = complete;
        //        evaluation.AverageRating = (from c in criteria where c.Evaluated select c.Value).Average();
        //        evaluation.SumRating = (from c in criteria where c.Evaluated select c.Value).Sum();

        //        List<long> IdDeleteValues = (from c in criteria where !c.Evaluated select c.IdEvaluatedCriterion).ToList();
        //        if (IdDeleteValues.Count > 0)
        //        {
        //            List<EvaluatedCriterion> DeleteCriterion = (from ec in Manager.GetIQ<EvaluatedCriterion>()
        //                                                        where
        //                                                            ec.EvaluationOf == evaluation && IdDeleteValues.Contains(ec.Id)
        //                                                        select ec).ToList();
        //            Manager.DeletePhysicalList(DeleteCriterion);
        //        }
        //        CriterionEvaluationError exception = new CriterionEvaluationError();
        //        foreach (dtoEvaluationCriterion evaluated in (from c in criteria where c.Evaluated select c).ToList())
        //        {
        //            EvaluatedCriterion criterion = null;
        //            if (evaluated.IdEvaluatedCriterion > 0)
        //            {
        //                criterion = Manager.Get<EvaluatedCriterion>(evaluated.IdEvaluatedCriterion);
        //                criterion.Value = evaluated.Value;
        //                criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //            }
        //            else
        //            {
        //                criterion = (from ec in Manager.Linq<EvaluatedCriterion>()
        //                             where ec.EvaluationOf == evaluation && ec.Deleted == BaseStatusDeleted.None && ec.Criterion != null
        //                             && ec.Criterion.Id == evaluated.IdCriterion
        //                             select ec).Skip(0).Take(1).ToList().FirstOrDefault();
        //                if (criterion == null)
        //                {
        //                    criterion = new EvaluatedCriterion();
        //                    criterion.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //                    criterion.Call = evaluation.Call;
        //                    criterion.Criterion = Manager.Get<CallForPaperCriterion>(evaluated.IdCriterion);
        //                    criterion.EvaluationOf = evaluation;
        //                    criterion.Submission = evaluation.Submission;
        //                }
        //                else
        //                    criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //                criterion.Value = evaluated.Value;
        //            }
        //            try
        //            {
        //                Manager.SaveOrUpdate(criterion);
        //            }
        //            catch (Exception ex)
        //            {
        //                exception.Criteria.Add(evaluated);
        //            }
        //        }
        //        if (exception.Criteria.Count > 0)
        //            throw exception;
        //        Manager.SaveOrUpdate(evaluation);
        //        Manager.Commit();
        //    }
        //    catch (CriterionEvaluationError ex)
        //    {
        //        Manager.RollBack();
        //        throw ex;
        //    }
        //    catch (EvaluationCompleteError ex)
        //    {
        //        Manager.RollBack();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        CriterionEvaluationError exception = new CriterionEvaluationError(ex.Message, ex);
        //        Manager.RollBack();
        //        throw exception;
        //    }
        //}
        #endregion
        #endregion

#region Sign link

        public ModuleLink GetSignModuleLink(Int64 callId, Int64 revisionId)
        {
            CFPSignLink cl = Manager.GetAll<CFPSignLink>(l =>
                        l.CallId == callId && l.RevisionId == revisionId).Skip(0).Take(1).FirstOrDefault();

            if (cl == null)
                return null;

            return Manager.Get<ModuleLink>(cl.LinkId);
        }

        public CFPSignLink SetSignLink(
            Int64 callId,
            Int64 revisionId,
            ModuleActionLink moduleActLink)
        {
            ModuleLink mLink = new ModuleLink(moduleActLink.Description, moduleActLink.Permission, moduleActLink.Action);

            //ToDo: CHECK NULL!!!
            Revision rev = Manager.Get<Revision>(revisionId);
            litePerson owner = rev.Submission.SubmittedBy;


            mLink.CreateMetaInfo(GetPersonFromLite(owner), UC.IpAddress, UC.ProxyIpAddress);
            mLink.DestinationItem = (ModuleObject)moduleActLink.ModuleObject;
            mLink.SourceItem = ModuleObject.CreateLongObject(
                rev.Id,
                rev,
                (int)ModuleCallForPaper.ObjectType.Revision,
                rev.Submission.Community.Id,
                ModuleCallForPaper.UniqueCode,
                ServiceModuleID(ModuleCallForPaper.UniqueCode));


            if (!Manager.IsInTransaction())
            {
                Manager.BeginTransaction();
            }

            CFPSignLink link = Manager.GetAll<CFPSignLink>(l =>
                        l.CallId == callId && l.RevisionId == revisionId).Skip(0).Take(1).FirstOrDefault();

            if (link == null)
            {
                link = new CFPSignLink();
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID) ?? Manager.GetLiteUnknownUser();
                link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                link.LinkId = 0;
                link.CallId = callId;
                link.RevisionId = revisionId;
            }

            if (link.LinkId == 0)
            {
                Manager.SaveOrUpdate<ModuleLink>(mLink);
                link.LinkId = mLink.Id;
                Manager.SaveOrUpdate(link);

            }
            else
            {
                //ToDo: ERRORE!
            }



            try
            {

                Manager.Commit();
            }
            catch (Exception)
            {

                Manager.RollBack();
            }

            return link;

        }

        
#endregion
    }
}