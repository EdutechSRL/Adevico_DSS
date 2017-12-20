using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// ID comunità selezionata per il TICKET o nella querystring se non inizializzata (creazione, primo step)
        /// ToDo: VERIFICARE!
        /// </summary>
        Int32 ViewCommunityId { get; set; }

        //To iView Base Internal
        void DisplaySessionTimeout(Int32 CommunityId);

       
        void SendUserActions(
            int ModuleId,
            ModuleTicket.ActionType Action,
            Int32 idCommunity,
            ModuleTicket.InteractionType Type,
            IList<KeyValuePair<Int32, String>> Objects = null);

        void ShowNoAccess();

        //void SendNotifications(IList<lm.Comol.Core.Notification.Domain.NotificationAction> Actions, Int32 CurrentPersonId);

        void SendNotification(lm.Comol.Core.Notification.Domain.NotificationAction Action, Int32 CurrentPersonId);
    }
}
