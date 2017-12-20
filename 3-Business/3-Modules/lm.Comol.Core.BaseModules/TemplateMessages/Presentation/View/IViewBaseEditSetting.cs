using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.TemplateMessages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewBaseEditSetting : IViewBase 
    {
        System.Guid PreloadCurrentSessionId { get; }
        Int32 PreloadFromIdCommunity { get; }
        String PreloadFromModuleCode { get; }
        long PreloadFromModulePermissions { get; }

        Boolean AllowSenderEdit { get; set; }
        Boolean AllowSubjectEdit { get; set; }
        Boolean AllowSignatureEdit { get; set; }
        String PreloadBackUrl { get; }
    
        Int32 IdTemplateCommunity { get; set; }
       
        Int32 IdTemplateModule { get; set; }
        String Portalname { get; }
        //Dictionary<String, List<long>> InUseNotificationAction { get; set; }
        List<dtoChannelConfigurator> ChannelSettings { get; set; }
        List<String> SelectedContentModules { get; }
        
        List<String> GetAvailableModules(List<String> removeModules= null);
        Dictionary<String, List<TranslatedItem<long>>> GetModuleCodesForNotification();

        //List<NotificationChannel> GetInUseNotificationChannels();
        List<dtoChannelConfigurator> GetChannelSettings();

        void LoadWizardSteps(int idCommunity, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> steps);
        void LoadChannelSettings(List<dtoChannelConfigurator> items);
        void LoadChannels(List<lm.Comol.Core.Notification.Domain.NotificationChannel> channels);

       
        void DisplayTemplateUnableToAddNotificationChannel();
        void UnableToReadUrlSettings();
        void SetBackUrl(String url);
        Boolean HasPermissionForObject(ModuleObject source);
        ModuleGenericTemplateMessages GetModulePermissions(string moduleCode, Int32 idModule, long permissions, Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);
        void GoToUrl(string url);
        String GetEncodedBackUrl();
    }
}