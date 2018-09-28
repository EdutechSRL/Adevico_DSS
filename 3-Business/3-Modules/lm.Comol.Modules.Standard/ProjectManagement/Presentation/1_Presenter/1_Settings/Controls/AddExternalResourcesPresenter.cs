using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Modules.Standard.ProjectManagement.Business;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public class AddExternalResourcesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewAddExternalResources View
            {
                get { return (IViewAddExternalResources)base.View; }
            }
            private ServiceProjectManagement Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceProjectManagement(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleProjectManagement.UniqueCode);
                    return currentIdModule;
                }
            }
            public AddExternalResourcesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddExternalResourcesPresenter(iApplicationContext oContext, IViewAddExternalResources view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 rows, Boolean displayMail)
            { 
            View.Rows= (rows>0) ? rows : 1;
            View.DisplayRows(rows, displayMail, !UserContext.isAnonymous);
        }

        public List<ProjectResource> AddResources(long idProject,List<dtoExternalResource> items,ProjectVisibility visibility,ActivityRole role, ref Int32 count) {
            count = items.Where(i => !string.IsNullOrEmpty(i.LongName) || !string.IsNullOrEmpty(i.ShortName)).Count();
            if (UserContext.isAnonymous){
                View.DisplayRows(View.Rows, View.AllowMail, UserContext.isAnonymous);
                return null;
            }
            else
                return Service.AddExternalResources(idProject,items,visibility,role);
        }
    }
}