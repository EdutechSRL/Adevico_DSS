using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO usato per le traduzioni dei termini di una categoria
    /// </summary>
    [CLSCompliant(true)]
    [Serializable]
    public class DTO_CategoryTranslation
    {
        /// <summary>
        /// Id Traduzione
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Id Categoria
        /// </summary>
        public long IdCategory { get; set; }
        /// <summary>
        /// Nome della lingua
        /// </summary>
        public String LanguageName { get;set; }
        /// <summary>
        /// Codice lingua
        /// </summary>
        public String LanguageCode { get; set; }
        /// <summary>
        /// Id Lingua
        /// </summary>
        public int LanguageId { get; set; }
        /// <summary>
        /// Internazionalizzazione Nome Categoria
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Internazionalizzazione Descrizione Categoria
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_CategoryTranslation()
        {
            Id = 0;
            IdCategory = 0;
            LanguageName = "";
            LanguageCode = "";
            LanguageId = 0;
            Name = "";
            Description = "";
        }

    }
}
