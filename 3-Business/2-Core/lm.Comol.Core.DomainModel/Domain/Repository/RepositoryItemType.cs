using System;
namespace lm.Comol.Core.DomainModel.Repository
{
	[Serializable(), CLSCompliant(true)]
	public enum RepositoryItemType : int
	{
        None = 0,
		FileStandard = 1,
        ScormPackage = 2,
        Folder = 3,
	    VideoStreaming = 4,
        Multimedia = 5,
        Url = 6
	}
}