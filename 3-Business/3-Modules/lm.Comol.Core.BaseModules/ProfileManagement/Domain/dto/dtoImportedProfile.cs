using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoImportedProfile
    {
        public virtual PersonInfo Info { get; set; }
        public virtual dtoBaseProfile Profile { get; set; }
        public dtoImportedProfile() {

        }

    }
}
