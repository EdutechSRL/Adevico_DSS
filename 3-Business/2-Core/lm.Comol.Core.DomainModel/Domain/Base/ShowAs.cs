using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable, Flags]
    public enum ShowAs
    {
        None = 0,
        List = 1,
        PopupWindow = 2,
        ModalWindow =4,
        SinglePage = 8,
        Memo = 16
    }
}
