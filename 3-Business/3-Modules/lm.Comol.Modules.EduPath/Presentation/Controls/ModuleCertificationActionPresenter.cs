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
    public class ModuleCertificationActionPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewModuleCertificationAction View
            {
                get { return (IViewModuleCertificationAction)base.View; }
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
            public ModuleCertificationActionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleCertificationActionPresenter(iApplicationContext oContext, IViewModuleCertificationAction view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        #region "Initializer view"
            public void InitView(dtoInternalActionInitializer dto)
            {
                InitView(dto, (Display(dto.Display, DisplayActionMode.defaultAction) ? StandardActionType.Play : StandardActionType.None));
            }
            public void InitView(int idUser, dtoInternalActionInitializer dto)
            {
                InitView(idUser,dto, (Display(dto.Display, DisplayActionMode.defaultAction) ? StandardActionType.Play : StandardActionType.None));
            }
            public void InitView(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.InsideOtherModule = true;
                InitializeControlByLink(dto, actionsToDisplay);
            }
            public void InitView(int idUser, dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.ForUserId = idUser;
                InitView(dto, actionsToDisplay);
            }
            public List<dtoModuleActionControl> InitRemoteControlView(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.InsideOtherModule = true;
                return InitializeControlByLink(dto, actionsToDisplay);
            }
            public List<dtoModuleActionControl> InitRemoteControlView(int idUser, dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.ForUserId = idUser;
                return InitRemoteControlView(dto, actionsToDisplay);
            }
            private List<dtoModuleActionControl> InitializeControlByLink(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (dto.SubActivity == null || dto.SubActivity.Id == 0)
                    View.DisplayEmptyAction();
                else
                {
                    View.AllowWithEmptyPlaceHolders = dto.SubActivity.AllowWithEmptyPlaceHolders;
                    View.AutoGenerated = dto.SubActivity.AutoGenerated;
                    View.ItemIdentifier = "subactivity_" + dto.SubActivity.Id.ToString();
                    actions = AnalyzeActions(dto, actionsToDisplay);
                }
                return actions;
            }
        #endregion

        #region "Action Parsing"
            private List<dtoModuleActionControl> AnalyzeActions(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                View.ContainerCSS = dto.ContainerCSS;
                View.IconSize = dto.IconSize;

                if (dto.SubActivity == null || dto.SubActivity.Id == 0)
                    View.DisplayEmptyAction();
                else
                {
                    View.Display = dto.Display;
                    View.IdTemplate = dto.SubActivity.IdCertificate;
                    View.IdTemplateVersion = dto.SubActivity.IdCertificateVersion;
                    View.IdPath = dto.IdPath;
                    View.IdUnit = dto.IdUnit;
                    View.IdActivity = dto.IdActivity;

                    liteCommunity community = Service.GetPathCommunity(dto.IdPath);
                    View.IdCommunityContainer = (community != null) ? community.Id : dto.IdPathCommunity;
                    View.CertificationName = dto.SubActivity.Name;
                    View.IdSubActivity = dto.SubActivity.Id;
                    if (dto.SubActivity.IdCertificate == 0)
                    {
                        if (Display(dto.Display, DisplayActionMode.adminMode))
                            View.DisplayUnselectedTemplateInfo();
                        else
                        {
                            Certification cer = GetAvailableUserCertification(dto.SubActivity, View.IdCommunityContainer, View.ForUserId);
                            if (cer == null)
                                View.DisplayUnselectedTemplate();
                            else
                            {
                                dto.SubActivity.SaveCertificate = false;
                                AnalyzeInternalItem(dto.IdPath, dto.SubActivity, dto.Display, actionsToDisplay);
                            }
                        }
                    }
                    else
                    {
                        // Check if associated template is valid
                        //Boolean isValid = ServiceTemplates.isValidTemplate(dto.SubActivity.IdCertificate, dto.SubActivity.IdCertificateVersion);
                        if (dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).Any() && (Display(dto.Display, DisplayActionMode.defaultAction) || Display(dto.Display, DisplayActionMode.text) || Display(dto.Display, DisplayActionMode.textDefault)))
                            View.DisplayPlaceHolders(dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).ToList());
                        actions = AnalyzeInternalItem(dto.IdPath, dto.SubActivity, dto.Display, actionsToDisplay);
                    }
                }
                return actions;
            }
            private List<dtoModuleActionControl> AnalyzeInternalItem(long idPath,  dtoSubActivity item, DisplayActionMode display, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (item != null)
                {
                    if (Display(display, DisplayActionMode.text))
                        View.DisplayItem(item.Name);
                    else if (Display(display, DisplayActionMode.adminMode))
                        View.DisplayItemAdminInfo(item, Service.GetDtoSubactivityActiveLinks(item.Id).Where(sl => sl.Mandatory || sl.Visible).ToList(), Service.GetPath(View.IdPath));
                    else if (Display(display, DisplayActionMode.defaultAction))
                        DisplayDefaultAction(idPath, item);
                    actions = GenerateActions(item);
                    if (Display(display, DisplayActionMode.actions) && actionsToDisplay != StandardActionType.None)
                        View.DisplayActions(actions.Where(a => ((int)a.ControlType & (int)actionsToDisplay) > 0).ToList());
                }
                return actions;
            }
            private void DisplayDefaultAction(long idPath,  dtoSubActivity item)
            {
                // find if user complete action
                //StatusStatistic status = Service.ServiceStat.GetSubActivityStatisticsStatus(item.Id, View.ForUserId, DateTime.Now);
                Boolean refreshContainer = !Service.ServiceStat.CheckStatusStatistic(item.StatusStat, StatusStatistic.CompletedPassed);

                //Boolean saveExecution = (stat == null || !(from s in stat.usersStat where s.Completion == 100 select s).Any());
                // Check if associated template is valid
                Boolean isValid = ServiceTemplates.isValidTemplate(item.IdCertificate, item.IdCertificateVersion);
                List<Certification> certifications = GetAvailableCertifications(item, View.IdCommunityContainer, View.ForUserId);

                //View.SaveRunningAction = saveExecution;
                if (certifications == null || certifications.Count == 0)
                {
                    if (isActionActive(View.IdPath, View.ForUserId, item, DateTime.Now))
                        View.DisplayDownloadUrl(item.Name, RootObject.LoadRenderCertificationPage(idPath, item, View.ForUserId), false, refreshContainer);
                        //View.DisplayItemForGenerate(item.Name, item.SaveCertificate);
                    else
                        View.DisplayItem(item.Name);
                }
                else if (certifications.Where(c => c.Deleted == BaseStatusDeleted.None && c.Status == CertificationStatus.Valid).Any())
                {
                    View.DisplayDownloadUrl(item.Name, RootObject.LoadRenderCertificationPage(idPath, item, View.ForUserId, GetAvailableUserCertification(certifications)), true, refreshContainer);
                    //LoadRenderCertificationPage
                    //View.DisplayItem(item.Name, cer.FileUniqueId, cer.FileExtension);
                }
            }
            private List<dtoModuleActionControl> GenerateActions(dtoSubActivity item)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();

                Boolean isValid = ServiceTemplates.isValidTemplate(item.IdCertificate, item.IdCertificateVersion);
                Certification c = GetAvailableUserCertification(item, View.IdCommunityContainer, View.ForUserId);
                if (c != null || (!item.SaveCertificate))
                {
                    actions.Add(new dtoModuleActionControl() { ControlType = StandardActionType.Play, isEnabled = true, ExtraData = (c != null) ? c.FileUniqueId.ToString() + "," + c.FileExtension : "" });
                    actions.Add(new dtoModuleActionControl() { ControlType = StandardActionType.ViewPersonalStatistics, isEnabled = true, ExtraData = (c != null) ? c.FileUniqueId.ToString() + "," + c.FileExtension : "" });
                    actions.Add(new dtoModuleActionControl() { ControlType = StandardActionType.ViewAdvancedStatistics, isEnabled = true, ExtraData = (c != null) ? c.FileUniqueId.ToString() + "," + c.FileExtension : "" });
                    actions.Add(new dtoModuleActionControl() { ControlType = StandardActionType.ViewUserStatistics, isEnabled = true, ExtraData = (c != null) ? c.FileUniqueId.ToString() + "," + c.FileExtension : "" });
                }
                else
                    actions.Add(new dtoModuleActionControl() { ControlType = StandardActionType.Play, isEnabled = true });

                actions.Where(a => !string.IsNullOrEmpty(a.LinkUrl) && a.LinkUrl.Contains("//")).ToList().ForEach(a => a.LinkUrl = a.LinkUrl.Replace("//", "/"));
                return actions;
            }
            private Boolean Display(DisplayActionMode current, DisplayActionMode required)
            {
                return ((long)current & (long)required) > 0;
            }
        #endregion

            public Boolean isCertificationActionMandatory(dtoSubActivity item)
            {
                return Service.ActivityIsMandatoryForParticipant(item.ActivityParentId, 0, 0);
            }

            public Boolean HasCertificationToAutoGenerate(long idPath, Int32 idCommunity,Int32 idUser, dtoSubActivity item, DateTime viewStatBefore)
            {
                if (GetAvailableUserCertification(item,idCommunity, idUser) == null && item.AutoGenerated)
                    return isActionActive(idPath, idUser, item, viewStatBefore);
                else
                    return false;
            }
            public Boolean isActionActive(long idPath,Int32 idUser,dtoSubActivity item,DateTime viewStatBefore) {
                List<lm.Comol.Modules.EduPath.Domain.DTO.dtoSubActivityLink> links = Service.GetDtoSubactivityActiveLinks(item.Id);
                return Service.isCertificationActionActive(idPath, item, links, (links.Any() ? View.GetQuizInfos(links.Where(l => l.Visible).Select(l => l.IdObject).ToList()) : new List<dtoQuizInfo>()), idUser, viewStatBefore);
            }
            public Certification GetAvailableUserCertification(dtoSubActivity item, Int32 idCommunity, Int32 idUser)
            {
                return GetAvailableUserCertification(GetAvailableCertifications(item, idCommunity,idUser));
            }
            private Certification GetAvailableUserCertification(List<Certification> items)
            {
                return items.Where(c => c.Deleted == BaseStatusDeleted.None && c.Status == CertificationStatus.Valid).OrderByDescending(c => c.Id).FirstOrDefault();
            }
            private List<Certification> GetAvailableCertifications(dtoSubActivity item, Int32 idCommunity, Int32 idUser)
            {
                ModuleObject source = ModuleObject.CreateLongObject(item.Id, (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity, idCommunity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex);
                source.ServiceID= Service.ServiceModuleID();
                source.FQN = typeof(SubActivity).FullName;
                return ServiceCertifications.GetUserCertifications(source, idUser,true );
            }

        #region "Fill Data Into Template"
            public lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template FillDataIntoTemplate(long idSubActivity, String basePath, String istanceName, ref CertificationError cError)
            {
                return FillDataIntoTemplate(View.IdCommunityContainer, View.ForUserId, View.IdPath, idSubActivity, basePath, View.IdTemplate, View.IdTemplateVersion, istanceName, ref cError);
            }
            public lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template FillDataIntoTemplate(Int32 idCommunity, Int32 idUser, long idPath, long idSubActivity, String basePath, long idTemplate, long idVersion, String istanceName, ref CertificationError cError)
            {
                lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template = ServiceTemplates.TemplateGet(idTemplate, idVersion, basePath);
                if (template != null)
                {
                    Person person = CurrentManager.GetPerson(idUser);
                    liteCommunity community = CurrentManager.GetLiteCommunity(idCommunity);
                    liteSubscriptionInfo subscription = CurrentManager.GetLiteSubscriptionInfo(idUser, idCommunity);
                    String organization = "";
                    SubActivity subActivity = Service.GetSubActivity(idSubActivity);
                    if (community != null)
                        organization = CurrentManager.GetOrganizationName(community.IdOrganization);
                    List<String> pHolders = TemplateEduPathPlaceHolders.PlaceHolders().Values.Select(v=>v).ToList();
                    pHolders.AddRange(lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.PlaceHolders().Values.Select(v => v).ToList());
                    if (template.Modules != null && template.Modules.Where(m => m.IdModule == Service.ServiceModuleID()).Any() && subActivity != null)
                    {
                        // DEVO RIEmPIRE I PLACE HOLDERS !
                        List<lm.Comol.Modules.EduPath.Domain.DTO.dtoSubActivityLink> links = Service.GetDtoSubactivityActiveLinks(idSubActivity);
                        if (links == null || links.Count == 0)
                            template.Body.Text = TemplateEduPathPlaceHolders.Translate(template.Body.Text, AppContext, idPath, idUser, subActivity);
                        else
                            template.Body.Text = TemplateEduPathPlaceHolders.Translate(template.Body.Text, AppContext, idPath, idUser, subActivity, View.GetQuizInfos(links.Where(l => l.Visible).Select(l => l.IdObject).ToList()));
                    }
                    Int32 idLanguage = (person == null) ? UserContext.Language.Id : person.LanguageID;
                    template.Body.Text = lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.Translate(template.Body.Text, person, community, (subscription == null) ? null : subscription.SubscribedOn, organization, CurrentManager.GetTranslatedRole(subscription.IdRole, idLanguage), CurrentManager.GetTranslatedProfileType((person == null) ? (int)UserTypeStandard.Guest : person.TypeID, idLanguage), istanceName);
                    Int32 missingPlaceHolders = pHolders.Where(p => template.Body.Text.Contains(p)).Count();
                    cError = (missingPlaceHolders==0 ) ? CertificationError.None : ((missingPlaceHolders==1) ? CertificationError.EmptyTemplateItem: CertificationError.EmptyTemplateItems);
                }
                return template;
            }
        #endregion
           
        #region "Funzioni Invariate"
            public Boolean ExecuteAction(long idPath, long idSubActivity, StatusStatistic status)
            {
                return Service.ExecuteSubActivityInternal(idSubActivity, UserContext.CurrentUserID, status);
            }
            public Boolean IsEvaluablePath(long idPath)
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
            public String GetUserCertificationFileName(String filename)
            {
                Person p = CurrentManager.GetPerson(View.ForUserId);
                if (p != null)
                    return ReplaceInvalidFileName(string.Format(filename, "_" + p.SurnameAndName));
                else
                    return ReplaceInvalidFileName(string.Format(filename, ""));
            }
            public String ReplaceInvalidFileName(String filename)
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