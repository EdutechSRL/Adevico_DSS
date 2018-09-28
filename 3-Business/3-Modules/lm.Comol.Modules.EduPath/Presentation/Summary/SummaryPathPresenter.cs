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
    public class SummaryPathPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewSummaryPath View
            {
                get { return (IViewSummaryPath)base.View; }
            }
            public SummaryPathPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SummaryPathPresenter(iApplicationContext oContext, IViewSummaryPath view)
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
                SummaryType type = View.PreloadFromSummary;

                if (type != SummaryType.PortalIndex && idCommunity <= 0)
                    type = SummaryType.PortalIndex;
                else if (type != SummaryType.PortalIndex){
                    type= SummaryType.CommunityIndex;
                    if (idCommunity == 0)
                        idCommunity = UserContext.CurrentCommunityID;
                }
                View.FromSummary = type;
                View.SummaryIdCommunity = idCommunity;
                // VERIFICA PERMESSI
                litePerson currentUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                ModuleEduPath mEduPath = PathService.ServiceStat.GetPermissionForSummary(type, idCommunity, currentUser);
                if (mEduPath.Administration || mEduPath.ViewMyStatistics)
                {
                    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.ViewSummaryPath);
                    View.AllowOrganizationSelection = (currentUser != null && (currentUser.TypeID == (int)UserTypeStandard.Administrator || currentUser.TypeID == (int)UserTypeStandard.SysAdmin));
                    View.LoadAvailableOrganizations(UserContext.CurrentUserID);
                    View.InitializeFilters();
                }
                else
                {
                    View.DisplayWrongPageAccess(RootObject.SummaryIndex(type, idCommunity));
                    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.WrongPageAccess);
                }
            }
        }
        public void LoadData()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.InitializeFilters();
                View.SendUserAction(View.SummaryIdCommunity, IdModule, ModuleEduPath.ActionType.ViewSummaryPath);
            }
        }
        public ModuleEduPath GetModulePermission(Int32 idCommunity) { 
            return new ModuleEduPath(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, IdModule));
        }
    }
}