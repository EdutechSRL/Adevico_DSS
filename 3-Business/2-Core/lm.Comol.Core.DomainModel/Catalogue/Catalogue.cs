using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Catalogues
{
    [Serializable]
    public class Catalogue : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean isEnabled { get; set; }
        public virtual IList<BaseCatalogueItem> Items { get; set; }
        public virtual IList<BaseCatalogueAssignment> Assignments { get; set; }
        public virtual IList<CatalogueAffiliation> Affiliations { get; set; }
        public Catalogue() {
            Items = new List<BaseCatalogueItem>();
            Assignments = new List<BaseCatalogueAssignment>();
            Affiliations = new List<CatalogueAffiliation>();
          //  Organizations = new List<Organization>();
        }
    }
}