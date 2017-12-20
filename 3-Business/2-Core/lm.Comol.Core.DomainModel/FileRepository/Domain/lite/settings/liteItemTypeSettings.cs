using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class liteItemTypeSettings : DomainBaseObject<long>
    {
        public virtual long IdSettings { get; set; }
        public virtual ItemType Type { get; set; }
        public virtual Boolean AllowDownload { get; set; }
        public virtual Boolean DefaultAllowDownload { get; set; }
        public virtual IList<litePlayerSettings> Players { get; set; }
        public virtual DisplayMode DefaultDisplayMode { get; set; }

        public liteItemTypeSettings()
        {
            Players = new List<litePlayerSettings>();
            DefaultDisplayMode = DisplayMode.downloadOrPlay;
        }
    }
}