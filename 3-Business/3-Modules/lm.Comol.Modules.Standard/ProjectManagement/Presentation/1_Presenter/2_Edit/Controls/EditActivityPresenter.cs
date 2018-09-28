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
    public class EditActivityPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
           #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditActivity View
            {
                get { return (IViewEditActivity)base.View; }
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
            public EditActivityPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditActivityPresenter(iApplicationContext oContext, IViewEditActivity view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion


        public void InitView(long idActivity, String unknownUser, List<dtoLiteMapActivity> activities)
        {
            View.IdActivity = idActivity; 
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Activities = activities;
                litePmActivity activity = Service.GetLiteActivity(idActivity);
                liteProjectSettings project = Service.GetProjectSettings((activity!=null) ? activity.IdProject :0);
                if (activity == null || project ==null)
                    View.DisplayUnknownActivity();
                else
                {
                    View.IdProject = activity.IdProject;
                    dtoActivity dto = GetActivity(project, activity, activities,true);
                    View.isCompleted = dto.IsCompleted;
                    View.LoadSettings(dto, project.AllowEstimatedDuration, project.AllowMilestones);
                    List<dtoResource> resources = project.Resources.Select(r => new dtoResource(r, unknownUser)).ToList();
                    View.LoadResources(dto.Permission.SetResources, resources, dto.IdResources);
                    LoadAssignments(project,dto, resources);
                }
            }
        }

        #region "Links"
            private void LoadLinks( dtoActivity dto, List<dtoLiteMapActivity> activities, Boolean firstLoad =false) {
                foreach (dtoActivityLink l in dto.Links) {
                    dtoLiteMapActivity act = activities.Where(a => a.IdActivity == l.IdTarget).FirstOrDefault();
                    if (act != null) {
                        l.Name =  (String.IsNullOrEmpty(act.Current.Name) ? act.Previous.Name : act.Current.Name);
                        l.RowNumber = act.RowNumber;
                        l.AllowDelete = dto.Permission.SetPredecessors;
                    }
                    else
                        l.Id=0;
                }
                List<long> idPredecessors = Service.GetAvailableIdPredecessors(dto,activities).Where(id=> !dto.Links.Where(l=> l.IdTarget== id).Any()).ToList();
                if (firstLoad)
                    View.LoaderLinks = dto.Links;
                View.LoadAvailableLinks(activities.Where(a => idPredecessors.Contains(a.IdActivity)).ToDictionary(a => a.IdActivity, act => act.RowNumber + " - " + (String.IsNullOrEmpty(act.Current.Name) ? act.Previous.Name : act.Current.Name)), dto.Links.Where(l => l.Id > 0).ToList());
            }
            public void VirtualDeleteLink(long idProject, long idActivity, String uniqueId, List<dtoLiteMapActivity> activities)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<dtoActivityLink> links = View.LoaderLinks;
                    dtoActivityLink link = links.Where(l => l.UniqueId == uniqueId).FirstOrDefault();
                    if (link != null && link.Id > 0)
                        link.Deleted = true;
                    else if (link != null)
                        links = links.Where(l => l.UniqueId != uniqueId).ToList();
                    View.LoaderLinks = links;
                    ReloadPredecessorsData(idProject, idActivity,links, activities);
                }
            }
            public void AddLink(long idProject, long idActivity, List<long> idPredecessors,List<dtoActivityLink> links, List<dtoLiteMapActivity> activities)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    if (idPredecessors.Any())
                    {
                        List<dtoActivityLink> addedLinks = Service.AddTemporaryPredecessors(idActivity, idPredecessors, links, activities);
                        if (addedLinks != null)
                        {
                            links.AddRange(addedLinks.Where(a => a.Id == 0 && !a.InCycles).ToList());
                            if (addedLinks.Where(l => l.InCycles).Any())
                                View.DisplayLinksInCycles(addedLinks.Where(l => l.InCycles).ToList());
                            ReloadPredecessorsData(idProject, idActivity, links, activities);
                        }
                        else
                            View.DisplayUnableToAddLink();
                    }
                }
            }
        //public void VirtualDeleteLink(long idProject,long idActivity, long idLink, List<dtoLiteMapActivity> activities)
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else {
        //        if (Service.SetPredecessorVirtualDelete(idActivity, idLink, true))
        //        {
        //            View.DisplayLinkRemoved();
        //            ReloadData(idProject, idActivity,activities, true);
        //        }
        //        else
        //            View.DisplayUnableToRemoveLink();
        //    }
        //}
        //public void AddLink(long idProject, long idActivity, List<long> idPredecessors, List<dtoLiteMapActivity> activities)
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else
        //    {
        //        if (idPredecessors.Any()){
        //            List<PmActivityLink> links = Service.AddPredecessors(idActivity, idPredecessors, activities);
        //            if (links !=null )
        //            {
        //                View.DisplayLinkRemoved();
        //                ReloadData(idProject, idActivity, activities,true);
        //            }
        //            else
        //                View.DisplayUnableToAddLink();
        //        }
        //    }
        //}

        //public void SaveLinks(long idProject, long idActivity, List<dtoActivityLink> links, List<dtoLiteMapActivity> activities)
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else
        //    {
        //        if (links.Any())
        //        {
        //            if (Service.SaveActivityPredecessors(idActivity, links))
        //            {
        //                View.DisplayLinksSaved();
        //                ReloadData(idProject, idActivity, activities, true);
        //            }
        //            else
        //                View.DisplayLinksUnsaved();
        //        }
        //    }
        //}

        private void ReloadPredecessorsData(long idProject, long idActivity, List<dtoActivityLink> links,List<dtoLiteMapActivity> activities)
        {
            litePmActivity activity = Service.GetLiteActivity(idActivity);
            liteProjectSettings project = Service.GetProjectSettings((activity != null) ? activity.IdProject : 0);
            if (activity != null && project != null) {
                dtoActivity dto = GetActivity(project, activity, activities);
                dto.Links = links.Where(l => !l.Deleted && !l.InCycles ).ToList();
                List<long> idPredecessors = Service.GetAvailableIdPredecessors(dto, activities).Where(id => !dto.Links.Where(l => l.IdTarget == id).Any()).ToList();
                View.LoadAvailableLinks(activities.Where(a => idPredecessors.Contains(a.IdActivity)).ToDictionary(a => a.IdActivity, act => act.RowNumber + " - " + (String.IsNullOrEmpty(act.Current.Name) ? act.Previous.Name : act.Current.Name)), dto.Links.ToList());
            }            
        }
        #endregion

        #region "Assignments"
            private void LoadAssignments(liteProjectSettings project,dtoActivity dto,List<dtoResource> resources ) {
                if (dto.IsSummary)
                    View.LoadSummaryCompletion(dto.Completeness, dto.IsCompleted, dto.Permission);
                else
                {
                    List<dtoActivityCompletion> assignments = new List<dtoActivityCompletion>();
                    foreach (dtoResource resource in resources.OrderBy(r => r.LongName))
                    {
                        dtoActivityCompletion assignment = dto.Assignments.Where(a => a.IdResource == resource.IdResource).FirstOrDefault();
                        if (assignment == null)
                        {
                            assignment = new dtoActivityCompletion();
                            assignment.IdResource = resource.IdResource;
                            assignment.Completeness = (dto.IsCompleted) ? 100 :0;
                            assignment.IsApproved = (dto.IsCompleted) ? true : !project.ConfirmCompletion;
                            assignment.Deleted = true;
                        }
                        assignment.IdPerson = resource.IdPerson;
                        assignment.DisplayName = resource.LongName;
                        assignment.AllowEdit = dto.Permission.SetCompleteness || dto.Permission.SetOthersCompletion || (dto.Permission.SetMyCompletion && resource.IdPerson == UserContext.CurrentUserID);
                        assignments.Add(assignment);
                    }
                    View.LoaderAssignments = assignments;
                    View.LoadCompletion(dto.Completeness, dto.IsCompleted, assignments, dto.Permission);
                }
            }
            //public void VirtualDeleteAssignment(String uniqueId, List<dtoActivityCompletion> assignments)
            //{
            //    if (UserContext.isAnonymous)
            //        View.DisplaySessionTimeout();
            //    else
            //    {
            //        dtoActivityCompletion assignment = assignment.Where(l => l.UniqueId == uniqueId).FirstOrDefault();
            //        if (assignment != null)
            //        {
            //            assignment.Deleted = false;
            //            View.RemoveResourceFromSelection(assignment.IdResource);
            //        }
            //        View.LoaderAssignments = assignments;
            //    }
            //}
            public void ConfirmCompletion(List<long> isSelectedResources) {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<dtoActivityCompletion> assignments = View.LoaderAssignments;
                    foreach (dtoActivityCompletion assignment in assignments)
                    {
                        assignment.Completeness = 100;
                        assignment.AllowEdit = false;
                        assignment.IsApproved = true;
                        assignment.Deleted = isSelectedResources.Contains(assignment.IdResource);
                    }
                    View.LoaderAssignments = assignments;
                    View.ReloadCompletion(100,true,assignments);
                }
            
            }
        #endregion
        private void ReloadData(long idProject, long idActivity, List<dtoLiteMapActivity> activities, Boolean reloadSettings = false)
        {
            litePmActivity activity = Service.GetLiteActivity(idActivity);
            liteProjectSettings project = Service.GetProjectSettings((activity != null) ? activity.IdProject : 0);
            if (activity == null || project == null)
                View.DisplayUnknownActivity();
            else {
                dtoActivity dto = GetActivity(project, activity, activities);
                if (reloadSettings)
                    View.ReloadSettings(dto, project.AllowEstimatedDuration, project.AllowMilestones);
            }
        }

        private dtoActivity GetActivity(liteProjectSettings project, litePmActivity activity, List<dtoLiteMapActivity> activities, Boolean firstLoad = false)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID) : new ModuleProjectManagement(CurrentManager.GetModulePermission(UserContext.CurrentUserID, project.IdCommunity, CurrentIdModule));
            PmActivityPermission pPermissions = Service.GetProjectPermission(activity.IdProject, UserContext.CurrentUserID);

            dtoActivity dto = new dtoActivity(activity);
            Boolean allowEdit = (mPermission.Administration && !project.isPersonal) || pPermissions.HasFlag(PmActivityPermission.ManageProject);
            Boolean allowDelete = (mPermission.Administration && !project.isPersonal) || pPermissions.HasFlag(PmActivityPermission.ManageProject);

            dto.Permission = new dtoActivityPermission(pPermissions, allowEdit, allowDelete, project.AllowSummary, project.DateCalculationByCpm);
            dto.Permission.ViewPredecessors = dto.Permission.ViewPredecessors && !dto.IsSummary;
            dto.Permission.SetPredecessors = (allowEdit || pPermissions.HasFlag(PmActivityPermission.ManageLinks)) && project.DateCalculationByCpm && !dto.IsSummary;
            dto.Permission.SetDuration = dto.Permission.SetDuration && !dto.IsSummary;
            dto.Permission.SetResources = dto.Permission.SetResources && !dto.IsSummary;
            View.AllowDelete = allowDelete;
            View.AllowEdit = allowEdit;
            LoadLinks(dto, activities, firstLoad);

            return dto;
        }


        public void SaveActivity(long idProject, long idActivity, dtoActivity activity, List<long> idResources, List<dtoActivityLink> links, List<dtoActivityCompletion> assignments, Boolean isCompleted, DateTime? prStartDate = null, DateTime? prEndDate = null, DateTime? prDeadline = null)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                dtoField<DateTime?> startDate = new dtoField<DateTime?>() { Init = prStartDate };
                dtoField<DateTime?> endDate = new dtoField<DateTime?>() { Init = prEndDate };
                dtoField<DateTime?> deadLine = new dtoField<DateTime?>() { Init = prDeadline };

                ActivityEditingErrors errors = Service.SaveActivity(idProject, idActivity, activity, idResources, links, assignments, isCompleted, ref startDate, ref endDate, ref deadLine);
                if (errors== ActivityEditingErrors.None)
                {
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivitySaved);
                    View.DisplayActivitySaved(idActivity,startDate, endDate, deadLine);
                }
                else{
                    View.DisplayUnableToSaveActivity();
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityUnableToSave);
                    }
            }
        }
        public Boolean VirtualDeleteActivity(long idProject, long idActivity, List<dtoLiteMapActivity> activities)
        {
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                dtoLiteMapActivity activity = activities.Where(a => a.IdActivity == idActivity).FirstOrDefault();
                if (Service.SetActivityVirtualDelete(idActivity, true))
                {
                    result = true;
                    View.SendUserAction(View.ProjectIdCommunity, CurrentIdModule, idProject, ModuleProjectManagement.ActionType.ActivityVirtualDelete);
                }
                else
                {
                    if (activity!=null)
                        View.DisplayUnableToRemoveActivity((activity != null) ? activity.Current.Name : "", GetChildrenCount(idActivity, activities));
                }
             }
            return result;
        }

        private long GetChildrenCount(long idActivity, List<dtoLiteMapActivity> activities)
        {
            long children = 0;
            foreach (dtoLiteMapActivity child in activities.Where(a => a.IdParent == idActivity))
            {
                children += 1 + GetChildrenCount(child.IdActivity, activities);
            }
            return children;
        }
    }
}