using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.PolicyManagement
{
    public class RootObject
    {
        public static string ProfilePolicyUserControl()
        {
            return "Modules/ProfilePolicy/UC/UC_ProfilePolicy.ascx";
        }
        public static string AcceptLogonPolicy() 
        {
            return "Modules/ProfilePolicy/AcceptLogonPolicy.aspx";
        }
    }
}