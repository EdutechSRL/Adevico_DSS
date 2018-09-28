using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectDateInfo : IViewBase
    {
        #region "Context"
            DateTime? PRstartDate { get; set; }
            DateTime? PRendDate { get; set; }
            DateTime? PRdeadline { get; set; }
            DateTime? InEditStartDate { get; }
            DateTime? InEditDeadline { get; }
            Boolean AllowEdit { get; set; }
            String CurrentShortDatePattern { get; set; }
            dtoWorkingDay DefaultWorkingDay { get; set; }
            System.Globalization.CultureInfo LoaderCultureInfo { get; set; } 
        #endregion

        void InitializeControl(long idProject);
        void InitializeControl(long idProject, Boolean allowEdit);
        void InitializeControl(dtoProject project, System.Globalization.CultureInfo culture, String currentShortDatePattern, Boolean allowEdit);
        void LoadProjectInfo(dtoProject project, Boolean setCulture);
        void DisplayUnknownProject();
    }
}