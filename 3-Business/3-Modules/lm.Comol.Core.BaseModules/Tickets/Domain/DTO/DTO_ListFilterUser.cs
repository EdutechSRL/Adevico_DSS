using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO con i valori impostati per i filtri
    /// </summary>
    /// <remarks>
    /// I campi "r_" hanno valenza SOLO per Manager/Resolver.
    /// Nella lista utente saranno ignorati.
    /// </remarks>
    [Serializable, CLSCompliant(true)]
    public class DTO_ListFilterUser
    {
        public DTO_ListFilterUser()
        {
            OrderField = Enums.TicketOrderUser.lastModify;
            OrderAscending = false;

            Title = "";

            DateStart = null;
            DateEnd = null;
            DateField = Enums.TicketUserDateFilter.LastModify;

            Status = Enums.TicketStatus.open;
            ShowAllStatus = true;

            PageIndex = 0;
            PageSize = 25;

            RecordTotal = 0;
        }

        public Enums.TicketOrderUser OrderField { get; set; }
        public Boolean OrderAscending { get; set; }

        public String Title { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public Enums.TicketUserDateFilter DateField { get; set; }
        public Enums.TicketStatus Status { get; set; }
        public Boolean ShowAllStatus { get; set; }

        public Boolean ShowOnlyNews { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordTotal { get; set; }

        public string OwnerName { get; set; }

        /// <summary>
        /// Filtro su Owner
        /// </summary>
        /// <value>
        /// -1  =   TUTTI
        ///  0  =   solo i MIEI
        ///  1  =   solo CREATI x Conto di...
        /// </value>
        public Int16 OwnerType { get; set; }


    }
}
