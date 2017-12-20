using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoDownloadError
    {
        public virtual long IdItem {get;set;}
        public virtual Guid IdGuidItem { get; set; }
        public virtual long IdVersion {get;set;}
        public virtual long IdLink { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual Int32 IdItemCommunity { get; set; }
        public virtual Boolean IsRepositoryItem {get { return IdGuidItem== Guid.Empty;}}
        public virtual Int32 IdCurrentCommunity {get;set;}
        public virtual Int32 IdCurrentUser {get;set;}
        public virtual String Name  {get;set;}
        public virtual String Extension {get;set;}
        public virtual String FullName {get { return Name+Extension;}}
        public virtual DownloadErrorType Type {get;set;}
        public virtual Guid IdNews { get; set; }
        public virtual Guid WorkingSession { get; set; }
        public virtual Boolean InModalWindow { get; set; }

    }
}