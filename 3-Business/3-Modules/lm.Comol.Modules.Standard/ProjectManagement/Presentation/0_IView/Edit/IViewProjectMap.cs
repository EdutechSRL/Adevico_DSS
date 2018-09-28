using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectMap : IViewBaseProjectMap
    {
        #region "Preload"
        #endregion

        #region "Context"
            //DateTime? PRstartDate { get; set; }
            //DateTime? PRendDate { get; set; }
            //DateTime? PRdeadline { get; set; }
            DateTime? InEditStartDate { get; }
            DateTime? InEditDeadline { get; }
            Boolean AllowAddExternalUser { get; set; }
            Boolean AllowAddCommunityUser { get; set; }
            Boolean AllowManageResources { get; set; }
            Boolean AllowSave { get; set; }
            Boolean AllowAddActivity { get; set; }
            Boolean AllowMilestones { get; set; }
            Boolean AllowEstimatedDuration { get; set; }
         
            List<dtoResource> ProjectResources { get; set; }
            String GetDefaultActivityName();
            dtoWorkingDay DefaultWorkingDay { get; set; }
        #endregion

        #region "ViewMethods"
         
            List<dtoLiteMapActivity> GetActivities();
            //void SetInEditItems();
            void DisplayErrorGetActivitiesFromDB();
            void LoadActivities(List<dtoMapActivity> activities);
            void LoadActivities(List<dtoMapActivity> activities, dtoField<DateTime?> startDate, dtoField<DateTime?> endDate, dtoField<DateTime?> deadLine);
            void DisplaySavedActivities(Boolean verifyLinks = false, Boolean fatherLinks = false, Boolean summaryLinks = false);
            void DisplayActivityMoved(Boolean verifyLinks = false, Boolean fatherLinks = false, Boolean summaryLinks = false);
            void DisplayActivitiesAdded();
            void DisplayActivityRemoved(String name, long children);
            //void DisplayUnableToEditActivity();
            void DisplayUnableToAddActivities();
            void DisplayUnableToRemoveActivity(String name, long children);
            void DisplayUnableToMoveActivity();
            void DisplaySavingActivitiesErrors(long toUpdate, long updated, long alreadyModified);
            void DisplaySavedActivityResources(String name, long count);
            void DisplayUnableToSaveActivityResources(String name);
            void ReloadCompletion(Dictionary<long, dtoCompletion> items);
            void InitializeActivityControl(long idActivity);
            void DisplayRemovedActivity();
            void DisplayActivitySaved(dtoMapActivity activity, dtoField<DateTime?> startDate, dtoField<DateTime?> endDate, dtoField<DateTime?> deadLine);
        #endregion

        #region "ResourcesMethods"
            void InitializeControlToAddUsers(Int32 idCommunity, List<Int32> hideUsers);
            void InitializeControlToAddExternalResource();


            List<dtoProjectResource> GetResources();
            void LoadResources(List<dtoProjectResource> resources);
            void DisplayResourceAdded(Int32 added, Int32 multipleLongName = 0, Int32 multipleShortName = 0);
            void DisplaySavedSettings(Int32 multipleLongName = 0, Int32 multipleShortName = 0);
            void DisplayRemovedResource(String name);
            void DisplayUnableToRemoveResource(String name);
            void DisplayUnableToRemoveResource(long idProject, long idResource, String name, long assignedTasks, long completedTasks);
            void DisplayErrors(Int32 multipleLongName, Int32 multipleShortName);
            void DisplayUnableToAddExternalResource();
            void DisplayUnableToAddInternalResource();
            void DisplayUnableToAddInternalResources();
            void DisplayUnableToSaveResources();
            void DisplayNoResources();
            void ReloadResources(Dictionary<long, List<dtoResource>> resources, Dictionary<long, dtoCompletion> items = null);
            void InitializeControlForResourcesSelection(List<dtoProjectResource> resources);
        #endregion
    }
}