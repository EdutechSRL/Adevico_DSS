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
    public interface IViewCommunityDetails : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void InitializeControl(liteCommunityInfo community);
        void LoadUserInfo(litePerson responsible, litePerson creator);
        void LoadTags(List<String> tags, Int32 idCommunityType);
        void LoadEnrollmentsInfo(liteCommunityInfo community,List<dtoEnrollmentsDetailInfo> items, long count, long waiting);
        void LoadConstraints(List<dtoCommunityConstraint> constraints);
        void LoadDetails(liteCommunityInfo community,String type, String description);
        void SendUserAction(int idCommunity, int idModule, int idActionCommunity, ModuleDashboard.ActionType action);
    }
}