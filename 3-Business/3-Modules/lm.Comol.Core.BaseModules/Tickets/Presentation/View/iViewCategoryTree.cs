using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewCategoryTree : iViewBase
    {
        void BindTree(IList<Domain.DTO.DTO_CategoryTree> TreeItems);

        String GetReorderStr();

        ////To iView Base Internal
        //void DisplaySessionTimeout();

        //int ViewCommunityId { get; set; }

        //void ShowNoPermission();

        //void ShowPostSave(Boolean IsSaved);

        //void SendAction(
        //    ModuleTicket.ActionType Action,
        //    Int32 idCommunity,
        //    ModuleTicket.InteractionType Type,
        //    IList<KeyValuePair<Int32, String>> Objects = null);

        void ShowMessage(Domain.Enums.CategoryTreeMessageType MsgType);

    }
}
