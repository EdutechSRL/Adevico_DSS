using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;
using lm.Comol.Core.TemplateMessages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewListTemplates : IViewBase
    {
        Boolean AllowAdd { get; set; }
        Int32 PreloadIdCommunity { get; }
        Boolean PreloadFromCookies { get; }
        String PreloadModuleCode { get; }
        long PreloadModulePermissions { get; }
        long PreloadIdTemplate { get; }

        dtoBaseFilters GetFromCookies();
        Dictionary<TemplateStatus, String> GetTranslationsStatus();
        Dictionary<TemplateType, String> GetTranslationsTypes();
                
        Int32 IdManagerCommunity { get; set; }
        String Portalname { get; }
        void SaveToCookies(dtoBaseFilters filter);
        //void InitializePersonalList(dtoBaseFilters filters, List<TemplateType> availableTypes, List<TemplateDisplay> availableDisplay);

        ModuleGenericTemplateMessages GetModulePermissions(string moduleCode, Int32 idModule,long permissions, Int32 idCommunity, Int32 profileType);
        void InitializeList(dtoModuleContext containerContext, dtoBaseFilters filters, List<TemplateType> availableTypes, List<TemplateDisplay> availableDisplay);
        void SetAddUrl(String url);
    }
}