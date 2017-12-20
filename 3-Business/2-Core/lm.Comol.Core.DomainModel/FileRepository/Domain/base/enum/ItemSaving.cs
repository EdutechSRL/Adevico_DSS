using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum ItemSaving : int
	{
        None = 0,
		NameDuplicate = 1,
        UrlDuplicate = 2,
        NameAndUrlDuplicate = 3,
        Saved = 4
	}
}