using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewImportAgencies : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean AllowManagement { set; }
        System.Guid ImportIdentifier { get; set; }

        AgencyImportStep CurrentStep { get; set; }
        List<AgencyImportStep> AvailableSteps { get; set; }
        List<AgencyImportStep> SkipSteps { get; set; }


        Boolean IsInitialized(AgencyImportStep pStep);
        void GotoStep(AgencyImportStep pStep);
        void GotoStep(AgencyImportStep pStep, Boolean initialize);
        void InitializeStep(AgencyImportStep pStep);

//    ' STEP CSV
        CsvFile RetrieveFile();
        ExternalResource GetFileContent(List<ExternalColumnComparer<String, Int32>> columns);
        List<ExternalColumnComparer<String, Int32>> AvailableColumns();

//    'SETP
        List<ExternalColumnComparer<String, Int32>> Fields { get; }
        Boolean ValidDestinationFields { get; }
        void InitializeFieldsMatcher(List<ExternalColumnComparer<String, Int32>> sourceColumns);

        //Step select Items
        ExternalResource  SelectedItems{ get;}
        Boolean  HasSelectedItems{ get;}
        Int32  ItemsToSelect{ get;}
        void UpdateSourceItems();

        //Step select Organizations
        Boolean AvailableForAll { get; }
        Dictionary<Int32, String> SelectedOrganizations { get; }
        Boolean HasAvailableOrganizations { get; }

//    'STEP SUMMARY
        Boolean isCompleted { get; set; }
        void SetupProgressInfo(Int32 agencyCount);
        void UpdateAgencyCreation(Int32 agencyCount, Int32 agencyIndex, Boolean created, String displayName);

//    '''/STEP SUMMARY
        void DisplayImportedAgencies(Int32 importedItems,Int32 itemsToImport);
        void DisplayError(AgencyImportStep currentStep);
        void DisplayImportError(Int32 importedItems, List<String> notCreatedItems);
        AgencyImportStep PreviousStep { get; set; }

        void DisplaySessionTimeout();
        void DisplayNoPermission();
    }
}