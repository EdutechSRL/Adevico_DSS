using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Glossary.Domain;
namespace lm.Comol.Modules.Standard.Glossary.Model
{
    public class EditItemModel
    {
        public IList<GlossaryGroup> Groups { get; set; }
        public GlossaryItem Item { get; set; }
        public Int64 SelectedGroup { get; set; }

        public ModuleGlossary Permission { get; set; }

        public int CurrentCommunityId { get; set; }
    }
}
