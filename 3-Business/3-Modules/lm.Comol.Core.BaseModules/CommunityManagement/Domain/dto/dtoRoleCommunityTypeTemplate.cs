using System.Runtime.Serialization;
using System;
using lm.Comol.Core.Communities;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
	[Serializable(), CLSCompliant(true)]
	public class dtoRoleCommunityTypeTemplate
	{
        public virtual Int32 IdCommunityType { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Boolean isDefault { get; set; }
    }
}