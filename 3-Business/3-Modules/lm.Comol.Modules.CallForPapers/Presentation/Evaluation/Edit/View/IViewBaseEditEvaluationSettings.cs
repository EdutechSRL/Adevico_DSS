using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewBaseEditEvaluationSettings : IViewBase
    {
        long PreloadIdCall { get; }
        int PreloadIdCommunity { get; }
        lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters PreloadView { get; }

        long IdCall { get; set; }
        Int32 IdCallModule { get; set; }
        int IdCommunity { get; set; }
        String Portalname { get; }
        Boolean AllowSave { get; set; }


        void LoadUnknowCall(int idCommunity, int idModule, long idCall, lm.Comol.Modules.CallForPapers.Domain.CallForPaperType type);
        void SetContainerName(String itemName);
        void SetContainerName(String communityName, String itemName);
        void LoadWizardSteps(long idCall, int idCommunity, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps);
        void LoadWizardSteps(long idCall, int idCommunity, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps, EvaluationEditorErrors err);

        void SetActionUrl(String url);
        void RedirectToUrl(String url);

    }
}