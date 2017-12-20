using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewUcPassword : iViewBase
    {
        string Password { get; }
        string NewPassword { get; }
        string RetypedPassword { get; }

        void ShowResponse(Domain.Enums.ExternalUserPasswordErrors error);

        Domain.DTO.DTO_User CurrentUser { get; }


        Domain.DTO.DTO_NotificationSettings NotificationSettings { get; }
    }


}
