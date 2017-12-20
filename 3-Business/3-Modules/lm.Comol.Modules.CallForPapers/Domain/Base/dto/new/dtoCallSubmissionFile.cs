using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    public class dtoCallSubmissionFile :dtoBase
    {
        public virtual Int32 DisplayOrder { get { return (FileToSubmit == null) ? 0 : FileToSubmit.DisplayOrder; } }
        public virtual dtoCallRequestedFile FileToSubmit { get; set; }
        public virtual dtoSubmittedFile Submitted { get; set; }
        public virtual Boolean AllowRemove { get; set; }
        public virtual Boolean AllowUpload { get; set; }
        public virtual FieldError FieldError { get; set; }
        public dtoCallSubmissionFile() {
            FieldError = Domain.FieldError.None;
        }
        public dtoCallSubmissionFile(RequestedFile requestedFile,  Boolean allowRemove, Boolean allowUpload)
        {
            Id = requestedFile.Id;
            FileToSubmit = new dtoCallRequestedFile(requestedFile);
            AllowRemove = allowRemove;
            AllowUpload = allowUpload;
            FieldError = Domain.FieldError.None;
        }
        public dtoCallSubmissionFile(RequestedFile requestedFile, SubmittedFile submittedFile, Boolean allowRemove, Boolean allowUpload)
        {
            Id = requestedFile.Id;
            FileToSubmit = new dtoCallRequestedFile(requestedFile);
            if (submittedFile!=null)
                Submitted = new dtoSubmittedFile(submittedFile);
            AllowRemove = allowRemove;
            AllowUpload = allowUpload;
            FieldError = Domain.FieldError.None;
        }

        public void SetSubmittedFile(SubmittedFile submittedFile, Boolean allowRemove, Boolean allowUpload)
        {
            if (submittedFile != null)
                Submitted = new dtoSubmittedFile(submittedFile);
            AllowRemove = allowRemove;
            AllowUpload = allowUpload;
        }
        public void SetError(Dictionary<long, FieldError> errors)
        {
            if (errors == null)
                FieldError = Domain.FieldError.None;
            else if (errors.ContainsKey(Id))
                FieldError = errors[Id];
            else
                FieldError = Domain.FieldError.None;
        }
    }
}