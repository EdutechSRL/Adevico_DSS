using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class GenericEncryption : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual MacType Type { get; set; }
        public virtual lm.Comol.Core.Authentication.Helpers.EncryptionInfo EncryptionInfo { get; set; }
        public virtual String ExternalIdentifier { get; set; }
        public virtual Boolean VerifyTimeout { get; set; }
    }
    [Serializable]
    public enum MacType: int
    {
       Url = 0
    }
}