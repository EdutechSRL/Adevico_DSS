using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    /// <summary>
    /// iView base per i Call For Peaper
    /// </summary>
    public interface IViewBase: lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        /// <summary>
        /// Indica di mostrare "sessione scaduta".
        /// </summary>
        void DisplaySessionTimeout();
        /// <summary>
        /// Indica all'utente che non ha i permessi per accedere ai contenuti
        /// </summary>
        /// <param name="idCommunity">Id comunità corrente</param>
        /// <param name="idModule">Id modulo (Call For Peaper)</param>
        void DisplayNoPermission(int idCommunity, int idModule);
    }
}
