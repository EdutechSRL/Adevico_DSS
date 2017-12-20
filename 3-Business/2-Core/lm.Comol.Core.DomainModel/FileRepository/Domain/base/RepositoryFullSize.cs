using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class RepositoryFullSize :   lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual long Size { get; set; }
        public virtual long VersionsSize { get; set; }
        public virtual long DeletedSize { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }
        public RepositoryFullSize()
        {
            Repository = new RepositoryIdentifier();
        }
    }
}