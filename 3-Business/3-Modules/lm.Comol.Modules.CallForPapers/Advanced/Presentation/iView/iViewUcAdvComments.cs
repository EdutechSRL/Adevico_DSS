using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Adv = lm.Comol.Modules.CallForPapers.Advanced;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
{
    /// <summary>
    /// UC commenti
    /// </summary>
    public interface iViewUcAdvComments : lm.Comol.Modules.CallForPapers.Presentation.IViewBase
    {
        /// <summary>
        /// Inizializzazione View
        /// </summary>
        /// <param name="comments"></param>
        void BindView(IList<dto.dtoAdvComment> comments);
    }
}
