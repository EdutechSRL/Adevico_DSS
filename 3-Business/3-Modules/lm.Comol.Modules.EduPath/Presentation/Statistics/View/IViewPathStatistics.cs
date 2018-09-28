using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewPathStatistics : IViewBaseStatistics
    {
        Boolean IsEvaluable { get; }
        Boolean DisplayHiddenUsers { get; set; }
        String SetPathListUrl(String url);
        String SetViewPathUrl(String url);

        void InitalizeTimePiker(DateTime selectedDate, String displayEndDate, String overTime, Boolean viewSelector, Boolean isManageMode, DateTime? endDate);
        void LoadStatistics(dtoEpGlobalStat statistic);
    }
}