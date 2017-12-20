using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewSelectUsers : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Preview"
            Int32 DefaultMaxPreviewItems { get; set; }
            Int32 SystemMaxGridItems { get; set; }
            Boolean ShowItemsExceeding { get; set; }
            List<Int32> TemporaryIdUsers { get; set; }
            List<dtoSelectItem<Int32>> GetTemporarySelectedItems();
            Int32 TemporaryItemsCount { get; set; }
            Boolean AllowSelectAllFromPreview { get; set; }
            Boolean IsFirstPreviewLoad { get; set; }
            PagerBase PreviewPager { get; set; }
            String SearchPreviewValue { get; set; }
            String SearchPreviewLetter { get; set; }
        #endregion
      
        Boolean DisplayDescription { get; set; }
        Boolean isInitialized { get; set; }
        UserSelectionType SelectionMode { get; set; }
        Boolean RaiseUserChangedEvent { get; set; }
        Boolean RaiseCommandEvents { get; set; }
        //Boolean AllowAutoupdate { get; }
        Boolean AllowSearchByTaxCode { get; }
        Boolean AllowSearchByAgency { get; set; }
        Boolean ShowSubscriptionsFilterByProfile { get; set; }
        Boolean ShowSubscriptionsProfileTypeColumn { get; set; }
        Boolean OrderAscending { get; set; }
        Boolean FromAllMyCommunity { get; set; }
        List<Int32> FromCommunities { get; set; }
        Boolean MultipleSelection { get; set; }
        
        //List<Int32> ExcludeUsers { get; set; }
        List<Int32> SelectedIdUsers { get; set; }
        List<Int32> UnavailableIdUsers { get; set; }
        //Dictionary<Boolean, List<Int32>> GetCurrentSelection();

        //List<dtoCommunityPlain> GetSelectedItems();

        Boolean DisplayHeaderSelectAll { get; set; }

        Boolean HasAvailableUsers { get; set; }


        int DefaultPageSize { get; set; }
        int CurrentPageSize { get; }

        long SelectedIdAgency { get; set; }
        String CurrentValue { get; set; }
        String CurrentStartWith { get; set; }
        //Int32 CurrentPageSize { get; set; }
        PagerBase Pager { get; set; }

        List<ProfileColumn> AvailableColumns { get; set; }
        List<dtoSelectItem<Int32>> GetCurrentSelectedItems();
        List<Int32> GetSelectedUsers();
        
        Boolean IsFirstLoad { get; set; }
       
        void NoPermission();
        void DisplaySessionTimeout();
        //void DisplayNoItems();

        #region "Common filters"
            Boolean SelectAllUsers { get; set; }
            SearchProfilesBy SelectedSearchBy { get; set; }
            Int32 SelectedIdProfileType { get; set; }
            void LoadProfileTypes(List<Int32> idProfileTypes, Int32 idDefaultType);
            void LoadAgencies(Dictionary<long, String> items, long idDefaultAgency, UserSelectionType mode);
            void UnLoadAgencies();
            void LoadSearchProfilesBy(List<SearchProfilesBy> list, SearchProfilesBy defaultSearch,UserSelectionType mode);
            
            void InitializeWordSelector(List<String> availableWords);
            void InitializeWordSelector(List<String> availableWords, String activeWord);
        #endregion

        void InitializeControl(UserSelectionType mode, Boolean multipleSelection, Boolean fromAllMyCommunity, List<Int32> unloadIdUsers = null, List<Int32> selectIdUsers = null, String description = "");
        void InitializeControl(UserSelectionType mode, Boolean multipleSelection, Int32 idCommunity, List<Int32> unloadIdUsers = null, List<Int32> selectIdUsers = null, String description = "");
        void InitializeControl(UserSelectionType mode, Boolean multipleSelection, List<Int32> idCommunities, List<Int32> unloadIdUsers = null, List<Int32> selectIdUsers = null, String description = "");
        void InitializeControlForSingleSelection(UserSelectionType mode, Boolean fromAllMyCommunity, List<Int32> unloadIdUsers = null, Int32 selectedIdUser = 0, String description = "");
        void InitializeControlForSingleSelection(UserSelectionType mode, Int32 idCommunity, List<Int32> unloadIdUsers = null, Int32 selectedIdUser = 0, String description = "");
        void InitializeControlForSingleSelection(UserSelectionType mode, List<Int32> idCommunities, List<Int32> unloadIdUsers = null,Int32 selectedIdUser=0, String description = "");

        void DisplayUsersPreview(List<dtoBaseProfile> items, Int32 itemsCount);
        void DisplayStartUsersPreview(List<dtoBaseProfile> items, List<String> availableWords, Int32 itemsCount);
        void HideUsersPreview();

        #region "Subscriptions"
            SubscriptionStatus SelectedSubscriptionStatus { get; set; }
            OrderUsersBy OrderUsersBy { get; set; }
            Int32 SelectedIdRole { get; set; }
            List<TranslatedItem<Int32>> GetTranslatedRoles { get; }
            void LoadRolesTypes(List<Int32> idRoles, Int32 idDefaultRole);

            dtoUserFilters GetCurrentUserFilters { get; }
            dtoUserFilters SearchUserFilters { get; set; }

            void LoadSubscriptions(List<dtoSubscriptionProfileItem<dtoBaseProfile>> items);
            void LoadSubscriptions(List<dtoSubscriptionProfileItem<dtoCompany>> items);
            void LoadSubscriptions(List<dtoSubscriptionProfileItem<dtoEmployee>> items);
            void LoadAvailableSubscriptionsStatus(List<SubscriptionStatus> list, SubscriptionStatus defaultStatus);
        #endregion
        #region "Profiles"
            StatusProfile SelectedProfileStatus { get; set; }
            OrderProfilesBy OrderProfilesBy { get; set; }
            Int32 SelectedIdOrganization { get; set; }
           
            long SelectedIdProvider { get; set; }
            List<Int32> AvailableOrganizations { get; set; }
            List<TranslatedItem<Int32>> GetTranslatedProfileTypes { get; }
            void LoadAvailableStatus(List<StatusProfile> list, StatusProfile defaultStatus);
            void LoadAvailableOrganizations(List<Organization> items, Int32 idDefaultOrganization);
            
            void LoadAuthenticationProviders(List<dtoBaseProvider> providers, long IdDefaultProvider);

            dtoFilters GetCurrentProfileFilters { get; }
            dtoFilters SearchProfileFilters { get; set; }

            void LoadProfiles(List<dtoProfileItem<dtoBaseProfile>> items);
            void LoadProfiles(List<dtoProfileItem<dtoCompany>> items);
            void LoadProfiles(List<dtoProfileItem<dtoEmployee>> items);
        #endregion

        void SelectAllItems();
    }
}