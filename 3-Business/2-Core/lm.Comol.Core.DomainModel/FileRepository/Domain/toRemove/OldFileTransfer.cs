using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.toRemove
{
    [Serializable]
    public class OldFileTransfer
    {
        public virtual long Id { get; set; }
        public virtual long IdFile { get; set; }
        public virtual System.Guid FileUniqueID { get; set; }
        public virtual System.Guid CloneId { get; set; }
        public virtual String Info {get; set;}
        public virtual String Path {get; set;}
        public virtual String Filename {get; set;}
        public virtual DateTime ModifiedOn { get; set; }
        public virtual Boolean Decompress { get; set; }
        public virtual Int32 TransferPolicy {get; set;}
        public virtual Int32 Discriminator {get; set;}
        public virtual Int32 IdTransferStatus {get; set;}
        public virtual long TotalActivity { get; set; }
        public virtual String DefaultDocumentPath { get; set; }
         public virtual long IdDefaultDocument { get; set; }
         public virtual Boolean Transferred { get; set; }
         public virtual Boolean Removed { get; set; }

    }
    [Serializable]
    public class NewFileTransfer
    {
      
          public virtual long Id { get; set; }
        public virtual long IdItem { get; set; }
        public virtual System.Guid UniqueIdItem { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual System.Guid CloneOf { get; set; }
        public virtual System.Guid CloneOfVersion { get; set; }
        
        public virtual String Info {get; set;}
        public virtual String Path {get; set;}
        public virtual String Filename {get; set;}
        public virtual DateTime ModifiedOn { get; set; }
        public virtual Boolean Decompress { get; set; }
        public virtual Int32 Policy {get; set;}
        public virtual Int32 Discriminator {get; set;}
        public virtual Int32 Status { get; set; }
        public virtual long TotalActivity { get; set; }
        public virtual String DefaultDocumentPath { get; set; }
        public virtual long IdDefaultDocument { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }
        public virtual Int32 _Deleted { get; set; }
    }
}