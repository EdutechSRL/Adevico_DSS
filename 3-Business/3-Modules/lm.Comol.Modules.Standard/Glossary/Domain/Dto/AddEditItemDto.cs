using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary.Domain.Dto
{
    public class AddEditItemDto
    {
        public Int64 Id { get; set; }

        public String Term { get; set; }
        public String Definition { get; set; }
        public Boolean IsPublic { get; set; }

        public Int64 GroupId { get; set; }
                
    }
}
