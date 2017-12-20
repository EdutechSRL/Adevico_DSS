using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class CommonNotificationSettings: DomainBaseObjectLiteMetaInfo<long>, IDisposable
    {
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual TemplateDefinition Template { get; set; }
        public virtual Boolean AlwaysLastVersion { get; set; }
        public virtual TemplateDefinitionVersion Version { get; set; }
        public virtual NotificationSettings Settings { get; set; }
        public virtual Boolean IsForPortal {get;set;}
        public virtual Boolean IsEnabled { get; set; }
        public virtual ModuleObject ObjectOwner { get; set; }
       
        public CommonNotificationSettings()
        {
            Settings = new NotificationSettings();
            IsEnabled = true;
        }
        public Boolean IsValid()
        {
            return Settings.ActionType != lm.Comol.Core.Notification.Domain.NotificationActionType.Ignore && (Template != null && ((AlwaysLastVersion && IsValidForVersion(Template.GetActiveVersion())) || (Version != null && IsValidForVersion(Version))));
        }

        private Boolean IsValidForVersion(TemplateDefinitionVersion v)
        {
            return (v != null && v.ChannelSettings != null && v.ChannelSettings.Where(a => a.Deleted == BaseStatusDeleted.None).ToList().Where(a => lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)a.Channel, (long)Settings.Channel)).Any());
        }

        public void Dispose()
        {

        }

    }
}