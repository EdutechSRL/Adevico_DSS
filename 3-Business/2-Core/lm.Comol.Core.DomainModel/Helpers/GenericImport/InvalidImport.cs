using System;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public enum InvalidImport
    {
        None = 0,
        InvalidData = 1,
        SourceDuplicatedData = 2,
        AlreadyExist = 4
    }
}