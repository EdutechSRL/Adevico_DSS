using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectPermission
    {
        public virtual Boolean Edit { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean VirtualUndelete { get; set; }
        public virtual Boolean PhisicalDelete { get; set; }
        public virtual Boolean EditResources { get; set; }
        public virtual Boolean ViewMap{ get; set; }
        public virtual Boolean ViewMyCompletion { get; set; }
        public virtual Boolean EditMap { get; set; }
        public virtual Boolean EditAttachments { get; set; }
        public virtual Boolean ViewAttachments { get; set; }

        public dtoProjectPermission() { }

        //public dtoActivityPermission(PmActivityPermission pPermissions,Boolean allowEdit, Boolean allowDelete, Boolean allowSummary, Boolean dateCalculationByCpm)
        //{
        //    AddActivityAfter = allowEdit;
        //    AddActivityBefore = allowEdit;
        //    AddChildren = allowEdit && allowSummary;
        //    AddLinkedActivityAfter = allowEdit && dateCalculationByCpm;
        //    AddLinkedChildren = allowEdit && allowSummary && dateCalculationByCpm;
        //    Edit = allowEdit;
        //    VirtualDelete = allowDelete;

        //    SetStartDate = allowEdit && !dateCalculationByCpm;
        //    ViewPredecessors = dateCalculationByCpm;
        //    SetPredecessors = allowEdit && dateCalculationByCpm;
        //    SetDuration = allowEdit;
        //    SetDeadline = allowEdit || ((pPermissions & PmActivityPermission.SetDeadline) != 0);
        //    SetResources = allowEdit || ((pPermissions & PmActivityPermission.ManageResources) != 0);
        //    SetOthersCompletion = allowEdit || ((pPermissions & PmActivityPermission.SetCompleteness) != 0); 
        //    SetMyCompletion = allowEdit || ((pPermissions & PmActivityPermission.SetMyCompleteness) != 0);
        //    SetCompleteness = allowEdit || ((pPermissions & PmActivityPermission.SetCompleteness) != 0);
        //    SetAttachments = allowEdit || ((pPermissions & PmActivityPermission.AddAttachments) != 0);
        //}
    }
}