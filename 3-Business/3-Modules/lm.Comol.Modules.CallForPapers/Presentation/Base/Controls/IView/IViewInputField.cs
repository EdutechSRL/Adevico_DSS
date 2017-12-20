using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewInputField : IViewBase
    {
        long IdSubmittedField { get; set; }
        long IdField { get; set; }
        FieldType FieldType { get; set; }
        FieldError CurrentError { get; set; }
        DisclaimerType DisclaimerType { get; set; }
        List<dtoFieldOption> Options { get; set; }
        Boolean Disabled { get; set; }
        Boolean isValid { get; }
        Boolean Mandatory { get; set; }
        Boolean ReviewMode { get; set; }
        long IdCall { get; set; }
        long IdSubmission { get; set; }
        long IdLink { get; set; }
        Int32 IdCallCommunity { get; set; }

        Int32 MaxChars { get; set; }
        Int32 MaxOptions { get; set; }
        Int32 MinOptions { get; set; }

        void InitializeControl(long idCall, long idSubmission, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, dtoSubmissionValueField field, Boolean disabled, Boolean isPublic);
        void InitializeControl(long idCall, long idSubmission, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, dtoSubmissionValueField field, Boolean disabled, Boolean isPublic, FieldError err);
        dtoSubmissionValueField GetField();
        void SetupView(dtoSubmissionValueField field, Int32 idUploader, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, Boolean isPublic);
        void DisplayInputError();
        void HideInputError();
        void DisplayEmptyField();
        void RefreshFileField(ModuleLink link);
    }
}