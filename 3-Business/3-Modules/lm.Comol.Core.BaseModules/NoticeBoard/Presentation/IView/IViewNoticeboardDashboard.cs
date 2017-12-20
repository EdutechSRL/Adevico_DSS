using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewNoticeboardDashboard : IViewNoticeboardEditing
    {
        NoticeBoard.Domain.ModuleNoticeboard CurrentPermissions { get; set; }
        String GetBackUrl();
        String GetDefaultHomePage();
        void SetEditingUrls(String urlAdvanced, String urlSimple);
        void SetNewMessageUrls(String urlAdvanced, String urlSimple, Boolean allowClean);

        void AllowVirtualDelete(Boolean allow);
        void AllowVirtualUndelete(Boolean allow);
        void AllowSetActive(Boolean allow);
        void AllowVirtualUndeleteAndSetActive(Boolean allow);
        void HideEditingCommands();
        void InitializeControl(Int32 idCommunity, NoticeBoard.Domain.ModuleNoticeboard permissions, long idMessage = 0);
     
    }
}