using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Domain
{
    /// <summary>
    /// Tabella economica
    /// </summary>
    /// <remarks>
    /// Viene creata all'apertura della commissione economica,
    /// partendo dalle tabelle economiche sottomesse dall'utente (da html ad oggetto di dominio)
    /// </remarks>
    public class EconomicTable : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Stringa con tutte le intestazioni, separate da |
        /// </summary>
        public virtual String HeaderValue { get; set; }
        /// <summary>
        /// Array di stringa con i singoli valori dell'intestazione
        /// </summary>
        public virtual String[] HeaderValues
        {
            get
            {
                return HeaderValue.Split('|');
            }
        }
        /// <summary>
        /// Elementi economici (righe delle tabelle, con dati aggiuntivi (ammesso, quantità ammessa, totale ammesso...)
        /// </summary>
        public virtual IList<EconomicItem> Items { get; set; }
        /// <summary>
        /// Valutazione economica di appartenenza
        /// </summary>
        public virtual EconomicEvaluation EcoEvaluation { get;set; }
        /// <summary>
        /// Definizione campo economico (massimali ammessi, etc...)
        /// </summary>
        public virtual lm.Comol.Modules.CallForPapers.Domain.FieldDefinition FieldDefinition { get; set; }
       /// <summary>
       /// Totale richeisto tabella
       /// </summary>
        public virtual Double RequestTotal
        {
            get
            {
                Double sum = 0;
                if(Items != null && Items.Any())
                {
                    sum = Items.Sum(i => i.RequestTotal);
                }
                return sum;
            }
        }
        /// <summary>
        /// Totale ammesso tabella
        /// </summary>
        public virtual Double AdmitTotal
        {
            get
            {
                Double sum = 0;
                if (Items != null && Items.Any())
                {
                    sum = Items.Sum(i => i.AdmitTotal);
                }
                return sum;
            }
        }
        /// <summary>
        /// Massimo ammesso tabella
        /// </summary>
        public virtual Double AdmitMax { get; set; }
    }
}