using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    public class DTO_TermEmbed
    {
        public DTO_TermEmbed(liteTerm liteTerm)
        {
            IdCommunity = liteTerm.IdCommunity;
            IdGlossary = liteTerm.IdGlossary;
            IdTerm = liteTerm.Id;
            Name = liteTerm.Name;
            Description = liteTerm.Description;
        }

        public Int32 IdCommunity { get; set; }
        public Int64 IdGlossary { get; set; }
        public Int64 IdTerm { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }
}