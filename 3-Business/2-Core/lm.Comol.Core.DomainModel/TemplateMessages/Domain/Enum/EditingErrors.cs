using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public enum EditingErrors
    {
        None = 0,
        NoModules = 1,
        NoNotificationType = 2,
        NoPermission = 4,
        EmptyMessage = 8,
        EmptyTranslations = 16,
        InvalidTranslations = 32
    }
}