using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(),Flags]
    public enum CalculationMode : int
	{
        None = 0,
        File = 1,
		Versioning = 2,
        RecycleBin = 4,
	}
}