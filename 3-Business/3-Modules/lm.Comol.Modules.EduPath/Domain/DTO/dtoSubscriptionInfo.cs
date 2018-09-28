using System;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoSubscriptionInfo
    {
        public Int32 Count { get; set; }
        public Int32 IdRole { get; set; }
        public String Name { get; set; }


        public dtoSubscriptionInfo()
        {
            Count = 0;
            Name = "";
            IdRole = 0;
        }
    }
}
