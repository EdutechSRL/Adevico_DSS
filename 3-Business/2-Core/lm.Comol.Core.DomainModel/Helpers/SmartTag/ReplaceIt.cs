using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers.Tags
{
    [Serializable()]
    public class ReplaceIt
    {
        public String Original { get; set; }
        public String Replaced { get; set; }
        public Boolean Regex  { get; set; }
        public ReplaceIt() { }
        public ReplaceIt(String original, String replaced, Boolean regex = false)
        {
            Original = original;
            Replaced = replaced;
            Regex = regex;
        }

    }
}