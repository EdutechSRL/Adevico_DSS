using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    public class DTO_ListPermission
    {
        /// <summary>
        /// Se posso attivare il template: deve essere DEFINITIVO ed avare ATTIVO = false.
        /// </summary>
        public virtual Boolean Activate { get; set; }
        /// <summary>
        /// Se posso disattivare il template: deve essere DEFINITIVO ed avare ATTIVO = false.
        /// </summary>
        public virtual Boolean DeActivate { get; set; }

        /// <summary>
        /// Se posso disattivare editare il template: PERMESSI e:
        /// In Bozza: modifico la bozza
        /// No Bozza: crea una bozza e la edito
        /// </summary>
        public virtual Boolean Edit { get; set; }

        /// <summary>
        /// Permessi di cancellazione.
        /// SOLO NO DEFINITIVI, IsSystem = FALSE
        /// </summary>
        public virtual Boolean DeleteVirtual { get; set; }
        public virtual Boolean UndeleteVirtual { get; set; }
        public virtual Boolean DeletePhisical { get; set; }
        

        /// <summary>
        /// Se posso creare una nuova versione: non devo avere BOZZE attive
        /// </summary>
        public virtual Boolean AllowNewVersion { get; set; }


    }
}
