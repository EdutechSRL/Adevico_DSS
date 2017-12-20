using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication.Helpers
{
    public class EncryptionInfo
    {
        public virtual EncryptionAlgorithm EncryptionAlgorithm { get; set; }
        public virtual String Key { get; set; }
        public virtual String InitializationVector { get; set; }
    }
}