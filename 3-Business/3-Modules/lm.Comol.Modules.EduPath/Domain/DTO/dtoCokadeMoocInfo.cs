using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain.DTO
{
    public class dtoCokadeMoocInfo
    {
        //Se comlpetato o meno
        public bool mookCompleted { get; set; }

        //% completamento
        public Int64 Completion { get; set; }

        //% minima completamento
        public Int64 MinCompletion { get; set; }

        //Tipo di Mooc
        public MoocType mType { get; set; }
    }
}
