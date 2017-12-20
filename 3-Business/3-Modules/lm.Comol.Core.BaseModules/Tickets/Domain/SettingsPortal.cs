using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Impostazioni di portale
    /// </summary>
    /// <remarks>
    /// SE su tabella in dB, un solo campo per tutto il sistema!
    /// </remarks>
    [Serializable]
    public class SettingsPortal : DomainBaseObjectMetaInfo<long>
    {
        public SettingsPortal()
        {
            //hasDraftLimitationError = false;
            //hasExternalLimitationError = false;
            //hasInternalLimitationError = false;

            HasExternalLimitation = true;
            HasInternalLimitation = true;
            HasDraftLimitation = true;
            ExternalLimitation = 3;
            InternalLimitation = 10;
            DraftLimitation = 5;
            CommunityTypeSettings = new List<SettingsComType>();
            PublicCategories = new List<Domain.DTO.DTO_CategoryTree>(); // new List<Category>();
            
            //MailSettings = Enums.MailSettings.Default;
            CategoryDefault = null;

            IsActive = true;
            CanCreateCategory = false;
            CanShowTicket = false;
            CanEditTicket = false;
        }
        /// <summary>
        /// Se ci sono limiti al numero di ticket aperti da esterni
        /// </summary>
        public virtual bool HasExternalLimitation { get; set; }
        /// <summary>
        /// Se ci sono limiti al numero di ticket aperti da interni
        /// </summary>
        public virtual bool HasInternalLimitation { get; set; }

        /// <summary>
        /// Massimo numero ticket per esterni
        /// </summary>
        public virtual int ExternalLimitation { get; set; }
        /// <summary>
        /// Massimo numero ticket per interni
        /// </summary>
        public virtual int InternalLimitation { get; set; }
        /// <summary>
        /// Se ci sono limiti al numero di bozze
        /// </summary>
        public virtual bool HasDraftLimitation { get; set; }
/// <summary>
        /// Massimo numero di bozze
        /// </summary>
        public virtual int DraftLimitation { get; set; }
        /// <summary>
        /// Impostazioni Tipo Comunità/Categorie.
        /// </summary>
        /// <remarks>
        /// Proprietà NON mappata. "Recuperare" a mano: tutti i campi!
        /// </remarks>
        public virtual IList<SettingsComType> CommunityTypeSettings { get; set; }

        /// <summary>
        /// Elenco categorie pubbliche (SOLO PREVIEW)
        /// </summary>
        /// <remarks>
        /// Proprietà NON mappata. "Recuperare" a mano: tutti i campi!
        /// </remarks>
        public virtual IList<Domain.DTO.DTO_CategoryTree> PublicCategories { get; set; }
            //IList<Category> PublicCategories { get; set; }

        /// <summary>
        /// Categoria di Default
        /// </summary>
        /// <remarks>
        /// Proprietà NON mappata. "Recuperare" a mano: tutti i campi!
        /// </remarks>
        public virtual Domain.DTO.DTO_CategoryTree CategoryDefault { get; set; }


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
        
        //secondo logiche "settings", da recuperare:
        public virtual Domain.Enums.MailSettings MailSettingsUser { get; set; }
        public virtual Domain.Enums.MailSettings MailSettingsManager { get; set; }


        //public virtual bool hasDraftLimitationError { get; set; }
        //public virtual bool hasExternalLimitationError { get; set; }
        //public virtual bool hasInternalLimitationError { get; set; }
        
    }
}

