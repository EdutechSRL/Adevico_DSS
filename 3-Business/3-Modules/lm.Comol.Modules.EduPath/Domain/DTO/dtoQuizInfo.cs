using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoQuizInfo
    {
        public long IdQuestionnaire { get; set; }
        public String Name { get; set; }
        public Int32 EvaluationScale { get; set; }
        public Int32 MinScore { get; set; }
        public List<dtoQuizAttemptInfo> Attempts { get; set; }
        public dtoQuizAttemptInfo LastAttempt { get { return Attempts.OrderByDescending(a => a.CompletedOn).ToList().FirstOrDefault(); } }
        public Boolean Evaluable { get; set; }
        public Boolean Passed { get { 
            return Attempts.Any() && (LastAttempt.Completed && (!Evaluable || (Evaluable && LastAttempt.Passed)));
        } }
        public dtoQuizInfo() {
            Attempts = new List<dtoQuizAttemptInfo>();
        }
        public Boolean PassedByDate(DateTime? before)
        {
            if (!before.HasValue)
                return Passed;
            else {
                dtoQuizAttemptInfo last = Attempts.Where(q=> q.CompletedOn.HasValue && q.CompletedOn<= before.Value).OrderByDescending(a => a.CompletedOn).ToList().FirstOrDefault();
                return last != null && (last.Completed && (!Evaluable || (Evaluable && last.Passed)));
            }
        }
        public Boolean CompiledBeforeDate(DateTime? before)
        {
            if (!before.HasValue)
                return Attempts.Any();
            else
            {
                dtoQuizAttemptInfo last = Attempts.Where(q => !q.CompletedOn.HasValue || q.CompletedOn <= before.Value).OrderByDescending(a => a.Id).ToList().FirstOrDefault();
                return last != null;
            }
        }
    }

    [Serializable]
    public class dtoQuizAttemptInfo
    {
        public long Id { get; set; }
        public long IdQuestionnaire { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Int32? QuestionsSkipped { get; set; }
        public Int32? QuestionsCount { get; set; }
        public Int32? CorrectAnswers { get; set; }
        public Int32? WrongAnswers { get; set; }
        public Int32? UngradedAnswers { get; set; }
        public Decimal? Score { get; set; }
        public Decimal? RelativeScore { get; set; }
        public Boolean Completed { get; set; }
        public Boolean Passed { get; set; }
    }
}