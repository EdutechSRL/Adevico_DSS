using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
	public enum ItemIdentifierType : int
	{
        standard = 0,
        module = 1,
		tags = 2,
	}
}