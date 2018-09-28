using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Base.DomainModel;

namespace lm.Comol.Modules.Standard.Faq
{
    public class BaseFaqModel
    {
        public Enum.ErrorCode ErrorCode { get; set; }
        public ModuleFaq Module { get; set; }
    }
}
