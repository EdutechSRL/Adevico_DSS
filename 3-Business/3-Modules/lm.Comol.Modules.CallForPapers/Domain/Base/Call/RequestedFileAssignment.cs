using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class RequestedFileAssignment : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual SubmitterType SubmitterType { get; set; }
        public virtual RequestedFile RequestedFile { get; set; }

        public RequestedFileAssignment()
        {
            Deleted = BaseStatusDeleted.None;
        }
        public RequestedFileAssignment(RequestedFile requestedFile, SubmitterType submitterType)
        {
            SubmitterType = submitterType;
            RequestedFile = requestedFile;
            Deleted = BaseStatusDeleted.None;
        }
    }
}
