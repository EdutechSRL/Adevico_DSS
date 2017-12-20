using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class SubmissionFieldStringValue : SubmissionFieldBaseValue
    {
        public virtual String Value { get; set; }
        public virtual String UserValue { get; set; }
    }
}