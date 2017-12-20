using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.BaseModules.CommonControls.Presentation
{
    public interface IViewContentTranslatorSelector : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        String MultiName { get; }
        String MultiToolTip { get; }
        Boolean IsInitialized { get; set; }
        Boolean RaiseSelectionEvent { get; set; }
        LanguageItem SelectedItem { get; set; }
        BaseLanguageItem FirstItemToLoad { get; set; }
        List<BaseLanguageItem> AvailableLanguages { get; set; }
        List<LanguageItem> InUseLanguages { get; set; }

        void InitializeControl(List<BaseLanguageItem> availableLanguages);
        void InitializeControl(List<BaseLanguageItem> availableLanguages, List<LanguageItem> inUseItems, LanguageItem current);
        void InitializeControl(List<lm.Comol.Core.DomainModel.Language> availableLanguages, Boolean alsoMultilanguage);
        void InitializeControl(List<lm.Comol.Core.DomainModel.Language> availableLanguages,List<Int32> inUseItems, Boolean alsoMultilanguage=false, Boolean selectMultilanguage= false, Int32 idSelected=-1);
        void InitializeControl( List<lm.Comol.Core.DomainModel.Language> availableLanguages, List<String> inUseItems,Boolean alsoMultilanguage = false, Boolean selectMultilanguage = false, String selectedCode="");

        LanguageItem RemoveCurrent();
        BaseLanguageItem GetMultiLanguageItem();
        void LoadItems(List<BaseLanguageItem> itemsToAdd, List<LanguageItem> inUseItems = null, LanguageItem selected = null);
        void LoadItems(List<LanguageItem> inUseItems);
        void DisplaySessionTimeout();
    }
}