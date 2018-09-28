using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewBase : iDomainView
    {
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
    }
}