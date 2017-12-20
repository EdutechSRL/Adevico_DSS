using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum ItemAvailability : int
	{
        available = 0,
		notavailable = 1,
        transfer = 2,
        waitingsettings = 3,
	    witherrors = 4,
        increation = 5,
        unzip = 6,
        unabletounzip = 7,
        unabletoverwrite = 8,
        invalidtype = 9,
        analyzing= 10,
        notuploaded = -1,
        ignore = -1000
	}
}