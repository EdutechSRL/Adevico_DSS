using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication{
    [Serializable]
    public enum AuthenticationResult
    {
        InternalError = 0,
        Authenticated = 1,
        AuthenticatedProfileToCreate = 2,
        AuthenticatedProfileCreated = 3,
        AuthenticationExpired =4,
        NotAuthenticated =5,
        UserNotFound = 6,
        UnableToConnectToProvider = 7,
        UserDisabled =8
    };
}