namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    public enum ShareStatusEnum
    {
        /// <summary>
        ///     Non impostata (0)
        /// </summary>
        None = 0,

        /// <summary>
        ///     Condivisione Attiva (1)
        /// </summary>
        Active = 1,

        /// <summary>
        ///     Condivisione in attesa di conferma (2)
        /// </summary>
        Pending = 2,

        /// <summary>
        ///     Condivisione disattiva (3)
        /// </summary>
        Disabled = 3,

        /// <summary>
        ///     Condivisione sempre attiva (4)
        /// </summary>
        ForceActive = 4,

        /// <summary>
        ///     Condivisione rifiutata (5)
        /// </summary>
        Refused = 5
    }
}