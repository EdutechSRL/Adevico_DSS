using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using lm.Comol.Core.DomainModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Notification.Domain
{
    [Serializable,DataContract]

    public class TemplateSettings
    {
        [DataMember]
        public virtual long IdTemplate { get; set; }
        [DataMember]
        public virtual long IdVersion { get; set; }
        [DataMember]
        public virtual Boolean IsCompliant { get; set; }
        public TemplateSettings() {

        }
    }
}