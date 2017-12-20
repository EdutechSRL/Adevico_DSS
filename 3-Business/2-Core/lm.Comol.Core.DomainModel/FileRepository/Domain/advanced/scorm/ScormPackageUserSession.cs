using lm.Comol.Core.DomainModel;
using System;
using System.ComponentModel;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class ScormPackageUserSession
	{
        public virtual Double UserScore { get; set; }
        public virtual long UserTime { get; set; }
        public virtual long ActivitiesDone { get; set; }
        public virtual ScormStatus LessonStatus { get; set; }
        public virtual ScormStatus CompletionStatus { get; set; }
        public virtual ScormStatus SuccessStatus { get; set; }
        public virtual long PlayNumber { get; set; }

        public ScormPackageUserSession()
		{
            ActivitiesDone = 0;
            UserTime = 0;
            UserScore = (Double)0;

            LessonStatus = ScormStatus.none;
            CompletionStatus = ScormStatus.none;
            SuccessStatus = ScormStatus.none;
		}
	}

    [Serializable()]
    public enum PackageStatus : int
    {
        notstarted = 0,
        started = 1,
        completed = 2,
        passed = 4,
        completedpassed = 6,
        failed = 8,
        completedfailed = 10,
    }
    [Serializable()]
    public enum ScormStatus : int
    {
        none = 0,
        failed = 1,
        completed = 2,
        incomplete = 3,
        browsed = 4,
        unknow = 5,
        not_attempted = 6,
        passed = 7,
        notattempted = 8,
        started = 9
    }
}