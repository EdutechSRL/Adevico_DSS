using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectSettings : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long IdProject { get; set; }
        Int32 IdCommunity { get; set; }
        Boolean ForPortal { get; set; }
        Boolean IsPersonal { get; set; }
        Boolean InEditingMode {get;set;}
        Boolean IsInitialized {get;set;}
        Boolean AllowSelectOwnerFromResources {get;set;}
        Boolean AllowSelectOwnerFromCommunity {get;set;}
        Boolean DisplayDuration {get;set;}
        Boolean DisplayOwnerChanger {get;set;}
        Boolean DisplayCompletion {get;set;}
        Boolean DisplayStatus {get;set;}
        Boolean DisplayActivitiesToAdd { get; set; }
        Boolean AllowEditDateCalculationByCpm {get;set;}
        dtoWorkingDay DefaultWorkingDay { get; set; }

        ProjectItemStatus Status { get; set; }
        Int32 ActivitiesToAdd { get; set; }
        void InitializeCalendar(dtoProject project, FlagDayOfWeek daysOfWeek, long calendar, long exceptions, PageListType fromView, Int32 idContainerCommunity);
        void InitializeControl(dtoProject project, dtoResource owner, List<ProjectAvailability> items, ProjectAvailability cAvailability, PageListType fromView, Int32 idContainerCommunity, Int32 activitiesToAdd = 0);


        void InitializeControlToSelectOwner(Int32 idCommunity, List<Int32> hideUsers);
        void InitializeControlToSelectOwnerFromProject(List<dtoProjectResource> resources);

        void DisplayOwnerInfo(String name);
        void LoadProject(dtoProject project, List<ProjectAvailability> items, ProjectAvailability cAvailability, Int32 activitiesToAdd, PageListType fromView, Int32 idContainerCommunity);
        void UpdateDefaultResourceSelector(List<dtoResource> resources);
        void UpdateSettings(dtoProjectSettingsSelectedActions actions, DateTime? startDate, DateTime? endDate);
        void UpdateSettings( DateTime? startDate, DateTime? endDate);
        dtoProject GetProject();
    }
}