using System.Runtime.Serialization;
using System;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), Flags(), CLSCompliant(true)]
	public enum StatTreeLeafType
	{
		None = 0,
		Personal = 1,
		Advanced = 2
	}
}