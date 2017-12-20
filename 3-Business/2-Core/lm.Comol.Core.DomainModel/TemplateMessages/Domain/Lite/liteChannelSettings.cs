using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.MailCommons.Domain.Messages;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class liteChannelSettings : DomainBaseObject<long>, IDisposable
    {
        public virtual long IdVersion { get; set; }
        public virtual lm.Comol.Core.Notification.Domain.NotificationChannel Channel { get; set; }
        public virtual MessageSettings MailSettings { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public liteChannelSettings()
        {
            Channel = Notification.Domain.NotificationChannel.Mail;
            MailSettings = new MessageSettings();
            MailSettings.CopyToSender = false;
            MailSettings.NotifyToSender = false;
            MailSettings.IsBodyHtml = true;
            MailSettings.PrefixType = MailCommons.Domain.SubjectPrefixType.SystemConfiguration;
            MailSettings.SenderType = MailCommons.Domain.SenderUserType.System;
            MailSettings.Signature = MailCommons.Domain.Signature.FromConfigurationSettings;
            IsEnabled = false;
        }
        public void Dispose()
        {
  
        }
    }
}