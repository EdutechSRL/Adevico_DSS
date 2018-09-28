using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectsList : IViewBaseList
    {
        void InitializeTopControls(dtoProjectContext context, Int32 idContainerCommunity, Boolean loadFromCookies, TabListItem tab, PageContainerType contanier, ItemsGroupBy dGroupBy = ItemsGroupBy.None, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemListStatus projectsStatus = ItemListStatus.All, ItemListStatus activitiesStatus = ItemListStatus.All, SummaryTimeLine timeline = SummaryTimeLine.Week, SummaryDisplay display = SummaryDisplay.All);
        void AllowAddProject(String personalUrl, String publicUrl = "", String personalPortalUrl ="");
    }
}