using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Mail
{
    [Serializable]
    public class dtoMailSettings : IEquatable<dtoMailSettings>
    {
        public virtual SenderType Sender {get;set;}
        public virtual SubjectType Subject {get;set;}
        public virtual Boolean isHtml {get;set;}
        public virtual SignatureType SignatureType { get; set; }
        public virtual Boolean NotifyToSender { get; set; }
        public virtual Boolean CopyToSender { get; set; }
        public virtual long IdSkin { get; set; }

        public dtoMailSettings() {
            SignatureType = Mail.SignatureType.None;
        }

        public virtual dtoMailSettings Copy()
        {
            dtoMailSettings settings = new dtoMailSettings();
            settings.Sender = Sender;
            settings.Subject = Subject;
            settings.isHtml = isHtml;
            settings.SignatureType = SignatureType;
            settings.NotifyToSender = NotifyToSender;
            settings.CopyToSender = CopyToSender;
            settings.IdSkin = IdSkin;
            return settings;
        }

        public bool Equals(dtoMailSettings other)
        {
            return other!=null && Sender == other.Sender && Subject == other.Subject && isHtml== other.isHtml && SignatureType == other.SignatureType && NotifyToSender == other.NotifyToSender && CopyToSender == other.CopyToSender && IdSkin == other.IdSkin;
        }
    }
}
