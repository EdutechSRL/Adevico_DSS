using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewDisplayGantt : IViewBase
    {
        String FormatDatePattern { get; set; }
        String GetDatePatternEncoded();
        void DisplayNoPermissionToSeeProjectGantt();
        void DisplayUnknownProject();
        void InitializeControl(long idProject, String formatDatePattern);
        void InitializeControl(dtoProject project, String formatDatePattern);

        void LoadGantt(String url);
    }
}