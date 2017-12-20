using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iTimeZone : iDomainObject<Guid>
	{

		string Description { get; set; }
		int Hours { get; set; }
		int Minutes { get; set; }
		string FullDescription { get; }
	}
}