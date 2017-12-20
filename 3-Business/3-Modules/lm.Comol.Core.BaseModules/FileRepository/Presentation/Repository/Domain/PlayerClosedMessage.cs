using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
{
    [Serializable]
    public enum PlayerClosedMessage
    {
        Successful = 0,
        EvaluationSaved = 1,
        EvaluationNotSaved = 2,
        SessionExpired = 3,
        ClosedForError = 4,
        Close = 5
    }
}