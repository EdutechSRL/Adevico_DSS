using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Tag.Domain;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public class TagsManagerPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewPageBase View
            {
                get { return (IViewPageBase)base.View; }
            }
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceCommunities
            {
                get
                {
                    if (serviceCommunities == null)
                        serviceCommunities = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceCommunities;
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
                        currentIdModule = CurrentManager.GetModuleID(ModuleTags.UniqueCode);
                    return currentIdModule;
                }
            }
            public TagsManagerPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TagsManagerPresenter(iApplicationContext oContext, IViewPageBase view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean forOrganization, Boolean fromRecycleBin)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                ModuleTags permissions = null;
                Int32 idCommunity = 0;
                if (forOrganization)
                {
                    idCommunity = UserContext.CurrentCommunityID;
                    //if (idCommunity > 0)
                    //    idOrganization = CurrentManager.GetIdOrganizationFromCommunity(UserContext.CurrentCommunityID);
                    //else
                    //    idOrganization = CurrentManager.GetUserDefaultIdOrganization(UserContext.CurrentUserID);
                    permissions = ServiceTags.GetPermission(idCommunity);
                }
                else
                    permissions = ModuleTags.CreatePortalmodule(p.TypeID);
                View.IdTagsCommunity = idCommunity;
                if (permissions.Administration || permissions.Edit || permissions.List)
                {
                    View.AllowAdd((permissions.Administration || permissions.Add));
                    View.AllowMultiple((p.TypeID == (Int32)UserTypeStandard.Administrator || p.TypeID == (Int32)UserTypeStandard.Administrative || p.TypeID==(Int32)UserTypeStandard.SysAdmin) && (permissions.Administration || permissions.Add));
                    if (permissions.DeleteMy || permissions.DeleteOther || permissions.Administration)
                    {
                        if (fromRecycleBin)
                            View.SetBackUrl(RootObject.List(false, forOrganization));
                        else
                            View.SetRecycleUrl(RootObject.List(true, forOrganization));
                    }
                    View.InitializeListControl(permissions,idCommunity, fromRecycleBin, forOrganization);
                }
                else
                    View.DisplayNoPermission(idCommunity, CurrentIdModule);
            }
        }
    }
}