using System;
using System.Collections.Generic;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.Wizard;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_GlossaryStep
    {
        public DTO_GlossaryStep()
        {
            Message = String.Empty;
            Errors = new List<EditingErrors>();
            Status = WizardItemStatus.none;
        }

        public DTO_GlossaryStep(GlossaryStep type)
            : this()
        {
            Type = type;
        }

        public virtual GlossaryStep Type { get; set; }
        public virtual List<EditingErrors> Errors { get; set; }
        public virtual String Message { get; set; }
        public WizardItemStatus Status { get; set; }
        public virtual String Url { get; set; }
    }

    [Serializable]
    public class dtoSettingsStep : DTO_GlossaryStep
    {
        public dtoSettingsStep()
        {
        }

        public dtoSettingsStep(GlossaryStep type)
            : base(type)
        {
            //DashboardStatus = (settings == null) ? AvailableStatus.Draft : settings.Status;
            //if (settings != null)
            //{
            //    Persons = 2;        // settings.GetAssignments(DashboardAssignmentType.User).Count();
            //    Roles = 2;          // settings.GetAssignments(DashboardAssignmentType.RoleType).Count();
            //    ProfileTypes = 2;   // settings.GetAssignments(DashboardAssignmentType.ProfileType).Count();
            //    if (!settings.ForAll && Persons == 0 && Roles == 0 && ProfileTypes == 0)
            //    {
            //        Errors.Add(EditingErrors.NoAssignedItems);
            //        Status =Comol.Core.Wizard.WizardItemStatus.warning;
            //    }
            //    else
            Status = WizardItemStatus.valid;
            //}
            //else
            //    Status = Comol.Core.Wizard.WizardItemStatus.none;
        }

        public virtual AvailableStatus DashboardStatus { get; set; }
        public virtual long Persons { get; set; }
        public virtual long Roles { get; set; }
        public virtual long ProfileTypes { get; set; }
    }
}