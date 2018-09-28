using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_TermMap
    {
        public DTO_TermMap()
        {
        }

        public DTO_TermMap(liteTermMap liteTermMap)
        {
            Id = liteTermMap.Id;
            IdGlossary = liteTermMap.IdGlossary;
            Name = liteTermMap.Name;
            FirstLetter = liteTermMap.FirstLetter;
            IsPublished = liteTermMap.IsPublished;
        }

        public Int64 Id { get; set; }
        public String Name { get; set; }
        public bool IsPublished { get; set; }
        public Int64 IdGlossary { get; set; }
        public Char FirstLetter { get; set; }
        public String CssClass { get; set; }
    }
}