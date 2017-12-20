using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.WebControls.Generic
{
    [Serializable]
    public class ProgressBarItem
    {
        private Double percentage { get; set; }
        public virtual Guid Id { get; set; }
        public virtual String CssClass { get; set; }
        public virtual Double Percentage { get { return percentage; } }
        public virtual long Value { get; set; }
        public virtual long Total { get; set; }
        public virtual String PercentageTranslation { get; set; }
        public virtual String ValueTranslation { get; set; }

        public lm.Comol.Core.DomainModel.ItemDisplayOrder DisplayOrder { get; set; }

        public ProgressBarItem() {
            DisplayOrder = DomainModel.ItemDisplayOrder.item;
            Id = Guid.NewGuid();
        }
        public ProgressBarItem(long value, long total) : this()
        {
            Value = value;
            Total = total;
            percentage = CalculatePercentage(value, total);
        }
        public ProgressBarItem(Int32 value, Int32 total)
            : this()
        {
            Value = (long)value;
            percentage = CalculatePercentage((long)value, (long)total);
        }
        public Int32 PercentageToInt(){
            return Convert.ToInt32(Math.Round(Percentage));
        }
        public String PercentageToString(){
            Double fractional = GetFractionalValue();
            if (fractional==0)
                return String.Format("{0:N0}", Percentage);
            else
                return String.Format("{0:N2}", Percentage);
        }
        public Double GetFractionalValue() { 
            return Percentage - Math.Floor(Percentage);
        }
        public String TranslatePercentage() {
            return PercentageTranslation + ": " + PercentageToString() + "%";
        }
        public String TranslateValue()
        {
            return ValueTranslation + ": " + Value.ToString() + "/" + Total.ToString();
        }
        private double CalculatePercentage(long value, long total)
        {
            return (total == 0) ? (double)0 : Math.Round(((value / (double)total) * 100), 2);
        }

        public void SetPercentage(Double value)
        {
            percentage = value;
        }
    }
}