using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable, CLSCompliant(true)]
    public class liteCategory
    {
        /// <summary>
        /// Id Categoria
        /// </summary>
        public virtual Int64 Id { get; set; }

        /// <summary>
        /// Elenco traduzioni
        /// </summary>
        public virtual IList<liteTranslation> Translations { get; set; }

        /// <summary>
        /// Nome originario
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// Comunità di appartenenza
        /// </summary>
        public virtual Int32 IdCommunity { get; set; }

        /// <summary>
        /// Recupera il NOME tradotto della categoria
        /// </summary>
        /// <param name="LanguageCode">CODICE lingua x traduzione</param>
        /// <returns></returns>
        public virtual String GetTranslatedName(String LanguageCode)
        {
            if(Translations != null && Translations.Count() >0)
            {
                liteTranslation Trl = Translations.Where(t => t.LanguageCode == LanguageCode).FirstOrDefault();
                if (Trl == null)
                    Trl = Translations.Where(t => t.LanguageCode == TicketService.LangMultiCODE).FirstOrDefault();
                if (Trl != null)
                    return Trl.Name;
                
            }

            return Name;
        }




    }
}
