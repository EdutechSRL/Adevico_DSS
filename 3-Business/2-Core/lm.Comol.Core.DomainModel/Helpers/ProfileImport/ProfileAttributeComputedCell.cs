
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ProfileAttributeComputedCell : ProfileAttributeCell 
    {
        bool invalid = false;
        public virtual ProfileAttributesRow Row { get; set; }
        public virtual String DisplayValue { get; set; }
        public virtual long InternallLongValue { get; set; }
        public virtual Int32 InternalIntValue { get; set; }
        public virtual Boolean isValid
        {
            get {
                Boolean result = ((Column.AllowEmpty || (!Column.AllowEmpty && !isEmpty))); // && (Column.AllowDuplicate || (!Column.AllowDuplicate && !isDuplicate)));
                switch (Column.Attribute) {
                    case Authentication.ProfileAttributeType.agencyInternalCode:
                        result = (InternallLongValue>0);
                        break;
                }
                return result; //(!Column.AllowEmpty && !isEmpty) && (!Column.AllowDuplicate && !isDuplicate);
            }
        }
        public virtual Boolean isEmpty
        {
            get
            {
                return (InternallLongValue == 0 && InternalIntValue==0);
            }
        }
        public ProfileAttributeComputedCell()
        {
            DuplicateOf = new List<Int32>();
        }
        public ProfileAttributeComputedCell(ProfileComputedColumn column)
        {
            DuplicateOf = new List<Int32>();
            Column = column;
        }
    }
}