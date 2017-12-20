using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class liteBaseRepositoryItem : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {

        public virtual Int32 IdOwner { get; set; }
        public virtual long Size {get;set;}
        public virtual String Description { get; set; }
        public virtual long Downloaded {get;set;}
        public virtual ItemType Type {get;set;}
        public virtual DisplayMode DisplayMode {get;set;}
        public virtual String Thumbnail {get;set;}
        public virtual Boolean AutoThumbnail { get; set; }
        public virtual long PreviewTime {get;set;}
        public virtual long Time { get; set; }
        public virtual long Number {get;set;}
        public virtual ItemAvailability Availability { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }
    }
}