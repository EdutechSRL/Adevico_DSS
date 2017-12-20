using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.FileRepository.Domain
{
    [CLSCompliant(true)]
    public class RepositoryItemLink <T>: RepositoryItemLinkBase<T>
    {
        #region "Owner/creator"
        public virtual Int32 IdOwner { get; set; }
        public virtual String Owner { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual String CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual String ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        #endregion


        public virtual liteModuleLink Link { get; set; }
        public virtual long IdStatus { get; set; }
        public virtual Boolean isModified { get; set; }
    }
}
