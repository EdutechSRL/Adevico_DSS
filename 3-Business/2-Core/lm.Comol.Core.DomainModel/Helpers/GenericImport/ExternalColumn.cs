
using System;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ExternalColumn
    {
        public virtual InputType InputType { get; set; }
        public virtual String Name { get; set; }
        public virtual int Number { get; set; }
        public virtual Boolean AllowEmpty { get; set; }
        public virtual Boolean AllowDuplicate { get; set; }
        public virtual Boolean ValidateValue
        {
            get
            {
                return (InputType == InputType.mail || InputType == InputType.int16 || InputType == InputType.int32 || InputType == InputType.int64 || InputType == InputType.datetime);
            }
        }

        public ExternalColumn(InputType inputType)
        {
            InputType = inputType;
            AllowEmpty = (inputType != InputType.mail);
            AllowDuplicate = (inputType != InputType.login && inputType != InputType.mail && inputType != InputType.taxCode);
        }
    }
}