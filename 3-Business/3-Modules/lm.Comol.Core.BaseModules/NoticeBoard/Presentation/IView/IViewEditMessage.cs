using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewEditMessage : IViewNoticeboardEditing
    {
        void AllowSave(Boolean allow);
        void EditMessage(NoticeboardMessage message, Boolean advancedEditor);
        void DisplayUnknownMessage();
    }
}