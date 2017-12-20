using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public class FileTransferVideoStreaming : FileTransferBase
    {
       protected internal virtual void InitClass()
        {
            Status = TransferStatus.ReadyForTransfer;
			Info = "";
            Decompress = false;
            ModifiedOn = DateTime.Now;
        }
        public FileTransferVideoStreaming()
		{
            InitClass();
		}
        public FileTransferVideoStreaming(FileTransferBase transfer)
            : base(transfer)
        {
            InitClass();
        }
    }
}