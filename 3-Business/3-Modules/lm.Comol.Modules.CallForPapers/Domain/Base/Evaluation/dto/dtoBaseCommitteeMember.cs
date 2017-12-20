using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoBaseCommitteeMember
    {
        public virtual long IdMembership { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual long IdCallEvaluator { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }
        public virtual String DisplayName { get; set; }
        
        public virtual MembershipStatus Status { get; set; }
        public virtual litePerson ReplacedBy { get; set; }
        public virtual litePerson ReplacingUser { get; set; }
        public virtual CallEvaluator ReplacedByEvaluator { get; set; }
        public virtual CallEvaluator ReplacingEvaluator { get; set; }

        public dtoBaseCommitteeMember(){}

        public dtoBaseCommitteeMember(CommitteeMember membership)
        {
            IdMembership = membership.Id;
            Status = membership.Status;
            IdCallEvaluator = (membership.Evaluator == null) ? 0 : membership.Evaluator.Id;
            IdPerson = (membership.Evaluator == null || (membership.Evaluator.Person == null)) ? 0 : membership.Evaluator.Person.Id;
            if (IdPerson > 0) {
                Name = membership.Evaluator.Person.Name;
                Surname = membership.Evaluator.Person.Surname;
                DisplayName = membership.Evaluator.Person.SurnameAndName;
            }
        }
        public dtoBaseCommitteeMember(CommitteeMember membership, String anonymousName) : this(membership)
        {
            if (IdPerson == 0)
                DisplayName = anonymousName;
        }


        public dtoBaseCommitteeMember(expCommitteeMember membership)
        {
            IdMembership = membership.Id;
            Status = membership.Status;
            IdCallEvaluator = (membership.Evaluator == null) ? 0 : membership.Evaluator.Id;
            IdPerson = (membership.Evaluator == null || (membership.Evaluator.Person == null)) ? 0 : membership.Evaluator.Person.Id;
            if (IdPerson > 0) {
                Name = membership.Evaluator.Person.Name;
                Surname = membership.Evaluator.Person.Surname;
                DisplayName = membership.Evaluator.Person.SurnameAndName;
            }
        }
         public dtoBaseCommitteeMember(expCommitteeMember membership, String anonymousName)
             : this(membership)
        {
            if (IdPerson == 0)
                DisplayName = anonymousName;
        }
        
    }
}