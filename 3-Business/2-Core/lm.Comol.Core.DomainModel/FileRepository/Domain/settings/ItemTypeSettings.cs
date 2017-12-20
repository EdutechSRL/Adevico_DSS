using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class ItemTypeSettings : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual RepositoryContextSettings Settings { get; set; }
        public virtual ItemType Type { get; set; }
        public virtual Boolean AllowDownload { get; set; }
        public virtual Boolean DefaultAllowDownload { get; set; }
        public virtual IList<PlayerSettings> Players { get; set; }
        public virtual DisplayMode DefaultDisplayMode { get; set; }



        public ItemTypeSettings()
        {
            Players = new List<PlayerSettings>();
            DefaultDisplayMode = DisplayMode.downloadOrPlay;
        }
    }
}