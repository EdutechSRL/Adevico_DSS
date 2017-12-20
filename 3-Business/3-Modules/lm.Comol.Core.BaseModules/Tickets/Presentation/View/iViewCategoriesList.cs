using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewCategoriesList : iViewBase
    {
        void BindList(IList<Domain.DTO.DTO_CategoryList> Categories, Boolean IsManager, Int64 DefaultCategoryId);

        //int PreloadedCommunityId { get; }

        String CommunityName { set; }

        //int ViewCommunityId { get; set; }

        void ShowNoPermission();

        void ShowSendInfo(bool sended);

        ////To iView Base Internal
        //void DisplaySessionTimeout();


        //void SendAction(
        //    ModuleTicket.ActionType Action,
        //    Int32 idCommunity,
        //    ModuleTicket.InteractionType Type,
        //    IList<KeyValuePair<Int32, String>> Objects = null);
    }
}
