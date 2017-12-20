using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable]
    public class expEvaluator : DomainObject<long>, IEquatable<expEvaluator>

    {
        public virtual long IdCall { get; set; }
        public virtual expPerson Person { get; set; }

        public expEvaluator()
        {

        }
        public virtual String DisplayName(String unknownUser)
        {
            return (Person == null) ? unknownUser : Person.SurnameAndName;
        }

        public virtual bool Equals(expEvaluator other)
        {
            return (other!=null && this.Id == other.Id);
        }
    }
}