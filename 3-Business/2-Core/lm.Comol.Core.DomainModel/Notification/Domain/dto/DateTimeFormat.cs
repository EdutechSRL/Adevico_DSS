using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]
    [KnownType(typeof(DateTimeFormat))]
    public class DateTimeFormat
    {
        [DataMember]
        public virtual String CodeLanguage { get; set; }
        [DataMember]
        public virtual String Format { get; set; }

        public DateTimeFormat()
        {
        }
    }
}