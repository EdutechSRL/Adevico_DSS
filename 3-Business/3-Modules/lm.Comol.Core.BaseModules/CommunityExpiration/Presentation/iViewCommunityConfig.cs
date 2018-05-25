using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity = Comol.Entity;

namespace lm.Comol.Core.BaseModules.CommunityExpiration.Presentation
{
    public interface iViewCommunityConfig : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// Impostazione configurazione corrente
        /// </summary>
        IList<Domain.dtoExpirationConfig> Configs { get; set; }

        /// <summary>
        /// Id comunità selezionato (di default comunità corrente, in futuro by url o altro)
        /// </summary>
        int CurrentCommunityId { get; }
        /// <summary>
        /// Nome comunità (soprattutto se diversa da quella corrente)
        /// </summary>
        string CurrentCommunityName { set; }

        /// <summary>
        /// Configurazione sistema (tempi)
        /// </summary>
        Entity.DelaySubscriptionConfig SysConfig { get; }

        void ShowError();
        void ShowNoPermission();
        void ShowNoCommunity();
        void ShowSuccessSave();
    }
}
