using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewAdd :IViewBaseEditSetting  
    {
        Boolean AllowAdd { get; set; }
        void DisplayInput(Language lng, long tNumber, List<String> availableModules, List<lm.Comol.Core.Notification.Domain.NotificationChannel> channels);
        void DisplayInput(Language lng, long tNumber, List<lm.Comol.Core.Notification.Domain.NotificationChannel> channels);
       
        void DisplayAddUnavailable();
        void DisplayTemplateAddError();
        void DisplayChannelSettingsError();
    }
}