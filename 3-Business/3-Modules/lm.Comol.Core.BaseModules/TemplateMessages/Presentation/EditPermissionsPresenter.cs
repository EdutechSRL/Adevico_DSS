using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class EditPermissionsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditPermissions View
            {
                get { return (IViewEditPermissions)base.View; }
            }
            private TemplateMessagesService Service
            {
                get
                {
                    if (service == null)
                        service = new TemplateMessagesService(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement _ServiceCommunityManagement;
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement CommunityService
            {
                get
                {
                    if (_ServiceCommunityManagement == null)
                        _ServiceCommunityManagement = new lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(AppContext);
                    return _ServiceCommunityManagement;
                }
            }
            public EditPermissionsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditPermissionsPresenter(iApplicationContext oContext, IViewEditPermissions view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            long idTemplate = View.PreloadIdTemplate;
            long idVersion = View.PreloadIdVersion;
            TemplateType t = View.PreloadTemplateType;
            dtoBaseTemplateOwner ownerInfo = View.PreloadOwnership;
            Int32 idCommunity = ownerInfo.IdCommunity;
            if (idCommunity == -1)
                idCommunity = UserContext.CurrentCommunityID;
            if (ownerInfo.IdModule == 0 && !String.IsNullOrEmpty(ownerInfo.ModuleCode))
                ownerInfo.IdModule = CurrentManager.GetModuleID(ownerInfo.ModuleCode);
            View.Ownership = ownerInfo;
            View.IdTemplate = idTemplate;
            View.IdTemplateCommunity = idCommunity;

            if (UserContext.isAnonymous)
                Logout(t,idTemplate, idVersion);
            else
            {
                Boolean allowSave = false;
                TemplateDefinitionVersion version = Service.GetVersion(idVersion);
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> steps = Service.GetAvailableSteps(idVersion, WizardTemplateStep.Permission, ownerInfo.Type);
                if (version == null || (version.Deleted != BaseStatusDeleted.None && !View.PreloadPreview) || (version.Template == null))
                {
                    View.DisplayUnknownTemplate();
                    steps.ForEach(s => s.Status = Wizard.WizardItemStatus.disabled);
                    View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.UnknownTemplateVersion);
                }
                else
                {
                    dtoTemplatePermission permission = null;
                    Boolean isPreview = View.PreloadPreview;
                    Boolean allowSee = false;
                    View.IdTemplate = version.Template.Id;
                    View.IdVersion = version.Id;
                    t = version.Template.Type;
                    switch (ownerInfo.Type)
                    {
                        case OwnerType.None:
                            View.UnableToReadUrlSettings();
                            View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.MalformedUrl);
                            break;
                        case OwnerType.Object:
                            //allowSave = View.HasPermissionForObject(ModuleObject.CreateLongObject(ownerInfo.IdObject, ownerInfo.IdObjectType, ownerInfo.IdObjectCommunity, CurrentManager.GetModuleCode(ownerInfo.IdObjectModule)));
                            //break;
                        case OwnerType.Module:
                            View.IdTemplateModule = ownerInfo.IdModule;
                            permission = Service.GetItemPermission(idVersion);
                            if (ownerInfo.IdModule == Service.ServiceModuleID())
                                permission = Service.GetItemPermission(idVersion);
                            else
                                permission = Service.GetManagementTemplatePermission(version, GetPermissions(ownerInfo.ModuleCode, ownerInfo.ModulePermission, ownerInfo.IdCommunity, t));
                            allowSave = permission.AllowChangePermission;
                            allowSee = permission.AllowUse;
                            if (!allowSave && !isPreview)
                                isPreview = permission.AllowEdit || allowSee;
                            break;
                        default:
                            permission = Service.GetItemPermission(idVersion);
                            allowSave = permission.AllowChangePermission;
                            allowSee = permission.AllowUse;
                            if (!allowSave && !isPreview)
                                isPreview = permission.AllowEdit || allowSee;
                            break;
                    }
                    allowSave = allowSave && !isPreview;
                    //allowActivate = allowDraft && (version.DefaultTranslation.IsValid() || (version!=null && version.Translations.Where(tn => tn.Deleted == BaseStatusDeleted.None && tn.IsValid).Any()));

                    Boolean isAdministrative = Service.IsAdministrativeUser(UserContext.CurrentUserID);
                    View.InputReadOnly = isPreview || (!allowSave && allowSee);
                    View.AllowSave = allowSave;
                    if (allowSave || allowSee)
                    {
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), (isPreview) ? ModuleTemplateMessages.ActionType.DisplayPermissions : ModuleTemplateMessages.ActionType.StartEditingPermissions);
                        LoadPermissions(version);
                    }
                    else
                    {
                        View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), (isPreview) ? ModuleTemplateMessages.ActionType.TryToDisplayPermissions : ModuleTemplateMessages.ActionType.TryToStartEditingPermissions);
                    }
                }
                View.CurrentType = t;

                View.LoadWizardSteps(idCommunity, steps);
                if (String.IsNullOrEmpty(View.PreloadBackUrl))
                    View.SetBackUrl(RootObject.List(View.PreloadFromIdCommunity, t, ownerInfo, true, View.PreloadFromModuleCode, View.PreloadFromModulePermissions, idTemplate, idVersion));
                else
                    View.SetBackUrl(View.PreloadBackUrl);
            }
        }
        private ModuleGenericTemplateMessages GetPermissions(String moduleCode, long permissions, Int32 idCommunity, TemplateType type)
        {
            ModuleGenericTemplateMessages permission = null;
            Int32 idUser = UserContext.CurrentUserID;
            switch (type)
            {
                case TemplateType.Module:
                    if (moduleCode == ModuleTemplateMessages.UniqueCode)
                        permission = new ModuleGenericTemplateMessages(Service.GetPermission(idCommunity, OwnerType.Module));
                    else
                    {
                        Int32 idModule = CurrentManager.GetModuleID(moduleCode);
                        dtoBaseTemplateOwner ownerInfo = View.PreloadOwnership;
                        ModuleObject obj = (ownerInfo.Type == OwnerType.Object) ? ModuleObject.CreateLongObject(ownerInfo.IdObject, ownerInfo.IdObjectType, ownerInfo.IdObjectCommunity, CurrentManager.GetModuleCode(ownerInfo.IdObjectModule), ownerInfo.IdObjectModule) : null;
                        if (obj != null && obj.ServiceID == 0 && !String.IsNullOrEmpty(obj.ServiceCode))
                            obj.ServiceID = CurrentManager.GetModuleID(obj.ServiceCode);
                        else if (obj != null && obj.ServiceID > 0 && String.IsNullOrEmpty(obj.ServiceCode))
                            obj.ServiceCode = CurrentManager.GetModuleCode(obj.ServiceID);
                        if (permissions > 0)
                            permission = View.GetModulePermissions(moduleCode, idModule, CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule), idCommunity, UserContext.UserTypeID, obj);
                        else
                            permission = View.GetModulePermissions(moduleCode, idModule, GetModulePermission(idCommunity, idModule), idCommunity, UserContext.UserTypeID, obj);
                    }
                    break;
                case TemplateType.User:
                    Person p = GetCurrentUser(ref idUser);
                    Boolean allowView = (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser);

                    permission = new ModuleGenericTemplateMessages("personal");
                    permission.Add = allowView;
                    permission.Administration = allowView;
                    permission.Clone = allowView;
                    permission.DeleteMyTemplates = allowView;
                    permission.Edit = allowView;
                    permission.List = allowView;
                    break;
            }
            if (permission == null)
                permission = new ModuleGenericTemplateMessages(moduleCode);
            return permission;
        }
        public long GetModulePermission(Int32 idCommunity, Int32 idModule)
        {
            return CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule);
        }
        private Person GetCurrentUser(ref Int32 idUser)
        {
            Person person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetPerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetPerson(idUser);
            return person;
        }

        #region "Initialize View"
            private void LoadPermissions(TemplateDefinitionVersion version)
            {
                View.ForPortal = version.IsForPortal();
                LoadAvailableItemsToAdd(version);
                List<dtoTemplateAssignment> assignments = UpdateAssigments(Service.GetPermissionAssignments(version, View.GetTranslatedModules(), View.GetTranslatedProfileTypes(), View.GetTranslatedRoles()));
               
                if (assignments.Count == 0 )
                {
                    View.SelectedIdUsers = new List<Int32>();
                    switch (version.Template.Type) { 
                        case TemplateType.System:
                            View.LoadAssignments(TranslateAndReorderAssignments(Service.GetDefaultAssignmentSettings(version.Id)));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    List<dtoTemplateAssignment> items = TranslateAndReorderAssignments(assignments);
                    View.SelectedIdUsers = assignments.Where(a => a.Type == PermissionType.Person).Select(a => ((dtoPersonAssignment)a).IdPerson).ToList();
                    View.LoadAssignments(items);
                }
            }
            private List<dtoTemplateAssignment> TranslateAndReorderAssignments(dtoTemplateAssignment assignment)
            {
                List<dtoTemplateAssignment> assignments = new List<dtoTemplateAssignment>();
                if (assignment != null)
                    assignments.Add(assignment);
                return TranslateAndReorderAssignments(assignments);
            }
            private List<dtoTemplateAssignment> TranslateAndReorderAssignments(List<dtoTemplateAssignment> assignments)
            {
                List<dtoTemplateAssignment> reorderedItems = new List<dtoTemplateAssignment>();
                if (assignments.Where(a => a.Type == PermissionType.Portal).Any())
                {
                    ((dtoPortalAssignment)assignments.Where(a => a.Type == PermissionType.Portal).FirstOrDefault()).DisplayName = View.Portalname;
                }
                Int32 index = -1;
                foreach (dtoCommunityAssignment cAssignment in assignments.Where(a => a.Type == PermissionType.Community).Select(a => ((dtoCommunityAssignment)a)).Where(a => a.IsUnknown).ToList())
                {
                    cAssignment.DisplayName = View.UnknownCommunityTranslation + " - " + index.ToString();
                    index--;
                }

                index = -1;
                foreach (dtoPersonAssignment pAssignment in assignments.Where(a => a.Type == PermissionType.Person).Select(a => ((dtoPersonAssignment)a)).Where(a => a.IsUnknown).ToList())
                {
                    pAssignment.DisplayName = View.UnknownUserTranslation + " - " + index.ToString();
                    index--;
                }
                if (assignments.Where(a => a.Type == PermissionType.Portal).Any())
                    reorderedItems.AddRange(assignments.Where(a => a.Type == PermissionType.Portal).ToList());
                if (assignments.Where(a => a.Type == PermissionType.Community).Any())
                    reorderedItems.AddRange(assignments.Where(a => a.Type == PermissionType.Community).OrderByDescending(a => a.IsDefault).ThenByDescending(a => a.IsUnknown).ThenBy(a => a.DisplayName).ToList());
                if (assignments.Where(a => a.Type == PermissionType.Person).Any())
                    reorderedItems.AddRange(assignments.Where(a => a.Type == PermissionType.Person).OrderByDescending(a => a.IsUnknown).ThenBy(a => a.DisplayName).ToList());
                return reorderedItems;
            }
            private void LoadAvailableItemsToAdd(TemplateDefinitionVersion version)
            {
                List<PermissionType> itemsToAdd = new List<PermissionType>();
                if (!View.InputReadOnly && View.AllowSave)
                {
                    if (version.Template != null)
                    {
                        Boolean isAdministrative = Service.IsAdministrativeUser(UserContext.CurrentUserID);
                        if (version.Template.Type == TemplateType.System || version.Template.OwnerInfo.Type == OwnerType.System || (View.IdTemplateCommunity <1 && isAdministrative))
                            itemsToAdd.Add(PermissionType.ProfileType);
                        // else{
                        List<Int32> removeCommunities = Service.GetIdCommunityAssignments(version.Id);
                        removeCommunities.Add((version.Template.OwnerInfo.Community == null) ? 0 : version.Template.OwnerInfo.Community.Id);
                        if (CommunityService.HasAvailableCommunitiesByModule(UserContext.CurrentUserID, true, removeCommunities))
                        {
                            itemsToAdd.Add(PermissionType.Community);
                            InitializeCommunitySelector();
                        }
                        // }
                        if (View.IdTemplateCommunity >0)
                            itemsToAdd.Add(PermissionType.Person);
                    }
                }
                View.LoadAvailableAssignments(itemsToAdd);
            }
        #endregion

        #region "Person Type Assignments"
            public dtoPortalAssignment GetPortalAssignments(Dictionary<Int32, String> profileTypes)
            {
                return Service.GetPortalAssignment(View.IdVersion, profileTypes);
            }
            public void SaveSettings(List<dtoTemplateAssignment> assignments, List<Int32> profileTypes)
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else
                {
                    List<dtoProfileTypeAssignment> pAssignments = assignments.Where(a => a.Type == PermissionType.ProfileType).Select(a => ((dtoProfileTypeAssignment)a)).ToList().Where(a => profileTypes.Contains(a.IdPersonType)).ToList();
                    pAssignments.AddRange(profileTypes.Where(t => !pAssignments.Where(a => a.IdPersonType == t).Any()).Select(t => new dtoProfileTypeAssignment() { IdPersonType = t, Use = true }).ToList());
                    Boolean saved = Service.SavePortalPermissions(idVersion, pAssignments);
                    if (saved)
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.PortalPermissionsSaved);
                        View.DisplayPortalPermissionsSaved();
                        LoadAssignments(idVersion, assignments, PermissionType.ProfileType);
                    }
                    else
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.PortalPermissionsSavingErrors);
                        View.DisplayPortalPermissionsErrorSaving();
                    }
                }
            }
        #endregion

        #region "Community Assignments"
            public void InitializeCommunitySelector()
            {
                long idVersion = View.IdVersion;
          
                TemplateDefinitionVersion version = Service.GetVersion(idVersion);
                Dictionary<Int32, long> rPermissions = new Dictionary<Int32, long>();
                Boolean forAdmin = (UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative);
                Core.BaseModules.CommunityManagement.CommunityAvailability availability = (forAdmin) ? Core.BaseModules.CommunityManagement.CommunityAvailability.All : Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed;

                /*if (version!=null && version.Template!= null && version.Template.OwnerInfo.Type== OwnerType.Module)
                    rPermissions.Add(version.Template.OwnerInfo.IdModule, version.Template.OwnerInfo.);
                else*/
                rPermissions.Add(Service.ServiceModuleID(), (long)ModuleTemplateMessages.Base2Permission.Administration | (long)ModuleTemplateMessages.Base2Permission.ManageTemplates);
                rPermissions.Add(CommunityService.ServiceModuleID(), (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.Manage | (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.AdminService);
                List<Int32> idCommunities = Service.GetIdCommunityAssignments(idVersion);
                View.InitializeCommunityToAdd(UserContext.CurrentUserID, forAdmin, rPermissions, idCommunities, availability);
            }
            //public void SelectCommunityToAdd()
            //{
            //    long idVersion = View.IdVersion;
            //    if (UserContext.isAnonymous)
            //        Logout(View.CurrentType, View.IdTemplate, idVersion);
            //    else
            //    {
            //        TemplateDefinitionVersion version = Service.GetVersion(idVersion);
            //        Dictionary<Int32, long> rPermissions = new Dictionary<Int32, long>();
            //        Boolean forAdmin = (UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator) || (UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative);
            //        Core.BaseModules.CommunityManagement.CommunityAvailability availability = (forAdmin) ? Core.BaseModules.CommunityManagement.CommunityAvailability.All : Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed;

            //        /*if (version!=null && version.Template!= null && version.Template.OwnerInfo.Type== OwnerType.Module)
            //            rPermissions.Add(version.Template.OwnerInfo.IdModule, version.Template.OwnerInfo.);
            //        else*/
            //        rPermissions.Add(Service.ServiceModuleID(), (long)ModuleTemplateMessages.Base2Permission.Administration | (long)ModuleTemplateMessages.Base2Permission.ManageTemplates);
            //        rPermissions.Add(CommunityService.ServiceModuleID(), (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.Manage | (long)lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.Base2Permission.AdminService);
            //        List<Int32> idCommunities = Service.GetIdCommunityAssignments(idVersion);
            //        View.InitializeCommunityToAdd(UserContext.CurrentUserID, forAdmin, rPermissions, idCommunities, availability);
            //    }
            //}
            public void AddCommunity(List<Int32> idCommunites)
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else if (idCommunites.Any())
                {
                    Boolean saved = Service.AddCommunityAssignment(idVersion, idCommunites);
                    if (saved)
                    {
                        if (idCommunites.Count > 1)
                            View.DisplayCommunityAssignmentsAdded();
                        else
                        {
                            liteCommunity c = CurrentManager.GetLiteCommunity(idCommunites.First());
                            if (c != null)
                                View.DisplayCommunityAssignmentAdded(c.Name);
                            else
                                View.DisplayCommunityAssignmentsAdded();
                        }
                        LoadAssignments(idVersion, View.GetPermissions(), PermissionType.Base);
                        InitializeCommunitySelector();
                    }
                    else
                        View.DisplayCommunityAddingError();
                }
            }
            public void LoadCommunityAssignments(Int32 idCommunity, long idAssignment, Dictionary<String, String> modules, Dictionary<Int32, String> roles)
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else
                {
                    String cName = View.UnknownCommunityTranslation;
                    Community community = CurrentManager.GetCommunity(idCommunity);
                    if (community != null)
                        cName = community.Name;
                    View.SelectedIdUsers = GetUpdatedUsersSelection();
                    View.LoadCommunityAssignment(Service.GetCommunityAssignments(idVersion, idCommunity, idAssignment,modules, roles), cName, idCommunity, CommunityService.GetCommunityAvailableIdRoles(community));
                }
            }
            public void SaveSettings(Int32 idCommunity, List<dtoTemplateAssignment> assignments, List<Int32> roles)
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
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

                    List<dtoRoleAssignment> rAssignments = assignments.Where(a => a.Type == PermissionType.Role).Select(a => ((dtoRoleAssignment)a)).ToList().Where(a => roles.Contains(a.IdRole) && a.IdCommunity == idCommunity).ToList();
                    rAssignments.AddRange(roles.Where(t => !rAssignments.Where(a => a.IdRole == t && a.IdCommunity == idCommunity).Any()).Select(t => new dtoRoleAssignment() { IdRole= t, IdCommunity=idCommunity , Use = true }).ToList());

                    Boolean saved = Service.SaveCommunityPermissions(idVersion, idCommunity, rAssignments);
                    if (saved)
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.CommunityPermissionsSaved);
                        View.DisplayCommunityAssignmentSaved(name);
                        LoadAssignments(idVersion, assignments, PermissionType.Role, idCommunity);
                    }
                    else
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.CommunityPermissionsSavingErrors);
                        View.DisplayCommunityAssignmentErrorSaving();
                    }
                }
            }
            public void DeleteCommunityAssignment(Int32 idCommunity, long idAssignment)
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else
                {
                    Boolean deleted = Service.DeleteCommunityAssignments(idVersion, idCommunity, idAssignment);
                    if (deleted)
                    {
                        Community c = CurrentManager.GetCommunity(idCommunity);
                        if (c != null)
                            View.DisplayCommunityAssignmentDeleted(c.Name);
                        LoadAssignments(idVersion, View.GetPermissions(), PermissionType.Base);
                    }
                    else
                        View.DisplayCommunityDeletingError();
                }
            }
        #endregion
        
        #region "User Assignments"
            public void AddUserFromCommunity()
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else
                {
                    View.SelectedIdUsers = GetUpdatedUsersSelection();
                    View.DisplayAddUsersFromCommunity(View.IdTemplateCommunity, Service.GetIdUserAssignments(idVersion));
                }
            }
            public void AddUserFromPortal()
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else
                {
                    List<Int32> idCommunities = new List<Int32>();
                    TemplateDefinition t = CurrentManager.Get<TemplateDefinition>(View.IdTemplate);
                    if (t != null && t.OwnerInfo.Community != null)
                    {
                        idCommunities.Add(t.OwnerInfo.Community.Id);
                    }
                    View.SelectedIdUsers = GetUpdatedUsersSelection();
                    View.DisplayAddUsersFromPortal(Service.GetIdUserAssignments(idVersion));
                }
            }
            public void AddUsers(List<dtoTemplateAssignment> cAssignments, List<Int32> idUsers)
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else
                {
                    List<VersionPersonPermission> pAssignments = Service.AddPersonAssignments(idVersion, idUsers);
                    if (pAssignments != null && pAssignments.Any())
                    {
                        List<Int32> users = GetUpdatedUsersSelection();
                        users.AddRange(idUsers);
                        View.SelectedIdUsers = users.Distinct().ToList();

                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.AddedPersonPermissions);
                        View.DisplayUserAssignmentsAdded();
                        LoadAssignments(idVersion, cAssignments, PermissionType.Base);
                    }
                    else if (pAssignments != null){
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.AddingPersonPermissionsErrors);
                        View.DisplayUserAddingError();
                    }
                }
            }
            public void SavePersonSettings(List<dtoTemplateAssignment> assignments)
            {
                long idVersion = View.IdVersion;
                if (UserContext.isAnonymous)
                    Logout(View.CurrentType, View.IdTemplate, idVersion);
                else
                {
                    List<Int32> users = GetUpdatedUsersSelection();
                    List<dtoPersonAssignment> pAssignments = assignments.Where(a => a.Type == PermissionType.Person).Select(a => ((dtoPersonAssignment)a)).ToList().Where(a => users.Contains(a.IdPerson)).ToList();
                    if (Service.SavePersonPermissions(idVersion, pAssignments))
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.PersonPermissionsSaved);
                        View.DisplayUserAssignmentsSaved();
                        LoadAssignments(idVersion, assignments, PermissionType.Base);
                    }
                    else {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.PortalPermissionsSavingErrors);
                        View.DisplayUserAssigmentsSavingError();
                    }
                }
            }
        #endregion

        #region "Load Assignments"
            private void LoadAssignments(long idVersion, List<dtoTemplateAssignment> currentItems, PermissionType fromDB, Int32 idCommunity=-1)
            {
                TemplateDefinitionVersion version = Service.GetVersion(idVersion);
                if (version == null)
                    View.DisplayUnknownTemplate();
                else
                {
                    List<dtoRoleAssignment> roles = currentItems.Where(i => i.Type == PermissionType.Role).Select(i => (dtoRoleAssignment)i).ToList();
                    List<dtoProfileTypeAssignment> types = currentItems.Where(i => i.Type == PermissionType.ProfileType).Select(i => (dtoProfileTypeAssignment)i).ToList();
                    List<dtoPersonAssignment> users = currentItems.Where(i => i.Type == PermissionType.Person).Select(i => (dtoPersonAssignment)i).ToList();

                    List<dtoTemplateAssignment> assignments = Service.GetPermissionAssignments(version, View.GetTranslatedModules(), View.GetTranslatedProfileTypes(), View.GetTranslatedRoles());
                    switch (fromDB)
                    {
                        case PermissionType.Role:
                            assignments.Where(a => a.Type == PermissionType.Portal).SelectMany(a => ((dtoPortalAssignment)a).ProfileTypes).Where(a => types.Where(r => r.IdPersonType == a.IdPersonType && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(types.Where(r => r.IdPersonType == a.IdPersonType).FirstOrDefault()));
                            assignments.Where(a => a.Type == PermissionType.Community).SelectMany(a => ((dtoCommunityAssignment)a).Roles).Where(a => a.IdCommunity != idCommunity && roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity).FirstOrDefault()));
                            assignments.Where(a => a.Type == PermissionType.Person).Select(a => (dtoPersonAssignment)a).Where(a => users.Where(r => r.IdPerson == a.IdPerson && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(users.Where(r => r.IdPerson == a.IdPerson ).FirstOrDefault()));
                            break;
                        case PermissionType.ProfileType:
                            assignments.Where(a => a.Type == PermissionType.Community).SelectMany(a => ((dtoCommunityAssignment)a).Roles).Where(a => roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity).FirstOrDefault()));
                            assignments.Where(a => a.Type == PermissionType.Person).Select(a => (dtoPersonAssignment)a).Where(a => users.Where(r => r.IdPerson == a.IdPerson && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(users.Where(r => r.IdPerson == a.IdPerson).FirstOrDefault()));
                            break;
                        case PermissionType.Person:
                            assignments.Where(a => a.Type == PermissionType.Portal).SelectMany(a => ((dtoPortalAssignment)a).ProfileTypes).Where(a => types.Where(r => r.IdPersonType == a.IdPersonType && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(types.Where(r => r.IdPersonType == a.IdPersonType).FirstOrDefault()));
                            assignments.Where(a => a.Type == PermissionType.Community).SelectMany(a => ((dtoCommunityAssignment)a).Roles).Where(a => roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity).FirstOrDefault()));
                            break;
                        default:
                            assignments.Where(a => a.Type == PermissionType.Portal).SelectMany(a => ((dtoPortalAssignment)a).ProfileTypes).Where(a => types.Where(r => r.IdPersonType == a.IdPersonType && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(types.Where(r => r.IdPersonType == a.IdPersonType).FirstOrDefault()));
                            assignments.Where(a => a.Type == PermissionType.Community).SelectMany(a => ((dtoCommunityAssignment)a).Roles).Where(a => roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(roles.Where(r => r.IdRole == a.IdRole && r.IdCommunity == a.IdCommunity).FirstOrDefault()));
                            assignments.Where(a => a.Type == PermissionType.Person).Select(a => (dtoPersonAssignment)a).Where(a => users.Where(r => r.IdPerson == a.IdPerson && a.PermissionToLong() != r.PermissionToLong()).Any()).ToList().ForEach(a => a.UpdatePermissions(users.Where(r => r.IdPerson == a.IdPerson).FirstOrDefault()));
                            break;
                    }
                    assignments = UpdateAssigments(assignments);
                    View.LoadWizardSteps(View.IdTemplateCommunity, Service.GetAvailableSteps(idVersion, WizardTemplateStep.Permission, View.Ownership.Type ));
                    View.LoadAssignments(TranslateAndReorderAssignments(assignments));
                }
            }
            private void LoadAssignments(long idVersion, List<Int32> usersToAdd)
            {
                TemplateDefinitionVersion version = Service.GetVersion(idVersion);
                if (version == null)
                    View.DisplayUnknownTemplate();
                else
                {
                    List<dtoTemplateAssignment> assignments = Service.GetPermissionAssignments(version, View.GetTranslatedModules(), View.GetTranslatedProfileTypes(), View.GetTranslatedRoles());
                    List<Int32> idSelectedUsers = GetUpdatedUsersSelection();
                    idSelectedUsers.AddRange(usersToAdd);
                    View.SelectedIdUsers = idSelectedUsers;

                    if (assignments == null || !assignments.Any())
                        View.DisplayNoAssignments();
                    else
                    {
                        assignments = UpdateAssigments(assignments);
                        View.LoadAssignments(TranslateAndReorderAssignments(assignments));
                    }
                }
            }
            private void LoadAssignments(long idVersion)
            {
                LoadAssignments(idVersion, new List<Int32>());
            }
            private List<dtoTemplateAssignment> UpdateAssigments(List<dtoTemplateAssignment> assignments)
            {
                Boolean allowEdit = !View.InputReadOnly;
                assignments.Where(a => a.Type == PermissionType.Portal).ToList().ForEach(a => ((dtoPortalAssignment)a).UpdateEditing(allowEdit));
                assignments.Where(a => a.Type == PermissionType.Community).ToList().ForEach(a => ((dtoCommunityAssignment)a).UpdateEditing(allowEdit));
                assignments.Where(a => a.Type == PermissionType.Person).ToList().ForEach(a => ((dtoPersonAssignment)a).AllowEdit = allowEdit);
                assignments.Where(a => a.Type == PermissionType.Person).Select(a => (dtoPersonAssignment)a).Where(a => a.IdPerson == UserContext.CurrentUserID).ToList().ForEach(a => a.AllowEdit = false);
                return assignments;    
            }
        #endregion

        public void SaveSettings() {
            long idVersion = View.IdVersion;
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, idVersion);
            else
            {
                List<dtoTemplateAssignment> assignments = View.GetPermissions();
                if (Service.SavePermissions(idVersion, assignments))
                {
                    View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.PermissionsSaved);
                    View.DisplayUserAssignmentsSaved();
                    View.SelectedIdUsers = assignments.Where(a => a.Type == PermissionType.Person).Select(a => ((dtoPersonAssignment)a).IdPerson).Distinct().ToList();
                    LoadAssignments(idVersion, new List<dtoTemplateAssignment>(), PermissionType.Base);
                }
                else
                {
                    View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.PermissionsSavingErrors);
                    View.DisplayUserAssigmentsSavingError();
                }
            }
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

        private void Logout(TemplateType t,long idTemplate,long idVersion) {
            View.DisplaySessionTimeout(RootObject.EditByStep(t, View.PreloadOwnership, WizardTemplateStep.Permission, View.PreloadFromIdCommunity, View.PreloadFromModuleCode, View.PreloadFromModulePermissions, View.GetEncodedBackUrl(), idTemplate, idVersion, View.PreloadPreview));
        }
    }
}