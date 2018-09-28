using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectManagementStep
    {
        public virtual WizardProjectStep Type { get; set; }
        public virtual String Message { get; set; }
        public virtual List<EditingErrors> Errors { get; set; }
        public lm.Comol.Core.Wizard.WizardItemStatus Status { get; set; }
        public virtual String Url { get; set; }
        public dtoProjectManagementStep()
        {
            Message = "";
            Errors = new List<EditingErrors>();
            Status = Core.Wizard.WizardItemStatus.none;
        }
        public dtoProjectManagementStep(WizardProjectStep type)
            : this()
        {
            Type = type;
            Errors = new List<EditingErrors>();
        }
    }

    [Serializable]
    public class dtoSettingsStep : dtoProjectManagementStep
    {
        public virtual ProjectAvailability Availability { get; set; }
        //public virtual long NotificationTypes { get; set; }
        
        public dtoSettingsStep()
            : base()
        {
        }
        public dtoSettingsStep(WizardProjectStep type, Project project)
            : base(type)
        {
            Availability = (project==null) ? ProjectAvailability.Draft : project.Availability;
        }
    }
    [Serializable]
    public class dtoResourcesStep : dtoProjectManagementStep
    {
        public virtual long Count { get { return Managers + Resources; } }
        public virtual long Managers { get; set; }
        public virtual long Resources { get; set; }
        public virtual long InternalResources { get; set; }
        public virtual long ExternalResources { get; set; }
        public virtual long RemovedResources { get; set; }
        public dtoResourcesStep()
            : base()
        {

        }
        public dtoResourcesStep(WizardProjectStep type)
            : base(type)
        {

        }
    }
    [Serializable]
    public class dtoCalendarsStep : dtoProjectManagementStep
    {
        public virtual long Calendars { get; set; }
        public virtual long Exceptions { get; set; }
        public dtoCalendarsStep()
            : base()
        {

        }
        public dtoCalendarsStep(WizardProjectStep type)
            : base(type)
        {

        }
    }
    [Serializable]
    public class dtoDocumentsStep : dtoProjectManagementStep
    {
        //public virtual long NoPermissionsCount { get; set; }
        public virtual long ActivitiesItemCount { get { return ActivitiesUrls + ActivitiesFiles; } }
        public virtual long ActivitiesUrls { get; set; }
        public virtual long ActivitiesFiles { get; set; }
        public virtual long ItemCount { get {return ProjectUrls +ProjectFiles;}}
        public virtual long ProjectUrls { get; set; }
        public virtual long ProjectFiles { get; set; }
        public dtoDocumentsStep()
            : base()
        {
        }
        public dtoDocumentsStep(WizardProjectStep type)
            : base(type)
        {
        }
        public dtoDocumentsStep(WizardProjectStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
    }

}