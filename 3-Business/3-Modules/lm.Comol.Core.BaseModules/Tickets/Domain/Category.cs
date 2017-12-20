using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Categoria di ticket
    /// </summary>
    [Serializable()]
    public class Category : DomainBaseObjectMetaInfo<long>
    {
        public Category()
        {
            Name = "";
            Description = "";
            Father = null;
            Order = 0;
        }



        /// <summary>
        /// Nome categoria
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Descrizione (alt sul nome nella treeview)
        /// </summary>
        public virtual String Description { get; set; }
        /// <summary>
        /// Se vuoto è categoria Padre.
        /// </summary>
        public virtual Category Father { get; set; }
        ///// <summary>
        ///// Managers associati
        ///// </summary>
        ///// <remarks>
        ///// Gestione "a mano"
        ///// </remarks>
        //public virtual IList<TicketUser> Managers { get; set; }
        ///// <summary>
        ///// Resolvers associati
        ///// </summary>
        ///// <remarks>
        ///// Gestione "a mano"
        ///// </remarks>
        //public virtual IList<TicketUser> Resolvers { get; set; }

        /// <summary>
        /// Elenco di manager/resolver. Gestito "a mano".
        /// </summary>
        public virtual IList<LK_UserCategory> UserRoles { get; set; }

        /// <summary>
        /// Eventuali categorie figlie
        /// </summary>
        public virtual IList<Category> Children { get; set; }
        /// <summary>
        /// Tipo categoria (visibilità)
        /// </summary>
        public virtual Enums.CategoryType Type { get; set; }
        
        /// <summary>
        /// Eventuale ordinamento
        /// </summary>
        public virtual int Order { get; set; }
        /// <summary>
        /// Comunità di appartenenza
        /// </summary>
        public virtual Int32 IdCommunity { get; set; }

        /// <summary>
        /// Traduzioni di Name e Description
        /// </summary>
        public virtual IList<CategoryTranslation> Translations { get; set; }

        /// <summary>
        /// Traduzione corrente. DA VEDERE...
        /// </summary>
        public virtual CategoryTranslation CurrentTranslation { get; set; }

        /// <summary>
        /// Indica che la categoria è quella di Default per il sistema:
        /// No cancellazione
        /// No modifica
        /// </summary>
        public virtual bool IsDefault { get; set; }
        /// <summary>
        /// Recupera il NOME tradotto della categoria
        /// </summary>
        /// <param name="LanguageCode">CODICE lingua x traduzione</param>
        /// <returns></returns>
        public virtual String GetTranslatedName(String LanguageCode)
        {
            if (Translations != null && Translations.Count() > 0)
            {
                CategoryTranslation Trl = Translations.Where(t => t.LanguageCode == LanguageCode).FirstOrDefault();
                if (Trl == null)
                    Trl = Translations.Where(t => t.LanguageCode == TicketService.LangMultiCODE).FirstOrDefault();
                if (Trl != null)
                    return Trl.Name;

            }

            return Name;
        }

        /// <summary>
        /// Recupera il NOME tradotto della categoria
        /// </summary>
        /// <param name="LanguageCode">CODICE lingua x traduzione</param>
        /// <returns></returns>
        public virtual String GetTranslatedDescription(String LanguageCode)
        {
            if (Translations != null && Translations.Count() > 0)
            {
                CategoryTranslation Trl = Translations.Where(t => t.LanguageCode == LanguageCode).FirstOrDefault();
                if (Trl == null)
                    Trl = Translations.Where(t => t.LanguageCode == TicketService.LangMultiCODE).FirstOrDefault();
                if (Trl != null)
                    return Trl.Description;

            }

            return Description;
        }
    }
}
