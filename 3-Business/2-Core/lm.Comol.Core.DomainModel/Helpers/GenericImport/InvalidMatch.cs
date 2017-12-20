using System;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public enum InvalidMatch
    {
        None = 0,
        DuplicatedItem = 1,
        DuplicatedItems = 2,
        IgnoredRequiredItem = 4,
        IgnoredRequiredItems = 8,
        IgnoredAllItems = 16,
        NoRequiredItems =32,
        GenericError = 64,
        IgnoredItem = 128,
        IgnoredAlternativeRequiredItem = 256,
        IgnoredAlternativeRequiredItems = 512
    }
}