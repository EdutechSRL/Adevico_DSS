using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class SubmissionExportPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSubmissionExport View
            {
                get { return (IViewSubmissionExport)base.View; }
            }
            private ServiceCallOfPapers ServiceCall
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
                }
            }
            private ServiceRequestForMembership ServiceRequest
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            public SubmissionExportPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SubmissionExportPresenter(iApplicationContext oContext, IViewSubmissionExport view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idUser, String owner, long idCall, long idSubmission, long idRevision, CallForPaperType callType, List<lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType> loadTypes)
        {
            View.CallType = callType;
            View.IdRevision = idRevision;
            View.IdSubmission = idSubmission;
            View.SubmissionOwner = owner;

            dtoRevision revision = ServiceCall.GetRevision(idSubmission, idRevision, false);
            if (revision == null)
                View.DisplayNone();
            else
            {
                LoadFiles(revision, idSubmission, loadTypes);
                SetSkinDetails(idUser, idCall);
            }
        }

        private void LoadFiles(dtoRevision revision, long idSubmission, List<lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType> loadTypes)
        {
            List<ExportFileType> types = null;
            if (loadTypes != null && loadTypes.Count > 0)
            {
                types = loadTypes;
                if (loadTypes.Contains(ExportFileType.zip) && !ServiceCall.SubmissionWithUploadedFile(idSubmission, revision.Id))
                    types.Add(ExportFileType.zip);
            }
            else
            {
                types = new List<ExportFileType>();
                if (ServiceCall.SubmissionWithUploadedFile(idSubmission, revision.Id))
                    types.Add(ExportFileType.zip);
                //types.Add(ExportFileType.rtf);
                types.Add(ExportFileType.pdf);
            }
          
            View.LoadFiles(revision.SubmissionFiles(), types);
        }
        private void SetSkinDetails(Int32 idUser, long idCall)
        {
            Language language = CurrentManager.GetDefaultLanguage();
            litePerson person = GetCurrentUser(ref idUser);
            View.IdUserSubmitter = idUser;
            if (language != null)
                View.DefaultLanguageCode = language.Code;
            if (idUser == UserContext.CurrentUserID || person == null || UserContext.CurrentUserID ==0)
                View.UserLanguageCode = UserContext.Language.Code;
            else if (person != null)
                {
                    language = CurrentManager.GetLanguage(person.LanguageID);
                    if (language!=null)
                        View.UserLanguageCode = language.Code;
                    else
                        View.UserLanguageCode = UserContext.Language.Code;
                }
            else
                View.UserLanguageCode = UserContext.Language.Code;

            View.SkinDetails = ServiceCall.GetUserExternalContext(idCall, GetCurrentUser(ref idUser));
            
        }
        public iTextSharp.text.Document ExportToRtf(Boolean webOnlyRender, long idSubmission, long idRevision, String baseFilePath, String clientFileName, Dictionary<SubmissionTranslations, string> translations, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie, lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template)
        {
            iTextSharp.text.Document exportFile = ServiceCall.ExportSubmissionToRTF(webOnlyRender,idSubmission, idRevision, baseFilePath, clientFileName, translations, webResponse, cookie, template);
            dtoRevision revision = ServiceCall.GetRevision(idSubmission, idRevision, false);
            if (revision == null)
                View.DisplayNone();
            else
                LoadFiles(revision, idSubmission, View.AvailableTypes);

            return exportFile;
        }
        public void ExportToPdf(
            Boolean webOnlyRender, 
            long idSubmission, 
            long idRevision,
            String baseFilePath,
            String baseDocTemplateImagePath,
            String clientFileName, 
            Dictionary<SubmissionTranslations, string> translations,
            System.Web.HttpResponse webResponse, 
            System.Web.HttpCookie cookie, 
            lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template)
        {

            Int64 IdCall = ServiceCall.CallIdGetFromSubmission(idSubmission);
            CallPrintSettings sets = new CallPrintSettings();

            if (IdCall > 0)
            {
                sets = ServiceCall.PrintSettingsGetFromCall(IdCall);
                template = ServiceCall.DocTemplateUpdate(template, sets.TemplateId, sets.VersionId,
                    baseDocTemplateImagePath); // baseFilePath);    
            }

            int currentComId = UserContext.CurrentCommunityID;
            Subscription subs = CurrentManager.GetActiveSubscription(UserContext.CurrentUserID, currentComId);
            Person currentUser = CurrentManager.GetPerson(UserContext.CurrentUserID);

            string userType = "";

            userType = CurrentManager.GetTranslatedProfileType(currentUser.TypeID, UserContext.Language.Id);

            CommonPlaceHolderData phData = new CommonPlaceHolderData
            {
                Person = currentUser,
                Community = CurrentManager.GetLiteCommunity(currentComId),
                InstanceName = "",
                OrganizationName = CurrentManager.GetOrganizationName(UserContext.CurrentCommunityOrganizationID),
                Subscription = subs,
                UserType = userType
            }
            ;

            //iTextSharp5.text.Document exportFile = ServiceCall.ExportSubmissionToPDF(webOnlyRender, idSubmission, idRevision, baseFilePath, clientFileName, translations, webResponse, cookie, template);
            iTextSharp5.text.Document exportFile =
                ServiceCall.ExportSubmissionToPDF(
                webOnlyRender,
                idSubmission,
                idRevision,
                baseDocTemplateImagePath,
                clientFileName,
                translations,
                webResponse,
                cookie,
                template,
                sets,
                phData);    //baseFilePath

            dtoRevision revision = ServiceCall.GetRevision(idSubmission, idRevision, false);
            if (revision == null)
                View.DisplayNone();
            else
                LoadFiles(revision, idSubmission, View.AvailableTypes);

            //return exportFile;
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