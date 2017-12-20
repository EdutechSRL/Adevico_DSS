using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewMacUrlProviderSettings : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long PreloadedIdProvider { get; }
        UrlMacAttributeType PreloadedAttributeType { get; }
        long PreloadedIdInEditingAttribute { get; }

        Boolean AllowManagement { set; }
        Boolean AllowEdit { get; set; }
        UrlMacAttributeType CurrentAttributeToAdd { get; }

        long IdProvider { get; set; }
        long IdAttributeEditing { get; set; }
        void LoadProviderInfo(dtoMacUrlProvider provider);
        void LoadAttributes(List<BaseUrlMacAttribute> items);
        void LoadAvailableTypes(List<UrlMacAttributeType> types, UrlMacAttributeType selected);
      
        void DisplaySessionTimeout();
        void DisplayProviderUnknown();
        void DisplayDeletedProvider(String name, AuthenticationProviderType type);
        void DisplayNoPermission();
        void DisplayAttributeAdded();
        void DisplayAttributeNotAdded();
        void DisplayAttributeNotDeleted();
        void DisplayAttributeOptionNotDeleted();
        void DisplayAttributeOptionNotAdded();
        void DisplayDuplicatedRemoteCode();
        void DisplayInvalidAttribute();
        void DisplayInvalidAttributeAndRemoteCode();
        void GotoManagement();
        void GotoUrl(String url);
    }
}