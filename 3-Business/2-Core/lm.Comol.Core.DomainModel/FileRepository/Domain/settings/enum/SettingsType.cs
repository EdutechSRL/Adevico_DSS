using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
	public enum SettingsType : int
	{
        Istance = 0,
		Portal = 1,
        Organization = 2,
        Community = 3,
	    Profile = 4,
        User = 5
	}
}