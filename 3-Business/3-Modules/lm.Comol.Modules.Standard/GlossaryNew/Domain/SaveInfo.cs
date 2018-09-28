using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    /// <summary>
    ///     Classe informazioni salvataggio
    /// </summary>
    public class SaveInfo : GlossaryCounterInfo
    {
        /// <summary>
        ///     Esito salvataggio
        /// </summary>
        public SaveStateEnum SaveState { get; set; }

        /// <summary>
        ///     Id Elemento dopo salvataggio
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        ///     Elementi con lo stesso nome, usato nel salvataggio di termini
        /// </summary>
        public Int32 ElementsWithSameName { get; set; }

        /// <summary>
        ///     Elementi scritti, usato nel export
        /// </summary>
        public Int32 WrittenElements { get; set; }

        /// <summary>
        ///     Eccezione
        /// </summary>
        public Exception Exception { get; set; }
    }

    [Serializable]
    public class CharInfo
    {
        public CharInfo(string currentChar, bool hasWordInPreviousPage, bool hasWordInNextPage)
        {
            CurrentChar = currentChar;
            FirstLetter = CurrentChar[0];
            HasWordInPreviousPage = hasWordInPreviousPage;
            HasWordInNextPage = hasWordInNextPage;
        }

        public CharInfo()
        {
        }

        public Boolean HasWordInPreviousPage { get; set; }
        public Boolean HasWordInNextPage { get; set; }
        public String CurrentChar { get; set; }
        public char FirstLetter { get; set; }

        public String CssClass { get; set; }
    }
}