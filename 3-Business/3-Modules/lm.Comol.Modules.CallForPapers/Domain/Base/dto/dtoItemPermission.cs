using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoItemPermission :dtoBase 
    {
        public virtual dtoBaseForPaper Call { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual int IdCommunity { get { return (Community == null) ? 0 : Community.Id; } }
        public virtual long SubmissionCount { get; set; }
        public virtual long AcceptedSubmissionCount { get; set; }
        public virtual Boolean HasUserSubmission { get { return SubmissionCount > 0; } }
        public virtual Boolean HasAcceptedSubmission { get { return AcceptedSubmissionCount > 0; } }
        public virtual CallStatusForSubmitters Status { get; set; }
        public virtual List<dtoSubmissionDisplayInfo> SubmissionsInfo { get; set; }
        public virtual List<dtoBaseSubmitterType> AllowSubmissionAs { get; set; }

        public virtual Boolean isNewItem {
            get {
                DateTime currentDate = DateTime.Now;
                return Call.Status != CallForPaperStatus.Draft && ((!HasUserSubmission && currentDate.AddDays(-10) <= Call.StartDate) || (HasUserSubmission && currentDate.AddDays(-5) <= Call.StartDate));
            } }
        public virtual Boolean isExpiringItem {
            get {
                Int32 days = (Call.EndDate.HasValue) ? (Call.EndDate.Value - DateTime.Now).Days : -1;
                return (Call.Status != CallForPaperStatus.Draft && Call.EndDate.HasValue && days > -1 && days <=7);
            }
        }

        public dtoItemPermission()
            : base()
        {
            SubmissionsInfo = new List<dtoSubmissionDisplayInfo>();
            AllowSubmissionAs = new List<dtoBaseSubmitterType>();
        }
        public dtoItemPermission(long id, liteCommunity community, CallStatusForSubmitters status)
            : base()
        {
            this.Id= id;
            Community = community;
            Status = status;
            SubmissionsInfo = new List<dtoSubmissionDisplayInfo>();
            AllowSubmissionAs = new List<dtoBaseSubmitterType>();
        }
        public dtoItemPermission(long id, liteCommunity community, CallStatusForSubmitters status, dtoSubmissionDisplayInfo subInfo)
            : this(id, community, status)
        {
            SubmissionsInfo.Add(subInfo);
        }
        //public dtoItemPermission(long id, Community community, CallStatusForSubmitters status, List<dtoSubmissionInfo> subInfos)
        //    : this(id, community, status)
        //{
        //    SubmissionsInfo = subInfos;
        //}
        //public dtoItemPermission(long id, Community community, CallStatusForSubmitters status, dtoSubmissionInfo subInfo, dtoRequest request)
        //    : this(id, community, status, subInfo)
        //{
        //    Call = request;
        //}
        //public dtoItemPermission(long id, Community community, CallStatusForSubmitters status, dtoSubmissionInfo subInfo, dtoRequest request)
        //    : this(id, community, status, subInfo)
        //{
        //    Call = request;
        //}
        //public dtoItemPermission(long id, Community community, CallStatusForSubmitters status, dtoSubmissionInfo subInfo, dtoCall call)
        //    : this(id, community, status, subInfo)
        //{
        //    Call = call;
        //}
        //public dtoItemPermission(long id, Community community, CallStatusForSubmitters status, dtoRequest request)
        //    : this(id, community, status, null, request)
        //{
        //}
        //public dtoItemPermission(long id, Community community, CallStatusForSubmitters status,  dtoCall call)
        //    : this(id, community, status, null, call)
        //{
        //}
    }
}