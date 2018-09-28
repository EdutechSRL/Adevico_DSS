using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
   
    public interface IViewSummaryUser : IViewBaseSummary
    {
        Boolean PreloadReloadFilters { get; }
        SummaryType PreloadFromSummary { get; }
        Int32 PreloadIdUser { get; }
        
        SummaryType FromSummary  { get; set; }
        Int32 SummaryIdUser { get; set; }
        lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
        Boolean AllowOrganizationSelection { get; set; }

        // Teoricamente andrebbero tutte le funzioni/metodi da gestione MVP, nel frattempo carica il vecchio bind dati della pagina...
        
        void LoadAvailableOrganizations(Int32 idUser);
        void LoadData(Int32 idUser, lm.Comol.Core.DomainModel.litePerson person);
        void DisplayWrongPageAccess(String url);
        void DisplayNoUserSelected(String url);

        String GetFileName(lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType type);
        List<dtoUserPathQuiz> GetQuizInfos(List<dtoUserPathQuiz> qInfos);
    }
}