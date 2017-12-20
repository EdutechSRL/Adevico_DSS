using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewConfirmUnsubscription : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Int32 IdCommunity { get; set; }
        String Path { get; set; }
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean RaiseCommandEvents { get; set; }
        //void InitializeControl(Int32 idCommunity, String path, List<dtoUnsubscribeTreeNode> communities, List<RemoveAction> actions, RemoveAction selected, String description = "");
        void InitializeControl(Int32 idCommunity, String path, dtoUnsubscribeTreeNode community, List<RemoveAction> actions, RemoveAction selected, List<dtoUnsubscribeTreeNode> alsoFromCommunities = null, String description = "");
//        void SetDescription(String description, long assignedTasks, long completedTasks);
        void LoadAvailableActions(List<RemoveAction> actions, RemoveAction selected);
        //void DisplayUnknownResource(String name = "");
        //void NoPermissionToRemoveResource(String name);
        RemoveAction GetSelectedAction();
        //Boolean DeleteResource(RemoveAction action);
    }
}