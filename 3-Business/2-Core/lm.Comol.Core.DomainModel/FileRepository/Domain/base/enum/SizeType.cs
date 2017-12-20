using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum SizeType : int
	{
        Folder = 0,
		Repository = 1,
        Disk = 2,
	}
}