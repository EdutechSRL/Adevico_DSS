using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Domain
{
    /// <summary>
    /// Elemento tabella economica (singola riga)
    /// </summary>
    public class EconomicItem : DomainBaseObjectLiteMetaInfo<long>
    {
        //public virtual String Name { get; set; }
        //public virtual String Description { get; set; }

        /// <summary>
        /// Stringa con le colonne aggiuntive previste nel bando (separate da |)
        /// </summary>
        public virtual String InfoValue { get; set; }
        /// <summary>
        /// Arrai singole voci aggiuntive
        /// </summary>
        public virtual String[] InfoValues
        {
            get
            {
                return InfoValue.Split('|');
            }
        }
        /// <summary>
        /// Quantità richiesta
        /// </summary>
        public virtual Double RequestQuantity { get; set; }
        /// <summary>
        /// Prezzo unitario richiesto
        /// </summary>
        public virtual Double RequestUnitPrice { get; set; }
        /// <summary>
        /// Totale richiesto
        /// </summary>
        public virtual Double RequestTotal
        {
            get
            {
                return (Double)RequestQuantity * RequestUnitPrice;
            }
        }
        /// <summary>
        /// Spesa ammessa
        /// </summary>
        public virtual bool IsAdmit { get; set; }
        /// <summary>
        /// Quantità ammessa
        /// </summary>
        public virtual Double AdmitQuantity { get; set; }
        /// <summary>
        /// Totale ammesso
        /// </summary>
        public virtual Double AdmitTotal { get; set; }
        /// <summary>
        /// Tabella economica di appartenenza
        /// </summary>
        public virtual EconomicTable Table { get; set; }
        /// <summary>
        /// Commenti
        /// </summary>
        public virtual String Comment { get; set; }
    }
}
