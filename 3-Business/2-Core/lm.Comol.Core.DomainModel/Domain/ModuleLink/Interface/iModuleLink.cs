using System;
namespace lm.Comol.Core.DomainModel
{

	[CLSCompliant(true)]
	public interface iModuleLink : IDomainBaseObjectMetaInfo<long>
	{

		ModuleObject SourceItem { get; set; }
		ModuleObject DestinationItem { get; set; }
		string Description { get; set; }
		string Link { get; set; }
		int Action { get; set; }
		long Permission { get; set; }
		bool EditEnabled { get; set; }
		bool NotifyExecution { get; set; }
		bool AutoEvaluable { get; set; }
	}
}