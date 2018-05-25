using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation.View
{
    /// <summary>
    /// iView per controllo esportazione XLSX
    /// </summary>
    public interface iViewTableExport : CallForPapers.Presentation.IViewBase
    {
        /// <summary>
        /// Id Valutazione
        /// </summary>
        Int64 EvaluationId { get; set; }
    }
}
