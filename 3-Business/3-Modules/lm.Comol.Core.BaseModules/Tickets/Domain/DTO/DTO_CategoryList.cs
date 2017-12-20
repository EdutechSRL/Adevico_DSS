using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO per la lista delle categorie nella gestione
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_CategoryList
    {
        /// <summary>
        /// Id Categoria
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Id Categoria padre
        /// </summary>
        public Int64 FatherId { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Descrizione
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// Lista di DTO Utenti
        /// </summary>
        public IList<DTO_CategoryListUser> Users { get; set; }
        /// <summary>
        /// Numero di Manager assegnati
        /// </summary>
        public int ManagerNum { get; set; }
        /// <summary>
        /// Numero di Resolver assegnati
        /// </summary>
        public int ResolverNum { get; set; }
        /// <summary>
        /// DA RIVEDERE! - Se è stata cancellata
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_CategoryList()
        {
            ManagerNum = 0;
            ResolverNum = 0;
        }
        public int Order { get; set; }
    }
}
