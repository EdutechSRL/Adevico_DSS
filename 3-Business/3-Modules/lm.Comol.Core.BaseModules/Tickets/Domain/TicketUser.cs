using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Utente ticket
    /// </summary>
    /// <remarks>
    /// Utenti interni verranno aggiunti, se non già presenti:
    /// - alla creazione del primo Ticket
    /// - all'aggiunta come risorsa di una categoria (Manager o Resolver)
    /// Utenti esterni verranno aggiunti PRIMA di poter creare il primo ticket (fase di "iscrizione")
    /// </remarks>
    [Serializable()]
    [CLSCompliant(true)]
    public class TicketUser : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// Se interno, riferimento alla person
        /// </summary>
        public virtual litePerson Person { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Cognome
        /// </summary>
        public virtual String Sname { get; set; }
        /// <summary>
        /// Mail utente
        /// </summary>
        public virtual String mail { get; set; }
        /// <summary>
        /// Codice lingua di default
        /// </summary>
        public virtual String LanguageCode { get; set; }
        /// <summary>
        /// Nome visualizzato (se "nascondi nome" sarà "Responsabile/Addetto Categoria XYZ"
        /// </summary>
        public virtual String DisplayName { get; set; }
        /// <summary>
        /// PWD Accesso esterno
        /// </summary>
        public virtual String Code { get; set; }
        /// <summary>
        /// Se la mail è stata confermata: creazione utente e primo accesso.
        /// Per utenti interni è di default a TRUE.
        /// </summary>
        public virtual bool MailChecked { get; set; }

        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Impostazioni utente: abilita o disabilita l'invio di TUTTE le mail.
        /// </summary>
        public virtual bool IsNotificationActiveUser { get; set; }

        /// <summary>
        /// Impostazioni manager/resolver: abilita o disabilita l'invio di TUTTE le mail.
        /// </summary>
        public virtual bool IsNotificationActiveManager { get; set; }


        public virtual String UsrCode
        {
            get
            {
                return string.Format("U{0:00000000}-{1:0000}", Id, (Person == null) ? 0 : Person.Id);
            }
        }
    }
}
