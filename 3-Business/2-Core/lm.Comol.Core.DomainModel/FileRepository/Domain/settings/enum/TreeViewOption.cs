using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum TreeViewOption : int
	{
        None = 0,
		OnlyWithFolders = 1,
        Always = 2,
        Never = 3
	}
}