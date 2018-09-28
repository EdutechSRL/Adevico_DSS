using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary.Domain.Dto
{
    public class LetterDto
    {
        public Boolean IsSelected { get; set; }
        public Boolean IsEnable { get; set; }
        public String Letter { get; set; }
        public Char value { get; set; }

        //public Int32 NumOccurrency { get; set; }
    }
}
