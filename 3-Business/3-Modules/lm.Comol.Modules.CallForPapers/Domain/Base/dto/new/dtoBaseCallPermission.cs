using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoBaseCallPermission
    {
        public virtual Boolean Delete { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean UnDelete { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Submit { get; set; }
        public virtual Boolean ViewSubmissions { get; set; }

        public dtoBaseCallPermission()
        { 
        }
        //public dtoCallPermission(long id, int communityId, CallStatusForSubmitters status, dtoCallForPaper callForPaper, ModuleCallForPaper module, litePerson person)
        //    : base(id)
        //{
        //    Deleted = callForPaper.Deleted;
        //    AllowDelete = false;
        //    AllowVirtualDelete = Deleted == BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    AllowUnDelete = Deleted != BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    AllowEdit = Deleted == BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    AllowEvaluation = Deleted == BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration);
        //}
        //public dtoCallPermission(dtoCallForPaperPermission dto, ModuleCallForPaper module, litePerson person, long submissionCount)
        //    : base(dto.Id)
        //{
        //    Deleted = dto.Deleted;
        //    AllowDelete = false;
        //    AllowVirtualDelete = !HasUserSubmission && Deleted == BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    AllowUnDelete = Deleted != BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    AllowEdit = Deleted == BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //}
    }
}