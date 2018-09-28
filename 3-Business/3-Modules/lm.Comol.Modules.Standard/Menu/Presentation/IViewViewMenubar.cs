using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewViewMenubar : iDomainView
    {
        MenuBarType PreviousView { get; }
        Boolean AllowManage { get; set; }
        long PreloadIdMenubar { get; }
        long IdMenubar { get; set; }


        void LoadMenubar(dtoTree tree, dtoItem selectedItem);
       // void SelectTreeItem(dtoTreeItem selectedItem);
        void NoPermission();
        void ItemUnknown();
        void MenubarUnknown();
        void LoadTopMenuItem(dtoTopMenuItem item, List<dtoTranslation> translations);
        void LoadTopMenuItem(dtoTopMenuItem item, List<dtoTranslation> translations, List<int> selectedTypes);
        void LoadMenuBarInfo(dtoMenubar menubar);
        void LoadColumnItem(dtoColumn column);
        void LoadSeparatorItem(dtoMenuItem item);
        void LoadMenuItem(dtoMenuItem item, List<dtoTranslation> translations, List<MenuItemType> availabeTypes, List<MenuItemType> availabeSubTypes);
        void LoadMenuItem(dtoMenuItem item, List<dtoTranslation> translations, List<int> selectedTypes, List<int> availableProfileTypes, List<MenuItemType> availabeTypes, List<MenuItemType> availabeSubTypes);
        void DisplaySessionTimeout(String url);
    }
}