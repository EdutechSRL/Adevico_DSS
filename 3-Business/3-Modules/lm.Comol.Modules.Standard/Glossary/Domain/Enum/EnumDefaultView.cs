using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary.Domain
{
    public enum DefaultView
    {
        NotSet = -1,
        MultiColumn = 1,
        AllDefinition = 2,
        SingleLetter = 10,   //Only a single letter per page
        AllLetter = 20       //ALL letter in a single page
    }
}
