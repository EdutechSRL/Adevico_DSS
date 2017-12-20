using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    public class dtoSubmitterTypePermission :dtoBase 
    {
        public virtual dtoSubmitterType Submitter { get; set; }
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean AllowVirtualDelete { get; set; }
        public virtual Boolean AllowUnDelete { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public virtual long SubmissionCount { get; set; }
        public dtoSubmitterTypePermission()
            : base()
        { 
        }
        public dtoSubmitterTypePermission(long id, dtoSubmitterType submitter)
            : base(id)
        {
            Submitter = submitter;
            Deleted = submitter.Deleted;
            SubmissionCount = 0;
        }

        public dtoSubmitterTypePermission(dtoSubmitterType submitter, dtoGenericPermission generic, long count)
            : base(submitter.Id)
        {
            Submitter = submitter;
            Deleted = submitter.Deleted;
            AllowDelete = generic.AllowDelete && Deleted != BaseStatusDeleted.None;
            AllowVirtualDelete = (Deleted == BaseStatusDeleted.None && generic.AllowVirtualDelete && !(count>0));
            AllowUnDelete = Deleted != BaseStatusDeleted.None && generic.AllowUnDelete;
            AllowEdit = (Deleted == BaseStatusDeleted.None && generic.AllowEdit);
            SubmissionCount = count;
        }

        //--------non servono
        
    }
}
