using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.DTO
{
    public class DTOUserInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// email utente
        /// </summary>
        public String username { get; set; }
        /// <summary>
        /// nome utente
        /// </summary>
        public String userpassword { get; set; }
        /// <summary>
        /// md5 della password utente
        /// </summary>
        public String usermail { get; set; }
        /// <summary>
        /// status=stato di attivazione dell'utente (0=non attivo, 1=attivo, 2=sospeso)
        /// </summary>
        public Int16 status { get; set; }
        /// <summary>
        /// data di creazione dell'utente (formato yyyy-mm-dd hh:mm:ss)
        /// </summary>
        public DateTime? creation_date { get; set; }
        /// <summary>
        /// data di scadenza dell'utente (formato yyyy-mm-dd hh:mm:ss)
        /// </summary>
        public DateTime? expiry_date { get; set; }
        /// <summary>
        /// lingua dell'utente (it,en,de,fr,es)
        /// </summary>
        public eWLanguages language { get; set; }
        /// <summary>
        /// differenza in ore dal fuso orario GMT
        /// </summary>
        public int timezone { get; set; }
        /// <summary>
        /// dimensione massima della stanza di videoconferenza (0=nessun limite)
        /// </summary>
        public int maxroomsize { get; set; }
        /// <summary>
        /// registrazione abilitata per l'utente (1=attiva,0=non attiva)
        /// </summary>
        public bool canrecord {get;set;}
        /// <summary>
        /// numero massimo di video attivabili (-1=nessuna limitazione)
        /// </summary>
        public int maxvideonum {get;set;}
        /// <summary>
        /// numero di eventi disponibili per l'utente (-1=gestione a eventi non attiva)
        /// </summary>
        public int daysevent {get;set;}
        /// <summary>
        /// host/ip portale di assegnazione dell'utente
        /// </summary>
        public string portalname {get;set;}
        /// <summary>
        /// elenco delle bitrate disponibili per l'utente separate da virgola
        /// </summary>
        public IList<int> bitratelist {get;set;}
        /// <summary>
        /// elenco dei formati video disponibili separati da virgola nel formato "larghezza"x"altezza"
        /// </summary>
        public IList<DTOVideoFormat> videoformatlist { get; set; }

    }
}
