using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement.Business;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using System.Globalization;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class AgenciesImportPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            private ServiceCommunityManagement _ServiceCommunity;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewImportAgencies View
            {
                get { return (IViewImportAgencies)base.View; }
            }
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            private ServiceCommunityManagement ServiceCommunity
            {
                get
                {
                    if (_ServiceCommunity == null)
                        _ServiceCommunity = new ServiceCommunityManagement(AppContext);
                    return _ServiceCommunity;
                }
            }
            public AgenciesImportPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AgenciesImportPresenter(iApplicationContext oContext, IViewImportAgencies view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.EditProfile || module.Administration || module.ViewProfiles);
                if (module.AddProfile || module.Administration)
                {
                    View.PreviousStep =  AgencyImportStep.None;
                    View.ImportIdentifier = System.Guid.NewGuid();
                    InitializeSteps();
                    View.GotoStep(AgencyImportStep.SourceCSV, true);
                }
                else
                    View.DisplayNoPermission();
            }
        }

        private void InitializeSteps() {
            List<AgencyImportStep> steps = new List<AgencyImportStep>();
            steps.Add(AgencyImportStep.SourceCSV);
            steps.Add(AgencyImportStep.FieldsMatcher);
            steps.Add(AgencyImportStep.ItemsSelctor);
            steps.Add(AgencyImportStep.OrganizationAvailability);
            steps.Add(AgencyImportStep.Summary);

            View.AvailableSteps = steps;
            View.SkipSteps = new List<AgencyImportStep>();    
        }

        public void MoveToNextStep(AgencyImportStep step)
        {
            switch (step)
            {
                case AgencyImportStep.SourceCSV:
                    MoveFromSelectedSource();
                    break;
                case AgencyImportStep.FieldsMatcher:
                    MoveFromFieldsMatcher();
                    break;
                case AgencyImportStep.ItemsSelctor:
                    MoveFromItemsSelector();
                    break;
                case AgencyImportStep.OrganizationAvailability:
                    MoveFromOrganizationAvailability();
                    break;
            }
        }
        public void MoveToPreviousStep(AgencyImportStep step)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                switch (step)
                {
                    case AgencyImportStep.FieldsMatcher:
                        View.GotoStep(AgencyImportStep.SourceCSV);
                        break;
                    case AgencyImportStep.ItemsSelctor:
                        View.GotoStep(AgencyImportStep.FieldsMatcher);
                        break;
                    case AgencyImportStep.OrganizationAvailability:
                        View.GotoStep(AgencyImportStep.ItemsSelctor);
                        break;
                    case AgencyImportStep.Summary:
                        View.GotoStep(AgencyImportStep.OrganizationAvailability);
                        break;
                    case AgencyImportStep.ImportCompleted:
                        if (View.ItemsToSelect > 0 && View.PreviousStep != AgencyImportStep.None)
                            View.GotoStep(View.PreviousStep);
                        else
                            View.GotoStep(AgencyImportStep.SourceCSV, true);
                        View.PreviousStep = AgencyImportStep.None;
                        break;
                    case AgencyImportStep.ImportWithErrors:
                        if (View.ItemsToSelect > 0 && View.PreviousStep != AgencyImportStep.None)
                            View.GotoStep(View.PreviousStep);
                        else
                            View.GotoStep(AgencyImportStep.SourceCSV, true);
                        View.PreviousStep = AgencyImportStep.None;
                        break;
                    case AgencyImportStep.Errors:
                        View.GotoStep(View.PreviousStep);
                        break;
                }
            }
        }

        private void MoveFromSelectedSource()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<ExternalColumnComparer<String, Int32>> columns = View.AvailableColumns();
                if (columns.Count > 0) {
                    View.InitializeFieldsMatcher(columns);
                    View.GotoStep(AgencyImportStep.FieldsMatcher, false);
                }
            }
        }
        private void MoveFromFieldsMatcher()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.ValidDestinationFields)
            {
                View.GotoStep(AgencyImportStep.ItemsSelctor, true);
            }
        }
        private void MoveFromItemsSelector()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.HasSelectedItems)
            {
                View.GotoStep(AgencyImportStep.OrganizationAvailability, true );
            }
        }
        private void MoveFromOrganizationAvailability()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.AvailableForAll || View.SelectedOrganizations.Count>0 )
            {
                View.GotoStep(AgencyImportStep.Summary, true );
            }
        }
        
        private void UpdateStepsToSkip(AgencyImportStep step, Boolean add)
        {
            List<AgencyImportStep> toSkip = View.SkipSteps;
            if (add && !toSkip.Contains(step))
                toSkip.Add(step);
            else if (!add && toSkip.Contains(step))
                toSkip.Remove(step);
            View.SkipSteps = toSkip;
        }

        #region "Import"
            public void ImportAgencies(ExternalResource selectedItems)
            {
                View.PreviousStep = AgencyImportStep.None;
                View.SetupProgressInfo(selectedItems.Rows.Count);
          
                // Create profiles
                List<String> notAddedItems = new List<String>();
                Dictionary<long, String> createdItems = CreateAgencies(View.AvailableForAll, View.SelectedOrganizations, selectedItems, notAddedItems);
           
                View.UpdateSourceItems();
                Int32 itemsToSelect = View.ItemsToSelect;
                View.PreviousStep = (itemsToSelect > 0) ? AgencyImportStep.ItemsSelctor : AgencyImportStep.None;

                if (notAddedItems.Count == 0)
                {
                    View.isCompleted = (itemsToSelect == 0);
                    View.DisplayImportedAgencies(createdItems.Count, itemsToSelect);
                }
                else
                    View.DisplayImportError(createdItems.Count, notAddedItems);   
            }

            public List<ExternalCell> DBValidation(DestinationItem<Int32> item, List<ExternalCell> cells)
            {
                return Service.ValidateAgencyDataToImport(item,cells);
            }

            public List<ExternalCell> DBValidation(List<DestinationItemCells<Int32>> items)
            {
                return Service.ValidateAgencyDataToImport(items);
            }

            private Dictionary<long, String> CreateAgencies(Boolean assignToAll, Dictionary<Int32, String> organizations, ExternalResource selectedItems, List<String> notAddedItems)
            {
                Dictionary<long, String> agencies = new Dictionary<long, String>();
                if (assignToAll == true || organizations.Count > 1)
                {
                    long idAgency = 0;
                    Int32 index = 1;
                    List<ExternalColumnComparer<String,Int32>> columns = View.Fields;
                    foreach (ExternalRow row in selectedItems.Rows)
                    {
                        dtoAgency dto = new  dtoAgency();
                        dto.AlwaysAvailable= assignToAll;
                        dto.Name =  row.GetCellValue(columns.Where(c=> c.DestinationColumn.Id == (int)ImportedAgencyColumn.name).Select(c=> c.Number).FirstOrDefault());
                        dto.NationalCode =  row.GetCellValue(columns.Where(c=> c.DestinationColumn.Id == (int)ImportedAgencyColumn.nationalCode).Select(c=> c.Number).FirstOrDefault());
                        dto.TaxCode =  row.GetCellValue(columns.Where(c=> c.DestinationColumn.Id == (int)ImportedAgencyColumn.taxCode).Select(c=> c.Number).FirstOrDefault());
                        dto.ExternalCode =  row.GetCellValue(columns.Where(c=> c.DestinationColumn.Id == (int)ImportedAgencyColumn.externalCode).Select(c=> c.Number).FirstOrDefault());
                        dto.IsDefault = false;
                        dto.IsEditable = true;
                        if (!assignToAll)
                            dto.Organizations = (from o in organizations select new dtoOrganizationAffiliation() { Id =o.Key}).ToList();
                        Agency agency = Service.SaveAgency(dto);

                        if (agency==null)
                            notAddedItems.Add(dto.Name);
                        else
                            agencies.Add(agency.Id,agency.Name);
                        View.UpdateAgencyCreation(selectedItems.Rows.Count, index, (agency!=null), " // ");
                        index++;
                    }

                }
                return agencies;
            }
    //     Public Function DBValidation(item As DestinationItem(Of Integer), cells As List(Of ExternalCell)) As List(Of ExternalCell)
    //    Dim errors As New List(Of ExternalCell)
    //    Dim idQuestionnnaire As Integer = View.CurrentIdQuestionnnaire
    //    If item.Id = ImportedUserColumn.mail Then
    //        For Each cell As ExternalCell In cells
    //            If (Service.ExistExternalInvitedUser(idQuestionnnaire, cell.Value)) Then
    //                errors.Add(cell)
    //            End If
    //        Next
    //    End If

    //    Return errors
    //End Function
        #endregion
    }
}