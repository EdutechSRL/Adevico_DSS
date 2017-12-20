using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    public class BaseModuleTextItem<T> : BaseTextItem<T>
    {
        public virtual String CodeModule { get; set; }
        public virtual int RequiredModuleAction { get; set; }
        public virtual int IdModule { get; set; }
        public virtual ModuleDefinition Module { get; set; }


        public virtual long IdObjectLong { get; set; }
        public virtual Guid IdObjectGuid { get; set; }
        public virtual long ContentPermission { get; set; }
        public virtual ModuleLink ModuleLink { get; set; }
    }
}