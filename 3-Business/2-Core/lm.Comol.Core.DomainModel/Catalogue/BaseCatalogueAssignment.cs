using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Catalogues
{
    [Serializable()]
    public class BaseCatalogueAssignment : DomainBaseObjectMetaInfo<long> 
    {
        public virtual Boolean FromProvider { get; set; }
        public virtual Boolean Allowed { get; set; }
        public virtual Catalogue Catalogue { get; set; }
    }

    [Serializable()]
    public class CatalogueCommunityAssignment : BaseCatalogueAssignment
    {
        public virtual Community AssignedTo { get; set; }
    }
    [Serializable()]
    public class CataloguePersonAssignment : BaseCatalogueAssignment
    {
        public virtual Person AssignedTo { get; set; }
    }

    [Serializable()]
    public class CataloguePersonTypeAssignment : BaseCatalogueAssignment
    {
        public virtual int AssignedTo { get; set; }
    }

    [Serializable()]
    public class CatalogueRoleAssignment : BaseCatalogueAssignment
    {
        public virtual Community Community { get; set; }
        public virtual Role AssignedTo { get; set; }
    }
}