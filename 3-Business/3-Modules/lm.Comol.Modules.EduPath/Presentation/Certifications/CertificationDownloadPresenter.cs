using lm.Comol.Modules.EduPath.BusinessLogic;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Modules.EduPath.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Certifications;
namespace lm.Comol.Modules.EduPath.Presentation
{
    public class CertificationDownloadPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private Service _Service;
            private lm.Comol.Core.Certifications.Business.CertificationsService _ServiceCertifications;
            public lm.Comol.Core.Certifications.Business.CertificationsService ServiceCertifications
            {
                get
                {
                    if (_ServiceCertifications == null)
                        _ServiceCertifications = new lm.Comol.Core.Certifications.Business.CertificationsService(AppContext);
                    return _ServiceCertifications;
                }
            }
            private lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService _ServiceTemplates;
            public lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService ServiceTemplates
            {
                get
                {
                    if (_ServiceTemplates == null)
                        _ServiceTemplates = new lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService(AppContext);
                    return _ServiceTemplates;
                }
            }

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCertificationDownload View
            {
                get { return (IViewCertificationDownload)base.View; }
            }
            private Service Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new Service(AppContext);
                    return _Service;
                }
            }
            public CertificationDownloadPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CertificationDownloadPresenter(iApplicationContext oContext, IViewCertificationDownload view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        #region "Initializer view"
            public void InitView(Boolean isOnModalWindow, long idPath, long idActivity, long idSubactivity, Int32 idUser = -1)
            {
                InitView(isOnModalWindow, idPath, idActivity, idSubactivity, Guid.Empty, idUser);
            }
            public void InitView(Boolean isOnModalWindow, long idPath, long idActivity, long idSubactivity, Guid uniqueId, Int32 idUser = -1)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Int32 idCommunity = Service.GetPathIdCommunity(idPath);
                    //dtoSubActivity dto = Service.GetDtoSubActivity(idSubactivity);
                    dtoSubActivityCertificate dto = Service.GetDtoSubActivityCertificate(idSubactivity);
                    if (dto == null || (dto.IdPath!= idPath))
                        View.DisplayUnknownItem();
                    else {
                        if (idUser < 1)
                            idUser = UserContext.CurrentUserID;
                        View.ForUserId = idUser;
                        View.IdPath = idPath;
                        View.IdCommunityContainer = idCommunity;
                        if (View.PreloadForManager)
                        {
                            //Service.
                        }
                        if (dto.IdTemplate==0){
                            Certification cer = GetAvailableUserCertification(dto, idCommunity, idUser);
                            if (cer == null)
                                View.DisplayUnselectedTemplate();
                            else
                            {
                                dto.SaveCertificate = false;
                                DownloadCertification(idCommunity, idPath, dto,uniqueId, idUser);
                            }
                        }
                        else
                            DownloadCertification(idCommunity, idPath, dto, uniqueId, idUser);
                    }
                }
            }
            public void InitView(Boolean isOnModalWindow, long time, long timeValidity, String mac, long idPath, long idActivity, long idSubactivity, Int32 idUser = -1)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Int32 idCommunity = Service.GetPathIdCommunity(idPath);
                    dtoSubActivityCertificate dto = Service.GetDtoSubActivityCertificate(idSubactivity);
                    if (dto == null || (dto.IdPath!= idPath))
                        View.DisplayUnknownItem();
                    else {
                        if (idUser < 1)
                            idUser = UserContext.CurrentUserID;
                        View.ForUserId = idUser;
                        View.IdPath = idPath;
                        View.IdCommunityContainer = idCommunity;
                        lm.Comol.Core.Authentication.liteGenericEncryption sEncryptor = null;

                        Boolean canRestore = false;
                        litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                        if (person != null)
                        {
                            canRestore = (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator);
                            if (!canRestore)
                            {
                                ModuleEduPath mEduPath = Service.ServiceStat.GetPermissionForCertification(idCommunity, person, idPath);
                                canRestore = mEduPath.Administration;
                            }
                            //if (canRestore)
                            //{
                            //    sEncryptor= CurrentManager.GetUrlMacEncryptor();
                            //    canRestore = (sEncryptor != null && sEncryptor.Validate(mac, UserContext.WorkSessionID, time, timeValidity,idPath,idSubactivity,idUser));
                            //}
                        }
                        if (canRestore)
                            View.RestoreCertificate(dto.AllowWithEmptyPlaceHolders,idUser,GetUserCertificationFileName(View.GetDefaulFileName(), idUser), ((dto.AutoGenerated) ?  lm.Comol.Core.Certifications.CertificationType.ManagerProduced :  lm.Comol.Core.Certifications.CertificationType.RuntimeProduced), dto.SaveCertificate);
                        else
                            View.DisplayNoPermissions();
                    }
                }
            }

   
        #endregion

        #region "Download Items"
            private void DownloadCertification(Int32 idCommunity, long idPath, dtoSubActivityCertificate item, Guid uniqueId, Int32 idUser)
            {
                // find if user complete action
                StatusStatistic status = Service.ServiceStat.GetSubActivityStatisticsStatus(item.Id, idUser, DateTime.Now);


                //dtoSubActListUserStat stat = Service.ServiceStat.GetSubActUserStat(item.Id, idUser, DateTime.Now);
                Boolean saveExecution = !Service.ServiceStat.CheckStatusStatistic(status, StatusStatistic.CompletedPassed);//(stat == null || !(from s in stat.usersStat where s.Completion == 100 select s).Any());

                // Check if associated template is valid
               
                List<Certification> certifications = GetAvailableCertifications(item, idCommunity, idUser);
                Certification certification = null;
                if (uniqueId != Guid.Empty)
                    certification = certifications.Where(c => c.FileUniqueId == uniqueId).FirstOrDefault();

                lm.Comol.Core.Certifications.CertificationType type = lm.Comol.Core.Certifications.CertificationType.RuntimeProduced;
                if (item.AutoGenerated)
                    type = lm.Comol.Core.Certifications.CertificationType.AutoProduced;
                else{
                    if (idUser == UserContext.CurrentUserID)
                        type = lm.Comol.Core.Certifications.CertificationType.UserRequired;
                    else
                        type = lm.Comol.Core.Certifications.CertificationType.ManagerProduced;
                }


                if (certifications == null || certifications.Count == 0)
                {
                    Boolean isValid = ServiceTemplates.isValidTemplate(item.IdTemplate, item.IdTemplateVersion);
                    if (isValid && isActionActive(idPath, idUser, item, DateTime.Now))
                        View.GenerateAndDownload(idPath, item.Id, item.AllowWithEmptyPlaceHolders, idUser, GetUserCertificationFileName(View.GetDefaulFileName(), idUser), type, item.SaveCertificate, saveExecution);
                    else if (!isValid)
                        View.DisplayUnselectedTemplate();
                    else
                        View.DisplayUnavailableAction(item.Name);
                }
                else if (certification != null || certifications.Where(c => c.Deleted == BaseStatusDeleted.None && c.Status == CertificationStatus.Valid).Any())
                {
                    if (saveExecution)
                        ExecuteAction(idPath, item.Id, StatusStatistic.CompletedPassed);
                    if (certification==null)
                        certification = GetAvailableUserCertification(certifications, View.PreloadForManager, View.PreloadReferenceTime);
                    View.DownloadCertification(item.AllowWithEmptyPlaceHolders, idUser, GetUserCertificationFileName(View.GetDefaulFileName(), idUser), lm.Comol.Core.Certifications.CertificationType.AutoProduced, item.SaveCertificate, saveExecution, certification.FileUniqueId, certification.FileExtension);
                }
            }

        #endregion

            //public Boolean isCertificationActionMandatory(dtoSubActivity item)
            //{
            //    return Service.ActivityIsMandatoryForParticipant(item.ActivityParentId, 0, 0);
            //}

            //public Boolean HasCertificationToAutoGenerate(long idPath, Int32 idCommunity,Int32 idUser, dtoSubActivity item, DateTime viewStatBefore)
            //{
            //    if (GetAvailableUserCertification(item,idCommunity, idUser) == null && item.AutoGenerated)
            //        return isActionActive(idPath, idUser, item, viewStatBefore);
            //    else
            //        return false;
            //}
            public Boolean isActionActive(long idPath, Int32 idUser, dtoSubActivityCertificate item, DateTime viewStatBefore)
            {
                List<lm.Comol.Modules.EduPath.Domain.DTO.dtoSubActivityLink> links = Service.GetDtoSubactivityActiveLinks(item.Id);
                return Service.isCertificationActionActive(idPath, item, links, (links.Any() ? View.GetQuizInfos(links.Where(l => l.Visible).Select(l => l.IdObject).ToList(), idUser, IsEvaluablePath(idPath)) : new List<dtoQuizInfo>()), idUser, viewStatBefore);
            }
            public Certification GetAvailableUserCertification(dtoSubActivityCertificate item, Int32 idCommunity, Int32 idUser)
            {
                return GetAvailableUserCertification(GetAvailableCertifications(item, idCommunity, idUser), View.PreloadForManager, View.PreloadReferenceTime);
            }
            private Certification GetAvailableUserCertification(List<Certification> items, Boolean isForManage,long timeReference)
            {
                Certification crt = items.Where(c=> c.Deleted== BaseStatusDeleted.None && c.Status == CertificationStatus.Valid
                     && (!isForManage || (isForManage && (!c.CreatedOn.HasValue || c.CreatedOn.Value.Ticks<=timeReference)))).FirstOrDefault();

                return (crt == null) ? items.Where(c => c.Deleted == BaseStatusDeleted.None && c.Status == CertificationStatus.Valid).OrderByDescending(c => c.Id).FirstOrDefault() : crt;
                    
            }
            private List<Certification> GetAvailableCertifications(dtoSubActivityCertificate item, Int32 idCommunity, Int32 idUser)
            {
                ModuleObject source = ModuleObject.CreateLongObject(item.Id, (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity, idCommunity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex);
                source.ServiceID= Service.ServiceModuleID();
                source.FQN = typeof(SubActivity).FullName;
                return ServiceCertifications.GetUserCertifications(source, idUser,true );
            }

        #region "Fill Data Into Template"
            public lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template FillDataIntoTemplate(long idSubActivity, String basePath, String istanceName, ref CertificationError cError)
            {
                return FillDataIntoTemplate(View.IdCommunityContainer, View.ForUserId, View.IdPath, idSubActivity, basePath, istanceName, ref cError);
            }
            public lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template FillDataIntoTemplate(Int32 idCommunity, Int32 idUser, long idPath, long idSubActivity, String basePath, String istanceName, ref CertificationError cError)
            {
                lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template = null;
                SubActivity subActivity = Service.GetSubActivity(idSubActivity);
                if (subActivity != null)
                {
                    template = ServiceTemplates.TemplateGet(subActivity.IdCertificate, subActivity.IdCertificateVersion, basePath);
                    if (template != null)
                    {
                        //litePerson person = CurrentManager.GetLitePerson(idUser);
                        Person person = CurrentManager.GetPerson(idUser);

                        liteCommunity community = CurrentManager.GetLiteCommunity(idCommunity);
                        liteSubscriptionInfo subscription = CurrentManager.GetLiteSubscriptionInfo(idUser, idCommunity);
                        String organization = "";


                        if (community != null)
                            organization = CurrentManager.GetOrganizationName(community.IdOrganization);

                        List<String> pHolders = TemplateEduPathPlaceHolders.PlaceHolders().Values.Select(v => v).ToList();

                        pHolders.AddRange(lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.PlaceHolders().Values.Select(v => v).ToList());
                        if (template.Modules != null && template.Modules.Where(m => m.IdModule == Service.ServiceModuleID()).Any() && subActivity != null)
                        {
                            // DEVO RIEmPIRE I PLACE HOLDERS !
                            List<lm.Comol.Modules.EduPath.Domain.DTO.dtoSubActivityLink> links = Service.GetDtoSubactivityActiveLinks(idSubActivity);
                            if (links == null || links.Count == 0)
                                template.Body.Text = TemplateEduPathPlaceHolders.Translate(template.Body.Text, AppContext, idPath, idUser, subActivity);
                            else
                                template.Body.Text = TemplateEduPathPlaceHolders.Translate(template.Body.Text, AppContext, idPath, idUser, subActivity, View.GetQuizInfos(links.Where(l => l.Visible).Select(l => l.IdObject).ToList(), idUser, IsEvaluablePath(idPath)));
                        }
                        Int32 idLanguage = (person == null) ? UserContext.Language.Id : person.LanguageID;
                        
                        template.Body.Text = lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.Translate(template.Body.Text, person, community, (subscription == null) ? null : subscription.SubscribedOn, organization, CurrentManager.GetTranslatedRole(subscription.IdRole, idLanguage), CurrentManager.GetTranslatedProfileType((person == null) ? (int)UserTypeStandard.Guest : person.TypeID, idLanguage), istanceName);
                        Int32 missingPlaceHolders = pHolders.Where(p => template.Body.Text.Contains(p)).Count();
                        cError = (missingPlaceHolders == 0) ? CertificationError.None : ((missingPlaceHolders == 1) ? CertificationError.EmptyTemplateItem : CertificationError.EmptyTemplateItems);
                    }
                }
                else
                    cError = CertificationError.ExternalItemUnknown;
                return template;
            }
        #endregion
           
        #region "Funzioni Invariate"
            public Boolean ExecuteAction(long idPath, long idSubActivity, StatusStatistic status)
            {
                return Service.ExecuteSubActivityInternal(idSubActivity, UserContext.CurrentUserID, status);
            }
            private Boolean IsEvaluablePath(long idPath)
            {
                try
                {
                    return Service.CheckEpType(Service.GetPathType(idPath), lm.Comol.Modules.EduPath.Domain.EPType.Mark);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            private String GetUserCertificationFileName(String filename,Int32 idUser)
            {
                String userName = CurrentManager.GetUsername(idUser);
                if (!String.IsNullOrEmpty(userName))
                    return ReplaceInvalidFileName(string.Format(filename, "_" + userName));
                else
                    return ReplaceInvalidFileName(string.Format(filename, ""));
            }
            private String ReplaceInvalidFileName(String filename)
            {
                string regex = string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())));
                System.Text.RegularExpressions.Regex removeInvalidChars = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

                return removeInvalidChars.Replace(filename.Replace(" ", "_"), "_");
            }
        #endregion

        #region "Funzioni Gennaio 2014"
            public Boolean SaveCertificationFile(CertificationType cType,Boolean withEmptyPlaceHolders, Int32 idCommunity, Int32 idUser, String cName, String cDescription, long idPath, long idSubActivity, Guid uniqueID, String extension, Boolean restore = false)
            {
                Certification cert = null;
                SubActivity s = Service.GetSubActivity(idSubActivity);
                if (s != null)
                {
                    dtoCertification dto = dtoCertification.Create(cType);
                    dto.Name = cName;
                    if (!String.IsNullOrEmpty(cDescription) && cDescription.Contains("{0}"))
                    {
                        cDescription = string.Format(cDescription, Service.GetPathName(idPath));
                    }
                    dto.Description = cDescription;
                    dto.IdCommunity = idCommunity;
                    dto.IdContainer = idPath;
                    dto.IdOwner = idUser;
                    dto.UniqueIdGeneratedFile = uniqueID;
                    dto.FileExtension = extension;
                    dto.IdTemplate = s.IdCertificate;
                    dto.IdTemplateVersion = s.IdCertificateVersion;
                    dto.WithEmptyPlaceHolders = withEmptyPlaceHolders;
                    dto.SourceItem = ModuleObject.CreateLongObject(idSubActivity, s, (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity, idCommunity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, Service.ServiceModuleID());
                    cert = ServiceCertifications.SaveUserCertification(dto);
                }

                return (cert != null);
            }
            public Language GetUserLanguage(Int32 idUser) {
                if (idUser == UserContext.CurrentUserID)
                    return (Language)UserContext.Language;
                else
                    return CurrentManager.GetUserLanguage(idUser, true);
            }
        #endregion
    }
}