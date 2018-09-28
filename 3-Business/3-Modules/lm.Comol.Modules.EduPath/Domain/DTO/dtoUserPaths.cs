using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class dtoUserPaths
    {
        public Int32 Id { get; set; }

        public litePerson Person { get; set; }

        public Int32 NotStarted { get; set; }

        public Int32 Started { get; set; }

        public Int32 Completed { get; set; }

        public Int32 Total { get; set; }

        public IList<dtoUserPathInfo> Paths { get; set; }
        public IList<dtoUserPathQuiz> QuizInfos {
            get {
                if (Paths == null || !Paths.Any())
                    return new List<dtoUserPathQuiz>();
                else
                    return Paths.SelectMany(p => p.Questionnaires).ToList();
            }
        }
        public ILookup<int,List<dtoUserPathInfo>> PathsByCommunity
        {
            get
            {
                return (from item in Paths group item by item.IdCommunity into gr select gr).ToLookup(x => x.Key,x=>x.ToList());
            }
        }


    }

    public class dtoUserPathInfo
    {
        public dtoFullPathItem PathInfo { get; set; }
        public Int32 IdPerson { get; set; }
        public Int64 IdPath { get { return (PathInfo == null) ? 0 : PathInfo.Id; } }
        public String PathName { get { return (PathInfo == null) ? Guid.NewGuid().ToString() : PathInfo.Name ; } }
        public EPType PathType { get { return (PathInfo == null) ? EPType.None : PathInfo.EPType; } }

        public Boolean IsMooc { get { return (PathInfo == null) ? false : PathInfo.IsMooc; } }

        //public   { get; set; }    
        public Int32 IdOrganization { get; set; }
        public String OrganizationName { get; set; }
        public Int32 IdCommunity { get; set; }
        public String CommunityName { get; set; }
        public Boolean PathLocked { get; set; }
        //
        //public DateTime? PathEndDate { get; set; }
        public StatusStatistic ItemStatus { get; set; }

        public String Status { get; set; }

        public String Completion { get; set; }


        // PERCHE' STRING ?!?!?!?!??!?!?!?!?!? 
        // NON SI POSSONO USARE DATETIME? E POI I METODI TO STRING
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String FirstActivity { get; set; }      

        public String Deadline { get; set; }

        public DateTime? StStartDate { get; set; }
        public DateTime? StEndDate { get; set; }
        public DateTime? StFirstActivity { get; set; }
        public DateTime? StDeadline { get; set; }

        public PathStatistic Ps { get; set; }
        public PathStatistic PsFirstActivity { get; set; }

        public Boolean CanManage { get; set; }
        public Boolean CanPlay { get; set; }
        public Boolean CanStat { get; set; }

        public IList<dtoUserPathQuiz> Questionnaires { get; set; }

        //public int Order { get; set; }
    }
    public class dtoBaseUserPathQuiz
    {
        public Int32 IdPerson { get; set; }
        public Int64 IdQuestionnaire { get; set; }
        public Int64 IdSub { get; set; }

       
        public IList<dtoUserQuizAnswer> Answers { get; set; }
        public Dictionary<Int32, dtoItemEvaluation<long>> EvaluationsInfo { get; set; }
        public dtoBaseUserPathQuiz()
        {
            Answers = new List<dtoUserQuizAnswer>();
            EvaluationsInfo = new Dictionary<Int32, dtoItemEvaluation<long>>();
        }
    }
    public class dtoUserPathQuiz :dtoBaseUserPathQuiz
    {
        public dtoUserQuizDetails QuestionnaireInfo { get; set; }
        public dtoUserPathQuiz() :base() {
            QuestionnaireInfo = new dtoUserQuizDetails();
        }
    }
    public class dtoUserQuizDetails
    {
        public String Name { get; set; }
        public Boolean EvaluationActive { get; set; }
        public Int32 MaxAttemptsNumber { get; set; }
        public Boolean AllowMultipleAttempts { get; set; }
        public Int32 MinScore { get; set; }
        public Int32 EvaluationScale { get; set; }
        
    }
    public class dtoUserQuizAnswer
    {
        public Int32 IdAnswer { get; set; }
        public Int32 AttemptNumber { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Int32? QuestionsSkipped { get; set; }
        public Int32? QuestionsCount { get; set; }
        public Int32? CorrectAnswers { get; set; }
        public Int32? WrongAnswers { get; set; }
        public Int32? UngradedAnswers { get; set; }
        public Int32? SemiCorrectAnswers { get; set; }
        public Decimal? Score { get; set; }
        public Decimal? RelativeScore { get; set; }
        public Boolean IsDeleted { get; set; }

        public Int32 QuestionAnswers {
            get {
                Int32 count = 0;
                count += (CorrectAnswers.HasValue) ? CorrectAnswers.Value : 0;
                count += (WrongAnswers.HasValue) ? WrongAnswers.Value : 0;
                count += (UngradedAnswers.HasValue) ? UngradedAnswers.Value : 0;
                count += (SemiCorrectAnswers.HasValue) ? SemiCorrectAnswers.Value : 0;

                return count;
            }
        }
    }
}
