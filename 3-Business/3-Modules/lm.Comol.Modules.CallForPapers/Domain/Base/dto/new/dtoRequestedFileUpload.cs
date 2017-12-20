using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoRequestedFileUpload :dtoBase 
    {
        public virtual ModuleActionLink ActionLink { get; set; }
        public virtual long IdSubmitted { get; set; }
        public virtual long IdRequired { get; set; }
        public dtoRequestedFileUpload()
            : base()
        { 
        }
    }
}