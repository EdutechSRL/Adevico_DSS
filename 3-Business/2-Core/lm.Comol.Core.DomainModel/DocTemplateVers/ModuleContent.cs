using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    /// <summary>
    /// Messa ad ABSTRACT solo per ricordare che a livello di template bastano QUESTI dati,
    /// ma poi il RENDER andrà implementato a livello di Servizio...
    /// </summary>
    public class ModuleContent : lm.Comol.Core.DomainModel.DomainBaseObject<Int64>
    {
        //public virtual Int64 Id { get; set; }
        public virtual Int64 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual String ModuleName { get; set; }
        public virtual Template Template { get; set; }
        //public virtual TextElement Content { get; set; }
    }
}
