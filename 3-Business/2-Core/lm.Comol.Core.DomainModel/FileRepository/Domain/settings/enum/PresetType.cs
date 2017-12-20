using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum PresetType : int
	{
        None = 0,
		Simple = 1,
        Standard = 2,
        Advanced = 3,
	}
}