using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    [Serializable]
    public class DTO_ListVersionPermission
    {
        //public virtual Boolean SetDefinitive { get; set; }
        public virtual Boolean Activate { get; set; }
        public virtual Boolean DeActivate { get; set; }

        public virtual Boolean DeleteVirtual { get; set; }
        public virtual Boolean UndeleteVirtual { get; set; }
        public virtual Boolean DeletePhisical { get; set; }

        public virtual Boolean AllowNewVersion { get; set; }

        public virtual Boolean Edit { get; set; }
        public virtual Boolean Copy { get; set; }
        public virtual Boolean Preview { get; set; }
    }
}
