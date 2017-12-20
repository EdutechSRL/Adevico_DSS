using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Repository
{
    public class FileTransfer  : DomainObject<long>
    {

        public virtual BaseCommunityFile File { get; set; }
        public virtual System.Guid FileUniqueID { get; set; }
        public virtual System.Guid CloneId { get; set; }

        //public virtual int CompleteID { get; set; }
        public virtual String Info { get; set; }
        public virtual String Path { get; set; }
        public virtual String Filename { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public virtual Boolean Decompress { get; set; }
        public virtual Boolean isCompleted { get; set; }

        public virtual FileTransferType Discriminator { get; set; }
        public virtual TransferStatus Status { get; set; }
        public virtual TransferPolicy Policy { get; set; }
        public virtual Boolean Transferred { get; set; }
        public FileTransfer() {
            Policy = TransferPolicy.none;
        }

        public FileTransfer(FileTransfer item) {
            File = item.File;
            FileUniqueID = item.FileUniqueID;
            CloneId = item.CloneId;
            Info = item.Info;
            Path = item.Path;
            Filename = item.Filename;
            ModifiedOn = item.ModifiedOn;
            Decompress = item.Decompress;
            isCompleted = item.isCompleted;
            Policy = item.Policy;
        }
    }
}