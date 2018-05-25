using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// DTO dati da sincronizzare con Gamma   - WIP
    /// </summary>
    public class dtoGammaInfo
    {
        /// <summary>
        /// Id sottomissione
        /// </summary>
        public Int64 SubmissionId { get; set; }

        /// <summary>
        /// Id Call
        /// </summary>
        public Int64 CallId { get; set; }

        /// <summary>
        /// Rank finale processo valutazione
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Id sottomittore (id person Adevico)
        /// </summary>
        public int SubmitterId { get; set; }

        /// <summary>
        /// Dati utente gamma
        /// </summary>
        /// <remarks>
        /// Generati in Adevico da Submitter,
        /// verrà aggiunto il codice Gamma per l'allineamento
        /// 
        /// NOTA: definire quali dati possono essere modificati 
        /// e quali utilizzare per la validazione dell'utente:
        /// nome, cognome, mail, cf, piva, etc...
        /// </remarks>
        public dtoGammaUser GammaUser { get; set; }

        /// <summary>
        /// Spesa ammessa totale
        /// </summary>
        public Double AdmitTotal { get; set; }

        /// <summary>
        /// Stato allineamento con Gamma
        /// </summary>
        public Gammastatus Status { get; set; }

    }
}
