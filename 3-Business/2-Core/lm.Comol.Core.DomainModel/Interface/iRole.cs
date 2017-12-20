using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iRole : iDomainObject<int>
	{
		string Name { get; set; }
		string Description { get; set; }
	}
}