using System;
using System.ComponentModel.DataAnnotations;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_Term
    {
        public DTO_Term()
        {
        }

        public DTO_Term(liteTerm liteTerm)
        {
            Id = liteTerm.Id;
            Name = liteTerm.Name;
            Description = liteTerm.Description;
            IdGlossary = liteTerm.IdGlossary;
            IsPublished = liteTerm.IsPublished;
            FirstLetter = liteTerm.FirstLetter;
            IdCommunity = liteTerm.IdCommunity;

            if (liteTerm.ModifiedOn.HasValue)
                ModifiedOn = liteTerm.ModifiedOn.Value;

            if (liteTerm.ModifiedBy != null)
                ModifiedBy = liteTerm.ModifiedBy.SurnameAndName;
        }

        public DTO_Term(liteTermSearch liteTerm)
        {
            Id = liteTerm.Id;
            Name = liteTerm.Name;
            Description = liteTerm.Description;
            DescriptionText = liteTerm.DescriptionText;
            IdGlossary = liteTerm.IdGlossary;
            IsPublished = liteTerm.IsPublished;
            FirstLetter = liteTerm.FirstLetter;
            IdCommunity = liteTerm.IdCommunity;

            if (liteTerm.ModifiedOn.HasValue)
                ModifiedOn = liteTerm.ModifiedOn.Value;

            if (liteTerm.ModifiedBy != null)
                ModifiedBy = liteTerm.ModifiedBy.SurnameAndName;
        }

        public Int64 Id { get; set; }
        public Int32 IdCommunity { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ErrorTermName")]
        public String Name { get; set; }

        public String GlossaryName { get; set; }
        public String Description { get; set; }
        public String DescriptionText { get; set; }

        [Range(1, Int64.MaxValue, ErrorMessageResourceName = "ErrorTermIdGlossary")]
        public Int64 IdGlossary { get; set; }

        public bool IsPublished { get; set; }
        public Char FirstLetter { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public String CssClass { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0} IdGlossary:{1} Name: {2}", Id, IdGlossary, Name);
        }
    }
}