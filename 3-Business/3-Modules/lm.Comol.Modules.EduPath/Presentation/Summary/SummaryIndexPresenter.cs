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
    public class SummaryIndexPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewSummaryIndex View
            {
                get { return (IViewSummaryIndex)base.View; }
            }
            public SummaryIndexPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SummaryIndexPresenter(iApplicationContext oContext, IViewSummaryIndex view)
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
                SummaryType type = View.PreloadSummaryType;
                switch (type) { 
                    case SummaryType.PortalIndex:
                        idCommunity = 0;
                        break;
                    case SummaryType.CommunityIndex:
                        if (idCommunity==0)
                            idCommunity = UserContext.CurrentCommunityID;
                        break;
                    default:
                        type = SummaryType.Unknown;
                        break;
                }
                //if (idCommunity == 0 && type == SummaryType.PortalIndex)
                //    type = SummaryType.PortalIndex;
                View.SummaryType = type;
                View.SummaryIdCommunity = idCommunity;
                if (type == SummaryType.Unknown || (idCommunity == 0 && type != SummaryType.PortalIndex))
                {
                    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.NoPermissionForSummary);
                    View.DisplayWrongPageAccess();
                }
                else
                    LoadAvailableViews(type, idCommunity);
            }
        }
        private void LoadAvailableViews(SummaryType type, Int32 idCommunity) {
            List<SummaryType> items = PathService.ServiceStat.GetAvailableSummaryViews(type, idCommunity, UserContext.CurrentUserID);

            if (items.Any())
                View.LoadAvailableItems(items);
            else
                View.DisplayNoPermission();
            View.SendUserAction(idCommunity, IdModule, (items.Any()) ? ModuleEduPath.ActionType.ViewSummaryIndex : ModuleEduPath.ActionType.NoPermissionForSummary);
        }
    }
}