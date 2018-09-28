using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoActivityPermission
    {
        public virtual Boolean Edit { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean AddChildren { get; set; }
        public virtual Boolean AddLinkedChildren { get; set; }
        public virtual Boolean AddActivityBefore { get; set; }
        public virtual Boolean AddActivityAfter{ get; set; }
        public virtual Boolean AddLinkedActivityAfter { get; set; }
        public virtual Boolean ToChild { get; set; }
        public virtual Boolean ToFather { get; set; }
        public virtual Boolean SetPredecessors { get; set; }
        public virtual Boolean ViewPredecessors { get; set; }
        public virtual Boolean SetStartDate { get; set; }
        public virtual Boolean SetDeadline { get; set; }
        public virtual Boolean SetDuration { get; set; }
        public virtual Boolean SetMyCompletion { get; set; }
        public virtual Boolean SetOthersCompletion { get; set; }
        public virtual Boolean SetCompleteness { get; set; }
        public virtual Boolean SetResources { get; set; }
        public virtual Boolean SetAttachments { get; set; }

        public dtoActivityPermission() { }

        public dtoActivityPermission(PmActivityPermission pPermissions,Boolean allowEdit, Boolean allowDelete, Boolean allowSummary, Boolean dateCalculationByCpm)
        {
            AddActivityAfter = allowEdit;
            AddActivityBefore = allowEdit;
            AddChildren = allowEdit && allowSummary;
            AddLinkedActivityAfter = allowEdit && dateCalculationByCpm;
            AddLinkedChildren = allowEdit && allowSummary && dateCalculationByCpm;
            Edit = allowEdit;
            VirtualDelete = allowDelete;

            SetStartDate = allowEdit && !dateCalculationByCpm;
            ViewPredecessors = dateCalculationByCpm;
            SetPredecessors = allowEdit && dateCalculationByCpm;
            SetDuration = allowEdit;
            SetDeadline = allowEdit || ((pPermissions & PmActivityPermission.SetDeadline) != 0);
            SetResources = allowEdit || ((pPermissions & PmActivityPermission.ManageResources) != 0);
            SetOthersCompletion = allowEdit || ((pPermissions & PmActivityPermission.SetCompleteness) != 0); 
            SetMyCompletion = allowEdit || ((pPermissions & PmActivityPermission.SetMyCompleteness) != 0);
            SetCompleteness = allowEdit || ((pPermissions & PmActivityPermission.SetCompleteness) != 0);
            SetAttachments = allowEdit || ((pPermissions & PmActivityPermission.AddAttachments) != 0);
        }
    }
}