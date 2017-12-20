using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Repository
{
    public class MultimediaFileTransfer : FileTransfer
    {
        public virtual String DefaultDocumentPath { get; set; }

        public virtual IList<MultimediaFileIndex> MultimediaIndexes { get; set; }

        public virtual MultimediaFileIndex DefaultDocument { get; set; }

        protected internal virtual void InitClass()
		{
            Status = TransferStatus.ReadyForTransfer;
			Info = "";
            Decompress = true;
			ModifiedOn = DateTime.Now;
            MultimediaIndexes = new List<MultimediaFileIndex>();
            // CHIEDERE CONFERME A ROBERTO SE SERVE STA COSA:::::
		}

        public MultimediaFileTransfer()
		{
            InitClass();
		}

        public MultimediaFileTransfer(FileTransfer transfer)
            :  base(transfer)
        {
            InitClass();
        }
    }
}