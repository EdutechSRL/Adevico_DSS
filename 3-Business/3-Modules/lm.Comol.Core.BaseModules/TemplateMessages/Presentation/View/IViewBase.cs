using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        TemplateType PreloadTemplateType { get; }
        TemplateType CurrentType { get; set; }
        dtoBaseTemplateOwner PreloadOwnership { get; }
        dtoBaseTemplateOwner Ownership { get; set; }

        void DisplaySessionTimeout(String url);
        void DisplayNoPermission(int idCommunity, int idModule);

        void SendUserAction(int idCommunity, int idModule, ModuleTemplateMessages.ActionType action);
    }
}