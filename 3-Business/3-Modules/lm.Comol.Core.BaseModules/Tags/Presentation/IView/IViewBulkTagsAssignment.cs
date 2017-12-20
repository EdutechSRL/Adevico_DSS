using lm.Comol.Core.Tag.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public interface IViewBulkTagsAssignment : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            Boolean PreloadFromPage  { get; }
            Int32 PreloadIdOrganization { get; }
            Boolean PreloadFromPortal { get; }
            Boolean PreloadAssigned { get; }
            lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
            Int32 CurrentPageSize { get; set; }
            Int32 CurrentPageIndex { get; }
            Int32 CurrentIdOrganization { get; set; }
            Int32 CurrentIdCommunity { get; set; }
           List<dtoBulkCommunityForTags> CurrentAssignedTags { get; set; }
           Boolean AllowSave { get; set; }
        #endregion

        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters CurrentFilters { get; set; }
        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters GetSubmittedFilters();
        void RedirectToUrl(String url);
        void SetBackUrl(String url);
        void LoadCommunities(List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityForTags> communities);
        void LoadDefaultTags(List<lm.Comol.Core.Tag.Domain.dtoTagSelectItem> tags, Boolean hasMultiPage);
        void DisplayErrorFromDB();
        void DisplayMessage(ModuleTags.ActionType action);
        void HideFilters();
        void DeselectAll();
        List<dtoBulkCommunityForTags> GetSelectedItems();
        List<Int32> GetSelectedCommunities();
        void LoadDefaultFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters);
        void DisplayNoCommunitiesToLoad();
        #region "Common"
            void DisplaySessionTimeout();
            void DisplaySessionTimeout(String url, int idCommunity);
            void DisplayNoPermission(int idCommunity, int idModule);
            void SendUserAction(int idCommunity, int idModule, ModuleTags.ActionType action);
        #endregion
    }
}