using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary.Domain.Dto
{
    [Serializable]
    public class ListItemDTO
    {
        public virtual Int64 Id { get; set; }
        /// <summary>
        /// Term.
        /// Text Only.
        /// </summary>
        public virtual String Term { get; set; }

        /// <summary>
        /// First letter for filter.
        /// To speed-up searches
        /// </summary>
        public virtual Char FirstLetter { get; set; }

        /// <summary>
        /// Group (Glossary)
        /// </summary>
        public virtual GlossaryGroup Group { get; set; }

    }
}
