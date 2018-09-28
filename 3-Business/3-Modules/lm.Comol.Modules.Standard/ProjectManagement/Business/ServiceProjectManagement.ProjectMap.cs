using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Modules.Standard.GraphTheory;
namespace lm.Comol.Modules.Standard.ProjectManagement.Business
{
    public partial class ServiceProjectManagement : BaseCoreServices
    {

        #region "Manage Project"
            public liteProjectSettings GetProjectSettings(long idProject)
            {
                liteProjectSettings settings = null;
                try
                {
                    Manager.BeginTransaction();
                    settings = Manager.Get<liteProjectSettings>(idProject);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    settings = null;
                }
                return settings;
            }
        #endregion

        #region "Assignments"
            public List<ProjectActivityAssignment> AddAssignments(PmActivity activity, List<ProjectResource> resources, ActivityRole role, litePerson person = null)
            {
                List<PmActivity> activities = new List<PmActivity>();
                if (activity!=null)
                    activities.Add(activity);
                return AddAssignments(activities, resources, role, person);
            }

            public List<ProjectActivityAssignment> AddAssignments(List<PmActivity> activities, List<ProjectResource> resources, ActivityRole role, litePerson person=null)
            { 
                List<ProjectActivityAssignment> assignments = null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    if (person==null)
                        person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser && activities != null && activities.Any())
                    {
                        assignments = new List<ProjectActivityAssignment>();
                        foreach(PmActivity activity in activities){
                            List<ProjectActivityAssignment> aAssignments = (from pam in Manager.GetIQ<ProjectActivityAssignment>()
                                                                            where pam.Activity != null && pam.Activity.Id == activity.Id
                                                                            select pam).ToList();
                            foreach (ProjectResource resource in resources) {
                                ProjectActivityAssignment assignment = aAssignments.Where(pam => pam.Resource != null && pam.Resource.Id == resource.Id).FirstOrDefault();
                                if (assignment == null) {
                                    assignment = new ProjectActivityAssignment();
                                    assignment.Resource = resource;
                                    assignment.Person = resource.Person;
                                    assignment.Activity = activity;
                                    assignment.Project = activity.Project;
                                    assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                                else if (assignment.Deleted != BaseStatusDeleted.None)
                                    assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                else
                                    assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                assignment.Completeness = 0;
                                assignment.Role = ActivityRole.Resource;
                                assignment.Permissions = GetRolePermissionsToLong(ActivityRole.Resource);
                                if (assignment.Id == 0)
                                {
                                    Manager.SaveOrUpdate(assignment);
                                    activity.Assignments.Add(assignment);
                                }
                                else
                                    Manager.SaveOrUpdate(assignment);
                                assignments.Add(assignment);
                            }
                        }
                        //Manager.SaveOrUpdateList<ProjectActivityAssignment>(assignments);
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    assignments = null;
                }
                return assignments;
            }
            public Boolean SetAssignmentPhisicalDelete(long idAssignment)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    ProjectActivityAssignment assignment = Manager.Get<ProjectActivityAssignment>(idAssignment);
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (assignment != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        if (assignment.Resource!=null)
                            UpdateResourceStatus(assignment.Resource);
                        Manager.DeletePhysical(assignment);
                    }
                    Manager.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return result;
            }
            /// <summary>
            /// Indica se cancellare o ripristinare un assignment di un task/progetto
            /// </summary>
            /// <param name="idAssignment"></param>
            /// <param name="delete"></param>
            /// <returns></returns>
            public Boolean SetAssignmentVirtualDelete(long idAssignment, Boolean delete)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    ProjectActivityAssignment assignment = Manager.Get<ProjectActivityAssignment>(idAssignment);
                    if (person != null && assignment != null)
                    {
                        assignment.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        assignment.UpdateMetaInfo(person);
                        if (assignment.Resource != null)
                            UpdateResourceStatus(assignment.Resource);
                        ClearCacheItems(assignment.Project);  
                    }
                    Manager.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return result;
            }
            public Boolean SetAssignmentsVirtualDelete(PmActivity activity, Boolean delete)
            {
                Boolean result = false;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {

                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    List<ProjectActivityAssignment> assignments = (from a in Manager.GetIQ<ProjectActivityAssignment>()
                                                                   where ((delete && a.Deleted == BaseStatusDeleted.None) || (!delete && a.Deleted== BaseStatusDeleted.Manual)) && a.Activity != null && a.Activity.Id == activity.Id
                                                                   select a).ToList();
                    if (person != null && assignments != null)
                    {
                        foreach (ProjectActivityAssignment assignment in assignments)
                        {
                            assignment.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                            assignment.UpdateMetaInfo(person);
                            if (assignment.Resource != null)
                                UpdateResourceStatus(assignment.Resource);
                        }
                        ClearCacheItems(activity.Project);  
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    else
                        throw ex;
                }
                return result;
            }
            //public Boolean SetAssignmentVirtualDelete(Person deletedBy, ProjectResource resource, Boolean delete)
            //{
            //    Boolean result = false;
            //    Boolean isInTransaction = Manager.IsInTransaction();
            //    try
            //    {
            //        if (!isInTransaction)
            //            Manager.BeginTransaction();
            //        ProjectTaskAssignment assignment = (from a in Manager.GetIQ<ProjectTaskAssignment>() where a.Resource != null && a.Resource.Id == resource.Id  select a).Skip(0).Take(1).ToList().FirstOrDefault();
            //        if (deletedBy != null && assignment != null)
            //        {
            //            assignment.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
            //            assignment.UpdateMetaInfo(deletedBy);
            //        }
            //        if (!isInTransaction)
            //            Manager.Commit();
            //        result = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        if (!isInTransaction && Manager.IsInTransaction())
            //            Manager.RollBack();
            //        result = false;
            //    }
            //    return result;
            //}
            public Boolean SetAssignmentRole(long idAssignment, ActivityRole role)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    ProjectActivityAssignment assignment = Manager.Get<ProjectActivityAssignment>(idAssignment);
                    if (person != null && assignment != null)
                    {
                        assignment.Role = role;
                        assignment.UpdateMetaInfo(person);
                    }
                    Manager.Commit();
                    result = true;
                    if (assignment != null)
                        ClearCacheItems(assignment.Project);  
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return result;
            }

            public List<ProjectResource> GetAssignedResources(PmActivity activity)
            {
                List<ProjectResource> resources = null;
                try
                {
                    List<ProjectActivityAssignment> assignments = (from a in Manager.GetIQ<ProjectActivityAssignment>()
                                                                   where a.Deleted == BaseStatusDeleted.None && a.Activity != null && a.Activity.Id == activity.Id
                                                                   select a).ToList();
                    if (assignments.Any())
                        resources = assignments.Select(a => a.Resource).ToList();
                }
                catch (Exception ex)
                {
                    resources = null;
                }
                return resources;
            }
        #endregion

        #region "Tasks"
            /// <summary>
            /// Return true if a Project has tasks
            /// </summary>
            /// <param name="idProject">id project</param>
            /// <returns></returns>
            public Boolean HasTasks(long idProject)
            {
                Boolean result = false;
                try
                {
                    result = (from a in Manager.GetIQ<liteDisplayActivity>() where a.Deleted== BaseStatusDeleted.None && a.IdProject==idProject select a.Id).Any();
                }
                catch (Exception ex) { 
                
                }
                return result;
            }

            #region "Add Methods"
                public List<PmActivity> AddTaks( Project project, String taskName, Int32 number, long idParent = 0, long idBrother = 0, long idLinkedTo = 0, Boolean linked = false) {
                    return AddActivities(Manager.GetLitePerson(UC.CurrentUserID), project, taskName, number, idParent, idBrother, idLinkedTo, linked);
                }
                public List<PmActivity> AddActivities(litePerson person, Project project, String activityName, Int32 number, long idParent = 0, long idBrother = 0, long idLinkedTo = 0, Boolean linked = false)
                {
                    List<PmActivity> activities = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        PmActivity parent = null;
                        PmActivity brother = null;
                        PmActivity linkedTo = null;
                        if (idParent>0)
                            parent = Manager.Get<PmActivity>(idParent);
                        if (idBrother > 0)
                            brother = Manager.Get<PmActivity>(idBrother);
                        if (idLinkedTo > 0)
                            linkedTo = Manager.Get<PmActivity>(idLinkedTo);

                        if (person != null && project != null && (idParent == 0 || parent != null) && (idBrother == 0 || brother != null) && (idLinkedTo == 0 || linkedTo != null))
                        {
                            activities = new List<PmActivity>();
                            number = (number > 0) ? number : 1;
                            long wbsIndex = GetNewWBSindex(idParent);
                            long displayOrder = project.GetMaxDisplayOrder() + 1;
                            for (Int32 n = 1; n <= number; n++) {
                                PmActivity activity = new PmActivity();
                                activity.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                activity.Community = project.Community;
                                activity.Completeness = 0;
                                activity.Duration = 1;
                                activity.ActualDuration = 0;
                                activity.IsDurationEstimated = project.AllowEstimatedDuration;
                                activity.IsSummary = false;
                                activity.Name = activityName + wbsIndex.ToString();
                                activity.Parent = parent;  
                                activity.Project = project;
                                activity.Status = ProjectItemStatus.notstarted;
                                //activity.PredecessorLinks = null;
                                //activity.SuccessorLinks = null;
                                activity.DisplayOrder = displayOrder++;
                                activity.WBSindex = wbsIndex;
                                if (!project.DateCalculationByCpm)
                                {
                                    activity.EarlyStartDate = DateTime.Now.Date;
                                    activity.EarlyFinishDate = DateTime.Now.Date.AddDays(1);
                                    activity.LatestFinishDate = activity.EarlyFinishDate;
                                    activity.LatestStartDate = activity.EarlyStartDate;
                                }
                                if (parent == null)
                                {
                                    activity.Name = activityName + activity.WBSindex.ToString();
                                    activity.WBSstring = activity.WBSindex.ToString() + ".";
                                    activity.Depth = 0;
                                }
                                else
                                {
                                    activity.Name = activityName + parent.WBSstring + activity.WBSindex.ToString();
                                    activity.WBSstring = parent.WBSstring + activity.WBSindex.ToString() + ".";
                                    activity.Depth = parent.Depth + 1;
                                }
                                

                                if (project.SetDefaultResourcesToNewActivity){
                                    Manager.SaveOrUpdate<PmActivity>(activity);
                                    AddAssignments(activity, project.DefaultActivityResources, ActivityRole.Resource, person);
                                }
                                wbsIndex++;
                                activities.Add(activity);
                            }
                            if (!project.SetDefaultResourcesToNewActivity)
                                Manager.SaveOrUpdateList<PmActivity>(activities);
                            if (!project.IsDurationEstimated && activities.Where(a => a.IsDurationEstimated).Any())
                                project.IsDurationEstimated = true;

                            if (parent == null)
                                activities.ForEach(a => project.Activities.Add(a));
                            else
                                activities.ForEach(a => parent.Children.Add(a));
                            Manager.SaveOrUpdate(project);
                            //if (project.Activities == null || !project.Activities.Any() || !project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None).Any()) {
                            InternalRecalculateProjectMap(project, person);
                            //}
                            Manager.SaveOrUpdate(project);
                            ClearCacheItems(project);
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        activities = null;
                        if (!isInTransaction)
                            Manager.RollBack();
                        else
                            throw ex;
                    }
                    return activities;
                }
                public List<dtoMapActivity> AddActivitiesToProject(AddActivityAction action, long idProject, long idActivity, String activityName, Int32 number, Boolean linked, Int32 children, List<dtoLiteMapActivity> savingActivities, DateTime? newStartDate, DateTime? newDeadline, ref  dtoField<DateTime?> startDate, ref dtoField<DateTime?> endDate, ref dtoField<DateTime?> deadLine, String unknownUser)
                {
                    List<dtoMapActivity> activities = null;
                    try
                    {
                        List<PmActivity> addedActivities = null;
                        List<DirectedGraphCycle> cycles = new List<DirectedGraphCycle>();
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);
                        PmActivity current = Manager.Get<PmActivity>(idActivity);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser && project != null && (idActivity==0 || action == AddActivityAction.ToProject || current!=null))
                        {
                            long updated = 0;
                            SaveActivities(project, savingActivities, newStartDate, newDeadline, person, ref updated, ref cycles);
                            Boolean saved = false;
                            long displayOrder = (current ==null) ? project.GetMaxDisplayOrder() : current.GetMaxChildrenDisplayOrder();
                           
                            switch (action) { 
                                case AddActivityAction.Before:
                                    displayOrder = current.DisplayOrder;
                                    if (project.Activities !=null){
                                        var queryBefore =  (from a in project.Activities where a.Deleted== BaseStatusDeleted.None && a.DisplayOrder>= current.DisplayOrder orderby a.DisplayOrder select a);
                                        foreach (PmActivity activity in queryBefore)
                                        {
                                            activity.DisplayOrder +=number;
                                        }
                                    }
                                    addedActivities = CreateActivities(project, person, activityName, (current.Parent == null) ? 0 : current.Parent.Id, current.Parent, number, displayOrder);                              
                                    break;
                                case AddActivityAction.After:
                                    var queryAfter = (from a in project.Activities where a.Deleted == BaseStatusDeleted.None && a.DisplayOrder > current.DisplayOrder orderby a.DisplayOrder select a);
                                    foreach (PmActivity activity in queryAfter)
                                    {
                                        activity.DisplayOrder += number;
                                    }
                                    addedActivities = CreateActivities(project, person, activityName, (current.Parent == null) ? 0 : current.Parent.Id , current.Parent, number, displayOrder+1);                              
                                   
                                    
                                    break;
                                case AddActivityAction.AsChildren:
                                    var queryChildren = (from a in project.Activities where a.Deleted == BaseStatusDeleted.None && a.DisplayOrder > displayOrder orderby a.DisplayOrder select a);
                                    foreach (PmActivity activity in queryChildren)
                                    {
                                        activity.DisplayOrder += number;
                                    }
                                    if (current.IsSummary)
                                        addedActivities = CreateActivities(project, person, activityName, idActivity, current, number, displayOrder+1);
                                    else
                                    {
                                        current.IsSummary = true;
                                        addedActivities = CreateActivities(project, person, activityName, idActivity, current, number, displayOrder+1,0, GetAssignedResources(current));
                                        SetAssignmentsVirtualDelete(current, true);
                                        saved = true;
                                    }
                                    break;
                                case AddActivityAction.ToProject:
                                    addedActivities = CreateActivities(project, person, activityName, idActivity, current, number, displayOrder,children);
                                    break;
                            }
                            if (!project.SetDefaultResourcesToNewActivity && !saved)
                                Manager.SaveOrUpdateList<PmActivity>(addedActivities);
                            switch (action)
                            {
                                case AddActivityAction.After:
                                case AddActivityAction.Before:
                                    if (current.Parent == null)
                                        addedActivities.ForEach(a => project.Activities.Add(a));
                                    else
                                    {
                                        addedActivities.ForEach(a => current.Parent.Children.Add(a));
                                        addedActivities.ForEach(a => project.Activities.Add(a));
                                    }
                                    if (linked && action == AddActivityAction.After && !current.IsSummary)
                                    {
                                        LinkActivities(person, current, addedActivities);
                                        Manager.SaveOrUpdate(current);
                                    }
                                    break;
                                case AddActivityAction.AsChildren:
                                    PmActivity firstAvailable = current.Children.Where(c => c.Deleted == BaseStatusDeleted.None && !c.IsSummary ).OrderByDescending(a => a.DisplayOrder).ThenBy(a => a.Id).FirstOrDefault();
                                    addedActivities.ForEach(a => current.Children.Add(a));
                                    addedActivities.ForEach(a => project.Activities.Add(a));
                                    if (!current.IsDurationEstimated && addedActivities.Where(a => a.IsDurationEstimated).Any())
                                        current.SetDurationEstimated(true);
                                    if (current.Predecessors.Any() || current.Successors.Any())
                                    {
                                        foreach (PmActivityLink predecessor in current.PredecessorLinks)
                                        {
                                            predecessor.Deleted = (predecessor.Deleted | BaseStatusDeleted.Automatic);
                                            predecessor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                        foreach (PmActivityLink successor in current.SuccessorLinks)
                                        {
                                            successor.Deleted = (successor.Deleted | BaseStatusDeleted.Automatic);
                                            successor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                        Manager.SaveOrUpdate(current);
                                    }
                                   
                                    if (linked)
                                    {
                                        if (firstAvailable != null)
                                        {
                                            LinkActivities(person, firstAvailable, addedActivities);
                                            Manager.SaveOrUpdate(firstAvailable);
                                        }
                                        else if (addedActivities.Count > 1) {
                                            firstAvailable = addedActivities.First();
                                            LinkActivities(person, firstAvailable, addedActivities.Where(a => a.Id != firstAvailable.Id).ToList());
                                            Manager.SaveOrUpdate(firstAvailable);
                                        }
                                    }
                                    break;
                                case AddActivityAction.ToProject:
                                    PmActivity lastActivity = project.Activities.Where(a=> a.Depth==0 ).OrderBy(a=> a.DisplayOrder).ThenBy(a=> a.Id).FirstOrDefault();
                                    if (project.Activities == null)
                                        project.Activities = new List<PmActivity>();
                                    addedActivities.ForEach(a => project.Activities.Add(a));

                                    if (children > 0 && addedActivities.Any())
                                    {
                                        foreach (PmActivity act in addedActivities)
                                        {
                                            List<PmActivity> cActivities = CreateActivities(project, person, activityName, act.Id, act, children, act.DisplayOrder + 1);
                                            if (!project.SetDefaultResourcesToNewActivity)
                                                Manager.SaveOrUpdateList<PmActivity>(cActivities);
                                            cActivities.ForEach(a => act.Children.Add(a));
                                            cActivities.ForEach(a => project.Activities.Add(a));
                                        }
                                    }
                                    else {
                                        if (lastActivity != null && lastActivity.IsSummary)
                                            lastActivity = null;
                                        if (((lastActivity == null && addedActivities.Count>1) || lastActivity != null) && linked){
                                            if (lastActivity ==null)
                                                lastActivity = addedActivities.FirstOrDefault();
                                            LinkActivities(person, lastActivity, addedActivities.Where(a=> a.Id != lastActivity.Id).ToList());
                                        }
                                    }
                                    break;
                            }
                            //if (action != AddActivityAction.ToProject)
                            //    current.RecalcDuration();
                            //else
                            //    project.RecalcDuration();
                            if (!project.IsDurationEstimated && addedActivities.Where(a => a.IsDurationEstimated).Any())
                                project.IsDurationEstimated = true;

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
                        ClearCacheItems(project);
                        activities = GetActivities(project, unknownUser, savingActivities, cycles);
                        if (addedActivities != null && addedActivities.Any()) {
                            foreach (dtoMapActivity activity in activities.Where(act => addedActivities.Where(a => a.Id == act.IdActivity).Any()))
                            {
                                activity.IsNew = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        activities = null;
                    }
                    return activities;
                }
                private List<PmActivity> CreateActivities(Project project, litePerson person, String activityName, long idParent, PmActivity parent, Int32 number, long displayOrder, Int32 children = 0, List<ProjectResource> resources = null)
                {

                    long wbsIndex = GetNewWBSindex(idParent);
                    Dictionary<Int32, String> names = GetActivitiesDefaultName(project, activityName, number);
                    List<PmActivity> activities = new List<PmActivity>();
                    for (Int32 n = 1; n <= number; n++)
                    {
                        PmActivity activity = new PmActivity();
                        activity.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        activity.Community = project.Community;
                        activity.Completeness = 0;
                        activity.Duration = 1;
                        activity.ActualDuration = 0;
                        activity.IsDurationEstimated = project.AllowEstimatedDuration;
                        activity.IsSummary = (children > 0);
                        activity.Name = names[n];
                        activity.Parent = parent;
                        activity.Project = project;
                        activity.Status = ProjectItemStatus.notstarted;
                        //activity.PredecessorLinks = null;
                        //activity.SuccessorLinks = null;
                        activity.WBSindex = wbsIndex;
                        activity.DisplayOrder = displayOrder++;
                        if (children > 0)
                            displayOrder += children;
                        if (!project.DateCalculationByCpm)
                        {
                            activity.EarlyStartDate = DateTime.Now.Date;
                            activity.EarlyFinishDate = DateTime.Now.Date.AddDays(1);
                            activity.LatestFinishDate = activity.EarlyFinishDate;
                            activity.LatestStartDate = activity.EarlyStartDate;
                        }
                        if (parent == null)
                        {
                            activity.WBSstring = activity.WBSindex.ToString() + ".";
                            activity.Depth = 0;
                        }
                        else
                        {
                            activity.WBSstring = parent.WBSstring + activity.WBSindex.ToString() + ".";
                            activity.Depth = parent.Depth + 1;
                        }
                        if (children == 0)
                        {
                            if (resources == null && project.SetDefaultResourcesToNewActivity)
                            {
                                Manager.SaveOrUpdate<PmActivity>(activity);
                                AddAssignments(activity, project.DefaultActivityResources, ActivityRole.Resource, person);
                            }
                            else if (resources != null)
                            {
                                Manager.SaveOrUpdate<PmActivity>(activity);
                                AddAssignments(activity, resources, ActivityRole.Resource, person);
                            }
                        }
                        wbsIndex++;
                        activities.Add(activity);
                    }
                    return activities;
                }
                private Dictionary<Int32, String> GetActivitiesDefaultName(Project project, String activityName, Int32 number)
                {
                    Dictionary<Int32, String> results = new Dictionary<Int32, String>();
                    long index = project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None).Count()+1;
                    for (Int32 n = 1; n <= number; n++)
                    {
                        if (project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None && a.Name == activityName + index.ToString()).Any())
                        {
                            index++;
                            while (project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None && a.Name == activityName + index.ToString()).Any()) {
                                index++;
                            }
                        }
                        results.Add(n, activityName + index.ToString());
                        index++;
                    }
                    return results;
                }
            #endregion

            #region "Move Methods"
                public List<dtoMapActivity> MoveActivityTo(long idProject, long idActivity, long idParent, List<dtoLiteMapActivity> savingActivities, DateTime? newStartDate, DateTime? newDeadline, ref  dtoField<DateTime?> startDate, ref dtoField<DateTime?> endDate, ref dtoField<DateTime?> deadLine, String unknownUser, ref Boolean moved )
                {
                    List<dtoMapActivity> activities = null;
                    try
                    {
                        List<DirectedGraphCycle> cycles = new List<DirectedGraphCycle>();
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);
                        PmActivity current = Manager.Get<PmActivity>(idActivity);
                        PmActivity newParent = Manager.Get<PmActivity>(idParent);

                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        Boolean edited = false;
                        long updated = 0;
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser && project != null)
                        {
                            if (savingActivities != null)
                                SaveActivities(project, savingActivities, newStartDate, newDeadline, person, ref updated, ref cycles);
                            if (current != null && (idParent == 0 || (idParent > 0 && newParent != null)))
                            {
                                long cDepth = current.Depth;
                                PmActivity oldParent = current.Parent;
                                long dispalyOrder = current.DisplayOrder;
                                if ((current.Parent != null && idParent == 0) || (current.Parent == null && idParent > 0) || (current.Parent != null && current.Parent.Id != idParent))
                                {
                                    if (oldParent != null)
                                        oldParent.Children.Remove(current);
                                    MoveActivityTo(project, current, newParent, person);

                                    if (oldParent != null && cDepth > current.Depth)
                                    {
                                        List<PmActivity> children = oldParent.Children.Where(c => c.Deleted == BaseStatusDeleted.None && c.Id != current.Id && c.DisplayOrder >= dispalyOrder).ToList();
                                        foreach (PmActivity child in children)
                                        {
                                            oldParent.Children.Remove(child);
                                            MoveActivityTo(project, child, current, person);
                                        }
                                        Manager.SaveOrUpdate(current);
                                    }
                                    if (oldParent != null)
                                    {
                                        oldParent.IsSummary = oldParent.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any();
                                        Manager.SaveOrUpdate(oldParent);
                                    }
                                    Manager.SaveOrUpdate(current);
                                    
                                    if (newParent!=null)
                                        Manager.SaveOrUpdate(newParent);
                                }
                                edited = true;
                            }

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
                        if (edited || startDate.Status != FieldStatus.none || endDate.Status != FieldStatus.none || deadLine.Status != FieldStatus.none || updated>0)
                            ClearCacheItems(project);
                        if (edited)
                            moved = true;
                        else
                            moved = false;
                        activities = GetActivities(project, unknownUser, savingActivities, cycles);
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        activities = null;
                    }
                    return activities;
                }
                private void MoveActivityTo(Project project, PmActivity current, PmActivity newParent, litePerson person)
                {
                    long currentDepth = current.Depth;
                    PmActivity oldParent = current.Parent;
                    if (newParent != null)
                    {
                        if (!newParent.IsSummary)
                        {
                            newParent.IsSummary = true;
                            foreach (PmActivityLink predecessor in newParent.PredecessorLinks)
                            {
                                predecessor.Deleted = (predecessor.Deleted | BaseStatusDeleted.Automatic);
                                predecessor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            foreach (PmActivityLink successor in newParent.SuccessorLinks)
                            {
                                successor.Deleted = (successor.Deleted | BaseStatusDeleted.Automatic);
                                successor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            List<ProjectActivityAssignment> assignments = (from a in Manager.GetIQ<ProjectActivityAssignment>()
                                                                           where a.Deleted == BaseStatusDeleted.None && a.Activity != null && a.Activity.Id == newParent.Id
                                                                           select a).ToList();
                            foreach (ProjectActivityAssignment assignment in assignments)
                            {
                                assignment.Deleted = (assignment.Deleted | BaseStatusDeleted.Automatic);
                                assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            Manager.SaveOrUpdate(newParent);
                        }
                        else
                        {
                            long displayOrder = (newParent.Children != null && newParent.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any()) ? newParent.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.DisplayOrder).Max() + 1 : newParent.DisplayOrder + 1;
                            current.DisplayOrder = displayOrder;
                        }

                        foreach (PmActivityLink predecessor in current.PredecessorLinks.Where(p => p.Target != null && p.Target.Id == newParent.Id))
                        {
                            predecessor.Deleted = (predecessor.Deleted | BaseStatusDeleted.Automatic);
                            predecessor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        foreach (PmActivityLink successor in newParent.SuccessorLinks.Where(p => p.Source != null && p.Source.Id == newParent.Id))
                        {
                            successor.Deleted = (successor.Deleted | BaseStatusDeleted.Automatic);
                            successor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        current.Parent = newParent;
                        RecalculateDepth(current, newParent.Depth + 1);
                        Manager.SaveOrUpdate(current);
                        newParent.Children.Add(current);
                        if (newParent.IsDurationEstimated != current.IsDurationEstimated)
                            newParent.RecalcDurationEstimated();
                        Manager.SaveOrUpdate(newParent);
                    }
                    else
                    {
                        current.Parent = null;
                        Manager.SaveOrUpdate(current);
                        RecalculateDepth(current, 0);
                        if (!project.Activities.Where(a=> a.Id == current.Id).Any())
                            project.Activities.Add(current);
                        if (project.IsDurationEstimated != current.IsDurationEstimated)
                            project.RecalcDurationEstimated();
                        Manager.SaveOrUpdate(project);
                    }
                }
                private void RecalculateDepth(PmActivity activity, long newDepth)
                {
                    activity.Depth = newDepth;
                    foreach (PmActivity child in activity.Children)
                    {
                        RecalculateDepth(child, newDepth + 1);
                    }
                }
            #endregion

            #region "Get Methods"


                public ProjectForGantt GetActivitiesForGantt(long idProject, ProjectResource resource, ProjectVisibility visibility, String formatDatePattern, Dictionary<GanttCssClass, String> cssClass, String urlBase = "")
                {
                    ProjectForGantt result = new ProjectForGantt();
                    try
                    {
                        Project project = Manager.Get<Project>(idProject);
                        if (project != null) {
                            result.Items = project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).Select(a =>
                                        new ActivityForGantt(resource,a, visibility, formatDatePattern, cssClass, urlBase)).ToList();
                        }
                    }
                    catch (Exception ex) { 
                    
                    }
                    return result;
                }
                public PmActivity GetActivity(long idActivity)
                {
                    PmActivity activity = null;
                    try
                    {
                        Manager.BeginTransaction();
                        activity = Manager.Get<PmActivity>(idActivity);
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        activity = null;
                    }
                    return activity;
                }
                public litePmActivity GetLiteActivity(long idActivity)
                {
                    litePmActivity activity = null;
                    try
                    {
                        Manager.BeginTransaction();
                        activity = Manager.Get<litePmActivity>(idActivity);
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        activity = null;
                    }
                    return activity;
                }

                public List<dtoMapActivity> GetActivities(long idProject, Int32 idCommunity, Int32 idModule, String unknownUser, List<dtoLiteMapActivity> cActivities = null)
                {
                    List<dtoMapActivity> activities = null;
                    try
                    {
                        Manager.BeginTransaction();
                        dtoProject project = (from p in Manager.GetIQ<Project>()
                                              where p.Id == idProject
                                              select new dtoProject()
                                              {
                                                  Id = idProject,
                                                  AllowMilestones = p.AllowMilestones
                                                  ,
                                                  AllowEstimatedDuration = p.AllowEstimatedDuration,
                                                  AllowSummary = p.AllowSummary
                                              }).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (project != null)
                        {
                            project.Resources = (from r in Manager.GetIQ<ProjectResource>() where r.Deleted == BaseStatusDeleted.None && r.Project != null && r.Project.Id == idProject select r).ToList().Select(r => new dtoResource(r, unknownUser)).ToList();
                            Person p = Manager.GetPerson(UC.CurrentUserID);
                            ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(UC.CurrentUserID, idCommunity, idModule));
                            PmActivityPermission pPermissions = GetProjectPermission(idProject, UC.CurrentUserID);
                            activities = GetActivities(mPermission, pPermissions, project,  cActivities);
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        activities = null;
                    }
                    return activities;
                }
                private List<dtoMapActivity> GetActivities(Project project, String unknownUser, List<dtoLiteMapActivity> cActivities = null,List<DirectedGraphCycle> cycles = null)
                {
                    List<dtoMapActivity> activities = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        dtoProject dProject = new dtoProject(project);
                        if (dProject != null)
                        {
                            Person p = Manager.GetPerson(UC.CurrentUserID);
                            ModuleProjectManagement mPermission = (dProject.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(UC.CurrentUserID, dProject.IdCommunity, Manager.GetModuleID(ModuleProjectManagement.UniqueCode)));
                            PmActivityPermission pPermissions = GetProjectPermission(dProject.Id, UC.CurrentUserID);
                            activities = GetActivities(mPermission, pPermissions, dProject, cActivities,cycles );
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                        {
                            Manager.RollBack();
                            activities = null;
                        }
                        else
                            throw ex;
                    }
                    return activities;
                }
                public List<dtoMapActivity> GetActivities(ModuleProjectManagement mPermission, PmActivityPermission pPermissions, dtoProject project, List<dtoLiteMapActivity> cActivities = null, List<DirectedGraphCycle> cycles = null)
                {
                    List<dtoMapActivity> results = new List<dtoMapActivity>();
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        results = GetActivities(project.Id);

                        Boolean allowEdit = (mPermission.Administration && !project.isPersonal) || pPermissions.HasFlag(PmActivityPermission.ManageProject);
                        Boolean allowDelete = (mPermission.Administration && !project.isPersonal) || pPermissions.HasFlag(PmActivityPermission.ManageProject);
                        Boolean setPredecessors = (allowEdit || pPermissions.HasFlag(PmActivityPermission.ManageLinks)) && project.DateCalculationByCpm;
                        foreach (dtoMapActivity activity in results)
                        {
                            

                            try
                            {
                                if (activity.Links.Init.Any())
                                {
                                    activity.Predecessors.Init = CPMExtensions.ActivityLinksToString(activity.Links.Init, results);
                                    activity.Predecessors.Current = activity.Predecessors.Init;
                                }
                                if (activity.IdResources.Init.Any())
                                {
                                    activity.Resources.Init = project.Resources.Where(r => activity.IdResources.Init.Contains(r.IdResource)).ToList();
                                    activity.Resources.Current = activity.Resources.Init;
                                }
                                activity.Permission = new dtoActivityPermission(pPermissions, allowEdit, allowDelete, project.AllowSummary, project.DateCalculationByCpm);
                                activity.Permission.ToFather = allowEdit && activity.Depth > 0;
                                activity.Permission.ToChild = allowEdit && project.AllowSummary && results.Where(a => a.RowNumber < activity.RowNumber && a.Depth == activity.Depth).Any();
                                activity.Permission.SetPredecessors = setPredecessors && !activity.IsSummary;
                                activity.Permission.SetDuration = activity.Permission.SetDuration && !activity.IsSummary;
                                CalculateActivityFieldsStatus(project.DateCalculationByCpm, activity, (cActivities == null) ? null : cActivities.Where(a => a.IdActivity == activity.IdActivity).FirstOrDefault(), cycles, results);
                            }
                            catch (Exception ex)
                            { 
                            
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                        {
                            Manager.RollBack();
                            results = null;
                        }
                        else
                            throw ex;
                    }
                    return results;
                }
                private List<dtoMapActivity> GetActivities(long idProject)
                {
                    List<dtoMapActivity> results = new List<dtoMapActivity>();
                    List<PmActivity> pActivities = (from a in Manager.GetIQ<PmActivity>()
                                                        where a.Deleted == BaseStatusDeleted.None && a.Project != null && a.Project.Id == idProject && a.Depth == 0
                                                        orderby a.DisplayOrder, a.Id
                                                        select a).ToList();
                    long rNumber = 1;
                    foreach (PmActivity activity in pActivities)
                    {
                        results.Add(new dtoMapActivity(activity, rNumber++));
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        {
                            results.AddRange(GetActivities(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToList(), ref rNumber));
                        }
                    }

                    return results;
                }
                private List<dtoMapActivity> GetActivities(List<PmActivity> activities, ref long rNumber)
                {
                    List<dtoMapActivity> results = new List<dtoMapActivity>();
                    foreach (PmActivity activity in activities)
                    {
                        results.Add(new dtoMapActivity(activity, rNumber++));
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        {
                            results.AddRange(GetActivities(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToList(), ref rNumber));
                        }
                    }
                    return results;
                }

                /// <summary>
                /// Calcola il cambiamento di stato dei campi dell'attività da visualizzare nella mappa di progetto
                /// </summary>
                /// <param name="nActivities">Lista delle attività da DB</param>
                /// <param name="oActivities">Lista delle attività da interfaccia</param>
                private void CalculateActivityFieldsStatus(Boolean byCpm, dtoMapActivity activity, dtoLiteMapActivity vActivity, List<DirectedGraphCycle> cycles = null, List<dtoMapActivity> cActivities = null)
                {
                    if (vActivity != null)
                    {
                        activity.Name.Status = (activity.Name.Current != vActivity.Previous.Name) ? FieldStatus.updated : FieldStatus.none;
                        activity.Name.IsUpdated = (activity.Name.Status == FieldStatus.updated);
                        if (!activity.Name.IsUpdated && activity.Name.Current != vActivity.Current.Name && vActivity.InEditMode)
                        {
                            activity.Name.Edit = vActivity.Current.Name;
                            activity.Name.InEditMode = true;
                        }
                        activity.Duration.Status = (!activity.Duration.Current.Equals(vActivity.Previous.Duration)) ? FieldStatus.updated : FieldStatus.none;
                        if (activity.IsSummary && activity.Duration.Status == FieldStatus.updated)
                            activity.Duration.Status = FieldStatus.recalc;
                        else
                        {
                            activity.Duration.IsUpdated = (activity.Duration.Status == FieldStatus.updated);
                            if (!activity.Duration.IsUpdated && !activity.Duration.Current.Equals(vActivity.Current.Duration) && vActivity.InEditMode)
                            {
                                activity.Duration.Edit = vActivity.Current.Duration;
                                activity.Duration.InEditMode = true;
                            }
                        }

                        activity.EarlyStartDate.Status = (activity.EarlyStartDate.Current != vActivity.Previous.EarlyStartDate || activity.Duration.Status == FieldStatus.recalc) ? (activity.IsSummary || byCpm || activity.Duration.Status == FieldStatus.recalc) ? FieldStatus.recalc : FieldStatus.updated : FieldStatus.none;
                        activity.EarlyStartDate.IsUpdated = (activity.EarlyStartDate.Status == FieldStatus.updated);
                        activity.EarlyFinishDate.Status = (activity.EarlyFinishDate.Current != vActivity.Previous.EarlyFinishDate || activity.Duration.Status == FieldStatus.recalc) ? (activity.isAfterDeadline ? FieldStatus.error : FieldStatus.recalc) : FieldStatus.none;
                        activity.EarlyFinishDate.IsUpdated = (activity.EarlyFinishDate.Status == FieldStatus.updated);
                        if (byCpm)
                        {
                            activity.Predecessors.Status = (activity.PredecessorsToIdString() != vActivity.Previous.PredecessorsIdString) ? FieldStatus.updated : FieldStatus.none;
                            if (cycles != null && vActivity.InEditPredecessorsMode)
                                activity.Predecessors.Status = ((cycles.Where(c => c.GetIdentifyers().Contains(activity.IdActivity)).Any() || vActivity.Current.IdActivityLinks.Contains(vActivity.IdActivity)) ? FieldStatus.error : activity.Predecessors.Status);
                            else if (cycles == null && vActivity.Current.IdActivityLinks.Contains(vActivity.IdActivity))
                                activity.Predecessors.Status = FieldStatus.error;
                            activity.Predecessors.IsUpdated = (activity.Predecessors.Status == FieldStatus.updated);
                            if (activity.Predecessors.Status == FieldStatus.error)
                            {
                                activity.Predecessors.Edit = vActivity.Current.Predecessors;
                                activity.Predecessors.InEditMode = true;
                                activity.InEditPredecessorsMode = true;
                            }
                            else if (cActivities != null && vActivity.Current.IdSummaryLinks.Any()) {
                                activity.Predecessors.Edit = activity.Predecessors.Current;
                                activity.Predecessors.InEditMode = true;
                                activity.InEditPredecessorsMode = true;
                                activity.Predecessors.Status = (isFather(activity, vActivity.Current.IdSummaryLinks, cActivities) ? FieldStatus.errorfatherlinked : FieldStatus.errorsummarylinked);
                            }
                        }
                        activity.Status = (activity.Predecessors.Status == FieldStatus.error || activity.Predecessors.Status == FieldStatus.errorfatherlinked || activity.Predecessors.Status == FieldStatus.errorsummarylinked) ? FieldStatus.error : FieldStatus.none;
                    }
                }
                private Boolean isFather(dtoMapActivity activity, List<long> idPredecessors, List<dtoMapActivity> activities)
                {
                    if (activity !=null && activity.IdParent>0)
                        return idPredecessors.Contains(activity.IdParent) || isFather(activities.Where(a => a.IdActivity == activity.IdParent).FirstOrDefault(), idPredecessors, activities);
                    else
                        return false;
                }
            #endregion

            #region "Save Methods"

                public ActivityEditingErrors SaveActivity(long idProject, long idActivity, dtoActivity dto, List<long> idResources, List<dtoActivityLink> dtoLinks, List<dtoActivityCompletion> dtoAssignments, Boolean isCompleted, ref  dtoField<DateTime?> startDate, ref dtoField<DateTime?> endDate, ref dtoField<DateTime?> deadLine)
                {
                    ActivityEditingErrors result = ActivityEditingErrors.None;
                    try
                    {
                        List<DirectedGraphCycle> cycles = new List<DirectedGraphCycle>();
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);
                        PmActivity activity = Manager.Get<PmActivity>(idActivity);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        Boolean updateCache = false;
                        Boolean update = false;
                        if (person != null && activity  != null && project !=null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                        {
                            update = (activity.Duration != dto.Duration.Value || activity.IsDurationEstimated != dto.Duration.IsEstimated  || (!project.DateCalculationByCpm && dto.EarlyStartDate != activity.EarlyStartDate));
                           
                            #region "Settings"
                            activity.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress); 
                            activity.Name = (String.IsNullOrEmpty(dto.Name) ? activity.Name : dto.Name);
                            activity.Description = dto.Description;
                            activity.Notes = dto.Note;
                            if (!activity.IsSummary)
                            {
                                activity.Duration = dto.Duration.Value;
                                if (activity.Duration < 0)
                                    activity.Duration = 1;
                                activity.SetDurationEstimated(dto.Duration.IsEstimated);
                            }
                            activity.Deadline = dto.Deadline;
                            
                            activity.Status = (isCompleted && dto.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.completed : dto.Status;
                            if (!project.DateCalculationByCpm && dto.EarlyStartDate.HasValue)
                                activity.EarlyStartDate = dto.EarlyStartDate;
                            Manager.SaveOrUpdate(activity);
                            #endregion

                            #region "Assignments"
                            if (!activity.IsSummary)
                            {
                                List<ProjectActivityAssignment> assignments = activity.Assignments.ToList();
                                if (assignments.Any() || idResources.Any())
                                {
                                    if (idResources.Any())
                                    {
                                        foreach (var a in assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Resource != null && !idResources.Contains(a.Resource.Id)))
                                        {
                                            a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            update = true;
                                        }
                                        foreach (var a in assignments.Where(a =>   a.Resource != null && idResources.Contains(a.Resource.Id)))
                                        {
                                            dtoActivityCompletion dtoAssignment = dtoAssignments.Where(d => d.IdResource == a.Resource.Id).FirstOrDefault();
                                            if (dtoAssignment != null)
                                            {
                                                if (a.Deleted != BaseStatusDeleted.None)
                                                {
                                                    a.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                    a.Role = ActivityRole.Resource;
                                                    a.Permissions = GetRolePermissionsToLong(ActivityRole.Resource);
                                                    update = true;
                                                }
                                                else
                                                    update = update || (dtoAssignment.Completeness != a.Completeness);
                                                a.Completeness =(isCompleted) ? 100: dtoAssignment.Completeness;
                                                a.IsApproved = (isCompleted) ? true : dtoAssignment.IsApproved;
                                            }
                                        }
                                        foreach (long idResource in idResources.Where(r => !assignments.Where(a => a.Resource != null && a.Resource.Id == r).Any()))
                                        {                                           
                                            dtoActivityCompletion dtoAssignment = dtoAssignments.Where(d => d.IdResource == idResource).FirstOrDefault();
                                            ProjectResource resource = Manager.Get<ProjectResource>(idResource);
                                            if (resource != null && dtoAssignment != null)
                                            {
                                                ProjectActivityAssignment assignment = new ProjectActivityAssignment();
                                                assignment.Activity = activity;
                                                assignment.Completeness = (isCompleted) ? 100 : dtoAssignment.Completeness;
                                                assignment.IsApproved = (isCompleted) ? true : dtoAssignment.IsApproved;
                                                assignment.Project = activity.Project;
                                                assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                assignment.Role = ActivityRole.Resource;
                                                assignment.Permissions = GetRolePermissionsToLong(ActivityRole.Resource);
                                                assignment.Visibility = resource.Visibility;
                                                assignment.Resource = resource;
                                                assignment.Person = resource.Person;
                                                Manager.SaveOrUpdate(assignment);
                                                assignments.Add(assignment);
                                                activity.Assignments.Add(assignment);
                                                Manager.SaveOrUpdate(activity);
                                                update = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        update = assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Any();
                                        foreach (var a in assignments.Where(a => a.Deleted == BaseStatusDeleted.None))
                                        {
                                            a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                    }
                                }
                                if (activity.IsCompleted != isCompleted)
                                    updateCache = true;
                                activity.IsCompleted = isCompleted;
                                CalculateTaskStatusAndCompleteness(activity,true);
                            }
                            else {
                                if (activity.Assignments != null && activity.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Any()) {
                                    foreach (var a in activity.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None))
                                    {
                                        a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        update = true;
                                    }
                                }
                                if (isCompleted && !activity.IsCompleted ){
                                    SetActionCompleted(activity);
                                    if (activity.Parent!=null)
                                        CalculateTaskStatusAndCompleteness(activity.Parent, true);
                                    else if (activity.Depth == 0) {
                                        CalculateProjectStatusAndCompletenessByChildren(activity.Project);
                                        //project.Completeness = (int)project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None && a.Depth == 0 && a.Parent == null).Select(a => a.Completeness).Average();
                                        //project.IsCompleted = (project.Completeness == 100) && !project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None && a.Depth == 0 && !a.IsCompleted).Any();
                                    }
                                    update = true;
                                }
                                else if (isCompleted != activity.IsCompleted)
                                {
                                    CalculateTaskStatusAndCompleteness(activity, true);
                                    update = true;
                                }
                            }
                            
                            #endregion

                            #region "Links"
                                List<long> idPredecessorCycles = new List<long>();
                                SetActivityPredecessors(person,activity, dtoLinks, ref idPredecessorCycles, ref update);
                            #endregion
                            if (update || updateCache) {
                                project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None).ToList().ForEach(r => UpdateResourceStatus(r));   
                            }
                            if (update)
                            {
                                InternalRecalculateProjectMap(project, person);
                            }
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
                        if (update || updateCache)
                            ClearCacheItems(project);      
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = ActivityEditingErrors.All;
                    }
                    return result;
                }
                public List<dtoMapActivity> SaveActivities(long idProject, List<dtoLiteMapActivity> cActivities, DateTime? newStartDate, DateTime? newDeadline, ref  dtoField<DateTime?> startDate, ref dtoField<DateTime?> endDate, ref dtoField<DateTime?> deadLine, ref long updated, ref long alreadyModified, String unknownUser = "")
                {
                    List<dtoMapActivity> activities = null;
                    try
                    {
                        List<DirectedGraphCycle> cycles = new List<DirectedGraphCycle>();
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        Boolean cleanCache = false;
                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser && project != null) {
                            SaveActivities(project, cActivities, newStartDate, newDeadline, person, ref updated, ref cycles);

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
                            cleanCache = (deadLine.Status != FieldStatus.none) || (startDate.Status != FieldStatus.none) || (endDate.Status != FieldStatus.none) || (updated>0);
                        }
                        Manager.Commit();
                        if (cleanCache)
                            ClearCacheItems(project);
                        activities = GetActivities(project, unknownUser, cActivities, cycles);
                    }
                    catch (Exception ex)
                    {
                        updated = 0;
                        Manager.RollBack();
                        activities = null;
                    }
                    return activities;
                }
                private void SaveActivities(Project project, List<dtoLiteMapActivity> savingActivities, DateTime? newStartDate, DateTime? newDeadline, litePerson person, ref long updated, ref List<DirectedGraphCycle> cycles)
                {
                    if (newStartDate.HasValue)
                        project.StartDate = newStartDate;
                    if (!newDeadline.HasValue || (newDeadline.HasValue && project.StartDate.HasValue && newDeadline.Value > project.StartDate.Value))
                        project.Deadline = newDeadline;

                    foreach (dtoLiteMapActivity dto in savingActivities)
                    {
                        dto.UpdateIdPredecessors(savingActivities);
                        if (dto.InEditMode)
                        {
                            PmActivity activity = Manager.Get<PmActivity>(dto.IdActivity);
                            if (activity != null)
                            {
                                updated++;
                                activity.Name = (String.IsNullOrEmpty(dto.Current.Name) ? dto.RowNumber.ToString() : dto.Current.Name);
                                activity.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                activity.Duration = dto.Current.Duration.Value;
                                if (activity.Duration < 0)
                                    activity.Duration = 1;
                                activity.SetDurationEstimated(dto.Current.Duration.IsEstimated);
                                if (!project.DateCalculationByCpm && dto.Current.EarlyStartDate.HasValue)
                                    activity.EarlyStartDate = dto.Current.EarlyStartDate;
                                Manager.SaveOrUpdate(activity);
                            }
                        }
                    }
                    if (project.DateCalculationByCpm) {
                        List<dtoGraphActivity> graphActivities = UpdateGraph(GetGraphActivities(project), savingActivities);
                        Boolean hasCycles = HasCycles(graphActivities);
                        if (hasCycles)
                            cycles = GetCycles(graphActivities).ToList();
                        List<long> idToReview = new List<long>();
                        if (cycles != null || cycles.Any())
                            idToReview.AddRange(cycles.SelectMany(c => c.GetIdentifyers()).Distinct().ToList());
                        foreach (dtoLiteMapActivity dto in savingActivities)
                        {

                            if (dto.InEditPredecessorsMode &&((idToReview.Any() && !idToReview.Contains(dto.IdActivity)) || (!idToReview.Any() && hasCycles ==false)) && !dto.Current.IdActivityLinks.Contains(dto.IdActivity))
                            {

                                PmActivity activity = Manager.Get<PmActivity>(dto.IdActivity);
                                if (activity != null)
                                {
                                    IList<PmActivityLink> predecessors = activity.PredecessorLinks;
                                    foreach (PmActivityLink predecessor in predecessors.Where(p => p.Deleted == BaseStatusDeleted.None && p.Target != null && !dto.Current.IdActivityLinks.Contains(p.Target.Id)))
                                    {
                                        predecessor.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    foreach (PmActivityLink predecessor in predecessors.Where(p => p.Target != null && dto.Current.IdActivityLinks.Contains (p.Target.Id)))
                                    {
                                        ParsedActivityLink link = dto.Current.Links.Where(l => l.Id == predecessor.Target.Id).FirstOrDefault();
                                        if (link!=null){
                                            if (predecessor.Deleted== BaseStatusDeleted.None)
                                                predecessor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            else
                                                predecessor.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            predecessor.LeadLag = link.LeadLag;
                                            predecessor.Type = link.LinkType;
                                        }
                                    }
                                    foreach (long idTarget in dto.Current.IdActivityLinks.Where(id => !predecessors.Where(p => p.Target != null && p.Target.Id == id).Any())) {
                                        ParsedActivityLink link = dto.Current.Links.Where(l => l.Id == idTarget).FirstOrDefault();
                                        if (link != null)
                                        {
                                            PmActivityLink activityLink = new PmActivityLink();
                                            activityLink.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            activityLink.Type = link.LinkType;
                                            activityLink.LeadLag = link.LeadLag;
                                            activityLink.Project = activity.Project;
                                            activityLink.Source = activity;
                                            activityLink.Target = Manager.Get<PmActivity>(link.Id);
                                            if (activityLink.Target != null)
                                            {
                                                Manager.SaveOrUpdate(activityLink);
                                                activity.PredecessorLinks.Add(activityLink);
                                                Manager.SaveOrUpdate(activity);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                  
                    //idToReview = idToReview.Where(id=> savingActivities.Where(s=> s.InEditMode =
                }
                private Boolean HasCycles(Project project, List<dtoLiteMapActivity> savingActivities)
                {
                    return HasCycles(UpdateGraph(GetGraphActivities(project), savingActivities));
                }
                private Boolean HasCycles(List<dtoGraphActivity> activities)
                {
                    return !CPMExtensions.HasValidDependencies(activities);
                }
                private IList<DirectedGraphCycle> GetCycles(Project project, List<dtoLiteMapActivity> savingActivities)
                {
                    return GetCycles(UpdateGraph(GetGraphActivities(project), savingActivities));
                }
                private IList<DirectedGraphCycle> GetCycles(List<dtoGraphActivity> activities)
                {
                    IList<DirectedGraphNode> nodes = activities.ToNodeList(a => a.IdActivity, a => a.Links.Select(l => l.IdPredecessor));

                    return nodes.FindAllCyclesDisconnectedGraph().ToList();
                }
                public List<long> GetAvailableIdPredecessors(dtoActivity source, List<dtoLiteMapActivity> savingActivities)
                {
                    return GetAvailableIdPredecessors(source.IdProject, source.Id, savingActivities);
                }
                public List<long> GetAvailableIdPredecessors(long idProject, long idSource, List<dtoLiteMapActivity> savingActivities)
                {
                    List<long> results = new List<long>();
                    savingActivities.ForEach(sa => sa.UpdateIdPredecessors(savingActivities));
                    List<dtoGraphActivity> activities = UpdateGraph(GetGraphActivities(idProject), savingActivities);
                    foreach (dtoGraphActivity act in activities.Where(a => !a.IsSummary && a.IdActivity != idSource && !a.Links.Where(l => l.IdPredecessor == idSource).Any()))
                    {
                        dtoGraphActivity activity = activities.Where(a => a.IdActivity == idSource).FirstOrDefault();
                        activity.Links.Add(new dtoGraphActivityLink() { IdPredecessor = act.IdActivity });
                        IList<DirectedGraphNode> nodes = activities.ToNodeList(a => a.IdActivity, a => a.Links.Select(l => l.IdPredecessor));
                        List<DirectedGraphCycle> cycles = nodes.FindAllCyclesDisconnectedGraph().ToList();
                        if (cycles == null || !cycles.Any() || !cycles.Where(c => c.Members.Where(m => m.A.Id == act.IdActivity || m.B.Id == act.IdActivity).Any()).Any())
                            results.Add(act.IdActivity);
                        activity.Links.RemoveAt(activity.Links.Count - 1);
                    }
                    return results;
                }
                public List<long> CheckAvailableIdPredecessors(long idProject, long idSource, List<long> idPredecessors ,List<dtoLiteMapActivity> savingActivities= null)
                {
                    List<long> results = new List<long>();
                    List<dtoGraphActivity> activities = null;
                    if (savingActivities == null)
                        activities = GetGraphActivities(idProject);
                    else
                    {
                        savingActivities.ForEach(sa => sa.UpdateIdPredecessors(savingActivities));
                        activities = UpdateGraph(GetGraphActivities(idProject), savingActivities);
                    }
                    foreach (dtoGraphActivity act in activities.Where(a => !a.IsSummary && a.IdActivity != idSource && idPredecessors.Contains(a.IdActivity) && !a.Links.Where(l => l.IdPredecessor == idSource).Any()))
                    {
                        dtoGraphActivity activity = activities.Where(a => a.IdActivity == idSource).FirstOrDefault();
                        activity.Links.Add(new dtoGraphActivityLink() { IdPredecessor = act.IdActivity });
                        IList<DirectedGraphNode> nodes = activities.ToNodeList(a => a.IdActivity, a => a.Links.Select(l => l.IdPredecessor));
                        List<DirectedGraphCycle> cycles = nodes.FindAllCyclesDisconnectedGraph().ToList();
                        if (cycles == null || !cycles.Any() || !cycles.Where(c => c.Members.Where(m => m.A.Id == act.IdActivity || m.B.Id == act.IdActivity).Any()).Any())
                            results.Add(act.IdActivity);
                        activity.Links.RemoveAt(activity.Links.Count - 1);
                    }
                    return results;
                }
                private Boolean GenerateCycleForActivity(long idActivity, long idPredecessor, long idProject, List<dtoLiteMapActivity> savingActivities)
                {
                    List<DirectedGraphCycle> cycles = VerifyPredecessorForActivity(idActivity, idPredecessor, idProject, savingActivities);
                    return !(cycles == null || !cycles.Any() || !cycles.Where(c => c.Members.Where(m => m.A.Id == idActivity || m.B.Id == idActivity).Any()).Any());
                }
                private List<DirectedGraphCycle> VerifyPredecessorForActivity(long idActivity, long idPredecessor, long idProject, List<dtoLiteMapActivity> savingActivities)
                {
                    List<dtoGraphActivity> activities = UpdateGraph(GetGraphActivities(idProject), savingActivities); 
                    dtoGraphActivity  activity = activities.Where(a=> a.IdActivity== idActivity && !a.Links.Where(l=> l.IdPredecessor == idPredecessor).Any()).FirstOrDefault();
                    if (activity != null) {
                        activity.Links.Add(new dtoGraphActivityLink() { IdPredecessor = idPredecessor });
                    }
                    IList<DirectedGraphNode> nodes = activities.ToNodeList(a => a.IdActivity, a => a.Links.Select(l => l.IdPredecessor));

                    return nodes.FindAllCyclesDisconnectedGraph().ToList();
                }

                private Boolean GenerateCycleForActivity(long idActivity, List<dtoActivityLink> currentLinks, long idPredecessor, long idProject, List<dtoLiteMapActivity> savingActivities)
                {
                    List<DirectedGraphCycle> cycles = VerifyPredecessorForActivity(idActivity,currentLinks, idPredecessor, idProject, savingActivities);
                    return !(cycles == null || !cycles.Any() || !cycles.Where(c => c.Members.Where(m => m.A.Id == idActivity || m.B.Id == idActivity).Any()).Any());
                }
                private List<DirectedGraphCycle> VerifyPredecessorForActivity(long idActivity, List<dtoActivityLink> currentLinks, long idPredecessor, long idProject, List<dtoLiteMapActivity> savingActivities)
                {
                    List<dtoGraphActivity> activities = UpdateGraph(GetGraphActivities(idProject), savingActivities);
                    dtoGraphActivity activity = activities.Where(a => a.IdActivity == idActivity && !a.Links.Where(l => l.IdPredecessor == idPredecessor).Any()).FirstOrDefault();
                    if (activity != null)
                    {
                        activity.Links = currentLinks.Where(l => !l.Deleted).Select(l => new dtoGraphActivityLink() { IdPredecessor = l.IdTarget }).ToList();
                        activity.Links.Add(new dtoGraphActivityLink() { IdPredecessor = idPredecessor });
                    }
                    IList<DirectedGraphNode> nodes = activities.ToNodeList(a => a.IdActivity, a => a.Links.Select(l => l.IdPredecessor));

                    return nodes.FindAllCyclesDisconnectedGraph().ToList();
                }
                #region "Graph Methods"
                private List<dtoGraphActivity> UpdateGraph(List<dtoGraphActivity> activities, List<dtoLiteMapActivity> savingActivities)
                {
                    foreach (dtoLiteMapActivity sActivity in savingActivities)
                    {
                        dtoGraphActivity gActivity = activities.Where(a => a.IdActivity == sActivity.IdActivity).FirstOrDefault();
                        if (gActivity != null)
                        {
                            List<ParsedActivityLink> links = CPMExtensions.ParseActivityLinks(sActivity.Current.PredecessorsIdString).ToList();
                            gActivity.Links = links.Select(l => new dtoGraphActivityLink()
                            {
                                IdPredecessor = l.Id,
                                LeadLag = l.LeadLag,
                                Type = l.LinkType,
                            }).ToList().Where(l => l.IdPredecessor > 0).ToList();
                        }
                    }
                    return activities;
                }
                private List<dtoGraphActivity> GetGraphActivities(Project project)
                {
                    List<dtoGraphActivity> results = new List<dtoGraphActivity>();
                    List<PmActivity> pActivities = project.Activities.Where(a=> a.Depth== 0 && a.Deleted ==  BaseStatusDeleted.None).ToList().OrderBy(a=> a.DisplayOrder).ThenBy(a=> a.Id).ToList();
                    long rNumber = 1;
                    foreach (PmActivity activity in pActivities)
                    {
                        results.Add(new dtoGraphActivity(activity, rNumber++));
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        {
                            results.AddRange(GetGraphActivities(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToList(), ref rNumber));
                        }
                    }
                    return results;
                }
                private List<dtoGraphActivity> GetGraphActivities(List<PmActivity> activities, ref long rNumber)
                {
                    List<dtoGraphActivity> results = new List<dtoGraphActivity>();
                    foreach (PmActivity activity in activities)
                    {
                        results.Add(new dtoGraphActivity(activity, rNumber++));
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        {
                            results.AddRange(GetGraphActivities(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToList(), ref rNumber));
                        }
                    }
                    return results;
                }
                private List<dtoGraphActivity> GetGraphActivities(long idProject)
                {
                    List<dtoGraphActivity> results = new List<dtoGraphActivity>();
                    List<litePmActivity> pActivities = (from a in Manager.GetIQ<litePmActivity>()
                                                        where a.Deleted == BaseStatusDeleted.None && a.IdProject == idProject && a.Depth == 0
                                                        orderby a.DisplayOrder, a.Id
                                                        select a).ToList();
                    long rNumber = 1;
                    foreach (litePmActivity activity in pActivities)
                    {
                        results.Add(new dtoGraphActivity(activity, rNumber++));
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        {
                            results.AddRange(GetGraphActivities(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToList(), ref rNumber));
                        }
                    }
                    return results;
                }
                private List<dtoGraphActivity> GetGraphActivities(List<litePmActivity> activities, ref long rNumber)
                {
                    List<dtoGraphActivity> results = new List<dtoGraphActivity>();
                    foreach (litePmActivity activity in activities)
                    {
                        results.Add(new dtoGraphActivity(activity, rNumber++));
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        {
                            results.AddRange(GetGraphActivities(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToList(), ref rNumber));
                        }
                    }
                    return results;
                }
                #endregion
            #endregion

            #region "Delete Methods"
                public Boolean SetActivityVirtualDelete(long idActivity, Boolean delete)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        PmActivity activity = Manager.Get<PmActivity>(idActivity);
                        if (person != null && activity != null)
                        {
                            SetActivityVirtualDelete(activity, person, delete);
                            InternalRecalculateProjectMap(activity.Project, person);
                            ClearCacheItems(activity.Project);
                        }
                        Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return result;
                }
                private void SetActivityVirtualDelete(PmActivity activity, litePerson person, Boolean delete)
                {

                    activity.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                    activity.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);

                    foreach (PmActivityLink predecessor in activity.PredecessorLinks)
                    {
                        predecessor.Deleted = delete ? (predecessor.Deleted | BaseStatusDeleted.Cascade) : (predecessor.Deleted = (BaseStatusDeleted)((int)predecessor.Deleted - (int)BaseStatusDeleted.Cascade));
                        predecessor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    //foreach (PmActivityLink successor in activity.SuccessorLinks)
                    //{
                    //    successor.Deleted = delete ? (successor.Deleted | BaseStatusDeleted.Cascade) : (successor.Deleted = (BaseStatusDeleted)((int)successor.Deleted - (int)BaseStatusDeleted.Cascade));
                    //    successor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    //}
                    List<ProjectActivityAssignment> assignments = (from a in Manager.GetIQ<ProjectActivityAssignment>()
                                                                   where ((delete && a.Deleted == BaseStatusDeleted.None) || (!delete && a.Deleted == BaseStatusDeleted.Manual)) && a.Activity != null && a.Activity.Id == activity.Id
                                                                   select a).ToList();
                    foreach (ProjectActivityAssignment assignment in assignments)
                    {
                        assignment.Deleted = delete ? (assignment.Deleted | BaseStatusDeleted.Cascade) : (assignment.Deleted = (BaseStatusDeleted)((int)assignment.Deleted - (int)BaseStatusDeleted.Cascade));
                        assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        //if (assignment.Resource !=null)
                        //    UpdateResourceStatus(assignment.Resource); 
                    }
                    if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                    {
                        foreach (PmActivity child in activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None))
                        {
                            SetActivityVirtualDelete(child, person, delete);
                        }
                    }
                    if (activity.Parent != null )
                        activity.Parent.IsSummary = activity.Parent.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any();
                }
            #endregion

            #region "Completion Methods"
                public Dictionary<long, dtoCompletion> GetActivityCompletionTree(long idActivity)
                {
                    Dictionary<long, dtoCompletion> results = new Dictionary<long, dtoCompletion>();
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        PmActivity activity = Manager.Get<PmActivity>(idActivity);
                        if (activity != null) {
                            results.Add(0, new dtoCompletion() { Completeness = activity.Project.Completeness, IsCompleted = activity.Project.IsCompleted });    
                            results.Add(activity.Id, new dtoCompletion() { Completeness = activity.Completeness, IsCompleted = activity.IsCompleted });
                            while (activity.Parent != null) {
                                activity = activity.Parent;
                                results.Add(activity.Id, new dtoCompletion() { Completeness = activity.Completeness, IsCompleted = activity.IsCompleted });    
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                        {
                            Manager.RollBack();
                            results = null;
                        }
                        else
                            throw ex;
                    }
                    return results;
                }

                public dtoTaskCompletion SetTaskCompletion(long idTask, Boolean isCompleted, ref Boolean cacheupdated, List<dtoActivityCompletion> cItems = null)
                {
                    dtoTaskCompletion result = null;
                    try
                    {
                        Manager.BeginTransaction();
                        PmActivity task = Manager.Get<PmActivity>(idTask);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        Boolean updateCache = false;
                        if (person != null && task != null && task.Project!=null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                        {
                            task.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            task.Completeness = (isCompleted) ? 100 :  task.Completeness;
                            Manager.SaveOrUpdate(task);
 
                            #region "Assignments"
                            if (!task.IsSummary)
                            {
                                foreach (ProjectActivityAssignment assignment in task.CurrentAssignments) {
                                    dtoActivityCompletion dtoAssignment = (cItems == null) ? null : cItems.Where(i => i.Id == assignment.Id).FirstOrDefault();
                                    assignment.Completeness = (isCompleted) ? 100 : (dtoAssignment == null ? 0 : dtoAssignment.Completeness);
                                    assignment.IsApproved = (isCompleted) ? true : (dtoAssignment == null ? false : dtoAssignment.IsApproved);
                                    assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }

                                if (task.IsCompleted != isCompleted)
                                    updateCache = true;
                                task.IsCompleted = isCompleted;
                                CalculateTaskStatusAndCompleteness(task, true);
                            }
                            else
                            {
                                if (task.Assignments != null && task.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Any())
                                {
                                    foreach (var a in task.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None))
                                    {
                                        a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        updateCache = true;
                                    }
                                }
                                if (isCompleted && !task.IsCompleted)
                                {
                                    SetActionCompleted(task);
                                    if (task.Parent != null)
                                        CalculateTaskStatusAndCompleteness(task.Parent, true);
                                    else if (task.Depth == 0)
                                    {
                                        CalculateProjectStatusAndCompletenessByChildren(task.Project);
                                       // task.Project.Completeness = (int)task.Project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None && a.Depth == 0 && a.Parent == null).Select(a => a.Completeness).Average();
                                        //task.Project.IsCompleted = (task.Project.Completeness == 100) && !task.Project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None && a.Depth == 0 && !a.IsCompleted).Any();
                                    }
                                    updateCache = true;
                                }
                                else if (isCompleted != task.IsCompleted)
                                {        
                                    CalculateTaskStatusAndCompleteness(task, true);
                                    updateCache = true;
                                }
                            }

                            #endregion

                            if (task.Status != ProjectItemStatus.suspended){
                                switch(task.Completeness){
                                    case 100:
                                        task.Status = (task.IsCompleted) ? ProjectItemStatus.completed: ProjectItemStatus.started;
                                        break;
                                    case 0:
                                        task.Status = ProjectItemStatus.notstarted;
                                        break;
                                    default:
                                        task.Status = ProjectItemStatus.started;
                                        break;
                                }
                            }
                            if (updateCache)
                            {
                                task.CurrentAssignments.Where(a => a.Resource.ProjectRole == ActivityRole.Manager || a.Resource.ProjectRole == ActivityRole.ProjectOwner).ToList().ForEach(a => UpdateResourceStatus(a.Resource));
                                task.Project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && (r.ProjectRole == ActivityRole.Manager || r.ProjectRole == ActivityRole.ProjectOwner)).ToList().ForEach(r => UpdateResourceStatus(r));
                            }
                            result = new dtoTaskCompletion() { IdTask = task.Id, Completion = task.Completeness, IsCompleted = task.IsCompleted, Status = task.Status };
                        }
                        Manager.Commit();
                        if (updateCache)
                        {
                            ClearCacheItems(task.Project);
                            cacheupdated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = null;
                    }
                    return result;
                }

                public dtoMyAssignmentCompletion UpdateTaskCompletion(long idTask, long idAssignment, Int32 completeness, ref Boolean cacheupdated)
                {
                    dtoMyAssignmentCompletion result = null;
                    try
                    {
                        Manager.BeginTransaction();
                        PmActivity task = Manager.Get<PmActivity>(idTask);
                        ProjectActivityAssignment assignment = Manager.Get<ProjectActivityAssignment>(idAssignment);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        Boolean updateCache = false;
                        if (person != null && task != null && task.Project != null && assignment !=null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                        {
                            assignment.Completeness = (completeness < 0) ? 0 : (completeness > 100 ? 100 : completeness);
                            assignment.IsApproved = (completeness == 100 && !task.Project.ConfirmCompletion) ? true : false;
                            assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            CalculateTaskStatusAndCompleteness(task, true);
                         
                            if (task.Status != ProjectItemStatus.suspended){
                                switch(task.Completeness){
                                    case 100:
                                        task.Status = (task.IsCompleted) ? ProjectItemStatus.completed: ProjectItemStatus.started;
                                        updateCache = true;
                                        break;
                                    case 0:
                                        task.Status = ProjectItemStatus.notstarted;
                                        break;
                                    default:
                                        task.Status = ProjectItemStatus.started;
                                        break;
                                }
                            }
                            task.CurrentAssignments.Where(a => a.Resource.ProjectRole == ActivityRole.Manager || a.Resource.ProjectRole == ActivityRole.ProjectOwner).ToList().ForEach(a => UpdateResourceStatus(a.Resource));
                            task.Project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && ( r.ProjectRole == ActivityRole.Manager ||  r.ProjectRole == ActivityRole.ProjectOwner)).ToList().ForEach(r => UpdateResourceStatus(r));
                        }
                        Manager.Commit();
                        if (task!=null && assignment != null){
                            result = new dtoMyAssignmentCompletion() { IdTask = task.Id, IdAssignment = assignment.Id, IdProject = (task.Project != null ? task.Project.Id : 0 ),TaskStatus = task.Status, TaskCompletion = task.Completeness, TaskIsCompleted = task.IsCompleted, ProjectCompletion = GetResourceCompletion(assignment.Resource), MyCompletion = new dtoField<string>(assignment.Completeness.ToString() + "%") { Status = FieldStatus.updated, IsUpdated = true } };
                        }
                        if (task != null && updateCache)
                        {
                            cacheupdated = true;
                            task.CurrentAssignments.Where(a => (a.Resource.ProjectRole == ActivityRole.Manager || a.Resource.ProjectRole == ActivityRole.ProjectOwner) && a.Resource.Type == ResourceType.Internal && a.Resource.Person != null).Select(a=> a.Resource.Person.Id).Distinct().ToList().ForEach(p => ClearCacheItems(p));
                            task.Project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && r.Type== ResourceType.Internal && r.Person !=null && (r.ProjectRole == ActivityRole.Manager || r.ProjectRole == ActivityRole.ProjectOwner)).ToList().ForEach(r => ClearCacheItems(r.Person.Id));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction()) {
                            result = null;
                            Manager.RollBack();
                        }
                    }
                    return result;
                }
                public Boolean UpdateTasksCompletion(List<dtoMyAssignmentCompletion> items, ref Int32 savedTasks, ref Int32 unsavedTasks, ref Boolean cacheupdated)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        Boolean updateCache = false;
                        List<ProjectActivityAssignment> assignments = new List<ProjectActivityAssignment>();

                        if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser){
                            Boolean updateToDo = false;
                            foreach(dtoMyAssignmentCompletion item in items.Where(i=>i.MyCompletion.InEditMode)){
                                String itemCompleteness = item.MyCompletion.Current;
                                if (!String.IsNullOrEmpty(itemCompleteness))
                                    itemCompleteness = itemCompleteness.Replace("%","");
                                Int32 completeness = 0;
                                if (Int32.TryParse(itemCompleteness, out completeness) && !String.IsNullOrEmpty(itemCompleteness) && (completeness >= 0 || completeness <= 100))
                                {
                                    item.MyCompletion.IsUpdated = true;
                                    Manager.BeginTransaction();
                                    ProjectActivityAssignment assignment = Manager.Get<ProjectActivityAssignment>(item.IdAssignment);
                                    if (assignment != null && assignment.Completeness != completeness)
                                    {
                                        assignment.Completeness = completeness;
                                        assignment.IsApproved = (completeness == 100 && !assignment.Project.ConfirmCompletion) ? true : false;
                                        assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(assignment);

                                        CalculateTaskStatusAndCompleteness(assignment.Activity, true);

                                        if (assignment.Activity.Status != ProjectItemStatus.suspended)
                                        {
                                            switch (assignment.Activity.Completeness)
                                            {
                                                case 100:
                                                    assignment.Activity.Status = (assignment.Activity.IsCompleted) ? ProjectItemStatus.completed : ProjectItemStatus.started;
                                                    updateCache = true;
                                                    break;
                                                case 0:
                                                    assignment.Activity.Status = ProjectItemStatus.notstarted;
                                                    break;
                                                default:
                                                    assignment.Activity.Status = ProjectItemStatus.started;
                                                    break;
                                            }
                                        }
                                        assignments.Add(assignment);
                                        item.MyCompletion.IsUpdated = true;
                                        item.TaskCompletion = assignment.Activity.Completeness;
                                        item.TaskIsCompleted = assignment.Activity.IsCompleted;
                                        item.MyCompletion.Status = FieldStatus.updated;
                                        item.TaskStatus=assignment.Activity.Status;
                                        item.IdProject = (assignment.Project != null) ? assignment.Project.Id : 0;
                                        item.IdResource = (assignment.Resource != null) ? assignment.Resource.Id : 0;
                                        updateToDo = true;
                                    }
                                    savedTasks++;
                                    Manager.Commit();
                                    item.MyCompletion.InEditMode = false;
                                }
                                else
                                {
                                    unsavedTasks++;
                                    item.MyCompletion.Edit = itemCompleteness;
                                    item.MyCompletion.Status = FieldStatus.error;
                                }
                            }

                            if (updateToDo)
                            {
                                assignments.Select(a=>a.Resource).Distinct().ToList().ForEach(a => UpdateResourceStatus(a));
                            }
                        }
                        result = true;
                        if (updateCache && assignments.Any())
                        {
                            assignments.Where(a => (a.Resource.ProjectRole == ActivityRole.Manager || a.Resource.ProjectRole == ActivityRole.ProjectOwner) && a.Resource.Type == ResourceType.Internal && a.Resource.Person != null).Select(a => a.Resource.Person.Id).Distinct().ToList().ForEach(p => ClearCacheItems(p));
                            assignments.First().Project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && r.Type == ResourceType.Internal && r.Person != null && (r.ProjectRole == ActivityRole.Manager || r.ProjectRole == ActivityRole.ProjectOwner)).ToList().ForEach(r => ClearCacheItems(r.Person.Id));
                            cacheupdated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                private void SetActionCompleted(PmActivity activity) {
                    if (activity.IsSummary) {
                        activity.IsCompleted = true;
                        activity.Completeness = 100;
                        activity.ActualDuration = activity.Duration;
                        foreach( PmActivity child in activity.Children.Where(c=> c.Deleted== BaseStatusDeleted.None)){
                            SetActionCompleted(child);
                        }
                    }
                    else
                    {
                        activity.IsCompleted = true;
                        activity.Completeness=100;
                        activity.ActualDuration = activity.Duration;
                        activity.Status = (activity.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.completed : activity.Status;
                        foreach(ProjectActivityAssignment assignment in activity.CurrentAssignments.Where(a=> !a.IsApproved || a.Completeness<100)){
                            assignment.Completeness = 100;
                            assignment.IsApproved = true;
                        }
                    }
                    
                }
            #endregion
            #region "Resources Methods"
                public List<dtoResource> SetActivityResources(long idActivity, List<long> idResources, ref Boolean complessionRecalculated, String unknownUser)
                {
                    List<dtoResource> resources = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        Boolean isModified = false;
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        PmActivity activity = Manager.Get<PmActivity>(idActivity);
                        if (person != null && person.TypeID != (Int32)UserTypeStandard.Guest && activity!=null)
                        {
                            List<ProjectActivityAssignment> assignments = (from a in Manager.GetIQ<ProjectActivityAssignment>()
                                                                               where a.Activity !=null  && a.Activity.Id == idActivity
                                                                               select a).ToList();
                          
                            if (idResources.Any())
                            {
                                foreach (var a in assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Resource != null && !idResources.Contains(a.Resource.Id)))
                                {
                                    a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    isModified = true;
                                }
                                foreach (var a in assignments.Where(a => a.Deleted != BaseStatusDeleted.None && a.Resource != null && idResources.Contains(a.Resource.Id)))
                                {
                                    a.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    a.Role = ActivityRole.Resource;
                                    a.Permissions = GetRolePermissionsToLong(ActivityRole.Resource);
                                    isModified = true;
                                }
                                foreach (long idResource in idResources.Where(r => !assignments.Where(a => a.Resource != null && a.Resource.Id == r).Any())) {
                                    isModified = true;
                                    ProjectResource resource = Manager.Get<ProjectResource>(idResource);
                                    if (resource !=null){
                                        ProjectActivityAssignment assignment = new ProjectActivityAssignment();
                                        assignment.Activity = activity;
                                        assignment.Completeness = 0;
                                        assignment.Project = activity.Project;
                                        assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        assignment.Role = ActivityRole.Resource;
                                        assignment.Permissions = GetRolePermissionsToLong(ActivityRole.Resource);
                                        assignment.Visibility = resource.Visibility;
                                        assignment.Resource = resource;
                                        assignment.Person = resource.Person;
                                        Manager.SaveOrUpdate(assignment);
                                        assignments.Add(assignment);
                                    }
                                }
                            }
                            else if (assignments.Any())
                            {
                                isModified = assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Any();
                                foreach (var a in assignments.Where(a => a.Deleted == BaseStatusDeleted.None))
                                {
                                    a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                            }
                            if (isModified)
                            {
                                Manager.SaveOrUpdateList(assignments);
                                if (assignments.Any())
                                    assignments.Where(a=> a.Resource !=null).ToList().ForEach(a=> UpdateResourceStatus(a.Resource)); 
                                  
                                complessionRecalculated = true;
                                RecalculateProjectCompleteness(activity.Project, person);
                                ClearCacheItems(activity.Project);
                            }
                            resources = assignments.Where(a => a.Resource != null && a.Deleted == BaseStatusDeleted.None).Select(a => new dtoResource(a.Resource, unknownUser)).ToList();
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                        {
                            Manager.RollBack();
                            resources = null;
                        }
                        else
                            throw ex;
                    }
                    return resources;
            
                }
                public List<long> GetIdActivitiesByResource(long idResource)
                {
                    List<long> idActivities = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        idActivities = (from a in Manager.GetIQ<liteProjectActivityAssignment>()
                                        where a.Deleted == BaseStatusDeleted.None && a.Resource.Id == idResource
                                        select a.Activity.Id).ToList();
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                        {
                            Manager.RollBack();
                            idActivities = null;
                        }
                        else
                            throw ex;
                    }
                    return idActivities;

                }
                public Dictionary<long, List<dtoResource>> GetResourcesForActivities(long idProject, String unknownUser)
                {
                    Dictionary<long, List<dtoResource>> results = new Dictionary<long, List<dtoResource>>();
                    try
                    {
                        Manager.BeginTransaction();

                        results = GetResourcesForActivities(idProject, (from r in Manager.GetIQ<ProjectResource>() where r.Deleted == BaseStatusDeleted.None && r.Project != null && r.Project.Id == idProject select new dtoResource(r, unknownUser)).ToList());

                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        results = null;
                    }
                    return results;
                }
                public Dictionary<long, List<dtoResource>> GetResourcesForActivities(long idProject, List<dtoResource> resources)
                {
                    Dictionary<long, List<dtoResource>> results = new Dictionary<long, List<dtoResource>>();
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        List<liteProjectActivityAssignment> assignments = (from a in Manager.GetIQ<liteProjectActivityAssignment>()
                                                                           where a.Deleted == BaseStatusDeleted.None && a.Project != null && a.Project.Id == idProject 
                                                            select a).ToList();
                        foreach(var assignment in assignments.GroupBy(a => a.Activity.Id)){
                            results.Add(assignment.Key, resources.Where(r => assignment.Where(a => a.Resource.Id == r.IdResource).Any()).ToList());
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                        {
                            Manager.RollBack();
                            results = null;
                        }
                        else
                            throw ex;
                    }
                    return results;
                }
            #endregion

        #endregion

        
        #region "Predecessors"
            private void LinkActivities(litePerson person, PmActivity predecessor, List<PmActivity> activities)
            {
                if (predecessor != null && activities.Any())
                {
                    PmActivity current = activities.FirstOrDefault();
                    if (current != null)
                    {
                        if (current.PredecessorLinks == null)
                            current.PredecessorLinks = new List<PmActivityLink>();
                        PmActivityLink link = current.PredecessorLinks.Where(p => p.Target != null && p.Target.Id == predecessor.Id).FirstOrDefault();
                        if (link == null)
                        {
                            link = new PmActivityLink();
                            link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            link.Type = PmActivityLinkType.FinishStart;
                            link.Project = current.Project;
                            link.Source = current;
                            link.Target = predecessor;
                            Manager.SaveOrUpdate(link);
                            current.PredecessorLinks.Add(link);
                            Manager.SaveOrUpdate(current);
                        }
                        else if (link.Deleted == BaseStatusDeleted.None)
                        {
                            link.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            Manager.SaveOrUpdate(link);
                        }
                        LinkActivities(person, current, activities.Where(a => a.Id != current.Id).ToList());
                        Manager.SaveOrUpdate(current);
                    }
                }
            }
            public List<PmActivityLink> AddPredecessors(long idActivity, List<long> idPredecessors, List<dtoLiteMapActivity> savingActivities)
            {
                List<PmActivityLink> results = new List<PmActivityLink>();
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    PmActivity activity = Manager.Get<PmActivity>(idActivity);
              
                    if (person != null  && activity != null && activity.Project != null && (activity.Children == null || !activity.IsSummary))
                    {
                        List<PmActivityLink> links = (activity.PredecessorLinks == null) ? new List<PmActivityLink>() : activity.PredecessorLinks.Where(p => p.Target != null).ToList();

                        idPredecessors = CheckAvailableIdPredecessors(activity.Project.Id, activity.Id, idPredecessors, savingActivities);

                        foreach (long idPredecessor in idPredecessors.Where(i => i != idActivity).Distinct())
                        {
                            PmActivity predecessor = Manager.Get<PmActivity>(idPredecessor);
                            if (!predecessor.IsSummary && !GenerateCycleForActivity(idActivity,idPredecessor,activity.Project.Id,savingActivities ))
                            {
                                PmActivityLink link = links.Where(p => p.Target.Id == idPredecessor).FirstOrDefault();
                                if (link == null)
                                {
                                    link = new PmActivityLink();
                                    link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    link.Project = activity.Project;
                                    link.Source = activity;
                                    link.Target = predecessor;

                                    links.Add(link);
                                    Manager.SaveOrUpdate(link);
                                    activity.PredecessorLinks.Add(link);
                                    Manager.SaveOrUpdate(link);
                                }
                                else if (link.Deleted != BaseStatusDeleted.None)
                                {
                                    link.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    link.LeadLag = 0;
                                    link.Type = PmActivityLinkType.FinishStart;
                                    Manager.SaveOrUpdate(link);
                                    results.Add(link);
                                }
                            }

                        }
                        if (activity.Project != null)
                        {
                            InternalRecalculateProjectMap(activity.Project, person);
                            ClearCacheItems(activity.Project);
                        }
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    results = null;
                }
                return results;
            }

            public List<PmActivityLink> SetActivityPredecessors(litePerson person, PmActivity activity, List<dtoActivityLink> dtoLinks, ref List<long> idPredecessorCycles, ref Boolean update)
            {
                List<PmActivityLink> links = (activity.PredecessorLinks == null) ? new List<PmActivityLink>() : activity.PredecessorLinks.Where(p => p.Target != null).ToList();

                if (activity.IsSummary && links.Any())
                {
                    foreach (PmActivityLink link in links)
                    {
                        update = true;
                        link.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                }
                else
                {
                    foreach (PmActivityLink link in links.Where(l => (dtoLinks.Where(d => d.Id > 0 && d.Deleted).Any() && l.Deleted == BaseStatusDeleted.None) || l.Target == null))
                    {
                        update = true;
                        link.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    foreach (PmActivityLink link in links.Where(l => l.Deleted == BaseStatusDeleted.None))
                    {
                        dtoActivityLink dtoLink = dtoLinks.Where(l => l.IdTarget == link.Target.Id && (link.LeadLag != l.LeadLag || link.Type != l.Type)).FirstOrDefault();
                        if (dtoLink != null)
                        {
                            link.LeadLag = dtoLink.LeadLag;
                            link.Type = dtoLink.Type;
                        }
                    }
                    List<PmActivityLink> added = new List<PmActivityLink>();
                    foreach (dtoActivityLink dtoLink in dtoLinks.Where(i => i.IdTarget != activity.Id).Distinct())
                    {
                        PmActivity predecessor = Manager.Get<PmActivity>(dtoLink.IdTarget);
                        if (predecessor != null && !predecessor.Predecessors.Where(p => p.Target.Id == activity.Id).Any()) {
                            PmActivityLink link = links.Where(p => p.Target.Id == dtoLink.IdTarget).FirstOrDefault();
                            if (link == null && predecessor != null && !predecessor.IsSummary)
                            {
                                link = new PmActivityLink();
                                link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                link.Project = activity.Project;
                                link.Source = activity;
                                link.Target = predecessor;
                                link.LeadLag = dtoLink.LeadLag;
                                link.Type = dtoLink.Type;


                                added.Add(link);
                                links.Add(link);
                                Manager.SaveOrUpdate(link);
                                activity.PredecessorLinks.Add(link);
                                Manager.SaveOrUpdate(link);

                                update = true;
                            }
                            else if (link.Deleted != BaseStatusDeleted.None)
                            {
                                link.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                link.LeadLag = dtoLink.LeadLag;
                                link.Type = dtoLink.Type;
                                Manager.SaveOrUpdate(link);
                                added.Add(link);
                                update = true;
                            }
                            else
                            {
                                link.LeadLag = dtoLink.LeadLag;
                                link.Type = dtoLink.Type;
                            }
                        }
                    }
                    if (added.Any()) {
                        while (added.Any() && !CPMExtensions.HasValidDependencies(activity.Project))
                        {
                            PmActivityLink link = added.First();
                            link.Deleted = BaseStatusDeleted.Manual;
                            if (CPMExtensions.HasValidDependencies(activity.Project))
                            {
                                idPredecessorCycles.Add(link.Target.Id);
                                if (dtoLinks.Where(l => l.Id == 0 && l.IdTarget == link.Target.Id).Any())
                                    activity.PredecessorLinks.Remove(link);
                            }
                            added.Remove(link);
                            links.Remove(link);
                        }
                    }
                }
                return links;
            }
                          
            //public Boolean isCPMValidated(Project project)
            //{
            //    try
            //        {

            //            CPMresult cpmResult = DefaultCPM(project.AllowSummary,project.Activities.Where(a=> a.Deleted == BaseStatusDeleted.None).ToList(), project.StartDate, project.DaysOfWeek, null, project.DefaultWorkingDay.HoursRange);

            //            return true;
            //        }
            //    catch(Exception ex){
            //        return false;
            //    }
            //}

            public List<dtoActivityLink> AddTemporaryPredecessors(long idActivity, List<long> idPredecessors,List<dtoActivityLink> currentLinks, List<dtoLiteMapActivity> savingActivities)
            {
                List<dtoActivityLink> results = new List<dtoActivityLink>();
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    PmActivity activity = Manager.Get<PmActivity>(idActivity);
                    dtoLiteMapActivity sActivity = savingActivities.Where(s=> s.IdActivity==idActivity).FirstOrDefault();
                    if (person != null && sActivity!=null && activity != null && activity.Project != null && (activity.Children == null || !activity.IsSummary))
                    {
                        foreach (long idPredecessor in idPredecessors.Where(i => i != idActivity).Distinct())
                        {
                            dtoActivityLink tLink = currentLinks.Where(c=> c.IdTarget==idPredecessor).FirstOrDefault();
                            if (tLink !=null && tLink.Deleted){
                                tLink.Deleted = false;
                            }
                            else if (tLink==null){
                                tLink = new dtoActivityLink();
                                tLink.IdSource = idActivity;
                                tLink.IdTarget = idPredecessor;
                                tLink.RowNumber  = sActivity.RowNumber;
                                dtoLiteMapActivity pActivity = savingActivities.Where(s => s.IdActivity == idPredecessor).FirstOrDefault();
                                tLink.Name = (pActivity == null) ? "" : (String.IsNullOrEmpty(pActivity.Current.Name) ? pActivity.Previous.Name : pActivity.Current.Name);
                            }
                            tLink.InCycles = GenerateCycleForActivity(idActivity,currentLinks, idPredecessor, activity.Project.Id, savingActivities);
                            results.Add(tLink);
                        }
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    results = null;
                }
                return results;
            }
            public Boolean SetPredecessorVirtualDelete(long idActivity,long idLink, Boolean delete)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    PmActivityLink link = Manager.Get<PmActivityLink>(idLink);
                    if (person != null && link != null && link.Source !=null)
                    {
                        link.Deleted = delete ? (link.Deleted | BaseStatusDeleted.Cascade) : (link.Deleted = (BaseStatusDeleted)((int)link.Deleted - (int)BaseStatusDeleted.Cascade));
                        link.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        if (link.Source.Project != null)
                        {
                            InternalRecalculateProjectMap(link.Source.Project, person);
                            ClearCacheItems(link.Source.Project);
                        }
                    }
                    Manager.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return result;
            }
            public Boolean SaveActivityPredecessors(long idActivity, List<dtoActivityLink> links)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    PmActivity activity = Manager.Get<PmActivity>(idActivity);
                    Boolean recalc = false;
                    if (person != null  && activity != null && activity.Project != null && activity.PredecessorLinks != null && (activity.Children == null || !activity.IsSummary))
                    {
                        foreach (dtoActivityLink l in links) {
                            PmActivityLink predecessor = activity.PredecessorLinks.Where(p => p.Id == l.Id && (p.LeadLag != l.LeadLag || p.Type != l.Type)).FirstOrDefault();
                            if (predecessor != null) {
                                recalc = true;
                                predecessor.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                predecessor.Type = l.Type;
                                predecessor.LeadLag = l.LeadLag;
                            }
                        }
                        if (recalc)
                            InternalRecalculateProjectMap(activity.Project, person);
                    }
                    Manager.Commit();
                    if (recalc)
                        ClearCacheItems(activity.Project);
                    result = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return result;
            }
        #endregion

        #region "Recalculate Methods"
            #region "FullEntity"
                /// <summary>
                /// Ricalcola la mappa di progetto
                /// </summary>
                /// <param name="project">Il progetto su cui effettuare il ricalcolo</param>
                /// <param name="pUpdater">L'utente che esegue il ricalcolo</param>
                private Boolean InternalRecalculateProjectMap(Project project, litePerson pUpdater)
                {
                    Boolean result = false;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        if (pUpdater == null)
                        {
                            pUpdater = Manager.GetLitePerson(UC.CurrentUserID);
                            pUpdater = (pUpdater != null && pUpdater.TypeID != (int)UserTypeStandard.Guest && pUpdater.TypeID != (int)UserTypeStandard.PublicUser) ? pUpdater : null;
                        }
                        if (pUpdater != null && project != null)
                        {
                            List<PmActivity> lActivities = project.Activities.Where(a => a.Deleted == BaseStatusDeleted.None).ToList();
                            if (lActivities != null && lActivities.Any())
                            {
                                CalculateProjectCompleteness(project, lActivities);
                                long displayOrder = 1;
                                ReorderProjectTasks(ref displayOrder, lActivities.Where(a => a.Depth == 0).ToList());
                                Manager.SaveOrUpdateList<PmActivity>(lActivities);
                                // Manager.Refresh<Project>(project);
                                if (project.DateCalculationByCpm)
                                {
                                    //lActivities = (from a in Manager.GetAll<litePmActivity>(a => a.Deleted == BaseStatusDeleted.None && a.IdProject == project.Id)
                                    //               select a).ToList();
                                    CPMresult cpmResult = DefaultCPM(project.AllowSummary, lActivities, project.StartDate, project.DaysOfWeek, null, project.DefaultWorkingDay.HoursRange);

                                    UpdateTasksStartEndDate(lActivities, cpmResult.Activities);//, pUpdater);
                                    project.EndDate = cpmResult.ProjectEndDate;
                                    project.Duration = cpmResult.ProjectLength;
                                    if (project.Status == ProjectItemStatus.completed && project.Duration < 100)
                                        project.Status = ProjectItemStatus.started;
                                    project.UpdateMetaInfo(pUpdater, UC.IpAddress, UC.ProxyIpAddress);
                                }
                                else
                                {
                                    //foreach (PmActivity activity in activities)
                                    //{
                                    //    activity.EarlyStartDate = DateTime.Now.Date;
                                    //    activity.EarlyFinishDate = activity.EarlyStartDate.AddDays(1);
                                    //}
                                }
                                project.LastMapUpdate = DateTime.Now;
                                foreach (ProjectResource resource in project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None))
                                {
                                    UpdateResourceStatus(resource);
                                }
                            }
                            else
                                project.LastMapUpdate = DateTime.Now;
                        }
                        else
                            result = (project == null && pUpdater != null);
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        else
                            throw ex;
                        result = false;
                    }
                    return result;
                }

                public Boolean RecalculateProjectCompleteness(Project project, litePerson pUpdater)
                {
                    Boolean result = false;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        if (pUpdater == null)
                        {
                            pUpdater = Manager.GetLitePerson(UC.CurrentUserID);
                            pUpdater = (pUpdater != null && pUpdater.TypeID != (int)UserTypeStandard.Guest && pUpdater.TypeID != (int)UserTypeStandard.PublicUser) ? pUpdater : null;
                        }
                        if (pUpdater != null && project != null)
                        {
                            if (project.Activities != null)
                            {
                                CalculateProjectCompleteness(project, project.Activities.Where(a => a.Deleted== BaseStatusDeleted.None).ToList());
                                ClearCacheItems(project);
                            }
                            Manager.SaveOrUpdate(project);
                        }
                        else
                            result = (project == null && pUpdater != null);
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        else
                            throw ex;
                        result = false;
                    }
                    return result;
                }
                private void CalculateProjectCompleteness(Project project, List<PmActivity> tasks)
                {
                    long depth = tasks.Select(a => a.Depth).Max();

                    foreach (PmActivity task in tasks.Where(a => a.Deleted == BaseStatusDeleted.None && !a.IsSummary)) { 
                        CalculateTaskCompleteness(task);
                        if (!project.ConfirmCompletion && task.Completeness == 100 && !task.IsCompleted)
                            task.IsCompleted = true;
                    }
                    for (long i = depth; i >= 0; i--)
                    {
                        foreach (PmActivity task in tasks.Where(a => a.Deleted == BaseStatusDeleted.None && a.IsSummary && a.Depth == i))
                        {
                            CalculateTaskCompleteness(task);
                            if (task.Completeness == 100 && !task.IsCompleted && !task.Children.Where(c => c.Deleted == BaseStatusDeleted.None && !c.IsCompleted).Any())
                                task.IsCompleted = true;
                        }
                    }
                    CalculateProjectStatusAndCompletenessByChildren(project);
                }
                private void CalculateTaskStatusAndCompleteness(PmActivity task, Boolean autoConfirm = false )
                {
                    CalculateTaskCompleteness(task);
                    CalculateTaskStatus(task, autoConfirm);
                    if (task.Parent != null) {
                        CalculateTaskStatusAndCompleteness(task.Parent, autoConfirm);
                    }
                    else if (task.Depth == 0) {
                        CalculateProjectStatusAndCompletenessByChildren(task.Project);
                       
                    }

                }
                private void CalculateProjectStatusAndCompletenessByChildren(Project project)
                {
                    Double aDuration = project.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.ActualDuration).Sum();
                    Double duration = project.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.Duration).Sum();
                    if (duration>0)
                        project.Completeness = (int)(Math.Round(((aDuration / duration) * 100), 0));
                    if (project.Completeness == 100 && project.Children.Where(c => c.Deleted == BaseStatusDeleted.None && c.Duration == 0 && c.Completeness < 100).Any())
                        project.Completeness = project.Completeness - 1;
                    project.IsCompleted = (project.Completeness == 100) && !project.Children.Where(a => a.Deleted == BaseStatusDeleted.None && a.Depth == 0 && !a.IsCompleted).Any();

                    if (project.Status == ProjectItemStatus.notstarted && project.Completeness > 0 && !project.IsCompleted)
                        project.Status = ProjectItemStatus.started;
                    else if (project.Status == ProjectItemStatus.started && project.Completeness ==100 && project.IsCompleted)
                        project.Status = ProjectItemStatus.completed;
                }
                private void CalculateTaskCompleteness(PmActivity task)
                {
                    if (task.IsSummary) {
                        if (task.Children != null)
                        {
                            double duration = task.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.Duration).Sum();
                            task.ActualDuration = task.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.ActualDuration).Sum();
                            if (duration>0)
                                task.Completeness = (int)(Math.Round(((task.ActualDuration / duration) * 100), 0));
                            if (task.Completeness == 100 && task.Children.Where(c => c.Deleted == BaseStatusDeleted.None && c.Duration == 0 && c.Completeness < 100).Any())
                                task.Completeness = task.Completeness - 1;
                        }
                    }
                    else
                    {
                        if (task.CurrentAssignments.Any())
                            task.Completeness = (int)task.CurrentAssignments.Select(a => a.Completeness).Average();
                        if (task.Duration>0)
                            task.ActualDuration = Math.Round(((task.Duration * task.Completeness) / 100), 0);
                    }
                }
                private void CalculateTaskStatus(PmActivity task, Boolean autoConfirm = false)
                {
                    if (task.IsSummary)
                    {
                        if (task.Children != null && task.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        {
                            if (task.Completeness == 100 && (autoConfirm || !task.Project.ConfirmCompletion))
                            {
                                task.Status = (task.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.completed : task.Status;
                                task.IsCompleted = true;
                            }
                            else if (task.Completeness > 0)
                                task.Status = (task.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.started : task.Status;
                            else
                                task.Status = (task.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.notstarted : task.Status;
                        }
                    }
                    else
                    {
                        if (!task.CurrentAssignments.Any())
                        {
                            if (task.Status != ProjectItemStatus.suspended)
                                task.Status = (task.Completeness == 100 && task.IsCompleted) ? ProjectItemStatus.completed : (task.Completeness > 0 ? ProjectItemStatus.started : ProjectItemStatus.notstarted);
                        }
                        else
                        {
                            if (task.Completeness == 100 && (autoConfirm || !task.Project.ConfirmCompletion))
                            {
                                task.Status = (task.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.completed : task.Status;
                                task.IsCompleted = true;
                            }
                            else if (task.Completeness > 0)
                                task.Status = (task.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.started : task.Status;
                            else
                                task.Status = (task.Status != ProjectItemStatus.suspended) ? ProjectItemStatus.notstarted : task.Status;
                        }
                    }
                }
                private void ReorderProjectTasks(ref long displayOrder, IEnumerable<PmActivity> lActivities)
                {
                    long wbsIndex = 1;
                    foreach (PmActivity activity in lActivities.OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id))
                    {
                        activity.DisplayOrder = displayOrder++;
                        activity.WBSindex = wbsIndex++;
                        if (activity.Parent == null)
                            activity.WBSstring = activity.WBSindex.ToString() + ".";
                        else
                            activity.WBSstring = activity.Parent.WBSstring + activity.WBSindex.ToString() + ".";
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                            ReorderProjectTasks(ref displayOrder, activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None));
                    }
                }
                private void UpdateTasksStartEndDate(IList<PmActivity> activities, IList<dtoCPMactivity> lActivities)
                {
                    foreach (PmActivity activity in activities)
                    {
                        dtoCPMactivity lite = lActivities.Where(a => a.Id == activity.Id).FirstOrDefault();
                        if (lite != null)
                        {
                            activity.Duration = lite.Duration;
                            activity.EarlyFinish = lite.EarlyFinish;
                            activity.EarlyFinishDate = lite.EarlyFinishDate;
                            activity.EarlyStart = lite.EarlyStart;
                            activity.EarlyStartDate = lite.EarlyStartDate;
                            activity.LatestStart = lite.LatestStart;
                            activity.LatestFinish = lite.LatestFinish;
                            activity.LatestStartDate = lite.LatestStartDate;
                            activity.LatestFinishDate = lite.LatestFinishDate;
                            if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                                UpdateTasksStartEndDate(activity.Children, lActivities);
                        }
                    }
                }
            #endregion
   
        #endregion
                
        #region "Standard WBS"
            #region "Lite WBS"
                private long GetNewWBSindex(long idParent)
                {
                    long index = 0;
                    try
                    {
                        index = (from a in Manager.GetIQ<liteDisplayActivity>() where a.Deleted == BaseStatusDeleted.None && a.IdParent == idParent select a.WBSindex).Max();

                        index++;
                    }
                    catch (Exception)
                    {
                        index = 0;
                    }
                    return index;
                }
                private void RecalculateWBS(List<litePmActivity> lActivities)
                {
                    long index = 1;
                    foreach (litePmActivity activity in lActivities.OrderBy(a => a.DisplayOrder).ToList())
                    {
                        activity.WBSindex = index++;
                        if (activity.Parent == null)
                            activity.WBSstring = activity.WBSindex.ToString() + ".";
                        else
                            activity.WBSstring = activity.Parent.WBSstring + activity.WBSindex.ToString() + ".";
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                            RecalculateWBS(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).ToList());
                    }
                }
            #endregion
                private void RecalculateWBS(List<PmActivity> lActivities, litePerson p)
                {
                    long index = 1;
                    foreach (PmActivity activity in lActivities.OrderBy(a => a.DisplayOrder).ToList())
                    {
                        activity.WBSindex = index++;
                        if (activity.Parent == null)
                            activity.WBSstring = activity.WBSindex.ToString() + ".";
                        else
                            activity.WBSstring = activity.Parent.WBSstring + activity.WBSindex.ToString() + ".";
                        if (activity.Children != null && activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                            RecalculateWBS(activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).ToList(),p);
                        activity.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    }
                }
        #endregion

        #region "Reorder Activities"
            /// <summary>
            ///     Get Project Activities for map reorder into tree structure
            /// </summary>
            /// <param name="idProject"></param>
            /// <returns>level 0 items with children</returns>
            public List<dtoActivityTreeItem> GetProjectTreeForReorder(long idProject, List<dtoReorderGraphActivity> activities = null)
            {
                List<dtoActivityTreeItem> items = new List<dtoActivityTreeItem>();
                try
                {
                    items = GetProjectItemsForReorder(idProject);
                    foreach (dtoActivityTreeItem item in items.Where(i => i.HasLinks))
                    {
                        item.Predecessors = CPMExtensions.TreeItemLinksToString(item.Links, items);
                    }
                    if (activities != null) {
                        foreach (dtoReorderGraphActivity dto in activities.Where(a => items.Where(i => i.Id == a.IdActivity && (i.IdParent != a.IdParent || i.DisplayOrder != a.DisplayOrder || i.Depth != a.Depth)).Any()))
                        {
                            dtoActivityTreeItem tItem = items.Where(i => i.Id == dto.IdActivity).FirstOrDefault();
                            tItem.IdParent = dto.IdParent;
                            tItem.DisplayOrder = dto.DisplayOrder;
                            tItem.Depth = dto.Depth;
                            tItem.Status = dto.Status;
                        }
                    }
                    return GenerateProjectTreeForReorder(items);
                }
                catch (Exception ex)
                {
                    items = new List<dtoActivityTreeItem>();
                }
                return items;
            }

            /// <summary>
            /// Get project plain list of activities for reordering
            /// </summary>
            /// <param name="idProject"></param>
            /// <returns></returns>
            private List<dtoActivityTreeItem> GetProjectItemsForReorder(long idProject)
            {
                List<dtoActivityTreeItem> items = new List<dtoActivityTreeItem>();
                try
                {
                    List<litePmActivity> activities = (from a in Manager.GetIQ<litePmActivity>()
                                                       where a.IdProject == idProject && a.Deleted == BaseStatusDeleted.None && a.Depth == 0 
                                                       orderby a.DisplayOrder, a.Id
                                                       select a).ToList().Where(a=> a.Parent == null).ToList();
                    long rNumber = 1;
                    items = GetProjectItemsForReorder(activities, ref rNumber);
                }
                catch (Exception ex)
                {

                }
                return items;
            }
            /// <summary>
            /// Get activiti plain list of sub-activities for reordering
            /// </summary>
            /// <param name="activities">direct children list</param>
            /// <param name="rNumber"></param>
            /// <returns></returns>
            private List<dtoActivityTreeItem> GetProjectItemsForReorder(List<litePmActivity> activities, ref long rNumber)
            {
                List<dtoActivityTreeItem> results = new List<dtoActivityTreeItem>();
                foreach (litePmActivity task in activities)
                {
                    dtoActivityTreeItem item = new dtoActivityTreeItem(task, rNumber++);
                    results.Add(item);
                    if (task.Children != null && task.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Any())
                        results.AddRange(GetProjectItemsForReorder(task.Children.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToList(), ref rNumber));
                }
                return results;
            }
            /// <summary>
            /// Geenrate Project activities tree from plain activities
            /// </summary>
            /// <param name="activities"></param>
            /// <returns></returns>
            public List<dtoActivityTreeItem> GenerateProjectTreeForReorder(List<dtoActivityTreeItem> activities)
            {
                long rNumber = 1;
                return GenerateProjectTreeForReorder(null,activities.Where(a => a.IdParent == 0), activities, ref rNumber);
            }
            private List<dtoActivityTreeItem> GenerateProjectTreeForReorder(dtoActivityTreeItem parent, IEnumerable<dtoActivityTreeItem> activities, List<dtoActivityTreeItem> allActivities, ref long rNumber)
            {
                long wbsIndex = 1;
                List<dtoActivityTreeItem> results = new List<dtoActivityTreeItem>();
                foreach (dtoActivityTreeItem act in activities.OrderBy(a=> a.DisplayOrder).ThenBy(a=> a.Id))
                {
                    act.DisplayOrder = rNumber++;
                    act.RowNumber = act.DisplayOrder;
                    act.WBSindex= wbsIndex++;
                    if (parent==null)
                        act.WBSstring = act.WBSindex.ToString() + ".";
                    else
                        act.WBSstring = parent.WBSstring + act.WBSindex.ToString() + ".";
                    if (allActivities.Where(a=> a.IdParent== act.Id).Any())
                        act.Children= GenerateProjectTreeForReorder(act, allActivities.Where(a=> a.IdParent== act.Id), allActivities, ref rNumber);
                    results.Add(act);
                }
                return results;
            }

            public ReorderError AllowReorder(List<dtoReorderGraphActivity> activities)
            {
                ReorderError result =  ReorderError.None;
                try
                {
                    if (activities.Where(a=> a.Status== FieldStatus.removed && a.IsSummary).Any())
                        return ReorderError.ProjectMapChanged;
                    else if (activities.Where(a => a.IsSummary && activities.Where(i=> i.Links.Where(l=> l.IdPredecessor== a.IdActivity).Any()).Any()).Any())
                        return ReorderError.SummaryWithLinks;
                    else if (CPMExtensions.HasValidDependencies(activities))
                        result = (activities.Where(a=> a.Status!= FieldStatus.none).Any()) ? ReorderError.ProjectMapChanged : ReorderError.None;
                    else
                        result = ReorderError.InConflictPredecessorsFound;
                }
                catch (Exception ex) {
                    result =  ReorderError.DataAccess;
                }
                return result;
            }

            public List<dtoReorderGraphActivity> AnalyzeActionsForReorder(long idProject, List<dtoReorderGraphActivity> activities)
            {
                var query = (from a in Manager.GetIQ<litePmActivity>()
                                where a.Deleted == BaseStatusDeleted.None && a.IdProject == idProject
                                select new { Id = a.Id, IdParent = (a.Parent == null) ? 0 : a.Parent.Id, DisplayOrder = a.DisplayOrder, Depth = a.Depth }).ToList();

                List<long> idCurrentActivities = activities.Select(a => a.IdActivity).ToList();
                activities.AddRange(query.Where(q => !idCurrentActivities.Contains(q.Id)).Select(q => new dtoReorderGraphActivity() { IdActivity = q.Id, IdParent = q.IdParent, DisplayOrder= q.DisplayOrder, Depth= q.Depth, Status= FieldStatus.updated }).ToList());
                List<liteReorderActivityLink> links = (from l in Manager.GetIQ<liteReorderActivityLink>() where l.IdProject == idProject && l.Deleted == BaseStatusDeleted.None select l).ToList();
                foreach (var link in links.GroupBy(l => l.IdSource))
                {
                    dtoReorderGraphActivity activity = activities.Where(a => a.IdActivity == link.Key).FirstOrDefault();
                    if (activity != null)
                        activity.Links = link.ToList().Select(l => new dtoGraphActivityLink() { IdPredecessor = l.IdTarget, LeadLag = l.LeadLag, Type = l.Type }).ToList();
                }
                foreach (dtoReorderGraphActivity activity in activities.Where(a => !query.Where(i => i.Id == a.IdActivity).Any())) {
                    activity.Status = FieldStatus.removed;
                }
                AnalyzeActionsForReorder(activities);
                return activities;
            }
            private void AnalyzeActionsForReorder(List<dtoReorderGraphActivity> activities)
            {
                long displayOrder = 1;
                foreach (dtoReorderGraphActivity activity in activities.Where(a => a.IdParent == 0).OrderBy(a => a.DisplayOrder))
                {
                    activity.DisplayOrder= displayOrder++;
                    activity.RowNumber = activity.DisplayOrder;
                    activity.Depth = 0;
                    activity.IsSummary = activities.Where(a => a.IdParent == activity.IdActivity).Any();
                    if (activity.IsSummary)
                        AnalyzeActionsForReorder(activity.IdActivity, 1, activities.Where(a => a.IdParent > 0), ref displayOrder);
                }
            }
            private void AnalyzeActionsForReorder(long idParent, long depth, IEnumerable<dtoReorderGraphActivity> activities, ref long displayOrder)
            {
                foreach (dtoReorderGraphActivity activity in activities.Where(a => a.IdParent == idParent).OrderBy(a => a.DisplayOrder))
                {
                    activity.DisplayOrder= displayOrder++;
                    activity.RowNumber = activity.DisplayOrder;
                    activity.Depth = depth;
                    activity.IsSummary = activities.Where(a => a.IdParent == activity.IdActivity).Any();
                    if (activity.IsSummary)
                        AnalyzeActionsForReorder(activity.IdActivity, depth + 1, activities.Where(a => a.IdParent > 0 && a.IdActivity != activity.IdActivity), ref displayOrder);
                }
            }
            public ReorderError ReorderActivities(long idProject, List<dtoReorderGraphActivity> activities, ReorderAction dAction, Boolean applyLinksRemoving = false )
            {
                ReorderError result = ReorderError.None;
                DateTime? lastUpdate = null;
                DateTime? updatedOn = null;
                try
                {
                    if (dAction != ReorderAction.RemoveAllPredecessors && dAction != ReorderAction.RemoveConflictPredecessors && activities.Where(a => a.IsSummary && activities.Where(i => i.Links.Where(l => l.IdPredecessor == a.IdActivity).Any()).Any()).Any())
                        return ReorderError.SummaryWithLinks;
                    else if (CPMExtensions.HasValidDependencies(activities) || (applyLinksRemoving && (dAction== ReorderAction.RemoveAllPredecessors || dAction== ReorderAction.RemoveConflictPredecessors)))
                    {
                        Manager.BeginTransaction();
                        Project project = Manager.Get<Project>(idProject);
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (project != null && p != null && p.TypeID != (int)UserTypeStandard.Guest && p.TypeID != (int)UserTypeStandard.PublicUser)
                        {
                            List<long> idActivities = activities.Select(a => a.IdActivity).ToList();
                            foreach (dtoReorderGraphActivity item in activities)
                            {
                                PmActivity activity = Manager.Get<PmActivity>(item.IdActivity);
                                if (activity != null)
                                {
                                    activity.DisplayOrder = item.DisplayOrder;
                                    activity.Depth = item.Depth;
                                    if (item.IdParent == 0 && activity.Parent != null)
                                    {
                                        activity.Parent.Children.Remove(activity);
                                        activity.Parent = null;
                                    }
                                    else if (item.IdParent > 0 && activity.Parent != null && activity.Parent.Id != item.IdParent)
                                    {
                                        activity.Parent.Children.Remove(activity);
                                        activity.Parent = Manager.Get<PmActivity>(item.IdParent);
                                        activity.Parent.Children.Add(activity);

                                        if (applyLinksRemoving)
                                        {
                                            foreach (PmActivityLink predecessor in activity.PredecessorLinks.Where(pr => pr.Deleted == BaseStatusDeleted.None && pr.Target.IsSummary))
                                            {
                                                predecessor.Deleted = (predecessor.Deleted | BaseStatusDeleted.Automatic);
                                                predecessor.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                            foreach (PmActivityLink successor in activity.SuccessorLinks.Where(pr => pr.Deleted == BaseStatusDeleted.None && pr.Target.IsSummary))
                                            {
                                                successor.Deleted = (successor.Deleted | BaseStatusDeleted.Automatic);
                                                successor.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                        }
                                    }
                                    else if (item.IdParent > 0 && activity.Parent == null)
                                    {
                                        activity.Parent = Manager.Get<PmActivity>(item.IdParent);
                                        activity.Parent.Children.Add(activity);

                                        if (applyLinksRemoving)
                                        {
                                            foreach (PmActivityLink predecessor in activity.PredecessorLinks.Where(pr => pr.Deleted == BaseStatusDeleted.None && pr.Target.IsSummary))
                                            {
                                                predecessor.Deleted = (predecessor.Deleted | BaseStatusDeleted.Automatic);
                                                predecessor.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                            foreach (PmActivityLink successor in activity.SuccessorLinks.Where(pr => pr.Deleted == BaseStatusDeleted.None && pr.Target.IsSummary))
                                            {
                                                successor.Deleted = (successor.Deleted | BaseStatusDeleted.Automatic);
                                                successor.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                        }
                                    }

                                    if (item.IsSummary && !activity.IsSummary)
                                    {
                                        foreach (PmActivityLink predecessor in activity.PredecessorLinks)
                                        {
                                            predecessor.Deleted = (predecessor.Deleted | BaseStatusDeleted.Automatic);
                                            predecessor.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                        foreach (PmActivityLink successor in activity.SuccessorLinks)
                                        {
                                            successor.Deleted = (successor.Deleted | BaseStatusDeleted.Automatic);
                                            successor.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                        List<ProjectActivityAssignment> assignments = (from a in Manager.GetIQ<ProjectActivityAssignment>()
                                                                                       where a.Deleted == BaseStatusDeleted.None && a.Activity != null && a.Activity.Id == activity.Id
                                                                                       select a).ToList();
                                        foreach (ProjectActivityAssignment assignment in assignments)
                                        {
                                            assignment.Deleted = (assignment.Deleted | BaseStatusDeleted.Automatic);
                                            assignment.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                    }
                                    activity.IsSummary = item.IsSummary;
                                }
                            }
                            if (applyLinksRemoving){
                                switch (dAction) { 
                                    case ReorderAction.RemoveAllPredecessors:
                                        DateTime dDate = DateTime.Now;
                                        foreach (PmActivityLink link in (from l in Manager.GetIQ<PmActivityLink>() where l.Deleted == BaseStatusDeleted.None && l.Project.Id == idProject select l))
                                        {
                                            link.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                            link.ModifiedOn = dDate;
                                        }
                                        break;
                                    case ReorderAction.RemoveConflictPredecessors:
                                        IList<DirectedGraphCycle> cycles = GetCycles(GetGraphActivities(project));
                                        if(cycles.Any()){
                                            
                                        }
                                        break;
                                }
                            }
                            lastUpdate = project.LastMapUpdate;
                            InternalRecalculateProjectMap(project, p);
                            Manager.Commit();
                            updatedOn = project.LastMapUpdate;
                            result = ReorderError.None;
                            ClearCacheItems(project);
                        }
                    }
                    else
                        result = ReorderError.InConflictPredecessorsFound;
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                    {
                        if (lastUpdate!= updatedOn)
                            result = ReorderError.InConflictPredecessorsFound;
                        else
                            result = ReorderError.DataAccess;
                        Manager.RollBack();
                    }
                }
                return result;
            }
        #endregion
    }
}