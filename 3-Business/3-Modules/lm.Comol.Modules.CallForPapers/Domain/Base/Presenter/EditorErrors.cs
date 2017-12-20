using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum EditorErrors
    {
        None = 0,
        NoSections = 1,
        Saving = 2,
        AddingSection = 4,
        RemovingSection = 8,
        AddingField = 16,
        RemovingField = 24,
        AddingOption = 32,
        RemovingOption = 64,
        SubmissionLinked = 128,
        AddingTemplate = 256,
        RemovingTemplate = 512,
        EditingTemplate = 1024,
        SavingTemplateSettings = 2048,
        AddingRequestedFile = 4096,
        EditingRequestedFiles = 8192,
        RemovingRequestedFiles = 16384,
        CloningSection = 32768,
        CloningField = 65536,
        SetAsDefaultOption = 137072,
        EditingDisclaimerType = 262144
    }
}
