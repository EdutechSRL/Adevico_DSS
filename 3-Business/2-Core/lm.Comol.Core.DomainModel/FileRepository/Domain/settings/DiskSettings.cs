using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class DiskSettings
    {
        public virtual long AvailableSpace { get; set; }
        public virtual long AdditionalSpace { get; set; }
        public virtual long MaxSpace { get; set; }
        public virtual long MaxAdditionalSpace { get; set; }
        public virtual long MaxUploadFileSize { get; set; }
        public virtual OverflowAction RepositoryOverflow { get; set; }
        public virtual OverflowAction UploadOverflow { get; set; }
        public DiskSettings()
        {
            UploadOverflow = OverflowAction.Allow;
            RepositoryOverflow = OverflowAction.AllowWithWarning;
        }
        public virtual List<OverflowAction> AvailableOverflowActions(Boolean forUpload = false)
        {
            return new List<OverflowAction>() { OverflowAction.Allow, OverflowAction.AllowWithWarning, OverflowAction.NotAllow };
        }
        public virtual Boolean IsDiskSpaceValid()
        {
            return (AvailableSpace >= 0 && AdditionalSpace >= 0 && MaxAdditionalSpace >= 0 && MaxSpace >= 0 && MaxUploadFileSize >= 0)
                && (MaxSpace== 0 || (AvailableSpace + AdditionalSpace <= MaxSpace + MaxAdditionalSpace));
        } 
    }
}
