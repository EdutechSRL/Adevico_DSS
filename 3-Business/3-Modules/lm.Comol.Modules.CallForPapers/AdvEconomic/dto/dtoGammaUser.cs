using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// Utente gamma - WIP
    /// </summary>
    public class dtoGammaUser
    {
        /// <summary>
        /// Id persona
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Codice gamma
        /// </summary>
        public string GammaCode { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cognome
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Mansione
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// Settore
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        ///  Codice fiscale: codice fiscale persona fisica tipo "Utente Aziendale"
        /// </summary>
        public string CodiceFiscale { get; set; }

        /// <summary>
        /// Nome azienda
        /// </summary>
        public string AziendaNome { get; set; }
        /// <summary>
        /// C.F/P.Iva azienda
        /// </summary>
        public string AziendaCfPiva { get; set; }
        /// <summary>
        /// Codice REA azienda
        /// </summary>
        public string AziendaREA { get; set; }
        /// <summary>
        /// Indirizzo Azienda
        /// </summary>
        public string AziendaIndirizzo { get; set; }
        /// <summary>
        /// Città azienda
        /// </summary>
        public string AziendaCitta { get; set; }
        /// <summary>
        /// Provincia/Regione azienda
        /// </summary>
        public string AziendaProvReg { get; set; }
        /// <summary>
        /// Associazione categorie azienda
        /// </summary>
        public string AziendaCategorie { get; set; }



    }
}
