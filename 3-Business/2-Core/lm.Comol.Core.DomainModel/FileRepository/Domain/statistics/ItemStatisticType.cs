using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), Flags]
    public enum ItemStatisticType : int
	{
        None = 0,
		Download = 1,
        Play = 2,
        Scorm = 4,
	}
}