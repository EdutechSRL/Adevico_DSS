using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iModuleDefinition : lm.Comol.Core.DomainModel.iDomainObject<int>
	{

		string Name { get; set; }
		string Code { get; set; }
		bool Available { get; set; }
		bool isNotificable { get; set; }
	}
}