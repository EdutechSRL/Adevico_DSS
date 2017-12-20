using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Adv = lm.Comol.Modules.CallForPapers.Advanced;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
{
    /// <summary>
    /// UC integrazioni
    /// </summary>
    public interface iViewUcIntegration : lm.Comol.Modules.CallForPapers.Presentation.IViewBase
    {
        /// <summary>
        /// Inizializzazione view
        /// </summary>
        /// <param name="integration"></param>
        void BindView(lm.Comol.Modules.CallForPapers.Advanced.dto.dtoIntegration integration);

        /// <summary>
        /// Forza l'aggiornamento del controllo
        /// </summary>
        void ForceBind();

        /// <summary>
        /// Id sottomittore
        /// </summary>
        long SubmitterId { get; set; }
    }
}
