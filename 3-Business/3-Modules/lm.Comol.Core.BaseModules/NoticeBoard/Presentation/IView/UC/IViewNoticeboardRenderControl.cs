using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public interface IViewNoticeboardRenderControl : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean DisplayHistory { get; set; }
        Boolean HasHistory { get; set; }
        Boolean IsForManagement { get; set; }
        Int32 HistoryPageSize { get; set; }
        Int32 HistoryPageIndex { get; set; }
        Int32 HistoryPageCount { get; set; }
        Int32 ContainerIdCommunity { get; set; }
        long IdCurrentMessage { get; set; }
        String GetTranslatedRemovedUser { get; }

        void InitializeControl(Int32 idCommunity, NoticeBoard.Domain.ModuleNoticeboard permissions, long idMessage=0);
        void InitializeControl(Int32 idCommunity, long idMessage=0);
        void InitalizeCommands(Boolean allowPrint, String editUrl = "");
        void DisplaySessionTimeout();
        void LoadMessage(long idMessage, Int32 idCommunity);
        void LoadHistory(List<dtoHistoryItem> items, Int32 pageIndex, Int32 pageCount);
    }
}