using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Domain
{
    [Serializable]
    public class dtoItemToEvaluate
    {
        public long Idlink { get; set; }
        public long IdItem { get; set; }
        public long IdVersion { get; set; }
        public Boolean AlwaysLastVersion { get; set; }
        public dtoItemToEvaluate()
        {

        }
        public dtoItemToEvaluate(long idLink, long idItem, long idVersion)
        {
            Idlink = idLink;
            IdItem = idItem;
            IdVersion = idVersion;
            AlwaysLastVersion = (idVersion==0);
        }
    }
}