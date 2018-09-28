using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectSettingsSelectedActions
    {
        public virtual ConfirmActions DateAction { get; set; }
        public virtual ConfirmActions CpmAction { get; set; }
        public virtual ConfirmActions ManualAction { get; set; }
        public virtual ConfirmActions SummariesAction { get; set; }
        public virtual ConfirmActions MilestonesAction { get; set; }
        public virtual ConfirmActions EstimatedAction { get; set; }

        public dtoProjectSettingsSelectedActions()
        {

        }
    }
}