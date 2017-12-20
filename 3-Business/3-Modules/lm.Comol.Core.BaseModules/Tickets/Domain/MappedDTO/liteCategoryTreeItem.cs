using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteCategoryTreeItem
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Int64 Id { get; set; }
        /// <summary>
        /// Traduzioni (eventuali)
        /// </summary>
        public virtual IList<liteTranslation> Translations { get; set; }
        /// <summary>
        /// Nome (non tradotto)
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Descrizione (non tradotta)
        /// </summary>
        public virtual String Description { get; set; }

        /// <summary>
        /// Se vuoto è categoria Padre.
        /// </summary>
        public virtual liteCategoryTreeItem Father { get; set; }

        /// <summary>
        /// Eventuali categorie figlie
        /// </summary>
        public virtual IList<liteCategoryTreeItem> Children { get; set; }

        /// <summary>
        /// Tipo categoria (visibilità)
        /// </summary>
        public virtual Enums.CategoryType Type { get; set; }

        /// <summary>
        /// Comunità di appartenenza
        /// </summary>
        public virtual Int32 IdCommunity { get; set; }

        public virtual lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }

        public virtual int Order { get; set; }
        #region Funzioni traduzione
        
        /// <summary>
        /// Recupera il NOME tradotto della categoria
        /// </summary>
        /// <param name="LanguageCode">CODICE lingua x traduzione</param>
        /// <returns></returns>
        public virtual String GetTranslatedName(String LanguageCode)
        {
            if (Translations != null && Translations.Count() > 0)
            {
                liteTranslation Trl = Translations.Where(t => t.LanguageCode == LanguageCode).FirstOrDefault();
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
        /// <param name="LanguageCode">ID lingua x traduzione</param>
        /// <returns></returns>
        public virtual String GetTranslatedName(int LanguageID)
        {
            if (Translations != null && Translations.Count() > 0)
            {
                liteTranslation Trl = Translations.Where(t => t.LanguageId == LanguageID).FirstOrDefault();
                if (Trl == null)
                    Trl = Translations.Where(t => t.LanguageId == TicketService.LangMultiID).FirstOrDefault();
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
                liteTranslation Trl = Translations.Where(t => t.LanguageCode == LanguageCode).FirstOrDefault();
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
        /// <param name="LanguageCode">ID lingua x traduzione</param>
        /// <returns></returns>
        public virtual String GetTranslatedDescription(int LanguageID)
        {
            if (Translations != null && Translations.Count() > 0)
            {
                liteTranslation Trl = Translations.Where(t => t.LanguageId == LanguageID).FirstOrDefault();
                if (Trl == null)
                    Trl = Translations.Where(t => t.LanguageId == TicketService.LangMultiID).FirstOrDefault();
                if (Trl != null)
                    return Trl.Description;
            }

            return Description;
        }

        #endregion
    }
}
