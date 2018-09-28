using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoMenubarPermission : dtoPermission 
    {
        public virtual bool SetActive { get; set; }
    }
}