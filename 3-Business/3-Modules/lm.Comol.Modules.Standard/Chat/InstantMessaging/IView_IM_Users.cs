using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Chat
{
    public interface IView_IM_Users
    {
        void SetCurrentUsers(IList<InstantMessangerService.Ct1o1_Chat_DTO> CurrentUsers);

        Int32 CurrentUserId { get; set; }
    }
}
