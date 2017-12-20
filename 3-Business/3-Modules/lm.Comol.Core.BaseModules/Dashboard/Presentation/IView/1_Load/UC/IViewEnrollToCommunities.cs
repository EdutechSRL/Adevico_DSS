using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewEnrollToCommunities : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            String AutoDisplayTitle { get; set; }
            Boolean CurrentAscending { get; set; }
            OrderItemsToSubscribeBy CurrentOrderBy { get; set; }
            lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
            Int32 CurrentPageSize { get; set; }
            Int32 DefaultPageSize { get; set; }
            RangeSettings DefaultRange { get; set; }
            Boolean IsInitialized { get; set; }
            Boolean FirstLoad { get; set; }

            Int32 CurrentPageIndex { get; }
            Int32 IdCurrentCommunityType { get; set; }
            String TitleCssClass { get; set; }
            String TitleImage { get; set; }
            Boolean KeepOpenBulkActions { get; set; }
            List<dtoCommunityToEnroll> CurrentSelectedItems { get; set; }
        #endregion

        void DeselectAll();
        List<dtoCommunityToEnroll> GetSelectedItems();

        void SetListTitle(String name, liteTile tile);
        void LoadDefaultFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters);
        List<searchColumn> AvailableColumns { get; set; }
        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters CurrentFilters { get; set; }

        void InitializeControl(Int32 itemsForPage, RangeSettings range, Int32 preloadIdCommunityType, String searchText, Boolean preloadList);

        void DisplaySessionTimeout();
        void DisplayFilters(Boolean show);
        void InitializeOrderBySelector(List<dtoItemFilter<OrderItemsToSubscribeBy>> items);
        void DisplayErrorFromDB();
        void InitializeBulkActions(Boolean hasMultiPage,List<dtoCommunityToEnroll> items);

        void LoadItems(List<dtoEnrollingItem> items, OrderItemsToSubscribeBy orderBy, Boolean ascending);
        void DisplayNoCommunitiesToEnroll(String name);

        void DisplayUnknownCommunity(String name);
        //void DisplayUnableToEnroll(String community="");
        //void DisplayEnrolledInto(String community);


        /// <summary>
        /// Display message about one community enrollment
        /// </summary>
        /// <param name="item"></param>
        /// <param name="action"></param>
        void DisplayEnrollMessage(dtoCommunityInfoForEnroll item, ModuleDashboard.ActionType action);
        /// <summary>
        /// Display message error/warning/success for n-items
        /// </summary>
        /// <param name="item"></param>
        /// <param name="action"></param>
        void DisplayEnrollMessage(Int32 item, ModuleDashboard.ActionType action);
        /// <summary>
        /// Display success message about enrollment to one community, and take care about notification to users
        /// </summary>
        /// <param name="item"></param>
        /// <param name="idCommunity"></param>
        /// <param name="person"></param>
        /// <param name="profileType"></param>
        /// <param name="organization"></param>
        void DisplayEnrollMessage(dtoEnrollment item, Int32 idCommunity, litePerson person, String profileType, String organization);
        /// <summary>
        /// Display message error/warning/success for enrolled items
        /// </summary>
        /// <param name="enrolledItems"></param>
        void DisplayEnrollMessage(List<dtoEnrollment> enrolledItems, List<String> notEnrolledCommunities = null);

        void DisplayConfirmMessage(dtoCommunityInfoForEnroll item);
        void DisplayConfirmMessage(List<dtoEnrollment> enrolledItems, List<dtoEnrollment> notEnrolledItems, List<dtoCommunityInfoForEnroll> enrollingCommunities, litePerson person, String profileType, String organization);
        void NotifyEnrollment(dtoEnrollment item, litePerson person, String profileType, String organization);

        void RemoveFromSelectedItems(List<Int32> idCommunities);
        void SendUserAction(int idCommunity, int idModule, ModuleDashboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, int idActionCommunity, ModuleDashboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, List<Int32> idCommunities, ModuleDashboard.ActionType action);
    }
}