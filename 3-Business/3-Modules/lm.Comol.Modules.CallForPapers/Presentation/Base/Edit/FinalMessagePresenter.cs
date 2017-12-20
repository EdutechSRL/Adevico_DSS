using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class FinalMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _Service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewFinalMessage View
            {
                get { return (IViewFinalMessage)base.View; }
            }
            private ServiceCallOfPapers Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCallOfPapers(AppContext);
                    return _Service;
                }
            }
            private ServiceRequestForMembership _ServiceRequest;
            private ServiceRequestForMembership RequestService
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            public FinalMessagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public FinalMessagePresenter(iApplicationContext oContext, IViewFinalMessage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean forPublicCall)
        {
            long idSubmission = View.PreloadedIdSubmission;
            long idRevision = View.PreloadedIdRevision;
            System.Guid uniqueID = View.PreloadedUniqueID;
            long idCall = View.PreloadIdCall;
            dtoBaseForPaper call = null;
            CallForPaperType type = Service.GetCallType(idCall);
            call = Service.GetDtoBaseCall(idCall);
            View.CallType = type;
            int idCommunity = SetCallCurrentCommunity(call);
            int idModule = Service.ServiceModuleID();

            Boolean isAnonymous = UserContext.isAnonymous;
            if (call == null)
                View.LoadUnknowCall(idCommunity, idModule, idCall, type);
            else if (isAnonymous && !forPublicCall)
            {
                if (call.IsPublic)
                    View.GoToUrl(RootObject.FinalMessage(type, idCall, idSubmission, idRevision, uniqueID, true, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                else
                    View.DisplaySessionTimeout();
            }
            else
            {
                int idUser = UserContext.CurrentUserID;
                litePerson currenUser = GetCurrentUser(ref idUser);

                View.IdCallModule = idModule;
                View.isAnonymousSubmission = forPublicCall;
                dtoLazySubmission submission = Service.GetSubmission(idCall, idSubmission, uniqueID);

                Boolean allowSeeList = false;
                Boolean hasPermissionToSubmit = false;
                Int32 containerIdCommunity = View.PreloadIdOtherCommunity;
                if (!isAnonymous && !forPublicCall)
                {
                    hasPermissionToSubmit = Service.IsCallAvailableByUser(idCall, UserContext.CurrentUserID);
                    switch (type) { 
                        case CallForPaperType.CallForBids:
                            ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                            allowSeeList = (module.ViewCallForPapers || module.ManageCallForPapers || module.Administration);
                            break;
                        case CallForPaperType.RequestForMembership:
                            ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(idUser, idCommunity);
                            allowSeeList = (moduleR.ViewBaseForPapers || moduleR.ManageBaseForPapers || moduleR.Administration);
                            break;
                    }
                    
                }

                if (forPublicCall)
                    View.InitializeView(Service.GetExternalContext(idCall));

                if (allowSeeList || hasPermissionToSubmit)
                    View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, type, CallStandardAction.List, (hasPermissionToSubmit && idCommunity >0 && containerIdCommunity >0 && idCommunity != containerIdCommunity) ? containerIdCommunity : idCommunity, CallStatusForSubmitters.Submitted));
                
                if (submission == null)
                    View.LoadUnknowSubmission(idCommunity,idModule,idSubmission, type);
                else if (!forPublicCall && submission.Owner.Id!=idUser)
                    View.DisplayNoPermission(idCommunity, idModule); 
                else{
                    if (submission.Owner.Id == idUser || forPublicCall) {
                        if (idRevision >= 1) {
                            Revision rev = Service.GetRevision(idRevision);
                            if (rev==null || (rev!= null && (rev.Type== RevisionType.Original || rev.Status==  RevisionStatus.Approved)))
                                View.SetActionUrl(CallStandardAction.ViewOwnSubmission, RootObject.ViewSubmission(
                                    type, 
                                    idCall, 
                                    idSubmission, 
                                    idRevision, 
                                    uniqueID, 
                                    call.IsPublic && submission.IsAnonymous, 
                                    View.FromPublicList, 
                                    CallStatusForSubmitters.Submitted, 
                                    (hasPermissionToSubmit && idCommunity > 0 && containerIdCommunity > 0 && idCommunity != containerIdCommunity) ? containerIdCommunity : idCommunity
                                    , 0));
                            else
                                View.SetActionUrl(CallStandardAction.ViewOwnSubmission, RootObject.UserReviewCall(type, idCall,  idSubmission, idRevision, CallStatusForSubmitters.Submitted, (hasPermissionToSubmit && idCommunity > 0 && containerIdCommunity > 0 && idCommunity != containerIdCommunity) ? containerIdCommunity : idCommunity));

                        }
                        else
                            View.SetActionUrl(CallStandardAction.ViewOwnSubmission, 
                                RootObject.ViewSubmission(
                                    type, 
                                    idCall, 
                                    idSubmission, 
                                    idRevision, 
                                    uniqueID, 
                                    call.IsPublic && submission.IsAnonymous, 
                                    View.FromPublicList, 
                                    CallStatusForSubmitters.Submitted, 
                                    (hasPermissionToSubmit && idCommunity > 0 && containerIdCommunity > 0 && idCommunity != containerIdCommunity) ? containerIdCommunity : idCommunity
                                    , 0)
                                );
                        switch (type) { 
                            case CallForPaperType.CallForBids:
                                View.LoadDefaultMessage();
                                break;
                            case CallForPaperType.RequestForMembership:
                                dtoRequest request =  RequestService.GetRequestMessages(idCall);

                                View.LoadMessage((request==null) ? "" : request.EndMessage);
                                break;
                        }
                    }
                    else
                        View.DisplayNoPermission(idCommunity, idModule);
                }
            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = RequestService.GetCallCommunityContext(call, View.Portalname);
            View.SetContainerName(context.CommunityName, context.CallName);
            View.IdCallCommunity = context.IdCommunity;
            return context.IdCommunity;
            //int idCommunity = 0;
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //Boolean forPortal = (call != null) ? call.IsPortal : false;
            //if (call != null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


            //community = CurrentManager.GetCommunity(idCommunity);
            //if (community != null)
            //    View.SetContainerName(community.Name, (call != null) ? call.Name : "");
            //else if (currentCommunity != null && !forPortal)
            //{
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //    View.SetContainerName(currentCommunity.Name, (call != null) ? call.Name : "");
            //}
            //else
            //{
            //    idCommunity = 0;
            //    View.SetContainerName(View.Portalname, (call != null) ? call.Name : "");
            //}
            //View.IdCallCommunity = idCommunity;
            //return idCommunity;
            }
        private litePerson GetCurrentUser(ref Int32 idUser)
        {
            litePerson person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetLitePerson(idUser);
            return person;
        }
    }
}