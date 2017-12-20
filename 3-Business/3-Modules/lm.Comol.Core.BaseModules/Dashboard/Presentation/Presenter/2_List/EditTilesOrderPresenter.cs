using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class EditTilesOrderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private Dashboard.Business.ServiceDashboardCommunities service;
            private lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles servicetiles;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditTilesOrder View
            {
                get { return (IViewEditTilesOrder)base.View; }
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
            private lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles ServiceTiles
            {
                get
                {
                    if (servicetiles == null)
                        servicetiles = new lm.Comol.Core.BaseModules.Tiles.Business.ServiceTiles(AppContext);
                    return servicetiles;
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
            public EditTilesOrderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditTilesOrderPresenter(iApplicationContext oContext, IViewEditTilesOrder view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(WizardDashboardStep step, long idDashboard, DashboardType dashboardType, Int32 idContainerCommunity)
        {
            View.IdDashboard = idDashboard;
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else if (step == WizardDashboardStep.None)
            {
                switch (dashboardType)
                {
                    case DashboardType.Portal:
                        View.RedirectToUrl(RootObject.DashboardTileReorder(WizardDashboardStep.Tiles, idDashboard, dashboardType, idContainerCommunity));
                        break;
                    default:
                        View.RedirectToUrl(RootObject.DashboardTileReorder(WizardDashboardStep.Modules, idDashboard, dashboardType, idContainerCommunity));
                        break;
                }
            }
            else
            {
                View.CurrentStep = step;
                
                liteDashboardSettings settings = CurrentManager.Get<liteDashboardSettings>(idDashboard);
                Int32 idDashboardCommunity = (settings == null) ? ((dashboardType == DashboardType.Community) ? idContainerCommunity : 0) : settings.IdCommunity;
                List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>> steps = Service.GetAvailableSteps(step, idDashboard, (settings == null) ? dashboardType : settings.Type, idDashboardCommunity);
                View.LoadWizardSteps(steps);


                ModuleDashboard permissions = (dashboardType == DashboardType.Community) ? Service.GetPermission(idDashboardCommunity) : ModuleDashboard.CreatePortalmodule(p.TypeID);
                String url = lm.Comol.Core.Dashboard.Domain.RootObject.TileList(settings.Type, false, idDashboardCommunity,0, idDashboard, step);
                Boolean generateTiles = false;

                switch (step)
                {
                    case WizardDashboardStep.CommunityTypes:
                        generateTiles = permissions.ManageTiles && permissions.Administration && (ServiceTiles.GetCommunityTypesWithoutTilesCount() > 0);
                        break;
                    case WizardDashboardStep.Modules:
                    case WizardDashboardStep.Tiles:
                        break;
                   
                }
                View.InitializeView(step, url, generateTiles);
                View.DashboardType = dashboardType;
                if (settings == null)
                {
                    View.DisplayUnknownDashboard();
                    if (dashboardType == DashboardType.Community)
                    {
                        if (idContainerCommunity < 0)
                            idDashboardCommunity = UserContext.CurrentCommunityID;
                    }
                    else
                        idDashboardCommunity = 0;
                        View.IdContainerCommunity = idDashboardCommunity;
                }
                else
                {
                    View.DashboardType = settings.Type;
                    View.IdContainerCommunity = idDashboardCommunity;
                    
                    if (permissions.Edit || permissions.Administration)
                        View.SetBackUrl(RootObject.DashboardList(dashboardType, false, idContainerCommunity));
                    if (settings.Deleted != BaseStatusDeleted.None)
                        View.DisplayDeletedDashboard();
                    else if (permissions.Edit || permissions.Administration)
                    {
                        View.AllowSave = (settings.Deleted == BaseStatusDeleted.None);
                        View.SetPreviewUrl(RootObject.DashboardPreview(settings.Id, settings.Type, settings.IdCommunity, step));
                        LoadTiles(step, idDashboard, idDashboardCommunity);
                    }
                    else
                        View.DisplayNoPermission(idDashboardCommunity, CurrentIdModule);
                }
            }
        }

        public void SaveOrder(WizardDashboardStep step, long idDashboard, Int32 idDashboardCommunity, List<dtoTileForReorder> tiles)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = ModuleDashboard.ActionType.DashboardSettingsTilesOrderUnableToSave;
                try
                {
                    if (Service.DashboardTilesSaveOrder(idDashboard, tiles))
                    {
                        action = ModuleDashboard.ActionType.DashboardSettingsTilesOrderSaved;
                        View.LoadWizardSteps(Service.GetAvailableSteps(step, idDashboard, View.DashboardType, idDashboardCommunity));
                    }
                    View.DisplayMessage(action);
                }
                catch (DashboardException ex)
                {
                    View.DisplayMessage(ex.ErrorType);
                }
                LoadTiles(step, idDashboard, idDashboardCommunity);
                View.SendUserAction(idDashboardCommunity, CurrentIdModule, idDashboard, action);
            }
        }
        public void GenerateCommunityTypeTiles(WizardDashboardStep step, long idDashboard, Int32 idDashboardCommunity)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = ModuleDashboard.ActionType.TileAutoGenerateForCommunityTypes;
                Int32 count = ServiceTiles.GetCommunityTypesWithoutTilesCount();
                if (count == 0)
                    action = ModuleDashboard.ActionType.TileAlreadyGeneratedForCommunityTypes;
                else
                {
                    List<Tile> tiles = ServiceTiles.GenerateCommunityTypesTile();
                    if (tiles == null || tiles.Count != count)
                    {
                        action = ModuleDashboard.ActionType.TileUnableAutoGenerateForCommunityTypes;
                        View.LoadWizardSteps(Service.GetAvailableSteps(step, idDashboard, View.DashboardType, idDashboardCommunity));
                    }
                }
                View.DisplayMessage(action);
                View.SendUserAction(0, CurrentIdModule, action);
                LoadTiles(step, idDashboard, idDashboardCommunity);
            }
        }

        private void LoadTiles(WizardDashboardStep step, long idDashboard, Int32 idContainerCommunity)
        {
            View.LoadTiles(Service.DashboardGetTilesForReorder(idDashboard, step));
            View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), idDashboard, ModuleDashboard.ActionType.DashboardSettingsTilesStartReorder);
        }
    }
}