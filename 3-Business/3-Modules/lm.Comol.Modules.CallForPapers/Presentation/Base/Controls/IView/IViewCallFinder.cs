using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewCallFinder :lm.Comol.Core.DomainModel.Common.iDomainView {
        long CurrentIdItem { get; set; }
        Boolean AllowPreview { set; }
        Boolean isInitialized { get; set; }
        Boolean OnlyAnonymousSubmissions { get; set; }
        Boolean AlsoAnonymousSubmissions { get; set; }
        Boolean AllowViewList { set; }
        SearchRange Range { get; set; }
        CallForPaperType ItemType { get; set; }
        Int32 FromIdCommunity { get; set; }
        DateTime? FromDate { get; set; }
        SubmissionStatus SelectedStatus { get; set; }

        Boolean isValid { get; set; }
        List<ProfileColumnComparer<String>> AvailableColumns { get; set; }
        List<ExternalColumnComparer<String,String>> StandardAvailableColumns { get; set; }


        void InitializeControl();
        void InitializeControl(CallForPaperType type, SearchRange range, Int32 idCommunity);
        void InitializeControl(long idItem);
        void DisplaySelectionError();
        void PreviewRows(lm.Comol.Core.DomainModel.Helpers.ExternalResource submissions);

        void LoadCalls(List<dtoCallToFind> calls);
        void LoadCallInfo(dtoCallToFind call); 
        void LoadAvailableStatus(List<SubmissionStatus> status);
        ProfileExternalResource RetrieveItemSubmissions();
        List<ProfileColumnComparer<String>> GetAvailableColumns();
        List<ExternalColumn> GetStandardAvailableColumns();

        ProfileExternalResource GetItemSubmissions(List<ProfileColumnComparer<String>> columns);

        // ExternalResource GetItemSubmissions(List<ExternalColumnComparer<String, String>> columns);
        void DisplaySessionTimeout();

        long CurrentSubmitterId { get; set;}
        void LoadSubmitterType(Dictionary<long,string> submitters); //oppure mandare alla DDL una lista di dictionary
        
    }
}
