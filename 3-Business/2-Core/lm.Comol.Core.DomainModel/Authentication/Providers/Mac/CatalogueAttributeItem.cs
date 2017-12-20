using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class CatalogueAttributeItem : DomainBaseObject<long>
    {
        public virtual CatalogueAttribute Owner { get; set; }
        public virtual String RemoteCode { get; set; }
        public virtual lm.Comol.Core.Catalogues.Catalogue Catalogue { get; set; }

        public CatalogueAttributeItem()
        {
            
        }
    } 
}