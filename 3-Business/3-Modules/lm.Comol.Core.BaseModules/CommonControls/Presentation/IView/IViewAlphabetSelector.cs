using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;


namespace lm.Comol.Core.BaseModules.CommonControls
{
    public interface IViewAlphabetSelector : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        AlphabetDisplayMode DisplayMode { get; set; }
        Boolean isInitialized { get; set; }
        Boolean RaiseSelectionEvent { get; set; }
        Boolean AutoSelectLetter { get; set; }
        String SelectedItem { get; set; }

        void InitializeControl(List<String> availableWords);
        void InitializeControl(List<String> availableWords, String activeWord);
        void InitializeControl(List<String> availableWords, AlphabetDisplayMode displayMode);
        void InitializeControl(List<String> availableWords, String activeWord, AlphabetDisplayMode displayMode);
        void LoadItems(List<AlphabetItem> alphabet);
        void DisplaySessionTimeout();
    }
}