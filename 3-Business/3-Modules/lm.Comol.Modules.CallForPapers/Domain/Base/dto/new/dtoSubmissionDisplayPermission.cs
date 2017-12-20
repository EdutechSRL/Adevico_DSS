using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmissionDisplayPermission
    {
        public virtual Boolean Evaluate { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Delete { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean VirtualUndelete { get; set; }
        public virtual Boolean DownloadFiles { get; set; }

        public dtoSubmissionDisplayPermission()
        { 
        }

        //Boolean forManage = (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && dto.Submission.Owner.Id == person.Id
        public dtoSubmissionDisplayPermission(dtoSubmissionDisplay submission, Boolean manage, CallForPaperType type)
        {
            Delete = (submission.Deleted != BaseStatusDeleted.None 
                && (type!= CallForPaperType.CallForBids || submission.Status== SubmissionStatus.draft)); // && type != CallForPaperType.CallForBids);

            VirtualDelete = 
                (submission.Deleted == BaseStatusDeleted.None 
                && (type != CallForPaperType.CallForBids || submission.Status == SubmissionStatus.draft || submission.Status == SubmissionStatus.waitforsignature)) 
                && manage;

            VirtualUndelete = (submission.Deleted != BaseStatusDeleted.None) && manage;
            Edit = (submission.Deleted == BaseStatusDeleted.None) && manage;
        }
    }
}