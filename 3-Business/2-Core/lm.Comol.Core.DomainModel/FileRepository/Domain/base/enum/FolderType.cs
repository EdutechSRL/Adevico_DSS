using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
	public enum FolderType : int
	{
        none = 0,
        standard = 1,
        recycleBin = 2,
	}
}