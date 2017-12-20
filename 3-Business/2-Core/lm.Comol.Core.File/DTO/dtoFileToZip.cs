using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.File
{    
    [Serializable]
    public class dtoFileToZip
    {            
        
        public virtual String StoredName { get; set; }
        public virtual String FileName { get; set; }

        public dtoFileToZip() { }
        public dtoFileToZip(String storedName, String displayName, ref int index, int count)
        {
            StoredName = storedName;
            FileName = index.ZeroPadFromCount(count) + " - " + displayName;
            index++;
        }
    }
}
