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
    public class CallPreviewPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _Service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewPreviewCall View
            {
                get { return (IViewPreviewCall)base.View; }
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
            public CallPreviewPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CallPreviewPresenter(iApplicationContext oContext, IViewPreviewCall view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView()
            {
                long idCall = View.PreloadIdCall;

                dtoCall call = null;
                CallForPaperType type = Service.GetCallType(idCall);
                call = Service.GetDtoCall(idCall);

                View.CallType = type;
                int idCommunity = SetCallCurrentCommunity(call);
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    litePerson currenUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                    ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);

                    int idModule = Service.ServiceModuleID();
                    View.IdCallModule = idModule;
                    if (call == null)
                        View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                    else if (module.Administration || module.ManageCallForPapers)
                    {
                        View.IdCall = idCall;
                        LoadCall(call);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewPreviewCallForPaper);
                    }
                    else
                        View.DisplayNoPermission(idCommunity, idModule);
                }
            }

            private int SetCallCurrentCommunity(dtoBaseForPaper call)
            {
                dtoCallCommunityContext context = Service.GetCallCommunityContext(call, View.Portalname);
                View.SetContainerName( context.CommunityName, context.CallName);
                View.IdCommunity = context.IdCommunity;
                return context.IdCommunity;
                //int idCommunity = 0;
                //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
                //Community community = null;
                //if (call != null)
                //    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


                //community = CurrentManager.GetCommunity(idCommunity);
                //if (community != null)
                //    View.SetContainerName(community.Name, (call != null) ? call.Name : "");
                //else if (currentCommunity != null && !call.IsPortal)
                //{
                //    idCommunity = this.UserContext.CurrentCommunityID;
                //    View.SetContainerName(currentCommunity.Name, (call != null) ? call.Name : "");
                //}
                //else
                //{
                //    idCommunity = 0;
                //    View.SetContainerName(View.Portalname, (call != null) ? call.Name : "");
                //}
                //View.IdCommunity = idCommunity;
                //return idCommunity;
            }

            private void LoadCall(dtoCall dto)
            {
                View.LoadCallInfo(dto);
                BaseForPaper call = Service.GetCall(dto.Id);
                if (call != null) {
                    long idSubmitter = 0;
                    List<dtoSubmitterType> submitters = Service.GetCallAvailableSubmittersType(call);
                    View.HasMultipleSubmitters = (submitters.Count>0);
                    if (submitters.Count==1)
                        idSubmitter = submitters[0].Id;
                    else
                        View.LoadSubmitterTypes(submitters);
                    if (idSubmitter != 0 && submitters.Where(s => s.Id == idSubmitter).Any())
                    {
                        if (submitters.Count > 1)
                            View.SetSubmitterName(submitters.Where(s => s.Id == idSubmitter).FirstOrDefault().Name);
                        else
                            View.SetGenericSubmitterName();
                    }
                    LoadSections(call, idSubmitter);
                }
            }

            public void LoadSections(long idCall, long idSubmitter){
                BaseForPaper call = Service.GetCall(idCall);
                if (call != null)
                {
                    SubmitterType submitter = Service.GetSubmitterType(idSubmitter);
                    if (View.HasMultipleSubmitters)
                    {
                        if (submitter != null)
                            View.SetSubmitterName(submitter.Name);
                        else
                        {
                            View.SetSubmitterName("");
                            idSubmitter = 0;
                        }
                    }
                    else if (submitter == null)
                        idSubmitter = 0;
                    LoadSections(call, idSubmitter);
                }
            }
            private void LoadSections(BaseForPaper call, long idSubmitter)
            {
                View.LoadAttachments(Service.GetAvailableCallAttachments(call.Id, idSubmitter));
                View.LoadRequiredFiles(Service.GetRequiredFiles(call,idSubmitter));
                View.LoadSections(Service.GetSubmissionFields(call,idSubmitter));
            }

            public lm.Comol.Core.DomainModel.Helpers.ExternalPageContext GetSkin() {
                return Service.GetExternalContext(View.IdCall);
            }
    }
}