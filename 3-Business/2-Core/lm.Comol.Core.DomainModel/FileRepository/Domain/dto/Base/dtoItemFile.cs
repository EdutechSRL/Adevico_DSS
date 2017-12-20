using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoItemFile
    {
        public long Id { get; set; }
        public ItemType Type { get; set; }
        public dtoDisplayRepositoryItem ItemReferrer { get; set; }
    }
}