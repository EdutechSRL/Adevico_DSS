using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public enum TermAttachmentType
    {
        /// <summary>
        ///     Allegato di tipo file
        /// </summary>
        File = 0,

        /// <summary>
        ///     Allegato di tipo url
        /// </summary>
        Url = 1
    }
}