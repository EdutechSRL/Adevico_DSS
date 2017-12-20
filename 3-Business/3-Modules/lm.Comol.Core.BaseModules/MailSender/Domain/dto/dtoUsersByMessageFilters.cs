using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.MailSender
{
    [Serializable]
    public class dtoUsersByMessageFilters
    {
        public virtual UserByMessagesOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual String StartWith { get; set; }
        public virtual lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy SearchBy { get; set; }
        public virtual String Value { get; set; }
        public virtual long IdAgency { get; set; }
        public virtual Dictionary<Int32, String> ProfyleTypeTranslations { get; set; }
        public virtual Dictionary<Int32, String> RoleTranslations { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public dtoUsersByMessageFilters()
        {
            ProfyleTypeTranslations = new Dictionary<Int32, String>();
            RoleTranslations = new Dictionary<Int32, String>();
        }
    }  
}