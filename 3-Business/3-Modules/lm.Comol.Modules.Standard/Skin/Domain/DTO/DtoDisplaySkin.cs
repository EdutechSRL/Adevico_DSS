using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    [Serializable]
    public class DtoDisplaySkin
    {
        public virtual System.Guid UniqueId { get; set; }
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual SkinDisplayType Type { get; set; }
        public virtual Boolean IsValid { get; set; }
        public DtoDisplaySkin() {
            UniqueId = Guid.NewGuid();
        }
    }
}