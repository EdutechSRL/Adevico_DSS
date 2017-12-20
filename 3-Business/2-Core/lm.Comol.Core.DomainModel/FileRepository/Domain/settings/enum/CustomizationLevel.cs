using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), Flags]
    public enum CustomizationLevel : int
	{
        None = 0,
        All = 1,
        ViewOptions = 1,
        Versioning = 2,
        ModuleVersioning = 4,
        AvailableTypesForVersioning = 8,
        AvailableSpace = 16,
        FileDiskSpaceOverflow  = 32,
        AvailableTypes
	}
}