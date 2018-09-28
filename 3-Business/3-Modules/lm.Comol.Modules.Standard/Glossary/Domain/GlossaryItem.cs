using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Glossary.Domain
{
    [Serializable]
    public class GlossaryItem : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// Term.
        /// Text Only.
        /// </summary>
        [Required(ErrorMessage = "*"), StringLength(100, ErrorMessage = "Must be under 100 characters")]
        public virtual String Term { get; set; }

        /// <summary>
        /// Definition of term.
        /// Html format, like all other text area.
        /// </summary>
        [Required(ErrorMessage = "*")]
        public virtual String Definition { get; set; }

        /// <summary>
        /// First letter for filter.
        /// To speed-up searches
        /// </summary>
        public virtual Char FirstLetter { get; set; }

        ///// <summary>
        ///// Link Id
        ///// To connect a term to community or other group of term (to define)
        ///// </summary>
        //public virtual Int32 LinkId { get; set; }

        /// <summary>
        /// Is public.
        /// If this term is visible to entire system (to define)
        /// </summary>
        public virtual Boolean IsPublic { get; set; }

        /// <summary>
        /// Group (Glossary)
        /// </summary>
        public virtual GlossaryGroup Group { get; set; }

        /// <summary>
        /// Partial link to this term
        /// </summary>
        public virtual String Link { get; set; }

        public virtual Int32 ExportedStatus { get; set; }

        public virtual void SetFirstLetter()
        {
            Char _firstLetter = Term.ToLower().ToCharArray(0, 1)[0];
            if (!char.IsLetterOrDigit(_firstLetter))
            {
                _firstLetter = '_';
            }

            FirstLetter = _firstLetter;
        }
    }
}
