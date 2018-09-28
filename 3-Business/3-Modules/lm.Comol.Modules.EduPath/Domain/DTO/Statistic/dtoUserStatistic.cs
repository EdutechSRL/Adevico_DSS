using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{

    [Serializable]
    public class dtoPathUserStatistic
    {
        public Path Path { get; set; }
        public Int64 PathId { get; set; }

        public String CommunityName { get; set; }
        public Int32 CommunityId { get; set; }

        public IList<dtoUserStatistic> Users { get; set; }
    }

    [Serializable]
    public class dtoUserStatistic
    {

        public litePerson User { get; set; }
        public Int32 UserId { get; set; }
        public Int32 CRoleId { get; set; }
       
        public IList<dtoUserActivityStatistic> Activities { get; set; }
    }

    [Serializable]
    public class dtoUserActivityStatistic
    {
        public long Id { get; set; }
        public long parentId { get; set; }

        public litePerson User { get; set; }
        public Int32 UserId { get; set; }
        
        public String Name { get; set; }
        public IList<dtoUserSubActivityStatistic> SubActivities { get; set; }

        public dtoUserActivityStatistic()
        { }

        public dtoUserActivityStatistic(dtoActivityStatistic dto, litePerson user)
        {
            this.Id = dto.Id;
            this.parentId = dto.parentId;
            this.Name = dto.Name;

            this.User = user;

            this.SubActivities = (from item in dto.SubActivities select new dtoUserSubActivityStatistic(item, user)).ToList();
        }

        public dtoUserActivityStatistic(dtoActivityStatistic dto, Int32 userId)
        {
            this.Id = dto.Id;
            this.parentId = dto.parentId;
            this.Name = dto.Name;

            this.UserId = userId;

            this.SubActivities = (from item in dto.SubActivities select new dtoUserSubActivityStatistic(item, userId)).ToList();
        }
    }

    [Serializable]
    public class dtoUserSubActivityStatistic
    {

        public litePerson User { get; set; }
        public Int32 UserId { get; set; }
        
        // public long Id { get; set; }
        public long parentId { get; set; }
        // public SubActivityType ContentType { get; set; }
        public Int64 Completion { get; set; }
        public Int16 Mark { get; set; }
        //   public Int64 Weight { get; set; }
        public StatusStatistic StatusStat { get; set; }
        //  public Status Status { get; set; }
        //  public lm.Comol.Core.DomainModel.ModuleLink ModuleLink { get; set; }
        public bool isMandatory { get; set; }
        public bool isSingle { get; set; }
        // public string Name { get; set; }
        public string Other { get; set; }
        public Boolean ActivityCompleted { get; set; }
        public dtoSubActivity SubActivity { get; set; }

        public dtoUserSubActivityStatistic() { }

        public dtoUserSubActivityStatistic(dtoSubActivityStatistic dto, litePerson user) {
            this.parentId = dto.parentId;
            this.Completion = dto.Completion;
            this.Mark = dto.Mark;
            this.StatusStat = dto.StatusStat;
            this.isMandatory = dto.isMandatory;
            this.isSingle = dto.isSingle;
            this.Other = dto.Other;
            this.ActivityCompleted = dto.ActivityCompleted;
            this.SubActivity = dto.SubActivity;

            this.User = user;
        }

        public dtoUserSubActivityStatistic(dtoSubActivityStatistic dto, Int32 userId) {
            this.parentId = dto.parentId;
            this.Completion = dto.Completion;
            this.Mark = dto.Mark;
            this.StatusStat = dto.StatusStat;
            this.isMandatory = dto.isMandatory;
            this.isSingle = dto.isSingle;
            this.Other = dto.Other;
            this.ActivityCompleted = dto.ActivityCompleted;
            this.SubActivity = dto.SubActivity;

            this.UserId = UserId;
        }
    }
}
