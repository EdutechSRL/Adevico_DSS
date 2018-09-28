using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Glossary.Domain
{
    [Serializable]
    public class GlossaryGroup : DomainBaseObjectMetaInfo<long>
    {

        /// <summary>
        /// Name of Glossary
        /// </summary>
        [Required(ErrorMessage = "*"), StringLength(255, ErrorMessage = "Must be under 255 characters")]
        public virtual String Name { get; set; }

        /// <summary>
        /// List of term for this glossary
        /// </summary>
        public virtual IList<GlossaryItem> Terms { get; set; }

        /// <summary>
        /// Owner ID
        /// </summary>
        public virtual Int64 OwnerId { get; set; }

        /// <summary>
        /// Owner Text: for future. Actually only community. (Text = 0)
        /// </summary>
        public virtual Int32 OwnerType { get; set; }

        /// <summary>
        /// Default view for this glossary
        /// </summary>
        public virtual DefaultView DefaultView { get; set; }

        /// <summary>
        /// If this Glossary is paged
        /// </summary>
        public virtual Boolean IsPaged { get; set; }


        /// <summary>
        /// If this Glossary is paged
        /// </summary>
        public virtual Boolean IsDefault { get; set; }

        /// <summary>
        /// Item per Page for paging
        /// </summary>
        public virtual Int32 ItemPerPage { get; set; }

        /// <summary>
        /// Total Item for paging
        /// </summary>
        public virtual Int32 TotalItems { get; set; }

        public virtual Int32 ExportedStatus { get; set; }
    }
}
