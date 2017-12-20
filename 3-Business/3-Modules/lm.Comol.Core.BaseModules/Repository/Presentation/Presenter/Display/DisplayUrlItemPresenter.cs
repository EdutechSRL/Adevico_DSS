using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.ModuleLinks;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class DisplayUrlItemPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private ServiceCommunityRepository _Service;
            private int ModuleID
            {
                get
                {
                    if (_ModuleID <= 0)
                    {
                        _ModuleID = this.Service.ServiceModuleID();
                    }
                    return _ModuleID;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewDisplayUrlItem View
            {
                get { return (IViewDisplayUrlItem)base.View; }
            }
            private ServiceCommunityRepository Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCommunityRepository(AppContext);
                    return _Service;
                }
            }
            public DisplayUrlItemPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DisplayUrlItemPresenter(iApplicationContext oContext, IViewDisplayUrlItem view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(DisplayActionMode mode, String url, String name = "", DateTime? createdOn = null, Person createdBy = null, List<dtoPlaceHolder> placeHolders = null)
            {
                InitializeControl(mode, url, name, createdOn, createdBy,null, placeHolders);
            }
            public void InitViewLite(DisplayActionMode mode, String url, String name = "", DateTime? createdOn = null, litePerson createdBy = null, List<dtoPlaceHolder> placeHolders = null)
            {
                InitializeControl(mode, url, name, createdOn, null, createdBy, placeHolders);
            }
            private void InitializeControl(DisplayActionMode mode, String url, String name = "", DateTime? createdOn = null, Person personCreator = null, litePerson litePersonCreator = null, List<dtoPlaceHolder> placeHolders = null)
            {
                if (String.IsNullOrEmpty(url) && String.IsNullOrEmpty(name))
                    View.DisplayEmptyAction();
                else{
                    if (!String.IsNullOrEmpty(url) && !Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                    {
                        if (Uri.IsWellFormedUriString("http://" + url, UriKind.RelativeOrAbsolute))
                            url = "http://" + url;
                        else
                            url = "";
                    }
                    else {
                        try
                        {
                            var uri = new Uri(url);
                        }
                        catch (Exception ex) {
                            url = "http://" + url; 
                        }
                    }
                   

                    String username = "";
                    String surname = "";
                    if (View.DisplayCreateInfo)
                    {

                        if (litePersonCreator == null && personCreator == null)
                            surname = View.GetRemovedUser;
                        else if (litePersonCreator != null)
                        {
                            if (litePersonCreator != null && litePersonCreator.TypeID != (int)UserTypeStandard.Guest)
                            {
                                username = litePersonCreator.Name;
                                surname = litePersonCreator.Surname;
                            }
                            else
                                surname = View.GetUnknownUser;
                        }
                        else if (personCreator != null && personCreator.TypeID != (int)UserTypeStandard.Guest)
                        {
                            username = personCreator.Name;
                            surname = personCreator.Surname;
                        }
                        else
                            surname = View.GetUnknownUser;
                    }
                    if (String.IsNullOrEmpty(name))
                        name = url;

                    if (String.IsNullOrEmpty(url)){
                        mode = DisplayActionMode.text;
                        View.DisplayItem(name, createdOn, username, surname);
                    }
                    else
                        View.DisplayItem(name, url, createdOn, username, surname);
                    if (placeHolders!= null && placeHolders.Where(p => !String.IsNullOrEmpty(p.Text)).Any() && (Display(mode, DisplayActionMode.defaultAction) || Display(mode, DisplayActionMode.text)))
                        View.DisplayPlaceHolders(placeHolders.Where(p => !String.IsNullOrEmpty(p.Text)).ToList());
                }
            }

            private Boolean Display(DisplayActionMode current, DisplayActionMode required)
            {
                return ((long)current & (long)required) > 0;
            }
    }
}