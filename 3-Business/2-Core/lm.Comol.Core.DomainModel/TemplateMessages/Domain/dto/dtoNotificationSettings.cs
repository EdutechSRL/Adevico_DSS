using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoNotificationSettings: DomainBaseObject<long>
    {
        public virtual long IdVersion { get; set; }
        public virtual lm.Comol.Core.Notification.Domain.NotificationChannel Channel { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public dtoNotificationSettings()
        {
            Channel = lm.Comol.Core.Notification.Domain.NotificationChannel.Mail;
            MailSettings = new MailCommons.Domain.Messages.MessageSettings();
            MailSettings.CopyToSender = false;
            MailSettings.NotifyToSender = false;
            MailSettings.IsBodyHtml = true;
            MailSettings.SenderType = MailCommons.Domain.SenderUserType.System;
            MailSettings.PrefixType = MailCommons.Domain.SubjectPrefixType.SystemConfiguration;
            MailSettings.Signature = MailCommons.Domain.Signature.FromConfigurationSettings;
            IsEnabled = false;
        }

        public dtoNotificationSettings(ChannelSettings settings)
        {
            Id = settings.Id;
            Deleted = settings.Deleted;
            MailSettings = settings.MailSettings;
            IsEnabled = settings.IsEnabled;
            Channel = settings.Channel;
            IdVersion = (settings.Version == null) ? 0 : settings.Version.Id;
        }
        public void Dispose()
        {
  
        }
    }
}