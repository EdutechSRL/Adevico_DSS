using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewFieldsMatcher : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean isValid { get; }
        Boolean AddTaxCode { get; }
        Boolean AddPassword { get; set; }
        Boolean AutoGenerateLogin { get; set; }
        Int32 SelectedProfileTypeId { get; set; }
        long SelectedIdProvider { get; set; }
        List<ProfileColumnComparer<String>> Fields { get; set; }

        dtoImportSettings ImportSettings { get; set; }
        List<ProfileColumnComparer<String>> Columns { get; set; }

        void InitializeControl(List<ProfileColumnComparer<String>> columns);
        void InitializeControl(List<dtoBaseProvider>providers);
        void LoadItems(List<ProfileColumnComparer<String>> source, List<dtoProfileAttributeType> attributes);
        void ReloadAvailableAttributes(List<ProfileColumnComparer<String>> columns,List<dtoProfileAttributeType> items);
        void DisplaySessionTimeout();
    }
}