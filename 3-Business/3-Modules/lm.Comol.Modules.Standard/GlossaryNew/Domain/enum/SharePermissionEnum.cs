using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Flags]
    public enum SharePermissionEnum
    {
        /// <summary>
        ///     Non impostata
        /// </summary>
        None = 0,

        /// <summary>
        ///     Aggiunta Termine
        /// </summary>
        AddTerm = 1,

        /// <summary>
        ///     Rimozione Termine
        /// </summary>
        DeleteTerm = 2,

        /// <summary>
        ///     Modifica Termine
        /// </summary>
        EditTerm = 4
    }
}