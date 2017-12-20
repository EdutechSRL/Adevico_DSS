
using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ExternalRow 
    {
        public virtual List<ExternalCell> Cells { get; set; }
        public virtual int Number { get; set; }
        public virtual Boolean AllowImport { get; set; }
        public virtual Boolean HasDuplicatedValues {
            get {

                return (Cells.Where(c => c.isDuplicate).Any());
            }
            
        }
        public virtual Boolean HasDBDuplicatedValues
        {
            get
            {

                return (Cells.Where(c => c.isDBduplicate).Any());
            }

        }
        public virtual List<Int32> DuplicatedRows
        {
            get;
            set;
        }
        public ExternalRow()
        {
            Cells = new List<ExternalCell>();
            DuplicatedRows = new List<Int32>();
            AllowImport = true;
        }

        public Boolean isValid() {
            return Cells.Where(c => c.isValid).Count() == Cells.Count();
        }
        public ExternalCell GetCell(Int32 columnIndex)
        {
            return Cells.Where(c => c.Column.Number == columnIndex).FirstOrDefault();
        }
        public String GetCellValue(Int32 columnIndex)
        {
            ExternalCell cell = Cells.Where(c => c.Column.Number == columnIndex).FirstOrDefault();
            if (cell == null)
                return "";
            else
                return cell.Value;
        }
        //public ProfileAttributeCell GetCell(ProfileAttributeColumn column)
        //{
        //    return Cells.Where(c => c.Column == column).FirstOrDefault();
        //}
        //public String GetCellValue(ProfileAttributeType attribute)
        //{
        //    ProfileAttributeCell cell = Cells.Where(c => c.Column.Attribute == attribute).FirstOrDefault();
        //    if (cell == null)
        //        return "";
        //    else
        //        return cell.Value;
        //}
    }
}