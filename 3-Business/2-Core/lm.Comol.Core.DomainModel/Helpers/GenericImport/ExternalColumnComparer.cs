using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ExternalColumnComparer<S,T> : ExternalColumnItem<S, DestinationItem<T>>
    {
        public virtual Boolean isValid { get { return this.DestinationColumn.InputType != InputType.skip && this.DestinationColumn.InputType != InputType.unknown; } }
    }
}