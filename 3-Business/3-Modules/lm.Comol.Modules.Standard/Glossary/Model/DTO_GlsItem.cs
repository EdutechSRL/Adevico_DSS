using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary
{
    [Serializable]
    public class DTO_GlsItem
    {
        public Int64 ID { get; set; }
        public String Term { get; set; }
        public String Definition { get; set; }
        public DTO_GlsGroup Group { get; set; }

        public DTO_GlsItem()
        {
            ID = 0;
            Term = "";
            Definition = "";
            Group = new DTO_GlsGroup();
        }
    }
}
