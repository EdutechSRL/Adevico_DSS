namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    public enum DisplayMode
    {
        /// <summary>
        ///     Non mostrare i termini
        /// </summary>
        None = -1,

        /// <summary>
        ///     Colonna Multipla
        /// </summary>
        MultiColumn = 1,

        /// <summary>
        ///     Tutte le definizioni
        /// </summary>
        AllDefinition = 2,

        /// <summary>
        ///     Una lettera per pagina
        /// </summary>
        SingleLetter = 10,

        /// <summary>
        ///     Tutte le lettere in una pagina
        /// </summary>
        AllLetter = 20 //ALL letter in a single page
    }
}