
using System;
namespace lm.Comol.Core.DomainModel
{
    [CLSCompliant(true)]
    public interface iCoreItemFileLink<T>
    {
        T ItemFileLinkId { get; set; }
        Person Owner { get; set; }
        Person CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        Person ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }

        BaseStatusDeleted Deleted { get; set; }
        ModuleLink Link { get; set; }
        BaseCommunityFile File { get; set; }
        Boolean isVisible { get; set; }
        long StatusId { get; set; }
       
        Boolean isModified();
    }
}
