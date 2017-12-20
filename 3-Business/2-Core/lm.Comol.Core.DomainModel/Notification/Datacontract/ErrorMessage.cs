using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Threading.Tasks;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.Notification.DataContract
{
    [Serializable, DataContract]
    public class ErrorMessage
    {
        [DataMember]
        public NotificationAction Action { get; set; }
        [DataMember]
        public lm.Comol.Core.Msmq.DataContract.ErrorInfo Error { get; set; }
        public ErrorMessage()
        {
            Error = new lm.Comol.Core.Msmq.DataContract.ErrorInfo();
        }
    }
}