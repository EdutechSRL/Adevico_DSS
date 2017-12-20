using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.DocTemplate
{
    [Serializable]
    public class dtoModulePlaceHolder
    {
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual String Name { get; set; }
        public virtual List<TranslatedItem<String>> Attributes { get; set; }

        public dtoModulePlaceHolder(){
            Attributes = new List<TranslatedItem<String>>();
        }
    }
}
