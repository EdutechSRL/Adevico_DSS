using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewNotificationSettings : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean PreloadForPortal { get; }
        Int32 PreloadIdCommunity { get; }
        Int32 PreloadIdOrganization { get; }
        lm.Comol.Core.DomainModel.ModuleObject PreloadModuleObject { get; }
        Int32 SettingsIdCommunity { get; set; }
        Int32 SettingsIdOrganization { get; set; }
        lm.Comol.Core.TemplateMessages.Domain.TemplateLevel SettingsLevel { get; set; }
        lm.Comol.Core.DomainModel.ModuleObject SettingsObj { get; set; }
        Boolean SettingsForPortal { get; set; }
        Boolean SettingsForObject { get; set; }
        Dictionary<String, lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages> CurrentPermissions { get; set; }

        lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages GetModulePermissions(String moduleCode, long permissions, Int32 idCommunity, Int32 profileType);
        //Dictionary<String, long> GetModulePermissions(List<String> moduleCodes);
        List<String> GetAvailableModules(Boolean forPortal);
        Dictionary<String, String> GetTranslatedModules(List<String> codes);
        List<dtoModuleAction> GetTranslatedModuleActions(String code);
        List<dtoModuleEvents> SelectedItems { get; }

        void LoadItems(List<dtoModuleEvents> events);
        void LoadEmptyItems();
        void SendUserAction(int idCommunity, int idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType action);
        void DisplayNoPermission(Int32 idCommunity, Int32 idModule);
        void DisplaySessionTimeout();
        void DisplaySessionTimeout(String url);
        void DisplaySavedSettings(Int32 savedItems, Int32 unselectedItems, Int32 unsavedItems, Int32 inheritedItems, Int32 mandatoryMissing);
        void DisplayMandatoryMissing(Int32 items);
        void DisplaySavingErrors();
        void DisplayPortalInfo();
        void DisplayOrganizationInfo(String name);
        void DisplayCommunityInfo(String name);
        void DisplayObjectInfo(String moduleCode, Int32 idObjectType);
    }
}