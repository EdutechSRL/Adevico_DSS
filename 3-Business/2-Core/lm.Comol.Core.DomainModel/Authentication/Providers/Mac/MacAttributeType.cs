using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum UrlMacAttributeType
    {
        url = 0,
        mac = 1,
        timestamp = 2,
        profile = 3,
        organization = 4,
        coursecatalogue = 5,
        applicationId = 6,
        functionId = 7,
        compositeProfile = 8
    }
}
