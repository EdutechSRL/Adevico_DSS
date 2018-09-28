using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Faq
{
    public class EditCategoryModel : BaseFaqModel
    {
        public virtual IList<Category> Categories { get; set; }
        public virtual Category CategoryForModify { get; set; }
    }
}
