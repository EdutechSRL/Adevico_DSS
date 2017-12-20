using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iApplicationContext
	{
		iUserContext UserContext { get; set; }
		iDataContext DataContext { get; set; }
	}
}