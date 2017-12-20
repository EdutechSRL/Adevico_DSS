using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoEvaluationLink
    {
        public virtual DateTime ModifiedOn { get; set; }
        public virtual long IdLink { get; set; }
        public virtual long IdItem { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual long Id { get; set; }
    }
}
