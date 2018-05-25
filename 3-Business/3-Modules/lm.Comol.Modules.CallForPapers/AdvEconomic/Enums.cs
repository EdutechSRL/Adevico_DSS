using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic
{
    /// <summary>
    /// Stato valutazione economica
    /// </summary>
    public enum EvalStatus : int
    {
        /// <summary>
        /// In bozza
        /// </summary>
        draft = 0,
        /// <summary>
        /// Preso in carico (non utilizzato)
        /// </summary>
        take = 1,
        /// <summary>
        /// Valutazione completata
        /// </summary>
        completed = 2,
        /// <summary>
        /// Valutazione confermata dal presidente
        /// </summary>
        confirmed = 3
    }

    /// <summary>
    /// Stato allineamento Gamma
    /// </summary>
    public enum Gammastatus : int
    {
        /// <summary>
        /// Utente non verificato
        /// </summary>
        NotChecked = 0,
        /// <summary>
        /// Errore: dati non validi
        /// </summary>
        InvalidData = 1,
        /// <summary>
        /// Errore: invio dati - riprovare
        /// </summary>
        SendError = 2,
        /// <summary>
        /// Utente non trovato in gamma
        /// </summary>
        NotFound = 4,
        /// <summary>
        /// Utente confermato
        /// </summary>
        UserConfirmed = 16,
        /// <summary>
        /// Dati inviati correttamente
        /// </summary>
        DataSended = 32
    }
}
