using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoFieldOption : dtoBase
    {
        public virtual long DisplayOrder { get; set; }
        public virtual String Name { get; set; }
        public virtual String Value { get; set; }
        public virtual long IdField { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsFreeValue { get; set; }

        public dtoFieldOption()
            : base()
        {
        }

        public dtoFieldOption(long id, String name, String value, long display, Boolean isDefault, Boolean isFreeValue)
            : base()
        {
            Id = id;
            DisplayOrder = display;
            Name = name;
            Value = value;
            IsDefault = isDefault;
            IsFreeValue = isFreeValue;
        }
        public dtoFieldOption(long id, String name, String value, long display, long idField, Boolean isDefault, Boolean isFreeValue)
            : base()
        {
            Id = id;
            DisplayOrder = display;
            Name = name;
            Value = value;
            IdField = idField;
            IsDefault = isDefault;
            IsFreeValue = isFreeValue;
        }
        public dtoFieldOption(FieldOption option)
            : base()
        {
            Id = option.Id;
            Deleted = option.Deleted;
            DisplayOrder = option.DisplayOrder;
            Name = option.Name;
            Value = option.Value;
            IdField = option.Field.Id;
            IsDefault = option.IsDefault;
            IsFreeValue = option.IsFreeValue;
        }
    }
}