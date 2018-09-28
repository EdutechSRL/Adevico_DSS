using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Parametri generici impostati a livello di sistema per le varie installazioni
    /// </summary>
    public abstract class WbSystemParameter
    {
        /// <summary>
        /// SE il servizio usa il database. In realtà TUTTI lo usano (altrimenti problemi con gestione template e altro)
        /// </summary>
        public bool UseDataBase { get; set; }
        /// <summary>
        /// Sistema implementato
        /// </summary>
        public wBImplementedSystem CurrentSystem { get; set; }
        /// <summary>
        /// Se è prevista la registrazione
        /// </summary>
        /// <remarks>Funzionalità non implementata</remarks>
        public bool RecordCan { get; set; }
        /// <summary>
        /// I giorni di vita di una registrazione, al termine dei quali verrà cancellata
        /// 0 = vita eterna
        /// </summary>
        /// <remarks>Funzionalità non implementata</remarks>
        public int RecordExpirationDay { get; set; }
        /// <summary>
        /// Se è previsto il salvataggio delle statistiche
        /// </summary>
        /// <remarks>Funzionalità non implementata</remarks>
        public bool StatisticsCan { get; set; }
        /// <summary>
        /// I giorni di vita delle statistiche, al termine dei quali le statistiche saranno cancellate
        /// 0 = vita eterna
        /// </summary>
        /// <remarks>Funzionalità non implementata</remarks>
        public int StatisticsExpirationDay { get; set; }

        /// <summary>
        /// Giorni di vita di un meeting, al termine dei quali il meeting e tutti i relativi dati (utenti, inviti, statistiche, allegati, registrazioni, statistiche) verranno cancellati
        /// 0 = vita eterna
        /// </summary>
        /// <remarks>Funzionalità non implementata</remarks>
        public int MeetingsExpirationDay { get; set; }

        ///// <summary>
        ///// Andrà sostituito con parametri standard/possibili come bitrate, videocodec, framerate, video resolution, audio codec, etc...
        ///// </summary>
        //public object MeetingsConfiguration { get; set; }
    }

    /// <summary>
    /// Sistemi implementati
    /// </summary>
    public enum wBImplementedSystem
    {
        eWorks,
        OpenMeetings
    }
}
