
using System;
namespace lm.Comol.Core.Communities
{
	[Serializable(), CLSCompliant(true) ,Flags]
	public enum CommunityStatus
	{
        None = 0,
        Active = 1,
        Stored = 2,
        Blocked = 4,
        Favorite = 8
	}
}