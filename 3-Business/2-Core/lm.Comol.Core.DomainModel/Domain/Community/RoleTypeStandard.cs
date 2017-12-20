using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum RoleTypeStandard : int
	{
		Administrator = 1,
		HiddenEnrollment = -3,
		HiddenCreator = -2,
        Guest = -4
	}
}