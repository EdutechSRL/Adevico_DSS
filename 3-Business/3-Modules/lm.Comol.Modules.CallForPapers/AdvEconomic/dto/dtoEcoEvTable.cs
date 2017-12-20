using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// dto Tabella economica - valutazione
    /// </summary>
    public class dtoEcoEvTable
    {
        /// <summary>
        /// Id campo bando di riferimento
        /// </summary>
        public long FieldId { get; set; }
        /// <summary>
        /// Nome campo bando
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Descrizione campo bando
        /// </summary>
        public string FieldDescription { get; set; }
        /// <summary>
        /// Array di stringhe con le intestazioni aggiuntive
        /// </summary>
        public String[] HeadValues { get; set; }

        /// <summary>
        /// Elenco elementi (righe tabella)
        /// </summary>
        public IList<dtoEcoEvlItem> Rows { get; set; }

        /// <summary>
        /// Totale richiesto
        /// </summary>
        public Double TotalRequired { get; set; }
        /// <summary>
        /// Totale ammesso
        /// </summary>
        public Double TotalAdmit { get; set; }
        /// <summary>
        /// Massimo ammesso
        /// </summary>
        public Double AdmitMax { get; set; }


        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoEcoEvTable() { }

        /// <summary>
        /// Costruttore da oggetto di dominio (tabella economica)
        /// </summary>
        /// <param name="Table">Tabella economica</param>
        public dtoEcoEvTable(Domain.EconomicTable Table)
        {
            if (Table == null)
                return;

            if (Table.FieldDefinition != null)
            {
                FieldId = Table.FieldDefinition.Id;
                FieldName = Table.FieldDefinition.Name;
                FieldDescription = Table.FieldDefinition.Description;
                
            } else
            {
                FieldId = 0;
                FieldName = "Unknow";
                FieldDescription = "";
            }

            HeadValues = Table.HeaderValues;
            TotalRequired = Table.RequestTotal;
            TotalAdmit = Table.AdmitTotal;
            AdmitMax = Table.AdmitMax;

            Rows = Table.Items.Select(i => new dtoEcoEvlItem(i)).ToList();
            

        }

    }
}
