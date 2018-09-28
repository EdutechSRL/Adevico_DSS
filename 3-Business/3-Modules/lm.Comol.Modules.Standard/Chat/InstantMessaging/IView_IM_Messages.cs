using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Chat
{
    public interface IView_IM_Messages
    {
         void SetCurrentChat(InstantMessangerService.Ct1o1_MessagesContainer_DTO CurrentChat);
         
         Guid CurrentChatId { get; set; }
         Int32 CurrentUserId { get; set; }
         String CurrentMessage { get; set; }

    }
}
