using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoSubmissionMultipleAssignment
    {
        public virtual long IdSubmission { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual long IdSubmitterType { get; set; }
        public virtual String SubmitterType { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual List<dtoCommitteeAssignment> Committees { get; set; }
        public virtual long EvaluatorsCount { get { return (Committees == null) ? 0 : Committees.Select(c => c.Evaluators.Count()).Sum(); } }
        public virtual Boolean NoEvaluators { get { return (Committees == null || (Committees!=null && Committees.Where(c=>c.Evaluators.Count==0).Any()));} }
        
        public dtoSubmissionMultipleAssignment()
        {
            Committees = new List<dtoCommitteeAssignment>();
        }

        public List<dtoCommitteePartialAssignment> GetPartialAssignments(Dictionary<long, Dictionary<long, String>> evaluators)
        {
            List<dtoCommitteePartialAssignment> items = new List<dtoCommitteePartialAssignment>();
            items.AddRange(Committees.Where(c => c.Evaluators.Count != evaluators[c.IdCommittee].Values.Count).Select(c=>new dtoCommitteePartialAssignment()
                                    { CommitteeName=c.Name, Display = c.Display , SubmitterName= this.DisplayName, SubmitterType= this.SubmitterType,
                                        SubmittedOn = this.SubmittedOn,AssignedEvaluators = c.Evaluators.Count, AvailableEvaluators=evaluators[c.IdCommittee].Values.Count
                                        , IdSubmission= this.IdSubmission}).ToList());
            return items;
        }
    }
    [Serializable]
    public class dtoCommitteeAssignment
    {
        public virtual long IdCommittee { get; set; }
        public virtual long IdSubmission { get; set; }
        public virtual String Name { get; set; }
        public virtual displayAs Display { get; set; }
        public virtual List<long> Evaluators { get; set; }

        public dtoCommitteeAssignment()
        {
            Evaluators = new List<long>();
            Display = displayAs.item;
        }
    }

    [Serializable]
    public class dtoCommitteePartialAssignment
    {
        public virtual long IdSubmission { get; set; }
        public virtual String SubmitterName { get; set; }
        public virtual String SubmitterType { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual String CommitteeName { get; set; }
        public virtual displayAs Display { get; set; }
        public virtual Int32 AssignedEvaluators { get; set; }
        public virtual Int32 AvailableEvaluators { get; set; }

        public dtoCommitteePartialAssignment()
        {
            Display = displayAs.item;
        }
    }
}