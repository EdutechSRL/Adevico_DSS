using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Impostazioni categorie legate ad un Tipo Comunità
    /// </summary>
    [Serializable]
    public class SettingsComType : DomainBaseObjectMetaInfo<long>
    {
        /// <summary>
        /// Tipo comunità
        /// </summary>
        public virtual lm.Comol.Core.DomainModel.CommunityType CommunityType { get; set; }
        /// <summary>
        /// Se tale tipo può visualizzare le categorie di tipo "Ticket"
        /// </summary>
        public virtual bool ViewTicket { get; set; }

        /// <summary>
        /// Se può creare categorie "Pubbliche"
        /// </summary>
        public virtual bool CreatePublic { get; set; }
        /// <summary>
        /// Se può creare categorie "Ticket"
        /// </summary>
        public virtual bool CreateTicket { get; set; }
        /// <summary>
        /// Se può creare categorie "Private"
        /// </summary>
        public virtual bool CreatePrivate { get; set; }

        public SettingsComType()
        {
            ViewTicket = false;
            CreatePublic = false;
            CreateTicket = false;
            CreatePrivate = false;
        }
    }
}
