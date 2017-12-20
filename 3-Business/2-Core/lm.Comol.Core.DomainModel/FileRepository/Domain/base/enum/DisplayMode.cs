using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum DisplayMode : int
	{
        none = 0,
		downloadOrPlay = 1,
        inModal = 2,
        downloadOrPlayOrModal = 3,
	}
}