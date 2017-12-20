using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    public class BaseCommunityTextItem<T> : BaseTextItem<T>
    {
        public virtual Community Community { get; set; }
    }
}