using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewNoticeboardTile : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        TileLayout CurrentLayout {get;set;}
        void DisplaySessionTimeout();
        void InitalizeControl(TileLayout layout, Int32 idCommunity);
        void LoadNoMessage();
        void LoadNoPermissionsToSeeMessage();

        void LoadMessage(lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem message, litePerson p);
        void LoadMessage(lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem message,Int32 idCommunity, liteSubscriptionInfo subscription);
    }
}