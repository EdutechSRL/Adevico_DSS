using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewNoticeboardRenderPage : IViewNoticeboardPage
    {
        void InitializeControl(Int32 idCommunity, NoticeBoard.Domain.ModuleNoticeboard permissions, long idMessage = 0);
    }
}