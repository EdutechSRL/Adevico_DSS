using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    [Serializable]
    public class SettingsPermission : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// ID Tipo Persona
        /// </summary>
        public virtual Int32? PersonTypeId { get; set; }

        /// <summary>
        /// Utente (Ticket)
        /// </summary>
        public virtual liteUser User { get; set; }

        /// <summary>
        /// ID Persona (Solo per velocizzare eventuali ricerche)
        /// </summary>
        public virtual Int32? PersonId { get; set; }

        /// <summary>
        /// Tipo di permesso. Vedi relativo Enum
        /// </summary>
        public virtual Enums.PermissionType PermissionType { get; set; }


        /// <summary>
        /// Valuro dei permessi
        /// </summary>
        public virtual Int64? PermissionValue { get; set; }

    }
}
