
using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ProfileAttributesRow 
    {
        public virtual List<ProfileAttributeCell> Cells { get; set; }
        public virtual String DisplayName {
            get{
                if (Cells.Count == 0)
                    return "";
                else {
                    return Cells.Where(c => c.Column.Attribute == ProfileAttributeType.surname).Select(c=>c.Value).FirstOrDefault() + ' ' + Cells.Where(c => c.Column.Attribute == ProfileAttributeType.name).Select(c=>c.Value).FirstOrDefault();
                }
            }
        }
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
        public virtual Boolean Selectable
        {
            get
            {
                return AllowImport && isValid() && !HasDBDuplicatedValues && !HasDuplicatedValues; 
            }

        }
        public virtual List<Int32> DuplicatedRows
        {
            get;
            set;
        }
        public ProfileAttributesRow() {
            Cells = new List<ProfileAttributeCell>();
            DuplicatedRows = new List<Int32>();
            AllowImport = true;
        }

        public Boolean isValid() {
            return Cells.Where(c => c.isValid).Count() == Cells.Count();
        }

        public ProfileAttributeCell GetCell(ProfileAttributeColumn column)
        {
            return Cells.Where(c => c.Column == column).FirstOrDefault();
        }
        public String GetCellValue(ProfileAttributeType attribute)
        {
            ProfileAttributeCell cell = Cells.Where(c => c.Column.Attribute == attribute).FirstOrDefault();
            if (cell == null)
                return "";
            else if (cell.Value.Contains(lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator)){
                return cell.Value.Replace(lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator, ", ");
            }
            else
                return cell.Value;
        }
        public KeyValuePair<long,String> GetCalculatedCellLongValue(ProfileAttributeType attribute)
        {
            ProfileAttributeComputedCell cell = Cells.Where(c =>c.GetType() == typeof( ProfileAttributeComputedCell) && c.Column.Attribute == attribute).Select(c=>( ProfileAttributeComputedCell)c).FirstOrDefault();
            if (cell == null)
                return new KeyValuePair<long, String>();
            else
                return new KeyValuePair<long, String>(cell.InternallLongValue,cell.DisplayValue);
        }
    }
}