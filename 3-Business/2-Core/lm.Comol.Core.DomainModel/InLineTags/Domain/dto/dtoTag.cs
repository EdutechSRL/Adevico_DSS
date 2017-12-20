using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Filters;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.InLineTags.Domain
{
    [Serializable]
    public class dtoInLineTag
    {
        public long Id { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }

        public dtoInLineTag()
        {

        }
    }
}