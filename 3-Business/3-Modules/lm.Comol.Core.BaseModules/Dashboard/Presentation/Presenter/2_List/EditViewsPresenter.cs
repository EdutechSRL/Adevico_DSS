using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class EditViewsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private Dashboard.Business.ServiceDashboardCommunities service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditViews View
            {
                get { return (IViewEditViews)base.View; }
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
            public EditViewsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditViewsPresenter(iApplicationContext oContext, IViewEditViews view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idDashboard, DashboardType dashboardType, Int32 idContainerCommunity, bool initialize)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            View.IdDashboard = idDashboard;
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
                {
                liteDashboardSettings settings = CurrentManager.Get<liteDashboardSettings>(idDashboard);
                Int32 idDashboardCommunity = (settings == null) ? ((dashboardType == DashboardType.Community) ? idContainerCommunity : 0) : settings.IdCommunity;
                List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>> steps = Service.GetAvailableSteps(WizardDashboardStep.HomepageSettings, idDashboard,   (settings == null) ? dashboardType :settings.Type , idDashboardCommunity);
                View.LoadWizardSteps(steps);

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
                    View.IdContainerCommunity = idDashboardCommunity;
                    ModuleDashboard permissions = (dashboardType == DashboardType.Community) ? Service.GetPermission(idDashboardCommunity) : ModuleDashboard.CreatePortalmodule(p.TypeID);
                    if (permissions.Edit || permissions.Administration)
                        View.SetBackUrl(RootObject.DashboardList(dashboardType, false, idContainerCommunity));
                    if (settings.Deleted != BaseStatusDeleted.None)
                        View.DisplayDeletedDashboard();
                    else if (permissions.Edit || permissions.Administration)
                    {
                        View.AllowSave = (settings.Deleted == BaseStatusDeleted.None);
                        View.SetPreviewUrl(RootObject.DashboardPreview(settings.Id, settings.Type, settings.IdCommunity));
                        View.LoadSettings(new dtoViewSettings(settings), initialize);
                        View.SendUserAction(idContainerCommunity, Service.ServiceModuleID(), settings.Id, ModuleDashboard.ActionType.DashboardSettingsViewsStartEditing);
                    }
                    else
                        View.DisplayNoPermission(idContainerCommunity, CurrentIdModule);
                }
            }
        }
        public void SaveViews(long idDashboard, dtoViewSettings dto)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = ModuleDashboard.ActionType.DashboardSettingsViewsUnableToSave;
                if (dto.Pages.Count > 0 && dto.Pages.Where(p => p.Type != DashboardViewType.Combined && p.Type != DashboardViewType.Tile && !p.Range.IsValid(p.MaxItems)).Any())
                {
                    View.DisplayRangeError(dto, dto.Pages.Where(p => p.Type != DashboardViewType.Combined && p.Type != DashboardViewType.Tile && !p.Range.IsValid(p.MaxItems)).Select(p => p).ToList());
                }
                else if (!dto.Pages.Where(p => p.Type != DashboardViewType.Search && p.Type != DashboardViewType.Subscribe).Any())
                    View.DisplayNoPageError();
                else if (dto.Pages.Where(p => p.Type == DashboardViewType.Tile).Any() && dto.Pages.Where(p => p.Type == DashboardViewType.Tile && !dto.Pages.Where(d => d.Type == p.TileRedirectOn).Any()).Any())
                    View.DisplayTileUnableToRedirectOn();
                else
                {
                    DashboardSettings settings = CurrentManager.Get<DashboardSettings>(idDashboard);
                    if (settings == null)
                    {
                        View.DisplayDeletedDashboard();
                        View.SendUserAction(View.IdContainerCommunity, CurrentIdModule, idDashboard, action);
                        return;
                    }
                    else
                    {
                        try
                        {
                            DashboardSettings item = Service.SaveViewSettings(settings, dto);
                            if (item != null)
                            {
                                action = ModuleDashboard.ActionType.DashboardSettingsViewsSaved;
                                View.LoadWizardSteps(Service.GetAvailableSteps(WizardDashboardStep.HomepageSettings, idDashboard, item.Type, View.IdContainerCommunity));
                            }

                            //Per aggiornare gli switch...
                            InitView(View.PreloadIdDashboard, View.DashboardType, View.IdContainerCommunity, false);

                            View.DisplayMessage(action);
                        }
                        catch (DashboardException ex)
                        {
                            View.DisplayMessage(ex.ErrorType);
                        }
                    }
                }
                View.SendUserAction(View.IdContainerCommunity, CurrentIdModule, idDashboard, action);
            }

        }
    }
}