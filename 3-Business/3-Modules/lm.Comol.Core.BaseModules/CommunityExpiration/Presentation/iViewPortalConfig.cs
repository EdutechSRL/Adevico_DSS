using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity = Comol.Entity;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Presentation
{
    /// <summary>
    /// View
    /// </summary>
    public interface iViewPortalConfig : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// Impostazione configurazione corrente
        /// </summary>
        IList<Domain.dtoExpirationConfig> Configs { get; set; }
        /// <summary>
        /// Tipo comunità selezionato
        /// </summary>
        int CurrentCommunityTypeId { get; set; }
        /// <summary>
        /// Configurazione sistema (tempi)
        /// </summary>
        Entity.DelaySubscriptionConfig SysConfig { get; }

        IList<KeyValuePair<int, string>> CommunityTypes { set; }

        void ShowError();
    }
}
