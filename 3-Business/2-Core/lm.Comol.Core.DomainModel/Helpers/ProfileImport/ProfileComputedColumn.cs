
using System;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ProfileComputedColumn : ProfileAttributeColumn
    {
        public virtual List<ProfileAttributeType> Attributes { get; set; }

        public ProfileComputedColumn() {
            Type = Helpers.ColumnType.computed;
            Attributes = new List<ProfileAttributeType>();
        }
        public ProfileComputedColumn(List<ProfileAttributeType> attributes)
        {
            Type = Helpers.ColumnType.computed;
            Attributes = (attributes == null) ? new List<ProfileAttributeType>() : attributes;
        }
    }
}