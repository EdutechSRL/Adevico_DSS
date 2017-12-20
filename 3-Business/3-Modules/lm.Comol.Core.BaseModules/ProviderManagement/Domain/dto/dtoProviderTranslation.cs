using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoProviderTranslation
    {
        public virtual long Id { get; set; }
        public virtual long IdAuthenticationProvider { get; set; }
        public virtual Int32 idLanguage { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String ForSubscribeName { get; set; }
        public virtual String ForSubscribeDescription { get; set; }
        public virtual String FieldLong { get; set; }
        public virtual String FieldString { get; set; }
    }
}