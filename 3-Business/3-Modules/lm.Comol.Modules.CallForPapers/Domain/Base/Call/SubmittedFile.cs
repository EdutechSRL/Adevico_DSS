using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class SubmittedFile : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Boolean isReplaced { get; set; }
        public virtual UserSubmission Submission {get;set;}
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem File { get; set; }
        public virtual liteModuleLink Link { get; set; }
        public virtual RequestedFile SubmittedAs { get; set; }
    }
}
