using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoCallPermission : dtoBaseCallPermission
    {
        public virtual Boolean ManageComittees { get; set; }
        public virtual Boolean ManageEvaluation { get; set; }
        public virtual Boolean Evaluate { get; set; }

        public dtoCallPermission()
            : base()
        { 
        }
    }
}