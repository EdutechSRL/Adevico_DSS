﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Faq
{
    [Serializable]
    public class Faq_Int : DomainBaseObjectMetaInfo<Int64>
    {
        //public virtual Int64 ID { get; set; }
        public virtual String Quest { get; set; }
        public virtual String Answer { get; set; }
    }
}
