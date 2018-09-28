using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
   
    public interface IViewSummaryIndex : IViewBaseSummary
    {
        SummaryType PreloadSummaryType { get; }
        SummaryType SummaryType { get; set; }

        void LoadAvailableItems(List<SummaryType> items);
    }
}