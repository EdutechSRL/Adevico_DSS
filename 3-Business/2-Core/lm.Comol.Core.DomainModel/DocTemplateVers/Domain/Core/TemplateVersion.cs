using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using lm.Comol.Core.DomainModel.DocTemplate;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    [Serializable]
    public class TemplateVersion : lm.Comol.Core.DomainModel.DomainBaseObjectMetaInfo<Int64>
    {
        public virtual Boolean IsActive { get; set; }
        public virtual Boolean IsDraft { get; set; }
        public virtual Template Template { get; set; }
        public virtual int Version { get; set; }        //Indica la versione in relazione ad un template. Creato alla creazione della version e più modificato.
        public virtual Int64 SubVersion { get; set; }   //Indica la sottoversione di una Versione. Incrementato ad ogni modifica alla versione.

        public virtual IList<Settings> Settings { get; set; }
        public virtual IList<PageElement> Elements { get; set; }
        

        public virtual Boolean HasSignatures { get; set; }
        public virtual IList<Signature> Signatures { get; set; }

    }
}
