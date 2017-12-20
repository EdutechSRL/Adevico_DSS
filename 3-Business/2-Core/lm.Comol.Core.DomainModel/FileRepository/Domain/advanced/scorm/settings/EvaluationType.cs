using System;
namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
	[Serializable()]
    public enum EvaluationType : int
	{
        FromScormEvaluation = 0,
		CustomForPackage = 1,
        CustomForActivities = 2,
	}
}