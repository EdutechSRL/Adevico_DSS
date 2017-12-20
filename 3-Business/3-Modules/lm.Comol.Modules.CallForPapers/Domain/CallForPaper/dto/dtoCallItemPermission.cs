using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoCallItemPermission : dtoItemPermission 
    {
        public virtual long ToEvaluateCount { get; set; }
        public virtual long EvaluatedCount { get; set; }
        public virtual Boolean HasItemsToEvaluate { get{return (ToEvaluateCount>EvaluatedCount && ToEvaluateCount>0);}}
        public virtual dtoCallPermission Permission { get; set; }
        public virtual bool AdvacedEvaluation { get; set; }


        public dtoCallItemPermission()
            : base()
        {
            Permission = new dtoCallPermission();
        }
        public dtoCallItemPermission(long id, liteCommunity community, CallStatusForSubmitters status, dtoCall callForPaper)
            : base(id, community, status)
        {
            Call = callForPaper;
            Deleted = callForPaper.Deleted;
            Permission = new dtoCallPermission();
            AdvacedEvaluation = callForPaper.AdvacedEvaluation;
        }
        public dtoCallItemPermission(long id, liteCommunity community, CallStatusForSubmitters status, dtoCall callForPaper, ModuleCallForPaper module, litePerson person)
            : this(id, community, status, callForPaper)
        {
            Permission.Delete = false;
            Permission.VirtualDelete = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && callForPaper.Owner.Id == person.Id));
            Permission.UnDelete = (Deleted != BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && callForPaper.Owner.Id == person.Id));
            Permission.Edit = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && callForPaper.Owner.Id == person.Id));
            Permission.ManageComittees = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration);
            Permission.ManageEvaluation = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration);
            Permission.ViewSubmissions = (Deleted == BaseStatusDeleted.None) && (Call.Status != CallForPaperStatus.Draft ) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && callForPaper.Owner.Id == person.Id));
        }

        public dtoCallItemPermission(long id, liteCommunity community, CallStatusForSubmitters status, dtoSubmissionDisplayInfo subInfo, dtoCall call)
            : this(id, community, status, call)
        {
            SubmissionsInfo.Add(subInfo);
        }
        public void RefreshPermission(ModuleCallForPaper module, litePerson person,long submissionCount){
            SubmissionCount = submissionCount;

            Permission.Delete = (Call.Status== CallForPaperStatus.Draft) && !HasUserSubmission && (Deleted != BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && Call.Owner.Id == person.Id));
            Permission.VirtualDelete = !HasUserSubmission && (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && Call.Owner.Id == person.Id));
            Permission.UnDelete = (Deleted != BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && Call.Owner.Id == person.Id));
            Permission.Edit = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && Call.Owner.Id == person.Id));
            Permission.ViewSubmissions = (Deleted == BaseStatusDeleted.None) && (Call.Status != CallForPaperStatus.Draft) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && Call.Owner.Id == person.Id));
            Permission.ManageComittees = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration);
            Permission.ManageEvaluation = (Deleted == BaseStatusDeleted.None) && (Call.Status!= CallForPaperStatus.Draft) && (submissionCount>0) && (module.ManageCallForPapers || module.Administration);
        }
        public void RefreshEvaluations(long toEvaluate, long evaluated)
        {
            ToEvaluateCount = toEvaluate;
            EvaluatedCount = evaluated;
            Permission.Evaluate = (ToEvaluateCount > 0 || EvaluatedCount > 0);
        }
        public void RefreshUserPermission(ModuleCallForPaper module, litePerson person)
        {
            Permission.Submit = module.ViewCallForPapers;
            //Permission.ManageEvaluation = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration);
            Permission.Evaluate = (ToEvaluateCount > 0 || EvaluatedCount > 0);
        }
        public void RefreshUserPermission(Dictionary<int, ModuleCallForPaper> permissions, litePerson person)
        {
            ModuleCallForPaper module = permissions[(this.Community == null) ? 0 : this.Community.Id];
            if (module == null)
                module = new ModuleCallForPaper();
            Permission.Submit = module.ViewCallForPapers;
            //Permission.ManageEvaluation = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration);
            Permission.Evaluate = (ToEvaluateCount > 0 || EvaluatedCount > 0);
        }
        
        //public dtoCallItemPermission(long id, int idCommunity, CallStatusForSubmitters status, dtoCallForPaper callForPaper, ModuleCallForPaper module, litePerson person)
        //    : this(id, idCommunity, status, callForPaper)
        //{
        //    Permission.Delete = false;
        //    Permission.VirtualDelete = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    Permission.UnDelete = (Deleted != BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    Permission.Edit = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    Permission.ManageEvaluation = (Deleted == BaseStatusDeleted.None) && (module.ManageCallForPapers || module.Administration);
        //}
        //public dtoCallItemPermission(dtoCallForPaperPermission dto, ModuleCallForPaper module, litePerson person, long submissionCount)
        //    : base(dto.Id)
        //{
        //    Status = dto.Status;
        //    CallForPaper = dto.CallForPaper;
        //    Deleted = dto.Deleted;
        //    CommunityId = dto.CommunityId;
        //    SubmissionCount = submissionCount;
        //    HasUserSubmission = (submissionCount > 0);
        //    AllowDelete = false;
        //    AllowVirtualDelete = !HasUserSubmission && Deleted == BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.DeleteOwnCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    AllowUnDelete = Deleted != BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //    AllowEdit = Deleted == BaseStatusDeleted.None && (module.ManageCallForPapers || module.Administration || (module.EditCallForPaper && CallForPaper.Owner.Id == person.Id));
        //}
    
    }
}