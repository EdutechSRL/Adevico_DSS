using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGlossaryList : IViewPageBase
    {
        void SetTitle(String name);
        void LoadViewData(Int32 idCommunity, Boolean manageShareEnabled, Boolean manageShare);
        void GoToGlossaryView(Int64 idGlossary);
    }
}