using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class dtoScormSettingsEvaluationBase : dtoScormSettingsBase
    {
        public virtual Boolean CheckScormCompletion { get; set; }
        public virtual Boolean CheckTime { get; set; }
        public virtual long MinTime { get; set; }
        public virtual Boolean CheckScore { get; set; }
        public virtual Boolean UseScoreScaled { get; set; }
        public virtual Decimal MinScore { get; set; }
    }
}