using System;
namespace lm.Comol.Core.DomainModel
{
    [Serializable()]
    public class MetaData: DomainObject<Guid>, iMetaData
    {

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }

        public iPerson CreatedBy { get; set; }
        public iPerson ModifiedBy { get; set; }
        public iPerson DeletedBy { get; set; }
        public iPerson ApprovedBy { get; set; }
        public bool isDeleted { get; set; }
        public bool canModify { get; set; }
        public bool canDelete { get; set; }

        public MetaApprovationStatus Approvation { get; set; }

        public bool isApproved { 
            get{
                return (Approvation == MetaApprovationStatus.Approved || Approvation== MetaApprovationStatus.Ignore);
            }
         }

        public MetaData(){
            Approvation= MetaApprovationStatus.Ignore;
            canDelete=true;
            canModify=true;
        }
    }
}