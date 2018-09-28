using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain; 

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectResources : IViewBaseEdit
    {
        String UnknownUser { get;}
        Boolean AllowAddExternalUser { get; set; }
        Boolean AllowAddCommunityUser { get; set; }
        Boolean AllowSave { get; set; }

        ////int IdCommunity { get; set; }
        
        //List<Int32> SelectedIdUsers { get; set; }
        //List<Int32> GetCurrentSelectedUsers();


        void InitializeControlToAddUsers(Int32 idCommunity, List<Int32> hideUsers);
        void InitializeControlToAddExternalResource();
        List<dtoProjectResource> GetResources();
        void LoadResources(List<dtoProjectResource> resources);
        void DisplayResourceAdded(Int32 added,Int32 multipleLongName = 0, Int32 multipleShortName = 0);
        void DisplaySavedSettings(Int32 multipleLongName = 0, Int32 multipleShortName = 0);
        void DisplayRemovedResource(String name);
        void DisplayUnableToRemoveResource(String name);
        void DisplayUnableToRemoveResource(long idProject, long idResource,  String name, long assignedTasks, long completedTasks);
        void DisplayErrors(Int32 multipleLongName, Int32 multipleShortName);
        void DisplayUnableToAddExternalResource();
        void DisplayUnableToAddInternalResource();
        void DisplayUnableToAddInternalResources();
        void DisplayUnableToSaveResources();
        void DisplayNoResources();
        void DisplayUnknownProject();
    }
}