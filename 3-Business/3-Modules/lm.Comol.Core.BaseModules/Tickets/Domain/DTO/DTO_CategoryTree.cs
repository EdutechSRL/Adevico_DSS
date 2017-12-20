using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO per albero categorie (riordino)
    /// </summary>
    [CLSCompliant(true)]
    [Serializable]
    public class DTO_CategoryTree
    {
        /// <summary>
        /// Id Categoria
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Nome categoria
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Descrizione categoria
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// Elenco figli
        /// </summary>
        public IList<DTO_CategoryTree> Children { get; set; }
        /// <summary>
        /// Posizione ordinamento
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Icona categoria (INUTILE)
        /// </summary>
        public String Icon { get; set; }
        /// <summary>
        /// Indica che è la categoria di DEFAULT (NO ORDINAMENTO): RIVEDERE!
        /// </summary>
        public Boolean IsDeleted { get; set; }
        /// <summary>
        /// Indica che è la categoria è stata cancellata.
        /// </summary>
        public Boolean IsDefault { get; set; }
        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_CategoryTree()
        {
            Id = 0;
            Name = "";
            Description = "";
            Children = new List<DTO_CategoryTree>();
            Icon = "";
            IsDeleted = false;
        }

        public bool IsSelectable { get; set; }
    }
}
