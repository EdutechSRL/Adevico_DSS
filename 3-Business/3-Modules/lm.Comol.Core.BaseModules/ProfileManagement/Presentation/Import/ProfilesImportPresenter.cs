using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement.Business;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using System.Globalization;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class ProfilesImportPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        public static DateTime DefaultBirthDate = new DateTime(2999, 1, 1);

         #region "Initialize"
            private int _ModuleID;
            private InternalAuthenticationService _InternalService;
            private UrlAuthenticationService _UrlService;
            private ProfileManagementService _Service;
            private ServiceCommunityManagement _ServiceCommunity;

            private UrlAuthenticationService UrlService
            {
                get
                {
                    if (_UrlService == null)
                        _UrlService = new UrlAuthenticationService(AppContext);
                    return _UrlService;
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
            protected virtual IViewProfilesImport View
            {
                get { return (IViewProfilesImport)base.View; }
            }
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            private ServiceCommunityManagement ServiceCommunity
            {
                get
                {
                    if (_ServiceCommunity == null)
                        _ServiceCommunity = new ServiceCommunityManagement(AppContext);
                    return _ServiceCommunity;
                }
            }
            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(AppContext);
                    return _InternalService;
                }
            }
            public ProfilesImportPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfilesImportPresenter(iApplicationContext oContext, IViewProfilesImport view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.EditProfile || module.Administration || module.ViewProfiles);
                if (module.AddProfile || module.Administration)
                {
                    View.PreviousStep = ProfileImportStep.None;
                    View.ImportIdentifier = System.Guid.NewGuid();
                    InitializeSteps();
                    View.GotoStep(ProfileImportStep.SelectSource, true);
                }
                else
                    View.DisplayNoPermission();
            }
        }

        private void InitializeSteps() {
            List<ProfileImportStep> steps = new List<ProfileImportStep>();
            steps.Add(ProfileImportStep.SelectSource);
            steps.Add(ProfileImportStep.SourceCSV);
            steps.Add(ProfileImportStep.SourceRequestForMembership);
            steps.Add(ProfileImportStep.SourceCallForPapers);
            steps.Add(ProfileImportStep.FieldsMatcher);
            steps.Add(ProfileImportStep.ItemsSelctor);
            steps.Add(ProfileImportStep.SelectOrganizations);
            steps.Add(ProfileImportStep.SelectCommunities);
            steps.Add(ProfileImportStep.SubscriptionsSettings);
            steps.Add(ProfileImportStep.MailTemplate);
            steps.Add(ProfileImportStep.Summary);

            View.AvailableSteps = steps;

            List<ProfileImportStep> stepsToSkip = new List<ProfileImportStep>();
            stepsToSkip.Add(ProfileImportStep.SourceRequestForMembership);
            stepsToSkip.Add(ProfileImportStep.SourceCallForPapers);
            
            View.SkipSteps = stepsToSkip;       
        }

        public void MoveToNextStep(ProfileImportStep step)
        {
            switch (step)
            {
                case ProfileImportStep.SelectSource:
                    MoveFromSourceSelector();
                    break;
                case ProfileImportStep.SourceCSV:
                case ProfileImportStep.SourceRequestForMembership:
                case ProfileImportStep.SourceCallForPapers:
                    MoveFromSelectedSource();
                    break;
               
                   // MoveFromSelectedSource();
                   //// MoveToSummary();
                   // break;
                case ProfileImportStep.FieldsMatcher:
                    MoveFromFieldsMatcher();
                    break;
                case ProfileImportStep.ItemsSelctor:
                    MoveFromItemsSelector();
                    break;
                case ProfileImportStep.SelectOrganizations:
                    MoveFromOrganizationsSelector();
                    break;
                case ProfileImportStep.SelectCommunities:
                    MoveFromCommunitiesSelector();
                    break;
                case ProfileImportStep.SubscriptionsSettings:
                    MoveFromSubscriptionsSettings();
                    break;
                case ProfileImportStep.MailTemplate:
                    MoveFromMailTemplate();
                    break;
                case ProfileImportStep.Summary:
                    break;
            }
        }
        public void MoveToPreviousStep(ProfileImportStep step)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                switch (step)
                {
                    case ProfileImportStep.SourceCSV:
                    case ProfileImportStep.SourceRequestForMembership:
                    case ProfileImportStep.SourceCallForPapers:
                        View.GotoStep(ProfileImportStep.SelectSource);
                        break;
                    //case ProfileImportStep.SourceRequestForMembership:
                    //    View.GotoStep(ProfileImportStep.SelectSource);
                    //    break;
                    case ProfileImportStep.FieldsMatcher:
                        switch( View.CurrentSource){
                            case SourceType.FileCSV:
                                View.GotoStep(ProfileImportStep.SourceCSV);
                                break;
                            case SourceType.RequestForMembership:
                                View.GotoStep(ProfileImportStep.SourceRequestForMembership);
                                break;
                            case SourceType.CallForPapers:
                                View.GotoStep(ProfileImportStep.SourceCallForPapers);
                                break;
                            default:
                                break;

                        }
                        break;
                    case ProfileImportStep.ItemsSelctor:
                        View.GotoStep(ProfileImportStep.FieldsMatcher);
                        break;
                    case ProfileImportStep.SelectOrganizations:
                        View.GotoStep(ProfileImportStep.ItemsSelctor);
                        break;
                    case ProfileImportStep.SelectCommunities:
                        View.GotoStep(ProfileImportStep.SelectOrganizations);
                        break;
                    case ProfileImportStep.SubscriptionsSettings:
                        View.GotoStep(ProfileImportStep.SelectCommunities);
                        break;
                    case ProfileImportStep.MailTemplate:
                        View.GotoStep(ProfileImportStep.SubscriptionsSettings);
                        break;
                    case ProfileImportStep.Summary:
                        View.GotoStep(ProfileImportStep.MailTemplate);
                        break;
                    case ProfileImportStep.ImportCompleted:
                        if (View.ItemsToSelect>0 && View.PreviousStep != ProfileImportStep.None)
                            View.GotoStep(View.PreviousStep);
                        else
                            View.GotoStep(ProfileImportStep.SelectSource, true);
                        View.PreviousStep = ProfileImportStep.None;
                        break;
                    case ProfileImportStep.ImportWithErrors:
                        if (View.ItemsToSelect > 0 && View.PreviousStep != ProfileImportStep.None)
                            View.GotoStep(View.PreviousStep);
                        else
                            View.GotoStep(ProfileImportStep.SelectSource, true);
                        View.PreviousStep = ProfileImportStep.None;
                        break;
                    case ProfileImportStep.Errors:
                        View.GotoStep(ProfileImportStep.Summary);
                        break;
                }
            }
        }

        private void MoveFromSourceSelector()
        {
            SourceType source = View.CurrentSource;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (source != SourceType.None)
            {
                UpdateStepsToSkip(ProfileImportStep.SourceCallForPapers, (source != SourceType.CallForPapers));
                UpdateStepsToSkip(ProfileImportStep.SourceRequestForMembership, (source!= SourceType.RequestForMembership));
                UpdateStepsToSkip(ProfileImportStep.SourceCSV, (source != SourceType.FileCSV));
                switch (View.CurrentSource) { 
                    case SourceType.FileCSV:
                        View.GotoStep(ProfileImportStep.SourceCSV, true);
                        break;
                    case SourceType.RequestForMembership:
                        View.GotoStep(ProfileImportStep.SourceRequestForMembership, true);
                        break;
                    case SourceType.CallForPapers:
                        View.GotoStep(ProfileImportStep.SourceCallForPapers, true);
                        break;
                    default:
                        break;
                }
            }
        }
        private void MoveFromSelectedSource()
        {
            SourceType source = View.CurrentSource;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (source != SourceType.None)
            {
                List<ProfileColumnComparer<String>> columns = View.AvailableColumns(source);
                if (columns.Count > 0) {
                    View.InitializeFieldsMatcher(columns);
                    View.GotoStep(ProfileImportStep.FieldsMatcher, false);
                }
            }
        }
        private void MoveFromFieldsMatcher()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.ValidDestinationFields)
            {
                View.GotoStep(ProfileImportStep.ItemsSelctor, true);
            }
        }
        private void MoveFromItemsSelector()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.HasSelectedItems)
            {
                if (!View.IsInitialized(ProfileImportStep.SelectOrganizations))
                    View.InitializeStep(ProfileImportStep.SelectOrganizations);
                if (View.AvailableOrganizationsId.Count == 1 && View.PrimaryOrganizationId > 0)
                {
                    UpdateStepsToSkip(ProfileImportStep.SelectOrganizations, true);
                    MoveToNextStep(ProfileImportStep.SelectOrganizations);
                }
                else
                {
                    View.GotoStep(ProfileImportStep.SelectOrganizations);
                    UpdateStepsToSkip(ProfileImportStep.SelectOrganizations, false);
                }
            }
        }
        private void MoveFromOrganizationsSelector()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.PrimaryOrganizationId>0)
            {
                List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> organizations = ServiceCommunity.GetOrganizationNodes(View.AllOrganizationsId);
                View.OrganizationsNodes = organizations;
                if (!View.IsInitialized(ProfileImportStep.SelectCommunities))
                    View.GotoStep(ProfileImportStep.SelectCommunities, true);
                else {
                    List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> items = View.SelectedCommunities();
                    List<String> avaliablePath = new List<String>();
                    organizations.ForEach(o=> avaliablePath.AddRange(o.PathsToList()));
                    items = items.Where(i => i.ContainsPath(avaliablePath)).ToList(); ;
                    if (items.Count == 0)
                        View.GotoStep(ProfileImportStep.SelectCommunities, true);
                    else{
                        View.UpdateSelectedCommunities(items.Select(i=>i.Community.Id).ToList());
                        View.GotoStep(ProfileImportStep.SelectCommunities, false);
                    }
                }
            }
        }
        private void MoveFromCommunitiesSelector()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.PrimaryOrganizationId > 0)
            {
                View.GotoStep(ProfileImportStep.SubscriptionsSettings, true);
            }
        }
        private void MoveFromSubscriptionsSettings()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.HasAvailableSubscriptions && View.SelectedSubscriptions().Count > 0)
            {
                View.GotoStep(ProfileImportStep.MailTemplate, true);
            }
        }
        private void MoveFromMailTemplate()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else 
            {
                Boolean sendMail = View.SendMailToUsers;
                lm.Comol.Core.Mail.dtoMailContent mailContent= View.MailContent;
                if (!sendMail || (sendMail && View.MailContent != null && !string.IsNullOrEmpty(mailContent.Body )))
                    View.GotoStep(ProfileImportStep.Summary, true);
            }
        }
        //private void MoveFromCommunities() {
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else if (View.HasAvailableCommunities == true)
        //    {
        //        List<dtoBaseCommunityNode> nodes = View.SelectedCommunities();
        //        Boolean hasSubscriptions = (nodes.Count != 0);
        //        Boolean hasUnsubscriptions = false;
        //        if (View.CurrentAvailability== CommunityAvailability.All || View.CurrentAvailability== CommunityAvailability.Subscribed)
        //            hasUnsubscriptions = ServiceCommunity.ProfileHasCommunityToUnsubscribe(View.IdProfile, View.CommunityFilters, (nodes.Select(n => n.Id).ToList()));
        //        UpdateStepsToSkip(CommunitySubscriptionWizardStep.SubscriptionsSettings, !hasSubscriptions);
        //        UpdateStepsToSkip(CommunitySubscriptionWizardStep.RemoveSubscriptions, !hasUnsubscriptions);

        //        if (hasSubscriptions)
        //            View.GotoStep(CommunitySubscriptionWizardStep.SubscriptionsSettings, true);
        //        else if (hasUnsubscriptions){
        //            SetupUnsubscriptions();
        //            View.GotoStep(CommunitySubscriptionWizardStep.RemoveSubscriptions);
        //        };
        //    }
        //}

        //private void MoveFromSubscriptions()
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else if (View.HasAvailableSubscriptions == true)
        //    {
        //        if (View.SkipSteps.Contains(CommunitySubscriptionWizardStep.RemoveSubscriptions))
        //            MoveToSummary();
        //        else if (View.HasUnsubscriptions)
        //            View.GotoStep(CommunitySubscriptionWizardStep.RemoveSubscriptions);
        //        else{
        //            SetupUnsubscriptions();
        //            View.GotoStep(CommunitySubscriptionWizardStep.RemoveSubscriptions);
        //        }
        //    }
        //}

        //private void SetupUnsubscriptions()
        //{
        //    Int32 idProfile = View.IdProfile;
        //    List<dtoBaseUserSubscription> unsubscriptions = ServiceCommunity.ProfileIdCommunitiesToUnsubscribe(idProfile,View.CommunityFilters,View.SelectedIdCommunities());
        //    List<dtoBaseCommunityNode> nodes = View.GetNodesById(unsubscriptions.Select(s=>s.IdCommunity).ToList());

        //    unsubscriptions.ForEach(s => s.Path = nodes.Where(n => n.Id == s.IdCommunity).Select(n => n.Path).ToList());
        //    View.InitializeUnsubscriptionControl(idProfile, unsubscriptions);
        //}

        //private void MoveToSummary()
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else if (View.HasAvailableSubscriptions || View.HasUnsubscriptions)
        //    {
        //        List<dtoBaseUserSubscription> toDelete = View.SelectedUnsubscriptions();
        //        List<dtoUserSubscription> subscriptions = View.SelectedSubscriptions();

        //        View.DisplaySummaryInfo((from s in subscriptions where s.isNew select s.CommunityName).ToList(), (from s in subscriptions where s.isToUpdate && s.isNew == false select s.CommunityName).ToList(), (from s in toDelete select s.CommunityName).ToList());
        //        View.GotoStep(CommunitySubscriptionWizardStep.Summary);
        //    }
        //}

        //public void SaveChanges(List<dtoUserSubscription> subscriptions, List<dtoBaseUserSubscription> toDelete)
        //{
        //    Int32 idProfile = View.IdProfile;
        //    Person person = CurrentManager.GetPerson(idProfile);
        //    if (person == null || UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else { 
        //        Person current = CurrentManager.GetPerson(UserContext.CurrentUserID);
        //        if (ServiceCommunity.UpdateUserSubscriptions(person, subscriptions, toDelete))
        //            View.UpdateUserSubscriptions(idProfile, person.SurnameAndName, current.SurnameAndName, (from s in subscriptions where s.isNew select s).ToList(), toDelete.Select(s => s.IdCommunity).ToList());
        //        else
        //            View.DisplayError();
        //    }
        //}
        public List<Int32> GetCommunityIdFromOrganization(List<Int32> idOrganizations) {

            return ServiceCommunity.GetOrganizationsCommunityId(idOrganizations);
        }

        private void UpdateStepsToSkip(ProfileImportStep step, Boolean add)
        {
            List<ProfileImportStep> toSkip = View.SkipSteps;
            if (add && !toSkip.Contains(step))
                toSkip.Add(step);
            else if (!add && toSkip.Contains(step))
                toSkip.Remove(step);
            View.SkipSteps = toSkip;
        }

        #region "Import"
            public void ImportProfiles(
                dtoImportSettings settings, 
                Int32 defaultIdOrganization, 
                List<Int32> allOrganizationsId, 
                ProfileExternalResource selectedItems, 
                List<dtoNewProfileSubscription> subscriptions, 
                Boolean sendMailToUsers, 
                lm.Comol.Core.Mail.dtoMailContent mailContent) 
            {
            
                List<ProfileImportAction> actions = new List<ProfileImportAction>();
                
                View.PreviousStep = ProfileImportStep.None;
                
                actions.Add(ProfileImportAction.Create);
                
                if (allOrganizationsId.Where(id => id != defaultIdOrganization).Any())
                    actions.Add(ProfileImportAction.AddToOtherOrganizations);
                
                actions.Add(ProfileImportAction.AddToCommunities);
                
                if (sendMailToUsers)
                    actions.Add(ProfileImportAction.SendMail);
                
                View.SetupProgressInfo(actions.Count, selectedItems.Rows.Count);

                // Create profiles
                List<dtoBaseProfile> notCreatedProfiles = new List<dtoBaseProfile>();
                List<dtoImportedProfile> createdProfiles = new List<dtoImportedProfile>();
                List<Person> profiles = CreateProfiles(settings, defaultIdOrganization, selectedItems, createdProfiles, notCreatedProfiles);
                Dictionary<Int32, String> notaddedToOrganizations = new Dictionary<Int32, String>();
                Dictionary<Int32, String> notSubscribedCommunities = new Dictionary<Int32, String>();
                List<dtoImportedProfile> notSentMail = new List<dtoImportedProfile>();
                
                if (profiles!= null && profiles.Count > 0)
                {
                    AddProfilesToOrganizations(defaultIdOrganization, allOrganizationsId, profiles, subscriptions.Where(s => s.IdCommunityType == 0).ToList(), notaddedToOrganizations);
                    AddProfilesToCommunities(actions, profiles, subscriptions.Where(s => s.IdCommunityType != 0).ToList(), notSubscribedCommunities);
                    if (sendMailToUsers)
                        SendMailToProfiles(actions,createdProfiles, mailContent, Service.GetProfileTypeMailTemplateAttributes(settings), notSentMail);
                }
                
                View.UpdateSourceItems(View.CurrentSource);
                
                Int32 itemsToSelect = View.ItemsToSelect;
                
                View.PreviousStep = (itemsToSelect > 0) ? ProfileImportStep.ItemsSelctor : ProfileImportStep.None;

                if (notCreatedProfiles.Count == 0 && notaddedToOrganizations.Count == 0 && notSubscribedCommunities.Count == 0 && notSentMail.Count == 0)
                {
                    View.isCompleted = (itemsToSelect == 0);
                    View.DisplayImportedProfiles(profiles.Count, itemsToSelect);
                }
                else
                    View.DisplayImportError(profiles.Count, notCreatedProfiles, notaddedToOrganizations, notSubscribedCommunities, notSentMail);   
            }
            #region "Profiles Add"
                private List<Person> CreateProfiles(dtoImportSettings settings, Int32 idDefaultOrganization, ProfileExternalResource selectedItems, List<dtoImportedProfile> createdProfiles, List<dtoBaseProfile> notCreatedProfiles)
                {
                    List<Person> profiles = new List<Person>();
                    Language language = CurrentManager.GetDefaultLanguage();
                    AuthenticationProvider provider = Service.GetAuthenticationProvider(settings.IdProvider);
                    if (provider != null && language != null) {
                        Boolean created = false;
                        Int32 idPerson = 0;
                        Int32 index = 1;
                        foreach (ProfileAttributesRow row in selectedItems.Rows)
                        {
                            dtoBaseProfile baseItem = null;
                            switch (settings.IdProfileType)
                            {
                                case (int)UserTypeStandard.ExternalUser:
                                    dtoExternal externalUser = CreateExternal(row);
                                    baseItem = externalUser;
                                    break;
                                case (int)UserTypeStandard.Company:
                                    dtoCompany company = CreateCompanyUser(row);
                                    baseItem = company;
                                    break;
                                case (int)UserTypeStandard.Employee:
                                    dtoEmployee employee = CreateEmployee(row);
                                    baseItem = employee;
                                    break;
                                default:
                                    baseItem = new dtoBaseProfile();
                                    break;
                            }
                            created = false;

                            if (baseItem != null) {
                                GenerateBaseProfile(baseItem, settings, row, provider.ProviderType, GetUserLanguage(row,language));
                                if (InternalService.isUniqueMail(baseItem.Mail))
                                {
                                    PersonInfo info = GeneratePersonInfo(row, baseItem);
                                    idPerson = View.AddUserProfile(baseItem, info, idDefaultOrganization, provider);
                                    if (idPerson > 0)
                                    {
                                        Person person = CurrentManager.GetPerson(idPerson);
                                        if (person != null)
                                        {
                                            created = AddAuthentication(person, baseItem, settings, row, provider);
                                            if (created)
                                                Service.SetDefaultProvider(provider.Id, person.Id);
                                            profiles.Add(person);
                                            createdProfiles.Add(new dtoImportedProfile() { Profile=baseItem, Info = info });
                                        }
                                    }
                                }
                                if (!created)
                                    notCreatedProfiles.Add(baseItem);
                                View.UpdateProfileCreation(0,index, created, baseItem.DisplayName);
                            }
                            else
                                View.UpdateProfileCreation(0, index, created, " // ");
                            index++;
                        }
                       
                    }
                    return profiles;
                }
                #region "Specialized"
                    private dtoExternal CreateExternal(ProfileAttributesRow row)
                    {
                        dtoExternal item = new dtoExternal();
                        item.ExternalUserInfo = row.GetCellValue(ProfileAttributeType.externalUserInfo);
                        return item;
                    }
                    private dtoCompany CreateCompanyUser(ProfileAttributesRow row)
                    {
                        dtoCompany item = new dtoCompany();
                        item.Info.Name = row.GetCellValue(ProfileAttributeType.companyName);
                        item.Info.Address = row.GetCellValue(ProfileAttributeType.companyAddress);
                        item.Info.City = row.GetCellValue(ProfileAttributeType.companyCity);
                        item.Info.Region = row.GetCellValue(ProfileAttributeType.companyRegion);
                        item.Info.TaxCode = row.GetCellValue(ProfileAttributeType.companyTaxCode);
                        item.Info.ReaNumber = row.GetCellValue(ProfileAttributeType.companyReaNumber);
                        item.Info.AssociationCategories = row.GetCellValue(ProfileAttributeType.companyAssociations);
                        return item;
                    }
                    private dtoEmployee CreateEmployee(ProfileAttributesRow row)
                    {
                        dtoEmployee item = new dtoEmployee();
                        item.CurrentAgency = row.GetCalculatedCellLongValue(ProfileAttributeType.agencyInternalCode);
                        return item;
                    }
                #endregion

                #region "Common"
                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="row">String row to analyze</param>
                    /// <param name="dLanguage">Default language</param>
                    /// <returns></returns>
                    private Language GetUserLanguage(ProfileAttributesRow row,Language dLanguage)
                    {
                        Language result = null;
                        String lCode = row.GetCellValue(ProfileAttributeType.language);
                        if (!String.IsNullOrEmpty(lCode)){
                            List<Language> languages = (from l in CurrentManager.GetIQ<Language>() where l.Code == lCode || l.Code.StartsWith(lCode + "-") select l).ToList();
                            if (languages.Count == 1 || languages.Count > 1)
                                result = languages.FirstOrDefault();
                        }
                        return (result == null) ? dLanguage : result;
                    }
                    private void GenerateBaseProfile(dtoBaseProfile profile, dtoImportSettings settings, ProfileAttributesRow row, AuthenticationProviderType type, Language language)
                    {
                        profile.AuthenticationProvider = type;
                        profile.IdDefaultProvider = settings.IdProvider;
                        profile.IdProfileType = settings.IdProfileType;
                        profile.IdLanguage = language.Id;
                        profile.LanguageName=language.Name;
                        profile.Mail = row.GetCellValue(ProfileAttributeType.mail).Trim();
                        profile.Name = row.GetCellValue(ProfileAttributeType.name).Trim();
                        profile.Surname = row.GetCellValue(ProfileAttributeType.surname).Trim();

                        profile.Sector = row.GetCellValue(ProfileAttributeType.sector);
                        profile.Job = row.GetCellValue(ProfileAttributeType.job);

                        if(profile.PersonInfo == null)
                            profile.PersonInfo = new PersonInfo();

                        string birthOn = row.GetCellValue(ProfileAttributeType.birthDate);


                        DateTime birthDate = DefaultBirthDate;
                        try
                        {
                            birthDate = System.DateTime.Parse(birthOn);
                        }
                        catch (Exception)
                        {
                            birthDate = DefaultBirthDate;
                        }
                        profile.PersonInfo.BirthDate = birthDate;


                        profile.PersonInfo.BirthPlace = row.GetCellValue(ProfileAttributeType.birthPlace);
                        
                        if (!String.IsNullOrEmpty(profile.Name))
                            profile.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(profile.Name);
                        if (!String.IsNullOrEmpty(profile.Surname))
                            profile.Surname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(profile.Surname);

                        if (type == AuthenticationProviderType.Internal)
                        {
                            profile.Password = (settings.AddPassword) ? row.GetCellValue(ProfileAttributeType.password) : lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, true, true, false);
                            profile.Login = (settings.AutoGenerateLogin) ? row.GetCellValue(ProfileAttributeType.autoGeneratedLogin) : row.GetCellValue(ProfileAttributeType.login);

                            if (string.IsNullOrEmpty(profile.Login))
                                profile.Login = InternalService.GenerateInternalLogin(profile.Name, profile.Surname);
                            else if (!InternalService.isInternalUniqueLogin(profile.Login))
                                profile.Login = InternalService.GenerateInternalLogin(profile.Name, profile.Surname);
                        }
                        else {
                            profile.Login = row.GetCellValue(ProfileAttributeType.externalId);
                        }
                        profile.ShowMail = false;
                        
                        if (!string.IsNullOrEmpty(profile.Surname))
                            profile.FirstLetter = profile.Surname[0].ToString().ToLower();

                        profile.TaxCode = row.GetCellValue(ProfileAttributeType.taxCode);
                        //if (settings.AddTaxCode)
                        //    profile.TaxCode = row.GetCellValue(ProfileAttributeType.taxCode);
                        if (String.IsNullOrEmpty(profile.TaxCode))
                            profile.TaxCode = InternalService.GenerateRandomTaxCode();

                        while(!InternalService.isUniqueTaxCode(profile.TaxCode)){
                            profile.TaxCode = InternalService.GenerateRandomTaxCode();
                        }
                    }
                    private PersonInfo GeneratePersonInfo(ProfileAttributesRow row, dtoBaseProfile baseItem)
                    {
                        
                        PersonInfo info = new PersonInfo();
                        if (baseItem.PersonInfo != null)
                            info = baseItem.PersonInfo;
                        else
                        {
                            info.BirthDate = new DateTime(2999,12,1);
                            info.BirthPlace = "";    
                        }

                        info.Address= row.GetCellValue(ProfileAttributeType.address);
                        
                        info.City= row.GetCellValue(ProfileAttributeType.city);
                        info.DefaultShowMailAddress=false;
                        info.Fax=row.GetCellValue(ProfileAttributeType.fax);
                        info.PostCode=row.GetCellValue(ProfileAttributeType.zipCode);
                        info.OfficePhone=row.GetCellValue(ProfileAttributeType.telephoneNumber);
                        info.Mobile =row.GetCellValue(ProfileAttributeType.mobile);
                        return info;
                    }
                    private Boolean AddAuthentication(Person person, dtoBaseProfile profile, dtoImportSettings settings, ProfileAttributesRow row ,AuthenticationProvider provider) {
                        Boolean result = false;
                        if (provider.ProviderType == AuthenticationProviderType.Internal)
                        {
                            InternalLoginInfo info = InternalService.GenerateUserInfo(person, profile.Login, profile.Password, (InternalAuthenticationProvider)provider,false );
                            result = (info != null);
                        }
                        else {
                            dtoExternalCredentials credentials = new dtoExternalCredentials();
                            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((int)provider.IdentifierFields, (int)IdentifierField.longField))
                            {
                                long identifierLong = 0;
                                long.TryParse(row.GetCellValue(ProfileAttributeType.externalId), out identifierLong);
                                credentials.IdentifierLong = identifierLong;
                            }
                            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((int)provider.IdentifierFields, (int)IdentifierField.stringField))
                                 credentials.IdentifierString=row.GetCellValue(ProfileAttributeType.externalId);

                            if (UrlService.VerifyDuplicateExternalLoginInfo(person, provider, credentials)== ProfilerError.none)
                            {
                                ExternalLoginInfo externaLogin = UrlService.AddExternalProfile(person, provider, credentials);
                                result = (externaLogin != null);
                            }
                        }
                        return result;
                    }
                #endregion

                public CompanyUser AddCompanyUser(CompanyUser profile, AuthenticationProvider provider)
                {
                    return Service.ImportCompanyUser(profile, provider);
                }
                public Employee AddEmployee(Employee profile, AuthenticationProvider provider)
                {
                    return Service.ImportEmployee(profile, provider);
                }
            #endregion

            #region "Organizations add"
                private void AddProfilesToOrganizations(Int32 defaultIdOrganization, List<Int32> allOrganizationsId, List<Person> profiles, List<dtoNewProfileSubscription> subscriptions, Dictionary<Int32, String> notCreatedOrganizations)
                {
                    Int32 index = 1;
                    Boolean created = false;
                    foreach (Int32 idOrganization in allOrganizationsId)
                    {
                        Organization organization =  CurrentManager.GetOrganization(idOrganization);
                        if (organization!=null){
                            List<LazySubscription> items = ServiceCommunity.AddProfilesToOrganization(idOrganization, profiles.Select(p=> p.Id).ToList(), subscriptions.Where(s => s.Node.Community.IdOrganization == idOrganization).FirstOrDefault(), (idOrganization == defaultIdOrganization));
                            created = (items.Count > 0);
                            if (!created)
                                notCreatedOrganizations.Add(idOrganization, organization.Name);
                            View.UpdateAddProfilesToOrganizations(1,allOrganizationsId.Count, index, created, organization.Name);
                        }
                        else
                            View.UpdateAddProfilesToOrganizations(1, allOrganizationsId.Count, index, false, subscriptions.Where(s => s.Node.Community.IdOrganization == idOrganization).FirstOrDefault().Name);
                        index++;
                    }
                }
            #endregion
            #region "Communities add"
                private void AddProfilesToCommunities(List<ProfileImportAction> actions, List<Person> profiles, List<dtoNewProfileSubscription> subscriptions, Dictionary<Int32, String> notSubscribedCommunities)
                {
                    Int32 index = 1;
                    Boolean created = false;
                    Int32 actionIndex = actions.Where(a => a != ProfileImportAction.SendMail).Count() - ((actions.Contains(ProfileImportAction.AddToOtherOrganizations)) ? 1 : 0);
                    foreach (dtoNewProfileSubscription subscription in subscriptions.OrderBy(s => s.Node.PathDepth).ToList())
                    {
                        List<LazySubscription> items = ServiceCommunity.AddProfilesToCommunity(profiles.Select(p=> p.Id).ToList(), subscription);
                        created = (items.Count > 0);
                        if (!created)
                            notSubscribedCommunities.Add(subscription.Id, subscription.Name);

                        View.UpdateAddProfilesToCommunities(actionIndex, subscriptions.Count, index, created, subscription.Name);
                        index++;
                    }
                }
            #endregion 
            #region "Send mail"
                private void SendMailToProfiles(List<ProfileImportAction> actions, List<dtoImportedProfile> profiles, lm.Comol.Core.Mail.dtoMailContent mailContent, List<ProfileAttributeType> attributes, List<dtoImportedProfile> notSentMail)
                {
                    Int32 index = 1;
                    Boolean mailSent = false;
                    Int32 actionIndex = actions.Count() - ((actions.Contains(ProfileImportAction.AddToOtherOrganizations) || actions.Contains(ProfileImportAction.AddToCommunities)) ? 1 : 0);
                    Person currentUser =  CurrentManager.GetPerson(UserContext.CurrentUserID);
                    lm.Comol.Core.Mail.MailService mailService = new lm.Comol.Core.Mail.MailService(View.CurrentSmtpConfig, mailContent.Settings);
                    Language dLanguage = CurrentManager.GetDefaultLanguage();
                    foreach (dtoImportedProfile profile in profiles)
                    {
                        if (currentUser!=null){
                            lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(mailContent.Subject, AnalyzeBody(mailContent.Body, profile, attributes));
                            message.FromUser = new System.Net.Mail.MailAddress(currentUser.Mail, currentUser.SurnameAndName);
                            message.To.Add(new System.Net.Mail.MailAddress(profile.Profile.Mail, profile.Profile.DisplayName));
                            //'dtoMessage.To.Add()
                            mailSent = (mailService.SendMail(currentUser.LanguageID, dLanguage,message) == Mail.MailException.MailSent);
                        }    
                        else
                            View.UpdateSendMailToProfile(actionIndex, profiles.Count, index, mailSent, profile.Profile.DisplayName);
                    
                        if (!mailSent)
                            notSentMail.Add(profile);
                        index++;
                    }
                }
                
                private String AnalyzeBody(String body, dtoImportedProfile dto, List<ProfileAttributeType> attributes){
                    String result = body;

                    foreach(ProfileAttributeType attribute in attributes){
                        String search = "[" + attribute.ToString() + "]";
                        switch(attribute){
                            case ProfileAttributeType.name:
                                result = result.Replace(search, dto.Profile.Name);
                                break;
                            case ProfileAttributeType.surname:
                                result = result.Replace(search, dto.Profile.Surname);
                                break;
                            case ProfileAttributeType.mail:
                                result = result.Replace(search, dto.Profile.Mail);
                                break;
                            case ProfileAttributeType.password:
                                result = result.Replace(search, dto.Profile.Password);
                                break;
                            case ProfileAttributeType.taxCode:
                                result = result.Replace(search, dto.Profile.TaxCode);
                                break;
                            case ProfileAttributeType.autoGeneratedLogin:
                                result = result.Replace("[" + ProfileAttributeType.login.ToString() + "]", dto.Profile.Login);
                                result = result.Replace("[" + ProfileAttributeType.autoGeneratedLogin.ToString() + "]", dto.Profile.Login);
                                break;
                            case ProfileAttributeType.login:
                                result = result.Replace("[" + ProfileAttributeType.login.ToString() + "]", dto.Profile.Login);
                                result = result.Replace("[" + ProfileAttributeType.autoGeneratedLogin.ToString() + "]", dto.Profile.Login);
                                break;
                            case ProfileAttributeType.externalUserInfo:
                                if (typeof(dtoExternal) == dto.Profile.GetType())
                                    result = result.Replace(search, ((dtoExternal)dto.Profile).ExternalUserInfo);
                                else
                                    result = result.Replace(search, "");
                                break;
                            case ProfileAttributeType.address:
                                result = result.Replace(search, dto.Info.Address);
                                break;
                            case ProfileAttributeType.city:
                                result = result.Replace(search, dto.Info.City);
                                break;
                            case ProfileAttributeType.fax:
                                result = result.Replace(search, dto.Info.Fax);
                                break;
                            case ProfileAttributeType.mobile:
                                result = result.Replace(search, dto.Info.Mobile);
                                break;
                            case ProfileAttributeType.zipCode:
                                result = result.Replace(search, dto.Info.PostCode);
                                break;
                            case ProfileAttributeType.telephoneNumber:
                                result = result.Replace(search, dto.Info.OfficePhone);
                                break;
                            case ProfileAttributeType.language:
                                result = result.Replace(search, dto.Profile.LanguageName);
                                break;
                        }
                    }
                    if (typeof(dtoCompany) == dto.Profile.GetType()){
                        dtoCompany company = (dtoCompany)dto.Profile;
                        result = result.Replace("[" + ProfileAttributeType.companyCity.ToString() + "]", company.Info.City);
                        result = result.Replace("[" + ProfileAttributeType.companyName.ToString() + "]", company.Info.Name);
                        result = result.Replace("[" + ProfileAttributeType.companyAddress.ToString() + "]", company.Info.Address);
                        result = result.Replace("[" + ProfileAttributeType.companyRegion.ToString() + "]", company.Info.Region);
                        result = result.Replace("[" + ProfileAttributeType.companyTaxCode.ToString() + "]", company.Info.TaxCode);
                    }
                    return result;
                }
           #endregion 
         
        #endregion
    }
}