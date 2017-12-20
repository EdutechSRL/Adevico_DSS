using lm.Comol.Core.Tag.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public interface IViewTagsSelectorForCommunity : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            Int32 IdCommunityToApply { get; set; }
            List<Int32> IdOrganizations { get; set; }
        #endregion
        Boolean HasAvailableTags();
        List<long> GetSelectedTags();

        void InitializeControlForCommunity(Int32 idCommunity);
        void InitializeControlForCommunityToAdd(Int32 idFatherCommunity, Int32 idCommunityType);
        void InitializeControlForOrganization(Int32 idOrganization, Int32 idCommunityType=-1);
        void InitializeControl(List<dtoTagSelectItem> tags, Int32 idOrganization = 0, Int32 idCommunity = 0);
       
        void LoadTags(List<dtoTagSelectItem> tags);

        void ApplyTags();
        void ApplyTags(Int32 idCommunity);
        void ApplyTags(List<Int32> idCommunities);
        void ReloadTags(Int32 idCommunityType);
        void ReloadTags(Int32 idCommunityType, Int32 idOrganization);
        void DisplaySessionTimeout();
        void DisplayMessage(ModuleTags.ActionType action);
        
    }
}