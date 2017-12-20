
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class DestinationItem<T>
    {
        public virtual T Id{ get; set; }
        public virtual String Name{ get; set; }
        public virtual String ColumnName { get; set; }
        public virtual InputType InputType { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual List<T> AlternativeAttributes { get; set; }
        public virtual Boolean HasAlternative { get { return !(AlternativeAttributes == null || AlternativeAttributes.Count == 0); } }

        public DestinationItem()
        {
            InputType = Helpers.InputType.strings;
        }
    }
    [Serializable]
    public class DestinationItemCells<T> : DestinationItem<T>
    {
        public virtual List<ExternalCell> Cells { get; set; }
        public DestinationItemCells()
        {
            Cells = new List<ExternalCell>();
        }

        public DestinationItemCells(DestinationItem<T> item)
        {
            Cells = new List<ExternalCell>();
            Id = item.Id;
            Name = item.Name;
            ColumnName = item.ColumnName;
            InputType = item.InputType;
            Mandatory = item.Mandatory;
            AlternativeAttributes = item.AlternativeAttributes;
        }
    }
}