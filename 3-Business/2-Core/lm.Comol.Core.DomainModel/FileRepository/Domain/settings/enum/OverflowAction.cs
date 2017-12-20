using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum OverflowAction : int
	{
        None = 0,
		Allow = 1,
        AllowWithWarning = 2,
        NotAllow = 3
	}
}