using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewCreateThumbnail : IViewNoticeboardBasePage
    {
        Guid PreloadWorkingSessionId { get; }

        void GenerateThumbnail(long idMessage, Int32 idCommunity);
    }
}