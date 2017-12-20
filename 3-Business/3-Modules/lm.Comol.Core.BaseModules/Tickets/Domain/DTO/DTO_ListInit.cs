using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// Inizializzazioni Lista Ticket
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class DTO_ListInit
    {
        public DTO_ListInit()
        {
            Categories = new List<DTO_CategoryTree>();

        }

        /// <summary>
        /// Categorie per la DDL delle categorie
        /// </summary>
        public IList<DTO_CategoryTree> Categories { get; set; }

        /// <summary>
        /// Lingue disponibili
        /// </summary>
        public IList<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> r_availableLanguages { get; set; }

        
    }
}
