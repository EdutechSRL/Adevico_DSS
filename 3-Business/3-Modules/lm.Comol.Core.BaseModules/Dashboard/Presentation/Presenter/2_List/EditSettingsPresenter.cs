using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class EditSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private Dashboard.Business.ServiceDashboardCommunities service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditSettings View
            {
                get { return (IViewEditSettings)base.View; }
            }
            private Dashboard.Business.ServiceDashboardCommunities Service
            {
                get
                {
                    if (service == null)
                        service = new Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleDashboard.UniqueCode);
                    return currentIdModule;
                }
            }
            public EditSettingsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditSettingsPresenter(iApplicationContext oContext, IViewEditSettings view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean add, long idDashboard, DashboardType dashboardType, Int32 idContainerCommunity)
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                if (UserContext.isAnonymous || p == null)
                    View.DisplaySessionTimeout();
                else
                {
                    Int32 idDashboardCommunity = (dashboardType == DashboardType.Community) ? idContainerCommunity : 0;
                    ModuleDashboard permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                    liteDashboardSettings settings = null;
                    if (add)
                    {
                        settings = new liteDashboardSettings();
                        settings.Type = dashboardType;
                        settings.Status = AvailableStatus.Draft;
                        settings.Name = View.GetNewSettingsName();
                        settings.IdCommunity = (dashboardType == DashboardType.Community ? idDashboardCommunity : (dashboardType == DashboardType.AllCommunities ? -1 : 0));
                    }
                    else
                    {
                        settings = CurrentManager.Get<liteDashboardSettings>(idDashboard);
                        if (View.PreloadFromAdd)
                            View.DisplaySettingsAdded();

                        idDashboardCommunity = (settings == null) ? ((dashboardType == DashboardType.Community) ? idContainerCommunity : 0) : settings.IdCommunity;
                    }

                    if (dashboardType == DashboardType.Community)
                    {
                        View.IdContainerCommunity = idContainerCommunity;
                        if (idContainerCommunity < 0)
                            idContainerCommunity = UserContext.CurrentCommunityID;
                        if (idContainerCommunity > 0 && (!permissions.Administration && !permissions.ManageTiles))
                            permissions = Service.GetPermission(idContainerCommunity);
                    }
                    else
                    {
                        idContainerCommunity = 0;
                        View.IdContainerCommunity = 0;
                    }

                    List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>> steps = Service.GetAvailableSteps(WizardDashboardStep.Settings, idDashboard, dashboardType, idContainerCommunity);
                    View.LoadWizardSteps(steps);

                    if ((settings == null && (idDashboard > 0 || !add)) || (settings != null && settings.Deleted != BaseStatusDeleted.None && settings.Deleted != BaseStatusDeleted.Manual))
                    {
                        View.DisplayUnknownDashboard();
                        if ((add && !permissions.Add) || (!add && (! permissions.Administration || !permissions.Edit)))
                            View.DisplayNoPermission(idContainerCommunity, CurrentIdModule);
                        else
                            View.SetBackUrl(RootObject.DashboardList(dashboardType, false, idContainerCommunity));
                    }
                    else if ((add && permissions.Add) || permissions.Administration || (!add && permissions.Edit))
                    {
                        View.AllowSave = true;
                        if (settings.Id > 0)
                        {
                            View.SetPreviewUrl(RootObject.DashboardPreview(settings.Id, settings.Type, settings.IdCommunity));
                            View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), settings.Id, ModuleDashboard.ActionType.DashboardSettingsStartEditing);
                        }
                        else
                            View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), ModuleDashboard.ActionType.DashboardSettingsStartAdding);

                        View.AllowSave = (settings.Deleted == BaseStatusDeleted.None);
                        View.SetBackUrl(RootObject.DashboardList(dashboardType, false, idContainerCommunity));
                        switch(settings.Type){
                            case DashboardType.Portal:
                                View.LoadSettings(settings, Service.GetDashboardAvailableProfileTypes(settings), settings.GetAssignments(DashboardAssignmentType.ProfileType).Select(a => a.IdProfileType).ToList());
                                break;
                            case DashboardType.Community:
                                View.LoadSettings(settings, Service.GetDashboardAvailableRoles(settings), settings.GetAssignments(DashboardAssignmentType.RoleType).Select(a => a.IdRole).ToList());
                                break;
                            case DashboardType.AllCommunities:
                                View.LoadSettings(settings);
                                break;
                        }
                    }
                    else
                        View.DisplayNoPermission(idContainerCommunity, CurrentIdModule);
                }
            }

        public void SaveSettings(dtoBaseDashboardSettings dto)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                String url ="";
                ModuleDashboard.ActionType action = (dto.Id == 0) ? ModuleDashboard.ActionType.DashboardSettingsUnableToAdd : ModuleDashboard.ActionType.DashboardSettingsUnableToSave;
                DashboardSettings settings = (dto.Id > 0) ? CurrentManager.Get<DashboardSettings>(dto.Id) : null;
                if (dto.Id > 0 && settings == null)
                {
                    View.DisplayDeletedDashboard();
                    View.SendUserAction(dto.IdCommunity, CurrentIdModule, dto.Id, action);
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.AllDashboard);
                    return;
                }
                else if (settings != null && settings.Active && !dto.ForAll && dto.Assignments.Count == 0)
                    View.DisplayMessage(DashboardErrorType.NoAssignmentsForActiveSettings);
                else
                {
                    try
                    {
                        DashboardSettings item = Service.SaveBaseSettings(dto);
                        if (item != null)
                        {
                            action = (dto.Id > 0) ? ModuleDashboard.ActionType.DashboardSettingsSaved : ModuleDashboard.ActionType.DashboardSettingsAdded;
                            if (action == ModuleDashboard.ActionType.DashboardSettingsAdded)
                                url = RootObject.DashboardEdit(item.Id, item.Type, dto.IdCommunity, true);
                            else
                                View.EnableSelector(!item.ForAll);
                        }
                        View.DisplayMessage(action);
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.AllDashboard);
                    }
                    catch (DashboardException ex)
                    {
                        View.DisplayMessage(ex.ErrorType);
                    }
                }
                View.SendUserAction(dto.IdCommunity, CurrentIdModule, dto.Id, action);
                if (!String.IsNullOrEmpty(url))
                    View.RedirectToUrl(url);
                else if (action == ModuleDashboard.ActionType.DashboardSettingsSaved)
                    View.LoadWizardSteps(Service.GetAvailableSteps(WizardDashboardStep.Settings, settings.Id, settings.Type, View.IdContainerCommunity));
            }

        }
    }
}