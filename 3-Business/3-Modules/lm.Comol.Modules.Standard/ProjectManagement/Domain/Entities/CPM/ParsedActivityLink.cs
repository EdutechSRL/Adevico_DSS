using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//lm.Comol.Modules.Standard.ProjectManagement.Domain
namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class ParsedActivityLink : IComparable <ParsedActivityLink>
    {
        public Int64 Id { get; set; }
        public Double LeadLag { get; set; }
        public PmActivityLinkType LinkType { get; set; }
        public Boolean IsSummary { get; set; }

        public int CompareTo(ParsedActivityLink other)
        {
            return Id.CompareTo(other.Id);
        }
    }
}
