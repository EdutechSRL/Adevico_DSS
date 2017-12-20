
using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iStatTreeLeaf : iStatBaseTreeNode
	{
		StatTreeLeafType Type { get; set; }
		long LinkId { get; set; }
	}
}