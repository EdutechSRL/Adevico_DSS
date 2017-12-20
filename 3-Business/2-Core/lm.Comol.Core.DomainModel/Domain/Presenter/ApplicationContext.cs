using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public class ApplicationContext : iApplicationContext
	{
        public virtual iDataContext DataContext { get; set; }
        public virtual iUserContext UserContext { get; set; }
	}
}