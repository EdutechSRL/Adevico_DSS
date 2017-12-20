using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ModulesLoader.Presentation
{
    public interface IViewInternalModuleLoader: lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String PreLoadedPageUrl { get; }
        String PreLoadedPreviousUrl { get; }
        String PreLoadedPlainPageUrl { get; }
        int PreLoadedIdCommunity { get; }
        String PreviousUrl {set; }
        String PortalName { get; }
        void NoPermissionToAccess();
        void NavigateToUrl(String url, Boolean decodeContent);
        void NavigateToCommunityUrl(int IdPerson,int IdCommunity,String url, Boolean decodeContent);
        void ShowNoCommunityAccess(String communityName);
        String DecodeUrl(String url);
    }
}