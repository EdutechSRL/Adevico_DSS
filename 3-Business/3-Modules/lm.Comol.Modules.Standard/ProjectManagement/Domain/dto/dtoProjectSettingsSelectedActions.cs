using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectSettingsAction
    {
        public virtual List<ConfirmActions> DateActions { get; set; }
        public virtual List<ConfirmActions> CpmActions { get; set; }
        public virtual List<ConfirmActions> ManualActions { get; set; }
        public virtual List<ConfirmActions> SummariesActions { get; set; }
        public virtual List<ConfirmActions> MilestonesActions { get; set; }
        public virtual List<ConfirmActions> EstimatedActions { get; set; }

        public virtual long EstimatedActivities { get; set; }
        public virtual long Activities { get; set; }
        public virtual long Summaries { get; set; }
        public virtual long Milestones { get; set; }

        public dtoProjectSettingsAction() {

        }
    }
}