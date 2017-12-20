using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewNoticeboardEditing : IViewNoticeboardPage
    {
        Int32 IdCurrentCommunity {get;set;}
        Boolean IsPortalPage {get;set;}
        Boolean PreloadBackUrl { get; }
        Boolean PreloadFromPortal { get; }      
        void SetHeaderTitle(Boolean isForPortal, String name);
        void SetBackUrl(String url);
        void RedirectToDasboardUrl(String url);
        void DisplaySessionTimeout(String url, Int32 idCommunity);
        void DisplayMessage(Boolean done, NoticeBoard.Domain.ModuleNoticeboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, NoticeboardMessage message, String notificationUrl, ModuleNoticeboard.ActionType action);
    }
}