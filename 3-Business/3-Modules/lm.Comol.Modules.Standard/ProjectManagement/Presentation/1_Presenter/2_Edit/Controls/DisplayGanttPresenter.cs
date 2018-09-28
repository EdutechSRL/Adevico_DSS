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
    public class DisplayGanttPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewDisplayGantt View
            {
                get { return (IViewDisplayGantt)base.View; }
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
            public DisplayGanttPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DisplayGanttPresenter(iApplicationContext oContext, IViewDisplayGantt view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion


        public void InitView(long idProject)
        {
            dtoProject project = Service.GetdtoProject(idProject);
            if (project == null)
                View.DisplayUnknownProject();
            else
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, project.IdCommunity, CurrentIdModule));
                PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);

                if ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageProject)))
                    View.LoadGantt(RootObject.GetGanttXML(project.Id, false,  project.IdCommunity, project.isPortal, project.isPersonal));
                else
                {
                    ProjectResource resource = Service.GetResource(idProject, UserContext.CurrentUserID);
                    if (pPermissions.HasFlag(PmActivityPermission.ViewProjectMap) && (resource != null && (resource.Visibility == ProjectVisibility.Full || resource.Visibility == ProjectVisibility.InvolvedTasks)))
                        View.LoadGantt(RootObject.GetGanttXML(project.Id, false,  project.IdCommunity, project.isPortal, project.isPersonal));
                    else
                        View.DisplayNoPermissionToSeeProjectGantt();
                }
            }
        }
        public void InitView(dtoProject project, String encodedFormatDatePattern)
        {
            View.LoadGantt(RootObject.GetGanttXML(project.Id, false, project.IdCommunity, project.isPortal, project.isPersonal));
        }
    }
}