using System;
using System.Collections.Generic;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class GlossaryContainer
    {
        public List<GlossarySerialized> Glossaries;
        public List<TermSerialized> Terms;
    }
}