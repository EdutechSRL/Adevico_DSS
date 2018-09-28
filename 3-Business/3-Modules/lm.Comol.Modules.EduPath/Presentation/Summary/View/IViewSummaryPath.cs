using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{

    public interface IViewSummaryPath : IViewBaseSummary
    {
        Boolean PreloadReloadFilters { get; }
        SummaryType PreloadFromSummary { get; }     
        SummaryType FromSummary  { get; set; }

        lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
        Boolean AllowOrganizationSelection { get; set; }

        // Teoricamente andrebbero tutte le funzioni/metodi da gestione MVP, nel frattempo carica il vecchio bind dati della pagina...
        
        void LoadAvailableOrganizations(Int32 idUser);
        void InitializeFilters();
        void DisplayWrongPageAccess(String url);

        //String GetFileName(lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType type);
        //List<dtoUserPathQuiz> GetQuizInfos(List<dtoUserPathQuiz> qInfos);
    }

 }