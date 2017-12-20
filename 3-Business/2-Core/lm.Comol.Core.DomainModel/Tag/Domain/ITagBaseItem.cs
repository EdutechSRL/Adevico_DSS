using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    public interface ITagBaseItem 
    {
        TagItem Tag { get; set; }
        lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }
    }
}
