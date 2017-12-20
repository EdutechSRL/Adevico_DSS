using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoImportSettings
    {
        public virtual KeyValuePair<long,String> DefaultAgency { get; set; }
        public virtual Boolean AddTaxCode { get; set; }
        public virtual Boolean AddPassword { get; set; }
        public virtual Boolean AutoGenerateLogin { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public virtual long IdProvider { get; set; }
        public virtual Int32 DefaultNationId { get; set; }
        public virtual Int32 DefaultProvinceId { get; set; }
    }
}