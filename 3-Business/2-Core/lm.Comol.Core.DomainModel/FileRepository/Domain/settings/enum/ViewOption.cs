using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(),Flags ]
    public enum ViewOption : int
	{
        None = 0,
		FolderPath = 1,
        Date = 2,
        Extrainfo = 4,
	    Statistics = 8,
        Tree = 16,
        AvailableSpace = 32,
        NarrowWideView = 64
	}
}