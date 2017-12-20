
using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iModuleActionLink
	{
		iModuleObject ModuleObject { get; set; }
		string Description { get; set; }
		string Link { get; set; }
		int Action { get; set; }
		int Permission { get; set; }
		bool EditEnabled { get; set; }
		bool NotifyExecution { get; set; }
	}
}