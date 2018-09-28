using System;
using System.Collections.Generic;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    public class DTO_TermsEmbed
    {
        public DTO_TermsEmbed()
        {
            TermEmbeds = new List<DTO_TermEmbed>();
        }

        public Int32 IdCommunity { get; set; }
        public Int64 IdGlossary { get; set; }
        public String GlossaryName { get; set; }
        public List<DTO_TermEmbed> TermEmbeds { get; set; }
    }
}