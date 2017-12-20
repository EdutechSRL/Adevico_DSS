using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewNoticeboardPage : IViewNoticeboardBasePage
    {
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
    }
}