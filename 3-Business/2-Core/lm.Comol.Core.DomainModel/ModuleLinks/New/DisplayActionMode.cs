using System;
namespace lm.Comol.Core.ModuleLinks
{
    [Serializable, Flags]
    public enum DisplayActionMode
    {
        none = 0,
        text = 1,
        actions = 2,
        defaultAction = 4,
        adminMode = 8,
        textDefault = 16,
        menuActions = 32,
        extraInfo = 64
    }
}