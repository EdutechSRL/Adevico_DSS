using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Catalogues
{
    [Serializable]
    public class BaseCatalogueItem : DomainBaseObjectMetaInfo<long>
    {
        public virtual Catalogue Catalogue { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean isEnabled { get; set; }
        public virtual CatalogueItemType ItemType { get; set; }

        public BaseCatalogueItem() { }
    }

    [Serializable]
    public class CommunityCatalogueItem : BaseCatalogueItem
    {
        public virtual Community Community { get; set; }
        public CommunityCatalogueItem() { 
        }
    }

    [Serializable]
    public enum CatalogueItemType 
    {
        none = 0,
        Community = 1,
        Module = 2,
    }
}
