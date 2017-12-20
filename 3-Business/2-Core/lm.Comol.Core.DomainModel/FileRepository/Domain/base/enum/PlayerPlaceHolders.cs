using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
    public enum PlayerPlaceHolders : int
	{
        idCommunity = 0,
		idUser = 1,
        userName = 2,
        folder = 3,
        defaultFile = 4,
        defaultDocumentPath = 5,
        courseId = 6,
        dbIdentifier = 7,
        workingSessionId = 8,
        platformId = 9,
        workingPlayId =10
	}
}