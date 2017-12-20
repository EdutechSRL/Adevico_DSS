using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewNoticeboardBasePage : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long PreloadedIdMessage { get; }
        int PreloadedIdCommunity { get; }
        long IdCurrentMessage { get; set; }
        String PortalName { get; }

        #region "Common"
        void SendUserAction(int idCommunity, int idModule, ModuleNoticeboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idMessage, ModuleNoticeboard.ActionType action);
        #endregion            
    }
}