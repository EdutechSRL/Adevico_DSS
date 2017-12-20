using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class AgenciesManagementPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewAgenciesManagement View
            {
                get { return (IViewAgenciesManagement)base.View; }
            }
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            public AgenciesManagementPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AgenciesManagementPresenter(iApplicationContext oContext, IViewAgenciesManagement view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView()
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                    View.AllowImport = module.Administration;
                    if (module.ViewProfiles || module.Administration)
                        InitializeFilters();
                    else
                        View.NoPermission();
                }
            }

            #region "ManageFilters"
                private void InitializeFilters()
                {
                    View.OrderAscending = true;
                    View.OrderBy = OrderAgencyBy.Name;
                    Boolean loadFilters = View.PreloadedReloadFilters;
                    dtoAgencyFilters filters = null;
                    if (loadFilters)
                        filters = View.GetSavedFilters;

                    SearchAgencyBy defaultSearch = SearchAgencyBy.Contains;
                    AgencyAvailability dAvailability = AgencyAvailability.All;
                    int pageIndex = 0;
                    if (loadFilters && filters != null)
                    {
                        defaultSearch = filters.SearchBy;
                        dAvailability = filters.Availability;
                        if (!String.IsNullOrEmpty(filters.StartWith) && !Service.DefaultChars().Contains(filters.StartWith))
                            filters.StartWith = "#";
                        View.CurrentStartWith = filters.StartWith;
                        View.CurrentValue = filters.Value;
                        View.OrderAscending = filters.Ascending;
                        View.OrderBy = filters.OrderBy;
                        View.SelectedAvailability = filters.Availability;
                        pageIndex = filters.PageIndex;
                        View.CurrentPageSize = filters.PageSize;
                    }
                    else
                    {
                        View.CurrentPageSize = View.PreLoadedPageSize;
                        if (filters == null)
                        {
                            filters = View.GetCurrentFilters;
                            View.InitializeWordSelector(Service.GetAvailableStartLetter(View.GetCurrentFilters));
                        }
                    }
                    View.LoadSearchAgenciesBy(GetSearchByItems(),defaultSearch);
                    View.LoadAgencyAvailability(GetAvailabilityItems(), dAvailability);
                    
                    View.SearchFilters = filters;
                    if (loadFilters && filters != null)
                        LoadAgencies(filters,filters.PageIndex, filters.PageSize);
                    else if (filters==null)
                        View.InitializeWordSelector(Service.GetAvailableStartLetter(View.GetCurrentFilters));
                }

                private List<SearchAgencyBy> GetSearchByItems()
                {
                    List<SearchAgencyBy> list = new List<SearchAgencyBy>();
                    list.Add(SearchAgencyBy.All);
                    list.Add(SearchAgencyBy.Contains);
                    list.Add(SearchAgencyBy.Name);
                    list.Add(SearchAgencyBy.TaxCode);
                    list.Add(SearchAgencyBy.NationalCode);
                    list.Add(SearchAgencyBy.ExternalCode);

                    return list;
                }
                private List<AgencyAvailability> GetAvailabilityItems()
                {
                    List<AgencyAvailability> list = new List<AgencyAvailability>();
                    list.Add(AgencyAvailability.All);
                    list.Add(AgencyAvailability.Default);
                    list.Add(AgencyAvailability.Empty);
                    return list;
                }
                public void LoadAgencies(dtoAgencyFilters filters ,int currentPageIndex, int currentPageSize)
                {
                    PagerBase pager = new PagerBase();
                    if (filters.StartWith != View.CurrentStartWith)
                        filters.StartWith = View.CurrentStartWith;

                    pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
                    pager.Count = (int)Service.AgenciesCount(filters) - 1;
                    pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
                    View.Pager = pager;
                    View.InitializeWordSelector(Service.GetAvailableStartLetter(filters), filters.StartWith);


                    List<dtoAgencyItem> items = Service.GetAgencies(filters, pager.PageIndex, currentPageSize);
                    View.LoadAgencies(items);
                }
                public void SearchAgencies(dtoAgencyFilters filters, int currentPageIndex, int currentPageSize)
                {
                    if (UserContext.isAnonymous)
                        View.DisplaySessionTimeout();
                    else
                        LoadAgencies(filters,currentPageIndex, currentPageSize);
                }
            #endregion

                public void Recover(long idAgency,int currentPageIndex, int currentPageSize) {
                    if (UserContext.isAnonymous)
                        View.DisplaySessionTimeout();
                    else
                    {
                        Service.VirtualDeleteAgency(idAgency, false);
                        LoadAgencies(View.SearchFilters, currentPageIndex, currentPageSize);
                    }
                }
                public void VirtualDelete(long idAgency, int currentPageIndex, int currentPageSize)
                {
                    if (UserContext.isAnonymous)
                        View.DisplaySessionTimeout();
                    else
                    {
                        Service.VirtualDeleteAgency(idAgency, true);
                        LoadAgencies(View.SearchFilters, currentPageIndex, currentPageSize);
                    }
                }

    }
}