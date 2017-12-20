using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class ScormPackageUserEvaluation : DomainBaseObject<long>
	{
        #region "File identifier"
            public virtual long IdItem { get; set; }
            public virtual System.Guid UniqueIdItem { get; set; }
            public virtual long IdVersion { get; set; }
            public virtual System.Guid UniqueIdVersion { get; set; }
        #endregion

        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime LastUpdate { get; set; }
        public virtual DateTime? EndPlayOn { get; set; }
        public virtual int IdPerson { get; set; }

        #region "Object identifier"
            public virtual long IdLink { get; set; }
            public virtual Int32 IdModule { get; set; }
            public virtual String ModuleCode { get; set; }
            public virtual long IdObject { get; set; }
            public virtual Int32 IdObjectType { get; set; }
            public virtual Boolean IsCreatedByModule { get; set; }
        #endregion

        #region "Play identifier"
            public virtual long IdSettings { get; set; }       
            public virtual ScormPackageUserSession PackageSession { get; set; }
            public virtual String PlaySession { get; set; }
            public virtual Boolean AlreadyCompleted { get; set; }
            public virtual PackageStatus Status { get; set; }
            public virtual int Completion { get; set; }
            public virtual Boolean IsPassed { get; set; }
            public virtual Boolean IsCompleted { get; set; }
            public virtual Boolean IsCreatedByPlay { get; set; }
            public virtual Boolean IsCalculated { get; set; }
        #endregion


        public ScormPackageUserEvaluation()
		{
			IsCalculated = false;
			IsCreatedByPlay = false;
            IsCreatedByModule = false;
            PackageSession = new ScormPackageUserSession();
            IsPassed = false;
            IsCompleted = false;
            Status = PackageStatus.notstarted;
            Completion = 0;
            EndPlayOn = null;
		}

        public static ScormPackageUserEvaluation CreateBy(liteModuleLink link, dtoPackageEvaluation dto, Int32 idPerson, DateTime referenceTime, Boolean isCalculated, Boolean isCreatedByPlayer)
        {
            ScormPackageUserEvaluation item = new ScormPackageUserEvaluation();
            item.IdItem = dto.IdItem;
            item.UniqueIdItem = dto.UniqueIdItem;
            item.IdVersion = dto.IdVersion;
            item.UniqueIdVersion = dto.UniqueIdVersion;
            item.IdLink = dto.IdLink;
            item.IdSettings = dto.IdSettings;
            item.PlaySession = dto.PlaySession;
            item.AlreadyCompleted = dto.AlreadyCompleted;
            item.Status = dto.Status;
            item.Completion = dto.Completion;
            item.IsPassed = dto.IsPassed;
            item.IsCompleted = dto.IsCompleted;
            item.IsCreatedByModule = (link != null);
            item.IdPerson = idPerson;
            if (link != null)
            {
                item.IdObject = link.SourceItem.ObjectLongID;
                item.IdObjectType = link.SourceItem.ObjectTypeID;
                item.IdModule = link.SourceItem.ServiceID;
                item.ModuleCode = link.SourceItem.ServiceCode;
            }
            item.IsCalculated = isCalculated;
            item.IsCreatedByPlay = isCreatedByPlayer;
            if (isCalculated)
            {
                item.CreatedOn = referenceTime;
                item.EndPlayOn = referenceTime;
                item.LastUpdate = referenceTime;
            }
            else
            {
                item.CreatedOn = referenceTime;
                item.LastUpdate = referenceTime;
            }
                
            item.PackageSession.ActivitiesDone = dto.ActivitiesDone;
            item.PackageSession.CompletionStatus = dto.CompletionStatus;
            item.PackageSession.LessonStatus = dto.LessonStatus;
            item.PackageSession.SuccessStatus= dto.SuccessStatus;
            item.PackageSession.UserScore= dto.UserScore;
            item.PackageSession.UserTime=dto.UserTime;
            item.PackageSession.PlayNumber = dto.PlayNumber;
            return item;
        }

        public virtual void UpdateStatisticsBy(dtoPackageEvaluation dto, DateTime referenceTime, Boolean playCompleted)
        {
            Boolean updated = false;
            if (dto.IdSettings > 0 && IdSettings != dto.IdSettings)
                IdSettings = dto.IdSettings;
            if (!AlreadyCompleted && dto.AlreadyCompleted)
                AlreadyCompleted = dto.AlreadyCompleted;
            if (playCompleted && playCompleted == IsCalculated)
            {
                if (dto.UserScore>PackageSession.UserScore)
                    PackageSession.UserScore = dto.UserScore;
                if (dto.UserTime > PackageSession.UserTime)
                    PackageSession.UserTime = dto.UserTime;
                if (dto.Completion > Completion)
                    Completion = dto.Completion;
                if (dto.IsPassed)
                {
                    IsPassed = dto.IsPassed;
                    if (!IsCompleted)
                    {
                        PackageSession.CompletionStatus = dto.CompletionStatus;
                        PackageSession.LessonStatus = dto.LessonStatus;
                        PackageSession.SuccessStatus = dto.SuccessStatus;
                        Status = dto.Status;
                    }
                }
                if (dto.IsCompleted)
                {
                    IsCompleted = dto.IsCompleted;
                    PackageSession.CompletionStatus = dto.CompletionStatus;
                    PackageSession.LessonStatus = dto.LessonStatus;
                    PackageSession.SuccessStatus = dto.SuccessStatus;
                    Status = dto.Status;
                }
                if (dto.ActivitiesDone > PackageSession.ActivitiesDone)
                    PackageSession.ActivitiesDone = dto.ActivitiesDone;
            }
            else if (!IsCalculated)
            {
                Status = dto.Status;
                Completion = dto.Completion;
                IsPassed = dto.IsPassed;
                IsCompleted = dto.IsCompleted;

                IsCalculated = playCompleted;
                PackageSession.ActivitiesDone = dto.ActivitiesDone;
                PackageSession.CompletionStatus = dto.CompletionStatus;
                PackageSession.LessonStatus = dto.LessonStatus;
                PackageSession.SuccessStatus = dto.SuccessStatus;
                PackageSession.UserScore = dto.UserScore;
                PackageSession.UserTime = dto.UserTime;
                PackageSession.PlayNumber = dto.PlayNumber;
                LastUpdate = referenceTime;
                updated = true;
            }
            if (playCompleted && updated)
                EndPlayOn = referenceTime;

           
        }
	}
}