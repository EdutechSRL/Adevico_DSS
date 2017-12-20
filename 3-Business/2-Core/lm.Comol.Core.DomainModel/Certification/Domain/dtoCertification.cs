using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Certifications
{
    [Serializable]
    public class dtoCertification : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Guid CertificationUniqueId { get; set; }
        public virtual Int32 IdCommunity  { get; set; }
        public virtual Int32 IdOwner { get; set; }
        public virtual long IdContainer { get; set; }
        public virtual ModuleObject SourceItem { get; set; }
        public virtual CertificationType Type { get; set; }
        public virtual CertificationStatus Status { get; set; }
        public virtual DateTime IssuedOn  { get; set; }
        public virtual Guid UniqueIdGeneratedFile { get; set; }
        public virtual long IdTemplate { get; set; }
        public virtual long IdTemplateVersion { get; set; }
        public virtual String FileExtension { get; set; }
        public virtual Boolean WithEmptyPlaceHolders { get; set; }
        public dtoCertification()
        {
            Type = CertificationType.Standard;
            Status = CertificationStatus.Valid;
        }
        public static dtoCertification Create(CertificationType type)
        {
            dtoCertification dto = new dtoCertification();
            dto.Type = type;
            dto.Status = CertificationStatus.Valid;
            dto.IssuedOn = DateTime.Now;
            dto.CertificationUniqueId = Guid.NewGuid();
            return dto;
        }
    }
}