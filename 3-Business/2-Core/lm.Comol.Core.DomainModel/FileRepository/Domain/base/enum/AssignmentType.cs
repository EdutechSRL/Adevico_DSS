using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum AssignmentType : int
	{
		community = 0,
        role = 1,
        person = 2
	}
}