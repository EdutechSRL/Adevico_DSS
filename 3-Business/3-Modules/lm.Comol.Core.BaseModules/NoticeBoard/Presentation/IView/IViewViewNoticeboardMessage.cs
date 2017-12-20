using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewViewNoticeboardMessage : IViewNoticeboardBasePage
    {
        Int32 IdLoaderCommunity { get; set; }
        Boolean AllowPrint { set;}
        void SetHeaderTitle(Boolean isForPortal, String name);
        void DisplayMessage(long idMessage, Int32 idCommunity);
        void DisplaySessionTimeout();
    }
}