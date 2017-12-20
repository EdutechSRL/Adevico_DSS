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
    public interface IViewItemsSelector : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean AddTaxCode { get; }
        Boolean AutoGenerateLogin { get; set; }
        String TranslatedAutoLogin { get; }
        ProfileExternalResource SelectedItems { get; }
        Boolean HasSelectedItems { get; }
        Int32 ItemsToSelect { get; }
        Int32 SelectedItemsCount { get; }
        List<Int32> SelectedRows { get; set; }
        void InitializeControl(ProfileExternalResource source, dtoImportSettings settings);
        void InitializeControlAfterImport(ProfileExternalResource source, dtoImportSettings settings);
        void LoadItems(List<ProfileExternalResource> items, List<InvalidImport> invalidItems, Int32 idProfileType,string providerName);
        void DisplaySessionTimeout();
    }
}