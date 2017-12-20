using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewNoticeboardBlock : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        PlainLayout CurrentLayout {get;set;}
        Boolean AllowPrint { get; set; }
        void DisplaySessionTimeout();
        void InitalizeControl(PlainLayout layout, Int32 idCommunity);
        void DisplayEdit(String url);
        void LoadMessage(long idMessage, Int32 idCommunity);
    }
}