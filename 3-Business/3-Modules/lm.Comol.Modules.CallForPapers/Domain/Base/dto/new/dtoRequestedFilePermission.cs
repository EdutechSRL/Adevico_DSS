using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoRequestedFilePermission :dtoBase 
    {
        public virtual dtoCallRequestedFile File { get; set; }
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean AllowVirtualDelete { get; set; }
        public virtual Boolean AllowUnDelete { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public virtual long SubmissionCount{ get; set; }
        public dtoRequestedFilePermission()
            : base()
        { 
        }
        public dtoRequestedFilePermission(long id, dtoCallRequestedFile file)
            : base(id)
        {
            File = file;
            Deleted = file.Deleted;
        }
        public dtoRequestedFilePermission(dtoCallRequestedFile file, dtoGenericPermission generic, long count)
            : base(file.Id)
        {
            File = file;
            Deleted = file.Deleted;
            AllowDelete = generic.AllowDelete  && Deleted != BaseStatusDeleted.None;
            AllowVirtualDelete = Deleted == BaseStatusDeleted.None && generic.AllowVirtualDelete && !(count>0);
            AllowUnDelete = Deleted != BaseStatusDeleted.None && generic.AllowUnDelete;
            AllowEdit = Deleted == BaseStatusDeleted.None && !(count > 0) && generic.AllowEdit;
            SubmissionCount = count;
        }

        public void UpdatePermission(dtoGenericPermission generic, long count)
        {
            AllowDelete = generic.AllowDelete && Deleted != BaseStatusDeleted.None;
            AllowVirtualDelete = Deleted == BaseStatusDeleted.None && generic.AllowVirtualDelete && !(count > 0);
            AllowUnDelete = Deleted != BaseStatusDeleted.None && generic.AllowUnDelete;
            AllowEdit = Deleted == BaseStatusDeleted.None && !(count > 0) && generic.AllowEdit;
            SubmissionCount = count;
        }
    }
}