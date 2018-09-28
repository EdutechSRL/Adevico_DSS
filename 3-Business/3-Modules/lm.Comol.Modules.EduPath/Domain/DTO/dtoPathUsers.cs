using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class dtoPathUsers
    {
        public dtoFullPathItem PathInfo { get; set; }
        
        public Int64 IdPath { get { return (PathInfo == null) ? 0 : PathInfo.Id; } }
        public String PathName { get { return (PathInfo == null) ? Guid.NewGuid().ToString() : PathInfo.Name; } }
        public EPType PathType { get { return (PathInfo == null) ? EPType.None : PathInfo.EPType; } }


        //public   { get; set; }        
        public Int32 IdCommunity { get; set; }
        public String CommunityName { get; set; }
        public Int32 IdOrganization { get; set; }
        public String OrganizationName { get; set; }

        public Boolean PathLocked { get; set; }
        public IList<dtoSubActivityQuestionnaire> Questionnaires { get; set; }
        
        public Int32 NotStarted { get { return (Users.Any() ? Users.Where(u => u.ItemStatus == StatusStatistic.None).Count() : 0); } }
        public Int32 Started { get { return (Users.Any() ? Users.Where(u => u.ItemStatus == StatusStatistic.Started).Count() : 0); } }
        public Int32 Completed { get { return (Users.Any() ? Users.Where(u => u.ItemStatus == StatusStatistic.Completed || u.ItemStatus == StatusStatistic.CompletedPassed).Count() : 0); } }
        public Int32 Total { get {  return (Users.Any() ? Users.Count :0);} }


        public IList<dtoPathUserInfo> Users { get; set; }

        public IList<dtoPathUserInfo> UsersFiltered
        {
            get
            {
               return (from item in Users where item.StStartDate.HasValue || item.StFirstActivity.HasValue || item.StEndDate.HasValue select item).ToList();
            }
        }

        public IList<dtoBaseUserPathQuiz> QuestionnaireStatistics
        {
            get
            {
                if (Users == null || !Users.Any())
                    return new List<dtoBaseUserPathQuiz>();
                else
                    return Users.Where(u => u.Questionnaires != null && u.Questionnaires.Any()).ToList().SelectMany(u => u.Questionnaires).ToList();
            }
        }
        public Dictionary<Int32, IList<dtoBaseUserPathQuiz>> UserQuestionnaireStatistics
        {
            get {
                if (Users == null || !Users.Any())
                    return new Dictionary<Int32, IList<dtoBaseUserPathQuiz>>();
                else
                    return Users.ToDictionary(u => u.IdPerson, u => u.Questionnaires);
            }
        }

        public IList<dtoBaseUserPathQuiz> QuestionnaireStatisticsFiltered
        {
            get
            {
                if (UsersFiltered == null || !UsersFiltered.Any())
                    return new List<dtoBaseUserPathQuiz>();
                else
                    return UsersFiltered.Where(u => u.Questionnaires != null && u.Questionnaires.Any()).ToList().SelectMany(u => u.Questionnaires).ToList();
            }
        }
        public Dictionary<Int32, IList<dtoBaseUserPathQuiz>> UserQuestionnaireStatisticsFiltered
        {
            get
            {
                if (UsersFiltered == null || !UsersFiltered.Any())
                    return new Dictionary<Int32, IList<dtoBaseUserPathQuiz>>();
                else
                    return UsersFiltered.ToDictionary(u => u.IdPerson, u => u.Questionnaires);
            }
        }


        public Boolean HasQuestionnaires(Boolean withEvaluationActive)
        {
            return (Questionnaires != null && Questionnaires.Where(q => q.QuestionnaireInfo.EvaluationActive == withEvaluationActive).Any());
        }
        public List<long> GetIdQuestionnaires(Boolean withEvaluationActive)
        {
            return (Questionnaires != null) ? Questionnaires.Where(q => q.QuestionnaireInfo.EvaluationActive == withEvaluationActive).Select(q => q.IdQuestionnaire).ToList() : new List<long>();
        }
        //public IList<dtoSubActivityQuestionnaire> HasQuestionnaires { get; set; }

        public dtoPathUsers() { 
        
        }
    }

    public class dtoPathUserInfo
    {
        public Int32 IdPerson { get { return (User == null) ? 0 : User.Id; } }
        public litePerson User { get; set; }
        public StatusStatistic ItemStatus { get; set; }

    
        public DateTime? StStartDate { get; set; }
        public DateTime? StEndDate { get; set; }
        public DateTime? StDeadline { get; set; }
        public DateTime? StFirstActivity { get; set; }

        public Int64 Completion { get; set; }
        public Int64 Mark { get; set; }
        public IList<dtoBaseUserPathQuiz> Questionnaires { get; set; }
    }
    public class dtoSubActivityQuestionnaire
    {
        public long IdQuestionnaire { get; set; }
        public long IdSubActivity { get; set; }
        public dtoUserQuizDetails QuestionnaireInfo { get; set; }

        public dtoSubActivityQuestionnaire()
        { 
            QuestionnaireInfo = new dtoUserQuizDetails();
        }
    }
}