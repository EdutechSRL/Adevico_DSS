using System;
namespace lm.Comol.Core.DomainModel.Helpers
{
	[Serializable()]
    public enum TypeAvailabilityTime : int
	{
		always = 0,
        range = 1,
        days = 2,
	}
}