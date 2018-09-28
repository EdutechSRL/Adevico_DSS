using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Glossary.Domain;
namespace lm.Comol.Modules.Standard.Glossary.Model
{
    public class EditGroup
    {
        public GlossaryGroup Group { get; set; }
        
        public Boolean CanShowDetailedList { get; set; }
        public Boolean CanShowNotPaged { get; set; }



    }
}
