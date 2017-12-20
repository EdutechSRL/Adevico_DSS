using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class ItemObjectTranslation
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String ShortName { get; set; }
        public virtual String GoodFor { get; set; }
        public virtual String BadFor { get; set; }

        public ItemObjectTranslation()
        {

        }

        public ItemObjectTranslation Copy()
        {
            return new ItemObjectTranslation() { Name = this.Name, Description = this.Description, GoodFor = this.GoodFor, BadFor = this.BadFor };
        }
        public Boolean IsValid()
        {
            return (!String.IsNullOrEmpty(Name));
        }

        public Boolean IsEmpty()
        {
            return String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Description);
        }
    }
}
