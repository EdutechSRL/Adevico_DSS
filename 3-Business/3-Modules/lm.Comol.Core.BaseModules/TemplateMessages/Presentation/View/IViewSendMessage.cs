using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewSendMessage : IViewBaseModuleMessage
    {
        UserSelection SelectionMode { get; set; }
        Boolean AllowSelectTemplate { get; set; }
        long CurrentIdTemplate { get; set; }
        long CurrentIdVersion { get; set; }
        long CurrentIdAction { get; set; }
        Boolean AlsoWithEmptyActions { get; set; }
        long ModulePermissions { get; set; }
        TemplateType CurrentTemplateType { get; set; }

        Boolean IsInitializedList { get; set; }
        Boolean IsInitializedSender { get; set; }
        Boolean PreloadFromCookies { get; }
        dtoBaseFilters GetFromCookies();
        Dictionary<TemplateStatus, String> GetTranslationsStatus();
        Dictionary<TemplateType, String> GetTranslationsTypes();
        void SaveToCookies(dtoBaseFilters filter);
        void DisplayList();
        void InitializeList(dtoModuleContext containerContext, dtoBaseFilters filters, List<TemplateType> availableTypes, List<TemplateDisplay> availableDisplay, Boolean allowAdd, String addModuleUrl = "", String addPersonalUrl = "", String addObjectUrl = "");

      
        void DisplaySendMessage();
        //List<OwnerType> availableSaveAs,
        void InitializeSendMessage( Boolean allowSelectTemplate, Int32 containerIdCommunity, String currentCode = "", long idTemplate = 0, long idVersion = 0, long idAction = 0);
    }
}