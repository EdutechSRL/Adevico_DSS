using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public class FileTransferBase : BaseItemIdentifiers
    {
        public virtual System.Guid CloneOf { get; set; }
        public virtual System.Guid CloneOfVersion { get; set; }
        public virtual String Info { get; set; }
        public virtual String Path { get; set; }
        public virtual String Filename { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public virtual Boolean Decompress { get; set; }
        public virtual Boolean isCompleted { get; set; }

        public virtual FileTransferType Discriminator { get; set; }
        public virtual TransferStatus Status { get; set; }
        public virtual TransferPolicy Policy { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }

        public FileTransferBase() {
            Policy = TransferPolicy.none;
            Repository = new RepositoryIdentifier();
        }

        public FileTransferBase(FileTransferBase item)
        {
            CloneOf = item.CloneOf;
            IdItem = item.Id;
            UniqueIdItem = item.UniqueIdItem;
            IdVersion = item.IdVersion;
            UniqueIdVersion = item.UniqueIdVersion;
            Info = item.Info;
            Path = item.Path;
            Filename = item.Filename;
            ModifiedOn = item.ModifiedOn;
            Decompress = item.Decompress;
            isCompleted = item.isCompleted;
            Policy = item.Policy;
            Repository = item.Repository;
        }
    }
}