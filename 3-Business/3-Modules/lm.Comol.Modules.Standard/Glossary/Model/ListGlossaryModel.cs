using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Glossary.Domain;

namespace lm.Comol.Modules.Standard.Glossary.Model
{
    public class ListGlossaryModel
    {
        public ListGlossaryModel()
        {
            Groups = new List<GlossaryGroup>();
            Permission = new ModuleGlossary();
        }

        public IList<GlossaryGroup> Groups { get; set; }

        public ModuleGlossary Permission { get; set; }

        //public bool DeleteDisabled { get; set; }
    }
}
