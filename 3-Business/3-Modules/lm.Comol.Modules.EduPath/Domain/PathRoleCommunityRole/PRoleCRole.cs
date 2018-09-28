using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class PRoleCRole : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        
        public virtual Path Path { get; set; }
        public virtual RoleEP RoleEP { get; set; }
        public virtual Role RoleCommunity { get; set; }
        public virtual liteCommunity Community { get; set; }  

        public PRoleCRole() 
        { }
    }
}
