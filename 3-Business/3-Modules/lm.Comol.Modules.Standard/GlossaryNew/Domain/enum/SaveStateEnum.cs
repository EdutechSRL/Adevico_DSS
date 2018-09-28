using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Flags]
    public enum SaveStateEnum
    {
        /// <summary>
        ///     Nessun errore
        /// </summary>
        None = 0,

        /// <summary>
        ///     Salvato
        /// </summary>
        Saved = 1,

        /// <summary>
        ///     Alert Glossario con stesso nome esistente
        /// </summary>
        GlossaryNameAlreadyExists = 2,

        /// <summary>
        ///     Alert Termine con stesso nome esistente
        /// </summary>
        TermNameAlreadyExists = 4,

        /// <summary>
        ///     Errore Salvataggio
        /// </summary>
        DbError = 8,

        /// <summary>
        ///     Errore durante travaso dati
        /// </summary>
        GenerateError = 16,

        /// <summary>
        ///     Glossario non esistente
        /// </summary>
        GlossaryNotExist = 32,

        /// <summary>
        ///     non salvato
        /// </summary>
        NotSaved = 64,
        NoRecord = 128
    }

    public enum FilterTypeEnum
    {
        Contains,
        StartWith,
        EndWith
    }

    public enum FilterVisibilityTypeEnum
    {
        All,
        Published,
        Unpublished
    }
}