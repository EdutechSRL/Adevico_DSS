using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoNodeFolderItem 
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public NodeType Type { get; set; }
        public Boolean IsCurrent { get; set; }
        public Boolean IsEmpty { get; set; }
        public Boolean Selected { get; set; }
        public Boolean Selectable { get; set; }
        public Boolean IsHome { get; set; }
        public dtoNodeFolderItem()
        {
        }

        public String ToString()
        {
            return Type.ToString() + Id.ToString() + " " + Name + " current:" + IsCurrent.ToString() + " selectable:" + Selectable.ToString();
        }
    }
}