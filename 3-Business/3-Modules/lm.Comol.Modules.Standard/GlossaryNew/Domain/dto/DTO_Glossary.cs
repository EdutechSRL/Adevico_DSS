using System;
using System.ComponentModel.DataAnnotations;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_Glossary
    {
        public DTO_Share GlossaryShare;
        //Permissions
        public GlossaryPermission Permission;

        public DTO_Glossary()
        {
            Permission = new GlossaryPermission();
        }

        public DTO_Glossary(GlossaryDisplayOrder glossaryDisplayOrder, Share share)
            : this()
        {
            var glossary = glossaryDisplayOrder.Glossary;

            Id = glossary.Id;
            //IdShare = share.Id;
            if (share != null)
                GlossaryShare = new DTO_Share(share);
            Name = glossary.Name;
            Description = glossary.Description;

            TermsArePaged = glossary.TermsArePaged;
            TermsPerPage = glossary.TermsPerPage;
            TermsCount = glossary.TermsCount;

            //IsDefault = glossary.IsDefault;
            IsPublic = glossary.IsPublic;
            IsShared = glossary.IsShared;
            IsPublished = glossary.IsPublished;

            IdCommunity = glossary.IdCommunity;
            IdLanguage = glossary.IdLanguage;

            DisplayOrder = glossaryDisplayOrder.DisplayOrder;
            IdShare = glossaryDisplayOrder.Id;

            IsDefault = glossaryDisplayOrder.IsDefault;
            if (IsDefault)
                DisplayOrder = -1;
            if (glossary.ModifiedOn.HasValue)
                LastUpdate = glossary.ModifiedOn.Value;
            else if (glossary.CreatedOn.HasValue)
                LastUpdate = glossary.CreatedOn.Value;
            else
                LastUpdate = DateTime.MinValue;
        }

        public DTO_Glossary(liteGlossary glossary, Boolean isDefault)
            : this()
        {
            Id = glossary.Id;
            Name = glossary.Name;
            Description = glossary.Description;

            TermsArePaged = glossary.TermsArePaged;
            TermsPerPage = glossary.TermsPerPage;
            TermsCount = glossary.TermsCount;

            IsDefault = isDefault;
            IsPublic = glossary.IsPublic;
            IsShared = glossary.IsShared;
            IsPublished = glossary.IsPublished;

            IdCommunity = glossary.IdCommunity;
            IdLanguage = glossary.IdLanguage;

            if (IsDefault)
                DisplayOrder = -1;

            if (glossary.ModifiedOn.HasValue)
                LastUpdate = glossary.ModifiedOn.Value;
            else if (glossary.CreatedOn.HasValue)
                LastUpdate = glossary.CreatedOn.Value;
            else
                LastUpdate = DateTime.MinValue;
        }

        public DTO_Glossary(Glossary glossary)
            : this()
        {
            Id = glossary.Id;
            Name = glossary.Name;
            Description = glossary.Description;

            TermsArePaged = glossary.TermsArePaged;
            TermsPerPage = glossary.TermsPerPage;
            TermsCount = glossary.TermsCount;

            //IsDefault = glossary.IsDefault;
            IsPublic = glossary.IsPublic;
            IsShared = glossary.IsShared;
            IsPublished = glossary.IsPublished;

            IdCommunity = glossary.IdCommunity;
            IdLanguage = glossary.IdLanguage;

            DisplayOrder = glossary.DisplayOrder;

            if (glossary.ModifiedOn.HasValue)
                LastUpdate = glossary.ModifiedOn.Value;
            else if (glossary.CreatedOn.HasValue)
                LastUpdate = glossary.CreatedOn.Value;
            else
                LastUpdate = DateTime.MinValue;
        }

        public Int64 Id { get; set; }
        public Int64 IdShare { get; set; }

        public Int32 IdCommunity { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Boolean IsDefault { get; set; }
        public ItemStatus Status { get; set; }
        public DisplayMode DisplayMode { get; set; }
        public Int32 DisplayOrder { get; set; }
        public Int32 IdLanguage { get; set; }
        public Boolean IsPublic { get; set; }
        public Boolean IsShared { get; set; }
        public bool IsPublished { get; set; }
        public Int32 TermsCount { get; set; }
        public Boolean TermsArePaged { get; set; }
        public Int32 TermsPerPage { get; set; }
        public GlossaryType Type { get; set; }
        public ShareStatusEnum Shared { get; set; }
        public DateTime LastUpdate { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0} IdCommunity:{1} Name: {2}", Id, IdCommunity, Name);
        }

        public void SetPermission(GlossaryPermission permission)
        {
            Permission.ViewTerm = permission.ViewTerm;
            Permission.EditGlossary = permission.EditGlossary;
            Permission.DeleteGlossary = permission.DeleteGlossary;
            Permission.AddTerm = permission.AddTerm;
            Permission.EditTerm = permission.EditTerm;
            Permission.DeleteTerm = permission.DeleteTerm;
            Permission.ViewStat = permission.ViewStat;
        }
    }
}