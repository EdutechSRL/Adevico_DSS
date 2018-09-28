using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary.Business
{
    public class GlossaryHelpers
    {
        public static String GetLinkLetter(String Value)
        {
            if (Value == null) { return "*"; }
            if (Char.IsLetter(Value[0])) { return Value[0].ToString(); }
            else
            {
                switch (Value)
                {
                    case "#":
                        return "Num";
                    case "$":
                        return "Sym";
                    default:
                        return "All";
                }
            }
        }

        public static String GetLetter(String LinkLetter)
        {
            if (LinkLetter == null) { return "*"; }
            
            else
            {
                switch (LinkLetter)
                {
                    case "All":
                        return "*";
                    case "Num":
                        return "#";
                    case "Sym":
                        return "$";
                    default:
                        LinkLetter = LinkLetter.ToLower();
                        if (Char.IsLetter(LinkLetter[0])) { return LinkLetter[0].ToString(); }
                        else { return "*"; }
                }
            }
        }
    }
}
