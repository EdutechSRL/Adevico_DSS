using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class CompanyInfo
    {
        public virtual String Name { get; set; }
        public virtual String Address { get; set; }
        public virtual String City { get; set; }
        public virtual String Region { get; set; }
        public virtual String TaxCode { get; set; }
        public virtual String ReaNumber { get; set; }
        public virtual String AssociationCategories { get; set; }
    }
}