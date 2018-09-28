using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    /// <summary>
    ///     Classe contenente informazioni di Export
    /// </summary>
    public class GlossaryCounterInfo
    {
        /// <summary>
        ///     Numero glossari istanza
        /// </summary>
        public Int32 GlossaryCount { get; set; }

        /// <summary>
        ///     Glossari non exportati, controllo colonna aggiunta ExportedStatus
        /// </summary>
        public Int32 GlossaryNotExportedCount { get; set; }
    }
}