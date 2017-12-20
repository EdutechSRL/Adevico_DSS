using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class CommunityDetailsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities service;
            private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewCommunityDetails View
            {
                get { return (IViewCommunityDetails)base.View; }
            }
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities Service
            {
                get
                {
                    if (service == null)
                        service = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.Tag.Business.ServiceTags ServiceTags
            {
                get
                {
                    if (servicetag == null)
                        servicetag = new lm.Comol.Core.Tag.Business.ServiceTags(AppContext);
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
            public CommunityDetailsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommunityDetailsPresenter(iApplicationContext oContext, IViewCommunityDetails view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        #region "Initalize View"
            public void InitView(liteCommunityInfo community)
            {

                View.LoadUserInfo( Service.GetResponsible(community.Id), (community.IdCreatedBy > 0) ? CurrentManager.GetLitePerson(community.IdCreatedBy) : null);
                Int32 idLanguage = UserContext.Language.Id;
                Language l = CurrentManager.GetDefaultLanguage();
                List<String> tags = ServiceTags.GetCommunityAssociationToString(community.Id,idLanguage, l.Id, true);
                if (tags != null && tags.Any())
                    View.LoadTags(tags, community.IdTypeOfCommunity);

                List<dtoEnrollmentsDetailInfo> items = Service.GetEnrollmentsInfo(community.Id,community.IdTypeOfCommunity, UserContext.Language.Id);
                View.LoadEnrollmentsInfo(community,items, items.Select(i => i.Count).Sum(), Service.GetWaitingEnrollments(community.Id));
                View.LoadConstraints(Service.GetDtoCommunityConstraints(community.Id, UserContext.CurrentUserID ));
                View.LoadDetails(community,  Service.GetTranslatedCommunityType(idLanguage, community.IdTypeOfCommunity), Service.GetDescription(community.Id));
            }
        #endregion
    }
}