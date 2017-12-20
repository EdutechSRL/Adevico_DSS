using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public enum TranslationType : int 
    {
        method = 0,
        ratingset = 1,
        ratingvalue = 2,
    }
}