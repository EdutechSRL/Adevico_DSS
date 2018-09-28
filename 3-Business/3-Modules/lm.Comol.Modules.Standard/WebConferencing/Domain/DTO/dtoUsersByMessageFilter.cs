using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    [Serializable]
    public class dtoUsersByMessageFilter
    {
        public virtual UserByMessagesOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual MailStatus MailStatus { get; set; }
        public virtual String StartWith { get; set; }
        public virtual lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy SearchBy { get; set; }
        public virtual String Value { get; set; }
        public virtual UserTypeFilter UserType { get; set; }
        public virtual long IdAgency { get; set; }
        public virtual List<long> IdMessages { get; set; }
        public virtual UserStatus UserStatus { get; set; }
        public virtual Dictionary<UserTypeFilter, String> TypeTranslations { get; set; }
        public virtual Dictionary<UserStatus, String> UserStatusTranslations { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public dtoUsersByMessageFilter()
        {
            TypeTranslations = new Dictionary<UserTypeFilter, String>();
            UserStatusTranslations = new Dictionary<UserStatus, String>();
            UserType = UserTypeFilter.All;
            UserStatus = Domain.UserStatus.All;
        }
    }  
}
