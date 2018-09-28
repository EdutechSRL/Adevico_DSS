using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewBaseProjectMap : IViewPageBaseEdit
    {
        #region "Context"
            String UnknownUser { get; }
            String CurrentShortDatePattern { get; }
        #endregion
        #region "Common"
            void LoadProjectDateInfo(dtoProject project, Boolean allowUpdate);
            void DisplayUnknownProject();
            void RedirectToUrl(String url);
            void SetProjectsUrl(String url);
            void SetEditProjectUrl(String url);
            void SetEditMapUrl(String url);
        
            void SetDashboardUrl(String url, PageListType dashboard);
        #endregion   
       
       
        void DisplaySessionTimeout();
        void LoadAttachments(List<dtoAttachmentItem> attachments);
    }
}