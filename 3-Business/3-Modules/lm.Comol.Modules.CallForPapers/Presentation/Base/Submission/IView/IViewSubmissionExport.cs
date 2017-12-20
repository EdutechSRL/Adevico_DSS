using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewSubmissionExport : IViewBase
    {
        Boolean isContainer { get; set; }
        Boolean RaiseContainerEvent { get; set; }
        CallForPaperType CallType {get;set;}
        long IdSubmission { get; set; }
        long IdRevision { get; set; }
        String SubmissionOwner { get; set; }
        Boolean isSubmissionOwner { get; set; }
        List<lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType> AvailableTypes { get; set; }
        lm.Comol.Core.DomainModel.Helpers.ExternalPageContext SkinDetails { get; set; }


        Int32 IdUserSubmitter { get; set; }
        String UserLanguageCode { get; set; }
        String DefaultLanguageCode { get; set; }

        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template GetTemplate();

        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template GetTemplate(
            lm.Comol.Core.DomainModel.Helpers.ExternalPageContext context, 
            String userLanguageCode, 
            String defaultLanguageCode);

        void InitializeControl(Boolean isOwner, Int32 idUser, String owner, long idCall, long idSubmission, long idRevision, Int32 idModule, Int32 idCallCommunity, CallForPaperType callType);
        void InitializeControl(Boolean isOwner, Int32 idUser, String owner, long idCall, long idSubmission, long idRevision, Int32 idModule, Int32 idCallCommunity, CallForPaperType callType, List<lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType> loadTypes);
        void LoadFiles(List<dtoSubmissionAttachment> files, List<lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType> availableTypes);
        void DisplayNone();
        void RefreshContainer();
    }
}