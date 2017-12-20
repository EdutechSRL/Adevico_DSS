using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    //dtoSubmissionInfo
    [Serializable]
    public class dtoSubmissionDisplayInfo : dtoBaseSubmission
    {
        public virtual long IdSubmitterType { get; set; }
        public dtoSubmissionDisplayInfo()
            : base()
        {

        }
        public dtoSubmissionDisplayInfo(long id)
            : base(id)
        {
        }
        public dtoSubmissionDisplayInfo(long id, List<Revision> revisions, Boolean full)
            : base(id, revisions, full)
        {
        }
    }
}