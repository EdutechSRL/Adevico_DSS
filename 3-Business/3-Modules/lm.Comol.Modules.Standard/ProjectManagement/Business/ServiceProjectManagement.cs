using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Business
{
    public partial class ServiceProjectManagement : BaseCoreServices
    {
        protected const int maxItemsForQuery = 100;
        protected iApplicationContext _Context;

        #region initClass
            private Int32 idModule ;
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement ServiceCommunityManagement;
            public ServiceProjectManagement() :base() { }
            public ServiceProjectManagement(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
                ServiceCommunityManagement = new Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(oContext);
            }
            public ServiceProjectManagement(iDataContext oDC)
                : base(oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
                ServiceCommunityManagement = new Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(oDC);
            }
        #endregion

        #region Wizard
            public List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>> GetAvailableSteps(WizardProjectStep current, long idProject)
            { 
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>>();
                WizardProjectStep startStep = WizardProjectStep.Settings;
                Project project = (idProject == 0) ? null : Manager.Get<Project>(idProject);

                if (current == WizardProjectStep.None)
                    current = startStep;

                items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>()
                {
                    DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.first,
                    Id = new dtoSettingsStep(WizardProjectStep.Settings, project), 
                    AutoPostBack=false,
                    Status= (project==null) ? Core.Wizard.WizardItemStatus.none :  ((project.Availability== ProjectAvailability.Active) ?  Core.Wizard.WizardItemStatus.valid : Core.Wizard.WizardItemStatus.warning), //se il progetto non è nullo metto valido? puo andare?
                    Active = (idProject==0) || (current == WizardProjectStep.Settings) 
                });

                dtoResourcesStep rStep = GetResourcesStepInfo(project);
                items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>()
                {
                    Id = rStep,
                    AutoPostBack = false,
                    Status = rStep.Status,
                    Active = (current == WizardProjectStep.ProjectUsers),
                });


                //dtoCalendarsStep dStep = GetDateExceptionsStepInfo(project);
                //items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>()
                //{
                //    Id = dStep,
                //    AutoPostBack = false,
                //    Status = dStep.Status,
                //    Active = (current == WizardProjectStep.Calendars),
                //    DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.none
                //});

                dtoDocumentsStep aStep = GetAttachmentsStepInfo(project);
                items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoProjectManagementStep>()
                {
                    Id = aStep,
                    AutoPostBack = false,
                    Status = aStep.Status,
                    Active = (current == WizardProjectStep.Documents),
                    DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last
                });
                return items;
            }
        
            private dtoResourcesStep GetResourcesStepInfo(Project project)
            {
                dtoResourcesStep item = new dtoResourcesStep(WizardProjectStep.ProjectUsers);
                if (project != null)
                {
                    var rQuery = (from r in Manager.GetIQ<ProjectResource>()
                                                       where r.Project.Id == project.Id  && r.Deleted == BaseStatusDeleted.None
                                                       select new { Role = r.ProjectRole, Type = r.Type}).ToList();
                    item.Resources = rQuery.Where(r => r.Role== ActivityRole.Resource).Count();
                    item.Managers = rQuery.Where(r => r.Role == ActivityRole.Manager).Count();
                    item.ExternalResources = rQuery.Where(r => r.Type == ResourceType.External).Count();
                    item.InternalResources = rQuery.Where(r => r.Type == ResourceType.Internal).Count();
                    item.RemovedResources = rQuery.Where(r => r.Type == ResourceType.Removed).Count();

                }
                item.Status = (project == null) ? Core.Wizard.WizardItemStatus.disabled : ((item.Count == 0) ? Core.Wizard.WizardItemStatus.warning : ((item.Managers > 0 && item.Resources == 0) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.valid)); 
                return item;
            }
            private dtoCalendarsStep GetDateExceptionsStepInfo(Project project)
            {
                dtoCalendarsStep item = new dtoCalendarsStep(WizardProjectStep.Calendars);
                if (project != null)
                {
                    item.Calendars = (project.Calendars == null) ? 0 : project.Calendars.Where(d => d.Deleted == BaseStatusDeleted.None).Count();
                    item.Exceptions = (project.DateExceptions != null) ? 0 : project.DateExceptions.Where(d => d.Deleted == BaseStatusDeleted.None).Count();
                }
                item.Status = (project == null) ? Core.Wizard.WizardItemStatus.disabled :  Core.Wizard.WizardItemStatus.none;
                return item;
            }
            private dtoDocumentsStep GetAttachmentsStepInfo(Project project)
            {
                dtoDocumentsStep item = new dtoDocumentsStep(WizardProjectStep.Documents);
                if (project != null)
                {
                    var query = project.Attachments.Where(a => a.Deleted == BaseStatusDeleted.None);
                    item.ProjectFiles = query.Where(a => a.Type == AttachmentType.file && a.IsForProject).Count();
                    item.ProjectUrls = query.Where(a => a.Type == AttachmentType.url && a.IsForProject).Count();
                    item.ActivitiesFiles = query.Where(a => a.Type == AttachmentType.file && !a.IsForProject).Count();
                    item.ActivitiesUrls = query.Where(a => a.Type == AttachmentType.url && !a.IsForProject).Count();
                }
                item.Status = (project == null) ? Core.Wizard.WizardItemStatus.disabled : Core.Wizard.WizardItemStatus.none;
                return item;
            }
        #endregion

        #region "Manage Project"
            #region "Settings"    
                public dtoProject GetdtoProject(long idProject)
                {
                    dtoProject result = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);
                        if (project!=null)
                            result = new dtoProject(project);
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = null;
                    }
                    return result;
                }
                public dtoProjectStatistics GetProjectStatistics(long idProject)
                {
                    dtoProjectStatistics result = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);
                        if (project != null)
                        {
                            result = new dtoProjectStatistics(project);
                            result.Activities = (from a in Manager.GetIQ<liteDisplayActivity>() where a.Deleted == BaseStatusDeleted.None && a.IdProject == idProject select a.Id).Count();
                            result.EstimatedActivities = (from a in Manager.GetIQ<liteDisplayActivity>() where a.Deleted == BaseStatusDeleted.None && a.IdProject == idProject && a.IsDurationEstimated select a.Id).Count();
                            result.Milestones = (from a in Manager.GetIQ<liteDisplayActivity>() where a.Deleted == BaseStatusDeleted.None && a.IdProject == idProject && !a.IsSummary && a.Duration == (double)0 select a.Id).Count();
                            result.Summaries = (from a in Manager.GetIQ<liteDisplayActivity>() where a.Deleted == BaseStatusDeleted.None && a.IdProject == idProject && a.IsSummary select a.Id).Count();


                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = null;
                    }
                    return result;
                }

                public Project AddProject(dtoProject dto, int idCommunity, Boolean forPortal, Boolean isPersonal, String dCalendarName = "")
                {
                    dto.IdCommunity = idCommunity;
                    dto.isPortal = forPortal;
                    dto.isPersonal = isPersonal;
                    return SaveProject(dto, dCalendarName);
                }
                public Project SaveProject(dtoProject dto, String dCalendarName = "", dtoProjectSettingsSelectedActions actions = null)
                {
                    Project project = null;
                    try 
                    {
                        Manager.BeginTransaction();
                        Boolean recalcMap = false;
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        project = Manager.Get<Project>(dto.Id);
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser && (dto.Id==0 || project !=null))
                        {
                            if (dto.Id==0){
                                project = new Project();
                                project.CreateMetaInfo(person, "","");// UC.IpAddress, UC.ProxyIpAddress);
                                if (dto.IdCommunity>0)
                                    project.Community = Manager.GetLiteCommunity(dto.IdCommunity);
                                project.Status = ProjectItemStatus.notstarted;
                                project.isPersonal = dto.isPersonal;
                                project.isPortal = (dto.isPortal && dto.IdCommunity==0);
                            }
                            else
                            {
                                project.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress); 
                                project.Status = dto.Status;
                            }
                            project.isArchived = dto.isArchived;
                            project.Name = dto.Name;
                            project.Description = dto.Description;
                            project.Completeness = dto.Completeness;
                            project.ConfirmCompletion = dto.ConfirmCompletion;
                            project.Visibility = dto.Visibility;
                            project.Availability = dto.Availability;
                            if (actions == null) {
                                project.AllowMilestones = dto.AllowMilestones;
                                project.AllowEstimatedDuration = dto.AllowEstimatedDuration;
                                project.AllowSummary = dto.AllowSummary;
                                project.StartDate = dto.StartDate;
                            }
                            project.Deadline= dto.Deadline;
                            if (dto.Id == 0)
                            {
                                #region "AddProjectActions"
                                project.DaysOfWeek = dto.DaysOfWeek;
                               
                                project.DateCalculationByCpm = dto.DateCalculationByCpm;
                                project.Duration = 0;
                                project.SetDefaultResourcesToNewActivity = dto.SetDefaultResourcesToNewActivity && dto.DefaultActivityResources.Any();
                                Manager.SaveOrUpdate(project);
                                ProjectResource resource = AddInternalResource(project, person, Manager.Get<litePerson>(UC.CurrentUserID), ProjectVisibility.Full, ActivityRole.ProjectOwner, project.SetDefaultResourcesToNewActivity);
                                project.Resources.Add(resource);
                                
                                AddCalendar(person, project, dCalendarName);
                                Manager.SaveOrUpdate(project);
                                #endregion
                            }
                            else {
                                DateTime? cStartDate = project.StartDate;
                                recalcMap = (dto.DaysOfWeek != project.DaysOfWeek) || (actions != null && (actions.DateAction == ConfirmActions.Apply || (actions.ManualAction == ConfirmActions.Apply) || (actions.CpmAction == ConfirmActions.Apply) || (actions.MilestonesAction == ConfirmActions.Apply && !dto.AllowMilestones)));
                                if (dto.DaysOfWeek != FlagDayOfWeek.None)
                                {
                                    project.DaysOfWeek = dto.DaysOfWeek;
                                    foreach (ProjectCalendar calendar in project.Calendars.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == CalendarType.Project))
                                    {
                                        calendar.DaysOfWeek = project.DaysOfWeek;
                                    }
                                }
                                ///DA SVOLGERE
                                ///
                                if (actions!=null){
                                    var qActivities = (from a in Manager.GetIQ<PmActivity>() where a.Deleted == BaseStatusDeleted.None && a.Project != null && a.Project == project select a);
                                    if (actions.DateAction!= ConfirmActions.Hold)
                                        project.StartDate = dto.StartDate;
                                    if (actions.EstimatedAction != ConfirmActions.Hold)
                                    {
                                        if (!dto.AllowEstimatedDuration && project.AllowEstimatedDuration)
                                        {
                                            foreach (PmActivity activity in qActivities.Where(a => a.IsDurationEstimated)) {
                                                activity.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                activity.IsDurationEstimated = false;
                                            }
                                            project.IsDurationEstimated = false;
                                        }
                                        project.AllowEstimatedDuration = dto.AllowEstimatedDuration;
                                    }
                                    if (actions.MilestonesAction != ConfirmActions.Hold)
                                    {
                                        if (!dto.AllowMilestones && project.AllowMilestones)
                                        {
                                            foreach (PmActivity activity in qActivities.Where(a => a.Duration == (double)0 && !a.IsSummary))
                                            {
                                                activity.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                activity.Duration = (double)1;
                                            }
                                        }
                                        project.AllowMilestones = dto.AllowMilestones;
                                    }
                                    if (actions.SummariesAction != ConfirmActions.Hold)
                                    {
                                        if (!dto.AllowSummary && project.AllowSummary)
                                        {
                                            foreach (PmActivity activity in qActivities.Where(a => !a.IsSummary))
                                            {
                                                activity.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                activity.Parent = null;
                                            }
                                            foreach (PmActivity activity in qActivities.Where(a => a.IsSummary))
                                            {
                                                activity.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                            recalcMap = true;
                                        }
                                        project.AllowSummary = dto.AllowSummary;
                                    }
                                    /// DA SVOLGERE
                                    if (actions.CpmAction != ConfirmActions.Hold) {
               
                                    }
                                    else if (actions.ManualAction != ConfirmActions.Hold)
                                    {
                                    }
                                }
                                project.SetDefaultResourcesToNewActivity = dto.SetDefaultResourcesToNewActivity && dto.DefaultActivityResources.Any();
                                if (project.SetDefaultResourcesToNewActivity)
                                {
                                    foreach (ProjectResource resource in project.Resources)
                                    {
                                        if (resource.Deleted != BaseStatusDeleted.None && dto.DefaultActivityResources.Where(r => r.IdResource == resource.Id).Any())
                                        {
                                            resource.DefaultForActivity = true;
                                            resource.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            resource.ProjectRole = (resource.ProjectRole == ActivityRole.ProjectOwner) ? ActivityRole.Manager : resource.ProjectRole;
                                        }
                                        else if ((resource.DefaultForActivity && !dto.DefaultActivityResources.Where(r => r.IdResource == resource.Id).Any()) || (!resource.DefaultForActivity && dto.DefaultActivityResources.Where(r => r.IdResource == resource.Id).Any()))
                                        {
                                            resource.DefaultForActivity = dto.DefaultActivityResources.Where(r => r.IdResource == resource.Id).Any();
                                            resource.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (ProjectResource resource in project.Resources.Where(r=> r.Deleted== BaseStatusDeleted.None && r.DefaultForActivity))
                                    {
                                        resource.DefaultForActivity = false;
                                        resource.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                }
                                if (project.Calendars ==null || !project.Calendars.Where(c=> c.Deleted== BaseStatusDeleted.None).Any())
                                    AddCalendar(person, project, dCalendarName);
                                recalcMap |= (cStartDate!= project.StartDate);
                                if (recalcMap)
                                    InternalRecalculateProjectMap(project, person);
                            }
                        }
                        else
                            project=null;
                        Manager.Commit();
                        if (dto.Id == 0 && person !=null)
                            ClearCacheItems(person.Id);
                        else if (recalcMap)
                            ClearCacheItems(project);
                    }
                    catch (Exception ex)
                    {   
                        Manager.RollBack();
                        project = null;
                    }
                    return project;
                }

                public dtoProject SetProjectDates(long idProject, DateTime? newStartDate, DateTime? newDeadline, ref  dtoField<DateTime?> startDate, ref dtoField<DateTime?> endDate, ref dtoField<DateTime?> deadLine)
                {
                    dtoProject result = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);

                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser && project != null)
                        {
                            if (newStartDate.HasValue)
                                project.StartDate = newStartDate;
                            if (!newDeadline.HasValue || (newDeadline.HasValue && project.StartDate.HasValue && newDeadline.Value > project.StartDate.Value))
                                project.Deadline = newDeadline;

                            Manager.SaveOrUpdate(project);
                            InternalRecalculateProjectMap(project, person);
                            startDate.Status = (project.StartDate.Equals(startDate.Init) ? FieldStatus.none : (project.DateCalculationByCpm) ? FieldStatus.updated : FieldStatus.recalc);
                            startDate.Current = project.StartDate;
                            startDate.InEditMode = false;
                            startDate.IsUpdated = (startDate.Init != startDate.Current);

                            endDate.Status = (project.EndDate.Equals(endDate.Init) ? FieldStatus.none : FieldStatus.recalc);
                            endDate.Current = project.EndDate;
                            endDate.IsUpdated = (endDate.Init != endDate.Current);

                            deadLine.Status = (project.Deadline.Equals(deadLine.Init) ? FieldStatus.none : FieldStatus.updated);
                            deadLine.Current = project.Deadline;
                            deadLine.IsUpdated = (deadLine.Init != deadLine.Current);
                            if (deadLine.Current.HasValue && endDate.Current.HasValue && deadLine.Current.Value < endDate.Current.Value)
                                endDate.Status = FieldStatus.error;
                           
                        }
                        Manager.Commit();
                        if (startDate.Status!= FieldStatus.none || endDate.Status != FieldStatus.none || deadLine.Status != FieldStatus.none)
                            ClearCacheItems(project);      
                        result = new dtoProject(project);
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = null;
                    }
                    return result;
                }
                public void ClearCacheItems(Project project)
                {
                    if (project != null && project.Resources != null) {
                        foreach (Int32 idUser in project.Resources.Where(r => r.Type != ResourceType.External && r.Person != null).Select(r => r.Person.Id)) {
                            ClearCacheItems(idUser);
                        }
                    }
                }
                public void ClearCacheItems(Int32 idUser)
                {
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.SummaryUser(idUser));
                }
            #endregion    

            #region "Resources"
                /// <summary>
                /// restituisce tutti gli id delle persone assegnate ad un progetto
                /// </summary>
                /// <param name="idProject"></param>
                /// <returns></returns>    
                public List<Int32> GetProjectIdPersons(long idProject)
                {
                    List<Int32> items = new List<Int32>();
                    try
                    {
                        items = (from a in Manager.GetIQ<ProjectResource>()
                                 where a.Deleted == BaseStatusDeleted.None && a.Project.Id == idProject && a.Type == ResourceType.Internal && a.Person != null
                                 select a.Person.Id).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.Distinct().ToList();
                }
                public List<dtoProjectResource> GetProjectResources(long idProject, String unkownUser)
                {
                    List<dtoProjectResource> resources = null;
                    try
                    {
                        resources = (from r in Manager.GetIQ<ProjectResource>()
                                     where r.Project != null && r.Project.Id == idProject && r.Deleted == BaseStatusDeleted.None
                                     select r).ToList().Select(r => new dtoProjectResource(r, unkownUser)).ToList();
                        foreach (dtoProjectResource r in resources)
                        {
                            r.isValid = (r.IdPerson > 0 && r.ResourceType == ResourceType.Internal) || (r.IdPerson == 0 && (r.ResourceType == ResourceType.External || r.ResourceType == ResourceType.Removed));
                            r.AllowDelete = (r.ProjectRole != ActivityRole.ProjectOwner);
                        }
                        //resources.Where(r => !String.IsNullOrEmpty(r.LongName)).GroupBy(r => r.LongName).Where(r => r.Count() > 1).SelectMany(r => r.ToList()).ToList().ForEach(r => r.DisplayErrors = EditingErrors.MultipleLongName);
                        resources.Where(r => !String.IsNullOrEmpty(r.ShortName)).GroupBy(r => r.ShortName).Where(r => r.Count() > 1).SelectMany(r => r.ToList()).ToList().ForEach(r => r.DisplayErrors |= EditingErrors.MultipleShortName);
                        resources = resources.OrderBy(r => r.ProjectRole).ThenBy(r => r.LongName).ToList();
                    }
                    catch (Exception ex)
                    {
                        resources = null;
                    }
                    return resources;

                }
                public List<dtoProjectResource> GetAvailableResourcesForOwner(long idProject, String unkownUser)
                {
                    List<dtoProjectResource> resources = null;
                    try
                    {
                        resources = (from r in Manager.GetIQ<ProjectResource>()
                                     where r.Project != null && r.Project.Id == idProject && r.Deleted == BaseStatusDeleted.None
                                     && r.ProjectRole != ActivityRole.ProjectOwner && r.Type == ResourceType.Internal
                                     select r).ToList().Select(r => new dtoProjectResource(r, unkownUser)).ToList();
                        foreach (dtoProjectResource r in resources)
                        {
                            r.isValid = (r.IdPerson > 0 && r.ResourceType == ResourceType.Internal);
                        }
                        resources.Where(r => r.isValid && !String.IsNullOrEmpty(r.ShortName)).GroupBy(r => r.ShortName).Where(r => r.Count() > 1).SelectMany(r => r.ToList()).ToList().ForEach(r => r.DisplayErrors |= EditingErrors.MultipleShortName);
                        resources = resources.Where(r => r.isValid).OrderBy(r => r.ProjectRole).ThenBy(r => r.LongName).ToList();
                    }
                    catch (Exception ex)
                    {
                        resources = null;
                    }
                    return resources;

                }
                public List<dtoResource> GetAvailableResources(long idProject, String unkownUser)
                {
                    List<dtoResource> resources = null;
                    try
                    {
                        resources = (from r in Manager.GetIQ<liteResource>()
                                     where r.IdProject == idProject && r.Deleted == BaseStatusDeleted.None
                                     select r).ToList().Select(r => new dtoResource(r, unkownUser)).ToList();
                        resources = resources.OrderBy(r => r.LongName).ToList();
                    }
                    catch (Exception ex)
                    {
                        resources = null;
                    }
                    return resources;

                }
        
                public ProjectResource SelectNewProjectOwner(long idProject, long idResource) {
                    ProjectResource owner = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try 
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                        {
                            List<ProjectResource> resources = (from r in Manager.GetIQ<ProjectResource>()
                                                               where r.Project != null && r.Project.Id == idProject && r.Deleted == BaseStatusDeleted.None
                                                               && (r.ProjectRole == ActivityRole.ProjectOwner || r.Id == idResource)
                                                               select r).ToList();
                            if (resources.Count > 1) {
                                DateTime cTime = DateTime.Now;
                                foreach (ProjectResource r in resources) {
                                    r.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    r.ModifiedOn = cTime;
                                    r.ProjectRole = (r.Id==idResource) ? ActivityRole.ProjectOwner: ActivityRole.Manager;
                                    if (r.Type != ResourceType.External && r.Person !=null)
                                        ClearCacheItems(r.Person.Id);
                                }
                                Manager.SaveOrUpdateList<ProjectResource>(resources);
                                owner = resources.Where(r => r.ProjectRole == ActivityRole.ProjectOwner).FirstOrDefault();
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        owner = null;
                    }
                    return owner;
                }

                public ProjectResource AddNewProjectOwner(long idProject, Int32 idPerson)
                {
                    ProjectResource owner = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try 
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        litePerson pToAdd = Manager.GetLitePerson(idPerson);
                        Project project = Manager.Get<Project>(idProject);
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser && pToAdd!=null && project !=null)
                        {
                            DateTime cTime = DateTime.Now;
                            ProjectResource cOwner = project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && r.ProjectRole == ActivityRole.ProjectOwner).FirstOrDefault();
                            if (cOwner != null)
                            {
                                cOwner.ProjectRole = ActivityRole.Manager;
                                cOwner.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                cOwner.ModifiedOn = cTime;
                                if (cOwner.Person !=null)
                                    ClearCacheItems(cOwner.Person.Id);
                            }
                            List<ProjectResource> aResources = AddProjectInternalResources(project, new List<Int32>() { idPerson }, ActivityRole.ProjectOwner);
                            if (aResources.Count > 0)
                                owner = aResources.FirstOrDefault();
                            else
                            {
                                cOwner.ProjectRole = ActivityRole.ProjectOwner;
                                cOwner.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                cOwner.ModifiedOn = cTime;
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        owner = null;
                    }
                    return owner;
                }
                public Boolean SaveProjectResources(long idProject, List<dtoProjectResource> resources, String unkownUser, List<Int32> idPersonsToAdd = null)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        Project project = Manager.Get<Project>(idProject);
                        if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && project != null)
                        {
                            List<ProjectResource> pResources = (from a in Manager.GetIQ<ProjectResource>()
                                                                where a.Project != null && a.Project.Id == idProject && a.Deleted == BaseStatusDeleted.None
                                                                select a).ToList();
                            foreach (dtoProjectResource dto in resources)
                            {
                                ProjectResource r = pResources.Where(rs => rs.Id == dto.IdResource).FirstOrDefault();
                                if (r != null)
                                {
                                    r.Visibility = dto.Visibility;
                                    r.ProjectRole = dto.ProjectRole;
                                    r.ShortName = dto.ShortName;
                                    if (String.IsNullOrEmpty(r.ShortName))
                                    {
                                        r.ShortName = "R" + GetUniqueResourceShortName(project, r.Number);
                                    }
                                    switch (r.Type)
                                    {
                                        case ResourceType.Internal:
                                            if (r.Person == null)
                                            {
                                                r.Type = ResourceType.Removed;
                                                r.LongName = unkownUser + "-" + r.Id;
                                            }
                                            break;
                                        case ResourceType.Removed:
                                        case ResourceType.External:
                                            r.LongName = dto.LongName;
                                            if (String.IsNullOrEmpty(dto.LongName))
                                                r.LongName = dto.ShortName;
                                            else
                                                r.LongName = dto.LongName;
                                            r.Mail = dto.Mail;
                                            if (r.ProjectRole == ActivityRole.ProjectOwner || r.ProjectRole == ActivityRole.Manager)
                                                r.ProjectRole = ActivityRole.Resource;
                                            break;
                                    }
                                    r.Deleted = BaseStatusDeleted.None;
                                    r.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                }
                            }
                            if (idPersonsToAdd != null && idPersonsToAdd.Any())
                                AddProjectInternalResources(project, idPersonsToAdd, ActivityRole.Resource);
                            Manager.SaveOrUpdateList<ProjectResource>(pResources);
                            result = true;
                        }

                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        result = false;
                    }
                    return result;

                }

                public List<ProjectResource> AddProjectInternalResources(long idProject, List<Int32> idPersonsToAdd, ActivityRole role)
                {
                    return AddProjectInternalResources(Manager.Get<Project>(idProject), idPersonsToAdd, role);
                }

                public List<ProjectResource> AddProjectInternalResources(Project project, List<Int32> idPersonsToAdd, ActivityRole role)
                {
                    List<ProjectResource> resources = new List<ProjectResource>();
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson creator = Manager.GetLitePerson(UC.CurrentUserID);
                        if (creator != null && creator.TypeID != (Int32)UserTypeStandard.Guest && project != null)
                        {
                            if (idPersonsToAdd != null && idPersonsToAdd.Any())
                            {
                                foreach (Int32 idPerson in idPersonsToAdd)
                                {
                                    litePerson person = Manager.Get<litePerson>(idPerson);
                                    if (person != null)
                                    {
                                        ProjectResource resource = (project.Resources != null && project.Resources.Where(r => r.Person != null).Any()) ? project.Resources.Where(r => r.Person != null && r.Person.Id == idPerson).FirstOrDefault() : null;
                                        if (resource == null)
                                        {
                                            resource = AddInternalResource(project, creator, person, project.Visibility, role);
                                            resources.Add(resource);
                                            project.Resources.Add(resource);
                                        }
                                        else if (resource.Deleted != BaseStatusDeleted.None)
                                        {
                                            resource.SetDeleteMetaInfo(creator, UC.IpAddress, UC.ProxyIpAddress);
                                            resources.Add(resource);
                                        }
                                        else
                                            resources.Add(resource);
                                        ClearCacheItems(person.Id);
                                    }
                                }
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction && Manager.IsInTransaction())
                            Manager.RollBack();
                        resources = new List<ProjectResource>();
                    }
                    return resources;
                }
                private ProjectResource AddInternalResource(Project project, litePerson creator, litePerson person, ProjectVisibility visibility, ActivityRole role, Boolean isDefault = false)
                {
                    ProjectResource resource = new ProjectResource();
                    resource.CreateMetaInfo(creator, UC.IpAddress, UC.ProxyIpAddress);
                    resource.AssignedActivities = 0;
                    resource.CompletedActivities = 0;
                    resource.ConfirmedActivities = 0;
                    resource.LateActivities = 0;
                    resource.Person = person;
                    resource.ProjectRole = role;
                    resource.Type = ResourceType.Internal;
                    resource.UniqueIdentifier = Guid.NewGuid();
                    resource.Visibility = ProjectVisibility.Full;
                    resource.Project = project;
                    resource.ShortName = resource.GetPersonShortName();
                    resource.Number = GetResourceNumber(project);
                    resource.DefaultForActivity = isDefault;
                    Manager.SaveOrUpdate(resource);

                    return resource;
                }
                public ProjectResource AddExternalResource(long idProject, ActivityRole role)
                {
                    ProjectResource resource = null;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        Project project = Manager.Get<Project>(idProject);
                        if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && project != null)
                        {
                            resource = new ProjectResource();
                            resource.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            resource.LongName = "--";

                            resource.Project = project;
                            resource.ProjectRole = role;
                            resource.UniqueIdentifier = Guid.NewGuid();
                            resource.Type = ResourceType.External;
                            resource.Visibility = project.Visibility;
                            resource.Number = GetResourceNumber(project);
                            resource.ShortName = "R" + resource.Number.ToString();
                            Manager.SaveOrUpdate(resource);
                            project.Resources.Add(resource);


                            //ProjectTaskAssignment assignment = new ProjectTaskAssignment()
                            //                                           {
                            //                                               Completeness = 0,
                            //                                               ForProject = true,
                            //                                               Project = project,
                            //                                               Resource = resource,
                            //                                               Permissions = permissions,
                            //                                               Task = project,
                            //                                               TaskRole = role,
                            //                                               Visibility = project.Visibility
                            //                                           };
                            //assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);

                            //Manager.SaveOrUpdate(assignment);
                        }

                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        resource = null;
                    }
                    return resource;

                }
                public List<ProjectResource> AddExternalResources(long idProject, List<dtoExternalResource> items, ActivityRole role)
                {
                    Project project = Manager.Get<Project>(idProject);
                    return AddExternalResources(project, items, (project != null) ? project.Visibility : ProjectVisibility.InvolvedTasks, role);
                }
                public List<ProjectResource> AddExternalResources(long idProject, List<dtoExternalResource> items, ProjectVisibility visibility, ActivityRole role)
                {
                    return AddExternalResources(Manager.Get<Project>(idProject), items, visibility, role);
                }

                public List<ProjectResource> AddExternalResources(Project project, List<dtoExternalResource> items, ProjectVisibility visibility, ActivityRole role)
                {
                    List<ProjectResource> resources = null;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && project != null && items != null)
                        {
                            resources = (from dtoExternalResource r in items where !String.IsNullOrEmpty(r.ShortName) || !String.IsNullOrEmpty(r.LongName) select new ProjectResource() { ShortName = r.ShortName, LongName = r.LongName, Mail = r.Mail, Project = project, ProjectRole = role, UniqueIdentifier = Guid.NewGuid(), Visibility = visibility, Type = ResourceType.External }).ToList();
                            resources.ForEach(r => r.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress));
                            resources.Where(r => String.IsNullOrEmpty(r.LongName)).ToList().ForEach(r => r.LongName = r.ShortName);

                            long number = GetResourceNumber(project);
                            foreach (ProjectResource rs in resources.Where(r => String.IsNullOrEmpty(r.ShortName)))
                            {
                                rs.Number = number;
                                rs.ShortName = GetUniqueResourceShortName(project, number);
                                number++;
                            }

                            Manager.SaveOrUpdateList<ProjectResource>(resources);
                            resources.ForEach(r => project.Resources.Add(r));
                        }

                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        resources = null;
                    }
                    return resources;

                }
                public long GetResourceNumber(Project project)
                {
                    long number = 1;
                    List<long> numbers = (project == null || project.Resources == null) ? new List<long>() : project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None).Select(r => r.Number).ToList();
                    while (numbers.Contains(number))
                    {
                        number++;
                    }
                    return number;
                }
                public String GetUniqueResourceShortName(Project project, long number)
                {
                    String result = "R";
                    if (project.Resources == null || !project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None).Any())
                        return result + number.ToString();
                    else if (!project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && !String.IsNullOrEmpty(r.ShortName) && r.ShortName.ToUpper() == "R" + number.ToString()).Any())
                        return result + number.ToString();
                    else
                    {
                        while (project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && !String.IsNullOrEmpty(r.ShortName) && r.ShortName.ToUpper() == "R" + number.ToString()).Any())
                        {
                            number++;
                        }
                        return result + number.ToString();
                    }
                }
                public ProjectResource GetResource(long idProject, Int32 idPerson)
                {
                    try
                    {
                        return (from r in Manager.GetIQ<ProjectResource>() where r.Deleted == BaseStatusDeleted.None && r.Project != null && r.Project.Id == idProject && r.Type == ResourceType.Internal && r.Person != null && r.Person.Id == idPerson select r).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                public ProjectResource GetResource(long idProject, long idResource)
                {
                    ProjectResource resource = null;
                    try
                    {
                        resource = Manager.Get<ProjectResource>(idResource);
                        if (resource != null && (resource.Project == null || (resource.Project != null && resource.Project.Id != idProject)))
                        {
                            resource = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        resource = null;
                    }
                    return resource;
                }
                public ProjectResource GetResourceForRemoving(long idProject, long idResource)
                {
                    ProjectResource resource = null;
                    try
                    {
                        resource = Manager.Get<ProjectResource>(idResource);
                        if (resource != null && (resource.Project == null || (resource.Project != null && resource.Project.Id != idProject)))
                            resource = null;
                        else
                            UpdateResourceStatus(resource, idProject, resource.Id);
                    }
                    catch (Exception ex)
                    {
                        resource = null;
                    }
                    return resource;
                }
                public Boolean SetResourceVirtualDelete(long idResource, Boolean delete, ref long assignedActivities, ref long completedActivities)
                {
                    Boolean result = false;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null && person.TypeID != (Int32)UserTypeStandard.Guest)
                        {
                            ProjectResource resource = Manager.Get<ProjectResource>(idResource);
                            if (resource != null)
                            {
                                UpdateResourceStatus(resource);
                                //var query = (from t in Manager.GetIQ<ProjectActivityAssignment>() where t.Deleted == BaseStatusDeleted.None && t.Resource != null && t.Resource.Id == idResource select t);
                                //assignedTask = query.Count();
                                //completedTasks = query.Where(t => t.Completeness > 0).Count();
                                //resource.AssignedActivities = query.Count();
                                assignedActivities = resource.AssignedActivities;
                                completedActivities = resource.CompletedActivities;
                                if (assignedActivities == 0 && completedActivities == 0)
                                {
                                    resource.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                    resource.UpdateMetaInfo(person);

                                    List<ProjectActivityAssignment> assignments = (from t in Manager.GetIQ<ProjectActivityAssignment>() where ((!delete && t.Deleted == BaseStatusDeleted.None) || (delete && t.Deleted == BaseStatusDeleted.Automatic)) && t.Resource != null && t.Resource.Id == idResource select t).ToList();

                                    foreach (ProjectActivityAssignment ta in assignments)
                                    {
                                        ta.Deleted = delete ? BaseStatusDeleted.Automatic : BaseStatusDeleted.None;
                                        ta.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        //if (reviewCompleteness && ta.Task !=null) { 
                                        //    /// INSERIRE LA GESTIONE DEL COMPLETAMENTO
                                        //}
                                    }
                                    UpdateResourceStatus(resource);
                                    result = true;
                                    if(resource.Person !=null)
                                        ClearCacheItems(resource.Person.Id);
                                }
                            }
                            else if (resource == null)
                                result = true;
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction && Manager.IsInTransaction())
                            Manager.RollBack();
                        result = false;
                    }
                    return result;
                }

                /// <summary>
                /// Rimuove anche le assegnazioni ai task
                /// </summary>
                /// <param name="idResource"></param>
                /// <param name="idAssignment"></param>
                /// <param name="delete"></param>
                /// <returns></returns>
                public Boolean SetResourceVirtualDelete(long idResource, Boolean delete, RemoveAction action)
                {
                    Boolean result = false;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null && person.TypeID != (Int32)UserTypeStandard.Guest)
                        {
                            ProjectResource resource = Manager.Get<ProjectResource>(idResource);
                            if (resource != null)
                            {
                                resource.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                resource.UpdateMetaInfo(person);
                                List<ProjectActivityAssignment> assignments = (from t in Manager.GetIQ<ProjectActivityAssignment>() where ((!delete && t.Deleted == BaseStatusDeleted.None) || (delete && t.Deleted == BaseStatusDeleted.Automatic)) && t.Resource != null && t.Resource.Id == idResource select t).ToList();

                                foreach (ProjectActivityAssignment ta in assignments)
                                {
                                    ta.Deleted = delete ? BaseStatusDeleted.Automatic : BaseStatusDeleted.None;
                                    ta.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    //if (reviewCompleteness && ta.Task !=null) { 
                                    //    /// INSERIRE LA GESTIONE DEL COMPLETAMENTO
                                    //}
                                }
                                Manager.SaveOrUpdateList(assignments);
                                UpdateResourceStatus(resource);
                                RecalculateProjectCompleteness(resource.Project, person);
                                ClearCacheItems(resource.Project);      
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                        
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction && Manager.IsInTransaction())
                            Manager.RollBack();
                        result = false;
                    }
                    return result;
                }

                private void UpdateResourceStatus(ProjectResource resource,long idProject, long idResource ) {
                    if (resource != null) {
                        DateTime today = DateTime.Now.Date;
                        var query = (from t in Manager.GetIQ<liteProjectActivityAssignment>()
                                     where t.Deleted == BaseStatusDeleted.None && t.Project != null && t.Project.Id == idProject && t.Resource.Id == idResource
                                     select t);
                        resource.AssignedActivities = query.Count();
                        resource.CompletedActivities = query.Where(t=> t.Completeness == 100).Count();
                        resource.ConfirmedActivities = query.Where(t => t.Completeness == 100 && t.Activity.IsCompleted).Count();
                        resource.LateActivities = query.Where(t => t.Completeness < 100 
                                                    && ((t.Activity.EarlyFinishDate.HasValue && t.Activity.EarlyFinishDate.Value.Date.Ticks < today.Ticks )
                                                    ||
                                                    ( t.Activity.Deadline.HasValue && t.Activity.Deadline.Value.Date.Ticks < today.Ticks )
                                                    )).Count();
                    }
                }
                private void UpdateResourceStatus(ProjectResource resource)
                {
                    if (resource != null)
                    {
                        DateTime today = DateTime.Now.Date;
                        var query = (from t in Manager.GetIQ<ProjectActivityAssignment>()
                                     where t.Deleted == BaseStatusDeleted.None && t.Resource.Id == resource.Id
                                     select t);
                        resource.AssignedActivities = query.Count();

                        resource.LateActivities = query.Where(t => t.Completeness < 100 && (t.Activity.EarlyFinishDate != null || t.Activity.Deadline != null)).ToList().Where(t =>
                                                   ((t.Activity.EarlyFinishDate != null && t.Activity.EarlyFinishDate.Value.Date.Ticks < today.Ticks)
                                                   ||
                                                   (t.Activity.Deadline != null && t.Activity.Deadline.Value.Date.Ticks < today.Ticks)
                                                   )).Count();
                        if (resource.Project.ConfirmCompletion)
                        {
                            resource.ConfirmedActivities = query.Where(t => t.Completeness == 100 && t.Activity.IsCompleted).Count();
                            resource.CompletedActivities = query.Where(t => t.Completeness == 100 && !t.Activity.IsCompleted).Count();
                        }
                        else
                        {
                            resource.CompletedActivities = query.Where(t => t.Completeness == 100).Count();
                            resource.ConfirmedActivities = resource.CompletedActivities;
                        }
                        resource.StartedActivities = query.Where(t => t.Completeness < 100 && !t.Activity.isLate(today)).Count();
                    }
                }
                private void UpdateProjectResourcesStatus(Project project)
                {
                    foreach (ProjectResource resource in project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None)) {
                        UpdateResourceStatus(resource);
                    }
                }


                public lm.Comol.Core.Mail.dtoRecipient GetUserRecipient(long idResource)
                {
                    ProjectResource resource = Manager.Get<ProjectResource>(idResource);
                    if (resource == null || resource.Id <= 0)
                        return null;

                    lm.Comol.Core.Mail.dtoRecipient recipient = new Core.Mail.dtoRecipient();

                    if (resource.Person != null && resource.Type== ResourceType.Internal)
                    {
                        recipient.DisplayName = resource.Person.SurnameAndName;
                        recipient.MailAddress = resource.Person.Mail;
                    }

                    if (String.IsNullOrEmpty(recipient.DisplayName))
                    {
                        recipient.DisplayName = resource.LongName;
                        recipient.MailAddress = resource.Mail;
                    }

                    return recipient;
                }
        
            #endregion

            #region "Calendar"
                public ProjectCalendar AddCalendar(long idProject, String name, long idResource = 0)
                {
                    ProjectCalendar calendar = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try 
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser )
                            calendar = AddCalendar(person,Manager.Get<Project>(idProject), name, idResource);
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch(Exception ex){
                        if (!isInTransaction)
                            Manager.RollBack();
                        calendar =  null;
                    }
                    return calendar;
                }
                private ProjectCalendar AddCalendar(litePerson person, Project project, String name, long idResource = 0)
                {
                    ProjectCalendar calendar = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        if (project != null && person != null  )
                        {
                            calendar = new ProjectCalendar();
                            calendar.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            calendar.Project = project;
                            calendar.DaysOfWeek = project.DaysOfWeek;
                            if (idResource>0)
                                calendar.Resource = Manager.Get<ProjectResource> (idResource);
                            calendar.Type = (idResource == 0) ? CalendarType.Project : (calendar.Resource == null && idResource > 0) ? CalendarType.Other : CalendarType.Resource;
                            calendar.Name = name;
                            Manager.SaveOrUpdate(calendar);

                            if (calendar != null)
                            {
                                project.Calendars.Add(calendar);
                                Manager.SaveOrUpdate(project);
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        calendar = null;
                    }
                    return calendar;
                }
            #endregion

            #region "Delete"

                /// <summary>
                /// Rimuove anche le assegnazioni ai task
                /// </summary>
                /// <param name="idResource"></param>
                /// <param name="idAssignment"></param>
                /// <param name="delete"></param>
                /// <returns></returns>
                public Boolean SetProjectVirtualDelete(long idProject, Boolean delete)
                {
                    Boolean result = false;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null && person.TypeID != (Int32)UserTypeStandard.Guest)
                        {
                            Project project = Manager.Get<Project>(idProject);
                            if (project != null)
                            {
                                project.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                project.UpdateMetaInfo(person);

                                foreach (ProjectResource resource in project.Resources.Where(r=> (delete && r.Deleted== BaseStatusDeleted.None) || (!delete && r.Deleted== BaseStatusDeleted.Cascade)))
                                {
                                    resource.Deleted = delete ? BaseStatusDeleted.Cascade : BaseStatusDeleted.None;
                                    resource.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);

                                    List<ProjectActivityAssignment> assignments = (from t in Manager.GetIQ<ProjectActivityAssignment>() where ((!delete && t.Deleted == BaseStatusDeleted.None) || (delete && t.Deleted == BaseStatusDeleted.Cascade)) && t.Resource != null && t.Resource.Id == resource.Id select t).ToList();

                                    foreach (ProjectActivityAssignment ta in assignments)
                                    {
                                        ta.Deleted = delete ? BaseStatusDeleted.Cascade : BaseStatusDeleted.None;
                                        ta.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    Manager.SaveOrUpdateList(assignments);

                                }
                                ClearCacheItems(project);
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction && Manager.IsInTransaction())
                            Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
            #endregion
        #endregion

                #region "Manage Permissions"
                public Int32 GetIdModule() {
                if (idModule == 0)
                    idModule = Manager.GetModuleID(ModuleProjectManagement.UniqueCode);
                return idModule;
            }
            public ModuleProjectManagement GetModulePermissions(Int32 idCommunity) {
                return GetModulePermissions(idCommunity, UC.CurrentUserID);
            }
            public ModuleProjectManagement GetModulePermissions(Int32 idCommunity, Int32 idPerson){
                Person p = Manager.GetPerson(idPerson);

                return (idCommunity == 0) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(idPerson, idCommunity, ModuleProjectManagement.UniqueCode));
            }


            /// <summary>
            /// Dato un ruolo di progetto restituisce una serie di permessi
            /// </summary>
            /// <param name="role"></param>
            /// <returns></returns>
            public PmActivityPermission GetRolePermissions(ActivityRole role)
            {
                PmActivityPermission Permission;
                switch (role)
                {
                    case ActivityRole.ProjectOwner:
                    case ActivityRole.Manager:
                        Permission =  PmActivityPermission.ManageResources
                           | PmActivityPermission.AddAttachments | PmActivityPermission.SetCompleteness | PmActivityPermission.SetStartDate | PmActivityPermission.ManageProject
                           | PmActivityPermission.SetEndDate | PmActivityPermission.SetDeadline | PmActivityPermission.SetStatus
                           | PmActivityPermission.ViewDetails | PmActivityPermission.UpdateDetails | PmActivityPermission.ManageLinks
                           | PmActivityPermission.VirtualDelete | PmActivityPermission.VirtualUnDelete | PmActivityPermission.PhisicalDelete | PmActivityPermission.Add | PmActivityPermission.ViewProjectMap
                           | PmActivityPermission.ViewAttachments | PmActivityPermission.PhisicalDeleteAttachments | PmActivityPermission.VirtualUnDeleteAttachments | PmActivityPermission.VirtualDeleteAttachments
                           | PmActivityPermission.ManageAttachments;
                        if (role == ActivityRole.ProjectOwner)
                            Permission |=PmActivityPermission.ChangeOwner;
                        break;
                    case ActivityRole.Resource:
                        Permission = PmActivityPermission.AddAttachments | PmActivityPermission.SetMyCompleteness | PmActivityPermission.ViewProjectMap
                           | PmActivityPermission.ViewDetails | PmActivityPermission.ViewAttachments | PmActivityPermission.DownloadAttacchments | PmActivityPermission.ManageActivityAttachments;
                        break;
                    case ActivityRole.Visitor:
                        Permission = PmActivityPermission.ViewDetails | PmActivityPermission.ViewAttachments | PmActivityPermission.ViewProjectMap;
                        break;
                    default:
                        Permission = PmActivityPermission.None;
                        break;
                }
                return Permission;
            }

            public Boolean HasPermission(PmActivityPermission permissions, PmActivityPermission permission)
            {
                return ((permissions & permission) == permission);
            }

            public long GetRolePermissionsToLong(ActivityRole role) {
                return (long)GetRolePermissions(role);
            }

            ///// <summary>
            ///// 
            ///// </summary>
            ///// <param name="idActivity">Id del Task su cui cercare i permessi</param>
            ///// <param name="PersonID">Id della persona di cui si cerca il ruolo </param>
            ///// <returns>Restituisce i permessi totali sul task sottoforma di flagenum</returns>

            //public TaskPermissionEnum GetPermissionsOverActivity(long idActivity, int idPerson) 
            //{

            //    TaskPermissionEnum permissions = TaskPermissionEnum.None;
            //    PmActivity activity = Manager.Get<PmActivity>(idActivity);
            //    ProjectResource resource = (activity != null && activity.Project !=null) ? GetResource(activity.Project.Id, idPerson) : null;
            //    if (activity != null && resource != null)
            //    {
            //        try
            //        {
            //            var query = (from ProjectActivityAssignment t in Manager.GetIQ<ProjectActivityAssignment>()
            //                                                       where  t.Resource != null && t.Resource.Id==resource.Id &&
            //                                                           t.Activity.Id == idActivity && t.Deleted == BaseStatusDeleted.None
            //                                                       select t.Role);
            //            List<ActivityRole> roles = query.ToList<ActivityRole>();
            //            if (resource.ProjectRole== ActivityRole.ProjectOwner)
            //                roles.Add(ActivityRole.ProjectOwner);
            //            roles.ForEach(r=> permissions |= GetRolePermissions(r));
            //        }
            //        catch (Exception)
            //        {

            //        }
            //    }
            //    return permissions;
            //}

            /// <summary>
            /// Individua il ruolo a livello di progetto e restituisce i relativi permessi
            /// </summary>
            /// <param name="idProject">Id del progetto preso in considerazione</param>
            /// <param name="idPerson">Id della persona di cui si vuol individuare il ruolo</param>
            /// <returns>TaskpermissionEnum che rappresenta i permessi a livello di progetto</returns>
            public PmActivityPermission GetProjectPermission(long idProject, int idPerson)
            {
                try
                {
                    ProjectResource resource = GetResource(idProject, idPerson);
                    if (resource == null)
                        return PmActivityPermission.None;
                    else
                        return GetRolePermissions(resource.ProjectRole);
                }
                catch (Exception ex)
                {
                    return PmActivityPermission.None;
                }
            }
            public Boolean HasOtherProjectsToManage(Int32 idPerson)
            {
                Boolean result = false;
                try
                {
                    ModuleProjectManagement permission = new ModuleProjectManagement() { Administration = true};
                    result = ServiceCommunityManagement.GetIdCommunityByModulePermissions(idPerson, new Dictionary<int, long>() { { GetIdModule(), permission.GetPermissions() } }).Any();
                }
                catch (Exception ex) { 
                
                }
                return result;
            }
            public Boolean HasOtherProjectsToManage(Person person, ItemDeleted status, Int32 idCommunity = -1)
            {
                Boolean result = false;
                try
                {
                    ModuleProjectManagement portalPermission = ModuleProjectManagement.CreatePortalmodule(person.TypeID);
                    ModuleProjectManagement permission = new ModuleProjectManagement() { Administration = true };
                    switch (idCommunity) { 
                        case 0:
                            result = portalPermission.Administration && ((from p in Manager.GetIQ<Project>()
                                                                          where ((status == ItemDeleted.No && p.Deleted == BaseStatusDeleted.None) || (status == ItemDeleted.Yes && p.Deleted == BaseStatusDeleted.Manual) || status == ItemDeleted.Ignore)
                                                                          && !p.Resources.Where(r=> r.ProjectRole != ActivityRole.ProjectOwner && r.ProjectRole != ActivityRole.Manager && r.Type== ResourceType.Internal && r.Person.Id != person.Id).Any()
                                                                          select p.Id).Any());
                            break;
                        case -1:
                            List<Int32> idCommunities = ServiceCommunityManagement.GetIdCommunityByModulePermissions(person.Id, new Dictionary<int, long>() { { GetIdModule(), permission.GetPermissions() } }).ToList();
                            result = HasOtherProjectsToManage(person, status,0) || ((from p in Manager.GetIQ<Project>()
                                                                                    where ((status == ItemDeleted.No && p.Deleted == BaseStatusDeleted.None) || (status == ItemDeleted.Yes && p.Deleted == BaseStatusDeleted.Manual) || status == ItemDeleted.Ignore)
                                                                                    && (p.Community != null && idCommunities.Contains(p.Community.Id))
                                                                                    && !p.Resources.Where(r => r.ProjectRole != ActivityRole.ProjectOwner && r.ProjectRole != ActivityRole.Manager && r.Type == ResourceType.Internal && r.Person.Id != person.Id).Any()
                                                                                    select p.Id).Any());
                            break;
                        default:
                            permission = new ModuleProjectManagement(Manager.GetModulePermission(person.Id, idCommunity, idModule));
                            result = permission.Administration && ((from p in Manager.GetIQ<Project>()
                                                                    where ((status == ItemDeleted.No && p.Deleted == BaseStatusDeleted.None) || (status == ItemDeleted.Yes && p.Deleted == BaseStatusDeleted.Manual) || status == ItemDeleted.Ignore)
                                                                          && (p.Community != null && p.Community.Id == idCommunity)
                                                                          && !p.Resources.Where(r => r.ProjectRole != ActivityRole.ProjectOwner && r.ProjectRole != ActivityRole.Manager && r.Type == ResourceType.Internal && r.Person.Id != person.Id).Any()
                                                                          select p.Id).Any());
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean HasProjectToManage(Int32 idPerson, ItemDeleted deleted, Int32 idCommunity = -1)
            {
                Boolean result = false;
                try
                {
                    result = (from r in Manager.GetIQ<ProjectResource>()
                              where (((deleted== ItemDeleted.No || deleted == ItemDeleted.Ignore) && r.Deleted == BaseStatusDeleted.None) 
                                        || ((deleted == ItemDeleted.Yes || deleted == ItemDeleted.Ignore)  && r.Deleted == BaseStatusDeleted.Cascade && r.Project.Deleted == BaseStatusDeleted.Manual)
                                        )
                                  && (r.ProjectRole== ActivityRole.Manager || r.ProjectRole == ActivityRole.ProjectOwner)
                                && r.Type== ResourceType.Internal && r.Person != null && r.Person.Id == idPerson
                               && (idCommunity == -1 || (idCommunity > 0 && !r.Project.isPortal && r.Project.Community != null && r.Project.Community.Id == idCommunity))
                              select r.Id ).Any();
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean HasPersonalProject(Int32 idPerson, ItemDeleted deleted, Int32 idCommunity = -1)
            {
                Boolean result = false;
                try
                {
                    result = (from r in Manager.GetIQ<ProjectResource>()
                              where (((deleted == ItemDeleted.No || deleted == ItemDeleted.Ignore) && r.Deleted == BaseStatusDeleted.None) || ((deleted == ItemDeleted.Yes || deleted == ItemDeleted.Ignore) && r.Project.Deleted == BaseStatusDeleted.Manual && r.Deleted == BaseStatusDeleted.Cascade)) && (r.ProjectRole == ActivityRole.ProjectOwner)
                                  && r.Type == ResourceType.Internal && r.Person != null && r.Person.Id == idPerson
                                  && r.Project.isPersonal
                                  && (idCommunity==-1 || (idCommunity>0 && !r.Project.isPortal && r.Project.Community !=null  && r.Project.Community.Id == idCommunity))
                              select r.Id).Any();
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean IsProjectManager(long idProject, int idPerson)
            {
                Boolean result = false;
                try
                {
                    result = (from r in Manager.GetIQ<liteResource>() where r.Person.Id == idPerson && r.IdProject == idProject && r.Deleted == BaseStatusDeleted.None && (r.ProjectRole == ActivityRole.ProjectOwner || r.ProjectRole == ActivityRole.Manager) select r.Id).Any();
                }
                catch (Exception ex)
                {
                    result = false;
                }
                return result;
            }
        #endregion
        
        //#region WBS
        //    private void SetWBSforNewTask(ProjectTask oTask)
        //    {
        //        switch (oTask.Level)
        //        {
        //            case 0:
        //                oTask.TaskWBSstring = "";
        //                oTask.TaskWBSindex = 0;
        //                break;

        //            case 1:
        //                oTask.TaskWBSstring = "";
        //                oTask.TaskWBSindex = GetMaxWBSindex(1, oTask.TaskParent.Id) + 1;
        //                break;
        //            case 2:
        //                oTask.TaskWBSstring = oTask.TaskParent.TaskWBSindex + ".";
        //                oTask.TaskWBSindex = GetMaxWBSindex(oTask.Level, oTask.TaskParent.Id) + 1;
        //                break;
        //            default:
        //                oTask.TaskWBSstring = oTask.TaskParent.TaskWBSstring + oTask.TaskParent.TaskWBSindex + ".";
        //                oTask.TaskWBSindex = GetMaxWBSindex(oTask.Level, oTask.TaskParent.Id) + 1;
        //                break;
        //        }
        //    }

        //    private int GetMaxWBSindex(int Level, long TaskParentID)
        //    {
        //        int maxIndex;
        //         //List <ProjectTask> list = (from ProjectTask oPrj in Manager.GetIQ<ProjectTask>() where (oPrj.TaskParent.Id == TaskParentID) select oPrj).ToList();  
        //        //List <ProjectTask> list = (Manager.GetAll<ProjectTask>(t =>(t.TaskParent.Id == TaskParentID) && (t.Level == Level))).ToList ();
        //        try
        //        {
                    
        //            maxIndex = (from ProjectTask p in Manager.GetIQ<ProjectTask>() where ((p.TaskParent.Id == TaskParentID) &&(p.Level==Level)) orderby  p.TaskWBSindex descending select p.TaskWBSindex).Skip(0).Take(1).ToList().FirstOrDefault();
        //        }
        //        catch (Exception ex)
        //            {
        //                maxIndex = 0;
        //            }
        //        return maxIndex;
        //    }            
        
        //#endregion

           

        #region Helper
      
        
        #endregion
    }
}
