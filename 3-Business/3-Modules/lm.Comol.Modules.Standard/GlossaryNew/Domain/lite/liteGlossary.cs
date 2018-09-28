using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class liteGlossary : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        //public virtual Boolean IsDefault { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual DisplayMode DisplayMode { get; set; }
        public virtual GlossaryType Type { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual Boolean IsPublic { get; set; }
        public virtual Boolean IsShared { get; set; }
        public virtual Boolean IsPublished { get; set; }
        public virtual bool TermsArePaged { get; set; }
        public virtual int TermsPerPage { get; set; }
        public virtual int TermsCount { get; set; }
    }
}