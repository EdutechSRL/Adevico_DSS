using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoMenubar :dtoItem
    {
        public virtual string CssClass { get; set; }
        public virtual String Name { get; set; }
        public virtual Boolean IsCurrent { get; set; }
        public virtual MenuBarType MenuBarType { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual litePerson ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public dtoMenubar()
        {
            this.Type = MenuItemType.Menubar;
        }
    }
}
