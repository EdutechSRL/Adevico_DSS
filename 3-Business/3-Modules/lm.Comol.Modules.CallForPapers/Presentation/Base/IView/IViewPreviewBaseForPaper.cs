using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewPreviewBaseForPaper: lm.Comol.Core.DomainModel.Common.iDomainView 
    {

        long PreloadIdCall { get; }
        int PreloadIdCommunity { get; }
        CallStatusForSubmitters PreloadView { get; }
        long IdCall { get; set; }
        Int32 IdCallModule { get; set; }
        int IdCommunity { get; set; }
        CallForPaperType CallType { get; set; }
        long IdSubmitterType { get; set; }
        String Portalname { get; }
        Boolean HasMultipleSubmitters { get; set; }

        void LoadUnknowCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void LoadSubmitterTypes(List<dtoSubmitterType> submitters);
        void LoadAttachments(List<dtoAttachmentFile> items);
      
        void LoadSections(List<dtoCallSection<dtoSubmissionValueField>> sections);
       
        void SetContainerName(String communityName, String itemName);
        void SetSubmitterName(String submitterName);
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
        void SetGenericSubmitterName();
    }
}
