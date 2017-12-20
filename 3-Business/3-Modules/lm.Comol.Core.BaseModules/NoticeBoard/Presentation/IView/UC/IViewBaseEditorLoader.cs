using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewBaseEditorLoader : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean UseRealFontSize { get; set; }
        Boolean isInitialized { get; set; }
        void InitializeControl(NoticeboardMessage message);
        void DisplaySessionTimeout();
        void DisplayEditingDisabled();
        void LoadFontFamily(List<String> items, String selected);
        void LoadFontSize(List<String> items, String selected);
        void LoadMessage(NoticeboardMessage message);
    }
}