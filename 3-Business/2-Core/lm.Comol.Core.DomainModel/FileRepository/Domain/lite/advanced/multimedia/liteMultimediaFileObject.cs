using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
     [Serializable]
    public class liteMultimediaFileObject : liteBaseItemIdentifiers
    {
        public virtual long IdItemTransfer { get; set; }
        public virtual String Fullname { get; set; }
        public virtual Boolean IsDefaultDocument { get; set; }
        public virtual Single Probability { get; set; }
    }
}