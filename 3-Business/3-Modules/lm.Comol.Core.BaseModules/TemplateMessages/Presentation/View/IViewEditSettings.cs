using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewEditSettings : IViewBaseEditSetting 
    {
        long PreloadIdTemplate { get; }
        long PreloadIdVersion { get; }
        Boolean PreloadPreview { get; }
        Boolean AllowSaveDraft { get; set; }
        Boolean AllowSave { get; set; }
        Boolean AllowEditModuleContent { get; set; }
        Boolean IsTemplateAdded { get; }
        Boolean InputReadOnly { get; set; }

        Dictionary<String, List<String>> GetOldContentPlaceHolders(List<string> modulesCodes);
        Dictionary<String, List<String>> InUsePlaceHolders { get; set; }
        Boolean SavingSettings { get; set; }
        TemplateStatus SavingStatus { get; set; }
        long IdTemplate { get; set; }
        long IdVersion { get; set; }

        void HideUserMessage();
        void DisplayUnknownTemplate();
        void DisplayTemplateAdded();
        void DisplayTemplateSettingsSaved();
        void DisplayTemplateSettingsErrors();
        void DisplayTemplateSettingDeleted();
        void DisplayTemplateSettingErrorDeleting();
        void DisplayContentModulesSaved();
        void DisplayContentModulesErrorSaving();
        void DisplayConfirmModules(List<string> modulesCodes, Dictionary<String, List<String>> placeHolders);
        
        void LoadContentModules(List<String> selectedModules);
        void DisplayInput(String name, Int32 versionNumber, TemplateStatus status, List<String> availableModules, List<String> selectedModules, List<lm.Comol.Core.Notification.Domain.NotificationChannel> channels);
        void DisplayInput(String name, Int32 versionNumber, TemplateStatus status);
    }
}