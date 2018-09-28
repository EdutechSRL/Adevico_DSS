using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Modules.EduPath.BusinessLogic;

namespace lm.Comol.Modules.EduPath.Presentation

{
    public class SummaryCommunityPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _IdModule;
            private Service _Service;
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService _ProfileService;
            private int IdModule
            {
                get
                {
                    if (_IdModule <= 0)
                    {
                        _IdModule = this.PathService.ServiceModuleID();
                    }
                    return _IdModule;
                }
            }
            
            private Service PathService
            {
                get
                {
                    if (_Service == null)
                        _Service = new Service(AppContext);
                    return _Service;
                }
            }
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (_ProfileService == null)
                        _ProfileService = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext);
                    return _ProfileService;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSummaryCommunity View
            {
                get { return (IViewSummaryCommunity)base.View; }
            }
            public SummaryCommunityPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SummaryCommunityPresenter(iApplicationContext oContext, IViewSummaryCommunity view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Int32 idCommunity = View.SummaryIdCommunity;
                SummaryType fromType = View.PreloadFromSummary;
                SummaryType current = View.PreloadSummaryType;

                if (fromType != SummaryType.PortalIndex && idCommunity <= 0)
                    fromType = SummaryType.PortalIndex;
                else if (fromType != SummaryType.PortalIndex)
                {
                    fromType = SummaryType.CommunityIndex;
                    if (idCommunity == 0)
                        idCommunity = UserContext.CurrentCommunityID;
                }
                View.FromSummary = fromType;
                View.SummaryIdCommunity = idCommunity;

                if (current != SummaryType.Community && current != SummaryType.Organization)
                    current = SummaryType.Community;
                View.CurrentSummaryType = current;

                if (current == SummaryType.Organization && fromType != SummaryType.PortalIndex)
                {
                    View.DisplayWrongPageAccess(RootObject.SummaryIndex(SummaryType.CommunityIndex, idCommunity));
                    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.WrongPageAccess);
                }
                else
                {
                    // VERIFICA PERMESSI
                    litePerson currentUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                    ModuleEduPath mEduPath = PathService.ServiceStat.GetPermissionForSummary(current, idCommunity, currentUser);
                    if (mEduPath.Administration || mEduPath.ViewMyStatistics || mEduPath.ViewMyStatistics)
                    {
                        View.SendUserAction(idCommunity, IdModule, (current== SummaryType.Community) ? ModuleEduPath.ActionType.ViewSummaryCommunity : ModuleEduPath.ActionType.ViewSummaryOrganization);

                        List<Organization> organizations = ProfileService.GetAvailableOrganizations(UserContext.CurrentUserID, SearchCommunityFor.SystemManagement);
                        switch (current)
                        {
                            case SummaryType.Organization:
                                View.AllowOrganizationSelection = (currentUser != null && ((organizations.Any() && currentUser.TypeID == (int)UserTypeStandard.Administrative) || currentUser.TypeID == (int)UserTypeStandard.Administrator || currentUser.TypeID == (int)UserTypeStandard.SysAdmin));
                                break;
                            default:
                                View.AllowOrganizationSelection = false;
                                break;

                        }
                        if (!organizations.Any())
                            organizations = ProfileService.GetAvailableOrganizations(UserContext.CurrentUserID, SearchCommunityFor.Subscribed);
                        View.LoadAvailableOrganizations(organizations);
                        View.InitializeFilters(current);
                    }
                    else
                    {
                        View.DisplayWrongPageAccess(RootObject.SummaryIndex(fromType, idCommunity));
                        View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.WrongPageAccess);
                    }
                }
               
            }
        }
        //public void LoadData(SummaryType current)
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else
        //    {
        //        View.InitializeFilters(current);
        //        View.SendUserAction(View.SummaryIdCommunity, IdModule, (current == SummaryType.Community) ? ModuleEduPath.ActionType.ViewSummaryCommunity : ModuleEduPath.ActionType.ViewSummaryOrganization);
        //    }
        //}
        public ModuleEduPath GetModulePermission(Int32 idCommunity) { 
            return new ModuleEduPath(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, IdModule));
        }
    }
}