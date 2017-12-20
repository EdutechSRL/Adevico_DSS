using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewBaseSubmitCall : IViewBaseSubmission
    {
        Boolean HasMultipleSubmitters { get; set; }
        DateTime InitSubmissionTime { get; set; }
        Boolean AllowSubmitterSelection { set; }
        Boolean AllowSave { get; set; }
        Boolean AllowCompleteSubmission { get; set; }
        Boolean AllowDeleteSubmission { get; set; }
        lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier CallRepository { get; set; }

        void InitializeView(lm.Comol.Core.DomainModel.Helpers.ExternalPageContext skin, Boolean displayProgress);
        void InitializeView(Boolean displayProgress);
        void InitializeEmptyView();
        void SetSubmitterName(String submitterName);
        void SetGenericSubmitterName();
        void DisplaySubmissionTimeBefore(DateTime submitBefore);
        void DisplaySubmissionTimeAfter(DateTime? submitAfter);
        void DisplayCallUnavailableForPublic();
        void DisplayCallUnavailable(CallForPaperStatus status);
        void LoadError(SubmissionErrorView error, int idCommunity, long idCall, CallStatusForSubmitters view);
    }
}
