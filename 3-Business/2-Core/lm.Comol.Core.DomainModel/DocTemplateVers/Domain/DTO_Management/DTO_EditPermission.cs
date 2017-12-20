using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    [Serializable]
    public class DTO_EditPermission
    {
        public virtual Boolean Preview { get; set; }
        public virtual Boolean List { get; set; }
        public virtual Boolean Export { get; set; }

        public virtual Boolean SetDefinitive { get; set; }
        public virtual Boolean ModifySettings { get; set; }
        public virtual Boolean ModifyPageElement { get; set; }
        public virtual Boolean GetSkin { get; set; }
        //...

    }
}
