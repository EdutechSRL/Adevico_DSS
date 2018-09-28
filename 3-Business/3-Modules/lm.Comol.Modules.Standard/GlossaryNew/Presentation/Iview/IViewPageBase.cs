using System;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewPageBase : IViewBase
    {
        #region "Preload"

        Int32 PreloadIdCommunity { get; }
        long PreloadIdGlossary { get; }
        long PreloadIdTerm { get; }
        Boolean PreloadForManagement { get; }
        Boolean PreloadFromViewMap { get; }
        Boolean IsFirstOpen { get; }
        Boolean IsFromListGlossary { get; }
        Boolean PreloadShowSave { get; }

        Int32 PreloadFromType { get; }

        Boolean LoadfromCookies { get; }
        String IdCookies { get; }

        Int32 PreloadPageIndex { get; }

        String PreloadTermList { get; }

        #endregion

        #region "Context"

        Int32 IdCommunity { get; set; }
        long IdGlossary { get; set; }
        long IdTerm { get; set; }
        Boolean FromViewMap { get; set; }
        Boolean FromViewGlossary { get; set; }

        #endregion

        #region "Filters"

        String SearchString { get; }
        String LemmaString { get; }
        String LemmaContentString { get; }
        Int32 LemmaSearchType { get; }
        Int32 LemmaVisibilityType { get; }

        #endregion

        #region "Common"

        void SendUserAction(int idCommunity, int idModule, ModuleGlossaryNew.ActionType action);

        void SendUserAction(int idCommunity, int idModule, long idItem, ModuleGlossaryNew.ObjectType type, ModuleGlossaryNew.ActionType action);

        #endregion
    }
}