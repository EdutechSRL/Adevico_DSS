using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum StatisticType : int
	{
        none = 0,
        downloads = 1,
		plays = 2,
        mydownloads = 3,
        myplays = 4,
	}
}