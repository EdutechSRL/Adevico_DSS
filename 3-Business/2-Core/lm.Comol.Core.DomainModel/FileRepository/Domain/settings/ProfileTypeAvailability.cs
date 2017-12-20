using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class ProfileTypeAvailability : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Int32 IdProfileType { get; set; }
        public ProfileTypeAvailability()
        {

        }
    }
}