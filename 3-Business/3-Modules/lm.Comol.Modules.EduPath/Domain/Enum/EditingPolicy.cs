using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable,Flags]
    public enum EditingPolicy
    {
        None = 0,
        QuizSettings = 1,
        QuizAction = 2,
        QuizAnswers = 4,
        ScormSettings = 8,
        MultimediaSettings = 16,
        FileAction = 32,
        CertificationSettings = 64,
        CertificationAction = 128,
        TextAction = 256,
        UnitNotes = 512,
        UnitDescription = 1024,
        ActivityNotes = 2048,
        ActivityDescription = 4096,
        ActivityRoles = 8192
    }
}