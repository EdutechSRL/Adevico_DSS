using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable]
    public enum AlgorithmType : int 
    {
        none = 0,
        weightedAverage = 1,
        owa = 2,
        topsis = 3,
        ahp = 4,
        //consenso = 5,
        //choquuet = 6,
        //sum = 7
    }
}