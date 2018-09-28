using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Faq
{
    public class UserDataModel : BaseFaqModel
    {
        /// <summary>
        /// L'elenco di Faq visualizzate
        /// </summary>
        public virtual IList<Faq> Faqs { get; set; }

        /// <summary>
        /// L'elenco delle categorie disponibili
        /// </summary>
        public virtual IList<Category> Category { get; set; }

        /// <summary>
        /// La categoria corrente
        /// </summary>
        public virtual Category CurrentCategory { get; set; }
    }
}
