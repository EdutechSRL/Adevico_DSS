using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewManageEvaluations : IViewBaseEditEvaluationSettings
    {
        String AnonymousTranslation { get; }
        DateTime? EndEvaluationOn { get; set; }
        ManageEvaluationsAction CurrentAction { get; set; }
        String FilterByName {get;set; }
        long FilterByType { get; set; }
        String SelectedSubmissionName { get; }
        long SelectedIdSubmitterType { get; }

        Boolean DisplayByEvaluator { get; set; }
        Boolean AllowSaveStatus { get; set; }
        Boolean SelectAllItems { get; set; }
        Int32 CurrentPageSize { get; set; }
        PagerBase Pager { get; set; }
        List<long> SelectedEvaluations { get; set; }


        void LoadAvailableActions(List<ManageEvaluationsAction> items);
        void LoadItems(List<dtoBasicCommitteeItem> items);
        Dictionary<long, List<long>> GetSelectedItemsForEvaluators();


        void LoadItems(List<dtoBasicSubmissionItem> items);
        void LoadSubmitterstype(List<dtoSubmitterType> submitters, long dSubmitter);
        List<dtoSelectEvaluationItem> GetCurrentSumbissionsItems();

        //void DisplayNoEvaluations();
        void DisplayNoEvaluations(Boolean showFilters, Boolean showNavigation, Boolean showDisplaySelector);
        void DisplayDateChangingError();
        void DisplayEndEvaluationDateSaved();
        void DisplayStatusEditingError(Boolean forClosing);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}