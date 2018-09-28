using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewConfirmResourceToRemove : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long IdProject { get; set; }
        long IdResource { get; set; }
        Boolean isInitialized { get; set; }
        Boolean AllowDelete { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean RaiseCommandEvents { get; set; }
        String UnknownUser { get; }
        String ResourceName { get; set; }
        void InitializeControl(long idProject, long idResource, String resourceName,long assignedTasks, long completedTasks, String description = "");
        void InitializeControl(long idProject, long idResource, String description="");
        void SetDescription(String description, long assignedTasks, long completedTasks);
        void LoadAvailableActions(List<RemoveAction> actions,RemoveAction selected);
        void DisplaySessionTimeout();
        void DisplayUnknownResource(String name ="");
        void NoPermissionToRemoveResource(String name);
        RemoveAction GetSelectedAction();
        Boolean DeleteResource(RemoveAction action);
    }
}