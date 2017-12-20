using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public class FileTransferScorm : FileTransferBase
    {
        public virtual long TotalActivity { get; set; }
        protected internal virtual void InitClass()
        {
            Status = TransferStatus.ReadyForTransfer;
			Info = "";
            Decompress = true;
            ModifiedOn = DateTime.Now;
            TotalActivity = 0;
        }
        public FileTransferScorm()
		{
            InitClass();
		}
        public FileTransferScorm(FileTransferBase transfer)
            : base(transfer)
        {
            InitClass();
        }
    }
}