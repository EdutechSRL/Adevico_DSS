using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoRequestItemPermission : dtoItemPermission 
    {
        //public virtual Boolean HasUserSubmission { get { return (SubmissionCount > 0); } }
        public virtual long DraftItems { get; set; }
        public virtual dtoBaseCallPermission Permission { get; set; }
  
        public dtoRequestItemPermission()
            : base()
        {
            Permission = new dtoBaseCallPermission();
        }
        public dtoRequestItemPermission(long id, liteCommunity community, CallStatusForSubmitters status, dtoRequest request)
            : base(id, community, status)
        {
            Call = request;
            Deleted = request.Deleted;
            Permission = new dtoBaseCallPermission();
        }

        public dtoRequestItemPermission(long id, liteCommunity community, CallStatusForSubmitters status, dtoSubmissionDisplayInfo subInfo, dtoRequest request)
            : this(id, community, status, request)
        {
            SubmissionsInfo.Add(subInfo);
        }
        public void RefreshPermission(ModuleRequestForMembership module, litePerson person,long submissionCount, long waiting){
            SubmissionCount = submissionCount;
            DraftItems = waiting;
            Permission.Delete = (Call.Status == CallForPaperStatus.Draft) && !HasUserSubmission && (Deleted != BaseStatusDeleted.None) && (module.ManageBaseForPapers || module.Administration || (module.DeleteOwnBaseForPaper && Call.Owner.Id == person.Id));
            Permission.VirtualDelete = !HasUserSubmission && (Deleted == BaseStatusDeleted.None) && (module.ManageBaseForPapers || module.Administration || (module.DeleteOwnBaseForPaper && Call.Owner.Id == person.Id));
            Permission.UnDelete = (Deleted != BaseStatusDeleted.None) && (module.ManageBaseForPapers || module.Administration || (module.EditBaseForPaper && Call.Owner.Id == person.Id));
            Permission.Edit = (Deleted == BaseStatusDeleted.None) && (module.ManageBaseForPapers || module.Administration || (module.EditBaseForPaper && Call.Owner.Id == person.Id));
            Permission.ViewSubmissions = (Deleted == BaseStatusDeleted.None) && (submissionCount > 0) && (module.ManageBaseForPapers || module.Administration || (module.EditBaseForPaper && Call.Owner.Id == person.Id));
        }
        public void RefreshUserPermission(ModuleRequestForMembership module, litePerson person)
        {
            Permission.Submit = module.ViewBaseForPapers;
        }
        public void RefreshUserPermission(Dictionary<int, ModuleRequestForMembership> permissions, litePerson person)
        {
            ModuleRequestForMembership module = permissions[(this.Community==null) ? 0 : this.Community.Id];
            if (module == null)
                module = new ModuleRequestForMembership();
            Permission.Submit = module.ViewBaseForPapers;
        }
    }
}