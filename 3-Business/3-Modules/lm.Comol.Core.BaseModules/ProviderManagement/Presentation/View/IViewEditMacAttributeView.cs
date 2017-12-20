using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public interface IViewEditMacAttribute : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long IdAttribute { get; set; }
        long IdProvider { get; set; }
        UrlMacAttributeType AttributeType { get; set; }
        Boolean isInitialized { get; set; }
        Boolean AllowSave { get; set; }
        Boolean DisplayMode { get; set; }
        BaseUrlMacAttribute GetAttribute { get; }
        List<lm.Comol.Core.DomainModel.TranslatedItem<lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType>> TranslatedAttributes { get; set; }
        List<lm.Comol.Core.DomainModel.TranslatedItem<Int32>> TranslatedProfileTypes { get; set; }
        Dictionary<long, String> AvailablePages { get; set; }
        Boolean ValidateContent();

        void InitializeDisplayControl(long idProvider, BaseUrlMacAttribute attribute, Boolean editAvailable);
        void InitializeEditControl(long idProvider,BaseUrlMacAttribute attribute, Boolean saveEnabled);
        void LoadAvailableProfileAttributes(List<lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType> items);
        void LoadAttribute(BaseUrlMacAttribute attribute, Boolean allowSave);
        void LoadSystemProfileTypes();
        //void LoadSystemPage();

        void DisplaySessionTimeout();
        void DisplayAttributeUnknown();
        Boolean SaveAttribute(BaseUrlMacAttribute attribute, ref Boolean validCodes);
    }
}