using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class dtoScormItemEvaluationSettings
    {
        public virtual long Id { get; set; }
        public virtual Boolean CheckScormCompletion { get; set; }
        public virtual Boolean CheckTime { get; set; }
        public virtual long MinTime { get; set; }
        public virtual Boolean CheckScore { get; set; }
        public virtual Boolean UseScoreScaled { get; set; }
        public virtual Decimal MinScore { get; set; }
        public virtual ScormSettingsError Error { get; set; }
        public virtual Boolean ForPackage { get; set; }

        public dtoScormItemEvaluationSettings()
        {
            MinScore = 0;
            MinTime=0;
            Error = ScormSettingsError.none;
            CheckScore = false;
            CheckTime = false;
            UseScoreScaled = false;
        }
    }

    [Serializable()]
    public enum ScormSettingsError
    {
        none = 0,
        time = 1,
        score = 2,
        timescore = 3
    }
}