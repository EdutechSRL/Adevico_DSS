using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewMenuItem : iDomainView
    {
        long IdItem { get; set; }
        long IdColumnOwner { get; set; }
        long IdItemOwner { get; set; }
        long IdTopItem { get; set; }
        dtoMenuItem GetItem { get; }
        void InitalizeControl(dtoMenuItem item, Boolean allowEdit, List<MenuItemType> availabeTypes, List<MenuItemType> availabeSubTypes);
    }
}