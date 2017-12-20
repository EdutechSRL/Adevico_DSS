using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoDashboardStep
    {
        public virtual WizardDashboardStep Type { get; set; }
        public virtual String Message { get; set; }
        public virtual List<EditingErrors> Errors { get; set; }
        public lm.Comol.Core.Wizard.WizardItemStatus Status { get; set; }
        public virtual String Url { get; set; }
        public dtoDashboardStep()
        {
            Message = "";
            Errors = new List<EditingErrors>();
            Status = Core.Wizard.WizardItemStatus.none;
        }
        public dtoDashboardStep(WizardDashboardStep type)
            : this()
        {
            Type = type;
            Errors = new List<EditingErrors>();
        }
    }

    [Serializable]
    public class dtoSettingsStep : dtoDashboardStep
    {
        public virtual AvailableStatus DashboardStatus { get; set; }
        public virtual long Persons { get; set; }
        public virtual long Roles { get; set; }
        public virtual long ProfileTypes { get; set; }
        
        public dtoSettingsStep()
            : base()
        {
        }
        public dtoSettingsStep(WizardDashboardStep type, liteDashboardSettings settings)
            : base(type)
        {
            DashboardStatus = (settings == null) ? AvailableStatus.Draft : settings.Status;
            if (settings != null)
            {
                Persons = settings.GetAssignments(DashboardAssignmentType.User).Count();
                Roles = settings.GetAssignments(DashboardAssignmentType.RoleType).Count();
                ProfileTypes = settings.GetAssignments(DashboardAssignmentType.ProfileType).Count();
                if (!settings.ForAll && Persons == 0 && Roles == 0 && ProfileTypes == 0)
                {
                    Errors.Add(EditingErrors.NoAssignedItems);
                    Status = Wizard.WizardItemStatus.warning;
                }
                else
                    Status = Wizard.WizardItemStatus.valid;
            }
            else
                Status= Wizard.WizardItemStatus.none;
                
        }
    }
    [Serializable]
    public class dtoHomeSettingsStep : dtoDashboardStep
    {
        public virtual long Pages { get; set; }
        public dtoHomeSettingsStep()
            : base()
        {

        }
        public dtoHomeSettingsStep(WizardDashboardStep type)
            : base(type)
        {

        }
        public dtoHomeSettingsStep(WizardDashboardStep type, liteDashboardSettings settings)
            : base(type)
        {
            if (settings != null)
            {
                Pages = settings.Pages.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type != DashboardViewType.Subscribe && p.Type != DashboardViewType.Search).Count();
                if (Pages == 0)
                    Errors.Add(EditingErrors.NoViews);
                Status = ((Pages == 0) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.valid);
            }
            else
                Status= Wizard.WizardItemStatus.disabled;
        }
    }
    [Serializable]
    public class dtoTileStep : dtoDashboardStep
    {
        public virtual long Tiles { get; set; }
        public virtual long InUseTiles { get; set; }
        public virtual long AvailableTiles { get; set; }
        public virtual long UserTile { get; set; }
        public dtoTileStep()
            : base()
        {

        }
        public dtoTileStep(WizardDashboardStep type)
            : base(type)
        {

        }
    }
    public enum EditingErrors
    {
        None = 0,
        NoViews = 1,
        NoTiles = 2,
        NoTilesSelected = 3,
        NoAssignedItems = 4
    }
}