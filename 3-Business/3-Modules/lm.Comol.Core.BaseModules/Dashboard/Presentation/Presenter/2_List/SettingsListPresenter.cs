using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Dashboard.Presentation;
namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class SettingsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private Dashboard.Business.ServiceDashboardCommunities service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewSettingsList View
            {
                get { return (IViewSettingsList)base.View; }
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
            public SettingsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SettingsListPresenter(iApplicationContext oContext, IViewSettingsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(ModuleDashboard permissions, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                View.IdContainerCommunity = idCommunity;
                View.CurrentType = type;
                View.FromRecycleBin = loadFromRecycleBin;
                View.CurrentOrderBy= OrderSettingsBy.Default;
                View.CurrentAscending = true;
                LoadSettings(permissions, type, idCommunity, loadFromRecycleBin);
            }
        }
        public void LoadSettings(DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, OrderSettingsBy orderBy, Boolean ascending)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                if (type == DashboardType.Community && idCommunity > 0 && (!permissions.Administration && !permissions.List))
                    permissions = Service.GetPermission(idCommunity);
                LoadSettings(permissions, type, idCommunity, loadFromRecycleBin, orderBy, ascending);
            }
        }
        private void LoadSettings(ModuleDashboard permissions, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, OrderSettingsBy orderBy = OrderSettingsBy.Default, Boolean ascending = true)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<dtoDashboardSettings> items = Service.DashboardGetSettings(permissions, type, View.GetUnknownUserName(), View.GetTranslatedStatus(), idCommunity, loadFromRecycleBin, orderBy, ascending);
                if (items == null)
                    View.DisplayErrorLoadingFromDB();
                else
                {
                    //if (items.Count == 1)
                    //    items[0].Permissions.AllowSetUnavailable = items[0].Permissions.AllowSetUnavailable && !(items[0].Active && items[0].ForAll);
                    //else if (items.Where())
                    ModuleDashboard.ActionType action = ModuleDashboard.ActionType.DashboardSettingsPortalList;
                    switch (type)
                    {
                        case DashboardType.AllCommunities:
                            action = ModuleDashboard.ActionType.DashboardSettingsAllCommunitiesList;
                            break;
                        case DashboardType.Community:
                            action = ModuleDashboard.ActionType.DashboardSettingsCommunityList;
                            break;
                    }
                    View.LoadSettings(items);
                    View.SendUserAction(idCommunity, CurrentIdModule,  action);
                }
            }
        }
        
        public void SetStatus(long idDashboard, AvailableStatus status, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, OrderSettingsBy orderBy, Boolean ascending)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (status == AvailableStatus.Available) ? ModuleDashboard.ActionType.DashboardSettingsEnable : ModuleDashboard.ActionType.DashboardSettingsDisable;
                try
                {
                    DashboardSettings item = Service.DashboardSettingsSetStatus(idDashboard, status);
                    if (item == null || item.Status != status)
                        action = (status == AvailableStatus.Available) ? ModuleDashboard.ActionType.DashboardSettingsUnableToEnable : ModuleDashboard.ActionType.DashboardSettingsUnableToDisable;
                    View.DisplayMessage(action);
                    View.SendUserAction(idCommunity, CurrentIdModule, idDashboard, action);
                }
                catch (DashboardException ex) {
                    action = (status == AvailableStatus.Available) ? ModuleDashboard.ActionType.DashboardSettingsUnableToEnable : ModuleDashboard.ActionType.DashboardSettingsUnableToDisable;
                    View.SendUserAction(idCommunity, CurrentIdModule, idDashboard, action);
                    View.DisplayMessage(ex.ErrorType);
                }
                
                LoadSettings(type, idCommunity, loadFromRecycleBin, orderBy, ascending);
            }
        }
        public void VirtualDelete(long idDashboard, Boolean delete, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, OrderSettingsBy orderBy, Boolean ascending)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (delete) ? ModuleDashboard.ActionType.DashboardSettingsVirtualDelete : ModuleDashboard.ActionType.DashboardSettingsVirtualUndelete;
                try
                {
                    DashboardSettings item = Service.DashboardSettingsVirtualDelete(idDashboard, delete);
                    if (item == null)
                        action = (delete) ? ModuleDashboard.ActionType.DashboardSettingsUnableToVirtualDelete : ModuleDashboard.ActionType.DashboardSettingsUnableToUndelete;
                    View.DisplayMessage(action);
                    View.SendUserAction(idCommunity, CurrentIdModule, idDashboard, action);
                }
                catch (DashboardException ex)
                {
                    action = (delete) ? ModuleDashboard.ActionType.DashboardSettingsUnableToVirtualDelete : ModuleDashboard.ActionType.DashboardSettingsUnableToUndelete;
                    View.SendUserAction(idCommunity, CurrentIdModule, idDashboard, action);
                    View.DisplayMessage(ex.ErrorType);
                }
                LoadSettings(type, idCommunity, loadFromRecycleBin, orderBy, ascending);
            }
        }
        public void Clone(long idDashboard,String cloneOf, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, OrderSettingsBy orderBy, Boolean ascending)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = ModuleDashboard.ActionType.DashboardSettingsClone;
                DashboardSettings item = Service.DashboardSettingsClone(idDashboard,cloneOf);
                if (item == null)
                    action = ModuleDashboard.ActionType.DashboardSettingsUnableToClone;
                View.DisplayMessage(action);
                View.SendUserAction(idCommunity, CurrentIdModule, idDashboard, action);
                LoadSettings(type, idCommunity, loadFromRecycleBin, orderBy, ascending);
            }
        }
    }
}