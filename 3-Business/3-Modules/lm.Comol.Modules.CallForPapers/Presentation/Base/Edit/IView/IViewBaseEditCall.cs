using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewBaseEditCall : IViewBase
    {
        long PreloadIdCall { get; }
        int PreloadIdCommunity { get; }
        CallStatusForSubmitters PreloadView { get; }
        CallForPaperType PreloadType { get; }

        long IdCall { get; set; }
        Int32 IdCallModule { get; set; }
        int IdCommunity { get; set; }
        CallForPaperType CallType { get; set; }
        String Portalname { get; }
        Boolean AllowSave { get; set; }
        Boolean AllowUpdateTags { set; }

        void LoadUnknowCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void SetContainerName(CallStandardAction action, String communityName, String itemName);
        void LoadWizardSteps(long idCall, CallForPaperType type, int idCommunity, List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardCallStep>> steps);
        void SetActionUrl(CallStandardAction action, String url);

    }
}