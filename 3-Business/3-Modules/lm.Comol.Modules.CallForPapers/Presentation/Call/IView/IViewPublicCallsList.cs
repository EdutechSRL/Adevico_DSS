using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewPublicCallsList : IViewBasePublicCallList<dtoCallItemPermission>
    {
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}