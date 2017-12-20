using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.PolicyManagement
{
    public class MandatoryError : Exception
    {
        public List<dtoUserDataPolicy> Items{get;set;}
        public MandatoryError()
        {
        }

        public MandatoryError(string message)
            : base(message)
        {
        }

        public MandatoryError(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
}
