using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class dtoTranslatedCommunityType
    {
        public virtual Int32 Id { get; set; }
        public virtual String Name { get; set; }
    }
}