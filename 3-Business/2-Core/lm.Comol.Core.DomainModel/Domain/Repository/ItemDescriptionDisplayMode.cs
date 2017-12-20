using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// DA SPOSTARE NEL CORE base modules c# al termine della traduzione del progetto VB
namespace lm.Comol.Core.DomainModel.Repository
{
    [Serializable, Flags ]
    public enum ItemDescriptionDisplayMode
    {
        None = 0,
        Hidden = 1,
        AsTitle = 2,
        AsTooltip = 4,
        Show = 8
    }
}