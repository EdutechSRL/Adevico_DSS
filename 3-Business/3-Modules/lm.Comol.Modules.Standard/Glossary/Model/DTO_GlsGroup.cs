using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary
{
    public class DTO_GlsGroup
    {
        public Int64 ID { get; set; }
        public virtual String Name { get; set; }
        //...
        //Eventualmente lista di Glossary term
    }
}
