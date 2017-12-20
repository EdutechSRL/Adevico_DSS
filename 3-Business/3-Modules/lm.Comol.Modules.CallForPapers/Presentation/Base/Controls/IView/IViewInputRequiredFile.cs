using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewInputRequiredFile : IViewBase
    {
        dtoCallSubmissionFile GetFile { get; }
        FieldError CurrentError { get; set; }
        Boolean Disabled { get; set; }
        Boolean isValid { get; }
        Boolean Mandatory { get; set; }
        Boolean ToUpload { get; }
        long IdSubmittedFile { get; set; }
        long IdRequiredFile { get; set; }
        long IdCall { get; set; }
        long IdSubmission { get; set; }
        long IdLink { get; set; }
        Int32 IdCallCommunity { get; set; }

        void InitializeControl(long idCall, long idSubmission, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, dtoCallSubmissionFile itemFile, Boolean disabled, Boolean allowAnonymous);
        void InitializeControl(long idCall, long idSubmission, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, dtoCallSubmissionFile itemFile, Boolean disabled, FieldError err, Boolean allowAnonymous);

        dtoRequestedFileUpload AddInternalFile(UserSubmission submission, String moduleCode, Int32 idModule, Int32 moduleAction, Int32 objectType);

        void SetupView(dtoCallSubmissionFile itemFile, Int32 idUploader, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier, Boolean allowAnonymous);
        void DisplayInputError();
        void HideInputError();
        void DisplayEmptyFile();
        void RefreshFileField();
        void RefreshFileField(ModuleLink link);
        void RefreshFileField(liteModuleLink link);
    }
}