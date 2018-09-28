using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewBaseSummary : IViewBaseEduPath
    {
        Int32 PreloadIdCommunity { get; }
        Int32 SummaryIdCommunity { get; set; }
    }
}