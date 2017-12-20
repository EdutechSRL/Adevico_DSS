using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Catalogues
{
    [Serializable]
    public class CatalogueAffiliation : DomainBaseObject<long>
    {
        public virtual Catalogue Catalogue { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        
        public CatalogueAffiliation() { }
    }

}
