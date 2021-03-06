﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public class ModuleAssociation :  DomainBaseObjectMetaInfo<long>
    {
        public virtual long IdSkin { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual Boolean IsPortal { get; set; }
        public virtual Int32 OwnerTypeID { get; set; }
        public virtual long OwnerLongID { get; set; }
        public virtual String OwnerFullyQualifiedName { get; set; }
    }
}