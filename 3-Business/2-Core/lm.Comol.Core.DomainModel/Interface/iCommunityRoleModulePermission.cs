using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iCommunityRoleModulePermission : lm.Comol.Core.DomainModel.iDomainObject<int>
	{

		ModuleDefinition Service { get; set; }
		Community Community { get; set; }
		string PermissionString { get; set; }
		long PermissionInt { get; }

		Role Role { get; set; }
	}
}