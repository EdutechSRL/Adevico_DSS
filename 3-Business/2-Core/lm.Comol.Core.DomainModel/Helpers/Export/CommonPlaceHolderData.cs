using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public class CommonPlaceHolderData
    {
        public Person Person { get; set; }
        public liteCommunity Community { get; set; }
        public Subscription Subscription { get; set; }
        public string OrganizationName { get; set; }
        public string UserType { get; set; }
        public string InstanceName { get; set; }

        public object ModuleObject { get; set; }
    }

    //[Serializable()]
    //public enum CommonPlaceHoldersType
    //{
    //    None = 0,
    //    CommunityName = 1,
    //    UserName = 2,
    //    RoleName = 3,
    //    ProfileType = 4,
    //    SubscriptionOn = 5,
    //    //CommunityType = 6,
    //    OrganizationName = 7,
    //    IstanceName = 8,
    //    UserMail = 10,
    //    TaxCode = 11,
    //    CompanyName = 20,
    //    CompanyTax = 21,
    //    CompanyRea = 22
    //}
}
