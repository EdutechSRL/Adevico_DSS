using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.TemplateMessages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewTemplateSelector : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean AllowPreview { set; }
        Boolean isInitialized { get; set; }
        Boolean isInAjaxPanel { get; set; }
        Boolean AllowSelect { get; set; }
        Boolean RaiseSelectionEvent { get; set; }
        //String DestinationUrl { get; }
        dtoSelectorContext SelectorContext { get; set; }
        lm.Comol.Core.Notification.Domain.NotificationChannel Channel { get; set; }

        dtoVersionItem SelectedItem { get; }
        List<dtoTemplateItem> Items { get; set; }

        void InitializeControl(ModuleGenericTemplateMessages permissions, lm.Comol.Core.Notification.Domain.NotificationChannel channel, dtoSelectorContext sContext, long idTemplate, long idVersion, List<dtoTemplateItem> items = null);
        void InitializeControl(ModuleGenericTemplateMessages permissions, lm.Comol.Core.Notification.Domain.NotificationChannel channel, long idAction, Int32 idModule, String moduleCode, Int32 idCommunty, Int32 idOrganization = 0, Boolean forPortal = false, long idTemplate = 0, long idVersion = 0, lm.Comol.Core.DomainModel.ModuleObject source = null, List<dtoTemplateItem> items = null);

        void LoadTemplates(List<dtoTemplateItem> templates);
        void LoadEmptyTemplate();
        void DisplaySessionTimeout();
    }
}