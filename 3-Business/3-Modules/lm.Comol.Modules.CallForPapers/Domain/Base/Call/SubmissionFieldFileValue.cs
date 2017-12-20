using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class SubmissionFieldFileValue : SubmissionFieldBaseValue
    {
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem Item { get; set; }
        public virtual liteModuleLink Link { get; set; }
    }
}
