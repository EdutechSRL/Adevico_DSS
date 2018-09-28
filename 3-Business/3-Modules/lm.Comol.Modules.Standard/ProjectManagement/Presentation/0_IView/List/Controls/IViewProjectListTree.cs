using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectListTree : IViewProjectListBase
    {
        #region "Context"
            Int32 CurrentPageIndex { get; }
            lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
            Int32 CurrentPageSize { get; set; }
            ItemsGroupBy CurrentGroupBy { get; set; }
        #endregion
        Dictionary<Int32, String> GetMonthNames();
        String GetDateTimePattern();
        String GetPortalName();
        String GetUnknownCommunityName();
        String GetContainerCssClass(ItemsGroupBy gType);
        String GetStartRowId(ItemsGroupBy gType);
        Int32 GetCellsCount(PageListType view);
        Dictionary<TimeGroup,String> GetTimeTranslations();

        void InitializeControl(dtoProjectContext context, dtoItemsFilter filter, PageListType pageType);
        void DisplayPager(Boolean display);
        void LoadedNoProjects(PageListType cPagetype);
        void LoadProjects(List<dtoProjectsGroup> pGroups, PageListType pageType);
    }
}