using System.Runtime.Serialization;
using System;
using lm.Comol.Core.Communities;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
	[Serializable(), CLSCompliant(true)]
	public class dtoBaseCommunityNode 
	{
        public Int32 Id { get; set; }
        public Int32 IdFather { get; set; }
        public Int32 IdCreatedBy { get; set; }
        
        public String Path { get; set; }

        public dtoBaseCommunityNode() { }
        public dtoBaseCommunityNode(Int32 id) {
            Id = id;
            IdFather = 0;
            Path = "." + id.ToString() + ".";
        }
    }
}