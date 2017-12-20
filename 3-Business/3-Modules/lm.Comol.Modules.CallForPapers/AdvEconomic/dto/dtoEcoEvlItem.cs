using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// Elemento di una tabella economica: righe
    /// </summary>
    public class dtoEcoEvlItem
    {
        /// <summary>
        /// Id elemento
        /// </summary>
        public long itmId { get; set; }
        /// <summary>
        /// Valori (inseriti dall'utente)
        /// </summary>
        public String[] values { get; set; }
        /// <summary>
        /// Quantità richiesta
        /// </summary>
        public double ReqQuantity { get; set; }
        /// <summary>
        /// Prezzo richiesto
        /// </summary>
        public double ReqPrice { get; set; }
        /// <summary>
        /// Totale richiesto
        /// </summary>
        public double ReqTotal { get; set; }
        /// <summary>
        /// Ammissione della spesa
        /// </summary>
        public bool isAdmit { get; set; }
        /// <summary>
        /// Spesa totale ammessa
        /// </summary>
        public double AdmitTotal { get; set; }
        /// <summary>
        /// Quantità totale ammessa
        /// </summary>
        public virtual Double AdmitQuantity { get; set; }
        /// <summary>
        /// Commento alla singola voce
        /// </summary>
        public virtual String Comment { get; set; }
        /// <summary>
        /// costruttore vuoto
        /// </summary>
        public dtoEcoEvlItem() { }
        /// <summary>
        /// Costruttore da oggetto di dominio
        /// </summary>
        /// <param name="item">Elemento (riga) tabella economica</param>
        public dtoEcoEvlItem(Domain.EconomicItem item) {
            if (item == null)
                return;

            itmId = item.Id;
            values = item.InfoValues;
            ReqQuantity = item.RequestQuantity;
            ReqPrice = item.RequestUnitPrice;
            ReqTotal = item.RequestTotal;
            isAdmit = item.IsAdmit;
            AdmitTotal = item.AdmitTotal;
            AdmitQuantity = item.AdmitQuantity;
            Comment = item.Comment;
        }
    }
}
