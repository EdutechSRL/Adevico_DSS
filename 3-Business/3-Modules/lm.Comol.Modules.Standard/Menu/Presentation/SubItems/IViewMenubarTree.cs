using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewMenubarTree
    {
        dtoItem SelectedItem { get; set; }
        void ChangeTreeItemName(dtoItem selectedItem, String name);
        void InitalizeControl(dtoTree item, dtoItem selectedItem);
    }
}