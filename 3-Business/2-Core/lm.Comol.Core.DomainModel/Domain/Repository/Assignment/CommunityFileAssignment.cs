using System;
namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class CommunityFileAssignment : DomainObject<long>
    {
        public virtual long Permission { get; set; }
        public virtual CommunityFile File { get; set; }
        public virtual Person CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Person ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual Boolean Deny { get; set; }
        public virtual Boolean Inherited { get; set; }

        public CommunityFileAssignment()
        {
        }

       
    }
}