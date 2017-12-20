using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{

    [Serializable]
    public class dtoSelectItem<T>
    {
        public virtual T Id { get; set; }
        public virtual Boolean Selected { get; set; }
    }
}