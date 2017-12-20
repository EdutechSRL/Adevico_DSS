using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Presentation.Base.Controls.IView;

namespace lm.Comol.Modules.CallForPapers.Presentation.Base.Controls
{
    public class PrintDraftPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewPrintDraft View
            {
                get { return (IViewPrintDraft)base.View; }
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
            public PrintDraftPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PrintDraftPresenter(iApplicationContext oContext, IViewPrintDraft view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        
        

        public bool CanPrint
        {
            get
            {
                try
                {
                    BaseForPaper call = CurrentManager.Get<BaseForPaper>(View.IdCall);
                    if (call == null)
                    {
                        return false;
                    }

                    if (CallService.HasManagePermission(call.Community.Id, UserContext.CurrentUserID, call.Type))
                    {
                        return true;
                    }

                    CallPrintSettings set = CallService.PrintSettingsGetFromCall(View.IdCall);

                    //Partecipanti/valutatori: SOLO se ammessa la stampa per le bozze!
                    if (set != null && set.AllowPrintDraft)
                    {
                        return CallService.IsCallAvailableByUser(View.IdCall, UserContext.CurrentUserID) //Partecipanti
                               || CallService.isEvaluatorOfCallForPaper(View.IdCall, UserContext.CurrentUserID);
                            //Valutatori
                    }
                }
                catch
                {
                    return false;
                }
                
                
                return false;
            }
        }

        public void Initialize(long idCall, long submissionTypeId, long idRevision, long idSubmission)
        {
            if (idCall <= 0)
            {
                return;
            }    

            View.IdCall = idCall;

            if (!CanPrint)
                return;

            BaseForPaper call = CurrentManager.Get<BaseForPaper>(idCall);

            if (call == null)
            {
                return;
            }

            int comId = call.Community.Id;
            int usrId = UserContext.CurrentUserID;


            //Manager Bando
            
            //Individuo eventuale sottomissione, revisione, bando, tipo di bando...
            View.CallName = call.Name;
            View.CallType = call.Type;
            View.IdSubmission = idSubmission;
            View.IdRevision = idRevision;

            SubmitterType type = null;
            if (idSubmission > 0 && idRevision > 0)
            {
                dtoSubmissionRevision subRev = CallService.GetSubmissionWithRevisions(idSubmission, true);

                if (subRev != null && subRev.Type != null && subRev.Revisions != null && subRev.Revisions.Any())
                {
                    dtoRevision rev = subRev.Revisions.FirstOrDefault(r => r.Id == idRevision);
                    if (rev != null)
                    {
                        View.IdRevision = idRevision;
                    }
                    else
                    {
                        View.IdRevision = 0;
                    }
                    View.IdSubmission = subRev.Id;

                    type = CurrentManager.Get<SubmitterType>(subRev.Type.Id);
                }
                else
                {
                    View.IdSubmission = 0;
                    View.IdRevision = 0;
                }
            }
            
            if(type == null)
            {
                type = call.SubmittersType.FirstOrDefault(st => st.Id == submissionTypeId);

                if (type == null)
                    type = call.SubmittersType.OrderBy(st => st.DisplayOrder).FirstOrDefault();
            }

            View.SubmissionType = type;
        }

        private lm.Comol.Core.DomainModel.Helpers.ExternalPageContext _skinDetails { get; set; }

        public lm.Comol.Core.DomainModel.Helpers.ExternalPageContext GetSkinDetails(long idCall)
        {
            if (_skinDetails == null)
            {
                if (UserContext == null)
                    return null;
                
                _skinDetails = CallService.GetUserExternalContext(idCall, UserContext.CurrentUserID);
            }

            return _skinDetails;

        }


        /// <summary>
        /// Esporta una sottomissione FITTIZIA/VUOTA! come bozza
        /// Al momento NON E' Prevista la stampa di una sottomissione/revisione in bozza!!!
        /// </summary>
        /// <param name="idCall">ID call</param>
        /// <param name="baseDocTemplateImagePath">Path immagini doctemplate (da configurazione)</param>
        /// <param name="clientFileName">Nome file esportato</param>
        /// <param name="translations">Translation: dictionary per la traduzione di termini</param>
        /// <param name="webResponse">Web response</param>
        /// <param name="cookie">Cookie</param>
        /// <param name="template">Template default (fake)</param>
        /// <param name="idSubmitterType">Id submitter Type: dipende dal bando. SE = 0, il primo disponibile. Per sviluppi futuri, al momento lasciato il primo che trova...</param>
        public void ExportDraftToPdf(
           String baseDocTemplateImagePath,
            String clientFileName,
            Dictionary<SubmissionTranslations, string> translations,
            System.Web.HttpResponse webResponse,
            System.Web.HttpCookie cookie,
            lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template)
        {
            long idCall = View.IdCall;

            long idSubmitterType = (View.SubmissionType != null) ? View.SubmissionType.Id : 0;

            //Pagine "normali"
            CallPrintSettings sets = new CallPrintSettings();

            if (idCall > 0)
            {
                sets = CallService.PrintSettingsGetFromCall(idCall);

                //Solo pagina di EDIT!
                View.UpdateSettings(ref sets);

                template = CallService.DocTemplateUpdate(
                    template, 
                    sets.TemplateId, 
                    sets.VersionId,
                    baseDocTemplateImagePath); // baseFilePath);    
            }

            int currentComId = UserContext.CurrentCommunityID;
            Subscription subs = CurrentManager.GetActiveSubscription(UserContext.CurrentUserID, currentComId);
            Person currentUser = CurrentManager.GetPerson(UserContext.CurrentUserID);

            string userType = "";

            if (currentUser == null)
                currentUser = CurrentManager.GetUnknownUser();

            userType = CurrentManager.GetTranslatedProfileType(currentUser.TypeID, UserContext.Language.Id);

            CommonPlaceHolderData phData = new CommonPlaceHolderData
            {
                Person = currentUser,
                Community = CurrentManager.GetLiteCommunity(currentComId),
                InstanceName = "",
                OrganizationName = CurrentManager.GetOrganizationName(UserContext.CurrentCommunityOrganizationID),
                Subscription = subs ?? new Subscription(),
                UserType = userType
            };
            
            long revisionId = View.IdRevision;


            


            if (revisionId <= 0)
            {
                iTextSharp5.text.Document exportFile =
                    CallService.ExportCallDraftToPDF(
                        idCall,
                        clientFileName,
                        translations,
                        webResponse,
                        cookie,
                        template,
                        sets,
                        phData,
                        idSubmitterType);
            }
            else
            {
                
                iTextSharp5.text.Document exportFile =
                    CallService.ExportSubmissionDraftToPDF(
                        true,
                        View.IdSubmission,
                        View.IdRevision,
                        baseDocTemplateImagePath,
                        clientFileName,
                        translations,
                        webResponse,
                        cookie,
                        template,
                        sets,
                        phData);
            }
            


        }
    }
}
