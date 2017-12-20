using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewDisplayMessage : IViewNoticeboardBasePage
    {
        System.Guid PreloadWorkingApplicationId { get; }
        String ContainerName{set;}
        void PreloadCssFiles(String cssFilesPath,List<lm.Comol.Core.BaseModules.Editor.EditorCssFile> files);
        void DisplayMessage(NoticeboardMessage message);
        void DisplayNoPermission();
        void DisplayUnknownMessage();
        void DisplaySessionTimeout();
        void DisplayEmptyMessage();
    }
}