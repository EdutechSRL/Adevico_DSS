using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmissionRevision : dtoBaseSubmission
    {
        public virtual int IdSubmittedBy { get; set; }
        public dtoSubmissionRevision()
            : base()
        {
        }

        public dtoSubmissionRevision(long id)
            : base(id)
        {
        }
        //public List<dtoSubmissionAttachment> SubmissionFiles()
        //{
        //    return Revision.SubmissionFiles();
        //}
    }
}