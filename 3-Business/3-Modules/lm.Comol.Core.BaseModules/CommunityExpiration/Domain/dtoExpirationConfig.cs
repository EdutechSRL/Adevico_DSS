using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Domain
{
    /// <summary>
    /// Dto utilizzato per lettura/modifica
    /// </summary>
    public class dtoExpirationConfig
    {
        /// <summary>
        /// Id
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Id Ruolo
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Id Ruolo
        /// </summary>
        public String RoleName { get; set; }
        /// <summary>
        /// Valore durata
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// Nome durata
        /// </summary>
        public string DurationName { get; set; }

        public dtoExpirationConfig()
        { }

        public dtoExpirationConfig(ExpirationConfig config)
        {
            Id = config.Id;
            RoleId = config.RoleId; 
            Duration = config.Duration;
        }
    }
}
