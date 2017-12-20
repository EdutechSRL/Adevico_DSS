using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum FileRepositoryType : int
	{
		None = 0,
		CommunityFile = 1,
		InternalLong = 2,
		InternalGuid = 3
	}
}