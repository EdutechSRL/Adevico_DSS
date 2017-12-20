using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum AttributeType
    {
        unknown = 0,
        externalId = 1,
        accountId = 2,
        name = 3,
        surname = 4,
        mail = 5,
        telephoneNumber = 6,
        fax = 7,
        im = 8,
        mobile = 9,
        taxCode = 10,
        login = 11,
        password = 12,
        externalRoles = 13,
        externalProfileId = 14,
        sessionId = 15
    }
}
