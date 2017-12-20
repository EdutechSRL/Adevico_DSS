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
    public class EditFieldAssociationsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditFieldAssociations View
            {
                get { return (IViewEditFieldAssociations)base.View; }
            }
            private ServiceCallOfPapers _ServiceCall;
            private ServiceCallOfPapers CallService
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
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
            public EditFieldAssociationsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditFieldAssociationsPresenter(iApplicationContext oContext, IViewEditFieldAssociations view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            long idCall = View.PreloadIdCall;

            dtoBaseForPaper call = null;
            CallForPaperType type = CallService.GetCallType(idCall);
            if (type == CallForPaperType.None)
                type = View.PreloadType;
            call = CallService.GetDtoBaseCall(idCall);

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
                ModuleCallForPaper module = null;
                ModuleRequestForMembership moduleR = null;
                switch (type)
                {
                    case CallForPaperType.CallForBids:
                        module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowView = (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                        allowManage = module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper;
                        allowSave = (module.Administration || module.ManageCallForPapers || (module.CreateCallForPaper && idCall == 0) || (call != null && module.EditCallForPaper && currenUser == call.Owner));
                        break;
                    case CallForPaperType.RequestForMembership:
                        moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowView = (moduleR.ViewBaseForPapers || moduleR.Administration || moduleR.ManageBaseForPapers);
                        allowManage = moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper;
                        allowSave = (moduleR.Administration || moduleR.ManageBaseForPapers || (moduleR.CreateBaseForPaper && idCall == 0) || (call != null && moduleR.EditBaseForPaper && currenUser == call.Owner));
                        break;
                    default:
                        break;
                }

                int idModule = (type == CallForPaperType.CallForBids) ? CallService.ServiceModuleID() : RequestService.ServiceModuleID();
                View.IdCallModule = idModule;
                if (call == null)
                    View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                else if (allowManage || allowSave)
                {
                    View.AllowSave = allowSave;
                    View.IdCall = idCall;
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps = CallService.GetAvailableSteps(idCall, WizardCallStep.FieldsAssociation, type);
                    View.LoadWizardSteps(idCall, type, idCommunity, steps);
                    View.LoadAssociations(CallService.GetFieldAssociations(idCall));
                    if (type== CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.EditFieldsAssociation);
                    else
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.EditFieldsAssociation);
                    if (steps.Where(s => s.Id == WizardCallStep.SubmittersType && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                        View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(type, idCall, idCommunity, View.PreloadView));
                }
                else
                    View.DisplayNoPermission(idCommunity, idModule);
            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = CallService.GetCallCommunityContext(call, View.Portalname);
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

        public void SaveSettings(List<dtoFieldAssociation> associations) {
            if (CallService.SaveSettings(associations, View.IdCall))
            {
                View.LoadAssociations(CallService.GetFieldAssociations(View.IdCall));
                View.DisplaySettingsSaved();
                if (View.CallType == CallForPaperType.CallForBids)
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleCallForPaper.ActionType.EditFieldsAssociation);
                else
                    View.SendUserAction(View.IdCommunity, View.IdCallModule, View.IdCall, ModuleRequestForMembership.ActionType.EditFieldsAssociation);
            }
            else
                View.DisplaySettingsUnSaved();
        }
    }
}