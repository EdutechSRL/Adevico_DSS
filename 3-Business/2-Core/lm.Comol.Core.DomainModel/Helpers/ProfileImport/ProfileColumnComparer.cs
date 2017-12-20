using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ProfileColumnComparer <S> : ColumnComparer<S, ProfileAttributeType>
    {
        public virtual Boolean isValid { get { return this.DestinationColumn != ProfileAttributeType.skip && this.DestinationColumn != ProfileAttributeType.unknown; } }
    }
}