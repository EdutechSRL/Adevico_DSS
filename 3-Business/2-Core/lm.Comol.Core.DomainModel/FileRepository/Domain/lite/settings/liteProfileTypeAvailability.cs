using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class liteProfileTypeAvailability : DomainBaseObject<long>
    {
        public virtual Int32 IdProfileType { get; set; }
        public liteProfileTypeAvailability()
        {

        }
    }
}