using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable()]
    public class TemplatePlaceHolder
    {
        public virtual Int32 Id { get; set; }
        public virtual String Tag { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual String ModuleName { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual Boolean IsCommon { get; set; }
    }
}
