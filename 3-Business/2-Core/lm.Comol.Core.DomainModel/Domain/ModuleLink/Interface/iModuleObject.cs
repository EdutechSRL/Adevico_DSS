
using System;
namespace lm.Comol.Core.DomainModel
{
    [CLSCompliant(true)]
    public interface iModuleObject
    {
        string FQN { get; set; }
        int ObjectTypeID { get; set; }
        long ObjectLongID { get; set; }
        long ObjectIdVersion { get; set; }
        System.Guid ObjectGuidID { get; set; }
        object ObjectOwner { get; set; }
        int CommunityID { get; set; }
        int ServiceID { get; set; }
        string ServiceCode { get; set; }
    }
}