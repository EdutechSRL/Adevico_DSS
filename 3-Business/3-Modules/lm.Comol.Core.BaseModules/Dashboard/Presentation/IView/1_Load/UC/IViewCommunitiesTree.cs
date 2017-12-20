using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation
{
    public interface IViewCommunitiesTree : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            Boolean IsInitialized { get; set; }
            Boolean IsFirstLoad { get; set; }
            Int32  ReferenceIdCommunity { get; set; }
            Boolean AdvancedMode { get; set; }
            lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability LoadMode { get; set; }
            lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters CurrentFilters { get; set; }
        #endregion


        void InitializeControl(Boolean advancedMode, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability availability = CommunityAvailability.Subscribed, Int32 idReferenceCommunity = 0, String referencePath="");

      
        //void ApplyFilters(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters);
        void SendUserAction(int idCommunity, int idModule, ModuleDashboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, int idActionCommunity, ModuleDashboard.ActionType action);
     
        void DisplaySessionTimeout();

        #region "Load items"
            List<Int32> GetIdcommunitiesWithNews(List<Int32> idCommunities, Int32 idUser);
            void LoadTree(List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityNodeItem> items);
            void LoadDefaultFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters);
            void DisplayErrorFromDB();
            void DisplayUnableToLoadTree();
            void DisplayNoTreeToLoad(String cName);
        #endregion

        #region "Manage enrollments"
            void DisplayUnableToUnsubscribe(String community);
            void DisplayUnsubscribedFrom(String community);
            void DisplayUnsubscriptionMessage(List<String> unsubscribedFrom, List<String> unableToUnsubscribeFrom);
            void DisplayConfirmMessage(Int32 idCommunity, String path, dtoUnsubscribeTreeNode community, List<RemoveAction> actions, RemoveAction selected, List<dtoUnsubscribeTreeNode> alsoFromCommunities = null);
            void DisplayUnsubscribeNotAllowed(String community);
        #endregion

        #region "Enroll to"
            void DisplayEnrollMessage(dtoEnrollment item, Int32 idCommunity, litePerson person, String profileType, String organization);
            void DisplayConfirmMessage(dtoCommunityInfoForEnroll item);
            void DisplayEnrollMessage(dtoCommunityInfoForEnroll item, ModuleDashboard.ActionType action);
            void NotifyEnrollment(dtoEnrollment item, litePerson person, String profileType, String organization);
            void DisplayUnknownCommunity(String name);
        #endregion
    }
  
}
