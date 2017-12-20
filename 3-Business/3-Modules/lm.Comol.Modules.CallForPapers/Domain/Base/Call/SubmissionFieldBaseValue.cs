using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class SubmissionFieldBaseValue : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual FieldDefinition Field { get; set; }
        public virtual UserSubmission Submission { get; set; }
        public virtual Revision Revision { get; set; }
        public virtual Boolean isReplaced { get; set; }
    }
}
