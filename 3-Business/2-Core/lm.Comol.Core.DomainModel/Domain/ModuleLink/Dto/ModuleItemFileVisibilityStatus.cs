using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable(), Flags()]
	public enum ModuleItemFileVisibilityStatus
	{
		NoChange = 1,
		HiddenForCommunity = 2,
		HiddenForModule = 4,
		VisibleForCommunity = 8,
		VisibleForModule = 16
	}
}