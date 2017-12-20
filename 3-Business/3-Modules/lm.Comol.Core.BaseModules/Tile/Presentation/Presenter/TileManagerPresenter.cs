using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Tiles.Business;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation
{
    public class TileManagerPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags servicetag;
            private ServiceTiles service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewPageBase View
            {
                get { return (IViewPageBase)base.View; }
            }
            private ServiceTiles Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceTiles(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags ServiceTags
            {
                get
                {
                    if (servicetag == null)
                        servicetag = new lm.Comol.Core.BaseModules.Tags.Business.ServiceTags(AppContext);
                    return servicetag;
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
            public TileManagerPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TileManagerPresenter(iApplicationContext oContext, IViewPageBase view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(DashboardType type, Boolean loadFromRecycleBin, Int32 idCommunity, long idDashboard =0, WizardDashboardStep step = WizardDashboardStep.None)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                if (type == DashboardType.Community)
                {
                    View.IdContainerCommunity = UserContext.CurrentCommunityID;
                    if (idCommunity<0)
                        idCommunity = UserContext.CurrentCommunityID;
                    if (idCommunity > 0 && (!permissions.Administration && !permissions.ManageTiles))
                        permissions = Service.GetPermission(idCommunity);
                }
                else
                {
                    idCommunity = 0;
                    View.IdContainerCommunity = 0;
                }
                View.IdTilesCommunity = idCommunity;
                SetBackUrl(type, idDashboard, step);
                if (permissions.ManageTiles)
                {
                   
                    if (loadFromRecycleBin)
                        View.SetBackUrl(RootObject.TileList(type, false, idCommunity));
                    else {
                        View.SetRecycleUrl(RootObject.TileList(type,true, idCommunity));
                        View.SetAddUrl(RootObject.TileAdd(type, idCommunity));
                        View.AllowCommunityTypesTileAutoGenerate = (Service.GetCommunityTypesWithoutTilesCount() > 0);
                    }
                    TileType preloadType = TileType.None;
                    if (idDashboard > 0)
                    {
                        switch (step)
                        {
                            case WizardDashboardStep.CommunityTypes:
                                preloadType = TileType.CommunityType;
                                break;
                        }
                    }
                    View.InitializeListControl(permissions, type, idCommunity, loadFromRecycleBin, View.PreloadIdTile, preloadType);
                }
                else
                    View.DisplayNoPermission(idCommunity, CurrentIdModule);
            }
        }

        private void SetBackUrl(DashboardType type, long idDashboard, WizardDashboardStep step)
        {
            if (idDashboard > 0)
            {
                String name = (step == WizardDashboardStep.None) ? "" : Service.GetSettingsName(idDashboard);
                switch (step)
                {
                    case WizardDashboardStep.Settings:
                        View.SetDashboardSettingsBackUrl(RootObject.DashboardEdit(idDashboard, type), name);
                        break;
                    case WizardDashboardStep.HomepageSettings:
                        View.SetDashboardSettingsBackUrl(RootObject.DashboardEditViews(idDashboard, type), name);
                        break;
                    case WizardDashboardStep.Modules:
                    case WizardDashboardStep.CommunityTypes:
                    case WizardDashboardStep.Tiles:
                        View.SetDashboardSettingsBackUrl(RootObject.DashboardTileReorder(step, idDashboard, type), name);
                        break;
                }
            }
        }
    }
}