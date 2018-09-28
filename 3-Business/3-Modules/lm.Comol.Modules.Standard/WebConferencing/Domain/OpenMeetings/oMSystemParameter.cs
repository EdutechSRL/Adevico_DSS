using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings
{
    /// <summary>
    /// Parametri di sistema, specifici per OpenMeetings
    /// </summary>
    public class oMSystemParameter : WbSystemParameter
    {
        /// <summary>
        /// URL base (per url accesso stanza
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// Login utente che crea le stanze
        /// </summary>
        public String MainUserLogin { get; set; }
        /// <summary>
        /// Password utente che crea le stanze
        /// </summary>
        public String MainUserPwd { get; set; }
    }
}
