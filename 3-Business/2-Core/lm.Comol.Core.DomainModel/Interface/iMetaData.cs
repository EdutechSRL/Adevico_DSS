using System;
namespace lm.Comol.Core.DomainModel
{
    [CLSCompliant(true)]
    public interface iMetaData : iDomainObject<Guid>
    {

        DateTime? CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
        DateTime? DeletedOn { get; set; }
        DateTime? ApprovedOn { get; set; }
        iPerson CreatedBy { get; set; }
        iPerson ModifiedBy { get; set; }
        iPerson DeletedBy { get; set; }
        iPerson ApprovedBy { get; set; }
        Boolean isDeleted { get; set; }
        Boolean canModify { get; set; }
        Boolean canDelete { get; set; }
        MetaApprovationStatus Approvation { get; set; }
        Boolean isApproved { get;}

       
    }
}