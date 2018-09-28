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
    public class AddProjectPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewAddProject View
            {
                get { return (IViewAddProject)base.View; }
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
            public AddProjectPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddProjectPresenter(iApplicationContext oContext, IViewAddProject view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            dtoProjectContext cContext = new dtoProjectContext() { IdCommunity= View.PreloadIdCommunity, isPersonal = View.PreloadIsPersonal, isForPortal = View.PreloadForPortal};
            InitializeContext(cContext);
           
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                View.SetProjectsUrl(Service.GetBackUrl(View.PreloadFromPage,View.PreloadIdContainerCommunity,0));
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>> steps = Service.GetAvailableSteps(WizardProjectStep.Settings, 0); //new List<Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>>();   //'Service.GetAvailableSteps(0, WizardTemplateStep.Settings, ownerInfo.Type);
                View.LoadWizardSteps(0, cContext.IdCommunity, cContext.isPersonal, cContext.isForPortal, steps);
                if (!cContext.isForPortal && !cContext.isValid)
                    View.DisplayNotAvailableComunity();
                else
                {
                    ModuleProjectManagement mPermission = Service.GetModulePermissions(cContext.IdCommunity);
                    if (cContext.isPersonal && !mPermission.CreatePersonalProject)
                    {
                        View.DisplayNotAvailableForAddPersonalProject();
                        View.SendUserAction(cContext.IdCommunity, CurrentIdModule, 0, ModuleProjectManagement.ActionType.ProjectTryToAdd);
                    }
                    else if (!cContext.isPersonal && !mPermission.CreatePublicProject)
                    {
                        if (cContext.IdCommunity > 0)
                            View.DisplayNotAvailableForAddCommunityProject();
                        else
                            View.DisplayNotAvailableForAddPortalProject();
                        View.SendUserAction(cContext.IdCommunity, CurrentIdModule, 0, ModuleProjectManagement.ActionType.ProjectTryToAdd);
                    }
                    else if (p != null && (mPermission.CreatePublicProject || mPermission.CreatePersonalProject))
                    {
                        View.AllowAdd = true;
                        dtoProject project = new dtoProject();
                        project.IdCommunity = cContext.IdCommunity;
                        project.isPersonal = cContext.isPersonal;
                        project.isPortal = cContext.isPersonal;
                        project.Name = View.GetDefaultProjectName() + DateTime.Now.Date.ToShortDateString();
                        project.StartDate = DateTime.Now.Date;
                        project.Status = ProjectItemStatus.notstarted;
                        project.Availability = ProjectAvailability.Draft;
                        project.AllowEstimatedDuration = true;
                        project.AllowMilestones = true;
                        project.AllowSummary = true;
                        List<ProjectAvailability> items = new List<ProjectAvailability>();
                        items.Add(ProjectAvailability.Draft);
                        dtoResource resource = new dtoResource() { IdResource = 0, LongName = p.SurnameAndName, ShortName = GetPersonShortName(p), IdPerson = p.Id, ProjectRole= ActivityRole.ProjectOwner };
                        project.Resources = new List<dtoResource>();
                        project.Resources.Add(resource);
                        View.InitializeProject(project, resource, items, ProjectAvailability.Draft,10);
                        View.SendUserAction(cContext.IdCommunity, CurrentIdModule, 0, ModuleProjectManagement.ActionType.ProjectStartAdding);
                    }
                    else
                        View.DisplayNoPermission(cContext.IdCommunity, CurrentIdModule);     
                }                     
            }
        }
        private void InitializeContext(dtoProjectContext cContex)
        {
            Int32 idCommunity = (!cContex.isForPortal && cContex.IdCommunity < 1) ? UserContext.CurrentCommunityID :  cContex.IdCommunity;
            Community community = (idCommunity > 0) ? CurrentManager.GetCommunity(idCommunity) : null;

            cContex.IdCommunity = (community != null) ? community.Id : 0;

            View.ProjectIdCommunity = cContex.IdCommunity;
            View.forPortal = cContex.isForPortal;
            View.isPersonal = cContex.isPersonal;
        }

        private String GetPersonShortName(Person p)
        {
            String name = p.Name.Trim();
            return ((name.Length > 0) ? name[0].ToString() : p.Id.ToString()) + p.FirstLetter;
        }
        public void AddProject(dtoProject dto, Int32 activitiesToAdd)
        {
            if (UserContext.isAnonymous) 
                View.DisplaySessionTimeout();
            else
            {
                if (dto == null){
                    View.DisplayProjectAddError();
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, 0, ModuleProjectManagement.ActionType.ProjectTryToAdd);
                }
                else if (!dto.StartDate.HasValue || dto.DaysOfWeek == FlagDayOfWeek.None)
                {
                    View.DisplayProjectAddError(dto.StartDate, dto.DaysOfWeek);
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, 0, ModuleProjectManagement.ActionType.ProjectTryToAdd);
                }
                else {
                    Project project = Service.AddProject(dto, View.ProjectIdCommunity, View.forPortal, View.isPersonal, View.GetDefaultCalendarName());
                    if (project == null)
                    {
                        View.DisplayProjectAddError();
                        View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, 0, ModuleProjectManagement.ActionType.ProjectTryToAdd);
                    }
                    else {
                         View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, project.Id, ModuleProjectManagement.ActionType.ProjectAdded);
                        if (activitiesToAdd > 0)
                        {
                            List<PmActivity> activities = Service.AddTaks(project, View.GetDefaultActivityName(), activitiesToAdd);
                            View.RedirectToEdit(project.Id, (project.Community != null) ? project.Community.Id : 0, project.isPortal, project.isPersonal, activitiesToAdd, activities.Count);
                        }
                        else
                            View.RedirectToEdit(project.Id, (project.Community != null) ? project.Community.Id : 0, project.isPortal, project.isPersonal);

                    }
                }
            }
        }
    }
}