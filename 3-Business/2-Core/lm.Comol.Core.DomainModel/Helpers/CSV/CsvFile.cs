using System;
using System.Linq;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel.Helpers
{
     [Serializable]
    public class CsvFile 
    {
         public virtual List<TextColumn> ColumHeader { get; set; }
         public virtual List<TextRow> Rows { get; set; }

         public virtual Boolean isValid {
             get {
                 int count = ColumHeader.Count;
                 return (count>0) && (Rows.Count == (from TextRow r in Rows where r.Count == count select r).Count());
             }
         }
         public CsvFile() {
             ColumHeader = new List<TextColumn>();
             Rows = new List<TextRow>();
         }
    }
}