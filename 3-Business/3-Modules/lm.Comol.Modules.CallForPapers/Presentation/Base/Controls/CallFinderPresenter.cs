using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class CallFinderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {

        #region "Initializer"
            private int _ModuleID;
            private ServiceRequestForMembership _Service;

            private int ModuleID
            {
                get
                {
                    if (_ModuleID <= 0)
                    {
                        _ModuleID = this.Service.ServiceModuleID();
                    }
                    return _ModuleID;
                }
            }
            public virtual ManagerRequestForMemebership CurrentManager { get; set; }

            protected virtual IViewCallFinder View
            {
                get { return (IViewCallFinder)base.View; }
            }
            private ServiceRequestForMembership Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceRequestForMembership(AppContext);
                    return _Service;
                }
            }
            public CallFinderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new ManagerRequestForMemebership(oContext);
            }
            public CallFinderPresenter(iApplicationContext oContext, IViewCallFinder view)
                : base(oContext, view)
            {
                this.CurrentManager = new ManagerRequestForMemebership(oContext);
            }
        #endregion

        public void InitView(CallForPaperType type, SearchRange range, Int32 idCommunity = -1)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                LoadAvailableCall(type, range, idCommunity);
            View.isInitialized = true; 
        }

        public void Preview()
        {
            if (View.CurrentIdItem >0)
                View.PreviewRows(Service.CreateExternalResource(View.CurrentIdItem,View.SelectedStatus, View.FromDate, 8, View.CurrentSubmitterId ));
            else
                View.PreviewRows(null);
        }

        public void LoadAvailableCall(CallForPaperType type, SearchRange range, Int32 idCommunity = -1)
        {
            View.CurrentIdItem = 0;
            View.AllowPreview = false;
            List<dtoCallToFind> items = Service.GetCallsForProfileInsert(type, range, idCommunity);
            View.AllowViewList = (items.Count > 1);
            if (items.Count == 1)
                SelectCall(items[0].Id); 
            else
                View.LoadCalls(items);
        }
        public void SelectCall(long idCall)
        {
            View.CurrentIdItem = idCall;
            List<SubmissionStatus> statusList = Service.GetAvailableSubmissionStatus(idCall, View.OnlyAnonymousSubmissions, View.AlsoAnonymousSubmissions);
            View.LoadAvailableStatus(statusList);

            if (statusList.Count>0)
                View.SelectedStatus = (statusList.Contains(SubmissionStatus.submitted) ? SubmissionStatus.submitted : statusList[0]); 
                
            // RECUPERO LE INFO PRINCIPALI DELLA RICHIESTA SELEZIONATA
            dtoCallToFind dto = Service.GetCallDtoForProfile(idCall);
            View.LoadCallInfo(dto);
            Dictionary<long, string> submitters = LoadSubmitters(idCall);
            View.LoadSubmitterType(submitters);
            View.CurrentSubmitterId = (submitters.Keys.Count > 0) ? submitters.Keys.FirstOrDefault() : 0;
            View.AllowPreview = true;
            if (View.FromDate == null)
                View.FromDate = dto.StartDate;
            if ((statusList.Count >= 0) && (submitters.Count >= 0) && (dto != null)) 
                View.isValid = true; 
        }

        public List<ProfileColumnComparer<String>> GetAvailableColumns()
        {
            return Service.GetAvailableProfileColumns(View.CurrentIdItem, View.CurrentSubmitterId);
        }
        public List<ExternalColumn> GetStandardAvailableColumns()
        {
            return Service.GetAvailableExternalColumns(View.CurrentIdItem, View.CurrentSubmitterId);
        }
        //public List<ProfileColumnComparer<String>> GetAvailableColumns(dtoCsvSettings settings, dtoCSVfile file)
        //{
        //    List<ProfileColumnComparer<String>> columns = new List<ProfileColumnComparer<String>>();
        //    if (file != null)
        //    {
        //        CsvFile header = ContentOf.LoadCsvFile(file.RealName, settings, 1);
        //        if (header != null)
        //            columns = (from c in header.ColumHeader
        //                       select new ProfileColumnComparer<String>
        //                       {
        //                           SourceColumn = (c.Empty) ? c.Number.ToString() : c.Value,
        //                           Number = c.Number,
        //                           DestinationColumn = Authentication.ProfileAttributeType.skip
        //                       }).ToList();
        //    }
        //    return columns;
        //}

        public void ChangeCall(CallForPaperType type, SearchRange range, Int32 idCommunity = -1)
        {
            LoadAvailableCall(type, range, idCommunity);
        }

        public ProfileExternalResource GetContent()
        {
            return Service.CreateProfileResource(View.CurrentIdItem, View.SelectedStatus, View.FromDate, -1, View.CurrentSubmitterId);
            //return Service.CreateExternalResourceRFM(View.CurrentIdItem, View.SelectedStatus, View.FromDate, -1, View.CurrentSubmitterId );
        }
        public ProfileExternalResource GetItemSubmissions(List<ProfileColumnComparer<String>> columns)
        {
            return Service.CreateProfileResource(columns,View.CurrentIdItem, View.SelectedStatus, View.FromDate, -1, View.CurrentSubmitterId);
            //return Service.CreateExternalResourceRFM(View.CurrentIdItem, View.SelectedStatus, View.FromDate, -1, View.CurrentSubmitterId );
        }
        public Dictionary<long, string> LoadSubmitters()
        {
            return LoadSubmitters(View.CurrentIdItem);
        }
        private Dictionary<long, string> LoadSubmitters(long idCall)
        {
            return Service.GetSubmittersList(idCall);
        }
    }
}