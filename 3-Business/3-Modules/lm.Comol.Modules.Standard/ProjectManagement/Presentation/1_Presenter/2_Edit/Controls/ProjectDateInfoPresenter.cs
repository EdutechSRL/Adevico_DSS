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
    public class ProjectDateInfoPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewProjectDateInfo View
            {
                get { return (IViewProjectDateInfo)base.View; }
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
            public ProjectDateInfoPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProjectDateInfoPresenter(iApplicationContext oContext, IViewProjectDateInfo view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idProject) {
            dtoProject project = Service.GetdtoProject(idProject);
            if (project == null)
                View.DisplayUnknownProject();
            else
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, project.IdCommunity, CurrentIdModule));
                PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                InitView(project, ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageProject))) &&!(UserContext.isAnonymous), true);
            }
        }
        public void InitView(long idProject, Boolean allowEdit) {
            dtoProject project = Service.GetdtoProject(idProject);
            if (project == null)
                View.DisplayUnknownProject();
            else
                InitView(project, allowEdit, !(UserContext.isAnonymous));
        }
       
        public void InitView(dtoProject project, System.Globalization.CultureInfo culture, String currentShortDatePattern, Boolean allowEdit) {
            View.LoaderCultureInfo = culture;
            View.CurrentShortDatePattern = currentShortDatePattern;
            InitView(project, allowEdit,false);
        }
        public void InitView(dtoProject project, Boolean allowEdit,Boolean setCulture) {
            View.AllowEdit = allowEdit && !UserContext.isAnonymous;
            View.LoadProjectInfo(project, setCulture);
        }
    }
}