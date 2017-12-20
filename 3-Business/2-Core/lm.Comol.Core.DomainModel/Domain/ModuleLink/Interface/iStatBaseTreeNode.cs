
using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iStatBaseTreeNode
	{
		long Id { get; set; }
		string Name { get; set; }
		string ToolTip { get; set; }
		bool isVisible { get; set; }
		bool Selected { get; set; }
		int NodeObjectTypeId { get; set; }
	}
}