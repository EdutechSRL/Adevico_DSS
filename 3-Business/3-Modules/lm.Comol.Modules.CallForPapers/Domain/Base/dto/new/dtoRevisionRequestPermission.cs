using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoRevisionRequestPermission
    {
        public virtual Boolean Accept { get; set; }
        public virtual Boolean Refuse { get; set; }
        public virtual Boolean RefuseUserRequest { get; set; }
        public virtual Boolean Cancell { get; set; }
        public virtual Boolean Approve { get; set; }

        public virtual Boolean Compile { get; set; }
        public virtual Boolean Manage { get; set; }
        public virtual Boolean Delete { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean VirtualUndelete { get; set; }

        public dtoRevisionRequestPermission()
        { 
        }

        public dtoRevisionRequestPermission(CallStandardAction action, dtoRevisionDisplay revision)
        { 
            Boolean forManage = (action == CallStandardAction.Manage);
            VirtualUndelete = (revision.Deleted!= BaseStatusDeleted.None);
            Delete = (revision.Deleted!= BaseStatusDeleted.None);
            VirtualDelete = (revision.Deleted == BaseStatusDeleted.None) && revision.Status != RevisionStatus.Approved;
            Manage = forManage;
            Compile = !forManage;
           
            
            Cancell = (!forManage && revision.Status== RevisionStatus.Request) || (forManage && revision.Status== RevisionStatus.Required);
            Accept = (forManage && revision.Status == RevisionStatus.Request);
            Approve = forManage && revision.Status == RevisionStatus.Submitted;
            Refuse = forManage && (revision.Status == RevisionStatus.Submitted);
            RefuseUserRequest = forManage && revision.Status == RevisionStatus.Request;

        }
    }
}