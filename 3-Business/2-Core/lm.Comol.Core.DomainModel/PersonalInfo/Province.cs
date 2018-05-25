using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.PersonalInfo
{
    public class Province
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
    }
}
