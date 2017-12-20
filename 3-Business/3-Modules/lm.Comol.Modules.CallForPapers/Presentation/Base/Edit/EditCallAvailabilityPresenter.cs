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
    public class EditCallAvailabilityPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditCallAvailability View
            {
                get { return (IViewEditCallAvailability)base.View; }
            }
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement _ServiceCommunityManagement;
            private ServiceRequestForMembership _ServiceRequest;
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
            private ServiceRequestForMembership RequestService
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement CommunityService
            {
                get
                {
                    if (_ServiceCommunityManagement == null)
                        _ServiceCommunityManagement = new lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(AppContext);
                    return _ServiceCommunityManagement;
                }
            }
        public EditCallAvailabilityPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditCallAvailabilityPresenter(iApplicationContext oContext, IViewEditCallAvailability view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView()
        {
            long idCall = View.PreloadIdCall;

            dtoBaseForPaper call = null;
            CallForPaperType type =  CallService.GetCallType(idCall); 
            if (type== CallForPaperType.None)
                type = View.PreloadType;
            call = CallService.GetDtoBaseCall(idCall);

            View.CallType = type;
            View.CurrentAction = CallStandardAction.Edit;

            int idCommunity = SetCallCurrentCommunity(call);
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                litePerson currenUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                Boolean allowManage = false;
                Boolean allowView = false;
                Boolean allowSave = false;
                switch (type) { 
                    case CallForPaperType.CallForBids:
                        ModuleCallForPaper module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowView = (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                        allowManage = module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper;
                        allowSave =  ( module.Administration || module.ManageCallForPapers || (module.CreateCallForPaper && idCall==0) || (call!= null && module.EditCallForPaper && currenUser== call.Owner));
                        break;
                    case CallForPaperType.RequestForMembership:
                        ModuleRequestForMembership moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
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
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps =CallService.GetAvailableSteps(idCall, WizardCallStep.CallAvailability, type);
                    View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));


                    View.LoadWizardSteps(idCall, type, idCommunity, steps);
                    LoadSettings(call);
                    if (type == CallForPaperType.CallForBids)
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewCallAvailability);
                    else
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.ViewCallAvailability);

                    if (steps.Where(s => s.Id == WizardCallStep.SubmittersType && (s.Status == Core.Wizard.WizardItemStatus.valid || s.Status == Core.Wizard.WizardItemStatus.warning)).Any())
                        View.SetActionUrl(CallStandardAction.PreviewCall, RootObject.PreviewCall(type, idCall, idCommunity, View.PreloadView));
                }
                else
                    View.DisplayNoPermission(idCommunity, idModule);
            }
        }

        #region "Initialize View"
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
                //if (call!=null)
                //    idCommunity = (call.IsPortal) ? 0 : (call.Community !=null) ? call.Community.Id : 0;


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
            private void LoadSettings(dtoBaseForPaper call)
            {
                View.IsPublic = call.IsPublic;
                View.ForPortal = call.IsPortal;

                LoadAvailableItemsToAdd(call);
                List<dtoCallAssignment> assignments = CallService.GetCallAssignments(call.Id, call.IsPortal, View.GetTranslatedProfileTypes(), View.GetTranslatedRoles());
            
                LoadSkins(call);
                if (call.IsPublic)
                {
                    switch (call.Type)
                    {
                        case CallForPaperType.RequestForMembership:
                            View.DisplayPublicUrls(call.Type, RootObject.StartNewSubmission(call.Type, call.Id, true,false, CallStatusForSubmitters.None,-1), RootObject.PublicCollectorCalls(call.Type, call.Id , (call.IsPortal) ? 0 : View.IdCommunity));
                            break;
                        default:
                            View.DisplayPublicUrl(call.Type, RootObject.StartNewSubmission(call.Type, call.Id, true, false, CallStatusForSubmitters.None, -1));
                            break;
                    }
                }
                else
                    View.HidePublicUrl();

                if (assignments.Count == 0 && !call.IsPublic && !call.ForSubscribedUsers)
                {
                    View.SelectedIdUsers = new List<Int32>();
                    View.LoadAssignments(TranslateAndReorderAssignments(CallService.GetDefaultAssignmentSettings(call.Id, call.IsPortal, call.IsPublic)));
                }
                else
                {
                    List<dtoCallAssignment> items = TranslateAndReorderAssignments(assignments);
                    View.SelectedIdUsers = assignments.Where(a => a.Type == CallAssignmentType.Person).Select(a => ((dtoCallPersonAssignment)a).IdPerson).ToList();
                    View.LoadAssignments(items);
                }
            }
            private List<dtoCallAssignment> TranslateAndReorderAssignments(dtoCallAssignment assignment){
                List<dtoCallAssignment> assignments = new List<dtoCallAssignment>();
                if (assignment != null)
                    assignments.Add(assignment);
                return TranslateAndReorderAssignments(assignments);
            }
            private List<dtoCallAssignment> TranslateAndReorderAssignments(List<dtoCallAssignment> assignments)
            {
                List<dtoCallAssignment> reorderedItems = new List<dtoCallAssignment>();
                if (assignments.Where(a => a.Type == CallAssignmentType.Portal).Any()) {
                    ((dtoCallPortalAssignment)assignments.Where(a => a.Type == CallAssignmentType.Portal).FirstOrDefault()).DisplayName = View.Portalname;
                }
                Int32 index = -1;
                foreach(dtoCallCommunityAssignment cAssignment in assignments.Where(a=>a.Type== CallAssignmentType.Community).Select(a=> ((dtoCallCommunityAssignment)a)).Where(a=>a.IsUnknown).ToList()){
                    cAssignment.DisplayName = View.UnknownCommunityTranslation + " - " + index.ToString();
                    index--;
                }

                index = -1;
                foreach (dtoCallPersonAssignment pAssignment in assignments.Where(a => a.Type == CallAssignmentType.Person).Select(a => ((dtoCallPersonAssignment)a)).Where(a => a.IsUnknown).ToList())
                {
                    pAssignment.DisplayName = View.UnknownUserTranslation + " - " + index.ToString();
                    index--;
                }
                if (assignments.Where(a => a.Type == CallAssignmentType.Portal).Any())
                    reorderedItems.AddRange(assignments.Where(a => a.Type == CallAssignmentType.Portal).ToList());
                if (assignments.Where(a => a.Type == CallAssignmentType.Community).Any())
                    reorderedItems.AddRange(assignments.Where(a => a.Type == CallAssignmentType.Community).OrderByDescending(a => a.IsDefault).ThenByDescending(a => a.IsUnknown).ThenBy(a => a.DisplayName).ToList());
                if (assignments.Where(a => a.Type == CallAssignmentType.Person).Any())
                    reorderedItems.AddRange(assignments.Where(a => a.Type == CallAssignmentType.Person).OrderByDescending(a => a.IsUnknown).ThenBy(a => a.DisplayName).ToList());
                return reorderedItems;
            }
            private void LoadAvailableItemsToAdd(dtoBaseForPaper call)
            {
                List<CallAssignmentType> itemsToAdd = new List<CallAssignmentType>();
                if (call.IsPortal)
                    itemsToAdd.Add(CallAssignmentType.PersonType);
                else
                {
                    itemsToAdd.Add(CallAssignmentType.Person);
                }
                List<Int32> removeCommunities = CallService.GetIdCommunityAssignments(call.Id);
                removeCommunities.Add((call.IsPortal && call.Community == null) ? 0 : call.Community.Id);

                if (CommunityService.HasAvailableCommunitiesByModule(UserContext.CurrentUserID, true, removeCommunities))
                    itemsToAdd.Add(CallAssignmentType.Community);

                if (CallService.ExistCallWithSubmissions(call.Id, call.Type, View.IdCommunity, call.IsPortal))
                    itemsToAdd.Add(CallAssignmentType.SubmitterOfBaseForPaper);
                View.LoadAvailableAssignments(itemsToAdd);
            }
        #endregion

        #region "Person Type Assignments"
            public dtoCallPortalAssignment GetCallPortalAssignments(Dictionary<Int32, String> profileTypes)
            {
                return CallService.GetPortalAssignments(View.IdCall, profileTypes);
            }
            public void SaveSettings(Boolean forAllUsers, List<Int32> types)
            {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else if (types.Count == 0 && !forAllUsers && !CallService.HasCallUserAssignments(idCall))
                    View.DisplayNoAvailability();
                else
                {
                    Boolean saved = CallService.SavePortalAvailability(idCall, forAllUsers, types);
                    if (saved)
                    {
                        View.DisplayAvailabilitySaved();
                        LoadAssignments(idCall);
                    }
                    else
                        View.DisplaySaveErrors(!saved);
                }
            }
        #endregion
        #region "Community Assignments"
            public void SelectCommunityToAdd(){
                long idCall = View.IdCall;
                if (UserContext.isAnonymous )
                    View.DisplaySessionTimeout();
                else
                {
                    Dictionary<Int32, long> rPermissions = new Dictionary<Int32, long>();
                    Boolean forAdmin = (UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative);
                    Core.BaseModules.CommunityManagement.CommunityAvailability availability = (forAdmin) ? Core.BaseModules.CommunityManagement.CommunityAvailability.All : Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed;
                    long permissions = -1;
                    switch(View.CallType){
                        case CallForPaperType.CallForBids:
                            permissions = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.AddCall;
                            break;
                        case CallForPaperType.RequestForMembership:
                            permissions = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests | (long)ModuleRequestForMembership.Base2Permission.AddRequest;
                            break;
                    }
                    rPermissions.Add(View.IdCallModule, permissions);
                    rPermissions.Add(CommunityService.ServiceModuleID(), (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.Manage | (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.AdminService);
                    List<Int32> idCommunities = CallService.GetIdCommunityAssignments(idCall);
                    View.DisplayCommunityToAdd(UserContext.CurrentUserID, forAdmin, rPermissions, idCommunities, availability);
                }
            }
            public void AddCommunity(List<Int32> idCommunites)
            {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else if (idCommunites.Any())
                {
                    Boolean saved = CallService.AddCommunityAssignment(idCall, idCommunites);
                    if (saved)
                    {
                        if (idCommunites.Count > 1)
                            View.DisplayCommunityAssignmentsAdded();
                        else {
                            Community c = CurrentManager.GetCommunity(idCommunites.First());
                            if (c != null)
                                View.DisplayCommunityAssignmentAdded(c.Name);
                            else
                                View.DisplayCommunityAssignmentsAdded();
                        }
                        LoadAssignments(idCall);
                    }
                    else
                        View.DisplaySaveErrors(!saved);
                }
            }
            public void LoadCommunityAssignments(Int32 idCommunity,long idAssignment,Dictionary<Int32, String> roles)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    String cName = View.UnknownCommunityTranslation;
                    Community community = CurrentManager.GetCommunity(idCommunity);
                    if (community!=null)
                        cName= community.Name;
                    View.SelectedIdUsers = GetUpdatedUsersSelection();
                    View.LoadCommunityAssignment(CallService.GetCommunityAssignments(View.IdCall, idCommunity, idAssignment, roles), cName, idCommunity, CommunityService.GetCommunityAvailableIdRoles(community));
                }
            }
            public void SaveSettings(Int32 idCommunity,Boolean forAllUsers, List<Int32> roles)
            {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else if (roles.Count == 0 && !forAllUsers && !CallService.HasCallUserAssignments(idCall))
                    View.DisplayNoAvailability();
                else
                {
                    String name = "";
                    Community c = CurrentManager.GetCommunity(idCommunity);
                    if (c != null)
                        name = c.Name;
                    else if (idCommunity > 0)
                        name = View.UnknownCommunityTranslation;
                    else
                        name = View.Portalname;

                    Boolean saved = CallService.SaveCommunityAvailability(idCall,idCommunity, forAllUsers, roles);
                    if (saved)
                    {
                        View.DisplayCommunityAssignmentSaved(name);
                        LoadAssignments(idCall);
                    }
                    else
                        View.DisplaySaveErrors(!saved);
                }
            }
            public void DeleteCommunityAssignment(Int32 idCommunity, long idAssignment) {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else {
                    Boolean deleted = CallService.DeleteCommunityAssignments(idCall, idCommunity, idAssignment);
                    if (deleted)
                    {
                        Community c = CurrentManager.GetCommunity(idCommunity);
                        if (c != null)
                            View.DisplayCommunityAssignmentDeleted(c.Name);
                        LoadAssignments(idCall);
                    }
                    else
                        View.DisplaySaveErrors(!deleted);
                }
            }
        #endregion
        #region "User Assignments"
            public void AddUserFromCall()
            {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else{
                    List<Int32> idCommunities = new List<Int32>();
                    //CallService.GetIdCommunityAssignments(idCall);
                    dtoBaseForPaper call = CallService.GetDtoBaseCall(idCall);
                    if (call != null && call.Community !=null) {
                        idCommunities.Add(call.Community.Id);
                    }
                    View.SelectedIdUsers = GetUpdatedUsersSelection();
                    View.DisplayAddUsersFromCall(View.CallType, View.ForPortal, idCommunities, CallService.GetIdUserAssignments(idCall));
                }
            }
            public void AddUserFromCommunity()
            {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    //List<Int32> idCommunities = CallService.GetIdCommunityAssignments(idCall);
                    //if (idCommunities
                    ////CallService.GetIdCommunityAssignments(idCall);
                    //dtoBaseForPaper call = CallService.GetDtoBaseCall(idCall);
                    //if (call != null && call.Community != null)
                    //{
                    //    idCommunities.Add(call.Community.Id);
                    //}
                    View.SelectedIdUsers = GetUpdatedUsersSelection();
                    View.DisplayAddUsersFromCommunity(View.IdCommunity, CallService.GetIdUserAssignments(idCall));
                }
            }
            public void AddUserFromPortal()
            {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<Int32> idCommunities = new List<Int32>();
                    //CallService.GetIdCommunityAssignments(idCall);
                    dtoBaseForPaper call = CallService.GetDtoBaseCall(idCall);
                    if (call != null && call.Community != null)
                    {
                        idCommunities.Add(call.Community.Id);
                    }
                    View.SelectedIdUsers = GetUpdatedUsersSelection();
                    View.DisplayAddUsersFromPortal(CallService.GetIdUserAssignments(idCall));
                }
            }
            public void SaveSettings(List<Int32> idUsers)
            {
                long idCall = View.IdCall;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<BaseForPaperPersonAssignment> pAssignments = CallService.AddPersonAssignments(idCall, idUsers);
                    if (pAssignments != null && pAssignments.Any())
                    {
                        View.DisplayUserAssignmentsAdded();
                        LoadAssignments(idCall,pAssignments.Select(a => a.AssignedTo.Id).ToList());
                    }
                    else if (pAssignments !=null)
                        View.DisplaySaveErrors(true);
                }
            }
        #endregion

        private void LoadAssignments(long idCall,List<Int32> usersToAdd) {
            dtoBaseForPaper call = CallService.GetDtoBaseCall(idCall);
            List<dtoCallAssignment> assignments = TranslateAndReorderAssignments(CallService.GetCallAssignments(idCall,View.ForPortal, View.GetTranslatedProfileTypes(), View.GetTranslatedRoles()));
            List<Int32> idSelectedUsers = GetUpdatedUsersSelection();
            idSelectedUsers.AddRange(usersToAdd);
            View.SelectedIdUsers = idSelectedUsers;

            if ((assignments==null || !assignments.Any()) && !call.IsPublic && !call.ForSubscribedUsers)
                View.DisplayNoAssignments();
            else
                View.LoadAssignments(assignments);
        }
        private void LoadAssignments(long idCall) {
            LoadAssignments(idCall, new List<Int32>());
        }
        private List<Int32> GetUpdatedUsersSelection()
        {
            // SELECTED ITEMS
            List<Int32> users = View.SelectedIdUsers;
            List<dtoSelectItem<Int32>> cSelectedItems = View.GetCurrentSelectedUsers();

            // REMOVE ITEMS
            users = users.Where(i => !cSelectedItems.Where(si => !si.Selected && si.Id == i).Any()).ToList();
            // ADD ITEMS
            users.AddRange(cSelectedItems.Where(si => si.Selected && !users.Contains(si.Id)).Select(si => si.Id).Distinct().ToList());
            return users;
        }
        public void SaveSettings() {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long idCall = View.IdCall;
                Boolean isPublic = View.IsPublic;
                List<Int32> users = GetUpdatedUsersSelection();
                dtoBaseForPaper call = CallService.GetDtoBaseCall(idCall);

                Boolean saved = CallService.SaveCallAvailability(idCall, isPublic, users);
                if (saved && isPublic && !View.isSkinSelectorVisible)
                {
                    LoadSkins(CallService.GetDtoBaseCall(View.IdCall));
                    View.SelectedIdUsers = users;
                    if (isPublic)
                    {
                        switch (View.CallType)
                        {
                            case CallForPaperType.RequestForMembership:
                                View.DisplayPublicUrls(View.CallType, RootObject.StartNewSubmission(View.CallType, View.IdCall, true, false, CallStatusForSubmitters.None, -1), RootObject.PublicCollectorCalls(View.CallType, View.IdCall, View.IdCommunity));
                                break;
                            default:
                                View.DisplayPublicUrl(View.CallType, RootObject.StartNewSubmission(View.CallType, View.IdCall, true, false, CallStatusForSubmitters.None, -1));
                                break;
                        }
                    }
                    else
                        View.HidePublicUrl();
                }
                else if (saved && !isPublic && View.isSkinSelectorVisible)
                    View.HidePublicUrl();
                if (saved)
                {
                    View.DisplayAvailabilitySaved();
                    LoadAssignments(idCall);
                }
                else
                    View.DisplaySaveErrors(!saved);
            }
        }

        #region "Skins"
            public void LoadSkins(long idCall){
                dtoBaseForPaper call = CallService.GetDtoBaseCall(idCall);
                LoadSkins(call);
            }
            public void LoadSkins(dtoBaseForPaper call)
            {
                if (call == null || !call.IsPublic)
                    View.HideSkinsInfo();
                else {
                    Int32 idModule = 0;
                    Int32 idCommunity = View.IdCommunity;
                    switch (call.Type) {
                        case CallForPaperType.CallForBids:
                            ModuleCallForPaper module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                            idModule = CallService.ServiceModuleID();
                            View.LoadSkinInfo(call.Id, typeof(CallForPaper).FullName, ModuleCallForPaper.ObjectType.CallForPaper, idModule, idCommunity, (module.Administration || module.ManageCallForPapers), (module.Administration || module.ManageCallForPapers) && View.AllowSave, (module.Administration || module.ManageCallForPapers));
                            break;
                        case CallForPaperType.RequestForMembership:
                            ModuleRequestForMembership moduleR = CallService.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                            idModule = RequestService.ServiceModuleID();
                            View.LoadSkinInfo(call.Id, typeof(RequestForMembership).FullName, ModuleRequestForMembership.ObjectType.RequestForMembership, idModule, idCommunity, (moduleR.Administration || moduleR.ManageBaseForPapers), (moduleR.Administration || moduleR.ManageBaseForPapers) && View.AllowSave, (moduleR.Administration || moduleR.ManageBaseForPapers));
                            break;
                    }
                }
            }
        #endregion
    }
}