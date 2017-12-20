using System;
using System.Linq;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ColumnComparer <S,D>
    {
         public virtual int Number { get; set; }
         public virtual S SourceColumn { get; set; }
         public virtual D DestinationColumn { get; set; }
    }
}