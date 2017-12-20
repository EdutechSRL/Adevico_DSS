using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class ItemModuleSettings
    {
        public virtual Int32 IdModuleAction { get; set; }
        public virtual Int32 IdModuleAjaxAction { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual String FullyQualifiedName { get; set; }
        public virtual Int32 IdObjectType { get; set; }
        public virtual long IdObject { get; set; }
    }
}