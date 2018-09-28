using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Faq
{
    [Serializable]
    public class FaqOnCategory : DomainBaseObjectMetaInfo<Int64>
    {
        //public virtual Int64 Id { get; set; }
        public virtual Int64 FaqId { get; set; }
        public virtual Int64 CatId { get; set; }
    }
}
