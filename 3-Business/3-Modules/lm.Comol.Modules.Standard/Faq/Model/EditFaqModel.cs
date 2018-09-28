using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Faq
{
    public class EditFaqModel : BaseFaqModel
    {
        /// <summary>
        /// La faq da modificare
        /// </summary>
        public virtual Faq Faq { get; set; }

        /// <summary>
        /// L'elenco di categorie che è possibile assegnargli
        /// </summary>
        public virtual IList<Category> Category { get; set; }

        //public virtual Int64 CurrentCategoryId { get; set; }
    }
}
