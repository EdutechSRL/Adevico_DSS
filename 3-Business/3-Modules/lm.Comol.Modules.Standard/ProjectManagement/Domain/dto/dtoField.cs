using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoField<T>
    {
        public virtual T Current { get; set; }
        public virtual T Edit { get; set; }
        public virtual T Init { get; set; }
        public virtual Boolean InEditMode { get; set; }
        public virtual Boolean IsUpdated { get; set; }
        public virtual Boolean WillBeUpdated { get; set; }
        public virtual FieldStatus Status { get; set; }
        public dtoField() { }
        public dtoField(T value){
            Init = value;
            Current = value;
            InEditMode = false;
        }
        public dtoField(T init, T current, T edit)
        {
            Init = init;
            Current = current;
            Edit = edit;
        }
        public dtoField(T init, T current, T edit, Boolean inEditMode, Boolean isUpdated, Boolean willBeUpdated)
        {
            Init = init;
            Current = current;
            Edit = edit;
            InEditMode = inEditMode;
            IsUpdated = isUpdated;
            WillBeUpdated = willBeUpdated;
        }

        public T GetValue() {
            return (InEditMode) ? Edit : Current;
        }

    }
}