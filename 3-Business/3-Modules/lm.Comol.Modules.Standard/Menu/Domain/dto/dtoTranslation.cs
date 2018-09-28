using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoTranslation
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual String Name { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual long IdMenuItem { get; set; }
    }
}