using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// DA SPOSTARE NEL CORE base modules c# al termine della traduzione del progetto VB
namespace lm.Comol.Core.DomainModel.Repository
{
    [Serializable,Flags]
    public enum ItemAvailableCommand
    {
        None = 0,
        Download = 1,
        Play = 2,
        Statistics = 4,
        Settings = 8,
        Info = 16,
        Edit = 32,
        VirtualDelete = 64,
        VirtualUndelete = 128,
        Delete = 256
    }
}
