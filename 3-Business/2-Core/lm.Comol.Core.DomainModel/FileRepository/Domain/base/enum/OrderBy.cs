using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
	public enum OrderBy : int
	{
        none = 0,
        name = 1,
		date = 2,
        size = 3,
        displayorder = 4
	}
}