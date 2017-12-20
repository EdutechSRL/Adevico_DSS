using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewBasePublicCallList<T> : IViewBase
    {
        int PreloadIdCommunity { get; }
        String Portalname { get; }
        int IdCallCommunity { get; set; }
        PagerBase Pager { get; set; }

        void InitializeSkin(lm.Comol.Core.DomainModel.Helpers.ExternalPageContext skin);
        void SetContainerName(int idCommunity, String name);
        void SetActionUrl(String url);
        void LoadCalls(List<T> items);
    }
}