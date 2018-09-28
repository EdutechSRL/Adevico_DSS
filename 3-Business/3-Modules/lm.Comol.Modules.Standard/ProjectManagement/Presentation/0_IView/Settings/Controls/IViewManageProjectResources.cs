using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewManageProjectResources : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 MaxDisplayUsers { get; }
        String SetMaxDisplayUsers { set; }
        Boolean DisplayDescription { get; set; }
        Boolean DisplayCommands { get; set; }
        Boolean RaiseCommandEvents { get; set; }

        List<dtoProjectResource> GetResources();
        void InitializeControl(long idProject, Boolean allowEdit, String unknownUser, String description ="");
        void InitializeControl(List<dtoProjectResource> resources, String description = "");
        void LoadResources(List<dtoProjectResource> resources); 
    }
}