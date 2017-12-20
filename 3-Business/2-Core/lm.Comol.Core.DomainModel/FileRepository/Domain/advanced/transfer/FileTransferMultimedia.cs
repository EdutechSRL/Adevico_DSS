using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public class FileTransferMultimedia : FileTransferBase
    {
        public virtual String DefaultDocumentPath { get; set; }

        public virtual IList<MultimediaFileObject> Objects { get; set; }

        public virtual MultimediaFileObject DefaultDocument { get; set; }

        protected internal virtual void InitClass()
		{
            Status = TransferStatus.ReadyForTransfer;
			Info = "";
            Decompress = true;
			ModifiedOn = DateTime.Now;
            Objects = new List<MultimediaFileObject>();
		}

        public FileTransferMultimedia()
		{
            InitClass();
		}

        public FileTransferMultimedia(FileTransferBase transfer)
            :  base(transfer)
        {
            InitClass();
        }
    }
}