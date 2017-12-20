using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    public class ModuleTextItem<T> : BaseTextItem<T>
    {
        public virtual AssignedModuleInfo ModuleInfo { get; set; }
    }
}