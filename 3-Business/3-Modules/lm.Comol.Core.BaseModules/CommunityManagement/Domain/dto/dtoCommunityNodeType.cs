using System.Runtime.Serialization;
using System;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
	[Serializable(), Flags(), CLSCompliant(true)]
    public enum dtoCommunityNodeType
	{
		None = 0,
		Active = 1,
		Stored = 2,
        Blocked = 4,
        NotSelectable = 8,
        Root = 16
	}
}