using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class ProfileAssignment : DomainBaseObjectLiteMetaInfo<long>
    {
       public virtual int IdProfileType { get; set; }
       public virtual Menubar Menubar { get; set; }
       public virtual BaseMenuItem ItemOwner { get; set; }
    }
}