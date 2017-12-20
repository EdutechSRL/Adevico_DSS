using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewSelectUsersFromCall : IViewBase
    {
        long SelectedIdCall { get; set; }
        Boolean isInitialized { get; set; }
        Boolean FromPortal { get; set; }
        Boolean RaiseEvents { get; set; }
        CallForPaperType CallsType { get; set; }
        List<Int32> FromCommunities { get; set; }
        List<Int32> RemoveUsers { get; set; }

        String PortalName { get; }
        String UnknownCommunityName { get; }

        Int32 CallsPageSize { get; set; }
        PagerBase CallsPager { get; set; }
        Int32 SubmissionsPageSize { get; set; }
        PagerBase SubmissionsPager { get; set; }

        dtoFilterSubmissions CurrentFilter { get; set; }
        dtoFilterSubmissions SelectedFilter { get;  }
  
        List<long> SelectedSubmissions { get; set; }
        Boolean SelectAllSubmissions { get; set; }

        void InitializeControl(CallForPaperType type, Boolean fromPortal, List<Int32> fromCommunities,List<Int32> removeUsers);
        void LoadCalls(List<dtoCallInfo> calls);
        void LoadAvailableCalls();

        void DisplayCallSubmissions(String callName);
        void LoadAvailableStatus(List<SubmissionFilterStatus> items, SubmissionFilterStatus selected);
        void LoadSubmitterstype(Dictionary<long, string> submitters, long dSubmitter);
        List<dtoSelectItem<long>> GetCurrentSelectedItems();
        void LoadSubmissions(List<dtoSubmissionDisplayItem> submissions);
        void DisplayNoSubmissionsAvailable();
        void DisplayNoCallsAvailable();
    }
}