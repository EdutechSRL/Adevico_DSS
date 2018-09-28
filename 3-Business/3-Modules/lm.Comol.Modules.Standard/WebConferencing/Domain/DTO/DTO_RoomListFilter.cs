using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    [Serializable]
    public class DTO_RoomListFilter
    {
        public DTO_RoomListFilter()
        {
            this.Access = RoomListFilterAccess.all;
            this.OrderBy = RoomListOrder.Name;
            this.OrderDir = true;
            this.Page = 1;
            this.RecordShowNum = 25;
            this.RecordTotal = 0;
            this.Type = RoomListFilterType.all;
            this.Visibility = RoomListFilterVisibility.all;
        }
        /// <summary>
        /// Filtra su accesso
        /// </summary>
        public RoomListFilterAccess Access { get; set; }
        /// <summary>
        /// Filtra su tipo stanza
        /// </summary>
        public RoomListFilterType  Type { get; set; }
        /// <summary>
        /// Filtra su visibilità
        /// </summary>
        public RoomListFilterVisibility Visibility { get; set; }

        /// <summary>
        /// Campo ordinamento
        /// </summary>
        public RoomListOrder OrderBy { get; set; }
        /// <summary>
        /// Direzione ordinamento. True = ascending
        /// </summary>
        public Boolean OrderDir { get; set; }
        
        /// <summary>
        /// Paginazione: numero record visualizzati
        /// </summary>
        public int RecordShowNum { get; set; }
        /// <summary>
        /// Paginazione: pagina corrente
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Paginazione: record totali
        /// </summary>
        public int RecordTotal { get; set; }
    }
}
