using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication.Helpers
{
    [Serializable]
    public enum EncryptionAlgorithm
    {
        Des = 1,
        Rc2 = 2,
        Rijndael = 3,
        TripleDes = 4,
        Md5 = 5,
        None= 0
    };
}
