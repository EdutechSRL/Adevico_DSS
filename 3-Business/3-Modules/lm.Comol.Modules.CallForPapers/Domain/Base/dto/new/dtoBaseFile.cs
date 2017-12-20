using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoBaseFile : dtoBase
    {
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItem Item { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.liteRepositoryItemVersion Version { get; set; }
        public long ModuleLinkId { get; set; }
        public liteModuleLink Link { get; set; }
        public dtoBaseFile() :base() {
        }
    }
}
