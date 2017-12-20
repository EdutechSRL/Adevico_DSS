using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewUcMailSets : iViewBase
    {
        Domain.Enums.MailSettings MailSettings { get; set; }


        //Int32 CurrentCommunityId { get; }
        //Int64 CurrentUserId { get; }
        //Int64 CurrentTicketId { get; }

        //Boolean IsPortal { get; }
    }
}
