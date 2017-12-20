using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class PlayerSettings : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual ItemTypeSettings Settings { get; set; }
        
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean EnableForPlay { get; set; }
        public virtual Boolean EnableForUse { get; set; }
        public virtual ItemType Type { get; set; }
        public virtual String PlayUrl { get; set; }
        public virtual String PlayerRenderUrl { get; set; }
        public virtual String ModalPlayerRenderUrl { get; set; }
        
        public virtual Boolean OverrideSSLsettings { get; set; }
        public virtual Boolean AutoEvaluate { get; set; }
        public virtual Boolean RedirectToFilePage { get; set; }
        public virtual String MappingPath { get; set; }
        public virtual String DBidentifier { get; set; }
        public virtual String NoSaveStatParameter { get; set; }
        public PlayerSettings()
        {

        }
    }
}