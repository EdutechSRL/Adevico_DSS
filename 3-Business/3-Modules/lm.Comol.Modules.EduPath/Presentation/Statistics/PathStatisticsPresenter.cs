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
    public class PathStatisticsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _IdModule;
            private Service _Service;
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
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewPathStatistics View
            {
                get { return (IViewPathStatistics)base.View; }
            }
            public PathStatisticsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PathStatisticsPresenter(iApplicationContext oContext, IViewPathStatistics view)
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
                Int32 idCommunity = View.PreloadIdCommunity;
                long idPath = View.PreloadIdPath;
                DateTime? dateToView = View.PreloadDateToView;

                SummaryType fromSummary = View.PreloadFromSummary;
                SummaryType fromIndex = View.PreloadFromSummaryIndex;
                fromSummary = (fromSummary== SummaryType.CommunityIndex || fromSummary== SummaryType.PortalIndex)? SummaryType.Unknown :fromSummary;

                // VENGO DA UN SOMMARIO ?
                View.CurrentFromSummary = fromSummary;
                View.CurrentFromSummaryIdCommunity = View.PreloadFromSummaryIdCommunity;
                View.CurrentSummaryIndex = (fromIndex == SummaryType.CommunityIndex || fromIndex == SummaryType.PortalIndex) ? fromIndex : (View.PreloadFromSummaryIdCommunity > 0) ? SummaryType.CommunityIndex : SummaryType.PortalIndex;

                if (fromSummary != SummaryType.Unknown)
                {
                    // View.SetBackUrl(RootObject.SummaryGeneric 
                }
                else { 
                    //View.SetPathListUrl(RootObject.EduPathList(
                }


                
                //// VERIFICA PERMESSI
                //Person cUser = CurrentManager.GetPerson(UserContext.CurrentUserID);
                //ModuleEduPath mEduPath = PathService.ServiceStat.GetPermissionForSummary(type, idCommunity, cUser);
                //if (mEduPath.Administration || mEduPath.ViewMyStatistics)
                //{
                //    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.ViewSummaryPath);
                //    View.AllowOrganizationSelection = (cUser != null && (cUser.TypeID == (int)UserTypeStandard.Administrator || cUser.TypeID == (int)UserTypeStandard.SysAdmin));
                //    View.LoadAvailableOrganizations(UserContext.CurrentUserID);
                //    View.InitializeFilters();
                //}
                //else
                //{
                //    View.DisplayWrongPageAccess(RootObject.SummaryIndex(type, idCommunity));
                //    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.WrongPageAccess);
                //}
            }
        }
        public void LoadData()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                //View.InitializeFilters();
                //View.SendUserAction(View.SummaryIdCommunity, IdModule, ModuleEduPath.ActionType.ViewSummaryPath);
            }
        }
        public ModuleEduPath GetModulePermission(Int32 idCommunity) { 
            return new ModuleEduPath(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, IdModule));
        }
    }
}