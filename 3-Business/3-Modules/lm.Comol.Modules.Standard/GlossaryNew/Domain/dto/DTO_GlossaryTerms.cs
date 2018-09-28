using System;
using System.Collections.Generic;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_ImportCommunity
    {
        public DTO_ImportCommunity()
        {
            GlossaryList = new List<DTO_ImportGlossary>();
        }

        public virtual Int64 IdCommunity { get; set; }
        public virtual String Name { get; set; }
        public List<DTO_ImportGlossary> GlossaryList { get; set; }
    }

    [Serializable]
    public class DTO_ImportGlossary
    {
        public DTO_ImportGlossary()
        {
            TermList = new List<DTO_ImportTerm>();
        }

        public virtual Int64 Id { get; set; }
        public virtual String Name { get; set; }
        public List<DTO_ImportTerm> TermList { get; set; }
    }

    [Serializable]
    public class DTO_ImportTerm
    {
        public virtual Int64 Id { get; set; }
        public virtual String Name { get; set; }
    }
}