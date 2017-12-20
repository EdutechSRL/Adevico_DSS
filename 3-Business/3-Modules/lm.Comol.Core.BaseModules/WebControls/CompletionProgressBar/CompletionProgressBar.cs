using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.WebControls.Generic
{
    [Serializable]
    public class AdvancedProgresBar
    {
        public virtual long itemsCount { get; set; }
        public virtual long ItemsCount { get { return itemsCount; } }
        public virtual List<ProgressBarItem> Items { get; set; }
        public AdvancedProgresBar(long count) {
            Items = new List<ProgressBarItem>();
            itemsCount = count;
        }
        public virtual void NormalizeItems() {
            if (Items.Any() && Items.Select(i => i.Percentage).Sum() < 100) {
                ProgressBarItem maxItem = Items.Where(i => i.Value > 0 && i.GetFractionalValue() > 0).OrderByDescending(i => i.Percentage).FirstOrDefault();
                if (maxItem == null)
                    maxItem = Items.Where(i => i.Value > 0).OrderBy(i => i.Percentage).FirstOrDefault();
                if (maxItem != null)
                    NormalizeItem(maxItem, Items.Where(i=> i.Id != maxItem.Id).Select(i=> i.Percentage).Sum());
            }
        }
        private void NormalizeItem(ProgressBarItem maxItem, double otherItemsSum)
        {
            maxItem.SetPercentage(100 - otherItemsSum);
        }
    }
}