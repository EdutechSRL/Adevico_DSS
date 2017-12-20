using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
{
    /// <summary>
    /// Sommario di uno step:
    /// elenco sottomissioni associate allo step con valutazioni delle commissioni
    /// </summary>
    public interface iViewAdvStepSummary : lm.Comol.Modules.CallForPapers.Presentation.IViewBase
    {
        /// <summary>
        /// Id dello step
        /// </summary>
        long StepId { get; set; }
        /// <summary>
        /// Inizializzazione View
        /// </summary>
        /// <param name="summary"></param>
        void BindView(dto.dtoStepSummary summary);
        /// <summary>
        /// Imposta i tasti di navigazione
        /// </summary>
        /// <param name="CommunityId">Id Comunità</param>
        /// <param name="CallId">Id Call For Peaper</param>
        /// <param name="CommissionId">Id Commissione</param>
        void BindNavigationUrl(int CommunityId, long CallId, long CommissionId);
    }
}
