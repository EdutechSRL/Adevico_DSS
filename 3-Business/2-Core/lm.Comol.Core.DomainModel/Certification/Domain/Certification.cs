using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Certifications
{
    [Serializable]
    public class Certification : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Guid UniqueId { get; set; }
        public virtual Community Community  { get; set; }
        public virtual Person Owner  { get; set; }
        public virtual long SourceIdContainer { get; set; }
        public virtual ModuleObject SourceItem { get; set; }
        public virtual CertificationType Type { get; set; }
        public virtual CertificationStatus Status { get; set; }
        public virtual DateTime IssuedOn  { get; set; }
        public virtual Guid FileUniqueId { get; set; }
        public virtual String FileExtension { get; set; }
        public virtual long IdTemplate { get; set; }
        public virtual long IdTemplateVersion { get; set; }
        public virtual Boolean WithEmptyPlaceHolders { get; set; }
        
        //public virtual ModuleLink ModuleLink { get; set; }
        public Certification() {
            Type = CertificationType.Standard;
            Status = CertificationStatus.Valid;
        }
    }

    [Serializable]
    public enum CertificationType
    {
        Standard = 0,
        UserRequired = 1,
        AutoProduced = 2,
        ManagerProduced = 3,
        RuntimeProduced = 4
    }
    [Serializable]
    public enum CertificationStatus
    {
        None = 0,
        Valid = 1,
        Invalid = 2,
        Ignore = 3,
        OverWritten = 4
    }

    [Serializable]
    public enum CertificationError
    {
        None = 0,
        Unknown = 1,
        SavingFile = 2,
        TransmittingFile = 4,
        ParsingTemplate = 8,
        EmptyTemplateItem = 16,
        EmptyTemplateItems = 32,
        RepositoryError = 64,
        ExternalItemUnknown = 128
    }
}