using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Mail
{
    [Serializable,DataContract]
    public class SmtpAuthentication
    {
        [DataMember]
        public virtual Boolean Enabled { get; set; }
        [DataMember]
        public virtual Boolean EnableSsl { get; set; }
        [DataMember]
        public virtual Boolean UseDefaultCredentials  { get; set; }
        [DataMember]
        public virtual String AccountName { get; set; }
        [DataMember]
        public virtual String AccountPassword { get; set; }

        public SmtpAuthentication() {
            Enabled = false;
            EnableSsl = false;
        }
    }
}
