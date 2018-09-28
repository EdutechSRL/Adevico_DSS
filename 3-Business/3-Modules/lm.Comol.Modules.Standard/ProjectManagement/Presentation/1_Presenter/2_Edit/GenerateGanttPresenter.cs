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
    public class GenerateGanttPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewGenerateGantt View
            {
                get { return (IViewGenerateGantt)base.View; }
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
            public GenerateGanttPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public GenerateGanttPresenter(iApplicationContext oContext, IViewGenerateGantt view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion


        public void InitView(long idProject,String formatDateString, Dictionary<GanttCssClass,String> cssClass, String baseUrl)
        {
            dtoProject project = Service.GetdtoProject(idProject);
            if (project == null)
                View.GenerateXML(new ProjectForGantt());
            else
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, project.IdCommunity, CurrentIdModule));
                PmActivityPermission pPermissions = Service.GetProjectPermission(idProject, UserContext.CurrentUserID);
                ProjectResource resource = Service.GetResource(idProject, UserContext.CurrentUserID);

                if ((mPermission.Administration && !project.isPersonal) || (pPermissions.HasFlag(PmActivityPermission.ManageProject)))
                    View.GenerateXML(Service.GetActivitiesForGantt(idProject,resource, ProjectVisibility.Full,formatDateString, cssClass,baseUrl));
                else
                {
                    if (pPermissions.HasFlag(PmActivityPermission.ViewProjectMap) && (resource != null && (resource.Visibility == ProjectVisibility.Full || resource.Visibility == ProjectVisibility.InvolvedTasks)))
                        View.GenerateXML(Service.GetActivitiesForGantt(idProject, resource, resource.Visibility, formatDateString, cssClass, baseUrl));
                    else
                        View.GenerateXML(new ProjectForGantt());
                }
            }
        }
    }
}