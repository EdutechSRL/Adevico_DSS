using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using Adevico.APIconnection.Core;

namespace lm.Comol.Core.BaseModules.ApiToken.Domain
{
    public class MobiNotification : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Message { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public virtual DateTime NotificationDate { get; set; }
        public virtual Int32 PersonId { get; set; }

    }
}
