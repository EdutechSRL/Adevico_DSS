using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto TAB (wizard) - Graphic, internal.
    /// </summary>
    public class dtoWizStep
    {
        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descrizione
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Tooltip tab
        /// </summary>
        public string ToolTip { get; set; }
        /// <summary>
        /// Nome comando
        /// </summary>
        public string CommandName { get; set; }
        /// <summary>
        /// Argomento comando
        /// </summary>
        public string CommandArgument { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Se è un separatore
        /// </summary>
        public bool IsSeparator { get; set; }
        /// <summary>
        /// Se è il tab corrente
        /// </summary>
        public bool IsCurrent { get; set; }
        /// <summary>
        /// Se è disabilitato
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// Info stato
        /// </summary>
        public StatusColor Status { get; set; }

        /// <summary>
        /// Info stato, per colore tab
        /// </summary>
        public enum StatusColor
        {
            /// <summary>
            /// none = grigio
            /// </summary>
            None,
            /// <summary>
            /// Valido = verde
            /// </summary>
            Valid,
            /// <summary>
            /// Attenzione = giallo
            /// </summary>
            Warning,
            /// <summary>
            /// Errore = rosso
            /// </summary>
            Error
        }
    }
}
