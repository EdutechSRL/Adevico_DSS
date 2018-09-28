using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation 
{
    public interface IViewAddExternalResources : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 Rows { get; set; }
        Boolean isInitialized { get; set; }
        Boolean AllowMail { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean RaiseCommandEvents { get; set; }
        void InitializeControl(String description="");
        void DisplayRows(Int32 rows, Boolean displayMail, Boolean editing = true);
        List<ProjectResource> AddResources(long idProject, List<dtoExternalResource> items, ProjectVisibility visibility, ActivityRole role);
        List<dtoExternalResource> GetResources();
    }
}