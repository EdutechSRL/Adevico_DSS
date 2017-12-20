using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoRevisionFilters
    {
        public virtual RevisionOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual RevisionStatus Status { get; set; }
        public virtual String SearchForName { get; set; }
        public virtual CallForPaperType CallType { get; set; }
        public virtual Dictionary<RevisionStatus, String> TranslationsStatus { get; set; }
        public virtual Dictionary<RevisionType, String> TranslationsType { get; set; }
        public dtoRevisionFilters()
        {
            TranslationsStatus = new Dictionary<RevisionStatus, String>();
            TranslationsType = new Dictionary<RevisionType, String>();
        }

    }  
    
}