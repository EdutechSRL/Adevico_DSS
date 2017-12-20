using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Federation
{
    public class Enums
    {
        public enum FederationType : int
        {
            None = 0,
            TrentinoSviluppo = 4
        }

        public enum FederationResult : int
        {
            UserNotFederated = -8,
            CommunityNotFound = -4,
            UserNotFound = -2,
            Unknow = 0,
            CommunityNotFederated = 2,
            Federated = 4
        }
    }
}
