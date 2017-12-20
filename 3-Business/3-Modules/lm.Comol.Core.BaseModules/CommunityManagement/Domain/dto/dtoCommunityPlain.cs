using System.Runtime.Serialization;
using System;
using System.Linq;
using lm.Comol.Core.Communities;
using System.Collections.Generic;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
	[Serializable(), CLSCompliant(true)]
    public class dtoCommunityPlain
	{
        public Int32 Id { get; set; }
        public Int32 IdOrganization { get; set; }
        public String Name { get; set; }
        public String FirstLetter
        {
            get
            {
                if (String.IsNullOrEmpty(Name))
                    return "";
                else
                    return Name[0].ToString().ToLower();
            }
        }
        public Int32 IdCommunityType { get; set; }
        public Int32 IdResponsible { get; set; }
        public Int32 PathDepth
        {
            get {
                return (from p in Paths select p.PathDepth).Max();
            }
        }
        public CommunityStatus Status { get; set; }

        public List<dtoCommunityPlainPath> Paths { get; set; }
        public List<String> Tags { get; set; }
        public List<long> IdTags { get; set; }

        public dtoCommunityPlain() {
            Paths = new List<dtoCommunityPlainPath>();
            Tags = new List<string>();
            IdTags = new List<long>();
        }
        public dtoCommunityPlain(Int32 id)
        {
            Id = id;
            Tags = new List<string>();
            IdTags = new List<long>();
            Paths = new List<dtoCommunityPlainPath>();
            Paths.Add(new dtoCommunityPlainPath() { isPrimary=true, IdFather=0, Path="." + id.ToString() + "."});
        }

        public String PathsToString(String separator){
            string result ="";
            if (String.IsNullOrEmpty(separator))
                separator = "|";
            result = String.Join(separator, Paths.OrderBy(p=>p.isPrimary).Select(p => p.Path).ToArray());
            return result;
        }
        public List<String> PathsToList()
        {
            List<String> result = new List<String>();
            Paths.OrderBy(p=>p.isPrimary).ToList().ForEach(p=> result.Add(p.Path));
            return result;
        }
        public Boolean ContainsPath(String path)
        {
            return Paths.Where(p=>p.Path.Contains(path)).Any();
        }
        public Boolean ContainsPath(List<String> paths)
        {
            Boolean result = false;
            foreach (String path in paths) {
                result = ContainsPath(path);
                if (result)
                    break;
            }
            return result;
        }
    }
}