using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Repository
{
    public class ScormFileTransfer : FileTransfer
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

        public ScormFileTransfer()
		{
            InitClass();
		}

        public ScormFileTransfer(FileTransfer transfer)
            : base(transfer)
        {
            InitClass();
        }
    }
}