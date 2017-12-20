using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewBaseModuleMessage : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// code
        /// </summary>
        String PreloadModuleCode {get;}
        long PreloadFromModulePermissions { get; }
        /// <summary>
        /// tab
        /// </summary>
        DisplayTab PreloadSelectedTab {get;}
        /// <summary>
        /// tabs
        /// </summary>
        DisplayTab PreloadTabs {get;}
        /// <summary>
        /// sl
        /// </summary>
        UserSelection PreloadSelectionMode {get;}
        /// <summary>
        /// dtsl
        /// </summary>
        Boolean  PreloadAllowSelectTemplate {get;}
        long PreloadIdTemplate {get;}
        long PreloadIdVersion {get;}
        /// <summary>
        /// back
        /// </summary>
        String  PreloadBackUrl {get;}
        /// <summary>
        /// action
        /// </summary>
        long PreloadIdAction {get;}
        Boolean PreloadWithEmptyActions { get; }
        Int32 PreloadIdCommunity {get;}
        Int32 PreloadIdModule { get; }

        /// <summary>
        /// type
        /// </summary>
        TemplateType PreloadTemplateType { get; }
        lm.Comol.Core.DomainModel.ModuleObject PreloadModuleObject { get; }
        

        String CurrentModuleCode { get; set; }
        DisplayTab SelectedTab { get; set; }
        DisplayTab AvailableTabs { get; set; }
     
        System.Guid CurrentSessionId { get; set; }
        Int32 CurrentIdCommunity { get; set; }
        Int32 CurrentIdModule { get; set; }
       
        lm.Comol.Core.DomainModel.ModuleObject CurrentModuleObject { get; set; }

        String GetEncodedBackUrl(String url);


        ModuleGenericTemplateMessages GetModulePermissions(string moduleCode, Int32 idModule,long permissions, Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);
        void SetBackUrl(String url,System.Guid sessionId, String currentUrl);
        void DisplaySessionTimeout(int idCommunity, String url);
        void DisplayNoPermission(int idCommunity, int idModule);
        void DisplayNoPermission(int idCommunity, int idModule, String moduleCode);
        void LoadTabs(List<DisplayTab> tabs, DisplayTab selected);
    }
}