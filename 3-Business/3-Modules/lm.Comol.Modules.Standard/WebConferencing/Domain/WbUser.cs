using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Indica il SINGOLO utente associato alla SINGOLA stanza.
    /// Tipi e Ruoli sono gestiti in maniera TOTALMENTE autonoma
    /// </summary>
    public class WbUser : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// ID Person di COMOL
        /// </summary>
        public virtual int PersonID { get; set; }
        /// <summary>
        /// Eventuale ID esterno (per eWork la KEY)
        /// </summary>
        public virtual String ExternalID { get; set; }
        

        /// <summary>
        /// Nome utente
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Cognome utente
        /// </summary>
        public virtual String SName { get; set; }

        /// <summary>
        /// Nome visualizzato (Anagrafica)
        /// </summary>
        public virtual String DisplayName 
        { 
            get
            {
                return Name + " " + SName;
            }
        }

        /// <summary>
        /// Mail dell'utente
        /// </summary>
        public virtual String Mail { get; set; }
        /// <summary>
        /// SE visualizzare la mail (sviluppi futuri)
        /// </summary>
        public virtual Boolean ShowMail { get; set; }
        /// <summary>
        /// SE mostrare lo stato nella stanza (sviluppi futuri)
        /// </summary>
        public virtual Boolean ShowStatus { get; set; }

        //public virtual String CurrentUserSessionID { get; set; }

        /// <summary>
        /// Id Stanza.
        /// Potrà essere sostituito con l'oggetto WBRoom, il quale conterra la relativa bag
        /// </summary>
        public virtual Int64 RoomId { get;set; }

        /// <summary>
        /// Solo come eventale aiuto. Potenzialmente superfluo.
        /// </summary>
        public virtual String ExternalRoomId { get; set; }


        /// <summary>
        /// SE è l'HOST (Colui che "ospita" il meeting. Al momento ha senso SOLO per eWorks)
        /// </summary>
        public virtual Boolean IsHost { get; set; }
        /// <summary>
        /// Se è "MODERATORE" (Colui che può dare parola ed attivare feature per altri utenti)
        /// </summary>
        public virtual Boolean IsController { get; set; }

        /// <summary>
        /// Audio utente abilitato
        /// </summary>
        public virtual Boolean Audio { get; set; }
        /// <summary>
        /// Video utente abilitato
        /// </summary>
        public virtual Boolean Video { get; set; }
        /// <summary>
        /// Chat utente abilitata
        /// </summary>
        public virtual Boolean Chat { get; set; }

        /// <summary>
        /// Numero di inviti mandati (sviluppo futuro)
        /// </summary>
        public virtual int SendedInvitation { get; set; }

        /// <summary>
        /// Se la mail è stata verificata
        /// </summary>
        /// <remarks>
        /// Per gli utenti inseriti dal sistema o inseriti manualmente sarà sempre TRUE.
        /// Sarà a FALSE solamente per gli utenti ESTERNI che accedono autonomamente alla stanza.
        /// </remarks>
        public virtual Boolean MailChecked { get; set; }
        /// <summary>
        /// Se l'utente è abilitato
        /// </summary>
        /// <remarks>
        /// Di default abilitato.
        /// Sarà disabilitato SOLO se viene esplicitamente disabilitato da un amministratore,
        /// oppure per gli esterni che accedono autonomamente ad una stanza APERTA con "Controllo utenti" attivato.
        /// </remarks>
        public virtual Boolean Enabled { get; set; }
        /// <summary>
        /// Chiave d'accesso per l'utente pubblico
        /// </summary>
        /// <remarks>
        /// Da rivedere, ma servirà com chiave d'accesso per l'utente esterno.
        /// SARA' COMUNQUE LEGATA ALL'INVITO!
        /// Inoltre servirà il tasto "Genera nuova chiave".
        /// </remarks>
        public virtual String UserKey { get; set; }
        /// <summary>
        /// Lingua relativa all'utente
        /// </summary>
        public virtual String LanguageCode { get; set; }
    }
}
