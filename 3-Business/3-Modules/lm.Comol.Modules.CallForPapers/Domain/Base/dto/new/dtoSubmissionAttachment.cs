using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmissionAttachment : dtoBaseFile
    {
        public lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType Type { get; set; }
        public dtoSubmissionAttachment()
            : base()
        {
        }
    }
   
}
