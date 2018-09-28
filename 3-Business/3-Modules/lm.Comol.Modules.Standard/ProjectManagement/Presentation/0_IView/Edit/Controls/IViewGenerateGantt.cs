using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewGenerateGantt : IViewPageBaseEdit
    {
        String PreloadFormatDateString { get; }
        Dictionary<GanttCssClass, String> GetActivityCssClass();
        void GenerateXML(ProjectForGantt project);
    }
}