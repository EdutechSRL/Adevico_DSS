using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    public class SubmissionNotFound : Exception
    {
        public SubmissionNotFound()
        {
        }

        public SubmissionNotFound(string message)
            : base(message)
        {
        }

        public SubmissionNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SubmissionNotSaved : Exception
    {
        public SubmissionNotSaved()
        {
        }

        public SubmissionNotSaved(string message)
            : base(message)
        {
        }

        public SubmissionNotSaved(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SubmissionNotCreated : Exception
    {
        public SubmissionNotCreated()
        {
        }

        public SubmissionNotCreated(string message)
            : base(message)
        {
        }

        public SubmissionNotCreated(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SubmissionInternalFileNotLinked : Exception
    {
        public SubmissionInternalFileNotLinked()
        {
        }

        public SubmissionInternalFileNotLinked(string message)
            : base(message)
        {
        }

        public SubmissionInternalFileNotLinked(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SubmissionStatusChange : Exception
    {
        public SubmissionStatusChange()
        {
        }

        public SubmissionStatusChange(string message)
            : base(message)
        {
        }

        public SubmissionStatusChange(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SubmissionItemsEmpty : Exception
    {
        public SubmissionItemsEmpty()
        {
        }

        public SubmissionItemsEmpty(string message)
            : base(message)
        {
        }

        public SubmissionItemsEmpty(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SubmissionTimeExpired : Exception
    {
        public SubmissionTimeExpired()
        {
        }

        public SubmissionTimeExpired(string message)
            : base(message)
        {
        }

        public SubmissionTimeExpired(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SubmissionCallUnavailable : Exception
    {
        public SubmissionCallUnavailable()
        {
        }

        public SubmissionCallUnavailable(string message)
            : base(message)
        {
        }

        public SubmissionCallUnavailable(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class CallForPaperInternalFileNotLinked : Exception
    {
        public CallForPaperInternalFileNotLinked()
        {
        }

        public CallForPaperInternalFileNotLinked(string message)
            : base(message)
        {
        }

        public CallForPaperInternalFileNotLinked(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class SubmissionLinked : Exception
    {
        public long SubmissionCount { get; set; }
        public SubmissionLinked()
        {
        }

        public SubmissionLinked(string message)
            : base(message)
        {
        }

        public SubmissionLinked(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class EvaluationsLinked : Exception
    {
        public long EvaluationsCount { get; set; }
        public EvaluationsLinked()
        {
        }

        public EvaluationsLinked(string message)
            : base(message)
        {
        }

        public EvaluationsLinked(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class EvaluationStarted : Exception
    {
        public long EvaluationsCount { get; set; }
        public EvaluationStarted()
        {
        }

        public EvaluationStarted(string message)
            : base(message)
        {
        }

        public EvaluationStarted(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SkipRequiredSteps : Exception
    {
        public List<WizardCallStep> Steps { get; set; }
        public SkipRequiredSteps()
        {
            Steps = new List<WizardCallStep>();
        }

        public SkipRequiredSteps(string message)
            : base(message)
        {
            Steps = new List<WizardCallStep>();
        }

        public SkipRequiredSteps(string message, Exception inner)
            : base(message, inner)
        {
            Steps = new List<WizardCallStep>();
        }
    }
    public class CallForPaperInvalidStatus : Exception
    {
        public CallForPaperInvalidStatus()
        {
        }

        public CallForPaperInvalidStatus(string message)
            : base(message)
        {
        }

        public CallForPaperInvalidStatus(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class UnknownCallForPaper : Exception
    {
        public UnknownCallForPaper()
        {
        }

        public UnknownCallForPaper(string message)
            : base(message)
        {
        }

        public UnknownCallForPaper(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class CallForPaperItemSave : Exception
    {
        public String Name { get; set; }

        public CallForPaperItemSave()
        {
        }

        public CallForPaperItemSave(string message)
            : base(message)
        {
        }

        public CallForPaperItemSave(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class CallForPaperItemDelete : Exception
    {
        public String Name { get; set; }
        public CallForPaperItemDelete()
        {
        }

        public CallForPaperItemDelete(string message)
            : base(message)
        {
        }

        public CallForPaperItemDelete(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class CallForPaperItemUndelete : Exception
    {
        public String Name { get; set; }
        public CallForPaperItemUndelete()
        {
        }

        public CallForPaperItemUndelete(string message)
            : base(message)
        {
        }

        public CallForPaperItemUndelete(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class UnknownItem : Exception
    {
        public UnknownItem()
        {
        }

        public UnknownItem(string message)
            : base(message)
        {
        }

        public UnknownItem(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class ItemDeleteWithSubmission : Exception
    {
        public String Name { get; set; }
        public long SubmissionCount { get; set; }
        public ItemDeleteWithSubmission()
        {
        }

        public ItemDeleteWithSubmission(string message)
            : base(message)
        {
        }

        public ItemDeleteWithSubmission(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    //public class CriterionEvaluationError : Exception
    //{
    //    public List<dtoEvaluationCriterion> Criteria { get; set; }
    //    public CriterionEvaluationError()
    //    {
    //        Criteria = new List<dtoEvaluationCriterion>();
    //    }

    //    public CriterionEvaluationError(string message)
    //        : base(message)
    //    {
    //        Criteria = new List<dtoEvaluationCriterion>();
    //    }

    //    public CriterionEvaluationError(string message, Exception inner)
    //        : base(message, inner)
    //    {
    //        Criteria = new List<dtoEvaluationCriterion>();
    //    }
    //}
    public class EvaluationCompleteError : Exception
    {
        public long CriteriaToEvaluate { get; set; }
        public EvaluationCompleteError()
        {
        }

        public EvaluationCompleteError(string message)
            : base(message)
        {
        }

        public EvaluationCompleteError(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class CriterionEvaluatedError : Exception
    {
        public List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated> Criteria { get; set; }
        public CriterionEvaluatedError()
        {
            Criteria = new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated>();
        }

        public CriterionEvaluatedError(string message)
            : base(message)
        {
            Criteria = new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated>();
        }

        public CriterionEvaluatedError(string message, Exception inner)
            : base(message, inner)
        {
            Criteria = new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated>();
        }
        public CriterionEvaluatedError(List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated> criteria, Exception inner)
            : base("", inner)
        {
            Criteria = criteria;
        }
    }
    public class EvaluationError : Exception
    {
        public List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated> Criteria { get; set; }
        public EvaluationError()
        {
            Criteria = new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated>();
        }

        public EvaluationError(string message)
            : base(message)
        {
            Criteria = new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated>();
        }

        public EvaluationError(string message, Exception inner)
            : base(message, inner)
        {
            Criteria = new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated>();
        }
    }
    public class ExportError : Exception
    {

        public iTextSharp5.text.Document ErrorPdfDocument { get; set; }
        public iTextSharp.text.Document ErrorDocument { get; set; }
        public String ExcelDocument { get; set; }
        public ExportError()
        {
        }

        public ExportError(string message)
            : base(message)
        {
        }

        public ExportError(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
   }
