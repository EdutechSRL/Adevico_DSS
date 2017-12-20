using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO per le DDL delle categorie
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_CategoryDDLItem
    {
        /// <summary>
        /// ID Categoria
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Nome categoria (Internazionalizzato)
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Description categoria (Internazionalizzato)
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// Id Categoria padre (Se 0 è di livello zero)
        /// </summary>
        public Int64 FatherId { get; set; }
        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_CategoryDDLItem()
        {
            Id = 0;
            Name = "";
            Description = "";
            FatherId = 0;
        }
    }
}
