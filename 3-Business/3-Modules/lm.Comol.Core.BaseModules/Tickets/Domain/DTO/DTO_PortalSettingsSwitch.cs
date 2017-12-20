using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_PortalSettingsSwitch
    {
        public DTO_PortalSettingsSwitch()
        {
            
        }

        public DTO_PortalSettingsSwitch(Domain.SettingsPortal settings)
        {
            IsActive = settings.IsActive;
            CanCreateCategory = settings.CanCreateCategory;
            CanShowTicket = settings.CanShowTicket;
            CanEditTicket = settings.CanEditTicket;
            CanBehalf = settings.CanBehalf;
            IsNotificationUserActive = settings.IsNotificationUserActive;
            IsNotificationManActive = settings.IsNotificationManActive;
        }
        

        /// <summary>
        /// Indica SE il servizio è attivo
        /// </summary>
        /// <remarks>
        /// NON MAPPATO: controllo sul sistema
        /// </remarks>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Indica se è attivo il management delle categorie
        /// </summary>
        public virtual bool CanCreateCategory { get; set; }
        /// <summary>
        /// Indica se è possibile visualizzare i Ticket.
        /// </summary>
        public virtual bool CanShowTicket { get; set; }
        /// <summary>
        /// Indica se è possibile modificare o creare Ticket.
        /// Se disattivo potrebbero essere visibili in sola lettura.
        /// </summary>
        public virtual bool CanEditTicket { get; set; }

        /// <summary>
        /// Se il "crea per conto di..." è attivo
        /// </summary>
        public virtual bool CanBehalf { get; set; }

        //Notifiche
        public virtual bool IsNotificationUserActive { get; set; }
        public virtual bool IsNotificationManActive { get; set; }
    }
}
