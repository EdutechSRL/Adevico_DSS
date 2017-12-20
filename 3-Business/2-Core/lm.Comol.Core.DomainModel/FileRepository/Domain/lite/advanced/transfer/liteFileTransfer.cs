using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public class liteFileTransfer : liteBaseItemIdentifiers
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
        public virtual long TotalActivity { get; set; }
        public virtual String DefaultDocumentPath { get; set; }
        public virtual liteMultimediaFileObject DefaultDocument { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }
        public liteFileTransfer()
        {
            Policy = TransferPolicy.none;
        }
    }
}