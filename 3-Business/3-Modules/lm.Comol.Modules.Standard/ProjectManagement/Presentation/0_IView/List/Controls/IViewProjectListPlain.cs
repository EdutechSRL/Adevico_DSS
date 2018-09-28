using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectListPlain : IViewProjectListBase
    {
        #region "Context"
            Int32 CurrentPageIndex { get; }
            lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
            Int32 CurrentPageSize { get; set; }
        #endregion
        void InitializeControl(dtoProjectContext context, dtoItemsFilter filter,  PageListType pageType);
        void DisplayPager(Boolean display);
        void LoadedNoProjects(PageListType cPagetype);
        void LoadProjects(List<dtoPlainProject> projects, PageListType pageType);
        void LoadAttachments(List<dtoAttachmentItem> items);
    }
}