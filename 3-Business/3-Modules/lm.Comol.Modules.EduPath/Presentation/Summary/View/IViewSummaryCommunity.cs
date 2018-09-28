using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Presentation
{

    public interface IViewSummaryCommunity : IViewBaseSummary
    {
        Boolean PreloadReloadFilters { get; }
        SummaryType PreloadFromSummary { get; }
        SummaryType PreloadSummaryType { get; }
        SummaryType FromSummary { get; set; }
        SummaryType CurrentSummaryType { get; set; }

        lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
        Boolean AllowOrganizationSelection { get; set; }

        // Teoricamente andrebbero tutte le funzioni/metodi da gestione MVP, nel frattempo carica il vecchio bind dati della pagina...

        void LoadAvailableOrganizations(List<Organization> items);
        void InitializeFilters(SummaryType summary);
        void DisplayWrongPageAccess(String url);
        void LoadSummaryItems(SummaryType summary,Boolean initialize = false);
        //String GetFileName(lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType type);
        //List<dtoUserPathQuiz> GetQuizInfos(List<dtoUserPathQuiz> qInfos);
    }

}