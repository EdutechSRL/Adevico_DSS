using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class SelectUsersForMessageService : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {

        #region "Initializer"
            private int _ModuleID;
            private ServiceCallOfPapers service;
            private lm.Comol.Core.Mail.Messages.MailMessagesService messageservice;
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService profileservice;

            private int ModuleID
            {
                get
                {
                    if (_ModuleID <= 0)
                    {
                        _ModuleID = this.Service.ServiceModuleID();
                    }
                    return _ModuleID;
                }
            }
            public virtual ManagerRequestForMemebership CurrentManager { get; set; }

            protected virtual IViewSelectUsersForMessageService View
            {
                get { return (IViewSelectUsersForMessageService)base.View; }
            }
            private ServiceCallOfPapers Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceCallOfPapers(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.Mail.Messages.MailMessagesService MessageService
            {
                get
                {
                    if (messageservice == null)
                        messageservice = new lm.Comol.Core.Mail.Messages.MailMessagesService(AppContext);
                    return messageservice;
                }
            }
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (profileservice == null)
                        profileservice = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext);
                    return profileservice;
                }
            }
            public SelectUsersForMessageService(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new ManagerRequestForMemebership(oContext);
            }
            public SelectUsersForMessageService(iApplicationContext oContext, IViewSelectUsersForMessageService view)
                : base(oContext, view)
            {
                this.CurrentManager = new ManagerRequestForMemebership(oContext);
            }
        #endregion

        public void InitView(CallForPaperType type, List<Int32> removeUsers, Boolean fromPortal, Int32 idCommunity, ModuleObject obj, long idTemplate, long idVersion, Boolean isTemplateCompliant, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = null)
        {
            View.WasGridInitialized = false;
            View.RemoveUsers = removeUsers;
            View.CallType = type;
            View.LoadedNoUsers = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                InitializeFilters(type, removeUsers,fromPortal, idCommunity, obj, idTemplate, idVersion, isTemplateCompliant, translations);
            View.isInitialized = true; 
        }

        public void InitializeFilters(CallForPaperType type, List<Int32> removeUsers, Boolean fromPortal, Int32 idCommunity, ModuleObject obj, long idTemplate, long idVersion, Boolean isTemplateCompliant, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = null)
        {
            View.IdCommunityContainer = idCommunity;
            View.CodeModuleContainer = (type == CallForPaperType.CallForBids) ? ModuleCallForPaper.UniqueCode : ModuleRequestForMembership.UniqueCode;
            View.IdModuleContainer= (type== CallForPaperType.CallForBids) ? Service.ServiceModuleID() : CurrentManager.GetModuleID(ModuleRequestForMembership.UniqueCode);
            View.CurrentObject = obj;
            View.CurrentOrderBy = UserByMessagesOrder.ByUser;
            View.Ascending = true;
            dtoUsersByMessageFilter filter = InitializeFilter(obj, translations);
            View.InitializeMessagesSelector(fromPortal,idCommunity,(type== CallForPaperType.CallForBids) ? ModuleCallForPaper.UniqueCode : ModuleRequestForMembership.UniqueCode, (type== CallForPaperType.CallForBids) ? CurrentManager.GetModuleID(ModuleCallForPaper.UniqueCode) : CurrentManager.GetModuleID(ModuleRequestForMembership.UniqueCode), obj);
            switch (filter.Access) { 
                case AccessTypeFilter.NoSubmitters:
                    InitializeAgencyFilter(filter, obj);
                    View.DisplayNoSubmittersFilter();
                    View.IsInitializedForNoSubmitters = true;
                    break;
                case AccessTypeFilter.Submitters:
                    InitializeSubmittersFilter(filter,obj);
                    View.DisplaySubmittersFilter();
                    View.IsInitializedForSubmitters = true;
                    break;
            }
            List<lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy> items = new List<Core.BaseModules.ProfileManagement.SearchProfilesBy>();
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.All);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Name);
            items.Add(Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail);

            View.LoadSearchProfilesBy(items, filter.SearchBy);
            View.CurrentFilter = filter;
            View.InitializeWordSelector(new List<String>());
            View.DisplayNoUsersFound();
        }
        private dtoUsersByMessageFilter InitializeFilter(ModuleObject obj, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = null)
        {
            List<AccessTypeFilter> accessTypes = new List<AccessTypeFilter>();
            Boolean allowAvailableSubmitters = true;
            if (translations != null)
                allowAvailableSubmitters = !TemplatePlaceHolders.HasUserValues(translations.Select(t => t.Translation.Subject).ToList(), translations.Select(t => t.Translation.Body).ToList());
            if (allowAvailableSubmitters)
                accessTypes.Add(AccessTypeFilter.NoSubmitters);
            if (Service.CallHasSubmissions(obj.ObjectLongID) || accessTypes.Count == 0)
                accessTypes.Add(AccessTypeFilter.Submitters);
            View.HasUserValues = !allowAvailableSubmitters;
            dtoUsersByMessageFilter filter = new dtoUsersByMessageFilter() { Ascending = true, OrderBy = UserByMessagesOrder.ByUser };
            View.Ascending = true;
            View.CurrentOrderBy = UserByMessagesOrder.ByUser;
            filter.Access = ((allowAvailableSubmitters) ? AccessTypeFilter.NoSubmitters : (accessTypes.Contains(AccessTypeFilter.Submitters) ? AccessTypeFilter.Submitters : accessTypes[0]));
            View.LoadAccessType(accessTypes, filter.Access);
            return filter;
        }
        private void InitializeSubmittersFilter(dtoUsersByMessageFilter filter,ModuleObject obj) {
            List<SubmissionFilterStatus> statusList = Service.GetAvailableSubmissionStatusForPersonAssignments(obj.ObjectLongID, View.RemoveUsers);
            SubmissionFilterStatus dStatus = (!statusList.Any() || statusList.Count > 1) ? SubmissionFilterStatus.All : statusList[0];
            filter.Status = dStatus;
            View.LoadAvailableStatus(statusList, dStatus);
            Dictionary<long, string> submitters = LoadSubmitters(obj.ObjectLongID);
            long idSubmitterType = (submitters != null && submitters.Keys.Count == 1) ? submitters.Keys.FirstOrDefault() : -1;
            filter.IdSubmitterType = idSubmitterType;
            View.LoadSubmittersType(submitters, idSubmitterType);
            InitializeAgencyFilter(filter, obj);
        }
        private Dictionary<long, string> LoadSubmitters(long idCall)
        {
            return Service.GetCallSubmittersTypeForPersonAssignments(idCall);
        }
        private void InitializeAgencyFilter(dtoUsersByMessageFilter filter, ModuleObject obj,long idDefaultAgency = 0)
        {
            Boolean hasAgencies = false;
            Dictionary<long, String> agencies = null;
            long idCall = (obj==null)? (long)0 : obj.ObjectLongID;
            switch(filter.Access){
                case AccessTypeFilter.NoSubmitters:
                    hasAgencies = Service.HasAssignmentWithProfileType(idCall, (int)UserTypeStandard.Employee);
                    if (hasAgencies)
                        agencies = Service.GetAgenciesForAssignments(idCall);
                    break;
                case AccessTypeFilter.Submitters:
                    hasAgencies = Service.HasSubmissionsWithProfileType(idCall, (int)UserTypeStandard.Employee);
                    if (hasAgencies)
                        agencies = Service.GetAgenciesForSubmitters(idCall);
                    break;
            }
            if (hasAgencies && agencies != null && agencies.Count > 0)
            {
                View.LoadAgencies(agencies, idDefaultAgency);
                LoadColumns(filter.Access, true);
            }
            else
            {
                View.UnLoadAgencies();
                LoadColumns(filter.Access, false);
            }

        }

        public void ChangeAccessFilter(AccessTypeFilter access) {
            dtoUsersByMessageFilter filter = View.SelectedFilter;
            switch (access) { 
                case  AccessTypeFilter.NoSubmitters:
                    InitializeAgencyFilter(filter, View.CurrentObject, View.SelectedIdAgency);
                    View.IsInitializedForNoSubmitters = true;
                    View.DisplayNoSubmittersFilter();
                    //LoadItems(filter, 0, View.UsersPageSize,false);
                    break;
                case AccessTypeFilter.Submitters:
                    UpdateFiltersForSubmitters(filter, View.CurrentObject, View.SelectedIdAgency);
                    View.DisplaySubmittersFilter();
                    View.IsInitializedForSubmitters = true;
                    break;
            }
        }
        private void UpdateFiltersForSubmitters(dtoUsersByMessageFilter filter, ModuleObject obj,long idSelectedAgency)
        {
            InitializeAgencyFilter(filter, obj, idSelectedAgency);
            if (!View.IsInitializedForSubmitters)
            {
                List<SubmissionFilterStatus> statusList = Service.GetAvailableSubmissionStatusForPersonAssignments(obj.ObjectLongID, View.RemoveUsers);
                SubmissionFilterStatus dStatus = (!statusList.Any() || statusList.Count > 1) ? SubmissionFilterStatus.All : statusList[0];
                filter.Status = dStatus;
                View.LoadAvailableStatus(statusList, dStatus);

                Dictionary<long, string> submitters = LoadSubmitters(obj.ObjectLongID);
                long idSubmitterType = (submitters != null && submitters.Keys.Count == 1) ? submitters.Keys.FirstOrDefault() : -1;
                filter.IdSubmitterType = idSubmitterType;
                View.LoadSubmittersType(submitters, idSubmitterType);
            }
        }
        public List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> GetSelectedRecipients()
        {
            List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> items = new List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient>();
            List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> sItems = UpdateItemsSelection();
            View.SelectedItems = sItems;

            dtoUsersByMessageFilter filter = View.CurrentFilter;
            filter.StartWith = View.CurrentStartWith;
            items = Service.GetSelectedUsers(View.CurrentObject, View.RemoveUsers, filter,  View.SelectAll, sItems, MessageService, ProfileService);
            return items;
        }
        public void EditItemsSelection(Boolean selectAll)
        {
            View.SelectAll = selectAll;
            View.SelectedItems = (selectAll) ? UpdateItemsSelection().Distinct().ToList() : new List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem>();
        }
        public void LoadItems(dtoUsersByMessageFilter filter, Int32 pageIndex, Int32 pageSize, Boolean initialize)
        {
            View.WasGridInitialized = true;
            List<dtoCallMessageRecipient> recipients = Service.GetAllUsers(View.UnknownUserTranslation, View.AnonymousUserTranslation, View.CurrentObject, View.RemoveUsers, filter, View.HasUserValues, MessageService, ProfileService);
            filter.StartWith = View.CurrentStartWith;
            if (initialize)
                View.InitializeWordSelector(recipients.Select(r=>r.FirstLetter).Distinct().ToList());
            else
                View.InitializeWordSelector(recipients.Select(r => r.FirstLetter).Distinct().ToList(), filter.StartWith);
            recipients = Service.GetUsers(ProfileService,recipients, filter);

            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = (recipients.Count > 0) ? recipients.Count - 1 : 0;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex

            View.Pager = pager;
            View.SelectedItems= UpdateItemsSelection();
            recipients = recipients.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            View.LoadedNoUsers = !recipients.Any();
            if (recipients.Any())
                View.LoadUsers(recipients);
            else
                View.DisplayNoUsersFound();
        }

        private void LoadColumns(AccessTypeFilter accessType, Boolean hasAgencies) {
            List<ColumnMessageGrid> columns = new List<ColumnMessageGrid>();
            if (accessType == AccessTypeFilter.Submitters)
            {
                columns.Add(ColumnMessageGrid.SubmissionStatus);
                columns.Add(ColumnMessageGrid.SubmissionType);
                columns.Add(ColumnMessageGrid.LastActionOn);
            }
            if (hasAgencies)
                columns.Add(ColumnMessageGrid.AgencyName);
            View.AvailableColumns = columns;
        }
        private List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> UpdateItemsSelection()
        {
            // SELECTED ITEMS
            List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> items = View.SelectedItems;
            List<dtoSelectItem<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem>> cSelectedItems = View.GetCurrentSelectedItems();

            // REMOVE ITEMS
            items = items.Where(i => !cSelectedItems.Where(si => !si.Selected && si.Id.IdModuleObject == i.IdModuleObject && si.Id.IdPerson == i.IdPerson && si.Id.IdUserModule == i.IdUserModule).Any()).ToList();
            // ADD ITEMS
            items.AddRange(cSelectedItems.Where(si => si.Selected && !items.Where(i =>  si.Id.IdModuleObject == i.IdModuleObject && si.Id.IdPerson == i.IdPerson && si.Id.IdUserModule == i.IdUserModule).Any()).Select(si => si.Id).Distinct().ToList());
            return items;
        }
    }
}