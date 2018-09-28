using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoReorderAction
    {
        public ReorderAction Action{get;set;}
        public Boolean Selected { get; set; }

        public dtoReorderAction() { }
        public dtoReorderAction(ReorderAction action, Boolean selected =false) {
            Action = action;
            Selected = selected;
        }
    }
}
