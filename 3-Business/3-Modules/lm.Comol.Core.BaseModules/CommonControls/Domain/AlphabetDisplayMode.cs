using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommonControls
{
    [Serializable,Flags]
    public enum AlphabetDisplayMode
    {
        none = 0,
        commonletters = 1,
        extendedletters = 2,
        addUnmatchLetters = 4,
        numbers = 8,
        otherCharsItem = 16,
        allCharsItem = 32,
    }
}
