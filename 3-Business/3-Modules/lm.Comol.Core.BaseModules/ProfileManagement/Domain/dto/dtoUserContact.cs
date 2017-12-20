using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoUserContact
    {
        public Int32 Id { get; set; }
        public Int32 IdProfileType { get; set; }
        public Int32 IdRole { get; set; }
        public long IdAgency { get; set; }
        public String Name { get; set; }
        public String RoleName { get; set; }
        public String ProfileName { get; set; }
        public String AgencyName { get; set; }
        public String Surname { get; set; }
        public String Mail { get; set; }
        public String DisplayName { get { return Surname + " " + Name; } }
        public String FirstLetter { get; set; }
        public List<dtoIstantMessaging> IMcontacts { get; set; }
        public SubscriptionStatus Status { get; set; }
        public dtoUserContact() {
            IMcontacts = new List<dtoIstantMessaging>();
            Status = SubscriptionStatus.none;
        }
        public void UpdateAgencyInfo(AgencyAffiliation affiliation) {
            if (affiliation!=null && affiliation.Agency !=null){
                IdAgency = affiliation.Agency.Id;
                AgencyName = affiliation.Agency.Name;
            }
        }
        
    }
}
