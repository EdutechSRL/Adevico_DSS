using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewBaseStatistics : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Int32 PreloadIdCommunity { get; }
        long PreloadIdPath { get; }
        DateTime? PreloadDateToView { get; }
        SummaryType PreloadFromSummary { get; }
        SummaryType PreloadFromSummaryIndex { get; }
        Int32 PreloadFromSummaryIdCommunity { get; }

        Int32 CurrentPathIdCommunity { get; set; }
        long CurrentIdPath { get; set; }
        DateTime? CurrentDateToView { get; set; }
        SummaryType CurrentFromSummary { get; set; }
        SummaryType CurrentSummaryIndex { get; set; }
        Int32 CurrentFromSummaryIdCommunity { get; set; }

        String CookieName { get; set; }
        String DisplayMessageToken { get; set; }
        String DisplayTitleToken { get; set; }


        String SetBackToSummaryUrl(String url);

        //void DisplayWrongPageAccess();
        void DisplayNoPermission();
        void DisplaySessionTimeout();

        void SendUserAction(int idCommunity, int idModule, ModuleEduPath.ActionType action);
    }
}