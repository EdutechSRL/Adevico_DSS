using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoTemplatePermission 
    {
        public virtual Boolean AllowUse { get; set; }
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean AllowVirtualDelete { get; set; }
        public virtual Boolean AllowUnDelete { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public virtual Boolean AllowChangePermission { get; set; }
        public virtual Boolean AllowClone { get; set; }
        public virtual Boolean AllowAdd { get; set; }
        public virtual Boolean AllowNewVersion { get; set; }

        public dtoTemplatePermission() { 
        
        }

        public virtual dtoTemplatePermission Copy() {
            dtoTemplatePermission p = new dtoTemplatePermission();
            p.AllowUse = AllowUse;
            p.AllowDelete = AllowDelete;
            p.AllowVirtualDelete = AllowVirtualDelete;
            p.AllowUnDelete = AllowUnDelete;
            p.AllowEdit = AllowEdit;
            p.AllowChangePermission = AllowChangePermission;
            p.AllowClone = AllowClone;
            p.AllowAdd = AllowAdd;
            p.AllowNewVersion = AllowNewVersion;
            return p;
        }
    }
}