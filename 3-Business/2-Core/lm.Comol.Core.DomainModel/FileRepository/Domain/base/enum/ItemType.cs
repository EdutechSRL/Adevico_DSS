using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
	public enum ItemType : int
	{
        None = 0,
		File = 1,
        ScormPackage = 2,
        Folder = 3,
	    VideoStreaming = 4,
        Multimedia = 5,
        Link = 6,
        SharedDocument = 7,
        RootFolder = -1
	}
}