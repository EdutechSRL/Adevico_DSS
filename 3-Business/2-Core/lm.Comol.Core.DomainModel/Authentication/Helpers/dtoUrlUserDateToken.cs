using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication.Helpers
{
    public class dtoUrlUserDateToken
    {
        public string Login { get; set; }
        public DateTime Data { get; set; }
        public virtual String ExceptionString { get; set; }
    }
}
