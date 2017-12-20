using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class dtoPackageEvaluation
	{
        public virtual long Id { get; set; }
        #region "File identifier"
            public virtual long IdItem { get; set; }
            public virtual System.Guid UniqueIdItem { get; set; }
            public virtual long IdVersion { get; set; }
            public virtual System.Guid UniqueIdVersion { get; set; }
        #endregion
        public virtual long IdLink { get; set; }

        #region "Play identifier"
            public virtual long IdSettings { get; set; }
            public virtual String PlaySession { get; set; }
            public virtual Boolean AlreadyCompleted { get; set; }
         #endregion

        #region "Play values"
            public virtual PackageStatus Status { get; set; }
            public virtual int Completion { get; set; }
            public virtual Boolean IsPassed { get; set; }
            public virtual Boolean IsCompleted { get; set; }
            public virtual Double UserScore { get; set; }
            public virtual long UserTime { get; set; }
            public virtual long ActivitiesDone { get; set; }
            public virtual ScormStatus LessonStatus { get; set; }
            public virtual ScormStatus CompletionStatus { get; set; }
            public virtual ScormStatus SuccessStatus { get; set; }
            public virtual long PlayNumber { get; set; }
        #endregion


        public dtoPackageEvaluation()
		{
            IsPassed = false;
            IsCompleted = false;
            Status = PackageStatus.notstarted;
            Completion = 0;
            ActivitiesDone = 0;
            UserTime = 0;
            UserScore = (float)0;
            LessonStatus = ScormStatus.none;
            CompletionStatus = ScormStatus.none;
            SuccessStatus = ScormStatus.none;
		}
        public dtoPackageEvaluation(ScormPackageUserEvaluation item)
		{
            Id = item.Id;
            IdItem = item.IdItem;
            UniqueIdItem = item.UniqueIdItem;
            IdVersion = item.IdVersion;
            UniqueIdVersion = item.UniqueIdVersion;
            IdLink = item.IdLink;
            IdSettings= item.IdSettings;
            PlaySession= item.PlaySession;
            AlreadyCompleted = item.AlreadyCompleted;
            PlayNumber = item.PackageSession.PlayNumber;
            IsPassed = item.IsPassed;
            IsCompleted = item.IsCompleted;
            Status = item.Status;
            Completion = item.Completion;
            ActivitiesDone = item.PackageSession.ActivitiesDone;
            UserTime = item.PackageSession.UserTime;
            UserScore = item.PackageSession.UserScore;
            LessonStatus = item.PackageSession.LessonStatus;
            CompletionStatus = item.PackageSession.CompletionStatus;
            SuccessStatus = item.PackageSession.SuccessStatus;
		}
	}
}