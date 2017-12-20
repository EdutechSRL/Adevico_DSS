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
    public class EditCallMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditCallMessages View
            {
                get { return (IViewEditCallMessages)base.View; }
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
        public EditCallMessagePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditCallMessagePresenter(iApplicationContext oContext, IViewEditCallMessages view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView()
        {
            long idCall = View.PreloadIdCall;

            CallForPaperType type =  RequestService.GetCallType(idCall);
            if (type != CallForPaperType.RequestForMembership)
                View.DisplayInvalidType();
            else{
                dtoRequest call =  RequestService.GetRequestMessages(idCall);

                View.CallType = type;

                int idCommunity = SetCallCurrentCommunity(call);
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    litePerson currenUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                    Boolean allowManage = false;
                    Boolean allowView = false;
                    Boolean allowSave = false;
                    ModuleRequestForMembership moduleR = RequestService.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                    allowView = (moduleR.ViewBaseForPapers || moduleR.Administration || moduleR.ManageBaseForPapers);
                    allowManage = moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper;
                    allowSave = (moduleR.Administration || moduleR.ManageBaseForPapers || (moduleR.CreateBaseForPaper && idCall == 0) || (call != null && moduleR.EditBaseForPaper && currenUser == call.Owner));


                    int idModule = RequestService.ServiceModuleID();
                    View.IdCallModule = idModule;
                    if (call == null)
                        View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                    else if (allowManage || allowSave)
                    {
                        View.AllowSave = allowSave;
                        View.IdCall = idCall;
                        View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                        List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps = RequestService.GetAvailableSteps(idCall, WizardCallStep.RequestMessages, type);
                        View.LoadWizardSteps(idCall, type, idCommunity, steps);
                        View.StartMessage = call.StartMessage;
                        View.EndMessage = call.EndMessage;
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.ViewCallMessages);
                        if (steps.Where(s => s.Id == WizardCallStep.SubmittersType && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                            View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(call.Type, call.Id, View.IdCommunity, View.PreloadView));
                    }
                    else
                        View.DisplayNoPermission(idCommunity, idModule);
                }
            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = RequestService.GetCallCommunityContext(call, View.Portalname);
            View.SetContainerName(CallStandardAction.Edit, context.CommunityName, context.CallName);
            View.IdCommunity = context.IdCommunity;
            return context.IdCommunity;
            //int idCommunity = 0;
            //Boolean forPortal = (call != null) ? call.IsPortal : false;
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (call != null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


            //community = CurrentManager.GetCommunity(idCommunity);
            //if (community != null)
            //    View.SetContainerName(CallStandardAction.Edit, community.Name, (call != null) ? call.Name : "");
            //else if (currentCommunity != null && !forPortal)
            //{
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //    View.SetContainerName(CallStandardAction.Edit, currentCommunity.Name, (call != null) ? call.Name : "");
            //}
            //else
            //{
            //    idCommunity = 0;
            //    View.SetContainerName(CallStandardAction.Edit, View.Portalname, (call != null) ? call.Name : "");
            //}
            //View.IdCommunity = idCommunity;
            //return idCommunity;
        }
        public void SaveSettings(String startMessage, String endMessage) {
            if (String.IsNullOrEmpty(startMessage) || string.IsNullOrEmpty(endMessage))
                View.DisplayInvalidMessage();
            else if (RequestService.SaveRequestMessages(View.IdCall, startMessage, endMessage)) {
                CallForPaperType type = View.CallType;
                long idCall = View.IdCall;
                View.LoadWizardSteps(View.IdCall, type, View.IdCommunity, RequestService.GetAvailableSteps(idCall, WizardCallStep.RequestMessages, type));
                View.DisplaySaveErrors(false);
            }
            else   
                View.DisplaySaveErrors(true);
        }
    }
}