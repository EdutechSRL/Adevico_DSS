using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Glossary.MVP
{
    public interface iViewGlossarySelector : iDomainView
    {
        #region Global function
        /// <summary>
        /// Recupera l'elenco dei nomi dei glossari selezionati.
        /// </summary>
        /// <returns>Lista in formato "Nome (#)" in cui # è il numero di post.</returns>
        /// <remarks>Da usare per il riepilogo</remarks>
         IList<Domain.Dto.GlossaryGroupSummaryDTO> GetSelectedGlossarySummary();

        /// <summary>
        /// Recupera l'elenco degli ID dei glossari selezionati per la copia
        /// </summary>
        /// <returns>Gli ID dei glossari da copiare</returns>
         IList<Int64> GetSelectedGlossaryIds();

        ///// <summary>
        ///// Effettua la copia vera e propria
        ///// </summary>
        //public void MakeCopy();

        /// <summary>
        /// Effettua la copia vera e propria
        /// </summary>
        /// <returns>
        /// Il numero di Glossari effettivamente copiati
        /// </returns>
         Int32 MakeCopy(Int32 DestinationCommunityId, Boolean IsNewCommunity);

        /// <summary>
        /// Inizializza il controllo impostando la comunità sorgente
        /// </summary>
        /// <param name="SourceCommunityId">ID della comunità sorgente</param>
        /// <returns>
        /// TRUE: se ci sono glossari nella comunità sorgente
        /// FALSE: se non ci sono glossari disponibili
        /// </returns>
        Boolean InitControl(Int32 SourceCommunityId);

        ///// <summary>
        ///// Inizializza il controllo impostando sia la comunità sorgente che quella destinazione
        ///// </summary>
        ///// <param name="SourceCommunityId">Id della comunità sorgente</param>
        ///// <param name="DestinationCommunityId">Id della comunità destinazione</param>
        //public void InitControl(Int32 SourceCommunityId, Int32 DestinationCommunityId);
        #endregion

        #region Presentation

        /// <summary>
        /// Mostra l'elenco dei glossari disponibili nella comnuità sorgente
        /// </summary>
        /// <param name="GlossaryGroups">I Glossari disponibili</param>
        void BindGlossary(IList<Glossary.Domain.GlossaryGroup> GlossaryGroups);

        #endregion
    }
}
