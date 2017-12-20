using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.MailCommons.Domain.Configurations
{
    [Serializable, DataContract]
    public class SmtpAuthenticationConfig
    {
        [DataMember]
        public virtual Boolean Enabled { get; set; }
        [DataMember]
        public virtual Boolean EnableSsl { get; set; }
        [DataMember]
        public virtual Boolean UseDefaultCredentials { get; set; }
        [DataMember]
        public virtual String AccountName { get; set; }
        [DataMember]
        public virtual String AccountPassword { get; set; }

        public SmtpAuthenticationConfig()
        {
            Enabled = false;
            EnableSsl = false;
        }
    }
}
