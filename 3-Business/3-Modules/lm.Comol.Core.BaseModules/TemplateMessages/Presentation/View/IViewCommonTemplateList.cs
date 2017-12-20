using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.TemplateMessages;
using System.Collections.Generic;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewCommonTemplateList : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        String UnknownUserName { get; }
        System.Guid CurrentSessionId { get; set; }
        Boolean SendTemplateActions { get; set; }
        Boolean RaiseApplyFiltersEvent { get; set; }
        Boolean RaisePageChangedEvent { get; set; }
        Boolean RaiseSessionTimeoutEvent { get; set; }
        Boolean AllowAdd { get; set; }
        long OpenIdTemplate { get; set; }
        PagerBase Pager { get; set; }
        dtoModuleContext ContainerContext { get; set; }

        Int32 CurrentPageSize { get; set; }
        dtoBaseFilters CurrentFilters { get; set; }
        TemplateOrder CurrentOrderBy { get; set; }
        Boolean CurrentAscending { get; set; }
        TemplateDisplay CurrentDisplay { get; set; }
           
        List<TemplateType> AvailableTypes { get; set; }
        List<TemplateDisplay> AvailableDisplay { get; set; }
        
        TemplateType CurrentType { get; set; }

        void InitializeControl(dtoModuleContext containerContext, dtoBaseFilters filter, List<TemplateType> availableType, List<TemplateDisplay> availableDisplay, System.Guid sessionId, long openIdTemplate = 0, Boolean displayAdd = false, String addUrl = "", String addPersonalUrl = "", String objectUrl="", String title = "");
        //void LoadTemplateTypes(List<TemplateType> types, TemplateType selected = TemplateType.None);
        void LoadTemplateDisplay(List<TemplateDisplay> types, TemplateDisplay selected =  TemplateDisplay.None);
        void LoadNoTemplatesFound();
        void LoadTemplates(List<dtoDisplayTemplateDefinition> templates);
        void ReloadPageAndFilters(dtoBaseFilters filter, List<TemplateDisplay> types, TemplateDisplay selected = TemplateDisplay.None);
        void DisplaySessionTimeout();
        void DisplayNoPermission(Int32 idCommunity, Int32 idModule);
        void DisplayNoPermission();
        void DisplayMessage(String name, lm.Comol.Core.BaseModules.TemplateMessages.Domain.ListAction action, ModuleTemplateMessages.ObjectType obj, Boolean completed);


        void SetAddUrl(String url);
        void SetAddUrl(String url, String personalUrl,String objectUrl);
        void SendUserAction(int idCommunity, int idModule, ModuleTemplateMessages.ActionType action);
    }
}