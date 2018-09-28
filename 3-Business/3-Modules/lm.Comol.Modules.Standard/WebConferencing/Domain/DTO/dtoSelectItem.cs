using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    [Serializable]
    public class dtoSelectItem<T>
    {
        public virtual T Id { get; set; }
        public virtual Boolean Selected { get; set; }
    }
}